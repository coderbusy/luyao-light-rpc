using LuYao.LightRpc.Descriptors;
using System;

namespace LuYao.LightRpc;

public class RpcService
{
    public RpcService(object controller, IActionDescriptor descriptor)
    {
        Controller = controller;
        Descriptor = descriptor;
    }
    public Object Controller { get; }
    public IActionDescriptor Descriptor { get; }
}
