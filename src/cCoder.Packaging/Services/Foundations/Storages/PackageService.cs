// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Storages;


namespace cCoder.Packaging.Services.Foundations.Storages;

internal class PackageService(IPackageBroker packageBroker, IAuthorizationBroker authorizationBroker)
    : IPackageService
{
    public Package Get(Guid id)
    {
        Package package = GetAll()
                              .FirstOrDefault(predicate:i => i.Id == id);

        if (package is not null)
            return package;

        Package unrestrictedPackage = GetAll(ignoreFilters:true)
                                          .FirstOrDefault(predicate:i => i.Id == id);

        if (unrestrictedPackage is not null)
            throw new SecurityException("Access Denied!");

        return null;
    }

    public IQueryable<Package> GetAll(bool ignoreFilters = false) =>
        packageBroker.GetAllPackages(ignoreFilters:ignoreFilters);

    public async ValueTask<Package> AddAsync(Package package)
    {
        authorizationBroker.Authorize(appId:null, privilege:$"{nameof(Package)}_create");

        Package newPackage = new()
        {
            Name = package.Name,
            Description = package.Description,
            Category = package.Category,
            SourceApi = package.SourceApi,
        };

        Package result = await packageBroker.AddPackageAsync(entity:newPackage);
        package.Id = result.Id;
        package.Name = result.Name;
        package.Description = result.Description;
        package.Category = result.Category;
        package.SourceApi = result.SourceApi;
        return package;
    }

    public async ValueTask<Package> UpdateAsync(Package package)
    {
        authorizationBroker.Authorize(appId:null, privilege:$"{nameof(Package)}_update");

        Package updatePackage = new()
        {
            Id = package.Id,
            Name = package.Name,
            Description = package.Description,
            Category = package.Category,
            SourceApi = package.SourceApi,
        };

        Package result = await packageBroker.UpdatePackageAsync(entity:updatePackage);
        package.Id = result.Id;
        package.Name = result.Name;
        package.Description = result.Description;
        package.Category = result.Category;
        package.SourceApi = result.SourceApi;
        return package;
    }

    public async ValueTask DeleteAsync(Guid id)
    {
        Package package = Get(id:id);
        authorizationBroker.Authorize(appId:null, privilege:$"{nameof(Package)}_delete");
        _ = await packageBroker.DeletePackageAsync(entity:package);
    }

    public Package ExportRoles(int appId)
    {
        EnsureAdmin(appId:appId);
        return packageBroker.ExportRoles(appId:appId);
    }

    public Package ExportLayouts(int appId)
    {
        EnsureAdmin(appId:appId);
        return packageBroker.ExportLayouts(appId:appId);
    }

    public Package ExportTemplates(int appId)
    {
        EnsureAdmin(appId:appId);
        return packageBroker.ExportTemplates(appId:appId);
    }

    public Package ExportComponents(int appId)
    {
        EnsureAdmin(appId:appId);
        return packageBroker.ExportComponents(appId:appId);
    }

    public Package ExportScripts(int appId)
    {
        EnsureAdmin(appId:appId);
        return packageBroker.ExportScripts(appId:appId);
    }

    public Package ExportResources(int appId)
    {
        EnsureAdmin(appId:appId);
        return packageBroker.ExportResources(appId:appId);
    }

    public Package ExportPages(int appId)
    {
        EnsureAdmin(appId:appId);
        return packageBroker.ExportPages(appId:appId);
    }

    public Package ExportPageRoles(int appId)
    {
        EnsureAdmin(appId:appId);
        return packageBroker.ExportPageRoles(appId:appId);
    }

    private void EnsureAdmin(int appId)
    {
        if (!authorizationBroker.IsAdminOfApp(appId:appId))
            throw new SecurityException("Access Denied!");
    }
}