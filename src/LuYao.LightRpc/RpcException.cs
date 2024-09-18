﻿using System;

namespace LuYao.LightRpc;

public class RpcException : Exception
{
    /// <summary>代码</summary>
    public Int32 Code { get; set; }

    /// <summary>实例化远程调用异常</summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public RpcException(Int32 code, String message) : base(message) => Code = code;

    /// <summary>实例化远程调用异常</summary>
    /// <param name="code"></param>
    /// <param name="ex"></param>
    public RpcException(Int32 code, Exception ex) : base(ex.Message, ex) => Code = code;
}
