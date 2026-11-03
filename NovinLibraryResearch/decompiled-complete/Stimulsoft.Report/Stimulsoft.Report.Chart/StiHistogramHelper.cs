using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Report.Chart;

public static class StiHistogramHelper
{
	public static void CheckValuesAndArguments(IStiSeries series, StiFormatService formatService = null)
	{
		if (!(series is StiHistogramSeries))
		{
			return;
		}
		double?[] values = series.Values;
		double standardDeviation = GetStandardDeviation(values);
		double d = 3.5 * standardDeviation / Math.Pow(values.Length, 1.0 / 3.0);
		if (double.IsNaN(d))
		{
			d = 1.0;
		}
		d = (double)RoundToSignificantDigits(d, 6);
		double? num = values.Min();
		double? num2 = num - d / 2.0;
		if (num < 0.0 && num2 < 0.0)
		{
			num2 = values.Min();
		}
		else if (num2 < 0.0)
		{
			num2 = 0.0;
		}
		List<double?> list = new List<double?>();
		List<string> list2 = new List<string>();
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		double? num6 = num2 + d;
		StiAxisArea stiAxisArea = series.Chart.Area as StiAxisArea;
		foreach (double? item2 in values.OrderBy((double? v) => v.GetValueOrDefault()))
		{
			if (item2 > num6 || num4 == values.Length - 1)
			{
				list.Add(num3);
				string text = ((formatService != null) ? formatService.Format(num2) : ((StiXAxisCoreXF)stiAxisArea.XAxis.Core).GetLabelText(num2.GetValueOrDefault(), num2.GetValueOrDefault(), series));
				string text2 = ((formatService != null) ? formatService.Format(num6) : ((StiXAxisCoreXF)stiAxisArea.XAxis.Core).GetLabelText(num6.GetValueOrDefault(), num6.GetValueOrDefault(), series));
				string item = ((num5 == 0) ? ("[" + text + ", " + text2 + ")") : ("(" + text + ", " + text2 + "]"));
				list2.Add(item);
				num3 = 0;
				num5++;
				num2 += d * (double)num5;
				num6 = num2 + d;
			}
			num4++;
			num3++;
		}
		series.Values = list.ToArray();
		object[] arguments = list2.ToArray();
		series.Arguments = arguments;
		arguments = list2.ToArray();
		series.OriginalArguments = arguments;
		series.ValuesStart = null;
	}

	private static decimal RoundToSignificantDigits(double d, int digits)
	{
		if (d == 0.0)
		{
			return 0m;
		}
		double num = Math.Pow(10.0, Math.Floor(Math.Log10(Math.Abs(d))) + 1.0);
		return (decimal)(num * Math.Round(d / num, digits));
	}

	private static double GetStandardDeviation(double?[] values)
	{
		double? xAverage = values.Sum() / (double)values.Length;
		double num = values.Sum((double? v) => Math.Pow(v.GetValueOrDefault() - xAverage.GetValueOrDefault(), 2.0));
		return Math.Sqrt(num / (double)(values.Length - 1));
	}
}
