using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;

namespace Stimulsoft.Report.Chart;

public class StiStockSeriesElementGeom : StiFinancialSeriesElementGeom
{
	private Color color;

	private TimeSpan beginTime;

	public Color Color => color;

	public TimeSpan BeginTime => beginTime;

	public override void Draw(StiContext context)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		IStiStockArea stiStockArea = base.AreaGeom.Area as IStiStockArea;
		IStiStockSeries stiStockSeries = base.Series as IStiStockSeries;
		float num = stiStockSeries.LineWidth * context.Options.Zoom;
		float num2 = (float)stiStockArea.XAxis.Info.Dpi / 3f;
		StiPenGeom pen = new StiPenGeom(color, num)
		{
			PenStyle = stiStockSeries.LineStyle
		};
		StiChart stiChart = base.Series.Chart as StiChart;
		if (stiChart.IsAnimation)
		{
			StiOpacityAnimation animation = new StiOpacityAnimation(StiChartHelper.GlobalDurationElement, beginTime);
			if (stiStockSeries.ShowShadow)
			{
				float num3 = num + 0.5f * context.Options.Zoom;
				StiPenGeom pen2 = new StiPenGeom(Color.FromArgb(50, 0, 0, 0), num3)
				{
					PenStyle = stiStockSeries.LineStyle
				};
				PointF[] points = new PointF[2]
				{
					new PointF(base.PositionX, base.High),
					new PointF(base.PositionX, base.Low)
				};
				PointF[] points2 = new PointF[2]
				{
					new PointF(base.PositionX - num2, base.Open),
					new PointF(base.PositionX - num3 / 2f, base.Open)
				};
				PointF[] points3 = new PointF[2]
				{
					new PointF(base.PositionX + num2, base.Close),
					new PointF(base.PositionX + num3 / 2f, base.Close)
				};
				context.DrawAnimationLines(pen2, points, animation);
				context.DrawAnimationLines(pen2, points2, animation);
				context.DrawAnimationLines(pen2, points3, animation);
			}
			PointF[] points4 = new PointF[2]
			{
				new PointF(base.PositionX, base.High),
				new PointF(base.PositionX, base.Low)
			};
			PointF[] points5 = new PointF[2]
			{
				new PointF(base.PositionX - num2, base.Open),
				new PointF(base.PositionX, base.Open)
			};
			PointF[] points6 = new PointF[2]
			{
				new PointF(base.PositionX + num2, base.Close),
				new PointF(base.PositionX, base.Close)
			};
			context.DrawAnimationLines(pen, points4, animation);
			context.DrawAnimationLines(pen, points5, animation);
			context.DrawAnimationLines(pen, points6, animation);
		}
		else
		{
			if (stiStockSeries.ShowShadow)
			{
				float num4 = num + 0.5f * context.Options.Zoom;
				StiPenGeom pen3 = new StiPenGeom(Color.FromArgb(50, 0, 0, 0), num4)
				{
					PenStyle = stiStockSeries.LineStyle
				};
				context.PushTranslateTransform(num, num);
				context.DrawLine(pen3, base.PositionX, base.High, base.PositionX, base.Low);
				context.DrawLine(pen3, base.PositionX - num2, base.Open, base.PositionX - num4 / 2f, base.Open);
				context.DrawLine(pen3, base.PositionX + num2, base.Close, base.PositionX + num4 / 2f, base.Close);
				context.PopTransform();
			}
			context.DrawLine(pen, base.PositionX, base.High, base.PositionX, base.Low);
			context.DrawLine(pen, base.PositionX - num2, base.Open, base.PositionX, base.Open);
			context.DrawLine(pen, base.PositionX + num2, base.Close, base.PositionX, base.Close);
		}
	}

	public StiStockSeriesElementGeom(StiAreaGeom areaGeom, IStiSeries series, RectangleF clientRectangle, float open, float close, float high, float low, float positionX, int index, Color color, TimeSpan beginTime)
		: base(areaGeom, series, clientRectangle, open, close, high, low, positionX, index)
	{
		this.color = color;
		this.beginTime = beginTime;
	}
}
