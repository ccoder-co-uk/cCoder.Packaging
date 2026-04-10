using System.Security;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers.Storages;
using IAuthorizationBroker = cCoder.Packaging.Brokers.IAuthorizationBroker;


namespace cCoder.Packaging.Services.Foundations.Storages;

internal class PackageItemService(
    IPackageItemBroker packageItemBroker,
    IAuthorizationBroker authorizationBroker
) : IPackageItemService
{
    public PackageItem Get(Guid id)
    {
        PackageItem packageItem = GetAll().FirstOrDefault(i => i.Id == id);
        if (packageItem is not null)
            return packageItem;

        PackageItem unrestrictedPackageItem = GetAll(true).FirstOrDefault(i => i.Id == id);
        if (unrestrictedPackageItem is not null)
            throw new SecurityException("Access Denied!");

        return null;
    }

    public IQueryable<PackageItem> GetAll(bool ignoreFilters = false) =>
        packageItemBroker.GetAllPackageItems(ignoreFilters);

    public async ValueTask<PackageItem> AddAsync(PackageItem packageItem)
    {
        authorizationBroker.Authorize(
            packageItemBroker.GetAppId(packageItem),
            $"{nameof(PackageItem)}_create"
        );
        PackageItem newPackageItem = new()
        {
            PackageId = packageItem.PackageId,
            Type = packageItem.Type,
            Data = packageItem.Data,
        };

        newPackageItem = await packageItemBroker.AddPackageItemAsync(newPackageItem);
        newPackageItem.Package = packageItem.Package;
        return newPackageItem;
    }

    public async ValueTask<PackageItem> UpdateAsync(PackageItem packageItem)
    {
        authorizationBroker.Authorize(
            packageItemBroker.GetAppId(packageItem),
            $"{nameof(PackageItem)}_update"
        );
        PackageItem updatePackageItem = new()
        {
            Id = packageItem.Id,
            PackageId = packageItem.PackageId,
            Type = packageItem.Type,
            Data = packageItem.Data,
        };

        updatePackageItem = await packageItemBroker.UpdatePackageItemAsync(updatePackageItem);
        updatePackageItem.Package = packageItem.Package;
        return updatePackageItem;
    }

    public async ValueTask DeleteAsync(Guid id)
    {
        PackageItem packageItem = Get(id);
        authorizationBroker.Authorize(
            packageItemBroker.GetAppId(packageItem),
            $"{nameof(PackageItem)}_delete"
        );
        _ = await packageItemBroker.DeletePackageItemAsync(packageItem);
    }
}












