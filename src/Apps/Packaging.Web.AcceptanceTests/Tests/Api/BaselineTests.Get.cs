using System.Text.Json;
using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class BaselineTests
{
    [Fact]
    public async Task Get_GivenBaselineEndpoint_ShouldReturnPackagesArray()
    {
        // Given
        JsonElement baseline = await GetBaselineAsync();

        // Then
        // When
        baseline.ValueKind.Should()
            .Be(expected: JsonValueKind.Array);
    }
}