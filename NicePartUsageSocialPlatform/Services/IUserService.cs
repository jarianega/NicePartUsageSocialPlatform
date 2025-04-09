using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NicePartUsageSocialPlatform.Models;

namespace NicePartUsageSocialPlatform.Services;

public interface IUserService
{
    Task<User?> GetLoggedInUserAsync(ClaimsPrincipal user);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<IdentityResult> CreateUserAsync(User user, string password);
    Task<bool> CheckPasswordAsync(User user, string password);
}