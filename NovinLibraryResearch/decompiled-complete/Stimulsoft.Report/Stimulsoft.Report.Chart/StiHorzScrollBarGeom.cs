using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiHorzScrollBarGeom : StiCellGeom
{
	private IStiXAxis axis;

	public IStiXAxis Axis => axis;

	public override void InvokeMouseDown(StiInteractionOptions options)
	{
		if (Axis.Interaction.RangeScrollEnabled)
		{
			StiAxisAreaCoreXF stiAxisAreaCoreXF = Axis.Area.Core as StiAxisAreaCoreXF;
			stiAxisAreaCoreXF.BlockScrollValueX = true;
			float height = base.ClientRectangle.Height;
			float num = options.MousePoint.X - height;
			float num2 = base.ClientRectangle.Width - height * 2f;
			stiAxisAreaCoreXF.ScrollValueX = (double)(num / num2) * stiAxisAreaCoreXF.ScrollRangeX;
			if (stiAxisAreaCoreXF.ScrollValueX < 0.0)
			{
				stiAxisAreaCoreXF.ScrollValueX = 0.0;
			}
			if (stiAxisAreaCoreXF.ScrollValueX >= stiAxisAreaCoreXF.ScrollRangeX - stiAxisAreaCoreXF.ScrollViewX)
			{
				stiAxisAreaCoreXF.ScrollValueX = stiAxisAreaCoreXF.ScrollRangeX - stiAxisAreaCoreXF.ScrollViewX;
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

	public StiHorzScrollBarGeom(IStiXAxis axis, RectangleF clientRectangle)
		: base(clientRectangle)
	{
		this.axis = axis;
	}
}
