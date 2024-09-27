using Newtonsoft.Json.Linq;

namespace LuYao.LightRpc;

internal class JTokenInvokeParameters : IInvokeParameters
{
    public JObject JObject { get; }

    public JTokenInvokeParameters(JObject jObject)
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
