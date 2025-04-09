namespace NicePartUsageSocialPlatform.Models;

public class Score
{
    public int CreativityScore { get; set; }
    public int UniquenessScore { get; set; }
    public User User { get; init; }
    public string UserId { get; init; }
    public NpuCreation NpuCreation { get; init; }
    public Guid NpuCreationId { get; init; }
}