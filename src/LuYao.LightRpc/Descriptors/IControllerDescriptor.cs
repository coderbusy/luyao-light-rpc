using System.Collections.Generic;

namespace LuYao.LightRpc.Descriptors;

public interface IControllerDescriptor
{
    IEnumerable<IActionDescriptor> GetActionDescriptors();
}
