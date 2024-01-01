using DocumentLibrary.Domain.Users;
using FluentAssertions;

namespace DocumentLibrary.UnitTests.Domain.Models
{
    public class AppRoleTests
    {
        [Fact]
        public void AppRole_WhenEnterRoleUser_ShouldRetrunObject()
        {
            // act
            var appRole = AppRole.Create(UserRole.User);

            // assert
            appRole.Should().NotBeNull();
        }
    }
}