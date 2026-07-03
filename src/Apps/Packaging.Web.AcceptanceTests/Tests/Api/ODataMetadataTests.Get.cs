using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class ODataMetadataTests
{
    [Fact]
    public async Task ShouldExposePackagingMetadata()
    {
        HttpResponseMessage response = await client.GetAsync("/Api/Packaging/$metadata");

        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Package");
        content.Should().Contain("PackageItem");
    }
}
