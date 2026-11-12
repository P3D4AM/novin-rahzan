using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base;

public static class StiDownloadCache
{
	private class FileInfo
	{
		public string Url;

		public byte[] Data;

		public int Ticks;

		public FileInfo(string url, byte[] data, int ticks)
		{
			Url = url;
			Data = data;
			Ticks = ticks;
		}
	}

	private static Hashtable cache;

	private static object lockCache;

	private static Timer expireTimer;

	public static bool Enabled { get; set; }

	public static int TicksToExpired { get; set; }

	public static int TicksToFailedAttempt { get; set; }

	public static int TicksDelayInProcess { get; set; }

	public static void Clear()
	{
		lock (lockCache)
		{
			cache.Clear();
		}
	}

	public static byte[] Get(string url, CookieContainer cookieContainer = null, NameValueCollection headers = null)
	{
		if (!Enabled)
		{
			return StiBytesFromURL.Load(url, cookieContainer, headers);
		}
		int num = url.GetHashCode();
		if (cookieContainer != null)
		{
			num = (num * 397) ^ cookieContainer.GetHashCode();
		}
		if (headers != null)
		{
			num = (num * 397) ^ headers.GetHashCode();
		}
		bool flag = false;
		bool flag2 = false;
		lock (lockCache)
		{
			if (cache.ContainsKey(num))
			{
				object obj = cache[num];
				if (obj == null)
				{
					flag2 = true;
				}
				else if (cache[num] is FileInfo fileInfo)
				{
					fileInfo.Ticks = Environment.TickCount;
				}
			}
			else
			{
				flag = true;
				cache[num] = null;
			}
		}
		if (flag2)
		{
			int num2 = 20000 / TicksDelayInProcess;
			while (cache.ContainsKey(num) && cache[num] == null && num2 > 0)
			{
				Thread.Sleep(TicksDelayInProcess);
				num2--;
			}
			if (num2 == 0)
			{
				flag = true;
			}
		}
		if (flag)
		{
			byte[] data;
			try
			{
				data = StiBytesFromURL.Load(url, cookieContainer, headers);
			}
			catch
			{
				FileInfo value = new FileInfo(url, null, Environment.TickCount - TicksToExpired + TicksToFailedAttempt);
				lock (lockCache)
				{
					cache[num] = value;
				}
				throw;
			}
			FileInfo value2 = new FileInfo(url, data, Environment.TickCount);
			lock (lockCache)
			{
				cache[num] = value2;
			}
		}
		if (!(cache[num] is FileInfo fileInfo2))
		{
			throw new Exception("StiDownloadCache: Something wrong ...");
		}
		fileInfo2.Ticks = Environment.TickCount;
		CheckExpiredUrls();
		return fileInfo2.Data;
	}

	public static void CheckExpiredUrls()
	{
		int tickCount = Environment.TickCount;
		List<string> list = new List<string>();
		lock (lockCache)
		{
			foreach (object value in cache.Values)
			{
				if (value is FileInfo fileInfo && tickCount - fileInfo.Ticks > TicksToExpired)
				{
					list.Add(fileInfo.Url);
				}
			}
			foreach (string item in list)
			{
				cache.Remove(item);
			}
		}
	}

	private static void OnTimedEvent(object obj)
	{
		CheckExpiredUrls();
	}

	static StiDownloadCache()
	{
		cache = new Hashtable();
		lockCache = new object();
		Enabled = true;
		TicksToExpired = 120000;
		TicksToFailedAttempt = 2000;
		TicksDelayInProcess = 50;
		expireTimer = new Timer(OnTimedEvent, null, 5000, TicksToExpired / 10);
	}
}
