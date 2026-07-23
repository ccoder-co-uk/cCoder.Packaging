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
        // Given
        Package expectedPackage = CreatePackage();
        Package[] filteredPackages = [expectedPackage];

        packageBrokerMock.Setup(expression: broker => broker.GetAllPackages(ignoreFilters: false))
            .Returns(value: filteredPackages.AsQueryable());

        // When
        Package actualPackage = service.Get(id: expectedPackage.Id);

        // Then
        actualPackage.Should()
            .BeSameAs(expected: expectedPackage);

        packageBrokerMock.Verify(expression: broker => broker.GetAllPackages(ignoreFilters: false), times: Times.Once);
        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldThrowSecurityExceptionWhenGetFindsPackageOnlyInUnrestrictedSet()
    {
        // Given
        Guid packageId = Guid.NewGuid();
        Package unrestrictedPackage = CreatePackage(id: packageId);

        packageBrokerMock.Setup(expression: broker => broker.GetAllPackages(ignoreFilters: false))
            .Returns(value: Array.Empty<Package>()
                                                                                    .AsQueryable());

        packageBrokerMock.Setup(expression: broker => broker.GetAllPackages(ignoreFilters: true))
            .Returns(value: new[] { unrestrictedPackage }.AsQueryable());

        // When
        Action act = () => service.Get(id: packageId);

        // Then
        act.Should()
            .Throw<SecurityException>()
            .WithMessage(expectedWildcardPattern: "Access Denied!");

        packageBrokerMock.Verify(expression: broker => broker.GetAllPackages(ignoreFilters: false), times: Times.Once);
        packageBrokerMock.Verify(expression: broker => broker.GetAllPackages(ignoreFilters: true), times: Times.Once);
        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldReturnNullWhenGetDoesNotFindPackage()
    {
        // Given
        Guid packageId = Guid.NewGuid();

        packageBrokerMock.Setup(expression: broker => broker.GetAllPackages(ignoreFilters: false))
            .Returns(value: Array.Empty<Package>()
                                                                                    .AsQueryable());

        packageBrokerMock.Setup(expression: broker => broker.GetAllPackages(ignoreFilters: true))
            .Returns(value: Array.Empty<Package>()
                                                                                   .AsQueryable());

        // When
        Package actualPackage = service.Get(id: packageId);

        // Then
        actualPackage.Should()
            .BeNull();

        packageBrokerMock.Verify(expression: broker => broker.GetAllPackages(ignoreFilters: false), times: Times.Once);
        packageBrokerMock.Verify(expression: broker => broker.GetAllPackages(ignoreFilters: true), times: Times.Once);
        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldReturnQueryableFromBrokerWhenGetAll()
    {
        // Given
        IQueryable<Package> expectedPackages = new[] { CreatePackage(), CreatePackage() }.AsQueryable();

        packageBrokerMock.Setup(expression: broker => broker.GetAllPackages(ignoreFilters: true))
            .Returns(value: expectedPackages);

        // When
        IQueryable<Package> actualPackages = service.GetAll(ignoreFilters: true);

        // Then
        actualPackages.Should()
            .BeSameAs(expected: expectedPackages);

        packageBrokerMock.Verify(expression: broker => broker.GetAllPackages(ignoreFilters: true), times: Times.Once);
        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldAuthorizeAndAddPackageWhenAddAsync()
    {
        // Given
        Package package = CreatePackage();

        Package storedPackage = CreatePackage(
id: package.Id,
            name: package.Name,
            description: package.Description,
            category: package.Category,
            sourceApi: package.SourceApi);

        authorizationBrokerMock.Setup(expression: broker => broker.Authorize(appId: null, privilege: "Package_create"));

        packageBrokerMock
            .Setup(expression: broker => broker.AddPackageAsync(entity: It.Is<Package>(match: item =>
                item.Id == Guid.Empty
                && item.Name == package.Name
                && item.Description == package.Description
                && item.Category == package.Category
                && item.SourceApi == package.SourceApi)))
            .ReturnsAsync(value: storedPackage);

        // When
        Package actualPackage = await service.AddAsync(package: package);

        // Then
        actualPackage.Should()
            .BeSameAs(expected: package);

        actualPackage.Id.Should()
            .Be(expected: storedPackage.Id);

        authorizationBrokerMock.Verify(expression: broker => broker.Authorize(appId: null, privilege: "Package_create"), times: Times.Once);
        packageBrokerMock.VerifyAll();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldAuthorizeAndUpdatePackageWhenUpdateAsync()
    {
        // Given
        Package package = CreatePackage();

        Package storedPackage = CreatePackage(
id: package.Id,
            name: package.Name,
            description: package.Description,
            category: package.Category,
            sourceApi: package.SourceApi);

        authorizationBrokerMock.Setup(expression: broker => broker.Authorize(appId: null, privilege: "Package_update"));

        packageBrokerMock
            .Setup(expression: broker => broker.UpdatePackageAsync(entity: It.Is<Package>(match: item =>
                item.Id == package.Id
                && item.Name == package.Name
                && item.Description == package.Description
                && item.Category == package.Category
                && item.SourceApi == package.SourceApi)))
            .ReturnsAsync(value: storedPackage);

        // When
        Package actualPackage = await service.UpdateAsync(package: package);

        // Then
        actualPackage.Should()
            .BeSameAs(expected: package);

        actualPackage.Id.Should()
            .Be(expected: storedPackage.Id);

        authorizationBrokerMock.Verify(expression: broker => broker.Authorize(appId: null, privilege: "Package_update"), times: Times.Once);
        packageBrokerMock.VerifyAll();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldAuthorizeAndDeletePackageWhenDeleteAsync()
    {
        // Given
        Package package = CreatePackage();

        packageBrokerMock.Setup(expression: broker => broker.GetAllPackages(ignoreFilters: false))
            .Returns(value: new[] { package }.AsQueryable());

        authorizationBrokerMock.Setup(expression: broker => broker.Authorize(appId: null, privilege: "Package_delete"));

        packageBrokerMock.Setup(expression: broker => broker.DeletePackageAsync(entity: package))
            .ReturnsAsync(value: 1);

        // When
        await service.DeleteAsync(id: package.Id);

        // Then
        packageBrokerMock.Verify(expression: broker => broker.GetAllPackages(ignoreFilters: false), times: Times.Once);
        authorizationBrokerMock.Verify(expression: broker => broker.Authorize(appId: null, privilege: "Package_delete"), times: Times.Once);
        packageBrokerMock.Verify(expression: broker => broker.DeletePackageAsync(entity: package), times: Times.Once);
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
        // Given
        const int appId = 42;
        Package expectedPackage = CreatePackage(name: methodName);

        authorizationBrokerMock.Setup(expression: broker => broker.IsAdminOfApp(appId: appId))
            .Returns(value: true);

        SetupExport(methodName: methodName, appId: appId, package: expectedPackage);

        // When
        Package actualPackage = InvokeExport(methodName: methodName, appId: appId);

        // Then
        actualPackage.Should()
            .BeSameAs(expected: expectedPackage);

        authorizationBrokerMock.Verify(expression: broker => broker.IsAdminOfApp(appId: appId), times: Times.Once);
        packageBrokerMock.VerifyAll();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldThrowSecurityExceptionWhenExportRequestedWithoutAdminAccess()
    {
        // Given
        const int appId = 42;

        authorizationBrokerMock.Setup(expression: broker => broker.IsAdminOfApp(appId: appId))
            .Returns(value: false);

        // When
        Action act = () => service.ExportRoles(appId: appId);

        // Then
        act.Should()
            .Throw<SecurityException>()
            .WithMessage(expectedWildcardPattern: "Access Denied!");

        authorizationBrokerMock.Verify(expression: broker => broker.IsAdminOfApp(appId: appId), times: Times.Once);
        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    private void SetupExport(string methodName, int appId, Package package)
    {
        switch (methodName)
        {
            case nameof(PackageService.ExportRoles):
                packageBrokerMock.Setup(expression: broker => broker.ExportRoles(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportLayouts):
                packageBrokerMock.Setup(expression: broker => broker.ExportLayouts(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportTemplates):
                packageBrokerMock.Setup(expression: broker => broker.ExportTemplates(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportComponents):
                packageBrokerMock.Setup(expression: broker => broker.ExportComponents(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportScripts):
                packageBrokerMock.Setup(expression: broker => broker.ExportScripts(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportResources):
                packageBrokerMock.Setup(expression: broker => broker.ExportResources(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportPages):
                packageBrokerMock.Setup(expression: broker => broker.ExportPages(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportPageRoles):
                packageBrokerMock.Setup(expression: broker => broker.ExportPageRoles(appId: appId))
                    .Returns(value: package);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(methodName), methodName, null);
        }
    }

    private Package InvokeExport(string methodName, int appId) =>
        methodName switch
        {
            nameof(PackageService.ExportRoles) => service.ExportRoles(appId: appId),
            nameof(PackageService.ExportLayouts) => service.ExportLayouts(appId: appId),
            nameof(PackageService.ExportTemplates) => service.ExportTemplates(appId: appId),
            nameof(PackageService.ExportComponents) => service.ExportComponents(appId: appId),
            nameof(PackageService.ExportScripts) => service.ExportScripts(appId: appId),
            nameof(PackageService.ExportResources) => service.ExportResources(appId: appId),
            nameof(PackageService.ExportPages) => service.ExportPages(appId: appId),
            nameof(PackageService.ExportPageRoles) => service.ExportPageRoles(appId: appId),
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