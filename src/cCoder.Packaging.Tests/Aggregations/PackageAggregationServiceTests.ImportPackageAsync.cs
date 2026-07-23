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
    [Fact]
    public async Task ShouldRaisePackageImportEventAsyncWhenImportPackageAsync()
    {
        // Given
        Package package = new("Roles") { Items = [] };
        Expression<Func<IPackageEventProcessingService, ValueTask>> expectedCall = service => service.RaisePackageImportEventAsync(appId: 1, package: It.IsAny<DataPackage>());

        var eventSetup = packageEventProcessingServiceMock.Setup(expression: expectedCall);
        eventSetup.Returns(value: ValueTask.CompletedTask);

        // When
        await aggregationService.ImportPackageAsync(appId: 1, package: package);

        // Then
        packageEventProcessingServiceMock.VerifyAll();

        packageProcessingServiceMock.VerifyNoOtherCalls();
        packageExportProcessingServiceMock.VerifyNoOtherCalls();
    }
}