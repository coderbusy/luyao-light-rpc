using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.LightRpc.Models.BuildControllerDescriptor;

public class ActionModel
{
    public string Name { get; set; }
    public string Path { get; set; }
    public bool ReturnsVoid { get; set; }
    public bool IsAwaitable { get; set; }
    public bool HasReturnValue { get; set; }
    public List<ActionParameterModel> Parameters { get; } = new List<ActionParameterModel>();
}
