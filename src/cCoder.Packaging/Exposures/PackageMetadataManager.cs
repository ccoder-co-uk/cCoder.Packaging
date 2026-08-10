// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Services.Foundations.Metadata;

namespace cCoder.Packaging.Exposures;

internal sealed class PackageMetadataManager(
    IMetadataService metadataService,
    IPackagingLoggingManager packagingLoggingManager)
    : IPackageMetadataManager
{
    public MetadataContainer CreateMetadataContainer(
        Type type,
        bool isEntity,
        bool hasEndpoint) =>
        metadataService.CreateMetadataContainer(
            type: type,
            isEntity: isEntity,
            hasEndpoint: hasEndpoint);

    public void LogError(Exception exception, string message) =>
        packagingLoggingManager.LogError(
            exception: exception,
            message: message);
}