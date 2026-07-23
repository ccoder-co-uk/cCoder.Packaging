// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Brokers;
using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Foundations.PackageManagers;

public interface IDocumentManagementPackageService
{
    ValueTask ImportPackageAsync(int appId, Package package);

    Package ExportPackage(int appId, string packageName);
}

internal sealed partial class DocumentManagementPackageService(
    IDocumentManagementPackageManagerBroker documentManagementPackageManagerBroker
) : IDocumentManagementPackageService
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnImport(appId: appId, package: package);

            return documentManagementPackageManagerBroker
                .ImportPackageAsync(appId: appId, package: package);
        });

    public Package ExportPackage(int appId, string packageName) =>
        TryCatch(operation: () =>
        {
            ValidatePackageOnExport(appId: appId, packageName: packageName);

            return documentManagementPackageManagerBroker
                .ExportPackage(appId: appId, packageName: packageName);
        });
}