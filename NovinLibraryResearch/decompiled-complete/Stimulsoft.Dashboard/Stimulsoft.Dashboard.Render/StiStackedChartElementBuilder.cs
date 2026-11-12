using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Render;

internal class StiStackedChartElementBuilder : StiChartElementBuilder
{
	protected override void RenderElements(StiChartElement element, StiChart chart, StiDataTable dataTable)
	{
		StiReport report = chart?.Report;
		int seriesMeterIndex = GetSeriesMeterIndex(dataTable);
		IEnumerable<string> seriesKeys = GetSeriesKeys(dataTable, seriesMeterIndex, GetSeriesKeysLimitCount(element));
		int[] argumentMeterIndexes = GetArgumentMeterIndexes(dataTable);
		List<object> list = GetArgumentKeys(element, dataTable).ToList();
		IEnumerable<StiMeter> valueMeters = GetValueMeters(dataTable);
		int num = seriesKeys.Count();
		List<string> list2 = new List<string>();
		foreach (string item in seriesKeys)
		{
			int num2 = 0;
			foreach (StiMeter item2 in valueMeters)
			{
				list2.Add(item);
				int itemIndex = GetValueMeterIndexes(dataTable)[num2];
				StiSeries stiSeries = RenderSeries(element, item2, item, chart);
				if (stiSeries == null)
				{
					continue;
				}
				StiChartElementBuilder.RenderConditions(item, stiSeries, element, item2);
				StiChartElementBuilder.RenderTrendLines(stiSeries, element, item2);
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
				IEnumerable<object[]> rows2 = enumerable;
				stiSeries.Values = new double?[list.Count];
				int num3 = 0;
				foreach (object item3 in list)
				{
					IEnumerable<object[]> detailRows = StiChartElementBuilder.GetDetailRows(rows2, item3, argumentMeterIndexes);
					if (num3 < stiSeries.Values.Length)
					{
						stiSeries.Values[num3] = detailRows.GetArrayItem(itemIndex).FirstOrDefaultAsNullableDouble();
					}
					num3++;
				}
				stiSeries.OriginalArguments = list.ToArray();
				object[] arguments = list.Select((object argument) => GetString(element.ArgumentFormat, argument, report)).ToArray();
				stiSeries.Arguments = arguments;
				num2++;
			}
		}
		ProcessTopNStackedChart(element, chart);
		for (int i = 0; i < list2.Count() && chart.Series.Count >= i && chart.Series[i] is StiSeries series; i++)
		{
			RenderSeriesInteraction(series, element, list.Count, list2[i]);
		}
	}

	private static DataTable GetDataTable(List<StiSeries> seriesCollection)
	{
		DataTable dataTable = new DataTable();
		DataColumn dataColumn = new DataColumn("Id", Type.GetType("System.Int32"));
		dataColumn.Unique = true;
		dataColumn.AllowDBNull = false;
		dataColumn.AutoIncrement = true;
		dataColumn.AutoIncrementSeed = 0L;
		dataColumn.AutoIncrementStep = 1L;
		dataTable.Columns.Add(dataColumn);
		DataColumn dataColumn2 = new DataColumn("Argument", typeof(object));
		dataColumn2.AllowDBNull = true;
		dataTable.Columns.Add(dataColumn2);
		for (int i = 0; i < seriesCollection.Count; i++)
		{
			DataColumn dataColumn3 = new DataColumn($"Value-{i}", typeof(double));
			dataColumn3.AllowDBNull = true;
			dataTable.Columns.Add(dataColumn3);
		}
		DataColumn dataColumn4 = new DataColumn("OriginalArguments", typeof(object));
		dataColumn4.AllowDBNull = true;
		dataTable.Columns.Add(dataColumn4);
		DataColumn dataColumn5 = new DataColumn("ValueSum", typeof(double));
		dataColumn5.AllowDBNull = true;
		dataTable.Columns.Add(dataColumn5);
		int countRow = GetCountRow(seriesCollection);
		for (int j = 0; j < countRow; j++)
		{
			dataTable.Rows.Add();
		}
		return dataTable;
	}

	private static int GetCountRow(List<StiSeries> seriesCollection)
	{
		int num = 0;
		foreach (StiSeries item in seriesCollection)
		{
			num = Math.Max(num, item.Arguments.Length);
		}
		return num;
	}

	internal static void ProcessTopNStackedChart(StiChartElement element, StiChart chart)
	{
		if (element.TopN.Mode == StiDataTopNMode.None)
		{
			return;
		}
		StiSeriesCollection series = chart.Series;
		if (!series.ToList().Any((StiSeries s) => s is IStiStackedBaseLineSeries || s is IStiStackedColumnSeries))
		{
			return;
		}
		DataTable dataTable = GetDataTable(series.ToList());
		if (dataTable.Rows.Count < 1)
		{
			return;
		}
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			if (series.Count > 0 && series[0].Arguments.Length > i)
			{
				dataTable.Rows[i][1] = series[0].Arguments[i];
			}
			else
			{
				dataTable.Rows[i][1] = DBNull.Value;
			}
			if (series.Count > 0 && series[0].OriginalArguments.Length > i)
			{
				dataTable.Rows[i][dataTable.Columns.Count - 2] = series[0].OriginalArguments[i];
			}
			else
			{
				dataTable.Rows[i][dataTable.Columns.Count - 2] = DBNull.Value;
			}
			double num = 0.0;
			for (int j = 0; j < series.Count; j++)
			{
				if (series.Count > 0 && series[j].Values.Length > i)
				{
					double? num2 = series[j].Values[i];
					object value = ((!num2.HasValue) ? DBNull.Value : ((object)num2));
					dataTable.Rows[i][j + 2] = value;
					num += num2.GetValueOrDefault();
				}
				else
				{
					dataTable.Rows[i][j + 2] = DBNull.Value;
				}
			}
			dataTable.Rows[i][dataTable.Columns.Count - 1] = num;
		}
		dataTable.DefaultView.Sort = ((element.TopN.Mode == StiDataTopNMode.Top) ? "ValueSum desc" : "ValueSum asc");
		dataTable = dataTable.DefaultView.ToTable();
		int index = 0;
		foreach (StiSeries item in chart.Series)
		{
			item.OriginalArguments = (from r in dataTable.AsEnumerable()
				select r.Field<object>("OriginalArguments")).ToArray();
			List<object> objects = (from r in dataTable.AsEnumerable()
				select r.Field<object>("Argument")).ToList();
			item.Arguments = GetArguments(objects, element).ToArray();
			List<double?> values = (from r in dataTable.AsEnumerable()
				select r.Field<double?>($"Value-{index}")).ToList();
			item.Values = GetValues(values, element).ToArray();
			index++;
		}
	}

	private static List<object> GetArguments(List<object> objects, StiChartElement element)
	{
		if (objects.Count <= element.TopN.Count)
		{
			return objects;
		}
		List<object> list = objects.Take(element.TopN.Count).ToList();
		if (element.TopN.ShowOthers)
		{
			string item = (string.IsNullOrEmpty(element.TopN.OthersText) ? Loc.Get("FormDesigner", "Others") : StiReportParser.Parse(element.TopN.OthersText, element));
			list.Add(item);
		}
		return list;
	}

	private static List<double?> GetValues(List<double?> values, StiChartElement element)
	{
		if (values.Count <= element.TopN.Count)
		{
			return values;
		}
		List<double?> list = values.Take(element.TopN.Count).ToList();
		if (element.TopN.ShowOthers)
		{
			double? item = values.Skip(element.TopN.Count).Sum();
			list.Add(item);
		}
		return list;
	}
}
