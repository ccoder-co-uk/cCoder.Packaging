// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------


namespace cCoder.Packaging.Services.Processings;

internal sealed partial class PackageExportProcessingService
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