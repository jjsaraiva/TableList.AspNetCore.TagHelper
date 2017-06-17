using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace JJSolutions.TableList.AspNet.TagHelper
{
    public static class RenderUtils
    {
        /// <summary>
        /// Wraps matched strings in HTML span elements styled with a background-color
        /// </summary>
        /// <param name="text"></param>
        /// <param name="keywords">Comma-separated list of strings to be highlighted</param>
        /// <param name="cssClass">The Css color to apply</param>
        /// <param name="fullMatch">false for returning all matches, true for whole word matches only</param>
        /// <returns>string</returns>
        public static string HighlightSearchString(this string text, string keywords, bool fullMatch = false, string cssClass = "highlight")
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keywords))
                return text;

            var words = keywords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (!fullMatch)
                return words.Select(word => word.Trim()).Aggregate(text,
                   (current, pattern) =>
                      Regex.Replace(current,
                         pattern,
                          $"<span class=\"{cssClass}\">$0</span>",
                         RegexOptions.IgnoreCase));

            return words.Select(word => "\\b" + word.Trim() + "\\b")
               .Aggregate(text, (current, pattern) =>
                  Regex.Replace(current,
                     pattern,
                      $"<span class=\"{cssClass}\">$0</span>",
                     RegexOptions.IgnoreCase));

        }
    }
}