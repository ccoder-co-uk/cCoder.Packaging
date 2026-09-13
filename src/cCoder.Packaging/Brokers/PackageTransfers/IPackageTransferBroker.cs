// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Brokers.PackageTransfers;

internal interface IPackageTransferBroker
{
    ValueTask<Package[]> ExportPackagesAsync(
        int appId,
        string[] packageNames,
        string sourceApi);
}