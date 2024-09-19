using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace LuYao.LightRpc.Demo.Functions.InAzure;

public class Handler
{
    private readonly ILogger<Handler> _logger;
    private readonly MainServer _mainServer;

    public Handler(ILogger<Handler> logger, MainServer mainServer)
    {
        _logger = logger;
        _mainServer = mainServer;
    }

    [Function("Handler")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        string action = req.Query["cmd"]?.ToString() ?? String.Empty;
        string data = await req.ReadAsStringAsync() ?? string.Empty;
        var response = req.CreateResponse();
        response.Headers.Add("Content-Type", "application/json");
        var result = await _mainServer.Invoke(action, data);
        response.StatusCode = System.Net.HttpStatusCode.OK;
        if (result != null)
        {
            response.StatusCode = (System.Net.HttpStatusCode)result.Code;
            if (result.Code == RpcResultCode.Ok)
            {
                if (result.Data != null)
                {
                    var output = _mainServer.DataConverter.Serialize(result.Data);
                    await response.WriteStringAsync(output);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(result.Message))
                {
                    await response.WriteStringAsync(result.Message);
                }
            }
        }

        return response;
    }
}
