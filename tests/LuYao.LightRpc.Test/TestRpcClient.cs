using LuYao.LightRpc.Attributes;

namespace LuYao.LightRpc.Test;

[RpcClientAgent]
public partial class TestRpcClient<T> : RpcClient<T>
{
    public TestRpcClient(IDataConverter<T> dataConverter, IRpcTunnel tunnel) : base(dataConverter, tunnel)
    {
    }

    [RemoteAction("Test/Sum")]
    public partial Task<int> Sum(int a, int b);

    [RemoteAction("Test/Sum")]
    public partial int Test1(int a, int b);
}
