// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Processings;

internal sealed partial class PackageExportProcessingService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidatePackageSourceApiOnGet(int appId) =>
        Validate(inputs: appId);
}