using LuYao.LightRpc.Models.BuildControllerDescriptor;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LuYao.LightRpc.Renders.BuildControllerDescriptor;

public class ControllerDescriptorRender
{
    public ControllerModel Controller { get; }

    public ControllerDescriptorRender(ControllerModel controller)
    {
        Controller = controller;
    }
    public string Render()
    {
        var sb = new CSharpStringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.Threading.Tasks;");
        sb.AppendLine("using LuYao.LightRpc.Attributes;");
        sb.AppendLine("using LuYao.LightRpc.Descriptors;");
        sb.AppendLine();
        sb.AppendLine($"namespace {this.Controller.Namespace}");
        sb.AppendLine("{");
        using (sb.Tab()) BuildClass(sb);
        sb.AppendLine("}");
        return sb.ToString();
    }

    private void BuildClass(CSharpStringBuilder sb)
    {
        sb.AppendLine($"[{this.Controller.Name}.{this.Controller.Name}DescriptorAttribute]");
        sb.AppendLine($"partial class {this.Controller.Name}");
        sb.AppendLine("{");
        using (sb.Tab())
        {
            this.WriteDescriptorAttribute(sb);
            //WriteOutput
            this.WriteDebugOutput(sb);
        }
        sb.AppendLine("}");
    }

    private void WriteDescriptorAttribute(CSharpStringBuilder sb)
    {
        sb.AppendLine($"internal class {this.Controller.Name}DescriptorAttribute : ControllerDescriptorAttribute");
        sb.AppendLine("{");
        using (sb.Tab())
        {
            sb.AppendLine($"public override IEnumerable<IActionDescriptor> GetActionDescriptors()");
            sb.AppendLine("{");
            using (sb.Tab())
            {
                if (this.Controller.Actions.Count > 0)
                {
                    foreach (var action in this.Controller.Actions)
                    {
                        sb.AppendLine($"yield return new ActionDescriptor<{this.Controller.Name}>(\"{this.Controller.GetActionPath(action)}\",{action.Name});");
                    }
                }
                else
                {
                    sb.AppendLine("yield break;");
                }
            }
            sb.AppendLine("}");

            if (this.Controller.Actions.Count > 0)
            {
                foreach (var action in this.Controller.Actions)
                {
                    sb.AppendLine($"private static {(action.IsAwaitable ? "async " : "")}Task {action.Name}({this.Controller.Name} controller, InvokeContext context)");
                    sb.AppendLine("{");
                    using (sb.Tab())
                    {
                        if (action.Parameters.Count > 0)
                        {
                            //拷贝变量
                            sb.AppendLine($"var converter = context.DataConverter;");
                            sb.AppendLine($"var dict = context.Params;");
                            //解析参数
                            foreach (var parameter in action.Parameters)
                            {
                                sb.AppendLine($"{parameter.Type} _{parameter.Name} = default;");
                                sb.AppendLine($"if (dict.TryGetValue(\"{parameter.Name}\", out var obj_{parameter.Name}))");
                                sb.AppendLine("{");
                                using (sb.Tab()) sb.AppendLine($"_{parameter.Name} = converter.ConvertTo<{parameter.Type}>(obj_{parameter.Name});");
                                sb.AppendLine("}");
                            }
                        }
                        var args = string.Join(", ", action.Parameters.Select(p => $"_{p.Name}"));

                        //函数调用
                        if (action.IsAwaitable)
                        {
                            if (action.HasReturnValue)
                            {
                                sb.AppendLine($"var result = await controller.{action.Name}({args});");
                                sb.AppendLine($"context.SetResult(result);");
                            }
                            else
                            {
                                sb.AppendLine($"await controller.{action.Name}({args});");
                            }
                        }
                        else
                        {
                            if (action.HasReturnValue)
                            {
                                sb.AppendLine($"var result = controller.{action.Name}({args});");
                                sb.AppendLine($"context.SetResult(result);");
                            }
                            else
                            {
                                sb.AppendLine($"controller.{action.Name}({args});");
                            }
                        }
                        if (!action.IsAwaitable)
                        {
                            sb.AppendLine($"return Task.CompletedTask;");
                        }
                    }
                    sb.AppendLine("}");
                }
            }
        }
        sb.AppendLine("}");
    }

    private void WriteDebugOutput(CSharpStringBuilder sb)
    {
#if DEBUG
        var xmlSerializer = new XmlSerializer(typeof(ControllerModel));
        string output = string.Empty;
        using (var stringWriter = new StringWriter())
        {
            xmlSerializer.Serialize(stringWriter, this.Controller);
            string xmlString = stringWriter.ToString();
            var bytes = Encoding.UTF8.GetBytes(xmlString);
            output = Convert.ToBase64String(bytes);
        }
        sb.AppendLine("public static String Output()");
        sb.AppendLine("{");
        using (sb.Tab()) sb.AppendLine($"return \"{output}\";");
        sb.AppendLine("}");
#endif
    }
}
