// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using cCoder.Data;
using cCoder.Data.Models;
using cCoder.Data.Models.Packaging;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Packaging.Web.AcceptanceTests.Infrastructure;

namespace Packaging.Web.AcceptanceTests.Tests.Api;

[Collection(WebAcceptanceCollection.Name)]
public sealed partial class CommonCachePackageTransferTests(WebAcceptanceFixture fixture)
{
    [Fact]
    public async Task Package_WhenImportedWithoutAppId_IsPersistedWithItsItemsAsync()
    {
        // Given
        string marker = $"common-cache-import-{Guid.NewGuid():N}";

        Package package = new()
        {
            Name = marker,
            Description = "Common Cache acceptance package",
            Category = "CommonCache",
            SourceApi = "https://source.acceptance.local",
            Items =
            [
                new PackageItem
                {
                    Type = "ContentManagement/Component",
                    Data = $$"""[{"Name":"{{marker}}","Content":"<div>{{marker}}</div>"}]""",
                },
            ],
        };

        // When
        using HttpResponseMessage response = await fixture.Client.PostAsJsonAsync(
            requestUri: "/Api/Packaging/Package/Import",
            value: package);

        await using AsyncServiceScope scope =
            fixture.Factory.Services.CreateAsyncScope();

        await using CoreDataContext context = scope.ServiceProvider
            .GetRequiredService<ICoreContextFactory>()
            .CreateCoreContext();

        Package storedPackage = await context.Packages
            .IgnoreQueryFilters()
            .Include(navigationPropertyPath: stored => stored.Items)
            .SingleOrDefaultAsync(predicate: stored => stored.Name == marker);

        // Then
        using AssertionScope assertionScope = new();

        response.StatusCode.Should()
            .Be(expected: HttpStatusCode.Accepted);

        storedPackage.Should()
            .NotBeNull();

        storedPackage?.Items.Should()
            .ContainSingle(predicate: item =>
                item.Type == "ContentManagement/Component"
                && item.Data.Contains(value: marker));
    }

    [Fact]
    public async Task CommonObjects_WhenExportedWithoutAppId_ContainOnlyTheLatestVersionAsync()
    {
        // Given
        string marker = $"common-cache-export-{Guid.NewGuid():N}";

        await using AsyncServiceScope scope =
            fixture.Factory.Services.CreateAsyncScope();

        await using CoreDataContext context = scope.ServiceProvider
            .GetRequiredService<ICoreContextFactory>()
            .CreateCoreContext();

        context.CommonObjects.AddRange(entities:
        [
            new CommonObject
            {
                Name = marker,
                Key = marker,
                Type = "ContentManagement/Component",
                Version = 1,
                Json = $$"""{"Name":"{{marker}}","Content":"old"}""",
            },
            new CommonObject
            {
                Name = marker,
                Key = marker,
                Type = "ContentManagement/Component",
                Version = 2,
                Json = $$"""{"Name":"{{marker}}","Content":"latest"}""",
            }
        ]);

        await context.SaveChangesAsync();

        // When
        using HttpResponseMessage response = await fixture.Client.GetAsync(
            requestUri: "/Api/Packaging/Package/Export");

        Package[] packages = response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<Package[]>() ?? []
            : [];

        PackageItem[] matchingItems =
        [
            .. packages
                .SelectMany(selector: exportedPackage => exportedPackage.Items ?? [])
                .Where(predicate: item => item.Data?.Contains(value: marker) == true)
        ];

        // Then
        response.StatusCode.Should()
            .Be(expected: HttpStatusCode.OK);

        matchingItems.Should()
            .ContainSingle();

        using JsonDocument document = JsonDocument.Parse(
            json: matchingItems.Single().Data);

        JsonElement[] matchingObjects =
        [
            .. document.RootElement
                .EnumerateArray()
                .Where(predicate: element =>
                    element.GetProperty(propertyName: "Name")
                        .GetString() == marker)
        ];

        matchingObjects.Should()
            .ContainSingle();

        matchingObjects.Single()
            .GetProperty(propertyName: "Content")
            .GetString()
            .Should()
            .Be(expected: "latest");
    }
}