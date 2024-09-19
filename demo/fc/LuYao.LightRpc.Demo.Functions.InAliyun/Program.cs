using LuYao.LightRpc.Demo;
using LuYao.LightRpc.Demo.Functions.InAliyun;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MainServer>();

var app = builder.Build();

app.MapPost("/initialize", () =>
{
    return "initialize succ";
});

app.MapPost("/invoke", ([FromHeader(Name = "x-fc-request-id")] string requestId) =>
{
    return "get reqeuestId from header:" + requestId;
});

app.Map("/", async (
    [FromQuery(Name = "cmd")] string? action,
    [FromBody] dynamic? body,
    [FromServices] MainServer server
    ) =>
{
    if (action is null) action = string.Empty;
    string data = body is not null ? body.ToString() : string.Empty;
    var result = await server.Invoke(action, data);
    var ret = new RpcHttpResult(result);
    return ret;
});

app.Run();
