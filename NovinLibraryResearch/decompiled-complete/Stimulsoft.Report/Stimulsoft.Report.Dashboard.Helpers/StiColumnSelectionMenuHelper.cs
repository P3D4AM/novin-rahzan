namespace Stimulsoft.Report.Dashboard.Helpers;

internal static class StiColumnSelectionMenuHelper
{
	public static bool IsAllowUserColumnsSelection(IStiElement element)
	{
		if ((element as IStiElementInteraction)?.DashboardInteraction is IStiAllowUserColumnSelectionDashboardInteraction stiAllowUserColumnSelectionDashboardInteraction)
		{
			return stiAllowUserColumnSelectionDashboardInteraction.AllowUserColumnSelection;
		}
		return false;
	}
}
