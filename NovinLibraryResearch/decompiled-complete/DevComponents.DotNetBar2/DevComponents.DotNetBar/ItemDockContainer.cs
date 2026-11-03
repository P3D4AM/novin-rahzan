using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[Designer("DevComponents.DotNetBar.Design.ItemContainerDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ProvideProperty("Dock", typeof(BaseItem))]
public class ItemDockContainer : ImageItem, IDesignTimeProvider, IExtenderProvider
{
	private ElementStyle _ᠩ = new ElementStyle();

	private Rectangle _ⲡ = Rectangle.Empty;

	private Size _Ⲣ = new Size(24, 24);

	private bool _ᅰ;

	private _249A __25A7;

	private Dictionary<BaseItem, eItemDock> _ⲣ = new Dictionary<BaseItem, eItemDock>();

	private bool _Ⲥ = true;

	private bool _ᑁ;

	private BaseItem Ⲡ
	{
		get
		{
			int num = SubItems.Count - 1;
			for (int num2 = num; num2 >= 0; num2--)
			{
				BaseItem baseItem = SubItems[num2];
				if (baseItem.Visible)
				{
					return baseItem;
				}
			}
			return null;
		}
	}

	[Description("Indicates whether the last child element within a ItemDockContainer stretches to fill the remaining available space.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool LastChildFill
	{
		get
		{
			return _Ⲥ;
		}
		set
		{
			_Ⲥ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	public bool SystemContainer
	{
		get
		{
			return _ᑁ;
		}
		internal set
		{
			_ᑁ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			Point _066A = new Point(value.X - m_Rect.X, value.Y - m_Rect.Y);
			base.Bounds = value;
			if (!_066A.IsEmpty)
			{
				ᆀ(_066A);
			}
		}
	}

	public ItemDockContainer()
	{
		m_IsContainer = true;
		_ᠩ.StyleChanged += ᅻ;
	}

	public override BaseItem Copy()
	{
		ItemDockContainer itemDockContainer = new ItemDockContainer();
		CopyToItem(itemDockContainer);
		return itemDockContainer;
	}

	internal void _0EBA(ButtonItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		base.CopyToItem(copy);
		ItemDockContainer itemDockContainer = (ItemDockContainer)copy;
		itemDockContainer.LastChildFill = _Ⲥ;
	}

	public override void RecalcSize()
	{
		if (SuspendLayout)
		{
			return;
		}
		if (SubItems.Count == 0 || base.VisibleSubItems == 0)
		{
			if (DesignMode && !SystemContainer)
			{
				m_Rect.Size = GetEmptyDesignTimeSize();
			}
			else
			{
				m_Rect = Rectangle.Empty;
			}
			base.RecalcSize();
		}
		else
		{
			m_Rect = ⲥ(m_Rect);
			base.RecalcSize();
		}
	}

	private Rectangle ⲥ(Rectangle ശ)
	{
		Rectangle rectangle = default(Rectangle);
		BaseItem ⲡ = Ⲡ;
		foreach (BaseItem subItem in m_SubItems)
		{
			if (!subItem.Visible)
			{
				subItem.Displayed = false;
				continue;
			}
			subItem.Displayed = true;
			subItem.RecalcSize();
			eItemDock dock = GetDock(subItem);
			if (_Ⲥ && subItem == ⲡ)
			{
				rectangle.Width += subItem.WidthInternal;
				if (subItem.HeightInternal > rectangle.Height)
				{
					rectangle.Height = subItem.HeightInternal;
				}
				continue;
			}
			switch (dock)
			{
			case eItemDock.Left:
			case eItemDock.Right:
				rectangle.Width += subItem.WidthInternal;
				if (subItem.HeightInternal > rectangle.Height)
				{
					rectangle.Height = subItem.HeightInternal;
				}
				break;
			case eItemDock.Top:
			case eItemDock.Bottom:
				rectangle.Height += subItem.HeightInternal;
				if (subItem.WidthInternal > rectangle.Width)
				{
					rectangle.Width = subItem.WidthInternal;
				}
				break;
			}
		}
		if (ശ.Width < rectangle.Width)
		{
			ശ.Width = rectangle.Width;
		}
		if (ശ.Height < rectangle.Height)
		{
			ശ.Height = rectangle.Height;
		}
		if (!Stretch && rectangle.Height < ശ.Height)
		{
			ശ.Height = rectangle.Height;
		}
		Ⲧ(ശ);
		return ശ;
	}

	private void Ⲧ(Rectangle ശ)
	{
		BaseItem ⲡ = Ⲡ;
		Rectangle rectangle = ശ;
		foreach (BaseItem subItem in m_SubItems)
		{
			if (!subItem.Visible)
			{
				continue;
			}
			eItemDock dock = GetDock(subItem);
			if (_Ⲥ && subItem == ⲡ)
			{
				subItem.LeftInternal = rectangle.Left;
				subItem.TopInternal = rectangle.Top;
				subItem.WidthInternal = rectangle.Width;
				subItem.HeightInternal = rectangle.Height;
				continue;
			}
			switch (dock)
			{
			case eItemDock.Left:
				subItem.LeftInternal = rectangle.Left;
				subItem.TopInternal = rectangle.Top;
				subItem.HeightInternal = rectangle.Height;
				if (subItem.WidthInternal <= rectangle.Width)
				{
					rectangle.Width -= subItem.WidthInternal;
					rectangle.X += subItem.WidthInternal;
				}
				else if (rectangle.Width > 0)
				{
					rectangle.X += rectangle.Width;
					rectangle.Width = 0;
				}
				break;
			case eItemDock.Right:
				subItem.HeightInternal = rectangle.Height;
				if (subItem.WidthInternal <= rectangle.Width)
				{
					subItem.LeftInternal = rectangle.Right - subItem.WidthInternal;
					subItem.TopInternal = rectangle.Top;
					rectangle.Width -= subItem.WidthInternal;
					break;
				}
				subItem.LeftInternal = rectangle.Left;
				subItem.TopInternal = rectangle.Top;
				if (rectangle.Width > 0)
				{
					rectangle.X += rectangle.Width;
					rectangle.Width = 0;
				}
				break;
			case eItemDock.Top:
				subItem.LeftInternal = rectangle.Left;
				subItem.TopInternal = rectangle.Top;
				subItem.WidthInternal = rectangle.Width;
				if (subItem.HeightInternal <= rectangle.Height)
				{
					rectangle.Height -= subItem.HeightInternal;
					rectangle.Y += subItem.HeightInternal;
				}
				else if (rectangle.Height > 0)
				{
					rectangle.Y += rectangle.Height;
					rectangle.Height = 0;
				}
				break;
			case eItemDock.Bottom:
				subItem.WidthInternal = rectangle.Width;
				if (subItem.HeightInternal <= rectangle.Height)
				{
					subItem.LeftInternal = rectangle.Left;
					subItem.TopInternal = rectangle.Bottom - subItem.HeightInternal;
					rectangle.Height -= subItem.HeightInternal;
					break;
				}
				subItem.LeftInternal = rectangle.Left;
				subItem.TopInternal = rectangle.Top;
				if (rectangle.Height > 0)
				{
					rectangle.Y += rectangle.Height;
					rectangle.Height = 0;
				}
				break;
			}
		}
	}

	protected override void OnExternalSizeChange()
	{
		if (!SuspendLayout)
		{
			Ⲧ(m_Rect);
		}
		base.OnExternalSizeChange();
	}

	protected override void OnTopLocationChanged(int oldValue)
	{
		base.OnTopLocationChanged(oldValue);
		Point _066A = new Point(0, TopInternal - oldValue);
		ᆀ(_066A);
	}

	protected override void OnLeftLocationChanged(int oldValue)
	{
		base.OnLeftLocationChanged(oldValue);
		Point _066A = new Point(LeftInternal - oldValue, 0);
		ᆀ(_066A);
	}

	internal void ᆀ(Point _066A)
	{
		if (_ᅰ || _066A.IsEmpty)
		{
			return;
		}
		try
		{
			_ᅰ = true;
			BaseItem[] array = new BaseItem[SubItems.Count];
			SubItems.CopyTo(array, 0);
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				Rectangle bounds = baseItem.Bounds;
				bounds.Offset(_066A);
				baseItem.Bounds = bounds;
			}
		}
		finally
		{
			_ᅰ = false;
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Region val = null;
		bool flag = false;
		PaintBackground(p);
		Rectangle clipRectangle = GetClipRectangle();
		val = graphics.Clip;
		graphics.SetClip(clipRectangle, (CombineMode)1);
		flag = true;
		_249A _249A2 = ᆆ();
		_249A2._139A(this, p);
		if (flag)
		{
			if (val != null)
			{
				graphics.Clip = val;
			}
			else
			{
				graphics.ResetClip();
			}
		}
		if (DesignMode && !SystemContainer && p.DesignerSelection)
		{
			Rectangle displayRectangle = DisplayRectangle;
			Pen val2 = null;
			val2 = ((!Focused) ? new Pen(Color.FromArgb(80, Color.Black), 1f) : new Pen(Color.FromArgb(190, Color.Navy), 1f));
			val2.DashStyle = (DashStyle)2;
			DisplayHelp.DrawRoundedRectangle(graphics, val2, displayRectangle, 3);
			val2.Dispose();
			val2 = null;
			Image val3 = (Image)(object)BarFunctions.ই("SystemImages.AddMoreItemsContainer.png");
			if (Parent is ItemContainer && !((ItemContainer)Parent).SystemContainer)
			{
				ItemContainer itemContainer = Parent as ItemContainer;
				while (itemContainer != null && !itemContainer.SystemContainer)
				{
					if (new Rectangle(itemContainer.DisplayRectangle.Location, val3.Size).Contains(DisplayRectangle.Location))
					{
						if (displayRectangle.X + val3.Width + 1 > DisplayRectangle.Right)
						{
							displayRectangle.X = DisplayRectangle.X;
							if (displayRectangle.Y + val3.Height >= DisplayRectangle.Bottom)
							{
								break;
							}
							displayRectangle.Y += val3.Height + 1;
						}
						displayRectangle.X += val3.Width + 1;
					}
					itemContainer = itemContainer.Parent as ItemContainer;
				}
			}
			_ⲡ = new Rectangle(displayRectangle.X + 1, displayRectangle.Y + 1, val3.Width, val3.Height);
			graphics.DrawImageUnscaled(val3, _ⲡ.Location);
		}
		DrawInsertMarker(p.Graphics);
	}

	internal _249A ᆆ()
	{
		if (__25A7 == null)
		{
			__25A7 = new _249A();
		}
		return __25A7;
	}

	protected virtual void PaintBackground(ItemPaintArgs p)
	{
		_ᠩ.SetColorScheme(p.Colors);
		Graphics graphics = p.Graphics;
		ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(_ᠩ, graphics, DisplayRectangle));
	}

	private void ᅻ(object _٠, EventArgs ؿ)
	{
		OnAppearanceChanged();
	}

	protected virtual Rectangle GetClipRectangle()
	{
		Rectangle displayRectangle = DisplayRectangle;
		bool ല = false;
		ElementStyle elementStyle = ElementStyleDisplay.റ(_ᠩ, out ല);
		displayRectangle.X += ᝈ.ᝐ(elementStyle);
		displayRectangle.Width -= ᝈ.ᝍ(elementStyle);
		displayRectangle.Y += ᝈ._1753(elementStyle);
		displayRectangle.Height -= ᝈ.ᝏ(elementStyle);
		if (ല)
		{
			elementStyle.Dispose();
		}
		return displayRectangle;
	}

	[DefaultValue(eItemDock.Left)]
	public eItemDock GetDock(BaseItem item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item cannot be null");
		}
		eItemDock value = eItemDock.Left;
		if (_ⲣ.TryGetValue(item, out value))
		{
			return value;
		}
		return eItemDock.Left;
	}

	[Localizable(true)]
	public void SetDock(BaseItem item, eItemDock dock)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item cannot be null");
		}
		if (dock == eItemDock.Left)
		{
			if (_ⲣ.ContainsKey(item))
			{
				_ⲣ.Remove(item);
			}
		}
		else if (_ⲣ.ContainsKey(item))
		{
			_ⲣ[item] = dock;
		}
		else
		{
			_ⲣ.Add(item, dock);
		}
		if (DesignMode)
		{
			NeedRecalcSize = true;
			Refresh();
		}
	}

	protected virtual Size GetEmptyDesignTimeSize()
	{
		return _Ⲣ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		if (DesignMode && !SystemContainer)
		{
			BaseItem baseItem = ItemAtLocation(objArg.X, objArg.Y);
			if (baseItem == this && (!baseItem.DesignMode || baseItem.DesignTimeVisible) && GetOwner() is IOwner owner)
			{
				owner.SetFocusItem(this);
				return;
			}
		}
		else if (!DisplayRectangle.Contains(objArg.X, objArg.Y))
		{
			if (DesignMode)
			{
				if (GetOwner() is IOwner owner2)
				{
					owner2.SetFocusItem(null);
				}
				LeaveHotSubItem(null);
			}
			return;
		}
		base.InternalMouseDown(objArg);
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		if (DesignMode && !SystemContainer && !_ⲡ.IsEmpty && _ⲡ.Contains(x, y))
		{
			return this;
		}
		if (!Visible || !DisplayRectangle.Contains(x, y))
		{
			return null;
		}
		if (m_SubItems != null)
		{
			foreach (BaseItem subItem in m_SubItems)
			{
				if (subItem.IsContainer)
				{
					BaseItem baseItem2 = subItem.ItemAtLocation(x, y);
					if (baseItem2 != null)
					{
						return baseItem2;
					}
				}
				else if ((subItem.Visible || base.IsOnCustomizeMenu) && subItem.Displayed && subItem.DisplayRectangle.Contains(x, y) && (!DesignMode || DisplayRectangle.IntersectsWith(subItem.DisplayRectangle)))
				{
					return subItem;
				}
			}
		}
		if (DisplayRectangle.Contains(x, y) && !SystemContainer)
		{
			return this;
		}
		return null;
	}

	private bool _᭕(object ⲧ)
	{
		if (ⲧ is BaseItem value && SubItems.Contains(value))
		{
			return true;
		}
		return false;
	}

	bool IExtenderProvider.CanExtend(object ⲧ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᭕
		return this._᭕(ⲧ);
	}

	protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
	}

	private InsertPosition ᆉ(Point ॺ, BaseItem ᆊ)
	{
		return GetContainerInsertPosition(ॺ, ᆊ);
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point ॺ, BaseItem ᆊ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆉ
		return this.ᆉ(ॺ, ᆊ);
	}

	private void ᆋ(int ᆌ, bool ᆍ)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, ᆌ, ᆍ);
	}

	void IDesignTimeProvider.DrawReversibleMarker(int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆋ
		this.ᆋ(ᆌ, ᆍ);
	}

	private void ᆎ(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		DesignTimeProviderContainer.InsertItemAt(this, ਤ, ᆌ, ᆍ);
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆎ
		this.ᆎ(ਤ, ᆌ, ᆍ);
	}
}
