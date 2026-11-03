using System;
using System.Collections.Generic;

namespace Stimulsoft.Report.Chart;

public static class StiWaterfallHelper
{
	public static void CheckTotals(StiChart chart)
	{
		int num = 0;
		foreach (object item in chart.Series)
		{
			if (item is StiWaterfallSeries stiWaterfallSeries)
			{
				num = Math.Max(num, stiWaterfallSeries.Values.Length);
			}
		}
		for (int i = 0; i < chart.Series.Count; i++)
		{
			if (!(chart.Series[i] is StiWaterfallSeries stiWaterfallSeries2))
			{
				continue;
			}
			List<object> list = new List<object>();
			List<double?> list2 = new List<double?>();
			double? num2 = 0.0;
			for (int j = 0; j < num; j++)
			{
				if (i == 0)
				{
					if (j < stiWaterfallSeries2.Arguments.Length)
					{
						list.Add(stiWaterfallSeries2.Arguments[j]);
					}
					else
					{
						list.Add(j);
					}
				}
				if (stiWaterfallSeries2.Values.Length > j)
				{
					list2.Add(stiWaterfallSeries2.Values[j]);
					num2 += stiWaterfallSeries2.Values[j];
				}
				else
				{
					list2.Add(null);
				}
			}
			if (stiWaterfallSeries2.Total.Visible)
			{
				list2.Add(num2);
			}
			if (i == 0)
			{
				list.Add(stiWaterfallSeries2.Total.Text);
			}
			stiWaterfallSeries2.Values = list2.ToArray();
			stiWaterfallSeries2.Arguments = list.ToArray();
			stiWaterfallSeries2.ValuesStart = null;
		}
	}
}
