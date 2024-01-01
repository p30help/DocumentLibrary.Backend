using TariffComparison.Application.Queries.GetTariffComparison;
using TariffComparison.Domain.Contracts;
using TariffComparison.Domain.Dto;
using TariffComparison.Domain.ValueObjects;

namespace TariffComparison.Tests.Queries
{
    public class GetTariffComparisonQueryHandlerTests
    {
        GetTariffComparisonQueryHandler sut;
        Mock<ITariffComparisonCalculation> tariffComparisonCalculationMock = new();

        public GetTariffComparisonQueryHandlerTests()
        {
            sut = new GetTariffComparisonQueryHandler(tariffComparisonCalculationMock.Object);
        }

        [Fact]
        public async Task Handle_WhenQueryIsNull_ShouldReturnIsNotSuccessWithErrorMessage()
        {
            // arrange
            GetTariffComparisonQuery? query = null;

            // act
            var result = await sut.Handle(query!, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Consumption amount is incorrect.");
        }

        [Fact]
        public async Task Handle_WhenConsumptionAmountIsLessThanZero_ShouldReturnIsNotSuccessWithErrorMessage()
        {
            // arrange
            var query = new GetTariffComparisonQuery()
            {
                Consumption = -1
            };

            // act
            var result = await sut.Handle(query, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Consumption amount is incorrect.");
        }

        [Fact]
        public async Task Handle_WhenThereAreNoTariffComparison_ShouldReturnEmptyList()
        {
            // arrange
            var query = new GetTariffComparisonQuery()
            {
                Consumption = 1000
            };
            var expectedTariffComparison = new List<TariffComparisonCalculationResult>();
            tariffComparisonCalculationMock.Setup(x => x.CalculateAnnualCost(It.IsAny<int>()))
                .Returns(expectedTariffComparison);

            // act
            var result = await sut.Handle(query, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Data!.Count().Should().Be(0);
        }

        [Fact]
        public async Task Handle_WhenConsumptionAmountBiggerThanZero_ShouldCalculateTarifAndReturnTariffComparisons()
        {
            // arrange
            var query = new GetTariffComparisonQuery()
            {
                Consumption = 1000
            };
            var expectedTariffComparison = new List<TariffComparisonCalculationResult>()
            {
                new TariffComparisonCalculationResult()
                {
                    AnnualCosts = 1000,
                    TariffName = "Basic"
                },
                new TariffComparisonCalculationResult()
                {
                    AnnualCosts = 2000,
                    TariffName = "Pacakge"
                },
            };
            tariffComparisonCalculationMock.Setup(x => x.CalculateAnnualCost(It.IsAny<int>()))
                .Returns(expectedTariffComparison);

            // act
            var result = await sut.Handle(query, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Data!.Count().Should().Be(expectedTariffComparison.Count());
            result.Data!.First(x => x.TariffName == "Basic").AnnualCosts.Should().Be(1000);
            result.Data!.First(x => x.TariffName == "Pacakge").AnnualCosts.Should().Be(2000);
        }
    }
}
