// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Api.OData;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Brokers.Metadata;


namespace cCoder.Packaging.Services.Foundations;

internal sealed partial class PackagingMetadataTypeService(IMetadataBroker metadataBroker)
    : IPackagingMetadataTypeService
{
    public IEnumerable<MetadataContainerSet> GetKnownMetadata() =>
        TryCatch(operation: () =>
        {
            ValidateKnownMetadataOnGet();

            return new MetadataContainerSet[]
            {
                new MetadataContainerSet
                {
                    Name = "Packaging",
                    UriBase = "Packaging",
                    Types =
                    [
                        Entity<Package>(),
                        Entity<PackageItem>(),
                    ],
                },
            };
        });

    private ExtendedMetadataContainer Entity<T>()
    {
        MetadataContainer metadata = metadataBroker.CreateMetadataContainer(
            type: typeof(T),
            isEntity: true,
            hasEndpoint: true);

        return new ExtendedMetadataContainer
        {
            Type = metadata.Type,
            ServerTypeName = metadata.ServerTypeName,
            IsValueType = metadata.IsValueType,
            IsEntity = metadata.IsEntity,
            IsJoinEntity = metadata.IsJoinEntity,
            HasEndpoint = metadata.HasEndpoint,
            IsSystemManaged = metadata.IsSystemManaged,
            Category = "Packaging",
            Name = metadata.Name,
            DisplayName = metadata.DisplayName,
            Description = metadata.Description,
            ServerType = metadata.ServerType,
            Properties = metadata.Properties,
        };
    }
}