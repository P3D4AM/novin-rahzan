using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Table;

public static class StiTableColumnVisibilityHelper
{
	public static bool GetVisible(IStiTableColumn column, IStiReport report)
	{
		if (column.Visibility == StiTableColumnVisibility.True)
		{
			return true;
		}
		if (column.Visibility == StiTableColumnVisibility.False)
		{
			return false;
		}
		if (string.IsNullOrWhiteSpace(column.VisibilityExpression))
		{
			return false;
		}
		string value = StiReportParser.Parse(column.VisibilityExpression, report);
		return StiValueHelper.TryToBool(value);
	}
}
