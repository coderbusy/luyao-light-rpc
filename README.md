# LuYao.LightRpc

[![NuGet](https://img.shields.io/nuget/v/LuYao.LightRpc.svg)](https://www.nuget.org/packages/LuYao.LightRpc/)
[![NuGet](https://img.shields.io/nuget/dt/LuYao.LightRpc.svg)](https://www.nuget.org/packages/LuYao.LightRpc/)
[![License](https://img.shields.io/github/license/coderbusy/luyao-light-rpc)](LICENSE)

一个支持 AOT（Ahead-of-Time）编译的轻量级 RPC 框架。

## 项目简介

LuYao.LightRpc 是一个专为 .NET 平台设计的轻量级远程过程调用（RPC）框架。它的核心优势在于：

- **支持 AOT 编译**：完全支持 .NET 的原生 AOT 编译，可生成无需运行时的原生可执行文件
- **轻量级设计**：最小化的依赖和开销，适合微服务和无服务器架构
- **源代码生成器**：利用 C# 源代码生成器在编译时生成客户端代理代码，零反射开销
- **多框架支持**：支持 .NET Framework 4.5+、.NET Standard 2.0/2.1、.NET 6/7/8
- **灵活的数据转换**：支持多种序列化方式（内置 Newtonsoft.Json 支持）

## 使用方式

### 1. 安装 NuGet 包

服务端：
```bash
dotnet add package LuYao.LightRpc
dotnet add package LuYao.LightRpc.Newtonsoft
```

客户端：
```bash
dotnet add package LuYao.LightRpc.Newtonsoft
dotnet add package LuYao.LightRpc.SourceGenerators
```

### 2. 定义 RPC 服务

```csharp
using LuYao.LightRpc.Attributes;

namespace YourNamespace;

[Controller]
public partial class TestController
{
    public int Sum(int a, int b)
    {
        return a + b;
    }
}
```

### 3. 创建 RPC 服务器

```csharp
using LuYao.LightRpc;
using LuYao.LightRpc.Newtonsoft;

public class MainServer : RpcServer<string>
{
    public MainServer(IDataConverter<string> dataConverter) : base(dataConverter)
    {
        this.Register<TestController>();
    }
}
```

### 4. 在 Web 应用中集成（ASP.NET Core Minimal API）

```csharp
using LuYao.LightRpc;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddSingleton<MainServer>();

var app = builder.Build();

app.Map("/", async (
    HttpContext context,
    [FromQuery(Name = "cmd")] string? action,
    [FromServices] MainServer server) =>
{
    if (action is null) action = string.Empty;
    
    using var reader = new StreamReader(context.Request.Body);
    string json = await reader.ReadToEndAsync();
    
    var input = server.DataConverter.Deserialize(json) ?? EmptyDataPackage.Instance;
    var result = await server.InvokeAsync(action, input);
    
    return new RpcHttpResult(result);
});

app.Run();
```

### 5. 创建 RPC 客户端

```csharp
using LuYao.LightRpc.Attributes;

[RpcClientAgent]
public partial class RpcClient : LuYao.LightRpc.RpcClient<string>
{
    public RpcClient(string endpoint) : base(new RpcTunnel(endpoint))
    {
    }

    [RemoteAction("test/sum")]
    public partial Task<int> SumAsync(int a, int b);
}
```

### 6. 调用远程服务

```csharp
var client = new RpcClient("http://localhost:5000/");
var sum = await client.SumAsync(1, 2);
Console.WriteLine($"sum is : {sum}");
```

## 软件架构

### 核心组件

```
LuYao.LightRpc/
├── src/
│   ├── LuYao.LightRpc/                    # 核心库
│   │   ├── Attributes/                     # 特性定义
│   │   │   ├── ControllerAttribute.cs      # 控制器标记
│   │   │   ├── RemoteActionAttribute.cs    # 远程方法标记
│   │   │   └── RpcClientAgentAttribute.cs  # 客户端代理标记
│   │   ├── Descriptors/                    # 描述符
│   │   ├── Http/                           # HTTP 传输实现
│   │   ├── RpcServer.cs                    # RPC 服务器
│   │   ├── RpcClient.cs                    # RPC 客户端基类
│   │   └── IDataConverter.cs               # 数据转换接口
│   ├── LuYao.LightRpc.Newtonsoft/         # Newtonsoft.Json 集成
│   └── LuYao.LightRpc.SourceGenerators/   # 源代码生成器
├── demo/                                   # 示例项目
│   ├── LuYao.LightRpc.Demo/               # 业务逻辑
│   ├── LuYao.LightRpc.Demo.MinimalApi/    # 服务端示例
│   └── LuYao.LightRpc.Demo.Client/        # 客户端示例
└── tests/                                  # 测试项目
```

### 工作原理

1. **服务端**：
   - 使用 `[Controller]` 特性标记服务类
   - `RpcServer` 在启动时注册所有控制器
   - 接收 HTTP 请求，解析 action 参数和 JSON 数据
   - 调用对应的控制器方法并返回结果

2. **客户端**：
   - 使用 `[RpcClientAgent]` 标记客户端类
   - 使用 `[RemoteAction]` 标记远程方法
   - 源代码生成器在编译时生成方法实现
   - 通过 HTTP 传输调用远程服务

3. **源代码生成**：
   - 编译时分析带有特性的类
   - 自动生成客户端代理方法实现
   - 生成控制器描述符，避免运行时反射
   - 支持 AOT 编译

### 技术特点

- **零反射**：通过源代码生成器在编译时生成所有必要代码
- **类型安全**：强类型的客户端代理，编译时检查
- **可扩展**：支持自定义数据转换器和传输通道
- **高性能**：最小化运行时开销，适合高并发场景

## 依赖组件

### 核心库（LuYao.LightRpc）
- **目标框架**：`net45; net461; netstandard2.0; netstandard2.1; net6.0; net7.0; net8.0`
- **依赖**：无外部依赖（仅使用 .NET 标准库）

### Newtonsoft.Json 集成（LuYao.LightRpc.Newtonsoft）
- **目标框架**：`net45; net461; netstandard2.0; netstandard2.1; net6.0; net8.0`
- **依赖**：
  - `Newtonsoft.Json` 13.0.3
  - `LuYao.LightRpc`

### 源代码生成器（LuYao.LightRpc.SourceGenerators）
- **目标框架**：`netstandard2.0`
- **依赖**：
  - `Microsoft.CodeAnalysis.CSharp` 4.11.0

## NuGet 包

### 主要包

| 包名 | NuGet 链接 | 下载量 |
|------|-----------|--------|
| LuYao.LightRpc | [![NuGet](https://img.shields.io/nuget/v/LuYao.LightRpc.svg)](https://www.nuget.org/packages/LuYao.LightRpc/) | [![NuGet](https://img.shields.io/nuget/dt/LuYao.LightRpc.svg)](https://www.nuget.org/packages/LuYao.LightRpc/) |
| LuYao.LightRpc.Newtonsoft | [![NuGet](https://img.shields.io/nuget/v/LuYao.LightRpc.Newtonsoft.svg)](https://www.nuget.org/packages/LuYao.LightRpc.Newtonsoft/) | [![NuGet](https://img.shields.io/nuget/dt/LuYao.LightRpc.Newtonsoft.svg)](https://www.nuget.org/packages/LuYao.LightRpc.Newtonsoft/) |
| LuYao.LightRpc.SourceGenerators | [![NuGet](https://img.shields.io/nuget/v/LuYao.LightRpc.SourceGenerators.svg)](https://www.nuget.org/packages/LuYao.LightRpc.SourceGenerators/) | [![NuGet](https://img.shields.io/nuget/dt/LuYao.LightRpc.SourceGenerators.svg)](https://www.nuget.org/packages/LuYao.LightRpc.SourceGenerators/) |

### 安装命令

```bash
# 服务端基础包
dotnet add package LuYao.LightRpc

# JSON 序列化支持
dotnet add package LuYao.LightRpc.Newtonsoft

# 客户端源代码生成器
dotnet add package LuYao.LightRpc.SourceGenerators
```

## 适用场景

- ✅ 微服务架构的服务间通信
- ✅ 无服务器（Serverless）应用（支持 Azure Functions、阿里云函数计算）
- ✅ 需要 AOT 编译的高性能应用
- ✅ 内部服务 API 快速开发
- ✅ 轻量级的 RESTful 替代方案

## 性能

根据示例项目测试，在本地环境下：
- 单次调用延迟低
- 10,000 次调用可在秒级完成

## 许可证

本项目采用 [MIT 许可证](LICENSE)。

Copyright (c) 2024 码农很忙

## 相关链接

- [GitHub 仓库](https://github.com/coderbusy/luyao-light-rpc)
- [NuGet 包](https://www.nuget.org/packages/LuYao.LightRpc/)
- [问题反馈](https://github.com/coderbusy/luyao-light-rpc/issues)
