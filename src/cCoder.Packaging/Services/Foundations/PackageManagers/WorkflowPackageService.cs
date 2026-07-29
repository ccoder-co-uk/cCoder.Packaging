// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Brokers;
using cCoder.Packaging.Models;
using cCoder.Packaging.Exposures.PackageManagers;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Foundations.PackageManagers;

internal interface IWorkflowPackageService
{
    ValueTask ImportPackageAsync(int appId, Package package);

    Package ExportPackage(int appId, string packageName);
}

internal sealed partial class WorkflowPackageService(
    IWorkflowPackageManager workflowPackageManager)
    : IWorkflowPackageService
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnImport(appId: appId, package: package);

            return workflowPackageManager
                .ImportPackageAsync(appId: appId, package: package);
        });

    public Package ExportPackage(int appId, string packageName) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnExport(appId: appId, packageName: packageName);

            return workflowPackageManager
                .ExportPackage(appId: appId, packageName: packageName);
        });
}