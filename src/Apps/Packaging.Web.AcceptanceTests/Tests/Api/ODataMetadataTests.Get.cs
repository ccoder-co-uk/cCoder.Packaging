using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class ODataMetadataTests
{
    [Fact]
    public async Task ShouldExposePackagingMetadata()
    {
        // Given
        HttpResponseMessage response = await client.GetAsync(requestUri:"/Api/Packaging/$metadata");

        response.EnsureSuccessStatusCode();
        // When
        string content = await response.Content.ReadAsStringAsync();

        // Then
        content.Should()
            .Contain(expected:"Package");

        content.Should()
            .Contain(expected:"PackageItem");
    }
}