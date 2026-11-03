using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiStackedSplineSeriesGeom : StiBaseLineSeriesGeom
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
			if (pointF.HasValue && pointF2.HasValue && StiPointHelper.IsLineContainsPoint(pointF.Value, pointF2.Value, StiMouseOverHelper.MouseOverSplineDistance, new PointF(x, y)))
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
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		if (base.Points.Length == 0)
		{
			return;
		}
		StiChart stiChart = base.Series.Chart as StiChart;
		IStiStackedSplineSeries stiStackedSplineSeries = base.Series as IStiStackedSplineSeries;
		StiAnimation animation = GetAnimation();
		context.PushSmoothingModeToAntiAlias();
		StiPenStyle lineStyle = stiStackedSplineSeries.LineStyle;
		float num = stiStackedSplineSeries.LineWidth * context.Options.Zoom;
		StiPenGeom penGeom = new StiPenGeom(stiStackedSplineSeries.LineColor, num)
		{
			PenStyle = lineStyle
		};
		if (base.Points != null && base.Points.Length > 1)
		{
			if (stiStackedSplineSeries.ShowShadow)
			{
				StiPenGeom stiPenGeom = new StiPenGeom(Color.FromArgb(50, 0, 0, 0), num + 0.5f * context.Options.Zoom);
				stiPenGeom.PenStyle = lineStyle;
				context.PushTranslateTransform(num, num);
				StiNullableDrawing.DrawCurve(context, stiPenGeom, base.Points, stiStackedSplineSeries.Tension, animation);
				context.PopTransform();
			}
			if (base.Series.Core.IsMouseOver)
			{
				float lineWidth = stiStackedSplineSeries.LineWidth;
				float zoom = context.Options.Zoom;
				float num2 = 11f + lineWidth;
				PointF?[] points = base.Points;
				for (int i = 0; i < points.Length; i++)
				{
					PointF? pointF = points[i];
					if (pointF.HasValue)
					{
						RectangleF rect = new RectangleF(pointF.Value.X - num2 / 2f * zoom, pointF.Value.Y - num2 / 2f * zoom, num2 * zoom, num2 * zoom);
						context.FillEllipse(StiMouseOverHelper.GetLineMouseOverColor(stiStackedSplineSeries.LineColor), rect);
					}
				}
				StiPenGeom penGeom2 = new StiPenGeom(StiMouseOverHelper.GetLineMouseOverColor(stiStackedSplineSeries.LineColor), (4f + lineWidth) * context.Options.Zoom)
				{
					StartCap = (StiPenLineCap)2,
					EndCap = (StiPenLineCap)2
				};
				StiNullableDrawing.DrawCurve(context, penGeom2, base.Points, stiStackedSplineSeries.Tension);
			}
			StiNullableDrawing.DrawCurve(context, penGeom, base.Points, stiStackedSplineSeries.Tension, animation);
			if (num >= 2f && stiStackedSplineSeries.Lighting)
			{
				float num3 = 0.5f * context.Options.Zoom;
				context.PushTranslateTransform(0f - num3, 0f - num3);
				StiPenGeom stiPenGeom2 = new StiPenGeom(StiColorUtils.Light(stiStackedSplineSeries.LineColor, (byte)70), num);
				stiPenGeom2.PenStyle = lineStyle;
				StiNullableDrawing.DrawCurve(context, stiPenGeom2, base.Points, stiStackedSplineSeries.Tension, animation);
				context.PopTransform();
			}
			if (stiStackedSplineSeries.AllowApplyColorNegative)
			{
				StiPenGeom penGeom3 = new StiPenGeom(stiStackedSplineSeries.LineColorNegative, num)
				{
					PenStyle = lineStyle
				};
				IStiAxisArea stiAxisArea = base.Series.Chart.Area as IStiAxisArea;
				float dividerY = stiAxisArea.AxisCore.GetDividerY();
				float width = (float)(stiAxisArea.AxisCore.ScrollRangeX * stiAxisArea.AxisCore.ScrollDpiX);
				float height = (float)(stiAxisArea.AxisCore.ScrollRangeY * stiAxisArea.AxisCore.ScrollDpiY - (double)dividerY);
				RectangleF clipRect = new RectangleF(0f, dividerY, width, height);
				context.PushClip(clipRect);
				StiNullableDrawing.DrawCurve(context, penGeom3, base.Points, stiStackedSplineSeries.Tension, animation);
				if (num >= 2f && stiStackedSplineSeries.Lighting)
				{
					float num4 = 0.5f * context.Options.Zoom;
					context.PushTranslateTransform(0f - num4, 0f - num4);
					StiPenGeom penGeom4 = new StiPenGeom(StiColorUtils.Light(stiStackedSplineSeries.LineColorNegative, (byte)70), num)
					{
						PenStyle = lineStyle
					};
					StiNullableDrawing.DrawCurve(context, penGeom4, base.Points, stiStackedSplineSeries.Tension, animation);
					context.PopTransform();
				}
				context.PopClip();
			}
		}
		context.PopSmoothingMode();
	}

	public StiStackedSplineSeriesGeom(StiAreaGeom areaGeom, StiSeriesPointsInfo pointsInfo, IStiSeries series)
		: base(areaGeom, pointsInfo, series)
	{
	}
}
