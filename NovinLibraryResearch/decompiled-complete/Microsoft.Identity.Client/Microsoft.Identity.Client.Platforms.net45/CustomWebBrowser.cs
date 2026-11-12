using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace Microsoft.Identity.Client.Platforms.net45;

internal class CustomWebBrowser : WebBrowser
{
	[ComVisible(true)]
	[ComDefaultInterface(typeof(NativeWrapper.IDocHostUIHandler))]
	protected class CustomSite : WebBrowserSite, NativeWrapper.IDocHostUIHandler, ICustomQueryInterface
	{
		private const int NotImplemented = -2147467263;

		private readonly WebBrowser host;

		public CustomSite(WebBrowser host)
			: base(host)
		{
			this.host = host;
		}

		public CustomQueryInterfaceResult GetInterface(ref Guid iid, out IntPtr ppv)
		{
			ppv = IntPtr.Zero;
			if (iid == typeof(NativeWrapper.IDocHostUIHandler).GUID)
			{
				ppv = Marshal.GetComInterfaceForObject(this, typeof(NativeWrapper.IDocHostUIHandler), CustomQueryInterfaceMode.Ignore);
				return CustomQueryInterfaceResult.Handled;
			}
			return CustomQueryInterfaceResult.NotHandled;
		}

		public int EnableModeless(bool fEnable)
		{
			return -2147467263;
		}

		public int FilterDataObject(IDataObject pDO, out IDataObject ppDORet)
		{
			ppDORet = null;
			return 1;
		}

		public int GetDropTarget(NativeWrapper.IOleDropTarget pDropTarget, out NativeWrapper.IOleDropTarget ppDropTarget)
		{
			ppDropTarget = null;
			return 0;
		}

		public int GetExternal(out object ppDispatch)
		{
			ppDispatch = host.ObjectForScripting;
			return 0;
		}

		public int GetHostInfo(NativeWrapper.DOCHOSTUIINFO info)
		{
			info.dwDoubleClick = 0;
			info.dwFlags = 131088;
			if (NativeWrapper.NativeMethods.IsProcessDPIAware())
			{
				info.dwFlags |= 1073741824;
			}
			if (host.ScrollBarsEnabled)
			{
				info.dwFlags |= 128;
			}
			else
			{
				info.dwFlags |= 8;
			}
			if (Application.RenderWithVisualStyles)
			{
				info.dwFlags |= 262144;
			}
			else
			{
				info.dwFlags |= 524288;
			}
			info.dwFlags |= 67108864;
			return 0;
		}

		public int GetOptionKeyPath(string[] pbstrKey, int dw)
		{
			return -2147467263;
		}

		public int HideUI()
		{
			return -2147467263;
		}

		public int OnDocWindowActivate(bool fActivate)
		{
			return -2147467263;
		}

		public int OnFrameWindowActivate(bool fActivate)
		{
			return -2147467263;
		}

		public int ResizeBorder(NativeWrapper.COMRECT rect, NativeWrapper.IOleInPlaceUIWindow doc, bool fFrameWindow)
		{
			return -2147467263;
		}

		public int ShowContextMenu(int dwID, NativeWrapper.POINT pt, object pcmdtReserved, object pdispReserved)
		{
			switch (dwID)
			{
			case 2:
			case 4:
			case 9:
			case 16:
				return 1;
			default:
				return 0;
			}
		}

		public int ShowUI(int dwID, NativeWrapper.IOleInPlaceActiveObject activeObject, NativeWrapper.IOleCommandTarget commandTarget, NativeWrapper.IOleInPlaceFrame frame, NativeWrapper.IOleInPlaceUIWindow doc)
		{
			return 1;
		}

		public int TranslateAccelerator(ref NativeWrapper.MSG msg, ref Guid group, int nCmdID)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Invalid comparison between Unknown and I4
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Invalid comparison between Unknown and I4
			if (msg.message != 258 && ((int)Control.ModifierKeys == 65536 || (int)Control.ModifierKeys == 262144 || (int)Control.ModifierKeys == 131072))
			{
				Shortcut item = (Shortcut)((int)msg.wParam | Control.ModifierKeys);
				if (shortcutBlacklist.Contains(item))
				{
					return 0;
				}
			}
			return 1;
		}

		public int TranslateUrl(int dwTranslate, string strUrlIn, out string pstrUrlOut)
		{
			pstrUrlOut = null;
			return 1;
		}

		public int UpdateUI()
		{
			return -2147467263;
		}
	}

	[ClassInterface(ClassInterfaceType.None)]
	private sealed class CustomWebBrowserEvent : StandardOleMarshalObject, NativeWrapper.DWebBrowserEvents2
	{
		private readonly CustomWebBrowser parent;

		public CustomWebBrowserEvent(CustomWebBrowser parent)
		{
			this.parent = parent;
		}

		public void NavigateError(object pDisp, ref object url, ref object frame, ref object statusCode, ref bool cancel)
		{
			string url2 = ((url == null) ? "" : ((string)url));
			string targetFrameName = ((frame == null) ? "" : ((string)frame));
			int statusCode2 = ((statusCode != null) ? ((int)statusCode) : 0);
			WebBrowserNavigateErrorEventArgs webBrowserNavigateErrorEventArgs = new WebBrowserNavigateErrorEventArgs(url2, targetFrameName, statusCode2, pDisp);
			parent.OnNavigateError(webBrowserNavigateErrorEventArgs);
			cancel = webBrowserNavigateErrorEventArgs.Cancel;
		}

		public void BeforeNavigate2(object pDisp, ref object urlObject, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
		{
		}

		public void ClientToHostWindow(ref long cX, ref long cY)
		{
		}

		public void CommandStateChange(long command, bool enable)
		{
		}

		public void DocumentComplete(object pDisp, ref object urlObject)
		{
		}

		public void DownloadBegin()
		{
		}

		public void DownloadComplete()
		{
		}

		public void FileDownload(ref bool cancel)
		{
		}

		public void NavigateComplete2(object pDisp, ref object urlObject)
		{
		}

		public void NewWindow2(ref object ppDisp, ref bool cancel)
		{
		}

		public void OnFullScreen(bool fullScreen)
		{
		}

		public void OnMenuBar(bool menuBar)
		{
		}

		public void OnQuit()
		{
		}

		public void OnStatusBar(bool statusBar)
		{
		}

		public void OnTheaterMode(bool theaterMode)
		{
		}

		public void OnToolBar(bool toolBar)
		{
		}

		public void OnVisible(bool visible)
		{
		}

		public void PrintTemplateInstantiation(object pDisp)
		{
		}

		public void PrintTemplateTeardown(object pDisp)
		{
		}

		public void PrivacyImpactedStateChange(bool bImpacted)
		{
		}

		public void ProgressChange(int progress, int progressMax)
		{
		}

		public void PropertyChange(string szProperty)
		{
		}

		public void SetSecureLockIcon(int secureLockIcon)
		{
		}

		public void StatusTextChange(string text)
		{
		}

		public void TitleChange(string text)
		{
		}

		public void UpdatePageStatus(object pDisp, ref object nPage, ref object fDone)
		{
		}

		public void WindowClosing(bool isChildWindow, ref bool cancel)
		{
		}

		public void WindowSetHeight(int height)
		{
		}

		public void WindowSetLeft(int left)
		{
		}

		public void WindowSetResizable(bool resizable)
		{
		}

		public void WindowSetTop(int top)
		{
		}

		public void WindowSetWidth(int width)
		{
		}
	}

	private const int S_OK = 0;

	private const int S_FALSE = 1;

	private const int WM_CHAR = 258;

	private static readonly HashSet<Shortcut> shortcutBlacklist;

	private CustomWebBrowserEvent webBrowserEvent;

	private ConnectionPointCookie webBrowserEventCookie;

	public event WebBrowserNavigateErrorEventHandler NavigateError;

	static CustomWebBrowser()
	{
		shortcutBlacklist = new HashSet<Shortcut>();
		shortcutBlacklist.Add((Shortcut)262152);
		shortcutBlacklist.Add((Shortcut)262184);
		shortcutBlacklist.Add((Shortcut)262181);
		shortcutBlacklist.Add((Shortcut)262183);
		shortcutBlacklist.Add((Shortcut)262182);
	}

	protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
	{
		return (WebBrowserSiteBase)(object)new CustomSite((WebBrowser)(object)this);
	}

	protected override void CreateSink()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		((WebBrowser)this).CreateSink();
		object activeXInstance = ((WebBrowserBase)this).ActiveXInstance;
		if (activeXInstance != null)
		{
			webBrowserEvent = new CustomWebBrowserEvent(this);
			webBrowserEventCookie = new ConnectionPointCookie(activeXInstance, (object)webBrowserEvent, typeof(NativeWrapper.DWebBrowserEvents2));
		}
	}

	protected override void DetachSink()
	{
		if (webBrowserEventCookie != null)
		{
			webBrowserEventCookie.Disconnect();
			webBrowserEventCookie = null;
		}
		((WebBrowser)this).DetachSink();
	}

	protected virtual void OnNavigateError(WebBrowserNavigateErrorEventArgs e)
	{
		if (this.NavigateError != null)
		{
			this.NavigateError(this, e);
		}
	}
}
