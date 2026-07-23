// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Events;
using Moq;


namespace cCoder.Packaging.Tests.Foundations.Events;

public partial class PackageEventServiceTests
{
    private readonly Mock<IPackageEventBroker> packageEventBrokerMock;
    private readonly Mock<IAuthInfoBroker> authInfoBrokerMock;
    private readonly cCoder.Packaging.Services.Foundations.Events.PackageEventService service;
    private const string CurrentUserId = "test-user";

    public PackageEventServiceTests()
    {
        packageEventBrokerMock = new Mock<IPackageEventBroker>(MockBehavior.Strict);
        authInfoBrokerMock = new Mock<IAuthInfoBroker>(MockBehavior.Strict);

        authInfoBrokerMock
            .Setup(expression: broker => broker.GetSSOUserId())
            .Returns(value: CurrentUserId);

        service = new cCoder.Packaging.Services.Foundations.Events.PackageEventService(
            packageEventBrokerMock.Object,
            authInfoBrokerMock.Object
        );
    }
}