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

internal class StiRangeChartElementBuilder : StiChartElementBuilder
{
	protected override void RenderElements(StiChartElement element, StiChart chart, StiDataTable dataTable)
	{
		StiReport report = chart?.Report;
		int seriesMeterIndex = GetSeriesMeterIndex(dataTable);
		IEnumerable<string> seriesKeys = GetSeriesKeys(dataTable, seriesMeterIndex, GetSeriesKeysLimitCount(element));
		int num = seriesKeys.Count();
		IEnumerable<StiMeter> valueMeters = GetValueMeters(dataTable);
		int[] argumentMeterIndexes = GetArgumentMeterIndexes(dataTable);
		List<object> list = GetArgumentKeys(element, dataTable).ToList();
		foreach (string item in seriesKeys)
		{
			int num2 = 0;
			foreach (StiMeter item2 in valueMeters)
			{
				int num3 = GetValueMeterIndexes(dataTable)[num2];
				StiSeries stiSeries = RenderSeries(element, item2, item, chart);
				if (stiSeries == null)
				{
					continue;
				}
				StiChartElementBuilder.RenderConditions(item, stiSeries, element, item2);
				IEnumerable<object[]> enumerable;
				if (seriesMeterIndex == -1 || num <= 1)
				{
					IEnumerable<object[]> rows = dataTable.Rows;
					enumerable = rows;
				}
				else
				{
					enumerable = dataTable.Rows.WhereArrayItemStringEqualsTo(seriesMeterIndex, ToString(item));
				}
				IEnumerable<object[]> enumerable2 = enumerable;
				stiSeries.Values = new double?[list.Count];
				int num4 = 0;
				foreach (object item3 in list)
				{
					IEnumerable<object[]> detailRows = StiChartElementBuilder.GetDetailRows(enumerable2, item3, argumentMeterIndexes);
					if (num4 < stiSeries.Values.Length)
					{
						stiSeries.Values[num4] = detailRows.GetArrayItem(num3).FirstOrDefaultAsNullableDouble();
					}
					num4++;
				}
				stiSeries.Core.IsDateTimeValues = IEnumerableExt.GetValueOrFirstOrDefault(enumerable2.GetArrayItem(num3).ToList().FirstOrDefault()) is DateTime;
				if (stiSeries is IStiRangeSeries stiRangeSeries)
				{
					stiRangeSeries.ValuesEnd = new double?[list.Count];
					int endValueIndex = GetEndValueIndex(dataTable, num3);
					if (stiRangeSeries.ValuesEnd == null)
					{
						stiRangeSeries.ValuesEnd = Enumerable.Repeat((double?)10.0, list.Count).ToArray();
					}
					if (endValueIndex != -1)
					{
						num4 = 0;
						foreach (object item4 in list)
						{
							IEnumerable<object[]> detailRows2 = StiChartElementBuilder.GetDetailRows(enumerable2, item4, argumentMeterIndexes);
							if (num4 < stiRangeSeries.ValuesEnd.Length)
							{
								stiRangeSeries.ValuesEnd[num4] = detailRows2.GetArrayItem(endValueIndex).FirstOrDefaultAsNullableDouble();
							}
							num4++;
						}
					}
				}
				stiSeries.OriginalArguments = list.ToArray();
				object[] arguments = list.Select((object argument) => GetString(element.ArgumentFormat, argument, report)).ToArray();
				stiSeries.Arguments = arguments;
				num2++;
				int count = Math.Min(stiSeries.Values.Length, stiSeries.Arguments.Length);
				RenderSeriesInteraction(stiSeries, element, count, item);
			}
		}
	}

	protected override StiSeries RenderSeries(StiChartElement element, StiMeter value, string groupKey, StiChart chart)
	{
		StiSeries stiSeries = StiChartSeriesCreator.New(((StiStartValueChartMeter)value).SeriesType);
		stiSeries.CoreTitle = GetSeriesTitle(element, groupKey, value);
		stiSeries.IsDashboard = true;
		chart.Series.Add(stiSeries);
		return stiSeries;
	}

	protected override void RenderSeriesInteraction(StiSeries series, StiChartElement element, int count, string seriesKey)
	{
		StiReport report = element?.Report;
		if (!(series is IStiRangeSeries stiRangeSeries))
		{
			return;
		}
		StiChartDashboardInteraction stiChartDashboardInteraction = element.DashboardInteraction as StiChartDashboardInteraction;
		if (stiChartDashboardInteraction.OnHover == StiInteractionOnHover.ShowToolTip)
		{
			series.ToolTips = new string[count];
			for (int i = 0; i < count; i++)
			{
				string value = string.Empty;
				string value2 = string.Empty;
				string value3 = string.Empty;
				if (i < series.Values.Length && series.Values[i].HasValue)
				{
					value = (series.Core.IsDateTimeValues ? DateTime.FromOADate(series.Values[i].GetValueOrDefault()).ToString() : series.Values[i].ToString());
				}
				if (stiRangeSeries.ValuesEnd != null && i < stiRangeSeries.ValuesEnd.Length)
				{
					value2 = (series.Core.IsDateTimeValues ? DateTime.FromOADate(stiRangeSeries.ValuesEnd[i].GetValueOrDefault()).ToString() : stiRangeSeries.ValuesEnd[i].ToString());
				}
				if (i < series.Arguments.Length && series.Arguments[i] != null)
				{
					value3 = $"{series.Arguments[i]}";
				}
				series.ToolTips[i] = GetToolTip(element, (element.DashboardInteraction as StiChartDashboardInteraction).ToolTip, GetString(element.ArgumentFormat, value3, report), GetString(element.ValueFormat, value, report), GetString(element.ValueFormat, value2, report), seriesKey);
			}
		}
		int num = 0;
		series.Hyperlinks = new string[count];
		object[] arguments = series.Arguments;
		foreach (object obj in arguments)
		{
			string value4 = string.Empty;
			if (num < series.Values.Length)
			{
				value4 = series.Values[num].ToString();
			}
			string endValue = string.Empty;
			if (num < stiRangeSeries.ValuesEnd.Length)
			{
				endValue = stiRangeSeries.ValuesEnd[num].ToString();
			}
			if (num < series.Hyperlinks.Length)
			{
				series.Hyperlinks[num] = GetHyperlink(element, (element.DashboardInteraction as StiChartDashboardInteraction).Hyperlink, obj?.ToString(), value4, endValue, series.TitleValue);
			}
			num++;
		}
		series.DrillDownEnabled = true;
	}

	private string GetToolTip(StiChartElement element, string toolTip, string tooltipArgument, string tooltipValue, string tooltipEndValue, string series)
	{
		if (tooltipArgument == null)
		{
			tooltipArgument = "";
		}
		if (series == null)
		{
			series = "";
		}
		if (string.IsNullOrWhiteSpace(toolTip))
		{
			if (tooltipArgument != null && !string.IsNullOrEmpty(tooltipArgument))
			{
				tooltipArgument = tooltipArgument.Replace("\"", "\\\"") ?? "";
			}
			string format = "{{\"StiColor\":\"{0}\",\"StiValue\":\"{1} - {3}\",\"StiArgument\":\"{2}\"}}";
			return string.Format(format, "#ffffff", tooltipValue, tooltipArgument, tooltipEndValue);
		}
		return GetHyperlink(element, toolTip, tooltipArgument, tooltipValue, tooltipEndValue, series);
	}

	private string GetHyperlink(StiChartElement element, string hyperlink, string argument, string value, string endValue, string series)
	{
		if (argument == null)
		{
			argument = "";
		}
		if (series == null)
		{
			series = "";
		}
		Dictionary<string, object> constants = new Dictionary<string, object>
		{
			{ "Argument", argument },
			{ "Value", value },
			{ "EndValue", endValue },
			{ "Series", series }
		};
		return StiReportParser.Parse(hyperlink, element, allowCache: false, constants);
	}

	protected override IEnumerable<StiMeter> GetValueMeters(StiDataTable table)
	{
		return table?.Meters.Where((IStiMeter m) => m is StiStartValueChartMeter).Cast<StiStartValueChartMeter>();
	}

	protected int GetEndValueIndex(StiDataTable table, int index)
	{
		if (table == null)
		{
			return -1;
		}
		int[] endValueMeterIndexes = GetEndValueMeterIndexes(table);
		if (endValueMeterIndexes == null || !endValueMeterIndexes.Any())
		{
			return -1;
		}
		if (endValueMeterIndexes.Length == 1)
		{
			return endValueMeterIndexes.FirstOrDefault();
		}
		if (endValueMeterIndexes.Length > index)
		{
			return (int)endValueMeterIndexes.GetValue(index);
		}
		return -1;
	}

	protected int[] GetEndValueMeterIndexes(StiDataTable table)
	{
		if (table == null)
		{
			return null;
		}
		return (from r in GetEndValueMeters(table)
			select table.Meters.IndexOf(r)).ToArray();
	}

	protected IEnumerable<StiEndValueChartMeter> GetEndValueMeters(StiDataTable table)
	{
		return table?.Meters.Where((IStiMeter m) => m is StiEndValueChartMeter).Cast<StiEndValueChartMeter>();
	}
}
