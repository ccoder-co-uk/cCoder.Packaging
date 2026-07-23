// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Dependencies;
using cCoder.Packaging.Models;

namespace cCoder.Packaging.Services.Orchestrations;

public interface IPackageOrchestrationService
{
    IEnumerable<Package> ExportPackages(int appId, string[] packageNames = null);
    ValueTask ImportPackageAsync(int appId, Package package);
    Package GetPackage(Guid packageId);
    IQueryable<Package> GetAllPackages(bool ignoreFilters = false);
    ValueTask<Package> AddPackageAsync(Package newPackage);
    ValueTask<Package> UpdatePackageAsync(Package updatedPackage);
    ValueTask DeletePackageAsync(Guid packageId);

    ValueTask<IEnumerable<Result<Package>>> AddOrUpdatePackagesAsync(
        IEnumerable<Package> packages);

    ValueTask DeleteAllPackagesAsync(IEnumerable<Package> deletedPackages);
}