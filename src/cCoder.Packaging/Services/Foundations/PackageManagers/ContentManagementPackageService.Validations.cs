// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;

namespace cCoder.Packaging.Services.Foundations.PackageManagers;

internal sealed partial class ContentManagementPackageService
{
    private static void Validate(params object[] inputs)
    {
        foreach (object input in inputs)
        {
            ArgumentNullException.ThrowIfNull(argument: input);
        }
    }

    private static void ValidatePackageOnImport(int appId, Package package) =>
        Validate(inputs: [appId, package]);

    private static void ValidatePackageOnExport(int appId, string packageName) =>
        Validate(inputs: [appId, packageName]);
}