// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Exposures.PackageManagers;

namespace cCoder.Packaging.Brokers.PackageManagers;

internal interface IDocumentManagementPackageBroker
{
    ValueTask ImportPackageAsync(int appId, Package package);
    Package ExportPackage(int appId, string packageName);
}

internal sealed class DocumentManagementPackageBroker(
    IDocumentManagementPackageManager documentManagementPackageManager)
    : IDocumentManagementPackageBroker
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        documentManagementPackageManager.ImportPackageAsync(
            appId: appId,
            package: package);

    public Package ExportPackage(int appId, string packageName) =>
        documentManagementPackageManager.ExportPackage(
            appId: appId,
            packageName: packageName);
}