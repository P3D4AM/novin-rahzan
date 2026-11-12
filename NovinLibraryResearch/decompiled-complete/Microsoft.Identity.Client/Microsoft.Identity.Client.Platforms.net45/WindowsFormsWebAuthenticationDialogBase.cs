using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.UI;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Platforms.net45;

[ComVisible(true)]
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class WindowsFormsWebAuthenticationDialogBase : Form
{
	private sealed class WindowsFormsWin32Window : IWin32Window
	{
		public IntPtr Handle { get; set; }
	}

	protected static class DpiHelper
	{
		public static int ZoomPercent { get; }

		static DpiHelper()
		{
			IntPtr dC = NativeWrapper.NativeMethods.GetDC(IntPtr.Zero);
			double num;
			double num2;
			if (dC != IntPtr.Zero)
			{
				num = NativeWrapper.NativeMethods.GetDeviceCaps(dC, 88);
				num2 = NativeWrapper.NativeMethods.GetDeviceCaps(dC, 90);
				NativeWrapper.NativeMethods.ReleaseDC(IntPtr.Zero, dC);
			}
			else
			{
				num = 96.0;
				num2 = 96.0;
			}
			int val = (int)(100.0 * (num / 96.0));
			int val2 = (int)(100.0 * (num2 / 96.0));
			ZoomPercent = Math.Min(val, val2);
		}
	}

	internal static class NativeMethods
	{
		internal enum SessionOp
		{
			SESSION_QUERY,
			SESSION_INCREMENT,
			SESSION_DECREMENT
		}

		[DllImport("IEFRAME.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		internal static extern int SetQueryNetSessionCount(SessionOp sessionOp);
	}

	private const int UIWidth = 566;

	private static readonly NavigateErrorStatus NavigateErrorStatus = new NavigateErrorStatus();

	private readonly CustomWebBrowser _webBrowser;

	private Uri _desiredCallbackUri;

	private Keys _key;

	private Panel _webBrowserPanel;

	internal RequestContext RequestContext { get; set; }

	protected IWin32Window ownerWindow { get; set; }

	internal AuthorizationResult Result { get; set; }

	public WebBrowser WebBrowser => (WebBrowser)(object)_webBrowser;

	protected WindowsFormsWebAuthenticationDialogBase(object ownerWindow)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		if (NativeMethods.SetQueryNetSessionCount(NativeMethods.SessionOp.SESSION_QUERY) == 0)
		{
			NativeMethods.SetQueryNetSessionCount(NativeMethods.SessionOp.SESSION_INCREMENT);
		}
		if (ownerWindow == null)
		{
			this.ownerWindow = null;
		}
		else if (ownerWindow is IWin32Window)
		{
			this.ownerWindow = (IWin32Window)ownerWindow;
		}
		else
		{
			if (!(ownerWindow is IntPtr))
			{
				throw new MsalException("invalid_owner_window_type", "Invalid owner window type. Expected types are IWin32Window or IntPtr (for window handle).");
			}
			this.ownerWindow = (IWin32Window)(object)new WindowsFormsWin32Window
			{
				Handle = (IntPtr)ownerWindow
			};
		}
		_webBrowser = new CustomWebBrowser();
		((Control)_webBrowser).PreviewKeyDown += new PreviewKeyDownEventHandler(WebBrowser_PreviewKeyDown);
		InitializeComponent();
	}

	private void WebBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.KeyCode == 8)
		{
			_key = (Keys)8;
		}
	}

	protected virtual void WebBrowserNavigatingHandler(object sender, WebBrowserNavigatingEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if ((int)((Form)this).DialogResult == 1)
		{
			((CancelEventArgs)(object)e).Cancel = true;
			return;
		}
		if (((Control)_webBrowser).IsDisposed)
		{
			((CancelEventArgs)(object)e).Cancel = true;
			return;
		}
		if ((int)_key == 8)
		{
			_key = (Keys)0;
			((CancelEventArgs)(object)e).Cancel = true;
		}
		((CancelEventArgs)(object)e).Cancel = CheckForClosingUrl(e.Url);
		if (e.Url.Scheme.Equals("browser", StringComparison.OrdinalIgnoreCase))
		{
			Process.Start(e.Url.AbsoluteUri.Replace("browser://", "https://"));
			((CancelEventArgs)(object)e).Cancel = true;
		}
		if (!((CancelEventArgs)(object)e).Cancel)
		{
			string arg = CoreHelpers.UrlDecode(e.Url.ToString());
			RequestContext.Logger.VerbosePii(string.Format(CultureInfo.InvariantCulture, "Navigating to '{0}'.", arg), string.Empty);
		}
	}

	private void WebBrowserNavigatedHandler(object sender, WebBrowserNavigatedEventArgs e)
	{
		if (!CheckForClosingUrl(e.Url))
		{
			string arg = CoreHelpers.UrlDecode(e.Url.ToString());
			RequestContext.Logger.VerbosePii(string.Format(CultureInfo.InvariantCulture, "Navigated to '{0}'.", arg), string.Empty);
		}
	}

	protected virtual void WebBrowserNavigateErrorHandler(object sender, WebBrowserNavigateErrorEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((Form)this).DialogResult == 1)
		{
			e.Cancel = true;
		}
		else if (((Control)_webBrowser).IsDisposed)
		{
			e.Cancel = true;
		}
		else if (((WebBrowserBase)_webBrowser).ActiveXInstance == e.WebBrowserActiveXInstance && (e.StatusCode < 300 || e.StatusCode >= 400))
		{
			e.Cancel = true;
			StopWebBrowser();
			OnNavigationCanceled(e.StatusCode);
		}
	}

	private bool CheckForClosingUrl(Uri url)
	{
		bool flag = false;
		if (url.Authority.Equals(_desiredCallbackUri.Authority, StringComparison.OrdinalIgnoreCase) && url.AbsolutePath.Equals(_desiredCallbackUri.AbsolutePath))
		{
			RequestContext.Logger.Info("Redirect Uri was reached. Stopping webview navigation...");
			Result = AuthorizationResult.FromUri(url.OriginalString);
			flag = true;
		}
		if (!flag && !url.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) && !url.AbsoluteUri.Equals("about:blank", StringComparison.OrdinalIgnoreCase) && !url.Scheme.Equals("javascript", StringComparison.OrdinalIgnoreCase))
		{
			RequestContext.Logger.Error(string.Format(CultureInfo.InvariantCulture, "Redirection to non-HTTPS scheme ({0}) found! Webview will fail...", url.Scheme));
			Result = AuthorizationResult.FromStatus(AuthorizationStatus.ErrorHttp, "non_https_redirect_failed", "Non-HTTPS url redirect is not supported in webview");
			flag = true;
		}
		if (flag)
		{
			StopWebBrowser();
			OnClosingUrl();
		}
		return flag;
	}

	private void StopWebBrowser()
	{
		InvokeHandlingOwnerWindow(delegate
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)_webBrowser).IsDisposed && ((WebBrowser)_webBrowser).IsBusy)
			{
				RequestContext.Logger.Verbose(string.Format(CultureInfo.InvariantCulture, "WebBrowser state: IsBusy: {0}, ReadyState: {1}, Created: {2}, Disposing: {3}, IsDisposed: {4}, IsOffline: {5}", ((WebBrowser)_webBrowser).IsBusy, ((WebBrowser)_webBrowser).ReadyState, ((Control)_webBrowser).Created, ((Control)_webBrowser).Disposing, ((Control)_webBrowser).IsDisposed, ((WebBrowser)_webBrowser).IsOffline));
				((WebBrowser)_webBrowser).Stop();
				RequestContext.Logger.Verbose(string.Format(CultureInfo.InvariantCulture, "WebBrowser state (after Stop): IsBusy: {0}, ReadyState: {1}, Created: {2}, Disposing: {3}, IsDisposed: {4}, IsOffline: {5}", ((WebBrowser)_webBrowser).IsBusy, ((WebBrowser)_webBrowser).ReadyState, ((Control)_webBrowser).Created, ((Control)_webBrowser).Disposing, ((Control)_webBrowser).IsDisposed, ((WebBrowser)_webBrowser).IsOffline));
			}
		});
	}

	protected abstract void OnClosingUrl();

	protected abstract void OnNavigationCanceled(int statusCode);

	internal AuthorizationResult AuthenticateAAD(Uri requestUri, Uri callbackUri)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		_desiredCallbackUri = callbackUri;
		Result = null;
		((WebBrowser)_webBrowser).Navigating += new WebBrowserNavigatingEventHandler(WebBrowserNavigatingHandler);
		((WebBrowser)_webBrowser).Navigated += new WebBrowserNavigatedEventHandler(WebBrowserNavigatedHandler);
		_webBrowser.NavigateError += WebBrowserNavigateErrorHandler;
		((WebBrowser)_webBrowser).Navigate(requestUri);
		OnAuthenticate();
		return Result;
	}

	protected virtual void OnAuthenticate()
	{
	}

	protected void InvokeHandlingOwnerWindow(Action action)
	{
		if (ownerWindow != null)
		{
			IWin32Window obj = ownerWindow;
			Control val = (Control)(object)((obj is Control) ? obj : null);
			if (val != null)
			{
				val.Invoke((Delegate)action);
				return;
			}
		}
		action();
	}

	private void InitializeComponent()
	{
		InvokeHandlingOwnerWindow(delegate
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			int height = (int)((double)Math.Max(((ownerWindow != null) ? Screen.FromHandle(ownerWindow.Handle) : Screen.PrimaryScreen).WorkingArea.Height, 160) * 70.0 / (double)DpiHelper.ZoomPercent);
			_webBrowserPanel = new Panel();
			((Control)_webBrowserPanel).SuspendLayout();
			((Control)this).SuspendLayout();
			((Control)_webBrowser).Dock = (DockStyle)5;
			((Control)_webBrowser).Location = new Point(0, 25);
			((Control)_webBrowser).MinimumSize = new Size(20, 20);
			((Control)_webBrowser).Name = "webBrowser";
			((Control)_webBrowser).Size = new Size(566, 565);
			((Control)_webBrowser).TabIndex = 1;
			((WebBrowser)_webBrowser).IsWebBrowserContextMenuEnabled = false;
			((Control)_webBrowserPanel).Controls.Add((Control)(object)_webBrowser);
			((Control)_webBrowserPanel).Dock = (DockStyle)5;
			_webBrowserPanel.BorderStyle = (BorderStyle)0;
			((Control)_webBrowserPanel).Location = new Point(0, 0);
			((Control)_webBrowserPanel).Name = "webBrowserPanel";
			((Control)_webBrowserPanel).Size = new Size(566, height);
			((Control)_webBrowserPanel).TabIndex = 2;
			((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
			((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
			((Form)this).ClientSize = new Size(566, height);
			((Control)this).Controls.Add((Control)(object)_webBrowserPanel);
			((Form)this).FormBorderStyle = (FormBorderStyle)1;
			((Control)this).Name = "BrowserAuthenticationWindow";
			((Form)this).StartPosition = (FormStartPosition)((ownerWindow == null) ? 1 : 4);
			((Control)this).Text = string.Empty;
			((Form)this).ShowIcon = false;
			((Form)this).MaximizeBox = false;
			((Form)this).MinimizeBox = false;
			((Form)this).ShowInTaskbar = ownerWindow == null;
			((Control)_webBrowserPanel).ResumeLayout(false);
			((Control)this).ResumeLayout(false);
		});
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StopWebBrowser();
		}
		((Form)this).Dispose(disposing);
	}

	protected MsalClientException CreateExceptionForAuthenticationUiFailed(int statusCode)
	{
		if (NavigateErrorStatus.Messages.ContainsKey(statusCode))
		{
			string format = "The browser based authentication dialog failed to complete. Reason: {0}";
			string errorMessage = string.Format(CultureInfo.InvariantCulture, format, NavigateErrorStatus.Messages[statusCode]);
			return new MsalClientException("authentication_ui_failed", errorMessage);
		}
		string format2 = "The browser based authentication dialog failed to complete for an unknown reason. StatusCode: {0}";
		string errorMessage2 = string.Format(CultureInfo.InvariantCulture, format2, statusCode);
		return new MsalClientException("authentication_ui_failed", errorMessage2);
	}
}
