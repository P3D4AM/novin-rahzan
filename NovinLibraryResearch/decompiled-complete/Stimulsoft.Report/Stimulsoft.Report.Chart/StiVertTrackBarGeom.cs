using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiVertTrackBarGeom : StiCellGeom
{
	private IStiYAxis axis;

	private StiVertScrollBarGeom scrollBar;

	public IStiYAxis Axis => axis;

	public StiVertScrollBarGeom ScrollBar => scrollBar;

	public override void InvokeMouseEnter(StiInteractionOptions options)
	{
		if (Axis.Interaction.RangeScrollEnabled && !Axis.Core.IsMouseOverTrackBar)
		{
			Axis.Core.IsMouseOverTrackBar = true;
			options.UpdateContext = true;
		}
	}

	public override void InvokeMouseLeave(StiInteractionOptions options)
	{
		if (Axis.Interaction.RangeScrollEnabled && Axis.Core.IsMouseOverTrackBar)
		{
			Axis.Core.IsMouseOverTrackBar = false;
			options.UpdateContext = true;
		}
	}

	public override void InvokeMouseDown(StiInteractionOptions options)
	{
		if (Axis.Interaction.RangeScrollEnabled)
		{
			StiAxisAreaCoreXF stiAxisAreaCoreXF = Axis.Area.Core as StiAxisAreaCoreXF;
			options.DragEnabled = true;
			stiAxisAreaCoreXF.ScrollDragStartValue = stiAxisAreaCoreXF.ScrollValueY;
		}
	}

	public override void InvokeDrag(StiInteractionOptions options)
	{
		if (Axis.Interaction.RangeScrollEnabled)
		{
			StiAxisAreaCoreXF stiAxisAreaCoreXF = Axis.Area.Core as StiAxisAreaCoreXF;
			stiAxisAreaCoreXF.BlockScrollValueY = true;
			float height = ScrollBar.ClientRectangle.Height;
			float num = ScrollBar.ClientRectangle.Height - height * 2f;
			stiAxisAreaCoreXF.ScrollValueY = (double)((0f - options.DragDelta.Height) / num) * stiAxisAreaCoreXF.ScrollRangeY + stiAxisAreaCoreXF.ScrollDragStartValue;
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
		float zoom = context.Options.Zoom;
		RectangleF rectangleF = base.ClientRectangle;
		StiPenGeom pen = new StiPenGeom(Axis.LineColor);
		StiPenGeom pen2 = new StiPenGeom(Color.White);
		StiAxisAreaCoreXF stiAxisAreaCoreXF = Axis.Area.Core as StiAxisAreaCoreXF;
		IStiXAxis xAxis = Axis.Area.XAxis;
		if (Axis.Core.IsMouseOverTrackBar)
		{
			context.FillRectangle(StiColorUtils.Light(Axis.LineColor, (byte)50), rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		}
		else
		{
			context.FillRectangle(Axis.LineColor, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		}
		context.DrawRectangle(pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		if (rectangleF.Height > 10f && rectangleF.Width > 4f)
		{
			float num = rectangleF.Y + rectangleF.Height / 2f;
			context.DrawLine(pen2, rectangleF.X + 2f, num - 2f, rectangleF.Right - 2f, num - 2f);
			context.DrawLine(pen2, rectangleF.X + 2f, num, rectangleF.Right - 2f, num);
			context.DrawLine(pen2, rectangleF.X + 2f, num + 2f, rectangleF.Right - 2f, num + 2f);
		}
	}

	public StiVertTrackBarGeom(IStiYAxis axis, RectangleF clientRectangle, StiVertScrollBarGeom scrollBar)
		: base(clientRectangle)
	{
		this.axis = axis;
		this.scrollBar = scrollBar;
	}
}
