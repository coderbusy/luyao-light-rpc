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
    public RpcClient(IDataConverter<TData> dataConverter, IRpcTunnel tunnel)
    {
        this.DataConverter = dataConverter ?? throw new ArgumentNullException(nameof(dataConverter));
        Tunnel = tunnel ?? throw new ArgumentNullException(nameof(tunnel));
    }
    public IDataConverter<TData> DataConverter { get; }
    public IRpcTunnel Tunnel { get; }

    public override IDataPackage CreateDataPackage() => this.DataConverter.CreatePackage();

    public override TResult Invoke<TResult>(string action, IDataPackage package)
    {
        var result = this.Tunnel.Send(action, package);
        if (result.Code != RpcResultCode.Ok) throw new RpcException(result.Code, result.Message!);
        var output = result.Data ?? EmptyDataPackage.Instance;
        TResult? ret = default;
        //return this.DataConverter.Deserialize<TResult>(output)!;
        if (output.TryGetValue<TResult>("", out var v_ret)) ret = v_ret!;
        return ret;
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
        TResult? ret = default(TResult);
        if (output.TryGetValue<TResult>("", out var v_ret)) ret = v_ret!;
        return ret;
    }
}