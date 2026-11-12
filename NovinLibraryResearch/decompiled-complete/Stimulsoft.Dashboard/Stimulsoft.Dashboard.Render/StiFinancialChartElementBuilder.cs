using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Render;

internal class StiFinancialChartElementBuilder : StiChartElementBuilder
{
	protected override void RenderElements(StiChartElement element, StiChart chart, StiDataTable dataTable)
	{
		StiReport report = chart?.Report;
		int seriesMeterIndex = GetSeriesMeterIndex(dataTable);
		IEnumerable<string> seriesKeys = GetSeriesKeys(dataTable, seriesMeterIndex, GetSeriesKeysLimitCount(element));
		IEnumerable<StiMeter> valueMeters = GetValueMeters(dataTable);
		int num = seriesKeys.Count();
		foreach (string item in seriesKeys)
		{
			int num2 = 0;
			foreach (StiMeter item2 in valueMeters)
			{
				int num3 = GetValueMeterIndexes(dataTable)[num2];
				StiSeries stiSeries = RenderSeries(element, item2, item, chart);
				IStiFinancialSeries stiFinancialSeries = stiSeries as IStiFinancialSeries;
				if (stiSeries == null || stiFinancialSeries == null)
				{
					continue;
				}
				IEnumerable<object[]> enumerable;
				if (seriesMeterIndex == -1 || num <= 1)
				{
					IEnumerable<object[]> rows = dataTable.Rows;
					enumerable = rows;
				}
				else
				{
					enumerable = dataTable.Rows.WhereArrayItemStringEqualsTo(seriesMeterIndex, item);
				}
				IEnumerable<object[]> source = enumerable;
				stiFinancialSeries.ValuesOpen = source.GetArrayItem(num3).TryCastToNullableDouble().ToArray();
				stiFinancialSeries.ValuesClose = null;
				int closeValueIndex = GetCloseValueIndex(dataTable, num3);
				if (closeValueIndex != -1)
				{
					stiFinancialSeries.ValuesClose = source.GetArrayItem(closeValueIndex).TryCastToNullableDouble().ToArray();
				}
				if (stiFinancialSeries.ValuesClose == null)
				{
					stiFinancialSeries.ValuesClose = Enumerable.Repeat((double?)10.0, stiSeries.Arguments.Length).ToArray();
				}
				stiFinancialSeries.ValuesLow = null;
				int lowValueIndex = GetLowValueIndex(dataTable, num3);
				if (lowValueIndex != -1)
				{
					stiFinancialSeries.ValuesLow = source.GetArrayItem(lowValueIndex).TryCastToNullableDouble().ToArray();
				}
				if (stiFinancialSeries.ValuesLow == null)
				{
					stiFinancialSeries.ValuesLow = Enumerable.Repeat((double?)10.0, stiSeries.Arguments.Length).ToArray();
				}
				stiFinancialSeries.ValuesHigh = null;
				int highValueIndex = GetHighValueIndex(dataTable, num3);
				if (highValueIndex != -1)
				{
					stiFinancialSeries.ValuesHigh = source.GetArrayItem(highValueIndex).TryCastToNullableDouble().ToArray();
				}
				if (stiFinancialSeries.ValuesHigh == null)
				{
					stiFinancialSeries.ValuesHigh = Enumerable.Repeat((double?)10.0, stiSeries.Arguments.Length).ToArray();
				}
				int argumentIndex = GetArgumentIndex(dataTable, num3);
				if (argumentIndex != -1)
				{
					object[] arguments = (from argument in source.GetArrayItem(argumentIndex)
						select GetString(element.ArgumentFormat, argument, report)).ToArray();
					stiSeries.Arguments = arguments;
				}
				num2++;
				int count = Math.Min(stiSeries.Values.Length, stiSeries.Arguments.Length);
				RenderSeriesInteraction(stiSeries, element, count, item);
			}
		}
	}

	protected override StiSeries RenderSeries(StiChartElement element, StiMeter value, string groupKey, StiChart chart)
	{
		StiSeries stiSeries = StiChartSeriesCreator.New(((StiOpenValueChartMeter)value).SeriesType);
		stiSeries.CoreTitle = GetSeriesTitle(element, groupKey, value);
		stiSeries.IsDashboard = true;
		chart.Series.Add(stiSeries);
		return stiSeries;
	}

	protected override void RenderSeriesInteraction(StiSeries series, StiChartElement element, int count, string seriesKey)
	{
		if (!(series is IStiFinancialSeries stiFinancialSeries))
		{
			return;
		}
		StiChartDashboardInteraction stiChartDashboardInteraction = element.DashboardInteraction as StiChartDashboardInteraction;
		if (stiChartDashboardInteraction.OnHover == StiInteractionOnHover.ShowToolTip)
		{
			series.ToolTips = new string[count];
			for (int i = 0; i < count; i++)
			{
				string text = string.Empty;
				string text2 = string.Empty;
				string text3 = string.Empty;
				string text4 = string.Empty;
				string text5 = string.Empty;
				if (i < stiFinancialSeries.ValuesOpen.Length && stiFinancialSeries.ValuesOpen[i].HasValue)
				{
					text = stiFinancialSeries.ValuesOpen[i].ToString();
				}
				if (i < stiFinancialSeries.ValuesClose.Length)
				{
					text2 = stiFinancialSeries.ValuesClose[i].ToString();
				}
				if (i < stiFinancialSeries.ValuesLow.Length)
				{
					text3 = stiFinancialSeries.ValuesLow[i].ToString();
				}
				if (i < stiFinancialSeries.ValuesHigh.Length)
				{
					text4 = stiFinancialSeries.ValuesHigh[i].ToString();
				}
				if (i < series.Arguments.Length && series.Arguments[i] != null)
				{
					text5 = $"{series.Arguments[i]}";
				}
				string format = "{{\"StiColor\":\"{0}\",\"StiOpenValue\":\"{1}\",\"StiCloseValue\":\"{2}\",\"StiLowValue\":\"{3}\",\"StiHighValue\":\"{4}\",\"StiArgument\":\"{5}\"}}";
				series.ToolTips[i] = string.Format(format, "#ffffff", text, text2, text3, text4, text5);
			}
		}
		series.DrillDownEnabled = true;
	}

	protected override void RenderArea(StiChartElement element, StiChart chart)
	{
		base.RenderArea(element, chart);
		double? num = null;
		double? num2 = null;
		foreach (object item9 in chart.Series)
		{
			if (item9 is IStiFinancialSeries stiFinancialSeries)
			{
				double? item = stiFinancialSeries.ValuesOpen.Min();
				double? item2 = stiFinancialSeries.ValuesClose.Min();
				double? item3 = stiFinancialSeries.ValuesLow.Min();
				double? item4 = stiFinancialSeries.ValuesHigh.Min();
				double? item5 = stiFinancialSeries.ValuesOpen.Max();
				double? item6 = stiFinancialSeries.ValuesClose.Max();
				double? item7 = stiFinancialSeries.ValuesLow.Max();
				double? item8 = stiFinancialSeries.ValuesHigh.Max();
				double? num3 = new List<double?> { item, item2, item3, item4 }.Min();
				double? num4 = new List<double?> { item5, item6, item7, item8 }.Max();
				if (!num.HasValue)
				{
					num = num3.GetValueOrDefault();
				}
				if (!num2.HasValue)
				{
					num2 = num4.GetValueOrDefault();
				}
				num = Math.Min(num.GetValueOrDefault(), num3.GetValueOrDefault());
				num2 = Math.Max(num2.GetValueOrDefault(), num4.GetValueOrDefault());
			}
		}
		IStiAxisArea stiAxisArea = (IStiAxisArea)chart.Area;
		stiAxisArea.YAxis.Range.Auto = false;
		stiAxisArea.YAxis.Range.Minimum = num.GetValueOrDefault();
		stiAxisArea.YAxis.Range.Maximum = num2.GetValueOrDefault();
		double num5 = stiAxisArea.YAxis.Range.Maximum - stiAxisArea.YAxis.Range.Minimum;
		stiAxisArea.YAxis.Labels.Step = ((num5 > 2.0) ? ((float)Math.Round(num5 / 10.0)) : ((float)Math.Round(num5 / 10.0, 4)));
	}

	protected override IEnumerable<StiMeter> GetValueMeters(StiDataTable table)
	{
		return table?.Meters.Where((IStiMeter m) => m is StiOpenValueChartMeter).Cast<StiOpenValueChartMeter>();
	}

	protected int GetCloseValueIndex(StiDataTable table, int index)
	{
		if (table == null)
		{
			return -1;
		}
		int[] closeMeterIndexes = GetCloseMeterIndexes(table);
		if (closeMeterIndexes == null || !closeMeterIndexes.Any())
		{
			return -1;
		}
		if (closeMeterIndexes.Length == 1)
		{
			return closeMeterIndexes.FirstOrDefault();
		}
		if (closeMeterIndexes.Length > index)
		{
			return (int)closeMeterIndexes.GetValue(index);
		}
		return -1;
	}

	protected int[] GetCloseMeterIndexes(StiDataTable table)
	{
		if (table == null)
		{
			return null;
		}
		return (from r in GetCloseMeters(table)
			select table.Meters.IndexOf(r)).ToArray();
	}

	protected IEnumerable<StiCloseValueChartMeter> GetCloseMeters(StiDataTable table)
	{
		return table?.Meters.Where((IStiMeter m) => m is StiCloseValueChartMeter).Cast<StiCloseValueChartMeter>();
	}

	protected int GetLowValueIndex(StiDataTable table, int index)
	{
		if (table == null)
		{
			return -1;
		}
		int[] lowMeterIndexes = GetLowMeterIndexes(table);
		if (lowMeterIndexes == null || !lowMeterIndexes.Any())
		{
			return -1;
		}
		if (lowMeterIndexes.Length == 1)
		{
			return lowMeterIndexes.FirstOrDefault();
		}
		if (lowMeterIndexes.Length > index)
		{
			return (int)lowMeterIndexes.GetValue(index);
		}
		return -1;
	}

	protected int[] GetLowMeterIndexes(StiDataTable table)
	{
		if (table == null)
		{
			return null;
		}
		return (from r in GetLowMeters(table)
			select table.Meters.IndexOf(r)).ToArray();
	}

	protected IEnumerable<StiLowValueChartMeter> GetLowMeters(StiDataTable table)
	{
		return table?.Meters.Where((IStiMeter m) => m is StiLowValueChartMeter).Cast<StiLowValueChartMeter>();
	}

	protected int GetHighValueIndex(StiDataTable table, int index)
	{
		if (table == null)
		{
			return -1;
		}
		int[] highMeterIndexes = GetHighMeterIndexes(table);
		if (highMeterIndexes == null)
		{
			return -1;
		}
		if (!highMeterIndexes.Any())
		{
			return -1;
		}
		if (highMeterIndexes.Length == 1)
		{
			return highMeterIndexes.FirstOrDefault();
		}
		if (highMeterIndexes.Length > index)
		{
			return (int)highMeterIndexes.GetValue(index);
		}
		return -1;
	}

	protected int[] GetHighMeterIndexes(StiDataTable table)
	{
		if (table == null)
		{
			return null;
		}
		return (from r in GetHighMeters(table)
			select table.Meters.IndexOf(r)).ToArray();
	}

	protected IEnumerable<StiHighValueChartMeter> GetHighMeters(StiDataTable table)
	{
		return table?.Meters.Where((IStiMeter m) => m is StiHighValueChartMeter).Cast<StiHighValueChartMeter>();
	}
}
