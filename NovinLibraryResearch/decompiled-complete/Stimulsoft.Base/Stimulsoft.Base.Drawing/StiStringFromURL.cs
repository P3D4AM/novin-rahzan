using System.IO;
using System.Net;
using System.Text;

namespace Stimulsoft.Base.Drawing;

public sealed class StiStringFromURL
{
	public static string BaseAddress { get; set; }

	public static string LoadString(string url, CookieContainer cookieContainer = null)
	{
		using StiWebClientEx stiWebClientEx = new StiWebClientEx(cookieContainer);
		stiWebClientEx.BaseAddress = (string.IsNullOrEmpty(BaseAddress) ? stiWebClientEx.BaseAddress : BaseAddress);
		stiWebClientEx.Credentials = CredentialCache.DefaultCredentials;
		byte[] buffer = stiWebClientEx.DownloadData(url);
		using MemoryStream stream = new MemoryStream(buffer);
		using StreamReader streamReader = new StreamReader(stream, Encoding.Default);
		return streamReader.ReadToEnd();
	}
}
