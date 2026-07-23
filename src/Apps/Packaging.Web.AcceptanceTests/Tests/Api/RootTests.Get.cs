using System.Net;
using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class RootTests
{
    [Fact]
    public async Task ShouldRedirectToToolsUi()
    {
        // Given
        HttpResponseMessage response = await client.GetAsync(requestUri: "/");

        // When
        response.StatusCode.Should()
            .Be(expected: HttpStatusCode.Redirect);

        // Then
        response.Headers.Location?.OriginalString.Should()
                                      .Be(expected: "/tools/index.html");
    }
}