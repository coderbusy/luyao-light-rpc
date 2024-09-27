using System;

namespace LuYao.LightRpc;

public class InvokeContext
{
    public IInvokeParameters Params { get; }

    public InvokeContext(IInvokeParameters @params)
    {
        Params = @params;
    }

    public Object? Result { get; internal set; }

    public void SetResult(Object result)
    {
        this.Result = result;
    }
}