// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Api.OData;


namespace cCoder.Packaging.Services.Foundations;

internal interface IPackagingMetadataTypeService
{
    IEnumerable<MetadataContainerSet> GetKnownMetadata();
}