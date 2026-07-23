using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class HealthTests
{
    [Fact]
    public async Task ShouldReturnOk()
    {
        // Given
        HttpResponseMessage response = await client.GetAsync(requestUri:"/Health");

        response.EnsureSuccessStatusCode();
        // When
        string content = await response.Content.ReadAsStringAsync();

        // Then
        content.Should()
            .Be(expected:"OK");
    }
}