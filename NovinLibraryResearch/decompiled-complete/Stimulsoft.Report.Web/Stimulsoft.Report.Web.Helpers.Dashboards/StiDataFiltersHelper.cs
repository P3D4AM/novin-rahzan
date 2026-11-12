using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiDataFiltersHelper
{
	public static void ApplyFiltering(StiReport report, Hashtable parameters)
	{
		if (parameters != null && parameters.Count != 0)
		{
			IStiElement element = report.Pages.GetComponentByName(parameters["elementName"] as string) as IStiElement;
			ArrayList filters = parameters["filters"] as ArrayList;
			ApplyFiltersToElement(element, filters);
			StiPivotToContainerCache.Clean(report.Key);
			StiPivotTableToCrossTabCache.Clean(report.Key);
			StiPivotToConvertedStateCache.Clean(report.Key);
		}
	}

	public static void ApplyFiltersToElement(IStiElement element, ArrayList filters)
	{
		if (element == null || filters == null)
		{
			return;
		}
		if (element is IStiDatePickerElement && StiDatePickerElementViewHelper.IsVariablePresent((IStiDatePickerElement)element))
		{
			ApplyDatePickerFiltersToVariable((IStiDatePickerElement)element, filters);
		}
		else
		{
			if (!(element is IStiUserFilters stiUserFilters))
			{
				return;
			}
			(from IStiFilterElement l in from c in element.Page.GetComponents().ToList()
					where c is IStiFilterElement
					select c
				where l.GetParentKey() == element.Key
				select l into c
				where c != null && c is IStiUserFilters
				select c).ToList().ForEach(delegate(IStiFilterElement c)
			{
				((IStiUserFilters)c).UserFilters = new List<StiDataFilterRule>();
			});
			stiUserFilters.UserFilters = new List<StiDataFilterRule>();
			foreach (Hashtable filter in filters)
			{
				StiDataFilterCondition condition = (StiDataFilterCondition)Enum.Parse(typeof(StiDataFilterCondition), filter["condition"] as string);
				stiUserFilters.UserFilters.Add(new StiDataFilterRule(filter["key"] as string, filter["path"] as string, filter["path2"] as string, condition, filter["value"] as string, filter["value2"] as string, isEnabled: true, isExpression: false));
			}
		}
	}

	public static ArrayList GetElementFilters(IStiElement element)
	{
		ArrayList filters = new ArrayList();
		if (element is IStiUserFilters { UserFilters: not null } stiUserFilters)
		{
			stiUserFilters.UserFilters.ForEach(delegate(StiDataFilterRule f)
			{
				filters.Add(FilterRuleItem(f));
			});
		}
		return filters;
	}

	public static ArrayList GetDrillDownFilters(IStiDrillDownElement drillDownElement)
	{
		ArrayList filters = new ArrayList();
		if (drillDownElement != null && drillDownElement.DrillDownFilters != null)
		{
			drillDownElement.DrillDownFilters.ForEach(delegate(StiDataFilterRule f)
			{
				filters.Add(FilterRuleItem(f));
			});
		}
		return filters;
	}

	public static ArrayList GetDrillDownFiltersList(IStiDrillDownElement drillDownElement)
	{
		ArrayList filtersList = new ArrayList();
		if (drillDownElement != null && drillDownElement.DrillDownFiltersList != null)
		{
			drillDownElement.DrillDownFiltersList.ForEach(delegate(List<StiDataFilterRule> fs)
			{
				ArrayList filters_ = new ArrayList();
				filtersList.Add(filters_);
				fs.ForEach(delegate(StiDataFilterRule f)
				{
					filters_.Add(FilterRuleItem(f));
				});
			});
		}
		return filtersList;
	}

	public static Hashtable GetFilterItems(StiReport report, StiRequestParams requestParams)
	{
		string componentName = requestParams.Interaction.DashboardFiltering["elementName"] as string;
		int columnIndex = Convert.ToInt32(requestParams.Interaction.DashboardFiltering["columnIndex"]);
		if (report.Pages.GetComponentByName(componentName) is IStiElement stiElement)
		{
			List<IStiMeter> meters = (from m in stiElement.GetMeters().AsEnumerable()
				where m is IStiTableColumn
				select m).ToList();
			return GetFilterItemsHelper(stiElement.Page as IStiDashboard, meters, columnIndex, (stiElement as IStiUserSorts)?.UserSorts, (stiElement as IStiUserFilters)?.UserFilters, stiElement);
		}
		return null;
	}

	private static StiDataTable GetDataTable(IStiElement element)
	{
		StiDataTable stiDataTable = (element as IStiManuallyEnteredData)?.GetManuallyEnteredDataTable();
		if (stiDataTable != null)
		{
			return stiDataTable;
		}
		return StiElementDataCache.TryToGetOrCreate(element);
	}

	public static Hashtable GetViewData(StiReport report, StiRequestParams requestParams)
	{
		string componentName = requestParams.Interaction.DashboardFiltering["elementName"] as string;
		IStiElement stiElement = report.Pages.GetComponentByName(componentName) as IStiElement;
		IStiElement stiElement2 = (stiElement as StiComponent)?.Clone() as IStiElement;
		Hashtable hashtable = new Hashtable();
		List<object> list = new List<object>();
		List<object> list2 = new List<object>();
		List<object> list3 = new List<object>();
		if (stiElement != null && stiElement2 != null)
		{
			StiElementStyleIdent style = StiDashboardStyleHelper.GetStyle(stiElement);
			StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(style);
			IStiTableElement stiTableElement = StiActivator.CreateObject("Stimulsoft.Dashboard.Components.Table.StiTableElement") as IStiTableElement;
			stiTableElement.Style = tableStyle.Ident;
			hashtable["settings"] = StiTableElementViewHelper.GetTableSettings(stiTableElement);
			stiElement2.Report = stiElement.Report;
			if (stiElement2 is IStiUserSorts)
			{
				((IStiUserSorts)stiElement2).UserSorts = new List<StiDataSortRule>();
			}
			StiDataTable dataTable = GetDataTable(stiElement2);
			if (dataTable != null)
			{
				DataTable dataTable2 = RemoveSystemMeters(stiElement2, dataTable);
				string text = requestParams.Interaction.DashboardFiltering["sortDirection"] as string;
				int num = ((requestParams.Interaction.DashboardFiltering["columnIndex"] != null) ? Convert.ToInt32(requestParams.Interaction.DashboardFiltering["columnIndex"]) : (-1));
				if (num >= 0 && text != null && num < dataTable2.Columns.Count)
				{
					string columnName = dataTable2.Columns[num].ColumnName;
					dataTable2.DefaultView.Sort = "[" + columnName + "] " + ((text == "Ascending") ? "Asc" : "Desc");
					dataTable2 = dataTable2.DefaultView.ToTable(true);
				}
				int num2 = 0;
				foreach (DataColumn column in dataTable2.Columns)
				{
					Hashtable hashtable2 = new Hashtable();
					hashtable2["columnIndex"] = num2;
					hashtable2["label"] = column.Caption;
					hashtable2["labelCorrect"] = column.Caption;
					hashtable2["dataType"] = TypeToString(column.DataType);
					hashtable2["align"] = (column.DataType.IsNumericType() ? "Left" : "Right");
					hashtable2["cellForeColor"] = StiReportHelper.GetHtmlColor(tableStyle.CellForeColor);
					hashtable2["cellForeColorInterlaced"] = StiReportHelper.GetHtmlColor(tableStyle.AlternatingCellForeColor);
					if (num >= 0 && text != null && num2 == num)
					{
						hashtable2["sortLabel"] = new Hashtable { ["direction"] = text };
					}
					list3.Add(hashtable2);
					int num3 = 0;
					foreach (DataRow row in dataTable2.Rows)
					{
						if (list2.Count < dataTable2.Rows.Count)
						{
							list2.Add(new List<object>());
						}
						Hashtable hashtable3 = new Hashtable();
						hashtable3["text"] = row[num2];
						((List<object>)list2[num3]).Add(hashtable3);
						num3++;
					}
					num2++;
				}
				if (list3.Count > 0)
				{
					list.Add(list3);
				}
				if (list2.Count > 0)
				{
					list.AddRange(list2);
				}
				hashtable["data"] = list;
			}
		}
		return hashtable;
	}

	private static DataTable RemoveSystemMeters(IStiElement element, StiDataTable dataTable)
	{
		DataTable table = dataTable.ToNetTable();
		if (element is IStiGaugeElement)
		{
			IStiMeter stiMeter = dataTable.Meters.FirstOrDefault((IStiMeter m) => m is IStiMinGaugeMeter);
			IStiMeter stiMeter2 = dataTable.Meters.FirstOrDefault((IStiMeter m) => m is IStiMaxGaugeMeter);
			int num = ((stiMeter != null) ? dataTable.Meters.IndexOf(stiMeter) : (-1));
			int num2 = ((stiMeter2 != null) ? dataTable.Meters.IndexOf(stiMeter2) : (-1));
			if (num != -1)
			{
				table.Columns.RemoveAt(num);
			}
			if (num2 != -1)
			{
				if (num2 > num)
				{
					num2--;
				}
				table.Columns.RemoveAt(num2);
			}
		}
		List<int> list = new List<int>();
		for (int j = 0; j < table.Columns.Count; j++)
		{
			if (table.Columns[j].DataType.IsGenericType)
			{
				list.Add(j);
			}
		}
		list.Reverse();
		list.ForEach(delegate(int i)
		{
			table.Columns.RemoveAt(i);
		});
		return table;
	}

	public static string GetDataTableFilterQueryStringRepresentation(IStiElement element)
	{
		if (!(element is IStiUserFilters stiUserFilters))
		{
			return "";
		}
		List<StiDataFilterRule> list = stiUserFilters.UserFilters.Select((StiDataFilterRule f) => f.Clone()).Cast<StiDataFilterRule>().ToList();
		list.ForEach(delegate(StiDataFilterRule f)
		{
			int num = ((f.Path != null) ? f.Path.IndexOfInvariant(".") : (-1));
			if (num != -1)
			{
				f.Path = f.Path.Substring(num + 1);
			}
			DateTime? dateTime = StiValueHelper.TryToNullableDateTime(f.Value);
			if (dateTime.HasValue)
			{
				f.Value = dateTime.Value.ToShortDateString();
			}
			DateTime? dateTime2 = StiValueHelper.TryToNullableDateTime(f.Value2);
			if (dateTime2.HasValue)
			{
				f.Value2 = dateTime2.Value.ToShortDateString();
			}
		});
		return StiDataFilterRuleHelper.GetDataTableFilterQuery(list, null, null, null);
	}

	public static bool IsBlankData(object data)
	{
		if (data != null && data != DBNull.Value)
		{
			if (data is string)
			{
				return string.IsNullOrEmpty(data as string);
			}
			return false;
		}
		return true;
	}

	public static bool IsStringColumnType(IStiElement element)
	{
		string column = null;
		if (element is IStiListBoxElement)
		{
			column = StiListBoxElementViewHelper.GetColumnPath(element as IStiListBoxElement);
		}
		else if (element is IStiComboBoxElement)
		{
			column = StiComboBoxElementViewHelper.GetColumnPath(element as IStiComboBoxElement);
		}
		else if (element is IStiTreeViewElement)
		{
			column = StiTreeViewElementViewHelper.GetColumnPath(element as IStiTreeViewElement);
		}
		else if (element is IStiTreeViewBoxElement)
		{
			column = StiTreeViewBoxElementViewHelper.GetColumnPath(element as IStiTreeViewBoxElement);
		}
		return StiDataColumn.GetDataColumnFromColumnName(element.Report.Dictionary, column)?.Type == typeof(string);
	}

	public static Hashtable FilterRuleItem(StiDataFilterRule filterRule)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["typeItem"] = "FilterRule";
		hashtable["key"] = filterRule.Key;
		hashtable["path"] = filterRule.Path;
		hashtable["condition"] = filterRule.Condition;
		hashtable["value"] = filterRule.Value;
		hashtable["value2"] = filterRule.Value2;
		hashtable["isEnabled"] = filterRule.IsEnabled;
		hashtable["isExpression"] = filterRule.IsExpression;
		return hashtable;
	}

	private static Hashtable SortFilterMenuItem(object item, Type type)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["type"] = TypeToString(type);
		hashtable["displayString"] = ToDisplayString(item, type);
		hashtable["filterString"] = ToFilterString(item, type);
		return hashtable;
	}

	public static Hashtable GetFilterItemsHelper(IStiQueryObject query, List<IStiMeter> meters, int columnIndex, List<StiDataSortRule> sorts, List<StiDataFilterRule> filters, IStiElement element = null)
	{
		Hashtable hashtable = new Hashtable();
		if (meters == null || columnIndex >= meters.Count || meters[columnIndex] == null)
		{
			return null;
		}
		if (!(StiDataExpressionHelper.GetDataColumnFromExpression(query, meters[columnIndex].Expression) is StiDataColumn stiDataColumn))
		{
			return null;
		}
		DataTable fromCache = StiDataPicker.GetFromCache(stiDataColumn.DataSource);
		if (fromCache == null)
		{
			return null;
		}
		string text = stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
		int dataColumnIndex = fromCache.Columns.IndexOf(text);
		if (dataColumnIndex == -1)
		{
			return null;
		}
		hashtable["columnPath"] = text;
		Type columnType = fromCache.Columns[dataColumnIndex].DataType;
		StiDataFilterComparer comparer = new StiDataFilterComparer();
		List<object> source = Distinct(from r in fromCache.AsEnumerable()
			select r[dataColumnIndex]).Where(IsValueCanBeFiltered).OrderBy((object c) => c, comparer).ToList();
		hashtable["mainItems"] = (from i in source
			where i != null && i != DBNull.Value && !string.IsNullOrWhiteSpace(i.ToString())
			select SortFilterMenuItem(i, columnType)).ToList();
		hashtable["haveBlanks"] = source.Any((object i) => i != null && i != DBNull.Value && string.IsNullOrWhiteSpace(i.ToString()));
		hashtable["haveNulls"] = source.Any((object i) => i == null || i == DBNull.Value);
		return hashtable;
	}

	public static string TypeToString(Type type)
	{
		if (type == null)
		{
			return "null";
		}
		if (type == typeof(bool))
		{
			return "bool";
		}
		if (type == typeof(byte))
		{
			return "byte";
		}
		if (type == typeof(byte[]))
		{
			return "byte[]";
		}
		if (type == typeof(char))
		{
			return "char";
		}
		if (type == typeof(DateTime))
		{
			return "datetime";
		}
		if (type == typeof(decimal))
		{
			return "decimal";
		}
		if (type == typeof(double))
		{
			return "double";
		}
		if (type == typeof(Guid))
		{
			return "guid";
		}
		if (type == typeof(short))
		{
			return "short";
		}
		if (type == typeof(int))
		{
			return "int";
		}
		if (type == typeof(long))
		{
			return "long";
		}
		if (type == typeof(sbyte))
		{
			return "sbyte";
		}
		if (type == typeof(float))
		{
			return "float";
		}
		if (type == typeof(string))
		{
			return "string";
		}
		if (type == typeof(TimeSpan))
		{
			return "timespan";
		}
		if (type == typeof(ushort))
		{
			return "ushort";
		}
		if (type == typeof(uint))
		{
			return "uint";
		}
		if (type == typeof(ulong))
		{
			return "ulong";
		}
		if (type == typeof(Image))
		{
			return "image";
		}
		if (type == typeof(bool?))
		{
			return "bool (Nullable)";
		}
		if (type == typeof(byte?))
		{
			return "byte (Nullable)";
		}
		if (type == typeof(char?))
		{
			return "char (Nullable)";
		}
		if (type == typeof(DateTime?))
		{
			return "datetime (Nullable)";
		}
		if (type == typeof(decimal?))
		{
			return "decimal (Nullable)";
		}
		if (type == typeof(double?))
		{
			return "double (Nullable)";
		}
		if (type == typeof(Guid?))
		{
			return "guid (Nullable)";
		}
		if (type == typeof(short?))
		{
			return "short (Nullable)";
		}
		if (type == typeof(int?))
		{
			return "int (Nullable)";
		}
		if (type == typeof(long?))
		{
			return "long (Nullable)";
		}
		if (type == typeof(sbyte?))
		{
			return "sbyte (Nullable)";
		}
		if (type == typeof(float?))
		{
			return "float (Nullable)";
		}
		if (type == typeof(TimeSpan?))
		{
			return "timespan (Nullable)";
		}
		if (type == typeof(ushort?))
		{
			return "ushort (Nullable)";
		}
		if (type == typeof(uint?))
		{
			return "uint (Nullable)";
		}
		if (type == typeof(ulong?))
		{
			return "ulong (Nullable)";
		}
		if (type == typeof(object))
		{
			return "object";
		}
		return type.ToString();
	}

	public static string ToFilterString(object value, Type type = null)
	{
		if (value == DBNull.Value || value == null)
		{
			return null;
		}
		if (type == null)
		{
			type = value.GetType();
		}
		if (string.IsNullOrWhiteSpace(value.ToString()))
		{
			return string.Empty;
		}
		if (type != null && type.IsEnum && Enum.IsDefined(type, value))
		{
			return Enum.GetName(type, value);
		}
		if (type != null && type.IsNumericType())
		{
			return value.ToString().Replace(",", ".");
		}
		if (value is DateTime dateTime)
		{
			return dateTime.ToString("MM'/'dd'/'yyyy");
		}
		return value.ToString();
	}

	public static string ToDisplayString(object value, Type type = null)
	{
		if (type == null)
		{
			type = value.GetType();
		}
		if (value == DBNull.Value || value == null || string.IsNullOrWhiteSpace(value.ToString()))
		{
			return string.Empty;
		}
		if (type.IsEnum && Enum.IsDefined(type, value))
		{
			return Enum.GetName(type, value);
		}
		if (value is DateTime dateTime)
		{
			return dateTime.ToShortDateString();
		}
		string text = value.ToString();
		if (text.Length <= 100)
		{
			return text;
		}
		return text.Substring(0, 100) + "...";
	}

	private static IEnumerable<object> Distinct(IEnumerable<object> items)
	{
		if (!(items.FirstOrDefault() is DateTime))
		{
			return items.Distinct();
		}
		return (from DateTime x in from d in items.Select(StiValueHelper.TryToNullableDateTime)
				where d.HasValue
				select d
			group x by x.Date into x
			select x.Key).Cast<object>().Distinct();
	}

	private static bool IsValueCanBeFiltered(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return true;
		}
		Type type = value.GetType();
		if (!type.IsNumericType() && !type.IsDateType() && !(type == typeof(string)))
		{
			return type == typeof(bool);
		}
		return true;
	}

	private static int GetLevel(IStiFilterElement element, List<IStiFilterElement> elements)
	{
		if (StiCrossLinkedFilterHelper.IsCrossLinkedFilter(element))
		{
			return 0;
		}
		int num = 0;
		while (true)
		{
			if (string.IsNullOrWhiteSpace(element.GetParentKey()))
			{
				return num;
			}
			element = elements.FirstOrDefault((IStiFilterElement e) => e.GetKey() == element.GetParentKey());
			if (element == null)
			{
				break;
			}
			num++;
		}
		return num;
	}

	public static void ApplyDefaultFiltersForFilterElements(StiReport report)
	{
		List<StiPage> list = (from p in report?.Pages.OfType<StiPage>()
			where p.Enabled && p.IsDashboard
			select p).ToList();
		foreach (StiPage item in list)
		{
			List<IStiFilterElement> filterElements = (from c in item.GetComponents().ToList()
				where c.IsEnabled && c is IStiFilterElement
				select c).Cast<IStiFilterElement>().ToList();
			List<IStiFilterElement> list2 = filterElements?.OrderBy((IStiFilterElement f) => GetLevel(f, filterElements)).ToList();
			if (list2.Any())
			{
				list2.ForEach(delegate(IStiFilterElement f)
				{
					f.ApplyDefaultFilters();
				});
			}
		}
	}

	private static void ApplyDatePickerFiltersToVariable(IStiDatePickerElement datePickerElement, ArrayList filters)
	{
		if (!(StiVariableExpressionHelper.GetVariableSpecifiedAsExpression(datePickerElement, datePickerElement.GetValueMeter()?.Expression) is StiVariable stiVariable))
		{
			return;
		}
		foreach (Hashtable filter in filters)
		{
			object obj = ((stiVariable.Type == typeof(DateTimeRange)) ? new DateTimeRange(TryToDateTime(filter["value"] as string), TryToDateTime(filter["value2"] as string)) : ((object)TryToDateTime(filter["value"] as string)));
			StiVariableHelper.SetVariableValue(datePickerElement.Report, stiVariable, obj);
			if (stiVariable.InitBy == StiVariableInitBy.Value)
			{
				stiVariable.ValueObject = obj;
			}
			StiCacheCleaner.Clean(datePickerElement.Report.Key);
		}
	}

	internal static DateTime TryToDateTime(string stringValue)
	{
		DateTime result = DateTime.Now;
		DateTime.TryParse(stringValue, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out result);
		return result;
	}
}
