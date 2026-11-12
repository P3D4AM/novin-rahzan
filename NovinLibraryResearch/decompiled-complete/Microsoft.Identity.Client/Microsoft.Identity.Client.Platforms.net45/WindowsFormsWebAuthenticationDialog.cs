using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Identity.Client.UI;

namespace Microsoft.Identity.Client.Platforms.net45;

[ComVisible(true)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class WindowsFormsWebAuthenticationDialog : WindowsFormsWebAuthenticationDialogBase
{
	private int _statusCode;

	private bool _zoomed;

	public WindowsFormsWebAuthenticationDialog(object ownerWindow)
		: base(ownerWindow)
	{
		((Form)this).Shown += FormShownHandler;
		base.WebBrowser.DocumentTitleChanged += WebBrowserDocumentTitleChangedHandler;
		base.WebBrowser.ObjectForScripting = this;
	}

	protected override void OnAuthenticate()
	{
		_zoomed = false;
		_statusCode = 0;
		ShowBrowser();
		base.OnAuthenticate();
	}

	public void ShowBrowser()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		DialogResult uiResult = (DialogResult)0;
		InvokeHandlingOwnerWindow(delegate
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			uiResult = ((Form)this).ShowDialog(base.ownerWindow);
		});
		DialogResult val = uiResult;
		if ((int)val != 1)
		{
			if ((int)val != 2)
			{
				throw CreateExceptionForAuthenticationUiFailed(_statusCode);
			}
			base.Result = AuthorizationResult.FromStatus(AuthorizationStatus.UserCancel);
		}
	}

	protected override void WebBrowserNavigatingHandler(object sender, WebBrowserNavigatingEventArgs e)
	{
		SetBrowserZoom();
		base.WebBrowserNavigatingHandler(sender, e);
	}

	protected override void OnClosingUrl()
	{
		((Form)this).DialogResult = (DialogResult)1;
	}

	protected override void OnNavigationCanceled(int inputStatusCode)
	{
		_statusCode = inputStatusCode;
		((Form)this).DialogResult = (DialogResult)((inputStatusCode == 0) ? 2 : 3);
	}

	private void SetBrowserZoom()
	{
		int zoomPercent = DpiHelper.ZoomPercent;
		if (NativeWrapper.NativeMethods.IsProcessDPIAware() && 100 != zoomPercent && !_zoomed)
		{
			SetBrowserControlZoom(zoomPercent - 1);
			SetBrowserControlZoom(zoomPercent);
			_zoomed = true;
		}
	}

	private void SetBrowserControlZoom(int zoomPercent)
	{
		if (((NativeWrapper.IWebBrowser2)((WebBrowserBase)base.WebBrowser).ActiveXInstance).Document is NativeWrapper.IOleCommandTarget oleCommandTarget)
		{
			object[] pvaIn = new object[1] { zoomPercent };
			Marshal.ThrowExceptionForHR(oleCommandTarget.Exec(IntPtr.Zero, 63, 2, pvaIn, IntPtr.Zero));
		}
	}

	private void FormShownHandler(object sender, EventArgs e)
	{
		if (((Form)this).Owner == null)
		{
			((Form)this).Activate();
		}
	}

	private void WebBrowserDocumentTitleChangedHandler(object sender, EventArgs e)
	{
		((Control)this).Text = base.WebBrowser.DocumentTitle;
	}
}
