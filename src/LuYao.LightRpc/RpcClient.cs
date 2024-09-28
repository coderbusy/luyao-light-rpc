using System;
using System.Threading.Tasks;

namespace LuYao.LightRpc;

public abstract class RpcClient
{
    public abstract IDataPackage CreateDataPackage();
    public abstract Task<TResult> InvokeAsync<TResult>(String action, IDataPackage package);
    public abstract Task InvokeAsync(String action, IDataPackage package);
    public abstract TResult Invoke<TResult>(String action, IDataPackage package);
    public abstract void Invoke(String action, IDataPackage package);
}

public class RpcClient<TData> : RpcClient
{
    public RpcClient(IRpcTunnel tunnel)
    {
        Tunnel = tunnel ?? throw new ArgumentNullException(nameof(tunnel));
    }

    public IRpcTunnel Tunnel { get; }

    public override IDataPackage CreateDataPackage() => this.Tunnel.DataConverter.CreatePackage();

    public override TResult Invoke<TResult>(string action, IDataPackage package)
    {
        var result = this.Tunnel.Send(action, package);
        if (result.Code != RpcResultCode.Ok) throw new RpcException(result.Code, result.Message!);
        var output = result.Data ?? EmptyDataPackage.Instance;
        TResult data = default!;
        if (output.TryGetValue<TResult>("data", out var v_ret)) data = v_ret!;
        return data;
    }

    public override void Invoke(string action, IDataPackage package)
    {
        var result = this.Tunnel.Send(action, package);
        if (result.Code != RpcResultCode.Ok) throw new RpcException(result.Code, result.Message!);
    }

    public override async Task InvokeAsync(String action, IDataPackage package)
    {
        var result = await this.Tunnel.SendAsync(action, package);
        if (result.Code != RpcResultCode.Ok) throw new RpcException(result.Code, result.Message!);
    }

    public override async Task<TResult> InvokeAsync<TResult>(string action, IDataPackage package)
    {
        var result = await this.Tunnel.SendAsync(action, package);
        if (result.Code != RpcResultCode.Ok) throw new RpcException(result.Code, result.Message!);
        var output = result.Data ?? EmptyDataPackage.Instance;
        TResult data = default!;
        if (output.TryGetValue<TResult>("data", out var v_ret)) data = v_ret!;
        return data;
    }
}