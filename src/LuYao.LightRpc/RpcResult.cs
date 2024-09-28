namespace LuYao.LightRpc;

public class RpcResult
{
    public RpcResultCode Code { get; internal set; }
    public string? Message { get; internal set; }
    public IDataPackage? Data { get; internal set; }
}