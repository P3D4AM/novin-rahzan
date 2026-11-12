using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class SuperTabStripBaseDisplay
{
	private const int MarkerSize = 10;

	private int _TabHeight;

	private SuperTabStripItem _TabStripItem;

	private SerialContentLayoutManager _ContentManager;

	private SuperTabItemLayoutManager _BlockManager;

	private SuperTabColorTable _DefaultColorTable;

	private Bitmap _CloseButton;

	private Color _CloseButtonColor;

	private Bitmap _MenuButton1;

	private Bitmap _MenuButton2;

	private Color _MenuButtonColor;

	private Bitmap _TopInsertMarker;

	private Bitmap _LeftInsertMarker;

	private Color _InsertMarkerColor;

	internal eTabStripAlignment Alignment => _TabStripItem.TabAlignment;

	internal SuperTabColorTable DefaultColorTable
	{
		get
		{
			return _DefaultColorTable;
		}
		set
		{
			_DefaultColorTable = value;
			for (int i = 0; i < Tabs.Count; i++)
			{
				if (Tabs[i] is SuperTabItem superTabItem)
				{
					superTabItem.TabItemDisplay.SetDefaultColorTable();
				}
			}
		}
	}

	internal bool IsRightToLeft => _TabStripItem.IsRightToLeft;

	internal bool IsVertical => _TabStripItem.IsVertical;

	internal virtual int MinTabStripHeight
	{
		get
		{
			if (TabItemsBounds.IsEmpty)
			{
				return TabHeight + 6;
			}
			if (Alignment == eTabStripAlignment.Top || Alignment == eTabStripAlignment.Bottom)
			{
				return TabItemsBounds.Height + 2;
			}
			return TabItemsBounds.Width + 2;
		}
	}

	internal bool ShowFocusRectangle => _TabStripItem.ShowFocusRectangle;

	internal bool CloseButtonOnTabs => _TabStripItem.CloseButtonOnTabsVisible;

	internal Size FixedTabSize => _TabStripItem.FixedTabSize;

	internal virtual Size MinTabSize => new Size(13, 13);

	internal SuperTabItem SelectedTab => _TabStripItem.SelectedTab;

	internal Font SelectedTabFont => _TabStripItem.SelectedTabFont;

	internal SuperTabControlBox TabControlBox => _TabStripItem.ControlBox;

	internal int TabHeight
	{
		get
		{
			if (_TabHeight <= 0)
			{
				_TabHeight = ((FixedTabSize.Height > 0) ? FixedTabSize.Height : GetTabHeight());
			}
			return _TabHeight;
		}
		set
		{
			_TabHeight = value;
		}
	}

	internal Rectangle TabItemsBounds
	{
		get
		{
			return _TabStripItem.TabItemsBounds;
		}
		set
		{
			_TabStripItem.TabItemsBounds = value;
		}
	}

	internal SubItemsCollection Tabs => _TabStripItem.SubItems;

	internal SuperTabStrip TabStrip => TabStripItem.TabStrip;

	internal SuperTabStripItem TabStripItem => _TabStripItem;

	internal virtual bool PreDrawStripBorder => false;

	internal virtual int SelectedPaddingWidth => 0;

	internal virtual int TabOverlap => 0;

	internal virtual int TabSpacing => 0;

	internal virtual bool TabOverlapLeft => false;

	internal virtual Size TabLayoutOffset => Size.Empty;

	public SuperTabStripBaseDisplay(SuperTabStripItem tabStripItem)
	{
		_TabStripItem = tabStripItem;
		_ContentManager = new SerialContentLayoutManager();
		_ContentManager.NextPosition += ContentManagerNextPosition;
		_BlockManager = new SuperTabItemLayoutManager(tabStripItem);
		_DefaultColorTable = SuperTabStyleColorFactory.GetColorTable(tabStripItem.TabStyle, ColorFactory.Empty);
	}

	private void ContentManagerNextPosition(object sender, LayoutManagerPositionEventArgs e)
	{
		NextBlockPosition(e);
	}

	internal virtual void RecalcSize(Graphics g)
	{
		RecalcLayoutInit(g);
		RecLayoutContent();
		int minTabStripHeight = MinTabStripHeight;
		eTabStripAlignment tabAlignment = _TabStripItem.TabAlignment;
		if ((uint)tabAlignment <= 1u)
		{
			((Control)_TabStripItem.TabStrip).Width = minTabStripHeight;
			_TabStripItem.WidthInternal = minTabStripHeight;
			_TabStripItem.HeightInternal = ((Control)_TabStripItem.TabStrip).Height;
		}
		else
		{
			((Control)_TabStripItem.TabStrip).Height = minTabStripHeight;
			_TabStripItem.HeightInternal = minTabStripHeight;
			_TabStripItem.WidthInternal = ((Control)_TabStripItem.TabStrip).Width;
		}
	}

	internal virtual void RecLayoutContent()
	{
		if (TabControlBox.MenuBox.AutoHide && TabControlBox.Visible)
		{
			RecalcLayout(GetTabClientArea(mbVisible: false));
			TabControlBox.MenuBox.Visible = !AllItemsVisible();
		}
		RecalcLayout(GetTabClientArea());
	}

	protected virtual void RecalcLayoutInit(Graphics g)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		_BlockManager.Graphics = g;
		g.CompositingMode = g.CompositingMode;
		_BlockManager.FixedTabSize = FixedTabSize;
		_BlockManager.TabLayoutOffset = TabLayoutOffset;
		_ContentManager.RightToLeft = IsRightToLeft;
		_ContentManager.EvenHeight = true;
		switch (Alignment)
		{
		case eTabStripAlignment.Bottom:
			_ContentManager.ContentAlignment = eContentAlignment.Left;
			_ContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
			_ContentManager.BlockLineAlignment = eContentVerticalAlignment.Top;
			_ContentManager.ContentOrientation = eContentOrientation.Horizontal;
			break;
		case eTabStripAlignment.Top:
			_ContentManager.ContentAlignment = eContentAlignment.Left;
			_ContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
			_ContentManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
			_ContentManager.ContentOrientation = eContentOrientation.Horizontal;
			break;
		case eTabStripAlignment.Left:
			_ContentManager.ContentAlignment = eContentAlignment.Left;
			_ContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
			_ContentManager.BlockLineAlignment = eContentVerticalAlignment.Top;
			_ContentManager.ContentOrientation = eContentOrientation.Vertical;
			break;
		case eTabStripAlignment.Right:
			_ContentManager.ContentAlignment = eContentAlignment.Left;
			_ContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
			_ContentManager.BlockLineAlignment = eContentVerticalAlignment.Top;
			_ContentManager.ContentOrientation = eContentOrientation.Vertical;
			break;
		}
		switch (_TabStripItem.TabLayoutType)
		{
		case eSuperTabLayoutType.SingleLine:
			_ContentManager.MultiLine = false;
			_ContentManager.FitContainer = false;
			break;
		case eSuperTabLayoutType.SingleLineFit:
			_ContentManager.MultiLine = false;
			_ContentManager.FitContainer = true;
			break;
		case eSuperTabLayoutType.MultiLine:
			_ContentManager.MultiLine = true;
			_ContentManager.FitContainer = false;
			break;
		case eSuperTabLayoutType.MultiLineFit:
			_ContentManager.MultiLine = true;
			_ContentManager.FitContainer = true;
			break;
		}
	}

	protected virtual void RecalcLayout(Rectangle r)
	{
		if (Tabs.Count > 0)
		{
			IBlock[] array = new IBlock[Tabs.Count];
			for (int i = 0; i < Tabs.Count; i++)
			{
				array[i] = Tabs[i];
			}
			if (IsRightToLeft)
			{
				Rectangle rectangle = r;
				if (_TabStripItem.ControlBox.Visible)
				{
					rectangle.X += TabControlBox.Size.Width;
				}
				TabItemsBounds = _ContentManager.Layout(rectangle, array, _BlockManager);
				RecalcSizeTabControlBox(r, rectangle);
			}
			else
			{
				TabItemsBounds = _ContentManager.Layout(r, array, _BlockManager);
				RecalcSizeTabControlBox(r, TabItemsBounds);
			}
		}
		else
		{
			TabItemsBounds = Rectangle.Empty;
		}
	}

	protected virtual void RecalcSizeTabControlBox(Rectangle dispRect, Rectangle tabsBounds)
	{
		if (_TabStripItem.ControlBox.Visible)
		{
			Size size = TabControlBox.Size;
			eTabStripAlignment alignment = Alignment;
			int x;
			int y;
			if ((uint)alignment <= 1u)
			{
				x = dispRect.X + (dispRect.Width - size.Width) / 2;
				y = _TabStripItem.DisplayRectangle.Bottom - size.Height;
			}
			else if (IsRightToLeft)
			{
				x = tabsBounds.X + (dispRect.Width - size.Width) / 2;
				y = _TabStripItem.DisplayRectangle.Bottom - size.Height;
			}
			else
			{
				x = _TabStripItem.DisplayRectangle.Right - size.Width;
				y = tabsBounds.Y + (dispRect.Height - size.Height) / 2;
			}
			TabControlBox.Bounds = new Rectangle(x, y, size.Width, size.Height);
		}
	}

	protected virtual Rectangle GetTabClientArea()
	{
		return GetTabClientArea(TabControlBox.MenuBox.Visible);
	}

	protected virtual Rectangle GetTabClientArea(bool mbVisible)
	{
		Rectangle result = default(Rectangle);
		result.Size = ((Control)TabStripItem.TabStrip).Size;
		if (TabStripItem.ControlBox.Visible)
		{
			if (Alignment == eTabStripAlignment.Right || Alignment == eTabStripAlignment.Left)
			{
				result.Height -= TabStripItem.ControlBox.Size.Height;
				if (TabStripItem.ControlBox.MenuBox.Visible != mbVisible)
				{
					if (mbVisible)
					{
						result.Height -= TabStripItem.ControlBox.MenuBox.HeightInternal;
					}
					else
					{
						result.Height += TabStripItem.ControlBox.MenuBox.HeightInternal;
					}
				}
				if (result.Height < TabStripItem.ControlBox.Size.Height)
				{
					result.Height = TabStripItem.ControlBox.Size.Height;
				}
			}
			else
			{
				result.Width -= TabStripItem.ControlBox.Size.Width;
				if (TabStripItem.ControlBox.MenuBox.Visible != mbVisible)
				{
					if (mbVisible)
					{
						result.Width -= TabStripItem.ControlBox.MenuBox.WidthInternal;
					}
					else
					{
						result.Width += TabStripItem.ControlBox.MenuBox.WidthInternal;
					}
				}
				if (result.Width < TabStripItem.ControlBox.Size.Width)
				{
					result.Width = TabStripItem.ControlBox.Size.Width;
				}
			}
		}
		return result;
	}

	protected virtual int GetTabHeight()
	{
		if (FixedTabSize.Height > 0)
		{
			return FixedTabSize.Height;
		}
		int num = ((Alignment == eTabStripAlignment.Left || Alignment == eTabStripAlignment.Right) ? 20 : 16);
		Font val = (_TabStripItem.SelectedTabFont ?? _TabStripItem.TabFont) ?? ((Control)TabStrip).Font;
		if (val.Height > num)
		{
			num = val.Height;
		}
		return GetMaxHeight(num) + 4;
	}

	private int GetMaxHeight(int height)
	{
		int num = 0;
		foreach (BaseItem tab in Tabs)
		{
			if (!(tab is SuperTabItem superTabItem))
			{
				continue;
			}
			CompositeImage tabImage = superTabItem.GetTabImage();
			if (tabImage != null)
			{
				int num2 = tabImage.Height;
				ImageAlignment effectiveImageAlignment = superTabItem.EffectiveImageAlignment;
				if ((uint)effectiveImageAlignment <= 2u || (uint)(effectiveImageAlignment - 6) <= 2u)
				{
					num2 += height;
				}
				if (num2 > num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	protected virtual void NextBlockPosition(LayoutManagerPositionEventArgs e)
	{
	}

	internal virtual Rectangle NextBlockPosition(BaseItem item, BaseItem vItem)
	{
		Rectangle bounds = item.Bounds;
		if (IsVertical)
		{
			bounds.X = vItem.Bounds.X;
			bounds.Y += item.Bounds.Height;
		}
		else
		{
			bounds.X += item.Bounds.Width;
			bounds.Y = vItem.Bounds.Y;
		}
		bounds.Size = vItem.Size;
		return bounds;
	}

	private bool AllItemsVisible()
	{
		foreach (BaseItem tab in Tabs)
		{
			if (tab.Visible && !tab.Displayed)
			{
				return false;
			}
		}
		return true;
	}

	public void Paint(ItemPaintArgs p)
	{
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		Rectangle displayRectangle = _TabStripItem.DisplayRectangle;
		if (displayRectangle.Width <= 0 || displayRectangle.Height <= 0)
		{
			return;
		}
		using SuperTabColorTable ct = GetColorTable();
		if (((Control)TabStrip).Tag is eStyle && ((eStyle)((Control)TabStrip).Tag == eStyle.Office2010Black || (eStyle)((Control)TabStrip).Tag == eStyle.Office2010Silver))
		{
			Color color = (((eStyle)((Control)TabStrip).Tag != eStyle.Office2010Black) ? Color.FromArgb(255, 238, 240, 242) : Color.FromArgb(255, 203, 203, 203));
			Brush val = (Brush)new SolidBrush(color);
			try
			{
				p.Graphics.FillRectangle(val, displayRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			DrawBackground(p, ct);
			_TabStripItem.OnPaintBackground(p.Graphics, ct);
		}
		if (Tabs.Count > 0)
		{
			if (_TabStripItem.ControlBox.Visible)
			{
				TabControlBox.Paint(p);
			}
			if (PreDrawStripBorder)
			{
				DrawStripBorder(p, ct);
			}
			DrawTabs(p);
			DrawTabInsertMarker(p, ct);
			if (!PreDrawStripBorder)
			{
				DrawStripBorder(p, ct);
			}
		}
	}

	protected virtual void DrawBackground(ItemPaintArgs p, SuperTabColorTable ct)
	{
		Graphics graphics = p.Graphics;
		Rectangle displayRectangle = _TabStripItem.DisplayRectangle;
		int num = ct.Background.GradientAngle;
		switch (_TabStripItem.TabAlignment)
		{
		case eTabStripAlignment.Bottom:
			num += 180;
			displayRectangle.Y--;
			break;
		case eTabStripAlignment.Left:
			num += -90;
			break;
		case eTabStripAlignment.Right:
			num += 90;
			displayRectangle.X--;
			break;
		}
		Brush brush = ct.Background.GetBrush(displayRectangle, num);
		try
		{
			if (brush != null)
			{
				graphics.FillRectangle(brush, displayRectangle);
			}
		}
		finally
		{
			((IDisposable)brush)?.Dispose();
		}
	}

	protected virtual void DrawTabs(ItemPaintArgs p)
	{
		for (int num = Tabs.Count - 1; num >= 0; num--)
		{
			BaseItem baseItem = Tabs[num];
			if (baseItem != null && baseItem.Displayed && !(baseItem is SuperTabItem { IsSelected: not false }))
			{
				baseItem.Paint(p);
			}
		}
		if (SelectedTab != null && SelectedTab.Displayed)
		{
			SelectedTab.Paint(p);
		}
		for (int num2 = Tabs.Count - 1; num2 >= 0; num2--)
		{
			BaseItem baseItem2 = Tabs[num2];
			if (baseItem2 != null && baseItem2.Displayed)
			{
				baseItem2.DrawInsertMarker(p.Graphics);
			}
		}
	}

	private void DrawTabInsertMarker(ItemPaintArgs p, SuperTabColorTable ct)
	{
		if (_TabStripItem.InsertTab == null)
		{
			return;
		}
		if (_InsertMarkerColor != ct.InsertMarker)
		{
			if (_TopInsertMarker != null)
			{
				((Image)_TopInsertMarker).Dispose();
				_TopInsertMarker = null;
			}
			if (_LeftInsertMarker != null)
			{
				((Image)_LeftInsertMarker).Dispose();
				_LeftInsertMarker = null;
			}
			_InsertMarkerColor = ct.InsertMarker;
		}
		switch (_TabStripItem.TabAlignment)
		{
		case eTabStripAlignment.Top:
		case eTabStripAlignment.Bottom:
			DrawTopInsertMarker(p.Graphics);
			break;
		case eTabStripAlignment.Left:
		case eTabStripAlignment.Right:
			DrawLeftInsertMarker(p.Graphics);
			break;
		}
	}

	private void DrawTopInsertMarker(Graphics g)
	{
		Rectangle topInsertRect = GetTopInsertRect();
		Bitmap topInsertMarker = GetTopInsertMarker(g);
		g.DrawImageUnscaled((Image)(object)topInsertMarker, topInsertRect);
	}

	private Rectangle GetTopInsertRect()
	{
		BaseItem insertTab = _TabStripItem.InsertTab;
		SuperTabStripBaseDisplay tabDisplay = _TabStripItem.TabDisplay;
		int num = insertTab.Bounds.X;
		int num2 = insertTab.Bounds.Right;
		int num3 = Tabs.IndexOf(insertTab);
		if (num3 > 0 && IsSameLine(insertTab, Tabs[num3 - 1]))
		{
			if (tabDisplay.TabOverlap > 0)
			{
				if (tabDisplay.TabOverlapLeft)
				{
					num = Tabs[num3 - 1].Bounds.Right;
				}
			}
			else
			{
				num = (insertTab.Bounds.X + Tabs[num3 - 1].Bounds.Right) / 2;
			}
		}
		if (num3 + 1 < Tabs.Count && IsSameLine(insertTab, Tabs[num3 + 1]))
		{
			if (tabDisplay.TabOverlap > 0)
			{
				if (!tabDisplay.TabOverlapLeft)
				{
					num2 = Tabs[num3 + 1].Bounds.X;
				}
			}
			else
			{
				num2 = (insertTab.Bounds.Right + Tabs[num3 + 1].Bounds.X) / 2;
			}
		}
		Rectangle bounds = insertTab.Bounds;
		bounds.X = num;
		bounds.Width = num2 - num;
		bounds.Height = 10;
		if (_TabStripItem.InsertBefore)
		{
			bounds.X -= 5;
			if (bounds.X < 0)
			{
				bounds.X = 0;
			}
		}
		else
		{
			bounds.X = bounds.Right - 5;
			if (bounds.X + 10 > _TabStripItem.Bounds.Right)
			{
				bounds.X = _TabStripItem.Bounds.Right - 10;
			}
		}
		bounds.Width = 10;
		return bounds;
	}

	private Bitmap GetTopInsertMarker(Graphics g)
	{
		if (_TopInsertMarker == null)
		{
			_TopInsertMarker = CreateTopInsertMarker(g);
		}
		return _TopInsertMarker;
	}

	private Bitmap CreateTopInsertMarker(Graphics g)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		Bitmap val = new Bitmap(10, 10, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.SmoothingMode = g.SmoothingMode;
			GraphicsPath val3 = new GraphicsPath();
			try
			{
				val3.AddPolygon(new Point[9]
				{
					new Point(6, 0),
					new Point(6, 5),
					new Point(9, 5),
					new Point(5, 9),
					new Point(4, 9),
					new Point(0, 5),
					new Point(3, 5),
					new Point(3, 0),
					new Point(6, 0)
				});
				val3.CloseFigure();
				Color color = Color.FromArgb(200, _InsertMarkerColor);
				SolidBrush val4 = new SolidBrush(color);
				try
				{
					val2.FillPath((Brush)(object)val4, val3);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				Pen val5 = new Pen(color);
				try
				{
					val2.DrawPath(val5, val3);
					return val;
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void DrawLeftInsertMarker(Graphics g)
	{
		Rectangle leftInsertRect = GetLeftInsertRect();
		Bitmap leftInsertMarker = GetLeftInsertMarker(g);
		g.DrawImageUnscaled((Image)(object)leftInsertMarker, leftInsertRect);
	}

	private Rectangle GetLeftInsertRect()
	{
		BaseItem insertTab = _TabStripItem.InsertTab;
		SuperTabStripBaseDisplay tabDisplay = _TabStripItem.TabDisplay;
		int num = insertTab.Bounds.Y;
		int num2 = insertTab.Bounds.Bottom;
		int num3 = Tabs.IndexOf(insertTab);
		if (num3 > 0 && IsSameLine(insertTab, Tabs[num3 - 1]))
		{
			if (tabDisplay.TabOverlap > 0)
			{
				if (tabDisplay.TabOverlapLeft)
				{
					num = Tabs[num3 - 1].Bounds.Bottom;
				}
			}
			else
			{
				num = (insertTab.Bounds.Y + Tabs[num3 - 1].Bounds.Bottom) / 2;
			}
		}
		if (num3 + 1 < Tabs.Count && IsSameLine(insertTab, Tabs[num3 + 1]))
		{
			if (tabDisplay.TabOverlap > 0)
			{
				if (!tabDisplay.TabOverlapLeft)
				{
					num2 = Tabs[num3 + 1].Bounds.Y;
				}
			}
			else
			{
				num2 = (insertTab.Bounds.Bottom + Tabs[num3 + 1].Bounds.Y) / 2;
			}
		}
		Rectangle bounds = insertTab.Bounds;
		bounds.Y = num;
		bounds.Height = num2 - num;
		bounds.Width = 10;
		if (_TabStripItem.InsertBefore)
		{
			bounds.Y -= 5;
			if (bounds.Y < 0)
			{
				bounds.Y = 0;
			}
		}
		else
		{
			bounds.Y = bounds.Bottom - 5;
			if (bounds.Y + 10 > _TabStripItem.Bounds.Bottom)
			{
				bounds.Y = _TabStripItem.Bounds.Bottom - 10;
			}
		}
		bounds.Height = 10;
		return bounds;
	}

	private Bitmap GetLeftInsertMarker(Graphics g)
	{
		if (_LeftInsertMarker == null)
		{
			_LeftInsertMarker = CreateLeftInsertMarker(g);
		}
		return _LeftInsertMarker;
	}

	private Bitmap CreateLeftInsertMarker(Graphics g)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		Bitmap val = new Bitmap(10, 10, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.SmoothingMode = g.SmoothingMode;
			GraphicsPath val3 = new GraphicsPath();
			try
			{
				val3.AddPolygon(new Point[9]
				{
					new Point(0, 6),
					new Point(5, 6),
					new Point(5, 9),
					new Point(9, 5),
					new Point(9, 4),
					new Point(5, 0),
					new Point(5, 3),
					new Point(0, 3),
					new Point(0, 6)
				});
				val3.CloseFigure();
				Color color = Color.FromArgb(200, _InsertMarkerColor);
				SolidBrush val4 = new SolidBrush(color);
				try
				{
					val2.FillPath((Brush)(object)val4, val3);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				Pen val5 = new Pen(color);
				try
				{
					val2.DrawPath(val5, val3);
					return val;
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private bool IsSameLine(BaseItem item1, BaseItem item2)
	{
		if (_TabStripItem.TabLines == 1)
		{
			return true;
		}
		return _TabStripItem.TabAlignment switch
		{
			eTabStripAlignment.Top => item1.Bounds.Bottom == item2.Bounds.Bottom, 
			eTabStripAlignment.Left => item1.Bounds.Right == item2.Bounds.Right, 
			eTabStripAlignment.Bottom => item1.Bounds.Top == item2.Bounds.Top, 
			_ => item1.Bounds.Left == item2.Bounds.Left, 
		};
	}

	protected virtual void DrawStripBorder(ItemPaintArgs p, SuperTabColorTable ct)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Rectangle bounds = TabStripItem.Bounds;
		Rectangle rectangle = ((TabStripItem.SelectedTab != null) ? TabStripItem.SelectedTab.DisplayRectangle : Rectangle.Empty);
		Pen val = new Pen(ct.InnerBorder);
		try
		{
			Pen val2 = new Pen(ct.OuterBorder);
			try
			{
				GetBorderLines(out var inner, out var outer);
				switch (TabStripItem.TabAlignment)
				{
				case eTabStripAlignment.Top:
				case eTabStripAlignment.Bottom:
					if (rectangle.X > bounds.X)
					{
						graphics.DrawLine(val, bounds.X, inner, rectangle.X + 1, inner);
						graphics.DrawLine(val2, bounds.X, outer, rectangle.X, outer);
					}
					if (rectangle.Right < bounds.Right)
					{
						graphics.DrawLine(val, rectangle.Right - 2, inner, bounds.Right - 1, inner);
						graphics.DrawLine(val2, rectangle.Right, outer, bounds.Right - 2, outer);
					}
					graphics.DrawLine(val2, bounds.X, outer, bounds.X, inner);
					graphics.DrawLine(val2, bounds.Right - 1, outer, bounds.Right - 1, inner);
					break;
				case eTabStripAlignment.Left:
				case eTabStripAlignment.Right:
					if (rectangle.Y > 0)
					{
						graphics.DrawLine(val, inner, bounds.Y, inner, rectangle.Y + 1);
						graphics.DrawLine(val2, outer, bounds.Y, outer, rectangle.Y);
					}
					if (rectangle.Bottom < bounds.Bottom)
					{
						graphics.DrawLine(val, inner, rectangle.Bottom - 2, inner, bounds.Bottom - 1);
						graphics.DrawLine(val2, outer, rectangle.Bottom - 1, outer, bounds.Bottom - 1);
					}
					graphics.DrawLine(val2, inner, bounds.Y, outer, bounds.Y);
					graphics.DrawLine(val2, inner, bounds.Bottom - 1, outer, bounds.Bottom - 1);
					break;
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void GetBorderLines(out int inner, out int outer)
	{
		Rectangle bounds = TabStripItem.Bounds;
		switch (TabStripItem.TabAlignment)
		{
		case eTabStripAlignment.Top:
			inner = bounds.Bottom - 1;
			outer = bounds.Bottom - 2;
			break;
		case eTabStripAlignment.Bottom:
			inner = bounds.Y;
			outer = bounds.Y + 1;
			break;
		case eTabStripAlignment.Left:
			inner = bounds.Right - 1;
			outer = bounds.Right - 2;
			break;
		default:
			inner = bounds.X;
			outer = bounds.X + 1;
			break;
		}
	}

	internal void SetDefaultColorTable()
	{
		DefaultColorTable = SuperTabStyleColorFactory.GetColorTable(TabStrip.TabStyle, ColorFactory.Empty);
	}

	internal virtual SuperTabColorTable GetColorTable()
	{
		SuperTabColorTable tabStripColor = TabStripItem.TabStripColor;
		SuperTabColorTable superTabColorTable = (SuperTabColorTable)_DefaultColorTable.Clone();
		if (tabStripColor != null)
		{
			if (!tabStripColor.Background.IsEmpty)
			{
				superTabColorTable.Background = tabStripColor.Background;
			}
			if (tabStripColor.InnerBorder != Color.Empty)
			{
				superTabColorTable.InnerBorder = tabStripColor.InnerBorder;
			}
			if (tabStripColor.OuterBorder != Color.Empty)
			{
				superTabColorTable.OuterBorder = tabStripColor.OuterBorder;
			}
			GetControlBoxColors(superTabColorTable.ControlBoxDefault, tabStripColor.ControlBoxDefault);
			GetControlBoxColors(superTabColorTable.ControlBoxMouseOver, tabStripColor.ControlBoxMouseOver);
			GetControlBoxColors(superTabColorTable.ControlBoxPressed, tabStripColor.ControlBoxPressed);
			if (tabStripColor.InsertMarker != Color.Empty)
			{
				superTabColorTable.InsertMarker = tabStripColor.InsertMarker;
			}
			if (tabStripColor.SelectionMarker != Color.Empty)
			{
				superTabColorTable.SelectionMarker = tabStripColor.SelectionMarker;
			}
		}
		return superTabColorTable;
	}

	private void GetControlBoxColors(SuperTabControlBoxStateColorTable sbc, SuperTabControlBoxStateColorTable bc)
	{
		if (bc.Background != Color.Empty)
		{
			sbc.Background = bc.Background;
		}
		if (bc.Border != Color.Empty)
		{
			sbc.Border = bc.Border;
		}
		if (bc.Image != Color.Empty)
		{
			sbc.Image = bc.Image;
		}
	}

	public Bitmap GetCloseButton(Graphics g, Color color)
	{
		if (color != _CloseButtonColor && _CloseButton != null)
		{
			((Image)_CloseButton).Dispose();
			_CloseButton = null;
		}
		_CloseButtonColor = color;
		if (_CloseButton == null)
		{
			_CloseButton = CreateCloseButton(g);
		}
		return _CloseButton;
	}

	public virtual Bitmap CreateCloseButton(Graphics g)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		Bitmap val = new Bitmap(16, 16, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingQuality = (CompositingQuality)2;
			val2.SmoothingMode = g.SmoothingMode;
			Pen val3 = new Pen(_CloseButtonColor, 2f);
			try
			{
				val2.DrawLine(val3, new Point(3, 3), new Point(11, 11));
				val2.DrawLine(val3, new Point(11, 3), new Point(3, 11));
				return val;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public Bitmap GetMenuButton1(Graphics g, Color color)
	{
		if (color != _MenuButtonColor)
		{
			if (_MenuButton1 != null)
			{
				((Image)_MenuButton1).Dispose();
				_MenuButton1 = null;
			}
			if (_MenuButton2 != null)
			{
				((Image)_MenuButton2).Dispose();
				_MenuButton2 = null;
			}
		}
		_MenuButtonColor = color;
		if (_MenuButton1 == null)
		{
			_MenuButton1 = CreateMenuButton1(g);
		}
		return _MenuButton1;
	}

	public virtual Bitmap CreateMenuButton1(Graphics g)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		Bitmap val = new Bitmap(16, 16, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingQuality = (CompositingQuality)2;
			val2.SmoothingMode = g.SmoothingMode;
			GraphicsPath menuButtonPath = GetMenuButtonPath1();
			try
			{
				Brush val3 = (Brush)new SolidBrush(_MenuButtonColor);
				try
				{
					val2.FillPath(val3, menuButtonPath);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				Pen val4 = new Pen(_MenuButtonColor);
				try
				{
					val2.DrawPath(val4, menuButtonPath);
					return val;
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)menuButtonPath)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private GraphicsPath GetMenuButtonPath1()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddLines(new Point[3]
		{
			new Point(4, 5),
			new Point(8, 9),
			new Point(12, 5)
		});
		val.CloseAllFigures();
		return val;
	}

	public Bitmap GetMenuButton2(Graphics g, Color color)
	{
		if (color != _MenuButtonColor)
		{
			if (_MenuButton1 != null)
			{
				((Image)_MenuButton1).Dispose();
				_MenuButton1 = null;
			}
			if (_MenuButton2 != null)
			{
				((Image)_MenuButton2).Dispose();
				_MenuButton2 = null;
			}
		}
		_MenuButtonColor = color;
		if (_MenuButton2 == null)
		{
			_MenuButton2 = CreateMenuButton2(g);
		}
		return _MenuButton2;
	}

	public virtual Bitmap CreateMenuButton2(Graphics g)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		Bitmap val = new Bitmap(16, 16, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingQuality = (CompositingQuality)2;
			val2.SmoothingMode = g.SmoothingMode;
			GraphicsPath menuButtonPath = GetMenuButtonPath2();
			try
			{
				Brush val3 = (Brush)new SolidBrush(_MenuButtonColor);
				try
				{
					val2.FillPath(val3, menuButtonPath);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				Pen val4 = new Pen(_MenuButtonColor);
				try
				{
					val2.DrawPath(val4, menuButtonPath);
					return val;
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)menuButtonPath)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private GraphicsPath GetMenuButtonPath2()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddLines(new Point[3]
		{
			new Point(3, 4),
			new Point(6, 7),
			new Point(3, 11)
		});
		val.CloseFigure();
		val.AddLines(new Point[3]
		{
			new Point(9, 4),
			new Point(12, 7),
			new Point(9, 11)
		});
		val.CloseFigure();
		return val;
	}
}
