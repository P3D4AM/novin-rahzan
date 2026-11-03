using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Stimulsoft.Base;

namespace Stimulsoft.Report;

internal class StiFileImageCache
{
	internal static List<string> CachedDirectories { get; set; }

	internal static string CreateNewCache()
	{
		string path = (string.IsNullOrEmpty(StiOptions.Engine.ReportCache.CachePath) ? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) : StiOptions.Engine.ReportCache.CachePath);
		path = Path.Combine(path, "StimulsoftImageCache");
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

	internal static string GetImageCacheName(string cache, string cacheImageGuid)
	{
		return Path.Combine(cache, cacheImageGuid + ".ich");
	}

	internal static void SaveImage(Image image, string path)
	{
		StiFileUtils.ProcessReadOnly(path);
		FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		image.Save((Stream)fileStream, ImageFormat.Png);
		fileStream.Flush();
		fileStream.Close();
	}

	internal static Image LoadImage(string path)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		Bitmap val = new Bitmap((Stream)fileStream);
		Bitmap result = new Bitmap((Image)(object)val);
		((Image)val).Dispose();
		fileStream.Close();
		return (Image)(object)result;
	}

	private StiFileImageCache()
	{
	}
}
