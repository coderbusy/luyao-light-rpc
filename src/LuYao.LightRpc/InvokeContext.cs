using System;
using System.Collections.Generic;

namespace LuYao.LightRpc;

public class InvokeContext
{
    public IDataConverter DataConverter { get; }
    public IDictionary<string, object> Params { get; }

    public InvokeContext(IDataConverter dataConverter, IDictionary<string, object> @params)
    {
        Params = @params;
        DataConverter = dataConverter;
    }

    public Object? Result { get; internal set; }

    public void SetResult(Object result)
    {
        this.Result = result;
    }
}