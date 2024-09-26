using System;
using System.Threading.Tasks;

namespace LuYao.LightRpc;

public class RpcClient<TData>
{
    public RpcClient(IDataConverter<TData> dataConverter, IRpcTunnel<TData> tunnel)
    {
        this.DataConverter = dataConverter ?? throw new ArgumentNullException(nameof(dataConverter));
        Tunnel = tunnel ?? throw new ArgumentNullException(nameof(tunnel));
    }
    public IDataConverter<TData> DataConverter { get; }
    public IRpcTunnel<TData> Tunnel { get; }
    public virtual async Task<TResult> InvokeAsync<TResult>(String action, Object? args = null)
    {
        var input = this.DataConverter.Serialize(args);
        var result = await this.Tunnel.InvokeAsync(action, input, this.DataConverter);
        if (result.Code != RpcResultCode.Ok) throw new RpcException((int)result.Code, result.Message!);
        var output = result.Data;
        return this.DataConverter.Deserialize<TResult>(output)!;
    }
}
