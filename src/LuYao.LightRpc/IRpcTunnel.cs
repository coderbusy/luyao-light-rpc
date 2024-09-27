using System.Threading.Tasks;

namespace LuYao.LightRpc;

public interface IRpcTunnel<TData> 
{
    Task<RpcResult<TData>> InvokeAsync(string action, TData data);
}
