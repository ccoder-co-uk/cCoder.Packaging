// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using System.Linq.Expressions;
using cCoder.Packaging.Services.Processings;
using FluentAssertions;
using Moq;
using Xunit;
using DataPackage = cCoder.Data.Models.Packaging.Package;


namespace cCoder.Packaging.Tests.Aggregations;

public partial class PackageAggregationServiceTests
{
    [Theory]
    [InlineData(1)]
    public async Task ShouldRaiseOnlyPackageImportEventAsyncWhenImportPackageAsync(
        int? appId)
    {
        // Given
        Package package = new() { Name = "Roles", Items = [] };

        Expression<Func<IPackageEventProcessingService, ValueTask>> expectedCall =
            service => service.RaisePackageImportEventAsync(
                appId: appId,
                package: package);

        var eventSetup = packageEventProcessingServiceMock.Setup(expression: expectedCall);
        eventSetup.Returns(value: ValueTask.CompletedTask);

        // When
        await aggregationService.ImportPackageAsync(
            appId: appId,
            package: package);

        // Then
        packageEventProcessingServiceMock.Verify(
            expression: expectedCall,
            times: Times.Once);

        packageEventProcessingServiceMock.VerifyNoOtherCalls();

        packageProcessingServiceMock.VerifyNoOtherCalls();
        packageItemProcessingServiceMock.VerifyNoOtherCalls();
        packageExportProcessingServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldPersistPackageAndRaiseImportEventWhenCommonCacheImportAsync()
    {
        // Given
        Package package = new() { Name = "CommonCache", Items = [] };

        packageProcessingServiceMock.Setup(expression: service =>
                service.AddPackageAsync(newPackage: package))
            .Returns(value: ValueTask.FromResult(result: package));

        packageEventProcessingServiceMock.Setup(expression: service =>
                service.RaisePackageImportEventAsync(
                    appId: null,
                    package: package))
            .Returns(value: ValueTask.CompletedTask);

        // When
        await aggregationService.ImportPackageAsync(
            appId: null,
            package: package);

        // Then
        packageProcessingServiceMock.Verify(expression: service =>
                service.AddPackageAsync(newPackage: package),
            times: Times.Once);

        packageProcessingServiceMock.VerifyNoOtherCalls();
        packageItemProcessingServiceMock.VerifyNoOtherCalls();

        packageEventProcessingServiceMock.Verify(expression: service =>
                service.RaisePackageImportEventAsync(
                    appId: null,
                    package: package),
            times: Times.Once);

        packageEventProcessingServiceMock.VerifyNoOtherCalls();
        packageExportProcessingServiceMock.VerifyNoOtherCalls();

        package.Description
            .Should()
            .BeEmpty();

        package.Category
            .Should()
            .BeEmpty();

        package.SourceApi
            .Should()
            .BeEmpty();
    }
}