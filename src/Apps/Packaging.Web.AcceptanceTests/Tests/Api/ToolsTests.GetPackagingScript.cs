// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using FluentAssertions;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

public sealed partial class ToolsTests
{
    [Fact]
    public async Task ShouldServeStandardPackagingGridScript()
    {
        // Given
        HttpResponseMessage response = await client.GetAsync(requestUri: "/tools/packaging.js");

        response.EnsureSuccessStatusCode();
        // When
        string content = await response.Content.ReadAsStringAsync();

        // Then
        content.Should()
            .Contain(expected: "packaging-expand-toggle");

        content.Should()
            .Contain(expected: "packaging-tabs");
    }
}