using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.LightRpc.Demo;

public class MainServer : RpcServer<String>
{
    public MainServer() : base(new LuYao.LightRpc.NewtonsoftDataConverter())
    {
        this.Register<TestController>();
    }
}
