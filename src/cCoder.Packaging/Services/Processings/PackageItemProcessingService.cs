// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Foundations.Storages;

namespace cCoder.Packaging.Services.Processings;

internal class PackageItemProcessingService(IPackageItemService service) : IPackageItemProcessingService
{
    public cCoder.Data.Models.Packaging.PackageItem Get(Guid id)
    {
        return service.Get(id:id);
    }

    public IQueryable<cCoder.Data.Models.Packaging.PackageItem> GetAll(bool ignoreFilters = false)
    {
        return service.GetAll(ignoreFilters:ignoreFilters);
    }

    public ValueTask<cCoder.Data.Models.Packaging.PackageItem> AddAsync(cCoder.Data.Models.Packaging.PackageItem entity)
    {
        return service.AddAsync(packageItem:entity);
    }

    public ValueTask<cCoder.Data.Models.Packaging.PackageItem> UpdateAsync(cCoder.Data.Models.Packaging.PackageItem entity)
    {
        return service.UpdateAsync(packageItem:entity);
    }

    public ValueTask DeleteAsync(Guid id)
    {
        return service.DeleteAsync(id:id);
    }

    public async ValueTask<IEnumerable<Result<cCoder.Data.Models.Packaging.PackageItem>>> AddOrUpdate(IEnumerable<cCoder.Data.Models.Packaging.PackageItem> items)
    {
        List<Result<cCoder.Data.Models.Packaging.PackageItem>> results = new List<Result<cCoder.Data.Models.Packaging.PackageItem>>();

        foreach (cCoder.Data.Models.Packaging.PackageItem item in items)
        {
            try
            {
                cCoder.Data.Models.Packaging.PackageItem savedItem =
                    item.Id == Guid.Empty
                        ? await AddAsync(entity:item)
                        : await UpdateAsync(entity:item);

                results.Add(item:new Result<cCoder.Data.Models.Packaging.PackageItem>
                {
                    Success = true,
                    Item = savedItem,
                    Message = item.Id == Guid.Empty ? "Added Successfully" : "Updated Successfully"
                });
            }
            catch (Exception ex)
            {
                results.Add(item:new Result<cCoder.Data.Models.Packaging.PackageItem>
                {
                    Success = false,
                    Item = item,
                    Message = ex.Message
                });
            }
        }

        return results;
    }

    public async ValueTask DeleteAllAsync(IEnumerable<cCoder.Data.Models.Packaging.PackageItem> items)
    {
        foreach (cCoder.Data.Models.Packaging.PackageItem item in items)
        {
            await DeleteAsync(id:item.Id);
        }
    }
}