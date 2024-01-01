using TariffComparison.Application.Tariffs;
using TariffComparison.Domain.Common.Exceptions;
using TariffComparison.Domain.Contracts;
using TariffComparison.Domain.Tariffs;
using TariffComparison.Domain.ValueObjects;

namespace TariffComparison.Tests.Applications
{
    public class TariffComparisonCalculationTests
    {
        TariffComparisonCalculation sut;
        Mock<ITariffRepositories> tariffRepositories = new();

        public TariffComparisonCalculationTests()
        {
            sut = new TariffComparisonCalculation(tariffRepositories.Object);
        }

        [Fact]
        public void Calculate_WhenInputIsNegative_ShouldThrowException()
        {
            // arrange
            int consomption = -1;

            // act
            var act = () => sut.CalculateAnnualCost(consomption);

            // assert
            act.Should().Throw<DomainStateException>().WithMessage("Consumption amount can not be negative");
        }

        [Fact]
        public void Calculate_WhenTarrifEmpty_ShouldReturnEmptyList()
        {
            // arrange
            int consomption = 1000;
            SetupTariff();

            // act
            var result = sut.CalculateAnnualCost(consomption);

            // assert
            result.Count().Should().Be(0);
        }

        [Fact]
        public void Calculate_WhenTariffExisted_ShouldReturnTariffComparisons()
        {
            // arrange
            int consomption = 1000;
            SetupTariff(BasicElectricityTariff.Create("B1", Money.Zero, Money.Zero));

            // act
            var result = sut.CalculateAnnualCost(consomption);

            // assert
            result.Count().Should().Be(1);
            result[0].TariffName.Should().Be("B1");
        }

        [Fact]
        public void Calculate_WhenTariffExisted_ShouldTariffsOrderdByCostAsc()
        {
            // arrange
            int consomption = 1000;
            var expensiveTarrif = BasicElectricityTariff.Create("Expensive", new Money(10), new Money(1000));
            var cheaperTarrif = BasicElectricityTariff.Create("Cheap", Money.Zero, Money.Zero);
            SetupTariff(expensiveTarrif, cheaperTarrif);

            // act
            var result = sut.CalculateAnnualCost(consomption);

            // assert
            result.Count().Should().Be(2);
            result[0].TariffName.Should().Be(cheaperTarrif.Name);
        }

        private void SetupTariff(params ITariff[] tariffs)
        {
            tariffRepositories.Setup(x => x.GetAllTariffs())
                .Returns(tariffs?.ToList() ?? new List<ITariff>());
        }
    }
}
