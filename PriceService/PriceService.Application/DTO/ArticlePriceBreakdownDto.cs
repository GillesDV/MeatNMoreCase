namespace PriceService.Application.DTO;

public sealed class ArticlePriceBreakdownDto
{
    public int ArticleId { get; init; }

    public string Unit { get; init; } = string.Empty;

    public decimal DefaultUnitPriceInEuros { get; init; }

    public IReadOnlyCollection<ArticlePriceTierDto> PriceTiers { get; init; } = [];
}
