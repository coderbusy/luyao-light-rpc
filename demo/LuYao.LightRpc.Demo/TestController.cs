using LuYao.LightRpc.Attributes;
using System;

namespace LuYao.LightRpc.Demo;

[Controller]
public partial class TestController
{
    public int Sum(int a, int b)
    {
        return a + b;
    }
}
