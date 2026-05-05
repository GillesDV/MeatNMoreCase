namespace PriceService.Domain.Entity;

public class ArticlePrice
{
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
        var defaultTier = new ArticlePriceTier
        {
            UnitPriceInEuros = BasicPriceInEuros,
            ReductionPercentage = 0m
        };

        if (!unit.Equals("kilogram", StringComparison.InvariantCultureIgnoreCase))
        {
            return [defaultTier];
        }

        return
        [
            defaultTier,
            new ArticlePriceTier
            {
                MinimumQuantity = 10,
                UnitPriceInEuros = CalculateReducedUnitPrice(0.10m),
                ReductionPercentage = 0.10m
            },
            new ArticlePriceTier
            {
                MinimumQuantity = 20,
                UnitPriceInEuros = CalculateReducedUnitPrice(0.20m),
                ReductionPercentage = 0.20m
            }
        ];
    }

    private decimal CalculateReducedUnitPrice(decimal reductionPercentage)
    {
        return Math.Round(BasicPriceInEuros * (1 - reductionPercentage), 2);
    }

    private ArticlePriceTier GetPriceTierForQuantity(int quantity, string unit)
    {
        return CalculatePriceTiers(unit)
            .Where(tier => tier.MinimumQuantity is null || quantity >= tier.MinimumQuantity)
            .MaxBy(tier => tier.MinimumQuantity ?? 0)
            ?? CalculatePriceTiers(unit).First();
    }
}
