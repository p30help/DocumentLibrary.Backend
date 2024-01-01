using TariffComparison.Infrastructure.Repositories;

namespace TariffComparison.Tests.Repositories
{
    public class TariffRepositoriesTests
    {
        TariffRepositories sut;

        public TariffRepositoriesTests()
        {
            sut = new TariffRepositories();
        }

        [Fact]
        public void GetAllTariffs_WhenCallIt_ShouldReturnTartiffs()
        {
            // act
            var list = sut.GetAllTariffs();

            //
            list.Should().NotBeEmpty();
        }
    }
}
