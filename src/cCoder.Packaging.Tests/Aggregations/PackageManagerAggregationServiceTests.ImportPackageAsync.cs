// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using cCoder.Data.Models.Security;
using cCoder.Packaging.Models.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Packaging.Tests.Aggregations;

public partial class PackageManagerAggregationServiceTests
{
    [Fact]
    public async Task ShouldDelegateToSchedulingPackageServiceWhenImportPackageAsync()
    {
        // Given
        Package package = CreateRandomPackage();
        package.Items = [new PackageItem { Type = "Core/Calendar", Data = "[]" }];

        authorizationBrokerMock.Setup(expression: x => x.IsAdminOfApp(appId: 1))
            .Returns(value: true);

        schedulingPackageServiceMock
            .Setup(expression: x => x.ImportPackageAsync(appId: 1, package: It.IsAny<Package>()))
            .Returns(value: ValueTask.CompletedTask);

        // When
        await packageManagerAggregationService.ImportPackageAsync(appId: 1, package: package);

        // Then
        authorizationBrokerMock.Verify(expression: x => x.IsAdminOfApp(appId: 1), times: Times.Once);
        schedulingPackageServiceMock.Verify(expression: x => x.ImportPackageAsync(appId: 1, package: It.IsAny<Package>()), times: Times.Once);
        workflowPackageServiceMock.VerifyNoOtherCalls();
        documentManagementPackageServiceMock.VerifyNoOtherCalls();
        contentManagementPackageServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldDelegateToWorkflowPackageServiceWhenImportPackageAsync()
    {
        // Given
        Package package = CreateRandomPackage();
        package.Items = [new PackageItem { Type = "Core/FlowDefinition", Data = "[]" }];

        authorizationBrokerMock.Setup(expression: x => x.IsAdminOfApp(appId: 1))
            .Returns(value: true);

        workflowPackageServiceMock
            .Setup(expression: x => x.ImportPackageAsync(appId: 1, package: It.IsAny<Package>()))
            .Returns(value: ValueTask.CompletedTask);

        // When
        await packageManagerAggregationService.ImportPackageAsync(appId: 1, package: package);

        // Then
        workflowPackageServiceMock.Verify(expression: x => x.ImportPackageAsync(appId: 1, package: It.IsAny<Package>()), times: Times.Once);
    }

    [Fact]
    public async Task ShouldDelegateToDocumentManagementPackageServiceWhenImportPackageAsync()
    {
        // Given
        Package package = CreateRandomPackage();
        package.Items = [new PackageItem { Type = "Core/FolderRole", Data = "[]" }];

        authorizationBrokerMock.Setup(expression: x => x.IsAdminOfApp(appId: 1))
            .Returns(value: true);

        documentManagementPackageServiceMock
            .Setup(expression: x => x.ImportPackageAsync(appId: 1, package: It.IsAny<Package>()))
            .Returns(value: ValueTask.CompletedTask);

        // When
        await packageManagerAggregationService.ImportPackageAsync(appId: 1, package: package);

        // Then
        documentManagementPackageServiceMock.Verify(expression: x => x.ImportPackageAsync(appId: 1, package: It.IsAny<Package>()), times: Times.Once);
    }

    [Fact]
    public async Task ShouldDelegateToContentManagementPackageServiceWhenImportPackageAsync()
    {
        // Given
        Package package = CreateRandomPackage();
        package.Items = [new PackageItem { Type = "Core/Component", Data = "[]" }];

        authorizationBrokerMock.Setup(expression: x => x.IsAdminOfApp(appId: 1))
            .Returns(value: true);

        contentManagementPackageServiceMock
            .Setup(expression: x => x.ImportPackageAsync(appId: 1, package: It.IsAny<Package>()))
            .Returns(value: ValueTask.CompletedTask);

        // When
        await packageManagerAggregationService.ImportPackageAsync(appId: 1, package: package);

        // Then
        contentManagementPackageServiceMock.Verify(expression: x => x.ImportPackageAsync(appId: 1, package: It.IsAny<Package>()), times: Times.Once);
    }

    [Fact]
    public async Task ShouldCompleteWithoutAggregationCallsWhenPackageHasNoItemsForImportPackageAsync()
    {
        // Given
        Package package = CreateRandomPackage();
        package.Items = [];

        // When
        await packageManagerAggregationService.ImportPackageAsync(appId: 1, package: package);

        // Then
        authorizationBrokerMock.VerifyNoOtherCalls();
        schedulingPackageServiceMock.VerifyNoOtherCalls();
        workflowPackageServiceMock.VerifyNoOtherCalls();
        documentManagementPackageServiceMock.VerifyNoOtherCalls();
        contentManagementPackageServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowSecurityExceptionWhenUserIsNotAdminForImportPackageAsync()
    {
        // Given
        Package package = CreateRandomPackage();
        package.Items = [new PackageItem { Type = "Core/Component", Data = "[]" }];

        // When
        authorizationBrokerMock.Setup(expression: x => x.IsAdminOfApp(appId: 1))
            .Returns(value: false);

        // Then
        PackagingOrchestrationServiceException exception =
            await Assert.ThrowsAsync<PackagingOrchestrationServiceException>(testCode: () =>
            packageManagerAggregationService.ImportPackageAsync(appId: 1, package: package)
                .AsTask()
        );

        exception.InnerException.Should()
            .BeOfType<PackagingServiceException>()
            .Which.InnerException.Should()
            .BeOfType<SecurityException>();

        authorizationBrokerMock.Verify(expression: x => x.IsAdminOfApp(appId: 1), times: Times.Once);
        schedulingPackageServiceMock.VerifyNoOtherCalls();
        workflowPackageServiceMock.VerifyNoOtherCalls();
        documentManagementPackageServiceMock.VerifyNoOtherCalls();
        contentManagementPackageServiceMock.VerifyNoOtherCalls();
    }
}