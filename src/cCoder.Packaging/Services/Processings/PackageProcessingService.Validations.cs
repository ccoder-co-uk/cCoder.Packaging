// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Processings;

internal sealed partial class PackageProcessingService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidatePackageOnExport(int appId, string packageName) =>
        Validate(inputs: [appId, packageName]);

    private static void ValidatePackagesOnExport(int appId, string[] packageNames) =>
        Validate(inputs: [appId, packageNames]);

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
}