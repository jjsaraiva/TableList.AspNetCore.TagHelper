using System;
using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace JJSolutions.TableList.AspNetCore.TagHelper
{
    public class RenderTableBody
    {
        private ParentContext _parentContext;

        public RenderTableBody(ParentContext parentContext)
        {
            _parentContext = parentContext;
        }

        public TagBuilder Render()
        {
            var output = new TagBuilder("tbody");
            int i = 1;
            foreach (var model in (IEnumerable) _parentContext.Model)
            {
                var tr = new TagBuilder("tr");
                tr.Attributes.Add("id", $"{_parentContext.Id}-row-{i}");

                foreach (var tc in _parentContext.TableColumns)
                {
                    if (!tc.Visible)
                        continue;

                    var td = new TagBuilder("td");
                    td.Attributes.Add("class", tc.ColumnClass);
                    td.Attributes.Add("style", tc.ColumnStyle);
                    td.Attributes.Add("name", tc.AspFor);
                    td.Attributes.Add("onclick", tc.OnClick);

                    foreach (var property in model.GetType().GetProperties())
                    {
                        // found column that work on
                        if (property.Name == tc.AspFor)
                        {
                            // column value
                            var columnValue = property.GetValue(model);

                            // search for DisplayFormat Data Annotation
                            // var displayFormat = model.GetType().GetInterfaces().First(x => x.Name.Contains("IList")).GenericTypeArguments.First(x => x.Name.Contains("ViewModel"))
                            var displayFormat = model.GetType()
                                .GetProperties()
                                .First(x => x.Name == tc.AspFor)
                                .CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "DisplayFormatAttribute");

                            if (displayFormat != null)
                            {
                                // get DisplayFormat attribute
                                var dataFormat = displayFormat
                                    .NamedArguments.First(x => x.MemberName == "DataFormatString")
                                    .TypedValue.Value.ToString();

                                // format the columnValue
                                columnValue = String.Format(dataFormat, columnValue);
                            }


                            if (property.GetValue(model).GetType() == typeof(System.Boolean))
                            {
                                // boolean simbol
                                td.InnerHtml.SetHtmlContent((bool) property.GetValue(model) ? "<i class=\"fa fa-check-square-o\">" : "<i class=\"fa fa-square-o\">");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(tc.CustomLink))
                                {
                                    // normal text
                                    td.InnerHtml.SetHtmlContent(columnValue.ToString().HighlightSearchString(_parentContext.SearchSettings.SearchString));
                                }
                                else
                                {
                                    // custom link
                                    var a = new TagBuilder("a");
                                    a.Attributes.Add("class", tc.ColumnClass);
                                    a.Attributes.Add("style", tc.ColumnStyle);
                                    a.Attributes.Add("target", tc.LinkTarget);

                                    var link = tc.CustomLink + (tc.CustomLink.Contains("?") ? "" : "?");
                                    link += $"&returnUrl={_parentContext.ReturnUrl}";

                                    // other parameters
                                    foreach (var route in tc.Routes)
                                    {
                                        // replace ColumnName for ColumnValue
                                        link += $"&{route.Key.Replace("asp-route-", "")}={model.GetType().GetProperties().First(x => x.Name == route.Value).GetValue(model)}";
                                    }

                                    var linkParametes = RenderUtils.GetCustomLinkParamters(tc.CustomLink);

                                    // get value of CustomLinkFor
                                    foreach (var linkParameter in linkParametes)
                                    {
                                        foreach (var p in model.GetType().GetProperties())
                                        {
                                            if (p.Name == linkParameter)
                                                link = link.Replace($"{{{linkParameter}}}", p.GetValue(model).ToString()); // replace {xxx} argument
                                        }
                                    }

                                    a.Attributes.Add("href", link);
                                    a.InnerHtml.SetHtmlContent(columnValue.ToString().HighlightSearchString(_parentContext.SearchSettings.SearchString));
                                    td.InnerHtml.SetHtmlContent(a);
                                }
                            }

                            break;
                        }
                    }

                    // check for javascript per row
                    if (!string.IsNullOrEmpty(_parentContext.RowsSettings.RowScript))
                    {
                        // insert row-id into {0} argumento if specified
                        tr.InnerHtml.AppendHtml(string.Format(_parentContext.RowsSettings.RowScript, $"{_parentContext.Id}-row-{i}"));
                    }

                    tr.InnerHtml.AppendHtml(td);
                }

                // Add Buttons
                if (_parentContext.TableButtons.Count > 0)
                {                    
                    var buttonsRender = new RenderButtons(_parentContext, model);
                    tr.InnerHtml.AppendHtml(buttonsRender.Render());
                }

                output.InnerHtml.AppendHtml(tr);
            }


            return output;
        }
    }
}