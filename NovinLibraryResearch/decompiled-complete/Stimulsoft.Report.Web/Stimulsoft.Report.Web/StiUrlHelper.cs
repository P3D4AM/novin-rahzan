using System;

namespace Stimulsoft.Report.Web;

internal static class StiUrlHelper
{
	public static string EscapeUrlPath(string url)
	{
		if (url.IndexOf("?") >= 0)
		{
			return Uri.EscapeUriString(url.Substring(0, url.IndexOf("?"))) + url.Substring(url.IndexOf("?"));
		}
		return Uri.EscapeUriString(url);
	}

	public static string EscapeUrlQuotes(string url)
	{
		return url.Replace("\"", "%22").Replace("'", "%27").Replace("`", "%60");
	}
}
