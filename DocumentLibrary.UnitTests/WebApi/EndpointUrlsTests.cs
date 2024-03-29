﻿using DocumentLibrary.Infrastructure.Api;
using FluentAssertions;

namespace DocumentLibrary.UnitTests.WebApi
{
    public class EndpointUrlsTests
    {
        EndpointUris sut;
        string baseUrl = "http://localhost";

        public EndpointUrlsTests()
        {
            sut = new EndpointUris(baseUrl);
        }

        [Fact]
        public void GetDocumentByTempLink_ValidateUrl()
        {
            var encryptedText = "aaaa";
            var expectedUrl = $"{baseUrl}/api/documents/temp/{encryptedText}";

            // act
            var url = sut.GetDocumentByTempLink(encryptedText);

            // assert
            url.ToString().Should().Be(expectedUrl);
        }

        [Fact]
        public void GetDocumentUrl_ValidateUrl()
        {
            var docId = Guid.NewGuid();
            var expectedUrl = $"{baseUrl}/api/documents/{docId}";

            // act
            var url = sut.GetDocumentUrl(docId);

            // assert
            url.ToString().Should().Be(expectedUrl);
        }

    }
}
