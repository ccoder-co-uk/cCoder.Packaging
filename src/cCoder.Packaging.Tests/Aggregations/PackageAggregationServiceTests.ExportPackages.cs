// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using FluentAssertions;
using Moq;
using Xunit;
using DataPackage = cCoder.Data.Models.Packaging.Package;


namespace cCoder.Packaging.Tests.Aggregations;

public partial class PackageAggregationServiceTests
{
    [Fact]
    public async Task ShouldDelegateAppPackageExportToCompositionRootAsync()
    {
        // Given
        const int appId = 1;
        const string sourceApi = "https://app.local:443/Api/";

        packageExportProcessingServiceMock
            .Setup(expression: service => service.GetPackageSourceApi(appId: appId))
            .Returns(value: sourceApi);

        DataPackage[] expectedPackages =
        [
            new DataPackage
            {
                Name = "AppConfiguration",
                Items = [],
            },
        ];

        packageExportProcessingServiceMock
            .Setup(expression: service => service.ExportPackagesAsync(
                appId: appId,
                packageNames: It.Is<string[]>(match: value => value.Length == 0),
                sourceApi: sourceApi))
            .ReturnsAsync(value: expectedPackages);

        // When
        Package[] result = await aggregationService.ExportPackagesAsync(
            appId: appId,
            packageNames: []);

        // Then
        result.Should()
            .BeEquivalentTo(expectation: expectedPackages);

        packageExportProcessingServiceMock.Verify(
            expression:service => service.GetPackageSourceApi(appId:appId),
            times:Times.Once);

        packageExportProcessingServiceMock.Verify(
            expression: service => service.ExportPackagesAsync(
                appId: appId,
                packageNames: It.Is<string[]>(match: value => value.Length == 0),
                sourceApi: sourceApi),
            times: Times.Once);

        packageExportProcessingServiceMock.VerifyNoOtherCalls();
        packageProcessingServiceMock.VerifyNoOtherCalls();
    }
}