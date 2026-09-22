// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Orchestrations;

internal sealed partial class PackageItemOrchestrationService
{
    private static void Validate(params object[] inputs)
    {
        foreach (object input in inputs)
        {
            ArgumentNullException.ThrowIfNull(argument: input);
        }
    }

    private static void ValidatePackageItemOnGet(Guid packageItemId) =>
        Validate(inputs: packageItemId);

    private static void ValidateAllPackageItemsOnGet(bool ignoreFilters) =>
        Validate(inputs: ignoreFilters);

    private static void ValidatePackageItemOnAdd(PackageItem newPackageItem) =>
        Validate(inputs: newPackageItem);

    private static void ValidatePackageItemOnUpdate(PackageItem updatedPackageItem) =>
        Validate(inputs: updatedPackageItem);

    private static void ValidatePackageItemOnDelete(Guid packageItemId) =>
        Validate(inputs: packageItemId);

    private static void ValidateOrUpdatePackageItemsOnAdd(
        IEnumerable<PackageItem> packageItems) =>
        Validate(inputs: packageItems);

    private static void ValidateAllPackageItemsOnDelete(
        IEnumerable<PackageItem> deletedPackageItems) =>
        Validate(inputs: deletedPackageItems);
}