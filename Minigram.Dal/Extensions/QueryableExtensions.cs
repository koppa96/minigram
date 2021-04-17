using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Minigram.Dal.Abstractions;
using Minigram.Dal.Entities;

namespace Minigram.Dal.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Utility method for including both ends of friendships with optional further includes.
        /// </summary>
        /// <param name="queryable">The source queryable</param>
        /// <param name="thenInclude">A delegate that contains optional thenInclude calls, and returns the included queriable</param>
        /// <returns>The queriable with the inclusions</returns>
        public static IQueryable<MinigramUser> IncludeFriendships(
            this IQueryable<MinigramUser> queryable,
            Func<IIncludableQueryable<MinigramUser, ICollection<Friendship>>, IQueryable<MinigramUser>> thenInclude = null)
        {
            var includableQueryable = queryable.Include(x => x.Friendships1);
            queryable = thenInclude?.Invoke(includableQueryable) ?? includableQueryable;

            includableQueryable = queryable.Include(x => x.Friendships2);
            return thenInclude?.Invoke(includableQueryable) ?? includableQueryable;
        }

        public static Task<TEntity> FindByIdAsync<TEntity>(
            this IQueryable<TEntity> queryable,
            Guid id,
            CancellationToken cancellationToken = default)
            where TEntity : IEntity
        {
            return queryable.SingleAsync(x => x.Id == id, cancellationToken);
        }
    }
}