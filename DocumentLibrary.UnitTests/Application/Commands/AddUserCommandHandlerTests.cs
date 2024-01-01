using DocumentLibrary.Domain.Common.Exceptions;
using DocumentLibrary.Domain.Contracts;
using DocumentsLibrary.Application.Commands.AddUser;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using FluentAssertions;
using Moq;

namespace DocumentLibrary.UnitTests.Application.Commands
{
    public class AddUserCommandHandlerTests
    {
        AddUserCommandHandler sut;
        Mock<IUserRepository> userRepositoryMock = new();
        AddUserCommand command = new AddUserCommand()
        {
            Email = "a@b.c",
            Password = "12345678"
        };

        public AddUserCommandHandlerTests()
        {
            sut = new AddUserCommandHandler(userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenUserExisted_ReturnFailure()
        {
            // arrage
            userRepositoryMock.Setup(x => x.ExistUser(It.IsAny<string>()))
                .ReturnsAsync(true);

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("User already existed");
        }

        [Fact]
        public async Task Handle_WhenEmailIsNotCorrect_ShouldThrowException()
        {
            // arrange
            command.Email = "wonge";

            // act
            var act = async () => await sut.Handle(command, CancellationToken.None);

            // assert
            await act.Should().ThrowAsync<DomainStateException>();
        }

        [Fact]
        public async Task Handle_WhenPasswordIsNotCorrect_ShouldThrowException()
        {
            // arrange
            command.Password = "";

            // act
            var act = async () => await sut.Handle(command, CancellationToken.None);

            // assert
            await act.Should().ThrowAsync<DomainStateException>();
        }

        [Fact]
        public async Task Handle_WhenUserDataIsCorrect_ShouldCreateUserAndReturnSuccess()
        {
            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            userRepositoryMock.Verify(x => x.ExistUser(It.IsAny<string>()), Times.Once);
        }
    }
}
