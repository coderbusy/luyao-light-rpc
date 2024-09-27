using LuYao.LightRpc.Models.BuildControllerDescriptor;
using LuYao.LightRpc.Renders.BuildControllerDescriptor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace LuYao.LightRpc;

[Generator]
public class ControllerDescriptorGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider.ForAttributeWithMetadataName(
            "LuYao.LightRpc.Attributes.ControllerAttribute",
            predicate: static (node, cancel) => node is ClassDeclarationSyntax,
            transform: static (ctx, cancel) => GetSemanticTargetForGeneration(ctx, cancel)
        ).Where(m => m.Item1 is not null && m.Item2 is not null);


        var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());
        context.RegisterSourceOutput(compilationAndClasses, Execute);
    }

    private static (ClassDeclarationSyntax, AttributeData) GetSemanticTargetForGeneration(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        if (context.TargetNode is not ClassDeclarationSyntax classDeclaration)
        {
            return (null, null);
        }
        AttributeData attribute = context.Attributes.FirstOrDefault(a => a.AttributeClass?.Name == "ControllerAttribute");

        return (classDeclaration, attribute);
    }

    private void Execute(SourceProductionContext context, (Compilation Left, ImmutableArray<(ClassDeclarationSyntax, AttributeData)> Right) args)
    {
        if (!args.Right.Any()) return;
        var compilation = args.Left;
        foreach ((ClassDeclarationSyntax Syntax, AttributeData Attr) item in args.Right)
        {
            if (item.Syntax is null || item.Attr is null) continue;


            string className = item.Syntax.Identifier.ValueText;
            string ns = Helpers.GetNamespace(item.Syntax);

            var semanticModel = compilation.GetSemanticModel(item.Syntax.SyntaxTree);
            var model = new ControllerModel
            {
                Name = className,
                FullName = $"{ns}.{className}",
                Namespace = ns,
            };

            BuildModel(context, item, semanticModel, model);

            var render = new ControllerDescriptorRender(model);
            var code = render.Render();

            var sourceText = SourceText.From(code, Encoding.UTF8);
            context.AddSource($"{ns}.{className}.g.cs", sourceText);
        }
    }

    private static void BuildModel(SourceProductionContext context, (ClassDeclarationSyntax Syntax, AttributeData Attr) item, SemanticModel semanticModel, ControllerModel model)
    {
        foreach (var n in item.Attr.NamedArguments)
        {
            //switch (n.Key)
            //{
            //    case "Controller":
            //        model.Controller = Convert.ToString(n.Value.Value);
            //        break;
            //    case "Area":
            //        model.Area = Convert.ToString(n.Value.Value);
            //        break;
            //}
        }

        var methods = item.Syntax.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
        if (methods.Count > 0)
        {
            foreach (var m in methods)
            {
                var methodSymbol = semanticModel.GetDeclaredSymbol(m) as IMethodSymbol;
                if (IsIgnore(methodSymbol)) continue;
                var action = new ActionModel
                {
                    Name = methodSymbol.Name,
                    ReturnsVoid = methodSymbol.ReturnsVoid,
                    HasReturnValue = !methodSymbol.ReturnsVoid
                };
                model.Actions.Add(action);
                // 获取方法的返回类型
                var returnType = methodSymbol.ReturnType;
                // 判断返回类型是否是 Task 或 Task<>
                bool returnsTask = returnType.Name == "Task" && (returnType.ContainingNamespace.ToString() == "System.Threading.Tasks");

                // 这个方法是否可以被 await
                action.IsAwaitable = returnsTask;

                if (action.IsAwaitable)
                {
                    // 检查返回类型是否是 Task
                    bool returnsTaskWithoutResult = returnType.Name == "Task" &&
                                                    returnType.ContainingNamespace.ToString() == "System.Threading.Tasks";

                    // 检查返回类型是否是 Task<T>
                    bool returnsTaskWithResult = returnType is INamedTypeSymbol namedTypeSymbol &&
                                                 namedTypeSymbol.Name == "Task" &&
                                                 namedTypeSymbol.TypeArguments.Length == 1 &&
                                                 returnType.ContainingNamespace.ToString() == "System.Threading.Tasks";

                    // 检查自定义类型是否有 GetAwaiter 并且 GetAwaiter().GetResult() 有返回值
                    bool customAwaitableReturnsValue = false;
                    var getAwaiterMethod = returnType.GetMembers("GetAwaiter").OfType<IMethodSymbol>().FirstOrDefault();

                    if (getAwaiterMethod != null && getAwaiterMethod.Parameters.Length == 0)
                    {
                        var awaiterReturnType = getAwaiterMethod.ReturnType;
                        var getResultMethod = awaiterReturnType.GetMembers("GetResult").OfType<IMethodSymbol>().FirstOrDefault();

                        if (getResultMethod != null)
                        {
                            // 检查 GetResult() 是否有返回值
                            customAwaitableReturnsValue = !getResultMethod.ReturnsVoid;
                        }
                    }

                    // 判断 await 后是否有返回值
                    action.HasReturnValue = returnsTaskWithResult || customAwaitableReturnsValue;
                }

                var paras = m.ParameterList.Parameters;
                foreach (var p in paras)
                {
                    if (p.Type is null) continue;
                    var typeInfo = semanticModel.GetTypeInfo(p.Type);
                    var ap = new ActionParameterModel
                    {
                        Name = p.Identifier.ValueText,
                        Type = typeInfo.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                    };
                    action.Parameters.Add(ap);
                }
            }
        }
    }

    private static bool IsIgnore(IMethodSymbol methodSymbol)
    {
        if (methodSymbol is null) return true;
        var accessibility = methodSymbol.DeclaredAccessibility;
        if (accessibility != Accessibility.Public) return true;
        if (methodSymbol.IsStatic) return true;
        if (methodSymbol.IsOverride) return true;
        foreach (var attr in methodSymbol.GetAttributes())
        {
            if (attr.AttributeClass.Name == "IgnoreAttribute")
            {
                return true;
            }
        }
        return false;
    }
}
