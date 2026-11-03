using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class SuperTabControlBox : BaseItem
{
	private SuperTabStripItem _ὗ;

	private SuperTabCloseBox __206E;

	private SuperTabMenuBox __206F;

	private BaseItem __2070;

	private BaseItem _ᡦ;

	private bool _ᆿ;

	private bool __171A;

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
			if (!base.Bounds.Equals((object?)value))
			{
				base.Bounds = value;
				ⁱ();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SuperTabCloseBox CloseBox => __206E;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SuperTabMenuBox MenuBox => __206F;

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
				ᾦ();
			}
		}
	}

	internal bool _206D => __171A;

	internal bool ᆼ => _ᆿ;

	internal ῲ έ => _ὗ.έ;

	public SuperTabControlBox(SuperTabStripItem tabStripItem)
	{
		_ὗ = tabStripItem;
		MouseDownCapture = true;
		MouseUpNotification = true;
		Style = eDotNetBarStyle.Office2007;
		__206F = new SuperTabMenuBox(this);
		__206E = new SuperTabCloseBox(this);
		__206F.Visible = true;
		__206E.Visible = false;
		SetParent(tabStripItem);
		SetIsContainer(b: true);
		SubItems.Add(__206F);
		SubItems.Add(__206E);
	}

	public override void RecalcSize()
	{
		Size ߒ = Size.Empty;
		foreach (BaseItem subItem in SubItems)
		{
			ߒ = _2072(subItem, ߒ, _2073: true);
		}
		WidthInternal = ߒ.Width;
		HeightInternal = ߒ.Height;
	}

	private void ⁱ()
	{
		Size ߒ = Size.Empty;
		if (_ὗ.IsRightToLeft)
		{
			int num = SubItems.IndexOf(__206E);
			int num2 = SubItems.IndexOf(__206F);
			if (num2 >= 0 && num > num2)
			{
				SubItems.Remove(__206E);
				SubItems.Insert(num2, __206E);
			}
		}
		foreach (BaseItem subItem in SubItems)
		{
			ߒ = _2072(subItem, ߒ, _2073: false);
		}
		Refresh();
	}

	private Size _2072(BaseItem _06EC, Size ߒ, bool _2073)
	{
		if (_06EC.Visible)
		{
			if (_2073)
			{
				_06EC.RecalcSize();
			}
			if (ߒ.IsEmpty)
			{
				if (Orientation == eOrientation.Horizontal)
				{
					ߒ.Width += Dpi.Width5;
				}
				else
				{
					ߒ.Height += Dpi.Height5;
				}
			}
			Rectangle bounds = new Rectangle(Bounds.X, Bounds.Y, _06EC.WidthInternal, _06EC.HeightInternal);
			if (_ὗ.ᓙ)
			{
				bounds.Y += ߒ.Height;
				bounds.X += (Bounds.Width - bounds.Width) / 2;
			}
			else
			{
				bounds.X += ߒ.Width;
				bounds.Y += (Bounds.Height - bounds.Height) / 2;
			}
			_06EC.Bounds = bounds;
			if (_ὗ.ᓙ)
			{
				ߒ.Height += _06EC.HeightInternal + Dpi.Height2;
				if (_06EC.WidthInternal > ߒ.Width)
				{
					ߒ.Width = _06EC.WidthInternal;
				}
			}
			else
			{
				ߒ.Width += _06EC.WidthInternal + Dpi.Width2;
				if (_06EC.HeightInternal > ߒ.Height)
				{
					ߒ.Height = _06EC.HeightInternal;
				}
			}
			if (!DesignMode)
			{
				_06EC.Displayed = true;
			}
		}
		return ߒ;
	}

	private void ᾦ()
	{
		if (έ != null)
		{
			SuperTabStripItem superTabStripItem = έ._1FD4;
			if (superTabStripItem != null)
			{
				superTabStripItem.NeedRecalcSize = true;
				superTabStripItem.Refresh();
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
		_ᆿ = true;
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		_ᆿ = false;
		if (__2070 != null)
		{
			__2070.InternalMouseLeave();
			__2070 = null;
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		base.InternalMouseMove(objArg);
		__2070 = _2074(objArg.Location);
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		base.InternalMouseDown(objArg);
		__171A = true;
		_ᡦ = __2070;
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		__171A = false;
		if (_ᡦ != null)
		{
			if (_ᡦ != __2070)
			{
				_ᡦ.InternalMouseUp(objArg);
			}
			_ᡦ = null;
		}
	}

	private BaseItem _2074(Point ᓏ)
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible && subItem.Bounds.Contains(ᓏ))
			{
				return subItem;
			}
		}
		return null;
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		base.OnItemAdded(item);
		ᾦ();
	}

	protected internal override void OnAfterItemRemoved(BaseItem item, int itemIndex)
	{
		base.OnAfterItemRemoved(item, itemIndex);
		ᾦ();
	}

	public void RemoveUserItems()
	{
		for (int num = SubItems.Count - 1; num >= 0; num--)
		{
			if (SubItems[num] != __206F && SubItems[num] != __206E)
			{
				SubItems._RemoveAt(num);
			}
		}
		ᾦ();
	}

	public override BaseItem Copy()
	{
		SuperTabControlBox superTabControlBox = new SuperTabControlBox(_ὗ);
		CopyToItem(superTabControlBox);
		return superTabControlBox;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		SuperTabControlBox objCopy = copy as SuperTabControlBox;
		base.CopyToItem(objCopy);
	}
}
