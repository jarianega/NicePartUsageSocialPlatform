using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NicePartUsageSocialPlatform.Models;

namespace NicePartUsageSocialPlatform.Services;

public class UserService(UserManager<User> userManager) : IUserService
{
    public async Task<User?> GetLoggedInUserAsync(ClaimsPrincipal user)
    {
        return await userManager.GetUserAsync(user);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await userManager.FindByNameAsync(username);
    }
    
    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IdentityResult> CreateUserAsync(User user, string password)
    {
        return await userManager.CreateAsync(user, password);
    }
}