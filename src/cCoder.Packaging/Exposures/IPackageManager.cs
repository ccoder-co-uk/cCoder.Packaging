// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Exposures;

public interface IPackageManager
{
    IEnumerable<Package> ExportPackages(
        int? appId,
        string[] packageNames = null);

    Package GetPackage(Guid packageId);
    IQueryable<Package> GetAllPackages(bool ignoreFilters = false);
    ValueTask<Package> AddPackageAsync(Package newPackage);
    ValueTask<Package> UpdatePackageAsync(Package updatedPackage);
    ValueTask ImportPackageAsync(int? appId, Package package);
    ValueTask DeletePackageAsync(Guid packageId);
    void LogError(Exception exception, string message);
}