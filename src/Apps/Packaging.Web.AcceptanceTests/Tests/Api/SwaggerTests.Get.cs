using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class SwaggerTests
{
    [Fact]
    public async Task ShouldExposePackagingApiDocument()
    {
        // Given
        HttpResponseMessage response = await client.GetAsync(requestUri:"/swagger/Packaging/swagger.json");

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