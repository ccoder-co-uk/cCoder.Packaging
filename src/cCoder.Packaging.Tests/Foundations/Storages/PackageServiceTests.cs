using System.Security;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Storages;
using cCoder.Packaging.Services.Foundations.Storages;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Packaging.Tests.Foundations.Storages;

public class PackageServiceTests
{
    private readonly Mock<IPackageBroker> packageBrokerMock;
    private readonly Mock<IAuthorizationBroker> authorizationBrokerMock;
    private readonly PackageService service;

    public PackageServiceTests()
    {
        packageBrokerMock = new Mock<IPackageBroker>(MockBehavior.Strict);
        authorizationBrokerMock = new Mock<IAuthorizationBroker>(MockBehavior.Strict);
        service = new PackageService(packageBrokerMock.Object, authorizationBrokerMock.Object);
    }

    [Fact]
    public void ShouldReturnPackageFromFilteredSetWhenGet()
    {
        Package expectedPackage = CreatePackage();
        Package[] filteredPackages = [expectedPackage];

        packageBrokerMock.Setup(broker => broker.GetAllPackages(false)).Returns(filteredPackages.AsQueryable());

        Package actualPackage = service.Get(expectedPackage.Id);

        actualPackage.Should().BeSameAs(expectedPackage);
        packageBrokerMock.Verify(broker => broker.GetAllPackages(false), Times.Once);
        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldThrowSecurityExceptionWhenGetFindsPackageOnlyInUnrestrictedSet()
    {
        Guid packageId = Guid.NewGuid();
        Package unrestrictedPackage = CreatePackage(packageId);

        packageBrokerMock.Setup(broker => broker.GetAllPackages(false)).Returns(Array.Empty<Package>().AsQueryable());
        packageBrokerMock.Setup(broker => broker.GetAllPackages(true)).Returns(new[] { unrestrictedPackage }.AsQueryable());

        Action act = () => service.Get(packageId);

        act.Should().Throw<SecurityException>().WithMessage("Access Denied!");
        packageBrokerMock.Verify(broker => broker.GetAllPackages(false), Times.Once);
        packageBrokerMock.Verify(broker => broker.GetAllPackages(true), Times.Once);
        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldReturnNullWhenGetDoesNotFindPackage()
    {
        Guid packageId = Guid.NewGuid();

        packageBrokerMock.Setup(broker => broker.GetAllPackages(false)).Returns(Array.Empty<Package>().AsQueryable());
        packageBrokerMock.Setup(broker => broker.GetAllPackages(true)).Returns(Array.Empty<Package>().AsQueryable());

        Package actualPackage = service.Get(packageId);

        actualPackage.Should().BeNull();
        packageBrokerMock.Verify(broker => broker.GetAllPackages(false), Times.Once);
        packageBrokerMock.Verify(broker => broker.GetAllPackages(true), Times.Once);
        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldReturnQueryableFromBrokerWhenGetAll()
    {
        IQueryable<Package> expectedPackages = new[] { CreatePackage(), CreatePackage() }.AsQueryable();

        packageBrokerMock.Setup(broker => broker.GetAllPackages(true)).Returns(expectedPackages);

        IQueryable<Package> actualPackages = service.GetAll(ignoreFilters: true);

        actualPackages.Should().BeSameAs(expectedPackages);
        packageBrokerMock.Verify(broker => broker.GetAllPackages(true), Times.Once);
        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldAuthorizeAndAddPackageWhenAddAsync()
    {
        Package package = CreatePackage();
        Package storedPackage = CreatePackage(
            package.Id,
            name: package.Name,
            description: package.Description,
            category: package.Category,
            sourceApi: package.SourceApi);

        authorizationBrokerMock.Setup(broker => broker.Authorize(null, "Package_create"));
        packageBrokerMock
            .Setup(broker => broker.AddPackageAsync(It.Is<Package>(item =>
                item.Id == Guid.Empty
                && item.Name == package.Name
                && item.Description == package.Description
                && item.Category == package.Category
                && item.SourceApi == package.SourceApi)))
            .ReturnsAsync(storedPackage);

        Package actualPackage = await service.AddAsync(package);

        actualPackage.Should().BeSameAs(package);
        actualPackage.Id.Should().Be(storedPackage.Id);
        authorizationBrokerMock.Verify(broker => broker.Authorize(null, "Package_create"), Times.Once);
        packageBrokerMock.VerifyAll();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldAuthorizeAndUpdatePackageWhenUpdateAsync()
    {
        Package package = CreatePackage();
        Package storedPackage = CreatePackage(
            package.Id,
            name: package.Name,
            description: package.Description,
            category: package.Category,
            sourceApi: package.SourceApi);

        authorizationBrokerMock.Setup(broker => broker.Authorize(null, "Package_update"));
        packageBrokerMock
            .Setup(broker => broker.UpdatePackageAsync(It.Is<Package>(item =>
                item.Id == package.Id
                && item.Name == package.Name
                && item.Description == package.Description
                && item.Category == package.Category
                && item.SourceApi == package.SourceApi)))
            .ReturnsAsync(storedPackage);

        Package actualPackage = await service.UpdateAsync(package);

        actualPackage.Should().BeSameAs(package);
        actualPackage.Id.Should().Be(storedPackage.Id);
        authorizationBrokerMock.Verify(broker => broker.Authorize(null, "Package_update"), Times.Once);
        packageBrokerMock.VerifyAll();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldAuthorizeAndDeletePackageWhenDeleteAsync()
    {
        Package package = CreatePackage();

        packageBrokerMock.Setup(broker => broker.GetAllPackages(false)).Returns(new[] { package }.AsQueryable());
        authorizationBrokerMock.Setup(broker => broker.Authorize(null, "Package_delete"));
        packageBrokerMock.Setup(broker => broker.DeletePackageAsync(package)).ReturnsAsync(1);

        await service.DeleteAsync(package.Id);

        packageBrokerMock.Verify(broker => broker.GetAllPackages(false), Times.Once);
        authorizationBrokerMock.Verify(broker => broker.Authorize(null, "Package_delete"), Times.Once);
        packageBrokerMock.Verify(broker => broker.DeletePackageAsync(package), Times.Once);
        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(nameof(PackageService.ExportRoles))]
    [InlineData(nameof(PackageService.ExportLayouts))]
    [InlineData(nameof(PackageService.ExportTemplates))]
    [InlineData(nameof(PackageService.ExportComponents))]
    [InlineData(nameof(PackageService.ExportScripts))]
    [InlineData(nameof(PackageService.ExportResources))]
    [InlineData(nameof(PackageService.ExportPages))]
    [InlineData(nameof(PackageService.ExportPageRoles))]
    public void ShouldAuthorizeAdminAndDelegateExportMethods(string methodName)
    {
        const int appId = 42;
        Package expectedPackage = CreatePackage(name: methodName);

        authorizationBrokerMock.Setup(broker => broker.IsAdminOfApp(appId)).Returns(true);
        SetupExport(methodName, appId, expectedPackage);

        Package actualPackage = InvokeExport(methodName, appId);

        actualPackage.Should().BeSameAs(expectedPackage);
        authorizationBrokerMock.Verify(broker => broker.IsAdminOfApp(appId), Times.Once);
        packageBrokerMock.VerifyAll();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldThrowSecurityExceptionWhenExportRequestedWithoutAdminAccess()
    {
        const int appId = 42;

        authorizationBrokerMock.Setup(broker => broker.IsAdminOfApp(appId)).Returns(false);

        Action act = () => service.ExportRoles(appId);

        act.Should().Throw<SecurityException>().WithMessage("Access Denied!");
        authorizationBrokerMock.Verify(broker => broker.IsAdminOfApp(appId), Times.Once);
        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    private void SetupExport(string methodName, int appId, Package package)
    {
        switch (methodName)
        {
            case nameof(PackageService.ExportRoles):
                packageBrokerMock.Setup(broker => broker.ExportRoles(appId)).Returns(package);
                break;
            case nameof(PackageService.ExportLayouts):
                packageBrokerMock.Setup(broker => broker.ExportLayouts(appId)).Returns(package);
                break;
            case nameof(PackageService.ExportTemplates):
                packageBrokerMock.Setup(broker => broker.ExportTemplates(appId)).Returns(package);
                break;
            case nameof(PackageService.ExportComponents):
                packageBrokerMock.Setup(broker => broker.ExportComponents(appId)).Returns(package);
                break;
            case nameof(PackageService.ExportScripts):
                packageBrokerMock.Setup(broker => broker.ExportScripts(appId)).Returns(package);
                break;
            case nameof(PackageService.ExportResources):
                packageBrokerMock.Setup(broker => broker.ExportResources(appId)).Returns(package);
                break;
            case nameof(PackageService.ExportPages):
                packageBrokerMock.Setup(broker => broker.ExportPages(appId)).Returns(package);
                break;
            case nameof(PackageService.ExportPageRoles):
                packageBrokerMock.Setup(broker => broker.ExportPageRoles(appId)).Returns(package);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(methodName), methodName, null);
        }
    }

    private Package InvokeExport(string methodName, int appId) =>
        methodName switch
        {
            nameof(PackageService.ExportRoles) => service.ExportRoles(appId),
            nameof(PackageService.ExportLayouts) => service.ExportLayouts(appId),
            nameof(PackageService.ExportTemplates) => service.ExportTemplates(appId),
            nameof(PackageService.ExportComponents) => service.ExportComponents(appId),
            nameof(PackageService.ExportScripts) => service.ExportScripts(appId),
            nameof(PackageService.ExportResources) => service.ExportResources(appId),
            nameof(PackageService.ExportPages) => service.ExportPages(appId),
            nameof(PackageService.ExportPageRoles) => service.ExportPageRoles(appId),
            _ => throw new ArgumentOutOfRangeException(nameof(methodName), methodName, null),
        };

    private static Package CreatePackage(
        Guid? id = null,
        string name = null,
        string description = null,
        string category = null,
        string sourceApi = null) =>
        new()
        {
            Id = id ?? Guid.NewGuid(),
            Name = name ?? $"Package-{Guid.NewGuid():N}",
            Description = description ?? $"Description-{Guid.NewGuid():N}",
            Category = category ?? "Core",
            SourceApi = sourceApi ?? "https://example.test/api",
        };
}
