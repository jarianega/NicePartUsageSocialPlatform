using NicePartUsageSocialPlatform.Models;

namespace NicePartUsageSocialPlatform.Services;

public interface IElementService
{
    Task<List<Element>> GetAllElements(CancellationToken cancellationToken);
    Task<List<Element>> GetElementsByIds(List<Guid> elementIds, CancellationToken cancellationToken);
}