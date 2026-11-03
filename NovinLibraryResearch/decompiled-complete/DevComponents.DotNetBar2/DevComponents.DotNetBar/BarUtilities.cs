using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace DevComponents.DotNetBar;

public class BarUtilities
{
	private const int ᆗ = -20;

	private const int ᆘ = 512;

	private const int _08F2 = 32;

	private const int _08ED = 1;

	private const int _08EC = 2;

	private const int _08EF = 4;

	private static volatile bool _ᆙ = true;

	private static bool _ᆚ = false;

	private static bool _ᆛ = false;

	private static int _ᆜ = 3;

	private static bool _ᆝ = false;

	public static bool FadeAnimatorEnabled
	{
		get
		{
			return _ᆙ;
		}
		set
		{
			_ᆙ = value;
		}
	}

	public static bool UseGenericDefaultStringFormat
	{
		get
		{
			return TextDrawing._၃;
		}
		set
		{
			TextDrawing._၃ = value;
		}
	}

	public static TextRenderingHint AntiAliasTextRenderingHint
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return DisplayHelp.AntiAliasTextRenderingHint;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			DisplayHelp.AntiAliasTextRenderingHint = value;
		}
	}

	public static bool UseTextRenderer
	{
		get
		{
			return TextDrawing.UseTextRenderer;
		}
		set
		{
			TextDrawing.UseTextRenderer = value;
		}
	}

	public static bool AlwaysGenerateAccessibilityFocusEvent
	{
		get
		{
			return _ᆚ;
		}
		set
		{
			_ᆚ = value;
		}
	}

	internal static bool ᆖ
	{
		get
		{
			for (int i = 0; i < ((ReadOnlyCollectionBase)(object)Application.OpenForms).Count; i++)
			{
				Form val = Application.OpenForms[i];
				if (val.Modal)
				{
					return true;
				}
			}
			return false;
		}
	}

	public static bool AutoRemoveMessageFilter
	{
		get
		{
			return _ᆛ;
		}
		set
		{
			_ᆛ = value;
		}
	}

	public static int TextMarkupCultureSpecificPadding
	{
		get
		{
			return _ᆜ;
		}
		set
		{
			_ᆜ = value;
		}
	}

	public static bool DisposeItemImages
	{
		get
		{
			return _ᆝ;
		}
		set
		{
			_ᆝ = value;
		}
	}

	public static void SetBarVisible(Bar bar, bool visible)
	{
		if (bar == null || bar.Visible == visible)
		{
			return;
		}
		DotNetBarManager dotNetBarManager = bar.Owner as DotNetBarManager;
		if (dotNetBarManager != null)
		{
			dotNetBarManager.SuspendLayout = true;
		}
		try
		{
			if (visible)
			{
				bar.Visible = true;
				if (bar.ந.ContainsKey(ᵙ.ᵚ))
				{
					bar.ந.Remove(ᵙ.ᵚ);
					bar.AutoHide = true;
				}
				return;
			}
			if (bar.ந.ContainsKey(ᵙ.ᵚ))
			{
				bar.ந.Remove(ᵙ.ᵚ);
			}
			if (bar.AutoHide)
			{
				bar.ந.Add(ᵙ.ᵚ, true);
			}
			bar.ఈ();
		}
		finally
		{
			if (dotNetBarManager != null)
			{
				dotNetBarManager.SuspendLayout = false;
			}
			bar.RecalcLayout();
		}
	}

	public static void SetDockContainerVisible(DockContainerItem item, bool visible)
	{
		if (item == null || item.Visible == visible)
		{
			return;
		}
		if (!(item.ContainerControl is Bar bar))
		{
			item.Visible = visible;
			return;
		}
		DotNetBarManager dotNetBarManager = bar.Owner as DotNetBarManager;
		if (dotNetBarManager != null)
		{
			dotNetBarManager.SuspendLayout = true;
		}
		try
		{
			int visibleItemCount = bar.VisibleItemCount;
			if (visible)
			{
				item.Visible = true;
				if (!bar.AutoHide && !bar.Visible && visibleItemCount <= 1)
				{
					bar.Visible = true;
					if (bar.ந.ContainsKey(ᵙ.ᵚ))
					{
						bar.ந.Remove(ᵙ.ᵚ);
						bar.AutoHide = true;
					}
				}
				return;
			}
			if (visibleItemCount <= 1)
			{
				if (bar.ந.ContainsKey(ᵙ.ᵚ))
				{
					bar.ந.Remove(ᵙ.ᵚ);
				}
				if (bar.AutoHide)
				{
					bar.ந.Add(ᵙ.ᵚ, true);
				}
				bar.ఈ();
			}
			item.Visible = false;
		}
		finally
		{
			if (dotNetBarManager != null)
			{
				dotNetBarManager.SuspendLayout = false;
			}
			bar.RecalcLayout();
		}
	}

	public static Bar CreateDocumentBar()
	{
		Bar bar = new Bar();
		InitializeDocumentBar(bar);
		return bar;
	}

	public static void InitializeDocumentBar(Bar bar)
	{
		TypeDescriptor.GetProperties(bar)["LayoutType"].SetValue(bar, eLayoutType.DockContainer);
		TypeDescriptor.GetProperties(bar)["DockTabAlignment"].SetValue(bar, eTabStripAlignment.Top);
		TypeDescriptor.GetProperties(bar)["AlwaysDisplayDockTab"].SetValue(bar, true);
		TypeDescriptor.GetProperties(bar)["Stretch"].SetValue(bar, true);
		TypeDescriptor.GetProperties(bar)["GrabHandleStyle"].SetValue(bar, eGrabHandleStyle.None);
		TypeDescriptor.GetProperties(bar)["CanDockBottom"].SetValue(bar, false);
		TypeDescriptor.GetProperties(bar)["CanDockTop"].SetValue(bar, false);
		TypeDescriptor.GetProperties(bar)["CanDockLeft"].SetValue(bar, false);
		TypeDescriptor.GetProperties(bar)["CanDockRight"].SetValue(bar, false);
		TypeDescriptor.GetProperties(bar)["CanDockDocument"].SetValue(bar, true);
		TypeDescriptor.GetProperties(bar)["CanUndock"].SetValue(bar, false);
		TypeDescriptor.GetProperties(bar)["CanHide"].SetValue(bar, true);
		TypeDescriptor.GetProperties(bar)["CanCustomize"].SetValue(bar, false);
		TypeDescriptor.GetProperties(bar)["TabNavigation"].SetValue(bar, true);
	}

	[DllImport("user32")]
	private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	[DllImport("user32")]
	private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

	[DllImport("user32")]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

	public static void ChangeMDIClientBorder(MdiClient c, bool removeBorder)
	{
		if (c != null)
		{
			int windowLong = GetWindowLong(((Control)c).Handle, -20);
			windowLong = ((!removeBorder) ? (windowLong | 0x200) : (windowLong ^ 0x200));
			SetWindowLong(((Control)c).Handle, -20, windowLong);
			SetWindowPos(((Control)c).Handle, IntPtr.Zero, 0, 0, 0, 0, 39);
		}
	}

	public static void ChangeMDIClientBorder(Form c, bool removeBorder)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		if (!c.IsMdiContainer || !((Control)c).IsHandleCreated)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)c).Controls)
		{
			Control val = item;
			if (val is MdiClient)
			{
				ChangeMDIClientBorder((MdiClient)(object)((val is MdiClient) ? val : null), removeBorder);
				break;
			}
		}
	}

	internal static void ડ(SubItemsCollection _104C)
	{
		foreach (BaseItem item in _104C)
		{
			ડ(item);
		}
	}

	internal static void ડ(BaseItem _06EC)
	{
		if (_06EC.TextMarkupBody != null)
		{
			_06EC.TextMarkupBody._0CC4();
		}
		if (_06EC.SubItems.Count > 0)
		{
			ડ(_06EC.SubItems);
		}
	}

	internal static void ᆞ(Control ॼ)
	{
		if (ॼ is Bar)
		{
			((Bar)(object)ॼ).RecalcLayout();
		}
		else if (ॼ is ItemControl)
		{
			((ItemControl)(object)ॼ).RecalcLayout();
		}
		else if (ॼ is BaseItemControl)
		{
			((BaseItemControl)(object)ॼ).RecalcLayout();
		}
		else if (ॼ is ExplorerBar)
		{
			((ExplorerBar)(object)ॼ).RecalcLayout();
		}
		else if (ॼ is SideBar)
		{
			((SideBar)(object)ॼ).RecalcLayout();
		}
	}

	internal static void ᆟ(ref Image ແ)
	{
		if (ແ != null)
		{
			ແ.Dispose();
			ແ = null;
		}
	}

	internal static void ᆟ(ref Icon ᆠ)
	{
		if (ᆠ != null)
		{
			ᆠ.Dispose();
			ᆠ = null;
		}
	}

	public static void InvokeDelayed(MethodInvoker method)
	{
		InvokeDelayed(method, 10);
	}

	public static void InvokeDelayed(MethodInvoker method, int delayInterval)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Timer val = new Timer();
		val = new Timer();
		val.Tag = method;
		val.Interval = delayInterval;
		val.Tick += ڥ;
		val.Start();
	}

	private static void ڥ(object _٠, EventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Timer val = (Timer)_٠;
		MethodInvoker val2 = (MethodInvoker)val.Tag;
		val.Stop();
		((Component)(object)val).Dispose();
		val2.Invoke();
	}
}
