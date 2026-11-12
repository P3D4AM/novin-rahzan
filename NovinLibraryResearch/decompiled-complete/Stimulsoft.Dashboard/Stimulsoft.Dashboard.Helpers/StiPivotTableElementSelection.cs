using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.PivotTable;

namespace Stimulsoft.Dashboard.Helpers;

internal static class StiPivotTableElementSelection
{
	private static Hashtable hash = new Hashtable();

	public static List<StiMeter> GetSelected(StiPivotTableElement element)
	{
		if (!(hash[element.Key] is Hashtable hashtable))
		{
			return null;
		}
		return hashtable.Keys.Cast<StiMeter>().ToList();
	}

	public static List<object> GetSelectedObjects(StiPivotTableElement element)
	{
		return GetSelected(element)?.Cast<object>().ToList();
	}

	public static bool IsSelected(StiPivotTableElement element, StiMeter column)
	{
		Hashtable hashtable = hash[element.Key] as Hashtable;
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			hash[element.Key] = hashtable;
		}
		return hashtable[column] != null;
	}

	public static void Select(StiPivotTableElement element, StiMeter column)
	{
		Hashtable hashtable = hash[element.Key] as Hashtable;
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			hash[element.Key] = hashtable;
		}
		hashtable[column] = column;
	}

	public static void ResetSelection(StiPivotTableElement element)
	{
		if (hash.ContainsKey(element.Key))
		{
			hash.Remove(element.Key);
		}
	}
}
