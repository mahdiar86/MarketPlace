using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.Paging
{
    public static class PagingExtension
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, BasePaging paging)
        {
            return query.Skip(paging.SkipEntity).Take(paging.TakeEntity);
        }

        public static List<T> PagingList<T>(this List<T> query, BasePaging paging)
        {
            return query.Skip(paging.SkipEntity).Take(paging.TakeEntity).ToList();
        }
    }
}
