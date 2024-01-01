using DocumentLibrary.Domain.Common;
using DocumentLibrary.Domain.ValueObjects;
using FluentAssertions;

namespace DocumentLibrary.UnitTests.Domain
{
    public class PagedDataTests
    {
        EmailField _email = new EmailField("a@b.c");
        PasswordField _pass = new PasswordField("12345678");

        [Fact]
        public void PagedData_DataIsNull_ShouldThrowException()
        {
            // act
            Action act = () => new PagedData<object>(null, 1, 0);

            // assert
            var exception = Assert.Throws<ArgumentNullException>(act);
            exception.Message.Should().Be("Data can not be null (Parameter 'data')");
        }

        [Fact]
        public void PagedData_WhenTotalCountIsNegative_ShouldThrowException()
        {
            // act
            Action act = () => new PagedData<object>([], 1, -1);

            // assert
            var exception = Assert.Throws<ArgumentException>(act);
            exception.Message.Should().Be("Total count can not be negative (Parameter 'totalCount')");
        }

        [Fact]
        public void PagedData_WhenPageNumberIsZero_ShouldThrowException()
        {
            // act
            Action act = () => new PagedData<object>([], 0, 0);

            // assert
            var exception = Assert.Throws<ArgumentException>(act);
            exception.Message.Should().Be("Page number must be positive (Parameter 'totalCount')");
        }


        [Fact]
        public void PagedData_WhenDataExistedButTotalCountIsZero_ShouldThrowException()
        {
            // act
            Action act = () => new PagedData<object>([new object()], 1, 0);

            // assert
            var exception = Assert.Throws<ArgumentException>(act);
            exception.Message.Should().Be("Total count is not correct (Parameter 'totalCount')");
        }

        [Fact]
        public void PagedData_WhenInputsAreCorrect_ShouldReturnObject()
        {
            // act
            var item = new PagedData<object>([], 1, 0);

            // assert
            item.Should().NotBeNull();
        }

    }
}