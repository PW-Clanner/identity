using Pw.Clanner.Identity.Common.Models;

namespace Pw.Clanner.Identity.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
    {
        return PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
    }
}