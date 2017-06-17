using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Html;
using TagBuilder = Microsoft.AspNetCore.Mvc.Rendering.TagBuilder;

namespace JJSolutions.TableList.AspNet.TagHelper
{
    public class RenderTableHeader
    {
        private ParentContext _parentContext;

        public RenderTableHeader(ParentContext parentContext)
        {
            _parentContext = parentContext;
        }

        /// <summary>
        /// Render table header
        /// </summary>
        /// <returns></returns>
        public TagBuilder Render()
        {
            if (!_parentContext.RowsSettings.ShowHeader)
                return new TagBuilder("span");

            var output = new TagBuilder("thead");
            var tr = new TagBuilder("tr");

            // Get First Model item
            object model = null;
            foreach (var entity in (IEnumerable) _parentContext.Model)
            {
                model = entity;
                break;                
            }

            foreach (var tc in _parentContext.TableColumns)
            {
                if (!tc.Visible)
                    continue;

                var th = new TagBuilder("th");
                th.Attributes.Add("class", tc.HeaderClass);
                th.Attributes.Add("style", tc.HeaderStyle);
                th.Attributes.Add("onclick", tc.OnClick);

                if (_parentContext.SortSettings.AllowSort && !tc.NoSort)
                {
                    // inser as routes sortOrder and sortDirection
                    var a = new TagBuilder("a");

                    // get original column name with DisplayAttribute Data Annotation if available
                    var columnName = tc.Title;
                    if (string.IsNullOrEmpty(columnName))
                    {
                        // search for DisplayAttribute Data Annotation
                        var displayName = model.GetType()
                            .GetProperties()
                            .First(x => x.Name == tc.AspFor)
                            .CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "DisplayAttribute");

                        if (displayName == null)
                        {
                            // if no DisplayAttribute, get the original column value
                            columnName = model.GetType()
                                .GetProperties()
                                .First(x => x.Name == tc.AspFor).Name;
                        }
                        else
                        {
                            // get Display attribute
                            columnName = displayName
                                .NamedArguments.First(x => x.MemberName == "Name")
                                .TypedValue.Value.ToString();
                        }
                    }                    

                    a.InnerHtml.SetHtmlContent(columnName);

                    // generate link href
                    var link = $"/{_parentContext.AspController}/{_parentContext.AspAction}/?";


                    link += $"searchString={_parentContext.SearchSettings.SearchString}";
                    link += $"&sortOrder={tc.AspFor}";
                    link += $"&sortDirection={(_parentContext.SortSettings.SortOrder == tc.AspFor ? "desc" : "asc")}";
                    link += $"&page={_parentContext.PaginationSettings.CurrentPage}";

                    // other parameters
                    foreach (var route in tc.Routes)
                    {
                        link += $"&{route.Key.Replace("asp-route-", "")}={route.Value}";
                    }

                    a.Attributes.Add("href", link);

                    if (_parentContext.SortSettings.SortOrder == tc.AspFor)
                    {
                        th.InnerHtml.AppendHtml($"<i class=\"fa fa-sort-alpha-{(_parentContext.SortSettings.SortDirection == "desc" ? "desc" : "asc")} pull-right\" style=\"padding: 3px 3px 0 0;\"></i>&nbsp;");
                    }
                    th.InnerHtml.AppendHtml(a);
                }
                else
                {
                    // mostra somente os captions, sem link
                    th.InnerHtml.SetHtmlContent(tc.Title);
                }
                tr.InnerHtml.AppendHtml(th);
            }

            // Add Buttons Column
            if (_parentContext.TableButtons.Count > 0)
            {
                var th = new TagBuilder("th");
                th.Attributes.Add("style", $"text-align: center; {_parentContext.ButtonHeaderStyle}");
                th.Attributes.Add("class", _parentContext.ButtonHeaderClass);
                th.InnerHtml.SetHtmlContent("Ações");
                tr.InnerHtml.AppendHtml(th);
            }

            output.InnerHtml.AppendHtml(tr);


            return output;
        }
    }
}