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
            IDictionary<string, string> settings = configProvider.GetSettings();

            string sslPort = settings.TryGetValue(
                key: "sslPort",
                value: out string configuredSslPort)
                    ? configuredSslPort
                    : "443";

            return $"https://{domain}:{sslPort}/Api/";
        });
}