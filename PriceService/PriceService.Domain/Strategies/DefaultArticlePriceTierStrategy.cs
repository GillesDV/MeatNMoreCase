namespace PriceService.Domain.Strategies;

using PriceService.Domain.Entity;

public sealed class DefaultArticlePriceTierStrategy : ArticlePriceTierStrategyBase
{
    public override bool CanCalculateFor(string unit)
    {
        return true;
    }

    public override IReadOnlyCollection<ArticlePriceTier> CalculatePriceTiers(decimal basicPriceInEuros)
    {
        return [CreateDefaultTier(basicPriceInEuros)];
    }
}
