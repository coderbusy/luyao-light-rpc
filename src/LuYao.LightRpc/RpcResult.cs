namespace LuYao.LightRpc;

public class RpcResult
{
    public RpcResultCode Code { get; set; }
    public string? Message { get; set; }
    public IDataPackage? Data { get; set; }
}