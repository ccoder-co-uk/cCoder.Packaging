// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Extensions;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Foundations.Storages;

namespace cCoder.Packaging.Services.Processings;

internal sealed partial class PackageProcessingService(
    IPackageService packageService)
    : IPackageProcessingService
{
    public Package ExportPackage(int appId, string packageName) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnExport(appId: appId, packageName: packageName);

            return packageName switch
            {
                "Roles" => packageService.ExportPackageRoles(appId: appId),
                "Layouts" => packageService.ExportPackageLayouts(appId: appId),
                "Templates" => packageService.ExportPackageTemplates(appId: appId),
                "Components" => packageService.ExportPackageComponents(appId: appId),
                "Scripts" => packageService.ExportPackageScripts(appId: appId),
                "Resources" => packageService.ExportPackageResources(appId: appId),
                "Pages" => packageService.ExportPackagePages(appId: appId),
                "PageRoles" => packageService.ExportPackagePageRoles(appId: appId),
                _ => new Package
                {
                    Name = packageName,
                    Items = [],
                },
            };
        });

    public Package[] ExportPackages(int appId, string[] packageNames) =>
        TryCatch(operation: () =>
        {
            ValidatePackagesOnExport(appId: appId, packageNames: packageNames);

            return packageNames
                .Select(selector: packageName => ExportPackageValue(
                    appId: appId,
                    packageName: packageName))
                .ToArray();
        });

    public Package GetPackage(Guid packageId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnGet(packageId: packageId);

            return packageService.GetPackage(packageId: packageId);
        });

    public IQueryable<Package> GetAllPackages(bool ignoreFilters = false) =>
        TryCatch(operation: () =>
        {
            ValidatePackagesOnGet(ignoreFilters: ignoreFilters);

            return packageService.GetAllPackages(ignoreFilters: ignoreFilters);
        });

    public ValueTask<Package> AddPackageAsync(Package newPackage) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnAdd(newPackage: newPackage);
            ClearPackageItems(package: newPackage);

            return packageService.AddPackageAsync(newPackage: newPackage);
        });

    public ValueTask<Package> UpdatePackageAsync(Package updatedPackage) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnUpdate(updatedPackage: updatedPackage);

            return packageService.UpdatePackageAsync(updatedPackage: updatedPackage);
        });

    public ValueTask DeletePackageAsync(Guid packageId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnDelete(packageId: packageId);

            return packageService.DeletePackageAsync(packageId: packageId);
        });

    private Package ExportPackageValue(int appId, string packageName) =>
        packageName switch
        {
            "Roles" => packageService.ExportPackageRoles(appId: appId),
            "Layouts" => packageService.ExportPackageLayouts(appId: appId),
            "Templates" => packageService.ExportPackageTemplates(appId: appId),
            "Components" => packageService.ExportPackageComponents(appId: appId),
            "Scripts" => packageService.ExportPackageScripts(appId: appId),
            "Resources" => packageService.ExportPackageResources(appId: appId),
            "Pages" => packageService.ExportPackagePages(appId: appId),
            "PageRoles" => packageService.ExportPackagePageRoles(appId: appId),
            _ => new Package
            {
                Name = packageName,
                Items = [],
            },
        };

    private static void ClearPackageItems(Package package)
    {
        if (package.Items is not null)
        {
            package.Items.ForEach(action: packageItem =>
            {
                packageItem.PackageId = package.Id;
                packageItem.Package = null;
            });
        }
    }
}