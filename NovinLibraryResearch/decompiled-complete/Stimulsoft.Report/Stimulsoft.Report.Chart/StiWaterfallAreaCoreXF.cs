using System;
using System.Collections.Generic;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiWaterfallAreaCoreXF : StiAxisAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Waterfall");

	public override int Position => 4;

	protected override void PrepareRange(IStiAxis specXAxis, IStiAxis specXTopAxis, IStiAxis specYAxis, IStiAxis specYRightAxis)
	{
		bool flag = true;
		double num = 0.0;
		specYAxis.Info.Maximum = 0.0;
		specYAxis.Info.Minimum = 0.0;
		List<IStiSeries> series = GetSeries();
		for (int i = 0; i < series.Count; i++)
		{
			StiWaterfallSeries stiWaterfallSeries = series[i] as StiWaterfallSeries;
			int num2 = (stiWaterfallSeries.Total.Visible ? 1 : 0);
			for (int j = 0; j < stiWaterfallSeries.Values.Length - num2; j++)
			{
				double? num3 = stiWaterfallSeries.Values[j];
				if (stiWaterfallSeries.YAxis == StiSeriesYAxis.LeftYAxis && num3.HasValue)
				{
					if (flag)
					{
						num = num3.Value;
						specYAxis.Info.Maximum = Math.Max(0.0, num3.Value);
						specYAxis.Info.Minimum = Math.Min(0.0, num3.Value);
						flag = false;
					}
					else
					{
						num += num3.Value;
						specYAxis.Info.Maximum = Math.Max(num, specYAxis.Info.Maximum);
						specYAxis.Info.Minimum = Math.Min(num, specYAxis.Info.Minimum);
					}
				}
			}
		}
	}

	public StiWaterfallAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
