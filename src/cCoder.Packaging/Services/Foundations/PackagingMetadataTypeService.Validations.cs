// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------


namespace cCoder.Packaging.Services.Foundations;

internal sealed partial class PackagingMetadataTypeService
{
    private static void Validate(params object[] inputs)
    {
        foreach (object input in inputs)
        {
            ArgumentNullException.ThrowIfNull(argument: input);
        }
    }

    private static void ValidateKnownMetadataOnGet() =>
        Validate(inputs: []);
}