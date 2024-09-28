namespace LuYao.LightRpc.Demo.MinimalApi;

public class RpcHttpResult : IResult
{
    public RpcResult RpcResult { get; }

    public RpcHttpResult(RpcResult rpcResult)
    {
        RpcResult = rpcResult;
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        var response = httpContext.Response;
        response.StatusCode = (int)this.RpcResult.Code;
        if (this.RpcResult.Code == RpcResultCode.Ok)
        {
            if (this.RpcResult.Data is not null)
            {
                var server = httpContext.RequestServices.GetRequiredService<MainServer>();
                //var output = server.DataConverter.Serialize(this.RpcResult.Data);
                //await response.WriteAsync(output);
                throw new NotImplementedException();
            }
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(this.RpcResult.Message))
            {
                await response.WriteAsync(this.RpcResult.Message);
            }
        }
    }
}