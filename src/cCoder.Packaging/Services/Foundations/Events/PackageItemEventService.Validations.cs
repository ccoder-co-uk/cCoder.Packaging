// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Foundations.Events;

internal sealed partial class PackageItemEventService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidatePackageItemEventOnAdd(PackageItem newPackageItem) =>
        Validate(inputs: newPackageItem);

    private static void ValidatePackageItemEventOnUpdate(PackageItem updatedPackageItem) =>
        Validate(inputs: updatedPackageItem);

    private static void ValidatePackageItemEventOnDelete(PackageItem deletedPackageItem) =>
        Validate(inputs: deletedPackageItem);
}