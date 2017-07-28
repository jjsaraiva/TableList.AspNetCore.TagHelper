// ********************************************************************************************************
// JJSaraiva.TableList.AspNetCore.Mvc
// Author: Jimmy J. Saraiva
// Update: 07/08/2017
//
// ********************************************************************************************************

namespace JJSolutions.TableList.AspNetCore.TagHelper
{
    /// <summary>
    /// Interface for PagedList
    /// </summary>
    public interface IPagedList
    {
        int TotalCount { get; }
        int PageCount { get; }
        int Page { get; }
        int PageSize { get; }
    }
}
