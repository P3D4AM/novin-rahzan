using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiCandlestickSeriesElementGeom : StiFinancialSeriesElementGeom
{
	private StiBrush brush;

	private Color borderColor;

	private TimeSpan beginTime;

	public StiBrush Brush => brush;

	public Color BorderColor => borderColor;

	public TimeSpan BeginTime => beginTime;

	public override void Draw(StiContext context)
	{
		IStiCandlestickArea stiCandlestickArea = base.AreaGeom.Area as IStiCandlestickArea;
		IStiCandlestickSeries stiCandlestickSeries = base.Series as IStiCandlestickSeries;
		float num = stiCandlestickSeries.BorderWidth * context.Options.Zoom;
		float num2 = (float)stiCandlestickArea.XAxis.Info.Dpi / 2f;
		float num3 = base.Open;
		float num4 = base.Close;
		float num5 = base.High;
		float num6 = base.Low;
		float num7 = base.PositionX;
		StiPenGeom pen = new StiPenGeom(borderColor, num);
		if (num4 == num3)
		{
			num3 -= 1f;
		}
		StiChart stiChart = base.Series.Chart as StiChart;
		if (stiChart.IsAnimation)
		{
			TimeSpan globalDurationElement = StiChartHelper.GlobalDurationElement;
			PointF[] points = new PointF[2]
			{
				new PointF(num7, num5),
				new PointF(num7, num3)
			};
			PointF[] points2 = new PointF[2]
			{
				new PointF(num7, num4),
				new PointF(num7, num6)
			};
			List<StiSegmentGeom> list = new List<StiSegmentGeom>();
			float num8 = num7 - num2 / 2f;
			float num9 = num3;
			float num10 = num8 + num2;
			float num11 = num9 + num4 - num3;
			list.Add(new StiLineSegmentGeom(num8, num9, num10, num9));
			list.Add(new StiLineSegmentGeom(num10, num9, num10, num11));
			list.Add(new StiLineSegmentGeom(num10, num11, num8, num11));
			list.Add(new StiLineSegmentGeom(num8, num11, num8, num9));
			list.Add(new StiCloseFigureSegmentGeom());
			StiOpacityAnimation animation = new StiOpacityAnimation(globalDurationElement, beginTime);
			if (stiCandlestickSeries.ShowShadow)
			{
				float num12 = num + 0.5f * context.Options.Zoom;
				StiPenGeom pen2 = new StiPenGeom(Color.FromArgb(50, 0, 0, 0), num12);
				PointF[] points3 = new PointF[2]
				{
					new PointF(num7 + num12 / 2f, num5 + num12 / 2f),
					new PointF(num7 + num12 / 2f, num3 + num12 / 2f)
				};
				PointF[] points4 = new PointF[2]
				{
					new PointF(num7 + num12 / 2f, num4 + num12 / 2f),
					new PointF(num7 + num12 / 2f, num6 + num12 / 2f)
				};
				context.DrawAnimationLines(pen2, points3, animation);
				context.DrawAnimationLines(pen2, points4, animation);
				List<StiSegmentGeom> list2 = new List<StiSegmentGeom>();
				list2.Add(new StiLineSegmentGeom(num8 + num12 / 2f, num9 + num12 / 2f, num10 + num12 / 2f, num9 + num12 / 2f));
				list2.Add(new StiLineSegmentGeom(num10 + num12 / 2f, num9 + num12 / 2f, num10 + num12 / 2f, num11 + num12 / 2f));
				list2.Add(new StiLineSegmentGeom(num10 + num12 / 2f, num11 + num12 / 2f, num8 + num12 / 2f, num11 + num12 / 2f));
				list2.Add(new StiLineSegmentGeom(num8 + num12 / 2f, num11 + num12 / 2f, num8 + num12 / 2f, num9 + num12 / 2f));
				list2.Add(new StiCloseFigureSegmentGeom());
				context.FillDrawAnimationPath(null, pen2, list2, new RectangleF(num8 + num12 / 2f, num9 + num12 / 2f, num2, num4 - num3), null, animation);
			}
			context.DrawAnimationLines(pen, points, animation);
			context.DrawAnimationLines(pen, points2, animation);
			context.FillDrawAnimationPath(brush, pen, list, new RectangleF(num8, num9, num2, num4 - num3), this, animation);
		}
		else
		{
			if (stiCandlestickSeries.ShowShadow)
			{
				float num13 = num + 0.5f * context.Options.Zoom;
				StiPenGeom pen3 = new StiPenGeom(Color.FromArgb(50, 0, 0, 0), num13);
				context.PushTranslateTransform(num, num);
				context.DrawLine(pen3, num7, num4 + num13 / 2f, num7, num6 + num13 / 2f);
				context.DrawLine(pen3, num7, num5, num7, num3 - num13 / 2f);
				context.DrawRectangle(pen3, num7 - num2 / 2f, num3, num2, num4 - num3);
				context.PopTransform();
			}
			context.DrawLine(pen, num7, num5, num7, num3);
			context.DrawLine(pen, num7, num4, num7, num6);
			context.FillRectangle(brush, num7 - num2 / 2f, num3, num2, num4 - num3);
			context.DrawRectangle(pen, num7 - num2 / 2f, num3, num2, num4 - num3);
		}
	}

	public StiCandlestickSeriesElementGeom(StiAreaGeom areaGeom, IStiSeries series, RectangleF clientRectangle, float bodyStart, float bodyEnd, float high, float low, float positionX, int index, StiBrush brush, Color borderColor, TimeSpan beginTime)
		: base(areaGeom, series, clientRectangle, bodyStart, bodyEnd, high, low, positionX, index)
	{
		this.brush = brush;
		this.borderColor = borderColor;
		this.beginTime = beginTime;
	}
}
