using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace LuYao.LightRpc;

internal class JObjectDataPackage : SortedDictionary<string, JToken>, IDataPackage
{
    public void Set<T>(string key, T? value)
    {
        if (value is null)
        {
            this.Remove(key);
        }
        else
        {
            this[key] = JToken.FromObject(value);
        }
    }

    public bool TryGetValue<T>(string key, out T? value)
    {
        if (this.TryGetValue(key, out var token))
        {
            value = token.ToObject<T>();
            return true;
        }
        value = default;
        return false;
    }
}
