using LuYao.LightRpc.Attributes;

namespace LuYao.LightRpc.Test;

[Controller]
public partial class TestController
{
    public int Sum(int a, int b)
    {
        return a + b;
    }
}
