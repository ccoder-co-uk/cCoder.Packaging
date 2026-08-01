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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace cCoder.Packaging.Exposures.Controllers;

public partial class PackageItemController(
    IPackageItemManager packageItemOrchestrationService)
    : ODataController
{

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
            return Ok(value: packageItemOrchestrationService.GetAllPackageItems());
        }
        catch (PackagingOrchestrationValidationException)
        {
            return BadRequest(error: "The package-item request is invalid.");
        }
        catch (SecurityException)
        {
            return StatusCode(statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception)
        {
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
            IQueryable<PackageItem> result =
                packageItemOrchestrationService.GetAllPackageItems()
                    .Where(predicate: packageItem => packageItem.Id == key);

            PackageItem packageItem = result.FirstOrDefault();

            if (packageItem is null)
            {
                return NotFound();
            }

            return Ok(value: SingleResult.Create(queryable: result));
        }
        catch (PackagingOrchestrationValidationException)
        {
            return BadRequest(error: "The package-item request is invalid.");
        }
        catch (SecurityException)
        {
            return StatusCode(statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception)
        {
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
    public async Task<IActionResult> Post([FromBody] PackageItem newPackageItem)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState: ModelState);
            }

            return StatusCode(
                statusCode: StatusCodes.Status201Created,
                value: await packageItemOrchestrationService
                    .AddPackageItemAsync(newPackageItem: newPackageItem));
        }
        catch (PackagingOrchestrationValidationException)
        {
            return BadRequest(error: "The package-item request is invalid.");
        }
        catch (SecurityException)
        {
            return StatusCode(statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception)
        {
            return StatusCode(statusCode: StatusCodes.Status500InternalServerError);
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
        [FromBody] PackageItem updatedPackageItem)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState: ModelState);
            }

            updatedPackageItem.Id = key;

            return Ok(value: await packageItemOrchestrationService
                .UpdatePackageItemAsync(updatedPackageItem: updatedPackageItem));
        }
        catch (PackagingOrchestrationValidationException)
        {
            return BadRequest(error: "The package-item request is invalid.");
        }
        catch (SecurityException)
        {
            return StatusCode(statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception)
        {
            return StatusCode(statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [AcceptVerbs("PATCH", "MERGE")]
    [ActionName("Patch")]
    public async Task<IActionResult> PutPackageItemPatch(
        [FromRoute] Guid key,
        Delta<PackageItem> updatedPackageItemDelta)
    {
        try
        {
            PackageItem originalEntity = packageItemOrchestrationService
                .GetPackageItem(packageItemId: key);

            if (originalEntity is null)
            {
                return NotFound();
            }

            updatedPackageItemDelta.Patch(original: originalEntity);

            return Ok(value: await packageItemOrchestrationService
                .UpdatePackageItemAsync(updatedPackageItem: originalEntity));
        }
        catch (PackagingOrchestrationValidationException)
        {
            return BadRequest(error: "The package-item request is invalid.");
        }
        catch (SecurityException)
        {
            return StatusCode(statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception)
        {
            return StatusCode(statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] Guid key)
    {
        try
        {
            await packageItemOrchestrationService
                .DeletePackageItemAsync(packageItemId: key);

            return NoContent();
        }
        catch (PackagingOrchestrationValidationException)
        {
            return BadRequest(error: "The package-item request is invalid.");
        }
        catch (SecurityException)
        {
            return StatusCode(statusCode: StatusCodes.Status403Forbidden);
        }
        catch (Exception)
        {
            return StatusCode(statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}