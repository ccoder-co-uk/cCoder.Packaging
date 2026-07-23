// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Foundations.Storages;

internal sealed partial class PackageService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidatePackageOnGet(Guid packageId) =>
        Validate(inputs: packageId);

    private static void ValidateAllPackagesOnGet(bool ignoreFilters) =>
        Validate(inputs: ignoreFilters);

    private static void ValidatePackageOnAdd(Package newPackage) =>
        Validate(inputs: newPackage);

    private static void ValidatePackageOnUpdate(Package updatedPackage) =>
        Validate(inputs: updatedPackage);

    private static void ValidatePackageOnDelete(Guid packageId) =>
        Validate(inputs: packageId);

    private static void ValidatePackageRolesOnExport(int appId) =>
        Validate(inputs: appId);

    private static void ValidatePackageLayoutsOnExport(int appId) =>
        Validate(inputs: appId);

    private static void ValidatePackageTemplatesOnExport(int appId) =>
        Validate(inputs: appId);

    private static void ValidatePackageComponentsOnExport(int appId) =>
        Validate(inputs: appId);

    private static void ValidatePackageScriptsOnExport(int appId) =>
        Validate(inputs: appId);

    private static void ValidatePackageResourcesOnExport(int appId) =>
        Validate(inputs: appId);

    private static void ValidatePackagePagesOnExport(int appId) =>
        Validate(inputs: appId);

    private static void ValidatePackagePageRolesOnExport(int appId) =>
        Validate(inputs: appId);
}