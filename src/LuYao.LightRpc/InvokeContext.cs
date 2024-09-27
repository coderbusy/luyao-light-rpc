using System;

namespace LuYao.LightRpc;

public class InvokeContext
{
    public IDataPackage Params { get; }

    public InvokeContext(IDataPackage @params)
    {
        Params = @params;
    }

    public Object? Result { get; internal set; }

    public void SetResult(Object result)
    {
        this.Result = result;
    }
}