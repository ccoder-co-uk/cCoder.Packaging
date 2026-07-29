// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Aggregations;

internal sealed partial class PackageAggregationService
{
    private static void Validate(params object[] inputs)
    {
        foreach (object input in inputs)
        {
            ArgumentNullException.ThrowIfNull(argument: input);
        }
    }

    private static void ValidatePackagesOnExport(int appId, string[] packageNames) =>
        Validate(inputs: appId);

    private static void ValidatePackageOnImport(int appId, Package package) =>
        Validate(inputs: [appId, package]);

    private static void ValidatePackageOnGet(Guid packageId) =>
        Validate(inputs: packageId);

    private static void ValidatePackagesOnGet(bool ignoreFilters) =>
        Validate(inputs: ignoreFilters);

    private static void ValidatePackageOnAdd(Package newPackage) =>
        Validate(inputs: newPackage);

    private static void ValidatePackageOnUpdate(Package updatedPackage) =>
        Validate(inputs: updatedPackage);

    private static void ValidatePackageOnDelete(Guid packageId) =>
        Validate(inputs: packageId);

    private static void ValidatePackagesOnAddOrUpdate(
        IEnumerable<Package> packages) =>
        Validate(inputs: packages);

    private static void ValidatePackagesOnDelete(
        IEnumerable<Package> deletedPackages) =>
        Validate(inputs: deletedPackages);
}