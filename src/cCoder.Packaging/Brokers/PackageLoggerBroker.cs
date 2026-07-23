// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Aggregations;

namespace cCoder.Packaging.Brokers;

internal sealed class PackageLoggerBroker(
    ILogger<PackageManagerAggregationService> logger)
    : IPackageLoggerBroker
{
    public void LogPackageItemImport(
        PackageItem packageItem,
        string packageSource) =>
        logger.LogDebug(
            message: "Importing {ItemType} items from {PackageSource}",
            args:
            [
                packageItem.Type,
                packageSource,
            ]);
}