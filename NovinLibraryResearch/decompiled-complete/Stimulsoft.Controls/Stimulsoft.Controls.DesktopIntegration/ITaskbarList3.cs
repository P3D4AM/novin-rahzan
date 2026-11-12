using System;
using System.Runtime.InteropServices;

namespace Stimulsoft.Controls.DesktopIntegration;

[ComImport]
[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ITaskbarList3
{
	[PreserveSig]
	void HrInit();

	[PreserveSig]
	void AddTab(IntPtr hwnd);

	[PreserveSig]
	void DeleteTab(IntPtr hwnd);

	[PreserveSig]
	void ActivateTab(IntPtr hwnd);

	[PreserveSig]
	void SetActiveAlt(IntPtr hwnd);

	[PreserveSig]
	void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

	void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

	void SetProgressState(IntPtr hwnd, TBPFLAG tbpFlags);

	void RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

	void UnregisterTab(IntPtr hwndTab);

	void SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

	void SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, TBATFLAG tbatFlags);

	void ThumbBarAddButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray)] THUMBBUTTON[] pButtons);

	void ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray)] THUMBBUTTON[] pButtons);

	void ThumbBarSetImageList(IntPtr hwnd, IntPtr himl);

	void SetOverlayIcon(IntPtr hwnd, IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

	void SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

	void SetThumbnailClip(IntPtr hwnd, ref RECT prcClip);
}
