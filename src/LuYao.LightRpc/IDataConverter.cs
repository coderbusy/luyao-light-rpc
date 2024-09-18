using System.Collections.Generic;

namespace LuYao.LightRpc;

public interface IDataConverter
{
    T ConvertTo<T>(object data);
}

public interface IDataConverter<T> : IDataConverter
{
    T Serialize(object? data);
    TResult? Deserialize<TResult>(T? data);
    IDictionary<string, object> Parse(T data);
}