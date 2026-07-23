// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Api.OData;
using cCoder.Data.Models.Packaging;


namespace cCoder.Packaging.Services.Foundations;

internal sealed class PackagingMetadataTypeService : IPackagingMetadataTypeService
{
    public IEnumerable<MetadataContainerSet> GetKnownMetadata() =>
    [
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
    ];

    private static ExtendedMetadataContainer Entity<T>() =>
        new(typeof(T), isEntity: true, hasEndpoint: true)
        {
            Category = "Packaging",
        };
}