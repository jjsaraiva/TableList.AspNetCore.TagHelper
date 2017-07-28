using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace JJSolutions.TableList.AspNetCore.TagHelper
{
    [RestrictChildren("table-button")]
    public class TableButtonsTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        [HtmlAttributeName("header-class")]
        public string HeaderClass { get; set; } = "bg-info";

        [HtmlAttributeName("header-style")]
        public string HeaderStyle { get; set; }

        [HtmlAttributeName("column-class")]
        public string ColumnClass { get; set; }

        [HtmlAttributeName("column-style")]
        public string ColumnStyle { get; set; }

        [HtmlAttributeName("header-title")]
        public string HeaderTitle { get; set; } = "Ações";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // busca todos os elementos filhos
            output.SuppressOutput();
            var parentContext = (ParentContext)context.Items[typeof(JjsolutionsTableListTagHelper)];
            parentContext.ButtonHeaderClass = HeaderClass;
            parentContext.ButtonHeaderStyle = HeaderStyle;
            parentContext.ButtonColumnClass = ColumnClass;
            parentContext.ButtonColumnStyle = ColumnStyle;
            parentContext.ButtonHeaderTitle = HeaderTitle;

            await output.GetChildContentAsync();
        }
    }

    [HtmlTargetElement("table-button", ParentTag = "table-buttons", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class TableButtonTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        [HtmlAttributeName("title")]
        public string Title { get; set; }

        [HtmlAttributeName("class")]
        public string Class { get; set; }

        [HtmlAttributeName("icon-class")]
        public string IconClass { get; set; }

        [HtmlAttributeName("style")]
        public string Style { get; set; }

        [HtmlAttributeName("asp-action")]
        public string AspAction { get; set; }

        [HtmlAttributeName("asp-controller")]
        public string AspController { get; set; }

        [HtmlAttributeName("target")]
        public string Target { get; set; } = "_self";

        [HtmlAttributeName("custom-link")]
        public string CustomLink { get; set; }

        [HtmlAttributeName("custom-link-target")]
        public string CustomLinkTarget { get; set; } = "_self";

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
            parentContext.TableButtons.Add(new TableButtonTagHelper
            {
                Title = Title,
                Class = Class,
                IconClass = IconClass,
                Style = Style,
                AspController = AspController,
                AspAction = AspAction,
                CustomLink = CustomLink,
                OnClick = OnClick,
                Routes = Routes,
                Target = Target,
                CustomLinkTarget = CustomLinkTarget
            });

            output.SuppressOutput();
        }
    }

}