using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStockSeriesCoreXF : StiSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Stock");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		if (base.Series.AllowApplyStyle)
		{
			((IStiStockSeries)base.Series).LineColor = color;
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		if (series == null || series.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		IStiStockArea stiStockArea = geom.Area as IStiStockArea;
		float dividerY = stiStockArea.AxisCore.GetDividerY();
		TimeSpan globalBeginTimeElement = StiChartHelper.GlobalBeginTimeElement;
		for (int i = 0; i < series.Length; i++)
		{
			IStiStockSeries stiStockSeries = (IStiStockSeries)series[i];
			double?[] valuesOpen = stiStockSeries.ValuesOpen;
			double?[] valuesClose = stiStockSeries.ValuesClose;
			double?[] valuesHigh = stiStockSeries.ValuesHigh;
			double?[] valuesLow = stiStockSeries.ValuesLow;
			int num = Math.Min(Math.Min(valuesOpen.Length, valuesClose.Length), Math.Min(valuesHigh.Length, valuesLow.Length));
			int val = stiStockSeries.Arguments.Length;
			int num2 = Math.Min(num, val);
			for (int j = 0; j < num2; j++)
			{
				double? num3 = valuesOpen[j];
				double? num4 = valuesClose[j];
				double? num5 = valuesHigh[j];
				double? num6 = valuesLow[j];
				int num7 = 0;
				foreach (StiStripLineXF stripLine in stiStockArea.XAxis.Info.StripLines)
				{
					if (stiStockSeries.Arguments[j].ToString() == stripLine.ValueObject?.ToString())
					{
						break;
					}
					num7++;
				}
				float num8 = (float)stiStockArea.XAxis.Info.Dpi;
				Color seriesColor = stiStockSeries.LineColor;
				if (!stiStockSeries.AllowApplyColorNegative && num3 < num4)
				{
					seriesColor = stiStockSeries.LineColorNegative;
				}
				if (num3.HasValue && num4.HasValue && num5.HasValue && num6.HasValue)
				{
					float num9 = (float)stiStockArea.YAxis.Info.Dpi;
					float num10 = Math.Abs(dividerY - (float)num3.Value * num9);
					float num11 = Math.Abs(dividerY - (float)num4.Value * num9);
					float num12 = Math.Abs(dividerY - (float)num5.Value * num9);
					float num13 = Math.Abs(dividerY - (float)num6.Value * num9);
					float num14 = (float)num7 * num8;
					if (stiStockArea.ReverseHor)
					{
						float num15 = num10;
						num10 = num11;
						num11 = num15;
					}
					RectangleF clientRectangle = new RectangleF(num14 - num8 / 3f, Math.Min(num12, num13), num8 * 2f / 3f, Math.Abs(num13 - num12));
					seriesColor = stiStockSeries.ProcessSeriesColors(j, seriesColor);
					StiStockSeriesElementGeom stiStockSeriesElementGeom = new StiStockSeriesElementGeom(geom, stiStockSeries, clientRectangle, num10, num11, num12, num13, num14, num7 - 1, seriesColor, new TimeSpan(globalBeginTimeElement.Ticks / num * j));
					if (stiStockSeriesElementGeom != null && stiStockSeries.Core.Interaction != null)
					{
						stiStockSeriesElementGeom.Interaction = new StiSeriesInteractionData(geom.Area, stiStockSeries, j);
					}
					geom.CreateChildGeoms();
					geom.ChildGeoms.Add(stiStockSeriesElementGeom);
				}
			}
		}
	}

	public StiStockSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
