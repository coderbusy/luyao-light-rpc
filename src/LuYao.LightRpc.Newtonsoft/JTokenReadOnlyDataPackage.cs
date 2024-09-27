using Newtonsoft.Json.Linq;

namespace LuYao.LightRpc;

internal class JTokenReadOnlyDataPackage : IReadOnlyDataPackage
{
    public JObject JObject { get; }

    public JTokenReadOnlyDataPackage(JObject jObject)
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
