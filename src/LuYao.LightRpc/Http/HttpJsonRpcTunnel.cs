﻿using System;
using System.IO;
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

    public virtual RpcResult Send(string action, IDataPackage data)
    {
#if NET5_0_OR_GREATER
        using var http = this.CreateHttpClient();
        using HttpRequestMessage request = CreateHttpRequestMessage(action, data);
        using var response = http.Send(request); string body = string.Empty;
        using (var ms = response.Content.ReadAsStream())
        using (var sr = new StreamReader(ms))
        {
            body = sr.ReadToEnd();
        }
        return CreateResult(response, body);
#else
        return this.SendAsync(action, data).GetAwaiter().GetResult();
#endif
    }

    public virtual async Task<RpcResult> SendAsync(string action, IDataPackage data)
    {
        using var http = this.CreateHttpClient();
        using HttpRequestMessage request = CreateHttpRequestMessage(action, data);
        using var response = await http.SendAsync(request);
        string body = await response.Content.ReadAsStringAsync();
        return CreateResult(response, body);
    }

    protected virtual RpcResult CreateResult(HttpResponseMessage response, string body)
    {
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

    protected virtual HttpRequestMessage CreateHttpRequestMessage(string action, IDataPackage data)
    {
        var builder = new UriBuilder(this.MatchEndpoint(action));
        var nv = System.Web.HttpUtility.ParseQueryString(builder.Query);
        nv["cmd"] = action;
        builder.Query = nv.ToString();
        var url = builder.ToString();
        var json = this.JsonConverter.Serialize(data);
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        return request;
    }
}
