// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Foundations.Storages;

namespace cCoder.Packaging.Services.Processings;

internal class PackageProcessingService(IPackageService service, IPackageItemProcessingService packageItemService) : IPackageProcessingService
{
    public cCoder.Data.Models.Packaging.Package ExportPackage(int appId, string packageName)
    {
        cCoder.Data.Models.Packaging.Package result = packageName switch
        {
            "Roles" => service.ExportRoles(appId:appId),
            "Layouts" => service.ExportLayouts(appId:appId),
            "Templates" => service.ExportTemplates(appId:appId),
            "Components" => service.ExportComponents(appId:appId),
            "Scripts" => service.ExportScripts(appId:appId),
            "Resources" => service.ExportResources(appId:appId),
            "Pages" => service.ExportPages(appId:appId),
            "PageRoles" => service.ExportPageRoles(appId:appId),
            _ => new cCoder.Data.Models.Packaging.Package
            {
                Name = packageName,
                Items = Array.Empty<cCoder.Data.Models.Packaging.PackageItem>()
            },
        };

        return result;
    }

    public cCoder.Data.Models.Packaging.Package[] ExportPackages(int appId, string[] packageNames)
    {
        return packageNames.Select(selector:(string name) => ExportPackage(appId:appId, packageName:name))
                   .ToArray();
    }

    public cCoder.Data.Models.Packaging.Package Get(Guid id)
    {
        return service.Get(id:id);
    }

    public IQueryable<cCoder.Data.Models.Packaging.Package> GetAll(bool ignoreFilters = false)
    {
        return service.GetAll(ignoreFilters:ignoreFilters);
    }

    public ValueTask<cCoder.Data.Models.Packaging.Package> AddAsync(cCoder.Data.Models.Packaging.Package entity)
    {
        if (entity.Items != null && entity.Items.Any())
        {
            entity.Items.ForEach(action:delegate (cCoder.Data.Models.Packaging.PackageItem item)
            {
                item.PackageId = entity.Id;
                item.Package = null;
            });
        }

        return service.AddAsync(package:entity);
    }

    public async ValueTask<cCoder.Data.Models.Packaging.Package> UpdateAsync(cCoder.Data.Models.Packaging.Package entity)
    {
        cCoder.Data.Models.Packaging.Package result = await service.UpdateAsync(package:entity);

        if (entity.Items != null && entity.Items.Any())
        {
            await packageItemService.DeleteAllAsync(items:(from item in packageItemService.GetAll()
                                                     where item.PackageId == result.Id
                                                     select item).ToArray());

            entity.Items.ForEach(action:delegate (cCoder.Data.Models.Packaging.PackageItem item)
            {
                item.PackageId = result.Id;
            });

            await packageItemService.AddOrUpdate(items:entity.Items);
        }

        return result;
    }

    public ValueTask DeleteAsync(Guid id)
    {
        return service.DeleteAsync(id:id);
    }

    public async ValueTask<IEnumerable<Result<cCoder.Data.Models.Packaging.Package>>> AddOrUpdate(IEnumerable<cCoder.Data.Models.Packaging.Package> items)
    {
        List<Result<cCoder.Data.Models.Packaging.Package>> results = new List<Result<cCoder.Data.Models.Packaging.Package>>();

        foreach (cCoder.Data.Models.Packaging.Package item in items)
        {
            try
            {
                cCoder.Data.Models.Packaging.Package savedItem =
                    item.Id == Guid.Empty
                        ? await AddAsync(entity:item)
                        : await UpdateAsync(entity:item);

                results.Add(item:new Result<cCoder.Data.Models.Packaging.Package>
                {
                    Success = true,
                    Item = savedItem,
                    Message = item.Id == Guid.Empty ? "Added Successfully" : "Updated Successfully"
                });
            }
            catch (Exception ex)
            {
                results.Add(item:new Result<cCoder.Data.Models.Packaging.Package>
                {
                    Success = false,
                    Item = item,
                    Message = ex.Message
                });
            }
        }

        return results;
    }

    public async ValueTask DeleteAllAsync(IEnumerable<cCoder.Data.Models.Packaging.Package> items)
    {
        foreach (cCoder.Data.Models.Packaging.Package item in items)
        {
            await DeleteAsync(id:item.Id);
        }
    }
}