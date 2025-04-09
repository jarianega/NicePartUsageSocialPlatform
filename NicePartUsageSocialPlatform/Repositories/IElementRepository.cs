using NicePartUsageSocialPlatform.Models;

namespace NicePartUsageSocialPlatform.Repositories;

public interface IElementRepository
{
    Task<List<Element>> GetAllElementsAsync(CancellationToken cancellationToken);
    Task<List<Element>> GetElementsByIds(List<Guid> elementIds, CancellationToken cancellationToken);
}