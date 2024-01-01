using MediatR;
using Microsoft.AspNetCore.Mvc;
using TariffComparison.Application.Common;
using TariffComparison.Application.Queries.GetTariffComparison;
using TariffComparison.Controllers;

namespace TariffComparison.Tests.Controllers
{
    public class TariffControllerTests
    {
        TariffController sut;
        Mock<IMediator> mediatorMock = new();

        public TariffControllerTests()
        {
            sut = new TariffController(mediatorMock.Object);
        }

        [Fact]
        public async void GetTariffComparison_WhenQueryHandlerRetunResultSuccessfully_ShouldRetunOk()
        {
            // arange
            var consumption = 0;
            SetupMediator(QueryResult<List<GetTariffComparisonResult>>.Success(new List<GetTariffComparisonResult>()));

            // act
            var result = await sut.GetTariffComparison(consumption);

            // assert
            ((ObjectResult)result).StatusCode.Should().Be(200);
        }

        [Fact]
        public async void GetTariffComparison_WhenQueryHandlerRetunError_ShouldRetunBadRequest()
        {
            // arange
            var consumption = 0;
            SetupMediator(QueryResult<List<GetTariffComparisonResult>>.Failure("Error"));

            // act
            var result = await sut.GetTariffComparison(consumption);

            // assert
            ((ObjectResult)result).StatusCode.Should().Be(400);
        }

        [Fact]
        public async void GetTariffComparison_WhenQueryHandlerRetunError_ShouldRetunNoContent()
        {
            // arange
            var consumption = 0;
            SetupMediator(QueryResult<List<GetTariffComparisonResult>>.NotFound("Not found"));

            // act
            var result = await sut.GetTariffComparison(consumption);

            // assert
            ((ObjectResult)result).StatusCode.Should().Be(404);
        }

        private void SetupMediator(QueryResult<List<GetTariffComparisonResult>> queryResult)
        {
            mediatorMock.Setup(x => x.Send(It.IsAny<GetTariffComparisonQuery>(), CancellationToken.None))
                .ReturnsAsync(queryResult);
        }
    }
}
