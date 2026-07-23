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
        // Given
        Package package = new("Roles") { Items = [] };

        packageEventProcessingServiceMock
            .Setup(expression: x => x.RaisePackageImportEventAsync(
appId: 1,
package: It.Is<DataPackage>(match: p => p.Name == package.Name && p.Items != null && !p.Items.Any())
            ))
            .Returns(value: ValueTask.CompletedTask);

        // When
        await orchestrationService.ImportPackageAsync(appId: 1, package: package);

        // Then
        packageEventProcessingServiceMock.Verify(
expression: x => x.RaisePackageImportEventAsync(
appId: 1,
package: It.Is<DataPackage>(match: p => p.Name == package.Name && p.Items != null && !p.Items.Any())
            ),
times: Times.Once
        );

        packageProcessingServiceMock.VerifyNoOtherCalls();
        appDomainProviderMock.VerifyNoOtherCalls();
    }
}