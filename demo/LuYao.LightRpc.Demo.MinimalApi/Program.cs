using LuYao.LightRpc.Demo;
using LuYao.LightRpc.Demo.MinimalApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);

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
    string data = string.Empty;
    var request = context.Request;
    using (var reader = new StreamReader(request.Body))
    {
        data = await reader.ReadToEndAsync();
    }
    var result = await server.InvokeAsync(action, data);
    var ret = new RpcHttpResult(result);
    return ret;
});

app.Run();
