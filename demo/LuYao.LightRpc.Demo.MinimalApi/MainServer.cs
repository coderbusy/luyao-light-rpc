namespace LuYao.LightRpc.Demo.MinimalApi;

public class MainServer : MainServer<String>
{
    public MainServer() : base(new NewtonsoftDataConverter())
    {

    }
}
