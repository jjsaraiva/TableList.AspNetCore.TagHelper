using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace JJSolutions.TableList.AspNet.TagHelper
{

    [RestrictChildren("search-settings", "sort-settings", "legend-setttings", "pagination-setttings")]
    public class TableSettingsTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // busca todos os elementos filhos
            output.SuppressOutput();
            await output.GetChildContentAsync();
        }
    }


    [HtmlTargetElement("search-settings", ParentTag = "table-settings", TagStructure = TagStructure.NormalOrSelfClosing)] 
    public class SearchSettingsTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        [HtmlAttributeNotBound]
        public bool AllowSearch { get; set; } = false;

        [HtmlAttributeName("search-string")]
        public string SearchString { get; set; } = "";

        [HtmlAttributeName("record-count")]
        public int RecordCount { get; set; } = 0;

        [HtmlAttributeName("class")]
        public string Class { get; set; } = "";

        [HtmlAttributeName("style")]
        public string Style { get; set; } = "";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var parentContext = (ParentContext)context.Items[typeof(JjsolutionsTableListTagHelper)];
            parentContext.SearchSettings.AllowSearch = true; // se passou aqui é porque tem que mostrar
            parentContext.SearchSettings.SearchString = SearchString;
            parentContext.SearchSettings.RecordCount = RecordCount;
            parentContext.SearchSettings.Class = Class;
            parentContext.SearchSettings.Style = Style;
        }
    }

    [HtmlTargetElement("sort-settings", ParentTag = "table-settings", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SortSettingsTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        [HtmlAttributeNotBound]
        public bool AllowSort { get; set; } = false;

        [HtmlAttributeName("sort-order")]
        public string SortOrder { get; set; }

        [HtmlAttributeName("sort-direction")]
        public string SortDirection { get; set; } = "asc";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var parentContext = (ParentContext)context.Items[typeof(JjsolutionsTableListTagHelper)];
            parentContext.SortSettings.AllowSort = true;
            parentContext.SortSettings.SortOrder = SortOrder;
            parentContext.SortSettings.SortDirection = SortDirection;
        }
    }

    [HtmlTargetElement("legend-setttings", ParentTag = "table-settings", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class LegendSettingsTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        [HtmlAttributeNotBound]
        public bool ShowLegend { get; set; } = false;

        [HtmlAttributeName("title")]
        public string Title { get; set; } = "Legenda";

        [HtmlAttributeName("class")]
        public string Class { get; set; } = "text-link";

        [HtmlAttributeName("style")]
        public string Style { get; set; } = "";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var parentContext = (ParentContext)context.Items[typeof(JjsolutionsTableListTagHelper)];
            parentContext.LegendSettings.ShowLegend = true;
            parentContext.LegendSettings.Title = Title;
            parentContext.LegendSettings.Class = Class;
            parentContext.LegendSettings.Style = Style;
        }
    }

    [HtmlTargetElement("rows-setttings", ParentTag = "table-settings", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class RowsSettingsTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        [HtmlAttributeName("show-header")]
        public bool ShowHeader { get; set; } = true;

        [HtmlAttributeName("row-script")]
        public string RowScript { get; set; } = "";  // use {0} to get row id

        [HtmlAttributeName("on-click")]
        public string OnClick { get; set; } = ""; // on-click on row

        [HtmlAttributeName("class")]
        public string Class { get; set; } = "";

        [HtmlAttributeName("style")]
        public string Style { get; set; } = "";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var parentContext = (ParentContext)context.Items[typeof(JjsolutionsTableListTagHelper)];
            parentContext.RowsSettings.ShowHeader = ShowHeader;
            parentContext.RowsSettings.RowScript = RowScript;
            parentContext.RowsSettings.OnClick = OnClick;
            parentContext.RowsSettings.Class = Class;
            parentContext.RowsSettings.Style = Style;
        }
    }


    [HtmlTargetElement("pagination-setttings", ParentTag = "table-settings", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PaginationSettingsTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        [HtmlAttributeNotBound]
        public bool AllowPagination { get; set; } = false;

        [HtmlAttributeName("page-count")]
        public int PageCount { get; set; } = 1;  

        [HtmlAttributeName("current-page")]
        public int CurrentPage { get; set; } = 1; 

        [HtmlAttributeName("class")]
        public string Class { get; set; } = "pagination pagination-sm";

        [HtmlAttributeName("style")]
        public string Style { get; set; } = "";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var parentContext = (ParentContext)context.Items[typeof(JjsolutionsTableListTagHelper)];
            parentContext.PaginationSettings.AllowPagination = true;
            parentContext.PaginationSettings.PageCount = PageCount;
            parentContext.PaginationSettings.CurrentPage = CurrentPage;
            parentContext.PaginationSettings.Class = Class;
            parentContext.PaginationSettings.Style = Style;
        }
    }

}