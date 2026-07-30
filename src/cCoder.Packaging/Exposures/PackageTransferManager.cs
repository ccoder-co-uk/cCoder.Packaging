// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Aggregations;

namespace cCoder.Packaging.Exposures;

internal sealed class PackageTransferManager(
    IPackageManagerAggregationService packageManagerAggregationService)
    : IPackageTransferManager
{
    public Package ExportPackage(int appId, string packageName) =>
        packageManagerAggregationService.ExportPackage(
            appId: appId,
            packageName: packageName);

    public ValueTask ImportPackageAsync(int appId, Package package) =>
        packageManagerAggregationService.ImportPackageAsync(
            appId: appId,
            package: package);
}