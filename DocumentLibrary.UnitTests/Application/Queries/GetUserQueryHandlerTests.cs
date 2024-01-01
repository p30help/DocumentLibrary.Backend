using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Users;
using DocumentLibrary.Domain.ValueObjects;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using DocumentsLibrary.Application.Queries.GetUser;
using FluentAssertions;
using Moq;

namespace DocumentLibrary.UnitTests.Application.Queries
{
    public class GetUserQueryHandlerTests
    {
        GetUserQueryHandler sut;
        Mock<IUserRepository> userRepositoryMock = new();
        GetUserQuery command = new()
        {
            UserId = Guid.NewGuid(),
        };
        AppUser appUser = AppUser.Create(new EmailField("a@b.c"), new PasswordField("12345678"));

        public GetUserQueryHandlerTests()
        {
            SetupGetUserWithPassword(appUser);

            sut = new GetUserQueryHandler(
                userRepositoryMock.Object
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
            result.IsNotFound.Should().BeTrue();
            result.ErrorMessage.Should().Be("User not found");
        }


        [Fact]
        public async Task Handle_WhenUserExisted_ReturnSuccessResult()
        {
            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
        }

        private void SetupGetUserWithPassword(AppUser? user)
        {
            userRepositoryMock.Setup(x => x.GetUser(It.IsAny<Guid>()))
                .ReturnsAsync(user);
        }

    }
}
