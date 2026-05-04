using System.Text.Json.Serialization;

namespace ArticleService.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ArticleUnit
    {
        Unknown,
        kilogram,
        piece
    }
}
