using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiSplineSeriesGeom : StiBaseLineSeriesGeom
{
	public PointF?[] PointsZeroConnect { get; private set; }

	public PointF?[] PointsNullConnect { get; private set; }

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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		StiPenStyle val = (StiPenStyle)0;
		float num = 1f;
		Color color = Color.Black;
		bool flag = true;
		bool flag2 = true;
		float tension = 1f;
		StiAnimation animation = GetAnimation();
		IStiSplineSeries stiSplineSeries = base.Series as IStiSplineSeries;
		if (stiSplineSeries != null)
		{
			val = stiSplineSeries.LineStyle;
			num = stiSplineSeries.LineWidth;
			color = stiSplineSeries.LineColor;
			flag = stiSplineSeries.ShowShadow;
			flag2 = stiSplineSeries.Lighting;
			tension = stiSplineSeries.Tension;
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
			StiNullableDrawing.DrawCurve(context, stiPenGeom2, base.Points, tension, animation);
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
			StiNullableDrawing.DrawCurve(context, stiPenGeom3, base.Points, tension, animation);
		}
		if (PointsZeroConnect != null)
		{
			StiPenGeom stiPenGeom4 = new StiPenGeom(color, num2);
			stiPenGeom4.PenStyle = (StiPenStyle)1;
			StiAnimation animationConnect = GetAnimationConnect(PointsZeroConnect);
			StiNullableDrawing.DrawCurve(context, stiPenGeom4, null, PointsZeroConnect, tension, animationConnect);
		}
		if (PointsNullConnect != null)
		{
			StiPenGeom stiPenGeom5 = new StiPenGeom(color, num2);
			stiPenGeom5.PenStyle = (StiPenStyle)1;
			StiAnimation animationConnect2 = GetAnimationConnect(PointsNullConnect);
			StiNullableDrawing.DrawCurve(context, stiPenGeom5, null, PointsNullConnect, tension, animationConnect2);
		}
		StiNullableDrawing.DrawCurve(context, stiPenGeom, base.PointsFrom, base.Points, tension, animation);
		if (num2 >= 2f && flag2)
		{
			float num4 = 0.5f * context.Options.Zoom;
			context.PushTranslateTransform(0f - num4, 0f - num4);
			StiPenGeom stiPenGeom6 = new StiPenGeom(StiColorUtils.Light(color, (byte)70), num2);
			stiPenGeom6.PenStyle = penStyle;
			StiNullableDrawing.DrawCurve(context, stiPenGeom6, base.Points, tension, animation);
			context.PopTransform();
		}
		if (stiSplineSeries != null && stiSplineSeries.AllowApplyColorNegative)
		{
			StiPenGeom stiPenGeom7 = new StiPenGeom(stiSplineSeries.LineColorNegative, num2);
			stiPenGeom7.PenStyle = penStyle;
			IStiAxisArea stiAxisArea = base.Series.Chart.Area as IStiAxisArea;
			float dividerY = stiAxisArea.AxisCore.GetDividerY();
			float width = (float)(stiAxisArea.AxisCore.ScrollRangeX * stiAxisArea.AxisCore.ScrollDpiX);
			float height = (float)(stiAxisArea.AxisCore.ScrollRangeY * stiAxisArea.AxisCore.ScrollDpiY - (double)dividerY);
			RectangleF clipRect = new RectangleF(0f, dividerY, width, height);
			context.PushClip(clipRect);
			StiNullableDrawing.DrawCurve(context, stiPenGeom7, base.Points, tension, animation);
			if (num2 >= 2f && flag2)
			{
				float num5 = 0.5f * context.Options.Zoom;
				context.PushTranslateTransform(0f - num5, 0f - num5);
				StiPenGeom stiPenGeom8 = new StiPenGeom(StiColorUtils.Light(stiSplineSeries.LineColorNegative, (byte)70), num2);
				stiPenGeom8.PenStyle = penStyle;
				StiNullableDrawing.DrawCurve(context, stiPenGeom8, base.Points, tension, animation);
				context.PopTransform();
			}
			context.PopClip();
		}
		context.PopSmoothingMode();
	}

	public StiSplineSeriesGeom(StiAreaGeom areaGeom, StiSeriesPointsInfo pointsInfo, IStiSeries series)
		: base(areaGeom, pointsInfo, series)
	{
		PointsZeroConnect = pointsInfo.PointsZeroConnect;
		PointsNullConnect = pointsInfo.PointsNullConnect;
	}
}
