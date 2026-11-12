using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabControlBox : BaseItem
{
	private SuperTabStripItem _TabStripItem;

	private SuperTabCloseBox _CloseBox;

	private SuperTabMenuBox _MenuBox;

	private BaseItem _HotItem;

	private BaseItem _MouseDownItem;

	private bool _IsMouseOver;

	private bool _IsMouseDown;

	[Browsable(false)]
	public override bool BeginGroup
	{
		get
		{
			return base.BeginGroup;
		}
		set
		{
			base.BeginGroup = value;
		}
	}

	[Browsable(false)]
	public override bool AutoCollapseOnClick
	{
		get
		{
			return base.AutoCollapseOnClick;
		}
		set
		{
			base.AutoCollapseOnClick = value;
		}
	}

	[Browsable(false)]
	public override bool CanCustomize
	{
		get
		{
			return base.CanCustomize;
		}
		set
		{
			base.CanCustomize = value;
		}
	}

	[Browsable(false)]
	public override string Category
	{
		get
		{
			return base.Category;
		}
		set
		{
			base.Category = value;
		}
	}

	[Browsable(false)]
	public override bool ClickAutoRepeat
	{
		get
		{
			return base.ClickAutoRepeat;
		}
		set
		{
			base.ClickAutoRepeat = value;
		}
	}

	[Browsable(false)]
	public override int ClickRepeatInterval
	{
		get
		{
			return base.ClickRepeatInterval;
		}
		set
		{
			base.ClickRepeatInterval = value;
		}
	}

	[Browsable(false)]
	public override Cursor Cursor
	{
		get
		{
			return base.Cursor;
		}
		set
		{
			base.Cursor = value;
		}
	}

	[Browsable(false)]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
		}
	}

	[Browsable(false)]
	public override bool Stretch
	{
		get
		{
			return base.Stretch;
		}
		set
		{
			base.Stretch = value;
		}
	}

	[Browsable(false)]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	[Browsable(false)]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			base.ThemeAware = value;
		}
	}

	[Browsable(false)]
	public override string Tooltip
	{
		get
		{
			return base.Tooltip;
		}
		set
		{
			base.Tooltip = value;
		}
	}

	[Browsable(false)]
	public override eItemAlignment ItemAlignment
	{
		get
		{
			return base.ItemAlignment;
		}
		set
		{
			base.ItemAlignment = value;
		}
	}

	[Browsable(false)]
	public override string KeyTips
	{
		get
		{
			return base.KeyTips;
		}
		set
		{
			base.KeyTips = value;
		}
	}

	[Browsable(false)]
	public override ShortcutsCollection Shortcuts
	{
		get
		{
			return base.Shortcuts;
		}
		set
		{
			base.Shortcuts = value;
		}
	}

	[Browsable(false)]
	public override string Description
	{
		get
		{
			return base.Description;
		}
		set
		{
			base.Description = value;
		}
	}

	[Browsable(false)]
	public override bool GlobalItem
	{
		get
		{
			return base.GlobalItem;
		}
		set
		{
			base.GlobalItem = value;
		}
	}

	[Browsable(false)]
	public override bool ShowSubItems
	{
		get
		{
			return base.ShowSubItems;
		}
		set
		{
			base.ShowSubItems = value;
		}
	}

	[Browsable(false)]
	public override string GlobalName
	{
		get
		{
			return base.GlobalName;
		}
		set
		{
			base.GlobalName = value;
		}
	}

	[Browsable(false)]
	public override Command Command
	{
		get
		{
			return base.Command;
		}
		set
		{
			base.Command = value;
		}
	}

	[Browsable(false)]
	public override object CommandParameter
	{
		get
		{
			return base.CommandParameter;
		}
		set
		{
			base.CommandParameter = value;
		}
	}

	[Browsable(false)]
	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			base.Bounds = value;
			RecalcLayout();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SuperTabCloseBox CloseBox => _CloseBox;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SuperTabMenuBox MenuBox => _MenuBox;

	public override bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			if (base.Visible != value)
			{
				base.Visible = value;
				MyRefresh();
			}
		}
	}

	internal bool IsMouseDown => _IsMouseDown;

	internal bool IsMouseOver => _IsMouseOver;

	internal SuperTabStripBaseDisplay TabDisplay => _TabStripItem.TabDisplay;

	public SuperTabControlBox(SuperTabStripItem tabStripItem)
	{
		_TabStripItem = tabStripItem;
		MouseDownCapture = true;
		MouseUpNotification = true;
		Style = eDotNetBarStyle.Office2007;
		_MenuBox = new SuperTabMenuBox(this);
		_CloseBox = new SuperTabCloseBox(this);
		_MenuBox.Visible = true;
		_CloseBox.Visible = false;
		SetParent(tabStripItem);
		SetIsContainer(b: true);
		SubItems.Add(_MenuBox);
		SubItems.Add(_CloseBox);
	}

	public override void RecalcSize()
	{
		Size size = Size.Empty;
		foreach (BaseItem subItem in SubItems)
		{
			size = LayoutItem(subItem, size, recalc: true);
		}
		WidthInternal = size.Width;
		HeightInternal = size.Height;
	}

	private void RecalcLayout()
	{
		Size size = Size.Empty;
		foreach (BaseItem subItem in SubItems)
		{
			size = LayoutItem(subItem, size, recalc: false);
		}
		Refresh();
	}

	private Size LayoutItem(BaseItem item, Size size, bool recalc)
	{
		if (item.Visible)
		{
			if (recalc)
			{
				item.RecalcSize();
			}
			if (size.IsEmpty)
			{
				if (Orientation == eOrientation.Horizontal)
				{
					size.Width += 5;
				}
				else
				{
					size.Height += 5;
				}
			}
			Rectangle bounds = new Rectangle(Bounds.X, Bounds.Y, item.WidthInternal, item.HeightInternal);
			if (_TabStripItem.IsVertical)
			{
				bounds.Y += size.Height;
				bounds.X += (Bounds.Width - bounds.Width) / 2;
			}
			else
			{
				bounds.X += size.Width;
				bounds.Y += (Bounds.Height - bounds.Height) / 2;
			}
			item.Bounds = bounds;
			if (_TabStripItem.IsVertical)
			{
				size.Height += item.HeightInternal + 2;
				if (item.WidthInternal > size.Width)
				{
					size.Width = item.WidthInternal;
				}
			}
			else
			{
				size.Width += item.WidthInternal + 2;
				if (item.HeightInternal > size.Height)
				{
					size.Height = item.HeightInternal;
				}
			}
			if (!DesignMode)
			{
				item.Displayed = true;
			}
		}
		return size;
	}

	private void MyRefresh()
	{
		if (TabDisplay != null)
		{
			SuperTabStripItem tabStripItem = TabDisplay.TabStripItem;
			if (tabStripItem != null)
			{
				tabStripItem.NeedRecalcSize = true;
				tabStripItem.Refresh();
			}
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible)
			{
				subItem.Paint(p);
			}
		}
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		_IsMouseOver = true;
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		_IsMouseOver = false;
		if (_HotItem != null)
		{
			_HotItem.InternalMouseLeave();
			_HotItem = null;
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		base.InternalMouseMove(objArg);
		_HotItem = GetBoxItemFromPoint(objArg.Location);
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		base.InternalMouseDown(objArg);
		_IsMouseDown = true;
		_MouseDownItem = _HotItem;
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		_IsMouseDown = false;
		if (_MouseDownItem != null)
		{
			if (_MouseDownItem != _HotItem)
			{
				_MouseDownItem.InternalMouseUp(objArg);
			}
			_MouseDownItem = null;
		}
	}

	private BaseItem GetBoxItemFromPoint(Point pt)
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible && subItem.Bounds.Contains(pt))
			{
				return subItem;
			}
		}
		return null;
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		base.OnItemAdded(item);
		MyRefresh();
	}

	protected internal override void OnAfterItemRemoved(BaseItem item)
	{
		base.OnAfterItemRemoved(item);
		MyRefresh();
	}

	public void RemoveUserItems()
	{
		for (int num = SubItems.Count - 1; num >= 0; num--)
		{
			if (SubItems[num] != _MenuBox && SubItems[num] != _CloseBox)
			{
				SubItems.Remove(num);
			}
		}
		MyRefresh();
	}

	public override BaseItem Copy()
	{
		SuperTabControlBox superTabControlBox = new SuperTabControlBox(_TabStripItem);
		CopyToItem(superTabControlBox);
		return superTabControlBox;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		SuperTabControlBox objCopy = copy as SuperTabControlBox;
		base.CopyToItem(objCopy);
	}
}
