namespace LuYao.LightRpc;

public interface IReadOnlyDataPackage
{
    bool TryGetValue<T>(string key, out T? value);
}
