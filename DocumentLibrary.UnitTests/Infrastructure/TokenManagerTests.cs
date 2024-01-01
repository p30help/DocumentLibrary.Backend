using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Users;
using DocumentLibrary.Domain.ValueObjects;
using DocumentLibrary.Infrastructure.Minio;
using DocumentLibrary.Infrastructure.Token;
using FluentAssertions;
using Moq;

namespace DocumentLibrary.UnitTests.Infrastructure
{
    public class TokenManagerTests
    {
        TokenManager sut;
        Mock<IDateProvider> dateProviderMock = new();

        JwtConfiguration jwtConfig = new()
        {
            Audience = "aud",
            ExpireIn = 3600,
            Issuer = "iss",
            Key = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
        };
        AppUser appUser = AppUser.Create(new EmailField("a@b.c"), new PasswordField("12345678"));

        public TokenManagerTests()
        {
            dateProviderMock.Setup(x => x.Now)
                .Returns(new DateTime(2022, 1, 1));

            sut = new TokenManager(jwtConfig,
                dateProviderMock.Object
                );
        }

        [Fact]
        public void GenerateToken_WhenUserIsNull_ShouldThrowException()
        {
            // arrange
            AppUser user = null;
            UserRole[] roles = [UserRole.User];

            // act
            var act = () => sut.GenerateToken(user, roles);

            // assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GenerateToken_WhenRoleIsNull_ShouldThrowException()
        {
            // arrange
            UserRole[] roles = null;

            // act
            var act = () => sut.GenerateToken(appUser, roles);

            // assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GenerateToken_WhenUserAndRoleIsNotNull_ShouldWorksProperly()
        {
            // arrange
            var expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjAwMDAwMDAwLTAwMDAtMDAwMC0wMDAwLTAwMDAwMDAwMDAwMCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFAYi5jIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiVXNlciIsImV4cCI6MTY0MDk5NTIwMCwiaXNzIjoiaXNzIiwiYXVkIjoiYXVkIn0.Mlo1pTDBMdAc8ftIROrl0yXbzUD3JXxIU8t_SCyc9pc";
            UserRole[] roles = [UserRole.User];

            // act
            var token = sut.GenerateToken(appUser, roles);

            // assert
            token.Should().Be(expectedToken);
        }

    }
}
