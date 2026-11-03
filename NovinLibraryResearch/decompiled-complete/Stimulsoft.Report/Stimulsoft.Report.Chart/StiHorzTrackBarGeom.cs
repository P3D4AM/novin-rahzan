using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiHorzTrackBarGeom : StiCellGeom
{
	private IStiXAxis axis;

	private StiHorzScrollBarGeom scrollBar;

	public IStiXAxis Axis => axis;

	public StiHorzScrollBarGeom ScrollBar => scrollBar;

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
			stiAxisAreaCoreXF.ScrollDragStartValue = stiAxisAreaCoreXF.ScrollValueX;
		}
	}

	public override void InvokeDrag(StiInteractionOptions options)
	{
		if (Axis.Interaction.RangeScrollEnabled)
		{
			StiAxisAreaCoreXF stiAxisAreaCoreXF = Axis.Area.Core as StiAxisAreaCoreXF;
			stiAxisAreaCoreXF.BlockScrollValueX = true;
			float height = ScrollBar.ClientRectangle.Height;
			float num = ScrollBar.ClientRectangle.Width - height * 2f;
			stiAxisAreaCoreXF.ScrollValueX = (double)(options.DragDelta.Width / num) * stiAxisAreaCoreXF.ScrollRangeX + stiAxisAreaCoreXF.ScrollDragStartValue;
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
		if (rectangleF.Width > 10f && rectangleF.Height > 4f)
		{
			float num = rectangleF.X + rectangleF.Width / 2f;
			context.DrawLine(pen2, num - 2f, rectangleF.Y + 2f, num - 2f, rectangleF.Bottom - 2f);
			context.DrawLine(pen2, num, rectangleF.Y + 2f, num, rectangleF.Bottom - 2f);
			context.DrawLine(pen2, num + 2f, rectangleF.Y + 2f, num + 2f, rectangleF.Bottom - 2f);
		}
	}

	public StiHorzTrackBarGeom(IStiXAxis axis, RectangleF clientRectangle, StiHorzScrollBarGeom scrollBar)
		: base(clientRectangle)
	{
		this.axis = axis;
		this.scrollBar = scrollBar;
	}
}
