using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.Identity.Client.Platforms.net45;

internal class NativeWrapper
{
	[StructLayout(LayoutKind.Sequential)]
	public class POINT
	{
		public int x;

		public int y;

		public POINT()
		{
		}

		public POINT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public class OLECMD
	{
		[MarshalAs(UnmanagedType.U4)]
		public int cmdID;

		[MarshalAs(UnmanagedType.U4)]
		public int cmdf;
	}

	[ComImport]
	[ComVisible(true)]
	[Guid("B722BCCB-4E68-101B-A2BC-00AA00404770")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleCommandTarget
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int QueryStatus(ref Guid pguidCmdGroup, int cCmds, [In][Out] OLECMD prgCmds, [In][Out] IntPtr pCmdText);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Exec(IntPtr guid, int nCmdID, int nCmdexecopt, [In][MarshalAs(UnmanagedType.LPArray)] object[] pvaIn, IntPtr pvaOut);
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(true)]
	public class DOCHOSTUIINFO
	{
		[MarshalAs(UnmanagedType.U4)]
		public int cbSize = Marshal.SizeOf(typeof(DOCHOSTUIINFO));

		[MarshalAs(UnmanagedType.I4)]
		public int dwFlags;

		[MarshalAs(UnmanagedType.I4)]
		public int dwDoubleClick;

		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved1;

		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved2;
	}

	[Serializable]
	public struct MSG
	{
		public IntPtr hwnd;

		public int message;

		public IntPtr wParam;

		public IntPtr lParam;

		public int time;

		public int pt_x;

		public int pt_y;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class COMRECT
	{
		public int left;

		public int top;

		public int right;

		public int bottom;

		public override string ToString()
		{
			return "Left = " + left + " Top " + top + " Right = " + right + " Bottom = " + bottom;
		}

		public COMRECT()
		{
		}

		public COMRECT(Rectangle r)
		{
			left = r.X;
			top = r.Y;
			right = r.Right;
			bottom = r.Bottom;
		}

		public COMRECT(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public static COMRECT FromXYWH(int x, int y, int width, int height)
		{
			return new COMRECT(x, y, x + width, y + height);
		}
	}

	[ComImport]
	[Guid("00000115-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleInPlaceUIWindow
	{
		IntPtr GetWindow();

		[PreserveSig]
		int ContextSensitiveHelp(int fEnterMode);

		[PreserveSig]
		int GetBorder([Out] COMRECT lprectBorder);

		[PreserveSig]
		int RequestBorderSpace([In] COMRECT pborderwidths);

		[PreserveSig]
		int SetBorderSpace([In] COMRECT pborderwidths);

		void SetActiveObject([In][MarshalAs(UnmanagedType.Interface)] IOleInPlaceActiveObject pActiveObject, [In][MarshalAs(UnmanagedType.LPWStr)] string pszObjName);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000117-0000-0000-C000-000000000046")]
	public interface IOleInPlaceActiveObject
	{
		[PreserveSig]
		int GetWindow(out IntPtr hwnd);

		void ContextSensitiveHelp(int fEnterMode);

		[PreserveSig]
		int TranslateAccelerator([In] ref MSG lpmsg);

		void OnFrameWindowActivate(bool fActivate);

		void OnDocWindowActivate(int fActivate);

		void ResizeBorder([In] COMRECT prcBorder, [In] IOleInPlaceUIWindow pUIWindow, bool fFrameWindow);

		void EnableModeless(int fEnable);
	}

	[StructLayout(LayoutKind.Sequential)]
	public sealed class tagOleMenuGroupWidths
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public int[] widths = new int[6];
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000116-0000-0000-C000-000000000046")]
	public interface IOleInPlaceFrame
	{
		IntPtr GetWindow();

		[PreserveSig]
		int ContextSensitiveHelp(int fEnterMode);

		[PreserveSig]
		int GetBorder([Out] COMRECT lprectBorder);

		[PreserveSig]
		int RequestBorderSpace([In] COMRECT pborderwidths);

		[PreserveSig]
		int SetBorderSpace([In] COMRECT pborderwidths);

		[PreserveSig]
		int SetActiveObject([In][MarshalAs(UnmanagedType.Interface)] IOleInPlaceActiveObject pActiveObject, [In][MarshalAs(UnmanagedType.LPWStr)] string pszObjName);

		[PreserveSig]
		int InsertMenus([In] IntPtr hmenuShared, [In][Out] tagOleMenuGroupWidths lpMenuWidths);

		[PreserveSig]
		int SetMenu([In] IntPtr hmenuShared, [In] IntPtr holemenu, [In] IntPtr hwndActiveObject);

		[PreserveSig]
		int RemoveMenus([In] IntPtr hmenuShared);

		[PreserveSig]
		int SetStatusText([In][MarshalAs(UnmanagedType.LPWStr)] string pszStatusText);

		[PreserveSig]
		int EnableModeless(bool fEnable);

		[PreserveSig]
		int TranslateAccelerator([In] ref MSG lpmsg, [In][MarshalAs(UnmanagedType.U2)] short wID);
	}

	[ComImport]
	[Guid("00000122-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOleDropTarget
	{
		[PreserveSig]
		int OleDragEnter([In][MarshalAs(UnmanagedType.Interface)] object pDataObj, [In][MarshalAs(UnmanagedType.U4)] int grfKeyState, [In] POINT pt, [In][Out] ref int pdwEffect);

		[PreserveSig]
		int OleDragOver([In][MarshalAs(UnmanagedType.U4)] int grfKeyState, [In] POINT pt, [In][Out] ref int pdwEffect);

		[PreserveSig]
		int OleDragLeave();

		[PreserveSig]
		int OleDrop([In][MarshalAs(UnmanagedType.Interface)] object pDataObj, [In][MarshalAs(UnmanagedType.U4)] int grfKeyState, [In] POINT pt, [In][Out] ref int pdwEffect);
	}

	[ComImport]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("BD3F23C0-D43E-11CF-893B-00AA00BDCE1A")]
	public interface IDocHostUIHandler
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int ShowContextMenu([In][MarshalAs(UnmanagedType.U4)] int dwID, [In] POINT pt, [In][MarshalAs(UnmanagedType.Interface)] object pcmdtReserved, [In][MarshalAs(UnmanagedType.Interface)] object pdispReserved);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetHostInfo([In][Out] DOCHOSTUIINFO info);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int ShowUI([In][MarshalAs(UnmanagedType.I4)] int dwID, [In] IOleInPlaceActiveObject activeObject, [In] IOleCommandTarget commandTarget, [In] IOleInPlaceFrame frame, [In] IOleInPlaceUIWindow doc);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int HideUI();

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int UpdateUI();

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int EnableModeless([In][MarshalAs(UnmanagedType.Bool)] bool fEnable);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int OnDocWindowActivate([In][MarshalAs(UnmanagedType.Bool)] bool fActivate);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int OnFrameWindowActivate([In][MarshalAs(UnmanagedType.Bool)] bool fActivate);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int ResizeBorder([In] COMRECT rect, [In] IOleInPlaceUIWindow doc, bool fFrameWindow);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int TranslateAccelerator([In] ref MSG msg, [In] ref Guid group, [In][MarshalAs(UnmanagedType.I4)] int nCmdID);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetOptionKeyPath([Out][MarshalAs(UnmanagedType.LPArray)] string[] pbstrKey, [In][MarshalAs(UnmanagedType.U4)] int dw);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetDropTarget([In][MarshalAs(UnmanagedType.Interface)] IOleDropTarget pDropTarget, [MarshalAs(UnmanagedType.Interface)] out IOleDropTarget ppDropTarget);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetExternal([MarshalAs(UnmanagedType.Interface)] out object ppDispatch);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int TranslateUrl([In][MarshalAs(UnmanagedType.U4)] int dwTranslate, [In][MarshalAs(UnmanagedType.LPWStr)] string strURLIn, [MarshalAs(UnmanagedType.LPWStr)] out string pstrURLOut);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int FilterDataObject(IDataObject pDO, out IDataObject ppDORet);
	}

	[ComImport]
	[Guid("D30C1661-CDAF-11D0-8A3E-00C04FC9E26E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface IWebBrowser2
	{
		[DispId(203)]
		object Document
		{
			[DispId(203)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			get;
		}

		[DispId(551)]
		bool Silent
		{
			[DispId(551)]
			[param: MarshalAs(UnmanagedType.Bool)]
			set;
		}
	}

	[ComImport]
	[Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
	[TypeLibType(TypeLibTypeFlags.FHidden)]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface DWebBrowserEvents2
	{
		[DispId(102)]
		void StatusTextChange([In] string text);

		[DispId(108)]
		void ProgressChange([In] int progress, [In] int progressMax);

		[DispId(105)]
		void CommandStateChange([In] long command, [In] bool enable);

		[DispId(106)]
		void DownloadBegin();

		[DispId(104)]
		void DownloadComplete();

		[DispId(113)]
		void TitleChange([In] string text);

		[DispId(112)]
		void PropertyChange([In] string szProperty);

		[DispId(250)]
		void BeforeNavigate2([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers, [In][Out] ref bool cancel);

		[DispId(251)]
		void NewWindow2([In][Out][MarshalAs(UnmanagedType.IDispatch)] ref object pDisp, [In][Out] ref bool cancel);

		[DispId(252)]
		void NavigateComplete2([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);

		[DispId(259)]
		void DocumentComplete([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);

		[DispId(253)]
		void OnQuit();

		[DispId(254)]
		void OnVisible([In] bool visible);

		[DispId(255)]
		void OnToolBar([In] bool toolBar);

		[DispId(256)]
		void OnMenuBar([In] bool menuBar);

		[DispId(257)]
		void OnStatusBar([In] bool statusBar);

		[DispId(258)]
		void OnFullScreen([In] bool fullScreen);

		[DispId(260)]
		void OnTheaterMode([In] bool theaterMode);

		[DispId(262)]
		void WindowSetResizable([In] bool resizable);

		[DispId(264)]
		void WindowSetLeft([In] int left);

		[DispId(265)]
		void WindowSetTop([In] int top);

		[DispId(266)]
		void WindowSetWidth([In] int width);

		[DispId(267)]
		void WindowSetHeight([In] int height);

		[DispId(263)]
		void WindowClosing([In] bool isChildWindow, [In][Out] ref bool cancel);

		[DispId(268)]
		void ClientToHostWindow([In][Out] ref long cx, [In][Out] ref long cy);

		[DispId(269)]
		void SetSecureLockIcon([In] int secureLockIcon);

		[DispId(270)]
		void FileDownload([In][Out] ref bool cancel);

		[DispId(271)]
		void NavigateError([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object frame, [In] ref object statusCode, [In][Out] ref bool cancel);

		[DispId(225)]
		void PrintTemplateInstantiation([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp);

		[DispId(226)]
		void PrintTemplateTeardown([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp);

		[DispId(227)]
		void UpdatePageStatus([In][MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object nPage, [In] ref object fDone);

		[DispId(272)]
		void PrivacyImpactedStateChange([In] bool bImpacted);
	}

	internal static class NativeMethods
	{
		[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		internal static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		internal static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[DllImport("Gdi32.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

		[DllImport("User32.dll", ExactSpelling = true)]
		internal static extern bool IsProcessDPIAware();
	}
}
