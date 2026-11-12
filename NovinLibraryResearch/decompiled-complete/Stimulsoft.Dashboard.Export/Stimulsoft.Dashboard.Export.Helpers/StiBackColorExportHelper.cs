using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Export.Helpers;

public static class StiBackColorExportHelper
{
	public static void Render(IStiElement element, StiComponent component)
	{
		if (component is IStiBackColor)
		{
			((IStiBackColor)component).BackColor = StiDashboardStyleHelper.GetBackColor(element);
		}
	}
}
