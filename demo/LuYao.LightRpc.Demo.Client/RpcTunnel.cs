using LuYao.LightRpc.Http;

namespace LuYao.LightRpc.Demo.Client;

public class RpcTunnel : HttpJsonRpcTunnel
{
    private static readonly SocketsHttpHandler _handler = new SocketsHttpHandler();
    public RpcTunnel(string endpoint) : base(endpoint, new NewtonsoftDataConverter())
    {
    }

    protected override HttpClient CreateHttpClient()
    {
        return new HttpClient(_handler, false);
    }
}
