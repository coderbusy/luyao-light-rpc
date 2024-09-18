using LuYao.LightRpc.Descriptors;
using System;
using System.Collections.Generic;

namespace LuYao.LightRpc.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public abstract class ControllerDescriptorAttribute : Attribute, IControllerDescriptor
{
    public abstract IEnumerable<IActionDescriptor> GetActionDescriptors();
}
