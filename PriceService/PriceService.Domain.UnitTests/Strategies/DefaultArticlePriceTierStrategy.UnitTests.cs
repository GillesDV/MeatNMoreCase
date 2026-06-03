using PriceService.Domain.Strategies;

namespace PriceService.Domain.UnitTests.Strategies;

public sealed class DefaultArticlePriceTierStrategyUnitTests
{
    [Test]
    public void CanCalculateFor_ReturnsTrue_ForAnyUnit()
    {
        // Arrange
        var strategy = new DefaultArticlePriceTierStrategy();

        // Act
        var canCalculate = strategy.CanCalculateFor("piece");

        // Assert
        Assert.That(canCalculate, Is.True);
    }

    [Test]
    public void CalculatePriceTiers_ReturnsDefaultTier()
    {
        // Arrange
        var strategy = new DefaultArticlePriceTierStrategy();
        var basicPriceInEuros = 5.00m;

        // Act
        var priceTiers = strategy.CalculatePriceTiers(basicPriceInEuros).ToArray();

        // Assert
        Assert.That(priceTiers, Has.Length.EqualTo(1));
        Assert.That(priceTiers[0].MinimumQuantity, Is.Null);
        Assert.That(priceTiers[0].UnitPriceInEuros, Is.EqualTo(5.00m));
        Assert.That(priceTiers[0].ReductionPercentage, Is.EqualTo(0.00m));
    }
}
