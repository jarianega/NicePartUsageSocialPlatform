namespace NicePartUsageSocialPlatform.Models;

public class NpuCreation
{
    public Guid Id { get; init; }
    public string ImageFileName { get; init; }
    public string Description { get; init; }
    public List<Element> Elements { get; init; }
    public User User { get; init; }
    public List<Score> Scores { get; init; }
}