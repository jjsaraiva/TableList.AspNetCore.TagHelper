using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

/*

    <jjsolutions-table-list model="@Model" asp-controller="CategoriaNoticia" asp-action="Index" return-url="@returnUrl">
        <table-columns>
            <table-column asp-for="CategoriaNoticiaId" header-style="width: 100px;" custom-link="/CategoriaNoticia/Details/{0}" />
            <table-column asp-for="Descr" />
        </table-columns>
        <table-buttons header-style="width: 100px;">
            <table-button title="Detalhes da categoria" icon-class="fa fa-search" asp-action="Details" asp-route-id="CategoriaNoticiaId" on-click="showProgress();" />
            <table-button title="Editar categoria" icon-class="fa fa-edit" asp-action="Edit" asp-route-id="CategoriaNoticiaId" on-click="showProgress();" />
            <table-button title="Excluir categoria" icon-class="fa fa-times" class="text-danger" asp-action="Delete" asp-route-id="CategoriaNoticiaId" on-click="showProgress();" />
        </table-buttons>
        <table-settings>
            <search-settings search-string="@ViewBag.SearchString" record-count="@ViewBag.RecordCount" />
            <sort-settings sort-order="@ViewBag.SortOrder" sort-direction="@ViewBag.SortDirection" />
            <legend-setttings />
            <pagination-setttings current-page="@ViewBag.CurrentPage" page-count="@ViewBag.PageCount" />
        </table-settings>
    </jjsolutions-table-list>


*/

namespace JJSolutions.TableList.AspNetCore.TagHelper
{
    [RestrictChildren("table-columns","table-buttons","table-settings")]
    //[HtmlTargetElement("jjsolutions-table-list", Attributes = "model, class, style, asp-action, asp-controller, return-url")]
    public class JjsolutionsTableListTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        private ParentContext _parentContext;

        [HtmlAttributeName("id")]
        public string Id { get; set; } = "jjsolution-table";

        [HtmlAttributeName("model")]
        public object Model { get; set; }

        [HtmlAttributeName("class")]
        public string Class { get; set; } = "table table-striped table-condensed table-bordered";

        [HtmlAttributeName("style")]
        public string Style { get; set; } = "";

        [HtmlAttributeName("asp-action")]
        public string AspAction { get; set; }

        [HtmlAttributeName("asp-controller")]
        public string AspController { get; set; }

        [HtmlAttributeName("return-url")]
        public string ReturnUrl { get; set; } = "";

        public override void Init(TagHelperContext context)
        {
            _parentContext = new ParentContext
            {
                Id = Id,
                Model = Model,
                Class = Class,
                Style = Style,
                AspAction = AspAction,
                AspController = AspController,
                ReturnUrl = System.Net.WebUtility.UrlEncode(ReturnUrl ?? "")
        };
            context.Items.Add(typeof(JjsolutionsTableListTagHelper), _parentContext);
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Encode return Url
            ReturnUrl = System.Net.WebUtility.UrlEncode(ReturnUrl ?? "");

            // set defaults and validations
            if (Model == null)
                throw new Exception("No Model specified.");

            if (string.IsNullOrEmpty(AspAction))
                throw new Exception("Default asp-action not specified.");

            if (string.IsNullOrEmpty(AspController))
                throw new Exception("Default asp-controller not specified.");

            // Get all child elements and load in _parentContext
            await output.GetChildContentAsync();

            // at this point all components are loaded in parentContex, now lets render the elements

            // render main div
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            
            // search
            if (_parentContext.SearchSettings.AllowSearch)
            {
                output.Content.AppendHtml(new RenderSearch(_parentContext).Render());
            }

            // if no records found, table not displayed
            if (((IList)Model).Count == 0)
            {
                output.TagName = "div";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Content.AppendHtml("<h4 class=\"text-danger\">Nenhum registro encontrado!</h4>");
                return;
            }

            // table
            var table = new TagBuilder("table") {TagRenderMode = TagRenderMode.Normal};
            table.Attributes.Add("id", Id);
            table.Attributes.Add("class", Class);
            table.Attributes.Add("style", Style);

            // table-header
            table.InnerHtml.AppendHtml(new RenderTableHeader(_parentContext).Render());

            // table-body
            table.InnerHtml.AppendHtml(new RenderTableBody(_parentContext).Render());

            output.Content.AppendHtml(table);

            // legend
            if (_parentContext.LegendSettings.ShowLegend)
            {
                output.Content.AppendHtml(new RenderLegend(_parentContext).Render());
            }

            // pagination
            if (_parentContext.PaginationSettings.AllowPagination)
            {
                output.Content.AppendHtml(new RenderPagination(_parentContext).Render());
            }

        }
    }
}
