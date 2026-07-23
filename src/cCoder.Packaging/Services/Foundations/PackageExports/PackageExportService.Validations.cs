// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Foundations.PackageExports;

internal sealed partial class PackageExportService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidatePackageSourceApiOnGet(int appId) =>
        Validate(inputs: appId);
}