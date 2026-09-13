// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Brokers;
using cCoder.Packaging.Brokers.HttpContexts;
using cCoder.Packaging.Models;

namespace cCoder.Packaging.Services.Foundations.PackageExports;

internal sealed partial class PackageExportService(
    IHttpContextBroker httpContextBroker,
    PackagingConfiguration configuration)
    : IPackageExportService
{
    public string GetPackageSourceApi(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageSourceApiOnGet(appId: appId);
            string domain = httpContextBroker.GetRequestDomain();
            string sslPort = configuration.PackageSourceSslPort;

            return $"https://{domain}:{sslPort}/Api/";
        });
}