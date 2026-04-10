using cCoder.Packaging.Brokers;
using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services;

public interface IAppSecurityPackageService
{
    ValueTask ImportPackageAsync(int appId, Package package);
    Package ExportPackage(int appId, string packageName);
}

internal class AppSecurityPackageService(IAppSecurityPackageManagerBroker appSecurityPackageManagerBroker)
    : IAppSecurityPackageService
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        appSecurityPackageManagerBroker.ImportPackageAsync(appId, package);

    public Package ExportPackage(int appId, string packageName) =>
        appSecurityPackageManagerBroker.ExportPackage(appId, packageName);
}


