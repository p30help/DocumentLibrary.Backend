using DocumentLibrary.Domain.Common.Exceptions;
using DocumentLibrary.Domain.ValueObjects;
using FluentAssertions;

namespace DocumentLibrary.UnitTests.Domain.ValueObjects
{
    public class PasswordFieldTests
    {        
        public void PasswordField_WhenIsEmpty_ShouldThrowException()
        {
            var pass = "  ";

            // act
            Action act = () => new PasswordField(pass);

            // assert
            var exception = Assert.Throws<InvalidValueObjectStateException>(act);
            exception.Message.Should().Be("Password can not be empty");
        }

        public void PasswordField_WhenIsLessThan8Character_ShouldThrowException(string email)
        {
            var pass = "1234567";

            // act
            Action act = () => new PasswordField(pass);

            // assert
            var exception = Assert.Throws<InvalidValueObjectStateException>(act);
            exception.Message.Should().Be("Password lenght must be at least 8 characters");

        }

        [Fact]
        public void PasswordField_WhenIsEqual8Chars_ShouldReturnObject()
        {
            var pass = "12345678";

            // act
            var result = new PasswordField(pass);

            // assert
            result.Should().NotBeNull();
        }
    }
}