// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Models;
using cCoder.Packaging.Services.Foundations.Storages;

namespace cCoder.Packaging.Services.Processings;

internal sealed partial class PackageItemProcessingService(
    IPackageItemService packageItemService)
    : IPackageItemProcessingService
{
    public PackageItem GetPackageItem(Guid packageItemId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemOnGet(packageItemId: packageItemId);

            return packageItemService.GetPackageItem(packageItemId: packageItemId);
        });

    public IQueryable<PackageItem> GetAllPackageItems(bool ignoreFilters = false) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemsOnGet(ignoreFilters: ignoreFilters);

            return packageItemService.GetAllPackageItems(ignoreFilters: ignoreFilters);
        });

    public ValueTask<PackageItem> AddPackageItemAsync(PackageItem newPackageItem) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemOnAdd(newPackageItem: newPackageItem);

            return packageItemService
                .AddPackageItemAsync(newPackageItem: newPackageItem);
        });

    public ValueTask<PackageItem> UpdatePackageItemAsync(PackageItem updatedPackageItem) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemOnUpdate(updatedPackageItem: updatedPackageItem);

            return packageItemService
                .UpdatePackageItemAsync(updatedPackageItem: updatedPackageItem);
        });

    public ValueTask DeletePackageItemAsync(Guid packageItemId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemOnDelete(packageItemId: packageItemId);

            return packageItemService
                .DeletePackageItemAsync(packageItemId: packageItemId);
        });

    public ValueTask<IEnumerable<Result<PackageItem>>> AddOrUpdatePackageItemsAsync(
        IEnumerable<PackageItem> packageItems) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemsOnAddOrUpdate(packageItems: packageItems);
            List<Result<PackageItem>> results = [];

            foreach (PackageItem packageItem in packageItems)
            {
                try
                {
                    bool isNewPackageItem = packageItem.Id == Guid.Empty;

                    PackageItem savedPackageItem = isNewPackageItem
                        ? await packageItemService
                            .AddPackageItemAsync(newPackageItem: packageItem)
                        : await packageItemService
                            .UpdatePackageItemAsync(updatedPackageItem: packageItem);

                    results.Add(item: new Result<PackageItem>
                    {
                        Success = true,
                        Item = savedPackageItem,
                        Message = isNewPackageItem
                            ? "Added Successfully"
                            : "Updated Successfully",
                    });
                }
                catch (Exception exception)
                {
                    results.Add(item: new Result<PackageItem>
                    {
                        Success = false,
                        Item = packageItem,
                        Message = exception.Message,
                    });
                }
            }

            return results.AsEnumerable();
        });

    public ValueTask DeleteAllPackageItemsAsync(IEnumerable<PackageItem> deletedPackageItems) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemsOnDelete(deletedPackageItems: deletedPackageItems);

            foreach (PackageItem packageItem in deletedPackageItems)
            {
                await packageItemService
                    .DeletePackageItemAsync(packageItemId: packageItem.Id);
            }
        });
}
