// ********************************************************************************************************
// JJSaraiva.TableList.AspNetCore.Mvc
// Author: Jimmy J. Saraiva
// Update: 08/06/2017
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Linq;

namespace JJSolutions.TableList.AspNetCore.TagHelper
{
    public class PagedList<TEntity> : List<TEntity>, IPagedList
    {
        public int TotalCount { get; }
        public int PageCount { get; }
        public int Page { get; }
        public int PageSize { get; }

        /// <summary>
        ///  Simple constructor of PagedList with default values
        /// </summary>
        public PagedList()
        {
            PageCount = 1;
            Page = 1;
            PageSize = 10;
        }

        /// <summary>
        /// Constructor of PagedList
        /// </summary>
        /// <param name="source">DataSource</param>
        /// <param name="page">Current page of PagedList</param>
        /// <param name="pageSize">Amount of lines in each page</param>
        public PagedList(IQueryable<TEntity> source, int page, int pageSize)
        {
            TotalCount = source.Count();
            PageCount = GetPageCount(pageSize, TotalCount);
            Page = page < 1 ? 0 : page - 1;
            PageSize = pageSize;

            AddRange(source.Skip(Page * PageSize).Take(PageSize).ToList());
        }

        /// <summary>
        /// Calculate de number of pages the PagedList will create
        /// </summary>
        /// <param name="pageSize">Amount of linhes in each page</param>
        /// <param name="totalCount">Number of pages in PagedList</param>
        /// <returns>int</returns>
        private int GetPageCount(int pageSize, int totalCount)
        {
            if (pageSize == 0)
                return 0;

            var remainder = totalCount % pageSize;
            return (totalCount / pageSize) + (remainder == 0 ? 0 : 1);
        }
    }
}