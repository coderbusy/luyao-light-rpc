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

    public IReadOnlyDataPackage CreatePackage(string data)
    {
        if (string.IsNullOrWhiteSpace(data)) return EmptyDataPackage.Instance;
        var job = JObject.Parse(data);
        return new JTokenReadOnlyDataPackage(job);
    }
}
