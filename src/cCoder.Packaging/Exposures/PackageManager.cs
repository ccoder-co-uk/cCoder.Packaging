// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers.Loggings;
using cCoder.Packaging.Services.Aggregations;

namespace cCoder.Packaging.Exposures;

internal sealed class PackageManager(
    IPackageAggregationService packageAggregationService,
    ILoggingBroker loggingBroker)
    : IPackageManager
{
    public ValueTask<Package[]> ExportPackagesAsync(
        int? appId,
        string[] packageNames = null) =>
        packageAggregationService.ExportPackagesAsync(
            appId: appId,
            packageNames: packageNames);

    public Package GetPackage(Guid packageId) =>
        packageAggregationService.GetPackage(packageId: packageId);

    public IQueryable<Package> GetAllPackages(bool ignoreFilters = false) =>
        packageAggregationService.GetAllPackages(ignoreFilters: ignoreFilters);

    public ValueTask<Package> AddPackageAsync(Package newPackage) =>
        packageAggregationService.AddPackageAsync(newPackage: newPackage);

    public ValueTask<Package> UpdatePackageAsync(Package updatedPackage) =>
        packageAggregationService.UpdatePackageAsync(
            updatedPackage: updatedPackage);

    public ValueTask ImportPackageAsync(int? appId, Package package) =>
        packageAggregationService.ImportPackageAsync(
            appId: appId,
            package: package);

    public ValueTask DeletePackageAsync(Guid packageId) =>
        packageAggregationService.DeletePackageAsync(packageId: packageId);

    public void LogError(Exception exception, string message) =>
        loggingBroker.LogError(exception: exception, message: message);
}