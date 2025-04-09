using Microsoft.EntityFrameworkCore;
using NicePartUsageSocialPlatform.Data;
using NicePartUsageSocialPlatform.Models;

namespace NicePartUsageSocialPlatform.Repositories;

public class NpuCreationRepository(ApplicationDbContext context) : INpuCreationRepository
{
    public async Task<NpuCreation> CreateNpuCreationAsync(NpuCreation npuCreation, CancellationToken cancellationToken)
    {
        var creation = await context.NpuCreations.AddAsync(npuCreation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return creation.Entity;
    }

    public async Task<NpuCreation?> GetNpuCreationWithUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.NpuCreations
            .Include(c => c.User)
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
    
    public async Task<List<NpuCreationResult>> GetNpuCreationsByElementNameAsync(string elementName, CancellationToken cancellationToken)
    {
        return await context.NpuCreations
            .Where(c => c.Elements
                .Any(e => e.Name.Contains(elementName)))
            .Select(c => MapNpuCreationToResult(c))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<NpuCreationResult>> GetAllNpuCreationsAsync(CancellationToken cancellationToken)
    {
        return await context.NpuCreations
            .Select(c => MapNpuCreationToResult(c))
            .ToListAsync(cancellationToken);
    }
    
    private static NpuCreationResult MapNpuCreationToResult(NpuCreation c)
    {
        return new NpuCreationResult(
            c.Id,
            c.ImageFileName,
            c.User.UserName,
            new AverageScoreResult(
                c.Scores.Average(s => s.CreativityScore),
                c.Scores.Average(s => s.UniquenessScore))
        );
    }
}

public record NpuCreationResult(
    Guid Id,
    string ImageFilePath,
    string UserName,
    AverageScoreResult AverageScore);
    
public record AverageScoreResult(
    double? AverageCreativityScore,
    double? AverageUniquenessScore);