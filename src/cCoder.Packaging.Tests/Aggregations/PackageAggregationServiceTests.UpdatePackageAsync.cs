// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Models.Results;
using Moq;
using Xunit;

namespace cCoder.Packaging.Tests.Aggregations;

public partial class PackageAggregationServiceTests
{
    [Fact]
    public async Task ShouldReplacePackageItemsAndRaiseEventOnUpdate()
    {
        // Given

        Guid packageId = Guid.NewGuid();
        var replacementItem = new PackageItem { Type = "Core/Role", Data = "[]" };
        var existingItem = new PackageItem { PackageId = packageId };
        var updatedPackage = new Package(name:"Roles")
        {
            Id = packageId,
            Items = [replacementItem]
        };

        var savedPackage = new Package(name:"Roles") { Id = packageId };

        packageProcessingServiceMock
            .Setup(expression:service => service.UpdatePackageAsync(
                updatedPackage:updatedPackage))
            .ReturnsAsync(value:savedPackage);

        packageItemProcessingServiceMock
            .Setup(expression:service => service.GetAllPackageItems(
                ignoreFilters:false))
            .Returns(value:new[] { existingItem }.AsQueryable());

        packageItemProcessingServiceMock
            .Setup(expression:service => service.DeleteAllPackageItemsAsync(
                It.Is<IEnumerable<PackageItem>>(
                    match:items => items.Single() == existingItem)))
            .Returns(value:ValueTask.CompletedTask);

        packageItemProcessingServiceMock
            .Setup(expression:service => service.AddOrUpdatePackageItemsAsync(
                packageItems:It.Is<IEnumerable<PackageItem>>(
                    match:items => items.Single().PackageId == packageId)))
            .ReturnsAsync(value:Array.Empty<Result<PackageItem>>());

        packageEventProcessingServiceMock
            .Setup(expression:service => service.RaisePackageUpdateEventAsync(
                updatedPackage:savedPackage))
            .Returns(value:ValueTask.CompletedTask);

        // When

        Package actualPackage =
            await aggregationService.UpdatePackageAsync(
                updatedPackage:updatedPackage);

        // Then

        Assert.Same(expected:savedPackage, actual:actualPackage);
        Assert.Equal(expected:packageId, actual:replacementItem.PackageId);
        packageProcessingServiceMock.VerifyAll();
        packageItemProcessingServiceMock.VerifyAll();
        packageEventProcessingServiceMock.VerifyAll();
    }
}