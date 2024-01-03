using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Models;
using DocumentLibrary.Domain.Users;
using DocumentLibrary.Domain.ValueObjects;
using DocumentsLibrary.Application.Commands.UploadDocument;
using DocumentsLibrary.Application.Contracts;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using FluentAssertions;
using Moq;

namespace DocumentLibrary.UnitTests.Application.Commands
{
    public class UploadDocumentCommandHandlerTests
    {
        UploadDocumentCommandHandler sut;
        Mock<IDocumentsRepository> documentsRepositoryMock = new();
        Mock<IFileRepository> fileRepositoryMock = new();
        Mock<IThumbnailGenerator> thumbnailGeneratorMock = new();
        UploadDocumentCommand command = new()
        {
            FileName = "a.txt",
            UserId = Guid.NewGuid(),
            ContentType = "text/plain",
            FileStream = new MemoryStream()
        };
        AppUser appUser = AppUser.Create(new EmailField("a@b.c"), new PasswordField("12345678"));
        UserRole[] appRoles = [UserRole.User];

        public UploadDocumentCommandHandlerTests()
        {            
            sut = new UploadDocumentCommandHandler(
                documentsRepositoryMock.Object,
                fileRepositoryMock.Object,
                thumbnailGeneratorMock.Object
                );
        }

        [Fact]
        public async Task Handle_WhenUploadFileThrowsException_ShouldAddDocumentNeverBeCalled()
        {
            // arrage
            fileRepositoryMock.Setup(x => x.UploadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<DocumentAccessPolicy>()))
                .ThrowsAsync(new Exception("error"));

            command.FileName = "a.txt";
            command.ContentType = "text/plain";

            // act
            var act = async () =>  await sut.Handle(command, CancellationToken.None);

            // assert
            await act.Should().ThrowAsync<Exception>();
            documentsRepositoryMock.Verify(x => x.AddDocument(It.IsAny<Document>()),
                Times.Never);
            thumbnailGeneratorMock.Verify(x => x.GenerateImageThumbnail(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Never);
            thumbnailGeneratorMock.Verify(x => x.GeneratePdfThumbnail(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_WhenAddDocumentThrowsException_ShouldUploadFileBeCalledOnce()
        {
            // arrage
            documentsRepositoryMock.Setup(x => x.AddDocument(It.IsAny<Document>()))
                .ThrowsAsync(new Exception("error"));

            command.FileName = "a.txt";
            command.ContentType = "text/plain";

            // act
            var act = async () => await sut.Handle(command, CancellationToken.None);

            // assert
            await act.Should().ThrowAsync<Exception>();
            fileRepositoryMock.Verify(x => x.UploadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<DocumentAccessPolicy>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenAddDocumentThrowsException_ShouldUploadFileBeCalledTwoTimes()
        {
            // arrage
            documentsRepositoryMock.Setup(x => x.AddDocument(It.IsAny<Document>()))
                .ThrowsAsync(new Exception("error"));

            command.FileName = "a.jpeg";
            command.ContentType = "image/jpeg";

            // act
            var act = async () => await sut.Handle(command, CancellationToken.None);

            // assert
            await act.Should().ThrowAsync<Exception>();
            fileRepositoryMock.Verify(x => x.UploadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<DocumentAccessPolicy>()),
                Times.Exactly(2));
        }


        [Fact]
        public async Task Handle_DocumentHasNotThumbnail_ShouldRunSuccessfullyAndGenerateThumbnailNeverBeCalled()
        {
            // arrage
            command.FileName = "a.txt";
            command.ContentType = "text/plain";

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            thumbnailGeneratorMock.Verify(x => x.GenerateImageThumbnail(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Never);
            thumbnailGeneratorMock.Verify(x => x.GeneratePdfThumbnail(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_DocumentIsImage_ShouldRunSuccessfullyAndGenerateImageThumbnailBeCalledOnce()
        {
            // arrage
            command.FileName = "a.jpeg";
            command.ContentType = "image/jpeg";

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            thumbnailGeneratorMock.Verify(x => x.GenerateImageThumbnail(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Once);
            thumbnailGeneratorMock.Verify(x => x.GeneratePdfThumbnail(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_DocumentIsPdf_ShouldRunSuccessfullyAndGeneratePdfThumbnailBeCalledOnce()
        {
            // arrage
            command.FileName = "a.pdf";
            command.ContentType = "application/pdf";

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            thumbnailGeneratorMock.Verify(x => x.GenerateImageThumbnail(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Never);
            thumbnailGeneratorMock.Verify(x => x.GeneratePdfThumbnail(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Once);
        }


        [Fact]
        public async Task Handle_WhenProcessWorksCorrectly_ShouldReturnSuccess()
        {
            // arrage
            command.FileName = "a.pdf";
            command.ContentType = "application/pdf";

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Data.FileName.Should().Be(command.FileName);
        }
    }
}
