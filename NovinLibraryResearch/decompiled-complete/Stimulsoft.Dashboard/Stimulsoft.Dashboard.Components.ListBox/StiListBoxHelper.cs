using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.ListBox;

public static class StiListBoxHelper
{
	public static StiListBoxItem[] FetchItems(StiListBoxElement listBoxElement, StiDataTable dataTable, bool showBlanks)
	{
		if (dataTable == null || dataTable == StiDataTable.NullTable || dataTable.Rows.Count == 0)
		{
			return null;
		}
		int num = GetNameMeterIndex(dataTable);
		int num2 = GetKeyMeterIndex(dataTable);
		if (num == -1 && num2 == 1)
		{
			return null;
		}
		if (num2 == -1)
		{
			num2 = num;
		}
		if (num == -1)
		{
			num = num2;
		}
		List<StiListBoxItem> list = new List<StiListBoxItem>();
		foreach (object[] row in dataTable.Rows)
		{
			object obj = row[num2];
			if (obj != null)
			{
				object obj2 = row[num];
				if (showBlanks || !IsBlank(obj2))
				{
					list.Add(new StiListBoxItem(Format(listBoxElement, obj2), obj));
				}
			}
		}
		return list.ToArray();
	}

	public static bool IsBlank(object data)
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

	public static List<StiDataFilterRule> FetchDefaultUserFilters(StiListBoxElement listBoxElement)
	{
		if (listBoxElement.ShowAllValue)
		{
			return null;
		}
		if (listBoxElement.SelectionMode == StiItemSelectionMode.Multi)
		{
			return null;
		}
		string keyMeterExpression = GetKeyMeterExpression(listBoxElement);
		if (keyMeterExpression == null)
		{
			return null;
		}
		StiDataTable orCreate = StiElementDataCache.GetOrCreate(listBoxElement);
		StiListBoxItem[] array = FetchItems(listBoxElement, orCreate, listBoxElement.ShowBlanks);
		if (array == null || array.Length == 0)
		{
			return null;
		}
		object value = array.FirstOrDefault().Value;
		return StiDataFilterCreator.CreateEqualBasedOnValue(value, keyMeterExpression, listBoxElement).ToList();
	}

	public static int GetNameMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiNameListBoxMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	public static int GetKeyMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiKeyListBoxMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	public static string GetKeyMeterExpression(StiListBoxElement listBoxElement)
	{
		if (listBoxElement.KeyMeter != null)
		{
			return listBoxElement.KeyMeter.Expression;
		}
		if (listBoxElement.NameMeter != null)
		{
			return listBoxElement.NameMeter.Expression;
		}
		return null;
	}

	public static string Format(StiListBoxElement listBoxElement, object value)
	{
		if (value is DateTime && !(listBoxElement.TextFormat is StiDateFormatService))
		{
			return ((DateTime)value).ToShortDateString();
		}
		if (StiExtValueConverter.IsEligable(value, listBoxElement?.Report?.Culture))
		{
			return StiExtValueConverter.Convert(value, listBoxElement?.Report?.Culture);
		}
		return listBoxElement.TextFormat.Format(value);
	}
}
