// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Packaging.Exposures.Setup;
using Microsoft.AspNetCore.Mvc;

namespace cCoder.Packaging.Exposures.Controllers;

[ApiController]
[Route("Api/Packaging/Baseline")]
public sealed class BaselineController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        Ok(value:UIBaseline.Packages);
}