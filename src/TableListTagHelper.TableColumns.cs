using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace JJSolutions.TableList.AspNet.TagHelper
{
    [RestrictChildren("table-column"), Serializable]
    public class TableColumnsTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // busca todos os elementos filhos
            output.SuppressOutput();
            await output.GetChildContentAsync();
        }
    }

    [HtmlTargetElement("table-column", ParentTag = "table-columns", TagStructure = TagStructure.NormalOrSelfClosing)] 
    public class TableColumnTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public string AspFor { get; set; }

        [HtmlAttributeName("title")]
        public string Title { get; set; }

        [HtmlAttributeName("visible")]
        public bool Visible { get; set; } = true;

        [HtmlAttributeName("header-class")]
        public string HeaderClass { get; set; } = "bg-info";

        [HtmlAttributeName("column-class")]
        public string ColumnClass { get; set; }

        [HtmlAttributeName("header-style")]
        public string HeaderStyle { get; set; }

        [HtmlAttributeName("column-style")]
        public string ColumnStyle { get; set; }

        [HtmlAttributeName("no-sort")]
        public bool NoSort { get; set; } = false;

        [HtmlAttributeName("column-action")]
        public string ColumnAction { get; set; }

        [HtmlAttributeName("column-controller")]
        public string ColumnController { get; set; }

        [HtmlAttributeName("custom-link")]
        public string CustomLink { get; set; }

        [HtmlAttributeName("on-click")]
        public string OnClick { get; set; }

        [HtmlAttributeNotBound]
        public List<KeyValuePair<string, string>> Routes { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Routes = new List<KeyValuePair<string, string>>();
            foreach (var attribute in context.AllAttributes)
            {
                if (attribute.Name.Contains("asp-route"))
                {
                    Routes.Add(new KeyValuePair<string, string>(attribute.Name, attribute.Value.ToString()));
                }
            }

            var parentContext = (ParentContext)context.Items[typeof(JjsolutionsTableListTagHelper)];

            parentContext.TableColumns.Add(new TableColumnTagHelper
            {
                AspFor = AspFor,
                Title = Title,
                Visible = Visible,
                ColumnAction = ColumnAction,
                ColumnClass = ColumnClass,
                ColumnController = ColumnController,
                ColumnStyle = ColumnStyle,
                CustomLink = CustomLink,
                HeaderStyle = HeaderStyle,
                HeaderClass = HeaderClass,
                NoSort = NoSort,
                OnClick = OnClick,
                Routes = Routes
            });
        }
    }

}
