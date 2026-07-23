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
        // Given
        PackageItem expectedItem = CreatePackageItem();

        packageItemBrokerMock
            .Setup(expression: broker => broker.GetAllPackageItems(ignoreFilters: false))
            .Returns(value: new[] { expectedItem }.AsQueryable());

        // When
        PackageItem actualItem = service.GetPackageItem(packageItemId: expectedItem.Id);

        // Then
        actualItem.Should()
            .BeSameAs(expected: expectedItem);

        packageItemBrokerMock.Verify(expression: broker => broker.GetAllPackageItems(ignoreFilters: false), times: Times.Once);
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldThrowSecurityExceptionWhenGetFindsPackageItemOnlyInUnrestrictedSet()
    {
        // Given
        Guid packageItemId = Guid.NewGuid();
        PackageItem unrestrictedItem = CreatePackageItem(id: packageItemId);

        packageItemBrokerMock
            .Setup(expression: broker => broker.GetAllPackageItems(ignoreFilters: false))
            .Returns(value: Array.Empty<PackageItem>()
                         .AsQueryable());

        packageItemBrokerMock
            .Setup(expression: broker => broker.GetAllPackageItems(ignoreFilters: true))
            .Returns(value: new[] { unrestrictedItem }.AsQueryable());

        // When
        Action act = () => service.GetPackageItem(packageItemId: packageItemId);

        // Then
        act.Should()
            .Throw<SecurityException>()
            .WithMessage(expectedWildcardPattern: "Access Denied!");

        packageItemBrokerMock.Verify(expression: broker => broker.GetAllPackageItems(ignoreFilters: false), times: Times.Once);
        packageItemBrokerMock.Verify(expression: broker => broker.GetAllPackageItems(ignoreFilters: true), times: Times.Once);
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldReturnNullWhenGetDoesNotFindPackageItem()
    {
        // Given
        Guid packageItemId = Guid.NewGuid();

        packageItemBrokerMock
            .Setup(expression: broker => broker.GetAllPackageItems(ignoreFilters: false))
            .Returns(value: Array.Empty<PackageItem>()
                         .AsQueryable());

        packageItemBrokerMock
            .Setup(expression: broker => broker.GetAllPackageItems(ignoreFilters: true))
            .Returns(value: Array.Empty<PackageItem>()
                         .AsQueryable());

        // When
        PackageItem actualItem = service.GetPackageItem(packageItemId: packageItemId);

        // Then
        actualItem.Should()
            .BeNull();

        packageItemBrokerMock.Verify(expression: broker => broker.GetAllPackageItems(ignoreFilters: false), times: Times.Once);
        packageItemBrokerMock.Verify(expression: broker => broker.GetAllPackageItems(ignoreFilters: true), times: Times.Once);
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldReturnQueryableFromBrokerWhenGetAll()
    {
        // Given
        IQueryable<PackageItem> expectedItems = new[] { CreatePackageItem(), CreatePackageItem() }.AsQueryable();

        packageItemBrokerMock.Setup(expression: broker => broker.GetAllPackageItems(ignoreFilters: true))
            .Returns(value: expectedItems);

        // When
        IQueryable<PackageItem> actualItems =
            service.GetAllPackageItems(ignoreFilters: true);

        // Then
        actualItems.Should()
            .BeSameAs(expected: expectedItems);

        packageItemBrokerMock.Verify(expression: broker => broker.GetAllPackageItems(ignoreFilters: true), times: Times.Once);
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldAuthorizeAndAddPackageItemWhenAddAsync()
    {
        // Given
        PackageItem packageItem = CreatePackageItem();

        PackageItem storedPackageItem = CreatePackageItem(
id: packageItem.Id,
packageId: packageItem.PackageId,
type: packageItem.Type,
data: packageItem.Data);

        packageItemBrokerMock.Setup(expression: broker => broker.GetAppId(entity: packageItem))
            .Returns(value: 7);

        authorizationBrokerMock.Setup(expression: broker => broker.Authorize(appId: 7, privilege: "PackageItem_create"));

        packageItemBrokerMock
            .Setup(expression: broker => broker.AddPackageItemAsync(entity: It.Is<PackageItem>(match: item =>
                item.Id == Guid.Empty
                && item.PackageId == packageItem.PackageId
                && item.Type == packageItem.Type
                && item.Data == packageItem.Data)))
            .ReturnsAsync(value: storedPackageItem);

        // When
        PackageItem actualItem =
            await service.AddPackageItemAsync(newPackageItem: packageItem);

        // Then
        actualItem.Should()
            .BeSameAs(expected: packageItem);

        actualItem.Id.Should()
            .Be(expected: storedPackageItem.Id);

        packageItemBrokerMock.VerifyAll();
        authorizationBrokerMock.VerifyAll();
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldAuthorizeAndUpdatePackageItemWhenUpdateAsync()
    {
        // Given
        PackageItem packageItem = CreatePackageItem();

        PackageItem storedPackageItem = CreatePackageItem(
id: packageItem.Id,
packageId: packageItem.PackageId,
type: packageItem.Type,
data: packageItem.Data);

        packageItemBrokerMock.Setup(expression: broker => broker.GetAppId(entity: packageItem))
            .Returns(value: 7);

        authorizationBrokerMock.Setup(expression: broker => broker.Authorize(appId: 7, privilege: "PackageItem_update"));

        packageItemBrokerMock
            .Setup(expression: broker => broker.UpdatePackageItemAsync(entity: It.Is<PackageItem>(match: item =>
                item.Id == packageItem.Id
                && item.PackageId == packageItem.PackageId
                && item.Type == packageItem.Type
                && item.Data == packageItem.Data)))
            .ReturnsAsync(value: storedPackageItem);

        // When
        PackageItem actualItem =
            await service.UpdatePackageItemAsync(updatedPackageItem: packageItem);

        // Then
        actualItem.Should()
            .BeSameAs(expected: packageItem);

        actualItem.Id.Should()
            .Be(expected: storedPackageItem.Id);

        packageItemBrokerMock.VerifyAll();
        authorizationBrokerMock.VerifyAll();
        packageItemBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldAuthorizeAndDeletePackageItemWhenDeleteAsync()
    {
        // Given
        PackageItem packageItem = CreatePackageItem();

        packageItemBrokerMock
            .Setup(expression: broker => broker.GetAllPackageItems(ignoreFilters: false))
            .Returns(value: new[] { packageItem }.AsQueryable());

        packageItemBrokerMock.Setup(expression: broker => broker.GetAppId(entity: packageItem))
            .Returns(value: 7);

        authorizationBrokerMock.Setup(expression: broker => broker.Authorize(appId: 7, privilege: "PackageItem_delete"));

        packageItemBrokerMock.Setup(expression: broker => broker.DeletePackageItemAsync(entity: packageItem))
            .ReturnsAsync(value: 1);

        // When
        await service.DeletePackageItemAsync(packageItemId: packageItem.Id);

        // Then
        packageItemBrokerMock.Verify(expression: broker => broker.GetAllPackageItems(ignoreFilters: false), times: Times.Once);
        packageItemBrokerMock.Verify(expression: broker => broker.GetAppId(entity: packageItem), times: Times.Once);
        authorizationBrokerMock.Verify(expression: broker => broker.Authorize(appId: 7, privilege: "PackageItem_delete"), times: Times.Once);
        packageItemBrokerMock.Verify(expression: broker => broker.DeletePackageItemAsync(entity: packageItem), times: Times.Once);
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