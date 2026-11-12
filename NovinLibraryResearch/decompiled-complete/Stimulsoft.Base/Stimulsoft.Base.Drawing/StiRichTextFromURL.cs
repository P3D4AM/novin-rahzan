using System.IO;
using System.Net;
using System.Text;

namespace Stimulsoft.Base.Drawing;

public sealed class StiRichTextFromURL
{
	public static string BaseAddress { get; set; }

	public static string LoadRichText(string url, CookieContainer cookieContainer = null)
	{
		try
		{
			using StiWebClientEx stiWebClientEx = new StiWebClientEx(cookieContainer);
			if (!string.IsNullOrEmpty(BaseAddress))
			{
				stiWebClientEx.BaseAddress = BaseAddress;
			}
			stiWebClientEx.Credentials = CredentialCache.DefaultCredentials;
			byte[] buffer = stiWebClientEx.DownloadData(url);
			using MemoryStream stream = new MemoryStream(buffer);
			using StreamReader streamReader = new StreamReader(stream, Encoding.Default);
			return streamReader.ReadToEnd();
		}
		catch
		{
			return string.Empty;
		}
	}
}
