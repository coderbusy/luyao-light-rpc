// See https://aka.ms/new-console-template for more information

using LuYao.LightRpc.Demo.Client;
using System.Diagnostics;

//var endpoint = "http://localhost:5297/";// in aliyun
//var endpoint = "http://localhost:5094/";//minimal api
var endpoint = "http://localhost:5000/";//release
var client = new RpcClient(endpoint);
var sum = await client.InvokeAsync<int>("test/sum", new { a = 1, b = 2 });
Console.WriteLine($"sum is : {sum}");
var st = new Stopwatch();
st.Start();
for (int i = 0; i < 1_0000; i++)
{
    await client.InvokeAsync<int>("test/sum", new { a = 1, b = 2 });
}
st.Stop();
Console.WriteLine($"10k times sum cost {st.ElapsedMilliseconds} ms");
Console.ReadLine();