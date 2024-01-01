using DocumentLibrary.Domain.Common.Exceptions;
using DocumentLibrary.Domain.ValueObjects;
using FluentAssertions;

namespace DocumentLibrary.UnitTests.Domain.ValueObjects
{
    public class EmailFieldTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("aaa")]
        [InlineData("aaa@")]
        [InlineData("@asd")]
        [InlineData("@asd.com")]
        [InlineData("ali@local.")]
        public void EmailField_WhenInNotCorrect_ShouldThrowException(string email)
        {
            // act
            Action act = () => new EmailField(email);

            // assert
            var exception = Assert.Throws<InvalidValueObjectStateException>(act);
        }

        [Fact]
        public void EmailField_WhenIsCorrect_ShouldReturnObject()
        {
            var email = "a@b.c";

            // act
            var result = new EmailField(email);

            // assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void EmailField_ShouldEmailBecomeLowercase()
        {
            var email = "A@B.C";

            // act
            var result = new EmailField(email);

            // assert
            result.Value.Should().Be("a@b.c");
        }

        [Fact]
        public void EmailField_2DifferentEmailObjectShouldBeEqual()
        {
            // arrange
            var email1 = "A@B.C";
            var email2 = "a@b.c";

            // act
            var result1 = new EmailField(email1);
            var result2 = new EmailField(email2);

            // assert
            result1.Should().Be(result2);
        }
    }
}