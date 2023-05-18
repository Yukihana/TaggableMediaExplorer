using System.Text.Json;

namespace TTX.Library.Configurations;

public static partial class JsonHelper
{
    public static readonly JsonSerializerOptions ApiEgressOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static readonly JsonSerializerOptions ApiIngressOptions = new()
    {
    };
}