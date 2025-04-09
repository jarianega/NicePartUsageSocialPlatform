namespace NicePartUsageSocialPlatform.Models;

public class Element
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public List<NpuCreation> NicePartUsageCreations { get; init; }
}