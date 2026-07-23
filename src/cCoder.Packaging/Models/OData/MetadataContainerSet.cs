// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Api.OData;

public sealed class MetadataContainerSet
{
    public string Name { get; set; }
    public string UriBase { get; set; }
    public ExtendedMetadataContainer[] Types { get; set; }
}