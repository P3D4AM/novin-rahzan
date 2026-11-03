using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiVertScrollBarGeom : StiCellGeom
{
	private IStiYAxis axis;

	public IStiYAxis Axis => axis;

	public override void InvokeMouseDown(StiInteractionOptions options)
	{
		if (Axis.Interaction.RangeScrollEnabled)
		{
			StiAxisAreaCoreXF stiAxisAreaCoreXF = Axis.Area.Core as StiAxisAreaCoreXF;
			stiAxisAreaCoreXF.BlockScrollValueY = true;
			float width = base.ClientRectangle.Width;
			float num = options.MousePoint.Y - width;
			float num2 = base.ClientRectangle.Height - width * 2f;
			stiAxisAreaCoreXF.ScrollValueY = (double)(num / num2) * stiAxisAreaCoreXF.ScrollRangeY;
			if (stiAxisAreaCoreXF.ScrollValueY < 0.0)
			{
				stiAxisAreaCoreXF.ScrollValueY = 0.0;
			}
			if (stiAxisAreaCoreXF.ScrollValueY >= stiAxisAreaCoreXF.ScrollRangeY - stiAxisAreaCoreXF.ScrollViewY)
			{
				stiAxisAreaCoreXF.ScrollValueY = stiAxisAreaCoreXF.ScrollRangeY - stiAxisAreaCoreXF.ScrollViewY;
			}
			options.UpdateContext = true;
		}
	}

	public override void Draw(StiContext context)
	{
		RectangleF rectangleF = base.ClientRectangle;
		StiPenGeom pen = new StiPenGeom(Axis.LineColor);
		context.DrawRectangle(pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
	}

	public StiVertScrollBarGeom(IStiYAxis axis, RectangleF clientRectangle)
		: base(clientRectangle)
	{
		this.axis = axis;
	}
}
