using System;
using System.Collections.Generic;
using System.IO;
using Stimulsoft.Base;

namespace Stimulsoft.Report;

internal static class StiFileImageCacheV2
{
	private static List<string> cachedDirectories;

	internal static List<string> CachedDirectories => cachedDirectories ?? (cachedDirectories = new List<string>());

	internal static void CreateNewCacheIfNeeded(StiReport report)
	{
		if (string.IsNullOrEmpty(report.ImageCachePath))
		{
			string path = (string.IsNullOrEmpty(StiOptions.Engine.ImageCache.CachePath) ? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) : StiOptions.Engine.ImageCache.CachePath);
			path = Path.Combine(path, "StimulsoftImageCache", StiGuidUtils.NewGuid());
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			CachedDirectories.Add(path);
			report.ImageCachePath = path;
		}
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
		if (cachedDirectories == null)
		{
			return;
		}
		foreach (string cachedDirectory in CachedDirectories)
		{
			if (Directory.Exists(cachedDirectory))
			{
				Directory.Delete(cachedDirectory, recursive: true);
			}
		}
	}

	internal static string GetImageCacheName(string cache, string cacheImageGuid)
	{
		return Path.Combine(cache, cacheImageGuid + ".ich");
	}

	internal static void SaveImage(StiReport report, string guid, byte[] bytes)
	{
		CreateNewCacheIfNeeded(report);
		string imageCacheName = GetImageCacheName(report.ImageCachePath, guid);
		StiFileUtils.ProcessReadOnly(imageCacheName);
		File.WriteAllBytes(imageCacheName, bytes);
	}

	internal static byte[] LoadImage(StiReport report, string guid)
	{
		if (guid == null || report == null || report.ImageCachePath == null)
		{
			return null;
		}
		string imageCacheName = GetImageCacheName(report.ImageCachePath, guid);
		if (!File.Exists(imageCacheName))
		{
			return null;
		}
		return File.ReadAllBytes(imageCacheName);
	}
}
