using NicePartUsageSocialPlatform.Models;
using NicePartUsageSocialPlatform.Repositories;

namespace NicePartUsageSocialPlatform.Services;

public class NpuCreationService(
    INpuCreationRepository npuCreationRepository,
    IScoreRepository scoreRepository,
    IFileService fileService,
    IElementService elementService) : INpuCreationService
{
    public async Task<NpuCreation> CreateNpuCreationAsync(
        Stream imageFileStream,
        string description,
        List<Guid> elementIds,
        User user,
        CancellationToken cancellationToken)
    {
        var imageFileName = await fileService.SaveImageFileAsync(imageFileStream, cancellationToken);

        var elements = await elementService.GetElementsByIds(elementIds, cancellationToken);

        var npuCreation = new NpuCreation
        {
            ImageFileName = imageFileName,
            Description = description,
            Elements = elements,
            User = user
        };

        return await npuCreationRepository.CreateNpuCreationAsync(npuCreation, cancellationToken);
    }

    public async Task<NpuCreation?> GetNpuCreationWithUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await npuCreationRepository.GetNpuCreationWithUserByIdAsync(id, cancellationToken);
    }

    public async Task<List<NpuCreationResult>> GetNpuCreationsByElementNameAsync(string elementName,
        CancellationToken cancellationToken)
    {
        var npuCreations =
            await npuCreationRepository.GetNpuCreationsByElementNameAsync(elementName, cancellationToken);
        return npuCreations
            .Select(MapFromRepositoryToServiceResult)
            .ToList();
    }

    public async Task<List<NpuCreationResult>> GetAllNpuCreationsAsync(CancellationToken cancellationToken)
    {
        var npuCreations =
            await npuCreationRepository.GetAllNpuCreationsAsync(cancellationToken);
        return npuCreations
            .Select(MapFromRepositoryToServiceResult)
            .ToList();
    }

    public async Task<Score> UpsertNpuCreationScoreAsync(
        int creativityScore,
        int uniquenessScore,
        User user,
        NpuCreation npuCreation,
        CancellationToken cancellationToken)
    {
        var existingScore = await scoreRepository.GetNpuCreationScoreByUserIdAndCreationIdAsync(
            user.Id,
            npuCreation.Id,
            cancellationToken);

        if (existingScore == null)
        {
            var score = new Score
            {
                CreativityScore = creativityScore,
                UniquenessScore = uniquenessScore,
                User = user,
                NpuCreation = npuCreation
            };

            await scoreRepository.CreateScoreAsync(score, cancellationToken);
            return score;
        }

        await scoreRepository.UpdateScoreAsync(existingScore, creativityScore, uniquenessScore, cancellationToken);
        return existingScore;
    }

    private NpuCreationResult MapFromRepositoryToServiceResult(Repositories.NpuCreationResult repositoryResult)
    {
        return new NpuCreationResult(
            repositoryResult.Id,
            fileService.ConvertFileNameToUri(repositoryResult.ImageFilePath),
            repositoryResult.UserName,
            new AverageScoreResult(
                repositoryResult.AverageScore.AverageCreativityScore ?? 0,
                repositoryResult.AverageScore.AverageUniquenessScore ?? 0));
    }
}

public record NpuCreationResult(
    Guid Id,
    Uri ImageUri,
    string UserName,
    AverageScoreResult AverageScore);

public record AverageScoreResult(
    double AverageCreativityScore,
    double AverageUniquenessScore);