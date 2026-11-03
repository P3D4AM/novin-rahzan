using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Chart;

public class StiChartHelper
{
	private delegate void InvokeGetValues(StiChart chart, StiGetValueEventArgs eg);

	public static TimeSpan GlobalDurationElement { get; set; } = new TimeSpan(0, 0, 0, 0, 800);


	public static TimeSpan GlobalBeginTimeElement { get; set; } = new TimeSpan(0, 0, 0, 0, 800);


	internal static void FillSeriesData(StiSeries series, List<StiDataItem> items)
	{
		series.Values = new double?[items.Count];
		series.Arguments = new object[items.Count];
		series.Tags = new object[items.Count];
		bool flag = items.Any((StiDataItem i) => i.ToolTip != null && !string.IsNullOrEmpty(i.ToolTip.ToString()));
		if (flag)
		{
			series.ToolTips = new string[items.Count];
		}
		for (int j = 0; j < items.Count; j++)
		{
			StiDataItem stiDataItem = items[j];
			series.Values[j] = ((stiDataItem.Value is double) ? ((double)stiDataItem.Value) : 0.0);
			series.Arguments[j] = stiDataItem.Argument;
			if (flag)
			{
				series.ToolTips[j] = stiDataItem.ToolTip.ToString();
			}
			series.Tags[j] = stiDataItem.Tag;
			if (series is IStiRangeSeries)
			{
				((IStiRangeSeries)series).ValuesEnd[j] = ((stiDataItem.ValueEnd is double) ? ((double)stiDataItem.ValueEnd) : 0.0);
			}
			if (series is IStiFinancialSeries)
			{
				((IStiFinancialSeries)series).ValuesOpen[j] = ((stiDataItem.ValueOpen is double) ? ((double)stiDataItem.ValueOpen) : 0.0);
				((IStiFinancialSeries)series).ValuesClose[j] = ((stiDataItem.ValueClose is double) ? ((double)stiDataItem.ValueClose) : 0.0);
				((IStiFinancialSeries)series).ValuesLow[j] = ((stiDataItem.ValueLow is double) ? ((double)stiDataItem.ValueLow) : 0.0);
				((IStiFinancialSeries)series).ValuesHigh[j] = ((stiDataItem.ValueHigh is double) ? ((double)stiDataItem.ValueHigh) : 0.0);
			}
		}
	}

	internal static object GetFilterData(StiReport report, StiChartFilter filter, string filterMethodName)
	{
		try
		{
			if (filter.Item == StiFilterItem.Expression)
			{
				if (report.CalculationMode == StiCalculationMode.Interpretation)
				{
					StiText component = new StiText
					{
						Name = "*Chart_Filter*",
						Page = report.Pages[0]
					};
					bool storeToPrint = false;
					return StiParser.ParseTextValue("{" + filter.Value + "}", component, ref storeToPrint, executeIfStoreToPrint: true);
				}
				MethodInfo method = report.GetType().GetMethod(filterMethodName);
				return method.Invoke(report, new object[0]);
			}
			switch (filter.DataType)
			{
			case StiFilterDataType.String:
				return filter.Value;
			case StiFilterDataType.Numeric:
			{
				double result2 = 0.0;
				double.TryParse(filter.Value.Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator), out result2);
				return result2;
			}
			case StiFilterDataType.DateTime:
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us", useUserOverride: false);
				DateTime result;
				bool flag = DateTime.TryParse(filter.Value, out result);
				Thread.CurrentThread.CurrentCulture = currentCulture;
				if (flag)
				{
					return result;
				}
				break;
			}
			case StiFilterDataType.Boolean:
				return filter.Value.ToLower(CultureInfo.InvariantCulture) == "true";
			}
		}
		catch (Exception innerException)
		{
			string message = $"Problem when parsing Chart {filter} value\n";
			throw new Exception(message, innerException);
		}
		return null;
	}

	internal static bool GetFilterResult(StiChartFilter filter, object itemArgument, object itemValue, object itemValueEnd, object itemValueOpen, object itemValueClose, object itemValueLow, object itemValueHigh, object itemTooltip, object data)
	{
		if (filter.Item == StiFilterItem.Expression)
		{
			return (bool)data;
		}
		object obj = null;
		switch (filter.Item)
		{
		case StiFilterItem.Argument:
			obj = itemArgument;
			break;
		case StiFilterItem.Value:
			obj = itemValue;
			break;
		case StiFilterItem.ValueEnd:
			obj = itemValueEnd;
			break;
		case StiFilterItem.ValueOpen:
			obj = itemValueOpen;
			break;
		case StiFilterItem.ValueClose:
			obj = itemValueClose;
			break;
		case StiFilterItem.ValueLow:
			obj = itemValueLow;
			break;
		case StiFilterItem.ValueHigh:
			obj = itemValueHigh;
			break;
		case StiFilterItem.Tooltip:
			obj = itemTooltip;
			break;
		}
		bool result = false;
		if (obj != null)
		{
			try
			{
				if (filter.DataType == StiFilterDataType.Numeric)
				{
					double result2 = 0.0;
					if (double.TryParse(obj.ToString(), out result2))
					{
						obj = result2;
					}
				}
				else if (filter.DataType == StiFilterDataType.DateTime)
				{
					obj = ((!DateTime.TryParse(obj.ToString(), out var result3)) ? ((object)DateTime.FromOADate((double)obj)) : ((object)result3));
				}
			}
			catch
			{
			}
			IComparable comparable = obj as IComparable;
			if (comparable != null && data != null)
			{
				if (obj.GetType() != data.GetType())
				{
					obj = obj.ToString();
					data = data.ToString();
					comparable = obj as IComparable;
				}
				try
				{
					switch (filter.Condition)
					{
					case StiFilterCondition.EqualTo:
						return comparable.CompareTo(data) == 0;
					case StiFilterCondition.NotEqualTo:
						return comparable.CompareTo(data) != 0;
					case StiFilterCondition.GreaterThan:
						return comparable.CompareTo(data) == 1;
					case StiFilterCondition.GreaterThanOrEqualTo:
						return comparable.CompareTo(data) >= 0;
					case StiFilterCondition.LessThan:
						return comparable.CompareTo(data) == -1;
					case StiFilterCondition.LessThanOrEqualTo:
						return comparable.CompareTo(data) <= 0;
					case StiFilterCondition.Containing:
					{
						string text4 = obj.ToString();
						string value4 = data.ToString();
						return text4.IndexOf(value4, StringComparison.InvariantCulture) != -1;
					}
					case StiFilterCondition.NotContaining:
					{
						string text3 = obj.ToString();
						string value3 = data.ToString();
						return text3.IndexOf(value3, StringComparison.InvariantCulture) == -1;
					}
					case StiFilterCondition.BeginningWith:
					{
						string text2 = obj.ToString();
						string value2 = data.ToString();
						return text2.StartsWith(value2, StringComparison.InvariantCulture);
					}
					case StiFilterCondition.EndingWith:
					{
						string text = obj.ToString();
						string value = data.ToString();
						return text.EndsWith(value, StringComparison.InvariantCulture);
					}
					case StiFilterCondition.Between:
					case StiFilterCondition.NotBetween:
						break;
					}
				}
				catch
				{
				}
			}
		}
		return result;
	}

	internal static StiImage GetImageFromChart(StiChart masterChart)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		StiImage stiImage = new StiImage
		{
			Left = masterChart.Left,
			Top = masterChart.Top,
			Width = masterChart.Width,
			Height = masterChart.Height,
			Page = masterChart.Page,
			Smoothing = false,
			Stretch = true,
			Name = masterChart.Name
		};
		RectangleD paintRectangle = masterChart.GetPaintRectangle();
		((RectangleD)(ref paintRectangle)).X = 0.0;
		((RectangleD)(ref paintRectangle)).Y = 0.0;
		float zoom = StiOptions.Engine.RenderChartAsBitmapZoom;
		stiImage.PutImageToDraw(masterChart.GetImage(ref zoom));
		return stiImage;
	}

	internal static object ConvertStringToColor(string colorStr)
	{
		object result = null;
		if (colorStr != null)
		{
			colorStr = colorStr.Trim();
			char c = ',';
			for (int i = 0; i < colorStr.Length; i++)
			{
				if (!char.IsDigit(colorStr[i]) && !char.IsWhiteSpace(colorStr[i]))
				{
					c = colorStr[i];
					break;
				}
			}
			string[] array = colorStr.Split(new char[1] { c });
			if (array.Length != 3 && array.Length != 4)
			{
				try
				{
					int result2 = 0;
					if (int.TryParse(colorStr, out result2))
					{
						Color color = Color.FromArgb(result2);
						if (result2 < 16777215)
						{
							color = Color.FromArgb(255, color.R, color.G, color.B);
						}
						return color;
					}
					ColorConverter colorConverter = new ColorConverter();
					return colorConverter.ConvertFromString(colorStr);
				}
				catch
				{
					string text = "Can't get color from '" + colorStr + "'";
					StiLogService.Write("StiChartHelper.ConvertStringToColor " + text);
					throw new ArgumentException(text);
				}
			}
			try
			{
				result = ((array.Length != 3) ? ((object)Color.FromArgb(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()), int.Parse(array[3].Trim()))) : ((object)Color.FromArgb(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()))));
			}
			catch
			{
				string text2 = "Can't get color from '" + colorStr + "'";
				StiLogService.Write("StiChartHelper.ConvertStringToColor " + text2);
				throw new ArgumentException(text2);
			}
		}
		return result;
	}

	internal static void CreateChart(StiChart masterChart, StiChart chartComp)
	{
		Hashtable hashtable = new Hashtable();
		bool flag = false;
		for (int j = 0; j < chartComp.Series.Count; j++)
		{
			StiSeries stiSeries = chartComp.Series[j] as StiSeries;
			SetTitle(masterChart, j, stiSeries);
			stiSeries.ValuesStart = null;
			stiSeries.Values = CheckValueNaN(GetValues(masterChart, stiSeries));
			if (chartComp.Series[j] is StiGanttSeries stiGanttSeries)
			{
				stiGanttSeries.ValuesEnd = GetValuesEnd(masterChart, stiGanttSeries, stiGanttSeries.ValueDataColumnEnd, stiGanttSeries.InvokeGetListOfValuesEnd, stiGanttSeries.InvokeGetValueEnd);
			}
			if (chartComp.Series[j] is StiRangeSeries stiRangeSeries)
			{
				stiRangeSeries.ValuesEnd = GetValuesEnd(masterChart, stiRangeSeries, stiRangeSeries.ValueDataColumnEnd, stiRangeSeries.InvokeGetListOfValuesEnd, stiRangeSeries.InvokeGetValueEnd);
			}
			if (chartComp.Series[j] is StiSteppedRangeSeries stiSteppedRangeSeries)
			{
				stiSteppedRangeSeries.ValuesEnd = GetValuesEnd(masterChart, stiSteppedRangeSeries, stiSteppedRangeSeries.ValueDataColumnEnd, stiSteppedRangeSeries.InvokeGetListOfValuesEnd, stiSteppedRangeSeries.InvokeGetValueEnd);
			}
			if (chartComp.Series[j] is StiRangeBarSeries stiRangeBarSeries)
			{
				stiRangeBarSeries.ValuesEnd = GetValuesEnd(masterChart, stiRangeBarSeries, stiRangeBarSeries.ValueDataColumnEnd, stiRangeBarSeries.InvokeGetListOfValuesEnd, stiRangeBarSeries.InvokeGetValueEnd);
			}
			if (chartComp.Series[j] is StiSplineRangeSeries stiSplineRangeSeries)
			{
				stiSplineRangeSeries.ValuesEnd = GetValuesEnd(masterChart, stiSplineRangeSeries, stiSplineRangeSeries.ValueDataColumnEnd, stiSplineRangeSeries.InvokeGetListOfValuesEnd, stiSplineRangeSeries.InvokeGetValueEnd);
			}
			if (chartComp.Series[j] is StiCandlestickSeries stiCandlestickSeries)
			{
				stiCandlestickSeries.ValuesHigh = GetValuesHigh(masterChart, stiCandlestickSeries);
				stiCandlestickSeries.ValuesLow = GetValuesLow(masterChart, stiCandlestickSeries);
				stiCandlestickSeries.ValuesClose = GetValuesClose(masterChart, stiCandlestickSeries);
				stiCandlestickSeries.ValuesOpen = GetValuesOpen(masterChart, stiCandlestickSeries);
			}
			StiBubbleSeries stiBubbleSeries = chartComp.Series[j] as StiBubbleSeries;
			if (stiBubbleSeries != null)
			{
				stiBubbleSeries.Weights = GetWeights(masterChart, stiBubbleSeries);
			}
			if (stiSeries is StiPieSeries series)
			{
				SetCutPieList(masterChart, series);
			}
			masterChart.CacheValues(cache: true);
			stiSeries.Arguments = GetArguments(masterChart, stiSeries);
			stiSeries.Tags = GetTags(masterChart, stiSeries);
			stiSeries.Hyperlinks = GetHyperlinks(masterChart, stiSeries);
			stiSeries.ToolTips = GetToolTips(masterChart, stiSeries);
			CheckParetoValues(stiSeries);
			if (stiSeries is StiHistogramSeries)
			{
				StiFormatService formatService = ((StiAxisArea)stiSeries.Chart.Area).XAxis.Labels.FormatService;
				StiHistogramHelper.CheckValuesAndArguments(stiSeries, formatService);
			}
			masterChart.CacheValues(cache: false);
			string[] autoSeriesKeysFromAutoSeriesKeyDataColumn = GetAutoSeriesKeysFromAutoSeriesKeyDataColumn(masterChart, stiSeries);
			string[] autoSeriesTitleFromAutoSeriesTitleDataColumn = GetAutoSeriesTitleFromAutoSeriesTitleDataColumn(masterChart, stiSeries);
			string[] autoSeriesColorFromAutoSeriesColorDataColumn = GetAutoSeriesColorFromAutoSeriesColorDataColumn(masterChart, stiSeries);
			if (stiSeries.SortBy != StiSeriesSortType.None || stiSeries.Filters.Count > 0 || autoSeriesKeysFromAutoSeriesKeyDataColumn.Length != 0)
			{
				int num = ((stiSeries.Values.Length > stiSeries.Arguments.Length) ? stiSeries.Values.Length : stiSeries.Arguments.Length);
				List<StiDataItem> list = new List<StiDataItem>();
				for (int k = 0; k < num; k++)
				{
					object value = ((stiSeries.Values.Length > k) ? stiSeries.Values[k] : new double?(0.0));
					object obj = ((stiSeries.Arguments.Length > k) ? stiSeries.Arguments[k] : ((object)k));
					object key = ((autoSeriesKeysFromAutoSeriesKeyDataColumn.Length > k) ? autoSeriesKeysFromAutoSeriesKeyDataColumn[k] : null);
					object title = ((autoSeriesTitleFromAutoSeriesTitleDataColumn.Length > k) ? autoSeriesTitleFromAutoSeriesTitleDataColumn[k] : null);
					string toolTip = ((stiSeries.ToolTips.Length > k) ? stiSeries.ToolTips[k] : string.Empty);
					string color = ((autoSeriesColorFromAutoSeriesColorDataColumn.Length > k) ? autoSeriesColorFromAutoSeriesColorDataColumn[k] : null);
					object tag = ((stiSeries.Tags.Length > k) ? stiSeries.Tags[k] : ((object)0));
					object valueEnd = null;
					if (stiSeries is IStiRangeSeries)
					{
						valueEnd = ((((IStiRangeSeries)stiSeries).ValuesEnd.Length > k) ? ((IStiRangeSeries)stiSeries).ValuesEnd[k] : new double?(0.0));
					}
					object valueOpen = null;
					object valueClose = null;
					object valueLow = null;
					object valueHight = null;
					if (stiSeries is IStiFinancialSeries)
					{
						valueOpen = ((((IStiFinancialSeries)stiSeries).ValuesOpen.Length > k) ? ((IStiFinancialSeries)stiSeries).ValuesOpen[k] : new double?(0.0));
						valueClose = ((((IStiFinancialSeries)stiSeries).ValuesClose.Length > k) ? ((IStiFinancialSeries)stiSeries).ValuesClose[k] : new double?(0.0));
						valueLow = ((((IStiFinancialSeries)stiSeries).ValuesLow.Length > k) ? ((IStiFinancialSeries)stiSeries).ValuesLow[k] : new double?(0.0));
						valueHight = ((((IStiFinancialSeries)stiSeries).ValuesHigh.Length > k) ? ((IStiFinancialSeries)stiSeries).ValuesHigh[k] : new double?(0.0));
					}
					object weight = null;
					if (stiBubbleSeries != null)
					{
						weight = ((stiBubbleSeries.Weights.Length > k) ? stiBubbleSeries.Weights[k] : 0.0);
					}
					list.Add(new StiDataItem(obj ?? string.Empty, value, valueEnd, weight, valueOpen, valueClose, valueLow, valueHight, title, key, color, toolTip, tag));
				}
				Hashtable hashtable2 = new Hashtable();
				int num2 = 0;
				foreach (StiChartFilter filter in stiSeries.Filters)
				{
					string filterMethodName = $"{chartComp.Name}Filters_{j}_{num2}";
					hashtable2[filter] = GetFilterData(chartComp.Report, filter, filterMethodName);
					num2++;
				}
				List<StiDataItem> list2 = new List<StiDataItem>();
				StiDataSource stiDataSource = null;
				StiBusinessObject stiBusinessObject = null;
				if (stiSeries.Filters.Count > 0)
				{
					try
					{
						stiDataSource = StiDataColumn.GetDataSourceFromDataColumn(chartComp.Report.Dictionary, stiSeries.ValueDataColumn);
					}
					catch
					{
					}
					if (stiDataSource == null)
					{
						try
						{
							stiDataSource = StiDataColumn.GetDataSourceFromDataColumn(chartComp.Report.Dictionary, stiSeries.ArgumentDataColumn);
						}
						catch
						{
						}
					}
					if (stiDataSource == null)
					{
						stiDataSource = chartComp.DataSource;
					}
					try
					{
						stiBusinessObject = StiDataColumn.GetBusinessObjectFromDataColumn(chartComp.Report.Dictionary, stiSeries.ValueDataColumn);
					}
					catch
					{
					}
					if (stiBusinessObject == null)
					{
						try
						{
							stiBusinessObject = StiDataColumn.GetBusinessObjectFromDataColumn(chartComp.Report.Dictionary, stiSeries.ArgumentDataColumn);
						}
						catch
						{
						}
					}
					if (stiBusinessObject == null)
					{
						stiBusinessObject = chartComp.BusinessObject;
					}
				}
				if (stiDataSource != null)
				{
					stiDataSource.SaveState("ChartFilter");
					stiDataSource.First();
				}
				if (stiBusinessObject != null)
				{
					stiBusinessObject.SaveState("ChartFilter");
					stiBusinessObject.CreateEnumerator();
				}
				foreach (StiDataItem item2 in list)
				{
					bool[] array = new bool[stiSeries.Filters.Count];
					int num3 = 0;
					foreach (StiChartFilter filter2 in stiSeries.Filters)
					{
						array[num3] = GetFilterResult(filter2, item2.Argument, item2.Value, item2.ValueEnd, item2.ValueOpen, item2.ValueClose, item2.ValueLow, item2.ValueHigh, item2.ToolTip, hashtable2[filter2]);
						if (filter2.Item == StiFilterItem.Expression)
						{
							string filterMethodName2 = $"{chartComp.Name}Filters_{j}_{num3}";
							try
							{
								array[num3] = (bool)GetFilterData(chartComp.Report, filter2, filterMethodName2);
							}
							catch
							{
							}
						}
						num3++;
					}
					bool flag2 = true;
					if (stiSeries.FilterMode == StiFilterMode.And)
					{
						bool[] array2 = array;
						for (int l = 0; l < array2.Length; l++)
						{
							if (!array2[l])
							{
								flag2 = false;
								break;
							}
						}
					}
					else
					{
						flag2 = false;
						bool[] array3 = array;
						for (int m = 0; m < array3.Length; m++)
						{
							if (array3[m])
							{
								flag2 = true;
								break;
							}
						}
					}
					if (flag2)
					{
						list2.Add(item2);
					}
					stiDataSource?.Next();
					stiBusinessObject?.Next();
				}
				stiDataSource?.RestoreState("ChartFilter");
				stiBusinessObject?.RestoreState("ChartFilter");
				list = list2;
				if (stiSeries.SortBy != StiSeriesSortType.None)
				{
					StiDataItemComparer comparer = new StiDataItemComparer(stiSeries.SortBy, stiSeries.SortDirection);
					list.Sort(comparer);
				}
				FillSeriesData(stiSeries, list);
				if (autoSeriesKeysFromAutoSeriesKeyDataColumn.Length != 0)
				{
					Hashtable hashtable3 = new Hashtable();
					object[] array4 = null;
					if (stiSeries.Arguments != null && stiSeries.Arguments.Length != 0)
					{
						array4 = list.Select((StiDataItem i) => i.Argument).Distinct().ToArray();
						if (stiSeries.SortBy == StiSeriesSortType.Argument)
						{
							Array.Sort(array4);
							if (stiSeries.SortDirection == StiSeriesSortDirection.Descending)
							{
								Array.Reverse((Array)array4);
							}
						}
					}
					else
					{
						Hashtable hashtable4 = new Hashtable();
						foreach (StiDataItem item3 in list)
						{
							List<StiDataItem> list3 = hashtable4[item3.Key] as List<StiDataItem>;
							if (list3 == null)
							{
								list3 = new List<StiDataItem>();
								hashtable4[item3.Key] = list3;
							}
							list3.Add(item3);
						}
						int num4 = 0;
						foreach (List<StiDataItem> value3 in hashtable4.Values)
						{
							num4 = Math.Max(num4, value3.Count);
							int num5 = 0;
							foreach (StiDataItem item4 in value3)
							{
								item4.Argument = num5.ToString();
								hashtable3[item4] = num5.ToString();
								num5++;
							}
						}
						object[] array5 = new string[num4];
						array4 = array5;
						for (int n = 0; n < num4; n++)
						{
							array4[n] = n.ToString();
						}
					}
					Hashtable hashtable5 = new Hashtable();
					string[] array6 = autoSeriesKeysFromAutoSeriesKeyDataColumn;
					foreach (string text in array6)
					{
						hashtable5[text] = text;
					}
					string[] arrayString = new string[hashtable5.Count];
					hashtable5.Keys.CopyTo(arrayString, 0);
					SortArray(ref arrayString);
					List<StiDataItem>[,] array7 = new List<StiDataItem>[array4.Length, arrayString.Length];
					Hashtable hashtable6 = new Hashtable();
					for (int num7 = 0; num7 < array4.Length; num7++)
					{
						hashtable6[array4[num7]] = num7;
					}
					Hashtable hashtable7 = new Hashtable();
					for (int num8 = 0; num8 < arrayString.Length; num8++)
					{
						hashtable7[arrayString[num8]] = num8;
					}
					Hashtable hashtable8 = new Hashtable();
					Hashtable hashtable9 = new Hashtable();
					foreach (StiDataItem item5 in list)
					{
						object key2 = GetKey(item5.Key);
						object key3 = item5.Argument;
						if (stiSeries.Arguments == null || stiSeries.Arguments.Length == 0)
						{
							key3 = hashtable3[item5];
						}
						int num9 = (int)hashtable7[key2];
						int num10 = (int)hashtable6[key3];
						if (item5.Title != null)
						{
							hashtable8[key2] = item5.Title;
						}
						else
						{
							hashtable8[key2] = item5.Key;
						}
						hashtable9[key2] = item5.Color;
						if (array7[num10, num9] != null)
						{
							array7[num10, num9].Add(item5);
							continue;
						}
						List<StiDataItem> list5 = new List<StiDataItem> { item5 };
						array7[num10, num9] = list5;
					}
					StiBubbleSeries stiBubbleSeries2 = null;
					if (stiBubbleSeries != null)
					{
						stiBubbleSeries2 = ((StiService)stiBubbleSeries).Clone() as StiBubbleSeries;
					}
					for (int num11 = 0; num11 < arrayString.Length; num11++)
					{
						string key4 = arrayString[num11];
						StiSeries stiSeries2;
						if (num11 == 0)
						{
							stiSeries2 = stiSeries;
						}
						else
						{
							stiSeries2 = ((StiService)stiSeries).Clone() as StiSeries;
							chartComp.Series.Insert(j + num11, stiSeries2);
						}
						stiSeries2.CoreTitle = hashtable8[key4] as string;
						string value2 = hashtable9[key4] as string;
						hashtable[stiSeries2] = value2;
						List<double?> list6 = new List<double?>();
						List<double?> list7 = new List<double?>();
						List<object> list8 = new List<object>();
						List<double> list9 = new List<double>();
						List<string> list10 = new List<string>();
						List<object> list11 = new List<object>();
						for (int num12 = 0; num12 < array4.Length; num12++)
						{
							List<StiDataItem> list12 = array7[num12, num11];
							if (list12 == null)
							{
								if (!(stiSeries2 is IStiScatterSeries))
								{
									list6.Add(null);
									list9.Add(0.0);
									list10.Add(null);
									list11.Add(null);
									if (stiSeries2 is IStiRangeSeries)
									{
										list7.Add(null);
									}
									list8.Add(array4[num12]);
								}
								continue;
							}
							foreach (StiDataItem item6 in list12)
							{
								double? item = null;
								bool flag3 = stiSeries2 is IStiScatterSeries;
								if (item6 != null && item6.Value is double)
								{
									item = (double)item6.Value;
									flag3 = false;
								}
								if (!(stiSeries2 is IStiScatterSeries) || array4[num12] != null)
								{
									list6.Add(item);
									if (!flag3)
									{
										list8.Add(array4[num12]);
									}
									if (stiSeries2 is IStiRangeSeries)
									{
										list7.Add((double)item6.ValueEnd);
									}
									if (stiBubbleSeries2 != null)
									{
										list9.Add((double)item6.Weight);
									}
									if (item6.ToolTip != null)
									{
										list10.Add(item6.ToolTip.ToString());
									}
									if (item6.Tag != null)
									{
										list11.Add(item6.Tag);
									}
								}
							}
						}
						double?[] array8 = new double?[list6.Count];
						list6.CopyTo(array8);
						stiSeries2.Values = array8;
						object[] array9 = new object[list8.Count];
						list8.CopyTo(array9);
						stiSeries2.Arguments = array9;
						if (stiSeries2 is IStiRangeSeries)
						{
							double?[] array10 = new double?[list7.Count];
							list7.CopyTo(array10);
							((IStiRangeSeries)stiSeries2).ValuesEnd = array10;
						}
						if (stiSeries2 is StiBubbleSeries)
						{
							double[] array11 = new double[list9.Count];
							list9.CopyTo(array11);
							((StiBubbleSeries)stiSeries2).Weights = array11;
						}
						stiSeries2.ToolTips = list10.ToArray();
						stiSeries2.Tags = list11.ToArray();
						CheckArgumentsDateTimeStep(stiSeries2);
						CreateValuesTopN(stiSeries2);
						stiSeries2.Filters = stiSeries.Filters;
						flag = true;
					}
					j += arrayString.Length - 1;
				}
				else
				{
					CreateValuesTopN(stiSeries);
				}
			}
			else
			{
				CheckArgumentsDateTimeStep(stiSeries);
				CreateValuesTopN(stiSeries);
			}
		}
		StiWaterfallHelper.CheckTotals(chartComp);
		StiBoxAndWhiskerHelper.CheckArgument(chartComp);
		if (StiOptions.Engine.AllowSimplifyChartValues)
		{
			chartComp.SimplifyValues();
		}
		if (StiOptions.Engine.AllowInvokeProcessChartEventForTemplateOfChart)
		{
			chartComp.InvokeEvents();
		}
		if (!flag || !StiOptions.Engine.ApplyStylesInAutoSeries)
		{
			return;
		}
		chartComp.Series.ApplyStyle(chartComp.Style);
		int num13 = 0;
		foreach (StiSeries item7 in chartComp.Series)
		{
			object obj7 = ConvertStringToColor(hashtable[item7] as string);
			StiNewAutoSeriesEventArgs stiNewAutoSeriesEventArgs = new StiNewAutoSeriesEventArgs(num13, item7, null);
			if (obj7 != null)
			{
				stiNewAutoSeriesEventArgs.Color = (Color)obj7;
			}
			item7.InvokeNewAutoSeries(stiNewAutoSeriesEventArgs);
			if (stiNewAutoSeriesEventArgs.Color is Color)
			{
				item7.Core.ApplyStyle(chartComp.Style, (Color)stiNewAutoSeriesEventArgs.Color);
			}
			num13++;
		}
	}

	internal static List<PointD> GetShorterListPoints(StiSeries series)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		PointD[] array = (PointD[])(object)new PointD[series.Values.Length];
		for (int i = 0; i < series.Values.Length; i++)
		{
			array[i] = new PointD(series.Values[i].GetValueOrDefault(), (double)i);
		}
		double[] array2 = new double[10] { 0.3, 0.6, 1.0, 5.0, 9.0, 15.0, 20.0, 25.0, 30.0, 40.0 };
		List<PointD> list = null;
		List<PointD> list2 = null;
		double[] array3 = array2;
		foreach (double tolerance in array3)
		{
			list = StiSimplifyHelper.Simplify(array, tolerance, highestQuality: true);
			if ((double)list.Count / (double)array.Length < 0.02 && list.Count < 900 && list2 != null)
			{
				list = list2;
				break;
			}
			list2 = list;
		}
		return list;
	}

	private static void CheckParetoValues(StiSeries series)
	{
		if (!(series is StiParetoSeries stiParetoSeries))
		{
			return;
		}
		List<double?> list = new List<double?>();
		List<object> list2 = new List<object>();
		for (int i = 0; i < series.Values.Length; i++)
		{
			double valueOrDefault = series.Values[i].GetValueOrDefault();
			if (valueOrDefault > 0.0)
			{
				list.Add(valueOrDefault);
				if (series.Arguments != null && i < series.Arguments.Length)
				{
					list2.Add(series.Arguments[i]);
				}
				else
				{
					list2.Add(i + 1);
				}
			}
		}
		stiParetoSeries.Values = list.ToArray();
		stiParetoSeries.Arguments = list2.ToArray();
		stiParetoSeries.ValuesStart = null;
	}

	private static double?[] CheckValueNaN(double?[] values)
	{
		for (int i = 0; i < values.Length; i++)
		{
			if (values[i].HasValue && double.IsNaN(values[i].GetValueOrDefault()))
			{
				values[i] = null;
			}
		}
		return values;
	}

	private static void CheckArgumentsDateTimeStep(StiSeries series)
	{
		object[] arguments = series.Arguments;
		if (!(series.Chart.Area is StiAxisArea))
		{
			return;
		}
		IStiAxisDateTimeStep dateTimeStep = ((StiXAxis)((StiAxisArea)series.Chart.Area).XAxis).DateTimeStep;
		StiTimeDateStep step = dateTimeStep.Step;
		int numberOfValues = dateTimeStep.NumberOfValues;
		if (!IsArgumentsDateTime(arguments) || step == StiTimeDateStep.None)
		{
			return;
		}
		DateTime[] array = new DateTime[arguments.Length];
		for (int i = 0; i < arguments.Length; i++)
		{
			array[i] = DateTime.Parse(arguments[i].ToString());
		}
		DateTime dateMax = MaximumDate(array);
		DateTime dateTime = MinimumDate(array);
		int num = (int)GetTotalTimeSpans(step, dateMax, dateTime) + 1;
		List<object> list = new List<object>();
		List<double?> list2 = new List<double?>();
		DateTime dateTime2 = dateTime;
		DateTime nextDate = GetNextDate(dateTime, step);
		for (int j = 0; j < num; j++)
		{
			long num2 = (nextDate.Ticks - dateTime2.Ticks) / numberOfValues;
			TimeSpan timeSpan = new TimeSpan(num2 / 2);
			for (int k = 0; k < numberOfValues; k++)
			{
				TimeSpan value = new TimeSpan(num2 * k);
				if (value.Ticks <= dateMax.Ticks)
				{
					list.Add(dateTime2.Add(value));
					DateTime dateStart = (DateTime)list[list.Count - 1] - timeSpan;
					DateTime dateEnd = (DateTime)list[list.Count - 1] + timeSpan;
					double? valueForDate = GetValueForDate(dateStart, dateEnd, array, series.Values);
					list2.Add(valueForDate);
				}
			}
			dateTime2 = nextDate;
			nextDate = GetNextDate(nextDate, step);
		}
		object[] array2 = new object[list.Count];
		double?[] array3 = new double?[list2.Count];
		list.CopyTo(array2);
		list2.CopyTo(array3);
		int num3 = 0;
		double? num4 = null;
		for (int l = 0; l < array3.Length; l++)
		{
			if (array3[l].HasValue && !num4.HasValue)
			{
				num3 = l;
				num4 = array3[l];
			}
			else
			{
				if (!array3[l].HasValue || !num4.HasValue)
				{
					continue;
				}
				int num5 = l;
				num4 = array3[num3];
				double? num6 = array3[num5];
				double num7 = (num4.GetValueOrDefault() - num6.GetValueOrDefault()) / (double)(num5 - num3);
				for (int m = num3 + 1; m < num5; m++)
				{
					if (dateTimeStep.Interpolation)
					{
						array3[m] = array3[m - 1] - num7;
					}
					else
					{
						array3[m] = 0.0;
					}
				}
				num3 = 0;
				num4 = null;
				l--;
			}
		}
		series.Arguments = array2;
		series.Values = array3;
	}

	internal static void CreateValuesTopN(StiSeries series)
	{
		if (series.TopN.Mode == StiTopNMode.None || series.Values.Length <= series.TopN.Count)
		{
			return;
		}
		int count = series.TopN.Count;
		double?[] array = series.Values.Clone() as double?[];
		int[] array2 = new int[series.Values.Length];
		for (int i = 0; i < series.Values.Length; i++)
		{
			array2[i] = i;
		}
		Array.Sort(array, array2);
		if (series.TopN.Mode == StiTopNMode.Top)
		{
			Array.Reverse((Array)array);
			Array.Reverse((Array)array2);
		}
		double?[] destinationArray = new double?[count];
		int[] array3 = new int[count];
		Array.Copy(array, destinationArray, count);
		Array.Copy(array2, array3, count);
		List<double?> list = new List<double?>();
		List<object> list2 = new List<object>();
		double? num = 0.0;
		for (int j = 0; j < series.Values.Length; j++)
		{
			if (FindIndex(array3, j))
			{
				list.Add(series.Values[j]);
				if (j < series.Arguments.Length)
				{
					list2.Add(series.Arguments[j]);
				}
				else
				{
					list2.Add(j);
				}
			}
			else
			{
				num += series.Values[j];
			}
		}
		if (series.TopN.ShowOthers)
		{
			if (series is StiClusteredBarSeries)
			{
				list.Insert(0, num);
				list2.Insert(0, series.TopN.OthersText);
			}
			else
			{
				list.Add(num);
				list2.Add(series.TopN.OthersText);
			}
		}
		double?[] array4 = new double?[list.Count];
		list.CopyTo(array4);
		object[] array5 = new object[list2.Count];
		list2.CopyTo(array5);
		series.Values = array4;
		series.Arguments = array5;
	}

	private static DateTime GetNextDate(DateTime firstDate, StiTimeDateStep step)
	{
		return step switch
		{
			StiTimeDateStep.Second => firstDate.AddSeconds(1.0), 
			StiTimeDateStep.Minute => firstDate.AddMinutes(1.0), 
			StiTimeDateStep.Day => firstDate.AddDays(1.0), 
			StiTimeDateStep.Hour => firstDate.AddHours(1.0), 
			StiTimeDateStep.Month => firstDate.AddMonths(1), 
			StiTimeDateStep.Year => firstDate.AddYears(1), 
			_ => firstDate, 
		};
	}

	private static string GetKey(object key)
	{
		if (double.TryParse(key.ToString(), out var result))
		{
			return result.ToString();
		}
		return key.ToString();
	}

	private static void SortArray(ref string[] arrayString)
	{
		List<double> list = new List<double>();
		List<string> list2 = new List<string>();
		for (int i = 0; i < arrayString.Length; i++)
		{
			if (double.TryParse(arrayString[i], out var result))
			{
				list.Add(result);
			}
			else
			{
				list2.Add(arrayString[i]);
			}
		}
		list.Sort();
		list2.Sort();
		string[] array = new string[arrayString.Length];
		int num = 0;
		foreach (double item in list)
		{
			array[num] = item.ToString();
			num++;
		}
		foreach (string item2 in list2)
		{
			array[num] = item2;
			num++;
		}
		arrayString = array;
	}

	private static bool FindIndex(int[] array, int value)
	{
		return array.Any((int t) => t == value);
	}

	private static double? GetValueForDate(DateTime dateStart, DateTime dateEnd, DateTime[] arguments, double?[] values)
	{
		int num = 0;
		double num2 = 0.0;
		double? result = null;
		for (int i = 0; i < arguments.Length; i++)
		{
			DateTime dateTime = arguments[i];
			if (dateTime > dateStart && dateTime <= dateEnd)
			{
				num++;
				num2 += values[i].GetValueOrDefault();
			}
		}
		if (num != 0)
		{
			result = num2 / (double)num;
		}
		return result;
	}

	private static double GetTotalTimeSpans(StiTimeDateStep step, DateTime dateMax, DateTime dateMin)
	{
		TimeSpan timeSpan = dateMax - dateMin;
		double result = 0.0;
		switch (step)
		{
		case StiTimeDateStep.Second:
			result = timeSpan.TotalSeconds;
			break;
		case StiTimeDateStep.Minute:
			result = timeSpan.TotalMinutes;
			break;
		case StiTimeDateStep.Hour:
			result = timeSpan.TotalHours;
			break;
		case StiTimeDateStep.Day:
			result = timeSpan.TotalDays;
			break;
		case StiTimeDateStep.Month:
			result = (dateMax.Year - dateMin.Year) * 12 + dateMax.Month - dateMin.Month;
			break;
		case StiTimeDateStep.Year:
			result = dateMax.Year - dateMin.Year;
			break;
		}
		return result;
	}

	private static bool IsArgumentsDateTime(object[] arguments)
	{
		if (arguments.Length == 0)
		{
			return false;
		}
		foreach (object obj in arguments)
		{
			if (obj == null)
			{
				return false;
			}
			string s = obj.ToString();
			if (!DateTime.TryParse(s, out var _))
			{
				return false;
			}
		}
		return true;
	}

	private static DateTime MaximumDate(DateTime[] dates)
	{
		DateTime result = DateTime.MinValue;
		for (int i = 0; i < dates.Length; i++)
		{
			DateTime dateTime = dates[i];
			if (dateTime.ToOADate() > result.ToOADate())
			{
				result = dateTime;
			}
		}
		return result;
	}

	private static DateTime MinimumDate(DateTime[] dates)
	{
		DateTime result = DateTime.MaxValue;
		for (int i = 0; i < dates.Length; i++)
		{
			DateTime dateTime = dates[i];
			if (dateTime.ToOADate() < result.ToOADate())
			{
				result = dateTime;
			}
		}
		return result;
	}

	private static string[] GetAutoSeriesColorFromAutoSeriesColorDataColumn(StiChart masterChart, StiSeries series)
	{
		if (series.AutoSeriesColorDataColumn == null || series.AutoSeriesColorDataColumn.Trim().Length <= 0)
		{
			return new string[0];
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.AutoSeriesColorDataColumn);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			string[] array = new string[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.AutoSeriesColorDataColumn);
				dataFromDataColumn = ((dataFromDataColumn != null) ? dataFromDataColumn.ToString() : string.Empty);
				array[i] = dataFromDataColumn as string;
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.AutoSeriesColorDataColumn);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			string[] array2 = new string[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.AutoSeriesColorDataColumn);
				dataFromBusinessObject = ((dataFromBusinessObject != null) ? dataFromBusinessObject.ToString() : string.Empty);
				array2[j] = dataFromBusinessObject as string;
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return new string[0];
	}

	private static string[] GetAutoSeriesTitleFromAutoSeriesTitleDataColumn(StiChart masterChart, StiSeries series)
	{
		if (series.AutoSeriesTitleDataColumn == null || series.AutoSeriesTitleDataColumn.Trim().Length <= 0)
		{
			return new string[0];
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.AutoSeriesTitleDataColumn);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			string[] array = new string[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.AutoSeriesTitleDataColumn);
				dataFromDataColumn = ((dataFromDataColumn != null) ? dataFromDataColumn.ToString() : string.Empty);
				array[i] = dataFromDataColumn as string;
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.AutoSeriesTitleDataColumn);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			string[] array2 = new string[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.AutoSeriesTitleDataColumn);
				dataFromBusinessObject = ((dataFromBusinessObject != null) ? dataFromBusinessObject.ToString() : string.Empty);
				array2[j] = dataFromBusinessObject as string;
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return new string[0];
	}

	private static string[] GetAutoSeriesKeysFromAutoSeriesKeyDataColumn(StiChart masterChart, StiSeries series)
	{
		if (series.AutoSeriesKeyDataColumn == null || series.AutoSeriesKeyDataColumn.Trim().Length <= 0)
		{
			return new string[0];
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.AutoSeriesKeyDataColumn);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			string[] array = new string[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.AutoSeriesKeyDataColumn);
				dataFromDataColumn = ((dataFromDataColumn != null) ? dataFromDataColumn.ToString() : string.Empty);
				array[i] = dataFromDataColumn as string;
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.AutoSeriesKeyDataColumn);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			string[] array2 = new string[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.AutoSeriesKeyDataColumn);
				dataFromBusinessObject = ((dataFromBusinessObject != null) ? dataFromBusinessObject.ToString() : string.Empty);
				array2[j] = dataFromBusinessObject as string;
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return new string[0];
	}

	private static void SetTitle(StiChart masterChart, int seriesIndex, StiSeries series)
	{
		StiGetTitleEventArgs stiGetTitleEventArgs = new StiGetTitleEventArgs
		{
			Series = series,
			Index = seriesIndex
		};
		series.InvokeGetTitle(masterChart, stiGetTitleEventArgs);
		if (!string.IsNullOrEmpty(stiGetTitleEventArgs.Value))
		{
			series.CoreTitle = stiGetTitleEventArgs.Value;
		}
	}

	private static void SetCutPieList(StiChart masterChart, StiPieSeries series)
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		series.InvokeGetCutPieList(masterChart, stiGetValueEventArgs);
		if (!string.IsNullOrEmpty(stiGetValueEventArgs.Value))
		{
			series.CutPieListValues = StiSeries.GetValuesFromString(stiGetValueEventArgs.Value);
		}
	}

	private static object[] GetArguments(StiChart masterChart, StiSeries series)
	{
		object[] argumentsFromListOfArguments = GetArgumentsFromListOfArguments(masterChart, series);
		if (argumentsFromListOfArguments != null)
		{
			return argumentsFromListOfArguments;
		}
		argumentsFromListOfArguments = GetArgumentsFromArgumentDataColumn(masterChart, series);
		if (argumentsFromListOfArguments != null)
		{
			return argumentsFromListOfArguments;
		}
		argumentsFromListOfArguments = GetArgumentsFromArgumentExpression(masterChart, series);
		if (argumentsFromListOfArguments != null)
		{
			return argumentsFromListOfArguments;
		}
		return new object[0];
	}

	private static object[] GetArgumentsFromArgumentExpression(StiChart masterChart, StiSeries series)
	{
		object[] array = new object[masterChart.Count];
		masterChart.First();
		for (int i = 0; i < masterChart.Count; i++)
		{
			StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
			series.InvokeGetArgument(masterChart, stiValueEventArgs);
			array[i] = stiValueEventArgs.Value;
			masterChart.Next();
		}
		return array;
	}

	private static object[] GetArgumentsFromArgumentDataColumn(StiChart masterChart, StiSeries series)
	{
		if (series.ArgumentDataColumn == null || series.ArgumentDataColumn.Trim().Length <= 0)
		{
			return null;
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.ArgumentDataColumn);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			object[] array = new object[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				StiDataColumn dataColumnFromColumnName = StiDataColumn.GetDataColumnFromColumnName(masterChart.Report.Dictionary, series.ArgumentDataColumn);
				if (dataColumnFromColumnName != null && dataColumnFromColumnName.Type == typeof(DateTime))
				{
					object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.ArgumentDataColumn);
					if (!(dataFromDataColumn is DateTime))
					{
						if (DateTime.TryParse(dataFromDataColumn.ToString(), out var result))
						{
							array[i] = result;
						}
						else
						{
							array[i] = dataFromDataColumn;
						}
					}
					else
					{
						array[i] = dataFromDataColumn;
					}
				}
				else
				{
					array[i] = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.ArgumentDataColumn);
				}
				if (array[i] is DateTime)
				{
					series.Core.IsDateTimeArguments = true;
				}
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.ArgumentDataColumn);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			object[] array2 = new object[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				array2[j] = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.ArgumentDataColumn);
				if (array2[j] is DateTime)
				{
					series.Core.IsDateTimeArguments = true;
				}
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return null;
	}

	private static object[] GetArgumentsFromListOfArguments(StiChart masterChart, StiSeries series)
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		series.InvokeGetListOfArguments(masterChart, stiGetValueEventArgs);
		if (!string.IsNullOrEmpty(stiGetValueEventArgs.Value))
		{
			return StiSeries.GetArgumentsFromString(stiGetValueEventArgs.Value);
		}
		return null;
	}

	private static double?[] GetValues(StiChart masterChart, StiSeries series)
	{
		double?[] valuesFromListOfValues = GetValuesFromListOfValues(masterChart, series);
		if (valuesFromListOfValues != null)
		{
			return valuesFromListOfValues;
		}
		valuesFromListOfValues = GetValuesFromValueDataColumn(masterChart, series);
		if (valuesFromListOfValues != null)
		{
			return valuesFromListOfValues;
		}
		valuesFromListOfValues = GetValuesFromValueExpression(masterChart, series);
		if (valuesFromListOfValues != null)
		{
			return valuesFromListOfValues;
		}
		return new double?[0];
	}

	private static double?[] GetValuesFromValueExpression(StiChart masterChart, StiSeries series)
	{
		double?[] array = new double?[masterChart.Count];
		masterChart.First();
		for (int i = 0; i < masterChart.Count; i++)
		{
			StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs
			{
				Value = "0"
			};
			series.InvokeGetValue(masterChart, stiGetValueEventArgs);
			array[i] = (double)StiReport.ChangeType(stiGetValueEventArgs.Value, typeof(double));
			masterChart.Next();
		}
		return array;
	}

	private static double?[] GetValuesFromValueDataColumn(StiChart masterChart, StiSeries series)
	{
		if (series.ValueDataColumn == null || series.ValueDataColumn.Trim().Length <= 0)
		{
			return null;
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumn);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			double?[] array = new double?[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumn);
				double? num = null;
				try
				{
					if (dataFromDataColumn is DateTime)
					{
						series.Core.IsDateTimeValues = true;
						num = ((DateTime)dataFromDataColumn).ToOADate();
					}
					else if (dataFromDataColumn != null && dataFromDataColumn != DBNull.Value)
					{
						num = (double?)StiReport.ChangeType(dataFromDataColumn, typeof(double));
					}
				}
				catch
				{
				}
				array[i] = num;
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumn);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			double?[] array2 = new double?[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.ValueDataColumn);
				double? num2 = null;
				if (dataFromBusinessObject is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num2 = ((DateTime)dataFromBusinessObject).ToOADate();
				}
				else if (dataFromBusinessObject != null && dataFromBusinessObject != DBNull.Value)
				{
					num2 = (double?)StiReport.ChangeType(dataFromBusinessObject, typeof(double));
				}
				array2[j] = num2;
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return null;
	}

	private static double?[] GetValuesFromListOfValues(StiChart masterChart, StiSeries series)
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		series.InvokeGetListOfValues(masterChart, stiGetValueEventArgs);
		if (!string.IsNullOrEmpty(stiGetValueEventArgs.Value))
		{
			return StiSeries.GetNullableValuesFromString(series, stiGetValueEventArgs.Value);
		}
		return null;
	}

	private static double?[] GetValuesEnd(StiChart masterChart, StiSeries series, string valueDataColumnEnd, InvokeGetValues listValuesEnd, InvokeGetValues valuesEnd)
	{
		double?[] valuesEndFromListOfValuesEnd = GetValuesEndFromListOfValuesEnd(masterChart, series, listValuesEnd);
		if (valuesEndFromListOfValuesEnd != null)
		{
			return valuesEndFromListOfValuesEnd;
		}
		valuesEndFromListOfValuesEnd = GetValuesEndFromValueDataColumnEnd(masterChart, series, valueDataColumnEnd);
		if (valuesEndFromListOfValuesEnd != null)
		{
			return valuesEndFromListOfValuesEnd;
		}
		valuesEndFromListOfValuesEnd = GetValuesEndFromValueEndExpression(masterChart, valuesEnd);
		if (valuesEndFromListOfValuesEnd != null)
		{
			return valuesEndFromListOfValuesEnd;
		}
		return new double?[0];
	}

	private static double?[] GetValuesEndFromValueEndExpression(StiChart masterChart, InvokeGetValues valuesEnd)
	{
		double?[] array = new double?[masterChart.Count];
		masterChart.First();
		for (int i = 0; i < masterChart.Count; i++)
		{
			StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs
			{
				Value = "0"
			};
			valuesEnd(masterChart, stiGetValueEventArgs);
			array[i] = (double)StiReport.ChangeType(stiGetValueEventArgs.Value, typeof(double));
			masterChart.Next();
		}
		return array;
	}

	private static double?[] GetValuesEndFromValueDataColumnEnd(StiChart masterChart, StiSeries series, string valueDataColumnEnd)
	{
		if (valueDataColumnEnd == null || valueDataColumnEnd.Trim().Length <= 0)
		{
			return null;
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, valueDataColumnEnd);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			double?[] array = new double?[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, valueDataColumnEnd);
				double? num = null;
				if (dataFromDataColumn is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num = ((DateTime)dataFromDataColumn).ToOADate();
				}
				else if (dataFromDataColumn != null && dataFromDataColumn != DBNull.Value)
				{
					num = (double)StiReport.ChangeType(dataFromDataColumn, typeof(double));
				}
				array[i] = num;
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, valueDataColumnEnd);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			double?[] array2 = new double?[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, valueDataColumnEnd);
				double? num2 = null;
				if (dataFromBusinessObject is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num2 = ((DateTime)dataFromBusinessObject).ToOADate();
				}
				else if (dataFromBusinessObject != null && dataFromBusinessObject != DBNull.Value)
				{
					num2 = (double?)StiReport.ChangeType(dataFromBusinessObject, typeof(double));
				}
				array2[j] = num2;
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return null;
	}

	private static double?[] GetValuesEndFromListOfValuesEnd(StiChart masterChart, StiSeries series, InvokeGetValues listValues)
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		listValues(masterChart, stiGetValueEventArgs);
		if (!string.IsNullOrEmpty(stiGetValueEventArgs.Value))
		{
			return StiSeries.GetNullableValuesFromString(series, stiGetValueEventArgs.Value);
		}
		return null;
	}

	private static double?[] GetValuesOpen(StiChart masterChart, StiCandlestickSeries series)
	{
		double?[] valuesOpenFromListOfValuesOpen = GetValuesOpenFromListOfValuesOpen(masterChart, series);
		if (valuesOpenFromListOfValuesOpen != null)
		{
			return valuesOpenFromListOfValuesOpen;
		}
		valuesOpenFromListOfValuesOpen = GetValuesOpenFromValueDataColumnOpen(masterChart, series);
		if (valuesOpenFromListOfValuesOpen != null)
		{
			return valuesOpenFromListOfValuesOpen;
		}
		valuesOpenFromListOfValuesOpen = GetValuesOpenFromValuesOpenExpression(masterChart, series);
		if (valuesOpenFromListOfValuesOpen != null)
		{
			return valuesOpenFromListOfValuesOpen;
		}
		return new double?[0];
	}

	private static double?[] GetValuesOpenFromValuesOpenExpression(StiChart masterChart, StiCandlestickSeries series)
	{
		double?[] array = new double?[masterChart.Count];
		masterChart.First();
		for (int i = 0; i < masterChart.Count; i++)
		{
			StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs
			{
				Value = "0"
			};
			series.InvokeGetValueOpen(masterChart, stiGetValueEventArgs);
			array[i] = (double)StiReport.ChangeType(stiGetValueEventArgs.Value, typeof(double));
			masterChart.Next();
		}
		return array;
	}

	private static double?[] GetValuesOpenFromValueDataColumnOpen(StiChart masterChart, StiCandlestickSeries series)
	{
		if (series.ValueDataColumnOpen == null || series.ValueDataColumnOpen.Trim().Length <= 0)
		{
			return null;
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumnOpen);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			double?[] array = new double?[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumnOpen);
				double num = 0.0;
				if (dataFromDataColumn is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num = ((DateTime)dataFromDataColumn).ToOADate();
				}
				else
				{
					num = (double)StiReport.ChangeType(dataFromDataColumn, typeof(double));
				}
				array[i] = num;
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumnOpen);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			double?[] array2 = new double?[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.ValueDataColumnOpen);
				double num2 = 0.0;
				if (dataFromBusinessObject is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num2 = ((DateTime)dataFromBusinessObject).ToOADate();
				}
				else
				{
					num2 = (double)StiReport.ChangeType(dataFromBusinessObject, typeof(double));
				}
				array2[j] = num2;
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return null;
	}

	private static double?[] GetValuesOpenFromListOfValuesOpen(StiChart masterChart, StiCandlestickSeries series)
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		series.InvokeGetListOfValuesOpen(masterChart, stiGetValueEventArgs);
		if (!string.IsNullOrEmpty(stiGetValueEventArgs.Value))
		{
			return StiSeries.GetNullableValuesFromString(series, stiGetValueEventArgs.Value);
		}
		return null;
	}

	private static double?[] GetValuesClose(StiChart masterChart, StiCandlestickSeries series)
	{
		double?[] valuesCloseFromListOfValuesClose = GetValuesCloseFromListOfValuesClose(masterChart, series);
		if (valuesCloseFromListOfValuesClose != null)
		{
			return valuesCloseFromListOfValuesClose;
		}
		valuesCloseFromListOfValuesClose = GetValuesCloseFromValueDataColumnClose(masterChart, series);
		if (valuesCloseFromListOfValuesClose != null)
		{
			return valuesCloseFromListOfValuesClose;
		}
		valuesCloseFromListOfValuesClose = GetValuesCloseFromValuesCloseExpression(masterChart, series);
		if (valuesCloseFromListOfValuesClose != null)
		{
			return valuesCloseFromListOfValuesClose;
		}
		return new double?[0];
	}

	private static double?[] GetValuesCloseFromValuesCloseExpression(StiChart masterChart, StiCandlestickSeries series)
	{
		double?[] array = new double?[masterChart.Count];
		masterChart.First();
		for (int i = 0; i < masterChart.Count; i++)
		{
			StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs
			{
				Value = "0"
			};
			series.InvokeGetValueClose(masterChart, stiGetValueEventArgs);
			array[i] = (double)StiReport.ChangeType(stiGetValueEventArgs.Value, typeof(double));
			masterChart.Next();
		}
		return array;
	}

	private static double?[] GetValuesCloseFromValueDataColumnClose(StiChart masterChart, StiCandlestickSeries series)
	{
		if (series.ValueDataColumnClose == null || series.ValueDataColumnClose.Trim().Length <= 0)
		{
			return null;
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumnClose);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			double?[] array = new double?[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumnClose);
				double num = 0.0;
				if (dataFromDataColumn is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num = ((DateTime)dataFromDataColumn).ToOADate();
				}
				else
				{
					num = (double)StiReport.ChangeType(dataFromDataColumn, typeof(double));
				}
				array[i] = num;
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumnClose);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			double?[] array2 = new double?[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.ValueDataColumnClose);
				double num2 = 0.0;
				if (dataFromBusinessObject is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num2 = ((DateTime)dataFromBusinessObject).ToOADate();
				}
				else
				{
					num2 = (double)StiReport.ChangeType(dataFromBusinessObject, typeof(double));
				}
				array2[j] = num2;
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return null;
	}

	private static double?[] GetValuesCloseFromListOfValuesClose(StiChart masterChart, StiCandlestickSeries series)
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		series.InvokeGetListOfValuesClose(masterChart, stiGetValueEventArgs);
		if (!string.IsNullOrEmpty(stiGetValueEventArgs.Value))
		{
			return StiSeries.GetNullableValuesFromString(series, stiGetValueEventArgs.Value);
		}
		return null;
	}

	private static double?[] GetValuesHigh(StiChart masterChart, StiCandlestickSeries series)
	{
		double?[] valuesHighFromListOfValuesHigh = GetValuesHighFromListOfValuesHigh(masterChart, series);
		if (valuesHighFromListOfValuesHigh != null)
		{
			return valuesHighFromListOfValuesHigh;
		}
		valuesHighFromListOfValuesHigh = GetValuesHighFromValueDataColumnHigh(masterChart, series);
		if (valuesHighFromListOfValuesHigh != null)
		{
			return valuesHighFromListOfValuesHigh;
		}
		valuesHighFromListOfValuesHigh = GetValuesHighFromValuesHighExpression(masterChart, series);
		if (valuesHighFromListOfValuesHigh != null)
		{
			return valuesHighFromListOfValuesHigh;
		}
		return new double?[0];
	}

	private static double?[] GetValuesHighFromValuesHighExpression(StiChart masterChart, StiCandlestickSeries series)
	{
		double?[] array = new double?[masterChart.Count];
		masterChart.First();
		for (int i = 0; i < masterChart.Count; i++)
		{
			StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs
			{
				Value = "0"
			};
			series.InvokeGetValueHigh(masterChart, stiGetValueEventArgs);
			array[i] = (double)StiReport.ChangeType(stiGetValueEventArgs.Value, typeof(double));
			masterChart.Next();
		}
		return array;
	}

	private static double?[] GetValuesHighFromValueDataColumnHigh(StiChart masterChart, StiCandlestickSeries series)
	{
		if (series.ValueDataColumnHigh == null || series.ValueDataColumnHigh.Trim().Length <= 0)
		{
			return null;
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumnHigh);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			double?[] array = new double?[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumnHigh);
				double num = 0.0;
				if (dataFromDataColumn is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num = ((DateTime)dataFromDataColumn).ToOADate();
				}
				else
				{
					num = (double)StiReport.ChangeType(dataFromDataColumn, typeof(double));
				}
				array[i] = num;
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumnHigh);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			double?[] array2 = new double?[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.ValueDataColumnHigh);
				double num2 = 0.0;
				if (dataFromBusinessObject is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num2 = ((DateTime)dataFromBusinessObject).ToOADate();
				}
				else
				{
					num2 = (double)StiReport.ChangeType(dataFromBusinessObject, typeof(double));
				}
				array2[j] = num2;
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return null;
	}

	private static double?[] GetValuesHighFromListOfValuesHigh(StiChart masterChart, StiCandlestickSeries series)
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		series.InvokeGetListOfValuesHigh(masterChart, stiGetValueEventArgs);
		if (!string.IsNullOrEmpty(stiGetValueEventArgs.Value))
		{
			return StiSeries.GetNullableValuesFromString(series, stiGetValueEventArgs.Value);
		}
		return null;
	}

	private static double?[] GetValuesLow(StiChart masterChart, StiCandlestickSeries series)
	{
		double?[] valuesLowFromListOfValuesLow = GetValuesLowFromListOfValuesLow(masterChart, series);
		if (valuesLowFromListOfValuesLow != null)
		{
			return valuesLowFromListOfValuesLow;
		}
		valuesLowFromListOfValuesLow = GetValuesLowFromValueDataColumnLow(masterChart, series);
		if (valuesLowFromListOfValuesLow != null)
		{
			return valuesLowFromListOfValuesLow;
		}
		valuesLowFromListOfValuesLow = GetValuesLowFromValuesLowExpression(masterChart, series);
		if (valuesLowFromListOfValuesLow != null)
		{
			return valuesLowFromListOfValuesLow;
		}
		return new double?[0];
	}

	private static double?[] GetValuesLowFromValuesLowExpression(StiChart masterChart, StiCandlestickSeries series)
	{
		double?[] array = new double?[masterChart.Count];
		masterChart.First();
		for (int i = 0; i < masterChart.Count; i++)
		{
			StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs
			{
				Value = "0"
			};
			series.InvokeGetValueLow(masterChart, stiGetValueEventArgs);
			array[i] = (double)StiReport.ChangeType(stiGetValueEventArgs.Value, typeof(double));
			masterChart.Next();
		}
		return array;
	}

	private static double?[] GetValuesLowFromValueDataColumnLow(StiChart masterChart, StiCandlestickSeries series)
	{
		if (series.ValueDataColumnLow == null || series.ValueDataColumnLow.Trim().Length <= 0)
		{
			return null;
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumnLow);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			double?[] array = new double?[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumnLow);
				double num = 0.0;
				if (dataFromDataColumn is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num = ((DateTime)dataFromDataColumn).ToOADate();
				}
				else
				{
					num = (double)StiReport.ChangeType(dataFromDataColumn, typeof(double));
				}
				array[i] = num;
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.ValueDataColumnLow);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			double?[] array2 = new double?[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.ValueDataColumnLow);
				double num2 = 0.0;
				if (dataFromBusinessObject is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num2 = ((DateTime)dataFromBusinessObject).ToOADate();
				}
				else
				{
					num2 = (double)StiReport.ChangeType(dataFromBusinessObject, typeof(double));
				}
				array2[j] = num2;
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return null;
	}

	private static double?[] GetValuesLowFromListOfValuesLow(StiChart masterChart, StiCandlestickSeries series)
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		series.InvokeGetListOfValuesLow(masterChart, stiGetValueEventArgs);
		if (!string.IsNullOrEmpty(stiGetValueEventArgs.Value))
		{
			return StiSeries.GetNullableValuesFromString(series, stiGetValueEventArgs.Value);
		}
		return null;
	}

	private static double[] GetWeights(StiChart masterChart, StiBubbleSeries series)
	{
		double[] weightsFromListOfWeights = GetWeightsFromListOfWeights(masterChart, series);
		if (weightsFromListOfWeights != null)
		{
			return weightsFromListOfWeights;
		}
		weightsFromListOfWeights = GetWeightsFromWeightDataColumn(masterChart, series);
		if (weightsFromListOfWeights != null)
		{
			return weightsFromListOfWeights;
		}
		weightsFromListOfWeights = GetWeightsWeightExpression(masterChart, series);
		if (weightsFromListOfWeights != null)
		{
			return weightsFromListOfWeights;
		}
		return new double[0];
	}

	private static double[] GetWeightsWeightExpression(StiChart masterChart, StiBubbleSeries series)
	{
		double[] array = new double[masterChart.Count];
		masterChart.First();
		for (int i = 0; i < masterChart.Count; i++)
		{
			StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs
			{
				Value = "0"
			};
			series.InvokeGetWeight(masterChart, stiGetValueEventArgs);
			array[i] = (double)StiReport.ChangeType(stiGetValueEventArgs.Value, typeof(double));
			masterChart.Next();
		}
		return array;
	}

	private static double[] GetWeightsFromWeightDataColumn(StiChart masterChart, StiBubbleSeries series)
	{
		if (series.WeightDataColumn == null || series.WeightDataColumn.Trim().Length <= 0)
		{
			return null;
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.WeightDataColumn);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			double[] array = new double[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.WeightDataColumn);
				double num = 0.0;
				if (dataFromDataColumn is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num = ((DateTime)dataFromDataColumn).ToOADate();
				}
				else
				{
					num = (double)StiReport.ChangeType(dataFromDataColumn, typeof(double));
				}
				array[i] = num;
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.WeightDataColumn);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			double[] array2 = new double[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.WeightDataColumn);
				double num2 = 0.0;
				if (dataFromBusinessObject is DateTime)
				{
					series.Core.IsDateTimeValues = true;
					num2 = ((DateTime)dataFromBusinessObject).ToOADate();
				}
				else
				{
					num2 = (double)StiReport.ChangeType(dataFromBusinessObject, typeof(double));
				}
				array2[j] = num2;
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return null;
	}

	private static double[] GetWeightsFromListOfWeights(StiChart masterChart, StiBubbleSeries series)
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		series.InvokeGetListOfWeights(masterChart, stiGetValueEventArgs);
		if (!string.IsNullOrEmpty(stiGetValueEventArgs.Value))
		{
			return StiSeries.GetValuesFromString(stiGetValueEventArgs.Value);
		}
		return null;
	}

	private static string[] GetHyperlinks(StiChart masterChart, StiSeries series)
	{
		string[] hyperlinksFromListOfHyperlinks = GetHyperlinksFromListOfHyperlinks(masterChart, series);
		if (hyperlinksFromListOfHyperlinks != null)
		{
			return hyperlinksFromListOfHyperlinks;
		}
		hyperlinksFromListOfHyperlinks = GetHyperlinksFromHyperlinkDataColumn(masterChart, series);
		if (hyperlinksFromListOfHyperlinks != null)
		{
			return hyperlinksFromListOfHyperlinks;
		}
		hyperlinksFromListOfHyperlinks = GetHyperlinksFromHyperlinkExpression(masterChart, series);
		if (hyperlinksFromListOfHyperlinks != null)
		{
			return hyperlinksFromListOfHyperlinks;
		}
		return new string[0];
	}

	private static string[] GetHyperlinksFromHyperlinkExpression(StiChart masterChart, StiSeries series)
	{
		string[] array = new string[masterChart.Count];
		masterChart.First();
		for (int i = 0; i < masterChart.Count; i++)
		{
			StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
			series.InvokeGetHyperlink(masterChart, stiValueEventArgs);
			if (stiValueEventArgs.Value != null)
			{
				array[i] = stiValueEventArgs.Value.ToString();
			}
			masterChart.Next();
		}
		return array;
	}

	private static string[] GetHyperlinksFromHyperlinkDataColumn(StiChart masterChart, StiSeries series)
	{
		if (series.HyperlinkDataColumn == null || series.HyperlinkDataColumn.Trim().Length <= 0)
		{
			return null;
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.HyperlinkDataColumn);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			string[] array = new string[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.HyperlinkDataColumn);
				if (dataFromDataColumn != null)
				{
					array[i] = dataFromDataColumn.ToString();
				}
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.HyperlinkDataColumn);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			string[] array2 = new string[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.HyperlinkDataColumn);
				if (dataFromBusinessObject != null)
				{
					array2[j] = dataFromBusinessObject.ToString();
				}
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return null;
	}

	private static string[] GetHyperlinksFromListOfHyperlinks(StiChart masterChart, StiSeries series)
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		series.InvokeGetListOfHyperlinks(masterChart, stiGetValueEventArgs);
		if (!string.IsNullOrEmpty(stiGetValueEventArgs.Value))
		{
			return StiSeries.GetStringsFromString(stiGetValueEventArgs.Value);
		}
		return null;
	}

	private static object[] GetTags(StiChart masterChart, StiSeries series)
	{
		object[] tagsFromListOfTags = GetTagsFromListOfTags(masterChart, series);
		if (tagsFromListOfTags != null)
		{
			return tagsFromListOfTags;
		}
		tagsFromListOfTags = GetTagsFromTagDataColumn(masterChart, series);
		if (tagsFromListOfTags != null)
		{
			return tagsFromListOfTags;
		}
		tagsFromListOfTags = GetTagsFromTagExpression(masterChart, series);
		if (tagsFromListOfTags != null)
		{
			return tagsFromListOfTags;
		}
		return new object[0];
	}

	private static object[] GetTagsFromTagExpression(StiChart masterChart, StiSeries series)
	{
		object[] array = new object[masterChart.Count];
		masterChart.First();
		for (int i = 0; i < masterChart.Count; i++)
		{
			StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
			series.InvokeGetTag(masterChart, stiValueEventArgs);
			array[i] = stiValueEventArgs.Value;
			masterChart.Next();
		}
		return array;
	}

	private static object[] GetTagsFromTagDataColumn(StiChart masterChart, StiSeries series)
	{
		if (series.TagDataColumn == null || series.TagDataColumn.Trim().Length <= 0)
		{
			return null;
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.TagDataColumn);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			object[] array = new object[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.TagDataColumn);
				array[i] = dataFromDataColumn;
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.TagDataColumn);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			object[] array2 = new object[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				array2[j] = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.TagDataColumn);
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return null;
	}

	private static object[] GetTagsFromListOfTags(StiChart masterChart, StiSeries series)
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		series.InvokeGetListOfTags(masterChart, stiGetValueEventArgs);
		if (!string.IsNullOrEmpty(stiGetValueEventArgs.Value))
		{
			return StiSeries.GetStringsFromString(stiGetValueEventArgs.Value);
		}
		return null;
	}

	private static string[] GetToolTips(StiChart masterChart, StiSeries series)
	{
		string[] toolTipsFromListOfToolTips = GetToolTipsFromListOfToolTips(masterChart, series);
		if (toolTipsFromListOfToolTips != null)
		{
			return toolTipsFromListOfToolTips;
		}
		toolTipsFromListOfToolTips = GetToolTipsFromToolTipDataColumn(masterChart, series);
		if (toolTipsFromListOfToolTips != null)
		{
			return toolTipsFromListOfToolTips;
		}
		toolTipsFromListOfToolTips = GetToolTipsFromToolTipExpression(masterChart, series);
		if (toolTipsFromListOfToolTips != null)
		{
			return toolTipsFromListOfToolTips;
		}
		return new string[0];
	}

	private static string[] GetToolTipsFromToolTipExpression(StiChart masterChart, StiSeries series)
	{
		string[] array = new string[masterChart.Count];
		masterChart.First();
		for (int i = 0; i < masterChart.Count; i++)
		{
			StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
			series.InvokeGetToolTip(masterChart, stiValueEventArgs);
			if (stiValueEventArgs.Value != null)
			{
				array[i] = stiValueEventArgs.Value.ToString();
			}
			masterChart.Next();
		}
		return array;
	}

	private static string[] GetToolTipsFromToolTipDataColumn(StiChart masterChart, StiSeries series)
	{
		if (series.ToolTipDataColumn == null || series.ToolTipDataColumn.Trim().Length <= 0)
		{
			return null;
		}
		StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(masterChart.Report.Dictionary, series.ToolTipDataColumn);
		if (dataSourceFromDataColumn != null)
		{
			dataSourceFromDataColumn.SaveState("ChartRender_DataColumn");
			string[] array = new string[dataSourceFromDataColumn.Count];
			dataSourceFromDataColumn.First();
			for (int i = 0; i < dataSourceFromDataColumn.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(masterChart.Report.Dictionary, series.ToolTipDataColumn);
				if (dataFromDataColumn != null)
				{
					array[i] = dataFromDataColumn.ToString();
				}
				dataSourceFromDataColumn.Next();
			}
			dataSourceFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(masterChart.Report.Dictionary, series.ToolTipDataColumn);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("ChartRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			string[] array2 = new string[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(masterChart.Report.Dictionary, series.ToolTipDataColumn);
				if (dataFromBusinessObject != null)
				{
					array2[j] = dataFromBusinessObject.ToString();
				}
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("ChartRender_DataColumn");
			return array2;
		}
		return null;
	}

	private static string[] GetToolTipsFromListOfToolTips(StiChart masterChart, StiSeries series)
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		series.InvokeGetListOfToolTips(masterChart, stiGetValueEventArgs);
		if (!string.IsNullOrEmpty(stiGetValueEventArgs.Value))
		{
			return StiSeries.GetStringsFromString(stiGetValueEventArgs.Value);
		}
		return null;
	}

	public static bool GetAnimationСompatibilitySeries(IStiSeries series1, IStiSeries series2)
	{
		StiSeriesAnimationType animationSeriesType = GetAnimationSeriesType(series1);
		StiSeriesAnimationType animationSeriesType2 = GetAnimationSeriesType(series2);
		if (animationSeriesType == StiSeriesAnimationType.None || animationSeriesType2 == StiSeriesAnimationType.None)
		{
			return false;
		}
		return animationSeriesType == animationSeriesType2;
	}

	private static StiSeriesAnimationType GetAnimationSeriesType(IStiSeries series)
	{
		Type type = series.GetType();
		if (type == typeof(StiClusteredColumnSeries))
		{
			return StiSeriesAnimationType.Column;
		}
		if (type == typeof(StiStackedColumnSeries))
		{
			return StiSeriesAnimationType.Column;
		}
		if (type == typeof(StiFullStackedColumnSeries))
		{
			return StiSeriesAnimationType.Column;
		}
		if (type == typeof(StiRangeBarSeries))
		{
			return StiSeriesAnimationType.Column;
		}
		if (type == typeof(StiLineSeries))
		{
			return StiSeriesAnimationType.Line;
		}
		if (type == typeof(StiStackedLineSeries))
		{
			return StiSeriesAnimationType.Line;
		}
		if (type == typeof(StiFullStackedLineSeries))
		{
			return StiSeriesAnimationType.Line;
		}
		if (type == typeof(StiSplineSeries))
		{
			return StiSeriesAnimationType.Line;
		}
		if (type == typeof(StiStackedSplineSeries))
		{
			return StiSeriesAnimationType.Line;
		}
		if (type == typeof(StiFullStackedSplineSeries))
		{
			return StiSeriesAnimationType.Line;
		}
		if (type == typeof(StiSteppedLineSeries))
		{
			return StiSeriesAnimationType.Line;
		}
		if (type == typeof(StiScatterLineSeries))
		{
			return StiSeriesAnimationType.Line;
		}
		if (type == typeof(StiScatterSplineSeries))
		{
			return StiSeriesAnimationType.Line;
		}
		if (type == typeof(StiClusteredBarSeries))
		{
			return StiSeriesAnimationType.Bar;
		}
		if (type == typeof(StiStackedBarSeries))
		{
			return StiSeriesAnimationType.Bar;
		}
		if (type == typeof(StiFullStackedBarSeries))
		{
			return StiSeriesAnimationType.Bar;
		}
		if (type == typeof(StiGanttSeries))
		{
			return StiSeriesAnimationType.Bar;
		}
		if (type == typeof(StiAreaSeries))
		{
			return StiSeriesAnimationType.Range;
		}
		if (type == typeof(StiStackedAreaSeries))
		{
			return StiSeriesAnimationType.Range;
		}
		if (type == typeof(StiFullStackedAreaSeries))
		{
			return StiSeriesAnimationType.Range;
		}
		if (type == typeof(StiSplineAreaSeries))
		{
			return StiSeriesAnimationType.Range;
		}
		if (type == typeof(StiStackedSplineAreaSeries))
		{
			return StiSeriesAnimationType.Range;
		}
		if (type == typeof(StiFullStackedSplineAreaSeries))
		{
			return StiSeriesAnimationType.Range;
		}
		if (type == typeof(StiSteppedAreaSeries))
		{
			return StiSeriesAnimationType.Range;
		}
		if (type == typeof(StiRangeSeries))
		{
			return StiSeriesAnimationType.Range;
		}
		if (type == typeof(StiSplineRangeSeries))
		{
			return StiSeriesAnimationType.Range;
		}
		if (type == typeof(StiSteppedRangeSeries))
		{
			return StiSeriesAnimationType.Range;
		}
		return StiSeriesAnimationType.None;
	}
}
