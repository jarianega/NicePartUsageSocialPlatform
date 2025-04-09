using NicePartUsageSocialPlatform.Models;
using NicePartUsageSocialPlatform.Repositories;

namespace NicePartUsageSocialPlatform.Services;

public interface INpuCreationService
{
    Task<NpuCreation> CreateNpuCreationAsync(
        Stream imageFileStream,
        string description,
        List<Guid> elementIds,
        User user,
        CancellationToken cancellationToken);
    Task<NpuCreation?> GetNpuCreationWithUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<NpuCreationResult>> GetNpuCreationsByElementNameAsync(string elementName, CancellationToken cancellationToken);
    Task<List<NpuCreationResult>> GetAllNpuCreationsAsync(CancellationToken cancellationToken);

    Task<Score> UpsertNpuCreationScoreAsync(
        int creativityScore, 
        int uniquenessScore, 
        User user, 
        NpuCreation npuCreation, 
        CancellationToken cancellationToken);
}