using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class BarUtilities
{
	private const int GWL_EXSTYLE = -20;

	private const int WS_EX_CLIENTEDGE = 512;

	private const int SWP_FRAMECHANGED = 32;

	private const int SWP_NOSIZE = 1;

	private const int SWP_NOMOVE = 2;

	private const int SWP_NOZORDER = 4;

	private static bool _AlwaysGenerateAccessibilityFocusEvent = false;

	private static bool _AutoRemoveMessageFilter = false;

	private static int _TextMarkupCultureSpecific = 3;

	private static bool _DisposeItemImages = false;

	public static bool UseGenericDefaultStringFormat
	{
		get
		{
			return TextDrawing.UseGenericDefault;
		}
		set
		{
			TextDrawing.UseGenericDefault = value;
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
			return _AlwaysGenerateAccessibilityFocusEvent;
		}
		set
		{
			_AlwaysGenerateAccessibilityFocusEvent = value;
		}
	}

	internal static bool IsModalFormOpen
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
			return _AutoRemoveMessageFilter;
		}
		set
		{
			_AutoRemoveMessageFilter = value;
		}
	}

	public static int TextMarkupCultureSpecificPadding
	{
		get
		{
			return _TextMarkupCultureSpecific;
		}
		set
		{
			_TextMarkupCultureSpecific = value;
		}
	}

	public static bool DisposeItemImages
	{
		get
		{
			return _DisposeItemImages;
		}
		set
		{
			_DisposeItemImages = value;
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
				if (bar.PropertyBag.ContainsKey(BarPropertyBagKeys.AutoHideSetting))
				{
					bar.PropertyBag.Remove(BarPropertyBagKeys.AutoHideSetting);
					bar.AutoHide = true;
				}
				return;
			}
			if (bar.PropertyBag.ContainsKey(BarPropertyBagKeys.AutoHideSetting))
			{
				bar.PropertyBag.Remove(BarPropertyBagKeys.AutoHideSetting);
			}
			if (bar.AutoHide)
			{
				bar.PropertyBag.Add(BarPropertyBagKeys.AutoHideSetting, true);
			}
			bar.CloseBar();
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
					if (bar.PropertyBag.ContainsKey(BarPropertyBagKeys.AutoHideSetting))
					{
						bar.PropertyBag.Remove(BarPropertyBagKeys.AutoHideSetting);
						bar.AutoHide = true;
					}
				}
				return;
			}
			if (visibleItemCount <= 1)
			{
				if (bar.PropertyBag.ContainsKey(BarPropertyBagKeys.AutoHideSetting))
				{
					bar.PropertyBag.Remove(BarPropertyBagKeys.AutoHideSetting);
				}
				if (bar.AutoHide)
				{
					bar.PropertyBag.Add(BarPropertyBagKeys.AutoHideSetting, true);
				}
				bar.CloseBar();
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

	internal static void InvalidateFontChange(SubItemsCollection col)
	{
		foreach (BaseItem item in col)
		{
			InvalidateFontChange(item);
		}
	}

	internal static void InvalidateFontChange(BaseItem item)
	{
		if (item.TextMarkupBody != null)
		{
			item.TextMarkupBody.InvalidateElementsSize();
		}
		if (item.SubItems.Count > 0)
		{
			InvalidateFontChange(item.SubItems);
		}
	}

	internal static void InvokeRecalcLayout(Control control)
	{
		if (control is Bar)
		{
			((Bar)(object)control).RecalcLayout();
		}
		else if (control is ItemControl)
		{
			((ItemControl)(object)control).RecalcLayout();
		}
		else if (control is BaseItemControl)
		{
			((BaseItemControl)(object)control).RecalcLayout();
		}
		else if (control is ExplorerBar)
		{
			((ExplorerBar)(object)control).RecalcLayout();
		}
		else if (control is SideBar)
		{
			((SideBar)(object)control).RecalcLayout();
		}
	}

	internal static void DisposeImage(ref Image image)
	{
		if (image != null)
		{
			image.Dispose();
			image = null;
		}
	}

	internal static void DisposeImage(ref Icon icon)
	{
		if (icon != null)
		{
			icon.Dispose();
			icon = null;
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
		val.Tick += DelayedInvokeTimerTick;
		val.Start();
	}

	private static void DelayedInvokeTimerTick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Timer val = (Timer)sender;
		MethodInvoker val2 = (MethodInvoker)val.Tag;
		val.Stop();
		((Component)(object)val).Dispose();
		val2.Invoke();
	}
}
