// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Foundations.PackageExports;

namespace cCoder.Packaging.Services.Processings;

internal sealed partial class PackageExportProcessingService(
    IPackageExportService packageExportService)
    : IPackageExportProcessingService
{
    public ValueTask<Package[]> ExportPackagesAsync(
        int appId,
        string[] packageNames,
        string sourceApi) =>
        TryCatch(operation: () =>
        {
            ValidatePackagesOnExport(
                appId: appId,
                packageNames: packageNames,
                sourceApi: sourceApi);

            return packageExportService.ExportPackagesAsync(
                appId: appId,
                packageNames: packageNames,
                sourceApi: sourceApi);
        });

    public string GetPackageSourceApi(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageSourceApiOnGet(appId: appId);

            return packageExportService.GetPackageSourceApi(appId: appId);
        });
}