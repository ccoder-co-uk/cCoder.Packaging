// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Models;
using cCoder.Packaging.Models.Exceptions;
using cCoder.Packaging.Exposures;
using cCoder.Data.Extensions;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Orchestrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace cCoder.Packaging.Exposures.Controllers;

public partial class PackageController(
    IPackageManager packageManager)
    : ODataController
{
    [HttpGet("/Api/Packaging/Package/Export")]
    public async Task<IActionResult> Get(
        [FromQuery] int? appId = null,
        [FromQuery] string[] packageNames = null)
    {
        try
        {
            return Ok(value: await packageManager.ExportPackagesAsync(
                appId: appId,
                packageNames: packageNames));
        }
        catch (PackagingOrchestrationValidationException exception)
        {
            packageManager.LogError(
                exception: exception,
                message: "Controller request failed.");

            return BadRequest(error: "The package request is invalid.");
        }
        catch (SecurityException exception)
        {
            packageManager.LogError(
                exception: exception,
                message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception exception)
        {
            packageManager.LogError(
                exception: exception,
                message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status500InternalServerError);
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
            return Ok(value: packageManager.GetAllPackages());
        }
        catch (PackagingOrchestrationValidationException exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return BadRequest(error: "The package request is invalid.");
        }
        catch (SecurityException exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status500InternalServerError);
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
            IQueryable<Package> result = packageManager.GetAllPackages()
                                             .Where(predicate: package => package.Id == key);

            Package package = result.FirstOrDefault();

            if (package is null)
            {
                return NotFound();
            }

            return Ok(value: package);
        }
        catch (PackagingOrchestrationValidationException exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return BadRequest(error: "The package request is invalid.");
        }
        catch (SecurityException exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status500InternalServerError);
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
                return BadRequest(modelState: ModelState);
            }

            return StatusCode(
                statusCode: StatusCodes.Status201Created,
                value: await packageManager
                    .AddPackageAsync(newPackage: newPackage));
        }
        catch (PackagingOrchestrationValidationException exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return BadRequest(error: "The package request is invalid.");
        }
        catch (SecurityException exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status500InternalServerError);
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
                return BadRequest(modelState: ModelState);
            }

            await packageManager.ImportPackageAsync(
                appId: appId,
                package: newPackage);

            return Accepted();
        }
        catch (PackagingOrchestrationValidationException exception)
        {
            packageManager.LogError(
                exception: exception,
                message: "Controller request failed.");

            return BadRequest(error: "The package request is invalid.");
        }
        catch (Exception exception)
        {
            packageManager.LogError(
                exception: exception,
                message: "Controller request failed.");

            return StatusCode(
                statusCode: StatusCodes.Status500InternalServerError);
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
                return BadRequest(modelState: ModelState);
            }

            updatedPackage.Id = key;

            return Ok(value: await packageManager
                .UpdatePackageAsync(updatedPackage: updatedPackage));
        }
        catch (PackagingOrchestrationValidationException exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return BadRequest(error: "The package request is invalid.");
        }
        catch (SecurityException exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] Guid key)
    {
        try
        {
            await packageManager.DeletePackageAsync(packageId: key);

            return NoContent();
        }
        catch (PackagingOrchestrationValidationException exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return BadRequest(error: "The package request is invalid.");
        }
        catch (SecurityException exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception exception)
        {
            packageManager.LogError(exception: exception, message: "Controller request failed.");

            return StatusCode(statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}