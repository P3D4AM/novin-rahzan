using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Net;

namespace Stimulsoft.Base.Drawing;

public sealed class StiImageFromURL
{
	public static Image LoadBitmap(string url, CookieContainer cookieContainer = null, NameValueCollection headers = null)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		byte[] buffer = StiDownloadCache.Get(url, cookieContainer, headers);
		MemoryStream memoryStream = new MemoryStream(buffer);
		return (Image)new Bitmap((Stream)memoryStream);
	}

	public static Image LoadMetafile(string url, CookieContainer cookieContainer = null, NameValueCollection headers = null)
	{
		byte[] bytes = StiDownloadCache.Get(url, cookieContainer, headers);
		return (Image)(object)StiMetafileConverter.BytesToMetafile(bytes);
	}

	public static Image LoadImage(string url, CookieContainer cookieContainer = null, NameValueCollection headers = null)
	{
		byte[] buffer = StiDownloadCache.Get(url, cookieContainer, headers);
		MemoryStream memoryStream = new MemoryStream(buffer);
		return Image.FromStream((Stream)memoryStream);
	}

	public static Image TryLoadImage(string url, CookieContainer cookieContainer = null, NameValueCollection headers = null)
	{
		try
		{
			byte[] buffer = StiDownloadCache.Get(url, cookieContainer, headers);
			MemoryStream memoryStream = new MemoryStream(buffer);
			return Image.FromStream((Stream)memoryStream);
		}
		catch
		{
			return null;
		}
	}
}
