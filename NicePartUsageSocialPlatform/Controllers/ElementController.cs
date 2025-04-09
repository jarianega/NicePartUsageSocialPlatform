using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NicePartUsageSocialPlatform.Services;

namespace NicePartUsageSocialPlatform.Controllers;

[Route("api/elements")]
public class ElementController(IElementService elementService) : Controller
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllElements(CancellationToken cancellationToken)
    {
        return Ok(await elementService.GetAllElements(cancellationToken));
    }
}