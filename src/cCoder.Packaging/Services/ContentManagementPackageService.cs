using cCoder.Packaging.Brokers;
using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services;

public interface IContentManagementPackageService
{
    ValueTask ImportPackageAsync(int appId, Package package);

    Package ExportPackage(int appId, string packageName);
}

internal class ContentManagementPackageService(
    IContentManagementPackageManagerBroker contentManagementPackageManagerBroker
) : IContentManagementPackageService
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        contentManagementPackageManagerBroker.ImportPackageAsync(appId, package);

    public Package ExportPackage(int appId, string packageName) =>
        contentManagementPackageManagerBroker.ExportPackage(appId, packageName);
}


