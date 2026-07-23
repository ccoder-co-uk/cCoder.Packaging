using cCoder.Packaging.Services.Aggregations;
using cCoder.Packaging.Services.Processings;
using Moq;


namespace cCoder.Packaging.Tests.Orchestrations;

public partial class PackageOrchestrationServiceTests
{
    private readonly Mock<IPackageExportProcessingService> packageExportProcessingServiceMock;
    private readonly Mock<IPackageProcessingService> packageProcessingServiceMock;
    private readonly Mock<IPackageItemProcessingService> packageItemProcessingServiceMock;
    private readonly Mock<IPackageEventProcessingService> packageEventProcessingServiceMock;
    private readonly PackageAggregationService orchestrationService;

    public PackageOrchestrationServiceTests()
    {
        packageExportProcessingServiceMock =
            new Mock<IPackageExportProcessingService>(MockBehavior.Strict);
        packageProcessingServiceMock = new Mock<IPackageProcessingService>(MockBehavior.Strict);
        packageItemProcessingServiceMock =
            new Mock<IPackageItemProcessingService>(MockBehavior.Strict);

        packageEventProcessingServiceMock = new Mock<IPackageEventProcessingService>(
            MockBehavior.Loose
        );

        orchestrationService = new PackageAggregationService(
            packageProcessingServiceMock.Object,
            packageItemProcessingServiceMock.Object,
            packageEventProcessingServiceMock.Object,
            packageExportProcessingServiceMock.Object
        );
    }
}