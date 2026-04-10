using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using Moq;
using Xunit;
using DataPackage = cCoder.Data.Models.Packaging.Package;


namespace cCoder.Packaging.Tests.Orchestrations;

public partial class PackageOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldRaisePackageImportEventAsyncWhenImportPackageAsync()
    {
        Package package = new("Roles") { Items = [] };

        packageEventProcessingServiceMock
            .Setup(x => x.RaisePackageImportEventAsync(
                1,
                It.Is<DataPackage>(p => p.Name == package.Name && p.Items != null && !p.Items.Any())
            ))
            .Returns(ValueTask.CompletedTask);

        await orchestrationService.ImportPackageAsync(1, package);

        packageEventProcessingServiceMock.Verify(
            x => x.RaisePackageImportEventAsync(
                1,
                It.Is<DataPackage>(p => p.Name == package.Name && p.Items != null && !p.Items.Any())
            ),
            Times.Once
        );
        packageProcessingServiceMock.VerifyNoOtherCalls();
        appDomainProviderMock.VerifyNoOtherCalls();
    }
}

