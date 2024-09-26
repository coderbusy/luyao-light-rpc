using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace LuYao.LightRpc;

public class NewtonsoftDataConverter : IDataConverter<String>
{
    public T ConvertTo<T>(object data)
    {
        if (data is T t) return t;
        if (data is JToken token) return token.Value<T>()!;
        return (T)Convert.ChangeType(data, typeof(T));
    }

    public IDictionary<string, object> Parse(string data)
    {
        var dict = new SortedDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        if (!string.IsNullOrWhiteSpace(data)) JsonConvert.PopulateObject(data, dict);
        return dict;
    }

    public TResult Deserialize<TResult>(string? data)
    {
        return JsonConvert.DeserializeObject<TResult>(data ?? string.Empty);
    }

    public string Serialize(object? data)
    {
        return JsonConvert.SerializeObject(data);
    }
}
