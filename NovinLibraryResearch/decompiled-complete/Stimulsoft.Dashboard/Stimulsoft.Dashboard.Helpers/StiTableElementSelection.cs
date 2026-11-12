using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Dashboard.Components.Table;

namespace Stimulsoft.Dashboard.Helpers;

internal static class StiTableElementSelection
{
	private static Hashtable hash = new Hashtable();

	public static List<StiTableColumn> GetSelected(StiTableElement element)
	{
		if (!(hash[element.Key] is Hashtable hashtable))
		{
			return null;
		}
		return hashtable.Keys.Cast<StiTableColumn>().ToList();
	}

	public static List<object> GetSelectedObjects(StiTableElement element)
	{
		return GetSelected(element)?.Cast<object>().ToList();
	}

	public static bool IsSelected(StiTableElement element, StiTableColumn column)
	{
		Hashtable hashtable = hash[element.Key] as Hashtable;
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			hash[element.Key] = hashtable;
		}
		return hashtable[column] != null;
	}

	public static void Select(StiTableElement element, StiTableColumn column)
	{
		Hashtable hashtable = hash[element.Key] as Hashtable;
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			hash[element.Key] = hashtable;
		}
		hashtable[column] = column;
	}

	public static void ResetSelection(StiTableElement element)
	{
		if (hash.ContainsKey(element.Key))
		{
			hash.Remove(element.Key);
		}
	}
}
