using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart.Geoms.Series.Doughnut;

public class StiDoughnutEmptySeriesElementGeom : StiCellGeom
{
	public override void Draw(StiContext context)
	{
		RectangleF rectangleF = base.ClientRectangle;
		StiPenGeom pen = new StiPenGeom(Color.LightGray);
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		list.Add(new StiArcSegmentGeom(rectangleF, 0f, 360f));
		list.Add(new StiLineSegmentGeom(rectangleF.Right, rectangleF.Y + rectangleF.Height / 2f, rectangleF.X + rectangleF.Width * 3f / 4f, rectangleF.Y + rectangleF.Height / 2f));
		list.Add(new StiArcSegmentGeom(new RectangleF(rectangleF.X + rectangleF.Width / 4f, rectangleF.Y + rectangleF.Height / 4f, rectangleF.Width / 2f, rectangleF.Height / 2f), 0f, 360f));
		context.PushSmoothingModeToAntiAlias();
		context.FillPath(Color.FromArgb(50, Color.LightGray), list, rectangleF);
		context.DrawPath(pen, list, rectangleF);
		context.DrawLine(pen, rectangleF.X + rectangleF.Width / 2f, rectangleF.Y + rectangleF.Height * 3f / 4f, rectangleF.X + rectangleF.Width / 2f, rectangleF.Bottom);
		context.PopSmoothingMode();
	}

	public StiDoughnutEmptySeriesElementGeom(RectangleF clientRectangle)
		: base(clientRectangle)
	{
	}
}
