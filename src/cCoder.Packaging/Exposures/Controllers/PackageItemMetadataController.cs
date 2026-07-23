// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Services.Foundations.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace cCoder.Packaging.Exposures.Controllers;

[ApiController]
public sealed class PackageItemMetadataController(IMetadataService metadataService)
    : ControllerBase
{
    [HttpGet("Api/Packaging/PackageItem/GetMetadata")]
    public IActionResult GetPackageItemMetadata()
    {
        MetadataContainer metadata = metadataService.CreateMetadataContainer(
            type: typeof(PackageItem),
            isEntity: true,
            hasEndpoint: true);

        return Ok(value: metadata);
    }
}