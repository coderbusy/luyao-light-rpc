namespace LuYao.LightRpc.Demo.Functions.InAliyun;

public class MainServer : MainServer<String>
{
    public MainServer() : base(new NewtonsoftDataConverter())
    {

    }
}
