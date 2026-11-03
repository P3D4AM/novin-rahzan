using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
public class SimpleItemContainer : ImageItem, IDesignTimeProvider
{
	private int _ᐿ;

	private Size _ᑀ = Size.Empty;

	private bool _ᑁ;

	private int _ᑂ = 6;

	private eOrientation _ᑃ;

	private bool _ᑄ;

	private int _ᑅ;

	private int _ᑆ;

	private int _ᑇ;

	private Type _ᑈ;

	[Browsable(false)]
	public Size MinimumSize
	{
		get
		{
			return _ᑀ;
		}
		set
		{
			_ᑀ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	public bool SystemContainer => _ᑁ;

	public int ItemSpacing
	{
		get
		{
			return _ᐿ;
		}
		set
		{
			if (value != _ᐿ)
			{
				int ᐿ = _ᐿ;
				_ᐿ = value;
				OnItemSpacingChanged(ᐿ, value);
			}
		}
	}

	[DefaultValue(6)]
	[Category("Appearance")]
	[Description("Indicates additional spacing between item when its BeginGroup property is set.")]
	public int BeginGroupSpacing
	{
		get
		{
			return _ᑂ;
		}
		set
		{
			_ᑂ = value;
		}
	}

	[Description("Specifies the layout orientation")]
	[DefaultValue(eOrientation.Horizontal)]
	[Category("Layout")]
	public eOrientation LayoutOrientation
	{
		get
		{
			return _ᑃ;
		}
		set
		{
			if (_ᑃ != value)
			{
				_ᑃ = value;
				NeedRecalcSize = true;
			}
		}
	}

	[Category("Layout")]
	[DefaultValue(false)]
	[Description("Indicates whether all items are resized to be of the size equal to largest item in the container")]
	public bool EqualizeButtonSize
	{
		get
		{
			return _ᑄ;
		}
		set
		{
			if (_ᑄ != value)
			{
				_ᑄ = value;
				NeedRecalcSize = true;
			}
		}
	}

	public int PaddingLeft
	{
		get
		{
			return _ᑅ;
		}
		set
		{
			_ᑅ = value;
		}
	}

	public int PaddingRight
	{
		get
		{
			return _ᑆ;
		}
		set
		{
			_ᑆ = value;
		}
	}

	public int OverlapSpacing
	{
		get
		{
			return _ᑇ;
		}
		set
		{
			_ᑇ = value;
		}
	}

	public Type OverlapType
	{
		get
		{
			return _ᑈ;
		}
		set
		{
			_ᑈ = value;
		}
	}

	public SimpleItemContainer()
	{
		m_IsContainer = true;
		AccessibleRole = (AccessibleRole)20;
	}

	public override void RecalcSize()
	{
		if (_ᑃ == eOrientation.Vertical)
		{
			ᑕ();
		}
		else
		{
			List<BaseItem> list = new List<BaseItem>();
			List<BaseItem> list2 = new List<BaseItem>();
			List<BaseItem> list3 = new List<BaseItem>();
			int ᑍ = 0;
			Size size = ᑉ(list, list2, list3, out ᑍ);
			Point point = Bounds.Location;
			_ = Bounds.Bottom;
			point.X += _ᑅ;
			if (WidthInternal == 0 || WidthInternal <= size.Width)
			{
				BaseItem[] array = new BaseItem[list.Count + list2.Count + list3.Count];
				list.CopyTo(array);
				list2.CopyTo(array, list.Count);
				list3.CopyTo(array, list.Count + list2.Count);
				BaseItem ᑏ = null;
				BaseItem[] array2 = array;
				foreach (BaseItem baseItem in array2)
				{
					if (baseItem.Visible)
					{
						baseItem.Displayed = true;
						point.X -= ᑎ(ᑏ, baseItem);
						baseItem.LeftInternal = point.X;
						if (IsVerticallyCentered(baseItem))
						{
							baseItem.TopInternal = point.Y + (size.Height - baseItem.HeightInternal) / 2;
						}
						else
						{
							baseItem.TopInternal = point.Y + (size.Height - baseItem.HeightInternal);
						}
						point.X += baseItem.WidthInternal + _ᐿ;
						ᑏ = baseItem;
					}
				}
				m_Rect.Size = size;
			}
			else
			{
				int num = 0;
				BaseItem ᑏ2 = null;
				foreach (BaseItem item in list)
				{
					item.Displayed = true;
					if (item.BeginGroup)
					{
						point.X += _ᑂ;
					}
					point.X -= ᑎ(ᑏ2, item);
					item.LeftInternal = point.X;
					if (IsVerticallyCentered(item))
					{
						item.TopInternal = point.Y + (size.Height - item.HeightInternal) / 2;
					}
					else
					{
						item.TopInternal = point.Y + (size.Height - item.HeightInternal);
					}
					point.X += item.WidthInternal + _ᐿ;
					ᑏ2 = item;
				}
				num = point.X;
				int num2 = 0;
				point = new Point(Bounds.Right - _ᑆ, Bounds.Y);
				ᑏ2 = null;
				for (int num3 = list3.Count - 1; num3 >= 0; num3--)
				{
					BaseItem baseItem2 = list3[num3];
					point.X -= baseItem2.WidthInternal;
					baseItem2.Displayed = true;
					point.X += ᑎ(ᑏ2, baseItem2);
					baseItem2.LeftInternal = point.X;
					point.X -= _ᐿ + (baseItem2.BeginGroup ? _ᑂ : 0);
					if (IsVerticallyCentered(baseItem2))
					{
						baseItem2.TopInternal = point.Y + (size.Height - baseItem2.HeightInternal) / 2;
					}
					else
					{
						baseItem2.TopInternal = point.Y + (size.Height - baseItem2.HeightInternal);
					}
					ᑏ2 = baseItem2;
				}
				num2 = point.X;
				point = new Point((WidthInternal - ᑍ) / 2, Bounds.Y);
				if (point.X < num || point.X + ᑍ > num2)
				{
					point.X = num + (num2 - num - ᑍ) / 2;
				}
				ᑏ2 = null;
				foreach (BaseItem item2 in list2)
				{
					item2.Displayed = true;
					if (item2.BeginGroup)
					{
						point.X += _ᑂ;
					}
					point.X -= ᑎ(ᑏ2, item2);
					item2.LeftInternal = point.X;
					if (IsVerticallyCentered(item2))
					{
						item2.TopInternal = point.Y + (size.Height - item2.HeightInternal) / 2;
					}
					else
					{
						item2.TopInternal = point.Y + (size.Height - item2.HeightInternal);
					}
					point.X += item2.WidthInternal + _ᐿ;
					ᑏ2 = item2;
				}
				m_Rect.Height = size.Height;
			}
			if (m_Rect.Width < _ᑀ.Width)
			{
				m_Rect.Width = _ᑀ.Width;
			}
			if (m_Rect.Height < _ᑀ.Height)
			{
				m_Rect.Height = _ᑀ.Height;
			}
		}
		base.RecalcSize();
	}

	protected virtual bool IsVerticallyCentered(BaseItem item)
	{
		return !(item is MetroTabItem);
	}

	private Size ᑉ(List<BaseItem> ᑊ, List<BaseItem> ᑋ, List<BaseItem> ᑌ, out int ᑍ)
	{
		Size result = default(Size);
		ᑍ = 0;
		BaseItem ᑏ = null;
		foreach (BaseItem subItem in SubItems)
		{
			if (!subItem.Visible)
			{
				subItem.Displayed = false;
				continue;
			}
			subItem.RecalcSize();
			result.Width += subItem.WidthInternal + _ᐿ + (subItem.BeginGroup ? _ᑂ : 0) - ᑎ(ᑏ, subItem);
			result.Height = Math.Max(subItem.HeightInternal, result.Height);
			if (subItem.ItemAlignment == eItemAlignment.Near)
			{
				ᑊ.Add(subItem);
			}
			else if (subItem.ItemAlignment == eItemAlignment.Center)
			{
				ᑋ.Add(subItem);
				ᑍ += subItem.WidthInternal + _ᐿ;
			}
			else if (subItem.ItemAlignment == eItemAlignment.Far)
			{
				ᑌ.Add(subItem);
			}
			ᑏ = subItem;
		}
		if (result.Width > 0)
		{
			result.Width -= _ᐿ;
		}
		if (ᑍ > 0)
		{
			ᑍ -= _ᐿ;
		}
		return result;
	}

	private int ᑎ(BaseItem ᑏ, BaseItem _06EC)
	{
		if (ᑏ == null || _ᑇ <= 0 || (object)_ᑈ == null || !_ᑈ.IsInstanceOfType(ᑏ) || !_ᑈ.IsInstanceOfType(_06EC))
		{
			return 0;
		}
		return _ᑇ;
	}

	private bool ᑐ(BaseItem _06EC)
	{
		return !(_06EC is MetroTabItem);
	}

	private Size ᑑ(List<BaseItem> ᑒ, List<BaseItem> ᑋ, List<BaseItem> ᑓ, out int ᑔ)
	{
		Size result = default(Size);
		ᑔ = 0;
		BaseItem ᑏ = null;
		foreach (BaseItem subItem in SubItems)
		{
			if (!subItem.Visible)
			{
				subItem.Displayed = false;
				continue;
			}
			subItem.RecalcSize();
			result.Height += subItem.HeightInternal + _ᐿ + (subItem.BeginGroup ? _ᑂ : 0) - ᑎ(ᑏ, subItem);
			result.Width = Math.Max(subItem.WidthInternal, result.Width);
			if (subItem.ItemAlignment == eItemAlignment.Near)
			{
				ᑒ.Add(subItem);
			}
			else if (subItem.ItemAlignment == eItemAlignment.Center)
			{
				ᑋ.Add(subItem);
				ᑔ += subItem.HeightInternal + _ᐿ;
			}
			else if (subItem.ItemAlignment == eItemAlignment.Far)
			{
				ᑓ.Add(subItem);
			}
			ᑏ = subItem;
		}
		if (result.Height > 0)
		{
			result.Height -= _ᐿ;
		}
		if (ᑔ > 0)
		{
			ᑔ -= _ᐿ;
		}
		return result;
	}

	private void ᑕ()
	{
		List<BaseItem> list = new List<BaseItem>();
		List<BaseItem> list2 = new List<BaseItem>();
		List<BaseItem> list3 = new List<BaseItem>();
		int ᑔ = 0;
		Size size = ᑑ(list, list2, list3, out ᑔ);
		Point point = Bounds.Location;
		_ = Bounds.Bottom;
		if (HeightInternal == 0 || HeightInternal <= size.Height)
		{
			BaseItem[] array = new BaseItem[list.Count + list2.Count + list3.Count];
			list.CopyTo(array);
			list2.CopyTo(array, list.Count);
			list3.CopyTo(array, list.Count + list2.Count);
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				if (baseItem.Visible)
				{
					baseItem.Displayed = true;
					baseItem.TopInternal = point.Y;
					baseItem.LeftInternal = point.X;
					baseItem.WidthInternal = size.Width;
					point.Y += baseItem.HeightInternal + _ᐿ;
				}
			}
			m_Rect.Size = size;
		}
		else
		{
			int num = 0;
			foreach (BaseItem item in list)
			{
				item.Displayed = true;
				if (item.BeginGroup)
				{
					point.Y += _ᑂ;
				}
				item.TopInternal = point.Y;
				item.WidthInternal = size.Width;
				if (ᑐ(item))
				{
					item.LeftInternal = point.X + (size.Width - item.WidthInternal) / 2;
				}
				else
				{
					item.LeftInternal = point.X + (size.Width - item.WidthInternal);
				}
				point.Y += item.HeightInternal + _ᐿ;
			}
			num = point.Y;
			int num2 = 0;
			point = new Point(Bounds.X, Bounds.Bottom);
			for (int num3 = list3.Count - 1; num3 >= 0; num3--)
			{
				BaseItem baseItem2 = list3[num3];
				point.Y -= baseItem2.HeightInternal;
				baseItem2.Displayed = true;
				baseItem2.TopInternal = point.Y;
				baseItem2.WidthInternal = size.Width;
				point.Y -= _ᐿ + (baseItem2.BeginGroup ? _ᑂ : 0);
				if (ᑐ(baseItem2))
				{
					baseItem2.LeftInternal = point.X + (size.Width - baseItem2.WidthInternal) / 2;
				}
				else
				{
					baseItem2.LeftInternal = point.X + (size.Width - baseItem2.WidthInternal);
				}
			}
			num2 = point.Y;
			point = new Point(Bounds.X, (HeightInternal - ᑔ) / 2);
			if (point.Y < num || point.Y + ᑔ > num2)
			{
				point.Y = num + (num2 - num - ᑔ) / 2;
			}
			foreach (BaseItem item2 in list2)
			{
				item2.Displayed = true;
				if (item2.BeginGroup)
				{
					point.Y += _ᑂ;
				}
				item2.TopInternal = point.Y;
				item2.WidthInternal = size.Width;
				if (ᑐ(item2))
				{
					item2.LeftInternal = point.X + (size.Width - item2.WidthInternal) / 2;
				}
				else
				{
					item2.LeftInternal = point.X + (size.Width - item2.WidthInternal);
				}
				point.Y += item2.HeightInternal + _ᐿ;
			}
			m_Rect.Width = size.Width;
		}
		if (m_Rect.Width < _ᑀ.Width)
		{
			m_Rect.Width = _ᑀ.Width;
		}
		if (m_Rect.Height < _ᑀ.Height)
		{
			m_Rect.Height = _ᑀ.Height;
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Displayed)
			{
				if (subItem.BeginGroup)
				{
					DisplayHelp.DrawLine(p.Graphics, subItem.LeftInternal - 3, TopInternal + 4, subItem.LeftInternal - 3, Bounds.Bottom - 8, p.Colors.ItemSeparator, 1);
				}
				subItem.Paint(p);
			}
		}
	}

	public override BaseItem Copy()
	{
		SimpleItemContainer simpleItemContainer = new SimpleItemContainer();
		CopyToItem(simpleItemContainer);
		return simpleItemContainer;
	}

	internal void _0EBA(SimpleItemContainer _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		SimpleItemContainer objCopy = copy as SimpleItemContainer;
		base.CopyToItem(objCopy);
	}

	internal void ௐ()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		if (m_HotSubItem != null || SubItems.Count == 0)
		{
			return;
		}
		BaseItem baseItem = ExpandedItem();
		if (baseItem != null)
		{
			m_HotSubItem = baseItem;
			m_HotSubItem.InternalMouseEnter();
			m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
			return;
		}
		foreach (BaseItem subItem in SubItems)
		{
			if (!subItem.SystemItem && subItem.Displayed && subItem.Visible)
			{
				m_HotSubItem = subItem;
				m_HotSubItem.InternalMouseEnter();
				m_HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, m_HotSubItem.LeftInternal + 2, m_HotSubItem.TopInternal + 2, 0));
				break;
			}
		}
	}

	internal void ඹ()
	{
		BaseItem.CollapseSubItems(this);
		if (m_HotSubItem == null)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			Point pt = val.PointToClient(Control.MousePosition);
			if (m_HotSubItem.DisplayRectangle.Contains(pt))
			{
				return;
			}
		}
		m_HotSubItem.InternalMouseLeave();
		m_HotSubItem = null;
	}

	internal void ᆁ(bool _0BCF)
	{
		_ᑁ = _0BCF;
	}

	protected virtual void OnItemSpacingChanged(int oldValue, int newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
	{
		return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, DragItem);
	}

	public void DrawReversibleMarker(int iPos, bool Before)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, iPos, Before);
	}

	public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
		DesignTimeProviderContainer.InsertItemAt(this, objItem, iPos, Before);
	}
}
