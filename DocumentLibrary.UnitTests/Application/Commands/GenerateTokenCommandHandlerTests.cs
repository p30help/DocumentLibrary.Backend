using Castle.DynamicProxy;
using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Models;
using DocumentLibrary.Domain.Users;
using DocumentLibrary.Domain.ValueObjects;
using DocumentsLibrary.Application.Commands.GenerateToken;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using FluentAssertions;
using Moq;

namespace DocumentLibrary.UnitTests.Application.Commands
{
    public class GenerateTokenCommandHandlerTests
    {
        GetTokenCommandHandler sut;
        Mock<IUserRepository> userRepositoryMock = new();
        Mock<ITokenManager> tokenManagerMock = new();
        GetDocumentUrlQuery command = new()
        {
            Email = "a@b.c",
            Password = "12345678"
        };
        AppUser appUser = AppUser.Create(new EmailField("a@b.c"), new PasswordField("12345678"));
        UserRole[] appRoles = [UserRole.User];

        public GenerateTokenCommandHandlerTests()
        {
            SetupGetUserWithPassword(appUser);
            SetupGetUserRoles(appRoles);

            sut = new GetTokenCommandHandler(
                userRepositoryMock.Object,
                tokenManagerMock.Object
                );
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ReturnFailedResult()
        {
            // arrage
            SetupGetUserWithPassword(null);

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("User or password is incorrect");
        }

        [Fact]
        public async Task Handle_WhenUserRolesIsNull_ReturnFailedResult()
        {
            // arrange
            SetupGetUserRoles([]);

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("User or password is incorrect");
        }

        [Fact]
        public async Task Handle_WhenTokenIsGenerated_ReturnSuccess()
        {
            // arrange
            var token = "toooooken";
            tokenManagerMock.Setup(x => x.GenerateToken(It.IsAny<AppUser>(), It.IsAny<UserRole[]>()))
                .Returns(token);

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Data.AccessToken.Should().Be(token);
        }

        private void SetupGetUserWithPassword(AppUser? user)
        {
            userRepositoryMock.Setup(x => x.GetUserWithPassword(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(user);
        }

        private void SetupGetUserRoles(UserRole[] roles)
        {
            userRepositoryMock.Setup(x => x.GetUserRoles(It.IsAny<string>()))
                .ReturnsAsync(roles);
        }
    }
}
