using System.Threading.Tasks;

namespace LuYao.LightRpc;

public interface IRpcTunnel<TData>
{
    Task<RpcResult<TData>> SendAsync(string action, TData data);
    RpcResult<TData> Send(string action, TData data);
}