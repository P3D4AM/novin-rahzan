using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Export.Helpers;

public static class StiBorderExportHelper
{
	public static void Render(IStiElement element, StiComponent component, StiDashboardExportSettings settings)
	{
		IStiBorder stiBorder = component as IStiBorder;
		if (element is IStiSimpleBorder && stiBorder != null && (settings == null || settings.RenderBorders))
		{
			stiBorder.Border = ((IStiSimpleBorder)element).Border.GetBorder();
			stiBorder.Border.Topmost = true;
		}
	}
}
