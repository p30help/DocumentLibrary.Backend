using DocumentLibrary.Domain.Common.Exceptions;
using DocumentLibrary.Domain.Models;
using FluentAssertions;

namespace DocumentLibrary.UnitTests.Domain.Models
{
    public class DocumentTests
    {
        Guid _docId = Guid.NewGuid();
        Guid _userId = Guid.NewGuid();
        string _contentType = "image/jpeg";
        string _fileName = "image.jpeg";


        [Fact]
        public void Document_WhenDocIdIsEmpty_ShouldThrowException()
        {
            // act
            Action act = () => Document.Create(Guid.Empty, _userId, _fileName, _contentType);

            // assert
            var exception = Assert.Throws<DomainStateException>(act);
            exception.Message.Should().Be("Document Id must be defined");
        }


        [Fact]
        public void Document_WhenUserIdIsEmpty_ShouldThrowException()
        {
            // act
            Action act = () => Document.Create(_docId, Guid.Empty, _fileName, _contentType);

            // assert
            var exception = Assert.Throws<DomainStateException>(act);
            exception.Message.Should().Be("User Id must be defined");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Document_WhenFileNameIsNullOrEmpty_ShouldThrowException(string fileName)
        {
            // act
            Action act = () => Document.Create(_docId, _userId, fileName, _contentType);

            // assert
            var exception = Assert.Throws<DomainStateException>(act);
            exception.Message.Should().Be("File name must be defined");
        }

        [Theory]
        [InlineData("a")]
        [InlineData("a.")]
        public void Document_WhenFilNameHasNotExtension_ShouldThrowException(string fileName)
        {
            // act
            Action act = () => Document.Create(_docId, _userId, fileName, _contentType);

            // assert
            var exception = Assert.Throws<DomainStateException>(act);
            exception.Message.Should().Be("File must have extension");
        }

        [Theory]
        [InlineData("a.mp4")]
        [InlineData("a.mp3")]
        [InlineData("a.html")]
        [InlineData("a.avi")]
        public void Document_WhenFilTypeIsNotAllowed_ShouldThrowException(string fileName)
        {
            // act
            Action act = () => Document.Create(_docId, _userId, fileName, _contentType);

            // assert
            var exception = Assert.Throws<DomainStateException>(act);
            exception.Message.Should().Be("Document type is not allowed. Just picture, pdf, word, text, excel are allowed");
        }

        [Theory]
        [InlineData("a.jpg")]
        [InlineData("a.jpeg")]
        [InlineData("a.png")]
        [InlineData("a.gif")]
        [InlineData("a.pdf")]
        [InlineData("a.xls")]
        [InlineData("a.xlsx")]
        [InlineData("a.doc")]
        [InlineData("a.docx")]
        [InlineData("a.txt")]
        public void Document_CheckAllowedFileTypes_ShouldReturnObject(string fileName)
        {
            // act
            var document = Document.Create(_docId, _userId, fileName, _contentType);

            // assert
            document.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Document_WhenConetntTypeIsNullOrEmpty_ShouldThrowException(string contentType)
        {
            // act
            Action act = () => Document.Create(_docId, _userId, _fileName, contentType);

            // assert
            var exception = Assert.Throws<DomainStateException>(act);
            exception.Message.Should().Be("ContentType must be defined");
        }


        [Fact]
        public void Document_WhenAllInputsAreCorrect_ShouldReturnObject()
        {
            // act
            var document = Document.Create(_docId, _userId, _fileName, _contentType);

            // assert
            document.Should().NotBeNull();
        }

        [Fact]
        public void Document_WhenCallIncreasDownloadCountMethod_ShouldPlus1DownloadCountAmount()
        {
            // arrange
            var document = Document.Create(_docId, _userId, _fileName, _contentType);
            var expectedDownloadCount = document.DownloadCount + 1;

            // act
            document.IncreasDownloadCount();

            // assert
            expectedDownloadCount.Should().Be(document.DownloadCount);
        }


    }
}