using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class ToolsTests
{
    [Fact]
    public async Task ShouldServeStandardPackagingGridScript()
    {
        HttpResponseMessage response = await client.GetAsync("/tools/packaging.js");

        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("packaging-expand-toggle");
        content.Should().Contain("packaging-tabs");
    }
}
