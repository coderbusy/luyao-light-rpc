using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.LightRpc.Demo;

public class MainServer<T> : RpcServer<T>
{
    public MainServer(IDataConverter<T> dataConverter) : base(dataConverter)
    {
        this.Register<TestController>();
    }
}
