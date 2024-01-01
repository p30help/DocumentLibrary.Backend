using DocumentLibrary.Infrastructure.TempLink;
using FluentAssertions;

namespace DocumentLibrary.UnitTests.Infrastructure
{
    public class EncyptionServiceTests
    {
        EncyptionService sut;
        string ivAsBase64 = "TzJJTfD245EmLZiJpNbGMg==";
        string encryptionKey = "wqerere51tuy5jsdfksju98349348594";


        public EncyptionServiceTests()
        {            
            sut = new EncyptionService(
                encryptionKey,
                ivAsBase64
                );
        }

        [Fact]
        public void Encrypt_ShouldEncryptTextProperly()
        {
            // arrange
            var text = "hello";
            var expectedEncryptedText = "gab+hW3W61QVl8sqNln0vA==";

            // act
            var encryptedText = sut.Encrypt(text);

            // assert
            encryptedText.Should().Be(expectedEncryptedText);
        }

        [Fact]
        public void Decrypt_ShouldDecryptTextProperly()
        {
            // arrange            
            var encryptedText = "gab+hW3W61QVl8sqNln0vA==";
            var expectedDecryptedText = "hello";

            // act
            var decryptedText = sut.Decrypt(encryptedText);

            // assert
            decryptedText.Should().Be(expectedDecryptedText);
        }


    }
}
