using System;
using System.Threading.Tasks;

namespace LuYao.LightRpc.Descriptors;

public abstract class ActionDescriptor : IActionDescriptor
{
    protected ActionDescriptor(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new System.ArgumentNullException(nameof(path));
        Path = path;
    }

    public string Path { get; }
    public abstract Task Invoke(object controller, InvokeContext context);
}

public class ActionDescriptor<T> : ActionDescriptor
{
    private readonly Func<T, InvokeContext, Task> _invoke;

    public ActionDescriptor(string path, Func<T, InvokeContext, Task> invoke) : base(path)
    {
        _invoke = invoke ?? throw new ArgumentNullException(nameof(invoke));
    }

    public override Task Invoke(object controller, InvokeContext context)
    {
        return _invoke((T)controller, context);
    }
}