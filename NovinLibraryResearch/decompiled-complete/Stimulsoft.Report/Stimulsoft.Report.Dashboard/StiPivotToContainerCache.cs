using System.Collections.Generic;
using System.Linq;

namespace Stimulsoft.Report.Dashboard;

public static class StiPivotToContainerCache
{
	private static Dictionary<string, IStiPivotGridContainer> cache = new Dictionary<string, IStiPivotGridContainer>();

	public static IStiPivotGridContainer Get(IStiPivotTableElement element)
	{
		lock (cache)
		{
			string key = StiElementDataCache.GetKey(element);
			return cache.ContainsKey(key) ? cache[key] : null;
		}
	}

	public static void Put(IStiPivotTableElement element, IStiPivotGridContainer container)
	{
		lock (cache)
		{
			string key = StiElementDataCache.GetKey(element);
			cache[key] = container;
		}
	}

	public static void Remove(IStiPivotTableElement element)
	{
		lock (cache)
		{
			string key = StiElementDataCache.GetKey(element);
			if (cache.ContainsKey(key))
			{
				cache.Remove(key);
			}
		}
	}

	public static bool Contains(IStiPivotTableElement element)
	{
		lock (cache)
		{
			string key = StiElementDataCache.GetKey(element);
			return cache.ContainsKey(key);
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
