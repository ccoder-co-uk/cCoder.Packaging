// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Services.Foundations.Metadata;

namespace cCoder.Packaging.Exposures;

internal sealed class PackageMetadataManager(IMetadataService metadataService)
    : IPackageMetadataManager
{
    public MetadataContainer CreateMetadataContainer(
        Type type,
        bool isEntity,
        bool hasEndpoint) =>
        metadataService.CreateMetadataContainer(
            type: type,
            isEntity: isEntity,
            hasEndpoint: hasEndpoint);
}