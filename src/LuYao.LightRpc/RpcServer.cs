using LuYao.LightRpc.Attributes;
using LuYao.LightRpc.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LuYao.LightRpc;

public class RpcServer<TData>
{
    public IDataConverter<TData> DataConverter { get; }

    public RpcServer(IDataConverter<TData> dataConverter)
    {
        DataConverter = dataConverter;
    }

    private IDictionary<string, RpcService> _services = new Dictionary<string, RpcService>(StringComparer.OrdinalIgnoreCase);
    public IDictionary<string, RpcService> Services => _services;

    public void Register<TController>() where TController : class, new()
    {
        var type = typeof(TController);
        var attribute = type.GetCustomAttributes<ControllerDescriptorAttribute>(false).FirstOrDefault();
        if (attribute == null) throw new InvalidOperationException($"The controller {type.FullName} is not decorated with RpcControllerDescriptorAttribute.");

        lock (this)
        {
            var controller = new TController();
            var temp = new Dictionary<string, RpcService>(this._services, StringComparer.OrdinalIgnoreCase);
            foreach (var action in attribute.GetActionDescriptors())
            {
                temp[action.Path] = new RpcService(controller, action);
            }
            this._services = temp;
        }
    }

    public async Task<RpcResult> InvokeAsync(string action, TData input)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        var result = new RpcResult
        {
            Code = RpcResultCode.Ok
        };
        if (!this.TryFindService(action, out var service))
        {
            result.Code = RpcResultCode.NotFound;
            return result;
        }
        var parameters = this.DataConverter.Parse(input);
        var context = new InvokeContext(this.DataConverter, parameters);
        try
        {
            await service!.Descriptor.Invoke(service.Controller, context);
            result.Data = context.Result;
        }
        catch (Exception e)
        {
            result.Code = RpcResultCode.InternalServerError;
            result.Message = e.Message;
        }
        return result;
    }

    private bool TryFindService(string action, out RpcService? service)
    {
        if (this.Services.TryGetValue(action, out service))
        {
            return true;
        }
        // 局部模糊匹配
        var p = action.IndexOf('/');
        if (p >= 0)
        {
            var ctrl = action.Substring(0, p);
            if (this.Services.TryGetValue(ctrl + "/*", out service)) return true;
        }
        // 全局模糊匹配
        if (this.Services.TryGetValue("*", out service)) return true;
        return false;
    }
}
