using DocumentLibrary.Domain.Common.Exceptions;
using DocumentLibrary.Domain.ValueObjects;
using FluentAssertions;

namespace DocumentLibrary.UnitTests.Domain.ValueObjects
{
    public class PaginationTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Pagination_WhenPageNumberIsZeroOrNegative_ShouldThrowException(int pageNumber)
        {
            int pageSize = 10;

            // act
            Action act = () => new Pagination(pageNumber, pageSize);

            // assert
            var exception = Assert.Throws<InvalidValueObjectStateException>(act);
            exception.Message.Should().Be("PageNumber is incorrect");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Pagination_WhenPageSizeIsZeroOrNegative_ShouldThrowException(int pageSize)
        {
            int pageNumber = 10;

            // act
            Action act = () => new Pagination(pageNumber, pageSize);

            // assert
            var exception = Assert.Throws<InvalidValueObjectStateException>(act);
            exception.Message.Should().Be("PageSize is incorrect");
        }

        [Theory]
        [InlineData(101)]
        [InlineData(200)]
        public void Pagination_WhenPageSizeIsMoreThan100_ShouldThrowException(int pageSize)
        {
            int pageNumber = 10;

            // act
            Action act = () => new Pagination(pageNumber, pageSize);

            // assert
            var exception = Assert.Throws<InvalidValueObjectStateException>(act);
            exception.Message.Should().Be("PageSize can not exceed more than 100");
        }

        [Theory]
        [InlineData(1,1)]
        [InlineData(1,10)]
        [InlineData(1,100)]
        [InlineData(100,1)]
        [InlineData(1000, 99)]
        public void Pagination_WhenPageSizeAndPageNumberIsInRange_ShouldReturnObject(int pageNumber, int pageSize)
        {            
            // act
            var res = new Pagination(pageNumber, pageSize);

            // assert
            res.Should().NotBeNull();
        }
    }
}