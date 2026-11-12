using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Controls.Helpers;

internal static class StiUserFiltersHelper
{
	internal static bool IsFilterPresent(IStiElement element)
	{
		if (!(element is IStiUserFilters stiUserFilters))
		{
			return false;
		}
		if (stiUserFilters.UserFilters != null)
		{
			return stiUserFilters.UserFilters.Count > 0;
		}
		return false;
	}
}
