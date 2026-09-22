// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers.Loggings;
using cCoder.Packaging.Exposures.Controllers;
using cCoder.Packaging.Services.Aggregations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace cCoder.Packaging.Tests.Exposures.Controllers;

public sealed partial class PackageControllerTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(null)]
    public async Task ShouldAcceptPackageImportAndDelegateExactlyOnceAsync(
        int? appId)
    {
        // Given
        Package package = new() { Name = "Acceptance", Items = [] };

        Mock<IPackageAggregationService> packageAggregationService =
            new(MockBehavior.Strict);

        Mock<ILoggingBroker> loggingBroker = new(MockBehavior.Strict);

        packageAggregationService.Setup(expression: service => service.ImportPackageAsync(
                appId: appId,
                package: package))
            .Returns(value: ValueTask.CompletedTask);

        PackageController controller = new(
            packageAggregationService: packageAggregationService.Object,
            loggingBroker: loggingBroker.Object);

        // When
        IActionResult result = await controller.PostImport(
            newPackage: package,
            appId: appId);

        // Then
        Assert.IsType<AcceptedResult>(@object: result);

        packageAggregationService.Verify(
            expression: service => service.ImportPackageAsync(
                appId: appId,
                package: package),
            times: Times.Once);

        packageAggregationService.VerifyNoOtherCalls();
        loggingBroker.VerifyNoOtherCalls();
    }
}