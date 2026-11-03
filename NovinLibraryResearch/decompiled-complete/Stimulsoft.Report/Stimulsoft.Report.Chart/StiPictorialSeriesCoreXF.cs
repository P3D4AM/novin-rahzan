using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiPictorialSeriesCoreXF : StiSeriesCoreXF
{
	private class DataPictorial
	{
		public double Value { get; set; }

		public StiPictorialSeries Series { get; set; }

		public int Index { get; set; }

		public DataPictorial(double value, StiPictorialSeries series, int index)
		{
			Value = value;
			Index = index;
			Series = series;
		}
	}

	private SizeF singleSizeConst = new Size(25, 25);

	public override string LocalizedName => StiLocalization.Get("Chart", "Pictorial");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiPictorialSeries stiPictorialSeries = base.Series as IStiPictorialSeries;
		if (stiPictorialSeries.AllowApplyStyle)
		{
			stiPictorialSeries.Brush = style.Core.GetColumnBrush(color);
		}
	}

	internal SizeF GetSingleSize(StiContext context)
	{
		return new SizeF(singleSizeConst.Width * context.Options.Zoom, singleSizeConst.Height * context.Options.Zoom);
	}

	private bool ShowEmptyGeom(IStiSeries[] seriesArray)
	{
		foreach (IStiSeries stiSeries in seriesArray)
		{
			if (stiSeries.Values.Length != 0)
			{
				return false;
			}
		}
		return true;
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] seriesArray)
	{
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		SizeF singleSize = GetSingleSize(context);
		int num = (int)Math.Floor(rect.Width / singleSize.Width);
		int num2 = (int)Math.Floor(rect.Height / singleSize.Height);
		float num3 = (float)num * singleSize.Width;
		float num4 = (float)num2 * singleSize.Height;
		float x2 = rect.X + (rect.Width - num3) / 2f;
		float y = rect.Y + (rect.Height - num4) / 2f;
		RectangleF rectangleF = new RectangleF(x2, y, num3, num4);
		int num5 = num * num2;
		if (ShowEmptyGeom(seriesArray) && seriesArray.Length != 0)
		{
			StiPictorialEmptySeriesElementGeom item = new StiPictorialEmptySeriesElementGeom(geom, 1.0, 0, (StiBrush)new StiSolidBrush(Color.LightGray), seriesArray[0], (StiFontIcons)31, new List<RectangleF> { rectangleF }, new List<RectangleF> { rectangleF }, rectangleF, null);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item);
			return;
		}
		StiPictorialArea stiPictorialArea = (StiPictorialArea)base.Series.Chart.Area;
		List<DataPictorial> list = new List<DataPictorial>();
		foreach (IStiSeries stiSeries in seriesArray)
		{
			StiPictorialSeries stiPictorialSeries = stiSeries as StiPictorialSeries;
			int num6 = 0;
			double?[] values = stiSeries.Values;
			for (int j = 0; j < values.Length; j++)
			{
				double? num7 = values[j];
				list.Add(new DataPictorial(Math.Abs((stiPictorialArea.RoundValues ? new double?(Math.Round(num7.HasValue ? num7.Value : 0.0)) : num7).GetValueOrDefault()), stiPictorialSeries, num6));
				num6++;
			}
		}
		double num8 = list.Sum((DataPictorial x) => x.Value);
		double num9 = (double)num5 / num8;
		if (num9 > 1.0 && stiPictorialArea.Actual)
		{
			num9 = 1.0;
		}
		PointF pointF = new PointF(rectangleF.X, rectangleF.Y);
		double num10 = 0.0;
		for (int k = 0; k < list.Count; k++)
		{
			double num11 = Math.Abs(list[k].Value);
			if (num11 == 0.0)
			{
				continue;
			}
			double num12 = num9 * num11;
			double num13 = (stiPictorialArea.RoundValues ? RoundPictorialValue(num12, num10) : num12);
			num10 += num12 - num13;
			double num14 = num13 * (double)singleSize.Width * (double)singleSize.Height;
			float num15 = (float)(num14 / (double)singleSize.Height);
			float num16 = num15;
			List<RectangleF> list2 = new List<RectangleF>();
			List<RectangleF> list3 = new List<RectangleF>();
			do
			{
				float num17 = 0f;
				PointF pointF2 = pointF;
				float num18 = rectangleF.Right - pointF.X;
				if (num18 > num16)
				{
					num17 = num16;
					pointF = new PointF(pointF.X + num16, pointF.Y);
				}
				else
				{
					num17 = num18;
					pointF = new PointF(rectangleF.X, pointF.Y + singleSize.Height);
				}
				num16 -= num17;
				float num19 = (pointF2.X - rectangleF.X) / singleSize.Width;
				float num20 = (float)((double)num19 - Math.Floor(num19)) * singleSize.Width;
				RectangleF item2 = new RectangleF(pointF2.X - num20, pointF2.Y, num17 + num20, singleSize.Height);
				RectangleF item3 = new RectangleF(pointF2.X, pointF2.Y, num17, singleSize.Height);
				list2.Add(item2);
				list3.Add(item3);
			}
			while (num16 > 0f && pointF.Y < rectangleF.Bottom);
			StiPictorialSeriesElementGeom item4 = new StiPictorialSeriesElementGeom(geom, num11, list[k].Index, GetSeriesBrush(k, list.Count), list[k].Series, list[k].Series.Icon.GetValueOrDefault(), list2, list3, rect, null);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item4);
		}
	}

	private double RoundPictorialValue(double currentFactorValue, double deltaValue)
	{
		if (deltaValue > 0.0)
		{
			return Math.Ceiling(currentFactorValue);
		}
		if (deltaValue < 0.0)
		{
			return Math.Floor(currentFactorValue);
		}
		return Math.Round(currentFactorValue);
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return ((IStiPictorialSeries)base.Series).Brush;
		}
		return seriesBrush;
	}

	public StiPictorialSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
