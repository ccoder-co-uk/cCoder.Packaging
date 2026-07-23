// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Foundations.Events;

namespace cCoder.Packaging.Services.Processings;

internal sealed partial class PackageItemEventProcessingService(
    IPackageItemEventService packageItemEventService)
    : IPackageItemEventProcessingService
{
    public ValueTask RaisePackageItemAddEventAsync(PackageItem newPackageItem) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemEventOnAdd(newPackageItem: newPackageItem);

            return packageItemEventService
                .RaisePackageItemAddEventAsync(entity: newPackageItem);
        });

    public ValueTask RaisePackageItemUpdateEventAsync(PackageItem updatedPackageItem) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemEventOnUpdate(updatedPackageItem: updatedPackageItem);

            return packageItemEventService
                .RaisePackageItemUpdateEventAsync(entity: updatedPackageItem);
        });

    public ValueTask RaisePackageItemDeleteEventAsync(PackageItem deletedPackageItem) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemEventOnDelete(deletedPackageItem: deletedPackageItem);

            return packageItemEventService
                .RaisePackageItemDeleteEventAsync(entity: deletedPackageItem);
        });
}