using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dashboard;

public static class StiOnlineMapLastImageCache
{
	private static Dictionary<string, Image> cache = new Dictionary<string, Image>();

	private static string GetKey(IStiOnlineMapElement element)
	{
		string text = StiAppKey.GetOrGeneratedKey((IStiReportComponent)element) ?? string.Empty;
		string text2 = ((element != null) ? ((IStiAppCell)element).GetKey() : null) ?? string.Empty;
		return text + "." + text2;
	}

	public static Image GetLastImage(IStiOnlineMapElement element)
	{
		lock (cache)
		{
			string key = GetKey(element);
			return cache.ContainsKey(key) ? cache[key] : null;
		}
	}

	public static bool ExistsLastImage(IStiOnlineMapElement element)
	{
		lock (cache)
		{
			string key = GetKey(element);
			return cache.ContainsKey(key);
		}
	}

	public static void StoreLastImage(IStiOnlineMapElement element, Image image)
	{
		lock (cache)
		{
			string key = GetKey(element);
			cache[key] = image;
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
