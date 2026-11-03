using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public abstract class StiAreaGeom : StiCellGeom
{
	public IStiArea Area { get; private set; }

	public override void Draw(StiContext context)
	{
		RectangleF rectangleF = base.ClientRectangle;
		if (!rectangleF.IsEmpty && rectangleF.Width > 0f && rectangleF.Height > 0f)
		{
			context.FillRectangle(Area.Brush, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		}
	}

	public StiAreaGeom(IStiArea area, RectangleF clientRectangle)
		: base(Rectangle.Round(clientRectangle))
	{
		Area = area;
	}
}
