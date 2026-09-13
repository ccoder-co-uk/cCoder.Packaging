// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Exposures;
using cCoder.Packaging.Exposures.Controllers;
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
        Mock<IPackageManager> packageManager = new(MockBehavior.Strict);

        packageManager.Setup(expression: manager => manager.ImportPackageAsync(
                appId: appId,
                package: package))
            .Returns(value: ValueTask.CompletedTask);

        PackageController controller = new(
            packageManager: packageManager.Object);

        // When
        IActionResult result = await controller.PostImport(
            newPackage: package,
            appId: appId);

        // Then
        Assert.IsType<AcceptedResult>(@object: result);

        packageManager.Verify(
            expression: manager => manager.ImportPackageAsync(
                appId: appId,
                package: package),
            times: Times.Once);

        packageManager.VerifyNoOtherCalls();
    }
}