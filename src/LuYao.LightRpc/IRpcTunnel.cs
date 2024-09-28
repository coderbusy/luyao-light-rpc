using System.Threading.Tasks;

namespace LuYao.LightRpc;

public interface IRpcTunnel
{
    IDataConverter DataConverter { get; }
    Task<RpcResult> SendAsync(string action, IDataPackage data);
    RpcResult Send(string action, IDataPackage data);
}