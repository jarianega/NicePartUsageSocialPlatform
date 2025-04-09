using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NicePartUsageSocialPlatform.Models;
using NicePartUsageSocialPlatform.Services;

namespace NicePartUsageSocialPlatform.Controllers;

[Route("api/users")]
public class UserController(
    IUserService userService) : Controller
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var existingUser = await userService.GetUserByUsernameAsync(request.Username);
        if (existingUser != null)
        {
            return BadRequest("Username already exists");
        }

        var user = new User
        {
            UserName = request.Username
        };
        var result = await userService.CreateUserAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }
}

public record CreateUserRequest(
    string Username,
    string Password);