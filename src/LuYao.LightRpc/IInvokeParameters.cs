namespace LuYao.LightRpc;

public interface IInvokeParameters
{
    bool TryGetValue<T>(string key, out T? value);
}
