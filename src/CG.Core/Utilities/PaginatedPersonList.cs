using CG.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CG.Core.Utilities;

public class PaginatedPersonList<T>(
    List<T> items,
    int count,
    int pageIndex,
    int pageSize) : PaginatedList<T>(items, count, pageIndex, pageSize)
    where T : Person
{
    public static async Task<PaginatedPersonList<T>> CreatePersonAsync(
        IQueryable<T> source,
        int pageIndex,
        int pageSize,
        string search,
        int sort)
    {
        var count = await source.CountAsync();

        if (sort == 0)
        {
            var items = await source
            .OrderBy(x => x.CreatedOn)
            .Where(w => w.FirstName.Contains(search) || w.LastName.Contains(search))
            .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedPersonList<T>(items, count, pageIndex, pageSize);
        }
        else
        {
            var items = await source
            .OrderByDescending(x => x.CreatedOn)
            .Where(w => w.FirstName.Contains(search) || w.LastName.Contains(search))
            .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedPersonList<T>(items, count, pageIndex, pageSize);
        }
    }
}
