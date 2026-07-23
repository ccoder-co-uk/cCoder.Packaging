// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Processings;

namespace cCoder.Packaging.Services.Orchestrations;

internal class PackageItemOrchestrationService(IPackageItemProcessingService processingService, IPackageItemEventProcessingService eventService) : IPackageItemOrchestrationService
{
    public cCoder.Data.Models.Packaging.PackageItem Get(Guid id)
    {
        return processingService.GetPackageItem(packageItemId: id);
    }

    public IQueryable<cCoder.Data.Models.Packaging.PackageItem> GetAll(bool ignoreFilters = false)
    {
        return processingService.GetAllPackageItems(ignoreFilters: ignoreFilters);
    }

    public async ValueTask<cCoder.Data.Models.Packaging.PackageItem> AddAsync(cCoder.Data.Models.Packaging.PackageItem entity)
    {
        cCoder.Data.Models.Packaging.PackageItem result =
            await processingService.AddPackageItemAsync(newPackageItem: entity);
        await eventService.RaisePackageItemAddEventAsync(newPackageItem: result);
        return result;
    }

    public async ValueTask<cCoder.Data.Models.Packaging.PackageItem> UpdateAsync(cCoder.Data.Models.Packaging.PackageItem entity)
    {
        cCoder.Data.Models.Packaging.PackageItem result =
            await processingService.UpdatePackageItemAsync(updatedPackageItem: entity);
        await eventService.RaisePackageItemUpdateEventAsync(updatedPackageItem: result);
        return result;
    }

    public async ValueTask DeleteAsync(Guid id)
    {
        cCoder.Data.Models.Packaging.PackageItem entity =
            processingService.GetPackageItem(packageItemId: id);
        await eventService.RaisePackageItemDeleteEventAsync(deletedPackageItem: entity);
        await processingService.DeletePackageItemAsync(packageItemId: id);
    }

    public async ValueTask<IEnumerable<Result<cCoder.Data.Models.Packaging.PackageItem>>> AddOrUpdate(IEnumerable<cCoder.Data.Models.Packaging.PackageItem> items)
    {
        return (await processingService
            .AddOrUpdatePackageItemsAsync(packageItems: items))
            .ToArray();
    }

    public ValueTask DeleteAllAsync(IEnumerable<cCoder.Data.Models.Packaging.PackageItem> items)
    {
        return processingService.DeleteAllPackageItemsAsync(deletedPackageItems: items);
    }
}
