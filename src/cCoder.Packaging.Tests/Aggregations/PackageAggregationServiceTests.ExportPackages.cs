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
    public void ShouldUseDefaultPackageListWhenPackagesAreEmpty()
    {
        // Given
        const int appId = 1;
        const string sourceApi = "https://app.local:443/Api/";

        packageExportProcessingServiceMock
            .Setup(expression: service => service.GetPackageSourceApi(appId: appId))
            .Returns(value: sourceApi);

        packageProcessingServiceMock
            .Setup(expression: x => x.ExportPackages(appId: appId, packageNames: It.IsAny<string[]>()))
            .Returns(valueFunction: (int _, string[] packageNames) =>
                [.. packageNames.Select(selector: packageName => new DataPackage(packageName) { Items = [] })]);

        // When
        Package[] result = aggregationService.ExportPackages(appId: appId, packageNames: [])
                               .ToArray();

        // Then
        result.Should()
            .HaveCount(expected: 12);

        packageProcessingServiceMock.Verify(expression: x => x.ExportPackages(appId: appId, packageNames: It.IsAny<string[]>()), times: Times.Once);

        packageExportProcessingServiceMock.Verify(
            expression: service => service.GetPackageSourceApi(appId: appId),
            times: Times.Once);

        packageExportProcessingServiceMock.VerifyNoOtherCalls();
        packageProcessingServiceMock.VerifyNoOtherCalls();
    }
}