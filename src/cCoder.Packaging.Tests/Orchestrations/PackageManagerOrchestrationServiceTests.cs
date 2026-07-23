using cCoder.Packaging.Brokers;
using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services;
using cCoder.Packaging.Services.Orchestrations;
using FizzWare.NBuilder;
using Microsoft.Extensions.Logging;
using Moq;


namespace cCoder.Packaging.Tests.Orchestrations;

public partial class PackageManagerOrchestrationServiceTests
{
    private readonly Mock<IAuthorizationBroker> authorizationBrokerMock = new();
    private readonly Mock<IAppSecurityPackageService> appSecurityPackageServiceMock = new();
    private readonly Mock<ISchedulingPackageService> schedulingPackageServiceMock = new();
    private readonly Mock<IWorkflowPackageService> workflowPackageServiceMock = new();
    private readonly Mock<IDocumentManagementPackageService> documentManagementPackageServiceMock = new();
    private readonly Mock<IContentManagementPackageService> contentManagementPackageServiceMock = new();
    private readonly PackageManagerOrchestrationService packageManagerOrchestrationService;

    public PackageManagerOrchestrationServiceTests()
    {
        packageManagerOrchestrationService = new PackageManagerOrchestrationService(
            Mock.Of<ILogger<PackageManagerOrchestrationService>>(),
            authorizationBrokerMock.Object,
            appSecurityPackageServiceMock.Object,
            schedulingPackageServiceMock.Object,
            workflowPackageServiceMock.Object,
            documentManagementPackageServiceMock.Object,
            contentManagementPackageServiceMock.Object
        );
    }

    private static Package CreateRandomPackage() =>
        Builder<Package>
            .CreateNew()
            .With(func: x => x.Id = Guid.NewGuid())
            .With(func: x => x.Name = $"Package-{Guid.NewGuid():N}")
            .With(func: x => x.Items = [])
            .Build();
}