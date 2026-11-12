using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Shape;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiShapeElementExportTool : StiElementExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		StiShapeElement stiShapeElement = element as StiShapeElement;
		destination.Components.Add(new StiShape
		{
			ShapeType = stiShapeElement.ShapeType,
			Size = stiShapeElement.Size,
			BorderColor = stiShapeElement.Stroke,
			Brush = stiShapeElement.Fill,
			Page = stiShapeElement.Page,
			Left = rect.Left,
			Top = rect.Top,
			Width = rect.Width,
			Height = rect.Height
		});
	}
}
