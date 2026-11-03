using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class CrumbBarItemView : ButtonItem
{
	private CrumbBarItem __2238;

	private DateTime __2239 = DateTime.MinValue;

	private bool __223A;

	public CrumbBarItem AttachedItem
	{
		get
		{
			return __2238;
		}
		internal set
		{
			if (__2238 != null)
			{
				__2238.TextChanged -= _223E;
				__2238.SubItemsChanged -= _223C;
				__2238.PopupClose -= _223B;
			}
			__2238 = value;
			if (__2238 != null)
			{
				Text = __2238.Text;
				Cursor = __2238.Cursor;
				ForeColor = __2238.ForeColor;
				HotForeColor = __2238.HotForeColor;
				Tooltip = __2238.Tooltip;
				__2238.TextChanged += _223E;
				__2238.SubItemsChanged += _223C;
				__2238.PopupClose += _223B;
				if (_2237)
				{
					PopupType = ePopupType.Container;
				}
				else
				{
					PopupType = ePopupType.Menu;
				}
			}
			else
			{
				PopupType = ePopupType.Menu;
			}
		}
	}

	private bool _2237
	{
		get
		{
			if (__2238 != null)
			{
				foreach (BaseItem subItem in __2238.SubItems)
				{
					if (subItem.Visible)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool Expanded
	{
		get
		{
			return __223A;
		}
		set
		{
			if (__223A != value)
			{
				__223A = value;
				_2240();
			}
		}
	}

	public CrumbBarItemView()
	{
		SubItemsExpandWidth = 15;
		base._0E63 = 2;
	}

	private void _223B(object _٠, EventArgs ؿ)
	{
		__223A = false;
		if (Parent is _21E3 _21E32)
		{
			_21E32.OnSubItemExpandChange(this);
		}
		__2239 = DateTime.Now;
		Refresh();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		if (__2239 != DateTime.MinValue && DateTime.Now.Subtract(__2239).TotalMilliseconds < 200.0)
		{
			__2239 = DateTime.MinValue;
		}
		else
		{
			base.InternalMouseDown(objArg);
		}
	}

	public override void InternalMouseEnter()
	{
		if (Parent is _21E3 { Expanded: not false } _21E32 && _2237)
		{
			_21E32.Expanded = false;
			Expanded = true;
		}
		base.InternalMouseEnter();
	}

	private void _223C(object _٠, CollectionChangeEventArgs ؿ)
	{
		_223D();
	}

	private void _223D()
	{
		if (_2237 && !(Parent is CrumbBarOverflowButton))
		{
			PopupType = ePopupType.Container;
		}
		else
		{
			PopupType = ePopupType.Menu;
		}
	}

	protected override void OnParentChanged()
	{
		_223D();
		base.OnParentChanged();
	}

	private void _223E(object _٠, EventArgs ؿ)
	{
		Text = __2238.Text;
	}

	protected override void Dispose(bool disposing)
	{
		AttachedItem = null;
		base.Dispose(disposing);
	}

	internal static object _223F(CrumbBarItem _06EC)
	{
		CrumbBarItemView crumbBarItemView = new CrumbBarItemView();
		crumbBarItemView.AttachedItem = _06EC;
		return crumbBarItemView;
	}

	private void _2240()
	{
		if (!__223A)
		{
			__2238.Expanded = false;
		}
		else
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val == null)
			{
				return;
			}
			Point point = new Point(base.SubItemsRect.X - 10, base.SubItemsRect.Bottom - 1);
			point.Offset(DisplayRectangle.Location.X, DisplayRectangle.Location.Y);
			point = val.PointToScreen(point);
			__2238.PopupMenu(point);
		}
		if (Parent is _21E3 _21E32)
		{
			_21E32.OnSubItemExpandChange(this);
		}
	}

	protected override void OnClick()
	{
		CrumbBar crumbBar = ContainerControl as CrumbBar;
		if (crumbBar == null)
		{
			crumbBar = GetOwner() as CrumbBar;
		}
		base.OnClick();
		if (crumbBar != null)
		{
			crumbBar.SelectedItem = __2238;
		}
	}

	protected override void RenderButton(ItemPaintArgs p)
	{
		if (!p.IsOnMenu)
		{
			BaseRenderer ໝ = p.ໝ;
			if (ໝ != null)
			{
				p.ButtonItemRendererEventArgs.Graphics = p.Graphics;
				p.ButtonItemRendererEventArgs.ButtonItem = this;
				p.ButtonItemRendererEventArgs.က = p;
				ໝ.DrawCrumbBarItemView(p.ButtonItemRendererEventArgs);
				return;
			}
		}
		base.RenderButton(p);
	}

	public override BaseItem Copy()
	{
		CrumbBarItemView crumbBarItemView = new CrumbBarItemView();
		CopyToItem(crumbBarItemView);
		return crumbBarItemView;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		CrumbBarItemView copy2 = copy as CrumbBarItemView;
		base.CopyToItem((BaseItem)copy2);
	}
}
