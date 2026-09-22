// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Brokers;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers.PackageTransfers;
using cCoder.Packaging.Models;

namespace cCoder.Packaging.Services.Foundations.PackageExports;

internal sealed partial class PackageExportService(
    IPackageTransferBroker packageTransferBroker,
    PackagingConfiguration configuration)
    : IPackageExportService
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

            return packageTransferBroker.ExportPackagesAsync(
                appId: appId,
                packageNames: packageNames,
                sourceApi: sourceApi);
        });

    public string GetPackageSourceApi(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageSourceApiOnGet(appId: appId);
            string domain = packageTransferBroker.GetRequestDomain();
            string sslPort = configuration.PackageSourceSslPort;

            return $"https://{domain}:{sslPort}/Api/";
        });
}