// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Foundations.Events;

internal sealed partial class PackageEventService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidatePackageEventOnImport(int appId, Package package) =>
        Validate(inputs: [appId, package]);

    private static void ValidatePackageEventOnAdd(Package newPackage) =>
        Validate(inputs: newPackage);

    private static void ValidatePackageEventOnUpdate(Package updatedPackage) =>
        Validate(inputs: updatedPackage);

    private static void ValidatePackageEventOnDelete(Package deletedPackage) =>
        Validate(inputs: deletedPackage);
}