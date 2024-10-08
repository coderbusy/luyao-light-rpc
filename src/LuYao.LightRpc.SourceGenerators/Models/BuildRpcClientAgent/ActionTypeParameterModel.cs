using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.LightRpc.Models.BuildRpcClientAgent;

public class ActionTypeParameterModel : ActionParameterModel
{
    public List<string> ConstraintTypes { get; } = new List<string>();
    public bool HasReferenceTypeConstraint { get; set; }
    public bool HasValueTypeConstraint { get; set; }
    public bool HasConstructorConstraint { get; set; }
    public bool HasUnmanagedTypeConstraint { get; set; }
    public bool HasConstraint => ConstraintTypes.Count > 0 || HasReferenceTypeConstraint || HasValueTypeConstraint || HasConstructorConstraint || HasUnmanagedTypeConstraint;
}
