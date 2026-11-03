using System;
using System.Collections.Generic;
using System.IO;
using Stimulsoft.Base;

namespace Stimulsoft.Report;

internal class StiReportCache
{
	internal static List<string> CachedDirectories { get; set; }

	internal static string CreateNewCache()
	{
		string path = (string.IsNullOrEmpty(StiOptions.Engine.ReportCache.CachePath) ? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) : StiOptions.Engine.ReportCache.CachePath);
		path = Path.Combine(path, "Stimulsoft", "ReportsCache");
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		string path2 = StiGuidUtils.NewGuid();
		path = Path.Combine(path, path2);
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		if (CachedDirectories == null)
		{
			CachedDirectories = new List<string>();
		}
		CachedDirectories.Add(path);
		return path;
	}

	internal static void DeleteCache(string path)
	{
		if (Directory.Exists(path))
		{
			Directory.Delete(path, recursive: true);
		}
	}

	internal static void ClearCache()
	{
		if (CachedDirectories == null)
		{
			return;
		}
		CachedDirectories.ForEach(delegate(string d)
		{
			if (Directory.Exists(d))
			{
				Directory.Delete(d, recursive: true);
			}
		});
	}

	internal static string GetPageCacheName(string cache, string cachePageGuid)
	{
		return Path.Combine(cache, cachePageGuid + ".mch");
	}

	private StiReportCache()
	{
	}
}
