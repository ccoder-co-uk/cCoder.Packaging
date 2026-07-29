// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------


namespace cCoder.Packaging.Services.Foundations.PackageExports;

internal sealed partial class PackageExportService
{
    private static void Validate(params object[] inputs)
    {
        foreach (object input in inputs)
        {
            ArgumentNullException.ThrowIfNull(argument: input);
        }
    }

    private static void ValidatePackageSourceApiOnGet(int appId) =>
        Validate(inputs: appId);
}