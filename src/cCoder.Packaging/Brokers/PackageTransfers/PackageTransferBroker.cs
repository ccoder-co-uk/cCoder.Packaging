// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Exposures;

namespace cCoder.Packaging.Brokers.PackageTransfers;

internal sealed class PackageTransferBroker(
    IPackageTransferManager packageTransferManager)
    : IPackageTransferBroker
{
    public ValueTask<Package[]> ExportPackagesAsync(
        int appId,
        string[] packageNames,
        string sourceApi) =>
        packageTransferManager.ExportPackagesAsync(
            appId: appId,
            packageNames: packageNames,
            sourceApi: sourceApi);
}