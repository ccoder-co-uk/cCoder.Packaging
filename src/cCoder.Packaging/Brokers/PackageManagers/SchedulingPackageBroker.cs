// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Exposures.PackageManagers;

namespace cCoder.Packaging.Brokers.PackageManagers;

internal interface ISchedulingPackageBroker
{
    ValueTask ImportPackageAsync(int appId, Package package);
    Package ExportPackage(int appId, string packageName);
}

internal sealed class SchedulingPackageBroker(
    ISchedulingPackageManager schedulingPackageManager)
    : ISchedulingPackageBroker
{
    public ValueTask ImportPackageAsync(int appId, Package package) =>
        schedulingPackageManager.ImportPackageAsync(
            appId: appId,
            package: package);

    public Package ExportPackage(int appId, string packageName) =>
        schedulingPackageManager.ExportPackage(
            appId: appId,
            packageName: packageName);
}