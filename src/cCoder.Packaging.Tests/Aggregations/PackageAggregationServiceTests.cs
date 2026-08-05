// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Services.Aggregations;
using cCoder.Packaging.Services.Processings;
using Moq;


namespace cCoder.Packaging.Tests.Aggregations;

public partial class PackageAggregationServiceTests
{
    private readonly Mock<IPackageExportProcessingService> packageExportProcessingServiceMock;
    private readonly Mock<IPackageProcessingService> packageProcessingServiceMock;
    private readonly Mock<IPackageItemProcessingService> packageItemProcessingServiceMock;
    private readonly Mock<IPackageEventProcessingService> packageEventProcessingServiceMock;
    private readonly PackageAggregationService aggregationService;

    public PackageAggregationServiceTests()
    {
        packageExportProcessingServiceMock =
            new Mock<IPackageExportProcessingService>(behavior:MockBehavior.Strict);

        packageProcessingServiceMock =
            new Mock<IPackageProcessingService>(behavior:MockBehavior.Strict);

        packageItemProcessingServiceMock =
            new Mock<IPackageItemProcessingService>(behavior:MockBehavior.Strict);

        packageEventProcessingServiceMock = new Mock<IPackageEventProcessingService>(
            behavior:MockBehavior.Loose);

        aggregationService = new PackageAggregationService(
            packageProcessingService:packageProcessingServiceMock.Object,
            packageItemProcessingService:packageItemProcessingServiceMock.Object,
            packageEventProcessingService:packageEventProcessingServiceMock.Object,
            packageExportProcessingService:packageExportProcessingServiceMock.Object);
    }
}