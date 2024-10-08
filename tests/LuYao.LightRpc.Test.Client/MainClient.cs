using LuYao.LightRpc.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.LightRpc.Test.Client;

[RpcClientAgent]
public partial class MainClient : RpcClient<string>
{
    public MainClient(string endpoint) : base(new MainTunnel(endpoint))
    {

    }
    [RemoteAction("Test/Sum")]
    public partial Task<int> Sum(int a, int b);

    [RemoteAction("Test/Sum")]
    public partial Task<T> Sum1<T, T1, T2, T3>(T1 a, T2 b, T3 c)
        where T : struct
        where T1 : class
        where T2 : unmanaged
        where T3 : BaseClass, new()
        ;
}
