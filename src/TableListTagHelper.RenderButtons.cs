using System.Linq;
using Microsoft.AspNetCore.Html;
using TagBuilder = Microsoft.AspNetCore.Mvc.Rendering.TagBuilder;
using System.Reflection;

namespace JJSolutions.TableList.AspNetCore.TagHelper
{
    public class RenderButtons
    {
        private ParentContext _parentContext;
        private object _model;

        public RenderButtons(ParentContext parentContext, object model)
        {
            _parentContext = parentContext;
            _model = model;
        }


        public TagBuilder Render()
        {
            var td = new TagBuilder("td");
            td.Attributes.Add("style", $"text-align: center; {_parentContext.ButtonColumnStyle}");
            td.Attributes.Add("class", _parentContext.ButtonColumnClass);

            foreach (var tableButton in _parentContext.TableButtons)
            {
                var a = new TagBuilder("a");
                a.Attributes.Add("title", tableButton.Title);
                a.Attributes.Add("class", tableButton.Class);
                a.Attributes.Add("style", tableButton.Style);
                a.Attributes.Add("onclick", tableButton.OnClick);
                a.InnerHtml.SetHtmlContent($"<i class=\"{tableButton.IconClass}\"></i>&nbsp;");

                var link = "";
                if (string.IsNullOrEmpty(tableButton.CustomLink))
                {
                    a.Attributes.Add("target", tableButton.Target);
                    link = $"/{tableButton.AspController ?? _parentContext.AspController}/{tableButton.AspAction ?? _parentContext.AspAction}/?";
                    link += $"returnUrl={_parentContext.ReturnUrl}";

                    // other parameters
                    foreach (var route in tableButton.Routes)
                    {
                        // replace ColumnName for ColumnValue
                        link += $"&{route.Key.Replace("asp-route-", "")}={_model.GetType().GetProperties().First(x => x.Name ==  route.Value).GetValue(_model)}";
                    }
                }
                else
                {
                    a.Attributes.Add("target", tableButton.CustomLinkTarget);
                    link = tableButton.CustomLink + (tableButton.CustomLink.Contains("?") ? "" : "?");

                    // other parameters
                    foreach (var route in tableButton.Routes)
                    {
                        // replace ColumnName for ColumnValue
                        link += $"&{route.Key.Replace("asp-route-", "")}={_model.GetType().GetProperties().First(x => x.Name == route.Value).GetValue(_model)}";
                    }
                }

                a.Attributes.Add("href", link);
                td.InnerHtml.AppendHtml(a);
                td.InnerHtml.AppendHtml("&nbsp;");
            }

            return td;
        }
    }
}