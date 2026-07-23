// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Storages;


namespace cCoder.Packaging.Services.Foundations.Storages;

internal class PackageItemService(
    IPackageItemBroker packageItemBroker,
    IAuthorizationBroker authorizationBroker
) : IPackageItemService
{
    public PackageItem Get(Guid id)
    {
        PackageItem packageItem = GetAll()
                                      .FirstOrDefault(predicate: i => i.Id == id);

        if (packageItem is not null)
        {
            return packageItem;
        }

        PackageItem unrestrictedPackageItem = GetAll(ignoreFilters: true)
                                                  .FirstOrDefault(predicate: i => i.Id == id);

        if (unrestrictedPackageItem is not null)
        {
            throw new SecurityException("Access Denied!");
        }

        return null;
    }

    public IQueryable<PackageItem> GetAll(bool ignoreFilters = false) =>
        packageItemBroker.GetAllPackageItems(ignoreFilters: ignoreFilters);

    public async ValueTask<PackageItem> AddAsync(PackageItem packageItem)
    {
        authorizationBroker.Authorize(
appId: packageItemBroker.GetAppId(entity: packageItem),
privilege: $"{nameof(PackageItem)}_create"
        );

        PackageItem newPackageItem = new()
        {
            PackageId = packageItem.PackageId,
            Type = packageItem.Type,
            Data = packageItem.Data,
        };

        PackageItem result = await packageItemBroker.AddPackageItemAsync(entity: newPackageItem);
        packageItem.Id = result.Id;
        packageItem.PackageId = result.PackageId;
        packageItem.Type = result.Type;
        packageItem.Data = result.Data;
        return packageItem;
    }

    public async ValueTask<PackageItem> UpdateAsync(PackageItem packageItem)
    {
        authorizationBroker.Authorize(
appId: packageItemBroker.GetAppId(entity: packageItem),
privilege: $"{nameof(PackageItem)}_update"
        );

        PackageItem updatePackageItem = new()
        {
            Id = packageItem.Id,
            PackageId = packageItem.PackageId,
            Type = packageItem.Type,
            Data = packageItem.Data,
        };

        PackageItem result = await packageItemBroker.UpdatePackageItemAsync(entity: updatePackageItem);
        packageItem.Id = result.Id;
        packageItem.PackageId = result.PackageId;
        packageItem.Type = result.Type;
        packageItem.Data = result.Data;
        return packageItem;
    }

    public async ValueTask DeleteAsync(Guid id)
    {
        PackageItem packageItem = Get(id: id);

        authorizationBroker.Authorize(
appId: packageItemBroker.GetAppId(entity: packageItem),
privilege: $"{nameof(PackageItem)}_delete"
        );

        _ = await packageItemBroker.DeletePackageItemAsync(entity: packageItem);
    }
}