// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using cCoder.Packaging.Api.OData;

namespace cCoder.Packaging.Brokers.Metadata;

internal interface IMetadataBroker
{
    MetadataContainer CreateMetadataContainer(Type type, bool isEntity, bool hasEndpoint);
}