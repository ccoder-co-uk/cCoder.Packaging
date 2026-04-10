using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Processings;

namespace cCoder.Packaging.Services.Orchestrations;

internal class PackageItemOrchestrationService(IPackageItemProcessingService processingService, IPackageItemEventProcessingService eventService) : IPackageItemOrchestrationService
{
    public cCoder.Data.Models.Packaging.PackageItem Get(Guid id)
    {
        return processingService.Get(id);
    }

    public IQueryable<cCoder.Data.Models.Packaging.PackageItem> GetAll(bool ignoreFilters = false)
    {
        return processingService.GetAll(ignoreFilters);
    }

    public async ValueTask<cCoder.Data.Models.Packaging.PackageItem> AddAsync(cCoder.Data.Models.Packaging.PackageItem entity)
    {
        cCoder.Data.Models.Packaging.PackageItem result = await processingService.AddAsync(entity);
        await eventService.RaisePackageItemAddEventAsync(result);
        return result;
    }

    public async ValueTask<cCoder.Data.Models.Packaging.PackageItem> UpdateAsync(cCoder.Data.Models.Packaging.PackageItem entity)
    {
        cCoder.Data.Models.Packaging.PackageItem result = await processingService.UpdateAsync(entity);
        await eventService.RaisePackageItemUpdateEventAsync(result);
        return result;
    }

    public async ValueTask DeleteAsync(Guid id)
    {
        cCoder.Data.Models.Packaging.PackageItem entity = processingService.Get(id);
        await eventService.RaisePackageItemDeleteEventAsync(entity);
        await processingService.DeleteAsync(id);
    }

    public async ValueTask<IEnumerable<Result<cCoder.Data.Models.Packaging.PackageItem>>> AddOrUpdate(IEnumerable<cCoder.Data.Models.Packaging.PackageItem> items)
    {
        return (await processingService.AddOrUpdate(items)).ToArray();
    }

    public ValueTask DeleteAllAsync(IEnumerable<cCoder.Data.Models.Packaging.PackageItem> items)
    {
        return processingService.DeleteAllAsync(items);
    }
}
