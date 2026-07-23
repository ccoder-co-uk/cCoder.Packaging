// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Services.Foundations.Baselines;
using Microsoft.AspNetCore.Mvc;

namespace cCoder.Packaging.Exposures.Controllers;

[ApiController]
[Route("Api/Packaging/Baseline")]
public sealed class BaselineController(IBaselineService baselineService)
    : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        Ok(value: baselineService.GetBaselinePackages());
}