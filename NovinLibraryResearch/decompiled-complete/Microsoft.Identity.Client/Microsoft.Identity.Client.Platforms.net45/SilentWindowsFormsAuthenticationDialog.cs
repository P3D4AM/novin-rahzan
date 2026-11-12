using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Microsoft.Identity.Client.Platforms.net45;

[ComVisible(true)]
[EditorBrowsable(EditorBrowsableState.Never)]
internal class SilentWindowsFormsAuthenticationDialog : WindowsFormsWebAuthenticationDialogBase
{
	internal delegate void SilentWebUIDoneEventHandler(object sender, SilentWebUIDoneEventArgs args);

	private bool doneSignaled;

	private DateTime navigationExpiry = DateTime.MaxValue;

	private Timer timer;

	public int NavigationWaitMiliSecs { get; set; }

	internal event SilentWebUIDoneEventHandler Done;

	public SilentWindowsFormsAuthenticationDialog(object ownerWindow)
		: base(ownerWindow)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		SuppressBrowserSubDialogs();
		base.WebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentCompletedHandler);
	}

	public void CloseBrowser()
	{
		SignalDone();
	}

	private void SuppressBrowserSubDialogs()
	{
		((NativeWrapper.IWebBrowser2)((WebBrowserBase)base.WebBrowser).ActiveXInstance).Silent = true;
	}

	protected override void WebBrowserNavigatingHandler(object sender, WebBrowserNavigatingEventArgs e)
	{
		if (timer == null)
		{
			timer = CreateStartedTimer(delegate
			{
				if (DateTime.Now > navigationExpiry)
				{
					OnUserInteractionRequired();
				}
			}, NavigationWaitMiliSecs);
		}
		navigationExpiry = DateTime.MaxValue;
		base.WebBrowserNavigatingHandler(sender, e);
	}

	private static Timer CreateStartedTimer(Action onTickAction, int interval)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		Timer val = new Timer
		{
			Interval = interval
		};
		val.Tick += delegate
		{
			onTickAction();
		};
		val.Start();
		return val;
	}

	private void SignalDone(Exception exception = null)
	{
		if (!doneSignaled)
		{
			timer.Stop();
			SilentWebUIDoneEventArgs args = new SilentWebUIDoneEventArgs(exception);
			this.Done?.Invoke(this, args);
			doneSignaled = true;
		}
	}

	private void DocumentCompletedHandler(object sender, WebBrowserDocumentCompletedEventArgs args)
	{
		navigationExpiry = DateTime.Now.AddMilliseconds(NavigationWaitMiliSecs);
		if (HasLoginPage())
		{
			OnUserInteractionRequired();
		}
	}

	private void OnUserInteractionRequired()
	{
		SignalDone(new MsalUiRequiredException("no_prompt_failed", "One of two conditions was encountered: 1. The Prompt.Never flag was passed, but the constraint could not be honored, because user interaction was required. 2. An error occurred during a silent web authentication that prevented the http authentication flow from completing in a short enough time frame"));
	}

	protected override void OnClosingUrl()
	{
		SignalDone();
	}

	protected override void OnNavigationCanceled(int statusCode)
	{
		SignalDone(CreateExceptionForAuthenticationUiFailed(statusCode));
	}

	private bool HasLoginPage()
	{
		HtmlDocument document = base.WebBrowser.Document;
		HtmlElement val = null;
		if ((HtmlDocument)null != document)
		{
			val = (from HtmlElement element in (IEnumerable)document.GetElementsByTagName("INPUT")
				where string.Compare(element.GetAttribute("type"), "password", StringComparison.Ordinal) == 0 && element.Enabled && element.OffsetRectangle.Height > 0 && element.OffsetRectangle.Width > 0
				select element).FirstOrDefault();
		}
		return val != (HtmlElement)null;
	}

	protected override void Dispose(bool disposing)
	{
		if (timer != null)
		{
			((Component)(object)timer).Dispose();
		}
		base.Dispose(disposing);
	}
}
