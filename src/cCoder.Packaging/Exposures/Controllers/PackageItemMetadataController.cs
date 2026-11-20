// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Brokers.Loggings;
using cCoder.Packaging.Services.Foundations.Metadata;
using cCoder.Packaging.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using System.Security.Authentication;

namespace cCoder.Packaging.Exposures.Controllers;

[ApiController]
public sealed class PackageItemMetadataController(
    IMetadataService metadataService,
    ILoggingBroker loggingBroker)
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


            {

                loggingBroker.LogInformation(message: "Controller action completed successfully.");

                return Ok(value: metadata);

            }
        }

        catch (PackagingOrchestrationValidationException exception)
{
    loggingBroker.LogWarning(message: "Controller validation failed.", args: exception);
    return BadRequest(error: "The packaging request is invalid.");
}

        catch (PackagingValidationException exception)
{
    loggingBroker.LogWarning(message: "Controller validation failed.", args: exception);
    return BadRequest(error: "The packaging request is invalid.");
}

        catch (ArgumentException exception)
{
    loggingBroker.LogWarning(message: "Controller argument validation failed.", args: exception);
    return BadRequest(error: "The packaging request is invalid.");
}

        catch (AuthenticationException exception)
{
    loggingBroker.LogWarning(message: "Controller authentication failed.", args: exception);
    return StatusCode(statusCode: StatusCodes.Status401Unauthorized, value: "Authentication failed.");
}

        catch (UnauthorizedAccessException exception)
{
    loggingBroker.LogWarning(message: "Controller authorization failed.", args: exception);
    return StatusCode(statusCode: StatusCodes.Status403Forbidden, value: "The packaging operation is forbidden.");
}

        catch (SecurityException exception)
{
    loggingBroker.LogWarning(message: "Controller security check failed.", args: exception);
    return StatusCode(statusCode: StatusCodes.Status403Forbidden, value: "The packaging operation is forbidden.");
}

        catch (Exception exception)
{
    loggingBroker.LogError(exception: exception, message: "Controller request failed.");
    return StatusCode(statusCode: StatusCodes.Status500InternalServerError, value: "The packaging operation failed.");
}
        }
}