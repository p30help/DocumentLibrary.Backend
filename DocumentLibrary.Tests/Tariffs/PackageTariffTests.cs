using TariffComparison.Domain.Common.Exceptions;
using TariffComparison.Domain.Tariffs;
using TariffComparison.Domain.ValueObjects;

namespace TariffComparison.Tests.Core;

public class PackageTariffTests
{    
    [Fact]
    public void Create_WhenNameIsEmpty_ShouldThrowException()
    {
        // arrange
        var name = "  ";

        // act
        var act = () => PackageTariff.Create(name, Money.Zero, 10, Money.Zero);

        // assert
        act.Should().Throw<DomainStateException>().WithMessage("Name can not be null or empty.");
    }

    [Fact]
    public void Create_WhenThresholdIsNegative_ShouldThrowException()
    {
        // arrange
        var threshold = -1;

        // act
        var act = () => PackageTariff.Create("name", Money.Zero, threshold, Money.Zero);

        // assert
        act.Should().Throw<DomainStateException>().WithMessage("Threshold can not be negative.");
    }

    [Theory]
    [InlineData(0, 800)]
    [InlineData(3500, 800)]
    [InlineData(4500, 950)]
    [InlineData(6000, 1400)]
    public void Calculate_WhenEnterDifferentConsumption_ShouldCalculateThenCorrectly(int consumption, decimal annualCost)
    {
        // arrange
        var tarrif = PackageTariff.Create("Package Tariff", new Money(800), 4000, new Money(0.30m));

        // act
        var actualAnnualCost = tarrif.CalculateAnnualCost(consumption);

        // assert
        actualAnnualCost.Should().Be(annualCost);
    }
}
