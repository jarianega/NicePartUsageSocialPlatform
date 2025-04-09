using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NicePartUsageSocialPlatform.Services;

namespace NicePartUsageSocialPlatform.Controllers;

[Route("api/npu-creations")]
public class NpuCreationsController(
    INpuCreationService npuCreationService,
    IUserService userService)
    : Controller
{
    [HttpPost]
    public async Task<IActionResult> UploadNpuCreation(
        [FromForm] IFormFile imageFile,
        [FromForm] 
        [MinLength(1, ErrorMessage = "There must be a description for the image")]
        string description,
        [FromForm]
        [MinLength(1, ErrorMessage = "There must be at least one element used as a NPU on the image")]
        [ModelBinder(BinderType = typeof(JsonFormDataModelBinder))]
        List<Guid> elementIds,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = await userService.GetLoggedInUserAsync(User);
        if (user == null)
        {
            throw new InvalidOperationException("The user could not be found.");
        }

        var fileType = Path.GetExtension(imageFile.FileName);
        if (fileType != ".png")
        {
            return BadRequest("You can only upload a .png file.");
        }

        await using var imageFileStream = imageFile.OpenReadStream();
        await npuCreationService.CreateNpuCreationAsync(
            imageFileStream,
            description,
            elementIds,
            user,
            cancellationToken);

        return Ok();
    }

    [HttpPost("{id:guid}/score")]
    public async Task<IActionResult> ScoreNpuCreation([FromRoute] Guid id, [FromBody] ScoreNpuCreationRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var npuCreation = await npuCreationService.GetNpuCreationWithUserByIdAsync(id, cancellationToken);
        if (npuCreation == null)
        {
            return NotFound();
        }

        var user = await userService.GetLoggedInUserAsync(User);
        if (user == null)
        {
            throw new InvalidOperationException("The user could not be found.");
        }

        if (npuCreation.User.Id == user.Id)
        {
            return Unauthorized("You cannot score your own creations");
        }

        await npuCreationService.UpsertNpuCreationScoreAsync(
            request.CreativityScore,
            request.UniquenessScore,
            user,
            npuCreation,
            cancellationToken);

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> SearchNpuCreations([FromQuery] string searchQuery,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var matchingNpuCreations =
            await npuCreationService.GetNpuCreationsByElementNameAsync(searchQuery, cancellationToken);

        return Ok(matchingNpuCreations.Select(MapNpuCreationResultToResponse));
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> NpuCreationsList(CancellationToken cancellationToken)
    {
        var npuCreations = await npuCreationService.GetAllNpuCreationsAsync(cancellationToken);

        return Ok(npuCreations.Select(MapNpuCreationResultToResponse));
    }
    
    private static NpuCreationsResponse MapNpuCreationResultToResponse(NpuCreationResult c)
    {
        return new NpuCreationsResponse(
            c.Id,
            c.ImageUri,
            c.UserName,
            new AverageScoreResponse(
                c.AverageScore.AverageCreativityScore,
                c.AverageScore.AverageUniquenessScore));
    }
}

public record ScoreNpuCreationRequest(
    [Range(1, 5)] int CreativityScore,
    [Range(1, 5)] int UniquenessScore
);

public record NpuCreationsResponse(
    Guid Id,
    Uri ImageUri,
    string UserName,
    AverageScoreResponse AverageScore
);

public record AverageScoreResponse(
    double AverageCreativityScore,
    double AverageUniquenessScore);