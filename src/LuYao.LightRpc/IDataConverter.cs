using System.Collections.Generic;

namespace LuYao.LightRpc;


public interface IDataConverter<T>
{
    IDataPackage? Deserialize(T? data);
    IDataPackage CreatePackage();
    T Serialize(IDataPackage data);
}