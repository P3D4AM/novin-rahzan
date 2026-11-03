using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiStackedLineSeriesGeom : StiBaseLineSeriesGeom
{
	public override bool Contains(float x, float y)
	{
		if (Invisible)
		{
			return false;
		}
		for (int i = 0; i < base.Points.Length - 1; i++)
		{
			PointF? pointF = base.Points[i];
			PointF? pointF2 = base.Points[i + 1];
			if (pointF.HasValue && pointF2.HasValue && StiPointHelper.IsLineContainsPoint(pointF.Value, pointF2.Value, StiMouseOverHelper.MouseOverLineDistance, new PointF(x, y)))
			{
				return true;
			}
		}
		return false;
	}

	public override void Draw(StiContext context)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		IStiStackedBaseLineSeries stiStackedBaseLineSeries = base.Series as IStiStackedBaseLineSeries;
		StiChart stiChart = base.Series.Chart as StiChart;
		if (base.Points.Length == 0)
		{
			return;
		}
		StiAnimation animation = GetAnimation();
		context.PushSmoothingModeToAntiAlias();
		StiPenStyle lineStyle = stiStackedBaseLineSeries.LineStyle;
		float num = stiStackedBaseLineSeries.LineWidth * context.Options.Zoom;
		StiPenGeom stiPenGeom = new StiPenGeom(stiStackedBaseLineSeries.LineColor, num);
		stiPenGeom.PenStyle = lineStyle;
		bool lighting = stiStackedBaseLineSeries.Lighting;
		if (stiStackedBaseLineSeries.ShowShadow)
		{
			StiPenGeom stiPenGeom2 = new StiPenGeom(Color.FromArgb(50, 0, 0, 0), num + 0.5f * context.Options.Zoom);
			stiPenGeom2.PenStyle = lineStyle;
			context.PushTranslateTransform(num, num);
			StiNullableDrawing.DrawLines(context, stiPenGeom2, base.Points, animation);
			context.PopTransform();
		}
		if (base.Series.Core.IsMouseOver)
		{
			float lineWidth = stiStackedBaseLineSeries.LineWidth;
			float zoom = context.Options.Zoom;
			float num2 = 11f + lineWidth;
			PointF?[] points = base.Points;
			for (int i = 0; i < points.Length; i++)
			{
				PointF? pointF = points[i];
				if (pointF.HasValue)
				{
					RectangleF rect = new RectangleF(pointF.Value.X - num2 / 2f * zoom, pointF.Value.Y - num2 / 2f * zoom, num2 * zoom, num2 * zoom);
					context.FillEllipse(StiMouseOverHelper.GetLineMouseOverColor(stiStackedBaseLineSeries.LineColor), rect);
				}
			}
			StiPenGeom penGeom = new StiPenGeom(StiMouseOverHelper.GetLineMouseOverColor(stiStackedBaseLineSeries.LineColor), (4f + lineWidth) * context.Options.Zoom)
			{
				StartCap = (StiPenLineCap)2,
				EndCap = (StiPenLineCap)2
			};
			StiNullableDrawing.DrawLines(context, penGeom, base.Points);
		}
		Color lineColor = ((StiStackedLineSeries)base.Series).LineColor;
		if (((StiStackedLineSeries)base.Series).AllowApplyColorNegative)
		{
			Color lineColorNegative = ((StiStackedLineSeries)base.Series).LineColorNegative;
			StiPenGeom penGeom2 = new StiPenGeom(lineColorNegative, num)
			{
				PenStyle = lineStyle
			};
			IStiAxisArea stiAxisArea = base.Series.Chart.Area as IStiAxisArea;
			float dividerY = stiAxisArea.AxisCore.GetDividerY();
			List<PointF?> list = new List<PointF?>();
			List<PointF?> list2 = new List<PointF?>();
			for (int j = 0; j < base.Points.Length; j++)
			{
				PointF? pointF2 = base.Points[j];
				PointF? item = ((j != base.Points.Length - 1) ? base.Points[j + 1] : null);
				if (pointF2.Value.Y > dividerY)
				{
					list.Add(pointF2);
					if (item.HasValue && !(item.Value.Y < dividerY) && (item.Value.Y != dividerY || j + 2 >= base.Points.Length || !(base.Points[j + 2].Value.Y <= dividerY)))
					{
						continue;
					}
					if (item.HasValue)
					{
						PointF? item2 = GetPointCross(pointF2, base.Points[j + 1], dividerY);
						list.Add(item2);
						if (item.Value.Y == dividerY && j + 2 < base.Points.Length && base.Points[j + 2].Value.Y <= dividerY)
						{
							list.Add(base.Points[j + 2]);
						}
						list2.Add(item2);
					}
					StiNullableDrawing.DrawLines(context, penGeom2, list.ToArray(), animation);
					if (num >= 2f * context.Options.Zoom && lighting)
					{
						float num3 = 0.5f * context.Options.Zoom;
						context.PushTranslateTransform(0f - num3, 0f - num3);
						StiPenGeom penGeom3 = new StiPenGeom(StiColorUtils.Light(lineColorNegative, (byte)70), num)
						{
							PenStyle = lineStyle
						};
						StiNullableDrawing.DrawLines(context, penGeom3, list.ToArray(), animation);
						context.PopTransform();
					}
					list.Clear();
					continue;
				}
				list2.Add(pointF2);
				if (!item.HasValue || item.Value.Y > dividerY)
				{
					if (item.HasValue)
					{
						PointF pointCross = GetPointCross(pointF2, base.Points[j + 1], dividerY);
						list.Add(pointCross);
						list2.Add(pointCross);
						list2.Add(item);
					}
					StiNullableDrawing.DrawLines(context, stiPenGeom, list2.ToArray(), animation);
					if (num >= 2f * context.Options.Zoom && lighting)
					{
						float num4 = 0.5f * context.Options.Zoom;
						context.PushTranslateTransform(0f - num4, 0f - num4);
						StiPenGeom penGeom4 = new StiPenGeom(StiColorUtils.Light(lineColor, (byte)70), num)
						{
							PenStyle = lineStyle
						};
						StiNullableDrawing.DrawLines(context, penGeom4, list2.ToArray(), animation);
						context.PopTransform();
					}
					list2.Clear();
				}
			}
		}
		else
		{
			StiNullableDrawing.DrawLines(context, stiPenGeom, base.Points, animation);
			if (num >= 2f * context.Options.Zoom && stiStackedBaseLineSeries.Lighting)
			{
				float num5 = 0.5f * context.Options.Zoom;
				context.PushTranslateTransform(0f - num5, 0f - num5);
				StiPenGeom penGeom5 = new StiPenGeom(StiColorUtils.Light(lineColor, (byte)70), num)
				{
					PenStyle = lineStyle
				};
				StiNullableDrawing.DrawLines(context, penGeom5, base.Points, animation);
				context.PopTransform();
			}
		}
		context.PopSmoothingMode();
	}

	private PointF GetPointCross(PointF? point1, PointF? point2, float posY)
	{
		float y = point1.Value.Y;
		float x = point1.Value.X;
		float y2 = point2.Value.Y;
		float x2 = point2.Value.X;
		double num = Math.Tan(Math.Atan((x2 - x) / (y - y2))) * (double)(y - posY) + (double)x;
		return new PointF((float)num, posY);
	}

	public StiStackedLineSeriesGeom(StiAreaGeom areaGeom, StiSeriesPointsInfo pointsInfo, IStiSeries series)
		: base(areaGeom, pointsInfo, series)
	{
	}
}
