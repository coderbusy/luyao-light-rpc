namespace LuYao.LightRpc;

public class RpcResultBase
{
    public RpcResultCode Code { get; set; }
    public string? Message { get; set; }
}
public class RpcResult : RpcResultBase
{
    public object? Data { get; set; }
}

public class RpcResult<TData> : RpcResultBase
{
    public TData? Data { get; set; }
}