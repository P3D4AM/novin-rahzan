using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.App;

internal static class StiAppHelper
{
	public static bool IsScreenPresent(StiReport report)
	{
		return report?.Pages.Cast<StiPage>().Any((StiPage p) => p.IsScreen && p.IsEnabled) ?? false;
	}

	public static List<IStiScreen> GetFormUIs(StiReport report)
	{
		return report?.Pages.OfType<IStiScreen>().ToList();
	}
}
