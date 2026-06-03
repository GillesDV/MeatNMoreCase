namespace PriceService.Domain.Strategies;

using PriceService.Domain.Entity;

public interface IArticlePriceTierStrategy
{
    bool CanCalculateFor(string unit);

    IReadOnlyCollection<ArticlePriceTier> CalculatePriceTiers(decimal basicPriceInEuros);
}
