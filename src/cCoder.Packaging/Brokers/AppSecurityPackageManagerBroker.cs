using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Brokers;

public interface IAppSecurityPackageManagerBroker
{
    ValueTask ImportPackageAsync(int appId, Package package);
    Package ExportPackage(int appId, string packageName);
}


