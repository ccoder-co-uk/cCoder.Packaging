// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Foundations.Baselines;

internal sealed partial class BaselineService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidateBaselinePackagesOnGet() =>
        Validate(inputs: []);
}