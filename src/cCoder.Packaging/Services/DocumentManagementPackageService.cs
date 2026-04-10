using cCoder.Packaging.Brokers;
using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services;

public interface IDocumentManagementPackageService
{
    ValueTask ImportPackageAsync(int appId, Package package);

    Package ExportPackage(int appId, string packageName);
}

internal class DocumentManagementPackageService(
    IDocumentManagementPackageManagerBroker documentManagementPackageManagerBroker
) : IDocumentManagementPackageService
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        documentManagementPackageManagerBroker.ImportPackageAsync(appId, package);

    public Package ExportPackage(int appId, string packageName) =>
        documentManagementPackageManagerBroker.ExportPackage(appId, packageName);
}


