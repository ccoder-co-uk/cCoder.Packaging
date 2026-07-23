// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Dependencies;

namespace cCoder.Packaging.Services.Foundations.Metadata;

internal sealed partial class MetadataService
{
    private static void ValidateMetadataContainerOnCreate(
        Type type,
        bool isEntity,
        bool hasEndpoint) =>
        ValidationRulesEngine.Validate(
            inputs:
            [
                type,
                isEntity,
                hasEndpoint,
            ]);
}