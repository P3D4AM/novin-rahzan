using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Cards;

public static class StiCardsColumnVisibilityHelper
{
	public static bool GetVisible(IStiCardsColumn column, IStiReport report)
	{
		if (column.Visibility == StiCardsColumnVisibility.True)
		{
			return true;
		}
		if (column.Visibility == StiCardsColumnVisibility.False)
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
