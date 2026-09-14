// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Exposures.PackageManagers;

namespace cCoder.Packaging.Brokers.PackageManagers;

internal interface IAppSecurityPackageBroker
{
    ValueTask ImportPackageAsync(int appId, Package package);
    Package ExportPackage(int appId, string packageName);
}

internal sealed class AppSecurityPackageBroker(
    IAppSecurityPackageManager appSecurityPackageManager)
    : IAppSecurityPackageBroker
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        appSecurityPackageManager.ImportPackageAsync(
            appId: appId,
            package: package);

    public Package ExportPackage(int appId, string packageName) =>
        appSecurityPackageManager.ExportPackage(
            appId: appId,
            packageName: packageName);
}