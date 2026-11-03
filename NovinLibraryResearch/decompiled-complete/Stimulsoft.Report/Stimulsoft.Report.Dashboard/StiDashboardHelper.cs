using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

internal static class StiDashboardHelper
{
	public static bool IsDashboardPresent(StiReport report)
	{
		return report?.Pages.Cast<StiPage>().Any((StiPage p) => p.IsDashboard && p.IsEnabled) ?? false;
	}

	public static List<IStiDashboard> GetDashboards(StiReport report)
	{
		return report?.Pages.OfType<IStiDashboard>().ToList();
	}
}
