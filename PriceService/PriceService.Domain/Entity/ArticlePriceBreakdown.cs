namespace PriceService.Domain.Entity;

public sealed class ArticlePriceBreakdown
{
    public int ArticleId { get; init; }

    public string Unit { get; init; } = string.Empty;

    public decimal DefaultUnitPriceInEuros { get; init; }

    public IReadOnlyCollection<ArticlePriceTier> PriceTiers { get; init; } = [];
}
