using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class ToolsTests
{
    [Fact]
    public async Task ShouldServeToolsUi()
    {
        HttpResponseMessage response = await client.GetAsync("/tools/index.html");

        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Packaging");
        content.Should().Contain("packaging-section-tabs");
        content.Should().Contain("/tools/packaging.js");
    }
}
