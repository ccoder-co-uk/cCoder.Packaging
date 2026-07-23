// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

namespace cCoder.Packaging.Api.OData;

public sealed class ExtendedMetadataContainer : MetadataContainer
{
    public IEnumerable<OperationContainer> Operations { get; set; }
}