using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiXAxisGeom : StiCellGeom
{
	private IStiXAxis axis;

	private bool isCenterAxis;

	private StiXAxisViewGeom view;

	public IStiXAxis Axis => axis;

	public bool IsCenterAxis => isCenterAxis;

	public StiXAxisViewGeom View
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		float zoom = context.Options.Zoom;
		StiPenGeom pen = new StiPenGeom(Axis.LineColor, Axis.LineWidth);
		StiPenGeom pen2 = new StiPenGeom(Axis.LineColor);
		StiSolidBrush brush = new StiSolidBrush(Axis.LineColor);
		float y = 0f;
		if (((StiXAxisCoreXF)Axis.Core).IsBottomSide)
		{
			y = rect.Y;
		}
		if (((StiXAxisCoreXF)Axis.Core).IsTopSide)
		{
			y = rect.Bottom;
		}
		PointF empty = PointF.Empty;
		empty = ((!Axis.Area.ReverseHor) ? new PointF(rect.Right, y) : new PointF(rect.X, y));
		switch (Axis.ArrowStyle)
		{
		case StiArrowStyle.Triangle:
		{
			context.DrawLine(pen, empty.X, empty.Y, empty.X + Axis.Core.ArrowHeight * zoom, empty.Y);
			context.PushSmoothingModeToAntiAlias();
			List<StiSegmentGeom> list2 = new List<StiSegmentGeom>();
			list2.Add(new StiLineSegmentGeom(empty.X + Axis.Core.ArrowHeight * zoom, empty.Y - Axis.Core.ArrowWidth * zoom, empty.X + Axis.Core.ArrowHeight * 4f * zoom, empty.Y));
			list2.Add(new StiLineSegmentGeom(empty.X + Axis.Core.ArrowHeight * 4f * zoom, empty.Y, empty.X + Axis.Core.ArrowHeight * zoom, empty.Y + Axis.Core.ArrowWidth * zoom));
			context.FillPath(brush, list2, rect);
			context.PopSmoothingMode();
			break;
		}
		case StiArrowStyle.Lines:
			context.DrawLine(pen, empty.X, empty.Y, empty.X + Axis.Core.ArrowHeight * 4f * zoom, empty.Y);
			context.PushSmoothingModeToAntiAlias();
			context.DrawLine(pen2, empty.X + Axis.Core.ArrowHeight * zoom, empty.Y - Axis.Core.ArrowWidth * zoom, empty.X + Axis.Core.ArrowHeight * 4f * zoom, empty.Y);
			context.DrawLine(pen2, empty.X + Axis.Core.ArrowHeight * 4f * zoom, empty.Y, empty.X + Axis.Core.ArrowHeight * zoom, empty.Y + Axis.Core.ArrowWidth * zoom);
			context.PopSmoothingMode();
			break;
		case StiArrowStyle.Circle:
		{
			float num3 = Axis.Core.ArrowHeight * zoom;
			context.DrawLine(pen, empty.X, empty.Y, empty.X + num3 * 2f, empty.Y);
			PointF pointF4 = new PointF(empty.X + num3 * 2f, empty.Y - num3 / 2f);
			context.PushSmoothingModeToAntiAlias();
			RectangleF rect5 = new RectangleF(pointF4.X, pointF4.Y, num3, num3);
			context.DrawEllipse(pen, rect5);
			context.PopSmoothingMode();
			break;
		}
		case StiArrowStyle.Arc:
		case StiArrowStyle.ArcAndCircle:
		{
			float num = Axis.Core.ArrowHeight * zoom;
			context.DrawLine(pen, empty.X, empty.Y, empty.X + num * 2f, empty.Y);
			context.PushSmoothingModeToAntiAlias();
			List<StiSegmentGeom> list = new List<StiSegmentGeom>();
			if (Axis.Area.ReverseHor)
			{
				PointF pointF = new PointF(empty.X + num * 3f, empty.Y + num / 2f);
				RectangleF rect2 = new RectangleF(pointF.X, pointF.Y, Math.Abs(num), Math.Abs(num));
				if (Axis.ArrowStyle == StiArrowStyle.ArcAndCircle)
				{
					rect2.X -= zoom;
					rect2.Inflate(1f, 1f);
				}
				list.Add(new StiArcSegmentGeom(rect2, 270f, 180f));
			}
			else
			{
				PointF pointF2 = new PointF(empty.X + num * 2f, empty.Y - num / 2f);
				RectangleF rect3 = new RectangleF(pointF2.X, pointF2.Y, Math.Abs(num), Math.Abs(num));
				if (Axis.ArrowStyle == StiArrowStyle.ArcAndCircle)
				{
					rect3.X += zoom;
					rect3.Inflate(1f, 1f);
				}
				list.Add(new StiArcSegmentGeom(rect3, 90f, 180f));
			}
			context.DrawPath(pen, list, StiPathGeom.GetBoundsState);
			if (Axis.ArrowStyle == StiArrowStyle.ArcAndCircle)
			{
				float num2 = Axis.Core.ArrowHeight * zoom;
				PointF pointF3 = new PointF(empty.X + num2 * 2f + zoom, empty.Y - num2 / 2f);
				RectangleF rect4 = new RectangleF(pointF3.X, pointF3.Y, num2, num2);
				if (Axis.Area.ReverseHor)
				{
					rect4.Inflate(0.5f * zoom, 0.5f * zoom);
					rect4.X -= zoom * 3f;
				}
				else
				{
					rect4.Inflate(-0.5f * zoom, -0.5f * zoom);
					rect4.X += zoom;
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
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		if (((StiXAxisCoreXF)Axis.Core).IsTopSide)
		{
			num = rect.Bottom;
		}
		if (((StiXAxisCoreXF)Axis.Core).IsBottomSide)
		{
			num = rect.Top;
		}
		StiPenGeom stiPenGeom = new StiPenGeom(Axis.LineColor, Axis.LineWidth);
		stiPenGeom.PenStyle = Axis.LineStyle;
		if (IsCenterAxis && (Axis.ShowXAxis == StiShowXAxis.Both || Axis.ShowXAxis == StiShowXAxis.Center))
		{
			if (Axis.Area.ReverseVert)
			{
				context.DrawLine(stiPenGeom, rect.X, num, rect.Right, num);
			}
			else
			{
				context.DrawLine(stiPenGeom, rect.X, num + rect.Height, rect.Right, num + rect.Height);
			}
		}
		else if (!IsCenterAxis && (Axis.ShowXAxis == StiShowXAxis.Both || Axis.ShowXAxis == StiShowXAxis.Bottom))
		{
			context.DrawLine(stiPenGeom, rect.X, num, rect.Right, num);
		}
	}

	private void DrawMinorTicks(StiContext context, StiPenGeom pen, float posX1, float posX2, float posY, IStiAxisTicks ticks)
	{
		float num = posX2 - posX1;
		float num2 = num / (float)(ticks.MinorCount + 1);
		float num3 = ticks.MinorLength * context.Options.Zoom;
		for (int i = 1; i <= ticks.MinorCount; i++)
		{
			float num4 = posX1 + num2 * (float)i;
			float y = ((((StiXAxisCoreXF)Axis.Core).IsTopSide || IsCenterAxis) ? (posY - num3) : (posY + num3));
			if (Axis.Area.ReverseVert && IsCenterAxis)
			{
				y = posY + num3;
			}
			context.DrawLine(pen, num4, posY, num4, y);
		}
	}

	private void DrawTicks(StiContext context, RectangleF rect, IStiAxisTicks ticks, StiPenGeom penLine)
	{
		if (!ticks.Visible)
		{
			return;
		}
		float num = ticks.Length * context.Options.Zoom;
		float num2 = ((((StiXAxisCoreXF)Axis.Core).IsTopSide || IsCenterAxis) ? rect.Bottom : rect.Top);
		float num3 = ((((StiXAxisCoreXF)Axis.Core).IsTopSide || IsCenterAxis) ? (num2 - num) : (num2 + num));
		if (IsCenterAxis && Axis.Area.ReverseVert)
		{
			num3 = num2 + num;
		}
		num = ticks.LengthUnderLabels * context.Options.Zoom;
		float num4 = ((((StiXAxisCoreXF)Axis.Core).IsTopSide || IsCenterAxis) ? rect.Bottom : rect.Top);
		float y = ((((StiXAxisCoreXF)Axis.Core).IsTopSide || IsCenterAxis) ? (num4 - num) : (num4 + num));
		if (IsCenterAxis && Axis.Area.ReverseVert)
		{
			y = num4 + num;
		}
		IEnumerable<float> enumerable = (from l in Axis.Info.LabelInfoCollection?.ToArray()
			select l.TextPoint.X);
		List<StiStripPositionXF> ticksCollection = Axis.Info.TicksCollection;
		if (IsArgumentDateTime(ticksCollection) && ((StiXBottomAxis)Axis.Area.XAxis).DateTimeStep.Step != 0)
		{
			DateTime dateTime = default(DateTime);
			DateTime dateTime2 = default(DateTime);
			bool flag = true;
			string text = string.Empty;
			float num5 = 0f;
			for (int i = 0; i < ticksCollection.Count; i++)
			{
				if (flag)
				{
					num5 = ticksCollection[i].Position;
					flag = false;
				}
				if (!(ticksCollection[i].StripLine.ValueObject is DateTime))
				{
					continue;
				}
				dateTime = (DateTime)ticksCollection[i].StripLine.ValueObject;
				dateTime2 = ((i >= ticksCollection.Count - 2) ? dateTime : ((DateTime)ticksCollection[i + 1].StripLine.ValueObject));
				switch (((StiXBottomAxis)Axis.Area.XAxis).DateTimeStep.Step)
				{
				case StiTimeDateStep.Day:
					if (dateTime.ToString("yyyyMMMMdd") != dateTime2.ToString("yyyyMMMMdd") || i == ticksCollection.Count - 2)
					{
						text = dateTime.ToString("dd");
					}
					break;
				case StiTimeDateStep.Hour:
					if (dateTime.ToString("yyyyMMMMddHH") != dateTime2.ToString("yyyyMMMMddHH") || i == ticksCollection.Count - 2)
					{
						text = dateTime.ToString("HH");
					}
					break;
				case StiTimeDateStep.Minute:
					if (dateTime.ToString("yyyyMMMMddhhmm") != dateTime2.ToString("yyyyMMMMddhhmm") || i == ticksCollection.Count - 2)
					{
						text = dateTime.ToString("mm");
					}
					break;
				case StiTimeDateStep.Month:
					if (dateTime.ToString("yyyyMMMM") != dateTime2.ToString("yyyyMMMM") || i == ticksCollection.Count - 2)
					{
						text = dateTime.ToString("MMMM");
					}
					break;
				case StiTimeDateStep.Second:
					if (dateTime.ToString("yyyyMMMMddHHmmss") != dateTime2.ToString("yyyyMMMMddHHmmss") || i == ticksCollection.Count - 2)
					{
						text = dateTime.ToString("ss");
					}
					break;
				case StiTimeDateStep.Year:
					if (dateTime.ToString("yyyy") != dateTime2.ToString("yyyy") || i == ticksCollection.Count - 2)
					{
						text = dateTime.ToString("yyyy");
					}
					break;
				}
				if (text != string.Empty)
				{
					float num6 = 0f;
					num6 = ((i != ticksCollection.Count - 2) ? (ticksCollection[i].Position + (ticksCollection[i + 1].Position - ticksCollection[i].Position) / 2f) : ticksCollection[i + 1].Position);
					context.DrawRectangle(penLine, num5, num2, num6 - num5, num3);
					text = string.Empty;
					num5 = num6;
				}
			}
			return;
		}
		int num7 = 0;
		foreach (StiStripPositionXF item in Axis.Info.TicksCollection)
		{
			float position = item.Position;
			if (enumerable != null && enumerable.Contains(position))
			{
				context.DrawLine(penLine, position, num4, position, y);
			}
			else
			{
				context.DrawLine(penLine, position, num2, position, num3);
			}
			if (ticks.MinorVisible && num7 != Axis.Info.TicksCollection.Count - 1)
			{
				float position2 = Axis.Info.TicksCollection[num7 + 1].Position;
				DrawMinorTicks(context, penLine, position, position2, num2, ticks);
			}
			num7++;
		}
	}

	private bool IsArgumentDateTime(List<StiStripPositionXF> infos)
	{
		foreach (StiStripPositionXF info in infos)
		{
			if (info.StripLine.ValueObject is DateTime)
			{
				return true;
			}
		}
		return false;
	}

	private void DrawAxis(StiContext context, RectangleF rect)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		StiPenGeom stiPenGeom = new StiPenGeom(Axis.LineColor, Axis.LineWidth);
		stiPenGeom.PenStyle = Axis.LineStyle;
		if (IsCenterAxis && (Axis.ShowXAxis == StiShowXAxis.Both || Axis.ShowXAxis == StiShowXAxis.Center))
		{
			DrawTicks(context, rect, Axis.Ticks, stiPenGeom);
		}
		else if (!IsCenterAxis)
		{
			if (((StiXAxisCoreXF)Axis.Core).IsTopSide)
			{
				DrawTicks(context, rect, Axis.Ticks, stiPenGeom);
			}
			if (((StiXAxisCoreXF)Axis.Core).IsBottomSide && (Axis.ShowXAxis == StiShowXAxis.Both || Axis.ShowXAxis == StiShowXAxis.Bottom))
			{
				DrawTicks(context, rect, Axis.Ticks, stiPenGeom);
			}
		}
		DrawAxisLine(context, rect);
	}

	private RectangleF GetViewClipRect()
	{
		RectangleF result = View.ClientRectangle;
		result.X = (float)((StiAxisAreaCoreXF)Axis.Area.Core).ScrollDistanceX;
		result.Y = 0f;
		result.Inflate(1f, 1f);
		if (IsCenterAxis)
		{
			result.Inflate(0f, 2f);
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
		if (!(rectangleF.Right < viewClipRect.X))
		{
			return !(rectangleF.X > viewClipRect.Right);
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

	public StiXAxisGeom(IStiXAxis axis, RectangleF clientRectangle, bool isCenterAxis)
		: base(clientRectangle)
	{
		this.axis = axis;
		this.isCenterAxis = isCenterAxis;
	}
}
