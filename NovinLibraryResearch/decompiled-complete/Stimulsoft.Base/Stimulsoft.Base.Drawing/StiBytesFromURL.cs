using System;
using System.Collections;
using System.Collections.Specialized;
using System.Net;

namespace Stimulsoft.Base.Drawing;

public sealed class StiBytesFromURL
{
	private static Hashtable failedAttempts = new Hashtable();

	public static bool AllowCacheFails { get; set; } = true;


	public static byte[] Load(string url, CookieContainer cookieContainer = null, NameValueCollection headers = null)
	{
		if (string.IsNullOrWhiteSpace(url))
		{
			return null;
		}
		if (AllowCacheFails)
		{
			object obj = failedAttempts[url];
			if (obj is int && (int)obj - Environment.TickCount < 2000)
			{
				return null;
			}
		}
		try
		{
			using StiWebClientEx stiWebClientEx = new StiWebClientEx(cookieContainer);
			stiWebClientEx.Credentials = CredentialCache.DefaultCredentials;
			if (headers != null)
			{
				stiWebClientEx.Headers.Add(headers);
			}
			return stiWebClientEx.DownloadData(url);
		}
		catch (Exception)
		{
			if (AllowCacheFails)
			{
				failedAttempts[url] = Environment.TickCount;
			}
			throw;
		}
	}

	public static byte[] TryLoad(string url, CookieContainer cookieContainer = null, NameValueCollection headers = null)
	{
		if (string.IsNullOrWhiteSpace(url))
		{
			return null;
		}
		object obj = failedAttempts[url];
		if (obj is int && (int)obj - Environment.TickCount < 2000)
		{
			return null;
		}
		try
		{
			using StiWebClientEx stiWebClientEx = new StiWebClientEx(cookieContainer);
			stiWebClientEx.Credentials = CredentialCache.DefaultCredentials;
			if (headers != null)
			{
				stiWebClientEx.Headers.Add(headers);
			}
			return stiWebClientEx.DownloadData(url);
		}
		catch
		{
			failedAttempts[url] = Environment.TickCount;
			return null;
		}
	}
}
