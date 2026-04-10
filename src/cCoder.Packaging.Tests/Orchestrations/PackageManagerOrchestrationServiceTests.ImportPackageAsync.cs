using System.Security;
using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using cCoder.Data.Models.Security;
using Moq;
using Xunit;


namespace cCoder.Packaging.Tests.Orchestrations;

public partial class PackageManagerOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldDelegateToSchedulingPackageServiceWhenImportPackageAsync()
    {
        Package package = CreateRandomPackage();
        package.Items = [new PackageItem { Type = "Core/Calendar", Data = "[]" }];

        authorizationBrokerMock.Setup(x => x.IsAdminOfApp(1)).Returns(true);
        schedulingPackageServiceMock
            .Setup(x => x.ImportPackageAsync(1, It.IsAny<Package>()))
            .Returns(ValueTask.CompletedTask);

        await packageManagerOrchestrationService.ImportPackageAsync(1, package);

        authorizationBrokerMock.Verify(x => x.IsAdminOfApp(1), Times.Once);
        schedulingPackageServiceMock.Verify(x => x.ImportPackageAsync(1, It.IsAny<Package>()), Times.Once);
        workflowPackageServiceMock.VerifyNoOtherCalls();
        documentManagementPackageServiceMock.VerifyNoOtherCalls();
        contentManagementPackageServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldDelegateToWorkflowPackageServiceWhenImportPackageAsync()
    {
        Package package = CreateRandomPackage();
        package.Items = [new PackageItem { Type = "Core/FlowDefinition", Data = "[]" }];

        authorizationBrokerMock.Setup(x => x.IsAdminOfApp(1)).Returns(true);
        workflowPackageServiceMock
            .Setup(x => x.ImportPackageAsync(1, It.IsAny<Package>()))
            .Returns(ValueTask.CompletedTask);

        await packageManagerOrchestrationService.ImportPackageAsync(1, package);

        workflowPackageServiceMock.Verify(x => x.ImportPackageAsync(1, It.IsAny<Package>()), Times.Once);
    }

    [Fact]
    public async Task ShouldDelegateToDocumentManagementPackageServiceWhenImportPackageAsync()
    {
        Package package = CreateRandomPackage();
        package.Items = [new PackageItem { Type = "Core/FolderRole", Data = "[]" }];

        authorizationBrokerMock.Setup(x => x.IsAdminOfApp(1)).Returns(true);
        documentManagementPackageServiceMock
            .Setup(x => x.ImportPackageAsync(1, It.IsAny<Package>()))
            .Returns(ValueTask.CompletedTask);

        await packageManagerOrchestrationService.ImportPackageAsync(1, package);

        documentManagementPackageServiceMock.Verify(x => x.ImportPackageAsync(1, It.IsAny<Package>()), Times.Once);
    }

    [Fact]
    public async Task ShouldDelegateToContentManagementPackageServiceWhenImportPackageAsync()
    {
        Package package = CreateRandomPackage();
        package.Items = [new PackageItem { Type = "Core/Component", Data = "[]" }];

        authorizationBrokerMock.Setup(x => x.IsAdminOfApp(1)).Returns(true);
        contentManagementPackageServiceMock
            .Setup(x => x.ImportPackageAsync(1, It.IsAny<Package>()))
            .Returns(ValueTask.CompletedTask);

        await packageManagerOrchestrationService.ImportPackageAsync(1, package);

        contentManagementPackageServiceMock.Verify(x => x.ImportPackageAsync(1, It.IsAny<Package>()), Times.Once);
    }

    [Fact]
    public async Task ShouldCompleteWithoutAggregationCallsWhenPackageHasNoItemsForImportPackageAsync()
    {
        Package package = CreateRandomPackage();
        package.Items = [];

        await packageManagerOrchestrationService.ImportPackageAsync(1, package);

        authorizationBrokerMock.VerifyNoOtherCalls();
        schedulingPackageServiceMock.VerifyNoOtherCalls();
        workflowPackageServiceMock.VerifyNoOtherCalls();
        documentManagementPackageServiceMock.VerifyNoOtherCalls();
        contentManagementPackageServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowSecurityExceptionWhenUserIsNotAdminForImportPackageAsync()
    {
        Package package = CreateRandomPackage();
        package.Items = [new PackageItem { Type = "Core/Component", Data = "[]" }];

        authorizationBrokerMock.Setup(x => x.IsAdminOfApp(1)).Returns(false);

        await Assert.ThrowsAsync<SecurityException>(() =>
            packageManagerOrchestrationService.ImportPackageAsync(1, package).AsTask()
        );

        authorizationBrokerMock.Verify(x => x.IsAdminOfApp(1), Times.Once);
        schedulingPackageServiceMock.VerifyNoOtherCalls();
        workflowPackageServiceMock.VerifyNoOtherCalls();
        documentManagementPackageServiceMock.VerifyNoOtherCalls();
        contentManagementPackageServiceMock.VerifyNoOtherCalls();
    }
}


