using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiScatterSplineSeriesGeom : StiBaseLineSeriesGeom
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		IStiScatterSplineSeries stiScatterSplineSeries = base.Series as IStiScatterSplineSeries;
		context.PushSmoothingModeToAntiAlias();
		StiPenStyle lineStyle = stiScatterSplineSeries.LineStyle;
		float num = stiScatterSplineSeries.LineWidth * context.Options.Zoom;
		StiAnimation animation = GetAnimation();
		StiPenGeom penGeom = new StiPenGeom(stiScatterSplineSeries.LineColor, stiScatterSplineSeries.LineWidth)
		{
			PenStyle = lineStyle
		};
		if (stiScatterSplineSeries.ShowShadow)
		{
			StiPenGeom stiPenGeom = new StiPenGeom(Color.FromArgb(50, 0, 0, 0), num + 0.5f * context.Options.Zoom);
			stiPenGeom.PenStyle = lineStyle;
			context.PushTranslateTransform(num, num);
			StiNullableDrawing.DrawCurve(context, stiPenGeom, base.Points, stiScatterSplineSeries.Tension, animation);
			context.PopTransform();
		}
		if (base.Series.Core.IsMouseOver)
		{
			float lineWidth = stiScatterSplineSeries.LineWidth;
			float zoom = context.Options.Zoom;
			float num2 = 11f + lineWidth;
			PointF?[] points = base.Points;
			for (int i = 0; i < points.Length; i++)
			{
				PointF? pointF = points[i];
				if (pointF.HasValue)
				{
					RectangleF rect = new RectangleF(pointF.Value.X - num2 / 2f * zoom, pointF.Value.Y - num2 / 2f * zoom, num2 * zoom, num2 * zoom);
					context.FillEllipse(StiMouseOverHelper.GetLineMouseOverColor(stiScatterSplineSeries.LineColor), rect);
				}
			}
			StiPenGeom stiPenGeom2 = new StiPenGeom(StiMouseOverHelper.GetLineMouseOverColor(stiScatterSplineSeries.LineColor), (4f + lineWidth) * context.Options.Zoom);
			stiPenGeom2.StartCap = (StiPenLineCap)2;
			stiPenGeom2.EndCap = (StiPenLineCap)2;
			StiNullableDrawing.DrawCurve(context, stiPenGeom2, base.Points, stiScatterSplineSeries.Tension, animation);
		}
		StiNullableDrawing.DrawCurve(context, penGeom, base.Points, stiScatterSplineSeries.Tension, animation);
		if (num >= 2f && stiScatterSplineSeries.Lighting)
		{
			float num3 = 0.5f * context.Options.Zoom;
			context.PushTranslateTransform(0f - num3, 0f - num3);
			StiPenGeom stiPenGeom3 = new StiPenGeom(StiColorUtils.Light(stiScatterSplineSeries.LineColor, (byte)70), 1f);
			stiPenGeom3.PenStyle = lineStyle;
			StiNullableDrawing.DrawCurve(context, stiPenGeom3, base.Points, stiScatterSplineSeries.Tension, animation);
			context.PopTransform();
		}
		context.PopSmoothingMode();
	}

	public StiScatterSplineSeriesGeom(StiAreaGeom areaGeom, StiSeriesPointsInfo pointsInfo, IStiSeries series)
		: base(areaGeom, pointsInfo, series)
	{
	}
}
