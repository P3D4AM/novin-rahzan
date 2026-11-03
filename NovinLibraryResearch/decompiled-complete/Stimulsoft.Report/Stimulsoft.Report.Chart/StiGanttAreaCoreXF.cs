using System;
using System.Collections.Generic;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiGanttAreaCoreXF : StiClusteredBarAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Gantt");

	public override StiChartSeriesOrientation SeriesOrientation => StiChartSeriesOrientation.Horizontal;

	public override int Position => 70;

	protected override void CreateStripLinesXAxis(IStiAxis axis)
	{
		axis.Info.StripLines.Clear();
		axis.Info.StripLines.Add(null, 0.0);
		List<IStiSeries> series = GetSeries();
		foreach (IStiSeries item in series)
		{
			object[] arguments = item.Arguments;
			foreach (object obj in arguments)
			{
				bool flag = true;
				foreach (StiStripLineXF stripLine in axis.Info.StripLines)
				{
					if (stripLine.ValueObject != null && stripLine.ValueObject.ToString() == obj.ToString())
					{
						flag = false;
					}
				}
				if (flag)
				{
					axis.Info.StripLines.Add(obj, axis.Info.StripLines.Count);
				}
			}
		}
		axis.Info.StripLines.Add(string.Empty, axis.Info.StripLines.Count);
	}

	protected override void PrepareRange(IStiAxis specXAxis, IStiAxis specXTopAxis, IStiAxis specYAxis, IStiAxis specYRightAxis)
	{
		specXAxis.Info.Maximum = 0.0;
		specXAxis.Info.Minimum = 0.0;
		specYAxis.Info.Maximum = 0.0;
		specYAxis.Info.Minimum = 0.0;
		List<IStiSeries> series = GetSeries();
		bool flag = true;
		foreach (IStiGanttSeries item in series)
		{
			double?[] values = item.Values;
			for (int i = 0; i < values.Length; i++)
			{
				double? num = values[i];
				if (num.HasValue)
				{
					if (flag)
					{
						flag = false;
						specYAxis.Info.Maximum = num.Value;
						specYAxis.Info.Minimum = num.Value;
					}
					else
					{
						specYAxis.Info.Maximum = Math.Max(specYAxis.Info.Maximum, num.Value);
						specYAxis.Info.Minimum = Math.Min(specYAxis.Info.Minimum, num.Value);
					}
				}
			}
			double?[] valuesEnd = item.ValuesEnd;
			for (int j = 0; j < valuesEnd.Length; j++)
			{
				double? num2 = valuesEnd[j];
				if (num2.HasValue)
				{
					specYAxis.Info.Maximum = Math.Max(specYAxis.Info.Maximum, num2.Value);
					specYAxis.Info.Minimum = Math.Min(specYAxis.Info.Minimum, num2.Value);
				}
			}
		}
		specYRightAxis.Info.Maximum = specYAxis.Info.Maximum;
		specYRightAxis.Info.Minimum = specYAxis.Info.Minimum;
	}

	public StiGanttAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
