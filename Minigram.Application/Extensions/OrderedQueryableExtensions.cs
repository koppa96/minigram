using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Minigram.Application.Abstractions.Dtos;

namespace Minigram.Application.Extensions
{
    public static class OrderedQueryableExtensions
    {
        public static Task<PagedListDto<TResult>> ToPagedListAsync<TEntity, TResult>(
            this IOrderedQueryable<TEntity> queryable,
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, TResult>> selector,
            CancellationToken cancellationToken = default)
        {
            return queryable.ToPagedListAsync(
                pageIndex,
                pageSize,
                entities => entities.Select(selector),
                cancellationToken);
        }
        
        public static Task<PagedListDto<TResult>> ToPagedListAsync<TEntity, TResult>(
            this IOrderedQueryable<TEntity> queryable,
            int pageIndex,
            int pageSize,
            IConfigurationProvider configurationProvider,
            CancellationToken cancellationToken = default)
        {
            return queryable.ToPagedListAsync(
                pageIndex,
                pageSize,
                entities => entities.ProjectTo<TResult>(configurationProvider),
                cancellationToken);
        }
        
        private static async Task<PagedListDto<TResult>> ToPagedListAsync<TEntity, TResult>(
            this IOrderedQueryable<TEntity> queryable,
            int pageIndex,
            int pageSize,
            Func<IQueryable<TEntity>, IQueryable<TResult>> mappingFunc,
            CancellationToken cancellationToken)
        {
            var totalCount = await queryable.CountAsync(cancellationToken);
            List<TResult> results;
            if (totalCount > 0)
            {
                results = await mappingFunc(queryable.Skip(pageIndex * pageSize)
                    .Take(pageSize))
                    .ToListAsync(cancellationToken);
            }
            else
            {
                results = new List<TResult>();
            }
        
            return new PagedListDto<TResult>
            {
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Data = results
            };
        }
    }
}