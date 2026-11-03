using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLineSeriesGeom : StiBaseLineSeriesGeom
{
	public PointF?[] PointsZeroConnect { get; }

	public PointF?[] PointsNullConnect { get; }

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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		StiPenStyle val = (StiPenStyle)0;
		float num = 1f;
		Color color = Color.Black;
		bool flag = true;
		IStiLineMarker stiLineMarker = null;
		bool flag2 = true;
		StiAnimation animation = GetAnimation();
		if (base.Series is IStiBaseLineSeries stiBaseLineSeries)
		{
			val = stiBaseLineSeries.LineStyle;
			num = stiBaseLineSeries.LineWidth;
			color = stiBaseLineSeries.LineColor;
			flag = stiBaseLineSeries.ShowShadow;
			stiLineMarker = stiBaseLineSeries.LineMarker;
			flag2 = stiBaseLineSeries.Lighting;
		}
		if (base.Series is IStiRadarLineSeries stiRadarLineSeries)
		{
			val = stiRadarLineSeries.LineStyle;
			num = stiRadarLineSeries.LineWidth;
			color = stiRadarLineSeries.LineColor;
			flag = stiRadarLineSeries.ShowShadow;
			flag2 = stiRadarLineSeries.Lighting;
		}
		if (base.Series is IStiRadarAreaSeries stiRadarAreaSeries)
		{
			val = stiRadarAreaSeries.LineStyle;
			num = stiRadarAreaSeries.LineWidth;
			color = stiRadarAreaSeries.LineColor;
			flag = stiRadarAreaSeries.ShowShadow;
			flag2 = stiRadarAreaSeries.Lighting;
		}
		context.PushSmoothingModeToAntiAlias();
		StiPenStyle penStyle = val;
		float num2 = num * context.Options.Zoom;
		StiPenGeom stiPenGeom = new StiPenGeom(color, num2);
		stiPenGeom.PenStyle = penStyle;
		StiChart stiChart = base.Series.Chart as StiChart;
		if (flag)
		{
			StiPenGeom stiPenGeom2 = new StiPenGeom(Color.FromArgb(50, 0, 0, 0), num2 + 0.5f * context.Options.Zoom);
			stiPenGeom2.PenStyle = penStyle;
			context.PushTranslateTransform(num2, num2);
			StiNullableDrawing.DrawLines(context, stiPenGeom2, base.PointsFrom, base.Points, animation);
			if (stiLineMarker != null && stiLineMarker.Visible)
			{
				StiSolidBrush brushMarker = new StiSolidBrush(Color.FromArgb(50, 0, 0, 0));
				stiLineMarker.Core.DrawLines(context, base.Points, context.Options.Zoom, brushMarker, null, stiLineMarker.Type, (float)stiLineMarker.Step, stiLineMarker.Size, stiLineMarker.Angle);
			}
			context.PopTransform();
		}
		if (base.Series.Core.IsMouseOver)
		{
			float zoom = context.Options.Zoom;
			float num3 = 11f + num;
			PointF?[] points = base.Points;
			for (int i = 0; i < points.Length; i++)
			{
				PointF? pointF = points[i];
				if (pointF.HasValue)
				{
					RectangleF rect = new RectangleF(pointF.Value.X - num3 / 2f * zoom, pointF.Value.Y - num3 / 2f * zoom, num3 * zoom, num3 * zoom);
					context.FillEllipse(StiMouseOverHelper.GetLineMouseOverColor(color), rect);
				}
			}
			StiPenGeom stiPenGeom3 = new StiPenGeom(StiMouseOverHelper.GetLineMouseOverColor(color), (4f + num) * context.Options.Zoom);
			stiPenGeom3.StartCap = (StiPenLineCap)2;
			stiPenGeom3.EndCap = (StiPenLineCap)2;
			StiNullableDrawing.DrawLines(context, stiPenGeom3, base.PointsFrom, base.Points, animation);
		}
		if (PointsZeroConnect != null)
		{
			StiPenGeom stiPenGeom4 = new StiPenGeom(color, num2);
			stiPenGeom4.PenStyle = (StiPenStyle)1;
			StiAnimation animationConnect = GetAnimationConnect(PointsZeroConnect);
			StiNullableDrawing.DrawLines(context, stiPenGeom4, null, PointsZeroConnect, animationConnect);
		}
		if (PointsNullConnect != null)
		{
			StiPenGeom stiPenGeom5 = new StiPenGeom(color, num2);
			stiPenGeom5.PenStyle = (StiPenStyle)1;
			StiAnimation animationConnect2 = GetAnimationConnect(PointsNullConnect);
			StiNullableDrawing.DrawLines(context, stiPenGeom5, null, PointsNullConnect, animationConnect2);
		}
		if (base.Series is StiLineSeries && ((StiLineSeries)base.Series).AllowApplyColorNegative)
		{
			Color lineColorNegative = ((StiLineSeries)base.Series).LineColorNegative;
			StiPenGeom stiPenGeom6 = new StiPenGeom(lineColorNegative, num2);
			stiPenGeom6.PenStyle = penStyle;
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
						PointF pointCross = GetPointCross(pointF2, base.Points[j + 1], dividerY);
						list.Add(pointCross);
						if (item.Value.Y == dividerY && j + 2 < base.Points.Length && base.Points[j + 2].Value.Y <= dividerY)
						{
							list.Add(base.Points[j + 2]);
						}
						list2.Add(pointCross);
					}
					StiNullableDrawing.DrawLines(context, stiPenGeom6, null, list.ToArray(), animation);
					if (num2 >= 2f * context.Options.Zoom && flag2)
					{
						float num4 = 0.5f * context.Options.Zoom;
						context.PushTranslateTransform(0f - num4, 0f - num4);
						StiPenGeom stiPenGeom7 = new StiPenGeom(StiColorUtils.Light(lineColorNegative, (byte)70), num2);
						stiPenGeom7.PenStyle = penStyle;
						StiNullableDrawing.DrawLines(context, stiPenGeom7, null, list.ToArray(), animation);
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
						PointF? item2 = GetPointCross(pointF2, base.Points[j + 1], dividerY);
						list.Add(item2);
						list2.Add(item2);
						list2.Add(item);
					}
					StiNullableDrawing.DrawLines(context, stiPenGeom, null, list2.ToArray(), animation);
					if (num2 >= 2f * context.Options.Zoom && flag2)
					{
						float num5 = 0.5f * context.Options.Zoom;
						context.PushTranslateTransform(0f - num5, 0f - num5);
						StiPenGeom stiPenGeom8 = new StiPenGeom(StiColorUtils.Light(color, (byte)70), num2);
						stiPenGeom8.PenStyle = penStyle;
						StiNullableDrawing.DrawLines(context, stiPenGeom8, null, list2.ToArray(), animation);
						context.PopTransform();
					}
					list2.Clear();
				}
			}
		}
		else
		{
			StiNullableDrawing.DrawLines(context, stiPenGeom, base.PointsFrom, base.Points, animation);
			if (num2 >= 2f * context.Options.Zoom && flag2)
			{
				float num6 = 0.5f * context.Options.Zoom;
				context.PushTranslateTransform(0f - num6, 0f - num6);
				StiPenGeom stiPenGeom9 = new StiPenGeom(StiColorUtils.Light(color, (byte)70), num2);
				stiPenGeom9.PenStyle = penStyle;
				StiNullableDrawing.DrawLines(context, stiPenGeom9, base.PointsFrom, base.Points, animation);
				context.PopTransform();
			}
		}
		if (stiLineMarker != null && stiLineMarker.Visible)
		{
			StiPenGeom penMarker = new StiPenGeom(stiLineMarker.BorderColor);
			stiLineMarker.Core.DrawLines(context, base.Points, context.Options.Zoom, stiLineMarker.Brush, penMarker, stiLineMarker.Type, (float)stiLineMarker.Step, stiLineMarker.Size, stiLineMarker.Angle);
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

	public StiLineSeriesGeom(StiAreaGeom areaGeom, StiSeriesPointsInfo pointsInfo, IStiSeries series)
		: base(areaGeom, pointsInfo, series)
	{
		PointsZeroConnect = pointsInfo.PointsZeroConnect;
		PointsNullConnect = pointsInfo.PointsNullConnect;
	}
}
