using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Infrastructure.TempLink;
using FluentAssertions;
using Moq;

namespace DocumentLibrary.UnitTests.Infrastructure
{
    public class TempCodeGeneratorTests
    {
        TempCodeGenerator sut;
        Mock<IEncyptionService> encyptionServiceMock = new();
        Mock<IDateProvider> dateProvider = new();
        Guid docId = Guid.NewGuid();

        public TempCodeGeneratorTests()
        {
            dateProvider.Setup(x => x.UtcNow)
                .Returns(new DateTime(2020, 1, 1, 15, 0, 0));

            sut = new TempCodeGenerator(
                encyptionServiceMock.Object,
                dateProvider.Object
                );
        }

        [Fact]
        public void GenerateTempCode_ShouldGenerateTempCodeProperly()
        {
            SetupEncryptReturnValue("aaaaa");
            var expectedTempCode = "YWFhYWE=";

            // act
            var tempCode = sut.GenerateTempCode(docId, TimeSpan.FromMinutes(60));

            tempCode.Should().Be(expectedTempCode);
        }

        [Fact]
        public void ValidateTempCode_WhenCodeTextIsWrong_ShouldReturnIsValidFalse()
        {
            var enryptedTempCode = "YWFhYWE=";
            SetupDecryptReturnValue("wrong data");

            // act
            var tempCode = sut.ValidateTempCode(enryptedTempCode);

            tempCode.Should().NotBeNull();
            tempCode.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ValidateTempCode_WhenCodeIsExpired_ShouldReturnIsValidFalse()
        {
            var enryptedTempCode = "YWFhYWE=";
            SetupDecryptReturnValue("{id: '8f054a8f-d3d6-4bb6-906f-a1c31269eee2', expiration: '2020-01-01 14:30:00'}");

            // act
            var tempCode = sut.ValidateTempCode(enryptedTempCode);

            tempCode.Should().NotBeNull();
            tempCode.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ValidateTempCode_WhenCodeIsNotExpired_ShouldReturnIsValidTrue()
        {
            var enryptedTempCode = "YWFhYWE=";
            var id = Guid.Parse("8f054a8f-d3d6-4bb6-906f-a1c31269eee2");
            SetupDecryptReturnValue("{id: '" + id + "', expiration: '2020-01-01 15:30:00'}");

            // act
            var tempCode = sut.ValidateTempCode(enryptedTempCode);

            tempCode.Should().NotBeNull();
            tempCode.IsValid.Should().BeTrue();
            tempCode.DocumentId.Should().Be(id);
        }

        public void SetupEncryptReturnValue(string code)
        {
            encyptionServiceMock.Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(code);
        }

        public void SetupDecryptReturnValue(string value)
        {
            encyptionServiceMock.Setup(x => x.Decrypt(It.IsAny<string>()))
                .Returns(value);
        }

    }
}
