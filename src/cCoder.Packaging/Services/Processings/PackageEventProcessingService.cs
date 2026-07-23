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
    public ValueTask RaisePackageImportEventAsync(int appId, Package package) =>
        TryCatch(operation: () =>
        {
            ValidatePackageEventOnImport(appId: appId, package: package);

            return packageEventService
                .RaisePackageImportEventAsync(appId: appId, package: package);
        });

    public ValueTask RaisePackageAddEventAsync(Package newPackage) =>
        TryCatch(operation: () =>
        {
            ValidatePackageEventOnAdd(newPackage: newPackage);

            return packageEventService
                .RaisePackageAddEventAsync(entity: newPackage);
        });

    public ValueTask RaisePackageUpdateEventAsync(Package updatedPackage) =>
        TryCatch(operation: () =>
        {
            ValidatePackageEventOnUpdate(updatedPackage: updatedPackage);

            return packageEventService
                .RaisePackageUpdateEventAsync(entity: updatedPackage);
        });

    public ValueTask RaisePackageDeleteEventAsync(Package deletedPackage) =>
        TryCatch(operation: () =>
        {
            ValidatePackageEventOnDelete(deletedPackage: deletedPackage);

            return packageEventService
                .RaisePackageDeleteEventAsync(entity: deletedPackage);
        });
}
