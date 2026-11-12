using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

internal class PopupHookManager
{
	private static bool inMouseHook;

	private static IntPtr mouseHookHandle;

	private static HookHandler mouseHookProc;

	private static Point mousePos;

	private static ArrayList Popups;

	internal static int PopupCount => Popups.Count;

	static PopupHookManager()
	{
		inMouseHook = false;
		mouseHookHandle = IntPtr.Zero;
		Popups = new ArrayList();
		mousePos = Control.MousePosition;
	}

	internal static bool NeedCloseOnLostFocus(ICodeCompletionWindow wnd)
	{
		if ((wnd.CompletionFlags & CodeCompletionFlags.CloseOnLostFocus) == 0)
		{
			return false;
		}
		foreach (PopupWnd popup in Popups)
		{
			CodeCompletionWindow codeCompletionWindow = (CodeCompletionWindow)popup.Popup;
			if (codeCompletionWindow != null && ((Control)codeCompletionWindow).Visible && codeCompletionWindow.IsFocused())
			{
				return false;
			}
		}
		return true;
	}

	internal static void CheckMouse(Control control, Point mousePos, bool mouseMove)
	{
		for (int num = Popups.Count - 1; num >= 0; num--)
		{
			PopupWnd popupWnd = (PopupWnd)Popups[num];
			CodeCompletionWindow codeCompletionWindow = (CodeCompletionWindow)popupWnd.Popup;
			ICodeCompletionWindow popup = popupWnd.Popup;
			if (popup != null && ((Control)codeCompletionWindow).Created && ((Control)codeCompletionWindow).Visible)
			{
				Control val = (Control)(object)((Control)codeCompletionWindow).FindForm();
				if (val != null && !val.Contains(control) && val != control && codeCompletionWindow != control && !codeCompletionWindow.ContainsControl(control))
				{
					if (mouseMove && codeCompletionWindow.OwnerControl != null && (codeCompletionWindow.CompletionFlags & CodeCompletionFlags.CloseOnMouseLeave) != 0)
					{
						Control ownerControl = codeCompletionWindow.OwnerControl;
						Rectangle rectangle = new Rectangle(0, 0, ownerControl.Width, ownerControl.Height);
						if (!((Control)codeCompletionWindow).Bounds.Contains(mousePos) && !rectangle.Contains(ownerControl.PointToClient(mousePos)))
						{
							popup.CloseDelayed(accept: false);
						}
					}
					else if (!mouseMove && !((Control)codeCompletionWindow).Bounds.Contains(mousePos))
					{
						popup.CloseDelayed(accept: false);
					}
				}
			}
		}
	}

	internal static void InstallHook()
	{
		mouseHookProc = MouseHook;
		mouseHookHandle = OSUtils.SetMouseHook(mouseHookProc);
		Application.ApplicationExit += OnApplicationExit;
	}

	internal static void RemoveHook()
	{
		if (mouseHookHandle != IntPtr.Zero)
		{
			Application.ApplicationExit -= OnApplicationExit;
			OSUtils.ReleaseHook(mouseHookHandle);
			mouseHookHandle = IntPtr.Zero;
			mouseHookProc = null;
		}
	}

	internal static void OnApplicationExit(object sender, EventArgs e)
	{
		RemoveHook();
	}

	private static IntPtr MouseHook(int ncode, IntPtr wParam, IntPtr lParam)
	{
		if (ncode == 0)
		{
			if (inMouseHook || !(lParam != IntPtr.Zero))
			{
				return OSUtils.CallNextHook(mouseHookHandle, ncode, wParam, lParam);
			}
			Point pt;
			IntPtr hWnd = OSUtils.GetMouseHookHandle(lParam, out pt);
			try
			{
				inMouseHook = true;
				FilterMouseMessage(wParam.ToInt32(), hWnd);
			}
			finally
			{
				inMouseHook = false;
			}
		}
		return OSUtils.CallNextHook(mouseHookHandle, ncode, wParam, lParam);
	}

	internal static bool IsSignificantMouseMove(Point pt)
	{
		if (Math.Abs(pt.X - mousePos.X) <= 5)
		{
			return Math.Abs(pt.Y - mousePos.Y) > 5;
		}
		return true;
	}

	internal static void FilterMouseMessage(int msg, IntPtr hWnd)
	{
		Point mousePosition = Control.MousePosition;
		if (OSUtils.IsMouseMsg(msg) || (msg == 512 && IsSignificantMouseMove(mousePosition)))
		{
			if (msg == 512)
			{
				mousePos = mousePosition;
			}
			CheckMouse(Control.FromHandle(hWnd), mousePosition, msg == 512);
		}
	}

	internal static void PopupClosed(ICodeCompletionWindow popup)
	{
		for (int num = Popups.Count - 1; num >= 0; num--)
		{
			PopupWnd popupWnd = (PopupWnd)Popups[num];
			if (popupWnd.Popup == popup)
			{
				popupWnd.Release();
				Popups.RemoveAt(num);
				break;
			}
		}
		if (Popups.Count == 0)
		{
			RemoveHook();
		}
	}

	internal static void PopupShowing(ICodeCompletionWindow popup)
	{
		Popups.Add(new PopupWnd(popup, (Control)(object)((CodeCompletionWindow)popup).OwnerControl.FindForm()));
		if (mouseHookHandle == IntPtr.Zero)
		{
			InstallHook();
		}
	}
}
