// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Foundations;

internal sealed partial class PackagingMetadataTypeService
{
    private static void Validate(params object[] inputs) =>
        ValidationRulesEngine.Validate(inputs: inputs);

    private static void ValidateKnownMetadataOnGet() =>
        Validate(inputs: []);
}