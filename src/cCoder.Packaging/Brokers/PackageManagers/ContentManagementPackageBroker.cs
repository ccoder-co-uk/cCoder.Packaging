// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Exposures.PackageManagers;

namespace cCoder.Packaging.Brokers.PackageManagers;

internal interface IContentManagementPackageBroker
{
    ValueTask ImportPackageAsync(int appId, Package package);
    Package ExportPackage(int appId, string packageName);
}

internal sealed class ContentManagementPackageBroker(
    IContentManagementPackageManager contentManagementPackageManager)
    : IContentManagementPackageBroker
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        contentManagementPackageManager.ImportPackageAsync(
            appId: appId,
            package: package);

    public Package ExportPackage(int appId, string packageName) =>
        contentManagementPackageManager.ExportPackage(
            appId: appId,
            packageName: packageName);
}