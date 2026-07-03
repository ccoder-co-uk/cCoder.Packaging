using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class ToolsTests
{
    [Fact]
    public async Task ShouldPostSecurityAuthShapeForLogin()
    {
        HttpResponseMessage response = await client.GetAsync("/tools/api.js");

        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("user: username");
        content.Should().Contain("pass: password");
        content.Should().NotContain("userName");
    }
}
