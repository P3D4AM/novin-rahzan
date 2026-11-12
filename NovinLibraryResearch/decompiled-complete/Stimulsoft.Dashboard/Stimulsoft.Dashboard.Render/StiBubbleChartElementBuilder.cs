using System;
using System.Collections.Generic;
using System.Globalization;
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

internal class StiBubbleChartElementBuilder : StiChartElementBuilder
{
	protected override void RenderElements(StiChartElement element, StiChart chart, StiDataTable dataTable)
	{
		StiReport report = chart.Report;
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
				if (!(RenderSeries(element, item2, item, chart) is StiBubbleSeries stiBubbleSeries))
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
				stiBubbleSeries.Values = source.GetArrayItem(num3).TryCastToNullableDouble().ToArray();
				int argumentIndex = GetArgumentIndex(dataTable, num3);
				if (argumentIndex != -1)
				{
					object[] arguments = (from argument in source.GetArrayItem(argumentIndex)
						select GetString(element.ArgumentFormat, argument, report)).ToArray();
					stiBubbleSeries.Arguments = arguments;
				}
				stiBubbleSeries.Weights = null;
				int weightIndex = GetWeightIndex(dataTable, num3);
				if (weightIndex != -1)
				{
					stiBubbleSeries.Weights = source.GetArrayItem(weightIndex).TryCastToDouble().ToArray();
				}
				if (stiBubbleSeries.Weights == null)
				{
					stiBubbleSeries.Weights = Enumerable.Repeat(10.0, stiBubbleSeries.Arguments.Length).ToArray();
				}
				num2++;
				int count = Math.Min(stiBubbleSeries.Values.Length, stiBubbleSeries.Arguments.Length);
				RenderSeriesInteraction(stiBubbleSeries, element, count, item);
			}
		}
	}

	protected override StiSeries RenderSeries(StiChartElement element, StiMeter value, string groupKey, StiChart chart)
	{
		StiBubbleSeries stiBubbleSeries = new StiBubbleSeries();
		stiBubbleSeries.CoreTitle = GetSeriesTitle(element, groupKey, value);
		stiBubbleSeries.IsDashboard = true;
		chart.Series.Add(stiBubbleSeries);
		return stiBubbleSeries;
	}

	protected override void RenderSeriesInteraction(StiSeries series, StiChartElement element, int count, string seriesKey)
	{
		if (!(series is StiBubbleSeries stiBubbleSeries))
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
				if (i < series.Values.Length && series.Values[i].HasValue)
				{
					text = series.Values[i].ToString();
				}
				if (i < series.Arguments.Length && series.Arguments[i] != null)
				{
					text2 = $"{series.Arguments[i]}";
				}
				if (i < stiBubbleSeries.Weights.Length)
				{
					text3 = stiBubbleSeries.Weights[i].ToString(CultureInfo.InvariantCulture);
				}
				string format = "{{\"StiColor\":\"{0}\",\"StiValue\":\"{1}\",\"StiArgument\":\"{2}\",\"StiWeight\":\"{3}\"}}";
				series.ToolTips[i] = string.Format(format, "#ffffff", text, text2, text3);
			}
		}
		series.DrillDownEnabled = true;
	}

	protected int GetWeightIndex(StiDataTable table, int index)
	{
		if (table == null)
		{
			return -1;
		}
		int[] weightMeterIndexes = GetWeightMeterIndexes(table);
		if (weightMeterIndexes == null || !weightMeterIndexes.Any())
		{
			return -1;
		}
		if (weightMeterIndexes.Length == 1)
		{
			return weightMeterIndexes.FirstOrDefault();
		}
		if (weightMeterIndexes.Length > index)
		{
			return (int)weightMeterIndexes.GetValue(index);
		}
		return -1;
	}

	protected int[] GetWeightMeterIndexes(StiDataTable table)
	{
		if (table == null)
		{
			return null;
		}
		return (from r in GetWeightMeters(table)
			select table.Meters.IndexOf(r)).ToArray();
	}

	protected IEnumerable<StiWeightChartMeter> GetWeightMeters(StiDataTable table)
	{
		return table?.Meters.Where((IStiMeter m) => m is StiWeightChartMeter).Cast<StiWeightChartMeter>();
	}

	protected override IEnumerable<StiMeter> GetValueMeters(StiDataTable table)
	{
		return table?.Meters.Where((IStiMeter m) => m is StiYChartMeter).Cast<StiYChartMeter>();
	}

	protected override IEnumerable<StiMeter> GetArgumentMeters(StiDataTable table)
	{
		return table?.Meters.Where((IStiMeter m) => m is StiXChartMeter).Cast<StiXChartMeter>();
	}
}
