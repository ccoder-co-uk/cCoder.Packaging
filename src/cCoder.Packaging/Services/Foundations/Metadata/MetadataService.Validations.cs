// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Services.Foundations.Metadata;

internal sealed partial class MetadataService
{
    private static void ValidateMetadataContainerOnCreate(Type type)
    {
        ArgumentNullException.ThrowIfNull(argument: type);
    }
}