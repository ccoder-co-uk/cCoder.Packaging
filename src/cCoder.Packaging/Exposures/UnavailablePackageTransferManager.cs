// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Exposures;

internal sealed class UnavailablePackageTransferManager
    : IPackageTransferManager
{
    public ValueTask<Package[]> ExportPackagesAsync(
        int appId,
        string[] packageNames,
        string sourceApi) =>
        throw new NotSupportedException(
            "App package export requires a composition-root package transfer manager.");
}