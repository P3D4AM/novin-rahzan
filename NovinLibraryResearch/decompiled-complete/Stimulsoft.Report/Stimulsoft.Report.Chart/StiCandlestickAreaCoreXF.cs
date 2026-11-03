using System;
using System.Collections.Generic;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiCandlestickAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Candlestick");

	public override int Position => 110;

	protected override void CreateStripLinesXAxis(IStiAxis axis)
	{
		axis.Info.StripLines.Clear();
		axis.Info.StripLines.Add(0, 0.0);
		List<IStiSeries> series = GetSeries();
		foreach (IStiSeries item in series)
		{
			object[] arguments = item.Arguments;
			foreach (object obj in arguments)
			{
				bool flag = true;
				foreach (StiStripLineXF stripLine in axis.Info.StripLines)
				{
					if (obj != null && stripLine.ValueObject.ToString() == obj.ToString())
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
		foreach (IStiCandlestickSeries item in series)
		{
			double?[] valuesOpen = item.ValuesOpen;
			for (int i = 0; i < valuesOpen.Length; i++)
			{
				double? num = valuesOpen[i];
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
			double?[] valuesClose = item.ValuesClose;
			for (int j = 0; j < valuesClose.Length; j++)
			{
				double? num2 = valuesClose[j];
				if (num2.HasValue)
				{
					specYAxis.Info.Maximum = Math.Max(specYAxis.Info.Maximum, num2.Value);
					specYAxis.Info.Minimum = Math.Min(specYAxis.Info.Minimum, num2.Value);
				}
			}
			double?[] valuesHigh = item.ValuesHigh;
			for (int k = 0; k < valuesHigh.Length; k++)
			{
				double? num3 = valuesHigh[k];
				if (num3.HasValue)
				{
					specYAxis.Info.Maximum = Math.Max(specYAxis.Info.Maximum, num3.Value);
				}
			}
			double?[] valuesLow = item.ValuesLow;
			for (int l = 0; l < valuesLow.Length; l++)
			{
				double? num4 = valuesLow[l];
				if (num4.HasValue)
				{
					specYAxis.Info.Minimum = Math.Min(specYAxis.Info.Minimum, num4.Value);
				}
			}
		}
		specYRightAxis.Info.Maximum = specYAxis.Info.Maximum;
		specYRightAxis.Info.Minimum = specYAxis.Info.Minimum;
	}

	public StiCandlestickAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
