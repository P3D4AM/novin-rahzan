using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[ProvideProperty("Dock", typeof(BaseItem))]
public class ItemDockContainer : ImageItem
{
	private ElementStyle _BackgroundStyle = new ElementStyle();

	private Rectangle _DesignerSelectionRectangle = Rectangle.Empty;

	private Size _EmptyDesignTimeSize = new Size(24, 24);

	private bool _Offsetting;

	private ItemDisplay _ItemDisplay;

	private Dictionary<BaseItem, eItemDock> _ItemDock = new Dictionary<BaseItem, eItemDock>();

	private bool _LastChildFill = true;

	private bool _SystemContainer;

	private BaseItem LastVisibleItem
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

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether the last child element within a ItemDockContainer stretches to fill the remaining available space.")]
	public bool LastChildFill
	{
		get
		{
			return _LastChildFill;
		}
		set
		{
			_LastChildFill = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	public bool SystemContainer
	{
		get
		{
			return _SystemContainer;
		}
		internal set
		{
			_SystemContainer = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			Point offset = new Point(value.X - m_Rect.X, value.Y - m_Rect.Y);
			base.Bounds = value;
			if (!offset.IsEmpty)
			{
				OffsetSubItems(offset);
			}
		}
	}

	public ItemDockContainer()
	{
		m_IsContainer = true;
		_BackgroundStyle.StyleChanged += BackgroundStyleChanged;
	}

	public override BaseItem Copy()
	{
		ItemDockContainer itemDockContainer = new ItemDockContainer();
		CopyToItem(itemDockContainer);
		return itemDockContainer;
	}

	internal void InternalCopyToItem(ButtonItem copy)
	{
		CopyToItem(copy);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		base.CopyToItem(copy);
		ItemDockContainer itemDockContainer = (ItemDockContainer)copy;
		itemDockContainer.LastChildFill = _LastChildFill;
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
			m_Rect = LayoutItems(m_Rect);
			base.RecalcSize();
		}
	}

	private Rectangle LayoutItems(Rectangle bounds)
	{
		Rectangle rectangle = default(Rectangle);
		BaseItem lastVisibleItem = LastVisibleItem;
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
			if (_LastChildFill && subItem == lastVisibleItem)
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
		if (bounds.Width < rectangle.Width)
		{
			bounds.Width = rectangle.Width;
		}
		if (bounds.Height < rectangle.Height)
		{
			bounds.Height = rectangle.Height;
		}
		if (!Stretch && rectangle.Height < bounds.Height)
		{
			bounds.Height = rectangle.Height;
		}
		ArrangeItems(bounds);
		return bounds;
	}

	private void ArrangeItems(Rectangle bounds)
	{
		BaseItem lastVisibleItem = LastVisibleItem;
		Rectangle rectangle = bounds;
		foreach (BaseItem subItem in m_SubItems)
		{
			if (!subItem.Visible)
			{
				continue;
			}
			eItemDock dock = GetDock(subItem);
			if (_LastChildFill && subItem == lastVisibleItem)
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
			ArrangeItems(m_Rect);
		}
		base.OnExternalSizeChange();
	}

	protected override void OnTopLocationChanged(int oldValue)
	{
		base.OnTopLocationChanged(oldValue);
		Point offset = new Point(0, TopInternal - oldValue);
		OffsetSubItems(offset);
	}

	protected override void OnLeftLocationChanged(int oldValue)
	{
		base.OnLeftLocationChanged(oldValue);
		Point offset = new Point(LeftInternal - oldValue, 0);
		OffsetSubItems(offset);
	}

	internal void OffsetSubItems(Point offset)
	{
		if (_Offsetting || offset.IsEmpty)
		{
			return;
		}
		try
		{
			_Offsetting = true;
			BaseItem[] array = new BaseItem[SubItems.Count];
			SubItems.CopyTo(array, 0);
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				Rectangle bounds = baseItem.Bounds;
				bounds.Offset(offset);
				baseItem.Bounds = bounds;
			}
		}
		finally
		{
			_Offsetting = false;
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
		ItemDisplay itemDisplay = GetItemDisplay();
		itemDisplay.Paint(this, p);
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
			Image val3 = (Image)(object)BarFunctions.LoadBitmap("SystemImages.AddMoreItemsContainer.png");
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
			_DesignerSelectionRectangle = new Rectangle(displayRectangle.X + 1, displayRectangle.Y + 1, val3.Width, val3.Height);
			graphics.DrawImageUnscaled(val3, _DesignerSelectionRectangle.Location);
		}
		DrawInsertMarker(p.Graphics);
	}

	internal ItemDisplay GetItemDisplay()
	{
		if (_ItemDisplay == null)
		{
			_ItemDisplay = new ItemDisplay();
		}
		return _ItemDisplay;
	}

	protected virtual void PaintBackground(ItemPaintArgs p)
	{
		_BackgroundStyle.SetColorScheme(p.Colors);
		Graphics graphics = p.Graphics;
		ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(_BackgroundStyle, graphics, DisplayRectangle));
	}

	private void BackgroundStyleChanged(object sender, EventArgs e)
	{
		OnAppearanceChanged();
	}

	protected virtual Rectangle GetClipRectangle()
	{
		Rectangle displayRectangle = DisplayRectangle;
		bool dispose = false;
		ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out dispose);
		displayRectangle.X += ElementStyleLayout.LeftWhiteSpace(elementStyle);
		displayRectangle.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(elementStyle);
		displayRectangle.Y += ElementStyleLayout.TopWhiteSpace(elementStyle);
		displayRectangle.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(elementStyle);
		if (dispose)
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
		if (_ItemDock.TryGetValue(item, out value))
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
			if (_ItemDock.ContainsKey(item))
			{
				_ItemDock.Remove(item);
			}
		}
		else if (_ItemDock.ContainsKey(item))
		{
			_ItemDock[item] = dock;
		}
		else
		{
			_ItemDock.Add(item, dock);
		}
	}

	protected virtual Size GetEmptyDesignTimeSize()
	{
		return _EmptyDesignTimeSize;
	}
}
