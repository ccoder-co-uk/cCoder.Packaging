using System.Security;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Storages;
using cCoder.Packaging.Services.Foundations.Storages;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Packaging.Tests.Foundations.Storages;

public class PackageItemServiceTests
{
    private readonly Mock<IPackageItemBroker> packageItemBrokerMock;
    private readonly Mock<IAuthorizationBroker> authorizationBrokerMock;
    private readonly PackageItemService service;

    public PackageItemServiceTests()
    {
        packageItemBrokerMock = new Mock<IPackageItemBroker>(MockBehavior.Strict);
        authorizationBrokerMock = new Mock<IAuthorizationBroker>(MockBehavior.Strict);
        service = new PackageItemService(
            packageItemBrokerMock.Object,
            authorizationBrokerMock.Object
        );
    }

    [Fact]
    public void ShouldReturnPackageItemFromFilteredSetWhenGet()
    {
        PackageItem expectedItem = CreatePackageItem();

        packageItemBrokerMock
            .Setup(broker => broker.GetAllPackageItems(false))
            .Returns(new[] { expectedItem }.AsQueryable());

        PackageItem actualItem = service.Get(expectedItem.Id);

        actualItem.Should().BeSameAs(expectedItem);
        packageItemBrokerMock.Verify(broker => broker.GetAllPackageItems(false), Times.Once);
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldThrowSecurityExceptionWhenGetFindsPackageItemOnlyInUnrestrictedSet()
    {
        Guid packageItemId = Guid.NewGuid();
        PackageItem unrestrictedItem = CreatePackageItem(packageItemId);

        packageItemBrokerMock
            .Setup(broker => broker.GetAllPackageItems(false))
            .Returns(Array.Empty<PackageItem>().AsQueryable());
        packageItemBrokerMock
            .Setup(broker => broker.GetAllPackageItems(true))
            .Returns(new[] { unrestrictedItem }.AsQueryable());

        Action act = () => service.Get(packageItemId);

        act.Should().Throw<SecurityException>().WithMessage("Access Denied!");
        packageItemBrokerMock.Verify(broker => broker.GetAllPackageItems(false), Times.Once);
        packageItemBrokerMock.Verify(broker => broker.GetAllPackageItems(true), Times.Once);
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldReturnNullWhenGetDoesNotFindPackageItem()
    {
        Guid packageItemId = Guid.NewGuid();

        packageItemBrokerMock
            .Setup(broker => broker.GetAllPackageItems(false))
            .Returns(Array.Empty<PackageItem>().AsQueryable());
        packageItemBrokerMock
            .Setup(broker => broker.GetAllPackageItems(true))
            .Returns(Array.Empty<PackageItem>().AsQueryable());

        PackageItem actualItem = service.Get(packageItemId);

        actualItem.Should().BeNull();
        packageItemBrokerMock.Verify(broker => broker.GetAllPackageItems(false), Times.Once);
        packageItemBrokerMock.Verify(broker => broker.GetAllPackageItems(true), Times.Once);
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldReturnQueryableFromBrokerWhenGetAll()
    {
        IQueryable<PackageItem> expectedItems = new[] { CreatePackageItem(), CreatePackageItem() }.AsQueryable();

        packageItemBrokerMock.Setup(broker => broker.GetAllPackageItems(true)).Returns(expectedItems);

        IQueryable<PackageItem> actualItems = service.GetAll(ignoreFilters: true);

        actualItems.Should().BeSameAs(expectedItems);
        packageItemBrokerMock.Verify(broker => broker.GetAllPackageItems(true), Times.Once);
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldAuthorizeAndAddPackageItemWhenAddAsync()
    {
        PackageItem packageItem = CreatePackageItem();
        PackageItem storedPackageItem = CreatePackageItem(
            packageItem.Id,
            packageItem.PackageId,
            packageItem.Type,
            packageItem.Data);

        packageItemBrokerMock.Setup(broker => broker.GetAppId(packageItem)).Returns(7);
        authorizationBrokerMock.Setup(broker => broker.Authorize(7, "PackageItem_create"));
        packageItemBrokerMock
            .Setup(broker => broker.AddPackageItemAsync(It.Is<PackageItem>(item =>
                item.Id == Guid.Empty
                && item.PackageId == packageItem.PackageId
                && item.Type == packageItem.Type
                && item.Data == packageItem.Data)))
            .ReturnsAsync(storedPackageItem);

        PackageItem actualItem = await service.AddAsync(packageItem);

        actualItem.Should().BeSameAs(packageItem);
        actualItem.Id.Should().Be(storedPackageItem.Id);
        packageItemBrokerMock.VerifyAll();
        authorizationBrokerMock.VerifyAll();
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldAuthorizeAndUpdatePackageItemWhenUpdateAsync()
    {
        PackageItem packageItem = CreatePackageItem();
        PackageItem storedPackageItem = CreatePackageItem(
            packageItem.Id,
            packageItem.PackageId,
            packageItem.Type,
            packageItem.Data);

        packageItemBrokerMock.Setup(broker => broker.GetAppId(packageItem)).Returns(7);
        authorizationBrokerMock.Setup(broker => broker.Authorize(7, "PackageItem_update"));
        packageItemBrokerMock
            .Setup(broker => broker.UpdatePackageItemAsync(It.Is<PackageItem>(item =>
                item.Id == packageItem.Id
                && item.PackageId == packageItem.PackageId
                && item.Type == packageItem.Type
                && item.Data == packageItem.Data)))
            .ReturnsAsync(storedPackageItem);

        PackageItem actualItem = await service.UpdateAsync(packageItem);

        actualItem.Should().BeSameAs(packageItem);
        actualItem.Id.Should().Be(storedPackageItem.Id);
        packageItemBrokerMock.VerifyAll();
        authorizationBrokerMock.VerifyAll();
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldAuthorizeAndDeletePackageItemWhenDeleteAsync()
    {
        PackageItem packageItem = CreatePackageItem();

        packageItemBrokerMock
            .Setup(broker => broker.GetAllPackageItems(false))
            .Returns(new[] { packageItem }.AsQueryable());
        packageItemBrokerMock.Setup(broker => broker.GetAppId(packageItem)).Returns(7);
        authorizationBrokerMock.Setup(broker => broker.Authorize(7, "PackageItem_delete"));
        packageItemBrokerMock.Setup(broker => broker.DeletePackageItemAsync(packageItem)).ReturnsAsync(1);

        await service.DeleteAsync(packageItem.Id);

        packageItemBrokerMock.Verify(broker => broker.GetAllPackageItems(false), Times.Once);
        packageItemBrokerMock.Verify(broker => broker.GetAppId(packageItem), Times.Once);
        authorizationBrokerMock.Verify(broker => broker.Authorize(7, "PackageItem_delete"), Times.Once);
        packageItemBrokerMock.Verify(broker => broker.DeletePackageItemAsync(packageItem), Times.Once);
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    private static PackageItem CreatePackageItem(
        Guid? id = null,
        Guid? packageId = null,
        string type = null,
        string data = null) =>
        new()
        {
            Id = id ?? Guid.NewGuid(),
            PackageId = packageId ?? Guid.NewGuid(),
            Type = type ?? "Core/Page",
            Data = data ?? "{\"name\":\"Home\"}",
        };
}
