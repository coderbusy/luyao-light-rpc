﻿using System.Collections.Generic;

namespace LuYao.LightRpc;


public interface IDataConverter<T>
{
    T Serialize(object? data);
    TResult Deserialize<TResult>(T? data);
    IDataPackage CreatePackage(T data);
}