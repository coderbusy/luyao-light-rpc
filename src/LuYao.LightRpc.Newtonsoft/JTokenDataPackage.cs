using Newtonsoft.Json.Linq;

namespace LuYao.LightRpc;

internal class JTokenDataPackage : IDataPackage
{
    public JObject JObject { get; }

    public JTokenDataPackage(JObject jObject)
    {
        JObject = jObject;
    }

    public bool TryGetValue<T>(string key, out T? value)
    {
        if (this.JObject.TryGetValue(key, out var token))
        {
            value = token.ToObject<T>();
            return true;
        }
        value = default;
        return false;
    }
}
