// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Security;
using cCoder.Packaging.Api.OData;
using cCoder.Packaging.Models;
using cCoder.Data.Extensions;
using cCoder.Data.Models.Packaging;
using cCoder.Packaging.Services.Orchestrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace cCoder.Packaging.Exposures.Controllers;

public partial class PackageController(IPackageOrchestrationService packageOrchestrationService) : ODataController
{
    [HttpGet]
    public IActionResult GetMetadata()
    {
        return Ok(value:new MetadataContainer(typeof(Package), true, true));
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
    public IActionResult GetAll(ODataQueryOptions<Package> queryOptions) => Ok(value:packageOrchestrationService.GetAll());

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
            IQueryable<Package> result = packageOrchestrationService.GetAll()
                                             .Where(predicate:package => package.Id == key);

            return Ok(value:SingleResult.Create(queryable:result));
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
    public async Task<IActionResult> Post([FromBody] Package entity)
    {
        if (!ModelState.IsValid)
            return BadRequest(modelState:ModelState);

        return Ok(value:await packageOrchestrationService.AddAsync(entity:entity));
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
    public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] Package entity)
    {
        if (!ModelState.IsValid)
            return BadRequest(modelState:ModelState);

        return Ok(value:await packageOrchestrationService.UpdateAsync(entity:entity));
    }

    [AcceptVerbs("PATCH", "MERGE")]
    public async Task<IActionResult> Patch([FromRoute] Guid key, Delta<Package> delta)
    {
        Package originalEntity = packageOrchestrationService.Get(id:key);

        if (originalEntity == null)
            return NotFound();

        delta.Patch(original:originalEntity);
        return Ok(value:await packageOrchestrationService.UpdateAsync(entity:originalEntity));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] Guid key)
    {
        await packageOrchestrationService.DeleteAsync(id:key);
        return Ok();
    }
}