// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Aggregations;

namespace cCoder.Packaging.Exposures;

internal sealed class PackageManager(
    IPackageAggregationService packageAggregationService)
    : IPackageManager
{
    public Package GetPackage(Guid packageId) =>
        packageAggregationService.GetPackage(packageId: packageId);

    public IQueryable<Package> GetAllPackages(bool ignoreFilters = false) =>
        packageAggregationService.GetAllPackages(ignoreFilters: ignoreFilters);

    public ValueTask<Package> AddPackageAsync(Package newPackage) =>
        packageAggregationService.AddPackageAsync(newPackage: newPackage);

    public ValueTask<Package> UpdatePackageAsync(Package updatedPackage) =>
        packageAggregationService.UpdatePackageAsync(
            updatedPackage: updatedPackage);

    public ValueTask DeletePackageAsync(Guid packageId) =>
        packageAggregationService.DeletePackageAsync(packageId: packageId);
}