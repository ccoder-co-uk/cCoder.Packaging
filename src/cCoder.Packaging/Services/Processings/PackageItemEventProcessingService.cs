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
    public ValueTask RaisePackageItemAddEventAsync(PackageItem packageItem) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemEventOnAdd(newPackageItem: packageItem);

            return packageItemEventService
                .RaisePackageItemAddEventAsync(packageItem: packageItem);
        });

    public ValueTask RaisePackageItemUpdateEventAsync(PackageItem packageItem) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemEventOnUpdate(updatedPackageItem: packageItem);

            return packageItemEventService
                .RaisePackageItemUpdateEventAsync(packageItem: packageItem);
        });

    public ValueTask RaisePackageItemDeleteEventAsync(PackageItem packageItem) =>
        TryCatch(operation: () =>
        {
            ValidatePackageItemEventOnDelete(deletedPackageItem: packageItem);

            return packageItemEventService
                .RaisePackageItemDeleteEventAsync(packageItem: packageItem);
        });
}