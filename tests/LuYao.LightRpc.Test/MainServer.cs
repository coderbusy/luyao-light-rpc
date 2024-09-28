using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.LightRpc.Test;

public class MainServer<TData> : RpcServer<TData>
{
    public MainServer(IDataConverter<TData> dataConverter) : base(dataConverter)
    {
        this.Register<TestController>();
    }
}
