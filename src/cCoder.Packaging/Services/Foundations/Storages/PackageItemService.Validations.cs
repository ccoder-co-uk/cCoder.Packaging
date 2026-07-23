// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Foundations.Storages;

internal sealed partial class PackageItemService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

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
}