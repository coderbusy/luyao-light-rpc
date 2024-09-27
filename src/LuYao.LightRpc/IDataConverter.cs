using System.Collections.Generic;

namespace LuYao.LightRpc;


public interface IDataConverter<T>
{
    TResult? Deserialize<TResult>(T? data);
    IInvokeParameters ReadParameters(T data);
    IDataPackage CreatePackage();
    T Serialize(IDataPackage data);
}