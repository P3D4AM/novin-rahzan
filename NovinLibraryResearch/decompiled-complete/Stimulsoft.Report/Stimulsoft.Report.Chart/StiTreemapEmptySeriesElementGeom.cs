using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiTreemapEmptySeriesElementGeom : StiCellGeom
{
	public override void Draw(StiContext context)
	{
		Rectangle rect = Rectangle.Round(base.ClientRectangle);
		StiPenGeom pen = new StiPenGeom(Color.LightGray);
		context.PushSmoothingModeToAntiAlias();
		context.FillRectangle(Color.FromArgb(50, Color.LightGray), rect);
		context.DrawRectangle(pen, rect.X, rect.Y, rect.Width / 3, rect.Height);
		context.DrawRectangle(pen, rect.X + rect.Width / 3, rect.Y, rect.Width * 2 / 3, rect.Height);
		context.PopSmoothingMode();
	}

	public StiTreemapEmptySeriesElementGeom(RectangleF clientRectangle)
		: base(clientRectangle)
	{
	}
}
