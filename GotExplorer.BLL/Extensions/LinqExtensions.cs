using GotExplorer.BLL.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GotExplorer.BLL.Extensions
{
    public static class LinqExtensions 
    {
        public static IOrderedQueryable<TSource> OrderByDynamic<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, OrderBy? orderby)
        {
            if (!orderby.HasValue || orderby == OrderBy.Asc)
            {
                return source.OrderBy(keySelector);
            }
            return source.OrderByDescending(keySelector);
        }

        public static IOrderedEnumerable<TSource> OrderByDynamic<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, OrderBy? orderby)
        {
            if (!orderby.HasValue || orderby == OrderBy.Asc)
            {
                return source.OrderBy(keySelector);
            }
            return source.OrderByDescending(keySelector);
        }

        public static IQueryable<TSource> TakeOrDefault<TSource>(this IQueryable<TSource> source, int? limit)
        {
            if (limit.HasValue)
            {
                return source.Take(limit.Value);
            }
            return source;
        }

        public static IEnumerable<TSource> TakeOrDefault<TSource>(this IEnumerable<TSource> source, int? limit)
        {
            if (limit.HasValue)
            {
                return source.Take(limit.Value);
            }
            return source;
        }
    }
}
