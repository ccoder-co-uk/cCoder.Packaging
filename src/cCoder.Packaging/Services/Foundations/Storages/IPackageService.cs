// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Foundations.Storages;

public interface IPackageService
{
    Package Get(Guid id);
    IQueryable<Package> GetAll(bool ignoreFilters = false);
    ValueTask<Package> AddAsync(Package package);
    ValueTask<Package> UpdateAsync(Package package);
    ValueTask DeleteAsync(Guid id);
    Package ExportRoles(int appId);
    Package ExportLayouts(int appId);
    Package ExportTemplates(int appId);
    Package ExportComponents(int appId);
    Package ExportScripts(int appId);
    Package ExportResources(int appId);
    Package ExportPages(int appId);
    Package ExportPageRoles(int appId);
}