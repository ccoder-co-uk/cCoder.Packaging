// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;
using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class BaselineTests
{
    [Fact]
    public async Task Get_GivenBaselineEndpoint_ShouldReturnPackagesArray()
    {
        // Given
        const JsonValueKind expectedValueKind = JsonValueKind.Array;

        // When
        JsonElement baseline = await GetBaselineAsync();

        // Then
        baseline.ValueKind.Should()
            .Be(expected: expectedValueKind);
    }
}