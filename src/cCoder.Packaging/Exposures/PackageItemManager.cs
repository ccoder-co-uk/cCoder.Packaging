// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Orchestrations;

namespace cCoder.Packaging.Exposures;

internal sealed class PackageItemManager(
    IPackageItemOrchestrationService packageItemOrchestrationService)
    : IPackageItemManager
{
    public PackageItem GetPackageItem(Guid packageItemId) =>
        packageItemOrchestrationService.GetPackageItem(
            packageItemId: packageItemId);

    public IQueryable<PackageItem> GetAllPackageItems(
        bool ignoreFilters = false) =>
        packageItemOrchestrationService.GetAllPackageItems(
            ignoreFilters: ignoreFilters);

    public ValueTask<PackageItem> AddPackageItemAsync(
        PackageItem newPackageItem) =>
        packageItemOrchestrationService.AddPackageItemAsync(
            newPackageItem: newPackageItem);

    public ValueTask<PackageItem> UpdatePackageItemAsync(
        PackageItem updatedPackageItem) =>
        packageItemOrchestrationService.UpdatePackageItemAsync(
            updatedPackageItem: updatedPackageItem);

    public ValueTask DeletePackageItemAsync(Guid packageItemId) =>
        packageItemOrchestrationService.DeletePackageItemAsync(
            packageItemId: packageItemId);
}