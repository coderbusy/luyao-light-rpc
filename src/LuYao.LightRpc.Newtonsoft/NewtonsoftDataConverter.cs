using Newtonsoft.Json;
using System;

namespace LuYao.LightRpc;

public class NewtonsoftDataConverter : IDataConverter<String>
{
    public IDataPackage CreatePackage() => new JObjectDataPackage();

    public IDataPackage? Deserialize(string? data)
    {
        if (string.IsNullOrWhiteSpace(data)) return EmptyDataPackage.Instance;
        return JsonConvert.DeserializeObject<JObjectDataPackage>(data!);
    }

    public string Serialize(IDataPackage data) => JsonConvert.SerializeObject(data);
}
