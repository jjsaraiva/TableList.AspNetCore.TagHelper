using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JJSolutions.TableList.AspNetCore.TagHelper
{
    public class RenderSearch
    {
        private ParentContext _parentContext;

        public RenderSearch(ParentContext parentContext)
        {
            _parentContext = parentContext;
        }


        /// <summary>
        /// Render Search Form
        /// </summary>
        /// <returns></returns>
        public TagBuilder Render()
        {
            // master tag
            var output = new TagBuilder("form");

            // attributes for master tag
            output.Attributes.Add("action", $"/{_parentContext.AspController}/{_parentContext.AspAction}?returnUrl={_parentContext.ReturnUrl}");
            output.Attributes.Add("method", "get");

            // div form-horizontal
            var divFormHorizontal = new TagBuilder("div");
            divFormHorizontal.Attributes.Add("class", "form-horizontal");

            // div input-group
            var divInputGroup = new TagBuilder("div");
            divInputGroup.Attributes.Add("class", "input-group col-md-6");

            // add elements to input-group
            divInputGroup.InnerHtml.AppendHtml($@"<input name=""SearchString"" type=""text"" class=""form-control"" placeholder=""Localizar"" style=""max-width: 100% !important;"" value=""{_parentContext.SearchSettings.SearchString}"">");
            divInputGroup.InnerHtml.AppendHtml(@"<span class=""input-group-btn"">
                                                    <button type=""submit"" class=""btn btn-default""><i class=""fa fa-search""></i></button>
                                                 </span>");

            // add divInputGroup to divFormHorizontal
            divFormHorizontal.InnerHtml.AppendHtml(divInputGroup);

            // add divFormHorizontal to output
            output.InnerHtml.AppendHtml(divFormHorizontal);

            // div welll
            var divWell = new TagBuilder("div");
            divWell.Attributes.Add("class", "well well-sm");

            if (!string.IsNullOrEmpty(_parentContext.SearchSettings.SearchString))
            {
                // clear searchString
                divWell.InnerHtml.AppendHtml($@"<em>Palavra chave:&nbsp;{_parentContext.SearchSettings.SearchString}</em>&nbsp;&nbsp;
                                                <span><a class=""text-danger"" href=""{FormatClearHref()}"">&nbsp;<i class=""fa fa-close"" ></i>&nbsp;Limpar</a></span>");
            }
            else
            {
                divWell.InnerHtml.AppendHtml(@"<em>&nbsp</em>"); // expand well bootstrap component
            }

            divWell.InnerHtml.AppendHtml($@"<span class=""pull-right"">{_parentContext.SearchSettings.RecordCount} itens encontrados</span>");

            // add divWell to output
            output.InnerHtml.AppendHtml(divWell);


            return output;
        }

        private string FormatClearHref()
        {
            return (_parentContext.AspAction.IndexOf("?", StringComparison.Ordinal) == -1 ? $"/{_parentContext.AspController}/{_parentContext.AspAction}?searchString=" : $"/{_parentContext.AspController}/{_parentContext.AspAction}&searchString=") + $"&returnUrl={_parentContext.ReturnUrl}";
        }
    }
}