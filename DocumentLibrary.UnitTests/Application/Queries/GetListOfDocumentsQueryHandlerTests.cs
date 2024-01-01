using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Models;
using DocumentLibrary.Domain.Users;
using DocumentLibrary.Domain.ValueObjects;
using DocumentsLibrary.Application.Queries.GetListOfDocuments;
using FluentAssertions;
using Moq;

namespace DocumentLibrary.UnitTests.Application.Queries
{
    public class GetListOfDocumentsQueryHandlerTests
    {
        GetListOfDocumentsQueryHandler sut;
        Mock<IDocumentsRepository> documentsRepositoryMock = new();
        Mock<IFileRepository> fileRepositoryMock = new();
        GetListOfDocumentsQuery command = new()
        {
            UserId = Guid.NewGuid(),
            PageNumber = 1,
            PageSize = 50
        };
        AppUser appUser = AppUser.Create(new EmailField("a@b.c"), new PasswordField("12345678"));
        UserRole[] appRoles = [UserRole.User];

        public GetListOfDocumentsQueryHandlerTests()
        {
            fileRepositoryMock.Setup(x => x.GetDirectFileUrl(It.IsAny<Guid>(), It.IsAny<DocumentAccessPolicy>()))
                .Returns("http://localhost/file/123");

            sut = new GetListOfDocumentsQueryHandler(
                documentsRepositoryMock.Object,
                fileRepositoryMock.Object
                );
        }

        [Fact]
        public async Task Handle_WhenGetListOfDocuments_ShouldReturnThemSuccessfully()
        {
            // arrage
            var docs = new List<Document>()
            {
                Document.Create(Guid.NewGuid(), command.UserId, "a.jpeg", "image/jpeg")
            };
            SetupGetUserDocuments(docs);

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Data.Count().Should().Be(1);
            var firstDoc = result.Data.Data.First();
            firstDoc.DocumentType.Should().Be("picture");
            firstDoc.ThumbnailUrl.Should().BeNull();
        }

        [Fact]
        public async Task Handle_WhenDocumentThumbnailIdIsNotNull_ShouldThumbnailUrlBeFilled()
        {
            // arrage
            var doc = Document.Create(Guid.NewGuid(), command.UserId, "a.jpeg", "image/jpeg");
            doc.SetThumbnailId(Guid.NewGuid());
            var docs = new List<Document>() { doc };
            SetupGetUserDocuments(docs);

            // act
            var result = await sut.Handle(command, CancellationToken.None);

            // assert
            result.IsSuccess.Should().BeTrue();
            var firstDoc = result.Data!.Data.First();
            firstDoc.ThumbnailUrl.Should().NotBeNull();
        }

        private void SetupGetUserDocuments(List<Document> docs)
        {
            var pageData = new DocumentLibrary.Domain.Common.PagedData<Document>(docs, 1, 50);

            documentsRepositoryMock.Setup(x => x.GetUserDocuments(It.IsAny<Guid>(), It.IsAny<Pagination>()))
                .ReturnsAsync(pageData);
        }

    }
}
