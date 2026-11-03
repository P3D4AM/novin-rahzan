using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[ComVisible(false)]
[ToolboxItem(false)]
public class SideBarPanelControlHost : Control, ል
{
	private SideBarPanelItem _1DF5;

	private BaseItem _1DF6;

	private Point _0AC2 = Point.Empty;

	private _0A79 ৰ;

	private _0A7A ৱ;

	private _0A7B _09F2;

	private _0A7C _09F3;

	private _0A7D _09F4;

	private _0A7E _09F5;

	private _0A7F _09F6;

	private _0A80 _09F7;

	private _1DFE _0DCD;

	private _1DFE _0DCE;

	_0A79 ል.ফ
	{
		get
		{
			if (ৰ == null)
			{
				ৰ = new _0A79((Control)(object)this);
			}
			return ৰ;
		}
	}

	_0A7A ል.ব
	{
		get
		{
			if (ৱ == null)
			{
				ৱ = new _0A7A((Control)(object)this);
			}
			return ৱ;
		}
	}

	_0A7B ል.ভ
	{
		get
		{
			if (_09F2 == null)
			{
				_09F2 = new _0A7B((Control)(object)this);
			}
			return _09F2;
		}
	}

	_0A7C ል.ম
	{
		get
		{
			if (_09F3 == null)
			{
				_09F3 = new _0A7C((Control)(object)this);
			}
			return _09F3;
		}
	}

	_0A7D ል.য
	{
		get
		{
			if (_09F4 == null)
			{
				_09F4 = new _0A7D((Control)(object)this);
			}
			return _09F4;
		}
	}

	_0A7E ል.র
	{
		get
		{
			if (_09F5 == null)
			{
				_09F5 = new _0A7E((Control)(object)this);
			}
			return _09F5;
		}
	}

	_0A7F ል._09B1
	{
		get
		{
			if (_09F6 == null)
			{
				_09F6 = new _0A7F((Control)(object)this);
			}
			return _09F6;
		}
	}

	_0A80 ል.ল
	{
		get
		{
			if (_09F7 == null)
			{
				_09F7 = new _0A80((Control)(object)this);
			}
			return _09F7;
		}
	}

	public SideBarPanelControlHost(SideBarPanelItem parentPanel)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).TabStop = false;
		_0DCD = new _1DFE();
		_0DCD.Ḃ = false;
		_0DCD.ḃ = true;
		((Control)_0DCD).Visible = false;
		_0DCD.ᡥ = eOrientation.Vertical;
		_0DCD._1DFF = eItemAlignment.Near;
		ColorScheme colorScheme = null;
		if (parentPanel.Appearance == eSideBarAppearance.Flat)
		{
			((Control)_0DCD).Size = new Size(14, 14);
			if (parentPanel.HeaderStyle != null)
			{
				colorScheme = new ColorScheme();
				colorScheme.ItemBackground = Color.Empty;
				colorScheme.ItemBackground = Color.Empty;
				colorScheme.ItemBackground = parentPanel.HeaderStyle.BackColor1.Color;
				colorScheme.ItemHotBackground = parentPanel.HeaderStyle.BackColor1.Color;
				colorScheme.ItemHotBackground2 = parentPanel.HeaderStyle.BackColor2.Color;
				colorScheme.ItemHotBorder = parentPanel.HeaderStyle.BorderColor.Color;
				colorScheme.ItemPressedBorder = parentPanel.HeaderStyle.BorderColor.Color;
				colorScheme.ItemText = parentPanel.HeaderStyle.ForeColor.Color;
				colorScheme.ItemHotText = parentPanel.HeaderStyle.ForeColor.Color;
				colorScheme.ItemPressedText = parentPanel.HeaderStyle.ForeColor.Color;
				colorScheme.ItemPressedBackground = ControlPaint.Light(parentPanel.HeaderStyle.BackColor1.Color);
				colorScheme.ItemPressedBackground2 = ControlPaint.Light(parentPanel.HeaderStyle.BackColor2.Color);
				_0DCD._ḁ = colorScheme;
				_0DCD.ḃ = false;
			}
		}
		else
		{
			((Control)_0DCD).Size = new Size(16, 16);
		}
		((Control)this).Controls.Add((Control)(object)_0DCD);
		_0DCE = new _1DFE();
		_0DCE.Ḃ = false;
		_0DCE.ḃ = true;
		((Control)_0DCE).Visible = false;
		_0DCE.ᡥ = eOrientation.Vertical;
		_0DCE._1DFF = eItemAlignment.Far;
		if (parentPanel.Appearance == eSideBarAppearance.Flat)
		{
			((Control)_0DCE).Size = new Size(14, 14);
			if (colorScheme != null)
			{
				_0DCE._ḁ = colorScheme;
				_0DCE.ḃ = false;
			}
		}
		else
		{
			((Control)_0DCE).Size = new Size(16, 16);
		}
		((Control)this).Controls.Add((Control)(object)_0DCE);
		((Control)_0DCE).MouseDown += new MouseEventHandler(ᥗ);
		((Control)_0DCD).MouseDown += new MouseEventHandler(ᥗ);
		_1DF5 = parentPanel;
		foreach (BaseItem subItem in _1DF5.SubItems)
		{
			subItem.ContainerControl = this;
		}
	}

	public void SetupScrollButtons()
	{
		if (_1DF5.TopItemIndex > 0 && _1DF5.EnableScrollButtons)
		{
			((Control)_0DCD).Location = new Point(((Control)this).Width - (((Control)_0DCD).Width + 2), 4);
			_0DCD._193C = _1DF5._193C;
			((Control)_0DCD).BringToFront();
			if (!((Control)_0DCD).Visible)
			{
				((Control)_0DCD).Visible = true;
			}
		}
		else
		{
			((Control)_0DCD).Visible = false;
		}
		if (_1DF5._193B && _1DF5.EnableScrollButtons)
		{
			((Control)_0DCE).Location = new Point(((Control)this).Width - (((Control)_0DCE).Width + 2), ((Control)this).Height - (((Control)_0DCE).Height + 2));
			_0DCE._193C = _1DF5._193C;
			((Control)_0DCE).BringToFront();
			if (!((Control)_0DCE).Visible)
			{
				((Control)_0DCE).Visible = true;
			}
		}
		else
		{
			((Control)_0DCE).Visible = false;
		}
	}

	private void ᥗ(object _٠, MouseEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)ؿ.Button == 1048576)
		{
			if (_٠ == _0DCD)
			{
				_1DF5.ᥗ(ᥖ: true);
			}
			else
			{
				_1DF5.ᥗ(ᥖ: false);
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (_1DF5 == null || ((Control)this).IsDisposed)
		{
			return;
		}
		ItemPaintArgs itemPaintArgs = null;
		SideBar sideBar = _1DF5.ContainerControl as SideBar;
		itemPaintArgs = ((sideBar == null) ? new ItemPaintArgs(_1DF5.GetOwner() as IOwner, (Control)(object)this, e.Graphics, new ColorScheme(e.Graphics)) : new ItemPaintArgs(_1DF5.GetOwner() as IOwner, (Control)(object)this, e.Graphics, sideBar.ColorScheme));
		itemPaintArgs.ໝ = sideBar.GetRenderer();
		if (_1DF5._193C)
		{
			((ል)this)._0A7A._0EE5(e.Graphics, ཤ.ཥ, ཫ.ཊ, ((Control)this).DisplayRectangle);
		}
		else if (!_1DF5.BackgroundStyle.BackColor1.IsEmpty)
		{
			if (_1DF5.Appearance == eSideBarAppearance.Flat)
			{
				eBorderSide borderSide = _1DF5.BackgroundStyle.BorderSide;
				_1DF5.BackgroundStyle.BorderSide = ~(borderSide & eBorderSide.Top) & borderSide;
				_1DF5.BackgroundStyle.Paint(e.Graphics, ((Control)this).ClientRectangle);
				_1DF5.BackgroundStyle.BorderSide = borderSide;
			}
			else
			{
				_1DF5.BackgroundStyle.Paint(e.Graphics, ((Control)this).ClientRectangle);
			}
		}
		else
		{
			Color color = SystemColors.Control;
			if (sideBar != null && BarFunctions.IsOffice2007Style(sideBar.Style) && ((Control)sideBar).BackColor == SystemColors.Control)
			{
				if (itemPaintArgs.ໝ is Office2007Renderer)
				{
					DisplayHelp.FillRectangle(e.Graphics, ((Control)this).ClientRectangle, ((Office2007Renderer)itemPaintArgs.ໝ).ColorTable.SideBar.Background);
					color = Color.Empty;
				}
			}
			else if (((Control)this).Parent != null && ((Control)this).Parent.BackColor != Color.Transparent)
			{
				color = ((Control)this).Parent.BackColor;
			}
			DisplayHelp.FillRectangle(e.Graphics, ((Control)this).ClientRectangle, color, Color.Empty);
		}
		for (int i = _1DF5.TopItemIndex; i < _1DF5.SubItems.Count; i++)
		{
			BaseItem baseItem = _1DF5.SubItems[i];
			if (baseItem.Displayed && baseItem.Visible)
			{
				baseItem.Paint(itemPaintArgs);
			}
		}
	}

	private void _1DF7(MouseButtons _1DF8, Point _1DF9)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (_1DF6 != null)
		{
			_1DF6.InternalClick(_1DF8, _1DF9);
		}
	}

	private void _1DFA(MouseButtons _1DF8, Point _1DF9)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (_1DF6 != null)
		{
			_1DF6.InternalDoubleClick(_1DF8, _1DF9);
		}
	}

	private void _1DFB(KeyEventArgs ᨍ)
	{
		if (_1DF5.DesignMode)
		{
			ᨍ.Handled = true;
		}
		else if (_1DF6 != null)
		{
			_1DF6.InternalKeyDown(ᨍ);
		}
	}

	private void _1DFC(MouseEventArgs ᨍ)
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Invalid comparison between Unknown and I4
		IOwner owner = null;
		_0AC2 = new Point(ᨍ.X, ᨍ.Y);
		BaseItem baseItem = _1DF5.ExpandedItem();
		if (baseItem != null && baseItem != _1DF6 && !_1DF5.DesignMode)
		{
			baseItem.Expanded = false;
			_1DF5.AutoExpand = false;
		}
		if (_1DF5.DesignMode && _1DF5.CanCustomize)
		{
			if (_1DF5.IsContainer && _1DF5.SubItems.Count > 0)
			{
				BaseItem baseItem2 = _1DFD(ᨍ.X, ᨍ.Y);
				if (baseItem2 != null && baseItem2.CanCustomize && _1DF5.GetOwner() is IOwner owner2)
				{
					owner2.SetFocusItem(baseItem2);
				}
				baseItem2?.InternalMouseDown(ᨍ);
			}
			if ((int)ᨍ.Button == 2097152 && !_1DF5.IsContainer && _1DF5.GetOwner() is IOwner owner3)
			{
				owner3.DesignTimeContextMenu(_1DF5);
			}
		}
		if (!_1DF5.DesignMode && _1DF5.GetOwner() is IOwner owner4 && owner4.GetFocusItem() != null)
		{
			BaseItem baseItem3 = _1DFD(ᨍ.X, ᨍ.Y);
			if (baseItem3 != owner4.GetFocusItem())
			{
				owner4.GetFocusItem().ReleaseFocus();
			}
		}
		if (_1DF6 != null)
		{
			_1DF6.InternalMouseDown(ᨍ);
		}
	}

	private void ఆ()
	{
		if (_1DF5.DesignMode || _1DF6 == null)
		{
			return;
		}
		if (!_1DF5.AutoExpand)
		{
			BaseItem baseItem = _1DF5.ExpandedItem();
			if (baseItem != null && _1DF6 != baseItem && (_1DF5.IsOnMenu || _1DF5.ContainerControl is Bar))
			{
				baseItem.Expanded = false;
			}
		}
		if (_1DF6 != null)
		{
			_1DF6.InternalMouseHover();
		}
	}

	public virtual void InternalMouseLeave()
	{
		if (!_1DF5.DesignMode && _1DF6 != null)
		{
			_1DF6.InternalMouseLeave();
			_1DF6 = null;
		}
	}

	private void _0BFD(MouseEventArgs ᨍ)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		if (_1DF5.DesignMode && (int)ᨍ.Button == 1048576 && (Math.Abs(ᨍ.X - _0AC2.X) >= 2 || Math.Abs(ᨍ.Y - _0AC2.Y) >= 2))
		{
			BaseItem baseItem = _1DF5.FocusedItem();
			if (baseItem != null && baseItem.CanCustomize)
			{
				if (!baseItem.SystemItem && _1DF5.GetOwner() is IOwner owner)
				{
					owner.StartItemDrag(baseItem);
				}
			}
			else if (!_1DF5.IsContainer && !_1DF5.SystemItem && _1DF5.CanCustomize && _1DF5.GetOwner() is IOwner owner2)
			{
				owner2.StartItemDrag(_1DF5);
			}
		}
		else
		{
			if (!_1DF5.IsContainer || _1DF5.DesignMode)
			{
				return;
			}
			BaseItem baseItem2 = _1DFD(ᨍ.X, ᨍ.Y);
			if (baseItem2 != _1DF6)
			{
				if (_1DF6 != null)
				{
					_1DF6.InternalMouseLeave();
					if (baseItem2 != null && _1DF6.Expanded && (_1DF5.IsOnMenu || _1DF5.ContainerControl is Bar))
					{
						_1DF6.Expanded = false;
					}
				}
				if (baseItem2 != null)
				{
					if (_1DF5.AutoExpand)
					{
						BaseItem baseItem3 = _1DF5.ExpandedItem();
						if (baseItem3 != null && baseItem3 != baseItem2)
						{
							baseItem3.Expanded = false;
						}
					}
					baseItem2.InternalMouseEnter();
					baseItem2.InternalMouseMove(ᨍ);
					if (_1DF5.AutoExpand && baseItem2.Enabled && baseItem2.ShowSubItems)
					{
						if (baseItem2 is PopupItem)
						{
							PopupItem popupItem = baseItem2 as PopupItem;
							ePopupAnimation popupAnimation = popupItem.PopupAnimation;
							popupItem.PopupAnimation = ePopupAnimation.None;
							if (baseItem2.SubItems.Count > 0)
							{
								baseItem2.Expanded = true;
							}
							popupItem.PopupAnimation = popupAnimation;
						}
						else
						{
							baseItem2.Expanded = true;
						}
					}
					_1DF6 = baseItem2;
					ఘ();
				}
				else
				{
					_1DF6 = null;
				}
			}
			else if (_1DF6 != null)
			{
				_1DF6.InternalMouseMove(ᨍ);
			}
		}
	}

	private BaseItem _1DFD(int و, int ى)
	{
		foreach (BaseItem subItem in _1DF5.SubItems)
		{
			if ((subItem.Visible || _1DF5.IsOnCustomizeMenu) && subItem.Displayed && subItem.DisplayRectangle.Contains(و, ى))
			{
				return subItem;
			}
		}
		return null;
	}

	private void ఘ()
	{
		_089F.TRACKMOUSEEVENT tme = default(_089F.TRACKMOUSEEVENT);
		tme.dwFlags = 1073741824u;
		tme.hwndTrack = ((Control)this).Handle;
		tme.cbSize = Marshal.SizeOf((object)tme);
		_089F.TrackMouseEvent(ref tme);
		tme.dwFlags |= 1u;
		_089F.TrackMouseEvent(ref tme);
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		_1DF7(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		_1DFA(Control.MouseButtons, Control.MousePosition);
		((Control)this).OnDoubleClick(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		_1DFB(e);
		((Control)this).OnKeyDown(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		_1DFC(e);
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		ఆ();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (((Control)this).Cursor != Cursors.Arrow)
		{
			((Control)this).Cursor = Cursors.Arrow;
		}
		InternalMouseLeave();
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		_0BFD(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (_1DF6 != null)
		{
			_1DF6.InternalMouseUp(e);
		}
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_0A78();
		((Control)this).OnHandleDestroyed(e);
	}

	private void _0A78()
	{
		if (ৰ != null)
		{
			ৰ.Dispose();
			ৰ = null;
		}
		if (ৱ != null)
		{
			ৱ.Dispose();
			ৱ = null;
		}
		if (_09F2 != null)
		{
			_09F2.Dispose();
			_09F2 = null;
		}
		if (_09F3 != null)
		{
			_09F3.Dispose();
			_09F3 = null;
		}
		if (_09F4 != null)
		{
			_09F4.Dispose();
			_09F4 = null;
		}
		if (_09F6 != null)
		{
			_09F6.Dispose();
			_09F6 = null;
		}
		if (_09F5 != null)
		{
			_09F5.Dispose();
			_09F5 = null;
		}
		if (_09F7 != null)
		{
			_09F7.Dispose();
			_09F7 = null;
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 794)
		{
			చ();
		}
		((Control)this).WndProc(ref m);
	}

	private void చ()
	{
		if (ৰ != null)
		{
			ৰ.Dispose();
			ৰ = new _0A79((Control)(object)this);
		}
		if (ৱ != null)
		{
			ৱ.Dispose();
			ৱ = new _0A7A((Control)(object)this);
		}
		if (_09F2 != null)
		{
			_09F2.Dispose();
			_09F2 = new _0A7B((Control)(object)this);
		}
		if (_09F3 != null)
		{
			_09F3.Dispose();
			_09F3 = new _0A7C((Control)(object)this);
		}
		if (_09F4 != null)
		{
			_09F4.Dispose();
			_09F4 = new _0A7D((Control)(object)this);
		}
		if (_09F6 != null)
		{
			_09F6.Dispose();
			_09F6 = new _0A7F((Control)(object)this);
		}
		if (_09F5 != null)
		{
			_09F5.Dispose();
			_09F5 = new _0A7E((Control)(object)this);
		}
	}
}
