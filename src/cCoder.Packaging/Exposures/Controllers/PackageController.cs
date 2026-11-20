// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security;
using cCoder.CodeAnalysis.Exposures;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Models;
using cCoder.Packaging.Models.Exceptions;
using cCoder.Packaging.Brokers.Loggings;
using cCoder.Data.Extensions;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Aggregations;
using cCoder.Packaging.Services.Orchestrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Security.Authentication;

namespace cCoder.Packaging.Exposures.Controllers;

public partial class PackageController(
    IPackageAggregationService packageAggregationService,
    ILoggingBroker loggingBroker)
    : ODataController, ICompositionExposure
{
    [HttpGet("/Api/Packaging/Package/Export")]
    public async Task<IActionResult> Get(
        [FromQuery] int? appId = null,
        [FromQuery] string[] packageNames = null)
    {
        try
        {

            {
                var actionValue = await packageAggregationService.ExportPackagesAsync(appId: appId, packageNames: packageNames);
                loggingBroker.LogInformation(message: "Controller action completed successfully.");
                return Ok(value: actionValue);
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

    [HttpGet]
    [EnableQuery(
        AllowedArithmeticOperators = AllowedArithmeticOperators.All,
        AllowedFunctions = AllowedFunctions.AllFunctions,
        AllowedLogicalOperators = AllowedLogicalOperators.All,
        AllowedQueryOptions = AllowedQueryOptions.All,
        MaxAnyAllExpressionDepth = 5,
        MaxExpansionDepth = 5
    )]
    [ActionName("Get")]
    public IActionResult GetAll()
    {
        try
        {

            {
                var actionValue = packageAggregationService.GetAllPackages();
                loggingBroker.LogInformation(message: "Controller action completed successfully.");
                return Ok(value: actionValue);
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

    [HttpGet]
    [AllowAnonymous]
    [EnableQuery(
        AllowedArithmeticOperators = AllowedArithmeticOperators.All,
        AllowedFunctions = AllowedFunctions.AllFunctions,
        AllowedLogicalOperators = AllowedLogicalOperators.All,
        AllowedQueryOptions = AllowedQueryOptions.All,
        MaxAnyAllExpressionDepth = 3,
        MaxExpansionDepth = 3
    )]
    public IActionResult Get([FromRoute] Guid key)
    {
        try
        {
            IQueryable<Package> result = packageAggregationService.GetAllPackages()
                                             .Where(predicate: package => package.Id == key);

            Package package = result.FirstOrDefault();

            if (package is null)
            {

                {
                    loggingBroker.LogWarning(message: "Controller action returned a client error.");
                    return NotFound();
                }
            }


            {

                loggingBroker.LogInformation(message: "Controller action completed successfully.");

                return Ok(value: package);

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

    [HttpPost]
    [EnableQuery(
        AllowedArithmeticOperators = AllowedArithmeticOperators.All,
        AllowedFunctions = AllowedFunctions.AllFunctions,
        AllowedLogicalOperators = AllowedLogicalOperators.All,
        AllowedQueryOptions = AllowedQueryOptions.All,
        MaxAnyAllExpressionDepth = 5,
        MaxExpansionDepth = 5
    )]
    public async Task<IActionResult> Post([FromBody] Package newPackage)
    {
        try
        {
            if (!ModelState.IsValid)
            {

                {
                    loggingBroker.LogWarning(message: "Controller action returned a client error.");
                    return BadRequest(modelState: ModelState);
                }
            }


            {

                var actionValue = await packageAggregationService.AddPackageAsync(newPackage: newPackage);

                loggingBroker.LogInformation(message: "Controller action completed successfully.");

                return StatusCode(statusCode: StatusCodes.Status201Created, value: actionValue);

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

    [HttpPost("/Api/Packaging/Package/Import")]
    public async Task<IActionResult> PostImport(
        [FromBody] Package newPackage,
        [FromQuery] int? appId = null)
    {
        try
        {
            if (!ModelState.IsValid)
            {

                {
                    loggingBroker.LogWarning(message: "Controller action returned a client error.");
                    return BadRequest(modelState: ModelState);
                }
            }

            await packageAggregationService.ImportPackageAsync(
                appId: appId,
                package: newPackage);


            {

                loggingBroker.LogInformation(message: "Controller action completed successfully.");

                return Accepted();

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

    [HttpPut]
    [EnableQuery(
        AllowedArithmeticOperators = AllowedArithmeticOperators.All,
        AllowedFunctions = AllowedFunctions.AllFunctions,
        AllowedLogicalOperators = AllowedLogicalOperators.All,
        AllowedQueryOptions = AllowedQueryOptions.All,
        MaxAnyAllExpressionDepth = 5,
        MaxExpansionDepth = 5
    )]
    public async Task<IActionResult> Put(
        [FromRoute] Guid key,
        [FromBody] Package updatedPackage)
    {
        try
        {
            if (!ModelState.IsValid)
            {

                {
                    loggingBroker.LogWarning(message: "Controller action returned a client error.");
                    return BadRequest(modelState: ModelState);
                }
            }

            updatedPackage.Id = key;


            {

                var actionValue = await packageAggregationService.UpdatePackageAsync(updatedPackage: updatedPackage);

                loggingBroker.LogInformation(message: "Controller action completed successfully.");

                return Ok(value: actionValue);

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

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] Guid key)
    {
        try
        {
            await packageAggregationService.DeletePackageAsync(packageId: key);


            {

                loggingBroker.LogInformation(message: "Controller action completed successfully.");

                return NoContent();

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