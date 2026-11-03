using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiYAxisGeom : StiCellGeom
{
	private IStiYAxis axis;

	private bool isCenterAxis;

	private StiYAxisViewGeom view;

	public IStiYAxis Axis => axis;

	public bool IsCenterAxis => isCenterAxis;

	public StiYAxisViewGeom View
	{
		get
		{
			return view;
		}
		set
		{
			view = value;
		}
	}

	protected void DrawArrow(StiContext context, RectangleF rect)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		float zoom = context.Options.Zoom;
		StiPenGeom pen = new StiPenGeom(Axis.LineColor);
		StiSolidBrush brush = new StiSolidBrush(Axis.LineColor);
		StiPenGeom pen2 = new StiPenGeom(Axis.LineColor);
		if (!Axis.Visible)
		{
			DrawAxisLine(context, rect);
		}
		PointF empty = PointF.Empty;
		empty = (Axis.Area.ReverseVert ? new PointF(rect.Right, rect.Bottom) : new PointF(rect.Right, rect.Y));
		if (((StiYAxisCoreXF)Axis.Core).IsRightSide)
		{
			empty.X = rect.X;
		}
		switch (Axis.ArrowStyle)
		{
		case StiArrowStyle.Triangle:
		{
			context.DrawLine(pen, empty.X, empty.Y, empty.X, empty.Y - Axis.Core.ArrowHeight * zoom);
			context.PushSmoothingModeToAntiAlias();
			List<StiSegmentGeom> list2 = new List<StiSegmentGeom>();
			list2.Add(new StiLineSegmentGeom(empty.X - Axis.Core.ArrowWidth * zoom, empty.Y - Axis.Core.ArrowHeight * zoom, empty.X + Axis.Core.ArrowWidth * zoom, empty.Y - Axis.Core.ArrowHeight * zoom));
			list2.Add(new StiLineSegmentGeom(empty.X + Axis.Core.ArrowWidth * zoom, empty.Y - Axis.Core.ArrowHeight * zoom, empty.X, empty.Y - Axis.Core.ArrowHeight * 4f * zoom));
			context.FillPath(brush, list2, StiPathGeom.GetBoundsState);
			context.PopSmoothingMode();
			break;
		}
		case StiArrowStyle.Lines:
			context.DrawLine(pen, empty.X, empty.Y, empty.X, empty.Y - Axis.Core.ArrowHeight * 4f * zoom);
			context.PushSmoothingModeToAntiAlias();
			context.DrawLine(pen2, empty.X - Axis.Core.ArrowWidth * zoom, empty.Y - Axis.Core.ArrowHeight * zoom, empty.X, empty.Y - Axis.Core.ArrowHeight * 4f * zoom);
			context.DrawLine(pen2, empty.X + Axis.Core.ArrowWidth * zoom, empty.Y - Axis.Core.ArrowHeight * zoom, empty.X, empty.Y - Axis.Core.ArrowHeight * 4f * zoom);
			context.PopSmoothingMode();
			break;
		case StiArrowStyle.Circle:
		{
			context.DrawLine(pen, empty.X, empty.Y, empty.X, empty.Y - Axis.Core.ArrowHeight * zoom * 2f);
			float num3 = Axis.Core.ArrowHeight * zoom;
			PointF pointF4 = new PointF(empty.X - num3 / 2f, empty.Y - num3 * 3f);
			context.PushSmoothingModeToAntiAlias();
			RectangleF rect5 = new RectangleF(pointF4.X, pointF4.Y, Axis.Core.ArrowHeight * zoom, Axis.Core.ArrowHeight * zoom);
			context.DrawEllipse(pen, rect5);
			context.PopSmoothingMode();
			break;
		}
		case StiArrowStyle.Arc:
		case StiArrowStyle.ArcAndCircle:
		{
			float num = Axis.Core.ArrowHeight * zoom;
			context.DrawLine(pen, empty.X, empty.Y, empty.X, empty.Y - num * 2f);
			context.PushSmoothingModeToAntiAlias();
			List<StiSegmentGeom> list = new List<StiSegmentGeom>();
			if (Axis.Area.ReverseVert)
			{
				PointF pointF = new PointF(empty.X + num / 2f, empty.Y - num * 2f);
				RectangleF rect2 = new RectangleF(pointF.X, pointF.Y, Math.Abs(num), Math.Abs(num));
				if (Axis.ArrowStyle == StiArrowStyle.ArcAndCircle)
				{
					rect2.Y += zoom;
					rect2.Inflate(1f, 1f);
				}
				list.Add(new StiArcSegmentGeom(rect2, 180f, 180f));
			}
			else
			{
				PointF pointF2 = new PointF(empty.X - num / 2f, empty.Y - num * 3f);
				RectangleF rect3 = new RectangleF(pointF2.X, pointF2.Y, num, num);
				if (Axis.ArrowStyle == StiArrowStyle.ArcAndCircle)
				{
					rect3.Y -= zoom;
					rect3.Inflate(1f, 1f);
				}
				list.Add(new StiArcSegmentGeom(rect3, 0f, 180f));
			}
			context.DrawPath(pen, list, StiPathGeom.GetBoundsState);
			if (Axis.ArrowStyle == StiArrowStyle.ArcAndCircle)
			{
				float num2 = Axis.Core.ArrowHeight * zoom;
				PointF pointF3 = new PointF(empty.X - num2 / 2f, empty.Y - num2 * 3f - zoom);
				RectangleF rect4 = new RectangleF(pointF3.X, pointF3.Y, num2, num2);
				if (Axis.Area.ReverseVert)
				{
					rect4.Inflate(0.5f * zoom, 0.5f * zoom);
					rect4.Y += zoom * 3f;
				}
				else
				{
					rect4.Inflate(-0.5f * zoom, -0.5f * zoom);
					rect4.Y -= zoom;
				}
				context.FillEllipse(brush, rect4);
			}
			context.PopSmoothingMode();
			break;
		}
		}
	}

	private void DrawAxisLine(StiContext context, RectangleF rect)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		float num = rect.Right;
		if (((StiYAxisCoreXF)Axis.Core).IsRightSide)
		{
			num = rect.Left;
		}
		if (IsCenterAxis && Axis.Area.ReverseHor)
		{
			num = rect.Right;
		}
		StiPenGeom stiPenGeom = new StiPenGeom(Axis.LineColor, Axis.LineWidth);
		stiPenGeom.PenStyle = Axis.LineStyle;
		if ((IsCenterAxis && (Axis.ShowYAxis == StiShowYAxis.Both || Axis.ShowYAxis == StiShowYAxis.Center)) || (!IsCenterAxis && (Axis.ShowYAxis == StiShowYAxis.Both || Axis.ShowYAxis == StiShowYAxis.Left)))
		{
			context.DrawLine(stiPenGeom, num, rect.Y, num, rect.Bottom);
		}
	}

	private void DrawMinorTicks(StiContext context, StiPenGeom pen, float posX, float posY1, float posY2, IStiAxisTicks ticks)
	{
		float num = posY2 - posY1;
		float num2 = num / (float)(ticks.MinorCount + 1);
		float num3 = ticks.MinorLength * context.Options.Zoom;
		for (int i = 1; i <= ticks.MinorCount; i++)
		{
			float num4 = posY1 + num2 * (float)i;
			float x = ((((StiYAxisCoreXF)Axis.Core).IsLeftSide || IsCenterAxis) ? (posX - num3) : (posX + num3));
			if (Axis.Area.ReverseHor && IsCenterAxis)
			{
				x = posX + num3;
			}
			context.DrawLine(pen, posX, num4, x, num4);
		}
	}

	private void DrawTicks(StiContext context, RectangleF rect, IStiAxisTicks ticks, StiPenGeom penLine)
	{
		if (!ticks.Visible)
		{
			return;
		}
		float num = ticks.Length * context.Options.Zoom;
		float num2 = (((StiYAxisCoreXF)Axis.Core).IsLeftSide ? rect.Right : rect.Left);
		float x = (((StiYAxisCoreXF)Axis.Core).IsLeftSide ? (num2 - num) : (num2 + num));
		if (IsCenterAxis && Axis.Area.ReverseHor)
		{
			num2 = rect.Right;
			x = rect.Right + num;
		}
		int num3 = 0;
		foreach (StiStripPositionXF item in Axis.Info.TicksCollection)
		{
			float position = item.Position;
			context.DrawLine(penLine, num2, position, x, position);
			if (ticks.MinorVisible && num3 != Axis.Info.TicksCollection.Count - 1)
			{
				float position2 = Axis.Info.TicksCollection[num3 + 1].Position;
				DrawMinorTicks(context, penLine, num2, position, position2, ticks);
			}
			num3++;
		}
	}

	private void DrawAxis(StiContext context, RectangleF rect)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		StiPenGeom stiPenGeom = new StiPenGeom(Axis.LineColor, Axis.LineWidth);
		stiPenGeom.PenStyle = Axis.LineStyle;
		if (IsCenterAxis && (Axis.ShowYAxis == StiShowYAxis.Both || Axis.ShowYAxis == StiShowYAxis.Center))
		{
			DrawTicks(context, rect, Axis.Ticks, stiPenGeom);
		}
		else if (!IsCenterAxis)
		{
			if (((StiYAxisCoreXF)Axis.Core).IsLeftSide && (Axis.ShowYAxis == StiShowYAxis.Both || Axis.ShowYAxis == StiShowYAxis.Left))
			{
				DrawTicks(context, rect, Axis.Area.YAxis.Ticks, stiPenGeom);
			}
			if (((StiYAxisCoreXF)Axis.Core).IsRightSide)
			{
				DrawTicks(context, rect, Axis.Area.YRightAxis.Ticks, stiPenGeom);
			}
		}
		DrawAxisLine(context, rect);
	}

	private RectangleF GetViewClipRect()
	{
		RectangleF result = View.ClientRectangle;
		result.X = 0f;
		result.Y = (float)((StiAxisAreaCoreXF)Axis.Area.Core).ScrollDistanceY;
		result.Inflate(1f, 1f);
		if (IsCenterAxis)
		{
			result.Inflate(2f, 0f);
		}
		return result;
	}

	protected override bool AllowChildDrawing(StiCellGeom cellGeom)
	{
		if (!(cellGeom is StiAxisLabelGeom stiAxisLabelGeom))
		{
			return true;
		}
		if (View == null)
		{
			return true;
		}
		RectangleF viewClipRect = GetViewClipRect();
		RectangleF rectangleF = stiAxisLabelGeom.ClientRectangle;
		if (!(rectangleF.Bottom < viewClipRect.Y))
		{
			return !(rectangleF.Y > viewClipRect.Bottom);
		}
		return false;
	}

	public override void Draw(StiContext context)
	{
		RectangleF rect = base.ClientRectangle;
		if (!(rect.Width <= 0f) && !(rect.Height <= 0f))
		{
			if (View != null)
			{
				RectangleF viewClipRect = GetViewClipRect();
				context.PushClip(viewClipRect);
			}
			DrawAxis(context, rect);
			if (View != null)
			{
				context.PopClip();
			}
		}
	}

	public StiYAxisGeom(IStiYAxis axis, RectangleF clientRectangle, bool isCenterAxis)
		: base(clientRectangle)
	{
		this.axis = axis;
		this.isCenterAxis = isCenterAxis;
	}
}
