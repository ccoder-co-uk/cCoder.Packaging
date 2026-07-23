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
        // Given
        const int appId = 1;
        const string domain = "app.local";

        appDomainProviderMock.Setup(expression:x => x.GetDomain(appId:appId))
            .Returns(value:domain);

        packageProcessingServiceMock
            .Setup(expression:x => x.ExportPackages(appId:appId, packageNames:It.IsAny<string[]>()))
            .Returns(valueFunction:(int _, string[] packageNames) =>
                [.. packageNames.Select(selector:packageName => new DataPackage(packageName) { Items = [] })]);

        // When
        Package[] result = orchestrationService.Export(appId:appId, packageNames:[])
                               .ToArray();

        // Then
        result.Should()
            .HaveCount(expected:12);

        packageProcessingServiceMock.Verify(expression:x => x.ExportPackages(appId:appId, packageNames:It.IsAny<string[]>()), times:Times.Once);
        appDomainProviderMock.Verify(expression:x => x.GetDomain(appId:appId), times:Times.Once);
        appDomainProviderMock.VerifyNoOtherCalls();
        packageProcessingServiceMock.VerifyNoOtherCalls();
    }

}