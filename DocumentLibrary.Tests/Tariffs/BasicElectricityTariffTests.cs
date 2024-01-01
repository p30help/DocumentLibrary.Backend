using TariffComparison.Domain.Common.Exceptions;
using TariffComparison.Domain.Tariffs;
using TariffComparison.Domain.ValueObjects;

namespace TariffComparison.Tests.Core;

public class BasicElectricityTariffTests
{    
    [Fact]
    public void Create_WhenNameIsEmpty_ShouldThrowException()
    {
        // arrange
        var name = "  ";

        // act
        var act = () => BasicElectricityTariff.Create(name, Money.Zero, Money.Zero);

        // assert
        act.Should().Throw<DomainStateException>().WithMessage("Name can not be null or empty.");
    }

    [Theory]
    [InlineData(0, 60)]
    [InlineData(3500, 830)]
    [InlineData(4500, 1050)]
    [InlineData(6000, 1380)]
    public void Calculate_WhenEnterDifferentConsumption_ShouldCalculateThenCorrectly(int consumption, decimal annualCost)
    {
        // arrange
        var tarrif = BasicElectricityTariff.Create("Basic Electricity Tariff", new Money(0.22m), new Money(5));

        // act
        var actualAnnualCost = tarrif.CalculateAnnualCost(consumption);

        // assert
        actualAnnualCost.Should().Be(annualCost);
    }
}
