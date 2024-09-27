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
}
