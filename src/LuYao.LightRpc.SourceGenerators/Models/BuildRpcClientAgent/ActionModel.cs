using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.LightRpc.Models.BuildRpcClientAgent;

public class ActionModel
{
    public string Name { get; set; }
    public string Command { get; set; }
    public bool ReturnsVoid { get; set; }
    public bool IsAwaitable { get; set; }
    public bool HasReturnValue { get; set; }
    public string ReturnType { get; set; }
    public string ReturnDataType { get; set; }
    public List<ActionParameterModel> Parameters { get; } = new List<ActionParameterModel>();
    public string Accessibility { get; set; }
}
