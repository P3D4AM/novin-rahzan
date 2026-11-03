using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart.Geoms.Series.Pie;

public class StiPieEmptySeriesElementGeom : StiCellGeom
{
	public override void Draw(StiContext context)
	{
		RectangleF rect = base.ClientRectangle;
		StiPenGeom pen = new StiPenGeom(Color.LightGray);
		context.PushSmoothingModeToAntiAlias();
		context.FillEllipse(Color.FromArgb(50, Color.LightGray), rect);
		context.DrawEllipse(pen, rect);
		context.DrawLine(pen, rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f, rect.Right, rect.Y + rect.Height / 2f);
		context.DrawLine(pen, rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f, rect.X + rect.Width / 2f, rect.Bottom);
		context.PopSmoothingMode();
	}

	public StiPieEmptySeriesElementGeom(RectangleF clientRectangle)
		: base(clientRectangle)
	{
	}
}
