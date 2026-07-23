using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class ToolsTests
{
    [Fact]
    public async Task ShouldServeToolsUi()
    {
        // Given
        HttpResponseMessage response = await client.GetAsync(requestUri:"/tools/index.html");

        response.EnsureSuccessStatusCode();
        // When
        string content = await response.Content.ReadAsStringAsync();

        // Then
        content.Should()
            .Contain(expected:"Packaging");

        content.Should()
            .Contain(expected:"packaging-section-tabs");

        content.Should()
            .Contain(expected:"/tools/packaging.js");
    }
}