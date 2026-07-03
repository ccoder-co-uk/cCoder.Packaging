using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class SwaggerTests
{
    [Fact]
    public async Task ShouldExposePackagingApiDocument()
    {
        HttpResponseMessage response = await client.GetAsync("/swagger/Packaging/swagger.json");

        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Package");
        content.Should().Contain("PackageItem");
    }
}
