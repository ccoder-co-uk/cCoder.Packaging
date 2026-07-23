// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Models;
using cCoder.Data.Extensions;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Orchestrations;
using cCoder.Packaging.Services.Aggregations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace cCoder.Packaging.Exposures.Controllers;

public partial class PackageController(
    IPackageAggregationService packageOrchestrationService)
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
    public IActionResult GetAll(ODataQueryOptions<Package> queryOptions) =>
        Ok(value: packageOrchestrationService.GetAllPackages());

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
            IQueryable<Package> result = packageOrchestrationService.GetAllPackages()
                                             .Where(predicate: package => package.Id == key);

            return Ok(value: SingleResult.Create(queryable: result));
        }
        catch (SecurityException)
        {
            return NotFound();
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
        if (!ModelState.IsValid)
        {
            return BadRequest(modelState: ModelState);
        }

        return Ok(value: await packageOrchestrationService
            .AddPackageAsync(newPackage: newPackage));
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
        if (!ModelState.IsValid)
        {
            return BadRequest(modelState: ModelState);
        }

        return Ok(value: await packageOrchestrationService
            .UpdatePackageAsync(updatedPackage: updatedPackage));
    }

    [AcceptVerbs("PATCH", "MERGE")]
    [ActionName("Patch")]
    public async Task<IActionResult> PutPackagePatch(
        [FromRoute] Guid key,
        Delta<Package> updatedPackageDelta)
    {
        Package originalEntity = packageOrchestrationService
            .GetPackage(packageId: key);

        if (originalEntity == null)
        {
            return NotFound();
        }

        updatedPackageDelta.Patch(original: originalEntity);

        return Ok(value: await packageOrchestrationService
            .UpdatePackageAsync(updatedPackage: originalEntity));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] Guid key)
    {
        await packageOrchestrationService.DeletePackageAsync(packageId: key);
        return Ok();
    }
}