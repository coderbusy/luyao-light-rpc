using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.LightRpc.Http;

public abstract class HttpJsonRpcTunnel : IRpcTunnel
{
    protected HttpJsonRpcTunnel(string endpoint, IDataConverter<string> jsonConverter)
    {
        if (string.IsNullOrWhiteSpace(endpoint)) throw new ArgumentNullException(nameof(endpoint));
        if (jsonConverter == null) throw new ArgumentNullException(nameof(jsonConverter));
        this.Endpoint = endpoint;
        JsonConverter = jsonConverter;
    }
    public string Endpoint { get; }
    public IDataConverter<String> JsonConverter { get; }

    public IDataConverter DataConverter => this.JsonConverter;
    protected virtual string MatchEndpoint(String action) => this.Endpoint;
    protected abstract HttpClient CreateHttpClient();

    public RpcResult Send(string action, IDataPackage data)
    {
        throw new NotImplementedException();
    }

    public async Task<RpcResult> SendAsync(string action, IDataPackage data)
    {
        var builder = new UriBuilder(this.MatchEndpoint(action));
        var nv = System.Web.HttpUtility.ParseQueryString(builder.Query);
        nv["cmd"] = action;
        builder.Query = nv.ToString();
        var url = builder.ToString();
        var json = this.JsonConverter.Serialize(data);
        using var http = this.CreateHttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        using var response = await http.SendAsync(request);
        string body = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return new RpcResult
            {
                Code = RpcResultCode.Ok,
                Data = this.JsonConverter.Deserialize(body)
            };
        }
        else
        {
            return new RpcResult
            {
                Code = (RpcResultCode)response.StatusCode,
                Message = body
            };
        }
    }
}
