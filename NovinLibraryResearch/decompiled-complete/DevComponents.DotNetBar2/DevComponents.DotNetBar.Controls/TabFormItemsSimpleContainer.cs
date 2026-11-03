using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class TabFormItemsSimpleContainer : SimpleItemContainer
{
	private NewTabFormItem __1CC9;

	private Rectangle __1CD1;

	private int _Ⴕ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DefaultValue(false)]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			base.Expanded = value;
			if (value)
			{
				return;
			}
			foreach (BaseItem subItem in SubItems)
			{
				subItem.Expanded = false;
			}
		}
	}

	public Rectangle ClippingRectangle
	{
		get
		{
			return __1CD1;
		}
		set
		{
			__1CD1 = value;
		}
	}

	[Obfuscation(Exclude = true)]
	public int ScrollPosition
	{
		get
		{
			return _Ⴕ;
		}
		set
		{
			value = Math.Min(0, value);
			if (_Ⴕ != value)
			{
				Point _066A = new Point(value - _Ⴕ, 0);
				_Ⴕ = value;
				ᆀ(_066A);
				Refresh();
			}
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		TabFormItem tabFormItem = null;
		for (int num = SubItems.Count - 1; num >= 0; num--)
		{
			BaseItem baseItem = SubItems[num];
			if (baseItem.Displayed)
			{
				if (baseItem.BeginGroup)
				{
					DisplayHelp.DrawLine(p.Graphics, baseItem.LeftInternal - 3, TopInternal + 4, baseItem.LeftInternal - 3, Bounds.Bottom - 8, p.Colors.ItemSeparator, 1);
				}
				if (baseItem is TabFormItem { Checked: not false } tabFormItem2 && tabFormItem == null)
				{
					tabFormItem = tabFormItem2;
				}
				else if (__1CD1.IsEmpty || __1CD1.IntersectsWith(baseItem.Bounds))
				{
					baseItem.Paint(p);
				}
			}
		}
		tabFormItem?.Paint(p);
	}

	public override void RecalcSize()
	{
		_Ⴕ = 0;
		base.RecalcSize();
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is TabFormItemBase { Visible: not false, Displayed: not false, TabPath: not null } tabFormItemBase && tabFormItemBase.TabPath.IsVisible(x, y))
			{
				return tabFormItemBase;
			}
		}
		return base.ItemAtLocation(x, y);
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		if (item is NewTabFormItem)
		{
			__1CC9 = (NewTabFormItem)item;
		}
		else if (__1CC9 != null)
		{
			SubItems._Remove(__1CC9);
			SubItems.__0AF3(__1CC9);
		}
		base.OnItemAdded(item);
	}

	protected internal override void OnAfterItemRemoved(BaseItem item, int itemIndex)
	{
		if (item == __1CC9)
		{
			__1CC9 = null;
		}
		base.OnAfterItemRemoved(item, itemIndex);
	}

	protected override bool IsVerticallyCentered(BaseItem item)
	{
		return !(item is TabFormItem);
	}

	private void ᆀ(Point _066A)
	{
		if (!_066A.IsEmpty)
		{
			BaseItem[] array = new BaseItem[SubItems.Count];
			SubItems.CopyTo(array, 0);
			BaseItem[] array2 = array;
			foreach (IBlock block in array2)
			{
				Rectangle bounds = block.Bounds;
				bounds.Offset(_066A);
				block.Bounds = bounds;
			}
		}
	}
}
