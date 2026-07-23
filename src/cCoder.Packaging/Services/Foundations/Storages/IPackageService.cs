// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Foundations.Storages;

internal interface IPackageService
{
    Package GetPackage(Guid packageId);
    IQueryable<Package> GetAllPackages(bool ignoreFilters = false);
    ValueTask<Package> AddPackageAsync(Package newPackage);
    ValueTask<Package> UpdatePackageAsync(Package updatedPackage);
    ValueTask DeletePackageAsync(Guid packageId);
    Package ExportPackageRoles(int appId);
    Package ExportPackageLayouts(int appId);
    Package ExportPackageTemplates(int appId);
    Package ExportPackageComponents(int appId);
    Package ExportPackageScripts(int appId);
    Package ExportPackageResources(int appId);
    Package ExportPackagePages(int appId);
    Package ExportPackagePageRoles(int appId);
}