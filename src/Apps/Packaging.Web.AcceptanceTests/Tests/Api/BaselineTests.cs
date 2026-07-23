// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Net;
using System.Text.Json;
using FluentAssertions;
using Packaging.Web.AcceptanceTests.Infrastructure;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

[Collection(WebAcceptanceCollection.Name)]
public sealed partial class BaselineTests(WebAcceptanceFixture fixture)
{
    private readonly HttpClient client = fixture.Client;

    private async Task<JsonElement> GetBaselineAsync()
    {
        // Given
        const string requestUri = "/Api/Packaging/Baseline";

        // When
        using HttpResponseMessage response = await client.GetAsync(requestUri: requestUri);
        string content = await response.Content.ReadAsStringAsync();

        // Then
        response.StatusCode.Should()
            .Be(expected: HttpStatusCode.OK, because: content);

        return JsonDocument.Parse(json: content)
                   .RootElement.Clone();
    }
}