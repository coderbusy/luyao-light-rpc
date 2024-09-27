namespace LuYao.LightRpc;

public class EmptyInvokeParameters : IInvokeParameters
{
    public static EmptyInvokeParameters Instance { get; } = new EmptyInvokeParameters();
    private EmptyInvokeParameters() { }
    public bool TryGetValue<T>(string key, out T? value)
    {
        value = default(T);
        return false;
    }
}
