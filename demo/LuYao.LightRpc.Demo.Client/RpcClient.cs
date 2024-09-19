namespace LuYao.LightRpc.Demo.Client;

public class RpcClient : LuYao.LightRpc.RpcClient<String>
{
    public RpcClient(string endpoint) : base(new LuYao.LightRpc.NewtonsoftDataConverter(), new RpcTunnel(endpoint))
    {

    }
}
