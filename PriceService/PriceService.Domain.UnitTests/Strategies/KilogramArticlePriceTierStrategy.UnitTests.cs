using PriceService.Domain.Strategies;

namespace PriceService.Domain.UnitTests.Strategies;

public sealed class KilogramArticlePriceTierStrategyUnitTests
{
    [Test]
    public void CanCalculateFor_ReturnsTrue_WhenUnitIsKilogramIgnoringCase()
    {
        // Arrange
        var strategy = new KilogramArticlePriceTierStrategy();

        // Act
        var canCalculate = strategy.CanCalculateFor("KILOGRAM");

        // Assert
        Assert.That(canCalculate, Is.True);
    }

    [Test]
    public void CalculatePriceTiers_ReturnsDefaultTenAndTwentyKilogramPriceTiers()
    {
        // Arrange
        var strategy = new KilogramArticlePriceTierStrategy();
        var basicPriceInEuros = 5.00m;
        var expectedPriceTiers = new[]
        {
            new { MinimumQuantity = (int?)null, UnitPriceInEuros = 5.00m, ReductionPercentage = 0.00m },
            new { MinimumQuantity = (int?)10, UnitPriceInEuros = 4.50m, ReductionPercentage = 0.10m },
            new { MinimumQuantity = (int?)20, UnitPriceInEuros = 4.00m, ReductionPercentage = 0.20m }
        };

        // Act
        var priceTiers = strategy.CalculatePriceTiers(basicPriceInEuros).ToArray();

        // Assert
        Assert.That(priceTiers, Has.Length.EqualTo(expectedPriceTiers.Length));

        for (var index = 0; index < expectedPriceTiers.Length; index++)
        {
            Assert.That(priceTiers[index].MinimumQuantity, Is.EqualTo(expectedPriceTiers[index].MinimumQuantity));
            Assert.That(priceTiers[index].UnitPriceInEuros, Is.EqualTo(expectedPriceTiers[index].UnitPriceInEuros));
            Assert.That(priceTiers[index].ReductionPercentage, Is.EqualTo(expectedPriceTiers[index].ReductionPercentage));
        }
    }
}
