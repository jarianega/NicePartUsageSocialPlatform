using NicePartUsageSocialPlatform.Models;
using NicePartUsageSocialPlatform.Repositories;

namespace NicePartUsageSocialPlatform.Services;

public class ElementService(IElementRepository elementRepository) : IElementService
{
    public async Task<List<Element>> GetAllElements(CancellationToken cancellationToken)
    {
        return await elementRepository.GetAllElementsAsync(cancellationToken);
    }

    public async Task<List<Element>> GetElementsByIds(List<Guid> elementIds, CancellationToken cancellationToken)
    {
        return await elementRepository.GetElementsByIds(elementIds, cancellationToken);
    }
}