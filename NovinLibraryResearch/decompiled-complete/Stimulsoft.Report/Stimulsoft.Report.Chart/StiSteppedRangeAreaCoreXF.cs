using System;
using System.Collections.Generic;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSteppedRangeAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "SteppedRange");

	public override int Position => 91;

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
			IStiSteppedRangeSeries stiSteppedRangeSeries = (IStiSteppedRangeSeries)series[i];
			double?[] values = stiSteppedRangeSeries.Values;
			double?[] valuesEnd = stiSteppedRangeSeries.ValuesEnd;
			int num = Math.Min(values.Length, valuesEnd.Length);
			for (int j = 0; j < num; j++)
			{
				double val = (values[j].HasValue ? values[j].Value : 0.0);
				double val2 = (valuesEnd[j].HasValue ? valuesEnd[j].Value : 0.0);
				double? num2 = Math.Max(val, val2);
				double? num3 = Math.Min(val, val2);
				if (stiSteppedRangeSeries.YAxis == StiSeriesYAxis.LeftYAxis)
				{
					if (flag)
					{
						flag3 = true;
						specYAxis.Info.Maximum = num2.Value;
						specYAxis.Info.Minimum = num3.Value;
						flag = false;
					}
					else
					{
						flag3 = true;
						specYAxis.Info.Maximum = Math.Max(num2.Value, specYAxis.Info.Maximum);
						specYAxis.Info.Minimum = Math.Min(num3.Value, specYAxis.Info.Minimum);
					}
				}
				else if (flag2)
				{
					flag4 = true;
					specYRightAxis.Info.Maximum = num2.Value;
					specYRightAxis.Info.Minimum = num3.Value;
					flag2 = false;
				}
				else
				{
					flag4 = true;
					specYRightAxis.Info.Maximum = Math.Max(num2.Value, specYRightAxis.Info.Maximum);
					specYRightAxis.Info.Minimum = Math.Min(num3.Value, specYRightAxis.Info.Minimum);
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

	public StiSteppedRangeAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
