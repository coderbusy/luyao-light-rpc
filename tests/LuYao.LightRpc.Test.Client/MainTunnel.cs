using LuYao.LightRpc.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.LightRpc.Test.Client;

public class MainTunnel : HttpJsonRpcTunnel
{
    private static readonly SocketsHttpHandler _handler = new SocketsHttpHandler();
    
    public MainTunnel(string endpoint) : base(endpoint, new NewtonsoftDataConverter())
    {

    }

    protected override HttpClient CreateHttpClient()
    {
        return new HttpClient(_handler, false);
    }
}
