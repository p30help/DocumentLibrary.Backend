using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Models;
using DocumentsLibrary.Application.Queries.GetDocumentStream;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using FluentAssertions;
using Moq;

namespace DocumentLibrary.UnitTests.Application.Queries
{
    public class GetDocumentStreamQueryHandlerTests
    {
        GetDocumentStreamQueryHandler sut;
        Mock<IDocumentsRepository> documentsRepositoryMock = new();
        Mock<IFileRepository> fileRepositoryMock = new();
        GetDocumentStreamQuery command = new()
        {
            DocumentId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
        };
        Document doc = Document.Create(Guid.NewGuid(), Guid.NewGuid(), "a.jpg", "image/png");
        MemoryStream fileStream = new MemoryStream();

        public GetDocumentStreamQueryHandlerTests()
        {
            SetupGetUserDocument(doc);
            SetupGetFileStream(fileStream);

            sut = new GetDocumentStreamQueryHandler(
                documentsRepositoryMock.Object,
                fileRepositoryMock.Object
                );
        }

        [Fact]
        public async Task Handle_WhenDocumentIsNotFound_ReturnNotFoundResult()
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
        public async Task Handle_WhenDocumentIsFound_ShouldRetunResult()
        {
            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Data.FileName.Should().Be(doc.FileName);
            result.Data.ContentType.Should().Be(doc.ContentType);
        }

        [Fact]
        public async Task Handle_WhenDocumentIsFound_ShouldIncreaseDownloadCount()
        {
            // arrage
            var expectedDownloadCount = doc.DownloadCount + 1;

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            doc.DownloadCount.Should().Be(expectedDownloadCount);
        }

        private void SetupGetFileStream(Stream? stream)
        {
            fileRepositoryMock.Setup(x => x.GetFileStream(It.IsAny<Guid>(), It.IsAny<DocumentAccessPolicy>()))
                .ReturnsAsync(stream);
        }

        private void SetupGetUserDocument(Document? document)
        {
            documentsRepositoryMock.Setup(x => x.GetUserDocument(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(document);
        }
    }
}
