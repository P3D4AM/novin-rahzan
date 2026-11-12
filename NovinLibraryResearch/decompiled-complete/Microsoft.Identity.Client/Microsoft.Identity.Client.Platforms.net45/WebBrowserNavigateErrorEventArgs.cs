using System.ComponentModel;

namespace Microsoft.Identity.Client.Platforms.net45;

public class WebBrowserNavigateErrorEventArgs : CancelEventArgs
{
	public string TargetFrameName { get; }

	public string Url { get; }

	public object WebBrowserActiveXInstance { get; }

	public int StatusCode { get; }

	public WebBrowserNavigateErrorEventArgs(string url, string targetFrameName, int statusCode, object webBrowserActiveXInstance)
	{
		Url = url;
		TargetFrameName = targetFrameName;
		StatusCode = statusCode;
		WebBrowserActiveXInstance = webBrowserActiveXInstance;
	}
}
