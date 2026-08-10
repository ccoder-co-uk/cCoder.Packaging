// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Brokers.Loggings;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Exposures;
using cCoder.Packaging.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace cCoder.Packaging.Exposures.Controllers;

[ApiController]
public sealed class PackageMetadataController(
    IPackageMetadataManager metadataService,
    ILoggingBroker loggingBroker)
    : ControllerBase
{
    [HttpGet("Api/Packaging/Package/GetMetadata")]
    public IActionResult GetPackageMetadata()
    {
        try
        {
            MetadataContainer metadata = metadataService.CreateMetadataContainer(
                type: typeof(Package),
                isEntity: true,
                hasEndpoint: true);

            return Ok(value: metadata);
        }
        catch (PackagingValidationException exception)
        {
            loggingBroker.LogError(exception: exception, message: "Controller request failed.");

            return BadRequest(error: "The package metadata request is invalid.");
        }
        catch (Exception exception)
        {
            loggingBroker.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}