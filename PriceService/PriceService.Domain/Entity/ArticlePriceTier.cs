namespace PriceService.Domain.Entity;

public sealed class ArticlePriceTier
{
    public int? MinimumQuantity { get; init; }

    public decimal UnitPriceInEuros { get; init; }

    public decimal ReductionPercentage { get; init; }

}
