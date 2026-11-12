using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiPanelElementExportTool : StiElementExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		Color backColor = StiDashboardStyleHelper.GetBackColor(element);
		StiContainer stiContainer = new StiContainer(rect)
		{
			Name = element.Name + "_Content",
			Brush = new StiSolidBrush(backColor)
		};
		if (element is IStiDashboardWatermark stiDashboardWatermark && (stiDashboardWatermark.DashboardWatermark.ImageEnabled || stiDashboardWatermark.DashboardWatermark.TextEnabled || stiDashboardWatermark.DashboardWatermark.WeaveEnabled))
		{
			destination.TagValue = stiDashboardWatermark.DashboardWatermark;
		}
	}
}
