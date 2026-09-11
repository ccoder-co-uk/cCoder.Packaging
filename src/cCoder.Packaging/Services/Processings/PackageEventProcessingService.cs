// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Foundations.Events;

namespace cCoder.Packaging.Services.Processings;

internal sealed partial class PackageEventProcessingService(
    IPackageEventService packageEventService)
    : IPackageEventProcessingService
{
    public ValueTask RaisePackageImportEventAsync(int? appId, Package package) =>
        TryCatch(operation: () =>
        {
            ValidatePackageEventOnImport(appId: appId, package: package);

            return packageEventService
                .RaisePackageImportEventAsync(appId: appId, package: package);
        });

    public ValueTask RaisePackageAddEventAsync(Package package) =>
        TryCatch(operation: () =>
        {
            ValidatePackageEventOnAdd(newPackage: package);

            return packageEventService
                .RaisePackageAddEventAsync(package: package);
        });

    public ValueTask RaisePackageUpdateEventAsync(Package package) =>
        TryCatch(operation: () =>
        {
            ValidatePackageEventOnUpdate(updatedPackage: package);

            return packageEventService
                .RaisePackageUpdateEventAsync(package: package);
        });

    public ValueTask RaisePackageDeleteEventAsync(Package package) =>
        TryCatch(operation: () =>
        {
            ValidatePackageEventOnDelete(deletedPackage: package);

            return packageEventService
                .RaisePackageDeleteEventAsync(package: package);
        });
}