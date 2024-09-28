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
}
