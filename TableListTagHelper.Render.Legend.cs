using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JJSolutions.TableList.AspNet.TagHelper
{
    public class RenderLegend
    {
        private ParentContext _parentContext;

        public RenderLegend(ParentContext parentContext)
        {
            _parentContext = parentContext;
        }

        /// <summary>
        /// Render Legend of table, if have buttons
        /// </summary>
        /// <returns></returns>
        public TagBuilder Render()
        {
            var output = new TagBuilder("div");
            output.Attributes.Add("class", _parentContext.LegendSettings.Class);
            output.Attributes.Add("style", _parentContext.LegendSettings.Style);

            if (_parentContext.TableButtons.Count == 0)
                return output;

            output.InnerHtml.AppendHtml($"<strong>{_parentContext.LegendSettings.Title}:</strong>&nbsp;&nbsp;");

            // primeiro botão
            output.InnerHtml.AppendHtml($"<i class=\"{_parentContext.TableButtons.First().IconClass}\"></i>&nbsp;{_parentContext.TableButtons.First().Title}");

            foreach (var actionButton in _parentContext.TableButtons.Skip(1))
            {
                output.InnerHtml.AppendHtml($"&nbsp;&nbsp;|&nbsp;&nbsp;<i class=\"{actionButton.IconClass}\"></i>&nbsp;{actionButton.Title}");
            }

            return output;
        }
    }
}