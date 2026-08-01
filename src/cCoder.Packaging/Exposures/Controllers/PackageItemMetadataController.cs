// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Exposures;
using cCoder.Packaging.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace cCoder.Packaging.Exposures.Controllers;

[ApiController]
public sealed class PackageItemMetadataController(
    IPackageMetadataManager metadataService)
    : ControllerBase
{
    [HttpGet("Api/Packaging/PackageItem/GetMetadata")]
    public IActionResult GetPackageItemMetadata()
    {
        try
        {
            MetadataContainer metadata = metadataService.CreateMetadataContainer(
                type: typeof(PackageItem),
                isEntity: true,
                hasEndpoint: true);

            return Ok(value: metadata);
        }
        catch (PackagingValidationException)
        {
            return BadRequest(error: "The package-item metadata request is invalid.");
        }
        catch (Exception)
        {
            return StatusCode(statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}