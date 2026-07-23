// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Dependencies;
using cCoder.Packaging.Models;
using cCoder.Packaging.Services.Processings;

namespace cCoder.Packaging.Services.Orchestrations;

internal sealed partial class PackageItemOrchestrationService(
    IPackageItemProcessingService packageItemProcessingService,
    IPackageItemEventProcessingService packageItemEventProcessingService)
    : IPackageItemOrchestrationService
{
    public PackageItem GetPackageItem(Guid packageItemId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemOnGet(packageItemId: packageItemId);

            return packageItemProcessingService
                .GetPackageItem(packageItemId: packageItemId);
        });

    public IQueryable<PackageItem> GetAllPackageItems(bool ignoreFilters = false) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemsOnGet(ignoreFilters: ignoreFilters);

            return packageItemProcessingService
                .GetAllPackageItems(ignoreFilters: ignoreFilters);
        });

    public ValueTask<PackageItem> AddPackageItemAsync(PackageItem newPackageItem) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemOnAdd(newPackageItem: newPackageItem);

            PackageItem savedPackageItem = await packageItemProcessingService
                .AddPackageItemAsync(newPackageItem: newPackageItem);

            await packageItemEventProcessingService
                .RaisePackageItemAddEventAsync(newPackageItem: savedPackageItem);

            return savedPackageItem;
        });

    public ValueTask<PackageItem> UpdatePackageItemAsync(PackageItem updatedPackageItem) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemOnUpdate(updatedPackageItem: updatedPackageItem);

            PackageItem savedPackageItem = await packageItemProcessingService
                .UpdatePackageItemAsync(updatedPackageItem: updatedPackageItem);

            await packageItemEventProcessingService
                .RaisePackageItemUpdateEventAsync(updatedPackageItem: savedPackageItem);

            return savedPackageItem;
        });

    public ValueTask DeletePackageItemAsync(Guid packageItemId) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemOnDelete(packageItemId: packageItemId);

            PackageItem deletedPackageItem = packageItemProcessingService
                .GetPackageItem(packageItemId: packageItemId);

            await packageItemEventProcessingService
                .RaisePackageItemDeleteEventAsync(deletedPackageItem: deletedPackageItem);

            await packageItemProcessingService
                .DeletePackageItemAsync(packageItemId: packageItemId);
        });

    public ValueTask<IEnumerable<Result<PackageItem>>> AddOrUpdatePackageItemsAsync(
        IEnumerable<PackageItem> packageItems) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemsOnAddOrUpdate(packageItems: packageItems);

            IEnumerable<Result<PackageItem>> results =
                await packageItemProcessingService
                    .AddOrUpdatePackageItemsAsync(packageItems: packageItems);

            return results;
        });

    public ValueTask DeleteAllPackageItemsAsync(
        IEnumerable<PackageItem> deletedPackageItems) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemsOnDelete(deletedPackageItems: deletedPackageItems);

            return packageItemProcessingService
                .DeleteAllPackageItemsAsync(deletedPackageItems: deletedPackageItems);
        });
}