using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiCandlestickSeriesCoreXF : StiSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Candlestick");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		if (base.Series.AllowApplyStyle)
		{
			StiBrush columnBrush = style.Core.GetColumnBrush(color);
			((IStiCandlestickSeries)base.Series).Brush = columnBrush;
			Color color2 = style.Core.GetColumnBorder(color);
			if (color2 == Color.Transparent)
			{
				color2 = StiBrush.ToColor(columnBrush);
			}
			((IStiCandlestickSeries)base.Series).BorderColor = color2;
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		if (series == null || series.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		IStiCandlestickArea stiCandlestickArea = geom.Area as IStiCandlestickArea;
		float dividerY = stiCandlestickArea.AxisCore.GetDividerY();
		TimeSpan globalBeginTimeElement = StiChartHelper.GlobalBeginTimeElement;
		for (int i = 0; i < series.Length; i++)
		{
			IStiCandlestickSeries stiCandlestickSeries = (IStiCandlestickSeries)series[i];
			double?[] valuesOpen = stiCandlestickSeries.ValuesOpen;
			double?[] valuesClose = stiCandlestickSeries.ValuesClose;
			double?[] valuesHigh = stiCandlestickSeries.ValuesHigh;
			double?[] valuesLow = stiCandlestickSeries.ValuesLow;
			int num = Math.Min(Math.Min(valuesOpen.Length, valuesClose.Length), Math.Min(valuesHigh.Length, valuesLow.Length));
			int val = stiCandlestickSeries.Arguments.Length;
			int num2 = Math.Min(num, val);
			for (int j = 0; j < num2; j++)
			{
				double? num3 = valuesOpen[j];
				double? num4 = valuesClose[j];
				double? num5 = valuesHigh[j];
				double? num6 = valuesLow[j];
				if (!num3.HasValue || !num4.HasValue || !num5.HasValue || !num6.HasValue)
				{
					continue;
				}
				int num7 = 0;
				foreach (StiStripLineXF stripLine in stiCandlestickArea.XAxis.Info.StripLines)
				{
					if (stiCandlestickSeries.Arguments[j]?.ToString() == stripLine.ValueObject?.ToString())
					{
						break;
					}
					num7++;
				}
				float num8 = (float)stiCandlestickArea.XAxis.Info.Dpi;
				Color borderColor;
				StiBrush seriesBrush;
				if (num3 > num4)
				{
					seriesBrush = stiCandlestickSeries.Brush;
					borderColor = stiCandlestickSeries.BorderColor;
					if (num5 < num3)
					{
						num5 = num3;
					}
					if (num4 < num6)
					{
						num6 = num4;
					}
				}
				else
				{
					seriesBrush = stiCandlestickSeries.BrushNegative;
					borderColor = stiCandlestickSeries.BorderColorNegative;
					if (num6 > num3)
					{
						num6 = num3;
					}
					if (num4 > num5)
					{
						num5 = num4;
					}
					num3 = valuesClose[j];
					num4 = valuesOpen[j];
				}
				if (stiCandlestickArea.ReverseVert && num3 > num4)
				{
					double? num9 = num3;
					num3 = num4;
					num4 = num9;
					num9 = num6;
					num6 = num5;
					num5 = num9;
				}
				float num10 = (float)stiCandlestickArea.YAxis.Info.Dpi;
				float num11 = Math.Abs(dividerY - (float)num3.Value * num10);
				float num12 = Math.Abs(dividerY - (float)num4.Value * num10);
				float high = Math.Abs(dividerY - (float)num5.Value * num10);
				float low = Math.Abs(dividerY - (float)num6.Value * num10);
				float num13 = (float)num7 * num8;
				RectangleF clientRectangle = new RectangleF(num13 - num8 / 4f, num11, num8 / 2f, num12 - num11);
				seriesBrush = stiCandlestickSeries.ProcessSeriesBrushes(j, seriesBrush);
				StiCandlestickSeriesElementGeom stiCandlestickSeriesElementGeom = new StiCandlestickSeriesElementGeom(geom, stiCandlestickSeries, clientRectangle, num11, num12, high, low, num13, num7 - 1, seriesBrush, borderColor, new TimeSpan(globalBeginTimeElement.Ticks / num * j));
				if (stiCandlestickSeriesElementGeom != null && stiCandlestickSeries.Core.Interaction != null)
				{
					stiCandlestickSeriesElementGeom.Interaction = new StiSeriesInteractionData(geom.Area, stiCandlestickSeries, j);
				}
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiCandlestickSeriesElementGeom);
			}
		}
	}

	public StiCandlestickSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
