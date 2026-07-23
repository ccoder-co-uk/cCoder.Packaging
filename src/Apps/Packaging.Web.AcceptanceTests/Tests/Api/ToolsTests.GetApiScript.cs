using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class ToolsTests
{
    [Fact]
    public async Task ShouldPostSecurityAuthShapeForLogin()
    {
        // Given
        HttpResponseMessage response = await client.GetAsync(requestUri:"/tools/api.js");

        response.EnsureSuccessStatusCode();
        // When
        string content = await response.Content.ReadAsStringAsync();

        // Then
        content.Should()
            .Contain(expected:"user: username");

        content.Should()
            .Contain(expected:"pass: password");

        content.Should()
            .NotContain(unexpected:"userName");
    }
}