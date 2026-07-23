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
        using HttpResponseMessage response = await client.GetAsync(requestUri:"/Api/Packaging/Baseline");
        string content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should()
            .Be(expected:HttpStatusCode.OK, because:content);

        return JsonDocument.Parse(json:content)
                   .RootElement.Clone();
    }
}