using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Controls;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ProvideProperty("ContextMenuEx", typeof(Control))]
[ComVisible(false)]
[DefaultEvent("ItemClick")]
public class ContextMenuBar : Bar, IExtenderProvider
{
	private delegate void WmContextEventHandler(object sender, WmContextEventArgs e);

	private class ContextMessageHandler : NativeWindow
	{
		private const int WM_CONTEXTMENU = 123;

		private const int WM_RBUTTONUP = 517;

		private const int WM_NCRBUTTONUP = 165;

		private const int WM_RBUTTONDOWN = 516;

		public Control ParentControl;

		public event WmContextEventHandler ContextMenu;

		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).Msg == 123 && this.ContextMenu != null)
			{
				int num = WinApi.ToInt(((Message)(ref m)).LParam);
				int num2 = num >> 16;
				int num3 = (short)(num & 0xFFFF);
				IntPtr intPtr = ((Message)(ref m)).WParam;
				if (intPtr == IntPtr.Zero)
				{
					intPtr = ((Message)(ref m)).HWnd;
				}
				bool wmContextMessage = true;
				if (((Message)(ref m)).HWnd != ((Message)(ref m)).WParam)
				{
					wmContextMessage = false;
				}
				WmContextEventArgs wmContextEventArgs = new WmContextEventArgs(intPtr, num3, num2, (MouseButtons)((num3 != 65535 || num2 != -1) ? 2097152 : 0), wmContextMessage);
				this.ContextMenu(this, wmContextEventArgs);
				if (wmContextEventArgs.Handled)
				{
					return;
				}
			}
			((NativeWindow)this).WndProc(ref m);
		}
	}

	private class WmContextEventArgs : EventArgs
	{
		private readonly int x;

		private readonly int y;

		private readonly MouseButtons button;

		private readonly IntPtr hwnd;

		private readonly bool wmcontext;

		public bool Handled;

		public int X => x;

		public int Y => y;

		public MouseButtons Button => button;

		public IntPtr Handle => hwnd;

		public bool WmContext => wmcontext;

		public WmContextEventArgs(IntPtr phwnd, int ix, int iy, MouseButtons eButton, bool WmContextMessage)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			x = ix;
			y = iy;
			button = eButton;
			hwnd = phwnd;
			wmcontext = WmContextMessage;
		}
	}

	private Hashtable m_ContextExMenus = new Hashtable();

	private Hashtable m_ContextExHandlers = new Hashtable();

	private bool m_ContextMenuSubclass = true;

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public new bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			base.Visible = value;
		}
	}

	public ContextMenuBar()
	{
		Visible = false;
		base.WrapItemsDock = true;
		base.WrapItemsFloat = true;
	}

	protected override bool IsContextPopup(BaseItem popup)
	{
		if (base.Items.Contains(popup))
		{
			return true;
		}
		return base.IsContextPopup(popup);
	}

	bool IExtenderProvider.CanExtend(object target)
	{
		if (target is Control)
		{
			return true;
		}
		return false;
	}

	[DefaultValue(null)]
	[Editor(typeof(ContextExMenuTypeEditor), typeof(UITypeEditor))]
	public BaseItem GetContextMenuEx(Control control)
	{
		if (control is RichTextBoxEx)
		{
			control = (Control)(object)((RichTextBoxEx)(object)control).RichTextBox;
		}
		return (BaseItem)m_ContextExMenus[control];
	}

	public void SetContextMenuEx(Control control, BaseItem value)
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		if (control is RichTextBoxEx)
		{
			control = (Control)(object)((RichTextBoxEx)(object)control).RichTextBox;
		}
		if (value == null)
		{
			if (!m_ContextExMenus.Contains(control))
			{
				return;
			}
			if (m_ContextExHandlers.Contains(control))
			{
				if (m_ContextExHandlers[control] is ContextMessageHandler contextMessageHandler)
				{
					contextMessageHandler.ContextMenu -= OnContextMenu;
					((NativeWindow)contextMessageHandler).ReleaseHandle();
					ContextMessageHandler contextMessageHandler2 = null;
				}
				m_ContextExHandlers.Remove(control);
			}
			m_ContextExMenus.Remove(control);
			control.MouseUp -= new MouseEventHandler(ContextExMouseUp);
			try
			{
				control.HandleDestroyed -= ContextExHandleDestroy;
			}
			catch
			{
			}
			try
			{
				control.HandleCreated -= ContextExHandleCreate;
			}
			catch
			{
			}
			try
			{
				control.MouseUp -= new MouseEventHandler(ContextExMouseUp);
				return;
			}
			catch
			{
				return;
			}
		}
		if (m_ContextExMenus.Contains(control))
		{
			m_ContextExMenus[control] = value;
			return;
		}
		m_ContextExMenus[control] = value;
		if (!m_ContextExHandlers.Contains(control) && !((Component)(object)this).DesignMode)
		{
			if (!(control is TreeView) && !(control is Form) && !(control is Panel) && !(control is DataGridView) && m_ContextMenuSubclass)
			{
				if (control.IsHandleCreated)
				{
					ContextMessageHandler contextMessageHandler3 = new ContextMessageHandler();
					contextMessageHandler3.ContextMenu += OnContextMenu;
					contextMessageHandler3.ParentControl = control;
					((NativeWindow)contextMessageHandler3).AssignHandle(control.Handle);
					m_ContextExHandlers[control] = contextMessageHandler3;
				}
				control.HandleDestroyed += ContextExHandleDestroy;
				control.HandleCreated += ContextExHandleCreate;
			}
			if (control is ComboBox)
			{
				ComboBox val = (ComboBox)(object)((control is ComboBox) ? control : null);
				((Control)val).ContextMenu = new ContextMenu();
			}
		}
		try
		{
			control.MouseUp += new MouseEventHandler(ContextExMouseUp);
		}
		catch
		{
		}
	}

	internal bool HasContextMenu(Control ctrl)
	{
		return m_ContextExMenus.Contains(ctrl);
	}

	private void ContextExHandleDestroy(object sender, EventArgs e)
	{
		Control val = (Control)((sender is Control) ? sender : null);
		if (val != null)
		{
			if (m_ContextExHandlers[val] is ContextMessageHandler contextMessageHandler)
			{
				contextMessageHandler.ContextMenu -= OnContextMenu;
				((NativeWindow)contextMessageHandler).ReleaseHandle();
				ContextMessageHandler contextMessageHandler2 = null;
			}
			m_ContextExHandlers.Remove(val);
		}
	}

	private void ContextExHandleCreate(object sender, EventArgs e)
	{
		Control val = (Control)((sender is Control) ? sender : null);
		if (val != null && !m_ContextExHandlers.Contains(val))
		{
			ContextMessageHandler contextMessageHandler = new ContextMessageHandler();
			contextMessageHandler.ContextMenu += OnContextMenu;
			contextMessageHandler.ParentControl = val;
			((NativeWindow)contextMessageHandler).AssignHandle(val.Handle);
			m_ContextExHandlers[val] = contextMessageHandler;
		}
	}

	private void ContextExMouseUp(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 2097152)
		{
			Control val = (Control)((sender is Control) ? sender : null);
			if (val != null && GetContextExItem(val) is PopupItem { Expanded: false } popupItem)
			{
				popupItem.Style = base.Style;
				popupItem.SetSourceControl(val);
				popupItem.Popup(val.PointToScreen(new Point(e.X, e.Y)));
			}
		}
	}

	private Control FindControl(Control parent, IntPtr handle)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)parent.Controls)
		{
			Control val = item;
			if (val.Handle == handle)
			{
				return val;
			}
			if (((ArrangedElementCollection)val.Controls).Count > 0)
			{
				Control val2 = FindControl(val, handle);
				if (val2 != null)
				{
					return val2;
				}
			}
		}
		return parent;
	}

	private BaseItem GetContextExItem(Control ctrl)
	{
		return (BaseItem)m_ContextExMenus[ctrl];
	}

	private void OnContextMenu(object sender, WmContextEventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (!(sender is ContextMessageHandler contextMessageHandler))
		{
			return;
		}
		BaseItem contextExItem = GetContextExItem(contextMessageHandler.ParentControl);
		if (contextExItem == null)
		{
			return;
		}
		PopupItem popupItem = contextExItem as PopupItem;
		popupItem.Style = base.Style;
		if ((int)e.Button == 0)
		{
			Control val = Control.FromChildHandle(e.Handle);
			if (val != null && val.Handle != e.Handle)
			{
				val = FindControl(val, e.Handle);
			}
			popupItem.SetSourceControl(contextMessageHandler.ParentControl);
			if (val != null)
			{
				popupItem.Popup(val.PointToScreen(Point.Empty));
			}
			else
			{
				popupItem.Popup(Control.MousePosition);
			}
			if (base.IgnoreSysKeyUp)
			{
				base.IgnoreSysKeyUp = false;
				base.EatSysKeyUp = true;
			}
		}
		else if (!e.WmContext)
		{
			popupItem.SetSourceControl(contextMessageHandler.ParentControl);
			popupItem.Popup(e.X, e.Y);
		}
		e.Handled = true;
	}
}
