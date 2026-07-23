// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.Storages;

namespace cCoder.Packaging.Services.Foundations.Storages;

internal sealed partial class PackageService(
    IPackageBroker packageBroker,
    IAuthorizationBroker authorizationBroker)
    : IPackageService
{
    public Package GetPackage(Guid packageId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnGet(packageId: packageId);

            Package package = SelectAllPackages()
                .FirstOrDefault(predicate: item => item.Id == packageId);

            if (package is not null)
            {
                return package;
            }

            Package unrestrictedPackage = SelectAllPackages(ignoreFilters: true)
                .FirstOrDefault(predicate: item => item.Id == packageId);

            if (unrestrictedPackage is not null)
            {
                throw new SecurityException("Access Denied!");
            }

            return null;
        });

    public IQueryable<Package> GetAllPackages(bool ignoreFilters = false) =>
        TryCatch(operation: () =>
        {
            ValidateAllPackagesOnGet(ignoreFilters: ignoreFilters);

            return SelectAllPackages(ignoreFilters: ignoreFilters);
        });

    public ValueTask<Package> AddPackageAsync(Package newPackage) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageOnAdd(newPackage: newPackage);
            authorizationBroker.Authorize(appId: null, privilege: $"{nameof(Package)}_create");

            Package package = new()
            {
                Name = newPackage.Name,
                Description = newPackage.Description,
                Category = newPackage.Category,
                SourceApi = newPackage.SourceApi,
            };

            Package savedPackage = await packageBroker.AddPackageAsync(newPackage: package);
            newPackage.Id = savedPackage.Id;
            newPackage.Name = savedPackage.Name;
            newPackage.Description = savedPackage.Description;
            newPackage.Category = savedPackage.Category;
            newPackage.SourceApi = savedPackage.SourceApi;

            return newPackage;
        });

    public ValueTask<Package> UpdatePackageAsync(Package updatedPackage) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageOnUpdate(updatedPackage: updatedPackage);
            authorizationBroker.Authorize(appId: null, privilege: $"{nameof(Package)}_update");

            Package package = new()
            {
                Id = updatedPackage.Id,
                Name = updatedPackage.Name,
                Description = updatedPackage.Description,
                Category = updatedPackage.Category,
                SourceApi = updatedPackage.SourceApi,
            };

            Package savedPackage =
                await packageBroker.UpdatePackageAsync(updatedPackage: package);

            updatedPackage.Id = savedPackage.Id;
            updatedPackage.Name = savedPackage.Name;
            updatedPackage.Description = savedPackage.Description;
            updatedPackage.Category = savedPackage.Category;
            updatedPackage.SourceApi = savedPackage.SourceApi;

            return updatedPackage;
        });

    public ValueTask DeletePackageAsync(Guid packageId) =>
        TryCatch(operation: async () =>
        {
            ValidatePackageOnDelete(packageId: packageId);
            Package deletedPackage = SelectPackage(packageId: packageId);
            authorizationBroker.Authorize(appId: null, privilege: $"{nameof(Package)}_delete");
            _ = await packageBroker.DeletePackageAsync(deletedPackage: deletedPackage);
        });

    public Package ExportPackageRoles(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageRolesOnExport(appId: appId);
            EnsureAdmin(appId: appId);

            return packageBroker.ExportRoles(appId: appId);
        });

    public Package ExportPackageLayouts(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageLayoutsOnExport(appId: appId);
            EnsureAdmin(appId: appId);

            return packageBroker.ExportLayouts(appId: appId);
        });

    public Package ExportPackageTemplates(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageTemplatesOnExport(appId: appId);
            EnsureAdmin(appId: appId);

            return packageBroker.ExportTemplates(appId: appId);
        });

    public Package ExportPackageComponents(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageComponentsOnExport(appId: appId);
            EnsureAdmin(appId: appId);

            return packageBroker.ExportComponents(appId: appId);
        });

    public Package ExportPackageScripts(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageScriptsOnExport(appId: appId);
            EnsureAdmin(appId: appId);

            return packageBroker.ExportScripts(appId: appId);
        });

    public Package ExportPackageResources(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageResourcesOnExport(appId: appId);
            EnsureAdmin(appId: appId);

            return packageBroker.ExportResources(appId: appId);
        });

    public Package ExportPackagePages(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackagePagesOnExport(appId: appId);
            EnsureAdmin(appId: appId);

            return packageBroker.ExportPages(appId: appId);
        });

    public Package ExportPackagePageRoles(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackagePageRolesOnExport(appId: appId);
            EnsureAdmin(appId: appId);

            return packageBroker.ExportPageRoles(appId: appId);
        });

    private Package SelectPackage(Guid packageId)
    {
        Package package = SelectAllPackages()
            .FirstOrDefault(predicate: item => item.Id == packageId);

        if (package is not null)
        {
            return package;
        }

        Package unrestrictedPackage = SelectAllPackages(ignoreFilters: true)
            .FirstOrDefault(predicate: item => item.Id == packageId);

        if (unrestrictedPackage is not null)
        {
            throw new SecurityException("Access Denied!");
        }

        return null;
    }

    private IQueryable<Package> SelectAllPackages(bool ignoreFilters = false)
    {
        if (ignoreFilters)
        {
            return packageBroker.GetAllPackagesIgnoringFilters();
        }

        return packageBroker.GetAllPackages();
    }

    private void EnsureAdmin(int appId)
    {
        if (!authorizationBroker.IsAdminOfApp(appId: appId))
        {
            throw new SecurityException("Access Denied!");
        }
    }
}