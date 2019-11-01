using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;

namespace Services
{

    public static class SortService
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string strSort)
        {
            if (strSort == null)
            {
                return source;
            }

            var lstSort = strSort.Split(',');

            string sortExpression = string.Empty;

            foreach (var sortOption in lstSort)
            {
                if (sortOption.StartsWith("-"))
                {
                    sortExpression = sortExpression + sortOption.Remove(0, 1) + " descending,";
                }
                else
                {
                    sortExpression = sortExpression + sortOption + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(sortExpression))
            {
                source = source.OrderBy(sortExpression.Remove(sortExpression.Count() - 1));
            }

            return source;
        }
    }

}
