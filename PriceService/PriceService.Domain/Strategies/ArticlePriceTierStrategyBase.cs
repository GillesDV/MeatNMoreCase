namespace PriceService.Domain.Strategies;

using PriceService.Domain.Entity;

public abstract class ArticlePriceTierStrategyBase : IArticlePriceTierStrategy
{
    public abstract bool CanCalculateFor(string unit);

    public abstract IReadOnlyCollection<ArticlePriceTier> CalculatePriceTiers(decimal basicPriceInEuros);

    protected static ArticlePriceTier CreateDefaultTier(decimal basicPriceInEuros)
    {
        return new ArticlePriceTier
        {
            UnitPriceInEuros = basicPriceInEuros,
            ReductionPercentage = 0m
        };
    }

    protected static ArticlePriceTier CreateReducedTier(
        int minimumQuantity,
        decimal basicPriceInEuros,
        decimal reductionPercentage)
    {
        return new ArticlePriceTier
        {
            MinimumQuantity = minimumQuantity,
            UnitPriceInEuros = Math.Round(basicPriceInEuros * (1 - reductionPercentage), 2),
            ReductionPercentage = reductionPercentage
        };
    }
}
