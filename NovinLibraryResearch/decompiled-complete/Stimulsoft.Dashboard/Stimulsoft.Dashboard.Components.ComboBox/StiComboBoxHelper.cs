using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.ComboBox;

public static class StiComboBoxHelper
{
	public static StiComboBoxItem[] FetchItems(StiComboBoxElement comboBoxElement, StiDataTable dataTable, bool showBlanks)
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
		List<StiComboBoxItem> list = new List<StiComboBoxItem>();
		foreach (object[] row in dataTable.Rows)
		{
			object obj = row[num2];
			if (obj != null)
			{
				object obj2 = row[num];
				if (showBlanks || !IsBlank(obj2))
				{
					list.Add(new StiComboBoxItem(Format(comboBoxElement, obj2), obj));
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

	public static List<StiDataFilterRule> FetchDefaultUserFilters(StiComboBoxElement comboBoxElement)
	{
		if (comboBoxElement.ShowAllValue)
		{
			return null;
		}
		if (comboBoxElement.SelectionMode == StiItemSelectionMode.Multi)
		{
			return null;
		}
		string keyMeterExpression = GetKeyMeterExpression(comboBoxElement);
		if (keyMeterExpression == null)
		{
			return null;
		}
		StiDataTable orCreate = StiElementDataCache.GetOrCreate(comboBoxElement);
		StiComboBoxItem[] array = FetchItems(comboBoxElement, orCreate, comboBoxElement.ShowBlanks);
		if (array == null || array.Length == 0)
		{
			return null;
		}
		object value = array.FirstOrDefault().Value;
		return StiDataFilterCreator.CreateEqualBasedOnValue(value, keyMeterExpression, comboBoxElement).ToList();
	}

	private static int GetNameMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiNameComboBoxMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	private static int GetKeyMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f is StiKeyComboBoxMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	public static string GetKeyMeterExpression(StiComboBoxElement comboBoxElement)
	{
		if (comboBoxElement.KeyMeter != null)
		{
			return comboBoxElement.KeyMeter.Expression;
		}
		if (comboBoxElement.NameMeter != null)
		{
			return comboBoxElement.NameMeter.Expression;
		}
		return null;
	}

	private static string Format(StiComboBoxElement comboBoxElement, object value)
	{
		if (value is DateTime && !(((IStiTextFormat)comboBoxElement).TextFormat is StiDateFormatService))
		{
			return ((DateTime)value).ToShortDateString();
		}
		if (StiExtValueConverter.IsEligable(value, comboBoxElement?.Report?.Culture))
		{
			return StiExtValueConverter.Convert(value, comboBoxElement?.Report?.Culture);
		}
		return ((IStiTextFormat)comboBoxElement).TextFormat.Format(value);
	}
}
