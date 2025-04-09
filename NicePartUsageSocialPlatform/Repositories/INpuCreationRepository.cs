using NicePartUsageSocialPlatform.Models;

namespace NicePartUsageSocialPlatform.Repositories;

public interface INpuCreationRepository
{
    Task<NpuCreation> CreateNpuCreationAsync(NpuCreation npuCreation, CancellationToken cancellationToken);
    Task<NpuCreation?> GetNpuCreationWithUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<NpuCreationResult>> GetNpuCreationsByElementNameAsync(string elementName, CancellationToken cancellationToken);
    Task<List<NpuCreationResult>> GetAllNpuCreationsAsync(CancellationToken cancellationToken);
}