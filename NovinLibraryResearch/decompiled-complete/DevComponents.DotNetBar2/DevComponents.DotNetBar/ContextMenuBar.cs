using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar;

[ToolboxItem(true)]
[DefaultEvent("ItemClick")]
[ComVisible(false)]
[Designer("DevComponents.DotNetBar.Design.ContextMenuBarDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ProvideProperty("ContextMenuEx", typeof(Control))]
public class ContextMenuBar : Bar, IExtenderProvider
{
	private class _᮲ : NativeWindow
	{
		private const int _08E4 = 123;

		private const int _08E0 = 517;

		private const int _᮴ = 165;

		private const int _08DF = 516;

		public Control _᮵;

		public event _᮶ _᮳;

		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).Msg == 123 && this._᮳ != null)
			{
				int num = _0EFE._0F11(((Message)(ref m)).LParam);
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
				_᮷ _᮷ = new _᮷(intPtr, num3, num2, (MouseButtons)((num3 != -1 || num2 != -1) ? 2097152 : 0), wmContextMessage, ((Message)(ref m)).HWnd);
				this._᮳(this, _᮷);
				if (_᮷.ᮻ)
				{
					return;
				}
			}
			((NativeWindow)this).WndProc(ref m);
		}
	}

	private delegate void _᮶(object sender, _᮷ e);

	private class _᮷ : EventArgs
	{
		private readonly int و;

		private readonly int ى;

		private readonly MouseButtons ᤉ;

		private readonly IntPtr _0F05;

		private readonly bool ᮺ;

		public bool ᮻ;

		private IntPtr ᮼ;

		public int ᒈ => و;

		public int ᒉ => ى;

		public MouseButtons _0F70 => ᤉ;

		public IntPtr ᖯ => _0F05;

		public bool _᮸ => ᮺ;

		public IntPtr _᮹ => ᮼ;

		public _᮷(IntPtr phwnd, int ix, int iy, MouseButtons eButton, bool WmContextMessage, IntPtr sourceHandle)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			و = ix;
			ى = iy;
			ᤉ = eButton;
			_0F05 = phwnd;
			ᮺ = WmContextMessage;
			ᮼ = sourceHandle;
		}
	}

	private Hashtable ล = new Hashtable();

	private Hashtable ฦ = new Hashtable();

	private bool _᭔ = true;

	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	private bool _᭕(object _᭖)
	{
		if (_᭖ is Control)
		{
			return true;
		}
		return false;
	}

	bool IExtenderProvider.CanExtend(object _᭖)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᭕
		return this._᭕(_᭖);
	}

	[DefaultValue(null)]
	[Editor(typeof(ContextExMenuTypeEditor), typeof(UITypeEditor))]
	public BaseItem GetContextMenuEx(Control control)
	{
		if (control is RichTextBoxEx)
		{
			control = (Control)(object)((RichTextBoxEx)(object)control).RichTextBox;
		}
		return (BaseItem)ล[control];
	}

	public void SetContextMenuEx(Control control, BaseItem value)
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		if (control is RichTextBoxEx)
		{
			control = (Control)(object)((RichTextBoxEx)(object)control).RichTextBox;
		}
		if (value == null)
		{
			if (!ล.Contains(control))
			{
				return;
			}
			if (ฦ.Contains(control))
			{
				if (ฦ[control] is _᮲ _᮲)
				{
					_᮲._᮳ -= _1B5D;
					((NativeWindow)_᮲).ReleaseHandle();
					_᮲ _᮲2 = null;
				}
				ฦ.Remove(control);
			}
			ล.Remove(control);
			control.MouseUp -= new MouseEventHandler(_1B5A);
			try
			{
				control.HandleDestroyed -= _᭘;
			}
			catch
			{
			}
			try
			{
				control.HandleCreated -= _᭙;
			}
			catch
			{
			}
			try
			{
				control.MouseUp -= new MouseEventHandler(_1B5A);
				return;
			}
			catch
			{
				return;
			}
		}
		if (ล.Contains(control))
		{
			ล[control] = value;
			return;
		}
		ล[control] = value;
		if (!ฦ.Contains(control) && !((Component)(object)this).DesignMode)
		{
			if (!(control is TreeView) && !(control is Form) && !(control is Panel) && !(control is DataGridView) && _᭔)
			{
				if (control.IsHandleCreated)
				{
					_᮲ _᮲3 = new _᮲();
					_᮲3._᮳ += _1B5D;
					_᮲3._᮵ = control;
					((NativeWindow)_᮲3).AssignHandle(control.Handle);
					ฦ[control] = _᮲3;
				}
				control.HandleDestroyed += _᭘;
				control.HandleCreated += _᭙;
			}
			if (control is ComboBox)
			{
				ComboBox val = (ComboBox)(object)((control is ComboBox) ? control : null);
				((Control)val).ContextMenu = new ContextMenu();
			}
		}
		try
		{
			control.MouseUp += new MouseEventHandler(_1B5A);
		}
		catch
		{
		}
	}

	internal bool _᭗(Control _0EF8)
	{
		return ล.Contains(_0EF8);
	}

	private void _᭘(object _٠, EventArgs ؿ)
	{
		Control val = (Control)((_٠ is Control) ? _٠ : null);
		if (val != null)
		{
			if (ฦ[val] is _᮲ _᮲)
			{
				_᮲._᮳ -= _1B5D;
				((NativeWindow)_᮲).ReleaseHandle();
				_᮲ _᮲2 = null;
			}
			ฦ.Remove(val);
		}
	}

	private void _᭙(object _٠, EventArgs ؿ)
	{
		Control val = (Control)((_٠ is Control) ? _٠ : null);
		if (val != null && !ฦ.Contains(val))
		{
			_᮲ _᮲ = new _᮲();
			_᮲._᮳ += _1B5D;
			_᮲._᮵ = val;
			((NativeWindow)_᮲).AssignHandle(val.Handle);
			ฦ[val] = _᮲;
		}
	}

	private void _1B5A(object _٠, MouseEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)ؿ.Button == 2097152)
		{
			Control val = (Control)((_٠ is Control) ? _٠ : null);
			if (val != null && _1B5C(val) is PopupItem { Expanded: false } popupItem)
			{
				popupItem.Style = base.Style;
				popupItem.SetSourceControl(val);
				popupItem.Popup(val.PointToScreen(new Point(ؿ.X, ؿ.Y)));
			}
		}
	}

	private Control _1B5B(Control ԟ, IntPtr ह)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)ԟ.Controls)
		{
			Control val = item;
			if (val.Handle == ह)
			{
				return val;
			}
			if (((ArrangedElementCollection)val.Controls).Count > 0)
			{
				Control val2 = _1B5B(val, ह);
				if (val2 != null)
				{
					return val2;
				}
			}
		}
		return ԟ;
	}

	private BaseItem _1B5C(Control _0EF8)
	{
		return (BaseItem)ล[_0EF8];
	}

	private void _1B5D(object _٠, _᮷ ؿ)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		if (!(_٠ is _᮲ _᮲))
		{
			return;
		}
		BaseItem baseItem = _1B5C(_᮲._᮵);
		if (baseItem == null)
		{
			return;
		}
		if (ؿ.ᖯ != ؿ._᮹ && ؿ._᮹ != IntPtr.Zero)
		{
			Control val = Control.FromChildHandle(ؿ.ᖯ);
			if (val != null && val is Panel && _1B5C(val) != null)
			{
				return;
			}
		}
		PopupItem popupItem = baseItem as PopupItem;
		popupItem.Style = base.Style;
		if ((int)ؿ._0F70 == 0)
		{
			Control val2 = Control.FromChildHandle(ؿ.ᖯ);
			if (val2 != null && val2.Handle != ؿ.ᖯ)
			{
				val2 = _1B5B(val2, ؿ.ᖯ);
			}
			popupItem.SetSourceControl(_᮲._᮵);
			if (val2 != null)
			{
				if (val2.ClientRectangle.Contains(val2.PointToClient(Control.MousePosition)))
				{
					popupItem.Popup(Control.MousePosition);
				}
				else
				{
					popupItem.Popup(val2.PointToScreen(Point.Empty));
				}
			}
			else
			{
				popupItem.Popup(Control.MousePosition);
			}
			if (base.ଝ)
			{
				base.ଝ = false;
				base.ଞ = true;
			}
		}
		else if (!ؿ._᮸)
		{
			popupItem.SetSourceControl(_᮲._᮵);
			popupItem.Popup(ؿ.ᒈ, ؿ.ᒉ);
		}
		ؿ.ᮻ = true;
	}
}
