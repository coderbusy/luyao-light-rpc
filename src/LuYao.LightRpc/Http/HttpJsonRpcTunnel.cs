using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.LightRpc.Http;

public abstract class HttpJsonRpcTunnel : IRpcTunnel<string>
{
    protected HttpJsonRpcTunnel(string endpoint)
    {
        if (string.IsNullOrWhiteSpace(endpoint)) throw new ArgumentNullException(nameof(endpoint));
        this.Endpoint = endpoint;
    }
    public string Endpoint { get; }
    protected virtual string MatchEndpoint(String action) => this.Endpoint;
    protected abstract HttpClient CreateHttpClient();
    public async Task<RpcResult<string>> InvokeAsync(string action, string data)
    {
        var builder = new UriBuilder(this.MatchEndpoint(action));
        var nv = System.Web.HttpUtility.ParseQueryString(builder.Query);
        nv["cmd"] = action;
        builder.Query = nv.ToString();
        var url = builder.ToString();
        using var http = this.CreateHttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent(data, Encoding.UTF8, "application/json");
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        using var response = await http.SendAsync(request);
        string body = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return new RpcResult<string>
            {
                Code = RpcResultCode.Ok,
                Data = body
            };
        }
        else
        {
            return new RpcResult<string>
            {
                Code = (RpcResultCode)response.StatusCode,
                Message = body
            };
        }
    }

    public RpcResult<string> Invoke(string action, string data)
    {
        return InvokeAsync(action, data).GetAwaiter().GetResult();
    }
}
