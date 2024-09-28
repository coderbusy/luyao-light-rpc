using LuYao.LightRpc;
using LuYao.LightRpc.Test.MinimalApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddSingleton<MainServer>();

var app = builder.Build();

app.Map("/", async (
    HttpContext context,
    [FromQuery(Name = "cmd")] string? action,
    [FromServices] MainServer server
    ) =>
{
    if (action is null) action = string.Empty;
    string json = string.Empty;
    var request = context.Request;
    using (var reader = new StreamReader(request.Body))
    {
        json = await reader.ReadToEndAsync();
    }
    var input = server.DataConverter.Deserialize(json) ?? EmptyDataPackage.Instance;
    var result = await server.InvokeAsync(action, input);
    var ret = new RpcHttpResult(result);
    return ret;
});

app.Run();
