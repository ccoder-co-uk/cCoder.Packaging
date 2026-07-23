// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data;
using cCoder.Data.Models.Packaging;
using Microsoft.EntityFrameworkCore;


namespace cCoder.Packaging.Brokers.Storages;

public interface IPackageItemBroker
{
    IQueryable<PackageItem> GetAllPackageItems();
    IQueryable<PackageItem> GetAllPackageItems(bool ignoreFilters);
    IQueryable<PackageItem> GetAllPackageItemsIgnoringFilters();
    ValueTask<PackageItem> AddPackageItemAsync(PackageItem newPackageItem);
    ValueTask<PackageItem> UpdatePackageItemAsync(PackageItem updatedPackageItem);
    ValueTask<int> DeletePackageItemAsync(PackageItem deletedPackageItem);
    int? GetAppId(PackageItem entity);
}

internal sealed class PackageItemBroker(ICoreContextFactory coreContextFactory) : IPackageItemBroker
{

    public IQueryable<PackageItem> GetAllPackageItems()
    {
        CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();

        return coreDataContext.PackageItems;
    }

    public IQueryable<PackageItem> GetAllPackageItems(bool ignoreFilters)
    {
        Func<IQueryable<PackageItem>>[] packageItemSelectors =
        [
            GetAllPackageItems,
            GetAllPackageItemsIgnoringFilters,
        ];

        return packageItemSelectors[Convert.ToInt32(value: ignoreFilters)]();
    }

    public IQueryable<PackageItem> GetAllPackageItemsIgnoringFilters()
    {
        CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();

        return coreDataContext.PackageItems.IgnoreQueryFilters();
    }

    public async ValueTask<PackageItem> AddPackageItemAsync(PackageItem newPackageItem)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();

        PackageItem result =
            (await coreDataContext.PackageItems.AddAsync(entity: newPackageItem)).Entity;

        _ = await coreDataContext.SaveChangesAsync();
        return result;
    }

    public async ValueTask<PackageItem> UpdatePackageItemAsync(PackageItem updatedPackageItem)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();

        PackageItem result = coreDataContext.PackageItems.Update(entity: updatedPackageItem)
                                 .Entity;

        _ = await coreDataContext.SaveChangesAsync();
        return result;
    }

    public async ValueTask<int> DeletePackageItemAsync(PackageItem deletedPackageItem)
    {
        using CoreDataContext coreDataContext = coreContextFactory.CreateCoreContext();
        coreDataContext.PackageItems.Remove(entity: deletedPackageItem);
        return await coreDataContext.SaveChangesAsync();
    }

    public int? GetAppId(PackageItem entity)
    {
        return null;
    }
}