// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers;

namespace cCoder.Packaging.Services.Foundations.PackageManagers;

internal sealed partial class PackageManagerTelemetryService(
    IAuthorizationBroker authorizationBroker,
    IPackageLoggerBroker packageLoggerBroker)
    : IPackageManagerTelemetryService
{
    public void EnsurePackageAdmin(int appId) =>
        TryCatch(operation: () =>
        {
            ValidatePackageAdminOnEnsure(appId: appId);

            if (!authorizationBroker.IsAdminOfApp(appId: appId))
            {
                throw new SecurityException("Access Denied!");
            }
        });

    public void LogPackageItemImport(
        PackageItem packageItem,
        string packageSource) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemImportOnLog(
                packageItem: packageItem,
                packageSource: packageSource);

            packageLoggerBroker.LogPackageItemImport(
                packageItem: packageItem,
                packageSource: packageSource);
        });
}