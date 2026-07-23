// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Storages;
using cCoder.Packaging.Services.Foundations.Storages;
using cCoder.Packaging.Models.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;


namespace cCoder.Packaging.Tests.Foundations.Storages;

public partial class PackageServiceTests
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
        Package actualPackage = service.GetPackage(packageId: expectedPackage.Id);

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
        Action act = () => service.GetPackage(packageId: packageId);

        // Then
        PackagingServiceException exception = act.Should()
            .Throw<PackagingServiceException>()
            .Which;

        exception.InnerException.Should()
            .BeOfType<SecurityException>()
            .Which.Message.Should()
            .Be(expected: "Access Denied!");

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
        Package actualPackage = service.GetPackage(packageId: packageId);

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
        IQueryable<Package> actualPackages = service.GetAllPackages(ignoreFilters: true);

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
            .Setup(expression: broker => broker.AddPackageAsync(newPackage: It.Is<Package>(match: item =>
                item.Id == Guid.Empty
                && item.Name == package.Name
                && item.Description == package.Description
                && item.Category == package.Category
                && item.SourceApi == package.SourceApi)))
            .ReturnsAsync(value: storedPackage);

        // When
        Package actualPackage = await service.AddPackageAsync(newPackage: package);

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
            .Setup(expression: broker => broker.UpdatePackageAsync(updatedPackage: It.Is<Package>(match: item =>
                item.Id == package.Id
                && item.Name == package.Name
                && item.Description == package.Description
                && item.Category == package.Category
                && item.SourceApi == package.SourceApi)))
            .ReturnsAsync(value: storedPackage);

        // When
        Package actualPackage = await service.UpdatePackageAsync(updatedPackage: package);

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

        packageBrokerMock.Setup(
            expression: broker => broker.DeletePackageAsync(deletedPackage: package))
            .ReturnsAsync(value: 1);

        // When
        await service.DeletePackageAsync(packageId: package.Id);

        // Then
        packageBrokerMock.Verify(expression: broker => broker.GetAllPackages(ignoreFilters: false), times: Times.Once);
        authorizationBrokerMock.Verify(expression: broker => broker.Authorize(appId: null, privilege: "Package_delete"), times: Times.Once);

        packageBrokerMock.Verify(
            expression: broker => broker.DeletePackageAsync(deletedPackage: package),
            times: Times.Once);

        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(nameof(PackageService.ExportPackageRoles))]
    [InlineData(nameof(PackageService.ExportPackageLayouts))]
    [InlineData(nameof(PackageService.ExportPackageTemplates))]
    [InlineData(nameof(PackageService.ExportPackageComponents))]
    [InlineData(nameof(PackageService.ExportPackageScripts))]
    [InlineData(nameof(PackageService.ExportPackageResources))]
    [InlineData(nameof(PackageService.ExportPackagePages))]
    [InlineData(nameof(PackageService.ExportPackagePageRoles))]
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
        Action act = () => service.ExportPackageRoles(appId: appId);

        // Then
        PackagingServiceException exception = act.Should()
            .Throw<PackagingServiceException>()
            .Which;

        exception.InnerException.Should()
            .BeOfType<SecurityException>()
            .Which.Message.Should()
            .Be(expected: "Access Denied!");

        authorizationBrokerMock.Verify(expression: broker => broker.IsAdminOfApp(appId: appId), times: Times.Once);
        packageBrokerMock.VerifyNoOtherCalls();
        authorizationBrokerMock.VerifyNoOtherCalls();
    }

    private void SetupExport(string methodName, int appId, Package package)
    {
        switch (methodName)
        {
            case nameof(PackageService.ExportPackageRoles):
                packageBrokerMock.Setup(expression: broker => broker.ExportRoles(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportPackageLayouts):
                packageBrokerMock.Setup(expression: broker => broker.ExportLayouts(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportPackageTemplates):
                packageBrokerMock.Setup(expression: broker => broker.ExportTemplates(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportPackageComponents):
                packageBrokerMock.Setup(expression: broker => broker.ExportComponents(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportPackageScripts):
                packageBrokerMock.Setup(expression: broker => broker.ExportScripts(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportPackageResources):
                packageBrokerMock.Setup(expression: broker => broker.ExportResources(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportPackagePages):
                packageBrokerMock.Setup(expression: broker => broker.ExportPages(appId: appId))
                    .Returns(value: package);
                break;
            case nameof(PackageService.ExportPackagePageRoles):
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
            nameof(PackageService.ExportPackageRoles) => service.ExportPackageRoles(appId: appId),
            nameof(PackageService.ExportPackageLayouts) => service.ExportPackageLayouts(appId: appId),
            nameof(PackageService.ExportPackageTemplates) => service.ExportPackageTemplates(appId: appId),
            nameof(PackageService.ExportPackageComponents) => service.ExportPackageComponents(appId: appId),
            nameof(PackageService.ExportPackageScripts) => service.ExportPackageScripts(appId: appId),
            nameof(PackageService.ExportPackageResources) => service.ExportPackageResources(appId: appId),
            nameof(PackageService.ExportPackagePages) => service.ExportPackagePages(appId: appId),
            nameof(PackageService.ExportPackagePageRoles) => service.ExportPackagePageRoles(appId: appId),
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