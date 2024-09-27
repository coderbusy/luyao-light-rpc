using System;

namespace LuYao.LightRpc;

public interface IDataPackage
{
    void Set<T>(String key, T? value);
}
