using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Stimulsoft.Dashboard.Viewer.Elements;

[ToolboxItem(false)]
public class StiDropDown : ToolStripDropDown
{
	private static HandleRef HWND_TOPMOST = new HandleRef(null, new IntPtr(-1));

	private const int AnimationSlide = 262144;

	private const int AnimationVerticalPositive = 4;

	protected override void OnVisibleChanged(EventArgs e)
	{
		((ToolStripDropDown)this).OnVisibleChanged(e);
		SetTopMost((Control)(object)this);
		AnimateWindow((Control)(object)this, 100, 262148);
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	[SuppressUnmanagedCodeSecurity]
	private static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	[SuppressUnmanagedCodeSecurity]
	private static extern int AnimateWindow(HandleRef windowHandle, int time, int flags);

	internal static void SetTopMost(Control control)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		try
		{
			SecurityPermission val = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			((CodeAccessPermission)val).Demand();
			SetWindowPos(new HandleRef(control, control.Handle), HWND_TOPMOST, 0, 0, 0, 0, 19);
		}
		catch (SecurityException)
		{
		}
	}

	internal static void AnimateWindow(Control control, int time, int flags)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		try
		{
			SecurityPermission val = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			((CodeAccessPermission)val).Demand();
			AnimateWindow(new HandleRef(control, control.Handle), time, flags);
		}
		catch (SecurityException)
		{
		}
	}
}
