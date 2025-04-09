using Microsoft.EntityFrameworkCore;
using NicePartUsageSocialPlatform.Data;
using NicePartUsageSocialPlatform.Models;

namespace NicePartUsageSocialPlatform.Repositories;

public class ScoreRepository(ApplicationDbContext context) : IScoreRepository
{
    public async Task<Score?> GetNpuCreationScoreByUserIdAndCreationIdAsync(string userId, Guid creationId, CancellationToken cancellationToken)
    {
        return await context.Scores.SingleOrDefaultAsync(s => 
            s.UserId == userId && 
            s.NpuCreationId == creationId, cancellationToken);
    }

    public async Task<Score> CreateScoreAsync(Score score, CancellationToken cancellationToken)
    {
        var entry = await context.Scores.AddAsync(score, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }
    
    public async Task UpdateScoreAsync(Score score, int creativityScore, int uniquenessScore, CancellationToken cancellationToken)
    {
        score.CreativityScore = creativityScore;
        score.UniquenessScore = uniquenessScore;
        await context.SaveChangesAsync(cancellationToken);
    }
}