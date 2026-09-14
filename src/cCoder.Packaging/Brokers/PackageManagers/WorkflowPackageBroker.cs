// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Exposures.PackageManagers;

namespace cCoder.Packaging.Brokers.PackageManagers;

internal interface IWorkflowPackageBroker
{
    ValueTask ImportPackageAsync(int appId, Package package);
    Package ExportPackage(int appId, string packageName);
}

internal sealed class WorkflowPackageBroker(
    IWorkflowPackageManager workflowPackageManager)
    : IWorkflowPackageBroker
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        workflowPackageManager.ImportPackageAsync(
            appId: appId,
            package: package);

    public Package ExportPackage(int appId, string packageName) =>
        workflowPackageManager.ExportPackage(
            appId: appId,
            packageName: packageName);
}