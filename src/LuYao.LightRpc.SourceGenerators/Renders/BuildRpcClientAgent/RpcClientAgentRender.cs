using LuYao.LightRpc.Models.BuildControllerDescriptor;
using LuYao.LightRpc.Models.BuildRpcClientAgent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LuYao.LightRpc.Renders.BuildRpcClientAgent;

public class RpcClientAgentRender
{
    public ClientModel Client { get; }

    public RpcClientAgentRender(ClientModel client)
    {
        Client = client;
    }
    public string Render()
    {
        var sb = new CSharpStringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.Threading.Tasks;");
        sb.AppendLine("using LuYao.LightRpc;");
        sb.AppendLine("using LuYao.LightRpc.Attributes;");
        sb.AppendLine("using LuYao.LightRpc.Descriptors;");
        sb.AppendLine();
        sb.AppendLine($"namespace {this.Client.Namespace}");
        sb.AppendLine("{");
        using (sb.Tab()) BuildClass(sb);
        sb.AppendLine("}");
        return sb.ToString();
    }

    private void BuildClass(CSharpStringBuilder sb)
    {
        if (this.Client.Type.Count == 0)
        {
            sb.AppendLine($"partial class {this.Client.Name}");
        }
        else
        {
            sb.AppendLine($"partial class {this.Client.Name}<{string.Join(",", this.Client.Type)}>");
        }
        sb.AppendLine("{");
        using (sb.Tab())
        {
            this.WriteAgent(sb);
            this.WriteDebugOutput(sb);
        }
        sb.AppendLine("}");
    }

    private void WriteAgent(CSharpStringBuilder sb)
    {
        foreach (var action in this.Client.Actions)
        {
            sb.AppendLine($"{action.Accessibility} partial {(action.IsAwaitable ? "async " : "")}{action.ReturnType} {action.Name}({BuildArgs(action)})");
            sb.AppendLine("{");
            using (sb.Tab())
            {
                sb.AppendLine("var _pkg_ = this.CreateDataPackage();");
                foreach (var arg in action.Parameters)
                {
                    sb.AppendLine($"_pkg_.Set(\"{arg.Name}\", {arg.Name});");
                }
                if (action.HasReturnValue)
                {
                    if (action.IsAwaitable)
                    {
                        sb.AppendLine($"var result = await this.InvokeAsync<{action.ReturnDataType}>(\"{action.Command}\", _pkg_);");
                        sb.AppendLine("return result;");
                    }
                    else
                    {
                        sb.AppendLine($"var result = this.Invoke<{action.ReturnDataType}>(\"{action.Command}\", _pkg_);");
                        sb.AppendLine("return result;");
                    }
                }
                else
                {
                    if (action.IsAwaitable)
                    {
                        sb.AppendLine($"await this.InvokeAsync(\"{action.Command}\", _pkg_);");
                    }
                    else
                    {
                        sb.AppendLine($"this.Invoke(\"{action.Command}\", _pkg_);");
                    }
                }
            }
            sb.AppendLine("}");
        }
    }

    private object BuildArgs(Models.BuildRpcClientAgent.ActionModel action)
    {
        if (action.Parameters.Count == 0) return string.Empty;
        return string.Join(", ", action.Parameters.Select(p => $"{p.Type} {p.Name}"));
    }

    private void WriteDebugOutput(CSharpStringBuilder sb)
    {
#if DEBUG
        var xmlSerializer = new XmlSerializer(this.Client.GetType());
        string output = string.Empty;
        using (var stringWriter = new StringWriter())
        {
            xmlSerializer.Serialize(stringWriter, this.Client);
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
