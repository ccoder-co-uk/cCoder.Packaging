// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Brokers.Metadata;

namespace cCoder.Packaging.Services.Foundations.Metadata;

internal sealed partial class MetadataService(IMetadataBroker metadataBroker)
    : IMetadataService
{
    public MetadataContainer CreateMetadataContainer(
        Type type,
        bool isEntity,
        bool hasEndpoint) =>
        TryCatch(operation: () =>
        {
            ValidateMetadataContainerOnCreate(
                type: type,
                isEntity: isEntity,
                hasEndpoint: hasEndpoint);

            return metadataBroker.CreateMetadataContainer(
                type: type,
                isEntity: isEntity,
                hasEndpoint: hasEndpoint);
        });
}