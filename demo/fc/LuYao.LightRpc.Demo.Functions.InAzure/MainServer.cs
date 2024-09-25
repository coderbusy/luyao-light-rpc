using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuYao.LightRpc.Demo.Functions.InAzure;

public class MainServer : MainServer<String>
{
    public MainServer() : base(new NewtonsoftDataConverter())
    {

    }
}
