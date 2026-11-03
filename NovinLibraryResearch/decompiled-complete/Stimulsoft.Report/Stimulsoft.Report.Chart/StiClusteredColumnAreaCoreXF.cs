using System;
using System.Collections.Generic;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiClusteredColumnAreaCoreXF : StiAxisAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "ClusteredColumn");

	public override int Position => 0;

	protected override void PrepareRange(IStiAxis specXAxis, IStiAxis specXTopAxis, IStiAxis specYAxis, IStiAxis specYRightAxis)
	{
		bool flag = true;
		bool flag2 = true;
		bool flag3 = false;
		bool flag4 = false;
		specYAxis.Info.Maximum = 0.0;
		specYAxis.Info.Minimum = 0.0;
		specYRightAxis.Info.Maximum = 0.0;
		specYRightAxis.Info.Minimum = 0.0;
		List<IStiSeries> series = GetSeries();
		for (int i = 0; i < series.Count; i++)
		{
			IStiSeries stiSeries = series[i];
			double?[] values = stiSeries.Values;
			double?[] array = values;
			for (int j = 0; j < array.Length; j++)
			{
				double? num = array[j];
				if (stiSeries.YAxis == StiSeriesYAxis.LeftYAxis)
				{
					if (num.HasValue)
					{
						if (flag)
						{
							flag3 = true;
							specYAxis.Info.Maximum = num.Value;
							specYAxis.Info.Minimum = num.Value;
							flag = false;
						}
						else
						{
							flag3 = true;
							specYAxis.Info.Maximum = Math.Max(num.Value, specYAxis.Info.Maximum);
							specYAxis.Info.Minimum = Math.Min(num.Value, specYAxis.Info.Minimum);
						}
					}
				}
				else if (num.HasValue)
				{
					if (flag2)
					{
						flag4 = true;
						specYRightAxis.Info.Maximum = num.Value;
						specYRightAxis.Info.Minimum = num.Value;
						flag2 = false;
					}
					else
					{
						flag4 = true;
						specYRightAxis.Info.Maximum = Math.Max(num.Value, specYRightAxis.Info.Maximum);
						specYRightAxis.Info.Minimum = Math.Min(num.Value, specYRightAxis.Info.Minimum);
					}
				}
			}
		}
		if (!flag3)
		{
			specYAxis.Info.Maximum = specYRightAxis.Info.Maximum;
			specYAxis.Info.Minimum = specYRightAxis.Info.Minimum;
		}
		if (!flag4)
		{
			specYRightAxis.Info.Maximum = specYAxis.Info.Maximum;
			specYRightAxis.Info.Minimum = specYAxis.Info.Minimum;
		}
	}

	public StiClusteredColumnAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
