// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Services.Foundations.PackageExports;

namespace cCoder.Packaging.Services.Processings;

internal sealed partial class PackageExportProcessingService(
    IPackageExportService packageExportService)
    : IPackageExportProcessingService
{
    public string GetPackageSourceApi(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageSourceApiOnGet(appId: appId);

            return packageExportService.GetPackageSourceApi(appId: appId);
        });
}