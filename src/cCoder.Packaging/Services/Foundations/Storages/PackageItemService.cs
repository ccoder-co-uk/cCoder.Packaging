// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Storages;

namespace cCoder.Packaging.Services.Foundations.Storages;

internal sealed partial class PackageItemService(
    IPackageItemBroker packageItemBroker,
    IAuthorizationBroker authorizationBroker)
    : IPackageItemService
{
    public PackageItem GetPackageItem(Guid packageItemId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemOnGet(packageItemId: packageItemId);

            return SelectPackageItem(packageItemId: packageItemId);
        });

    public IQueryable<PackageItem> GetAllPackageItems(bool ignoreFilters = false) =>
        TryCatch(operation: () =>
        {
            ValidateAllPackageItemsOnGet(ignoreFilters: ignoreFilters);

            return packageItemBroker.GetAllPackageItems(ignoreFilters: ignoreFilters);
        });

    public ValueTask<PackageItem> AddPackageItemAsync(PackageItem newPackageItem) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemOnAdd(newPackageItem: newPackageItem);

            authorizationBroker.Authorize(
                appId: packageItemBroker.GetAppId(entity: newPackageItem),
                privilege: $"{nameof(PackageItem)}_create");

            PackageItem packageItem = new()
            {
                PackageId = newPackageItem.PackageId,
                Type = newPackageItem.Type,
                Data = newPackageItem.Data,
            };

            PackageItem savedPackageItem =
                await packageItemBroker.AddPackageItemAsync(entity: packageItem);

            newPackageItem.Id = savedPackageItem.Id;
            newPackageItem.PackageId = savedPackageItem.PackageId;
            newPackageItem.Type = savedPackageItem.Type;
            newPackageItem.Data = savedPackageItem.Data;

            return newPackageItem;
        });

    public ValueTask<PackageItem> UpdatePackageItemAsync(PackageItem updatedPackageItem) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemOnUpdate(updatedPackageItem: updatedPackageItem);

            authorizationBroker.Authorize(
                appId: packageItemBroker.GetAppId(entity: updatedPackageItem),
                privilege: $"{nameof(PackageItem)}_update");

            PackageItem packageItem = new()
            {
                Id = updatedPackageItem.Id,
                PackageId = updatedPackageItem.PackageId,
                Type = updatedPackageItem.Type,
                Data = updatedPackageItem.Data,
            };

            PackageItem savedPackageItem =
                await packageItemBroker.UpdatePackageItemAsync(entity: packageItem);

            updatedPackageItem.Id = savedPackageItem.Id;
            updatedPackageItem.PackageId = savedPackageItem.PackageId;
            updatedPackageItem.Type = savedPackageItem.Type;
            updatedPackageItem.Data = savedPackageItem.Data;

            return updatedPackageItem;
        });

    public ValueTask DeletePackageItemAsync(Guid packageItemId) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageItemOnDelete(packageItemId: packageItemId);
            PackageItem deletedPackageItem = SelectPackageItem(packageItemId: packageItemId);

            authorizationBroker.Authorize(
                appId: packageItemBroker.GetAppId(entity: deletedPackageItem),
                privilege: $"{nameof(PackageItem)}_delete");

            _ = await packageItemBroker.DeletePackageItemAsync(entity: deletedPackageItem);
        });

    private PackageItem SelectPackageItem(Guid packageItemId)
    {
        PackageItem packageItem = packageItemBroker
            .GetAllPackageItems(ignoreFilters: false)
            .FirstOrDefault(predicate: item => item.Id == packageItemId);

        if (packageItem is not null)
        {
            return packageItem;
        }

        PackageItem unrestrictedPackageItem = packageItemBroker
            .GetAllPackageItems(ignoreFilters: true)
            .FirstOrDefault(predicate: item => item.Id == packageItemId);

        if (unrestrictedPackageItem is not null)
        {
            throw new SecurityException("Access Denied!");
        }

        return null;
    }
}