// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Net;
using System.Net.Http.Json;
using cCoder.Data.Models.Packaging;
using FluentAssertions;
using Packaging.Web.AcceptanceTests.Infrastructure;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

[Collection(WebAcceptanceCollection.Name)]
public sealed partial class PackageImportTests(WebAcceptanceFixture fixture)
{
    [Fact]
    public async Task ShouldAcceptAppPackageImportFromPackagingEndpoint()
    {
        // Given
        Package package = new()
        {
            Name = "Acceptance",
            Items =
            [
                new PackageItem
                {
                    Type = "ContentManagement/Component",
                    Data = "[]",
                },
            ],
        };

        // When
        using HttpResponseMessage response = await fixture.Client.PostAsJsonAsync(
            requestUri: "/Api/Packaging/Package/Import?appId=1",
            value: package);

        // Then
        response.StatusCode.Should()
            .Be(expected: HttpStatusCode.Accepted);
    }

    [Fact]
    public async Task ShouldAcceptCommonCachePackageImportWithoutAppId()
    {
        // Given
        Package package = new()
        {
            Name = "CommonCache",
            Items =
            [
                new PackageItem
                {
                    Type = "ContentManagement/CommonObject",
                    Data = "[]",
                },
            ],
        };

        // When
        using HttpResponseMessage response = await fixture.Client.PostAsJsonAsync(
            requestUri: "/Api/Packaging/Package/Import",
            value: package);

        // Then
        response.StatusCode.Should()
            .Be(expected: HttpStatusCode.Accepted);
    }
}