using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using FluentAssertions;
using Moq;
using Xunit;
using DataPackage = cCoder.Data.Models.Packaging.Package;


namespace cCoder.Packaging.Tests.Orchestrations;

public partial class PackageOrchestrationServiceTests
{
    [Fact]
    public void ShouldUseDefaultPackageListWhenPackagesAreEmpty()
    {
        const int appId = 1;
        const string domain = "app.local";

        appDomainProviderMock.Setup(x => x.GetDomain(appId)).Returns(domain);
        packageProcessingServiceMock
            .Setup(x => x.ExportPackages(appId, It.IsAny<string[]>()))
            .Returns((int _, string[] packageNames) =>
                [.. packageNames.Select(packageName => new DataPackage(packageName) { Items = [] })]);

        Package[] result = orchestrationService.Export(appId, []).ToArray();

        result.Should().HaveCount(12);
        packageProcessingServiceMock.Verify(x => x.ExportPackages(appId, It.IsAny<string[]>()), Times.Once);
        appDomainProviderMock.Verify(x => x.GetDomain(appId), Times.Once);
        appDomainProviderMock.VerifyNoOtherCalls();
        packageProcessingServiceMock.VerifyNoOtherCalls();
    }

}





