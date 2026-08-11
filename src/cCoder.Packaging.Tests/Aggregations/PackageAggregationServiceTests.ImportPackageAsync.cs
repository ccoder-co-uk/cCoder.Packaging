// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using System.Linq.Expressions;
using cCoder.Packaging.Services.Processings;
using Moq;
using Xunit;
using DataPackage = cCoder.Data.Models.Packaging.Package;


namespace cCoder.Packaging.Tests.Aggregations;

public partial class PackageAggregationServiceTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(null)]
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
}