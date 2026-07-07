using cCoder.Packaging.Exposures.Setup;
using Microsoft.AspNetCore.Mvc;

namespace cCoder.Packaging.Exposures.Controllers;

[ApiController]
[Route("Api/Packaging/Baseline")]
public sealed class BaselineController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        Ok(PackagingBaselinePackages.All);
}
