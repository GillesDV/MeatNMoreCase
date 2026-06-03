namespace PriceService.Domain.Entity;

using PriceService.Domain.Strategies;

public class ArticlePrice
{
    private static readonly IReadOnlyCollection<IArticlePriceTierStrategy> PriceTierStrategies =
    [
        new KilogramArticlePriceTierStrategy(),
        new DefaultArticlePriceTierStrategy()
    ];

    public int ArticleId { get; set; }

    public decimal BasicPriceInEuros { get; set; }

    public decimal CalculateTotalPrice(int quantityInKg, int quantityInStock, string unit)
    {
        var selectedTier = GetPriceTierForQuantity(quantityInKg, unit);

        return Math.Round(selectedTier.UnitPriceInEuros * quantityInKg, 2);
    }

    public ArticlePriceBreakdown CalculatePriceBreakdown(string unit)
    {
        var priceTiers = CalculatePriceTiers(unit);

        return new ArticlePriceBreakdown
        {
            ArticleId = ArticleId,
            Unit = unit,
            DefaultUnitPriceInEuros = BasicPriceInEuros,
            PriceTiers = priceTiers
        };
    }

    private IReadOnlyCollection<ArticlePriceTier> CalculatePriceTiers(string unit)
    {
        return PriceTierStrategies
            .First(strategy => strategy.CanCalculateFor(unit))
            .CalculatePriceTiers(BasicPriceInEuros);
    }

    private ArticlePriceTier GetPriceTierForQuantity(int quantity, string unit)
    {
        return CalculatePriceTiers(unit)
            .Where(tier => tier.MinimumQuantity is null || quantity >= tier.MinimumQuantity)
            .MaxBy(tier => tier.MinimumQuantity ?? 0)
            ?? CalculatePriceTiers(unit).First();
    }
}
