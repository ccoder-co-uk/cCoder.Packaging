// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Exposures;
using Microsoft.AspNetCore.Mvc;

namespace cCoder.Packaging.Exposures.Controllers;

[ApiController]
public sealed class PackageMetadataController(
    IPackageMetadataManager metadataService)
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