using System;

namespace LuYao.LightRpc;

public class InvokeContext
{
    public IDataPackage Params { get; }
    public IDataPackage Response { get; }

    public InvokeContext(IDataPackage @params, IDataPackage response)
    {
        this.Params = @params;
        this.Response = response;
    }


    public void SetResult<T>(T result)
    {
        this.Response.Set("data", result);
    }
}