using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Html;
using TagBuilder = Microsoft.AspNetCore.Mvc.Rendering.TagBuilder;
using System.Reflection;

namespace JJSolutions.TableList.AspNetCore.TagHelper
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
            object model = ((IList)_parentContext.Model)[0];

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

                    a.InnerHtml.SetHtmlContent(RenderCaption(model, tc));

                    // generate link href
                    var link = $"/{_parentContext.AspController}/{_parentContext.AspAction}/?";


                    link += $"searchString={_parentContext.SearchSettings.SearchString}";
                    link += $"&sortOrder={tc.SpecialSort ?? tc.AspFor}";
                    link += $"&sortDirection={(_parentContext.SortSettings.SortOrder == tc.SpecialSort || _parentContext.SortSettings.SortOrder == tc.AspFor ? (_parentContext.SortSettings.SortDirection == "desc" ? "asc" : "desc") : "asc")}";
                    link += $"&page={_parentContext.PaginationSettings.CurrentPage}";
                    link += $"&returnUrl={_parentContext.ReturnUrl}";

                    // other parameters
                    foreach (var route in tc.Routes)
                    {
                        link += $"&{route.Key.Replace("asp-route-", "")}={route.Value}";
                    }

                    a.Attributes.Add("href", link);

                    if (_parentContext.SortSettings.SortOrder == tc.SpecialSort || _parentContext.SortSettings.SortOrder == tc.AspFor)
                    {
                        th.InnerHtml.AppendHtml($"<i class=\"fa fa-sort-alpha-{(_parentContext.SortSettings.SortDirection == "desc" ? "desc" : "asc")} pull-right\" style=\"padding: 3px 3px 0 0;\"></i>&nbsp;");
                    }
                    th.InnerHtml.AppendHtml(a);
                }
                else
                {
                    // mostra somente os captions, sem link
                    th.InnerHtml.SetHtmlContent(RenderCaption(model, tc));
                }
                tr.InnerHtml.AppendHtml(th);
            }

            // Add Buttons Column
            if (_parentContext.TableButtons.Count > 0)
            {
                var th = new TagBuilder("th");
                th.Attributes.Add("style", $"text-align: center; {_parentContext.ButtonHeaderStyle}");
                th.Attributes.Add("class", _parentContext.ButtonHeaderClass);
                th.InnerHtml.SetHtmlContent(_parentContext.ButtonHeaderTitle);
                tr.InnerHtml.AppendHtml(th);
            }

            output.InnerHtml.AppendHtml(tr);

            return output;
        }

        // get original column name with DisplayAttribute Data Annotation if available
        private string RenderCaption(object model, TableColumnTagHelper tc)
        {
            var columnName = tc.Title;
            if (string.IsNullOrEmpty(columnName))
            {
                // search for DisplayAttribute Data Annotation
                //var displayName = model.GetType().GetInterfaces().First(x => x.Name.Contains("IList")).GenericTypeArguments.First(x => x.Name.Contains("ViewModel"))
                var displayName = model.GetType()
                    .GetProperties()
                    .First(x => x.Name == tc.AspFor)
                    .CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "DisplayAttribute");

                if (displayName == null)
                {
                    // if no DisplayAttribute, get the original column value
                    //columnName = model.GetType().GetInterfaces().First(x => x.Name.Contains("IList")).GenericTypeArguments.First(x => x.Name.Contains("ViewModel"))
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

            return columnName;
        }
    }
}