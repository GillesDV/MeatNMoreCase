namespace PriceService.Application.Proxies.ArticleService;

public sealed class ArticleDto
{
    public int ArticleId { get; init; }

    public string Description { get; init; } = string.Empty;

    public string Unit { get; init; }
}
