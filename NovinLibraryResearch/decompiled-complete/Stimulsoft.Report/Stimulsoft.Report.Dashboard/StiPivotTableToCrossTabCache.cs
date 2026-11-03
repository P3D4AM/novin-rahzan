using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Report.CrossTab;

namespace Stimulsoft.Report.Dashboard;

public static class StiPivotTableToCrossTabCache
{
	private static Dictionary<string, StiCrossTab> cache = new Dictionary<string, StiCrossTab>();

	public static string GetKey(IStiPivotTableElement element)
	{
		string text = StiAppKey.GetOrGeneratedKey((IStiReportComponent)element) ?? string.Empty;
		string text2 = ((element != null) ? ((IStiAppCell)element).GetKey() : null) ?? string.Empty;
		string text3 = StiElementDataCache.GetKey(element) ?? string.Empty;
		return text + "." + text2 + "." + text3;
	}

	public static StiCrossTab Get(IStiPivotTableElement element)
	{
		lock (cache)
		{
			string key = GetKey(element);
			return cache.ContainsKey(key) ? cache[key] : null;
		}
	}

	public static void Put(IStiPivotTableElement element, StiCrossTab crossTab)
	{
		lock (cache)
		{
			string key = GetKey(element);
			cache[key] = crossTab;
		}
	}

	public static bool Contains(IStiPivotTableElement element)
	{
		lock (cache)
		{
			string key = GetKey(element);
			return cache.ContainsKey(key);
		}
	}

	public static void Remove(string key)
	{
		lock (cache)
		{
			if (cache.ContainsKey(key))
			{
				cache.Remove(key);
			}
		}
	}

	public static void Remove(IStiPivotTableElement element)
	{
		lock (cache)
		{
			string key = GetKey(element);
			if (cache.ContainsKey(key))
			{
				cache.Remove(key);
			}
		}
	}

	public static void Clean(string reportKey)
	{
		lock (cache)
		{
			if (reportKey == null)
			{
				cache.Clear();
				return;
			}
			cache.Keys.Where((string k) => k.StartsWith(reportKey)).ToList().ForEach(delegate(string k)
			{
				cache.Remove(k);
			});
		}
	}
}
