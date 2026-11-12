using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiDataSortsHelper
{
	public static void ApplySorting(StiReport report, Hashtable parameters)
	{
		if (parameters != null && parameters.Count != 0)
		{
			IStiElement element = report.Pages.GetComponentByName(parameters["elementName"] as string) as IStiElement;
			ArrayList sorts = parameters["sorts"] as ArrayList;
			ApplySortsToElement(element, sorts);
		}
	}

	public static void ApplySortsToElement(IStiElement element, ArrayList sorts)
	{
		if (element == null || sorts == null || !(element is IStiUserSorts stiUserSorts))
		{
			return;
		}
		stiUserSorts.UserSorts = new List<StiDataSortRule>();
		foreach (Hashtable sort in sorts)
		{
			StiDataSortDirection direction = (StiDataSortDirection)Enum.Parse(typeof(StiDataSortDirection), sort["direction"] as string);
			stiUserSorts.UserSorts.Add(new StiDataSortRule(sort["key"] as string, direction));
		}
	}

	public static ArrayList GetElementSorts(IStiElement element)
	{
		ArrayList arrayList = new ArrayList();
		if (element is IStiUserSorts stiUserSorts)
		{
			foreach (StiDataSortRule userSort in stiUserSorts.UserSorts)
			{
				arrayList.Add(SortRuleItem(userSort));
			}
		}
		return arrayList;
	}

	public static Hashtable SortRuleItem(StiDataSortRule sortRule)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["typeItem"] = "SortRule";
		hashtable["key"] = sortRule.Key;
		hashtable["direction"] = sortRule.Direction;
		return hashtable;
	}

	public static ArrayList GetSortMenuItems(IStiElement element)
	{
		ArrayList items = new ArrayList();
		IStiAllowUserSortingDashboardInteraction stiAllowUserSortingDashboardInteraction = (element as IStiElementInteraction)?.DashboardInteraction as IStiAllowUserSortingDashboardInteraction;
		List<StiDataSortRule> userSorts = (element as IStiUserSorts)?.UserSorts;
		string sortDirection = GetSortDirection(userSorts).ToString();
		if (stiAllowUserSortingDashboardInteraction != null && stiAllowUserSortingDashboardInteraction.AllowUserSorting && StiSortMenuHelper.IsAllowUserSorting(element))
		{
			StiDataTable manualDataTable = GetManualDataTable(element);
			List<IStiMeter> list = FetchAllArguments(element, manualDataTable);
			if (list != null)
			{
				IEnumerable<string> enumerable = from a in list
					select StiLabelHelper.GetLabel(a) into a
					where !string.IsNullOrWhiteSpace(a)
					select a;
				if (enumerable.Any())
				{
					IStiMeter argument = list.FirstOrDefault();
					Hashtable hashtable = new Hashtable();
					hashtable["text"] = string.Join(", ", enumerable);
					hashtable["checked"] = userSorts != null && userSorts.Any((StiDataSortRule r) => r.Key == argument.Key);
					hashtable["key"] = argument.Key;
					hashtable["sortDirection"] = sortDirection;
					items.Add(hashtable);
				}
			}
			FetchAllValues(element, manualDataTable)?.ForEach(delegate(IStiMeter value)
			{
				string label2 = StiLabelHelper.GetLabel(value);
				if (!string.IsNullOrWhiteSpace(label2))
				{
					Hashtable value2 = new Hashtable
					{
						["text"] = label2,
						["checked"] = userSorts != null && userSorts.Any((StiDataSortRule r) => r.Key == value.Key),
						["key"] = value.Key,
						["sortDirection"] = sortDirection
					};
					items.Add(value2);
				}
			});
			IStiMeter series = GetSeries(element, manualDataTable);
			if (series != null)
			{
				string label = StiLabelHelper.GetLabel(series);
				if (!string.IsNullOrWhiteSpace(label))
				{
					Hashtable hashtable2 = new Hashtable();
					hashtable2["text"] = label;
					hashtable2["checked"] = userSorts != null && userSorts.Any((StiDataSortRule r) => r.Key == series.Key);
					hashtable2["key"] = series.Key;
					hashtable2["sortDirection"] = sortDirection;
					items.Add(hashtable2);
				}
			}
			if (element is IStiAllowSortByVariation)
			{
				Hashtable hashtable3 = new Hashtable();
				hashtable3["isSortByVariation"] = true;
				hashtable3["checked"] = StiDataSortVariation.IsVariationSort(userSorts);
				hashtable3["key"] = StiDataSortVariation.Ident;
				hashtable3["sortDirection"] = sortDirection;
				items.Add(hashtable3);
			}
		}
		return items;
	}

	private static StiDataSortDirection GetSortDirection(List<StiDataSortRule> userSorts)
	{
		if (userSorts == null || userSorts.Count == 0)
		{
			return StiDataSortDirection.Ascending;
		}
		return userSorts.FirstOrDefault().Direction;
	}

	private static List<IStiMeter> FetchAllArguments(IStiElement element, StiDataTable manualDataTable)
	{
		if (element == null)
		{
			return null;
		}
		if (element is IStiChartElement)
		{
			IStiChartElement stiChartElement = element as IStiChartElement;
			if (manualDataTable != null)
			{
				if (!IsArgumentPresentedInManualData(manualDataTable))
				{
					return null;
				}
				return manualDataTable?.Meters?.Where((IStiMeter m) => m.Key == "Argument")?.ToList();
			}
			return stiChartElement.FetchAllArguments();
		}
		if (element is IStiIndicatorElement && ((IStiIndicatorElement)element).GetSeries() != null && ((IStiIndicatorElement)element).GetTarget() != null)
		{
			return new List<IStiMeter> { ((IStiIndicatorElement)element).GetTarget() };
		}
		if (element is IStiProgressElement && ((IStiProgressElement)element).GetSeries() != null && ((IStiProgressElement)element).GetTarget() != null)
		{
			return new List<IStiMeter> { ((IStiProgressElement)element).GetTarget() };
		}
		if (element is IStiGaugeElement && ((IStiGaugeElement)element).GetSeries() != null && ((IStiGaugeElement)element).GetTarget() != null)
		{
			return new List<IStiMeter> { ((IStiGaugeElement)element).GetTarget() };
		}
		return null;
	}

	private static List<IStiMeter> FetchAllValues(IStiElement element, StiDataTable manualDataTable)
	{
		if (element == null)
		{
			return null;
		}
		if (element is IStiChartElement)
		{
			IStiChartElement stiChartElement = element as IStiChartElement;
			if (manualDataTable != null)
			{
				if (!IsValuePresentedInManualData(manualDataTable))
				{
					return null;
				}
				return manualDataTable?.Meters?.Where((IStiMeter m) => m.Key == "Value")?.ToList();
			}
			return stiChartElement.FetchAllValues();
		}
		if (element is IStiIndicatorElement && ((IStiIndicatorElement)element).GetSeries() != null && ((IStiIndicatorElement)element).GetValue() != null)
		{
			return new List<IStiMeter> { ((IStiIndicatorElement)element).GetValue() };
		}
		if (element is IStiProgressElement && ((IStiProgressElement)element).GetSeries() != null && ((IStiProgressElement)element).GetValue() != null)
		{
			return new List<IStiMeter> { ((IStiProgressElement)element).GetValue() };
		}
		if (element is IStiGaugeElement && ((IStiGaugeElement)element).GetSeries() != null && ((IStiGaugeElement)element).GetValue() != null)
		{
			return new List<IStiMeter> { ((IStiGaugeElement)element).GetValue() };
		}
		return null;
	}

	private static IStiMeter GetSeries(IStiElement element, StiDataTable manualDataTable)
	{
		if (element == null)
		{
			return null;
		}
		if (element is IStiChartElement)
		{
			IStiChartElement stiChartElement = element as IStiChartElement;
			if (manualDataTable != null)
			{
				if (!IsSeriesPresentedInManualData(manualDataTable))
				{
					return null;
				}
				return manualDataTable?.Meters?.Where((IStiMeter m) => m.Key == "Series")?.FirstOrDefault();
			}
			return stiChartElement.GetSeries();
		}
		if (element is IStiIndicatorElement)
		{
			return ((IStiIndicatorElement)element).GetSeries();
		}
		if (element is IStiProgressElement)
		{
			return ((IStiProgressElement)element).GetSeries();
		}
		if (element is IStiGaugeElement)
		{
			return ((IStiGaugeElement)element).GetSeries();
		}
		return null;
	}

	private static StiDataTable GetManualDataTable(IStiElement element)
	{
		if (element is IStiChartElement && element is IStiManuallyEnteredData { DataMode: StiDataMode.ManuallyEnteringData } stiManuallyEnteredData)
		{
			return stiManuallyEnteredData.GetManuallyEnteredDataTable();
		}
		return null;
	}

	private static bool IsValuePresentedInManualData(StiDataTable manualDataTable)
	{
		IStiMeter stiMeter = manualDataTable.Meters.FirstOrDefault((IStiMeter m) => m.Key == "Value");
		if (stiMeter == null)
		{
			return false;
		}
		int meterIndex = manualDataTable.Meters.IndexOf(stiMeter);
		return manualDataTable.Rows.Select((object[] r) => r[meterIndex]).Distinct().Count() > 1;
	}

	private static bool IsArgumentPresentedInManualData(StiDataTable manualDataTable)
	{
		IStiMeter stiMeter = manualDataTable.Meters.FirstOrDefault((IStiMeter m) => m.Key == "Argument");
		if (stiMeter == null)
		{
			return false;
		}
		int meterIndex = manualDataTable.Meters.IndexOf(stiMeter);
		return manualDataTable.Rows.Select((object[] r) => r[meterIndex]).Distinct().Count() > 1;
	}

	private static bool IsSeriesPresentedInManualData(StiDataTable manualDataTable)
	{
		IStiMeter stiMeter = manualDataTable.Meters.FirstOrDefault((IStiMeter m) => m.Key == "Series");
		if (stiMeter == null)
		{
			return false;
		}
		int meterIndex = manualDataTable.Meters.IndexOf(stiMeter);
		return manualDataTable.Rows.Select((object[] r) => r[meterIndex]).Distinct().Count() > 1;
	}
}
