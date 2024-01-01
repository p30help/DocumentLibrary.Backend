using DocumentLibrary.Domain.Common.Exceptions;
using DocumentLibrary.Domain.Users;
using DocumentLibrary.Domain.ValueObjects;
using FluentAssertions;

namespace DocumentLibrary.UnitTests.Domain.Models
{
    public class AppUserTests
    {
        EmailField _email = new EmailField("a@b.c");
        PasswordField _pass = new PasswordField("12345678");

        [Fact]
        public void AppUser_WhenEmailIsEmpty_ShouldThrowException()
        {
            // act
            Action act = () => AppUser.Create(null, _pass);

            // assert
            var exception = Assert.Throws<DomainStateException>(act);
            exception.Message.Should().Be("Email must be defined");
        }

        [Fact]
        public void AppUser_WhenPasswordIsNull_ShouldThrowException()
        {
            // act
            Action act = () => AppUser.Create(_email, null);

            // assert
            var exception = Assert.Throws<DomainStateException>(act);
            exception.Message.Should().Be("Password must be defined");
        }

        [Fact]
        public void AppUser_WhenInputsAreCorrect_ShouldRetrunObject()
        {
            // act
            var appRole = AppUser.Create(_email, _pass);

            // assert
            appRole.Should().NotBeNull();
        }

        [Fact]
        public void AppUser_EmailAndUserName_ShouldBeSame()
        {
            var email = _email.Value;

            // act
            var user = AppUser.Create(_email, _pass);

            // assert
            user.Email.Should().Be(email);
            user.UserName.Should().Be(email);
        }
    }
}