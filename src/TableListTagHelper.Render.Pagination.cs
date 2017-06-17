using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JJSolutions.TableList.AspNet.TagHelper
{
    public class RenderPagination
    {
        private ParentContext _parentContext;
        private string _link;
        private int _buttonCount = 0;

        public RenderPagination(ParentContext parentContext)
        {
            _parentContext = parentContext;
            
            // generate base link href
            _link = $"/{_parentContext.AspController}/{_parentContext.AspAction}/?";
            _link += $"searchString={_parentContext.SearchSettings.SearchString}";
            _link += $"&sortOrder={_parentContext.SortSettings.SortOrder}";
            _link += $"&sortDirection={_parentContext.SortSettings.SortDirection}";
        }

        public TagBuilder Render()
        {
            if (_parentContext.PaginationSettings.PageCount <= 1)
                return new TagBuilder("span");

            var output = new TagBuilder("nav");
            var ul = new TagBuilder("ul");
            ul.Attributes.Add("class", _parentContext.PaginationSettings.Class);

            // first page always visible
            AddPage(ul, "Primeira Página", "1", 1);

            if (_parentContext.PaginationSettings.CurrentPage <= 5)
            {
                for (int i = 2; i <= Math.Min(5, _parentContext.PaginationSettings.PageCount - 1); i++)
                {
                    // create page 2 till 5 or less
                    AddPage(ul, $"Página {i}", i.ToString(), i);
                }
                if (_parentContext.PaginationSettings.PageCount > 5)
                {
                    // if have more than 5 pages, include ... to go to page 6 and forward
                    AddPage(ul, "Próximas páginas", "...", 6);
                }
            }
            else
            {
                // previous pages
                AddPage(ul, "Páginas anteriores", "...", _parentContext.PaginationSettings.CurrentPage - 1);

                if (_parentContext.PaginationSettings.PageCount < 10)
                {
                    // when have less than 10 pages, show more numbers after ...
                    var limite = _parentContext.PaginationSettings.CurrentPage - 5;
                    for (int i = _parentContext.PaginationSettings.CurrentPage - limite; i <= Math.Min(_parentContext.PaginationSettings.CurrentPage + (4 - limite), _parentContext.PaginationSettings.PageCount - 1); i++)
                    {
                        AddPage(ul, $"Página {i}", i.ToString(), i);
                    }
                }
                else
                {
                    // generate page 6 and forward
                    for (int i = _parentContext.PaginationSettings.CurrentPage; i <= Math.Min(_parentContext.PaginationSettings.CurrentPage + 4, _parentContext.PaginationSettings.PageCount - 1); i++)
                    {
                        AddPage(ul, $"Página {i}", i.ToString(), i);
                    }
                }
                if (_parentContext.PaginationSettings.CurrentPage + 5 <= _parentContext.PaginationSettings.PageCount)
                {
                    // if have more pages, include ... to next pages
                    AddPage(ul, "Próximas páginas", "...", _parentContext.PaginationSettings.CurrentPage + 5);
                }
            }

            // Check button count, must have at least 6 buttons
            if (_buttonCount < 6)
            {
                // clear and rebuild the pagination
                ul.InnerHtml.Clear();
                _buttonCount = 0;
                AddPage(ul, "Primeira Página", "1", 1);
                AddPage(ul, "Páginas anteriores", "...", _parentContext.PaginationSettings.PageCount - (6 - _buttonCount));
                for (int i = _parentContext.PaginationSettings.PageCount - (6 - _buttonCount); _buttonCount < 6; i++)
                {
                    AddPage(ul, $"Página {i}", i.ToString(), i);
                }
            }

            // Last page always visible
            AddPage(ul, "Última página", _parentContext.PaginationSettings.PageCount.ToString(), _parentContext.PaginationSettings.PageCount);

            output.InnerHtml.AppendHtml(ul);
            return output;
        }

        /// <summary>
        /// Insert li element in ul
        /// </summary>
        /// <param name="ul">Parent ul to insert</param>
        /// <param name="title">Title of button</param>
        /// <param name="value">Value of button (Text)</param>
        /// <param name="page">Page number of button index</param>
        private void AddPage(TagBuilder ul, string title, string value, int page)
        {
            var li = new TagBuilder("li");
            if (_parentContext.PaginationSettings.CurrentPage == page)
                li.Attributes.Add("class", "active");

            var a = new TagBuilder("a");
            a.Attributes.Add("aria-label", title);
            a.Attributes.Add("title", title);
            a.Attributes.Add("data-page", page.ToString());
            a.Attributes.Add("href", $"{_link}&page={page}");
            a.InnerHtml.SetHtmlContent(value);

            li.InnerHtml.AppendHtml(a);
            ul.InnerHtml.AppendHtml(li);
            _buttonCount++;
        }
    }
}