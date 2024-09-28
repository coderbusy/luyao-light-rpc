// See https://aka.ms/new-console-template for more information
using LuYao.LightRpc.Test.Client;

var endpoint = "http://localhost:5287/";
var client = new MainClient(endpoint);
var sum = await client.Sum(1, 2);
Console.WriteLine(sum);
Console.ReadLine();