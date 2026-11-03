using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Report.Dashboard;

public static class StiPivotToConvertedStateCache
{
	private static Dictionary<string, bool> cache = new Dictionary<string, bool>();

	private static string GetIntKey(IStiPivotTableElement element)
	{
		List<IStiMeter> list = element?.GetMeters();
		if (list == null || !list.Any())
		{
			return "0";
		}
		int index = 0;
		List<string> list2 = list.Select((IStiMeter m) => index++.ToString() + m.GetUniqueCode()).ToList();
		string text = (element as IStiTitleElement)?.Title?.Text ?? string.Empty;
		list2.Add(text.GetHashCode().ToString());
		long num = 0L;
		foreach (string item in list2)
		{
			num += item.GetHashCode();
		}
		return $"{num}";
	}

	private static string GetKey(IStiPivotTableElement element)
	{
		string text = StiAppKey.GetOrGeneratedKey((IStiReportComponent)element) ?? string.Empty;
		string text2 = ((element != null) ? ((IStiAppCell)element).GetKey() : null) ?? string.Empty;
		string text3 = GetIntKey(element) ?? string.Empty;
		return text + "." + text2 + "." + text3;
	}

	public static bool IsConverted(IStiPivotTableElement element)
	{
		lock (cache)
		{
			string key = GetKey(element);
			return cache.ContainsKey(key) && cache[key];
		}
	}

	public static void PutTrue(IStiPivotTableElement element)
	{
		Put(element, converted: true);
	}

	public static void PutFalse(IStiPivotTableElement element)
	{
		Put(element, converted: false);
	}

	private static void Put(IStiPivotTableElement element, bool converted)
	{
		lock (cache)
		{
			string key = GetKey(element);
			cache[key] = converted;
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

	public static void Clean(string reportKey = null)
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
