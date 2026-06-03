namespace PriceService.Domain.Strategies;

using PriceService.Domain.Entity;

public sealed class KilogramArticlePriceTierStrategy : ArticlePriceTierStrategyBase
{
    private const string KilogramUnit = "kilogram";

    public override bool CanCalculateFor(string unit)
    {
        return unit.Equals(KilogramUnit, StringComparison.InvariantCultureIgnoreCase);
    }

    public override IReadOnlyCollection<ArticlePriceTier> CalculatePriceTiers(decimal basicPriceInEuros)
    {
        return
        [
            CreateDefaultTier(basicPriceInEuros),
            CreateReducedTier(10, basicPriceInEuros, 0.10m),
            CreateReducedTier(20, basicPriceInEuros, 0.20m)
        ];
    }
}
