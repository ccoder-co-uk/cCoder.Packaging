// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Brokers;
using cCoder.Packaging.Exposures.PackageManagers;
using cCoder.Packaging.Models;

namespace cCoder.Packaging.Services.Foundations.PackageExports;

internal sealed partial class PackageExportService(
    IAppDomainManager appDomainManager,
    PackagingConfiguration configuration)
    : IPackageExportService
{
    public string GetPackageSourceApi(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageSourceApiOnGet(appId: appId);
            string domain = appDomainManager.GetDomain(appId: appId);
            string sslPort = configuration.PackageSourceSslPort;

            return $"https://{domain}:{sslPort}/Api/";
        });
}