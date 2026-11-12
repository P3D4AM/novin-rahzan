using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client;

public class SystemWebViewOptions
{
	public string HtmlMessageSuccess { get; set; }

	public string HtmlMessageError { get; set; }

	public Uri BrowserRedirectSuccess { get; set; }

	public Uri BrowserRedirectError { get; set; }

	public Func<Uri, Task> OpenBrowserAsync { get; set; }

	internal void LogParameters(ICoreLogger logger)
	{
		logger.Info("DefaultBrowserOptions configured");
		logger.InfoPii("HtmlMessageSuccess " + HtmlMessageSuccess, "HtmlMessageSuccess? " + !string.IsNullOrEmpty(HtmlMessageSuccess));
		logger.InfoPii("HtmlMessageError " + HtmlMessageError, "HtmlMessageError? " + !string.IsNullOrEmpty(HtmlMessageError));
		logger.InfoPii("BrowserRedirectSuccess " + BrowserRedirectSuccess, "BrowserRedirectSuccess? " + (BrowserRedirectSuccess != null));
		logger.InfoPii("BrowserRedirectError " + BrowserRedirectError, "BrowserRedirectError? " + (BrowserRedirectError != null));
	}

	public static async Task OpenWithEdgeBrowserAsync(Uri uri)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		string absoluteUri = uri.AbsoluteUri;
		absoluteUri = absoluteUri.Replace("&", "^&");
		Process.Start(new ProcessStartInfo("cmd", "/c start microsoft-edge:" + absoluteUri)
		{
			CreateNoWindow = true
		});
		await Task.FromResult(0).ConfigureAwait(continueOnCapturedContext: false);
	}

	public static async Task OpenWithChromeEdgeBrowserAsync(Uri uri)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		string absoluteUri = uri.AbsoluteUri;
		absoluteUri = absoluteUri.Replace("&", "^&");
		Process.Start(new ProcessStartInfo("cmd", "/c start msedge " + absoluteUri)
		{
			CreateNoWindow = true
		});
		await Task.FromResult(0).ConfigureAwait(continueOnCapturedContext: false);
	}
}
