// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Foundations.PackageManagers;

internal sealed partial class PackageManagerTelemetryService
{
    private static void Validate(params object[] inputs)
    {
        foreach (object input in inputs)
        {
            ArgumentNullException.ThrowIfNull(argument: input);
        }
    }

    private static void ValidatePackageAdminOnEnsure(int appId) =>
        Validate(inputs: appId);

    private static void ValidatePackageItemImportOnLog(
        PackageItem packageItem,
        string packageSource) =>
        Validate(inputs: [packageItem, packageSource]);
}