// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Foundations.PackageExports;

internal interface IPackageExportService
{
    ValueTask<Package[]> ExportPackagesAsync(
        int appId,
        string[] packageNames,
        string sourceApi);

    string GetPackageSourceApi(int appId);
}