using System.Net;
using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class RootTests
{
    [Fact]
    public async Task ShouldRedirectToToolsUi()
    {
        HttpResponseMessage response = await client.GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location?.OriginalString.Should().Be("/tools/index.html");
    }
}
