using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiRightButtonGeom : StiCellGeom
{
	private IStiXAxis axis;

	public IStiXAxis Axis => axis;

	public override void InvokeMouseEnter(StiInteractionOptions options)
	{
		if (Axis.Interaction.RangeScrollEnabled && !Axis.Core.IsMouseOverIncreaseButton)
		{
			Axis.Core.IsMouseOverIncreaseButton = true;
			options.UpdateContext = true;
		}
	}

	public override void InvokeMouseLeave(StiInteractionOptions options)
	{
		if (Axis.Interaction.RangeScrollEnabled && Axis.Core.IsMouseOverIncreaseButton)
		{
			Axis.Core.IsMouseOverIncreaseButton = false;
			options.UpdateContext = true;
		}
	}

	public override void InvokeMouseDown(StiInteractionOptions options)
	{
		if (!Axis.Interaction.RangeScrollEnabled)
		{
			return;
		}
		options.UpdateContext = MoveRight();
		if (options.UpdateContext)
		{
			if (!options.IsRecalled)
			{
				options.RecallTime = TimeSpan.FromSeconds(StiAxisCoreXF.DefaultScrollBarFirstRecallTime);
			}
			else
			{
				options.RecallTime = TimeSpan.FromSeconds(StiAxisCoreXF.DefaultScrollBarOtherRecallTime);
			}
			options.RecallEvent = true;
		}
	}

	private bool MoveRight()
	{
		StiAxisAreaCoreXF stiAxisAreaCoreXF = Axis.Area.Core as StiAxisAreaCoreXF;
		if (stiAxisAreaCoreXF.ScrollValueX >= stiAxisAreaCoreXF.ScrollRangeX - stiAxisAreaCoreXF.ScrollViewX)
		{
			return false;
		}
		stiAxisAreaCoreXF.ScrollValueX += stiAxisAreaCoreXF.ScrollViewX * (double)StiAxisCoreXF.DefaultScrollBarSmallFactor;
		if (stiAxisAreaCoreXF.ScrollValueX >= stiAxisAreaCoreXF.ScrollRangeX - stiAxisAreaCoreXF.ScrollViewX)
		{
			stiAxisAreaCoreXF.ScrollValueX = stiAxisAreaCoreXF.ScrollRangeX - stiAxisAreaCoreXF.ScrollViewX;
		}
		stiAxisAreaCoreXF.BlockScrollValueX = true;
		return true;
	}

	public override void Draw(StiContext context)
	{
		float zoom = context.Options.Zoom;
		RectangleF rectangleF = base.ClientRectangle;
		float num = rectangleF.Y + rectangleF.Height / 2f;
		Color color = Color.White;
		if (Axis.Core.IsMouseOverIncreaseButton)
		{
			context.FillRectangle(StiColorUtils.Light(Axis.LineColor, (byte)50), rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
			context.DrawRectangle(new StiPenGeom(Axis.LineColor), rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		}
		else
		{
			color = Axis.LineColor;
		}
		float num2 = rectangleF.Width / 4f;
		float num3 = rectangleF.Height / 3f;
		PointF pointF = new PointF(rectangleF.X + rectangleF.Width / 2f, rectangleF.Y + rectangleF.Height / 2f);
		context.PushSmoothingModeToAntiAlias();
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		list.Add(new StiLineSegmentGeom(pointF.X - num2, pointF.Y - num3, pointF.X + num2, pointF.Y));
		list.Add(new StiLineSegmentGeom(pointF.X + num2, pointF.Y, pointF.X - num2, pointF.Y + num3));
		context.FillPath(color, list, rectangleF);
		context.PopSmoothingMode();
	}

	public StiRightButtonGeom(IStiXAxis axis, RectangleF clientRectangle)
		: base(clientRectangle)
	{
		this.axis = axis;
	}
}
