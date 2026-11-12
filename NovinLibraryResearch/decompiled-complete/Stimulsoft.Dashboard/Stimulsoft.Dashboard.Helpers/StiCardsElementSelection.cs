using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Dashboard.Components.Cards;

namespace Stimulsoft.Dashboard.Helpers;

internal static class StiCardsElementSelection
{
	private static Hashtable hash = new Hashtable();

	public static List<StiCardsColumn> GetSelected(StiCardsElement element)
	{
		if (!(hash[element.Key] is Hashtable hashtable))
		{
			return null;
		}
		return hashtable.Keys.Cast<StiCardsColumn>().ToList();
	}

	public static List<object> GetSelectedObjects(StiCardsElement element)
	{
		return GetSelected(element)?.Cast<object>().ToList();
	}

	public static bool IsSelected(StiCardsElement element, StiCardsColumn column)
	{
		Hashtable hashtable = hash[element.Key] as Hashtable;
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			hash[element.Key] = hashtable;
		}
		return hashtable[column] != null;
	}

	public static void Select(StiCardsElement element, StiCardsColumn column)
	{
		Hashtable hashtable = hash[element.Key] as Hashtable;
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			hash[element.Key] = hashtable;
		}
		hashtable[column] = column;
	}

	public static void ResetSelection(StiCardsElement element)
	{
		if (hash.ContainsKey(element.Key))
		{
			hash.Remove(element.Key);
		}
	}
}
