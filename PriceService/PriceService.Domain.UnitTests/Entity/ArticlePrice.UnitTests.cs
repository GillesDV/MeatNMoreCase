using PriceService.Domain.Entity;

namespace PriceService.Domain.UnitTests.Entity;

public sealed class ArticlePriceUnitTests
{
    [TestCase(5.00, 9, 0, "kilogram", 45.00)]
    [TestCase(5.00, 9, 100, "kilogram", 45.00)]
    [TestCase(5.00, 10, 100, "kilogram", 45.00)]
    [TestCase(5.00, 19, 100, "kilogram", 85.50)]
    [TestCase(5.00, 20, 100, "kilogram", 80.00)]
    [TestCase(5.00, 21, 100, "kilogram", 84.00)]
    [TestCase(5.00, 20, 100, "piece", 100.00)]
    [TestCase(5.00, 10, 100, "KILOGRAM", 45.00)]
    [TestCase(1.11, 10, 0, "kilogram", 10.00)]
    public void CalculateTotalPrice_ReturnsExpectedTotalPrice(
        double basicPriceInEurosInput,
        int quantityOrderedInput,
        int quantityInStockInput,
        string unitInput,
        double expectedTotalPriceInEurosOutput)
    {
        // Arrange
        var articlePrice = new ArticlePrice
        {
            ArticleId = 7,
            BasicPriceInEuros = (decimal)basicPriceInEurosInput
        };
        var quantityOrdered = quantityOrderedInput;
        var quantityInStock = quantityInStockInput;
        var unit = unitInput;
        var expectedTotalPriceInEuros = (decimal)expectedTotalPriceInEurosOutput;

        // Act
        var totalPriceInEuros = articlePrice.CalculateTotalPrice(quantityOrdered, quantityInStock, unit);

        // Assert
        Assert.That(totalPriceInEuros, Is.EqualTo(expectedTotalPriceInEuros));
    }

    [Test]
    public void CalculatePriceBreakdown_ReturnsDefaultTenAndTwentyKilogramPriceTiers()
    {
        // Arrange
        var articlePrice = new ArticlePrice { ArticleId = 7, BasicPriceInEuros = 5.00m };
        var unit = "kilogram";
        var expectedArticleId = 7;
        var expectedUnit = "kilogram";
        var expectedDefaultUnitPriceInEuros = 5.00m;
        var expectedPriceTiers = new[]
        {
            new { MinimumQuantity = (int?)null, UnitPriceInEuros = 5.00m, ReductionPercentage = 0.00m },
            new { MinimumQuantity = (int?)10, UnitPriceInEuros = 4.50m, ReductionPercentage = 0.10m },
            new { MinimumQuantity = (int?)20, UnitPriceInEuros = 4.00m, ReductionPercentage = 0.20m }
        };

        // Act
        var breakdown = articlePrice.CalculatePriceBreakdown(unit);
        var priceTiers = breakdown.PriceTiers.ToArray();

        // Assert
        Assert.That(breakdown.ArticleId, Is.EqualTo(expectedArticleId));
        Assert.That(breakdown.Unit, Is.EqualTo(expectedUnit));
        Assert.That(breakdown.DefaultUnitPriceInEuros, Is.EqualTo(expectedDefaultUnitPriceInEuros));
        Assert.That(priceTiers, Has.Length.EqualTo(expectedPriceTiers.Length));

        for (var index = 0; index < expectedPriceTiers.Length; index++)
        {
            Assert.That(priceTiers[index].MinimumQuantity, Is.EqualTo(expectedPriceTiers[index].MinimumQuantity));
            Assert.That(priceTiers[index].UnitPriceInEuros, Is.EqualTo(expectedPriceTiers[index].UnitPriceInEuros));
            Assert.That(priceTiers[index].ReductionPercentage, Is.EqualTo(expectedPriceTiers[index].ReductionPercentage));
        }
    }

    [Test]
    public void CalculatePriceBreakdown_ReturnsOnlyDefaultTier_WhenUnitIsNotKilogram()
    {
        // Arrange
        var articlePrice = new ArticlePrice { ArticleId = 7, BasicPriceInEuros = 5.00m };
        var unit = "piece";
        var expectedArticleId = 7;
        var expectedUnit = "piece";
        var expectedDefaultUnitPriceInEuros = 5.00m;
        var expectedMinimumQuantity = (int?)null;
        var expectedUnitPriceInEuros = 5.00m;
        var expectedReductionPercentage = 0.00m;

        // Act
        var breakdown = articlePrice.CalculatePriceBreakdown(unit);
        var priceTiers = breakdown.PriceTiers.ToArray();

        // Assert
        Assert.That(breakdown.ArticleId, Is.EqualTo(expectedArticleId));
        Assert.That(breakdown.Unit, Is.EqualTo(expectedUnit));
        Assert.That(breakdown.DefaultUnitPriceInEuros, Is.EqualTo(expectedDefaultUnitPriceInEuros));
        Assert.That(priceTiers, Has.Length.EqualTo(1));
        Assert.That(priceTiers[0].MinimumQuantity, Is.EqualTo(expectedMinimumQuantity));
        Assert.That(priceTiers[0].UnitPriceInEuros, Is.EqualTo(expectedUnitPriceInEuros));
        Assert.That(priceTiers[0].ReductionPercentage, Is.EqualTo(expectedReductionPercentage));
    }
}
