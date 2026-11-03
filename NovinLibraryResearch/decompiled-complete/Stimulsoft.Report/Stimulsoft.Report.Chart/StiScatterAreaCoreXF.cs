using System;
using System.Collections.Generic;
using System.Globalization;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiScatterAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	private bool isArgumentDateTime;

	public override string LocalizedName => StiLocalization.Get("Chart", "Scatter");

	public override int Position => 80;

	private bool IsXAxisAutoRange(IStiAxis xAxis)
	{
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		if (!xAxis.Range.Auto && stiAxisArea.XAxis.Range.Maximum != stiAxisArea.XAxis.Range.Minimum)
		{
			return xAxis.LogarithmicScale;
		}
		return true;
	}

	protected override void PrepareRange(IStiAxis specXAxis, IStiAxis specXTopAxis, IStiAxis specYAxis, IStiAxis specYRightAxis)
	{
		base.PrepareRange(specXAxis, specXTopAxis, specYAxis, specYRightAxis);
		bool flag = true;
		specXAxis.Info.Maximum = 0.0;
		specXAxis.Info.Minimum = 0.0;
		isArgumentDateTime = false;
		List<IStiSeries> series = GetSeries();
		foreach (IStiScatterSeries item in series)
		{
			if (item.Arguments.Length != 0 && item.Arguments[0] is string)
			{
				string text = item.Arguments[0] as string;
				if (text.EndsWith("AM") || text.EndsWith("PM"))
				{
					object[] array = new object[item.Arguments.Length];
					CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-Us");
					for (int i = 0; i < item.Arguments.Length; i++)
					{
						if (DateTime.TryParse(item.Arguments[i] as string, cultureInfo, DateTimeStyles.None, out var result))
						{
							array[i] = result;
						}
						else
						{
							array[i] = item.Arguments[i];
						}
					}
					item.Arguments = array;
				}
			}
			object[] arguments = item.Arguments;
			foreach (object obj in arguments)
			{
				double num = 0.0;
				if (obj is DateTime)
				{
					isArgumentDateTime = true;
					num = ((DateTime)obj).ToOADate();
				}
				else if (obj is string)
				{
					if (StiSeries.TryParseValue((string)obj, ((StiChart)item.Chart).Report.Culture, out var result2))
					{
						num = result2;
					}
				}
				else
				{
					try
					{
						num = (double)StiConvert.ChangeType(obj, typeof(double));
					}
					catch
					{
						num = 0.0;
					}
				}
				if (flag)
				{
					flag = false;
					specXAxis.Info.Maximum = num;
					specXAxis.Info.Minimum = num;
					continue;
				}
				specXAxis.Info.Maximum = Math.Max(specXAxis.Info.Maximum, num);
				if (num != 0.0 || !specXAxis.LogarithmicScale)
				{
					specXAxis.Info.Minimum = Math.Min(specXAxis.Info.Minimum, num);
				}
			}
		}
	}

	protected override void CheckStripLinesAndMaximumMinimumXAxis(IStiAxis axis)
	{
		if (axis.Info.StripLines.Count > 0)
		{
			axis.Info.Minimum = axis.Info.StripLines[0].Value;
			axis.Info.Maximum = axis.Info.StripLines[axis.Info.StripLines.Count - 1].Value;
		}
		else
		{
			axis.Info.Minimum = 0.0;
			axis.Info.Maximum = 1.0;
		}
	}

	protected override void CreateStripLinesXAxis(IStiAxis axis)
	{
		if (!isArgumentDateTime && !axis.LogarithmicScale && IsXAxisAutoRange(axis))
		{
			double num = Math.Abs(axis.Info.Maximum - axis.Info.Minimum) * 0.05;
			if (num != 0.0)
			{
				axis.Info.Maximum += num;
				axis.Info.Minimum -= num;
				if (axis.Info.Minimum < 0.0 && axis.Info.Minimum + num >= 0.0)
				{
					axis.Info.Minimum = 0.0;
				}
			}
			else
			{
				axis.Info.Maximum *= 1.05;
				if (axis.Info.Minimum < 0.0)
				{
					axis.Info.Minimum *= 1.05;
				}
				else
				{
					axis.Info.Minimum *= 0.95;
				}
			}
			if (axis.Core.GetStartFromZero())
			{
				axis.Info.Minimum = 0.0;
			}
			if (axis.Info.Minimum == axis.Info.Maximum)
			{
				if (axis.Info.Maximum == 0.0)
				{
					axis.Info.Maximum = 100.0;
				}
				else
				{
					axis.Info.Minimum -= axis.Info.Minimum * 0.1;
					axis.Info.Maximum -= axis.Info.Maximum * 0.1;
				}
			}
		}
		if (isArgumentDateTime && IsXAxisAutoRange(axis))
		{
			double num2 = Math.Abs(axis.Info.Maximum - axis.Info.Minimum);
			if (num2 == 0.0)
			{
				axis.Info.Maximum += 1.0;
				axis.Info.Minimum -= 1.0;
			}
		}
		bool asDateTimeValue = false;
		List<IStiSeries> series = GetSeries();
		for (int i = 0; i < series.Count; i++)
		{
			if (series[i].Core.IsDateTimeValues)
			{
				asDateTimeValue = true;
				break;
			}
		}
		double num3 = axis.Labels.Step;
		if (num3 == 0.0)
		{
			num3 = StiStripLineCalculatorXF.GetInterval(axis.Info.Minimum, axis.Info.Maximum, 6);
		}
		StiStripLinesXF stiStripLinesXF = ((!axis.LogarithmicScale) ? StiStripLineCalculatorXF.GetStripLines(axis.Info.Minimum, axis.Info.Maximum, num3, asDateTimeValue, defaultMinValue: true) : StiStripLineCalculatorXF.GetStripLinesLogScale(axis.Info.Minimum, axis.Info.Maximum));
		axis.Info.StripLines.Clear();
		for (int num4 = stiStripLinesXF.Count - 1; num4 >= 0; num4--)
		{
			double value = stiStripLinesXF[num4].Value;
			object valueObject = stiStripLinesXF[num4].ValueObject;
			if (isArgumentDateTime)
			{
				valueObject = DateTime.FromOADate(Convert.ToDouble(value));
			}
			axis.Info.StripLines.Add(valueObject, value);
		}
	}

	protected override void CreateStripLinesYAxis(IStiAxis axis, bool isDateTimeValues)
	{
		if (base.Area.IsDefaultSeriesTypeFullStackedColumnSeries || base.Area.IsDefaultSeriesTypeFullStackedBarSeries)
		{
			bool flag = false;
			bool flag2 = false;
			List<IStiSeries> series = GetSeries();
			foreach (IStiSeries item in series)
			{
				double?[] values = item.Values;
				foreach (double? num in values)
				{
					if (num > 0.0)
					{
						flag = true;
					}
					if (num < 0.0)
					{
						flag2 = true;
					}
				}
			}
			double minValue = (flag2 ? (-100) : 0);
			double maxValue = (flag ? 100 : 0);
			double num2 = axis.Labels.Step;
			if (num2 == 0.0)
			{
				num2 = StiStripLineCalculatorXF.GetInterval(minValue, maxValue, 6);
			}
			axis.Info.StripLines = StiStripLineCalculatorXF.GetStripLines(minValue, maxValue, num2, asDateTimeValue: false);
			{
				foreach (StiStripLineXF stripLine in axis.Info.StripLines)
				{
					stripLine.ValueObject = $"{stripLine.ValueObject}%";
				}
				return;
			}
		}
		float num3 = axis.Labels.Step;
		if (num3 > 0f && axis.Info.Range > 0.0 && axis.Info.Range / (double)num3 > 500.0)
		{
			num3 = 0f;
		}
		if (num3 == 0f)
		{
			num3 = (float)StiStripLineCalculatorXF.GetInterval(axis.Info.Minimum, axis.Info.Maximum, 6);
		}
		if (axis.LogarithmicScale)
		{
			axis.Info.StripLines = StiStripLineCalculatorXF.GetStripLinesLogScale(axis.Info.Minimum, axis.Info.Maximum);
		}
		else
		{
			axis.Info.StripLines = StiStripLineCalculatorXF.GetStripLines(axis.Info.Minimum, axis.Info.Maximum, (double)Convert.ToDecimal(num3), isDateTimeValues);
		}
	}

	public StiScatterAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
