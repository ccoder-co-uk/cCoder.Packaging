// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Api.OData;

namespace cCoder.Packaging.Services.Foundations.Metadata;

public interface IMetadataService
{
    MetadataContainer CreateMetadataContainer(Type type, bool isEntity, bool hasEndpoint);
}