// ********************************************************************************************************
// JJSaraiva.TableList.AspNetCore.Mvc
// Author: Jimmy J. Saraiva
// Update: 08/06/2017
//
// ********************************************************************************************************

using System.Linq;

namespace JJSolutions.TableList.AspNetCore.TagHelper
{
    public static class PagedListExtenders
    {
        /// <summary>
        /// Extender to transform a source in PagedList, executing a fetch in Database
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="source">IQueryable of TEntity</param>
        /// <param name="page">Current page of PagedList</param>
        /// <param name="pageSize">Amount of lines of each page of PagedList</param>
        /// <returns>PagedList of TEntity</returns>
        public static PagedList<TEntity> ToPagedList<TEntity>(this IQueryable<TEntity> source, int page, int pageSize)
        {
            return new PagedList<TEntity>(source, page, pageSize);
        }
    }
}
