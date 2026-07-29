// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Exposures;

public interface IPackageManager
{
    Package GetPackage(Guid packageId);
    IQueryable<Package> GetAllPackages(bool ignoreFilters = false);
    ValueTask<Package> AddPackageAsync(Package newPackage);
    ValueTask<Package> UpdatePackageAsync(Package updatedPackage);
    ValueTask DeletePackageAsync(Guid packageId);
}