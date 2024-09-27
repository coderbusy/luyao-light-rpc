using System;

namespace LuYao.LightRpc.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class RemoteActionAttribute : Attribute
{
    public RemoteActionAttribute(string command)
    {
        this.Command = command;
    }
    public string Command { get; }
}