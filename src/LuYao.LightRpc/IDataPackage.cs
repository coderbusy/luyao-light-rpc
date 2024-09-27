using System;

namespace LuYao.LightRpc;

public interface IDataPackage : IReadOnlyDataPackage
{
    void Set<T>(String key, T? value);
}
