using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Data.Comparers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Render;

internal class StiChartElementBuilder : StiElementBuilder
{
	public StiChart Render(StiChartElement element, StiDataTable dataTable)
	{
		CultureInfo culture = StiCultureHelper.Set(element?.Report);
		try
		{
			StiChart stiChart = new StiChart
			{
				HorSpacing = 6,
				VertSpacing = 6,
				IsDashboard = true,
				Page = element.Page,
				PreviousAnimations = element.PreviousAnimations
			};
			SetStyle(element, stiChart);
			stiChart.Series.Clear();
			if (dataTable == null)
			{
				return stiChart;
			}
			RenderElements(element, stiChart, dataTable);
			RenderArea(element, stiChart);
			RenderLegend(element, stiChart);
			RenderSeriesLabels(element, stiChart);
			RenderConstantLines(element, stiChart);
			SimplifyValues(stiChart);
			stiChart.ApplyStyle();
			stiChart.AllowApplyStyle = false;
			Color backColor = StiDashboardExpressionHelper.GetBackColor(element, element.BackColor);
			if (backColor != Color.Transparent)
			{
				stiChart.Brush = new StiSolidBrush(backColor);
			}
			return stiChart;
		}
		finally
		{
			StiCultureHelper.Restore(culture);
		}
	}

	protected virtual void RenderElements(StiChartElement element, StiChart chart, StiDataTable dataTable)
	{
		StiReport report = chart?.Report;
		int seriesMeterIndex = GetSeriesMeterIndex(dataTable);
		IEnumerable<object> seriesKeyValues = GetSeriesKeyValues(dataTable, seriesMeterIndex, GetSeriesKeysLimitCount(element));
		int[] argumentMeterIndexes = GetArgumentMeterIndexes(dataTable);
		List<object> list = GetArgumentKeys(element, dataTable).ToList();
		IEnumerable<StiMeter> valueMeters = GetValueMeters(dataTable);
		int num = seriesKeyValues.Count();
		foreach (object item in seriesKeyValues)
		{
			int num2 = 0;
			foreach (StiMeter item2 in valueMeters)
			{
				int itemIndex = GetValueMeterIndexes(dataTable)[num2];
				StiSeries stiSeries = RenderSeries(element, item2, ToReadableString(item, report), chart);
				if (stiSeries == null)
				{
					continue;
				}
				RenderConditions(ToString(item), stiSeries, element, item2);
				RenderTrendLines(stiSeries, element, item2);
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
				IEnumerable<object[]> rows2 = enumerable;
				stiSeries.Values = new double?[list.Count];
				int num3 = 0;
				foreach (object item3 in list)
				{
					IEnumerable<object[]> detailRows = GetDetailRows(rows2, item3, argumentMeterIndexes);
					if (num3 < stiSeries.Values.Length)
					{
						object valueOrFirstOrDefault = IEnumerableExt.GetValueOrFirstOrDefault(detailRows.GetArrayItem(itemIndex).FirstOrDefault());
						if (valueOrFirstOrDefault is DateTime)
						{
							stiSeries.Core.IsDateTimeValues = true;
						}
						stiSeries.Values[num3] = detailRows.GetArrayItem(itemIndex).FirstOrDefaultAsNullableDouble();
					}
					num3++;
				}
				stiSeries.OriginalArguments = list.ToArray();
				stiSeries.Arguments = ProccessSelectArguments(list, element, stiSeries, report);
				if (stiSeries is IStiFunnelSeries stiFunnelSeries)
				{
					double?[] array = stiSeries.Values.ToArray();
					object[] array2 = stiSeries.Arguments.ToArray();
					Array.Sort(array, array2);
					Array.Reverse((Array)array);
					Array.Reverse((Array)array2);
					stiFunnelSeries.Values = array;
					stiFunnelSeries.Arguments = array2;
				}
				else if (stiSeries is IStiParetoSeries)
				{
					List<double?> list2 = new List<double?>();
					List<object> list3 = new List<object>();
					for (int i = 0; i < stiSeries.Values.Length; i++)
					{
						double valueOrDefault = stiSeries.Values[i].GetValueOrDefault();
						if (valueOrDefault > 0.0)
						{
							list2.Add(valueOrDefault);
							if (stiSeries.Arguments != null && i < stiSeries.Arguments.Length)
							{
								list3.Add(stiSeries.Arguments[i]);
							}
							else
							{
								list3.Add(i + 1);
							}
						}
					}
					double?[] array3 = list2.ToArray();
					object[] array4 = list3.ToArray();
					Array.Sort(array3, array4);
					Array.Reverse((Array)array3);
					Array.Reverse((Array)array4);
					stiSeries.ValuesStart = null;
					stiSeries.Values = array3;
					stiSeries.Arguments = array4;
				}
				num2++;
				ProcessTopNElements(element, stiSeries);
				StiHistogramHelper.CheckValuesAndArguments(stiSeries, element.ArgumentFormat);
				RenderSeriesInteraction(stiSeries, element, list.Count, ToString(item));
			}
		}
		StiBoxAndWhiskerHelper.CheckArgument(chart);
		StiWaterfallHelper.CheckTotals(chart);
		CheckProcessSeriesTopN(element, chart);
	}

	private object[] ProccessSelectArguments(List<object> argumentKeys, StiChartElement element, StiSeries series, StiReport report)
	{
		if (series is StiScatterSeries)
		{
			object[] array = new object[series.OriginalArguments.Length];
			for (int i = 0; i < series.OriginalArguments.Length; i++)
			{
				object[] array2 = series.OriginalArguments[i] as object[];
				object obj = ((array2 != null) ? array2.FirstOrDefault() : array2);
				array[i] = obj;
			}
			return array;
		}
		return argumentKeys.Select((object argument) => GetString(element.ArgumentFormat, argument, report)).ToArray();
	}

	private void CheckProcessSeriesTopN(StiChartElement element, StiChart chart)
	{
		if (element.TopN.Mode == StiDataTopNMode.None || chart.Series.Count <= 1)
		{
			return;
		}
		DataTable dataTableForTopNProcess = GetDataTableForTopNProcess(chart);
		for (int i = 0; i < chart.Series.Count; i++)
		{
			IStiSeries stiSeries = chart.Series[i];
			for (int j = 0; j < stiSeries.Arguments.Length; j++)
			{
				object argument = ((stiSeries.Arguments.Length > j) ? stiSeries.Arguments[j] : null);
				object value = ((stiSeries.Arguments.Length > j) ? stiSeries.OriginalArguments[j] : null);
				double? num = ((stiSeries.Values.Length > j) ? stiSeries.Values[j] : null);
				if (dataTableForTopNProcess.AsEnumerable().Any((DataRow row) => argument.Equals(row.Field<object>("Argument"))))
				{
					DataRow dataRow = dataTableForTopNProcess.AsEnumerable().FirstOrDefault((DataRow x) => argument.Equals(x.Field<object>("Argument")));
					dataRow[$"Value{i}"] = num.GetValueOrDefault();
				}
				else if (!argument.ToString().Equals(element.TopN.OthersText))
				{
					DataRow dataRow2 = dataTableForTopNProcess.NewRow();
					dataRow2["Argument"] = argument;
					dataRow2["OriginalArgument"] = value;
					dataRow2[$"Value{i}"] = num.GetValueOrDefault();
					dataTableForTopNProcess.Rows.Add(dataRow2);
				}
			}
		}
		if (element.TopN.ShowOthers)
		{
			DataRow dataRow3 = dataTableForTopNProcess.NewRow();
			for (int k = 0; k < chart.Series.Count; k++)
			{
				IStiSeries stiSeries2 = chart.Series[k];
				dataRow3["Argument"] = element.TopN.OthersText;
				dataRow3["OriginalArgument"] = element.TopN.OthersText;
				if (stiSeries2.Values.Length != 0)
				{
					dataRow3[$"Value{k}"] = stiSeries2.Values[stiSeries2.Values.Length - 1];
				}
			}
		}
		DataView defaultView = dataTableForTopNProcess.DefaultView;
		defaultView.Sort = "Argument asc";
		dataTableForTopNProcess = defaultView.ToTable();
		int indexSeries;
		for (indexSeries = 0; indexSeries < chart.Series.Count; indexSeries++)
		{
			StiSeries stiSeries3 = chart.Series[indexSeries] as StiSeries;
			stiSeries3.Arguments = (from DataRow row in dataTableForTopNProcess.Rows
				select row["Argument"]).ToArray();
			stiSeries3.OriginalArguments = (from DataRow row in dataTableForTopNProcess.Rows
				select row["OriginalArgument"]).ToArray();
			stiSeries3.Values = (from DataRow row in dataTableForTopNProcess.Rows
				select StiValueHelper.TryToNullableDouble(row[$"Value{indexSeries}"])).ToArray();
			RenderSeriesInteraction(stiSeries3, element, stiSeries3.Arguments.Length, "seriesKey");
		}
	}

	private DataTable GetDataTableForTopNProcess(StiChart chart)
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
		DataColumn dataColumn3 = new DataColumn("OriginalArgument", typeof(object));
		dataColumn3.AllowDBNull = true;
		dataTable.Columns.Add(dataColumn3);
		for (int i = 0; i < chart.Series.Count; i++)
		{
			DataColumn column = new DataColumn($"Value{i}", typeof(double));
			dataTable.Columns.Add(column);
		}
		return dataTable;
	}

	internal static void ProcessTopNElements(StiChartElement element, StiSeries series)
	{
		if (element.TopN.Mode == StiDataTopNMode.None)
		{
			return;
		}
		List<KeyValuePair<KeyValuePair<object, object>, object>> list = new List<KeyValuePair<KeyValuePair<object, object>, object>>();
		int num = ((series.Values.Length > series.Arguments.Length) ? series.Values.Length : series.Arguments.Length);
		for (int i = 0; i < num; i++)
		{
			double? num2 = ((i < series.Values.Length) ? series.Values[i] : null);
			object key = ((i < series.Arguments.Length) ? series.Arguments[i] : null);
			object value = ((i < series.OriginalArguments.Length) ? series.OriginalArguments[i] : null);
			KeyValuePair<object, object> key2 = new KeyValuePair<object, object>(key, value);
			list.Add(new KeyValuePair<KeyValuePair<object, object>, object>(key2, num2));
		}
		list = ((element.TopN.Mode == StiDataTopNMode.Bottom) ? list.OrderBy((KeyValuePair<KeyValuePair<object, object>, object> e) => e.Value).ToList() : list.OrderByDescending((KeyValuePair<KeyValuePair<object, object>, object> e) => e.Value).ToList());
		List<KeyValuePair<KeyValuePair<object, object>, object>> list2 = list.Take(element.TopN.Count).ToList();
		if (list.Count > element.TopN.Count && element.TopN.ShowOthers)
		{
			decimal num3 = list.Skip(element.TopN.Count).Sum((KeyValuePair<KeyValuePair<object, object>, object> c) => StiValueHelper.TryToDecimal(c.Value));
			string text = (string.IsNullOrEmpty(element.TopN.OthersText) ? Loc.Get("FormDesigner", "Others") : StiReportParser.Parse(element.TopN.OthersText, element));
			list2.Add(new KeyValuePair<KeyValuePair<object, object>, object>(new KeyValuePair<object, object>(text, text), num3));
		}
		if (element.IsBarChart)
		{
			list2.Reverse();
		}
		series.OriginalArguments = list2.Select((KeyValuePair<KeyValuePair<object, object>, object> r) => r.Key.Value).ToArray();
		series.Arguments = list2.Select((KeyValuePair<KeyValuePair<object, object>, object> r) => r.Key.Key).ToArray();
		series.Values = list2.Select((KeyValuePair<KeyValuePair<object, object>, object> r) => StiValueHelper.TryToNullableDouble(r.Value)).ToArray();
	}

	private static void SetStyle(StiChartElement element, StiChart chart)
	{
		if (element.Style != StiElementStyleIdent.Custom)
		{
			chart.Style = StiDashboardStyleHelper.GetChartStyle(element);
			return;
		}
		StiCustomStyle customChartStyle = element.Report.Styles.GetCustomChartStyle(element.CustomStyleName);
		if (customChartStyle != null)
		{
			chart.Style = customChartStyle;
			chart.CustomStyleName = element.CustomStyleName;
		}
		else
		{
			chart.Style = StiDashboardStyleHelper.GetChartStyle(StiElementStyleIdent.Blue);
		}
	}

	protected static IEnumerable<object[]> GetDetailRows(IEnumerable<object[]> rows, object argument, int[] argumentIndexes)
	{
		if (!argumentIndexes.Any())
		{
			return rows;
		}
		return rows.Where(delegate(object[] row)
		{
			object[] value = argumentIndexes.Select((int index) => row[index]).ToArray();
			return StiValueComparer.EqualValues(value, argument);
		});
	}

	protected virtual StiSeries RenderSeries(StiChartElement element, StiMeter value, string seriesKey, StiChart chart)
	{
		StiValueChartMeter stiValueChartMeter = value as StiValueChartMeter;
		StiSeries stiSeries = StiChartSeriesCreator.New(stiValueChartMeter.SeriesType);
		RenderSeriesYAxis(element, stiValueChartMeter, stiSeries);
		RenderSeriesLine(stiValueChartMeter, stiSeries);
		RenderSeriesShowZeros(stiValueChartMeter, stiSeries);
		RenderSeriesShowNulls(stiValueChartMeter, stiSeries);
		stiSeries.CoreTitle = GetSeriesTitle(element, seriesKey, value);
		stiSeries.Core.SeriesColors = ((element.SeriesColors != null && element.SeriesColors.Length != 0) ? element.SeriesColors : null);
		stiSeries.IsDashboard = true;
		RenderSeriesNegativeColor(element, stiSeries, chart.Series.Count);
		RenderSeriesParetoColor(element, stiSeries, chart.Series.Count);
		RenderSeriesMarker(element, stiSeries);
		RenderSeriesLighting(stiSeries);
		RenderSeriesIcon(element, stiSeries);
		RenderSeriesPie3d(element, stiSeries);
		chart.Series.Add(stiSeries);
		return stiSeries;
	}

	private void RenderSeriesPie3d(StiChartElement element, StiSeries series)
	{
		if (series is StiPie3dSeries stiPie3dSeries && element.Options3D is StiPie3dOptions options3D)
		{
			stiPie3dSeries.Options3D = options3D;
		}
	}

	private static void RenderSeriesYAxis(StiChartElement element, StiValueChartMeter valueMeter, StiSeries series)
	{
		if (valueMeter != null && element.Values.Count > 1)
		{
			series.YAxis = valueMeter.YAxis;
		}
	}

	private static void RenderSeriesLine(StiValueChartMeter valueMeter, StiSeries series)
	{
		IStiBaseLineSeries stiBaseLineSeries = series as IStiBaseLineSeries;
		if (valueMeter != null && stiBaseLineSeries != null)
		{
			stiBaseLineSeries.LineStyle = valueMeter.LineStyle;
			stiBaseLineSeries.LineWidth = valueMeter.LineWidth;
		}
	}

	private static void RenderSeriesShowZeros(StiValueChartMeter valueMeter, StiSeries series)
	{
		if (!(series is IStiShowZerosSeries stiShowZerosSeries))
		{
			return;
		}
		stiShowZerosSeries.ShowZeros = valueMeter.ShowZeros == StiEmptyCellsAs.Zero;
		if (series is IStiBaseLineSeries stiBaseLineSeries)
		{
			if (valueMeter.ShowZeros == StiEmptyCellsAs.Gap)
			{
				stiBaseLineSeries.ShowZerosAs = StiShowEmptyCellsAs.Gap;
			}
			if (valueMeter.ShowZeros == StiEmptyCellsAs.ConnectPointsWithLine)
			{
				stiBaseLineSeries.ShowZerosAs = StiShowEmptyCellsAs.ConnectPointsWithLine;
			}
		}
	}

	private static void RenderSeriesShowNulls(StiValueChartMeter valueMeter, StiSeries series)
	{
		if (!(series is IStiShowNullsSeries stiShowNullsSeries))
		{
			return;
		}
		stiShowNullsSeries.ShowNulls = valueMeter.ShowNulls == StiEmptyCellsAs.Zero;
		if (series is IStiBaseLineSeries stiBaseLineSeries)
		{
			if (valueMeter.ShowNulls == StiEmptyCellsAs.Gap)
			{
				stiBaseLineSeries.ShowNullsAs = StiShowEmptyCellsAs.Gap;
			}
			if (valueMeter.ShowNulls == StiEmptyCellsAs.ConnectPointsWithLine)
			{
				stiBaseLineSeries.ShowNullsAs = StiShowEmptyCellsAs.ConnectPointsWithLine;
			}
		}
	}

	private static void RenderSeriesLighting(StiSeries series)
	{
		if (series is StiPieSeries)
		{
			((StiPieSeries)series).Lighting = false;
		}
		if (series is StiDoughnutSeries)
		{
			((StiDoughnutSeries)series).Lighting = false;
		}
	}

	private static void RenderSeriesIcon(StiChartElement element, StiSeries series)
	{
		if (series is IStiFontIconsSeries stiFontIconsSeries)
		{
			if (series is StiPictorialSeries && !element.Icon.HasValue)
			{
				stiFontIconsSeries.Icon = StiFontIcons.QuarterFull;
			}
			else
			{
				stiFontIconsSeries.Icon = element.Icon;
			}
		}
	}

	private void RenderSeriesMarker(StiChartElement element, StiSeries series)
	{
		if (series is StiBaseLineSeries stiBaseLineSeries)
		{
			stiBaseLineSeries.Marker.Size = element.Marker.Size;
			stiBaseLineSeries.Marker.Angle = element.Marker.Angle;
			stiBaseLineSeries.Marker.Type = element.Marker.Type;
			if (stiBaseLineSeries.Marker is StiMarker stiMarker)
			{
				stiMarker.ExtendedVisible = element.Marker.Visible;
			}
		}
	}

	private void RenderSeriesNegativeColor(StiChartElement element, StiSeries series, int seriesIndex)
	{
		if (element.NegativeSeriesColors != null && element.NegativeSeriesColors.Length != 0)
		{
			while (seriesIndex >= element.NegativeSeriesColors.Length)
			{
				seriesIndex -= element.NegativeSeriesColors.Length;
			}
			if (series is IStiAllowApplyBrushNegative stiAllowApplyBrushNegative)
			{
				stiAllowApplyBrushNegative.AllowApplyBrushNegative = true;
				stiAllowApplyBrushNegative.BrushNegative = new StiSolidBrush(element.NegativeSeriesColors[seriesIndex]);
			}
			if (series is IStiAllowApplyColorNegative stiAllowApplyColorNegative)
			{
				stiAllowApplyColorNegative.AllowApplyColorNegative = true;
				stiAllowApplyColorNegative.LineColorNegative = element.NegativeSeriesColors[seriesIndex];
			}
		}
	}

	private void RenderSeriesParetoColor(StiChartElement element, StiSeries series, int seriesIndex)
	{
		if (element.ParetoSeriesColors != null && element.ParetoSeriesColors.Length != 0)
		{
			while (seriesIndex >= element.ParetoSeriesColors.Length)
			{
				seriesIndex -= element.ParetoSeriesColors.Length;
			}
			if (series is StiParetoSeries stiParetoSeries)
			{
				stiParetoSeries.AllowApplyLineColor = true;
				stiParetoSeries.LineColor = element.ParetoSeriesColors[seriesIndex];
			}
		}
	}

	protected virtual void RenderArea(StiChartElement element, StiChart chart)
	{
		Color backColor = StiDashboardExpressionHelper.GetBackColor(element, element.BackColor);
		if (backColor != Color.Transparent)
		{
			chart.Area.Brush = ((backColor.A == byte.MaxValue) ? ((StiBrush)new StiSolidBrush(backColor)) : ((StiBrush)new StiEmptyBrush()));
		}
		if (element.IsPieChart || element.IsPie3dChart || element.IsDoughnutChart || element.IsFunnelChart || element.IsSunburstChart)
		{
			chart.Area.AllowApplyStyle = false;
			chart.Area.BorderColor = Color.Transparent;
		}
		chart.Area.ColorEach = GetColorEach(element);
		if (chart.Area is IStiRoundValuesArea stiRoundValuesArea)
		{
			stiRoundValuesArea.RoundValues = element.RoundValues;
		}
		RenderAxis(element, chart);
		RenderRadarAxis(element, chart);
	}

	private static bool GetColorEach(StiChartElement element)
	{
		if (element.IsPieChart || element.IsPie3dChart || element.IsDoughnutChart)
		{
			return true;
		}
		if (element.Area != null)
		{
			return element.Area.ColorEach;
		}
		return false;
	}

	private void RenderRadarAxis(StiChartElement element, StiChart chart)
	{
		if (chart.Series.Count > 0 && chart.Area is StiRadarArea)
		{
			RenderXRadarAxis(element, chart);
			RenderYRadarAxis(element, chart);
		}
	}

	private void RenderAxis(StiChartElement element, StiChart chart)
	{
		if (chart.Series.Count <= 0)
		{
			return;
		}
		StiAxisArea stiAxisArea = chart.Area as StiAxisArea;
		StiRadarArea stiRadarArea = chart.Area as StiRadarArea;
		if (stiAxisArea != null)
		{
			RenderInterlacingHor(element, stiAxisArea);
			RenderInterlacingVert(element, stiAxisArea);
			RenderGridLinesHor(element, stiAxisArea);
			RenderGridLinesVert(element, stiAxisArea);
			stiAxisArea.ReverseHor = element.Area.ReverseHor;
			stiAxisArea.ReverseVert = element.Area.ReverseVert;
			RenderXAxis(element, chart, isTopAxis: false);
			RenderYAxis(element, chart, isRightAxis: false);
			if (element.XTopAxis.Visible)
			{
				RenderXAxis(element, chart, isTopAxis: true);
			}
			if (element.YRightAxis.Visible)
			{
				RenderYAxis(element, chart, isRightAxis: true);
			}
			else
			{
				RenderYRightAxis(element, chart);
			}
		}
	}

	private static void RenderInterlacingHor(StiChartElement element, StiAxisArea axisArea)
	{
		if (element.Area.InterlacingHor.Visible && element.Area.InterlacingHor.Color != Color.Transparent)
		{
			axisArea.InterlacingHor.InterlacedBrush = new StiSolidBrush(element.Area.InterlacingHor.Color);
		}
		axisArea.InterlacingHor.Visible = element.Area.InterlacingHor.Visible;
	}

	private static void RenderInterlacingVert(StiChartElement element, StiAxisArea axisArea)
	{
		if (element.Area.InterlacingVert.Visible && element.Area.InterlacingVert.Color != Color.Transparent)
		{
			axisArea.InterlacingVert.InterlacedBrush = new StiSolidBrush(element.Area.InterlacingVert.Color);
		}
		axisArea.InterlacingVert.Visible = element.Area.InterlacingVert.Visible;
	}

	private static void RenderGridLinesHor(StiChartElement element, StiAxisArea axisArea)
	{
		if (element.Area.GridLinesHor.Visible && element.Area.GridLinesHor.Color != Color.Transparent)
		{
			axisArea.GridLinesHor.Color = element.Area.GridLinesHor.Color;
		}
		axisArea.GridLinesHor.Visible = element.Area.GridLinesHor.Visible;
		axisArea.GridLinesHor.Style = StiPenStyle.Solid;
	}

	private static void RenderGridLinesVert(StiChartElement element, StiAxisArea axisArea)
	{
		if (element.Area.GridLinesVert.Visible && element.Area.GridLinesVert.Color != Color.Transparent)
		{
			axisArea.GridLinesVert.Color = element.Area.GridLinesVert.Color;
		}
		axisArea.GridLinesVert.Visible = element.Area.GridLinesVert.Visible;
		axisArea.GridLinesVert.Style = StiPenStyle.Solid;
	}

	private void RenderXAxis(StiChartElement element, StiChart chart, bool isTopAxis)
	{
		if (chart.Area is StiAxisArea stiAxisArea)
		{
			StiXAxis stiXAxis = (isTopAxis ? (stiAxisArea.XTopAxis as StiXAxis) : (stiAxisArea.XAxis as StiXAxis));
			StiXChartAxis stiXChartAxis = (isTopAxis ? element.XTopAxis : element.XAxis);
			bool flag = chart.Area is IStiGanttArea || chart.Area is IStiClusteredBarArea;
			List<StiArgumentChartMeter> source = ((StiDrillDownHelper.IsDrillAvailable(element) && element.DrillDownCurrentLevel < element.DrillDownLevelCount) ? element.Arguments.GetRange(element.DrillDownCurrentLevel, 1) : element.Arguments);
			IEnumerable<string> enumerable = ((!flag) ? source.Select(GetTitle).Distinct() : element.Values.Select(GetTitle).Distinct());
			RenderAxis(element, stiXAxis, stiXChartAxis);
			stiXAxis.Ticks.Visible = enumerable.Any();
			stiXAxis.Visible = stiXChartAxis.Visible;
			RenderXAxisTitleText(element, stiXAxis, stiXChartAxis, enumerable);
			bool flag2 = chart.Series.Cast<StiSeries>().All((StiSeries s) => s is IStiBaseLineSeries || s is IStiStackedBaseLineSeries);
			if (stiXChartAxis.StartFromZero == StiAutoBool.Auto)
			{
				stiXAxis.StartFromZero = !flag2;
			}
			if (chart.Series.Cast<StiSeries>().All((StiSeries s) => s.IsDateTimeValues) && stiAxisArea is StiClusteredBarArea)
			{
				stiXAxis.StartFromZero = false;
			}
			if (stiXChartAxis.ShowEdgeValues == StiAutoBool.Auto)
			{
				stiXAxis.ShowEdgeValues = flag2;
			}
			if (stiXAxis.Labels.Placement == StiLabelsPlacement.AutoRotation)
			{
				stiXAxis.Labels.TextAlignment = StiHorAlignment.Left;
			}
			if (element.ValueFormat != null && stiAxisArea is StiClusteredBarArea)
			{
				stiXAxis.Labels.FormatService = CheckValueFormat(element.ValueFormat, chart);
			}
			if (element.ArgumentFormat != null && stiAxisArea is StiScatterArea)
			{
				stiXAxis.Labels.FormatService = element.ArgumentFormat;
			}
			RenderAxisLineColor(element, stiXAxis);
		}
	}

	private void RenderYAxis(StiChartElement element, StiChart chart, bool isRightAxis)
	{
		if (chart.Area is StiAxisArea stiAxisArea)
		{
			StiYAxis stiYAxis = (isRightAxis ? (stiAxisArea.YRightAxis as StiYAxis) : (stiAxisArea.YAxis as StiYAxis));
			StiYChartAxis stiYChartAxis = (isRightAxis ? element.YRightAxis : element.YAxis);
			IEnumerable<string> axisTitles = GetAxisTitles(element, chart);
			RenderAxis(element, stiYAxis, stiYChartAxis);
			stiYAxis.Visible = axisTitles.Any() && stiYChartAxis.Visible;
			RenderYAxisTitleText(element, stiYAxis, stiYChartAxis, axisTitles);
			if (element.ValueFormat != null && !(stiAxisArea is StiClusteredBarArea))
			{
				stiYAxis.Labels.FormatService = CheckValueFormat(element.ValueFormat, chart);
			}
			if (chart.Series.Cast<StiSeries>().All((StiSeries s) => s.IsDateTimeValues))
			{
				stiAxisArea.YAxis.StartFromZero = false;
			}
			RenderAxisLineColor(element, stiYAxis);
		}
	}

	private void RenderXRadarAxis(StiChartElement element, StiChart chart)
	{
		if (chart.Area is StiRadarArea stiRadarArea)
		{
			if (element.ValueFormat != null)
			{
				stiRadarArea.YAxis.Labels.FormatService = CheckValueFormat(element.ValueFormat, chart);
			}
			RenderXRadarAxisLabels(element.Area.XAxis.Labels, stiRadarArea.XAxis.Labels);
			RenderAxisRange(stiRadarArea.XAxis.Range, element.XAxis);
			Color foreColor = StiDashboardStyleHelper.GetForeColor(element);
			RenderRadarXAxisLabelsColor(element, stiRadarArea.XAxis.Labels, element.XAxis, foreColor);
			stiRadarArea.XAxis.Visible = element.XAxis.Visible;
		}
	}

	private static void RenderXRadarAxisLabels(StiChartAxisLabels elementLabel, IStiRadarAxisLabels radarLabel)
	{
		radarLabel.Font = elementLabel.Font;
		radarLabel.TextBefore = elementLabel.TextBefore;
		radarLabel.TextAfter = elementLabel.TextAfter;
		radarLabel.AllowApplyStyle = false;
	}

	private void RenderYRadarAxis(StiChartElement element, StiChart chart)
	{
		if (chart.Area is StiRadarArea stiRadarArea)
		{
			RenderYRadarAxisLabels(element.YAxis.Labels, stiRadarArea.YAxis.Labels);
			RenderAxisRange(stiRadarArea.YAxis.Range, element.YAxis);
			Color foreColor = StiDashboardStyleHelper.GetForeColor(element);
			RenderRadarYAxisLabelsColor(element, stiRadarArea.YAxis.Labels, element.YAxis, foreColor);
			stiRadarArea.YAxis.Visible = element.YAxis.Visible;
		}
	}

	private static void RenderYRadarAxisLabels(StiChartAxisLabels elementLabel, IStiAxisLabels radarLabel)
	{
		radarLabel.Font = elementLabel.Font;
		radarLabel.TextBefore = elementLabel.TextBefore;
		radarLabel.TextAfter = elementLabel.TextAfter;
		radarLabel.Step = elementLabel.Step;
		radarLabel.Angle = elementLabel.Angle;
		radarLabel.TextAlignment = elementLabel.TextAlignment;
		radarLabel.AllowApplyStyle = false;
	}

	private static StiFormatService CheckValueFormat(StiFormatService format, StiChart chart)
	{
		if (format is StiNumberFormatService stiNumberFormatService && (stiNumberFormatService.State & Stimulsoft.Report.Components.StiTextFormatState.Abbreviation) > Stimulsoft.Report.Components.StiTextFormatState.None)
		{
			StiNumberFormatService stiNumberFormatService2 = stiNumberFormatService.Clone() as StiNumberFormatService;
			double? num = null;
			for (int i = 0; i < chart.Series.Count; i++)
			{
				IStiSeries stiSeries = chart.Series[i];
				double?[] values = stiSeries.Values;
				foreach (double? num2 in values)
				{
					if (!num.HasValue)
					{
						num = num2;
					}
					else if (num > num2)
					{
						num = num2;
					}
				}
			}
			if (num.HasValue && num != 0.0)
			{
				num = Math.Abs(num.GetValueOrDefault());
				int num3 = 0;
				if (num < 5.0)
				{
					while ((int)num.Value % 10 == 0)
					{
						num *= 10.0;
						num3++;
					}
					num3 += 2;
				}
				stiNumberFormatService2.UseLocalSetting = false;
				stiNumberFormatService2.DecimalDigits = num3;
			}
			return stiNumberFormatService2;
		}
		return format;
	}

	private static void RenderYRightAxis(StiChartElement element, StiChart chart)
	{
		if (chart.Area is StiAxisArea stiAxisArea)
		{
			RenderAxis(element, stiAxisArea.YRightAxis as StiAxis, element.YRightAxis);
			RenderAxisLineColor(element, stiAxisArea.YRightAxis as StiAxis);
		}
	}

	private static void RenderXAxisTitleText(StiChartElement element, StiXAxis chartAxis, StiXChartAxis elementAxis, IEnumerable<string> argumentTitles)
	{
		if (elementAxis.Title.Visible)
		{
			chartAxis.Title.Text = (string.IsNullOrEmpty(elementAxis.Title.Text) ? string.Join(" / ", argumentTitles) : StiReportParser.Parse(elementAxis.Title.Text, element));
		}
	}

	private static void RenderYAxisTitleText(StiChartElement element, StiYAxis chartAxis, StiYChartAxis elementAxis, IEnumerable<string> valueTitles)
	{
		if (elementAxis.Title.Visible)
		{
			chartAxis.Title.Text = (string.IsNullOrEmpty(elementAxis.Title.Text) ? string.Join(" / ", valueTitles) : StiReportParser.Parse(elementAxis.Title.Text, element));
		}
	}

	private static void RenderAxisLineColor(StiChartElement element, StiAxis chartAxis)
	{
		if (element.Style == StiElementStyleIdent.Custom && chartAxis.LineColor != Color.Transparent)
		{
			chartAxis.AllowApplyStyle = false;
		}
		else if (StiDashboardStyleHelper.IsDarkStyle(element.Dashboard))
		{
			chartAxis.AllowApplyStyle = false;
			chartAxis.LineColor = StiDashboardStyleHelper.GetForeColor(element);
		}
	}

	private static void RenderAxis(StiChartElement element, StiAxis chartAxis, StiChartAxis elementAxis)
	{
		Color foreColor = StiDashboardStyleHelper.GetForeColor(element);
		RenderAxisTitle(element, chartAxis, elementAxis, foreColor);
		RenderAxisLabels(element, chartAxis, elementAxis);
		RenderAxisStartFromZero(chartAxis, elementAxis);
		RenderAxisShowEdgeValues(chartAxis, elementAxis);
		RenderAxisRange(chartAxis.Range, elementAxis);
		RenderAxisLabelsColor(element, chartAxis, elementAxis, foreColor);
	}

	private static void RenderAxisTitle(StiChartElement element, StiAxis chartAxis, StiChartAxis elementAxis, Color styledColor)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		StiChartAxisTitle titleAxisChart = GetTitleAxisChart(elementAxis);
		IStiAxisTitle title = chartAxis.Title;
		title.Alignment = titleAxisChart.Alignment;
		title.Position = titleAxisChart.Position;
		title.Font = titleAxisChart.Font;
		RenderAxisLabelsTitleColor(element, title, titleAxisChart, styledColor);
		if (titleAxisChart is StiXChartAxisTitle)
		{
			title.Direction = (titleAxisChart as StiXChartAxisTitle).Direction;
		}
		if (titleAxisChart is StiYChartAxisTitle)
		{
			title.Direction = (titleAxisChart as StiYChartAxisTitle).Direction;
		}
	}

	private static void RenderAxisLabels(StiChartElement element, StiAxis chartAxis, StiChartAxis elementAxis)
	{
		chartAxis.Labels.Angle = elementAxis.Labels.Angle;
		if (!(chartAxis is StiYRightAxis) || !element.IsParetoChart)
		{
			chartAxis.Labels.TextAfter = StiReportParser.Parse(elementAxis.Labels.TextAfter, element);
		}
		chartAxis.Labels.TextAlignment = elementAxis.Labels.TextAlignment;
		chartAxis.Labels.TextBefore = StiReportParser.Parse(elementAxis.Labels.TextBefore, element);
		chartAxis.Labels.Font = elementAxis.Labels.Font;
		chartAxis.Labels.Placement = elementAxis.Labels.Placement;
		chartAxis.Labels.Step = elementAxis.Labels.Step;
	}

	private static void RenderAxisStartFromZero(StiAxis chartAxis, StiChartAxis elementAxis)
	{
		if (elementAxis is StiXChartAxis { StartFromZero: not StiAutoBool.Auto } stiXChartAxis)
		{
			chartAxis.StartFromZero = stiXChartAxis.StartFromZero != StiAutoBool.False;
		}
		if (elementAxis is StiYChartAxis stiYChartAxis)
		{
			chartAxis.StartFromZero = stiYChartAxis.StartFromZero;
		}
	}

	private static void RenderAxisShowEdgeValues(StiAxis chartAxis, StiChartAxis elementAxis)
	{
		if (elementAxis is StiXChartAxis { ShowEdgeValues: not StiAutoBool.Auto } stiXChartAxis)
		{
			((StiXAxis)chartAxis).ShowEdgeValues = stiXChartAxis.ShowEdgeValues != StiAutoBool.False;
		}
	}

	private static void RenderAxisRange(IStiAxisRange axisRange, StiChartAxis elementAxis)
	{
		if (elementAxis.GetType() == typeof(StiXChartAxis) || elementAxis.GetType() == typeof(StiYChartAxis))
		{
			axisRange.Auto = elementAxis.Range.Auto;
			axisRange.Maximum = elementAxis.Range.Maximum;
			axisRange.Minimum = elementAxis.Range.Minimum;
		}
	}

	private static void RenderAxisLabelsTitleColor(StiChartElement element, IStiAxisTitle title, StiChartAxisTitle chartTitle, Color styledColor)
	{
		if (chartTitle.Color != Color.Transparent)
		{
			title.Color = chartTitle.Color;
		}
		else if (element.Style != StiElementStyleIdent.Custom || title.Color == Color.Transparent)
		{
			title.Color = styledColor;
		}
	}

	private static void RenderAxisLabelsColor(StiChartElement element, StiAxis chartAxis, StiChartAxis elementAxis, Color styledColor)
	{
		if (elementAxis.Labels.Color != Color.Transparent)
		{
			chartAxis.Labels.Color = elementAxis.Labels.Color;
		}
		else if (element.Style != StiElementStyleIdent.Custom || chartAxis.Labels.Color == Color.Transparent)
		{
			chartAxis.Labels.Color = styledColor;
		}
	}

	private static void RenderRadarXAxisLabelsColor(StiChartElement element, IStiRadarAxisLabels chartAxisLabels, StiChartAxis elementAxis, Color styledColor)
	{
		if (elementAxis.Labels.Color != Color.Transparent)
		{
			chartAxisLabels.Color = elementAxis.Labels.Color;
		}
		else if (element.Style != StiElementStyleIdent.Custom || chartAxisLabels.Color == Color.Transparent)
		{
			chartAxisLabels.Color = styledColor;
		}
	}

	private static void RenderRadarYAxisLabelsColor(StiChartElement element, IStiAxisLabels chartAxisLabels, StiChartAxis elementAxis, Color styledColor)
	{
		if (elementAxis.Labels.Color != Color.Transparent)
		{
			chartAxisLabels.Color = elementAxis.Labels.Color;
		}
		else if (element.Style != StiElementStyleIdent.Custom || chartAxisLabels.Color == Color.Transparent)
		{
			chartAxisLabels.Color = styledColor;
		}
	}

	private static StiChartAxisTitle GetTitleAxisChart(StiChartAxis elementAxis)
	{
		if (elementAxis is StiXChartAxis)
		{
			return (elementAxis as StiXChartAxis).Title;
		}
		if (elementAxis is StiYChartAxis)
		{
			return (elementAxis as StiYChartAxis).Title;
		}
		return null;
	}

	private void RenderLegend(StiChartElement element, StiChart chart)
	{
		chart.Legend.HorAlignment = element.Legend.HorAlignment;
		chart.Legend.VertAlignment = element.Legend.VertAlignment;
		chart.Legend.Title = StiReportParser.Parse(element.Legend.Title.Text, element);
		chart.Legend.TitleFont = element.Legend.Title.Font;
		chart.Legend.AllowApplyStyle = false;
		chart.Legend.Font = element.Legend.Labels.Font;
		chart.Legend.Visible = GetChartLegendVisible(element, chart);
		chart.Legend.Columns = element.Legend.Columns;
		chart.Legend.Direction = element.Legend.Direction;
		Color foreColor = StiDashboardStyleHelper.GetForeColor(element);
		RenderLegendTitleColor(element, chart, foreColor);
		RenderLegendLabelsColor(element, chart, foreColor);
	}

	private static bool GetChartLegendVisible(StiChartElement element, StiChart chart)
	{
		if (chart.Series.Count == 1 && !(chart.Series[0] is StiPieSeries))
		{
			return element.Legend.Visibility == StiLegendVisibility.Always;
		}
		return element.Legend.Visibility != StiLegendVisibility.False;
	}

	private static void RenderLegendTitleColor(StiChartElement element, StiChart chart, Color styledColor)
	{
		if (element.Legend.Title.Color != Color.Transparent)
		{
			chart.Legend.TitleColor = element.Legend.Title.Color;
		}
		else if (element.Style != StiElementStyleIdent.Custom || chart.Legend.TitleColor == Color.Transparent)
		{
			chart.Legend.TitleColor = styledColor;
		}
	}

	private static void RenderSeriesLabelsLegendValueType(StiChartElement element, StiChart chart)
	{
		int argumentsCount = 0;
		(from StiSeries s in chart.Series
			where s.Arguments != null
			select s).ToList().ForEach(delegate(StiSeries s)
		{
			argumentsCount += s.Arguments.Count((object a) => a != null);
		});
		if (!element.Legend.Labels.ValueType.HasValue)
		{
			if (argumentsCount > 0 && chart.Series.Count > 1)
			{
				chart.SeriesLabels.LegendValueType = StiSeriesLabelsValueType.SeriesTitleArgument;
			}
			else if (argumentsCount == 1 && chart.Series.Count == 1 && (element.IsPieChart || element.IsPie3dChart || element.IsDoughnutChart))
			{
				chart.SeriesLabels.LegendValueType = StiSeriesLabelsValueType.SeriesTitle;
			}
			else if (argumentsCount > 0 && chart.Series.Count == 1)
			{
				chart.SeriesLabels.LegendValueType = StiSeriesLabelsValueType.Argument;
			}
			else if (argumentsCount == 0 && chart.Series.Count > 1)
			{
				chart.SeriesLabels.LegendValueType = StiSeriesLabelsValueType.SeriesTitle;
			}
			else
			{
				chart.SeriesLabels.LegendValueType = (element.IsBubbleChart ? StiSeriesLabelsValueType.Weight : StiSeriesLabelsValueType.Value);
			}
		}
		else
		{
			chart.SeriesLabels.LegendValueType = element.Legend.Labels.ValueType.GetValueOrDefault();
		}
	}

	private static void RenderLegendLabelsColor(StiChartElement element, StiChart chart, Color styledColor)
	{
		if (element.Legend.Labels.Color != Color.Transparent)
		{
			chart.Legend.LabelsColor = element.Legend.Labels.Color;
		}
		else if (element.Style != StiElementStyleIdent.Custom || chart.Legend.LabelsColor == Color.Transparent)
		{
			chart.Legend.LabelsColor = styledColor;
		}
	}

	protected static void RenderConditions(string seriesKey, StiSeries series, StiChartElement element, StiMeter valueMeter)
	{
		List<StiChartElementCondition> list = element.ChartConditions?.Where((StiChartElementCondition x) => x.KeyValueMeter == valueMeter.Key).ToList();
		if (list != null)
		{
			RenderValueConditions(element, series, list);
		}
		List<StiChartElementCondition> list2 = element.ChartConditions?.Where((StiChartElementCondition x) => x.Field == StiChartConditionalField.Argument).ToList();
		if (list2 != null)
		{
			RenderArgumentConditions(element, series, list2);
		}
		List<StiChartElementCondition> list3 = element.ChartConditions?.Where((StiChartElementCondition x) => x.Field == StiChartConditionalField.Series).ToList();
		if (list3 != null)
		{
			RenderSeriesConditions(element, series, list3, seriesKey);
		}
	}

	private static void RenderValueConditions(StiChartElement element, StiSeries series, List<StiChartElementCondition> conditions)
	{
		foreach (StiChartElementCondition condition in conditions)
		{
			string value = (condition.IsExpression ? StiReportParser.Parse("{" + condition.Value + "}", element) : condition.Value);
			series.Conditions.Add(new StiChartCondition(condition.Color, Stimulsoft.Report.Components.StiFilterItem.Value, condition.DataType, condition.Condition, value, condition.MarkerType, condition.MarkerAngle));
		}
	}

	private static void RenderArgumentConditions(StiChartElement element, StiSeries series, List<StiChartElementCondition> conditions)
	{
		foreach (StiChartElementCondition condition in conditions)
		{
			string value = (condition.IsExpression ? StiReportParser.Parse("{" + condition.Value + "}", element) : condition.Value);
			series.Conditions.Add(new StiChartCondition(condition.Color, Stimulsoft.Report.Components.StiFilterItem.Argument, condition.DataType, condition.Condition, value, condition.MarkerType, condition.MarkerAngle));
		}
	}

	private static void RenderSeriesConditions(StiChartElement element, StiSeries series, List<StiChartElementCondition> conditions, string seriesKey)
	{
		foreach (StiChartElementCondition condition in conditions)
		{
			if (GetConditionResult((condition.IsExpression ? StiReportParser.Parse("{" + condition.Value + "}", element) : condition.Value)?.ToString(), seriesKey, condition.Condition))
			{
				series.ColorLegend = condition.Color;
				series.Conditions.Add(new StiChartCondition(condition.Color, Stimulsoft.Report.Components.StiFilterItem.Argument, Stimulsoft.Report.Components.StiFilterDataType.String, Stimulsoft.Report.Components.StiFilterCondition.BeginningWith, "", condition.MarkerType, condition.MarkerAngle));
			}
		}
	}

	private static bool GetConditionResult(string conditionValue, string iterationData, Stimulsoft.Report.Components.StiFilterCondition condition)
	{
		if (iterationData == null)
		{
			return false;
		}
		IComparable comparable = iterationData;
		if (comparable == null || conditionValue == null)
		{
			return false;
		}
		if (iterationData.GetType() != conditionValue.GetType())
		{
			iterationData = iterationData.ToString();
			conditionValue = conditionValue.ToString();
			comparable = iterationData;
		}
		try
		{
			return condition switch
			{
				Stimulsoft.Report.Components.StiFilterCondition.EqualTo => comparable.CompareTo(conditionValue) == 0, 
				Stimulsoft.Report.Components.StiFilterCondition.NotEqualTo => comparable.CompareTo(conditionValue) != 0, 
				Stimulsoft.Report.Components.StiFilterCondition.GreaterThan => comparable.CompareTo(conditionValue) == 1, 
				Stimulsoft.Report.Components.StiFilterCondition.GreaterThanOrEqualTo => comparable.CompareTo(conditionValue) >= 0, 
				Stimulsoft.Report.Components.StiFilterCondition.LessThan => comparable.CompareTo(conditionValue) == -1, 
				Stimulsoft.Report.Components.StiFilterCondition.LessThanOrEqualTo => comparable.CompareTo(conditionValue) <= 0, 
				Stimulsoft.Report.Components.StiFilterCondition.Containing => iterationData.ToString().IndexOfInvariant(conditionValue.ToString()) != -1, 
				Stimulsoft.Report.Components.StiFilterCondition.NotContaining => iterationData.ToString().IndexOfInvariant(conditionValue.ToString()) == -1, 
				Stimulsoft.Report.Components.StiFilterCondition.BeginningWith => iterationData.ToString().StartsWithInvariant(conditionValue.ToString()), 
				Stimulsoft.Report.Components.StiFilterCondition.EndingWith => iterationData.ToString().EndsWithInvariant(conditionValue.ToString()), 
				_ => false, 
			};
		}
		catch
		{
			return false;
		}
	}

	private static void RenderConstantLines(StiChartElement element, StiChart chart)
	{
		foreach (StiChartConstantLines constantLine in element.ConstantLines)
		{
			bool titleVisible = !string.IsNullOrEmpty(constantLine.Text);
			chart.ConstantLines.Add(new StiConstantLines
			{
				AllowApplyStyle = false,
				Text = StiReportParser.Parse(constantLine.Text, element),
				AxisValue = StiReportParser.Parse(constantLine.AxisValue, element),
				LineColor = constantLine.LineColor,
				LineStyle = constantLine.LineStyle,
				LineWidth = constantLine.LineWidth,
				TitleVisible = titleVisible,
				Orientation = (element.IsBarChart ? StiConstantLines.StiOrientation.Vertical : StiConstantLines.StiOrientation.Horizontal),
				Position = GetChartPosition(constantLine.Position)
			});
		}
	}

	private static StiConstantLines.StiTextPosition GetChartPosition(StiTextPosition position)
	{
		return position switch
		{
			StiTextPosition.LeftTop => StiConstantLines.StiTextPosition.LeftTop, 
			StiTextPosition.LeftBottom => StiConstantLines.StiTextPosition.LeftBottom, 
			StiTextPosition.CenterTop => StiConstantLines.StiTextPosition.CenterTop, 
			StiTextPosition.CenterBottom => StiConstantLines.StiTextPosition.CenterBottom, 
			StiTextPosition.RightTop => StiConstantLines.StiTextPosition.RightTop, 
			StiTextPosition.RightBottom => StiConstantLines.StiTextPosition.RightBottom, 
			_ => StiConstantLines.StiTextPosition.LeftTop, 
		};
	}

	protected static void RenderTrendLines(StiSeries series, StiChartElement element, StiMeter valueMeter)
	{
		List<StiChartTrendLine> list = element.TrendLines?.Where((StiChartTrendLine x) => x.KeyValueMeter == valueMeter.Key).ToList();
		series.TrendLines.Clear();
		foreach (StiChartTrendLine item in list)
		{
			series.TrendLines.Add(GetTrendLine(item, series.Chart) as StiTrendLine);
		}
	}

	private static IStiTrendLine GetTrendLine(StiChartTrendLine trendLine, IStiChart chart)
	{
		return trendLine.Type switch
		{
			StiChartTrendLineType.None => new StiTrendLineNone(), 
			StiChartTrendLineType.Exponential => new StiTrendLineExponential
			{
				AllowApplyStyle = false,
				LineColor = ((trendLine.LineColor != Color.Transparent) ? trendLine.LineColor : chart.Style.Core.TrendLineColor),
				LineStyle = trendLine.LineStyle,
				LineWidth = trendLine.LineWidth,
				ShowShadow = false
			}, 
			StiChartTrendLineType.Linear => new StiTrendLineLinear
			{
				AllowApplyStyle = false,
				LineColor = ((trendLine.LineColor != Color.Transparent) ? trendLine.LineColor : chart.Style.Core.TrendLineColor),
				LineStyle = trendLine.LineStyle,
				LineWidth = trendLine.LineWidth,
				ShowShadow = false
			}, 
			StiChartTrendLineType.Logarithmic => new StiTrendLineLogarithmic
			{
				AllowApplyStyle = false,
				LineColor = ((trendLine.LineColor != Color.Transparent) ? trendLine.LineColor : chart.Style.Core.TrendLineColor),
				LineStyle = trendLine.LineStyle,
				LineWidth = trendLine.LineWidth,
				ShowShadow = false
			}, 
			_ => new StiTrendLineNone(), 
		};
	}

	private void RenderSeriesLabels(StiChartElement element, StiChart chart)
	{
		if (!element.IsPieChart && !element.IsPie3dChart && !element.IsFunnelChart && !element.IsDoughnutChart && !element.IsSunburstChart && !element.IsWaterfallChart && !element.IsTreemapChart && !element.IsRadarChart && !element.IsAxisAreaChart)
		{
			chart.SeriesLabels.Visible = false;
			return;
		}
		chart.AllowApplyStyle = false;
		if (element.IsPieChart)
		{
			RenderPieLabelsPosition(element, chart);
		}
		else if (element.IsPie3dChart)
		{
			RenderPie3dLabelsPosition(element, chart);
		}
		else if (element.IsDoughnutChart || element.IsSunburstChart)
		{
			RenderDoughnutLabelsPosition(element, chart);
		}
		else if (element.IsFunnelChart)
		{
			RenderFunnelLabelsPosition(element, chart);
		}
		else if (element.IsTreemapChart)
		{
			RenderTreemapLabelsPosition(element, chart);
		}
		else if (element.IsStackedChart)
		{
			RenderStackedLabelsPosition(element, chart);
		}
		else if (element.IsRadarChart)
		{
			RenderRadarLabelsPosition(element, chart);
		}
		else if (element.IsWaterfallChart)
		{
			RenderWaterfallLabelsPosition(element, chart);
		}
		else if (element.IsAxisAreaChart)
		{
			RenderAxisLabelsPosition(element, chart);
		}
		chart.SeriesLabels.Core.ApplyStyle(chart.Style);
		ApplyPropertiesToSeriesLabels(element, chart.SeriesLabels as StiSeriesLabels);
		if (element.IsStackedChart)
		{
			chart.Series.ToList().ForEach(delegate(StiSeries s)
			{
				ApplyPropertiesToSeriesLabels(element, s.SeriesLabels as StiSeriesLabels);
			});
		}
		if (element.IsPieChart || element.IsPie3dChart || element.IsDoughnutChart || element.IsSunburstChart)
		{
			RenderPieLabelsStyle(element, chart);
			RenderPieLabelsAutoRotate(element, chart);
			RenderSeriesLabelsLegendValueType(element, chart);
		}
		else if (element.IsFullStackedChart)
		{
			RenderLabelsStyle(element, chart);
			((IStiAxisSeriesLabels)chart.SeriesLabels).ShowInPercent = true;
		}
		else
		{
			RenderLabelsStyle(element, chart);
		}
		RenderLabelsColor(element, chart);
		RenderLabelsTextFormat(element, chart);
	}

	private static void ApplyPropertiesToSeriesLabels(StiChartElement element, StiSeriesLabels labels)
	{
		labels.PreventIntersection = true;
		labels.TextAfter = StiReportParser.Parse(element.Labels.TextAfter, element);
		labels.TextBefore = StiReportParser.Parse(element.Labels.TextBefore, element);
		labels.Font = element.Labels.Font;
		labels.ShowNulls = false;
		labels.ShowZeros = false;
	}

	private static void RenderPieLabelsAutoRotate(StiChartElement element, StiChart chart)
	{
		if (chart.SeriesLabels is IStiCenterPieLabels stiCenterPieLabels)
		{
			stiCenterPieLabels.AutoRotate = element.Labels.AutoRotate;
		}
	}

	private static void RenderLabelsColor(StiChartElement element, StiChart chart)
	{
		StiSeriesLabels stiSeriesLabels = chart.SeriesLabels as StiSeriesLabels;
		IStiOutsidePieLabels stiOutsidePieLabels = chart.SeriesLabels as IStiOutsidePieLabels;
		if (element.Labels.ForeColor != Color.Transparent)
		{
			stiSeriesLabels.LabelColor = element.Labels.ForeColor;
			if (stiOutsidePieLabels != null)
			{
				stiOutsidePieLabels.LineColor = element.Labels.ForeColor;
			}
		}
		else
		{
			stiSeriesLabels.LabelColor = (StiDashboardStyleHelper.IsDarkStyle(element.Style) ? Color.FromArgb(50, 50, 50) : ((chart?.Style?.Core != null) ? chart.Style.Core.SeriesLabelsColor : Color.Gray));
			if (stiOutsidePieLabels != null)
			{
				stiOutsidePieLabels.LineColor = ((chart?.Style?.Core != null) ? chart.Style.Core.SeriesLabelsLineColor : Color.Gray);
			}
		}
	}

	private static void RenderLabelsTextFormat(StiChartElement element, StiChart chart)
	{
		chart.SeriesLabels.FormatService = element.ValueFormat;
	}

	private static void RenderFunnelLabelsPosition(StiChartElement element, StiChart chart)
	{
		switch (element.Labels.FunnelPosition)
		{
		case StiChartLabelsPosition.None:
			chart.SeriesLabels.Visible = false;
			break;
		case StiChartLabelsPosition.Left:
			chart.SeriesLabels = new StiOutsideLeftFunnelLabels();
			break;
		case StiChartLabelsPosition.Center:
			chart.SeriesLabels = new StiCenterFunnelLabels();
			break;
		case StiChartLabelsPosition.Right:
			chart.SeriesLabels = new StiOutsideRightFunnelLabels();
			break;
		default:
			chart.SeriesLabels.Visible = false;
			break;
		}
	}

	private static void RenderTreemapLabelsPosition(StiChartElement element, StiChart chart)
	{
		switch (element.Labels.TreemapPosition)
		{
		case StiChartLabelsPosition.None:
			chart.SeriesLabels.Visible = false;
			break;
		case StiChartLabelsPosition.Center:
			chart.SeriesLabels = new StiCenterTreemapLabels();
			break;
		default:
			chart.SeriesLabels.Visible = false;
			break;
		}
	}

	private static void RenderPieLabelsPosition(StiChartElement element, StiChart chart)
	{
		switch (element.Labels.PiePosition)
		{
		case StiChartLabelsPosition.None:
			chart.SeriesLabels.Visible = false;
			break;
		case StiChartLabelsPosition.Center:
			chart.SeriesLabels = new StiCenterPieLabels();
			break;
		case StiChartLabelsPosition.InsideEnd:
			chart.SeriesLabels = new StiInsideEndPieLabels();
			break;
		case StiChartLabelsPosition.Outside:
			chart.SeriesLabels = new StiOutsidePieLabels();
			break;
		case StiChartLabelsPosition.TwoColumns:
			chart.SeriesLabels = new StiTwoColumnsPieLabels();
			break;
		default:
			chart.SeriesLabels.Visible = false;
			break;
		}
	}

	private static void RenderPie3dLabelsPosition(StiChartElement element, StiChart chart)
	{
		switch (element.Labels.Pie3dPosition)
		{
		case StiChartLabelsPosition.None:
			chart.SeriesLabels.Visible = false;
			break;
		case StiChartLabelsPosition.Center:
			chart.SeriesLabels = new StiCenterPie3dLabels();
			break;
		default:
			chart.SeriesLabels.Visible = false;
			break;
		}
	}

	private static void RenderStackedLabelsPosition(StiChartElement element, StiChart chart)
	{
		StiChartLabelsPosition axisPosition = element.Labels.AxisPosition;
		if (axisPosition == StiChartLabelsPosition.Total)
		{
			StiSeries mainSeries = chart.Series.ToList().LastOrDefault();
			(from s in chart.Series.ToList()
				where s != mainSeries
				select s).ToList().ForEach(delegate(StiSeries c)
			{
				c.ShowSeriesLabels = StiShowSeriesLabels.FromSeries;
				c.SeriesLabels.Visible = false;
			});
			mainSeries.ShowSeriesLabels = StiShowSeriesLabels.FromSeries;
			mainSeries.SeriesLabels = new StiValueAxisLabels();
			mainSeries.IsTotalLabel = true;
			mainSeries.SeriesLabels.FormatService = element.ValueFormat;
		}
		else
		{
			RenderAxisLabelsPosition(element, chart);
		}
	}

	private void RenderRadarLabelsPosition(StiChartElement element, StiChart chart)
	{
		switch (element.Labels.RadarPosition)
		{
		case StiChartLabelsPosition.None:
			chart.SeriesLabels.Visible = false;
			break;
		case StiChartLabelsPosition.Center:
			chart.SeriesLabels = new StiCenterAxisLabels();
			break;
		default:
			chart.SeriesLabels.Visible = false;
			break;
		}
	}

	private static void RenderWaterfallLabelsPosition(StiChartElement element, StiChart chart)
	{
		switch (element.Labels.AxisPosition)
		{
		case StiChartLabelsPosition.None:
			chart.SeriesLabels.Visible = false;
			break;
		case StiChartLabelsPosition.Center:
			chart.SeriesLabels = new StiCenterAxisLabels();
			break;
		case StiChartLabelsPosition.InsideBase:
			chart.SeriesLabels = new StiInsideBaseAxisLabels();
			break;
		case StiChartLabelsPosition.InsideEnd:
			chart.SeriesLabels = new StiInsideEndAxisLabels();
			break;
		case StiChartLabelsPosition.Outside:
			chart.SeriesLabels = new StiOutsideAxisLabels();
			break;
		case StiChartLabelsPosition.OutsideBase:
			chart.SeriesLabels = new StiOutsideBaseAxisLabels();
			break;
		case StiChartLabelsPosition.OutsideEnd:
			chart.SeriesLabels = new StiOutsideEndAxisLabels();
			break;
		case StiChartLabelsPosition.Value:
			chart.SeriesLabels = new StiValueAxisLabels();
			break;
		default:
			chart.SeriesLabels.Visible = false;
			break;
		}
	}

	private static void RenderAxisLabelsPosition(StiChartElement element, StiChart chart)
	{
		switch (element.Labels.AxisPosition)
		{
		case StiChartLabelsPosition.None:
			chart.SeriesLabels.Visible = false;
			break;
		case StiChartLabelsPosition.Center:
			chart.SeriesLabels = new StiCenterAxisLabels();
			break;
		case StiChartLabelsPosition.InsideBase:
			chart.SeriesLabels = new StiInsideBaseAxisLabels();
			break;
		case StiChartLabelsPosition.InsideEnd:
			chart.SeriesLabels = new StiInsideEndAxisLabels();
			break;
		case StiChartLabelsPosition.Left:
			chart.SeriesLabels = new StiLeftAxisLabels();
			break;
		case StiChartLabelsPosition.Outside:
			chart.SeriesLabels = new StiOutsideAxisLabels();
			break;
		case StiChartLabelsPosition.OutsideBase:
			chart.SeriesLabels = new StiOutsideBaseAxisLabels();
			break;
		case StiChartLabelsPosition.OutsideEnd:
			chart.SeriesLabels = new StiOutsideEndAxisLabels();
			break;
		case StiChartLabelsPosition.Right:
			chart.SeriesLabels = new StiRightAxisLabels();
			break;
		case StiChartLabelsPosition.Value:
			chart.SeriesLabels = new StiValueAxisLabels();
			break;
		default:
			chart.SeriesLabels.Visible = false;
			break;
		}
	}

	private static void RenderDoughnutLabelsPosition(StiChartElement element, StiChart chart)
	{
		switch (element.Labels.DoughnutPosition)
		{
		case StiChartLabelsPosition.None:
			chart.SeriesLabels.Visible = false;
			break;
		case StiChartLabelsPosition.Center:
			chart.SeriesLabels = new StiCenterPieLabels();
			break;
		default:
			chart.SeriesLabels.Visible = false;
			break;
		}
	}

	private static void RenderPieLabelsStyle(StiChartElement element, StiChart chart)
	{
		IStiPieSeriesLabels stiPieSeriesLabels = chart.SeriesLabels as IStiPieSeriesLabels;
		switch (element.Labels.Style)
		{
		case StiChartLabelsStyle.PercentOfTotal:
			stiPieSeriesLabels.ShowInPercent = true;
			stiPieSeriesLabels.ValueType = StiSeriesLabelsValueType.Value;
			break;
		case StiChartLabelsStyle.Category:
			stiPieSeriesLabels.ShowInPercent = false;
			stiPieSeriesLabels.ValueType = ((!element.Arguments.Any()) ? StiSeriesLabelsValueType.SeriesTitle : StiSeriesLabelsValueType.Argument);
			break;
		case StiChartLabelsStyle.CategoryPercentOfTotal:
			stiPieSeriesLabels.ShowInPercent = true;
			stiPieSeriesLabels.ValueType = (element.Arguments.Any() ? StiSeriesLabelsValueType.ArgumentValue : StiSeriesLabelsValueType.SeriesTitleValue);
			break;
		case StiChartLabelsStyle.CategoryValue:
			stiPieSeriesLabels.ShowInPercent = false;
			stiPieSeriesLabels.ValueType = (element.Arguments.Any() ? StiSeriesLabelsValueType.ArgumentValue : StiSeriesLabelsValueType.SeriesTitleValue);
			break;
		case StiChartLabelsStyle.Value:
			stiPieSeriesLabels.ShowInPercent = false;
			stiPieSeriesLabels.ValueType = StiSeriesLabelsValueType.Value;
			break;
		}
	}

	private static void RenderLabelsStyle(StiChartElement element, StiChart chart)
	{
		IStiSeriesLabels seriesLabels = chart.SeriesLabels;
		switch (element.Labels.Style)
		{
		case StiChartLabelsStyle.PercentOfTotal:
			seriesLabels.ValueType = StiSeriesLabelsValueType.Value;
			break;
		case StiChartLabelsStyle.Category:
			seriesLabels.ValueType = ((!element.Arguments.Any()) ? StiSeriesLabelsValueType.SeriesTitle : StiSeriesLabelsValueType.Argument);
			break;
		case StiChartLabelsStyle.CategoryPercentOfTotal:
			seriesLabels.ValueType = (element.Arguments.Any() ? StiSeriesLabelsValueType.ArgumentValue : StiSeriesLabelsValueType.SeriesTitleValue);
			break;
		case StiChartLabelsStyle.CategoryValue:
			seriesLabels.ValueType = (element.Arguments.Any() ? StiSeriesLabelsValueType.ArgumentValue : StiSeriesLabelsValueType.SeriesTitleValue);
			break;
		case StiChartLabelsStyle.Value:
			seriesLabels.ValueType = StiSeriesLabelsValueType.Value;
			break;
		}
	}

	protected virtual void RenderSeriesInteraction(StiSeries series, StiChartElement element, int count, string seriesKey)
	{
		if (series.Chart.IsDesigning)
		{
			return;
		}
		StiReport report = element?.Report;
		int num = 0;
		StiChartDashboardInteraction stiChartDashboardInteraction = element.DashboardInteraction as StiChartDashboardInteraction;
		if (stiChartDashboardInteraction.OnHover == StiInteractionOnHover.ShowToolTip)
		{
			series.ToolTips = new string[count];
			object[] arguments = series.Arguments;
			foreach (object value in arguments)
			{
				string value2 = string.Empty;
				if (num < series.Values.Length)
				{
					value2 = series.Values[num].ToString();
				}
				if (num < series.ToolTips.Length)
				{
					series.ToolTips[num] = GetToolTip(element, (element.DashboardInteraction as StiChartDashboardInteraction).ToolTip, GetString(element.ArgumentFormat, value, report), GetString(element.ValueFormat, value2, report), seriesKey);
				}
				num++;
			}
		}
		num = 0;
		series.Hyperlinks = new string[count];
		object[] arguments2 = series.Arguments;
		foreach (object obj in arguments2)
		{
			string value3 = string.Empty;
			if (num < series.Values.Length)
			{
				value3 = series.Values[num].ToString();
			}
			if (num < series.Hyperlinks.Length)
			{
				series.Hyperlinks[num] = GetHyperlink(element, (element.DashboardInteraction as StiChartDashboardInteraction).Hyperlink, obj?.ToString(), value3, series.TitleValue);
			}
			num++;
		}
		series.DrillDownEnabled = true;
	}

	private string GetToolTip(StiChartElement element, string toolTip, string argument, string value, string series)
	{
		if (argument == null)
		{
			argument = "";
		}
		if (series == null)
		{
			series = "";
		}
		if (string.IsNullOrWhiteSpace(toolTip))
		{
			if (argument != null && !string.IsNullOrEmpty(argument))
			{
				argument = argument.Replace("\"", "\\\"") ?? "";
			}
			string format = "{{\"StiColor\":\"{0}\",\"StiValue\":\"{1}\",\"StiArgument\":\"{2}\"}}";
			return string.Format(format, "#ffffff", value, argument);
		}
		return GetHyperlink(element, toolTip, argument, value, series);
	}

	private string GetHyperlink(StiChartElement element, string hyperlink, string argument, string value, string series)
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
			{ "Series", series }
		};
		return StiReportParser.Parse(hyperlink, element, allowCache: false, constants);
	}

	public string GetTitle(StiMeter meter)
	{
		return StiLabelHelper.GetLabel(meter);
	}

	private IEnumerable<string> GetAxisTitles(StiChartElement element, StiChart chart)
	{
		return ((!(chart.Area is IStiGanttArea) && !(chart.Area is IStiClusteredBarArea)) || element.DataMode == StiDataMode.ManuallyEnteringData) ? element.ChartValuesList().Select(GetTitle).Distinct() : element.Arguments.Select(GetTitle).Distinct();
	}

	protected string GetSeriesTitle(StiChartElement element, string seriesKey, StiMeter meter)
	{
		if (element.Values.Count < 2)
		{
			return seriesKey;
		}
		string label = StiLabelHelper.GetLabel(meter);
		string text = ((!string.IsNullOrWhiteSpace(label)) ? label : null);
		if (text == null)
		{
			return seriesKey;
		}
		if (!string.IsNullOrWhiteSpace(seriesKey))
		{
			return seriesKey + " - " + text;
		}
		return text;
	}

	protected int GetArgumentIndex(StiDataTable dataTable, int index)
	{
		int[] argumentMeterIndexes = GetArgumentMeterIndexes(dataTable);
		if (argumentMeterIndexes == null || !argumentMeterIndexes.Any())
		{
			return -1;
		}
		if (argumentMeterIndexes.Length == 1)
		{
			return argumentMeterIndexes.FirstOrDefault();
		}
		if (argumentMeterIndexes.Length > index)
		{
			return (int)argumentMeterIndexes.GetValue(index);
		}
		return -1;
	}

	private void SimplifyValues(StiChart chart)
	{
		for (int i = 0; i < chart.Series.Count; i++)
		{
			StiSeries stiSeries = chart.Series[i] as StiSeries;
			if (stiSeries.Values.Length > 800)
			{
				List<PointD> shorterListPoints = GetShorterListPoints(stiSeries);
				stiSeries.Values = new double?[shorterListPoints.Count];
				stiSeries.Arguments = new object[shorterListPoints.Count];
				for (int j = 0; j < shorterListPoints.Count; j++)
				{
					stiSeries.Values[j] = shorterListPoints[j].X;
					stiSeries.Arguments[j] = shorterListPoints[j].Y;
				}
			}
		}
	}

	internal List<PointD> GetShorterListPoints(StiSeries series)
	{
		PointD[] array = new PointD[series.Values.Length];
		for (int i = 0; i < series.Values.Length; i++)
		{
			array[i] = new PointD(series.Values[i].GetValueOrDefault(), i);
		}
		double[] array2 = new double[10] { 0.3, 0.6, 1.0, 5.0, 9.0, 15.0, 20.0, 25.0, 30.0, 40.0 };
		List<PointD> list = null;
		List<PointD> list2 = null;
		for (int j = 0; j < array2.Length; j++)
		{
			list = StiSimplifyHelper.Simplify(array, j, highestQuality: true);
			if ((double)list.Count / (double)array.Length < 0.02 && list.Count < 900 && list2 != null)
			{
				list = list2;
				break;
			}
			list2 = list;
		}
		return list;
	}

	protected string GetString(StiFormatService format, object value, StiReport report)
	{
		if (value == null)
		{
			return "";
		}
		if (value is object[] source)
		{
			return string.Join(" - ", source.Select((object v) => GetFormatValue(format, v, report)));
		}
		return GetFormatValue(format, value, report);
	}

	private string GetFormatValue(StiFormatService format, object value, StiReport report)
	{
		if (value == null)
		{
			return null;
		}
		if (value is DateTime)
		{
			if (format is StiDateFormatService || format is StiTimeFormatService || format is StiCustomFormatService)
			{
				return format.Format(value);
			}
			return ((DateTime)value).ToShortDateString();
		}
		if (StiExtValueConverter.IsEligable(value, report?.Culture))
		{
			return StiExtValueConverter.Convert(value, report?.Culture);
		}
		if (format != null)
		{
			return format.Format(value);
		}
		return value.ToString();
	}

	public static StiChartElementBuilder Create(StiChartElement chart)
	{
		if (chart.IsSunburstChart)
		{
			return new StiSunburstChartElementBuilder();
		}
		if (chart.IsBubbleChart)
		{
			return new StiBubbleChartElementBuilder();
		}
		if (chart.IsRange)
		{
			return new StiRangeChartElementBuilder();
		}
		if (chart.IsFinancial)
		{
			return new StiFinancialChartElementBuilder();
		}
		if (chart.IsFullStackedChart || chart.IsStackedChart)
		{
			return new StiStackedChartElementBuilder();
		}
		return new StiChartElementBuilder();
	}

	protected virtual int[] GetValueMeterIndexes(StiDataTable table)
	{
		if (table == null)
		{
			return null;
		}
		return (from r in GetValueMeters(table)
			select table.Meters.IndexOf(r)).ToArray();
	}

	protected virtual int[] GetArgumentMeterIndexes(StiDataTable table)
	{
		if (table == null)
		{
			return null;
		}
		return (from r in GetArgumentMeters(table)
			select table.Meters.IndexOf(r)).ToArray();
	}

	protected virtual int GetSeriesMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiSeriesChartMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	protected virtual IEnumerable<object> GetArgumentKeys(IStiElement element, StiDataTable table)
	{
		if (table == null)
		{
			return null;
		}
		IEnumerable<StiMeter> argumentMeters = GetArgumentMeters(table);
		List<int> indexes = argumentMeters.Select((StiMeter f) => table.Meters.IndexOf(f)).ToList();
		IEnumerable<IGrouping<object[], object[]>> source = table.Rows.GroupBy((object[] row) => indexes.Select((int i) => row[i]).ToArray(), new StiArrayEqualityComparer());
		return source.Select((IGrouping<object[], object[]> row) => row.Key.ToArray());
	}

	protected virtual IEnumerable<StiMeter> GetValueMeters(StiDataTable table)
	{
		return table?.Meters.Where((IStiMeter m) => m is StiValueChartMeter).Cast<StiValueChartMeter>();
	}

	protected virtual IEnumerable<StiMeter> GetArgumentMeters(StiDataTable table)
	{
		return table?.Meters.Where((IStiMeter m) => m is StiArgumentChartMeter).Cast<StiArgumentChartMeter>();
	}
}
