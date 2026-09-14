// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Brokers;
using cCoder.Packaging.Models;
using cCoder.Packaging.Brokers.PackageManagers;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Foundations.PackageManagers;

internal interface IDocumentManagementPackageService
{
    ValueTask ImportPackageAsync(int appId, Package package);

    Package ExportPackage(int appId, string packageName);
}

internal sealed partial class DocumentManagementPackageService(
    IDocumentManagementPackageBroker documentManagementPackageBroker
) : IDocumentManagementPackageService
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnImport(appId: appId, package: package);

            return documentManagementPackageBroker
                .ImportPackageAsync(appId: appId, package: package);
        });

    public Package ExportPackage(int appId, string packageName) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnExport(appId: appId, packageName: packageName);

            return documentManagementPackageBroker
                .ExportPackage(appId: appId, packageName: packageName);
        });
}