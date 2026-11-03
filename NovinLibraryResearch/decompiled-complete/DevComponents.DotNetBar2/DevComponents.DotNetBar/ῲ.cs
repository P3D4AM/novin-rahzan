using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

internal class ῲ
{
	private const int _2002 = 10;

	private int __2003;

	private SuperTabStripItem _ὗ;

	private SerialContentLayoutManager __2004;

	private SuperTabItemLayoutManager __2005;

	private SuperTabColorTable __2006;

	private Bitmap __2007;

	private Color __2008;

	private Bitmap __2009;

	private Bitmap __200A;

	private Color __200B;

	private Bitmap __200C;

	private Bitmap __200D;

	private Color __200E;

	internal eTabStripAlignment ῳ => _ὗ.TabAlignment;

	internal SuperTabColorTable ῴ
	{
		get
		{
			return __2006;
		}
		set
		{
			__2006 = value;
			for (int i = 0; i < ᇃ.Count; i++)
			{
				if (ᇃ[i] is SuperTabItem superTabItem)
				{
					superTabItem.Ὀ._1FDF();
				}
			}
		}
	}

	internal bool ᓛ => _ὗ.IsRightToLeft;

	internal bool ᓙ => _ὗ.ᓙ;

	internal virtual int _1FF5
	{
		get
		{
			if (ᓚ.IsEmpty)
			{
				return ᓘ + Dpi.Height6;
			}
			if (ῳ == eTabStripAlignment.Top || ῳ == eTabStripAlignment.Bottom)
			{
				return ᓚ.Height + Dpi.Height2;
			}
			return ᓚ.Width + Dpi.Width2;
		}
	}

	internal bool ῶ => _ὗ.ShowFocusRectangle;

	internal bool ῷ => _ὗ.CloseButtonOnTabsVisible;

	internal Size Ὸ => _ὗ.FixedTabSize;

	internal virtual Size Ό => Dpi.Size(new Size(13, 13));

	internal SuperTabItem Ὼ => _ὗ.SelectedTab;

	internal Font Ώ => _ὗ.SelectedTabFont;

	internal SuperTabControlBox ῼ => _ὗ.ControlBox;

	internal int ᓘ
	{
		get
		{
			if (__2003 <= 0)
			{
				__2003 = ((Ὸ.Height > 0) ? Dpi.Height(Ὸ.Height) : GetTabHeight());
			}
			return Dpi.Height(__2003);
		}
		set
		{
			__2003 = value;
		}
	}

	internal Rectangle ᓚ
	{
		get
		{
			return _ὗ.ᓚ;
		}
		set
		{
			_ὗ.ᓚ = value;
		}
	}

	internal SubItemsCollection ᇃ => _ὗ.SubItems;

	internal SuperTabStrip Ὁ => _1FD4.Ὁ;

	internal SuperTabStripItem _1FD4 => _ὗ;

	internal virtual bool _1FFD => false;

	internal virtual int _1FFE => 0;

	internal virtual int _136F => 0;

	internal virtual int _1FFF => 0;

	internal virtual bool _2000 => false;

	internal virtual Size _2001 => Size.Empty;

	public ῲ(SuperTabStripItem tabStripItem)
	{
		_ὗ = tabStripItem;
		__2004 = new SerialContentLayoutManager();
		__2004.NextPosition += _200F;
		__2005 = new SuperTabItemLayoutManager(tabStripItem);
		__2006 = ጦ.ጧ(tabStripItem.TabStyle, ColorFactory.Empty);
	}

	private void _200F(object _٠, LayoutManagerPositionEventArgs ؿ)
	{
		if (ؿ.Block is SuperTabItem superTabItem)
		{
			int num = superTabItem.Ὁ.Tabs.IndexOf(superTabItem);
			if (num >= 0)
			{
				for (int i = num + 1; i < superTabItem.Ὁ.Tabs.Count; i++)
				{
					BaseItem baseItem = superTabItem.Ὁ.Tabs[i];
					if (baseItem == null || baseItem.Visible)
					{
						if (baseItem is SuperTabItem)
						{
							ؿ.SwitchToNewLine = ((SuperTabItem)baseItem).StartsNewTabLine;
						}
						break;
					}
				}
			}
		}
		NextBlockPosition(ؿ);
	}

	internal virtual void _0BE5(Graphics _٥)
	{
		RecalcLayoutInit(_٥);
		_2010();
		int num = _1FF5;
		switch (_ὗ.TabAlignment)
		{
		case eTabStripAlignment.Left:
		case eTabStripAlignment.Right:
			((Control)_ὗ.Ὁ).Width = num;
			_ὗ.WidthInternal = num;
			_ὗ.HeightInternal = ((Control)_ὗ.Ὁ).Height;
			break;
		default:
			((Control)_ὗ.Ὁ).Height = num;
			_ὗ.HeightInternal = num;
			_ὗ.WidthInternal = ((Control)_ὗ.Ὁ).Width;
			break;
		}
	}

	internal virtual void _2010()
	{
		if (ῼ.MenuBox.AutoHide && ῼ.Visible)
		{
			RecalcLayout(GetTabClientArea(mbVisible: false));
			ῼ.MenuBox.Visible = !_2015();
		}
		RecalcLayout(GetTabClientArea());
	}

	protected virtual void RecalcLayoutInit(Graphics g)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		__2005.Graphics = g;
		g.CompositingMode = g.CompositingMode;
		__2005.FixedTabSize = Ὸ;
		__2005.TabLayoutOffset = _2001;
		__2004.RightToLeft = ᓛ;
		__2004.EvenHeight = true;
		switch (ῳ)
		{
		case eTabStripAlignment.Bottom:
			__2004.ContentAlignment = eContentAlignment.Left;
			__2004.ContentVerticalAlignment = eContentVerticalAlignment.Top;
			__2004.BlockLineAlignment = eContentVerticalAlignment.Top;
			__2004.ContentOrientation = eContentOrientation.Horizontal;
			break;
		case eTabStripAlignment.Top:
			__2004.ContentAlignment = eContentAlignment.Left;
			__2004.ContentVerticalAlignment = eContentVerticalAlignment.Top;
			__2004.BlockLineAlignment = eContentVerticalAlignment.Bottom;
			__2004.ContentOrientation = eContentOrientation.Horizontal;
			break;
		case eTabStripAlignment.Left:
			__2004.ContentAlignment = eContentAlignment.Left;
			__2004.ContentVerticalAlignment = eContentVerticalAlignment.Top;
			__2004.BlockLineAlignment = eContentVerticalAlignment.Top;
			__2004.ContentOrientation = eContentOrientation.Vertical;
			break;
		case eTabStripAlignment.Right:
			__2004.ContentAlignment = eContentAlignment.Left;
			__2004.ContentVerticalAlignment = eContentVerticalAlignment.Top;
			__2004.BlockLineAlignment = eContentVerticalAlignment.Top;
			__2004.ContentOrientation = eContentOrientation.Vertical;
			break;
		}
		switch (_ὗ.TabLayoutType)
		{
		case eSuperTabLayoutType.SingleLine:
			__2004.MultiLine = false;
			__2004.FitContainer = false;
			break;
		case eSuperTabLayoutType.SingleLineFit:
			__2004.MultiLine = false;
			__2004.FitContainer = true;
			break;
		case eSuperTabLayoutType.MultiLine:
			__2004.MultiLine = true;
			__2004.FitContainer = false;
			break;
		case eSuperTabLayoutType.MultiLineFit:
			__2004.MultiLine = true;
			__2004.FitContainer = true;
			break;
		}
	}

	protected virtual void RecalcLayout(Rectangle r)
	{
		if (ᇃ.Count > 0)
		{
			IBlock[] array = new IBlock[ᇃ.Count];
			for (int i = 0; i < ᇃ.Count; i++)
			{
				array[i] = ᇃ[i];
			}
			if (ᓛ)
			{
				Rectangle rectangle = r;
				if (_ὗ.ControlBox.Visible)
				{
					rectangle.X += ῼ.Size.Width;
				}
				ᓚ = __2004.Layout(rectangle, array, __2005);
				RecalcSizeTabControlBox(r, rectangle);
			}
			else
			{
				ᓚ = __2004.Layout(r, array, __2005);
				RecalcSizeTabControlBox(r, ᓚ);
			}
		}
		else
		{
			ᓚ = Rectangle.Empty;
		}
	}

	protected virtual void RecalcSizeTabControlBox(Rectangle dispRect, Rectangle tabsBounds)
	{
		if (_ὗ.ControlBox.Visible)
		{
			Size size = ῼ.Size;
			int x;
			int y;
			switch (ῳ)
			{
			case eTabStripAlignment.Left:
			case eTabStripAlignment.Right:
				x = dispRect.X + (dispRect.Width - size.Width) / 2;
				y = _ὗ.DisplayRectangle.Bottom - size.Height;
				break;
			default:
				x = ((!ᓛ) ? (_ὗ.DisplayRectangle.Right - size.Width) : dispRect.X);
				y = tabsBounds.Y + (dispRect.Height - size.Height) / 2;
				break;
			}
			ῼ.Bounds = new Rectangle(x, y, size.Width, size.Height);
		}
	}

	protected virtual Rectangle GetTabClientArea()
	{
		return GetTabClientArea(ῼ.MenuBox.Visible);
	}

	protected virtual Rectangle GetTabClientArea(bool mbVisible)
	{
		Rectangle result = default(Rectangle);
		result.Size = ((Control)_1FD4.Ὁ).Size;
		if (_1FD4.ControlBox.Visible)
		{
			if (ῳ == eTabStripAlignment.Right || ῳ == eTabStripAlignment.Left)
			{
				result.Height -= _1FD4.ControlBox.Size.Height;
				if (_1FD4.ControlBox.MenuBox.Visible != mbVisible)
				{
					if (mbVisible)
					{
						result.Height -= _1FD4.ControlBox.MenuBox.HeightInternal;
					}
					else
					{
						result.Height += _1FD4.ControlBox.MenuBox.HeightInternal;
					}
				}
				if (result.Height < _1FD4.ControlBox.Size.Height)
				{
					result.Height = _1FD4.ControlBox.Size.Height;
				}
			}
			else
			{
				result.Width -= _1FD4.ControlBox.Size.Width;
				if (_1FD4.ControlBox.MenuBox.Visible != mbVisible)
				{
					if (mbVisible)
					{
						result.Width -= _1FD4.ControlBox.MenuBox.WidthInternal;
					}
					else
					{
						result.Width += _1FD4.ControlBox.MenuBox.WidthInternal;
					}
				}
				if (result.Width < _1FD4.ControlBox.Size.Width)
				{
					result.Width = _1FD4.ControlBox.Size.Width;
				}
			}
		}
		return result;
	}

	protected virtual int GetTabHeight()
	{
		if (Ὸ.Height > 0)
		{
			return Dpi.Height(Ὸ.Height);
		}
		int num = ((ῳ == eTabStripAlignment.Left || ῳ == eTabStripAlignment.Right) ? Dpi.Height20 : Dpi.Height16);
		Font val = (_ὗ.SelectedTabFont ?? _ὗ.TabFont) ?? ((Control)Ὁ).Font;
		if (val.Height > num)
		{
			num = val.Height;
		}
		return _2011(num) + Dpi.Height4;
	}

	private int _2011(int _0970)
	{
		int num = 0;
		foreach (BaseItem item in ᇃ)
		{
			if (!(item is SuperTabItem superTabItem))
			{
				continue;
			}
			ᇎ ᇎ2 = superTabItem.ᕂ();
			if (ᇎ2 != null)
			{
				int num2 = Dpi.ImageHeight(ᇎ2._0711);
				switch (superTabItem.ὅ)
				{
				case ImageAlignment.TopLeft:
				case ImageAlignment.TopCenter:
				case ImageAlignment.TopRight:
				case ImageAlignment.BottomLeft:
				case ImageAlignment.BottomCenter:
				case ImageAlignment.BottomRight:
					num2 += _0970;
					break;
				}
				if (num2 > num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	internal virtual void _2012(ref Size ߒ)
	{
		ߒ.Width += Dpi.Width(_ὗ.TabHorizontalSpacing) * 2;
		ߒ.Height += Dpi.Height(_ὗ.TabVerticalSpacing) * 2;
	}

	protected virtual void NextBlockPosition(LayoutManagerPositionEventArgs e)
	{
	}

	internal virtual Rectangle _2013(BaseItem _06EC, BaseItem _2014)
	{
		Rectangle bounds = _06EC.Bounds;
		if (ᓙ)
		{
			bounds.X = _2014.Bounds.X;
			bounds.Y += _06EC.Bounds.Height;
		}
		else
		{
			bounds.X += _06EC.Bounds.Width;
			bounds.Y = _2014.Bounds.Y;
		}
		bounds.Size = _2014.Size;
		return bounds;
	}

	private bool _2015()
	{
		foreach (BaseItem item in ᇃ)
		{
			if (item.Visible && !item.Displayed)
			{
				return false;
			}
		}
		return true;
	}

	public void _139A(ItemPaintArgs _065C)
	{
		Rectangle displayRectangle = _ὗ.DisplayRectangle;
		if (displayRectangle.Width <= 0 || displayRectangle.Height <= 0)
		{
			return;
		}
		using SuperTabColorTable superTabColorTable = ጧ();
		DrawBackground(_065C, superTabColorTable);
		_ὗ.Ά(_065C.Graphics, superTabColorTable);
		if (ᇃ.Count > 0)
		{
			if (_ὗ.ControlBox.Visible)
			{
				ῼ.Paint(_065C);
			}
			if (_1FFD)
			{
				DrawStripBorder(_065C, superTabColorTable);
			}
			DrawTabs(_065C);
			_2016(_065C, superTabColorTable);
			if (!_1FFD)
			{
				DrawStripBorder(_065C, superTabColorTable);
			}
		}
	}

	protected virtual void DrawBackground(ItemPaintArgs p, SuperTabColorTable ct)
	{
		Graphics graphics = p.Graphics;
		Rectangle displayRectangle = _ὗ.DisplayRectangle;
		int num = ct.Background.GradientAngle;
		switch (_ὗ.TabAlignment)
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
		for (int num = ᇃ.Count - 1; num >= 0; num--)
		{
			BaseItem baseItem = ᇃ[num];
			if (baseItem != null && baseItem.Displayed && p.ClipRectangle.IntersectsWith(baseItem.DisplayRectangle) && !(baseItem is SuperTabItem { IsSelected: not false }))
			{
				baseItem.Paint(p);
			}
		}
		if (Ὼ != null && Ὼ.Displayed)
		{
			Ὼ.Paint(p);
		}
		for (int num2 = ᇃ.Count - 1; num2 >= 0; num2--)
		{
			BaseItem baseItem2 = ᇃ[num2];
			if (baseItem2 != null && baseItem2.Displayed)
			{
				baseItem2.DrawInsertMarker(p.Graphics);
			}
		}
	}

	private void _2016(ItemPaintArgs _065C, SuperTabColorTable ዅ)
	{
		if (_ὗ.ὰ == null)
		{
			return;
		}
		if (__200E != ዅ.InsertMarker)
		{
			if (__200C != null)
			{
				((Image)__200C).Dispose();
				__200C = null;
			}
			if (__200D != null)
			{
				((Image)__200D).Dispose();
				__200D = null;
			}
			__200E = ዅ.InsertMarker;
		}
		switch (_ὗ.TabAlignment)
		{
		case eTabStripAlignment.Top:
		case eTabStripAlignment.Bottom:
			_2017(_065C.Graphics);
			break;
		case eTabStripAlignment.Left:
		case eTabStripAlignment.Right:
			_201B(_065C.Graphics);
			break;
		}
	}

	private void _2017(Graphics _٥)
	{
		Rectangle rectangle = _2018();
		Bitmap val = _2019(_٥);
		_٥.DrawImageUnscaled((Image)(object)val, rectangle);
	}

	private Rectangle _2018()
	{
		BaseItem ὰ = _ὗ.ὰ;
		ῲ έ = _ὗ.έ;
		int num = ὰ.Bounds.X;
		int num2 = ὰ.Bounds.Right;
		int num3 = ᇃ.IndexOf(ὰ);
		if (num3 > 0)
		{
			int num4 = _ὗ.ᾭ(ᇃ, num3 - 1);
			if (num4 >= 0 && _201F(ὰ, ᇃ[num4]))
			{
				if (έ._136F > 0)
				{
					if (έ._2000)
					{
						num = ᇃ[num4].Bounds.Right;
					}
				}
				else
				{
					num = (ὰ.Bounds.X + ᇃ[num4].Bounds.Right) / 2;
				}
			}
		}
		if (num3 + 1 < ᇃ.Count)
		{
			int num5 = _ὗ.ᾬ(ᇃ, num3 + 1);
			if (num5 >= 0 && _201F(ὰ, ᇃ[num5]))
			{
				if (έ._136F > 0)
				{
					if (!έ._2000)
					{
						num2 = ᇃ[num5].Bounds.X;
					}
				}
				else
				{
					num2 = (ὰ.Bounds.Right + ᇃ[num5].Bounds.X) / 2;
				}
			}
		}
		Rectangle bounds = ὰ.Bounds;
		bounds.X = num;
		bounds.Width = num2 - num;
		bounds.Height = 10;
		if (_ὗ.ড)
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
			if (bounds.X + 10 > _ὗ.Bounds.Right)
			{
				bounds.X = _ὗ.Bounds.Right - 10;
			}
		}
		bounds.Width = 10;
		return bounds;
	}

	private Bitmap _2019(Graphics _٥)
	{
		if (__200C == null)
		{
			__200C = _201A(_٥);
		}
		return __200C;
	}

	private Bitmap _201A(Graphics _٥)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		Bitmap val = new Bitmap(10, 10, _٥);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.SmoothingMode = _٥.SmoothingMode;
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
				Color color = Color.FromArgb(200, __200E);
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

	private void _201B(Graphics _٥)
	{
		Rectangle rectangle = _201C();
		Bitmap val = _201D(_٥);
		_٥.DrawImageUnscaled((Image)(object)val, rectangle);
	}

	private Rectangle _201C()
	{
		BaseItem ὰ = _ὗ.ὰ;
		ῲ έ = _ὗ.έ;
		int num = ὰ.Bounds.Y;
		int num2 = ὰ.Bounds.Bottom;
		int num3 = ᇃ.IndexOf(ὰ);
		if (num3 > 0 && _201F(ὰ, ᇃ[num3 - 1]))
		{
			if (έ._136F > 0)
			{
				if (έ._2000)
				{
					num = ᇃ[num3 - 1].Bounds.Bottom;
				}
			}
			else
			{
				num = (ὰ.Bounds.Y + ᇃ[num3 - 1].Bounds.Bottom) / 2;
			}
		}
		if (num3 + 1 < ᇃ.Count && _201F(ὰ, ᇃ[num3 + 1]))
		{
			if (έ._136F > 0)
			{
				if (!έ._2000)
				{
					num2 = ᇃ[num3 + 1].Bounds.Y;
				}
			}
			else
			{
				num2 = (ὰ.Bounds.Bottom + ᇃ[num3 + 1].Bounds.Y) / 2;
			}
		}
		Rectangle bounds = ὰ.Bounds;
		bounds.Y = num;
		bounds.Height = num2 - num;
		bounds.Width = 10;
		if (_ὗ.ড)
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
			if (bounds.Y + 10 > _ὗ.Bounds.Bottom)
			{
				bounds.Y = _ὗ.Bounds.Bottom - 10;
			}
		}
		bounds.Height = 10;
		return bounds;
	}

	private Bitmap _201D(Graphics _٥)
	{
		if (__200D == null)
		{
			__200D = _201E(_٥);
		}
		return __200D;
	}

	private Bitmap _201E(Graphics _٥)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		Bitmap val = new Bitmap(10, 10, _٥);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.SmoothingMode = _٥.SmoothingMode;
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
				Color color = Color.FromArgb(200, __200E);
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

	private bool _201F(BaseItem _2020, BaseItem _2021)
	{
		if (_ὗ.ὴ == 1)
		{
			return true;
		}
		return _ὗ.TabAlignment switch
		{
			eTabStripAlignment.Top => _2020.Bounds.Bottom == _2021.Bounds.Bottom, 
			eTabStripAlignment.Left => _2020.Bounds.Right == _2021.Bounds.Right, 
			eTabStripAlignment.Bottom => _2020.Bounds.Top == _2021.Bounds.Top, 
			_ => _2020.Bounds.Left == _2021.Bounds.Left, 
		};
	}

	protected virtual void DrawStripBorder(ItemPaintArgs p, SuperTabColorTable ct)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		SuperTabItem selectedTab = _1FD4.SelectedTab;
		Rectangle bounds = _1FD4.Bounds;
		Rectangle rectangle = selectedTab?.DisplayRectangle ?? Rectangle.Empty;
		Color color = ct.InnerBorder;
		Color color2 = ct.OuterBorder;
		if (selectedTab != null && ጦ.ፘ(selectedTab.PredefinedColor))
		{
			SuperTabLinearGradientColorTable superTabLinearGradientColorTable = ጦ.ፗ(selectedTab.PredefinedColor, new ColorFactory());
			color = ((superTabLinearGradientColorTable.Colors != null && superTabLinearGradientColorTable.Colors.Length > 0) ? superTabLinearGradientColorTable.Colors[superTabLinearGradientColorTable.Colors.Length - 1] : Color.DimGray);
			color2 = color;
		}
		Pen val = new Pen(color);
		try
		{
			Pen val2 = new Pen(color2);
			try
			{
				_2022(out var _2023, out var _2024);
				switch (_1FD4.TabAlignment)
				{
				case eTabStripAlignment.Top:
				case eTabStripAlignment.Bottom:
					if (rectangle.X > bounds.X)
					{
						graphics.DrawLine(val, bounds.X, _2023, rectangle.X + 1, _2023);
						graphics.DrawLine(val2, bounds.X, _2024, rectangle.X, _2024);
					}
					if (rectangle.Right < bounds.Right)
					{
						graphics.DrawLine(val, rectangle.Right - 2, _2023, bounds.Right - 1, _2023);
						graphics.DrawLine(val2, rectangle.Right, _2024, bounds.Right - 2, _2024);
					}
					graphics.DrawLine(val2, bounds.X, _2024, bounds.X, _2023);
					graphics.DrawLine(val2, bounds.Right - 1, _2024, bounds.Right - 1, _2023);
					break;
				case eTabStripAlignment.Left:
				case eTabStripAlignment.Right:
					if (rectangle.Y > 0)
					{
						graphics.DrawLine(val, _2023, bounds.Y, _2023, rectangle.Y + 1);
						graphics.DrawLine(val2, _2024, bounds.Y, _2024, rectangle.Y);
					}
					if (rectangle.Bottom < bounds.Bottom)
					{
						graphics.DrawLine(val, _2023, rectangle.Bottom - 2, _2023, bounds.Bottom - 1);
						graphics.DrawLine(val2, _2024, rectangle.Bottom - 1, _2024, bounds.Bottom - 1);
					}
					graphics.DrawLine(val2, _2023, bounds.Y, _2024, bounds.Y);
					graphics.DrawLine(val2, _2023, bounds.Bottom - 1, _2024, bounds.Bottom - 1);
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

	private void _2022(out int _2023, out int _2024)
	{
		Rectangle bounds = _1FD4.Bounds;
		switch (_1FD4.TabAlignment)
		{
		case eTabStripAlignment.Top:
			_2023 = bounds.Bottom - 1;
			_2024 = bounds.Bottom - 2;
			break;
		case eTabStripAlignment.Bottom:
			_2023 = bounds.Y;
			_2024 = bounds.Y + 1;
			break;
		case eTabStripAlignment.Left:
			_2023 = bounds.Right - 1;
			_2024 = bounds.Right - 2;
			break;
		default:
			_2023 = bounds.X;
			_2024 = bounds.X + 1;
			break;
		}
	}

	internal void _1FDF()
	{
		ῴ = ጦ.ጧ(Ὁ.TabStyle, ColorFactory.Empty);
	}

	internal virtual SuperTabColorTable ጧ()
	{
		SuperTabColorTable tabStripColor = _1FD4.TabStripColor;
		SuperTabColorTable superTabColorTable = (SuperTabColorTable)__2006.Clone();
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
			_2025(superTabColorTable.ControlBoxDefault, tabStripColor.ControlBoxDefault);
			_2025(superTabColorTable.ControlBoxMouseOver, tabStripColor.ControlBoxMouseOver);
			_2025(superTabColorTable.ControlBoxPressed, tabStripColor.ControlBoxPressed);
			if (tabStripColor.InsertMarker != Color.Empty)
			{
				superTabColorTable.InsertMarker = tabStripColor.InsertMarker;
			}
			if (tabStripColor.SelectionMarker != Color.Empty)
			{
				superTabColorTable.SelectionMarker = tabStripColor.SelectionMarker;
			}
		}
		if (Ὼ != null && ጦ.ፘ(Ὼ.PredefinedColor))
		{
			SuperTabItemStateColorTable superTabItemStateColorTable = Ὼ.ὢ(eTabState.Selected);
			if (superTabItemStateColorTable != null)
			{
				if (superTabItemStateColorTable.InnerBorder != Color.Empty)
				{
					superTabColorTable.InnerBorder = superTabItemStateColorTable.InnerBorder;
				}
				if (superTabItemStateColorTable.OuterBorder != Color.Empty)
				{
					superTabColorTable.OuterBorder = superTabItemStateColorTable.OuterBorder;
				}
			}
		}
		return superTabColorTable;
	}

	private void _2025(SuperTabControlBoxStateColorTable _2026, SuperTabControlBoxStateColorTable _2027)
	{
		if (_2027.Background != Color.Empty)
		{
			_2026.Background = _2027.Background;
		}
		if (_2027.Border != Color.Empty)
		{
			_2026.Border = _2027.Border;
		}
		if (_2027.Image != Color.Empty)
		{
			_2026.Image = _2027.Image;
		}
	}

	public Bitmap _2028(Graphics _٥, Color _065B)
	{
		if (_065B != __2008 && __2007 != null)
		{
			((Image)__2007).Dispose();
			__2007 = null;
		}
		__2008 = _065B;
		if (__2007 == null)
		{
			__2007 = CreateCloseButton(_٥);
		}
		return __2007;
	}

	public virtual Bitmap CreateCloseButton(Graphics g)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		Bitmap val = new Bitmap(Dpi.Width16, Dpi.Height16, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingQuality = (CompositingQuality)2;
			val2.SmoothingMode = g.SmoothingMode;
			Pen val3 = new Pen(__2008, (float)Dpi.Width2);
			try
			{
				val2.DrawLine(val3, new Point(Dpi.Width3, Dpi.Height3), new Point(Dpi.Width11, Dpi.Height11));
				val2.DrawLine(val3, new Point(Dpi.Width11, Dpi.Height3), new Point(Dpi.Width3, Dpi.Height11));
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

	public Bitmap _2029(Graphics _٥, Color _065B)
	{
		if (_065B != __200B)
		{
			if (__2009 != null)
			{
				((Image)__2009).Dispose();
				__2009 = null;
			}
			if (__200A != null)
			{
				((Image)__200A).Dispose();
				__200A = null;
			}
		}
		__200B = _065B;
		if (__2009 == null)
		{
			__2009 = CreateMenuButton1(_٥);
		}
		return __2009;
	}

	public virtual Bitmap CreateMenuButton1(Graphics g)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		Bitmap val = new Bitmap(Dpi.Width16, Dpi.Height16, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingQuality = (CompositingQuality)2;
			val2.SmoothingMode = g.SmoothingMode;
			GraphicsPath val3 = _202A();
			try
			{
				Brush val4 = (Brush)new SolidBrush(__200B);
				try
				{
					val2.FillPath(val4, val3);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				Pen val5 = new Pen(__200B);
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

	private GraphicsPath _202A()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddLines(new Point[3]
		{
			new Point(Dpi.Width4, Dpi.Height5),
			new Point(Dpi.Width8, Dpi.Height9),
			new Point(Dpi.Width12, Dpi.Height5)
		});
		val.CloseAllFigures();
		return val;
	}

	public Bitmap _202B(Graphics _٥, Color _065B)
	{
		if (_065B != __200B)
		{
			if (__2009 != null)
			{
				((Image)__2009).Dispose();
				__2009 = null;
			}
			if (__200A != null)
			{
				((Image)__200A).Dispose();
				__200A = null;
			}
		}
		__200B = _065B;
		if (__200A == null)
		{
			__200A = CreateMenuButton2(_٥);
		}
		return __200A;
	}

	public virtual Bitmap CreateMenuButton2(Graphics g)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		Bitmap val = new Bitmap(Dpi.Width16, Dpi.Height16, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingQuality = (CompositingQuality)2;
			val2.SmoothingMode = g.SmoothingMode;
			GraphicsPath val3 = _202C();
			try
			{
				Brush val4 = (Brush)new SolidBrush(__200B);
				try
				{
					val2.FillPath(val4, val3);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				Pen val5 = new Pen(__200B);
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

	private GraphicsPath _202C()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddLines(new Point[3]
		{
			new Point(Dpi.Width3, Dpi.Height4),
			new Point(Dpi.Width6, Dpi.Height7),
			new Point(Dpi.Width3, Dpi.Height11)
		});
		val.CloseFigure();
		val.AddLines(new Point[3]
		{
			new Point(Dpi.Width9, Dpi.Height4),
			new Point(Dpi.Width12, Dpi.Height7),
			new Point(Dpi.Width9, Dpi.Height11)
		});
		val.CloseFigure();
		return val;
	}
}
