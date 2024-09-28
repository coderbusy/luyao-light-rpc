using LuYao.LightRpc.Attributes;

namespace LuYao.LightRpc.Demo.Client;

[RpcClientAgent]
public partial class RpcClient : LuYao.LightRpc.RpcClient<String>
{
    public RpcClient(string endpoint) : base(new RpcTunnel(endpoint))
    {

    }

    [RemoteAction("test/sum")]
    public partial Task<int> SumAsync(int a, int b);
}
