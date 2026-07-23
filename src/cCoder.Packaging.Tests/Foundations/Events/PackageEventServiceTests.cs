using cCoder.Data;
using cCoder.Packaging.Brokers.Events;
using Moq;


namespace cCoder.Packaging.Tests.Foundations.Events;

public partial class PackageEventServiceTests
{
    private readonly Mock<IPackageEventBroker> packageEventBrokerMock;
    private readonly Mock<ICoreAuthInfo> authInfoMock;
    private readonly cCoder.Packaging.Services.Foundations.Events.PackageEventService service;
    private const string CurrentUserId = "test-user";

    public PackageEventServiceTests()
    {
        packageEventBrokerMock = new Mock<IPackageEventBroker>(MockBehavior.Strict);
        authInfoMock = new Mock<ICoreAuthInfo>(MockBehavior.Strict);

        authInfoMock.SetupGet(expression:x => x.SSOUserId)
            .Returns(value:CurrentUserId);

        service = new cCoder.Packaging.Services.Foundations.Events.PackageEventService(
            packageEventBrokerMock.Object,
            authInfoMock.Object
        );
    }
}