using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiBoxAndWhiskerSeriesElementGeom : StiCellGeom
{
	public StiBrush Brush { get; }

	public Color BorderColor { get; }

	public TimeSpan BeginTime { get; }

	public StiAreaGeom AreaGeom { get; }

	public double? Mean { get; }

	public float Maximum { get; }

	public float Minimum { get; }

	public float Median { get; }

	public float FirstQuartile { get; }

	public float ThirdQuartile { get; }

	public float PositionX { get; }

	public double[] Values { get; }

	public IStiSeries Series { get; }

	public override void Draw(StiContext context)
	{
		StiChart stiChart = Series.Chart as StiChart;
		IStiBoxAndWhiskerArea stiBoxAndWhiskerArea = AreaGeom.Area as IStiBoxAndWhiskerArea;
		StiPenGeom pen = new StiPenGeom(BorderColor, GetSeriesBorderThickness());
		float num = (float)stiBoxAndWhiskerArea.XAxis.Info.Dpi / 2f;
		float num2 = num / 3f;
		float num3 = num2 / 6f;
		context.PushSmoothingModeToAntiAlias();
		if (stiChart.IsAnimation)
		{
			StiOpacityAnimation animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, BeginTime);
			context.DrawAnimationLines(pen, new PointF[2]
			{
				new PointF(PositionX - num2 / 2f, Maximum),
				new PointF(PositionX + num2 / 2f, Maximum)
			}, animation);
			context.DrawAnimationLines(pen, new PointF[2]
			{
				new PointF(PositionX - num2 / 2f, Minimum),
				new PointF(PositionX + num2 / 2f, Minimum)
			}, animation);
			context.DrawAnimationLines(pen, new PointF[2]
			{
				new PointF(PositionX, Maximum),
				new PointF(PositionX, ThirdQuartile)
			}, animation);
			context.DrawAnimationLines(pen, new PointF[2]
			{
				new PointF(PositionX, FirstQuartile),
				new PointF(PositionX, Minimum)
			}, animation);
			context.DrawAnimationRectangle(Brush, pen, new RectangleF(PositionX - num / 2f, ThirdQuartile, num, FirstQuartile - ThirdQuartile), null, animation);
			context.DrawAnimationLines(pen, new PointF[2]
			{
				new PointF(PositionX - num / 2f, Median),
				new PointF(PositionX + num / 2f, Median)
			}, animation);
			if (Values != null)
			{
				int num4 = 1;
				double[] values = Values;
				foreach (double num5 in values)
				{
					StiOpacityAnimation animation2 = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, BeginTime + new TimeSpan(StiChartHelper.GlobalBeginTimeElement.Ticks / Values.Length * num4));
					context.FillDrawAnimationEllipse(null, pen, PositionX - num3 / 2f, (float)num5 - num3 / 2f, num3, num3, null, null, animation2);
					num4++;
				}
			}
			if (Mean.HasValue)
			{
				float num6 = (float)Mean.GetValueOrDefault();
				context.DrawAnimationLines(pen, new PointF[2]
				{
					new PointF(PositionX - num3, num6 - num3),
					new PointF(PositionX + num3, num6 + num3)
				}, animation);
				context.DrawAnimationLines(pen, new PointF[2]
				{
					new PointF(PositionX + num3, num6 - num3),
					new PointF(PositionX - num3, num6 + num3)
				}, animation);
			}
		}
		else
		{
			context.DrawLine(pen, PositionX - num2 / 2f, Maximum, PositionX + num2 / 2f, Maximum);
			context.DrawLine(pen, PositionX - num2 / 2f, Minimum, PositionX + num2 / 2f, Minimum);
			context.DrawLine(pen, PositionX, Maximum, PositionX, ThirdQuartile);
			context.DrawLine(pen, PositionX, FirstQuartile, PositionX, Minimum);
			context.FillRectangle(Brush, PositionX - num / 2f, ThirdQuartile, num, FirstQuartile - ThirdQuartile);
			context.DrawRectangle(pen, PositionX - num / 2f, ThirdQuartile, num, FirstQuartile - ThirdQuartile);
			context.DrawLine(pen, PositionX - num / 2f, Median, PositionX + num / 2f, Median);
			if (Values != null)
			{
				double[] values2 = Values;
				foreach (double num7 in values2)
				{
					context.DrawEllipse(pen, new RectangleF(PositionX - num3 / 2f, (float)num7 - num3 / 2f, num3, num3));
				}
			}
			if (Mean.HasValue)
			{
				float num8 = (float)Mean.GetValueOrDefault();
				context.DrawLine(pen, PositionX - num3, num8 - num3, PositionX + num3, num8 + num3);
				context.DrawLine(pen, PositionX + num3, num8 - num3, PositionX - num3, num8 + num3);
			}
		}
		context.PopSmoothingMode();
	}

	private int GetSeriesBorderThickness()
	{
		if (Series is IStiSeriesBorderThickness stiSeriesBorderThickness)
		{
			return stiSeriesBorderThickness.BorderThickness;
		}
		return 1;
	}

	public StiBoxAndWhiskerSeriesElementGeom(StiAreaGeom areaGeom, IStiSeries series, float positionX, float minimum, float maximim, float firstQuartile, float thirdQuartile, float median, double[] values, double? mean, RectangleF clientRectangle, StiBrush brush, Color borderColor, TimeSpan beginTime)
		: base(clientRectangle)
	{
		Series = series;
		AreaGeom = areaGeom;
		PositionX = positionX;
		Minimum = minimum;
		Maximum = maximim;
		FirstQuartile = firstQuartile;
		ThirdQuartile = thirdQuartile;
		Median = median;
		Mean = mean;
		Values = values;
		Brush = brush;
		BorderColor = borderColor;
		BeginTime = beginTime;
	}
}
