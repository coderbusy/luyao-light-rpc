using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.LightRpc.Models.BuildControllerDescriptor;

public class ControllerModel
{
    public string Namespace { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }

    public List<ActionModel> Actions { get; } = new List<ActionModel>();

    public string GetActionPath(ActionModel action)
    {
        var parts = new List<string>();
        var n = this.Name;

        if (n.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
        {
            n = n.Substring(0, n.Length - "Controller".Length);
        }
        if (n.EndsWith("Service", StringComparison.OrdinalIgnoreCase))
        {
            n = n.Substring(0, n.Length - "Service".Length);
        }
        parts.Add(n);

        n = action.Name;
        if (n.EndsWith("Async", StringComparison.OrdinalIgnoreCase))
        {
            n = n.Substring(0, n.Length - "Async".Length);
        }
        parts.Add(n);

        return string.Join("/", parts).ToLowerInvariant();
    }
}
