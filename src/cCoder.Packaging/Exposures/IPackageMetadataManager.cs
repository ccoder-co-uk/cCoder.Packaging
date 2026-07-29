// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Api.OData;

namespace cCoder.Packaging.Exposures;

public interface IPackageMetadataManager
{
    MetadataContainer CreateMetadataContainer(
        Type type,
        bool isEntity,
        bool hasEndpoint);
}