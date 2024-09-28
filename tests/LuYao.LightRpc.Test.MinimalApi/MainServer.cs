namespace LuYao.LightRpc.Test.MinimalApi;

public class MainServer : MainServer<String>
{
    public MainServer() : base(new NewtonsoftDataConverter())
    {

    }
}
