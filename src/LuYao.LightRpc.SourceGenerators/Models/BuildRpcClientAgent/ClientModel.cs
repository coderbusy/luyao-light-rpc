using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.LightRpc.Models.BuildRpcClientAgent;

public class ClientModel
{
    public string Namespace { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public List<string> Type { get; } = new List<string>();
    public List<ActionModel> Actions { get; } = new List<ActionModel>();
}
