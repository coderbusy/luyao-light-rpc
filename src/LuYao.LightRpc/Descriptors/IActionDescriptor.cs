using System.Threading.Tasks;

namespace LuYao.LightRpc.Descriptors;

public interface IActionDescriptor
{
    string Path { get; }
    Task Invoke(object controller, InvokeContext context);
}
