using System.Text.Json.Serialization;

namespace StockService.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StockLocation
    {
        Unknown,
        MainWharehouse,
        ExtraWharehouse,
    }
}
