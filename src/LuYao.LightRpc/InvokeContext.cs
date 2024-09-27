using System;

namespace LuYao.LightRpc;

public class InvokeContext
{
    public IReadOnlyDataPackage Params { get; }

    public InvokeContext(IReadOnlyDataPackage @params)
    {
        Params = @params;
    }

    public Object? Result { get; internal set; }

    public void SetResult(Object result)
    {
        this.Result = result;
    }
}