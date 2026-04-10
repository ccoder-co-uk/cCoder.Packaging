using cCoder.Packaging.Api.OData;


namespace cCoder.Packaging.Services.Foundations;

internal interface IPackagingMetadataTypeService
{
    IEnumerable<MetadataContainerSet> GetKnownMetadata();
}

