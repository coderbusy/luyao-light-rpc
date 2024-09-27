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
    public RpcClient(IDataConverter<TData> dataConverter, IRpcTunnel<TData> tunnel)
    {
        this.DataConverter = dataConverter ?? throw new ArgumentNullException(nameof(dataConverter));
        Tunnel = tunnel ?? throw new ArgumentNullException(nameof(tunnel));
    }
    public IDataConverter<TData> DataConverter { get; }
    public IRpcTunnel<TData> Tunnel { get; }

    public override IDataPackage CreateDataPackage() => this.DataConverter.CreatePackage();

    public override TResult Invoke<TResult>(string action, IDataPackage package)
    {
        var input = this.DataConverter.Serialize(package);
        var result = this.Tunnel.Invoke(action, input);
        if (result.Code != RpcResultCode.Ok) throw new RpcException(result.Code, result.Message!);
        var output = result.Data;
        return this.DataConverter.Deserialize<TResult>(output)!;
    }

    public override void Invoke(string action, IDataPackage package)
    {
        var input = this.DataConverter.Serialize(package);
        var result = this.Tunnel.Invoke(action, input);
        if (result.Code != RpcResultCode.Ok) throw new RpcException(result.Code, result.Message!);
    }

    public override async Task InvokeAsync(String action, IDataPackage package)
    {
        var input = this.DataConverter.Serialize(package);
        var result = await this.Tunnel.InvokeAsync(action, input);
        if (result.Code != RpcResultCode.Ok) throw new RpcException(result.Code, result.Message!);
    }

    public override async Task<TResult> InvokeAsync<TResult>(string action, IDataPackage package)
    {
        var input = this.DataConverter.Serialize(package);
        var result = await this.Tunnel.InvokeAsync(action, input);
        if (result.Code != RpcResultCode.Ok) throw new RpcException(result.Code, result.Message!);
        var output = result.Data;
        return this.DataConverter.Deserialize<TResult>(output)!;
    }
}