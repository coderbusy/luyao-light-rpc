namespace LuYao.LightRpc;

public interface IDataPackage
{
    bool TryGetValue<T>(string key, out T? value);
}
