using Microsoft.EntityFrameworkCore;
using NicePartUsageSocialPlatform.Data;
using NicePartUsageSocialPlatform.Models;

namespace NicePartUsageSocialPlatform.Repositories;

public class ElementRepository(ApplicationDbContext context) : IElementRepository
{
    public async Task<List<Element>> GetAllElementsAsync(CancellationToken cancellationToken)
    {
        return await context.Elements.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<Element>> GetElementsByIds(List<Guid> elementIds, CancellationToken cancellationToken)
    {
        return await context.Elements.Where(e => elementIds.Contains(e.Id)).ToListAsync(cancellationToken);
    }
}