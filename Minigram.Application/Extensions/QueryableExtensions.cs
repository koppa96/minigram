using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minigram.Application.Abstractions.Dtos;

namespace Minigram.Application.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedListDto<T>> ToPagedListAsync<T>(
            this IQueryable<T> queryable,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var totalCount = await queryable.CountAsync(cancellationToken);
            List<T> results;
            if (totalCount > 0)
            {
                results = await queryable.Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                results = new List<T>();
            }
        
            return new PagedListDto<T>
            {
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Data = results
            };
        }
    }
}