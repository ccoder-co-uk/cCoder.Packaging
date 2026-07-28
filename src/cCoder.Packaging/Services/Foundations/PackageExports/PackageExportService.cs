// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Brokers;
using cCoder.Packaging.Exposures.Configuration;

namespace cCoder.Packaging.Services.Foundations.PackageExports;

internal sealed partial class PackageExportService(
    IAppDomainProvider appDomainProvider,
    IConfigProvider configProvider)
    : IPackageExportService
{
    public string GetPackageSourceApi(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageSourceApiOnGet(appId: appId);
            string domain = appDomainProvider.GetDomain(appId: appId);
            string sslPort = configProvider.GetPackageSourceSslPort();

            return $"https://{domain}:{sslPort}/Api/";
        });
}