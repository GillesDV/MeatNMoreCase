namespace PriceService.Application.DTO;

public sealed class ArticlePriceTierDto
{
    public int? MinimumQuantity { get; init; }

    public decimal UnitPriceInEuros { get; init; }

    public decimal ReductionPercentage { get; init; }

    public string Description { get; init; } = string.Empty;
}
