// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Foundations.Events;

internal sealed partial class PackageItemEventService
{
    private static void Validate(params object[] inputs)
    {
        foreach (object input in inputs)
        {
            ArgumentNullException.ThrowIfNull(argument: input);
        }
    }

    private static void ValidatePackageItemEventOnAdd(PackageItem newPackageItem) =>
        Validate(inputs: newPackageItem);

    private static void ValidatePackageItemEventOnUpdate(PackageItem updatedPackageItem) =>
        Validate(inputs: updatedPackageItem);

    private static void ValidatePackageItemEventOnDelete(PackageItem deletedPackageItem) =>
        Validate(inputs: deletedPackageItem);
}