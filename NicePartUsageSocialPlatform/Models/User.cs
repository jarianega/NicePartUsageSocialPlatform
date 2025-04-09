using Microsoft.AspNetCore.Identity;

namespace NicePartUsageSocialPlatform.Models;

public class User : IdentityUser
{
    public List<NpuCreation> NicePartUsageCreations { get; init; }
}