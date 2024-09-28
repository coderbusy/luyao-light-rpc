using System.Collections.Generic;

namespace LuYao.LightRpc;

public interface IDataConverter
{
    IDataPackage CreatePackage();
}

public interface IDataConverter<T> : IDataConverter
{
    T Serialize(IDataPackage data);
    IDataPackage? Deserialize(T? data);
}