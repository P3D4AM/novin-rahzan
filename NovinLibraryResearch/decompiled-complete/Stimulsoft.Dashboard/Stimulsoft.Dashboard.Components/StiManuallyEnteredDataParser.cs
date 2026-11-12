using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Components.Gauge;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Components;

public class StiManuallyEnteredDataParser
{
	public const string ColumnValue = "Value";

	public const string ColumnEndValue = "EndValue";

	public const string ColumnTarget = "Target";

	public const string ColumnCloseValue = "CloseValue";

	public const string ColumnLowValue = "LowValue";

	public const string ColumnHighValue = "HighValue";

	public const string ColumnArgument = "Argument";

	public const string ColumnWeight = "Weight";

	public const string ColumnSeries = "Series";

	public const string ColumnLocation = "Location";

	public const string ColumnLongitude = "Longitude";

	public const string ColumnLatitude = "Latitude";

	public const string ColumnColor = "Color";

	public static StiDataTable GetIndicatorDataTable(string content, IStiElement element)
	{
		List<string[]> list = StiStringsTableHelper.LoadStringRowsFromCache(element);
		if (list == null)
		{
			list = StiStringsTableHelper.LoadStringRowsFromPackedString(content, "Value", "Target", "Series");
		}
		List<IStiMeter> meters = new List<IStiMeter>
		{
			new StiValueIndicatorMeter
			{
				Label = Loc.GetMain("Value")
			},
			new StiTargetIndicatorMeter
			{
				Label = Loc.GetMain("Target")
			},
			new StiSeriesIndicatorMeter
			{
				Label = Loc.Get("Chart", "Series")
			}
		};
		if (list == null)
		{
			return new StiDataTable(meters, new List<object[]>());
		}
		IEnumerable<object[]> source = from r in list
			group r by r[2] into g
			select new object[3]
			{
				g.Sum((string[] r) => ParseDecimal(r[0], element)),
				SumNullable(g.Select((string[] r) => ParseNullableDecimal(r[1], element)).ToList()),
				ParseString(g.Key, element)
			};
		return new StiDataTable(meters, source.ToList());
	}

	public static StiDataTable GetProgressDataTable(string content, IStiElement element)
	{
		List<string[]> list = StiStringsTableHelper.LoadStringRowsFromCache(element);
		if (list == null)
		{
			list = StiStringsTableHelper.LoadStringRowsFromPackedString(content, "Value", "Target", "Series");
		}
		List<IStiMeter> meters = new List<IStiMeter>
		{
			new StiValueProgressMeter
			{
				Label = Loc.GetMain("Value")
			},
			new StiTargetProgressMeter
			{
				Label = Loc.GetMain("Target")
			},
			new StiSeriesProgressMeter
			{
				Label = Loc.Get("Chart", "Series")
			}
		};
		if (list == null)
		{
			return new StiDataTable(meters, new List<object[]>());
		}
		IEnumerable<object[]> source = from r in list
			group r by r[2] into g
			select new object[3]
			{
				g.Sum((string[] r) => ParseDecimal(r[0], element)),
				SumNullable(g.Select((string[] r) => ParseNullableDecimal(r[1], element)).ToList()),
				ParseString(g.Key, element)
			};
		return new StiDataTable(meters, source.ToList());
	}

	public static StiDataTable GetGaugeDataTable(string content, IStiElement element)
	{
		List<string[]> list = StiStringsTableHelper.LoadStringRowsFromCache(element);
		if (list == null)
		{
			list = StiStringsTableHelper.LoadStringRowsFromPackedString(content, "Value", "Target", "Series");
		}
		List<IStiMeter> meters = new List<IStiMeter>
		{
			new StiValueGaugeMeter
			{
				Label = Loc.GetMain("Value")
			},
			new StiTargetGaugeMeter
			{
				Label = Loc.GetMain("Target")
			},
			new StiSeriesGaugeMeter
			{
				Label = Loc.Get("Chart", "Series")
			}
		};
		if (list == null)
		{
			return new StiDataTable(meters, new List<object[]>());
		}
		IEnumerable<object[]> source = from r in list
			group r by r[2] into g
			select new object[3]
			{
				g.Sum((string[] r) => ParseDecimal(r[0], element)),
				SumNullable(g.Select((string[] r) => ParseNullableDecimal(r[1], element)).ToList()),
				ParseString(g.Key, element)
			};
		return new StiDataTable(meters, source.ToList());
	}

	public static StiDataTable GetChartDataTable(string content, IStiElement element)
	{
		StiChartElement stiChartElement = element as StiChartElement;
		string[] columns;
		List<string[]> list = StiStringsTableHelper.LoadColumnsAndStringRowsFromCache(element, out columns);
		if (list == null)
		{
			list = StiStringsTableHelper.LoadColumnsAndStringRowsFromPackedString(content, out columns);
		}
		bool isBubbleChart = stiChartElement.IsBubbleChart;
		bool isRange = stiChartElement.IsRange;
		bool isFinancial = stiChartElement.IsFinancial;
		StiValueChartMeter manuallyEnteredChartMeter = stiChartElement.ManuallyEnteredChartMeter;
		List<string> list2 = new List<string>();
		List<IStiMeter> list3 = new List<IStiMeter>();
		string[] array = columns;
		foreach (string text in array)
		{
			int count = list3.Count;
			switch (text)
			{
			case "Value":
				if (isBubbleChart)
				{
					list3.Add(new StiYChartMeter
					{
						Expression = "T.Value",
						Label = "Y",
						Key = "Value"
					});
				}
				else if (isRange)
				{
					list3.Add(new StiStartValueChartMeter
					{
						Expression = "Sum(T.Value)",
						SeriesType = manuallyEnteredChartMeter.SeriesType,
						Label = Loc.GetMain("Values"),
						Key = "Value"
					});
				}
				else if (isFinancial)
				{
					list3.Add(new StiOpenValueChartMeter
					{
						Expression = "Sum(T.Value)",
						SeriesType = manuallyEnteredChartMeter.SeriesType,
						Label = Loc.GetMain("OpenValues"),
						Key = "Value"
					});
				}
				else
				{
					StiValueChartMeter stiValueChartMeter = manuallyEnteredChartMeter.Clone() as StiValueChartMeter;
					stiValueChartMeter.Expression = "Sum(T.Value)";
					stiValueChartMeter.Label = Loc.GetMain("Values");
					stiValueChartMeter.Key = "Value";
					stiValueChartMeter.ShowNulls = StiEmptyCellsAs.ConnectPointsWithLine;
					stiValueChartMeter.ShowZeros = StiEmptyCellsAs.ConnectPointsWithLine;
					list3.Add(stiValueChartMeter);
				}
				break;
			case "EndValue":
				if (isRange)
				{
					list3.Add(new StiEndValueChartMeter
					{
						Expression = "Sum(T.EndValue)",
						Label = Loc.GetMain("EndValues"),
						Key = "EndValue"
					});
				}
				break;
			case "CloseValue":
				if (isFinancial)
				{
					list3.Add(new StiCloseValueChartMeter
					{
						Expression = "Sum(T.CloseValue)",
						Label = Loc.GetMain("CloseValues"),
						Key = "CloseValue"
					});
				}
				break;
			case "LowValue":
				if (isFinancial)
				{
					list3.Add(new StiLowValueChartMeter
					{
						Expression = "Sum(T.LowValue)",
						Label = Loc.GetMain("LowValues"),
						Key = "LowValue"
					});
				}
				break;
			case "HighValue":
				if (isFinancial)
				{
					list3.Add(new StiHighValueChartMeter
					{
						Expression = "Sum(T.HighValue)",
						Label = Loc.GetMain("HighValues"),
						Key = "HighValue"
					});
				}
				break;
			case "Argument":
				if (isBubbleChart)
				{
					list3.Add(new StiXChartMeter
					{
						Expression = "T.Argument",
						Label = "X",
						Key = "Argument"
					});
				}
				else
				{
					list3.Add(new StiArgumentChartMeter
					{
						Expression = "T.Argument",
						Label = Loc.GetMain("Arguments"),
						Key = "Argument"
					});
				}
				break;
			case "Weight":
				if (isBubbleChart)
				{
					list3.Add(new StiWeightChartMeter
					{
						Expression = "Sum(T.Weight)",
						Label = Loc.GetMain("Weights"),
						Key = "Weight"
					});
				}
				break;
			case "Series":
				list3.Add(new StiSeriesChartMeter
				{
					Expression = "T.Series",
					Label = Loc.GetMain("Series"),
					Key = "Series"
				});
				break;
			}
			if (list3.Count != count)
			{
				list2.Add(text);
			}
		}
		if (list == null || !list.Any())
		{
			return new StiDataTable(list3, new List<object[]>());
		}
		DataTable dataTable = new DataTable
		{
			TableName = "T"
		};
		foreach (string[] item in list)
		{
			object[] array2 = new object[list2.Count];
			int num = 0;
			string[] array3 = columns;
			foreach (string text2 in array3)
			{
				object obj = null;
				switch (text2)
				{
				case "Value":
					obj = ParseDecimal(item[num], element);
					break;
				case "EndValue":
					obj = ParseDecimal(item[num], element);
					break;
				case "CloseValue":
					obj = ParseDecimal(item[num], element);
					break;
				case "LowValue":
					obj = ParseDecimal(item[num], element);
					break;
				case "HighValue":
					obj = ParseDecimal(item[num], element);
					break;
				case "Argument":
					obj = ((!isBubbleChart) ? ParseString(item[num], element) : ((object)ParseDecimal(item[num], element)));
					break;
				case "Weight":
					obj = ParseDecimal(item[num], element);
					break;
				case "Series":
					obj = ParseString(item[num], element);
					break;
				}
				int num2 = list2.IndexOf(text2);
				if (num2 != -1)
				{
					if (!dataTable.Columns.Contains("T." + text2))
					{
						dataTable.Columns.Add("T." + text2, (obj is string) ? typeof(string) : typeof(decimal));
					}
					array2[num2] = obj;
				}
				num++;
			}
			dataTable.Rows.Add(array2);
		}
		StiDataTable inTable = StiDataGrouper.Group(element.GetDictionary(), dataTable, list3.ToList());
		return StiDataSorter.Sort(inTable, stiChartElement.UserSorts, stiChartElement.Report, 0);
	}

	public static StiDataTable GetOnlineMapDataTable(string content, IStiElement element)
	{
		StiOnlineMapElement stiOnlineMapElement = element as StiOnlineMapElement;
		string[] columns;
		List<string[]> list = StiStringsTableHelper.LoadColumnsAndStringRowsFromCache(element, out columns);
		if (list == null)
		{
			list = StiStringsTableHelper.LoadColumnsAndStringRowsFromPackedString(content, out columns);
		}
		List<string> list2 = new List<string>();
		List<IStiMeter> list3 = new List<IStiMeter>();
		string[] array = columns;
		foreach (string text in array)
		{
			int count = list3.Count;
			if (stiOnlineMapElement.MapMode == StiOnlineMapMode.Location)
			{
				switch (text)
				{
				case "Location":
					list3.Add(new StiLocationMapMeter
					{
						Expression = "T.Location",
						Label = Loc.GetMain("Location")
					});
					break;
				case "Value":
					list3.Add(new StiLocationValueMapMeter
					{
						Expression = "T.Value",
						Label = Loc.GetMain("Value")
					});
					break;
				case "Color":
					list3.Add(new StiLocationColorMapMeter
					{
						Expression = "T.Color",
						Label = GetLocLocationColor(stiOnlineMapElement)
					});
					break;
				}
			}
			else if (!(text == "Latitude"))
			{
				if (text == "Longitude")
				{
					list3.Add(new StiLongitudeMapMeter
					{
						Expression = "T.Longitude",
						Label = Loc.GetMain("Longitude")
					});
				}
			}
			else
			{
				list3.Add(new StiLatitudeMapMeter
				{
					Expression = "T.Latitude",
					Label = Loc.GetMain("Latitude")
				});
			}
			if (list3.Count != count)
			{
				list2.Add(text);
			}
		}
		if (list == null || !list.Any())
		{
			return new StiDataTable(list3, new List<object[]>());
		}
		DataTable dataTable = new DataTable
		{
			TableName = "T"
		};
		foreach (string[] item in list)
		{
			object[] array2 = new object[list2.Count];
			int num = 0;
			string[] array3 = columns;
			foreach (string text2 in array3)
			{
				object obj = null;
				if (stiOnlineMapElement.MapMode == StiOnlineMapMode.Location)
				{
					switch (text2)
					{
					case "Location":
						obj = ParseString(item[num], element);
						break;
					case "Value":
						obj = ParseDecimal(item[num], element);
						break;
					case "Color":
						obj = ParseString(item[num], element);
						break;
					}
				}
				else if (!(text2 == "Latitude"))
				{
					if (text2 == "Longitude")
					{
						obj = ParseString(item[num], element);
					}
				}
				else
				{
					obj = ParseString(item[num], element);
				}
				int num2 = list2.IndexOf(text2);
				if (num2 != -1)
				{
					if (!dataTable.Columns.Contains("T." + text2))
					{
						dataTable.Columns.Add("T." + text2, (obj is string) ? typeof(string) : typeof(decimal));
					}
					array2[num2] = obj;
				}
				num++;
			}
			dataTable.Rows.Add(array2);
		}
		return StiDataGrouper.Group(element.GetDictionary(), dataTable, list3.ToList());
	}

	private static decimal? SumNullable(IEnumerable<decimal?> values)
	{
		if (values == null)
		{
			return null;
		}
		values = values.Where((decimal? v) => v.HasValue);
		if (!values.Any())
		{
			return null;
		}
		return values.Sum();
	}

	private static decimal ParseDecimal(string str, IStiElement element)
	{
		if (str != null && str.Contains("{"))
		{
			return StiValueHelper.TryToDecimal(StiReportParser.Parse(str, element.Report));
		}
		return StiValueHelper.TryToDecimal(str);
	}

	private static decimal? ParseNullableDecimal(string str, IStiElement element)
	{
		if (str != null && str.Contains("{"))
		{
			str = StiReportParser.Parse(str, element.Report);
		}
		if (!string.IsNullOrWhiteSpace(str))
		{
			return StiValueHelper.TryToDecimal(str);
		}
		return null;
	}

	public static string ParseString(object value, IStiElement element)
	{
		if (value == null)
		{
			return "";
		}
		string text = ((value is string) ? (value as string) : value.ToString());
		if (text != null && text.Contains("{"))
		{
			return StiReportParser.Parse(text, element.Report);
		}
		return text;
	}

	private static string GetLocLocationColor(StiOnlineMapElement element)
	{
		if (element != null && element.LocationColorType == StiOnlineMapLocationColorType.Group)
		{
			return Loc.GetMain("Color") + " " + Loc.GetMain("Group");
		}
		if (element != null && element.LocationColorType == StiOnlineMapLocationColorType.Value)
		{
			return Loc.GetMain("Color") + " " + Loc.GetMain("Value");
		}
		return Loc.GetMain("Color");
	}
}
