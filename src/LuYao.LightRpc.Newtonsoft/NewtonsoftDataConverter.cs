using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace LuYao.LightRpc;

public class NewtonsoftDataConverter : IDataConverter<String>
{
    public TResult Deserialize<TResult>(string? data)
    {
        return JsonConvert.DeserializeObject<TResult>(data ?? string.Empty);
    }

    public string Serialize(object? data)
    {
        return JsonConvert.SerializeObject(data);
    }

    public IInvokeParameters ReadParameters(string data)
    {
        if (string.IsNullOrWhiteSpace(data)) return EmptyInvokeParameters.Instance;
        var job = JObject.Parse(data);
        return new JTokenInvokeParameters(job);
    }

    public IDataPackage CreatePackage() => new JObjectDataPackage();

    public string Serialize(IDataPackage data) => JsonConvert.SerializeObject(data);
}
