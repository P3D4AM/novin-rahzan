using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dashboard.Helpers;

internal static class StiDashboardImageHyperlinkCache
{
	private static object lockObject = new object();

	private static Dictionary<string, byte[]> cache = new Dictionary<string, byte[]>();

	internal static byte[] Get(string hyperlink, IStiApp app)
	{
		lock (cache)
		{
			string cacheKey = GetCacheKey(hyperlink, app);
			byte[] array = (cache.ContainsKey(cacheKey) ? GetFromCache(hyperlink, app) : null);
			if (array == null)
			{
				array = StiHyperlinkProcessor.TryGetBytes(app as StiReport, hyperlink, firstPositionInDataSource: true, allowDataLoading: true);
				if (array == null)
				{
					array = new byte[0];
				}
				AddToCache(hyperlink, array, app);
				return array;
			}
			if (array.Length == 0)
			{
				return null;
			}
			return array;
		}
	}

	private static string GetCacheKey(string hyperlink, IStiApp app)
	{
		if (app != null)
		{
			((IStiAppCell)app).SetKey(StiKeyHelper.GetOrGeneratedKey(((IStiAppCell)app).GetKey()));
		}
		string text = ((app != null) ? ((IStiAppCell)app).GetKey() : null) ?? string.Empty;
		if (string.IsNullOrWhiteSpace(hyperlink))
		{
			return text + ".0";
		}
		return text + "." + hyperlink;
	}

	private static byte[] GetFromCache(string hyperlink, IStiApp app)
	{
		lock (lockObject)
		{
			string cacheKey = GetCacheKey(hyperlink, app);
			return cache.ContainsKey(cacheKey) ? cache[cacheKey] : null;
		}
	}

	internal static void AddToCache(string hyperlink, byte[] bytes, IStiApp app)
	{
		lock (lockObject)
		{
			string cacheKey = GetCacheKey(hyperlink, app);
			if (bytes == null)
			{
				bytes = new byte[0];
			}
			cache[cacheKey] = bytes;
		}
	}

	internal static void Clean(string appKey)
	{
		lock (cache)
		{
			if (appKey == null)
			{
				cache.Clear();
				return;
			}
			cache.Keys.Where((string k) => k.StartsWith(appKey)).ToList().ForEach(delegate(string k)
			{
				cache.Remove(k);
			});
		}
	}
}
