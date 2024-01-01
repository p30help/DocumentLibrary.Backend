using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Models;
using DocumentsLibrary.Application.Commands.GenerateTemporaryLink;
using DocumentsLibrary.Application.Common;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using FluentAssertions;
using Moq;

namespace DocumentLibrary.UnitTests.Application.Commands
{
    public class GenerateTemporaryLinkCommandHandlerTests
    {
        GenerateTemporaryLinkCommandHandler sut;
        Mock<IDocumentsRepository> documentsRepositoryMock = new();
        Mock<ITempCodeGenerator> tempLinkGeneratorMock = new();
        Mock<IEndpointUrls> endpointUrlsMock = new();
        GenerateTemporaryLinkCommand command = new()
        {
            DocumentId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ExpirationTime = 60
        };
        Document doc = Document.Create(Guid.NewGuid(), Guid.NewGuid(), "a.jpg", "image/gif");

        public GenerateTemporaryLinkCommandHandlerTests()
        {
            SetupGetUserDocument(doc);

            sut = new GenerateTemporaryLinkCommandHandler(
                documentsRepositoryMock.Object,
                tempLinkGeneratorMock.Object,
                endpointUrlsMock.Object
                );
        }

        [Fact]
        public async Task Handle_DocumentIsNotExisted_ReturnFailResult()
        {
            // arrage
            SetupGetUserDocument(null);

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.IsNotFound.Should().BeTrue();
            result.ErrorMessage.Should().Be("Document not found");
        }

        [Fact]
        public async Task Handle_WhenExpirationTimeIsLessThan5Min_ReturnFailResult()
        {
            // arrange
            command.ExpirationTime = 4;

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Expiration time must be greater than 5 minutes");
        }

        [Fact]
        public async Task Handle_WhenGenerateTempLinkIsEmpty_ShouldThrowException()
        {
            // arrange
            SetupGenerateTempLink(null);

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Generating temporary link faced with the problem");
        }

        [Fact]
        public async Task Handle_WhenInputsAreCorrect_ShouldCreateTemporaryLinkAndReturnSuccess()
        {
            // arrange
            var endpointLink = "https://localhost:2020/link";
            SetupGenerateTempLink("link");
            endpointUrlsMock.Setup(x => x.GetDocumentByTempLink(It.IsAny<string>()))
                .Returns(new Uri(endpointLink));
                

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Url.Should().Be(endpointLink);
            result.Data.FileName.Should().Be(doc.FileName);
            result.Data.ContentType.Should().Be(doc.ContentType);
        }

        private void SetupGetUserDocument(Document? document)
        {
            documentsRepositoryMock.Setup(x => x.GetUserDocument(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(document);
        }

        private void SetupGenerateTempLink(string? tempLink)
        {
            tempLinkGeneratorMock.Setup(x => x.GenerateTempCode(It.IsAny<Guid>(), It.IsAny<TimeSpan>()))
                .Returns(tempLink);
        }
    }
}
