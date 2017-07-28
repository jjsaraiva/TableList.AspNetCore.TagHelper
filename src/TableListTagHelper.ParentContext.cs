using System.Collections.Generic;

namespace JJSolutions.TableList.AspNetCore.TagHelper
{
    public class ParentContext
    {
        // parent parameters
        public string Id { get; set; }
        public object Model { get; set; }
        public string Class { get; set; }
        public string Style { get; set; }
        public string AspAction { get; set; }
        public string AspController { get; set; }
        public string ReturnUrl { get; set; }

        public string ButtonHeaderTitle { get; set; }
        public string ButtonHeaderClass { get; set; }
        public string ButtonHeaderStyle { get; set; }
        public string ButtonColumnClass { get; set; }
        public string ButtonColumnStyle { get; set; }

        // children components
        public ICollection<TableColumnTagHelper> TableColumns { get; set; } = new List<TableColumnTagHelper>();
        public ICollection<TableButtonTagHelper> TableButtons { get; set; } = new List<TableButtonTagHelper>();
        public SearchSettingsTagHelper SearchSettings { get; set; } = new SearchSettingsTagHelper {AllowSearch = false};  // if not specify these tags, all are disabled by default
        public SortSettingsTagHelper SortSettings { get; set; } = new SortSettingsTagHelper {AllowSort = false};
        public LegendSettingsTagHelper LegendSettings { get; set; } = new LegendSettingsTagHelper {ShowLegend = false};
        public RowsSettingsTagHelper RowsSettings { get; set; } = new RowsSettingsTagHelper();
        public PaginationSettingsTagHelper PaginationSettings { get; set; } = new PaginationSettingsTagHelper {AllowPagination = false};
    }
}