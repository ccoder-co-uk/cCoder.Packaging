using cCoder.Packaging.Brokers;
using cCoder.Packaging.Models;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services;

public interface ISchedulingPackageService
{
    ValueTask ImportPackageAsync(int appId, Package package);

    Package ExportPackage(int appId, string packageName);
}

internal class SchedulingPackageService(ISchedulingPackageManagerBroker schedulingPackageManagerBroker)
    : ISchedulingPackageService
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        schedulingPackageManagerBroker.ImportPackageAsync(appId, package);

    public Package ExportPackage(int appId, string packageName) =>
        schedulingPackageManagerBroker.ExportPackage(appId, packageName);
}


