// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Foundations.PackageManagers;

internal sealed partial class PackageManagerTelemetryService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidatePackageAdminOnEnsure(int appId) =>
        Validate(inputs: appId);

    private static void ValidatePackageItemImportOnLog(
        PackageItem packageItem,
        string packageSource) =>
        Validate(inputs: [packageItem, packageSource]);
}