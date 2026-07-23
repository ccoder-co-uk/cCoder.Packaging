// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Brokers;
using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Foundations.PackageManagers;

public interface IAppSecurityPackageService
{
    ValueTask ImportPackageAsync(int appId, Package package);
    Package ExportPackage(int appId, string packageName);
}

internal sealed partial class AppSecurityPackageService(
    IAppSecurityPackageManagerBroker appSecurityPackageManagerBroker)
    : IAppSecurityPackageService
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnImport(appId: appId, package: package);

            return appSecurityPackageManagerBroker
                .ImportPackageAsync(appId: appId, package: package);
        });

    public Package ExportPackage(int appId, string packageName) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnExport(appId: appId, packageName: packageName);

            return appSecurityPackageManagerBroker
                .ExportPackage(appId: appId, packageName: packageName);
        });
}