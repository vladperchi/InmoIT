// --------------------------------------------------------------------------------------------------
// <copyright file="QueryableExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InmoIT.Shared.Core.Exceptions;
using InmoIT.Shared.Core.Wrapper;
using Microsoft.Extensions.Localization;
using InmoIT.Shared.Core.Contracts;
using InmoIT.Shared.Core.Interfaces.Specifications;

namespace InmoIT.Shared.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, IStringLocalizer localizer)
            where T : class
        {
            if (source == null)
            {
                throw new PagedListEmptyException(localizer);
            }

            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = await source.AsNoTracking().CountAsync();
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
        }

        public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> spec)
            where T : class, IEntity
        {
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(
                    query,
                    (current, include) => current.Include(include));
            var secondaryResult = spec.IncludeStrings
                .Aggregate(
                    queryableResultWithIncludes,
                    (current, include) => current.Include(include));
            return secondaryResult.Where(spec.Criteria);
        }
    }
}