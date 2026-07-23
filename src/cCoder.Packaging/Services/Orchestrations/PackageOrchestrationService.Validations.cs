// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Orchestrations;

internal sealed partial class PackageOrchestrationService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

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