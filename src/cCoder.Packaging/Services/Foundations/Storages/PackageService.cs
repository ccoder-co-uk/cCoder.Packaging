using System.Security;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers.Storages;
using IAuthorizationBroker = cCoder.Packaging.Brokers.IAuthorizationBroker;


namespace cCoder.Packaging.Services.Foundations.Storages;

internal class PackageService(IPackageBroker packageBroker, IAuthorizationBroker authorizationBroker)
    : IPackageService
{
    public Package Get(Guid id)
    {
        Package package = GetAll().FirstOrDefault(i => i.Id == id);
        if (package is not null)
            return package;

        Package unrestrictedPackage = GetAll(true).FirstOrDefault(i => i.Id == id);

        if (unrestrictedPackage is not null)
            throw new SecurityException("Access Denied!");

        return null;
    }

    public IQueryable<Package> GetAll(bool ignoreFilters = false) =>
        packageBroker.GetAllPackages(ignoreFilters);

    public async ValueTask<Package> AddAsync(Package package)
    {
        authorizationBroker.Authorize(null, $"{nameof(Package)}_create");
        Package newPackage = new()
        {
            Name = package.Name,
            Description = package.Description,
            Category = package.Category,
            SourceApi = package.SourceApi,
        };

        Package result = await packageBroker.AddPackageAsync(newPackage);
        package.Id = result.Id;
        package.Name = result.Name;
        package.Description = result.Description;
        package.Category = result.Category;
        package.SourceApi = result.SourceApi;
        return package;
    }

    public async ValueTask<Package> UpdateAsync(Package package)
    {
        authorizationBroker.Authorize(null, $"{nameof(Package)}_update");
        Package updatePackage = new()
        {
            Id = package.Id,
            Name = package.Name,
            Description = package.Description,
            Category = package.Category,
            SourceApi = package.SourceApi,
        };

        Package result = await packageBroker.UpdatePackageAsync(updatePackage);
        package.Id = result.Id;
        package.Name = result.Name;
        package.Description = result.Description;
        package.Category = result.Category;
        package.SourceApi = result.SourceApi;
        return package;
    }

    public async ValueTask DeleteAsync(Guid id)
    {
        Package package = Get(id);
        authorizationBroker.Authorize(null, $"{nameof(Package)}_delete");
        _ = await packageBroker.DeletePackageAsync(package);
    }

    public Package ExportRoles(int appId)
    {
        EnsureAdmin(appId);
        return packageBroker.ExportRoles(appId);
    }

    public Package ExportLayouts(int appId)
    {
        EnsureAdmin(appId);
        return packageBroker.ExportLayouts(appId);
    }

    public Package ExportTemplates(int appId)
    {
        EnsureAdmin(appId);
        return packageBroker.ExportTemplates(appId);
    }

    public Package ExportComponents(int appId)
    {
        EnsureAdmin(appId);
        return packageBroker.ExportComponents(appId);
    }

    public Package ExportScripts(int appId)
    {
        EnsureAdmin(appId);
        return packageBroker.ExportScripts(appId);
    }

    public Package ExportResources(int appId)
    {
        EnsureAdmin(appId);
        return packageBroker.ExportResources(appId);
    }

    public Package ExportPages(int appId)
    {
        EnsureAdmin(appId);
        return packageBroker.ExportPages(appId);
    }

    public Package ExportPageRoles(int appId)
    {
        EnsureAdmin(appId);
        return packageBroker.ExportPageRoles(appId);
    }

    private void EnsureAdmin(int appId)
    {
        if (!authorizationBroker.IsAdminOfApp(appId))
            throw new SecurityException("Access Denied!");
    }
}

