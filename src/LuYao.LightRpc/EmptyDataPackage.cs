namespace LuYao.LightRpc;

public class EmptyDataPackage : IReadOnlyDataPackage
{
    public static EmptyDataPackage Instance { get; } = new EmptyDataPackage();
    private EmptyDataPackage() { }
    public bool TryGetValue<T>(string key, out T? value)
    {
        value = default(T);
        return false;
    }
}
