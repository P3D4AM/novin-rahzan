namespace Stimulsoft.Report.Dashboard.Helpers;

internal static class StiUserViewStatesMenuHelper
{
	public static bool IsAllowUserViewStatesChanging(IStiElement element)
	{
		if (element is IStiChartElement stiChartElement)
		{
			return stiChartElement.DataMode == StiDataMode.UsingDataFields;
		}
		return false;
	}
}
