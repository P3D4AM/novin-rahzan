using System;
using System.Collections;
using System.IO;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Export;

internal static class StiMatrixCache
{
	internal static ArrayList CachedDirectories { get; set; }

	internal static string CreateNewCache()
	{
		string path = (string.IsNullOrEmpty(StiOptions.Engine.ReportCache.CachePath) ? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) : StiOptions.Engine.ReportCache.CachePath);
		path = Path.Combine(path, "Stimulsoft", "MatrixCache");
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
			CachedDirectories = new ArrayList();
		}
		CachedDirectories.Add(path);
		return path;
	}

	internal static void DeleteCache(string path)
	{
		try
		{
			if (Directory.Exists(path))
			{
				Directory.Delete(path, recursive: true);
			}
		}
		catch
		{
		}
	}

	internal static void ClearCache()
	{
		if (CachedDirectories == null)
		{
			return;
		}
		foreach (string cachedDirectory in CachedDirectories)
		{
			try
			{
				if (Directory.Exists(cachedDirectory))
				{
					Directory.Delete(cachedDirectory, recursive: true);
				}
			}
			catch
			{
			}
		}
	}

	internal static string GetCacheSegmentName(string cache, string cachePageGuid)
	{
		return Path.Combine(cache, cachePageGuid + ".mch");
	}
}
