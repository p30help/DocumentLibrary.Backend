using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Models;
using DocumentsLibrary.Application.Common;
using DocumentsLibrary.Application.Queries.GetDocumentStreamByTempUrl;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using FluentAssertions;
using Moq;

namespace DocumentLibrary.UnitTests.Application.Queries
{
    public class GetDocumentStreamByTempUrlQueryHandlerTests
    {
        GetDocumentStreamByTempUrlQueryHandler sut;
        Mock<IDocumentsRepository> documentsRepositoryMock = new();
        Mock<IFileRepository> fileRepositoryMock = new();
        Mock<ITempCodeGenerator> tempLinkGeneratorMock = new();

        GetDocumentStreamByTempUrlQuery command = new()
        {
            EncryptedText = "aaaa"
        };
        Document doc = Document.Create(Guid.NewGuid(), Guid.NewGuid(), "a.jpg", "image/png");
        MemoryStream fileStream = new MemoryStream();

        public GetDocumentStreamByTempUrlQueryHandlerTests()
        {
            SetupReturnGetDocument(doc);
            SetupGetFileStream(fileStream);

            sut = new GetDocumentStreamByTempUrlQueryHandler(
                documentsRepositoryMock.Object,
                tempLinkGeneratorMock.Object,
                fileRepositoryMock.Object
                );
        }


        [Fact]
        public async Task Handle_WhenTempCodeIsNotValid_ReturnNotFoundResult()
        {
            // arrage
            SetupReturnValidateTempCode(new TempCode() { IsValid = false });

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeFalse();
            result.IsNotFound.Should().BeTrue();
            result.ErrorMessage.Should().Be("Document not found");
        }

        [Fact]
        public async Task Handle_WhenDocumentIsNotFound_ReturnNotFoundResult()
        {
            // arrage
            SetupReturnValidateTempCode(new TempCode() { IsValid = true, DocumentId = Guid.NewGuid() });
            SetupReturnGetDocument(null);

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
            // arrange
            SetupReturnValidateTempCode(new TempCode() { IsValid = true, DocumentId = Guid.NewGuid() });

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
            SetupReturnValidateTempCode(new TempCode() { IsValid = true, DocumentId = Guid.NewGuid() });
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

        private void SetupReturnGetDocument(Document? document)
        {
            documentsRepositoryMock.Setup(x => x.GetDocument(It.IsAny<Guid>()))
                .ReturnsAsync(document);
        }

        private void SetupReturnValidateTempCode(TempCode tempCode)
        {
            tempLinkGeneratorMock.Setup(x => x.ValidateTempCode(It.IsAny<string>()))
                .Returns(tempCode);
        }
    }
}
