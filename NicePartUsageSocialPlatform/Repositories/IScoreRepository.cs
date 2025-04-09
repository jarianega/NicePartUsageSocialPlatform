using NicePartUsageSocialPlatform.Models;

namespace NicePartUsageSocialPlatform.Repositories;

public interface IScoreRepository
{
    Task<Score?> GetNpuCreationScoreByUserIdAndCreationIdAsync(string userId, Guid creationId, CancellationToken cancellationToken);
    Task<Score> CreateScoreAsync(Score score, CancellationToken cancellationToken);
    Task UpdateScoreAsync(Score score, int creativityScore, int uniquenessScore, CancellationToken cancellationToken);
}