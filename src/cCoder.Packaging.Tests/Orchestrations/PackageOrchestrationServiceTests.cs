using cCoder.Data;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Services.Processings;
using Moq;
using PackageOrchestrationService = cCoder.Packaging.Services.Orchestrations.PackageOrchestrationService;


namespace cCoder.Packaging.Tests.Orchestrations;

public partial class PackageOrchestrationServiceTests
{
    private readonly Mock<IAppDomainProvider> appDomainProviderMock;
    private readonly Mock<IPackageProcessingService> packageProcessingServiceMock;
    private readonly Mock<IPackageItemProcessingService> packageItemProcessingServiceMock;
    private readonly Mock<IPackageEventProcessingService> packageEventProcessingServiceMock;
    private readonly PackageOrchestrationService orchestrationService;

    public PackageOrchestrationServiceTests()
    {
        appDomainProviderMock = new Mock<IAppDomainProvider>(MockBehavior.Strict);
        packageProcessingServiceMock = new Mock<IPackageProcessingService>(MockBehavior.Strict);
        packageItemProcessingServiceMock =
            new Mock<IPackageItemProcessingService>(MockBehavior.Strict);

        packageEventProcessingServiceMock = new Mock<IPackageEventProcessingService>(
            MockBehavior.Loose
        );

        orchestrationService = new PackageOrchestrationService(
            appDomainProviderMock.Object,
            packageProcessingServiceMock.Object,
            packageItemProcessingServiceMock.Object,
            packageEventProcessingServiceMock.Object,
            new Config
            {
                Settings = new Dictionary<string, string> { ["sslPort"] = "443" },
                Services = new Dictionary<string, string>(),
            }
        );
    }
}