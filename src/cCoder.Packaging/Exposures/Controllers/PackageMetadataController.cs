// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Services.Foundations.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace cCoder.Packaging.Exposures.Controllers;

[ApiController]
public sealed class PackageMetadataController(IMetadataService metadataService)
    : ControllerBase
{
    [HttpGet("Api/Packaging/Package/GetMetadata")]
    public IActionResult GetPackageMetadata()
    {
        MetadataContainer metadata = metadataService.CreateMetadataContainer(
            type: typeof(Package),
            isEntity: true,
            hasEndpoint: true);

        return Ok(value: metadata);
    }
}