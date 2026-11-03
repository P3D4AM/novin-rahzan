using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.AdvTree.Display;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Metro.Helpers;
using DevComponents.DotNetBar.Metro.Rendering;
using DevComponents.DotNetBar.Presentation;
using DevComponents.DotNetBar.Ribbon;
using DevComponents.DotNetBar.Schedule;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Rendering;

internal class _1A6E
{
	private static readonly Size _1A6F = new Size(16, 16);

	private static readonly Point _1A70 = new Point(3, 3);

	private static readonly Color _1A71 = ColorScheme.GetColor(15204352);

	private static readonly Color _1A72 = ColorScheme.GetColor(12910851);

	private static readonly Color ᒙ = Color.White;

	public static Rectangle _139A(Graphics _٥, Rectangle _1A73, eNotificationMarkPosition ᙒ, string _06EA)
	{
		return _139A(_٥, _1A73, ᙒ, _06EA, _1A6F, _1A70);
	}

	public static Rectangle _139A(Graphics _٥, Rectangle _1A73, eNotificationMarkPosition ᙒ, string _06EA, int _1A74)
	{
		return _139A(_٥, _1A73, ᙒ, _06EA, (_1A74 > 0) ? new Size(_1A74, _1A74) : _1A6F, _1A70);
	}

	public static Rectangle _139A(Graphics _٥, Rectangle _1A73, eNotificationMarkPosition ᙒ, string _06EA, int _1A74, Color _1A75)
	{
		if (_1A75.IsEmpty)
		{
			return _139A(_٥, _1A73, ᙒ, _06EA, (_1A74 > 0) ? new Size(_1A74, _1A74) : _1A6F, _1A70, _1A71, _1A72, ᒙ);
		}
		_1A76(_1A75, out var ጇ, out var ገ, out var ໆ);
		return _139A(_٥, _1A73, ᙒ, _06EA, (_1A74 > 0) ? new Size(_1A74, _1A74) : _1A6F, _1A70, ጇ, ገ, ໆ);
	}

	private static void _1A76(Color _1A75, out Color ጇ, out Color ገ, out Color ໆ)
	{
		_1A78 obj = _1A7B._1A7C(_1A75);
		ໆ = ((ColorFunctions.RGBToHSL(_1A75).Lightness < 0.65) ? Color.White : Color.Black);
		ጇ = _1A7B._1A7D(obj._1A79, obj._1A7A, obj._༡ + 0.10000000149011612);
		ገ = _1A7B._1A7D(obj._1A79, obj._1A7A, obj._༡ + 0.20000000298023224);
	}

	public static Rectangle _139A(Graphics _٥, Rectangle _1A73, eNotificationMarkPosition ᙒ, string _06EA, Size _1A74, Point _066A)
	{
		return _139A(_٥, _1A73, ᙒ, _06EA, _1A74, _066A, _1A71, _1A72, ᒙ);
	}

	public static Rectangle _139A(Graphics _٥, Rectangle _1A73, eNotificationMarkPosition ᙒ, string _06EA, Size _1A74, Point _066A, Color _1A75)
	{
		if (_1A75.IsEmpty)
		{
			return _139A(_٥, _1A73, ᙒ, _06EA, _1A74, _066A, _1A71, _1A72, ᒙ);
		}
		_1A76(_1A75, out var ጇ, out var ገ, out var ໆ);
		return _139A(_٥, _1A73, ᙒ, _06EA, _1A74, _066A, ጇ, ገ, ໆ);
	}

	public static Rectangle _139A(Graphics _٥, Rectangle _1A73, eNotificationMarkPosition ᙒ, string _06EA, Size _1A74, Point _066A, Color _1A75, Rectangle _1A77)
	{
		if (_1A75.IsEmpty)
		{
			return _139A(_٥, _1A73, ᙒ, _06EA, _1A74, _066A, _1A71, _1A72, ᒙ, _1A77);
		}
		_1A76(_1A75, out var ጇ, out var ገ, out var ໆ);
		return _139A(_٥, _1A73, ᙒ, _06EA, _1A74, _066A, ጇ, ገ, ໆ, _1A77);
	}

	public static Rectangle _139A(Graphics _٥, Rectangle _1A73, eNotificationMarkPosition ᙒ, string _06EA, Size _1A74, Point _066A, Color ጇ, Color ገ, Color ໆ)
	{
		return _139A(_٥, _1A73, ᙒ, _06EA, _1A74, _066A, ጇ, ገ, ໆ, Rectangle.Empty);
	}

	public static Rectangle _139A(Graphics _٥, Rectangle _1A73, eNotificationMarkPosition ᙒ, string _06EA, Size _1A74, Point _066A, Color ጇ, Color ገ, Color ໆ, Rectangle _1A77)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Expected O, but got Unknown
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Expected O, but got Unknown
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Expected O, but got Unknown
		if (_1A74.IsEmpty)
		{
			_1A74 = _1A6F;
		}
		Color white = Color.White;
		int width = Dpi.Width2;
		_1A74 = Dpi.Size(_1A74);
		_066A = Dpi.Size(_066A);
		Font val = new Font("Microsoft Sans Serif", (_06EA.Length > 1) ? 6.75f : 8.25f, (FontStyle)1);
		Rectangle rectangle = ᙒ switch
		{
			eNotificationMarkPosition.TopRight => new Rectangle(_1A73.Right - _1A74.Width - 1 + _066A.X, _1A73.Y - _066A.Y, _1A74.Width, _1A74.Height), 
			eNotificationMarkPosition.TopLeft => new Rectangle(_1A73.X - _066A.X, _1A73.Y - _066A.Y, _1A74.Width, _1A74.Height), 
			eNotificationMarkPosition.BottomLeft => new Rectangle(_1A73.X - _066A.X, _1A73.Bottom - _1A74.Height - 1 + _066A.Y, _1A74.Width, _1A74.Height), 
			_ => new Rectangle(_1A73.Right - _1A74.Width - 1 + _066A.X, _1A73.Bottom - _1A74.Height + _066A.Y, _1A74.Width, _1A74.Height), 
		};
		SmoothingMode smoothingMode = _٥.SmoothingMode;
		TextRenderingHint textRenderingHint = _٥.TextRenderingHint;
		_٥.SmoothingMode = (SmoothingMode)2;
		_٥.TextRenderingHint = (TextRenderingHint)5;
		Region clip = _٥.Clip;
		if (_1A77.IsEmpty)
		{
			_٥.ResetClip();
		}
		else
		{
			_٥.SetClip(_1A77);
		}
		Rectangle rectangle2 = rectangle;
		rectangle2.Inflate(1, 1);
		rectangle2.Offset(0, 3);
		GraphicsPath val2 = new GraphicsPath();
		try
		{
			val2.AddEllipse(rectangle2);
			PathGradientBrush val3 = new PathGradientBrush(val2);
			try
			{
				val3.CenterColor = Color.FromArgb(148, Color.Black);
				val3.SurroundColors = new Color[1] { Color.FromArgb(4, Color.Black) };
				_٥.FillPath((Brush)(object)val3, val2);
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
		LinearGradientBrush val4 = new LinearGradientBrush(rectangle, ጇ, ገ, 90f);
		try
		{
			_٥.FillEllipse((Brush)(object)val4, rectangle);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		Rectangle rectangle3 = new Rectangle(rectangle.X - (int)((double)rectangle.Width * 0.15), rectangle.Y - (int)((double)rectangle.Height * 0.15), (int)((double)rectangle.Width * 1.3), (int)((double)rectangle.Height * 1.32) / 2);
		LinearGradientBrush val5 = new LinearGradientBrush(rectangle3, Color.FromArgb(128, Color.White), Color.FromArgb(8, Color.White), 90f);
		try
		{
			GraphicsPath val6 = new GraphicsPath();
			val6.AddEllipse(rectangle);
			if (_1A77.IsEmpty)
			{
				_٥.SetClip(val6);
			}
			else
			{
				_٥.SetClip(val6, (CombineMode)1);
			}
			_٥.FillEllipse((Brush)(object)val5, rectangle3);
			if (_1A77.IsEmpty)
			{
				_٥.ResetClip();
			}
			else
			{
				_٥.SetClip(_1A77);
			}
			val6.Dispose();
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		Pen val7 = new Pen(white, (float)width);
		try
		{
			_٥.DrawEllipse(val7, rectangle);
		}
		finally
		{
			((IDisposable)val7)?.Dispose();
		}
		if (!string.IsNullOrEmpty(_06EA))
		{
			Rectangle rectangle4 = rectangle;
			if (_06EA.Length == 1)
			{
				rectangle4.Y++;
			}
			StringFormat val8 = (StringFormat)StringFormat.GenericDefault.Clone();
			try
			{
				val8.LineAlignment = (StringAlignment)1;
				val8.Alignment = (StringAlignment)1;
				SolidBrush val9 = new SolidBrush(ໆ);
				try
				{
					_٥.DrawString(_06EA, val, (Brush)(object)val9, (RectangleF)rectangle4, val8);
				}
				finally
				{
					((IDisposable)val9)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
		_٥.Clip = clip;
		clip.Dispose();
		val.Dispose();
		_٥.SmoothingMode = smoothingMode;
		_٥.TextRenderingHint = textRenderingHint;
		return rectangle;
	}
}
internal abstract class _20A8
{
	public abstract void Paint(QatOverflowItemRendererEventArgs e);
}
internal class _20A9 : _20A8, ᙓ
{
	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void Paint(QatOverflowItemRendererEventArgs e)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		QatOverflowItem overflowItem = e.OverflowItem;
		Rectangle displayRectangle = overflowItem.DisplayRectangle;
		Region val = null;
		if (graphics.Clip != null)
		{
			val = graphics.Clip.Clone();
		}
		graphics.SetClip(overflowItem.DisplayRectangle, (CombineMode)1);
		Office2007ButtonItemColorTable colorTable = GetColorTable();
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = colorTable.Default;
		if (overflowItem.Expanded)
		{
			office2007ButtonItemStateColorTable = colorTable.Expanded;
		}
		else if (overflowItem.IsMouseOver)
		{
			office2007ButtonItemStateColorTable = colorTable.MouseOver;
		}
		ᯊ.ᯓ(graphics, office2007ButtonItemStateColorTable, displayRectangle, RoundRectangleShapeDescriptor.RoundCorner2);
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)0;
		Color expandBackground = office2007ButtonItemStateColorTable.ExpandBackground;
		Color expandLight = office2007ButtonItemStateColorTable.ExpandLight;
		ᙝ ᙝ = new ᙝ();
		ᙔ border = new ᙔ(expandLight, 1);
		ᙔ border2 = new ᙔ(expandBackground, 1);
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(0, 0), new ᙞ(2, 2), border));
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(2, 2), new ᙞ(0, 4), border));
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(0, 1), new ᙞ(1, 2), border2));
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(1, 2), new ᙞ(0, 3), border2));
		ᙝ.ᙠ._0CCB(new _0CF3(new ᙞ(0, 3), new ᙞ(0, 1), border2));
		Rectangle rectangle = new Rectangle(displayRectangle.X + (displayRectangle.Width - 5) / 2, displayRectangle.Y + (displayRectangle.Height - 4) / 2, 3, 6);
		ᙰ ᙰ = new ᙰ(graphics, rectangle);
		ᙝ.Paint(ᙰ);
		rectangle.Offset(4, 0);
		ᙰ.ঋ = rectangle;
		ᙝ.Paint(ᙰ);
		graphics.SmoothingMode = smoothingMode;
		if (val != null)
		{
			graphics.Clip = val;
		}
		else
		{
			graphics.ResetClip();
		}
	}

	protected virtual Office2007ButtonItemColorTable GetColorTable()
	{
		Office2007ColorTable office2007ColorTable = ᘽ;
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = null;
		eButtonColor eButtonColor = eButtonColor.Orange;
		office2007ButtonItemColorTable = office2007ColorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor)];
		if (office2007ButtonItemColorTable == null)
		{
			return office2007ColorTable.ButtonItemColors[0];
		}
		return office2007ButtonItemColorTable;
	}
}
internal static class _21B1
{
	public static void _21B2(Office2007ColorTable ቯ, ColorFactory ቖ)
	{
		ቯ.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010NormalSilver.png");
		ቯ.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010HotSilver.png");
		ቯ.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010PressedSilver.png");
		int color = 8691133;
		ቯ.RibbonControl.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7507629), ቖ.GetColor(color));
		ቯ.RibbonControl.InnerBorder = null;
		ቯ.RibbonControl.TabsBackground = new LinearGradientColorTable(ቖ.GetColor(12308198));
		ቯ.RibbonControl.TabsGlassBackground = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(16448250));
		ቯ.RibbonControl.TabDividerBorder = Color.Empty;
		ቯ.RibbonControl.TabDividerBorderLight = Color.Empty;
		ቯ.RibbonControl.CornerSize = 1;
		ቯ.RibbonControl.PanelTopBackgroundHeight = 0f;
		ቯ.RibbonControl.PanelTopBackground = new LinearGradientColorTable(ቖ.GetColor(15726333), ቖ.GetColor(14214386));
		ቯ.RibbonControl.PanelBottomBackground = null;
		ቯ.ItemGroup.OuterBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.ItemGroupDividerDark = Color.Empty;
		ቯ.ItemGroup.ItemGroupDividerLight = Color.Empty;
		ቯ.RibbonBar.Default = ᱬ(ቖ);
		ቯ.RibbonBar.MouseOver = _21B3(ቖ);
		ቯ.RibbonBar.Expanded = _21B4(ቖ);
		ቯ.RibbonButtonItemColors.Clear();
		ቯ.ButtonItemColors.Clear();
		ቯ.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _12C7(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ወ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዉ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዊ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዋ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዌ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ው(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		ቯ.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(ቖ));
		ቯ.BackstageButtonItemColors.Add(_21BA(ቖ));
		ቯ.NavigationPaneButtonItemColors.Clear();
		ቯ.NavigationPaneButtonItemColors.Add(_21B5(ቖ));
		ቯ.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = ዑ(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዒ(ቖ);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዓ(ቖ);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዔ(ቖ);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		ቯ.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = _21B6(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B7(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B8(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B9(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		ቯ.Bar.ToolbarTopBackground = new LinearGradientColorTable(ቖ.GetColor(14477558), ቖ.GetColor(13162216));
		ቯ.Bar.ToolbarBottomBackground = new LinearGradientColorTable(ቖ.GetColor(13162216), ቖ.GetColor(11781336));
		ቯ.Bar.ToolbarBottomBorder = ቖ.GetColor(color);
		ቯ.Bar.PopupToolbarBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.Bar.PopupToolbarBorder = ቖ.GetColor(10988464);
		ቯ.Bar.StatusBarTopBorder = ቖ.GetColor(color);
		ቯ.Bar.StatusBarTopBorderLight = ቖ.GetColor(Color.FromArgb(128, Color.White));
		ቯ.Bar.StatusBarAltBackground.Clear();
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(14477558), 0f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(11781336), 1f));
		ቯ.Menu.Background = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.Menu.Border = new LinearGradientColorTable(ቖ.GetColor(10988464), Color.Empty);
		ቯ.Menu.Side = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.Menu.SideBorder = new LinearGradientColorTable(ቖ.GetColor(14869735), Color.Empty);
		ቯ.Menu.SideBorderLight = new LinearGradientColorTable(ቖ.GetColor(16119285), Color.Empty);
		ቯ.Menu.SideUnused = new LinearGradientColorTable(ቖ.GetColor(15066597), Color.Empty);
		ቯ.Menu.FileBackgroundBlend.Clear();
		ቯ.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(16777215), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16777215), 1f)
		});
		ቯ.Menu.FileContainerBorder = ቖ.GetColor(10988464);
		ቯ.Menu.FileContainerBorderLight = Color.Transparent;
		ቯ.Menu.FileColumnOneBackground = ቖ.GetColor(16777215);
		ቯ.Menu.FileColumnOneBorder = ቖ.GetColor(10988464);
		ቯ.Menu.FileColumnTwoBackground = ቖ.GetColor(16777215);
		ቯ.Menu.FileBottomContainerBackgroundBlend.Clear();
		ቯ.ComboBox.Default.Background = ቖ.GetColor(15595005);
		ቯ.ComboBox.Default.Border = ቖ.GetColor(11649238);
		ቯ.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandText = ቖ.GetColor(5922145);
		ቯ.ComboBox.DefaultStandalone.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.DefaultStandalone.Border = ቖ.GetColor(color);
		ቯ.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(14806522), ቖ.GetColor(13688814), 90);
		ቯ.ComboBox.DefaultStandalone.ExpandBorderInner = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(11254480), Color.Empty, 90);
		ቯ.ComboBox.DefaultStandalone.ExpandText = ቖ.GetColor(5922145);
		ቯ.ComboBox.MouseOver.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.MouseOver.Border = ቖ.GetColor(color);
		ቯ.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(16376199), ቖ.GetColor(16446415), 90);
		ቯ.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(16578789), ቖ.GetColor(16645359), 90);
		ቯ.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(15648856), Color.Empty, 90);
		ቯ.ComboBox.MouseOver.ExpandText = ቖ.GetColor(5922145);
		ቯ.ComboBox.DroppedDown.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.DroppedDown.Border = ቖ.GetColor(color);
		ቯ.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(16438647), ቖ.GetColor(16770186), 90);
		ቯ.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(15975522), ቖ.GetColor(16373627), 90);
		ቯ.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(12744491), ቖ.GetColor(12744491), 90);
		ቯ.ComboBox.DroppedDown.ExpandText = ቖ.GetColor(5922145);
		ቯ.DialogLauncher.Default.DialogLauncher = ቖ.GetColor(8752021);
		ቯ.DialogLauncher.Default.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.MouseOver.DialogLauncher = ቖ.GetColor(8752021);
		ቯ.DialogLauncher.MouseOver.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16641758), ቖ.GetColor(16638141));
		ቯ.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16765551), ቖ.GetColor(16774553));
		ቯ.DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16710131), ቖ.GetColor(16777203));
		ቯ.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(16758528), Color.Empty);
		ቯ.DialogLauncher.Pressed.DialogLauncher = ቖ.GetColor(8752021);
		ቯ.DialogLauncher.Pressed.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14993573), ቖ.GetColor(15583377));
		ቯ.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15645794), ቖ.GetColor(15656075));
		ቯ.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13942696), ቖ.GetColor(15724994));
		ቯ.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12751419), ቖ.GetColor(12760162));
		ቯ.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(15397110));
		ቯ.SystemButton.Default.LightShade = Color.Empty;
		ቯ.SystemButton.Default.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(15594233));
		ቯ.SystemButton.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButton.MouseOver.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13953279), ቖ.GetColor(13361142));
		ቯ.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13361142), ቖ.GetColor(12571370));
		ቯ.SystemButton.MouseOver.TopHighlight = null;
		ቯ.SystemButton.MouseOver.BottomHighlight = null;
		ቯ.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9414079));
		ቯ.SystemButton.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(15331318));
		ቯ.SystemButton.Pressed.LightShade = Color.Empty;
		ቯ.SystemButton.Pressed.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(12045026));
		ቯ.SystemButton.Pressed.TopHighlight = null;
		ቯ.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(12045026));
		ቯ.SystemButton.Pressed.BottomHighlight = null;
		ቯ.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9414079));
		ቯ.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(11387354));
		ቯ.SystemButtonClose = new Office2007SystemButtonColorTable();
		ቯ.SystemButtonClose.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(15397110));
		ቯ.SystemButtonClose.Default.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Default.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButtonClose.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(16570327));
		ቯ.SystemButtonClose.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver.DarkShade = ቖ.GetColor(10173757);
		ቯ.SystemButtonClose.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16745602), ቖ.GetColor(16482174));
		ቯ.SystemButtonClose.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16152953), ቖ.GetColor(14901602));
		ቯ.SystemButtonClose.MouseOver.TopHighlight = null;
		ቯ.SystemButtonClose.MouseOver.BottomHighlight = null;
		ቯ.SystemButtonClose.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10173757));
		ቯ.SystemButtonClose.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.SystemButtonClose.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(16307413));
		ቯ.SystemButtonClose.Pressed.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Pressed.DarkShade = ቖ.GetColor(10173757);
		ቯ.SystemButtonClose.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15890294));
		ቯ.SystemButtonClose.Pressed.TopHighlight = null;
		ቯ.SystemButtonClose.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15890294));
		ቯ.SystemButtonClose.Pressed.BottomHighlight = null;
		ቯ.SystemButtonClose.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10173757));
		ቯ.SystemButtonClose.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14311779));
		ቯ.Form.Active.BorderColors = new Color[5]
		{
			ቖ.GetColor(9476774),
			ቖ.GetColor(13952757),
			ቖ.GetColor(12308198),
			ቖ.GetColor(12308198),
			ቖ.GetColor(12308198)
		};
		ቯ.Form.Inactive.BorderColors = new Color[5]
		{
			ቖ.GetColor(10661305),
			ቖ.GetColor(12308198),
			ቖ.GetColor(12308198),
			ቖ.GetColor(12308198),
			ቖ.GetColor(12308198)
		};
		ቯ.Form.Active.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(12702956), ቖ.GetColor(12637162));
		ቯ.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(12637162), ቖ.GetColor(12308198));
		ቯ.Form.Active.CaptionBottomBorder = new Color[2]
		{
			ቖ.GetColor(9476774),
			ቖ.GetColor(13952757)
		};
		ቯ.Form.Active.CaptionText = ቖ.GetColor(2046325);
		ቯ.Form.Active.CaptionTextExtra = ቖ.GetColor(2046325);
		ቯ.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(14674935));
		ቯ.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(14674935));
		ቯ.Form.Inactive.CaptionText = ቖ.GetColor(6979752);
		ቯ.Form.Inactive.CaptionTextExtra = ቖ.GetColor(6979752);
		ቯ.Form.BackColor = ቖ.GetColor(13622766);
		ቯ.Form.TextColor = ቖ.GetColor(1980763);
		ቯ.Form.MdiClientBackgroundImage = (Image)(object)BarFunctions.ই("SystemImages.Office2010BlueClientBackground.png");
		ቯ.QuickAccessToolbar.Active.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		ቯ.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13492213));
		ቯ.QuickAccessToolbar.Standalone.OutterBorderColor = ቖ.GetColor(9479873);
		ቯ.QuickAccessToolbar.Standalone.MiddleBorderColor = ቖ.GetColor(14018814);
		ቯ.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelBackground = ቖ.GetColor(15790837);
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelText = ቖ.GetColor(1980763);
		ቯ.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13163764), ቖ.GetColor(11522814));
		ቯ.TabControl.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(9879539), ቖ.GetColor(11914473));
		ቯ.TabControl.Default.InnerBorder = ቖ.GetColor(14938111);
		ቯ.TabControl.Default.OuterBorder = ቖ.GetColor(color);
		ቯ.TabControl.Default.Text = ቖ.GetColor(3953784);
		ቯ.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14740991), ቖ.GetColor(14149887));
		ቯ.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11588351), ቖ.GetColor(12507903));
		ቯ.TabControl.MouseOver.InnerBorder = ቖ.GetColor(16777215);
		ቯ.TabControl.MouseOver.OuterBorder = ቖ.GetColor(6655167);
		ቯ.TabControl.MouseOver.Text = ቖ.GetColor(3953784);
		ቯ.TabControl.Selected = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15459289), ቖ.GetColor(16629108));
		ቯ.TabControl.Selected.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16364633), ቖ.GetColor(16777215));
		ቯ.TabControl.Selected.InnerBorder = ቖ.GetColor(13481628);
		ቯ.TabControl.Selected.OuterBorder = ቖ.GetColor(color);
		ቯ.TabControl.Selected.Text = ቖ.GetColor(1980763);
		ቯ.TabControl.TabBackground = new LinearGradientColorTable(ቖ.GetColor(13622766), Color.Empty);
		ቯ.TabControl.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.TabControl.TabPanelBorder = ቖ.GetColor(color);
		Office2007CheckBoxColorTable checkBoxItem = ቯ.CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16053492), Color.Empty);
		checkBoxItem.Default.CheckBorder = ቖ.GetColor(11256286);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(15264235), ቖ.GetColor(15921907));
		checkBoxItem.Default.CheckInnerBorder = ቖ.GetColor(12502220);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4877206), Color.Empty);
		checkBoxItem.Default.Text = ቖ.GetColor(1980763);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16576962), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = ቖ.GetColor(13602871);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(16444616), ቖ.GetColor(16643812));
		checkBoxItem.MouseOver.CheckInnerBorder = ቖ.GetColor(16559453);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4877206), Color.Empty);
		checkBoxItem.MouseOver.Text = ቖ.GetColor(1980763);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16574397));
		checkBoxItem.Pressed.CheckBorder = ቖ.GetColor(12741917);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(16441506), ቖ.GetColor(16642775));
		checkBoxItem.Pressed.CheckInnerBorder = ቖ.GetColor(16557400);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4877206), Color.Empty);
		checkBoxItem.Pressed.Text = ቖ.GetColor(1980763);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = ቖ.GetColor(11974842);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(16316665));
		checkBoxItem.Disabled.CheckInnerBorder = ቖ.GetColor(14672096);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(ቖ.GetColor(11645876), Color.Empty);
		checkBoxItem.Disabled.Text = ቖ.GetColor(8884895);
		ዱ(ቯ, ቖ);
		ዳ(ቯ, ቖ);
		Office2007ProgressBarColorTable progressBarItem = ቯ.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(7247820);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
		progressBarItem.Chunk = new GradientColorTable(6918698, 15200980, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(15663063)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(9286228)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(6918699)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = ቯ.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(7247820);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
		progressBarItem.Chunk = new GradientColorTable(11446016, 16776653, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16776099)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13814272)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16708608)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = ቯ.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(7247820);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
		progressBarItem.Chunk = new GradientColorTable(13762560, 16764365, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16748431)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13762560)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16646144)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		Office2007GalleryColorTable gallery = ቯ.Gallery;
		gallery.GroupLabelBackground = ቖ.GetColor(15790837);
		gallery.GroupLabelText = ቖ.GetColor(1980763);
		gallery.GroupLabelBorder = ቖ.GetColor(15790837);
		ቯ.LegacyColors.BarBackground = ቖ.GetColor(14477558);
		ቯ.LegacyColors.BarBackground2 = ቖ.GetColor(11781336);
		ቯ.LegacyColors.BarStripeColor = ቖ.GetColor(7900581);
		ቯ.LegacyColors.BarCaptionBackground = ቖ.GetColor(10268097);
		ቯ.LegacyColors.BarCaptionBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionText = ቖ.GetColor(2046317);
		ቯ.LegacyColors.BarCaptionInactiveBackground = ቖ.GetColor(11650009);
		ቯ.LegacyColors.BarCaptionInactiveBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveText = ቖ.GetColor(2046322);
		ቯ.LegacyColors.BarFloatingBorder = ቖ.GetColor(color);
		ቯ.LegacyColors.BarPopupBackground = ቖ.GetColor(16777215);
		ቯ.LegacyColors.BarPopupBorder = ቖ.GetColor(10988464);
		ቯ.LegacyColors.ItemBackground = Color.Empty;
		ቯ.LegacyColors.ItemBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBackground = ቖ.GetColor(16770421);
		ቯ.LegacyColors.ItemCheckedBackground2 = ቖ.GetColor(16771445);
		ቯ.LegacyColors.ItemCheckedBorder = ቖ.GetColor(12749360);
		ቯ.LegacyColors.ItemCheckedText = ቖ.GetColor(2046328);
		ቯ.LegacyColors.ItemDisabledBackground = Color.Empty;
		ቯ.LegacyColors.ItemDisabledText = ቖ.GetColor(9276813);
		ቯ.LegacyColors.ItemExpandedShadow = Color.Empty;
		ቯ.LegacyColors.ItemExpandedBackground = ቖ.GetColor(16770421);
		ቯ.LegacyColors.ItemExpandedBackground2 = ቖ.GetColor(16768110);
		ቯ.LegacyColors.ItemExpandedText = ቖ.GetColor(2046328);
		ቯ.LegacyColors.ItemHotBackground = ቖ.GetColor(16310663);
		ቯ.LegacyColors.ItemHotBackground2 = ቖ.GetColor(16314828);
		ቯ.LegacyColors.ItemHotBorder = ቖ.GetColor(15848521);
		ቯ.LegacyColors.ItemHotText = ቖ.GetColor(2046328);
		ቯ.LegacyColors.ItemPressedBackground = ቖ.GetColor(16173159);
		ቯ.LegacyColors.ItemPressedBackground2 = ቖ.GetColor(16704135);
		ቯ.LegacyColors.ItemPressedBorder = ቖ.GetColor(12744235);
		ቯ.LegacyColors.ItemPressedText = ቖ.GetColor(2046328);
		ቯ.LegacyColors.ItemSeparator = ቖ.GetColor(9610946);
		ቯ.LegacyColors.ItemSeparatorShade = Color.FromArgb(128, ቖ.GetColor(16777215));
		ቯ.LegacyColors.ItemText = ቖ.GetColor(2046328);
		ቯ.LegacyColors.MenuBackground = ቖ.GetColor(16777215);
		ቯ.LegacyColors.MenuBackground2 = Color.Empty;
		ቯ.LegacyColors.MenuBarBackground = ቖ.GetColor(13622766);
		ቯ.LegacyColors.MenuBorder = ቖ.GetColor(10988464);
		ቯ.LegacyColors.ItemExpandedBorder = ቯ.LegacyColors.MenuBorder;
		ቯ.LegacyColors.MenuSide = ቖ.GetColor(16777215);
		ቯ.LegacyColors.MenuSide2 = Color.Empty;
		ቯ.LegacyColors.MenuUnusedBackground = ቯ.LegacyColors.MenuBackground;
		ቯ.LegacyColors.MenuUnusedSide = ቖ.GetColor(14342874);
		ቯ.LegacyColors.MenuUnusedSide2 = Color.Empty;
		ቯ.LegacyColors.ItemDesignTimeBorder = Color.DarkGray;
		ቯ.LegacyColors.BarDockedBorder = ቖ.GetColor(color);
		ቯ.LegacyColors.DockSiteBackColor = ቖ.GetColor(13622766);
		ቯ.LegacyColors.DockSiteBackColor2 = Color.Empty;
		ቯ.LegacyColors.CustomizeBackground = ቖ.GetColor(9546697);
		ቯ.LegacyColors.CustomizeBackground2 = ቖ.GetColor(12177126);
		ቯ.LegacyColors.CustomizeText = ቖ.GetColor(2046328);
		ቯ.LegacyColors.PanelBackground = ቖ.GetColor(15726333);
		ቯ.LegacyColors.PanelBackground2 = ቖ.GetColor(14214386);
		ቯ.LegacyColors.PanelText = ቖ.GetColor(2046328);
		ቯ.LegacyColors.PanelBorder = ቖ.GetColor(6122618);
		ቯ.LegacyColors.ExplorerBarBackground = ቖ.GetColor(13097712);
		ቯ.LegacyColors.ExplorerBarBackground2 = ቖ.GetColor(8955074);
		ቯ.LegacyColors.SplitterBackground = ቖ.GetColor(14412799);
		ቯ.LegacyColors.SplitterBackground2 = ቖ.GetColor(11846870);
		ቯ.LegacyColors.SplitterBorder = ቖ.GetColor(8756927);
		ቯ.LegacyColors.SplitterText = ቯ.LegacyColors.SplitterBorder;
		ቯ.NavigationPane.ButtonBackground = new GradientColorTable();
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(13622766), 0f));
		ቯ.SuperTooltip.BackgroundColors = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(13228527));
		ቯ.SuperTooltip.TextColor = ቖ.GetColor(5000268);
		Office2007SliderColorTable slider = ቯ.Slider;
		slider.Default.LabelColor = ቖ.GetColor(1980763);
		slider.Default.SliderLabelColor = ቖ.GetColor(1980763);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16251902), 0f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14937336), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12372701), 1f));
		slider.Default.PartBorderColor = ቖ.GetColor(7767709);
		slider.Default.PartBorderLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		slider.Default.PartForeColor = ቖ.GetColor(8290951);
		slider.Default.PartForeLightColor = Color.Empty;
		slider.Default.TrackLineColor = ቖ.GetColor(9084088);
		slider.Default.TrackLineLightColor = ቖ.GetColor(14609143);
		slider.MouseOver.LabelColor = ቖ.GetColor(1980763);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(1980763);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16180380), 0f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16180634), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16446933), 1f));
		slider.MouseOver.PartBorderColor = ቖ.GetColor(15581758);
		slider.MouseOver.PartBorderLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		slider.MouseOver.PartForeColor = ቖ.GetColor(8290951);
		slider.MouseOver.PartForeLightColor = Color.Empty;
		slider.MouseOver.TrackLineColor = ቖ.GetColor(9084088);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(14609143);
		slider.Pressed.LabelColor = ቖ.GetColor(1980763);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(1980763);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16504439), 0f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770185), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770185), 1f));
		slider.Pressed.PartBorderColor = ቖ.GetColor(12746038);
		slider.Pressed.PartBorderLightColor = ቖ.GetColor(15843679);
		slider.Pressed.PartForeColor = ቖ.GetColor(8290951);
		slider.Pressed.PartForeLightColor = Color.Empty;
		slider.Pressed.TrackLineColor = ቖ.GetColor(9084088);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(14609143);
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(9276813);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(9276813);
		slider.Disabled.PartBackground = new GradientColorTable();
		foreach (BackgroundColorBlend color2 in slider.Default.PartBackground.Colors)
		{
			slider.Disabled.PartBackground.Colors.Add(new BackgroundColorBlend(colorBlendFactory.GetColor(color2.Color), color2.Position));
		}
		slider.Disabled.PartBorderColor = colorBlendFactory.GetColor(slider.Default.PartBorderColor);
		slider.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(slider.Default.PartBorderLightColor);
		slider.Disabled.PartForeColor = colorBlendFactory.GetColor(slider.Default.PartForeColor);
		slider.Disabled.PartForeLightColor = colorBlendFactory.GetColor(slider.Default.PartForeLightColor);
		slider.Disabled.TrackLineColor = colorBlendFactory.GetColor(slider.Default.TrackLineColor);
		slider.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(slider.Default.TrackLineLightColor);
		Office2007ColorTable.ቮ(ቯ, ቖ);
		ቯ.ListViewEx.Border = ቖ.GetColor(color);
		ቯ.ListViewEx.ColumnBackground = new LinearGradientColorTable(ቖ.GetColor(15726075), ቖ.GetColor(14806266));
		ቯ.ListViewEx.ColumnSeparator = ቖ.GetColor(color);
		ቯ.ListViewEx.ColumnForeColor = ቖ.GetColor(1980763);
		ቯ.ListViewEx.SelectionBackground = new LinearGradientColorTable(ቖ.GetColor(10997232), Color.Empty);
		ቯ.ListViewEx.SelectionBorder = ቖ.GetColor(14938111);
		ቯ.DataGridView.ColumnHeaderNormalBorder = ቖ.GetColor(color);
		ቯ.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(ቖ.GetColor(15726075), ቖ.GetColor(14806266), 90);
		ቯ.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16374175), ቖ.GetColor(15843679), 90);
		ቯ.DataGridView.ColumnHeaderSelectedBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(16766349), ቖ.GetColor(15897146), 90);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(13952247), ቖ.GetColor(12834540), 90);
		ቯ.DataGridView.ColumnHeaderMouseOverBorder = ቖ.GetColor(color);
		ቯ.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(ቖ.GetColor(11914211), ቖ.GetColor(10861783), 90);
		ቯ.DataGridView.ColumnHeaderPressedBorder = ቖ.GetColor(color);
		ቯ.DataGridView.RowNormalBackground = new LinearGradientColorTable(ቖ.GetColor(15002871));
		ቯ.DataGridView.RowNormalBorder = ቖ.GetColor(color);
		ቯ.DataGridView.RowSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16766349));
		ቯ.DataGridView.RowSelectedBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(15843420));
		ቯ.DataGridView.RowSelectedMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(15843420));
		ቯ.DataGridView.RowMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowPressedBackground = new LinearGradientColorTable(ቖ.GetColor(11914211), ቖ.GetColor(10861783), 90);
		ቯ.DataGridView.RowPressedBorder = ቖ.GetColor(color);
		ቯ.DataGridView.GridColor = ቖ.GetColor(11184810);
		ቯ.DataGridView.SelectorBackground = new LinearGradientColorTable(ቖ.GetColor(10861783));
		ቯ.DataGridView.SelectorBorder = ቖ.GetColor(color);
		ቯ.DataGridView.SelectorBorderDark = ቖ.GetColor(12829635);
		ቯ.DataGridView.SelectorBorderLight = ቖ.GetColor(16382457);
		ቯ.DataGridView.SelectorSign = new LinearGradientColorTable(ቖ.GetColor(15921906));
		ቯ.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(9150645));
		ቯ.DataGridView.SelectorMouseOverBorder = ቖ.GetColor(10401486);
		ቯ.DataGridView.SelectorMouseOverBorderDark = ቖ.GetColor(11587575);
		ቯ.DataGridView.SelectorMouseOverBorderLight = ቖ.GetColor(14017778);
		ቯ.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(ቖ.GetColor(16382715), ቖ.GetColor(14146274));
		ቯ.SideBar.Background = new LinearGradientColorTable(ቖ.GetColor(Color.White));
		ቯ.SideBar.Border = ቖ.GetColor(color);
		ቯ.SideBar.SideBarPanelItemText = ቖ.GetColor(1980763);
		ቯ.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15595005), 0f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14214386), 1f));
		ቯ.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16505781), 0f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16697208), 0.4f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16692310), 0.4f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16640927), 1f));
		ቯ.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16776409), 0f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770957), 0.4f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16766792), 0.4f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770963), 1f));
		ቯ.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16300137), 0f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16622433), 0.4f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16484924), 0.4f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16694112), 1f));
		ቯ.AdvTree = new TreeColorTable();
		_135B._135C(ቯ.AdvTree, ቖ);
		ቯ.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(1980763);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(1980763);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor("F9E8A1"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FAF2CD"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("EEC955");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(1980763);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFFFFDEC"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF4CA"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFEBA6"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF2C5"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8E8F8F");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(1980763);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFC59B61"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFEEB469"), 0.1f),
			new BackgroundColorBlend(ቖ.GetColor("FFFCA060"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFFB8E3D"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFFEBB60"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8B7654");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("408B7654");
		ቯ.WarningBox.BackColor = ቖ.GetColor(ቖ.GetColor(13492213));
		ቯ.WarningBox.WarningBorderColor = ቖ.GetColor(color);
		ቯ.WarningBox.WarningBackColor1 = ቖ.GetColor(13096681);
		ቯ.WarningBox.WarningBackColor2 = ቖ.GetColor(11518940);
		ቯ.CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(ቖ.GetColor(color)),
			new ColorDef(ቖ.GetColor(1980763)),
			new ColorDef(ቖ.GetColor(10862561)),
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(10862561)),
			new ColorDef(ቖ.GetColor(15134199)),
			new ColorDef(ቖ.GetColor(10862561)),
			new ColorDef(ቖ.GetColor(14017009)),
			new ColorDef(ቖ.GetColor(2706554)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(13619928),
				ቖ.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(1980763)),
			new ColorDef(ቖ.GetColor(color)),
			new ColorDef(ቖ.GetColor(color)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(ቖ.GetColor(color)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(1980763)),
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(ቖ.GetColor(15134199)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(color)),
			new ColorDef(ቖ.GetColor(10862561)),
			new ColorDef(ቖ.GetColor(2046329)),
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(ቖ.GetColor(15134199)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(10862561)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(13619928),
				ቖ.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2046329)),
			new ColorDef(ቖ.GetColor(16777215))
		};
		ቯ.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(14739958),
				ቖ.GetColor(11782630)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2642318)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(11650540),
				ቖ.GetColor(7708122)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2909476)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12773564),
				ቖ.GetColor(8704379)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(9125386)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16369568),
				ቖ.GetColor(16029528)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(4073329)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12957158),
				ቖ.GetColor(9796562)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8787740)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(15837868),
				ቖ.GetColor(15034222)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8157204)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16776362),
				ቖ.GetColor(16775512)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(-1)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(8388736))
		};
		ቯ.SuperTab.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(12308198));
		ቯ.SuperTab.InnerBorder = ቖ.GetColor(Color.Transparent);
		ቯ.SuperTab.OuterBorder = ቖ.GetColor(10465991);
		ቯ.SuperTab.ControlBoxDefault.Image = ቖ.GetColor(1980763);
		ቯ.SuperTab.ControlBoxMouseOver.Background = ቖ.GetColor(13561338);
		ቯ.SuperTab.ControlBoxMouseOver.Border = ቖ.GetColor(3381759);
		ቯ.SuperTab.ControlBoxMouseOver.Image = ቖ.GetColor(255, 1980763);
		ቯ.SuperTab.ControlBoxPressed.Background = ቖ.GetColor(12045024);
		ቯ.SuperTab.ControlBoxPressed.Border = ቖ.GetColor(3381759);
		ቯ.SuperTab.ControlBoxPressed.Image = ቖ.GetColor(255, 1980763);
		ቯ.SuperTab.InsertMarker = ቖ.GetColor(255, 0);
		ቯ.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable();
		ቯ.SuperTabItem.Default.Normal.Text = ቖ.GetColor(1980763);
		ቯ.SuperTabItem.Default.Normal.CloseMarker = ቖ.GetColor(1980763);
		ቯ.SuperTabItem.Default.Disabled.Text = ቖ.GetColor(8884895);
		ቯ.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		ቯ.SuperTabItem.Default.Disabled.CloseMarker = ቖ.GetColor(8884895);
		ቯ.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(16120575),
			ቖ.GetColor(15726333)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.Selected.InnerBorder = Color.Transparent;
		ቯ.SuperTabItem.Default.Selected.OuterBorder = ቖ.GetColor(10465991);
		ቯ.SuperTabItem.Default.Selected.Text = ቖ.GetColor(1980763);
		ቯ.SuperTabItem.Default.Selected.CloseMarker = ቖ.GetColor(1980763);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(16120575),
			ቖ.GetColor(15726333)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.SelectedMouseOver.InnerBorder = Color.Transparent;
		ቯ.SuperTabItem.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(10465991);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Text = ቖ.GetColor(1980763);
		ቯ.SuperTabItem.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(1980763);
		ቯ.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(48, 16777215),
			ቖ.GetColor(180, 16777215)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.MouseOver.InnerBorder = ቖ.GetColor(190, 16777215);
		ቯ.SuperTabItem.Default.MouseOver.OuterBorder = ቖ.GetColor(11646394);
		ቯ.SuperTabItem.Default.MouseOver.Text = ቖ.GetColor(1980763);
		ቯ.SuperTabItem.Default.MouseOver.CloseMarker = ቖ.GetColor(1980763);
		ቯ.SuperTabItem.Left = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Bottom = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Right = ቯ.SuperTabItem.Right;
		ቯ.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(15726333));
		ቯ.SuperTabPanel.Default.OuterBorder = ቖ.GetColor(10465991);
		ቯ.SuperTabPanel.Default.InnerBorder = ቖ.GetColor(Color.Transparent);
		ቯ.SuperTabPanel.Left = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Bottom = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Right = ቯ.SuperTabPanel.Default;
		ጦ.ጮ(ቯ.Backstage, ቖ);
		ጦ.ጶ(ቯ.BackstageItem, ቖ);
		ጦ.ጼ(ቯ.BackstagePanel, ቖ);
		ቯ.SwitchButton = new SwitchButtonColors();
		ቯ.SwitchButton.Disabled.BorderColor = ቯ.CheckBoxItem.Disabled.CheckBorder;
		ቯ.SwitchButton.Disabled.SwitchBorderColor = ቯ.SwitchButton.Disabled.BorderColor;
		ቯ.SwitchButton.Disabled.OffTextColor = ቯ.CheckBoxItem.Disabled.Text;
		ቯ.SwitchButton.Disabled.OnTextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.TextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.SwitchBackColor = ቯ.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		ቯ.SwitchButton.Disabled.OffBackColor = ቯ.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		ቯ.SwitchButton.Disabled.OnBackColor = ቯ.SwitchButton.Disabled.OffBackColor;
		ቯ.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = ቖ.GetColor(13028045);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = ቖ.GetColor(16777215);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዸ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዹ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዺ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዻ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዼ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዽ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዿ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጀ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጂ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጋ(ቖ.GetColor(color), SystemColors.Window);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጌ(ቖ, eOffice2010ColorScheme.Blue);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጏ(ቯ.ListViewEx);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1311(ቯ.Bar);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጅ(ቖ.GetColor(color), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጆ(ቖ.GetColor(15726075), ቖ.GetColor(14806266), ቖ.GetColor(10401486));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጉ(ቖ.GetColor(16383229), ቖ.GetColor(13884393), ቖ.GetColor(10401486));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጊ(ቖ.GetColor(0));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጒ(ቖ.GetColor(Color.White), ቖ.GetColor("FF567DB0"), ቖ.GetColor("FF2F578D"));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጕ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1317();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1316();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጘ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጙ(ቯ.CheckBoxItem.Default.CheckBorder);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጃ(ቖ.GetColor(Color.White));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጄ(ቖ.GetColor(Color.DarkGray));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጚ(SystemColors.Window);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጛ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጜ(ቖ.GetColor(12308198));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጝ(ቖ.GetColor(16119543));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጞ(ቖ.GetColor(ቯ.LegacyColors.PanelBorder), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጟ(ቖ.GetColor(3690099));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጠ(ቖ.GetColor(3690099), ቖ.GetColor(16311446));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጡ(ቖ.GetColor(3690099), ቖ.GetColor(14214386));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		ቯ.StepIndicator.BackgroundColor = ቖ.GetColor(ColorFunctions.GetShade(ቯ.Form.BackColor, 10));
		ቯ.StepIndicator.IndicatorColor = ቖ.GetColor(10800168);
		ቯ.RadialMenu = new RadialMenuColorTable();
		ቯ.RadialMenu.CircularBackColor = ቖ.GetColor(13913902);
		ቯ.RadialMenu.CircularBorderColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.CircularForeColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBackground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBorder = ቖ.GetColor(6317934);
		ቯ.RadialMenu.RadialMenuButtonBackground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuButtonBorder = ቖ.GetColor(6317934);
		ቯ.RadialMenu.RadialMenuExpandForeground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuInactiveBorder = Color.FromArgb(128, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.RadialMenu.RadialMenuItemForeground = ቖ.GetColor(6317934);
		ቯ.RadialMenu.RadialMenuItemDisabledForeground = ቖ.GetColor(9276813);
		ቯ.RadialMenu.RadialMenuItemMouseOverBackground = Color.FromArgb(72, ቯ.RadialMenu.RadialMenuItemForeground);
		ቯ.RadialMenu.RadialMenuItemMouseOverForeground = ቖ.GetColor(6317934);
		ቯ.RadialMenu.RadialMenuMouseOverBorder = Color.FromArgb(200, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.StepItem.Default = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14806522),
			ቖ.GetColor(14477557),
			ቖ.GetColor(14477557),
			ቖ.GetColor(13688814)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(0), new Color[1] { ቖ.GetColor(1980763) });
		ቯ.StepItem.Progress = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14218615),
			ቖ.GetColor(7717632),
			ቖ.GetColor(7717632),
			ቖ.GetColor(11135267)
		}, 90, new float[4] { 0f, 0.6f, 0.6f, 1f }, Color.Empty, new Color[0]);
		ቯ.StepItem.MouseOver = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16510387),
			ቖ.GetColor(16376198),
			ቖ.GetColor(16376198),
			ቖ.GetColor(16311446)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(1980763), new Color[1] { ቖ.GetColor(15649112) });
		ቯ.StepItem.Pressed = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16504440),
			ቖ.GetColor(16770186),
			ቖ.GetColor(16770186),
			ቖ.GetColor(16704135)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(1980763), new Color[1] { ቖ.GetColor(12744235) });
		ቯ.TokenEditor = new TokenEditorColorTable(3887985, 15923454, 3887985, 12308198, 16777215, 29382);
		ቯ.Flyout.BorderColor = ቯ.Form.Active.BorderColors[0];
		ቯ.SideNav.SideNavItem.Default = new SideNavItemStateColorTable(ቖ.GetColor(1980763));
		ቯ.SideNav.SideNavItem.MouseOver = new SideNavItemStateColorTable(ቖ.GetColor(1980763), new Color[4]
		{
			ቖ.GetColor(16510387),
			ቖ.GetColor(16376198),
			ቖ.GetColor(16376198),
			ቖ.GetColor(16311446)
		}, new Color[2]
		{
			ቖ.GetColor(15649112),
			ቖ.GetColor(15848521)
		}, 0);
		ቯ.SideNav.SideNavItem.Pressed = new SideNavItemStateColorTable(ቖ.GetColor(1980763), new Color[4]
		{
			ቖ.GetColor(16504440),
			ቖ.GetColor(16770186),
			ቖ.GetColor(16770186),
			ቖ.GetColor(16704135)
		}, new Color[2]
		{
			ቖ.GetColor(12744235),
			ቖ.GetColor(16173159)
		}, 0);
		ቯ.SideNav.SideNavItem.Selected = new SideNavItemStateColorTable(ቖ.GetColor(1980763), new Color[1] { ቯ.Form.BackColor }, new Color[4]
		{
			Color.Empty,
			ቖ.GetColor(10465991),
			Color.Empty,
			ቖ.GetColor(10465991)
		});
		ቯ.SideNav.BorderColors = new Color[1] { ቖ.GetColor(10465991) };
		ቯ.SideNav.TitleBackColor = ቖ.GetColor(15726333);
		ቯ.SideNav.TitleBorderColors = new Color[4]
		{
			Color.Empty,
			Color.Empty,
			Color.Empty,
			ቖ.GetColor(10465991)
		};
		ቯ.SideNav.PanelBackColor = ቯ.Form.BackColor;
		ቯ.TabForm.Active.CaptionText = ቖ.GetColor(1980763);
		ቯ.TabForm.Inactive.CaptionText = ቖ.GetColor(1980763);
		ቯ.TabForm.Active.CaptionBackColors = new Color[1] { ቖ.GetColor(12308198) };
		ቯ.TabForm.Inactive.CaptionBackColors = new Color[1] { ቖ.GetColor(12308198) };
		ቯ.TabFormItemColorTables.Clear();
		ቯ.TabFormItemColorTables.Add(TabFormItem.DefaultColorTableKey, Office2007ColorTable._1257(ቖ, new Color[1] { ቖ.GetColor(12308198) }, new Color[1] { ቖ.GetColor(12308198) }, ቖ.GetColor(1980763), new Color[2]
		{
			ቖ.GetColor(16120575),
			ቖ.GetColor(15726333)
		}, new Color[2]
		{
			ቖ.GetColor(12110299),
			ቖ.GetColor(10465991)
		}, ቖ.GetColor(1980763)));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueColorTableKey, Office2007ColorTable._125E(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.YellowColorTableKey, Office2007ColorTable._125F(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleColorTableKey, Office2007ColorTable.ቡ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.CyanColorTableKey, Office2007ColorTable.ቢ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueMistColorTableKey, Office2007ColorTable.ቦ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleMistColorTableKey, Office2007ColorTable.ቧ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TanColorTableKey, Office2007ColorTable.ቨ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.LemonLimeColorTableKey, Office2007ColorTable.ቩ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.AppleColorTableKey, Office2007ColorTable.ቪ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TealColorTableKey, Office2007ColorTable.ቫ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedChalkColorTableKey, Office2007ColorTable.ቭ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.SilverColorTableKey, Office2007ColorTable.ቬ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.OrangeColorTableKey, Office2007ColorTable.ባ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.GreenColorTableKey, Office2007ColorTable.ቤ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.MagentaColorTableKey, Office2007ColorTable.ብ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedColorTableKey, Office2007ColorTable.በ(ቖ));
	}

	public static Office2007RibbonBarStateColorTable ᱬ(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14871799), ቖ.GetColor(9676739));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(20, 16777215), ቖ.GetColor(60, 16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15726333), ቖ.GetColor(14214386));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(3690099);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable _21B3(ColorFactory ቖ)
	{
		return ᱬ(ቖ);
	}

	public static Office2007RibbonBarStateColorTable _21B4(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14871799), ቖ.GetColor(9676739));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(40, 16777215), ቖ.GetColor(120, 16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15726333), ቖ.GetColor(15200762));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15200762), ቖ.GetColor(14214386));
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007ButtonItemColorTable _12C7(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(1980763);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15649112), ቖ.GetColor(15848521));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16644584), ቖ.GetColor(16579573));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16510387), ቖ.GetColor(16376198));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16376198), ቖ.GetColor(16311446));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16513503), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(15648599), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16645098), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(1980763);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15648599));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12744235), ቖ.GetColor(12753733));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16173159));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16504440), ቖ.GetColor(16770186));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16770186), ቖ.GetColor(16704135));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(12754759), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16439162), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(1980763);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12749360), ቖ.GetColor(12821324));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14862766), Color.Transparent);
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16045233), ቖ.GetColor(16633997));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16761950), ቖ.GetColor(16640148));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16777157), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(1980763);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12751419), ቖ.GetColor(12759375));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16770421), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16770421), ቖ.GetColor(16767340));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16767340), ቖ.GetColor(16771446));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16774272), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(1980763);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ወ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _12C7(ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14806522), ቖ.GetColor(14477557));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(14477557), ቖ.GetColor(13688814));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11254480));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13557229), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(10987431);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ው(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _12C7(ቖ);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(2500134);
		office2007ButtonItemColorTable.Checked.Text = office2007ButtonItemColorTable.Default.Text;
		office2007ButtonItemColorTable.Expanded.Text = office2007ButtonItemColorTable.Default.Text;
		office2007ButtonItemColorTable.MouseOver.Text = office2007ButtonItemColorTable.Default.Text;
		office2007ButtonItemColorTable.MouseOverSplitInactive.Text = office2007ButtonItemColorTable.Default.Text;
		office2007ButtonItemColorTable.Pressed.Text = office2007ButtonItemColorTable.Default.Text;
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(16579837));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16579837), ቖ.GetColor(15593457));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14606304), ቖ.GetColor(10592932));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13557229), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(10987431);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ዊ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ዂ.ዉ(ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(4095451), ቖ.GetColor(2843573));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(2711731), ቖ.GetColor(2977999));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(4557022), Color.Transparent);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(4425700), Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(2050209), ቖ.GetColor(2379686));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13557229), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(10987431);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ዌ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ዂ.ዋ(ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(11865171), ቖ.GetColor(11470669));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11470669), ቖ.GetColor(15221887));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14958972), Color.Transparent);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(12523614), ቖ.GetColor(15222403));
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9306682), ቖ.GetColor(9569085));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13557229), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(10987431);
		return office2007ButtonItemColorTable;
	}

	public static void ዄ(Office2007ButtonItemColorTable ዅ, ColorFactory ቖ)
	{
		Color color = ቖ.GetColor(6514539);
		Color empty = Color.Empty;
		ዅ.Default.ExpandBackground = color;
		ዅ.Default.ExpandLight = empty;
		ዅ.Checked.ExpandBackground = color;
		ዅ.Checked.ExpandLight = empty;
		ዅ.Expanded.ExpandBackground = color;
		ዅ.Expanded.ExpandLight = empty;
		ዅ.MouseOver.ExpandBackground = color;
		ዅ.MouseOver.ExpandLight = empty;
		ዅ.Pressed.ExpandBackground = color;
		ዅ.Pressed.ExpandLight = empty;
	}

	public static Office2007ButtonItemColorTable _21B5(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(6456);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(8756927));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16252927));
		office2007ButtonItemColorTable.MouseOver.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(15069691), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15069692), ቖ.GetColor(12770289));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(6456);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7704235));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(9085890));
		office2007ButtonItemColorTable.Pressed.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(10467286), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(10467286));
		office2007ButtonItemColorTable.Pressed.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(6456);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(8756927));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14543351));
		office2007ButtonItemColorTable.Checked.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14478326), Color.Transparent);
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(13163503), ቖ.GetColor(11848677));
		office2007ButtonItemColorTable.Checked.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(6456);
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Checked;
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዑ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(1980763);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16120575), ቖ.GetColor(15726333));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12110299), ቖ.GetColor(10465991));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(1980763);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16120575), ቖ.GetColor(15726333));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12110299), ቖ.GetColor(10465991));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(1980763);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(48, 16777215), ቖ.GetColor(180, 16777215));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(190, 16777215), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11646394), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(1980763);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዒ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(1980763);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15916273), ቖ.GetColor(15726333));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15660541), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15043261), ቖ.GetColor(14575270));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(1980763);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15916273), ቖ.GetColor(15726333));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15660541), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15043261), ቖ.GetColor(14575270));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(1980763);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(48, 14891409), ቖ.GetColor(180, 16777215));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(190, 16439536), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14891409), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(1980763);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዓ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(1980763);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15916273), ቖ.GetColor(15726333));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15660541), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(6602591), ቖ.GetColor(5489230));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(1980763);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15916273), ቖ.GetColor(15726333));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15660541), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(6602591), ቖ.GetColor(5489230));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(1980763);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(48, 2535459), ቖ.GetColor(180, 16777215));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(190, 13170865), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(2535459), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(1980763);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዔ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(1980763);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15984875), ቖ.GetColor(15726333));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15595005), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15902565), ቖ.GetColor(15900672));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(1980763);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15984875), ቖ.GetColor(15726333));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15595005), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15902565), ቖ.GetColor(15900672));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(1980763);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16110224), ቖ.GetColor(180, 16777215));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(190, 16508527), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15760646), ቖ.GetColor(15695368));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(1980763);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B6(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(4491493), Color.Transparent);
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(0);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(2050209), Color.Transparent);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B7(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(12933008), Color.Transparent);
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(0);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(13172855), Color.Transparent);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B8(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(192, 2535459), Color.Transparent);
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(0);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(2200605), Color.Transparent);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B9(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(192, 16751872), Color.Transparent);
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(0);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(14054681), Color.Transparent);
		return office2007RibbonTabGroupColorTable;
	}

	public static ElementStyle ዸ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.PaddingBottom = 3;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 22;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BorderWidth = 1;
		elementStyle.BorderColor = Color.Transparent;
		elementStyle.CornerDiameter = 3;
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBackgroundBlend.Count];
		menu.FileBackgroundBlend._0CD0(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle ዹ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BorderBottom = eStyleBorderType.Double;
		elementStyle.BorderBottomWidth = 0;
		elementStyle.BorderColor = menu.FileContainerBorder;
		elementStyle.BorderLeft = eStyleBorderType.None;
		elementStyle.BorderLeftWidth = 0;
		elementStyle.BorderRight = eStyleBorderType.None;
		elementStyle.BorderRightWidth = 0;
		elementStyle.BorderTop = eStyleBorderType.Solid;
		elementStyle.BorderTopWidth = 1;
		elementStyle.BorderBottom = eStyleBorderType.Solid;
		elementStyle.BorderBottomWidth = 1;
		elementStyle.PaddingBottom = 0;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 0;
		return elementStyle;
	}

	public static ElementStyle ዺ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnOneBackground;
		elementStyle.BorderRight = eStyleBorderType.Solid;
		elementStyle.BorderRightColor = menu.FileColumnOneBorder;
		elementStyle.BorderRightWidth = 1;
		elementStyle.PaddingRight = 1;
		return elementStyle;
	}

	public static ElementStyle ዻ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnTwoBackground;
		return elementStyle;
	}

	public static ElementStyle ዼ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
		_ = ቯ.Menu;
		elementStyle.MarginTop = 2;
		elementStyle.MarginRight = 2;
		return elementStyle;
	}

	public static void ዳ(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.AppScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(11124437), ቖ.GetColor(12308196), 0);
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(7507629), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(12177126), 0f),
			new BackgroundColorBlend(ቖ.GetColor(9546697), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(9408399)), Color.FromArgb(64, Color.White));
		@default = ዲ.AppScrollBar.MouseOver;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(ቖ.GetColor(12703728), 0f),
			new BackgroundColorBlend(ቖ.GetColor(11849191), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(10731227), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926));
		@default.ThumbSignBackground = ዲ.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(ቖ.GetColor(12703728), 0f),
			new BackgroundColorBlend(ቖ.GetColor(11849191), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(10731227), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(9408399));
		@default = ዲ.AppScrollBar.MouseOverControl;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(ቖ.GetColor(12177126), 0f),
			new BackgroundColorBlend(ቖ.GetColor(11125211), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(9546697), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926));
		@default.ThumbSignBackground = ዲ.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(ቖ.GetColor(12177126), 0f),
			new BackgroundColorBlend(ቖ.GetColor(11125211), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(9546697), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(9408399));
		@default = ዲ.AppScrollBar.Pressed;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(11585502), 0f),
			new BackgroundColorBlend(ቖ.GetColor(9020609), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926));
		@default.ThumbSignBackground = ዲ.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(11585502), 0f),
			new BackgroundColorBlend(ቖ.GetColor(9020609), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926), Color.Empty);
		@default.TrackSignBackground = ዲ.AppScrollBar.Default.TrackSignBackground;
		@default = ዲ.AppScrollBar.Disabled;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void ዱ(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.ScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(16053492), ቖ.GetColor(16579836), 0);
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(15133938));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7632246), ቖ.GetColor(4539975), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15791352), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15396595), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(14738665), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(14146529), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9081241), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(8224125));
		@default = ዲ.ScrollBar.MouseOver;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16580094), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15988473), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15396337), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15001836), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16580094), ቖ.GetColor(15856629));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9081241), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7632246), ቖ.GetColor(4539975), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = @default.ThumbInnerBorder;
		@default.TrackOuterBorder = @default.ThumbOuterBorder;
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(8224125));
		@default = ዲ.ScrollBar.MouseOverControl;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.CopyFrom(ዲ.ScrollBar.Default.TrackBackground);
		@default.ThumbInnerBorder = ዲ.ScrollBar.Default.TrackInnerBorder;
		@default.ThumbOuterBorder = ዲ.ScrollBar.Default.TrackOuterBorder;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7632246), ቖ.GetColor(4539975));
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(ዲ.ScrollBar.Default.TrackBackground);
		@default.TrackInnerBorder = ዲ.ScrollBar.Default.TrackInnerBorder;
		@default.TrackOuterBorder = ዲ.ScrollBar.Default.TrackOuterBorder;
		@default.TrackSignBackground = ዲ.ScrollBar.Default.TrackSignBackground;
		@default = ዲ.ScrollBar.Pressed;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15462387), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15067373), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(14343907), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(13685978), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9081241), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9081241), Color.Empty);
		@default.TrackSignBackground = ዲ.ScrollBar.Default.TrackSignBackground;
		@default = ዲ.ScrollBar.Disabled;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static ElementStyle ጌ(ColorFactory ግ, eOffice2010ColorScheme ጎ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonClientPanelKey;
		elementStyle.BackColorGradientAngle = 90;
		switch (ጎ)
		{
		case eOffice2010ColorScheme.Blue:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(13097712), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(10138575), 0.7f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(8757696), 1f));
			break;
		case eOffice2010ColorScheme.Silver:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(13488598), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(12633288), 0.7f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(12238530), 1f));
			break;
		case eOffice2010ColorScheme.Black:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(7303023), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(5197647), 0.7f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(4144959), 1f));
			break;
		}
		return elementStyle;
	}

	public static Office2007ButtonItemColorTable _21BA(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5406176));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(135, 16777215));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(32, 32767));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(32, 32767));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(5406176), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(135, 16777215), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5406176));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5406176));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(192, 16777215));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(32, 32767));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(32, 32767));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(5406176), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(135, 16777215), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}
}
internal static class _21BB
{
	public static void _21B2(Office2007ColorTable ቯ, ColorFactory ቖ)
	{
		ቯ.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010NormalSilver.png");
		ቯ.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010HotSilver.png");
		ቯ.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010PressedSilver.png");
		int color = 9146519;
		ቯ.RibbonControl.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12238531), ቖ.GetColor(color));
		ቯ.RibbonControl.InnerBorder = null;
		ቯ.RibbonControl.TabsBackground = new LinearGradientColorTable(ቖ.GetColor(14935784));
		ቯ.RibbonControl.TabsGlassBackground = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(16448250));
		ቯ.RibbonControl.TabDividerBorder = Color.Empty;
		ቯ.RibbonControl.TabDividerBorderLight = Color.Empty;
		ቯ.RibbonControl.CornerSize = 1;
		ቯ.RibbonControl.PanelTopBackgroundHeight = 0f;
		ቯ.RibbonControl.PanelTopBackground = new LinearGradientColorTable(ቖ.GetColor(16711422), ቖ.GetColor(15067630));
		ቯ.RibbonControl.PanelBottomBackground = null;
		ቯ.ItemGroup.OuterBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.ItemGroupDividerDark = Color.Empty;
		ቯ.ItemGroup.ItemGroupDividerLight = Color.Empty;
		ቯ.RibbonBar.Default = ᱬ(ቖ);
		ቯ.RibbonBar.MouseOver = _21B3(ቖ);
		ቯ.RibbonBar.Expanded = _21B4(ቖ);
		ቯ.RibbonButtonItemColors.Clear();
		ቯ.ButtonItemColors.Clear();
		ቯ.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _12C7(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ወ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዉ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዊ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዋ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዌ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21B1.ው(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		ቯ.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(ቖ));
		ቯ.BackstageButtonItemColors.Clear();
		ቯ.BackstageButtonItemColors.Add(_21BA(ቖ));
		ቯ.NavigationPaneButtonItemColors.Clear();
		ቯ.NavigationPaneButtonItemColors.Add(_21B5(ቖ));
		ቯ.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = ዑ(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዒ(ቖ);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዓ(ቖ);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዔ(ቖ);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		ቯ.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = _21B1._21B6(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B1._21B7(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B1._21B8(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B1._21B9(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		ቯ.Bar.ToolbarTopBackground = new LinearGradientColorTable(ቖ.GetColor(15461872), ቖ.GetColor(15461872));
		ቯ.Bar.ToolbarBottomBackground = new LinearGradientColorTable(ቖ.GetColor(15461872), ቖ.GetColor(14014684));
		ቯ.Bar.ToolbarBottomBorder = ቖ.GetColor(5987163);
		ቯ.Bar.PopupToolbarBackground = new LinearGradientColorTable(ቖ.GetColor(16579836), Color.Empty);
		ቯ.Bar.PopupToolbarBorder = ቖ.GetColor(10988464);
		ቯ.Bar.StatusBarTopBorder = ቖ.GetColor(8817553);
		ቯ.Bar.StatusBarTopBorderLight = ቖ.GetColor(Color.FromArgb(148, Color.White));
		ቯ.Bar.StatusBarAltBackground.Clear();
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(15461872), 0f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(14014684), 1f));
		ቯ.Menu.Background = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.Menu.Border = new LinearGradientColorTable(ቖ.GetColor(10988464), Color.Empty);
		ቯ.Menu.Side = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.Menu.SideBorder = new LinearGradientColorTable(ቖ.GetColor(14869735), Color.Empty);
		ቯ.Menu.SideBorderLight = new LinearGradientColorTable(ቖ.GetColor(16119285), Color.Empty);
		ቯ.Menu.SideUnused = new LinearGradientColorTable(ቖ.GetColor(15066597), Color.Empty);
		ቯ.Menu.FileBackgroundBlend.Clear();
		ቯ.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(16777215), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16777215), 1f)
		});
		ቯ.Menu.FileContainerBorder = ቖ.GetColor(10988464);
		ቯ.Menu.FileContainerBorderLight = Color.Transparent;
		ቯ.Menu.FileColumnOneBackground = ቖ.GetColor(16777215);
		ቯ.Menu.FileColumnOneBorder = ቖ.GetColor(10988464);
		ቯ.Menu.FileColumnTwoBackground = ቖ.GetColor(16777215);
		ቯ.Menu.FileBottomContainerBackgroundBlend.Clear();
		ቯ.ComboBox.Default.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.Default.Border = ቖ.GetColor(13948633);
		ቯ.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandText = ቖ.GetColor(4542298);
		ቯ.ComboBox.DefaultStandalone.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.DefaultStandalone.Border = ቖ.GetColor(color);
		ቯ.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(15593457), 90);
		ቯ.ComboBox.DefaultStandalone.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(16777215));
		ቯ.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(14606304), Color.Empty, 90);
		ቯ.ComboBox.DefaultStandalone.ExpandText = ቖ.GetColor(4542298);
		ቯ.ComboBox.MouseOver.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.MouseOver.Border = ቖ.GetColor(10790052);
		ቯ.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(16640209), ቖ.GetColor(16765288), 90);
		ቯ.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(16776435), 90);
		ቯ.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(10790052), Color.Empty, 90);
		ቯ.ComboBox.MouseOver.ExpandText = ቖ.GetColor(4542298);
		ቯ.ComboBox.DroppedDown.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.DroppedDown.Border = ቖ.GetColor(color);
		ቯ.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(14993831), ቖ.GetColor(15711329), 90);
		ቯ.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(13942953), ቖ.GetColor(15724985), 90);
		ቯ.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(12751419), Color.Empty, 90);
		ቯ.ComboBox.DroppedDown.ExpandText = ቖ.GetColor(4542298);
		ቯ.DialogLauncher.Default.DialogLauncher = ቖ.GetColor(7041917);
		ቯ.DialogLauncher.Default.DialogLauncherShade = ቖ.GetColor(172, 16777215);
		ቯ.DialogLauncher.MouseOver.DialogLauncher = ቖ.GetColor(10001565);
		ቯ.DialogLauncher.MouseOver.DialogLauncherShade = Color.FromArgb(192, ቖ.GetColor(16777215));
		ቯ.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16641758), ቖ.GetColor(16638141));
		ቯ.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16765551), ቖ.GetColor(16774553));
		ቯ.DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16710131), ቖ.GetColor(16777203));
		ቯ.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(16758528), Color.Empty);
		ቯ.DialogLauncher.Pressed.DialogLauncher = ቖ.GetColor(7041917);
		ቯ.DialogLauncher.Pressed.DialogLauncherShade = Color.FromArgb(128, ቖ.GetColor(16777215));
		ቯ.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14993573), ቖ.GetColor(15583377));
		ቯ.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15645794), ቖ.GetColor(15656075));
		ቯ.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13942696), ቖ.GetColor(15724994));
		ቯ.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12751419), ቖ.GetColor(12760162));
		ቯ.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(16448250));
		ቯ.SystemButton.Default.LightShade = Color.Empty;
		ቯ.SystemButton.Default.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(16448250));
		ቯ.SystemButton.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButton.MouseOver.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(16316664));
		ቯ.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16316664), ቖ.GetColor(15198441));
		ቯ.SystemButton.MouseOver.TopHighlight = null;
		ቯ.SystemButton.MouseOver.BottomHighlight = null;
		ቯ.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10923187));
		ቯ.SystemButton.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215));
		ቯ.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(16448250));
		ቯ.SystemButton.Pressed.LightShade = Color.Empty;
		ቯ.SystemButton.Pressed.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14673131));
		ቯ.SystemButton.Pressed.TopHighlight = null;
		ቯ.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(14673131));
		ቯ.SystemButton.Pressed.BottomHighlight = null;
		ቯ.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(16777215));
		ቯ.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(10923187));
		ቯ.SystemButtonClose = new Office2007SystemButtonColorTable();
		ቯ.SystemButtonClose.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(16448250));
		ቯ.SystemButtonClose.Default.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Default.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButtonClose.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(16570327));
		ቯ.SystemButtonClose.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver.DarkShade = ቖ.GetColor(10173757);
		ቯ.SystemButtonClose.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16745602), ቖ.GetColor(16482174));
		ቯ.SystemButtonClose.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16152953), ቖ.GetColor(14901602));
		ቯ.SystemButtonClose.MouseOver.TopHighlight = null;
		ቯ.SystemButtonClose.MouseOver.BottomHighlight = null;
		ቯ.SystemButtonClose.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10173757));
		ቯ.SystemButtonClose.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.SystemButtonClose.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(16307413));
		ቯ.SystemButtonClose.Pressed.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Pressed.DarkShade = ቖ.GetColor(10173757);
		ቯ.SystemButtonClose.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15890294));
		ቯ.SystemButtonClose.Pressed.TopHighlight = null;
		ቯ.SystemButtonClose.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15890294));
		ቯ.SystemButtonClose.Pressed.BottomHighlight = null;
		ቯ.SystemButtonClose.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10173757));
		ቯ.SystemButtonClose.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14311779));
		ቯ.Form.Active.BorderColors = new Color[5]
		{
			ቖ.GetColor(6647157),
			ቖ.GetColor(16777215),
			ቖ.GetColor(14935784),
			ቖ.GetColor(14935784),
			ቖ.GetColor(14935784)
		};
		ቯ.Form.Inactive.BorderColors = new Color[5]
		{
			ቖ.GetColor(8817553),
			ቖ.GetColor(16579836),
			ቖ.GetColor(16579836),
			ቖ.GetColor(16448250),
			ቖ.GetColor(16448250)
		};
		ቯ.Form.Active.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(15461872), ቖ.GetColor(15330286));
		ቯ.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(15330286), ቖ.GetColor(14935784));
		ቯ.Form.Active.CaptionBottomBorder = new Color[2]
		{
			ቖ.GetColor(7043972),
			ቖ.GetColor(16777215)
		};
		ቯ.Form.Active.CaptionText = ቖ.GetColor(3026478);
		ቯ.Form.Active.CaptionTextExtra = ቖ.GetColor(4542298);
		ቯ.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(16579836));
		ቯ.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(16579836));
		ቯ.Form.Inactive.CaptionText = ቖ.GetColor(9079434);
		ቯ.Form.Inactive.CaptionTextExtra = ቖ.GetColor(9079434);
		ቯ.Form.BackColor = ቖ.GetColor(14738665);
		ቯ.Form.TextColor = ቖ.GetColor(3026478);
		ቯ.Form.MdiClientBackgroundImage = (Image)(object)BarFunctions.ই("SystemImages.Office2010SilverClientBackground.png");
		ቯ.QuickAccessToolbar.Active.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		ቯ.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(14344166));
		ቯ.QuickAccessToolbar.Standalone.OutterBorderColor = ቖ.GetColor(10002859);
		ቯ.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelBackground = ቖ.GetColor(15790837);
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelText = ቖ.GetColor(5002076);
		ቯ.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15725043), ቖ.GetColor(14804201));
		ቯ.TabControl.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15593458), ቖ.GetColor(15725043));
		ቯ.TabControl.Default.InnerBorder = ቖ.GetColor(14672872);
		ቯ.TabControl.Default.OuterBorder = ቖ.GetColor(8356744);
		ቯ.TabControl.Default.Text = ቖ.GetColor(5067609);
		ቯ.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14672872), ቖ.GetColor(16251129));
		ቯ.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16251129), ቖ.GetColor(16711423));
		ቯ.TabControl.MouseOver.InnerBorder = ቖ.GetColor(15987958);
		ቯ.TabControl.MouseOver.OuterBorder = ቖ.GetColor(8817553);
		ቯ.TabControl.MouseOver.Text = ቖ.GetColor(5067609);
		ቯ.TabControl.Selected = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14870251), ቖ.GetColor(16382715));
		ቯ.TabControl.Selected.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16382715), ቖ.GetColor(16777215));
		ቯ.TabControl.Selected.InnerBorder = ቖ.GetColor(14672872);
		ቯ.TabControl.Selected.OuterBorder = ቖ.GetColor(8356744);
		ቯ.TabControl.Selected.Text = ቖ.GetColor(5067609);
		ቯ.TabControl.TabBackground = new LinearGradientColorTable(ቖ.GetColor(15067629), Color.Empty);
		ቯ.TabControl.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.TabControl.TabPanelBorder = ቖ.GetColor(5067609);
		Office2007CheckBoxColorTable checkBoxItem = ቯ.CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(15922165), Color.Empty);
		checkBoxItem.Default.CheckBorder = ቖ.GetColor(9014156);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(15264235), ቖ.GetColor(15921907));
		checkBoxItem.Default.CheckInnerBorder = ቖ.GetColor(11974842);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(ቖ.GetColor(7435124), Color.Empty);
		checkBoxItem.Default.Text = ቖ.GetColor(3026478);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16576962), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = ቖ.GetColor(13602871);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(16444616), ቖ.GetColor(16577501));
		checkBoxItem.MouseOver.CheckInnerBorder = ቖ.GetColor(16559453);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4933956), Color.Empty);
		checkBoxItem.MouseOver.Text = ቖ.GetColor(3026478);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16574397));
		checkBoxItem.Pressed.CheckBorder = ቖ.GetColor(12741917);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(16441506), ቖ.GetColor(16642775));
		checkBoxItem.Pressed.CheckInnerBorder = ቖ.GetColor(16557400);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4933956), Color.Empty);
		checkBoxItem.Pressed.Text = ቖ.GetColor(3026478);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = ቖ.GetColor(11974842);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(16316665));
		checkBoxItem.Disabled.CheckInnerBorder = ቖ.GetColor(14672096);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(ቖ.GetColor(11645876), Color.Empty);
		checkBoxItem.Disabled.Text = ቖ.GetColor(9276813);
		ዱ(ቯ, ቖ);
		ዳ(ቯ, ቖ);
		Office2007ProgressBarColorTable progressBarItem = ቯ.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(8817553);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
		progressBarItem.Chunk = new GradientColorTable(6918698, 15200980, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(15663063)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(9286228)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(6918699)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = ቯ.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(8817553);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
		progressBarItem.Chunk = new GradientColorTable(11446016, 16776653, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16776099)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13814272)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16708608)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = ቯ.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(8817553);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
		progressBarItem.Chunk = new GradientColorTable(13762560, 16764365, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16748431)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13762560)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16646144)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		Office2007GalleryColorTable gallery = ቯ.Gallery;
		gallery.GroupLabelBackground = ቖ.GetColor(15790837);
		gallery.GroupLabelText = ቖ.GetColor(5002076);
		gallery.GroupLabelBorder = ቖ.GetColor(15790837);
		ቯ.LegacyColors.BarBackground = ቖ.GetColor(16711422);
		ቯ.LegacyColors.BarBackground2 = ቖ.GetColor(15067630);
		ቯ.LegacyColors.BarStripeColor = ቖ.GetColor(12238531);
		ቯ.LegacyColors.BarCaptionBackground = ቖ.GetColor(11318719);
		ቯ.LegacyColors.BarCaptionBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveBackground = ቖ.GetColor(14344166);
		ቯ.LegacyColors.BarCaptionInactiveBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveText = ቖ.GetColor(5660525);
		ቯ.LegacyColors.BarCaptionText = ቖ.GetColor(3026478);
		ቯ.LegacyColors.BarFloatingBorder = ቖ.GetColor(9278621);
		ቯ.LegacyColors.BarPopupBackground = ቖ.GetColor(16777215);
		ቯ.LegacyColors.BarPopupBorder = ቖ.GetColor(10988464);
		ቯ.LegacyColors.ItemBackground = Color.Empty;
		ቯ.LegacyColors.ItemBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBackground = ቖ.GetColor(16569720);
		ቯ.LegacyColors.ItemCheckedBackground2 = ቖ.GetColor(16500815);
		ቯ.LegacyColors.ItemCheckedBorder = ቖ.GetColor(12276995);
		ቯ.LegacyColors.ItemCheckedText = ቖ.GetColor(0);
		ቯ.LegacyColors.ItemDisabledBackground = Color.Empty;
		ቯ.LegacyColors.ItemDisabledText = ቖ.GetColor(9276813);
		ቯ.LegacyColors.ItemExpandedShadow = Color.Empty;
		ቯ.LegacyColors.ItemExpandedBackground = ቖ.GetColor(14938110);
		ቯ.LegacyColors.ItemExpandedBackground2 = ቖ.GetColor(10076144);
		ቯ.LegacyColors.ItemExpandedText = ቖ.GetColor(0);
		ቯ.LegacyColors.ItemHotBackground = ቖ.GetColor(16774604);
		ቯ.LegacyColors.ItemHotBackground2 = ቖ.GetColor(16767861);
		ቯ.LegacyColors.ItemHotBorder = ቖ.GetColor(16760169);
		ቯ.LegacyColors.ItemHotText = ቖ.GetColor(3026478);
		ቯ.LegacyColors.ItemPressedBackground = ቖ.GetColor(16553789);
		ቯ.LegacyColors.ItemPressedBackground2 = ቖ.GetColor(16758878);
		ቯ.LegacyColors.ItemPressedBorder = ቖ.GetColor(16485436);
		ቯ.LegacyColors.ItemPressedText = ቖ.GetColor(3026478);
		ቯ.LegacyColors.ItemSeparator = ቖ.GetColor(8817553);
		ቯ.LegacyColors.ItemSeparatorShade = Color.FromArgb(192, ቖ.GetColor(16645630));
		ቯ.LegacyColors.ItemText = ቖ.GetColor(3026478);
		ቯ.LegacyColors.MenuBackground = ቖ.GetColor(16777215);
		ቯ.LegacyColors.MenuBackground2 = Color.Empty;
		ቯ.LegacyColors.MenuBarBackground = ቖ.GetColor(16185078);
		ቯ.LegacyColors.MenuBorder = ቖ.GetColor(10988464);
		ቯ.LegacyColors.ItemExpandedBorder = ቯ.LegacyColors.MenuBorder;
		ቯ.LegacyColors.MenuSide = ቖ.GetColor(16777215);
		ቯ.LegacyColors.MenuSide2 = Color.Empty;
		ቯ.LegacyColors.MenuUnusedBackground = ቯ.LegacyColors.MenuBackground;
		ቯ.LegacyColors.MenuUnusedSide = ቖ.GetColor(14342874);
		ቯ.LegacyColors.MenuUnusedSide2 = Color.Empty;
		ቯ.LegacyColors.ItemDesignTimeBorder = Color.Black;
		ቯ.LegacyColors.BarDockedBorder = ቖ.GetColor(8817553);
		ቯ.LegacyColors.DockSiteBackColor = ቖ.GetColor(14476008);
		ቯ.LegacyColors.DockSiteBackColor2 = Color.Empty;
		ቯ.LegacyColors.CustomizeBackground = ቖ.GetColor(14476008);
		ቯ.LegacyColors.CustomizeBackground2 = ቖ.GetColor(10002859);
		ቯ.LegacyColors.CustomizeText = ቖ.GetColor(0);
		ቯ.LegacyColors.PanelBackground = ቖ.GetColor(16711422);
		ቯ.LegacyColors.PanelBackground2 = ቖ.GetColor(15067630);
		ቯ.LegacyColors.PanelText = ቖ.GetColor(3026478);
		ቯ.LegacyColors.PanelBorder = ቖ.GetColor(10002859);
		ቯ.LegacyColors.ExplorerBarBackground = ቖ.GetColor(14476008);
		ቯ.LegacyColors.ExplorerBarBackground2 = ቖ.GetColor(15461872);
		ቯ.LegacyColors.SplitterBackground = ቖ.GetColor(16251903);
		ቯ.LegacyColors.SplitterBackground2 = ቖ.GetColor(12896460);
		ቯ.LegacyColors.SplitterBorder = ቖ.GetColor(10791858);
		ቯ.LegacyColors.SplitterText = ቯ.LegacyColors.SplitterBorder;
		ቯ.NavigationPane.ButtonBackground = new GradientColorTable();
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14738665), 0f));
		ቯ.SuperTooltip.BackgroundColors = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(15000816));
		ቯ.SuperTooltip.TextColor = ቖ.GetColor(5000268);
		Office2007SliderColorTable slider = ቯ.Slider;
		slider.Default.LabelColor = ቖ.GetColor(8358292);
		slider.Default.SliderLabelColor = ቖ.GetColor(3026478);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16185336), 0f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16119801), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16119801), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(13817305), 1f));
		slider.Default.PartBorderColor = ቖ.GetColor(8358292);
		slider.Default.PartBorderLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		slider.Default.PartForeColor = ቖ.GetColor(5132370);
		slider.Default.PartForeLightColor = Color.Empty;
		slider.Default.TrackLineColor = ቖ.GetColor(2434341);
		slider.Default.TrackLineLightColor = ቖ.GetColor(13421772);
		slider.MouseOver.LabelColor = ቖ.GetColor(8358292);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(3026478);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16777215), 0f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16776693), 0.2f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16768899), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14526221), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16774350), 0.85f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16774350), 1f));
		slider.MouseOver.PartBorderColor = ቖ.GetColor(8358292);
		slider.MouseOver.PartBorderLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		slider.MouseOver.PartForeColor = ቖ.GetColor(5132370);
		slider.MouseOver.PartForeLightColor = Color.Empty;
		slider.MouseOver.TrackLineColor = ቖ.GetColor(2434341);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(13421772);
		slider.Pressed.LabelColor = ቖ.GetColor(8358292);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(3026478);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14382360), 0f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16224303), 0.2f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16368011), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15561728), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16761224), 0.85f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15913653), 1f));
		slider.Pressed.PartBorderColor = ቖ.GetColor(8358292);
		slider.Pressed.PartBorderLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		slider.Pressed.PartForeColor = ቖ.GetColor(5132370);
		slider.Pressed.PartForeLightColor = Color.Empty;
		slider.Pressed.TrackLineColor = ቖ.GetColor(2434341);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(13421772);
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(9276813);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(9276813);
		slider.Disabled.PartBackground = new GradientColorTable();
		foreach (BackgroundColorBlend color2 in slider.Default.PartBackground.Colors)
		{
			slider.Disabled.PartBackground.Colors.Add(new BackgroundColorBlend(colorBlendFactory.GetColor(color2.Color), color2.Position));
		}
		slider.Disabled.PartBorderColor = colorBlendFactory.GetColor(slider.Default.PartBorderColor);
		slider.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(slider.Default.PartBorderLightColor);
		slider.Disabled.PartForeColor = colorBlendFactory.GetColor(slider.Default.PartForeColor);
		slider.Disabled.PartForeLightColor = colorBlendFactory.GetColor(slider.Default.PartForeLightColor);
		slider.Disabled.TrackLineColor = colorBlendFactory.GetColor(slider.Default.TrackLineColor);
		slider.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(slider.Default.TrackLineLightColor);
		Office2007ColorTable.ቮ(ቯ, ቖ);
		ቯ.ListViewEx.Border = ቖ.GetColor(color);
		ቯ.ListViewEx.ColumnBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(14476008));
		ቯ.ListViewEx.ColumnSeparator = ቖ.GetColor(8817553);
		ቯ.ListViewEx.ColumnForeColor = ቖ.GetColor(3026478);
		ቯ.ListViewEx.SelectionBackground = new LinearGradientColorTable(ቖ.GetColor(10997232), Color.Empty);
		ቯ.ListViewEx.SelectionBorder = ቖ.GetColor(14938111);
		ቯ.DataGridView.ColumnHeaderNormalBorder = ቖ.GetColor(8817553);
		ቯ.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(ቖ.GetColor(15461872), ቖ.GetColor(14014684), 90);
		ቯ.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16374175), ቖ.GetColor(15843679), 90);
		ቯ.DataGridView.ColumnHeaderSelectedBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(16766349), ቖ.GetColor(15897146), 90);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(14672612), ቖ.GetColor(12371410), 90);
		ቯ.DataGridView.ColumnHeaderMouseOverBorder = ቖ.GetColor(8888247);
		ቯ.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(ቖ.GetColor(12371410), ቖ.GetColor(14672612), 90);
		ቯ.DataGridView.ColumnHeaderPressedBorder = ቖ.GetColor(16777215);
		ቯ.DataGridView.RowNormalBackground = new LinearGradientColorTable(ቖ.GetColor(15461872));
		ቯ.DataGridView.RowNormalBorder = ቖ.GetColor(8817553);
		ቯ.DataGridView.RowSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16766349));
		ቯ.DataGridView.RowSelectedBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(15843420));
		ቯ.DataGridView.RowSelectedMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(15843420));
		ቯ.DataGridView.RowMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowPressedBackground = new LinearGradientColorTable(ቖ.GetColor(12305617));
		ቯ.DataGridView.RowPressedBorder = ቖ.GetColor(16777215);
		ቯ.DataGridView.GridColor = ቖ.GetColor(11184810);
		ቯ.DataGridView.SelectorBackground = new LinearGradientColorTable(ቖ.GetColor(13027014));
		ቯ.DataGridView.SelectorBorder = ቖ.GetColor(9474450);
		ቯ.DataGridView.SelectorBorderDark = ቖ.GetColor(12829635);
		ቯ.DataGridView.SelectorBorderLight = ቖ.GetColor(16382457);
		ቯ.DataGridView.SelectorSign = new LinearGradientColorTable(ቖ.GetColor(15921906));
		ቯ.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(9150645));
		ቯ.DataGridView.SelectorMouseOverBorder = ቖ.GetColor(10401486);
		ቯ.DataGridView.SelectorMouseOverBorderDark = ቖ.GetColor(11587575);
		ቯ.DataGridView.SelectorMouseOverBorderLight = ቖ.GetColor(14017778);
		ቯ.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(ቖ.GetColor(16382715), ቖ.GetColor(14146274));
		ቯ.SideBar.Background = new LinearGradientColorTable(ቖ.GetColor(Color.White));
		ቯ.SideBar.Border = ቖ.GetColor(color);
		ቯ.SideBar.SideBarPanelItemText = ቖ.GetColor(3026478);
		ቯ.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14410215), 0f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12305357), 1f));
		ቯ.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16505781), 0f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16697208), 0.4f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16692310), 0.4f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16640927), 1f));
		ቯ.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16776409), 0f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770957), 0.4f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16766792), 0.4f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770963), 1f));
		ቯ.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16300137), 0f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16622433), 0.4f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16484924), 0.4f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16694112), 1f));
		ቯ.AdvTree = new TreeColorTable();
		_135B._135C(ቯ.AdvTree, ቖ);
		ቯ.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(3026478);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(3026478);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFFCD9"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFE78D"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFD748"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFE793"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FFB8A98E");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(3026478);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFFFFDEC"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF4CA"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFEBA6"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF2C5"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8E8F8F");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(3026478);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFC59B61"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFEEB469"), 0.1f),
			new BackgroundColorBlend(ቖ.GetColor("FFFCA060"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFFB8E3D"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFFEBB60"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8B7654");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("408B7654");
		ቯ.WarningBox.BackColor = ቖ.GetColor(ቖ.GetColor(14410215));
		ቯ.WarningBox.WarningBorderColor = ቖ.GetColor(color);
		ቯ.WarningBox.WarningBackColor1 = ቖ.GetColor(16711422);
		ቯ.WarningBox.WarningBackColor2 = ቖ.GetColor(15067630);
		ቯ.CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(ቖ.GetColor(6384246)),
			new ColorDef(ቖ.GetColor(6384246)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13883098),
				ቖ.GetColor(11844289),
				ቖ.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(15264492)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(15264492)),
			new ColorDef(ቖ.GetColor(3560290)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(13619928),
				ቖ.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(16119285)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(ቖ.GetColor(15790578)),
			new ColorDef(ቖ.GetColor(7303284)),
			new ColorDef(ቖ.GetColor(7303284)),
			new ColorDef(ቖ.GetColor(7303284)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13883098),
				ቖ.GetColor(11844289),
				ቖ.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }),
			new ColorDef(ቖ.GetColor(6384246)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13817306),
				ቖ.GetColor(12041411),
				ቖ.GetColor(13291220)
			}, new float[4] { 0f, 0.6f, 0.6f, 1f }, 0f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13883098),
				ቖ.GetColor(11844289),
				ቖ.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(ቖ.GetColor(15264492)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(13619928),
				ቖ.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215))
		};
		ቯ.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(ቖ.GetColor(4944290)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16645887),
				ቖ.GetColor(12702698)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2642318)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(11650540),
				ቖ.GetColor(7708122)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2909476)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12773564),
				ቖ.GetColor(8704379)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(9125386)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16369568),
				ቖ.GetColor(16029528)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(4073329)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12957158),
				ቖ.GetColor(9796562)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8787740)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(15837868),
				ቖ.GetColor(15034222)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8157204)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16776362),
				ቖ.GetColor(16775512)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(-1)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(8388736))
		};
		ቯ.SuperTab.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(15067629), Color.Empty);
		ቯ.SuperTab.InnerBorder = ቖ.GetColor(14672872);
		ቯ.SuperTab.OuterBorder = ቖ.GetColor(8356744);
		ቯ.SuperTab.ControlBoxDefault.Image = ቖ.GetColor(255, 0);
		ቯ.SuperTab.ControlBoxMouseOver.Background = ቖ.GetColor(16770978);
		ቯ.SuperTab.ControlBoxMouseOver.Border = ቖ.GetColor(16760169);
		ቯ.SuperTab.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		ቯ.SuperTab.ControlBoxPressed.Background = ቖ.GetColor(16485436);
		ቯ.SuperTab.ControlBoxPressed.Border = ቖ.GetColor(16760169);
		ቯ.SuperTab.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		ቯ.SuperTab.InsertMarker = ቖ.GetColor(255, 128);
		ቯ.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(15725043),
			ቖ.GetColor(14804201),
			ቖ.GetColor(15593458),
			ቖ.GetColor(15725043)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		ቯ.SuperTabItem.Default.Normal.InnerBorder = ቖ.GetColor(14672872);
		ቯ.SuperTabItem.Default.Normal.OuterBorder = ቖ.GetColor(8356744);
		ቯ.SuperTabItem.Default.Normal.Text = ቖ.GetColor(5067609);
		ቯ.SuperTabItem.Default.Normal.CloseMarker = ቖ.GetColor(4222879);
		ቯ.SuperTabItem.Default.Disabled.Text = ቖ.GetColor(9276813);
		ቯ.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		ቯ.SuperTabItem.Default.Disabled.CloseMarker = ቖ.GetColor(9276813);
		ቯ.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(14870251),
			ቖ.GetColor(16382715),
			ቖ.GetColor(16382715),
			ቖ.GetColor(16777215)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		ቯ.SuperTabItem.Default.Selected.InnerBorder = ቖ.GetColor(14672872);
		ቯ.SuperTabItem.Default.Selected.OuterBorder = ቖ.GetColor(8356744);
		ቯ.SuperTabItem.Default.Selected.Text = ቖ.GetColor(5067609);
		ቯ.SuperTabItem.Default.Selected.CloseMarker = ቖ.GetColor(4222879);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(14672872),
			ቖ.GetColor(16251129),
			ቖ.GetColor(16251129),
			ቖ.GetColor(16711423)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		ቯ.SuperTabItem.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(15987958);
		ቯ.SuperTabItem.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(8817553);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Text = ቖ.GetColor(5067609);
		ቯ.SuperTabItem.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(4222879);
		ቯ.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(14672872),
			ቖ.GetColor(16251129),
			ቖ.GetColor(16251129),
			ቖ.GetColor(16711423)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		ቯ.SuperTabItem.Default.MouseOver.InnerBorder = ቖ.GetColor(15987958);
		ቯ.SuperTabItem.Default.MouseOver.OuterBorder = ቖ.GetColor(8817553);
		ቯ.SuperTabItem.Default.MouseOver.Text = ቖ.GetColor(5067609);
		ቯ.SuperTabItem.Default.MouseOver.CloseMarker = ቖ.GetColor(4222879);
		ቯ.SuperTabItem.Left = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Bottom = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Right = ቯ.SuperTabItem.Default;
		ቯ.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.SuperTabPanel.Default.InnerBorder = ቖ.GetColor(14672872);
		ቯ.SuperTabPanel.Default.OuterBorder = ቖ.GetColor(5067609);
		ቯ.SuperTabPanel.Left = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Bottom = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Right = ቯ.SuperTabPanel.Default;
		ጦ.ጱ(ቯ.Backstage, ቖ);
		ጦ.ጸ(ቯ.BackstageItem, ቖ);
		ጦ.ጾ(ቯ.BackstagePanel, ቖ);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = ቖ.GetColor(13948633);
		switchButtonColorTable.OffBackColor = ቖ.GetColor(16777215);
		switchButtonColorTable.OffTextColor = ቖ.GetColor(3026478);
		switchButtonColorTable.OnBackColor = ቖ.GetColor(9621584);
		switchButtonColorTable.OnTextColor = ቖ.GetColor(3026478);
		switchButtonColorTable.SwitchBackColor = ቖ.GetColor(15198959);
		switchButtonColorTable.SwitchBorderColor = ቖ.GetColor(11712702);
		switchButtonColorTable.TextColor = ቖ.GetColor(3026478);
		ቯ.SwitchButton = new SwitchButtonColors();
		ቯ.SwitchButton.Default = switchButtonColorTable;
		ቯ.SwitchButton.Disabled.BorderColor = ቯ.CheckBoxItem.Disabled.CheckBorder;
		ቯ.SwitchButton.Disabled.SwitchBorderColor = ቯ.SwitchButton.Disabled.BorderColor;
		ቯ.SwitchButton.Disabled.OffTextColor = ቯ.CheckBoxItem.Disabled.Text;
		ቯ.SwitchButton.Disabled.OnTextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.TextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.SwitchBackColor = ቯ.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		ቯ.SwitchButton.Disabled.OffBackColor = ቯ.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		ቯ.SwitchButton.Disabled.OnBackColor = ቯ.SwitchButton.Disabled.OffBackColor;
		ቯ.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = ቖ.GetColor(13028045);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = ቖ.GetColor(16777215);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዸ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዹ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዺ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዻ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዼ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዽ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዿ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጀ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጂ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጋ(ቖ.GetColor(color), SystemColors.Window);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = _21B1.ጌ(ቖ, eOffice2010ColorScheme.Silver);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጏ(ቯ.ListViewEx);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1311(ቯ.Bar);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጅ(ቖ.GetColor(color), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጆ(ቖ.GetColor(16383229), ቖ.GetColor(13884393), ቖ.GetColor(10401486));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጉ(ቖ.GetColor(16383229), ቖ.GetColor(13884393), ቖ.GetColor(10401486));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጊ(ቖ.GetColor(0));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጒ(ቖ.GetColor(Color.White), ቖ.GetColor("FF567DB0"), ቖ.GetColor("FF2F578D"));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጕ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1317();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1316();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጘ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጙ(ቯ.CheckBoxItem.Default.CheckBorder);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጃ(ቖ.GetColor(Color.White));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጄ(ቖ.GetColor(Color.DarkGray));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጚ(SystemColors.Window);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጛ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጜ(ቖ.GetColor(14014684));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጝ(ቖ.GetColor(16711422));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጞ(ቖ.GetColor(ቯ.LegacyColors.PanelBorder), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጟ(ቖ.GetColor(3026478));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጠ(ቖ.GetColor(3026478), ቖ.GetColor(16639112));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጡ(ቖ.GetColor(3026478), ቖ.GetColor(Color.DarkGray));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		ቯ.StepIndicator.BackgroundColor = ቖ.GetColor(ColorFunctions.GetShade(ቯ.Form.BackColor, 10));
		ቯ.StepIndicator.IndicatorColor = ቖ.GetColor(10800168);
		ቯ.RadialMenu = new RadialMenuColorTable();
		ቯ.RadialMenu.CircularBackColor = ቖ.GetColor(13913902);
		ቯ.RadialMenu.CircularBorderColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.CircularForeColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBackground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBorder = ቖ.GetColor(6645609);
		ቯ.RadialMenu.RadialMenuButtonBackground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuButtonBorder = ቖ.GetColor(6645609);
		ቯ.RadialMenu.RadialMenuExpandForeground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuInactiveBorder = Color.FromArgb(128, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.RadialMenu.RadialMenuItemForeground = ቖ.GetColor(6645609);
		ቯ.RadialMenu.RadialMenuItemDisabledForeground = ቖ.GetColor(9276813);
		ቯ.RadialMenu.RadialMenuItemMouseOverBackground = Color.FromArgb(72, ቯ.RadialMenu.RadialMenuItemForeground);
		ቯ.RadialMenu.RadialMenuItemMouseOverForeground = ቖ.GetColor(6645609);
		ቯ.RadialMenu.RadialMenuMouseOverBorder = Color.FromArgb(200, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.StepItem.Default = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16777215),
			ቖ.GetColor(16514044),
			ቖ.GetColor(16514044),
			ቖ.GetColor(15593457)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(3026478), new Color[1] { ቖ.GetColor(9146519) });
		ቯ.StepItem.Progress = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14218615),
			ቖ.GetColor(7717632),
			ቖ.GetColor(7717632),
			ቖ.GetColor(11135267)
		}, 90, new float[4] { 0f, 0.6f, 0.6f, 1f }, Color.Empty, new Color[0]);
		ቯ.StepItem.MouseOver = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16641715),
			ቖ.GetColor(16639112),
			ቖ.GetColor(16639112),
			ቖ.GetColor(16644318)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(3026478), new Color[1] { ቖ.GetColor(15845976) });
		ቯ.StepItem.Pressed = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16504440),
			ቖ.GetColor(16770186),
			ቖ.GetColor(16770186),
			ቖ.GetColor(16704135)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(3026478), new Color[1] { ቖ.GetColor(12744235) });
		ቯ.TokenEditor = new TokenEditorColorTable(0, 15461874, 0, 12305099, 16777215, 29382);
		ቯ.Flyout.BorderColor = ቯ.Form.Active.BorderColors[0];
		ቯ.SideNav.SideNavItem.Default = new SideNavItemStateColorTable(ቖ.GetColor(3026478));
		ቯ.SideNav.SideNavItem.MouseOver = new SideNavItemStateColorTable(ቖ.GetColor(3026478), new Color[4]
		{
			ቖ.GetColor(16510387),
			ቖ.GetColor(16376198),
			ቖ.GetColor(16376198),
			ቖ.GetColor(16311446)
		}, new Color[2]
		{
			ቖ.GetColor(15649112),
			ቖ.GetColor(15848521)
		}, 0);
		ቯ.SideNav.SideNavItem.Pressed = new SideNavItemStateColorTable(ቖ.GetColor(1980763), new Color[4]
		{
			ቖ.GetColor(16504440),
			ቖ.GetColor(16770186),
			ቖ.GetColor(16770186),
			ቖ.GetColor(16704135)
		}, new Color[2]
		{
			ቖ.GetColor(12744235),
			ቖ.GetColor(16173159)
		}, 0);
		ቯ.SideNav.SideNavItem.Selected = new SideNavItemStateColorTable(ቖ.GetColor(3026478), new Color[1] { ቯ.Form.BackColor }, new Color[4]
		{
			Color.Empty,
			ቖ.GetColor(12238531),
			Color.Empty,
			ቖ.GetColor(12238531)
		});
		ቯ.SideNav.BorderColors = new Color[1] { ቖ.GetColor(12238531) };
		ቯ.SideNav.TitleBackColor = ቖ.GetColor(15067630);
		ቯ.SideNav.TitleBorderColors = new Color[4]
		{
			Color.Empty,
			Color.Empty,
			Color.Empty,
			ቖ.GetColor(12238531)
		};
		ቯ.SideNav.PanelBackColor = ቯ.Form.BackColor;
		ቯ.TabForm.Active.CaptionText = ቖ.GetColor(3026478);
		ቯ.TabForm.Inactive.CaptionText = ቖ.GetColor(3026478);
		ቯ.TabForm.Active.CaptionBackColors = new Color[1] { ቖ.GetColor(14935784) };
		ቯ.TabForm.Inactive.CaptionBackColors = new Color[1] { ቖ.GetColor(14935784) };
		ቯ.TabFormItemColorTables.Clear();
		ቯ.TabFormItemColorTables.Add(TabFormItem.DefaultColorTableKey, Office2007ColorTable._1257(ቖ, new Color[1] { ቖ.GetColor(16711422) }, new Color[1] { ቖ.GetColor(12238531) }, ቖ.GetColor(3026478), new Color[2]
		{
			ቖ.GetColor(16777215),
			ቖ.GetColor(16711422)
		}, new Color[1] { ቖ.GetColor(11975359) }, ቖ.GetColor(3026478)));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueColorTableKey, Office2007ColorTable._125E(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.YellowColorTableKey, Office2007ColorTable._125F(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleColorTableKey, Office2007ColorTable.ቡ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.CyanColorTableKey, Office2007ColorTable.ቢ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueMistColorTableKey, Office2007ColorTable.ቦ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleMistColorTableKey, Office2007ColorTable.ቧ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TanColorTableKey, Office2007ColorTable.ቨ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.LemonLimeColorTableKey, Office2007ColorTable.ቩ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.AppleColorTableKey, Office2007ColorTable.ቪ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TealColorTableKey, Office2007ColorTable.ቫ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedChalkColorTableKey, Office2007ColorTable.ቭ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.SilverColorTableKey, Office2007ColorTable.ቬ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.OrangeColorTableKey, Office2007ColorTable.ባ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.GreenColorTableKey, Office2007ColorTable.ቤ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.MagentaColorTableKey, Office2007ColorTable.ብ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedColorTableKey, Office2007ColorTable.በ(ቖ));
	}

	public static Office2007RibbonBarStateColorTable ᱬ(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(16185336), ቖ.GetColor(11581116));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(120, 16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16711422), ቖ.GetColor(15067630));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(5660525);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable _21B3(ColorFactory ቖ)
	{
		return ᱬ(ቖ);
	}

	public static Office2007RibbonBarStateColorTable _21B4(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15725042), ቖ.GetColor(13488855));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(120, 16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16711422), ቖ.GetColor(14607595));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(16317179), Color.Transparent);
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	private static Office2007ButtonItemColorTable _21BA(ColorFactory ቖ)
	{
		return _21B1._21BA(ቖ);
	}

	public static Office2007ButtonItemColorTable _12C7(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(3026478);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15845976), ቖ.GetColor(16242763));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16644584), ቖ.GetColor(16645365));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16641715), ቖ.GetColor(16639112));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16639112), ቖ.GetColor(16644318));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16644832), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(15845976), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16644584), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(3026478);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12744235), ቖ.GetColor(12754503));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16173159), ቖ.GetColor(16439162));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16504440), ቖ.GetColor(16770186));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16770186), ቖ.GetColor(16704135));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16770186), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(12744235), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16173159), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(3026478);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12749360), ቖ.GetColor(12756045));
		office2007ButtonItemColorTable.Checked.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16770421), ቖ.GetColor(16767083));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16767083), ቖ.GetColor(16772217));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16774272), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(3026478);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12749360), ቖ.GetColor(12756045));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16770421), ቖ.GetColor(16767083));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16767083), ቖ.GetColor(16772217));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16774272), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(3026478);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ወ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _12C7(ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(16514044));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16514044), ቖ.GetColor(15593457));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9146519));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16119285), ቖ.GetColor(15527148));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15066597), ቖ.GetColor(15198183));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12369084), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(9276813);
		return office2007ButtonItemColorTable;
	}

	public static void ዄ(Office2007ButtonItemColorTable ዅ, ColorFactory ቖ)
	{
		Color color = ቖ.GetColor(4542298);
		Color expandLight = Color.FromArgb(192, Color.White);
		ዅ.Default.ExpandBackground = color;
		ዅ.Default.ExpandLight = expandLight;
		ዅ.Checked.ExpandBackground = color;
		ዅ.Checked.ExpandLight = expandLight;
		ዅ.Expanded.ExpandBackground = color;
		ዅ.Expanded.ExpandLight = expandLight;
		ዅ.MouseOver.ExpandBackground = color;
		ዅ.MouseOver.ExpandLight = expandLight;
		ዅ.Pressed.ExpandBackground = color;
		ዅ.Pressed.ExpandLight = expandLight;
	}

	public static Office2007ButtonItemColorTable _21B5(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10594739));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215));
		office2007ButtonItemColorTable.MouseOver.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16185337), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16054009), ቖ.GetColor(14870252));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9345187));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(11712704));
		office2007ButtonItemColorTable.Pressed.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(12634063), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(12634063));
		office2007ButtonItemColorTable.Pressed.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10594739));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15725045));
		office2007ButtonItemColorTable.Checked.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(15527924), Color.Transparent);
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(14607338), ቖ.GetColor(13621214));
		office2007ButtonItemColorTable.Checked.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Checked;
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዑ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3026478);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(16711422));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11975359), ቖ.GetColor(12238531));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3026478);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(16711422));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11975359), ቖ.GetColor(12238531));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3026478);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(14277081), ቖ.GetColor(16514043));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16185078), ቖ.GetColor(16514043));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11908533), ቖ.GetColor(11646394));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3026478);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዒ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3026478);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16047854), ቖ.GetColor(16711422));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14715072), ቖ.GetColor(14642602));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3026478);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16047854), ቖ.GetColor(16711422));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14715072), ቖ.GetColor(14642602));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3026478);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15578584), ቖ.GetColor(16579579));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16579579), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16373742), ቖ.GetColor(16641528));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14891409));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3026478);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዓ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3026478);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(15004383));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14019283), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12173751), ቖ.GetColor(12566974));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3026478);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(15004383));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14019283), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12173751), ቖ.GetColor(12566974));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3026478);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(11788003), ቖ.GetColor(9100959));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14609663), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14872575), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3026478);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዔ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3026478);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289388), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3026478);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289388), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3026478);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13623521), ቖ.GetColor(15329177));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(13361919), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14872575), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3026478);
		return office2007RibbonTabItemColorTable;
	}

	public static ElementStyle ዸ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.PaddingBottom = 3;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 22;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BorderWidth = 1;
		elementStyle.BorderColor = Color.Transparent;
		elementStyle.CornerDiameter = 3;
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBackgroundBlend.Count];
		menu.FileBackgroundBlend._0CD0(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle ዹ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BorderBottom = eStyleBorderType.Double;
		elementStyle.BorderBottomWidth = 0;
		elementStyle.BorderColor = menu.FileContainerBorder;
		elementStyle.BorderLeft = eStyleBorderType.None;
		elementStyle.BorderLeftWidth = 0;
		elementStyle.BorderRight = eStyleBorderType.None;
		elementStyle.BorderRightWidth = 0;
		elementStyle.BorderTop = eStyleBorderType.Solid;
		elementStyle.BorderTopWidth = 1;
		elementStyle.BorderBottom = eStyleBorderType.Solid;
		elementStyle.BorderBottomWidth = 1;
		elementStyle.PaddingBottom = 0;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 0;
		return elementStyle;
	}

	public static ElementStyle ዺ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnOneBackground;
		elementStyle.BorderRight = eStyleBorderType.Solid;
		elementStyle.BorderRightColor = menu.FileColumnOneBorder;
		elementStyle.BorderRightWidth = 1;
		elementStyle.PaddingRight = 1;
		return elementStyle;
	}

	public static ElementStyle ዻ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnTwoBackground;
		return elementStyle;
	}

	public static ElementStyle ዼ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
		_ = ቯ.Menu;
		elementStyle.MarginTop = 2;
		elementStyle.MarginRight = 2;
		return elementStyle;
	}

	public static void ዱ(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.ScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(15724527), ቖ.GetColor(16579836), 0);
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(15461871), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15922163), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15790320), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(15461356), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(15132648), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11975359), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(9408399)), Color.FromArgb(64, Color.White));
		@default = ዲ.ScrollBar.MouseOver;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16777215), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16580093), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11975359));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16777215), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16580093), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11449272), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(9408399));
		@default = ዲ.ScrollBar.MouseOverControl;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16777215), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16580093), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11975359));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16777215), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16580093), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11449272), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(9408399));
		@default = ዲ.ScrollBar.Pressed;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(14803683), 0f),
			new BackgroundColorBlend(ቖ.GetColor(14803683), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11449272));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(8947849));
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(14803683), 0f),
			new BackgroundColorBlend(ቖ.GetColor(14803683), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11449272), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(9408399));
		@default = ዲ.ScrollBar.Disabled;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void ዳ(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.AppScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(14081250), ቖ.GetColor(14212836), 0);
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(15133938));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7632246), ቖ.GetColor(4539975), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15791352), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15396595), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(14738665), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(14146529), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9081241), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(8224125));
		@default = ዲ.AppScrollBar.MouseOver;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16580094), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15988473), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15396337), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15001836), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16580094), ቖ.GetColor(15856629));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9081241), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7632246), ቖ.GetColor(4539975), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = @default.ThumbInnerBorder;
		@default.TrackOuterBorder = @default.ThumbOuterBorder;
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(8224125));
		@default = ዲ.AppScrollBar.MouseOverControl;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.CopyFrom(ዲ.AppScrollBar.Default.TrackBackground);
		@default.ThumbInnerBorder = ዲ.AppScrollBar.Default.TrackInnerBorder;
		@default.ThumbOuterBorder = ዲ.AppScrollBar.Default.TrackOuterBorder;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7632246), ቖ.GetColor(4539975));
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(ዲ.AppScrollBar.Default.TrackBackground);
		@default.TrackInnerBorder = ዲ.AppScrollBar.Default.TrackInnerBorder;
		@default.TrackOuterBorder = ዲ.AppScrollBar.Default.TrackOuterBorder;
		@default.TrackSignBackground = ዲ.AppScrollBar.Default.TrackSignBackground;
		@default = ዲ.AppScrollBar.Pressed;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15462387), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15067373), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(14343907), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(13685978), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9081241), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9081241), Color.Empty);
		@default.TrackSignBackground = ዲ.AppScrollBar.Default.TrackSignBackground;
		@default = ዲ.AppScrollBar.Disabled;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}
}
internal static class _21BC
{
	public static void _21B2(Office2007ColorTable ቯ, ColorFactory ቖ)
	{
		ቯ.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010NormalSilver.png");
		ቯ.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010HotSilver.png");
		ቯ.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010PressedSilver.png");
		ቯ.RibbonControl.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(4079423), ቖ.GetColor(5131854));
		ቯ.RibbonControl.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.RibbonControl.TabsBackground = new LinearGradientColorTable(ቖ.GetColor(7434609));
		ቯ.RibbonControl.TabsGlassBackground = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(4802889));
		ቯ.RibbonControl.TabDividerBorder = Color.Empty;
		ቯ.RibbonControl.TabDividerBorderLight = Color.Empty;
		ቯ.RibbonControl.CornerSize = 1;
		ቯ.RibbonControl.PanelTopBackgroundHeight = 0f;
		ቯ.RibbonControl.PanelTopBackground = new LinearGradientColorTable(ቖ.GetColor(13224393), ቖ.GetColor(10724259));
		ቯ.RibbonControl.PanelBottomBackground = null;
		ቯ.ItemGroup.OuterBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.ItemGroupDividerDark = Color.Empty;
		ቯ.ItemGroup.ItemGroupDividerLight = Color.Empty;
		ቯ.RibbonBar.Default = ᱬ(ቖ);
		ቯ.RibbonBar.MouseOver = _21B3(ቖ);
		ቯ.RibbonBar.Expanded = _21B4(ቖ);
		ቯ.RibbonButtonItemColors.Clear();
		ቯ.ButtonItemColors.Clear();
		ቯ.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _12C7(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ወ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዉ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዊ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዋ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዌ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21B1.ው(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		ቯ.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(ቖ));
		ቯ.BackstageButtonItemColors.Clear();
		ቯ.BackstageButtonItemColors.Add(_21BA(ቖ));
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007ButtonItemColorTable), "StatusBar"), _21BD(ቖ));
		ቯ.NavigationPaneButtonItemColors.Clear();
		ቯ.NavigationPaneButtonItemColors.Add(_21B5(ቖ));
		ቯ.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = _21BE(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = _21BF(ቖ);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዓ(ቖ);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዔ(ቖ);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		ቯ.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = _21B1._21B6(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B1._21B7(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B1._21B8(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B1._21B9(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		ቯ.Bar.ToolbarTopBackground = new LinearGradientColorTable(ቖ.GetColor(4934475), ቖ.GetColor(4144959));
		ቯ.Bar.ToolbarBottomBackground = new LinearGradientColorTable(ቖ.GetColor(4144959), ቖ.GetColor(3355443));
		ቯ.Bar.ToolbarBottomBorder = ቖ.GetColor(5395026);
		ቯ.Bar.PopupToolbarBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.Bar.PopupToolbarBorder = ቖ.GetColor(10988464);
		ቯ.Bar.StatusBarTopBorder = ቖ.GetColor(2894892);
		ቯ.Bar.StatusBarTopBorderLight = ቖ.GetColor(Color.FromArgb(32, Color.White));
		ቯ.Bar.StatusBarAltBackground.Clear();
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(4934475), 0f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(3355443), 1f));
		ቯ.Menu.Background = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.Menu.Border = new LinearGradientColorTable(ቖ.GetColor(10988464), Color.Empty);
		ቯ.Menu.Side = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.Menu.SideBorder = new LinearGradientColorTable(ቖ.GetColor(14869735), Color.Empty);
		ቯ.Menu.SideBorderLight = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.Menu.SideUnused = new LinearGradientColorTable(ቖ.GetColor(15066597), Color.Empty);
		ቯ.Menu.FileBackgroundBlend.Clear();
		ቯ.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(16777215), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16777215), 1f)
		});
		ቯ.Menu.FileContainerBorder = ቖ.GetColor(10988464);
		ቯ.Menu.FileContainerBorderLight = Color.Transparent;
		ቯ.Menu.FileColumnOneBackground = ቖ.GetColor(16777215);
		ቯ.Menu.FileColumnOneBorder = ቖ.GetColor(10988464);
		ቯ.Menu.FileColumnTwoBackground = ቖ.GetColor(16777215);
		ቯ.Menu.FileBottomContainerBackgroundBlend.Clear();
		ቯ.ComboBox.Default.Background = ቖ.GetColor(13027014);
		ቯ.ComboBox.Default.Border = ቖ.GetColor(9539985);
		ቯ.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandText = ቖ.GetColor(5263699);
		ቯ.ComboBox.DefaultStandalone.Background = ቖ.GetColor(16777215);
		int color = 3223857;
		ቯ.ComboBox.DefaultStandalone.Border = ቖ.GetColor(color);
		ቯ.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(14869218), ቖ.GetColor(12895428), 90);
		ቯ.ComboBox.DefaultStandalone.ExpandBorderInner = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(9539985), Color.Empty, 90);
		ቯ.ComboBox.DefaultStandalone.ExpandText = ቖ.GetColor(5592664);
		ቯ.ComboBox.MouseOver.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.MouseOver.Border = ቖ.GetColor(9539985);
		ቯ.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(16114849), ቖ.GetColor(16182985), 90);
		ቯ.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(16512739));
		ቯ.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(15253839), Color.Empty, 90);
		ቯ.ComboBox.MouseOver.ExpandText = ቖ.GetColor(6051393);
		ቯ.ComboBox.DroppedDown.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.DroppedDown.Border = ቖ.GetColor(9539985);
		ቯ.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(14993831), ቖ.GetColor(15711329), 90);
		ቯ.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(13942953), ቖ.GetColor(15724985), 90);
		ቯ.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(12751419), Color.Empty, 90);
		ቯ.ComboBox.DroppedDown.ExpandText = ቖ.GetColor(4542298);
		ቯ.DialogLauncher.Default.DialogLauncher = ቖ.GetColor(7039851);
		ቯ.DialogLauncher.Default.DialogLauncherShade = Color.Transparent;
		ቯ.DialogLauncher.MouseOver.DialogLauncher = ቖ.GetColor(9866340);
		ቯ.DialogLauncher.MouseOver.DialogLauncherShade = Color.Transparent;
		ቯ.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15851417), ቖ.GetColor(15849339));
		ቯ.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15849339), ቖ.GetColor(15852463));
		ቯ.DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16446946), ቖ.GetColor(16447470));
		ቯ.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15056715), Color.Empty);
		ቯ.DialogLauncher.Pressed.DialogLauncher = ቖ.GetColor(7041917);
		ቯ.DialogLauncher.Pressed.DialogLauncherShade = Color.FromArgb(128, ቖ.GetColor(16777215));
		ቯ.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14993573), ቖ.GetColor(15583377));
		ቯ.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15645794), ቖ.GetColor(15656075));
		ቯ.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13942696), ቖ.GetColor(15724994));
		ቯ.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12751419), ቖ.GetColor(12760162));
		ቯ.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(14474460));
		ቯ.SystemButton.Default.LightShade = Color.Empty;
		ቯ.SystemButton.Default.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(14474460));
		ቯ.SystemButton.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButton.MouseOver.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(9737364), ቖ.GetColor(9145227));
		ቯ.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(9145227), ቖ.GetColor(7829367));
		ቯ.SystemButton.MouseOver.TopHighlight = null;
		ቯ.SystemButton.MouseOver.BottomHighlight = null;
		ቯ.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5329233));
		ቯ.SystemButton.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(14474460));
		ቯ.SystemButton.Pressed.LightShade = Color.Empty;
		ቯ.SystemButton.Pressed.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(6842472));
		ቯ.SystemButton.Pressed.TopHighlight = null;
		ቯ.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(7434609));
		ቯ.SystemButton.Pressed.BottomHighlight = null;
		ቯ.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5329233));
		ቯ.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(6513507));
		ቯ.SystemButtonClose = new Office2007SystemButtonColorTable();
		ቯ.SystemButtonClose.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(14474460));
		ቯ.SystemButtonClose.Default.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Default.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButtonClose.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(14474460));
		ቯ.SystemButtonClose.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver.DarkShade = ቖ.GetColor(10173757);
		ቯ.SystemButtonClose.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16745602), ቖ.GetColor(16482174));
		ቯ.SystemButtonClose.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16152953), ቖ.GetColor(14901602));
		ቯ.SystemButtonClose.MouseOver.TopHighlight = null;
		ቯ.SystemButtonClose.MouseOver.BottomHighlight = null;
		ቯ.SystemButtonClose.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10173757));
		ቯ.SystemButtonClose.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.SystemButtonClose.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(14474460));
		ቯ.SystemButtonClose.Pressed.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Pressed.DarkShade = ቖ.GetColor(10173757);
		ቯ.SystemButtonClose.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15890294));
		ቯ.SystemButtonClose.Pressed.TopHighlight = null;
		ቯ.SystemButtonClose.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15890294));
		ቯ.SystemButtonClose.Pressed.BottomHighlight = null;
		ቯ.SystemButtonClose.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10173757));
		ቯ.SystemButtonClose.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14311779));
		ቯ.Form.Active.BorderColors = new Color[5]
		{
			ቖ.GetColor(6513507),
			ቖ.GetColor(8618883),
			ቖ.GetColor(7829367),
			ቖ.GetColor(7829367),
			ቖ.GetColor(7829367)
		};
		ቯ.Form.Inactive.BorderColors = new Color[5]
		{
			ቖ.GetColor(7829367),
			ቖ.GetColor(10395294),
			ቖ.GetColor(10395294),
			ቖ.GetColor(10395294),
			ቖ.GetColor(10395294)
		};
		ቯ.Form.Active.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(7960953), ቖ.GetColor(7960953));
		ቯ.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(7895160), ቖ.GetColor(7434609));
		ቯ.Form.Active.CaptionBottomBorder = new Color[2]
		{
			ቖ.GetColor(6184542),
			ቖ.GetColor(8618883)
		};
		ቯ.Form.Active.CaptionText = ቖ.GetColor(16777215);
		ቯ.Form.Active.CaptionTextExtra = ቖ.GetColor(14869218);
		ቯ.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(10395294));
		ቯ.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(10395294));
		ቯ.Form.Inactive.CaptionText = ቖ.GetColor(13948116);
		ቯ.Form.Inactive.CaptionTextExtra = ቖ.GetColor(13948116);
		ቯ.Form.BackColor = ቖ.GetColor(13882323);
		ቯ.Form.TextColor = ቖ.GetColor(0);
		ቯ.Form.MdiClientBackgroundImage = (Image)(object)BarFunctions.ই("SystemImages.Office2010BlackClientBackground.png");
		ቯ.QuickAccessToolbar.Active.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		ቯ.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(7960953));
		ቯ.QuickAccessToolbar.Standalone.OutterBorderColor = ቖ.GetColor(8684676);
		ቯ.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelBackground = ቖ.GetColor(15790837);
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelText = ቖ.GetColor(3881787);
		ቯ.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(9539985), ቖ.GetColor(8289918));
		ቯ.TabControl.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(8289918), ቖ.GetColor(7105644));
		ቯ.TabControl.Default.InnerBorder = ቖ.GetColor(9539985);
		ቯ.TabControl.Default.OuterBorder = ቖ.GetColor(color);
		ቯ.TabControl.Default.Text = ቖ.GetColor(16777215);
		ቯ.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(12698049), ቖ.GetColor(11447982));
		ቯ.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11447982), ቖ.GetColor(9539985));
		ቯ.TabControl.MouseOver.InnerBorder = ቖ.GetColor(9539985);
		ቯ.TabControl.MouseOver.OuterBorder = ቖ.GetColor(3421236);
		ቯ.TabControl.MouseOver.Text = ቖ.GetColor(0);
		ቯ.TabControl.Selected = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(9539985), ቖ.GetColor(16777215));
		ቯ.TabControl.Selected.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(16777215));
		ቯ.TabControl.Selected.InnerBorder = ቖ.GetColor(9539985);
		ቯ.TabControl.Selected.OuterBorder = ቖ.GetColor(3421236);
		ቯ.TabControl.Selected.Text = ቖ.GetColor(0);
		ቯ.TabControl.TabBackground = new LinearGradientColorTable(ቖ.GetColor(6974058), Color.Empty);
		ቯ.TabControl.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.TabControl.TabPanelBorder = ቖ.GetColor(color);
		Office2007CheckBoxColorTable checkBoxItem = ቯ.CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(15922165), Color.Empty);
		checkBoxItem.Default.CheckBorder = ቖ.GetColor(9014156);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(15264235), ቖ.GetColor(15987700));
		checkBoxItem.Default.CheckInnerBorder = ቖ.GetColor(11974842);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(ቖ.GetColor(7435124), Color.Empty);
		checkBoxItem.Default.Text = ቖ.GetColor(0);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16576962), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = ቖ.GetColor(13602871);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(16444616), ቖ.GetColor(16577501));
		checkBoxItem.MouseOver.CheckInnerBorder = ቖ.GetColor(16559453);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4933956), Color.Empty);
		checkBoxItem.MouseOver.Text = ቖ.GetColor(0);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16574397));
		checkBoxItem.Pressed.CheckBorder = ቖ.GetColor(12741917);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(16441506), ቖ.GetColor(16642775));
		checkBoxItem.Pressed.CheckInnerBorder = ቖ.GetColor(16557400);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4933956), Color.Empty);
		checkBoxItem.Pressed.Text = ቖ.GetColor(3881787);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = ቖ.GetColor(11974842);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(16316665));
		checkBoxItem.Disabled.CheckInnerBorder = ቖ.GetColor(14672096);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(ቖ.GetColor(11645876), Color.Empty);
		checkBoxItem.Disabled.Text = ቖ.GetColor(9276813);
		ዱ(ቯ, ቖ);
		_21C0(ቯ, ቖ);
		Office2007ProgressBarColorTable progressBarItem = ቯ.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(8882571, 9934999);
		progressBarItem.OuterBorder = ቖ.GetColor(3618617);
		progressBarItem.InnerBorder = ቖ.GetColor(10921638);
		progressBarItem.Chunk = new GradientColorTable(6918698, 15200980, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(15663063)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(9286228)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(6918699)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(8224383, 9474448, 0);
		progressBarItem = ቯ.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(8882571, 9934999);
		progressBarItem.OuterBorder = ቖ.GetColor(3618617);
		progressBarItem.InnerBorder = ቖ.GetColor(10921638);
		progressBarItem.Chunk = new GradientColorTable(11446016, 16776653, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16776099)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13814272)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16708608)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(8224383, 9474448, 0);
		progressBarItem = ቯ.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(8882571, 9934999);
		progressBarItem.OuterBorder = ቖ.GetColor(3618617);
		progressBarItem.InnerBorder = ቖ.GetColor(10921638);
		progressBarItem.Chunk = new GradientColorTable(13762560, 16764365, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16748431)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13762560)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16646144)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(8224383, 9474448, 0);
		Office2007GalleryColorTable gallery = ቯ.Gallery;
		gallery.GroupLabelBackground = ቖ.GetColor(15790837);
		gallery.GroupLabelText = ቖ.GetColor(3881787);
		gallery.GroupLabelBorder = ቖ.GetColor(15790837);
		ቯ.LegacyColors.BarBackground = ቖ.GetColor(13224393);
		ቯ.LegacyColors.BarBackground2 = ቖ.GetColor(10724259);
		ቯ.LegacyColors.BarStripeColor = ቖ.GetColor(5395026);
		ቯ.LegacyColors.BarCaptionBackground = ቖ.GetColor(6842472);
		ቯ.LegacyColors.BarCaptionBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveBackground = ቖ.GetColor(8158332);
		ቯ.LegacyColors.BarCaptionInactiveBackground2 = Color.Empty;
		ቯ.LegacyColors.BarCaptionInactiveText = ቖ.GetColor(16777215);
		ቯ.LegacyColors.BarCaptionText = ቖ.GetColor(16777215);
		ቯ.LegacyColors.BarFloatingBorder = ቖ.GetColor(4473924);
		ቯ.LegacyColors.BarPopupBackground = ቖ.GetColor(16777215);
		ቯ.LegacyColors.BarPopupBorder = ቖ.GetColor(10988464);
		ቯ.LegacyColors.ItemBackground = Color.Empty;
		ቯ.LegacyColors.ItemBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBackground = ቖ.GetColor(16569720);
		ቯ.LegacyColors.ItemCheckedBackground2 = ቖ.GetColor(16500815);
		ቯ.LegacyColors.ItemCheckedBorder = ቖ.GetColor(12276995);
		ቯ.LegacyColors.ItemCheckedText = ቖ.GetColor(0);
		ቯ.LegacyColors.ItemDisabledBackground = Color.Empty;
		ቯ.LegacyColors.ItemDisabledText = ቖ.GetColor(9276813);
		ቯ.LegacyColors.ItemExpandedShadow = Color.Empty;
		ቯ.LegacyColors.ItemExpandedBackground = ቖ.GetColor(14938110);
		ቯ.LegacyColors.ItemExpandedBackground2 = ቖ.GetColor(10076144);
		ቯ.LegacyColors.ItemExpandedText = ቖ.GetColor(0);
		ቯ.LegacyColors.ItemHotBackground = ቖ.GetColor(16774604);
		ቯ.LegacyColors.ItemHotBackground2 = ቖ.GetColor(16767861);
		ቯ.LegacyColors.ItemHotBorder = ቖ.GetColor(16760169);
		ቯ.LegacyColors.ItemHotText = ቖ.GetColor(3881787);
		ቯ.LegacyColors.ItemPressedBackground = ቖ.GetColor(16553789);
		ቯ.LegacyColors.ItemPressedBackground2 = ቖ.GetColor(16758878);
		ቯ.LegacyColors.ItemPressedBorder = ቖ.GetColor(16485436);
		ቯ.LegacyColors.ItemPressedText = ቖ.GetColor(3881787);
		ቯ.LegacyColors.ItemSeparator = ቖ.GetColor(8817553);
		ቯ.LegacyColors.ItemSeparatorShade = Color.FromArgb(192, ቖ.GetColor(16645630));
		ቯ.LegacyColors.ItemText = ቖ.GetColor(0);
		ቯ.LegacyColors.MenuBackground = ቖ.GetColor(16777215);
		ቯ.LegacyColors.MenuBackground2 = Color.Empty;
		ቯ.LegacyColors.MenuBarBackground = ቖ.GetColor(13224393);
		ቯ.LegacyColors.MenuBorder = ቖ.GetColor(10988464);
		ቯ.LegacyColors.ItemExpandedBorder = ቯ.LegacyColors.MenuBorder;
		ቯ.LegacyColors.MenuSide = ቖ.GetColor(16777215);
		ቯ.LegacyColors.MenuSide2 = Color.Empty;
		ቯ.LegacyColors.MenuUnusedBackground = ቯ.LegacyColors.MenuBackground;
		ቯ.LegacyColors.MenuUnusedSide = ቖ.GetColor(14342874);
		ቯ.LegacyColors.MenuUnusedSide2 = Color.Empty;
		ቯ.LegacyColors.ItemDesignTimeBorder = Color.Black;
		ቯ.LegacyColors.BarDockedBorder = ቖ.GetColor(6184542);
		ቯ.LegacyColors.DockSiteBackColor = ቖ.GetColor(13224393);
		ቯ.LegacyColors.DockSiteBackColor2 = Color.Empty;
		ቯ.LegacyColors.CustomizeBackground = ቖ.GetColor(8684676);
		ቯ.LegacyColors.CustomizeBackground2 = ቖ.GetColor(7960953);
		ቯ.LegacyColors.CustomizeText = ቖ.GetColor(0);
		ቯ.LegacyColors.PanelBackground = ቖ.GetColor(13224393);
		ቯ.LegacyColors.PanelBackground2 = ቖ.GetColor(10724259);
		ቯ.LegacyColors.PanelText = ቖ.GetColor(0);
		ቯ.LegacyColors.PanelBorder = ቖ.GetColor(3158064);
		ቯ.LegacyColors.ExplorerBarBackground = ቖ.GetColor(13619151);
		ቯ.LegacyColors.ExplorerBarBackground2 = ቖ.GetColor(13224393);
		ቯ.LegacyColors.SplitterBackground = ቖ.GetColor(6710886);
		ቯ.LegacyColors.SplitterBackground2 = ቖ.GetColor(5329233);
		ቯ.LegacyColors.SplitterBorder = ቖ.GetColor(3158064);
		ቯ.LegacyColors.SplitterText = ቯ.LegacyColors.SplitterBorder;
		ቯ.NavigationPane.ButtonBackground = new GradientColorTable();
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(5987163), 0f));
		ቯ.SuperTooltip.BackgroundColors = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(15000816));
		ቯ.SuperTooltip.TextColor = ቖ.GetColor(5000268);
		ቯ.Slider = _21C2(ቖ);
		ቯ.ContextualTables.Add(Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), typeof(Bar)) + "+StatusBar", _21C1(ቖ));
		Office2007ColorTable.ቮ(ቯ, ቖ);
		ቯ.ListViewEx.Border = ቖ.GetColor(color);
		ቯ.ListViewEx.ColumnBackground = new LinearGradientColorTable(ቖ.GetColor(15395562), ቖ.GetColor(14671839));
		ቯ.ListViewEx.ColumnSeparator = ቖ.GetColor(3881787);
		ቯ.ListViewEx.ColumnForeColor = ቖ.GetColor(0);
		ቯ.ListViewEx.SelectionBackground = new LinearGradientColorTable(ቖ.GetColor(10997232), Color.Empty);
		ቯ.ListViewEx.SelectionBorder = ቖ.GetColor(5779983);
		ቯ.DataGridView.ColumnHeaderNormalBorder = ቖ.GetColor(8817553);
		ቯ.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(ቖ.GetColor(15461872), ቖ.GetColor(14014684), 90);
		ቯ.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16374175), ቖ.GetColor(15843679), 90);
		ቯ.DataGridView.ColumnHeaderSelectedBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(16766349), ቖ.GetColor(15897146), 90);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(14672612), ቖ.GetColor(12371410), 90);
		ቯ.DataGridView.ColumnHeaderMouseOverBorder = ቖ.GetColor(8888247);
		ቯ.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(ቖ.GetColor(12371410), ቖ.GetColor(14672612), 90);
		ቯ.DataGridView.ColumnHeaderPressedBorder = ቖ.GetColor(16777215);
		ቯ.DataGridView.RowNormalBackground = new LinearGradientColorTable(ቖ.GetColor(15461872));
		ቯ.DataGridView.RowNormalBorder = ቖ.GetColor(8817553);
		ቯ.DataGridView.RowSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16766349));
		ቯ.DataGridView.RowSelectedBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(15843420));
		ቯ.DataGridView.RowSelectedMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(14672612));
		ቯ.DataGridView.RowMouseOverBorder = ቖ.GetColor(8888247);
		ቯ.DataGridView.RowPressedBackground = new LinearGradientColorTable(ቖ.GetColor(12305617));
		ቯ.DataGridView.RowPressedBorder = ቖ.GetColor(16777215);
		ቯ.DataGridView.GridColor = ቖ.GetColor(11184810);
		ቯ.DataGridView.SelectorBackground = new LinearGradientColorTable(ቖ.GetColor(13027014));
		ቯ.DataGridView.SelectorBorder = ቖ.GetColor(9474450);
		ቯ.DataGridView.SelectorBorderDark = ቖ.GetColor(12829635);
		ቯ.DataGridView.SelectorBorderLight = ቖ.GetColor(16382457);
		ቯ.DataGridView.SelectorSign = new LinearGradientColorTable(ቖ.GetColor(15921906));
		ቯ.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(9150645));
		ቯ.DataGridView.SelectorMouseOverBorder = ቖ.GetColor(10401486);
		ቯ.DataGridView.SelectorMouseOverBorderDark = ቖ.GetColor(11587575);
		ቯ.DataGridView.SelectorMouseOverBorderLight = ቖ.GetColor(14017778);
		ቯ.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(ቖ.GetColor(16382715), ቖ.GetColor(14146274));
		ቯ.SideBar.Background = new LinearGradientColorTable(ቖ.GetColor(Color.White));
		ቯ.SideBar.Border = ቖ.GetColor(color);
		ቯ.SideBar.SideBarPanelItemText = ቖ.GetColor(3881787);
		ቯ.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14410215), 0f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12305357), 1f));
		ቯ.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16505781), 0f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16697208), 0.4f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16692310), 0.4f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16640927), 1f));
		ቯ.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16776409), 0f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770957), 0.4f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16766792), 0.4f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770963), 1f));
		ቯ.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16300137), 0f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16622433), 0.4f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16484924), 0.4f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16694112), 1f));
		ቯ.AdvTree = new TreeColorTable();
		_135B._135C(ቯ.AdvTree, ቖ);
		ቯ.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(3881787);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(3881787);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFFCD9"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFE78D"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFD748"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFE793"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FFB8A98E");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(3881787);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFFFFDEC"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF4CA"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFEBA6"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF2C5"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8E8F8F");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(3881787);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFC59B61"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFEEB469"), 0.1f),
			new BackgroundColorBlend(ቖ.GetColor("FFFCA060"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFFB8E3D"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFFEBB60"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8B7654");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("408B7654");
		ቯ.WarningBox.BackColor = ቖ.GetColor(ቖ.GetColor(13882323));
		ቯ.WarningBox.WarningBorderColor = ቖ.GetColor(color);
		ቯ.WarningBox.WarningBackColor1 = ቖ.GetColor(16711422);
		ቯ.WarningBox.WarningBackColor2 = ቖ.GetColor(15067630);
		ቯ.CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(3552822)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(15264492)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(15264492)),
			new ColorDef(ቖ.GetColor(5002076)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(13619928),
				ቖ.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(16119285)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(3552822)),
			new ColorDef(ቖ.GetColor(12105912)),
			new ColorDef(ቖ.GetColor(12105912)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13883098),
				ቖ.GetColor(11844289),
				ቖ.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }),
			new ColorDef(ቖ.GetColor(6384246)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13817306),
				ቖ.GetColor(12041411),
				ቖ.GetColor(13291220)
			}, new float[4] { 0f, 0.6f, 0.6f, 1f }, 0f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(ቖ.GetColor(15264492)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(13619928),
				ቖ.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215))
		};
		ቯ.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(ቖ.GetColor(4944290)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16645887),
				ቖ.GetColor(12702698)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2642318)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(11650540),
				ቖ.GetColor(7708122)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2909476)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12773564),
				ቖ.GetColor(8704379)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(9125386)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16369568),
				ቖ.GetColor(16029528)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(4073329)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12957158),
				ቖ.GetColor(9796562)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8787740)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(15837868),
				ቖ.GetColor(15034222)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8157204)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16776362),
				ቖ.GetColor(16775512)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(-1)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(8388736))
		};
		ቯ.SuperTab.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(15067629), Color.Empty);
		ቯ.SuperTab.InnerBorder = ቖ.GetColor(14672872);
		ቯ.SuperTab.OuterBorder = ቖ.GetColor(8356744);
		ቯ.SuperTab.ControlBoxDefault.Image = ቖ.GetColor(255, 0);
		ቯ.SuperTab.ControlBoxMouseOver.Background = ቖ.GetColor(16770978);
		ቯ.SuperTab.ControlBoxMouseOver.Border = ቖ.GetColor(16760169);
		ቯ.SuperTab.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		ቯ.SuperTab.ControlBoxPressed.Background = ቖ.GetColor(16485436);
		ቯ.SuperTab.ControlBoxPressed.Border = ቖ.GetColor(16760169);
		ቯ.SuperTab.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		ቯ.SuperTab.InsertMarker = ቖ.GetColor(255, 128);
		ቯ.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(15725043),
			ቖ.GetColor(14804201),
			ቖ.GetColor(15593458),
			ቖ.GetColor(15725043)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		ቯ.SuperTabItem.Default.Normal.InnerBorder = ቖ.GetColor(14672872);
		ቯ.SuperTabItem.Default.Normal.OuterBorder = ቖ.GetColor(8356744);
		ቯ.SuperTabItem.Default.Normal.Text = ቖ.GetColor(5067609);
		ቯ.SuperTabItem.Default.Normal.CloseMarker = ቖ.GetColor(4222879);
		ቯ.SuperTabItem.Default.Disabled.Text = ቖ.GetColor(9276813);
		ቯ.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		ቯ.SuperTabItem.Default.Disabled.CloseMarker = ቖ.GetColor(9276813);
		ቯ.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(14870251),
			ቖ.GetColor(16382715),
			ቖ.GetColor(16382715),
			ቖ.GetColor(16777215)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		ቯ.SuperTabItem.Default.Selected.InnerBorder = ቖ.GetColor(14672872);
		ቯ.SuperTabItem.Default.Selected.OuterBorder = ቖ.GetColor(8356744);
		ቯ.SuperTabItem.Default.Selected.Text = ቖ.GetColor(5067609);
		ቯ.SuperTabItem.Default.Selected.CloseMarker = ቖ.GetColor(4222879);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(14672872),
			ቖ.GetColor(16251129),
			ቖ.GetColor(16251129),
			ቖ.GetColor(16711423)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		ቯ.SuperTabItem.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(15987958);
		ቯ.SuperTabItem.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(8817553);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Text = ቖ.GetColor(5067609);
		ቯ.SuperTabItem.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(4222879);
		ቯ.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[4]
		{
			ቖ.GetColor(14672872),
			ቖ.GetColor(16251129),
			ቖ.GetColor(16251129),
			ቖ.GetColor(16711423)
		}, new float[4] { 0f, 0.5f, 0.5f, 1f });
		ቯ.SuperTabItem.Default.MouseOver.InnerBorder = ቖ.GetColor(15987958);
		ቯ.SuperTabItem.Default.MouseOver.OuterBorder = ቖ.GetColor(8817553);
		ቯ.SuperTabItem.Default.MouseOver.Text = ቖ.GetColor(5067609);
		ቯ.SuperTabItem.Default.MouseOver.CloseMarker = ቖ.GetColor(4222879);
		ቯ.SuperTabItem.Left = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Bottom = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Right = ቯ.SuperTabItem.Default;
		ቯ.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.SuperTabPanel.Default.InnerBorder = ቖ.GetColor(14672872);
		ቯ.SuperTabPanel.Default.OuterBorder = ቖ.GetColor(5067609);
		ቯ.SuperTabPanel.Left = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Bottom = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Right = ቯ.SuperTabPanel.Default;
		ጦ.ጰ(ቯ.Backstage, ቖ);
		ጦ.ጷ(ቯ.BackstageItem, ቖ);
		ጦ.ጽ(ቯ.BackstagePanel, ቖ);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = ቖ.GetColor(9539985);
		switchButtonColorTable.OffBackColor = ቖ.GetColor(13224393);
		switchButtonColorTable.OffTextColor = ቖ.GetColor(0);
		switchButtonColorTable.OnBackColor = ቖ.GetColor(9621584);
		switchButtonColorTable.OnTextColor = ቖ.GetColor(0);
		switchButtonColorTable.SwitchBackColor = ቖ.GetColor(10790052);
		switchButtonColorTable.SwitchBorderColor = ቖ.GetColor(5592405);
		switchButtonColorTable.TextColor = ቖ.GetColor(0);
		ቯ.SwitchButton = new SwitchButtonColors();
		ቯ.SwitchButton.Default = switchButtonColorTable;
		ቯ.SwitchButton.Disabled.BorderColor = ቯ.CheckBoxItem.Disabled.CheckBorder;
		ቯ.SwitchButton.Disabled.SwitchBorderColor = ቯ.SwitchButton.Disabled.BorderColor;
		ቯ.SwitchButton.Disabled.OffTextColor = ቯ.CheckBoxItem.Disabled.Text;
		ቯ.SwitchButton.Disabled.OnTextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.TextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.SwitchBackColor = ቯ.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		ቯ.SwitchButton.Disabled.OffBackColor = ቯ.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		ቯ.SwitchButton.Disabled.OnBackColor = ቯ.SwitchButton.Disabled.OffBackColor;
		ቯ.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = ቖ.GetColor(8684676);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = ቖ.GetColor(12303291);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዸ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዹ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዺ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዻ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዼ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዽ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዿ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጀ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጂ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጋ(ቖ.GetColor(color), SystemColors.Window);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = _21B1.ጌ(ቖ, eOffice2010ColorScheme.Black);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጏ(ቯ.ListViewEx);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1311(ቯ.Bar);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጅ(ቖ.GetColor(color), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጆ(ቯ.DataGridView.ColumnHeaderNormalBackground.Start, ቯ.DataGridView.ColumnHeaderNormalBackground.End, ቯ.DataGridView.ColumnHeaderNormalBorder);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጉ(ቯ.DataGridView.ColumnHeaderNormalBackground.Start, ቯ.DataGridView.ColumnHeaderNormalBackground.End, ቯ.DataGridView.ColumnHeaderNormalBorder);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጊ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጒ(ቖ.GetColor(Color.White), ቖ.GetColor("FF567DB0"), ቖ.GetColor("FF2F578D"));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጕ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1317();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1316();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጘ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጙ(ቯ.CheckBoxItem.Default.CheckBorder);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጃ(ቖ.GetColor(Color.White));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጄ(ቖ.GetColor(Color.DarkGray));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጚ(SystemColors.Window);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጛ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጜ(ቖ.GetColor(10724259));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጝ(ቖ.GetColor(16119543));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጞ(ቖ.GetColor(ቯ.LegacyColors.PanelBorder), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጟ(ቖ.GetColor(2368548));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጠ(ቖ.GetColor(2368548), ቖ.GetColor(16769916));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጡ(ቖ.GetColor(2368548), ቖ.GetColor(10724259));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		ቯ.StepIndicator.BackgroundColor = ቖ.GetColor(ColorFunctions.GetShade(ቯ.Form.BackColor, 10));
		ቯ.StepIndicator.IndicatorColor = ቖ.GetColor(10800168);
		ቯ.RadialMenu = new RadialMenuColorTable();
		ቯ.RadialMenu.CircularBackColor = ቖ.GetColor(13913902);
		ቯ.RadialMenu.CircularBorderColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.CircularForeColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBackground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBorder = ቖ.GetColor(6645609);
		ቯ.RadialMenu.RadialMenuButtonBackground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuButtonBorder = ቖ.GetColor(6645609);
		ቯ.RadialMenu.RadialMenuExpandForeground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuInactiveBorder = Color.FromArgb(128, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.RadialMenu.RadialMenuItemForeground = ቖ.GetColor(6645609);
		ቯ.RadialMenu.RadialMenuItemDisabledForeground = ቖ.GetColor(9276813);
		ቯ.RadialMenu.RadialMenuItemMouseOverBackground = Color.FromArgb(72, ቯ.RadialMenu.RadialMenuItemForeground);
		ቯ.RadialMenu.RadialMenuItemMouseOverForeground = ቖ.GetColor(6645609);
		ቯ.RadialMenu.RadialMenuMouseOverBorder = Color.FromArgb(200, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.LabelItemColors.Clear();
		ቯ.LabelItemColors.Add(string.Empty, new LabelColors(ቖ.GetColor(0), ቖ.GetColor(9276813)));
		ቯ.LabelItemColors.Add(Office2007ColorTable.GetContextualKey(typeof(Bar), "StatusBar"), new LabelColors(ቖ.GetColor(16777215), ቖ.GetColor(9276813)));
		ቯ.StepItem.Default = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14869218),
			ቖ.GetColor(14277081),
			ቖ.GetColor(14277081),
			ቖ.GetColor(12895428)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(0), new Color[1] { ቖ.GetColor(9539985) });
		ቯ.StepItem.Progress = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14218615),
			ቖ.GetColor(7717632),
			ቖ.GetColor(7717632),
			ቖ.GetColor(11135267)
		}, 90, new float[4] { 0f, 0.6f, 0.6f, 1f }, Color.Empty, new Color[0]);
		ቯ.StepItem.MouseOver = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16772776),
			ቖ.GetColor(16769916),
			ቖ.GetColor(16769916),
			ቖ.GetColor(16776416)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(4605510), new Color[1] { ቖ.GetColor(16238656) });
		ቯ.StepItem.Pressed = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16504440),
			ቖ.GetColor(16770186),
			ቖ.GetColor(16770186),
			ቖ.GetColor(16704135)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(4605510), new Color[1] { ቖ.GetColor(12744235) });
		ቯ.SideNav.SideNavItem.Default = new SideNavItemStateColorTable(ቖ.GetColor(0));
		ቯ.SideNav.SideNavItem.MouseOver = new SideNavItemStateColorTable(ቖ.GetColor(0), new Color[4]
		{
			ቖ.GetColor(16772776),
			ቖ.GetColor(16769916),
			ቖ.GetColor(16769916),
			ቖ.GetColor(16776416)
		}, new Color[2]
		{
			ቖ.GetColor(16238656),
			ቖ.GetColor(16777200)
		}, 0);
		ቯ.SideNav.SideNavItem.Pressed = new SideNavItemStateColorTable(ቖ.GetColor(0), new Color[4]
		{
			ቖ.GetColor(16504440),
			ቖ.GetColor(16770186),
			ቖ.GetColor(16770186),
			ቖ.GetColor(16704135)
		}, new Color[2]
		{
			ቖ.GetColor(12744235),
			ቖ.GetColor(16173159)
		}, 0);
		ቯ.SideNav.SideNavItem.Selected = new SideNavItemStateColorTable(ቖ.GetColor(0), new Color[1] { ቯ.Form.BackColor }, new Color[4]
		{
			Color.Empty,
			ቖ.GetColor(6184542),
			Color.Empty,
			ቖ.GetColor(6184542)
		});
		ቯ.SideNav.BorderColors = new Color[1] { ቖ.GetColor(6184542) };
		ቯ.SideNav.TitleBackColor = ቖ.GetColor(4934475);
		ቯ.SideNav.TitleBorderColors = new Color[4]
		{
			Color.Empty,
			Color.Empty,
			Color.Empty,
			ቖ.GetColor(6184542)
		};
		ቯ.SideNav.PanelBackColor = ቯ.Form.BackColor;
		ቯ.TabForm.Active.CaptionText = ቖ.GetColor(16777215);
		ቯ.TabForm.Inactive.CaptionText = ቖ.GetColor(16777215);
		ቯ.TabForm.Active.CaptionBackColors = new Color[1] { ቖ.GetColor(7434609) };
		ቯ.TabForm.Inactive.CaptionBackColors = new Color[1] { ቖ.GetColor(7434609) };
		ቯ.TabFormItemColorTables.Clear();
		ቯ.TabFormItemColorTables.Add(TabFormItem.DefaultColorTableKey, Office2007ColorTable._1257(ቖ, new Color[1] { ቖ.GetColor(7434609) }, new Color[1] { ቖ.GetColor(7434609) }, ቖ.GetColor(16777215), new Color[2]
		{
			ቖ.GetColor(13619151),
			ቖ.GetColor(16119543)
		}, new Color[1] { ቖ.GetColor(6184542) }, ቖ.GetColor(0)));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueColorTableKey, Office2007ColorTable._125E(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.YellowColorTableKey, Office2007ColorTable._125F(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleColorTableKey, Office2007ColorTable.ቡ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.CyanColorTableKey, Office2007ColorTable.ቢ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueMistColorTableKey, Office2007ColorTable.ቦ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleMistColorTableKey, Office2007ColorTable.ቧ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TanColorTableKey, Office2007ColorTable.ቨ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.LemonLimeColorTableKey, Office2007ColorTable.ቩ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.AppleColorTableKey, Office2007ColorTable.ቪ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TealColorTableKey, Office2007ColorTable.ቫ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedChalkColorTableKey, Office2007ColorTable.ቭ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.SilverColorTableKey, Office2007ColorTable.ቬ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.OrangeColorTableKey, Office2007ColorTable.ባ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.GreenColorTableKey, Office2007ColorTable.ቤ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.MagentaColorTableKey, Office2007ColorTable.ብ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedColorTableKey, Office2007ColorTable.በ(ቖ));
	}

	public static Office2007RibbonBarStateColorTable ᱬ(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12105912), ቖ.GetColor(5395026));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13355979), ቖ.GetColor(11513775));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13224393), ቖ.GetColor(10724259));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(2368548);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable _21B3(ColorFactory ቖ)
	{
		return ᱬ(ቖ);
	}

	public static Office2007RibbonBarStateColorTable _21B4(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12105912), ቖ.GetColor(5395026));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13355979), ቖ.GetColor(11513775));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13224393), ቖ.GetColor(10724259));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(2368548);
		return office2007RibbonBarStateColorTable;
	}

	private static Office2007ButtonItemColorTable _21BA(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21B1._21BA(ቖ);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(16777215);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21BD(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _12C7(ቖ);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(16777215);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _12C7(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(16238656), ቖ.GetColor(16307239));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16775910), ቖ.GetColor(16777200));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16772776), ቖ.GetColor(16769916));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16769916), ቖ.GetColor(16776416));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(15253582), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16775912), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(4605510);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(16238400));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12744235), ቖ.GetColor(12754503));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16173159));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16504440), ቖ.GetColor(16770186));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16770186), ቖ.GetColor(16704135));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(15253582), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16775912), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(4605510);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12749360), ቖ.GetColor(12756045));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16770421), Color.Transparent);
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16770421), ቖ.GetColor(16767083));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16767083), ቖ.GetColor(16772217));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16774272), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(4605510);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12749360), ቖ.GetColor(12756045));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16770421), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16770421), ቖ.GetColor(16767083));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16767083), ቖ.GetColor(16772474));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16774272), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(4605510);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ወ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _12C7(ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14869218), ቖ.GetColor(14277081));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(14277081), ቖ.GetColor(12895428));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9539985));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16119285), ቖ.GetColor(15527148));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15066597), ቖ.GetColor(15198183));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12369084), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(9276813);
		return office2007ButtonItemColorTable;
	}

	public static void ዄ(Office2007ButtonItemColorTable ዅ, ColorFactory ቖ)
	{
		Color color = ቖ.GetColor(6182461);
		Color empty = Color.Empty;
		ዅ.Default.ExpandBackground = color;
		ዅ.Default.ExpandLight = empty;
		ዅ.Checked.ExpandBackground = color;
		ዅ.Checked.ExpandLight = empty;
		ዅ.Expanded.ExpandBackground = color;
		ዅ.Expanded.ExpandLight = empty;
		ዅ.MouseOver.ExpandBackground = color;
		ዅ.MouseOver.ExpandLight = empty;
		ዅ.Pressed.ExpandBackground = color;
		ዅ.Pressed.ExpandLight = empty;
	}

	public static Office2007ButtonItemColorTable _21B5(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(2500134));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(10987431));
		office2007ButtonItemColorTable.MouseOver.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(9013641), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(8289918), ቖ.GetColor(5329233));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(2500134));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(2631720));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(4079166));
		office2007ButtonItemColorTable.Pressed.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(4737096), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(4737096));
		office2007ButtonItemColorTable.Pressed.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(3026478));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(7105642));
		office2007ButtonItemColorTable.Checked.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(8289918), Color.Transparent);
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(5987163), ቖ.GetColor(5592405));
		office2007ButtonItemColorTable.Checked.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Checked;
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable _21BE(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.GlassText = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(13619151), ቖ.GetColor(13224393));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(6184542));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13619151), ቖ.GetColor(13224393));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(6184542));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(9145486), ቖ.GetColor(5723991));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(11711154), ቖ.GetColor(10790052));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(6184542));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(16777215);
		office2007RibbonTabItemColorTable.MouseOver.GlassText = ቖ.GetColor(0);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable _21BF(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007RibbonTabItemColorTable.Default.GlassText = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(12165809), ቖ.GetColor(13224393));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13158344), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11559317), ቖ.GetColor(10235767));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(12165809), ቖ.GetColor(13224393));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13158344), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11559317), ቖ.GetColor(10235767));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(10911896), ቖ.GetColor(5854295));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(11898021), ቖ.GetColor(9207944));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10235767));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(16777215);
		office2007RibbonTabItemColorTable.MouseOver.GlassText = ቖ.GetColor(0);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዓ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007RibbonTabItemColorTable.Default.GlassText = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(11583660), ቖ.GetColor(13224393));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13158344), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5807443), ቖ.GetColor(1143569));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(11583660), ቖ.GetColor(13224393));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13158344), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5807443), ቖ.GetColor(1143569));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(8692860), ቖ.GetColor(5528147));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(9286786), ቖ.GetColor(8295541));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(32256), ቖ.GetColor(32256));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(16777215);
		office2007RibbonTabItemColorTable.MouseOver.GlassText = ቖ.GetColor(0);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዔ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007RibbonTabItemColorTable.Default.GlassText = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(12233636), ቖ.GetColor(13224393));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13158344), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10511915), ቖ.GetColor(11032064));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(12233636), ቖ.GetColor(13224393));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13158344), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10511915), ቖ.GetColor(11032064));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(11442027), ቖ.GetColor(6117710));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(12692322), ቖ.GetColor(10392922));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11032064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(16777215);
		office2007RibbonTabItemColorTable.MouseOver.GlassText = ቖ.GetColor(0);
		return office2007RibbonTabItemColorTable;
	}

	public static ElementStyle ዸ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.PaddingBottom = 3;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 22;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BorderWidth = 1;
		elementStyle.BorderColor = Color.Transparent;
		elementStyle.CornerDiameter = 3;
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBackgroundBlend.Count];
		menu.FileBackgroundBlend._0CD0(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle ዹ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BorderBottom = eStyleBorderType.Double;
		elementStyle.BorderBottomWidth = 0;
		elementStyle.BorderColor = menu.FileContainerBorder;
		elementStyle.BorderLeft = eStyleBorderType.None;
		elementStyle.BorderLeftWidth = 0;
		elementStyle.BorderRight = eStyleBorderType.None;
		elementStyle.BorderRightWidth = 0;
		elementStyle.BorderTop = eStyleBorderType.Solid;
		elementStyle.BorderTopWidth = 1;
		elementStyle.BorderBottom = eStyleBorderType.Solid;
		elementStyle.BorderBottomWidth = 1;
		elementStyle.PaddingBottom = 0;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 0;
		return elementStyle;
	}

	public static ElementStyle ዺ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnOneBackground;
		elementStyle.BorderRight = eStyleBorderType.Solid;
		elementStyle.BorderRightColor = menu.FileColumnOneBorder;
		elementStyle.BorderRightWidth = 1;
		elementStyle.PaddingRight = 1;
		return elementStyle;
	}

	public static ElementStyle ዻ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnTwoBackground;
		return elementStyle;
	}

	public static ElementStyle ዼ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
		_ = ቯ.Menu;
		elementStyle.MarginTop = 2;
		elementStyle.MarginRight = 2;
		return elementStyle;
	}

	public static void ዱ(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.ScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(15724527), ቖ.GetColor(16579836), 0);
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(15461871), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15922163), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15790320), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(15461356), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(15132648), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11975359), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(9408399)), Color.FromArgb(64, Color.White));
		@default = ዲ.ScrollBar.MouseOver;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16777215), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16580093), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11975359));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16777215), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16580093), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11449272), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(9408399));
		@default = ዲ.ScrollBar.MouseOverControl;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16777215), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16580093), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11975359));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16777215), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16580093), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15922164), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11449272), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(9408399));
		@default = ዲ.ScrollBar.Pressed;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(14803683), 0f),
			new BackgroundColorBlend(ቖ.GetColor(14803683), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11449272));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(8947849));
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(14803683), 0f),
			new BackgroundColorBlend(ቖ.GetColor(14803683), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(11449272), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(9408399));
		@default = ዲ.ScrollBar.Disabled;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void _21C0(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.AppScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(6513507), ቖ.GetColor(6842472), 0);
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(5131854));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(3421236), ቖ.GetColor(2039583), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(ቖ.GetColor(7631988), 0f),
			new BackgroundColorBlend(ቖ.GetColor(6316128), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(5855577), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(2368548), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(10000536));
		@default = ዲ.AppScrollBar.MouseOver;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(8355711), 0f),
			new BackgroundColorBlend(ቖ.GetColor(6447714), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(8355711), ቖ.GetColor(7237230));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(2368548), Color.Empty);
		@default.ThumbSignBackground = ዲ.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = @default.ThumbInnerBorder;
		@default.TrackOuterBorder = @default.ThumbOuterBorder;
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(10000536));
		@default = ዲ.AppScrollBar.MouseOverControl;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.CopyFrom(ዲ.AppScrollBar.Default.TrackBackground);
		@default.ThumbInnerBorder = ዲ.AppScrollBar.Default.TrackInnerBorder;
		@default.ThumbOuterBorder = ዲ.AppScrollBar.Default.TrackOuterBorder;
		@default.ThumbSignBackground = ዲ.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(ዲ.AppScrollBar.Default.TrackBackground);
		@default.TrackInnerBorder = ዲ.AppScrollBar.Default.TrackInnerBorder;
		@default.TrackOuterBorder = ዲ.AppScrollBar.Default.TrackOuterBorder;
		@default.TrackSignBackground = ዲ.AppScrollBar.Default.TrackSignBackground;
		@default = ዲ.AppScrollBar.Pressed;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(7039851), 0f),
			new BackgroundColorBlend(ቖ.GetColor(5131854), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(2368548), Color.Empty);
		@default.ThumbSignBackground = ዲ.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(2368548), Color.Empty);
		@default.TrackSignBackground = ዲ.AppScrollBar.Default.TrackSignBackground;
		@default = ዲ.AppScrollBar.Disabled;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	private static Office2007SliderColorTable _21C1(ColorFactory ቖ)
	{
		Office2007SliderColorTable office2007SliderColorTable = _21C2(ቖ);
		office2007SliderColorTable.Default.LabelColor = ቖ.GetColor(16777215);
		office2007SliderColorTable.MouseOver.LabelColor = ቖ.GetColor(16777215);
		office2007SliderColorTable.Pressed.LabelColor = ቖ.GetColor(16777215);
		return office2007SliderColorTable;
	}

	private static Office2007SliderColorTable _21C2(ColorFactory ቖ)
	{
		Office2007SliderColorTable office2007SliderColorTable = new Office2007SliderColorTable();
		office2007SliderColorTable.Default.LabelColor = ቖ.GetColor(0);
		office2007SliderColorTable.Default.SliderLabelColor = ቖ.GetColor(0);
		office2007SliderColorTable.Default.PartBackground = new GradientColorTable();
		office2007SliderColorTable.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12632256), 0f));
		office2007SliderColorTable.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(8158332), 1f));
		office2007SliderColorTable.Default.PartBorderColor = ቖ.GetColor(2500134);
		office2007SliderColorTable.Default.PartBorderLightColor = Color.Transparent;
		office2007SliderColorTable.Default.PartForeColor = ቖ.GetColor(3355443);
		office2007SliderColorTable.Default.PartForeLightColor = Color.Empty;
		office2007SliderColorTable.Default.TrackLineColor = ቖ.GetColor(2500134);
		office2007SliderColorTable.Default.TrackLineLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		office2007SliderColorTable.MouseOver.LabelColor = ቖ.GetColor(0);
		office2007SliderColorTable.MouseOver.SliderLabelColor = ቖ.GetColor(0);
		office2007SliderColorTable.MouseOver.PartBackground = new GradientColorTable();
		office2007SliderColorTable.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16777215), 0f));
		office2007SliderColorTable.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16776693), 0.2f));
		office2007SliderColorTable.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16768899), 0.5f));
		office2007SliderColorTable.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14526221), 0.5f));
		office2007SliderColorTable.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16774350), 0.85f));
		office2007SliderColorTable.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16774350), 1f));
		office2007SliderColorTable.MouseOver.PartBorderColor = ቖ.GetColor(8358292);
		office2007SliderColorTable.MouseOver.PartBorderLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		office2007SliderColorTable.MouseOver.PartForeColor = ቖ.GetColor(5132370);
		office2007SliderColorTable.MouseOver.PartForeLightColor = Color.Empty;
		office2007SliderColorTable.MouseOver.TrackLineColor = ቖ.GetColor(2500134);
		office2007SliderColorTable.MouseOver.TrackLineLightColor = ቖ.GetColor(13421772);
		office2007SliderColorTable.Pressed.LabelColor = ቖ.GetColor(0);
		office2007SliderColorTable.Pressed.SliderLabelColor = ቖ.GetColor(0);
		office2007SliderColorTable.Pressed.PartBackground = new GradientColorTable();
		office2007SliderColorTable.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14382360), 0f));
		office2007SliderColorTable.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16224303), 0.2f));
		office2007SliderColorTable.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16368011), 0.5f));
		office2007SliderColorTable.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15561728), 0.5f));
		office2007SliderColorTable.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16761224), 0.85f));
		office2007SliderColorTable.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15913653), 1f));
		office2007SliderColorTable.Pressed.PartBorderColor = ቖ.GetColor(8358292);
		office2007SliderColorTable.Pressed.PartBorderLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		office2007SliderColorTable.Pressed.PartForeColor = ቖ.GetColor(5132370);
		office2007SliderColorTable.Pressed.PartForeLightColor = Color.Empty;
		office2007SliderColorTable.Pressed.TrackLineColor = ቖ.GetColor(2500134);
		office2007SliderColorTable.Pressed.TrackLineLightColor = ቖ.GetColor(13421772);
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		office2007SliderColorTable.Disabled.LabelColor = ቖ.GetColor(9276813);
		office2007SliderColorTable.Disabled.SliderLabelColor = ቖ.GetColor(9276813);
		office2007SliderColorTable.Disabled.PartBackground = new GradientColorTable();
		foreach (BackgroundColorBlend color in office2007SliderColorTable.Default.PartBackground.Colors)
		{
			office2007SliderColorTable.Disabled.PartBackground.Colors.Add(new BackgroundColorBlend(colorBlendFactory.GetColor(color.Color), color.Position));
		}
		office2007SliderColorTable.Disabled.PartBorderColor = colorBlendFactory.GetColor(office2007SliderColorTable.Default.PartBorderColor);
		office2007SliderColorTable.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(office2007SliderColorTable.Default.PartBorderLightColor);
		office2007SliderColorTable.Disabled.PartForeColor = colorBlendFactory.GetColor(office2007SliderColorTable.Default.PartForeColor);
		office2007SliderColorTable.Disabled.PartForeLightColor = colorBlendFactory.GetColor(office2007SliderColorTable.Default.PartForeLightColor);
		office2007SliderColorTable.Disabled.TrackLineColor = colorBlendFactory.GetColor(office2007SliderColorTable.Default.TrackLineColor);
		office2007SliderColorTable.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(office2007SliderColorTable.Default.TrackLineLightColor);
		return office2007SliderColorTable;
	}
}
internal static class _21C3
{
	public static void _21B2(Office2007ColorTable ቯ, ColorFactory ቖ)
	{
		ቯ.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010NormalSilver.png");
		ቯ.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010HotSilver.png");
		ቯ.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.ই("SystemImages.BlankOffice2010PressedSilver.png");
		ቯ.RibbonControl.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(8950952));
		ቯ.RibbonControl.InnerBorder = null;
		ቯ.RibbonControl.TabsBackground = new LinearGradientColorTable(ቖ.GetColor(12371928));
		ቯ.RibbonControl.TabsGlassBackground = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(12371928));
		ቯ.RibbonControl.TabDividerBorder = Color.Empty;
		ቯ.RibbonControl.TabDividerBorderLight = Color.Empty;
		ቯ.RibbonControl.CornerSize = 1;
		ቯ.RibbonControl.PanelTopBackgroundHeight = 0f;
		ቯ.RibbonControl.PanelTopBackground = new LinearGradientColorTable(ቖ.GetColor(13292514), ቖ.GetColor(11450829));
		ቯ.RibbonControl.PanelBottomBackground = null;
		ቯ.ItemGroup.OuterBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.TopBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.ItemGroup.ItemGroupDividerDark = Color.Empty;
		ቯ.ItemGroup.ItemGroupDividerLight = Color.Empty;
		ቯ.RibbonBar.Default = ᱬ(ቖ);
		ቯ.RibbonBar.MouseOver = _21B3(ቖ);
		ቯ.RibbonBar.Expanded = _21B4(ቖ);
		ቯ.RibbonButtonItemColors.Clear();
		ቯ.ButtonItemColors.Clear();
		ቯ.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _12C7(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ወ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዉ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዊ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዋ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዌ(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ው(ቖ);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		ቯ.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(ቖ));
		ቯ.BackstageButtonItemColors.Add(_21BA(ቖ));
		ቯ.NavigationPaneButtonItemColors.Clear();
		ቯ.NavigationPaneButtonItemColors.Add(_21B5(ቖ));
		ቯ.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = ዑ(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዒ(ቖ);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዓ(ቖ);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዔ(ቖ);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		ቯ.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = _21B6(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B7(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B8(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B9(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		ቯ.Bar.ToolbarTopBackground = new LinearGradientColorTable(ቖ.GetColor(12371928));
		ቯ.Bar.ToolbarBottomBackground = new LinearGradientColorTable(ቖ.GetColor(12371928));
		ቯ.Bar.ToolbarBottomBorder = ቖ.GetColor(10267329);
		ቯ.Bar.PopupToolbarBackground = new LinearGradientColorTable(ቖ.GetColor(15198957), Color.Empty);
		ቯ.Bar.PopupToolbarBorder = ቖ.GetColor(10201015);
		ቯ.Bar.StatusBarTopBorder = ቖ.GetColor(10267329);
		ቯ.Bar.StatusBarTopBorderLight = Color.Empty;
		ቯ.Bar.StatusBarAltBackground.Clear();
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(10267329), 0f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(10267329), 1f));
		ቯ.Menu.Background = new LinearGradientColorTable(ቖ.GetColor(15330542), ቖ.GetColor(13686754));
		ቯ.Menu.Border = new LinearGradientColorTable(ቖ.GetColor(10201015), Color.Empty);
		ቯ.Menu.Side = new LinearGradientColorTable(ቖ.GetColor(15330542), Color.Empty);
		ቯ.Menu.SideBorder = LinearGradientColorTable.Empty;
		ቯ.Menu.SideBorderLight = LinearGradientColorTable.Empty;
		ቯ.Menu.SideUnused = new LinearGradientColorTable(ቖ.GetColor(12371928), Color.Empty);
		ቯ.Menu.FileBackgroundBlend.Clear();
		ቯ.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(15330542), 0f),
			new BackgroundColorBlend(ቖ.GetColor(13686754), 1f)
		});
		ቯ.Menu.FileContainerBorder = ቖ.GetColor(10201015);
		ቯ.Menu.FileContainerBorderLight = Color.Transparent;
		ቯ.Menu.FileColumnOneBackground = ቖ.GetColor(15330542);
		ቯ.Menu.FileColumnOneBorder = ቖ.GetColor(12501963);
		ቯ.Menu.FileColumnTwoBackground = Color.Empty;
		ቯ.Menu.FileBottomContainerBackgroundBlend.Clear();
		ቯ.ComboBox.Default.Background = ቖ.GetColor(15856632);
		int color = 8753570;
		ቯ.ComboBox.Default.Border = ቖ.GetColor(color);
		ቯ.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandText = ቖ.GetColor(5922145);
		ቯ.ComboBox.DefaultStandalone.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.DefaultStandalone.Border = ቖ.GetColor(color);
		ቯ.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(13292514), ቖ.GetColor(11450829), 90);
		ቯ.ComboBox.DefaultStandalone.ExpandBorderInner = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(11254480), Color.Empty, 90);
		ቯ.ComboBox.DefaultStandalone.ExpandText = ቖ.GetColor(1780030);
		ቯ.ComboBox.MouseOver.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.MouseOver.Border = ቖ.GetColor(15057765);
		ቯ.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(16776434), ቖ.GetColor(16772277), 90);
		ቯ.ComboBox.MouseOver.ExpandBorderInner = LinearGradientColorTable.Empty;
		ቯ.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(15057765), Color.Empty, 90);
		ቯ.ComboBox.MouseOver.ExpandText = ቖ.GetColor(0);
		ቯ.ComboBox.DroppedDown.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.DroppedDown.Border = ቖ.GetColor(15057765);
		ቯ.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(16771238), ቖ.GetColor(16771238), 90);
		ቯ.ComboBox.DroppedDown.ExpandBorderInner = LinearGradientColorTable.Empty;
		ቯ.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(15057765));
		ቯ.ComboBox.DroppedDown.ExpandText = ቖ.GetColor(0);
		ቯ.DialogLauncher.Default.DialogLauncher = ቖ.GetColor(6120816);
		ቯ.DialogLauncher.Default.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.MouseOver.DialogLauncher = ቖ.GetColor(6120816);
		ቯ.DialogLauncher.MouseOver.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16776177), ቖ.GetColor(16773057));
		ቯ.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16773057), ቖ.GetColor(16772277));
		ቯ.DialogLauncher.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15057765), Color.Empty);
		ቯ.DialogLauncher.Pressed.DialogLauncher = ቖ.GetColor(6120816);
		ቯ.DialogLauncher.Pressed.DialogLauncherShade = Color.Empty;
		ቯ.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14993573), ቖ.GetColor(15583377));
		ቯ.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15645794), ቖ.GetColor(15656075));
		ቯ.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13942696), ቖ.GetColor(15724994));
		ቯ.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12751419), ቖ.GetColor(12760162));
		ቯ.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(15397110));
		ቯ.SystemButton.Default.LightShade = Color.Empty;
		ቯ.SystemButton.Default.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(15594233));
		ቯ.SystemButton.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButton.MouseOver.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13292514), ቖ.GetColor(12437463));
		ቯ.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(12437463), ቖ.GetColor(11450829));
		ቯ.SystemButton.MouseOver.TopHighlight = null;
		ቯ.SystemButton.MouseOver.BottomHighlight = null;
		ቯ.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10201015));
		ቯ.SystemButton.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(15331318));
		ቯ.SystemButton.Pressed.LightShade = Color.Empty;
		ቯ.SystemButton.Pressed.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(11450829));
		ቯ.SystemButton.Pressed.TopHighlight = null;
		ቯ.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11450829));
		ቯ.SystemButton.Pressed.BottomHighlight = null;
		ቯ.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9414079));
		ቯ.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(11387354));
		ቯ.SystemButtonClose = new Office2007SystemButtonColorTable();
		ቯ.SystemButtonClose.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(15397110));
		ቯ.SystemButtonClose.Default.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Default.DarkShade = ቖ.GetColor(5395813);
		ቯ.SystemButtonClose.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(16570327));
		ቯ.SystemButtonClose.MouseOver.LightShade = Color.Empty;
		ቯ.SystemButtonClose.MouseOver.DarkShade = ቖ.GetColor(10173757);
		ቯ.SystemButtonClose.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16745602), ቖ.GetColor(16482174));
		ቯ.SystemButtonClose.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16152953), ቖ.GetColor(14901602));
		ቯ.SystemButtonClose.MouseOver.TopHighlight = null;
		ቯ.SystemButtonClose.MouseOver.BottomHighlight = null;
		ቯ.SystemButtonClose.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10173757));
		ቯ.SystemButtonClose.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		ቯ.SystemButtonClose.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButtonClose.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(16307413));
		ቯ.SystemButtonClose.Pressed.LightShade = Color.Empty;
		ቯ.SystemButtonClose.Pressed.DarkShade = ቖ.GetColor(10173757);
		ቯ.SystemButtonClose.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15890294));
		ቯ.SystemButtonClose.Pressed.TopHighlight = null;
		ቯ.SystemButtonClose.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15890294));
		ቯ.SystemButtonClose.Pressed.BottomHighlight = null;
		ቯ.SystemButtonClose.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10173757));
		ቯ.SystemButtonClose.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14311779));
		ቯ.Form.Active.BorderColors = new Color[5]
		{
			ቖ.GetColor(9476774),
			ቖ.GetColor(13952757),
			ቖ.GetColor(12308198),
			ቖ.GetColor(12308198),
			ቖ.GetColor(12308198)
		};
		ቯ.Form.Inactive.BorderColors = new Color[5]
		{
			ቖ.GetColor(10661305),
			ቖ.GetColor(12308198),
			ቖ.GetColor(12308198),
			ቖ.GetColor(12308198),
			ቖ.GetColor(12308198)
		};
		ቯ.Form.Active.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(12371928), ቖ.GetColor(12371928));
		ቯ.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(12371928), ቖ.GetColor(12371928));
		ቯ.Form.Active.CaptionBottomBorder = new Color[2]
		{
			ቖ.GetColor(10267329),
			ቖ.GetColor(13226721)
		};
		ቯ.Form.Active.CaptionText = ቖ.GetColor(1780030);
		ቯ.Form.Active.CaptionTextExtra = ቖ.GetColor(1780030);
		ቯ.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(13226721));
		ቯ.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(13226721));
		ቯ.Form.Inactive.CaptionText = ቖ.GetColor(5654106);
		ቯ.Form.Inactive.CaptionTextExtra = ቖ.GetColor(5654106);
		ቯ.Form.BackColor = ቖ.GetColor(13686754);
		ቯ.Form.TextColor = ቖ.GetColor(1780030);
		ቯ.Form.MdiClientBackgroundImage = (Image)(object)BarFunctions.ই("SystemImages.VS2010ClientBackground.png");
		ቯ.QuickAccessToolbar.Active.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Active.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.TopBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		ቯ.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(12371928));
		ቯ.QuickAccessToolbar.Standalone.OutterBorderColor = ቖ.GetColor(10267329);
		ቯ.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelBackground = ቖ.GetColor(13292514);
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelText = ቖ.GetColor(1780030);
		ቯ.QuickAccessToolbar.Active.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.QuickAccessToolbar.Inactive.GlassBorder = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Default.TopBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default.BottomBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.Default.InnerBorder = Color.Empty;
		ቯ.TabControl.Default.OuterBorder = Color.Empty;
		ቯ.TabControl.Default.Text = ቖ.GetColor(16777215);
		ቯ.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(7305078), ቖ.GetColor(6384757));
		ቯ.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(6121845), ቖ.GetColor(5201780));
		ቯ.TabControl.MouseOver.InnerBorder = Color.Empty;
		ቯ.TabControl.MouseOver.OuterBorder = ቖ.GetColor(10201015);
		ቯ.TabControl.MouseOver.Text = ቖ.GetColor(16777215);
		ቯ.TabControl.Selected = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16776434), ቖ.GetColor(16774095));
		ቯ.TabControl.Selected.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16771238), ቖ.GetColor(16771238));
		ቯ.TabControl.Selected.InnerBorder = Color.Empty;
		ቯ.TabControl.Selected.OuterBorder = Color.Empty;
		ቯ.TabControl.Selected.Text = ቖ.GetColor(0);
		ቯ.TabControl.TabBackground = LinearGradientColorTable.Empty;
		ቯ.TabControl.TabBackgroundImage = (Image)(object)BarFunctions.ই("SystemImages.VS2010ClientBackground.png");
		ቯ.TabControl.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		ቯ.TabControl.TabPanelBorder = ቖ.GetColor(color);
		Office2007CheckBoxColorTable checkBoxItem = ቯ.CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16053492), Color.Empty);
		checkBoxItem.Default.CheckBorder = ቖ.GetColor(11256286);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(15264235), ቖ.GetColor(15921907));
		checkBoxItem.Default.CheckInnerBorder = ቖ.GetColor(12502220);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4877206), Color.Empty);
		checkBoxItem.Default.Text = ቖ.GetColor(1780030);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16576962), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = ቖ.GetColor(13602871);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(16444616), ቖ.GetColor(16643812));
		checkBoxItem.MouseOver.CheckInnerBorder = ቖ.GetColor(16559453);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4877206), Color.Empty);
		checkBoxItem.MouseOver.Text = ቖ.GetColor(1780030);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16574397));
		checkBoxItem.Pressed.CheckBorder = ቖ.GetColor(12741917);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(16441506), ቖ.GetColor(16642775));
		checkBoxItem.Pressed.CheckInnerBorder = ቖ.GetColor(16557400);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(ቖ.GetColor(4877206), Color.Empty);
		checkBoxItem.Pressed.Text = ቖ.GetColor(1780030);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = ቖ.GetColor(11974842);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(ቖ.GetColor(16316665));
		checkBoxItem.Disabled.CheckInnerBorder = ቖ.GetColor(14672096);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(ቖ.GetColor(11645876), Color.Empty);
		checkBoxItem.Disabled.Text = ቖ.GetColor(8884895);
		ዱ(ቯ, ቖ);
		ዳ(ቯ, ቖ);
		Office2007ProgressBarColorTable progressBarItem = ቯ.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(7247820);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
		progressBarItem.Chunk = new GradientColorTable(6918698, 15200980, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(15663063)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(9286228)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(6918699)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = ቯ.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(7247820);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
		progressBarItem.Chunk = new GradientColorTable(11446016, 16776653, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16776099)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13814272)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16708608)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = ቯ.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(15461872, 14014684);
		progressBarItem.OuterBorder = ቖ.GetColor(7247820);
		progressBarItem.InnerBorder = ቖ.GetColor(16777215);
		progressBarItem.Chunk = new GradientColorTable(13762560, 16764365, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16748431)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13762560)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16646144)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		Office2007GalleryColorTable gallery = ቯ.Gallery;
		gallery.GroupLabelBackground = ቖ.GetColor(15790837);
		gallery.GroupLabelText = ቖ.GetColor(1780030);
		gallery.GroupLabelBorder = ቖ.GetColor(15790837);
		ቯ.LegacyColors.BarBackground = ቖ.GetColor(12371928);
		ቯ.LegacyColors.BarBackground2 = Color.Empty;
		ቯ.LegacyColors.BarStripeColor = ቖ.GetColor(6320780);
		ቯ.LegacyColors.BarCaptionBackground = ቖ.GetColor(16776434);
		ቯ.LegacyColors.BarCaptionBackground2 = ቖ.GetColor(16771238);
		ቯ.LegacyColors.BarCaptionText = ቖ.GetColor(0);
		ቯ.LegacyColors.BarCaptionInactiveBackground = ቖ.GetColor(5070978);
		ቯ.LegacyColors.BarCaptionInactiveBackground2 = ቖ.GetColor(4018807);
		ቯ.LegacyColors.BarCaptionInactiveText = ቖ.GetColor(16777215);
		ቯ.LegacyColors.BarFloatingBorder = ቖ.GetColor(2833497);
		ቯ.LegacyColors.BarPopupBackground = ቖ.GetColor(16777215);
		ቯ.LegacyColors.BarPopupBorder = ቖ.GetColor(10201015);
		ቯ.LegacyColors.ItemBackground = Color.Empty;
		ቯ.LegacyColors.ItemBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemCheckedBackground = ቖ.GetColor(16773051);
		ቯ.LegacyColors.ItemCheckedBackground2 = ቖ.GetColor(16773051);
		ቯ.LegacyColors.ItemCheckedBorder = ቖ.GetColor(15057765);
		ቯ.LegacyColors.ItemCheckedText = ቖ.GetColor(1780030);
		ቯ.LegacyColors.ItemDisabledBackground = Color.Empty;
		ቯ.LegacyColors.ItemDisabledText = ቖ.GetColor(9276813);
		ቯ.LegacyColors.ItemExpandedShadow = Color.Empty;
		ቯ.LegacyColors.ItemExpandedBackground = ቖ.GetColor(15330542);
		ቯ.LegacyColors.ItemExpandedBackground2 = Color.Empty;
		ቯ.LegacyColors.ItemExpandedText = ቖ.GetColor(1780030);
		ቯ.LegacyColors.ItemExpandedBorder = ቖ.GetColor(1780030);
		ቯ.LegacyColors.ItemHotBackground = ቖ.GetColor(16776434);
		ቯ.LegacyColors.ItemHotBackground2 = ቖ.GetColor(16772277);
		ቯ.LegacyColors.ItemHotBorder = ቖ.GetColor(10201015);
		ቯ.LegacyColors.ItemHotText = ቖ.GetColor(1780030);
		ቯ.LegacyColors.ItemPressedBackground = ቖ.GetColor(16776434);
		ቯ.LegacyColors.ItemPressedBackground2 = ቖ.GetColor(16772277);
		ቯ.LegacyColors.ItemPressedBorder = ቖ.GetColor(15057765);
		ቯ.LegacyColors.ItemPressedText = ቖ.GetColor(1780030);
		ቯ.LegacyColors.ItemSeparator = ቖ.GetColor(12501963);
		ቯ.LegacyColors.ItemSeparatorShade = Color.Empty;
		ቯ.LegacyColors.ItemText = ቖ.GetColor(1780030);
		ቯ.LegacyColors.MenuBackground = ቖ.GetColor(15330542);
		ቯ.LegacyColors.MenuBackground2 = ቖ.GetColor(13686754);
		ቯ.LegacyColors.MenuBarBackground = ቖ.GetColor(13292514);
		ቯ.LegacyColors.MenuBarBackground2 = ቖ.GetColor(11450829);
		ቯ.LegacyColors.MenuBorder = ቖ.GetColor(10201015);
		ቯ.LegacyColors.ItemExpandedBorder = ቯ.LegacyColors.MenuBorder;
		ቯ.LegacyColors.MenuSide = ቖ.GetColor(15330542);
		ቯ.LegacyColors.MenuSide2 = Color.Empty;
		ቯ.LegacyColors.MenuUnusedBackground = ቯ.LegacyColors.MenuBackground;
		ቯ.LegacyColors.MenuUnusedSide = ቖ.GetColor(12371928);
		ቯ.LegacyColors.MenuUnusedSide2 = Color.Empty;
		ቯ.LegacyColors.ItemDesignTimeBorder = Color.DarkGray;
		ቯ.LegacyColors.BarDockedBorder = ቖ.GetColor(10267329);
		ቯ.LegacyColors.DockSiteBackColor = ቖ.GetColor(10267329);
		ቯ.LegacyColors.DockSiteBackColor2 = Color.Empty;
		ቯ.LegacyColors.CustomizeBackground = ቖ.GetColor(14015720);
		ቯ.LegacyColors.CustomizeBackground2 = Color.Empty;
		ቯ.LegacyColors.CustomizeText = ቖ.GetColor(1780030);
		ቯ.LegacyColors.PanelBackground = ቖ.GetColor(13292514);
		ቯ.LegacyColors.PanelBackground2 = ቖ.GetColor(11450829);
		ቯ.LegacyColors.PanelText = ቖ.GetColor(1780030);
		ቯ.LegacyColors.PanelBorder = ቖ.GetColor(color);
		ቯ.LegacyColors.ExplorerBarBackground = ቖ.GetColor(5070978);
		ቯ.LegacyColors.ExplorerBarBackground2 = ቖ.GetColor(4018807);
		ቯ.LegacyColors.AutoHidePanelBackgroundImage = ቯ.Form.MdiClientBackgroundImage;
		ቯ.LegacyColors.AutoHidePanelBackground = Color.Transparent;
		ቯ.LegacyColors.AutoHideTabBackground = ቖ.GetColor(4939393);
		ቯ.LegacyColors.AutoHideTabText = ቖ.GetColor(16777215);
		ቯ.LegacyColors.AutoHideTabBorder = Color.Transparent;
		ቯ.LegacyColors.AutoHideSelectedTabBackground = ቖ.GetColor(5004404);
		ቯ.LegacyColors.AutoHideSelectedTabBackground2 = ቖ.GetColor(7305078);
		ቯ.LegacyColors.AutoHideSelectedTabText = ቖ.GetColor(16777215);
		ቯ.LegacyColors.AutoHideSelectedTabBorder = ቖ.GetColor(10201015);
		ቯ.LegacyColors.SplitterBackground = ቖ.GetColor(14412799);
		ቯ.LegacyColors.SplitterBackground2 = ቖ.GetColor(11846870);
		ቯ.LegacyColors.SplitterBorder = ቖ.GetColor(8756927);
		ቯ.LegacyColors.SplitterText = ቯ.LegacyColors.SplitterBorder;
		ቯ.NavigationPane.ButtonBackground = new GradientColorTable();
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12371928), 0f));
		ቯ.SuperTooltip.BackgroundColors = new LinearGradientColorTable(ቖ.GetColor(16645630), ቖ.GetColor(15001072));
		ቯ.SuperTooltip.TextColor = ቖ.GetColor(5000268);
		Office2007SliderColorTable slider = ቯ.Slider;
		slider.Default.LabelColor = ቖ.GetColor(1780030);
		slider.Default.SliderLabelColor = ቖ.GetColor(1780030);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16251902), 0f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14937336), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12372701), 1f));
		slider.Default.PartBorderColor = ቖ.GetColor(7767709);
		slider.Default.PartBorderLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		slider.Default.PartForeColor = ቖ.GetColor(8290951);
		slider.Default.PartForeLightColor = Color.Empty;
		slider.Default.TrackLineColor = ቖ.GetColor(9084088);
		slider.Default.TrackLineLightColor = ቖ.GetColor(14609143);
		slider.MouseOver.LabelColor = ቖ.GetColor(1780030);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(1780030);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16180380), 0f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16180634), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16446933), 1f));
		slider.MouseOver.PartBorderColor = ቖ.GetColor(15581758);
		slider.MouseOver.PartBorderLightColor = Color.FromArgb(96, ቖ.GetColor(16777215));
		slider.MouseOver.PartForeColor = ቖ.GetColor(8290951);
		slider.MouseOver.PartForeLightColor = Color.Empty;
		slider.MouseOver.TrackLineColor = ቖ.GetColor(9084088);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(14609143);
		slider.Pressed.LabelColor = ቖ.GetColor(1780030);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(1780030);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16504439), 0f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770185), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770185), 1f));
		slider.Pressed.PartBorderColor = ቖ.GetColor(12746038);
		slider.Pressed.PartBorderLightColor = ቖ.GetColor(15843679);
		slider.Pressed.PartForeColor = ቖ.GetColor(8290951);
		slider.Pressed.PartForeLightColor = Color.Empty;
		slider.Pressed.TrackLineColor = ቖ.GetColor(9084088);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(14609143);
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(9276813);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(9276813);
		slider.Disabled.PartBackground = new GradientColorTable();
		foreach (BackgroundColorBlend color2 in slider.Default.PartBackground.Colors)
		{
			slider.Disabled.PartBackground.Colors.Add(new BackgroundColorBlend(colorBlendFactory.GetColor(color2.Color), color2.Position));
		}
		slider.Disabled.PartBorderColor = colorBlendFactory.GetColor(slider.Default.PartBorderColor);
		slider.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(slider.Default.PartBorderLightColor);
		slider.Disabled.PartForeColor = colorBlendFactory.GetColor(slider.Default.PartForeColor);
		slider.Disabled.PartForeLightColor = colorBlendFactory.GetColor(slider.Default.PartForeLightColor);
		slider.Disabled.TrackLineColor = colorBlendFactory.GetColor(slider.Default.TrackLineColor);
		slider.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(slider.Default.TrackLineLightColor);
		Office2007ColorTable.ቮ(ቯ, ቖ);
		ቯ.ListViewEx.Border = ቖ.GetColor(color);
		ቯ.ListViewEx.ColumnBackground = new LinearGradientColorTable(ቖ.GetColor(15726075), ቖ.GetColor(14806266));
		ቯ.ListViewEx.ColumnSeparator = ቖ.GetColor(color);
		ቯ.ListViewEx.ColumnForeColor = ቖ.GetColor(1780030);
		ቯ.ListViewEx.SelectionBackground = new LinearGradientColorTable(ቖ.GetColor(10997232), Color.Empty);
		ቯ.ListViewEx.SelectionBorder = ቖ.GetColor(14938111);
		ቯ.DataGridView.ColumnHeaderNormalBorder = ቖ.GetColor(color);
		ቯ.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(ቖ.GetColor(15726075), ቖ.GetColor(14806266), 90);
		ቯ.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16374175), ቖ.GetColor(15843679), 90);
		ቯ.DataGridView.ColumnHeaderSelectedBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(16766349), ቖ.GetColor(15897146), 90);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(13952247), ቖ.GetColor(12834540), 90);
		ቯ.DataGridView.ColumnHeaderMouseOverBorder = ቖ.GetColor(color);
		ቯ.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(ቖ.GetColor(11914211), ቖ.GetColor(10861783), 90);
		ቯ.DataGridView.ColumnHeaderPressedBorder = ቖ.GetColor(color);
		ቯ.DataGridView.RowNormalBackground = new LinearGradientColorTable(ቖ.GetColor(15002871));
		ቯ.DataGridView.RowNormalBorder = ቖ.GetColor(color);
		ቯ.DataGridView.RowSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16766349));
		ቯ.DataGridView.RowSelectedBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(15843420));
		ቯ.DataGridView.RowSelectedMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(15843420));
		ቯ.DataGridView.RowMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowPressedBackground = new LinearGradientColorTable(ቖ.GetColor(11914211), ቖ.GetColor(10861783), 90);
		ቯ.DataGridView.RowPressedBorder = ቖ.GetColor(color);
		ቯ.DataGridView.GridColor = ቖ.GetColor(11184810);
		ቯ.DataGridView.SelectorBackground = new LinearGradientColorTable(ቖ.GetColor(10861783));
		ቯ.DataGridView.SelectorBorder = ቖ.GetColor(color);
		ቯ.DataGridView.SelectorBorderDark = ቖ.GetColor(12829635);
		ቯ.DataGridView.SelectorBorderLight = ቖ.GetColor(16382457);
		ቯ.DataGridView.SelectorSign = new LinearGradientColorTable(ቖ.GetColor(15921906));
		ቯ.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(9150645));
		ቯ.DataGridView.SelectorMouseOverBorder = ቖ.GetColor(10401486);
		ቯ.DataGridView.SelectorMouseOverBorderDark = ቖ.GetColor(11587575);
		ቯ.DataGridView.SelectorMouseOverBorderLight = ቖ.GetColor(14017778);
		ቯ.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(ቖ.GetColor(16382715), ቖ.GetColor(14146274));
		ቯ.SideBar.Background = new LinearGradientColorTable(ቖ.GetColor(Color.White));
		ቯ.SideBar.Border = ቖ.GetColor(color);
		ቯ.SideBar.SideBarPanelItemText = ቖ.GetColor(1780030);
		ቯ.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15595005), 0f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14214386), 1f));
		ቯ.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16505781), 0f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16697208), 0.4f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16692310), 0.4f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16640927), 1f));
		ቯ.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16776409), 0f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770957), 0.4f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16766792), 0.4f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770963), 1f));
		ቯ.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16300137), 0f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16622433), 0.4f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16484924), 0.4f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16694112), 1f));
		ቯ.AdvTree = new TreeColorTable();
		_135B._135C(ቯ.AdvTree, ቖ);
		ቯ.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(1780030);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(1780030);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor("F9E8A1"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FAF2CD"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("EEC955");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(1780030);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFFFFDEC"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF4CA"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFEBA6"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF2C5"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8E8F8F");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(1780030);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFC59B61"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFEEB469"), 0.1f),
			new BackgroundColorBlend(ቖ.GetColor("FFFCA060"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFFB8E3D"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFFEBB60"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8B7654");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("408B7654");
		ቯ.WarningBox.BackColor = ቖ.GetColor(ቖ.GetColor(13492213));
		ቯ.WarningBox.WarningBorderColor = ቖ.GetColor(color);
		ቯ.WarningBox.WarningBackColor1 = ቖ.GetColor(13096681);
		ቯ.WarningBox.WarningBackColor2 = ቖ.GetColor(11518940);
		ቯ.CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(ቖ.GetColor(color)),
			new ColorDef(ቖ.GetColor(1780030)),
			new ColorDef(ቖ.GetColor(10862561)),
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(10862561)),
			new ColorDef(ቖ.GetColor(15134199)),
			new ColorDef(ቖ.GetColor(10862561)),
			new ColorDef(ቖ.GetColor(14017009)),
			new ColorDef(ቖ.GetColor(2706554)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(13619928),
				ቖ.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(1780030)),
			new ColorDef(ቖ.GetColor(color)),
			new ColorDef(ቖ.GetColor(color)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(ቖ.GetColor(color)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(1780030)),
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(ቖ.GetColor(15134199)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(color)),
			new ColorDef(ቖ.GetColor(10862561)),
			new ColorDef(ቖ.GetColor(2046329)),
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(ቖ.GetColor(15134199)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(10862561)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(13619928),
				ቖ.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2046329)),
			new ColorDef(ቖ.GetColor(16777215))
		};
		ቯ.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(ቖ.GetColor(9285337)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(14739958),
				ቖ.GetColor(11782630)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2642318)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(11650540),
				ቖ.GetColor(7708122)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2909476)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12773564),
				ቖ.GetColor(8704379)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(9125386)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16369568),
				ቖ.GetColor(16029528)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(4073329)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12957158),
				ቖ.GetColor(9796562)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8787740)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(15837868),
				ቖ.GetColor(15034222)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8157204)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16776362),
				ቖ.GetColor(16775512)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(-1)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(8388736))
		};
		ቯ.SuperTab.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(12308198));
		ቯ.SuperTab.InnerBorder = ቖ.GetColor(Color.Transparent);
		ቯ.SuperTab.OuterBorder = ቖ.GetColor(10465991);
		ቯ.SuperTab.ControlBoxDefault.Image = ቖ.GetColor(1780030);
		ቯ.SuperTab.ControlBoxMouseOver.Background = ቖ.GetColor(13561338);
		ቯ.SuperTab.ControlBoxMouseOver.Border = ቖ.GetColor(3381759);
		ቯ.SuperTab.ControlBoxMouseOver.Image = ቖ.GetColor(255, 1780030);
		ቯ.SuperTab.ControlBoxPressed.Background = ቖ.GetColor(12045024);
		ቯ.SuperTab.ControlBoxPressed.Border = ቖ.GetColor(3381759);
		ቯ.SuperTab.ControlBoxPressed.Image = ቖ.GetColor(255, 1780030);
		ቯ.SuperTab.InsertMarker = ቖ.GetColor(255, 0);
		ቯ.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable();
		ቯ.SuperTabItem.Default.Normal.Text = ቖ.GetColor(1780030);
		ቯ.SuperTabItem.Default.Normal.CloseMarker = ቖ.GetColor(1780030);
		ቯ.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(16120575),
			ቖ.GetColor(15726333)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.Selected.InnerBorder = Color.Transparent;
		ቯ.SuperTabItem.Default.Selected.OuterBorder = ቖ.GetColor(10465991);
		ቯ.SuperTabItem.Default.Selected.Text = ቖ.GetColor(1780030);
		ቯ.SuperTabItem.Default.Selected.CloseMarker = ቖ.GetColor(1780030);
		ቯ.SuperTabItem.Default.Disabled.Text = ቖ.GetColor(8884895);
		ቯ.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		ቯ.SuperTabItem.Default.Disabled.CloseMarker = ቖ.GetColor(8884895);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(16120575),
			ቖ.GetColor(15726333)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.SelectedMouseOver.InnerBorder = Color.Transparent;
		ቯ.SuperTabItem.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(10465991);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Text = ቖ.GetColor(1780030);
		ቯ.SuperTabItem.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(1780030);
		ቯ.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(48, 16777215),
			ቖ.GetColor(180, 16777215)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.MouseOver.InnerBorder = ቖ.GetColor(190, 16777215);
		ቯ.SuperTabItem.Default.MouseOver.OuterBorder = ቖ.GetColor(11646394);
		ቯ.SuperTabItem.Default.MouseOver.Text = ቖ.GetColor(1780030);
		ቯ.SuperTabItem.Default.MouseOver.CloseMarker = ቖ.GetColor(1780030);
		ቯ.SuperTabItem.Left = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Bottom = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Right = ቯ.SuperTabItem.Right;
		ቯ.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(15726333));
		ቯ.SuperTabPanel.Default.OuterBorder = ቖ.GetColor(10465991);
		ቯ.SuperTabPanel.Default.InnerBorder = ቖ.GetColor(Color.Transparent);
		ቯ.SuperTabPanel.Left = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Bottom = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Right = ቯ.SuperTabPanel.Default;
		ጦ.ጯ(ቯ.Backstage, ቖ);
		ጦ.ጶ(ቯ.BackstageItem, ቖ);
		ጦ.ጼ(ቯ.BackstagePanel, ቖ);
		ቯ.SwitchButton = new SwitchButtonColors();
		ቯ.SwitchButton.Disabled.BorderColor = ቯ.CheckBoxItem.Disabled.CheckBorder;
		ቯ.SwitchButton.Disabled.SwitchBorderColor = ቯ.SwitchButton.Disabled.BorderColor;
		ቯ.SwitchButton.Disabled.OffTextColor = ቯ.CheckBoxItem.Disabled.Text;
		ቯ.SwitchButton.Disabled.OnTextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.TextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.SwitchBackColor = ቯ.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		ቯ.SwitchButton.Disabled.OffBackColor = ቯ.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		ቯ.SwitchButton.Disabled.OnBackColor = ቯ.SwitchButton.Disabled.OffBackColor;
		ቯ.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = ቖ.GetColor(8950952);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = ቖ.GetColor(15856632);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዸ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዹ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዺ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዻ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዼ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዽ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዿ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጀ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጂ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጋ(ቖ.GetColor(color), SystemColors.Window);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጌ(ቖ, eOffice2010ColorScheme.Blue);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጏ(ቯ.ListViewEx);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1311(ቯ.Bar);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጅ(ቖ.GetColor(color), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጆ(ቖ.GetColor(16383229), ቖ.GetColor(13884393), ቖ.GetColor(10401486));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጉ(ቖ.GetColor(16383229), ቖ.GetColor(13884393), ቖ.GetColor(10401486));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጊ(ቖ.GetColor(0));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጒ(ቖ.GetColor(Color.White), ቖ.GetColor("FF567DB0"), ቖ.GetColor("FF2F578D"));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጕ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1317();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1316();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጘ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጙ(ቯ.CheckBoxItem.Default.CheckBorder);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጃ(ቖ.GetColor(Color.White));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጄ(ቖ.GetColor(Color.DarkGray));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጚ(SystemColors.Window);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጛ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጜ(ቖ.GetColor(12371928));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጝ(ቖ.GetColor(13292514));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጞ(ቖ.GetColor(ቯ.LegacyColors.PanelBorder), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጟ(ቖ.GetColor(1780030));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጠ(ቖ.GetColor(1780030), ቖ.GetColor(16772277));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጡ(ቖ.GetColor(1780030), ቖ.GetColor(11450829));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		ቯ.StepIndicator.BackgroundColor = ቖ.GetColor(ColorFunctions.GetShade(ቯ.Form.BackColor, 10));
		ቯ.StepIndicator.IndicatorColor = ቖ.GetColor(10800168);
		ቯ.RadialMenu = new RadialMenuColorTable();
		ቯ.RadialMenu.CircularBackColor = ቖ.GetColor(13913902);
		ቯ.RadialMenu.CircularBorderColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.CircularForeColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBackground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBorder = ቖ.GetColor(5463924);
		ቯ.RadialMenu.RadialMenuButtonBackground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuButtonBorder = ቖ.GetColor(5463924);
		ቯ.RadialMenu.RadialMenuExpandForeground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuInactiveBorder = Color.FromArgb(128, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.RadialMenu.RadialMenuItemForeground = ቖ.GetColor(5463924);
		ቯ.RadialMenu.RadialMenuItemDisabledForeground = ቖ.GetColor(9276813);
		ቯ.RadialMenu.RadialMenuItemMouseOverBackground = Color.FromArgb(72, ቯ.RadialMenu.RadialMenuItemForeground);
		ቯ.RadialMenu.RadialMenuItemMouseOverForeground = ቖ.GetColor(5463924);
		ቯ.RadialMenu.RadialMenuMouseOverBorder = Color.FromArgb(200, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.StepItem.Default = new OfficeStepItemStateColorTable(new Color[2]
		{
			ቖ.GetColor(13292514),
			ቖ.GetColor(11450829)
		}, 90, new float[0], ቖ.GetColor(1780030), new Color[1] { ቖ.GetColor(8950952) });
		ቯ.StepItem.Progress = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14218615),
			ቖ.GetColor(7717632),
			ቖ.GetColor(7717632),
			ቖ.GetColor(11135267)
		}, 90, new float[4] { 0f, 0.6f, 0.6f, 1f }, Color.Empty, new Color[0]);
		ቯ.StepItem.MouseOver = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16776434),
			ቖ.GetColor(16774095),
			ቖ.GetColor(16772277),
			ቖ.GetColor(16772277)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(1780030), new Color[1] { ቖ.GetColor(15057765) });
		ቯ.StepItem.Pressed = new OfficeStepItemStateColorTable(new Color[2]
		{
			ቖ.GetColor(16771238),
			ቖ.GetColor(16771238)
		}, 90, new float[0], ቖ.GetColor(1780030), new Color[1] { ቖ.GetColor(15057765) });
		ቯ.TokenEditor = new TokenEditorColorTable(3887985, 15923454, 3887985, 12308198, 16777215, 29382);
		ቯ.Flyout.BorderColor = ቯ.Form.Active.BorderColors[0];
		ቯ.SideNav.SideNavItem.Default = new SideNavItemStateColorTable(ቖ.GetColor(1780030));
		ቯ.SideNav.SideNavItem.MouseOver = new SideNavItemStateColorTable(ቖ.GetColor(1780030), new Color[4]
		{
			ቖ.GetColor(16776434),
			ቖ.GetColor(16774095),
			ቖ.GetColor(16772277),
			ቖ.GetColor(16772277)
		}, new Color[1] { ቖ.GetColor(15057765) }, 0);
		ቯ.SideNav.SideNavItem.Pressed = new SideNavItemStateColorTable(ቖ.GetColor(1780030), new Color[2]
		{
			ቖ.GetColor(16771238),
			ቖ.GetColor(16771238)
		}, new Color[1] { ቖ.GetColor(15057765) }, 0);
		ቯ.SideNav.SideNavItem.Selected = new SideNavItemStateColorTable(ቖ.GetColor(1780030), new Color[1] { ቯ.Form.BackColor }, new Color[4]
		{
			Color.Empty,
			ቖ.GetColor(8950952),
			Color.Empty,
			ቖ.GetColor(8950952)
		});
		ቯ.SideNav.BorderColors = new Color[1] { ቖ.GetColor(8950952) };
		ቯ.SideNav.TitleBackColor = ቖ.GetColor(13292514);
		ቯ.SideNav.TitleBorderColors = new Color[4]
		{
			Color.Empty,
			Color.Empty,
			Color.Empty,
			ቖ.GetColor(6184542)
		};
		ቯ.SideNav.PanelBackColor = ቯ.Form.BackColor;
		ቯ.TabForm.Active.CaptionText = ቖ.GetColor(1780030);
		ቯ.TabForm.Inactive.CaptionText = ቖ.GetColor(1780030);
		ቯ.TabForm.Active.CaptionBackColors = new Color[1] { ቖ.GetColor(12371928) };
		ቯ.TabForm.Inactive.CaptionBackColors = new Color[1] { ቖ.GetColor(12371928) };
		ቯ.TabFormItemColorTables.Clear();
		ቯ.TabFormItemColorTables.Add(TabFormItem.DefaultColorTableKey, Office2007ColorTable._1257(ቖ, new Color[1] { ቖ.GetColor(12308198) }, new Color[1] { ቖ.GetColor(8950952) }, ቖ.GetColor(1780030), new Color[1] { ቖ.GetColor(13292514) }, new Color[1] { ቖ.GetColor(8950952) }, ቖ.GetColor(1780030)));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueColorTableKey, Office2007ColorTable._125E(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.YellowColorTableKey, Office2007ColorTable._125F(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleColorTableKey, Office2007ColorTable.ቡ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.CyanColorTableKey, Office2007ColorTable.ቢ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueMistColorTableKey, Office2007ColorTable.ቦ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleMistColorTableKey, Office2007ColorTable.ቧ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TanColorTableKey, Office2007ColorTable.ቨ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.LemonLimeColorTableKey, Office2007ColorTable.ቩ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.AppleColorTableKey, Office2007ColorTable.ቪ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TealColorTableKey, Office2007ColorTable.ቫ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedChalkColorTableKey, Office2007ColorTable.ቭ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.SilverColorTableKey, Office2007ColorTable.ቬ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.OrangeColorTableKey, Office2007ColorTable.ባ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.GreenColorTableKey, Office2007ColorTable.ቤ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.MagentaColorTableKey, Office2007ColorTable.ብ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedColorTableKey, Office2007ColorTable.በ(ቖ));
	}

	public static Office2007RibbonBarStateColorTable ᱬ(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10267329));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable();
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13292514), ቖ.GetColor(11450829));
		office2007RibbonBarStateColorTable.BottomBackground = null;
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(3690099);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable _21B3(ColorFactory ቖ)
	{
		return ᱬ(ቖ);
	}

	public static Office2007RibbonBarStateColorTable _21B4(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 0.8f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14871799), ቖ.GetColor(9676739));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(40, 16777215), ቖ.GetColor(120, 16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15726333), ቖ.GetColor(15200762));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15200762), ቖ.GetColor(14214386));
		office2007RibbonBarStateColorTable.TitleBackground = null;
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007ButtonItemColorTable _12C7(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(1780030);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15057765));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16776434), ቖ.GetColor(16774095));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16772277), ቖ.GetColor(16772277));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(15057765), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(1780030);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15648599));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15057765));
		office2007ButtonItemColorTable.Pressed.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16771238));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16771238));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(15057765), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(1780030);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15057765));
		office2007ButtonItemColorTable.Checked.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16773051));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16773051));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(1780030);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15057765));
		office2007ButtonItemColorTable.Expanded.InnerBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16771238));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16771238));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(1780030);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ወ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _12C7(ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13292514));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11450829));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(8950952));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(12371928));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(12371928));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10267329), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(10987431);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ው(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _12C7(ቖ);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(2500134);
		office2007ButtonItemColorTable.Checked.Text = office2007ButtonItemColorTable.Default.Text;
		office2007ButtonItemColorTable.Expanded.Text = office2007ButtonItemColorTable.Default.Text;
		office2007ButtonItemColorTable.MouseOver.Text = office2007ButtonItemColorTable.Default.Text;
		office2007ButtonItemColorTable.MouseOverSplitInactive.Text = office2007ButtonItemColorTable.Default.Text;
		office2007ButtonItemColorTable.Pressed.Text = office2007ButtonItemColorTable.Default.Text;
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(16579837));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16579837), ቖ.GetColor(15593457));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14606304), ቖ.GetColor(10592932));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13557229), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(10987431);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ዊ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ዂ.ዉ(ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(4095451), ቖ.GetColor(2843573));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(2711731), ቖ.GetColor(2977999));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(4557022), Color.Transparent);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(4425700), Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(2050209), ቖ.GetColor(2379686));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOverSplitInactive = office2007ButtonItemColorTable.Default;
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13557229), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(10987431);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ዌ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ዂ.ዋ(ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(11865171), ቖ.GetColor(11470669));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11470669), ቖ.GetColor(15221887));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14958972), Color.Transparent);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(12523614), ቖ.GetColor(15222403));
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9306682), ቖ.GetColor(9569085));
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15595005));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13557229), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(10987431);
		return office2007ButtonItemColorTable;
	}

	public static void ዄ(Office2007ButtonItemColorTable ዅ, ColorFactory ቖ)
	{
		Color color = ቖ.GetColor(1780030);
		Color empty = Color.Empty;
		ዅ.Default.ExpandBackground = color;
		ዅ.Default.ExpandLight = empty;
		ዅ.Checked.ExpandBackground = color;
		ዅ.Checked.ExpandLight = empty;
		ዅ.Expanded.ExpandBackground = color;
		ዅ.Expanded.ExpandLight = empty;
		ዅ.MouseOver.ExpandBackground = color;
		ዅ.MouseOver.ExpandLight = empty;
		ዅ.Pressed.ExpandBackground = color;
		ዅ.Pressed.ExpandLight = empty;
	}

	public static Office2007ButtonItemColorTable _21B5(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(6456);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(8756927));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16252927));
		office2007ButtonItemColorTable.MouseOver.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(15069691), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15069692), ቖ.GetColor(12770289));
		office2007ButtonItemColorTable.MouseOver.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(6456);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11647684));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7704235));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(9085890));
		office2007ButtonItemColorTable.Pressed.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(10467286), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.Background = new LinearGradientColorTable(ቖ.GetColor(10467286));
		office2007ButtonItemColorTable.Pressed.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(6456);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(8756927));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14543351));
		office2007ButtonItemColorTable.Checked.TopBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackground = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14478326), Color.Transparent);
		office2007ButtonItemColorTable.Checked.Background = new LinearGradientColorTable(ቖ.GetColor(13163503), ቖ.GetColor(11848677));
		office2007ButtonItemColorTable.Checked.SplitBorder = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.SplitBorderLight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(6456);
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.Checked;
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዑ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(1780030);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(13292514));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(8950952));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(1780030);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13292514));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(8950952));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(1780030);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(48, 16777215), ቖ.GetColor(180, 16777215));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(190, 16777215), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(8950952), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(1780030);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዒ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(1780030);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(13292514));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15043261), ቖ.GetColor(14575270));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(1780030);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13292514));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15043261), ቖ.GetColor(14575270));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(1780030);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(48, 14891409), ቖ.GetColor(180, 16777215));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(190, 16439536), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14891409), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(1780030);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዓ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(1780030);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(13292514));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(6602591), ቖ.GetColor(5489230));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(1780030);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13292514));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(6602591), ቖ.GetColor(5489230));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(1780030);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(48, 2535459), ቖ.GetColor(180, 16777215));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(190, 13170865), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(2535459), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(1780030);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዔ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(1780030);
		office2007RibbonTabItemColorTable.CornerSize = 2;
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(13292514));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15902565), ቖ.GetColor(15900672));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(1780030);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13292514));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15902565), ቖ.GetColor(15900672));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(1780030);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16110224), ቖ.GetColor(180, 16777215));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(190, 16508527), Color.Empty);
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15760646), ቖ.GetColor(15695368));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(1780030);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B6(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(4491493), Color.Transparent);
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(0);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(2050209), Color.Transparent);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B7(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(12933008), Color.Transparent);
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(0);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(13172855), Color.Transparent);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B8(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(192, 2535459), Color.Transparent);
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(0);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(2200605), Color.Transparent);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B9(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(ቖ.GetColor(192, 16751872), Color.Transparent);
		office2007RibbonTabGroupColorTable.BackgroundHighlight = LinearGradientColorTable.Empty;
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(0);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(ቖ.GetColor(14054681), Color.Transparent);
		return office2007RibbonTabGroupColorTable;
	}

	public static ElementStyle ዸ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.PaddingBottom = 3;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 22;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BorderWidth = 1;
		elementStyle.BorderColor = Color.Transparent;
		elementStyle.CornerDiameter = 3;
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBackgroundBlend.Count];
		menu.FileBackgroundBlend._0CD0(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle ዹ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BorderBottom = eStyleBorderType.Double;
		elementStyle.BorderBottomWidth = 0;
		elementStyle.BorderColor = menu.FileContainerBorder;
		elementStyle.BorderLeft = eStyleBorderType.None;
		elementStyle.BorderLeftWidth = 0;
		elementStyle.BorderRight = eStyleBorderType.None;
		elementStyle.BorderRightWidth = 0;
		elementStyle.BorderTop = eStyleBorderType.Solid;
		elementStyle.BorderTopWidth = 1;
		elementStyle.BorderBottom = eStyleBorderType.Solid;
		elementStyle.BorderBottomWidth = 1;
		elementStyle.PaddingBottom = 0;
		elementStyle.PaddingLeft = 0;
		elementStyle.PaddingRight = 0;
		elementStyle.PaddingTop = 0;
		return elementStyle;
	}

	public static ElementStyle ዺ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnOneBackground;
		elementStyle.BorderRight = eStyleBorderType.Solid;
		elementStyle.BorderRightColor = menu.FileColumnOneBorder;
		elementStyle.BorderRightWidth = 1;
		elementStyle.PaddingRight = 1;
		return elementStyle;
	}

	public static ElementStyle ዻ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnTwoBackground;
		return elementStyle;
	}

	public static ElementStyle ዼ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
		_ = ቯ.Menu;
		elementStyle.MarginTop = 2;
		elementStyle.MarginRight = 2;
		return elementStyle;
	}

	public static void ዳ(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.AppScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(11124437), ቖ.GetColor(12308196), 0);
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(7507629), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(12177126), 0f),
			new BackgroundColorBlend(ቖ.GetColor(9546697), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(9408399)), Color.FromArgb(64, Color.White));
		@default = ዲ.AppScrollBar.MouseOver;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(ቖ.GetColor(12703728), 0f),
			new BackgroundColorBlend(ቖ.GetColor(11849191), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(10731227), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926));
		@default.ThumbSignBackground = ዲ.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(ቖ.GetColor(12703728), 0f),
			new BackgroundColorBlend(ቖ.GetColor(11849191), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(10731227), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(9408399));
		@default = ዲ.AppScrollBar.MouseOverControl;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(ቖ.GetColor(12177126), 0f),
			new BackgroundColorBlend(ቖ.GetColor(11125211), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(9546697), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926));
		@default.ThumbSignBackground = ዲ.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[3]
		{
			new BackgroundColorBlend(ቖ.GetColor(12177126), 0f),
			new BackgroundColorBlend(ቖ.GetColor(11125211), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(9546697), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(9408399));
		@default = ዲ.AppScrollBar.Pressed;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(11585502), 0f),
			new BackgroundColorBlend(ቖ.GetColor(9020609), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926));
		@default.ThumbSignBackground = ዲ.AppScrollBar.Default.ThumbSignBackground;
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(11585502), 0f),
			new BackgroundColorBlend(ቖ.GetColor(9020609), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6059926), Color.Empty);
		@default.TrackSignBackground = ዲ.AppScrollBar.Default.TrackSignBackground;
		@default = ዲ.AppScrollBar.Disabled;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void ዱ(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.ScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(16053492), ቖ.GetColor(16579836), 0);
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(15133938));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7632246), ቖ.GetColor(4539975), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15791352), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15396595), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(14738665), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(14146529), 1f)
		});
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9081241), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(8224125));
		@default = ዲ.ScrollBar.MouseOver;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16580094), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15988473), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15396337), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(15001836), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16580094), ቖ.GetColor(15856629));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9081241), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7632246), ቖ.GetColor(4539975), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = @default.ThumbInnerBorder;
		@default.TrackOuterBorder = @default.ThumbOuterBorder;
		@default.TrackSignBackground = new LinearGradientColorTable(ቖ.GetColor(8224125));
		@default = ዲ.ScrollBar.MouseOverControl;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.CopyFrom(ዲ.ScrollBar.Default.TrackBackground);
		@default.ThumbInnerBorder = ዲ.ScrollBar.Default.TrackInnerBorder;
		@default.ThumbOuterBorder = ዲ.ScrollBar.Default.TrackOuterBorder;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7632246), ቖ.GetColor(4539975));
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(ዲ.ScrollBar.Default.TrackBackground);
		@default.TrackInnerBorder = ዲ.ScrollBar.Default.TrackInnerBorder;
		@default.TrackOuterBorder = ዲ.ScrollBar.Default.TrackOuterBorder;
		@default.TrackSignBackground = ዲ.ScrollBar.Default.TrackSignBackground;
		@default = ዲ.ScrollBar.Pressed;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15462387), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15067373), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(14343907), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(13685978), 1f)
		});
		@default.ThumbInnerBorder = LinearGradientColorTable.Empty;
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9081241), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = LinearGradientColorTable.Empty;
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9081241), Color.Empty);
		@default.TrackSignBackground = ዲ.ScrollBar.Default.TrackSignBackground;
		@default = ዲ.ScrollBar.Disabled;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static ElementStyle ጌ(ColorFactory ግ, eOffice2010ColorScheme ጎ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonClientPanelKey;
		elementStyle.BackColorGradientAngle = 90;
		switch (ጎ)
		{
		case eOffice2010ColorScheme.Blue:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(13097712), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(10138575), 0.7f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(8757696), 1f));
			break;
		case eOffice2010ColorScheme.Silver:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(13488598), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(12633288), 0.7f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(12238530), 1f));
			break;
		case eOffice2010ColorScheme.Black:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(7303023), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(5197647), 0.7f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(4144959), 1f));
			break;
		}
		return elementStyle;
	}

	public static Office2007ButtonItemColorTable _21BA(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5406176));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(135, 16777215));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(32, 32767));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(32, 32767));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(5406176), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(135, 16777215), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.MouseOverSplitInactive = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOverSplitInactive.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5406176));
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5406176));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(192, 16777215));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(32, 32767));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(32, 32767));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = LinearGradientColorTable.Empty;
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(5406176), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(135, 16777215), Color.Empty);
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Checked = office2007ButtonItemColorTable.Pressed;
		office2007ButtonItemColorTable.Expanded = office2007ButtonItemColorTable.MouseOver;
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}
}
internal class _21C4
{
	public static Office2007RibbonBarStateColorTable ᱬ(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12435393), ቖ.GetColor(8750469));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16185337), ቖ.GetColor(16448250));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15659510), ቖ.GetColor(14804718));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(14015463), ቖ.GetColor(15660276));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(14672879), ቖ.GetColor(12830673));
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(3355443);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable _21B3(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12435393), ቖ.GetColor(8750469));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16448508), ቖ.GetColor(16448250));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16251130), ቖ.GetColor(15987959));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15659253), ቖ.GetColor(16251385));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(14607086), ቖ.GetColor(11778503));
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(3355443);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable _21B4(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(8224125), ቖ.GetColor(9737364));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(Color.FromArgb(128, ቖ.GetColor(10000537)), Color.Transparent);
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15066598), ቖ.GetColor(12567500));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11778499), ቖ.GetColor(14607588));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(14607588), Color.Empty);
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonTabItemColorTable _21BE(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3355443);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15921908), ቖ.GetColor(14936047));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15727097), ቖ.GetColor(11926015));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12500670), ቖ.GetColor(12500670));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3355443);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15923194), ቖ.GetColor(14804719));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16444610), ቖ.GetColor(16777149));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15252338), ቖ.GetColor(16699742));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3355443);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(14406851), ቖ.GetColor(15322765));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(13883359), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15264494), ቖ.GetColor(14343652));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12435137), ቖ.GetColor(12435137));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3355443);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable _21BF(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3355443);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15452103), ቖ.GetColor(16179165));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16180966), ቖ.GetColor(15121339));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12827836), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3355443);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16506522), ቖ.GetColor(16642516));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16438654), ቖ.GetColor(16309396));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14138493), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3355443);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(14800345), ቖ.GetColor(15108485));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14735581), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14799831), ቖ.GetColor(15173765));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12435137), ቖ.GetColor(12500670));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3355443);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable _21C5(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3355443);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(12706488), ቖ.GetColor(14807515));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12501693), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3355443);
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16506782), ቖ.GetColor(16642516));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16642773), ቖ.GetColor(16308362));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13091250), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3355443);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(14213089), ቖ.GetColor(10143890));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14278367), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14213089), ቖ.GetColor(10143890));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12435137), ቖ.GetColor(12500670));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3355443);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable _21C6(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(3355443);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13092276), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(3355443);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16506522), ቖ.GetColor(16642516));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16372342), ቖ.GetColor(16308362));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14138493), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(3355443);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(14737884), ቖ.GetColor(11970867));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14737884), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14737884), ቖ.GetColor(15392621));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12435137), ቖ.GetColor(12435137));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(3355443);
		return office2007RibbonTabItemColorTable;
	}

	public static void _21C7(Office2007ButtonItemColorTable ዅ, ColorFactory ቖ)
	{
		Color color = ቖ.GetColor(8355711);
		Color color2 = ቖ.GetColor(15001320);
		ዅ.Default.ExpandBackground = color;
		ዅ.Default.ExpandLight = color2;
		ዅ.Checked.ExpandBackground = color;
		ዅ.Checked.ExpandLight = color2;
		ዅ.Expanded.ExpandBackground = color;
		ዅ.Expanded.ExpandLight = color2;
		ዅ.MouseOver.ExpandBackground = color;
		ዅ.MouseOver.ExpandLight = color2;
		ዅ.Pressed.ExpandBackground = color;
		ዅ.Pressed.ExpandLight = color2;
	}

	public static Office2007ButtonItemColorTable _21C8(bool ዧ, ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(3355443);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14536603), ቖ.GetColor(12625782));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777207), ቖ.GetColor(16775062));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16776151), ቖ.GetColor(16770957));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16766792), ቖ.GetColor(16770963));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(14402420), ቖ.GetColor(13155212));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16773311), ቖ.GetColor(16774879));
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(3355443);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9139796), ቖ.GetColor(12892584));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15312993), ቖ.GetColor(16624913));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16299370), ቖ.GetColor(16556128));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16484924), ቖ.GetColor(16694626));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16769199), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(11566411), ቖ.GetColor(9467987));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16433300), ቖ.GetColor(16767404));
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(3355443);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10450265), ቖ.GetColor(16764717));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15454644), Color.Transparent);
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16505781), ቖ.GetColor(16697208));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16692310), ቖ.GetColor(16640927));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16774559), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(3355443);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9339237), ቖ.GetColor(13025458));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15052373), ቖ.GetColor(16766538));
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16569255), ቖ.GetColor(16427099));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16289065), ቖ.GetColor(16639129));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16774857), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(11566411), ቖ.GetColor(9139796));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16433300), ቖ.GetColor(16767147));
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(3355443);
		_21C7(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21C9(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21C8(ዧ: false, ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15462138), ቖ.GetColor(14080740));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(12961745), ቖ.GetColor(13949154));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9278621), Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14936302), ቖ.GetColor(13357787));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13357787), ቖ.GetColor(13357787));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11776947), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CA(bool ዧ, ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(3355443);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(3889794), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(14741247));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(12639487), ቖ.GetColor(11128575));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(10931711), ቖ.GetColor(13296127));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(124, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(7640514), ቖ.GetColor(9482198));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14083059), ቖ.GetColor(8627664));
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(3355443);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(3889794), Color.Empty);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(14741247));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(12967160), ቖ.GetColor(11127543));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(96, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(9484010), ቖ.GetColor(7640514));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(184, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(7640514), ቖ.GetColor(9482198));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14083059), ቖ.GetColor(8627664));
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(3355443);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5668272), ቖ.GetColor(5668272));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(9484010), ቖ.GetColor(7640514));
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14149369), ቖ.GetColor(13098232));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11784437), ቖ.GetColor(14149111));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(3355443);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(6391735), ቖ.GetColor(5014472));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(10663117), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14149369), ቖ.GetColor(13098232));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11784437), ቖ.GetColor(14149111));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(5471928), ቖ.GetColor(6915498));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(10141941), ቖ.GetColor(12179455));
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(3355443);
		_21C7(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CB(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21CA(ዧ: false, ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15462138), ቖ.GetColor(14080740));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(12961745), ቖ.GetColor(13949154));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9278621), Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14936302), ቖ.GetColor(13357787));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13357787), ቖ.GetColor(13357787));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11776947), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CC(bool ዧ, ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7012448), ቖ.GetColor(5636429));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(15259391));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15917035), ቖ.GetColor(15124187));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(96, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15378644), ቖ.GetColor(14778817));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(10714065), ቖ.GetColor(8939694));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14271219), ቖ.GetColor(12297939));
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7012448), ቖ.GetColor(5636429));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(15259391));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13807814), ቖ.GetColor(13348289));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13793973), ቖ.GetColor(13126815));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(184, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(10714065), ቖ.GetColor(8939694));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14271219), ቖ.GetColor(12297939));
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7012448), ቖ.GetColor(5636429));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(15259391));
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15325154), ቖ.GetColor(14856401));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13603002), ቖ.GetColor(13595568));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7012448), ቖ.GetColor(5636429));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(15259391));
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15325154), ቖ.GetColor(14856401));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13603002), ቖ.GetColor(13595568));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(10714065), ቖ.GetColor(8939694));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14271219), ቖ.GetColor(12297939));
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(0);
		ዂ._12C6(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable _21CD(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21CC(ዧ: false, ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15462138), ቖ.GetColor(14080740));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(12961745), ቖ.GetColor(13949154));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9278621), Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14936302), ቖ.GetColor(13357787));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13357787), ቖ.GetColor(13357787));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11776947), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B6(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(15985558));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, ቖ.GetColor(16704797)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(3355443);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B7(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(14658993));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, ቖ.GetColor(15884889)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(3355443);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B8(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(11918000));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, ቖ.GetColor(7125332)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(3355443);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable _21B9(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(15719840));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, ቖ.GetColor(15971611)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(3355443);
		return office2007RibbonTabGroupColorTable;
	}

	public static void ዱ(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.ScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(16579836), ቖ.GetColor(15790320), 0);
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(15461871), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15462128), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15133168), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(13753060), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(12503771), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(15264233), ቖ.GetColor(15330546));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6254227), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(6316128)), Color.FromArgb(64, Color.White));
		@default = ዲ.ScrollBar.MouseOver;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15857406), 0f),
			new BackgroundColorBlend(ቖ.GetColor(14674679), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(12243179), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(13491955), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16448767), ቖ.GetColor(15660284));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9213861), ቖ.GetColor(6713727));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(3357780), ቖ.GetColor(5596557));
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(12571114), 0f),
			new BackgroundColorBlend(ቖ.GetColor(13295610), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(11193334), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(13886714), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(15329771), ቖ.GetColor(16580095));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(3960496), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(6316128)), Color.FromArgb(64, Color.White));
		@default = ዲ.ScrollBar.MouseOverControl;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16054783), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16054783), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(13623800), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(14675196), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16514559), Color.Empty);
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9213861), ቖ.GetColor(6713727));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675));
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15462128), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15133168), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(13753060), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(12503771), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(15264233), ቖ.GetColor(15330802));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6254227), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(6316128)), Color.FromArgb(64, Color.White));
		@default = ዲ.ScrollBar.Pressed;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15133164), 0f),
			new BackgroundColorBlend(ቖ.GetColor(14869734), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor(12831442), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor(14146785), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16119800), Color.Empty);
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9213861), ቖ.GetColor(6647935));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675));
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(10403561), 0f),
			new BackgroundColorBlend(ቖ.GetColor(11259126), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(7251696), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(11915767), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(12768231), ቖ.GetColor(14543867));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(1526154), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(6316128)), Color.FromArgb(64, Color.White));
		@default = ዲ.ScrollBar.Disabled;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void _21B2(Office2007ColorTable ቯ, ColorFactory ቖ)
	{
		ቯ.RibbonBar.Default = ᱬ(ቖ);
		ቯ.RibbonBar.MouseOver = _21B3(ቖ);
		ቯ.RibbonBar.Expanded = _21B4(ቖ);
		ቯ.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = _21BE(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = _21C5(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = _21BF(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = _21C6(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		ቯ.RibbonControl = new Office2007RibbonColorTable();
		ቯ.RibbonControl.TabsBackground = new LinearGradientColorTable(ቖ.GetColor(13685981), ቖ.GetColor(13685981));
		ቯ.RibbonControl.InnerBorder = new LinearGradientColorTable();
		ቯ.RibbonControl.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12500670), ቖ.GetColor(12500670));
		ቯ.RibbonControl.TabDividerBorder = ቖ.GetColor(11317175);
		ቯ.RibbonControl.TabDividerBorderLight = Color.Empty;
		ቯ.RibbonControl.PanelTopBackground = new LinearGradientColorTable(ቖ.GetColor(15659252), ቖ.GetColor(14804718));
		ቯ.RibbonControl.PanelBottomBackground = new LinearGradientColorTable(ቖ.GetColor(14015463), ቖ.GetColor(15792891));
		ቯ.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.ই("SystemImages.BlankStartButtonNormalSilver.png");
		ቯ.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.ই("SystemImages.BlankStartButtonHotSilver.png");
		ቯ.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.ই("SystemImages.BlankStartButtonPressedSilver.png");
		ቯ.ItemGroup.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15856627), ቖ.GetColor(15790834));
		ቯ.ItemGroup.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15198958), ቖ.GetColor(16185336));
		ቯ.ItemGroup.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16382457), ቖ.GetColor(16777215));
		ቯ.ItemGroup.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12961479), ቖ.GetColor(12895942));
		ቯ.ItemGroup.ItemGroupDividerDark = Color.FromArgb(196, ቖ.GetColor(13553358));
		ቯ.ItemGroup.ItemGroupDividerLight = Color.FromArgb(128, ቖ.GetColor(16777215));
		ቯ.Bar.ToolbarTopBackground = new LinearGradientColorTable(ቖ.GetColor(15198443), ቖ.GetColor(11910087));
		ቯ.Bar.ToolbarBottomBackground = new LinearGradientColorTable(ቖ.GetColor(11120566), ቖ.GetColor(13093326));
		ቯ.Bar.ToolbarBottomBorder = ቖ.GetColor(7501952);
		ቯ.Bar.PopupToolbarBackground = new LinearGradientColorTable(ቖ.GetColor(16448250), Color.Empty);
		ቯ.Bar.PopupToolbarBorder = ቖ.GetColor(3092271);
		ቯ.Bar.StatusBarTopBorder = ቖ.GetColor(11317175);
		ቯ.Bar.StatusBarTopBorderLight = ቖ.GetColor(Color.FromArgb(148, Color.White));
		ቯ.Bar.StatusBarAltBackground.Clear();
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(15198443), 0f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(11975623), 0.4f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(10857391), 0.4f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(11449530), 1f));
		ቯ.ButtonItemColors.Clear();
		ቯ.RibbonButtonItemColors.Clear();
		ቯ.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _21C8(ዧ: false, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21C9(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CA(ዧ: false, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CB(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CC(ዧ: false, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CD(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21C8(ዧ: true, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቯ.RibbonButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21C9(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቯ.RibbonButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CA(ዧ: true, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቯ.RibbonButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CB(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቯ.RibbonButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CC(ዧ: true, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቯ.RibbonButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = _21CD(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቯ.RibbonButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ዦ(ዧ: true, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(3355443);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(3355443);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(3355443);
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(3355443);
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(3355443);
		ቯ.MenuButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዂ.ው(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		ቯ.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(ቖ));
		ቯ.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = _21B6(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B7(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B8(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = _21B9(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		ቯ.Menu.Background = new LinearGradientColorTable(ቖ.GetColor(16448250), Color.Empty);
		ቯ.Menu.Border = new LinearGradientColorTable(ቖ.GetColor(8816262), Color.Empty);
		ቯ.Menu.Side = new LinearGradientColorTable(ቖ.GetColor(15724527), Color.Empty);
		ቯ.Menu.SideBorder = new LinearGradientColorTable(ቖ.GetColor(12961221), Color.Empty);
		ቯ.Menu.SideBorderLight = new LinearGradientColorTable(ቖ.GetColor(16119285), Color.Empty);
		ቯ.Menu.SideUnused = new LinearGradientColorTable(ቖ.GetColor(15066597), Color.Empty);
		ቯ.Menu.FileBackgroundBlend.Clear();
		ቯ.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(14607077), 0f),
			new BackgroundColorBlend(ቖ.GetColor(13817820), 1f)
		});
		ቯ.Menu.FileContainerBorder = ቖ.GetColor(11120308);
		ቯ.Menu.FileContainerBorderLight = ቖ.GetColor(15987956);
		ቯ.Menu.FileColumnOneBackground = ቖ.GetColor(16448250);
		ቯ.Menu.FileColumnOneBorder = ቖ.GetColor(11120308);
		ቯ.Menu.FileColumnTwoBackground = ቖ.GetColor(16448250);
		ቯ.Menu.FileBottomContainerBackgroundBlend.Clear();
		ቯ.Menu.FileBottomContainerBackgroundBlend.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(13291735), 0f),
			new BackgroundColorBlend(ቖ.GetColor(13423064), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(12765392), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(14279398), 1f)
		});
		ቯ.ComboBox.Default.Background = ቖ.GetColor(15264492);
		ቯ.ComboBox.Default.Border = ቖ.GetColor(11121080);
		ቯ.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandText = ቖ.GetColor(8158332);
		ቯ.ComboBox.DefaultStandalone.Background = ቖ.GetColor(16777215);
		int color = 10790052;
		ቯ.ComboBox.DefaultStandalone.Border = ቖ.GetColor(color);
		ቯ.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(15528181), ቖ.GetColor(14673131), 90);
		ቯ.ComboBox.DefaultStandalone.ExpandBorderInner = new LinearGradientColorTable();
		ቯ.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(12040119), Color.Empty, 90);
		ቯ.ComboBox.DefaultStandalone.ExpandText = ቖ.GetColor(0);
		ቯ.ComboBox.MouseOver.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.MouseOver.Border = ቖ.GetColor(color);
		ቯ.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(16776418), ቖ.GetColor(16770981), 90);
		ቯ.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(16776435), 90);
		ቯ.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(14405273), Color.Empty, 90);
		ቯ.ComboBox.MouseOver.ExpandText = ቖ.GetColor(0);
		ቯ.ComboBox.DroppedDown.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.DroppedDown.Border = ቖ.GetColor(9013641);
		ቯ.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(15392959), ቖ.GetColor(16766038), 90);
		ቯ.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(15854549), ቖ.GetColor(16770708), 90);
		ቯ.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(10129251), Color.Empty, 90);
		ቯ.ComboBox.DroppedDown.ExpandText = ቖ.GetColor(0);
		ቯ.DialogLauncher.Default.DialogLauncher = ቖ.GetColor(6645872);
		ቯ.DialogLauncher.Default.DialogLauncherShade = ቖ.GetColor(15461355);
		ቯ.DialogLauncher.MouseOver.DialogLauncher = ቖ.GetColor(6645872);
		ቯ.DialogLauncher.MouseOver.DialogLauncherShade = ቖ.GetColor(15461355);
		ቯ.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16776415), ቖ.GetColor(16773031));
		ቯ.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16767349), ቖ.GetColor(16769944));
		ቯ.DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(16776178));
		ቯ.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14405273), Color.Empty);
		ቯ.DialogLauncher.Pressed.DialogLauncher = ቖ.GetColor(6645872);
		ቯ.DialogLauncher.Pressed.DialogLauncherShade = ቖ.GetColor(15461355);
		ቯ.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15261373), ቖ.GetColor(15386253));
		ቯ.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16754488), ቖ.GetColor(16763982));
		ቯ.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15788756), ቖ.GetColor(16769937));
		ቯ.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10129251), ቖ.GetColor(11576434));
		ዷ(ቯ.LegacyColors, ቖ);
		ቯ.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(7174537), ቖ.GetColor(8952241));
		ቯ.SystemButton.Default.LightShade = ቖ.GetColor(16645631);
		ቯ.SystemButton.Default.DarkShade = ቖ.GetColor(4539717);
		ቯ.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(7174537), ቖ.GetColor(8952241));
		ቯ.SystemButton.MouseOver.LightShade = ቖ.GetColor(16645631);
		ቯ.SystemButton.MouseOver.DarkShade = ቖ.GetColor(4539717);
		ቯ.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16251387), ቖ.GetColor(15857146));
		ቯ.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(14936820), ቖ.GetColor(14870770));
		ቯ.SystemButton.MouseOver.TopHighlight = new LinearGradientColorTable(ቖ.GetColor(16514045), Color.Transparent);
		ቯ.SystemButton.MouseOver.BottomHighlight = new LinearGradientColorTable(ቖ.GetColor(16382716), Color.Transparent);
		ቯ.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13159892), ቖ.GetColor(12174032));
		ቯ.SystemButton.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16711423), ቖ.GetColor(16580094));
		ቯ.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(7174537), ቖ.GetColor(8952241));
		ቯ.SystemButton.Pressed.LightShade = ቖ.GetColor(16645631);
		ቯ.SystemButton.Pressed.DarkShade = ቖ.GetColor(4539717);
		ቯ.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13291219), ቖ.GetColor(11186102));
		ቯ.SystemButton.Pressed.TopHighlight = new LinearGradientColorTable(ቖ.GetColor(15001578), Color.Transparent);
		ቯ.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(9015447), ቖ.GetColor(12832212));
		ቯ.SystemButton.Pressed.BottomHighlight = new LinearGradientColorTable(ቖ.GetColor(14017257), Color.Transparent);
		ቯ.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9936032), ቖ.GetColor(11383480));
		ቯ.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15659509), ቖ.GetColor(16514559));
		ቯ.Form.Active.BorderColors = new Color[5]
		{
			ቖ.GetColor(7501952),
			ቖ.GetColor(12303034),
			ቖ.GetColor(13685981),
			ቖ.GetColor(13685981),
			ቖ.GetColor(13685981)
		};
		ቯ.Form.Inactive.BorderColors = new Color[5]
		{
			ቖ.GetColor(11844033),
			ቖ.GetColor(15790320),
			ቖ.GetColor(14737632),
			ቖ.GetColor(13685981),
			ቖ.GetColor(13685981)
		};
		ቯ.Form.Active.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(15198443), ቖ.GetColor(13290961));
		ቯ.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(12239306), ቖ.GetColor(15331063));
		ቯ.Form.Active.CaptionBottomBorder = new Color[2]
		{
			ቖ.GetColor(15331063),
			ቖ.GetColor(11317175)
		};
		ቯ.Form.Active.CaptionText = ቖ.GetColor(5067353);
		ቯ.Form.Active.CaptionTextExtra = ቖ.GetColor(3501745);
		ቯ.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(15658992), ቖ.GetColor(15066856));
		ቯ.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(14541029), ቖ.GetColor(16054267));
		ቯ.Form.Inactive.CaptionText = ቖ.GetColor(4473924);
		ቯ.Form.Inactive.CaptionTextExtra = ቖ.GetColor(4473924);
		ቯ.Form.BackColor = ቖ.GetColor(13290966);
		ቯ.Form.TextColor = ቖ.GetColor(0);
		ቯ.QuickAccessToolbar.Active.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14146011), ቖ.GetColor(14606306));
		ቯ.QuickAccessToolbar.Active.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13817821), ቖ.GetColor(14343392));
		ቯ.QuickAccessToolbar.Active.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Active.MiddleBorderColor = ቖ.GetColor(11447984);
		ቯ.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15198184), ቖ.GetColor(15658992));
		ቯ.QuickAccessToolbar.Inactive.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15264493), ቖ.GetColor(14277340));
		ቯ.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.MiddleBorderColor = ቖ.GetColor(11645362);
		ቯ.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		ቯ.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(14278374), ቖ.GetColor(14080995));
		ቯ.QuickAccessToolbar.Standalone.OutterBorderColor = ቖ.GetColor(12765652);
		ቯ.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.InnerBorderColor = ቖ.GetColor(15593459);
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelBackground = ቖ.GetColor(15461355);
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelText = ቖ.GetColor(5002076);
		ቯ.QuickAccessToolbar.Active.GlassBorder = new LinearGradientColorTable(ቖ.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
		ቯ.QuickAccessToolbar.Inactive.GlassBorder = new LinearGradientColorTable(ቖ.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
		ቯ.TabControl.Default = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14475493), ቖ.GetColor(14212578));
		ቯ.TabControl.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(12829635), ቖ.GetColor(14671839));
		ቯ.TabControl.Default.InnerBorder = ቖ.GetColor(15594484);
		ቯ.TabControl.Default.OuterBorder = ቖ.GetColor(7303284);
		ቯ.TabControl.Default.Text = ቖ.GetColor(0);
		ቯ.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16776683), ቖ.GetColor(16772264));
		ቯ.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16767577), ቖ.GetColor(16770701));
		ቯ.TabControl.MouseOver.InnerBorder = ቖ.GetColor(16777211);
		ቯ.TabControl.MouseOver.OuterBorder = ቖ.GetColor(11967859);
		ቯ.TabControl.MouseOver.Text = ቖ.GetColor(0);
		ቯ.TabControl.Selected = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(Color.White), ቖ.GetColor(15264494));
		ቯ.TabControl.Selected.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15264494), ቖ.GetColor(15264494));
		ቯ.TabControl.Selected.InnerBorder = ቖ.GetColor(Color.White);
		ቯ.TabControl.Selected.OuterBorder = ቖ.GetColor(7303284);
		ቯ.TabControl.Selected.Text = ቖ.GetColor(0);
		ቯ.TabControl.TabBackground = new LinearGradientColorTable(ቖ.GetColor(13093587), ቖ.GetColor(13093587));
		ቯ.TabControl.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(15264494), ቖ.GetColor(13948896));
		ቯ.TabControl.TabPanelBorder = ቖ.GetColor(7303284);
		Office2007CheckBoxColorTable checkBoxItem = ቯ.CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16053492), Color.Empty);
		checkBoxItem.Default.CheckBorder = ቖ.GetColor(10198432);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, ቖ.GetColor(13291477)), Color.FromArgb(164, ቖ.GetColor(16185078)));
		checkBoxItem.Default.CheckInnerBorder = ቖ.GetColor(10661049);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(ቖ.GetColor(5204872), Color.Empty);
		checkBoxItem.Default.Text = ቖ.GetColor(3355443);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16053492), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = ቖ.GetColor(10198432);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, ቖ.GetColor(16574383)), Color.FromArgb(128, ቖ.GetColor(16709863)));
		checkBoxItem.MouseOver.CheckInnerBorder = ቖ.GetColor(16438650);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(ቖ.GetColor(5204872), Color.Empty);
		checkBoxItem.MouseOver.Text = ቖ.GetColor(3355443);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(15068407), Color.Empty);
		checkBoxItem.Pressed.CheckBorder = ቖ.GetColor(10198432);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, ቖ.GetColor(16765031)), Color.FromArgb(164, ቖ.GetColor(16774357)));
		checkBoxItem.Pressed.CheckInnerBorder = ቖ.GetColor(15894822);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(ቖ.GetColor(5204872), Color.Empty);
		checkBoxItem.Pressed.Text = ቖ.GetColor(3355443);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = ቖ.GetColor(14014683);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, ቖ.GetColor(15659250)), Color.FromArgb(164, ቖ.GetColor(16514043)));
		checkBoxItem.Disabled.CheckInnerBorder = ቖ.GetColor(14738149);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(ቖ.GetColor(9276813), Color.Empty);
		checkBoxItem.Disabled.Text = ቖ.GetColor(9276813);
		ዱ(ቯ, ቖ);
		ዂ.ዴ(ቯ, ቖ);
		Office2007ProgressBarColorTable progressBarItem = ቯ.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(13028309, 14738669);
		progressBarItem.OuterBorder = ቖ.GetColor(16317439);
		progressBarItem.InnerBorder = ቖ.GetColor(12632256);
		progressBarItem.Chunk = new GradientColorTable(6590497, 15072444, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(164, ቖ.GetColor(14214851)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(8958024)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(6987039)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = ቯ.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(13028309, 14738669);
		progressBarItem.OuterBorder = ቖ.GetColor(16317439);
		progressBarItem.InnerBorder = ቖ.GetColor(12632256);
		progressBarItem.Chunk = new GradientColorTable(11446016, 16776653, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16776099)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13814272)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16708608)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = ቯ.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(13028309, 14738669);
		progressBarItem.OuterBorder = ቖ.GetColor(16317439);
		progressBarItem.InnerBorder = ቖ.GetColor(12632256);
		progressBarItem.Chunk = new GradientColorTable(13762560, 16764365, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16748431)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13762560)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16646144)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		Office2007GalleryColorTable gallery = ቯ.Gallery;
		gallery.GroupLabelBackground = ቖ.GetColor(15461355);
		gallery.GroupLabelText = ቖ.GetColor(5002076);
		gallery.GroupLabelBorder = ቖ.GetColor(12961221);
		ቯ.ListViewEx.Border = ቖ.GetColor(color);
		ቯ.ListViewEx.ColumnBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(13948891));
		ቯ.ListViewEx.ColumnSeparator = ቖ.GetColor(7237007);
		ቯ.ListViewEx.ColumnForeColor = ቖ.GetColor(3355443);
		ቯ.ListViewEx.SelectionBackground = new LinearGradientColorTable(ቖ.GetColor(10997232), Color.Empty);
		ቯ.ListViewEx.SelectionBorder = ቖ.GetColor(14938111);
		ቯ.NavigationPane.ButtonBackground = new GradientColorTable();
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15462138), 0f));
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14080740), 0.4f));
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12961745), 0.4f));
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(13949154), 1f));
		ቯ.SuperTooltip.BackgroundColors = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(15000816));
		ቯ.SuperTooltip.TextColor = ቖ.GetColor(5000268);
		Office2007SliderColorTable slider = ቯ.Slider;
		slider.Default.LabelColor = ቖ.GetColor(2303530);
		slider.Default.SliderLabelColor = ቖ.GetColor(3355443);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16777215), 0f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15856114), 0.15f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12435651), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(7896453), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16316665), 0.85f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16777215), 1f));
		slider.Default.PartBorderColor = ቖ.GetColor(4014923);
		slider.Default.PartBorderLightColor = Color.FromArgb(28, ቖ.GetColor(16777215));
		slider.Default.PartForeColor = ቖ.GetColor(6252401);
		slider.Default.PartForeLightColor = Color.FromArgb(168, ቖ.GetColor(15461356));
		slider.Default.TrackLineColor = ቖ.GetColor(2434341);
		slider.Default.TrackLineLightColor = ቖ.GetColor(13421772);
		slider.MouseOver.LabelColor = ቖ.GetColor(2303530);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(3355443);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16777215), 0f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16776693), 0.2f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16768899), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14526221), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16774350), 0.85f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16774350), 1f));
		slider.MouseOver.PartBorderColor = ቖ.GetColor(3092271);
		slider.MouseOver.PartBorderLightColor = Color.FromArgb(28, ቖ.GetColor(16777215));
		slider.MouseOver.PartForeColor = ቖ.GetColor(6775369);
		slider.MouseOver.PartForeLightColor = Color.FromArgb(168, ቖ.GetColor(16776955));
		slider.MouseOver.TrackLineColor = ቖ.GetColor(2434341);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(13421772);
		slider.Pressed.LabelColor = ቖ.GetColor(2303530);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(3355443);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15959586), 0f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15901267), 0.2f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16368011), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15760912), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15976092), 0.85f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16572615), 1f));
		slider.Pressed.PartBorderColor = ቖ.GetColor(3092271);
		slider.Pressed.PartBorderLightColor = Color.FromArgb(28, ቖ.GetColor(16777215));
		slider.Pressed.PartForeColor = ቖ.GetColor(6771265);
		slider.Pressed.PartForeLightColor = Color.FromArgb(168, ቖ.GetColor(16770766));
		slider.Pressed.TrackLineColor = ቖ.GetColor(2434341);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(13421772);
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(9276813);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(9276813);
		slider.Disabled.PartBackground = new GradientColorTable();
		foreach (BackgroundColorBlend color2 in slider.Default.PartBackground.Colors)
		{
			slider.Disabled.PartBackground.Colors.Add(new BackgroundColorBlend(colorBlendFactory.GetColor(color2.Color), color2.Position));
		}
		slider.Disabled.PartBorderColor = colorBlendFactory.GetColor(slider.Default.PartBorderColor);
		slider.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(slider.Default.PartBorderLightColor);
		slider.Disabled.PartForeColor = colorBlendFactory.GetColor(slider.Default.PartForeColor);
		slider.Disabled.PartForeLightColor = colorBlendFactory.GetColor(slider.Default.PartForeLightColor);
		slider.Disabled.TrackLineColor = colorBlendFactory.GetColor(slider.Default.TrackLineColor);
		slider.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(slider.Default.TrackLineLightColor);
		Office2007ColorTable.ቮ(ቯ, ቖ);
		ቯ.DataGridView.ColumnHeaderNormalBorder = ቖ.GetColor(9474450);
		ቯ.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(ቖ.GetColor(15856627), ቖ.GetColor(13158858), 90);
		ቯ.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16764057), ቖ.GetColor(16751464), 90);
		ቯ.DataGridView.ColumnHeaderSelectedBorder = ቖ.GetColor(13923901);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(14145495), ቖ.GetColor(color), 90);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBorder = ቖ.GetColor(13923901);
		ቯ.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(13684944), ቖ.GetColor(10921638), 90);
		ቯ.DataGridView.ColumnHeaderMouseOverBorder = ቖ.GetColor(10331049);
		ቯ.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(ቖ.GetColor(13684944), ቖ.GetColor(10921638), 90);
		ቯ.DataGridView.ColumnHeaderPressedBorder = ቖ.GetColor(16777215);
		ቯ.DataGridView.RowNormalBackground = new LinearGradientColorTable(ቖ.GetColor(15198183));
		ቯ.DataGridView.RowNormalBorder = ቖ.GetColor(9474450);
		ቯ.DataGridView.RowSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16107413));
		ቯ.DataGridView.RowSelectedBorder = ቖ.GetColor(13923901);
		ቯ.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(15567694));
		ቯ.DataGridView.RowSelectedMouseOverBorder = ቖ.GetColor(13923901);
		ቯ.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(12107716));
		ቯ.DataGridView.RowMouseOverBorder = ቖ.GetColor(10331049);
		ቯ.DataGridView.RowPressedBackground = new LinearGradientColorTable(ቖ.GetColor(12107716));
		ቯ.DataGridView.RowPressedBorder = ቖ.GetColor(16777215);
		ቯ.DataGridView.GridColor = ቖ.GetColor(13686757);
		ቯ.DataGridView.SelectorBackground = new LinearGradientColorTable(ቖ.GetColor(13027014));
		ቯ.DataGridView.SelectorBorder = ቖ.GetColor(9474450);
		ቯ.DataGridView.SelectorBorderDark = ቖ.GetColor(12829635);
		ቯ.DataGridView.SelectorBorderLight = ቖ.GetColor(16382457);
		ቯ.DataGridView.SelectorSign = new LinearGradientColorTable(ቖ.GetColor(16645629), ቖ.GetColor(15724527));
		ቯ.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(10592673));
		ቯ.DataGridView.SelectorMouseOverBorder = ቖ.GetColor(9474450);
		ቯ.DataGridView.SelectorMouseOverBorderDark = ቖ.GetColor(12829635);
		ቯ.DataGridView.SelectorMouseOverBorderLight = ቖ.GetColor(16382457);
		ቯ.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(ቖ.GetColor(16645629), ቖ.GetColor(15724527));
		ቯ.SideBar.Background = new LinearGradientColorTable(ቖ.GetColor(Color.White));
		ቯ.SideBar.Border = ቖ.GetColor(7303284);
		ቯ.SideBar.SideBarPanelItemText = ቖ.GetColor(3355443);
		ቯ.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15462138), 0f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14080740), 0.4f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12961745), 0.4f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(13949154), 1f));
		ቯ.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16505781), 0f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16697208), 0.4f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16692310), 0.4f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16640927), 1f));
		ቯ.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16776409), 0f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770957), 0.4f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16766792), 0.4f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770963), 1f));
		ቯ.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16300137), 0f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16622433), 0.4f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16484924), 0.4f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16694112), 1f));
		ቯ.AdvTree = new TreeColorTable();
		_135B._135D(ቯ.AdvTree, ቖ);
		ቯ.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(3355443);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(3355443);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFFFFCE4"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFECA1"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFD842"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFE47B"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FFDBCE99");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(3355443);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFFFFDEC"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF4CA"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFEBA6"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF2C5"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8E8F8F");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(3355443);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFDCA36F"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFF2B077"), 0.1f),
			new BackgroundColorBlend(ቖ.GetColor("FFE57840"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFDE550A"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFEF7D31"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8B7654");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("408B7654");
		ቯ.WarningBox.BackColor = ቖ.GetColor(Color.FromArgb(255, 212, 216, 219));
		ቯ.WarningBox.WarningBorderColor = ቖ.GetColor(Color.FromArgb(180, 180, 180));
		ቯ.WarningBox.WarningBackColor1 = ቖ.GetColor(Color.FromArgb(255, 255, 255, 255));
		ቯ.WarningBox.WarningBackColor2 = ቖ.GetColor(Color.FromArgb(255, 234, 234, 234));
		ቯ.CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(ቖ.GetColor(6384246)),
			new ColorDef(ቖ.GetColor(6384246)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13883098),
				ቖ.GetColor(11844289),
				ቖ.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(15264492)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(15264492)),
			new ColorDef(ቖ.GetColor(3560290)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(13619928),
				ቖ.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(16119285)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(ቖ.GetColor(15790578)),
			new ColorDef(ቖ.GetColor(7303284)),
			new ColorDef(ቖ.GetColor(7303284)),
			new ColorDef(ቖ.GetColor(7303284)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13883098),
				ቖ.GetColor(11844289),
				ቖ.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }),
			new ColorDef(ቖ.GetColor(6384246)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13817306),
				ቖ.GetColor(12041411),
				ቖ.GetColor(13291220)
			}, new float[4] { 0f, 0.6f, 0.6f, 1f }, 0f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13883098),
				ቖ.GetColor(11844289),
				ቖ.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(ቖ.GetColor(15264492)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(13619928),
				ቖ.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215))
		};
		ቯ.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(ቖ.GetColor(4944290)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16645887),
				ቖ.GetColor(12702698)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2642318)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(11650540),
				ቖ.GetColor(7708122)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2909476)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12773564),
				ቖ.GetColor(8704379)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(9125386)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16369568),
				ቖ.GetColor(16029528)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(4073329)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12957158),
				ቖ.GetColor(9796562)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8787740)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(15837868),
				ቖ.GetColor(15034222)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8157204)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16776362),
				ቖ.GetColor(16775512)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(-1)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(8388736))
		};
		ቯ.SuperTab.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(13685981));
		ቯ.SuperTab.InnerBorder = ቖ.GetColor(15727097);
		ቯ.SuperTab.OuterBorder = ቖ.GetColor(12500670);
		ቯ.SuperTab.ControlBoxDefault.Image = ቖ.GetColor(3355443);
		ቯ.SuperTab.ControlBoxMouseOver.Background = ቖ.GetColor(16770978);
		ቯ.SuperTab.ControlBoxMouseOver.Border = ቖ.GetColor(16760169);
		ቯ.SuperTab.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		ቯ.SuperTab.ControlBoxPressed.Background = ቖ.GetColor(16485436);
		ቯ.SuperTab.ControlBoxPressed.Border = ቖ.GetColor(16760169);
		ቯ.SuperTab.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		ቯ.SuperTab.InsertMarker = ቖ.GetColor(255, 128);
		ቯ.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable();
		ቯ.SuperTabItem.Default.Normal.Text = ቖ.GetColor(3355443);
		ቯ.SuperTabItem.Default.Normal.CloseMarker = ቖ.GetColor(3355443);
		ቯ.SuperTabItem.Default.Disabled.Text = ቖ.GetColor(9276813);
		ቯ.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		ቯ.SuperTabItem.Default.Disabled.CloseMarker = ቖ.GetColor(9276813);
		ቯ.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(15921908),
			ቖ.GetColor(14936047)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.Selected.InnerBorder = ቖ.GetColor(15727097);
		ቯ.SuperTabItem.Default.Selected.OuterBorder = ቖ.GetColor(12500670);
		ቯ.SuperTabItem.Default.Selected.Text = ቖ.GetColor(3355443);
		ቯ.SuperTabItem.Default.Selected.CloseMarker = ቖ.GetColor(3355443);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(15923194),
			ቖ.GetColor(14804719)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(16444610);
		ቯ.SuperTabItem.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(15252338);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Text = ቖ.GetColor(3355443);
		ቯ.SuperTabItem.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(3355443);
		ቯ.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(14406851),
			ቖ.GetColor(15322765)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.MouseOver.InnerBorder = ቖ.GetColor(15264494);
		ቯ.SuperTabItem.Default.MouseOver.OuterBorder = ቖ.GetColor(12435137);
		ቯ.SuperTabItem.Default.MouseOver.Text = ቖ.GetColor(3355443);
		ቯ.SuperTabItem.Default.MouseOver.CloseMarker = ቖ.GetColor(3355443);
		ቯ.SuperTabItem.Left = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Bottom = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Right = ቯ.SuperTabItem.Default;
		ቯ.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(15264494));
		ቯ.SuperTabPanel.Default.InnerBorder = ቖ.GetColor(15727097);
		ቯ.SuperTabPanel.Default.OuterBorder = ቖ.GetColor(12500670);
		ቯ.SuperTabPanel.Left = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Bottom = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Right = ቯ.SuperTabPanel.Default;
		ጦ.ጱ(ቯ.Backstage, ቖ);
		ጦ.ጸ(ቯ.BackstageItem, ቖ);
		ጦ.ጾ(ቯ.BackstagePanel, ቖ);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = ቖ.GetColor(11121080);
		switchButtonColorTable.OffBackColor = ቖ.GetColor(15330803);
		switchButtonColorTable.OffTextColor = ቖ.GetColor(3355443);
		switchButtonColorTable.OnBackColor = ቖ.GetColor(9621584);
		switchButtonColorTable.OnTextColor = ቖ.GetColor(3355443);
		switchButtonColorTable.SwitchBackColor = ቖ.GetColor(13883362);
		switchButtonColorTable.SwitchBorderColor = ቖ.GetColor(9211021);
		switchButtonColorTable.TextColor = ቖ.GetColor(3355443);
		ቯ.SwitchButton = new SwitchButtonColors();
		ቯ.SwitchButton.Default = switchButtonColorTable;
		ቯ.SwitchButton.Disabled.BorderColor = ቯ.CheckBoxItem.Disabled.CheckBorder;
		ቯ.SwitchButton.Disabled.SwitchBorderColor = ቯ.SwitchButton.Disabled.BorderColor;
		ቯ.SwitchButton.Disabled.OffTextColor = ቯ.CheckBoxItem.Disabled.Text;
		ቯ.SwitchButton.Disabled.OnTextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.TextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.SwitchBackColor = ቯ.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		ቯ.SwitchButton.Disabled.OffBackColor = ቯ.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		ቯ.SwitchButton.Disabled.OnBackColor = ቯ.SwitchButton.Disabled.OffBackColor;
		ቯ.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = ቖ.GetColor(11121080);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = ቖ.GetColor(15264492);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዸ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዹ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዺ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዻ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዼ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዽ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ዿ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጀ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጂ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጋ(ቖ.GetColor(color), SystemColors.Window);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጌ(ቖ, eOffice2007ColorScheme.Silver);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጏ(ቯ.ListViewEx);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1311(ቯ.Bar);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጅ(ቖ.GetColor(color), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጆ(ቖ.GetColor(15856627), ቖ.GetColor(13158858), ቖ.GetColor(9474450));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጉ(ቖ.GetColor(15856627), ቖ.GetColor(13158858), ቖ.GetColor(9474450));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጊ(ቖ.GetColor(0));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጒ(ቖ.GetColor(Color.White), ቖ.GetColor("FFACAFB7"), ቖ.GetColor("FF5A5B5C"));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጕ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1317();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ._1316();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጘ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጙ(ቯ.CheckBoxItem.Default.CheckBorder);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጃ(ቖ.GetColor(Color.White));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጄ(ቖ.GetColor(Color.DarkGray));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጚ(SystemColors.Window);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጛ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጜ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጝ(ቖ.GetColor(16119543));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጞ(ቖ.GetColor(ቯ.LegacyColors.PanelBorder), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጟ(ቖ.GetColor(3355443));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጠ(ቖ.GetColor(3355443), ቖ.GetColor(14015463));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዂ.ጡ(ቖ.GetColor(3355443), ቖ.GetColor(Color.DarkGray));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		ቯ.StepIndicator.BackgroundColor = ቖ.GetColor(ColorFunctions.GetShade(ቯ.Form.BackColor, 10));
		ቯ.StepIndicator.IndicatorColor = ቖ.GetColor(10800168);
		ቯ.RadialMenu = new RadialMenuColorTable();
		ቯ.RadialMenu.CircularBackColor = ቖ.GetColor(13913902);
		ቯ.RadialMenu.CircularBorderColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.CircularForeColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBackground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBorder = ቖ.GetColor(7174537);
		ቯ.RadialMenu.RadialMenuButtonBackground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuButtonBorder = ቖ.GetColor(7174537);
		ቯ.RadialMenu.RadialMenuExpandForeground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuInactiveBorder = Color.FromArgb(128, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.RadialMenu.RadialMenuItemForeground = ቖ.GetColor(7174537);
		ቯ.RadialMenu.RadialMenuItemDisabledForeground = ቖ.GetColor(9276813);
		ቯ.RadialMenu.RadialMenuItemMouseOverBackground = Color.FromArgb(72, ቯ.RadialMenu.RadialMenuItemForeground);
		ቯ.RadialMenu.RadialMenuItemMouseOverForeground = ቖ.GetColor(7174537);
		ቯ.RadialMenu.RadialMenuMouseOverBorder = Color.FromArgb(200, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.StepItem.Default = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(15462138),
			ቖ.GetColor(14080740),
			ቖ.GetColor(12961745),
			ቖ.GetColor(13949154)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(3355443), new Color[1] { ቖ.GetColor(9278621) });
		ቯ.StepItem.Progress = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14218615),
			ቖ.GetColor(7717632),
			ቖ.GetColor(7717632),
			ቖ.GetColor(11135267)
		}, 90, new float[4] { 0f, 0.6f, 0.6f, 1f }, Color.Empty, new Color[0]);
		ቯ.StepItem.MouseOver = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16776151),
			ቖ.GetColor(16770957),
			ቖ.GetColor(16766792),
			ቖ.GetColor(16770963)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(3355443), new Color[2]
		{
			ቖ.GetColor(14536603),
			ቖ.GetColor(16775062)
		});
		ቯ.StepItem.Pressed = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16299370),
			ቖ.GetColor(16556128),
			ቖ.GetColor(16484924),
			ቖ.GetColor(16694626)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(3355443), new Color[2]
		{
			ቖ.GetColor(9139796),
			ቖ.GetColor(16624913)
		});
		ቯ.ListBoxItem.Default = new OfficeListBoxItemStateColorTable(ቖ.GetColor(0));
		ቯ.ListBoxItem.MouseOver = new OfficeListBoxItemStateColorTable(ቖ.GetColor(0), new Color[2]
		{
			ቖ.GetColor(16776409),
			ቖ.GetColor(16770963)
		}, 90, new float[2] { 0f, 1f });
		ቯ.ListBoxItem.Selected = new OfficeListBoxItemStateColorTable(ቖ.GetColor(0), new Color[2]
		{
			ቖ.GetColor(16569720),
			ቖ.GetColor(16500815)
		}, 90, new float[2] { 0f, 1f });
		ቯ.TokenEditor = new TokenEditorColorTable(0, 15461874, 0, 12305099, 16777215, 29382);
		ቯ.Flyout.BorderColor = ቯ.Form.Active.BorderColors[0];
		ቯ.SideNav.SideNavItem.Default = new SideNavItemStateColorTable(ቖ.GetColor(3355443));
		ቯ.SideNav.SideNavItem.MouseOver = new SideNavItemStateColorTable(ቖ.GetColor(3355443), new Color[4]
		{
			ቖ.GetColor(16776151),
			ቖ.GetColor(16770957),
			ቖ.GetColor(16766792),
			ቖ.GetColor(16770963)
		}, new Color[2]
		{
			ቖ.GetColor(12625782),
			ቖ.GetColor(16773822)
		}, 2);
		ቯ.SideNav.SideNavItem.Pressed = new SideNavItemStateColorTable(ቖ.GetColor(3355443), new Color[4]
		{
			ቖ.GetColor(16300137),
			ቖ.GetColor(16622433),
			ቖ.GetColor(16484924),
			ቖ.GetColor(16694112)
		}, new Color[2]
		{
			ቖ.GetColor(6258614),
			ቖ.GetColor(16624913)
		}, 2);
		ቯ.SideNav.SideNavItem.Selected = new SideNavItemStateColorTable(ቖ.GetColor(3355443), new Color[1] { ቯ.Form.BackColor }, new Color[4]
		{
			Color.Empty,
			ቖ.GetColor(12500670),
			Color.Empty,
			ቖ.GetColor(12500670)
		});
		ቯ.SideNav.BorderColors = new Color[1] { ቖ.GetColor(12500670) };
		ቯ.SideNav.TitleBackColor = ቖ.GetColor(14936047);
		ቯ.SideNav.TitleBorderColors = new Color[4]
		{
			Color.Empty,
			Color.Empty,
			Color.Empty,
			ቖ.GetColor(12500670)
		};
		ቯ.SideNav.PanelBackColor = ቯ.Form.BackColor;
		ቯ.TabForm.Active.CaptionText = ቖ.GetColor(5067353);
		ቯ.TabForm.Inactive.CaptionText = ቖ.GetColor(4473924);
		ቯ.TabForm.Active.CaptionBackColors = new Color[1] { ቖ.GetColor(16119543) };
		ቯ.TabForm.Inactive.CaptionBackColors = new Color[1] { ቖ.GetColor(16119543) };
		ቯ.TabFormItemColorTables.Add(TabFormItem.DefaultColorTableKey, Office2007ColorTable._1257(ቖ, 3355443));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueColorTableKey, Office2007ColorTable._125E(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.YellowColorTableKey, Office2007ColorTable._125F(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleColorTableKey, Office2007ColorTable.ቡ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.CyanColorTableKey, Office2007ColorTable.ቢ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueMistColorTableKey, Office2007ColorTable.ቦ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleMistColorTableKey, Office2007ColorTable.ቧ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TanColorTableKey, Office2007ColorTable.ቨ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.LemonLimeColorTableKey, Office2007ColorTable.ቩ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.AppleColorTableKey, Office2007ColorTable.ቪ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TealColorTableKey, Office2007ColorTable.ቫ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedChalkColorTableKey, Office2007ColorTable.ቭ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.SilverColorTableKey, Office2007ColorTable.ቬ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.OrangeColorTableKey, Office2007ColorTable.ባ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.GreenColorTableKey, Office2007ColorTable.ቤ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.MagentaColorTableKey, Office2007ColorTable.ብ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedColorTableKey, Office2007ColorTable.በ(ቖ));
	}

	public static void ዷ(ColorScheme _0BDE, ColorFactory ቖ)
	{
		_0BDE.BarBackground = ቖ.GetColor(15921913);
		_0BDE.BarBackground2 = ቖ.GetColor(10065845);
		_0BDE.BarStripeColor = ቖ.GetColor(7237007);
		_0BDE.BarCaptionBackground = ቖ.GetColor(8026521);
		_0BDE.BarCaptionBackground2 = ቖ.GetColor(6447481);
		_0BDE.BarCaptionInactiveBackground = ቖ.GetColor(16185336);
		_0BDE.BarCaptionInactiveBackground2 = ቖ.GetColor(14344166);
		_0BDE.BarCaptionInactiveText = ቖ.GetColor(3355443);
		_0BDE.BarCaptionText = ቖ.GetColor(16777215);
		_0BDE.BarFloatingBorder = ቖ.GetColor(8026521);
		_0BDE.BarPopupBackground = ቖ.GetColor(16644863);
		_0BDE.BarPopupBorder = ቖ.GetColor(8026521);
		_0BDE.ItemBackground = Color.Empty;
		_0BDE.ItemBackground2 = Color.Empty;
		_0BDE.ItemCheckedBackground = ቖ.GetColor(16764818);
		_0BDE.ItemCheckedBackground2 = ቖ.GetColor(16756553);
		_0BDE.ItemCheckedBorder = ቖ.GetColor(16755519);
		_0BDE.ItemCheckedText = ቖ.GetColor(0);
		_0BDE.ItemDisabledBackground = Color.Empty;
		_0BDE.ItemDisabledText = ቖ.GetColor(9276813);
		_0BDE.ItemExpandedShadow = Color.Empty;
		_0BDE.ItemExpandedBackground = ቖ.GetColor(15264241);
		_0BDE.ItemExpandedBackground2 = ቖ.GetColor(12237261);
		_0BDE.ItemExpandedText = ቖ.GetColor(0);
		_0BDE.ItemHotBackground = ቖ.GetColor(16774604);
		_0BDE.ItemHotBackground2 = ቖ.GetColor(16768900);
		_0BDE.ItemHotBorder = ቖ.GetColor(16760169);
		_0BDE.ItemHotText = ቖ.GetColor(0);
		_0BDE.ItemPressedBackground = ቖ.GetColor(16553789);
		_0BDE.ItemPressedBackground2 = ቖ.GetColor(16758878);
		_0BDE.ItemPressedBorder = ቖ.GetColor(16485436);
		_0BDE.ItemPressedText = ቖ.GetColor(0);
		_0BDE.ItemSeparator = Color.FromArgb(225, ቖ.GetColor(7237007));
		_0BDE.ItemSeparatorShade = Color.FromArgb(180, ቖ.GetColor(16777215));
		_0BDE.ItemText = ቖ.GetColor(0);
		_0BDE.MenuBackground = ቖ.GetColor(16644863);
		_0BDE.MenuBackground2 = Color.Empty;
		_0BDE.MenuBarBackground = ቖ.GetColor(15066606);
		_0BDE.MenuBorder = ቖ.GetColor(8158356);
		_0BDE.ItemExpandedBorder = _0BDE.MenuBorder;
		_0BDE.MenuSide = ቖ.GetColor(15724527);
		_0BDE.MenuSide2 = Color.Empty;
		_0BDE.MenuUnusedBackground = _0BDE.MenuBackground;
		_0BDE.MenuUnusedSide = ቖ.GetColor(15329769);
		_0BDE.MenuUnusedSide2 = Color.Empty;
		_0BDE.ItemDesignTimeBorder = Color.Black;
		_0BDE.BarDockedBorder = ቖ.GetColor(7303284);
		_0BDE.DockSiteBackColor = ቖ.GetColor(14145509);
		_0BDE.DockSiteBackColor2 = ቖ.GetColor(15987703);
		_0BDE.CustomizeBackground = ቖ.GetColor(11776712);
		_0BDE.CustomizeBackground2 = ቖ.GetColor(7763090);
		_0BDE.CustomizeText = ቖ.GetColor(0);
		_0BDE.PanelBackground = ቖ.GetColor(15790578);
		_0BDE.PanelBackground2 = ቖ.GetColor(12436169);
		_0BDE.PanelText = Color.Black;
		_0BDE.PanelBorder = ቖ.GetColor(7303284);
		_0BDE.ExplorerBarBackground = ቖ.GetColor(15987703);
		_0BDE.ExplorerBarBackground2 = ቖ.GetColor(14145509);
		_0BDE.SplitterBackground = _0BDE.PanelBackground;
		_0BDE.SplitterBackground2 = _0BDE.PanelBackground2;
		_0BDE.SplitterText = _0BDE.PanelText;
		_0BDE.SplitterBorder = _0BDE.PanelBorder;
	}
}
internal class _2209 : ᯊ
{
	protected override Rectangle GetTextRectangle(ButtonItem button, ItemPaintArgs pa, eTextFormat stringFormat, ᇎ image)
	{
		Rectangle textRectangle = base.GetTextRectangle(button, pa, stringFormat, image);
		if (image == null && string.IsNullOrEmpty(button.SymbolRealized))
		{
			textRectangle.Inflate(-(TabFormItem._136F / 2), 0);
		}
		else
		{
			textRectangle.X += TabFormItem._136F / 2 - TabFormItem.ܭ;
		}
		return textRectangle;
	}

	public override Rectangle GetImageRectangle(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		Rectangle imageRectangle = base.GetImageRectangle(button, pa, image);
		imageRectangle.X += TabFormItem._136F / 2 - TabFormItem.ܭ;
		return imageRectangle;
	}

	public override eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		eTextFormat stringFormat = base.GetStringFormat(button, pa, image);
		return stringFormat & ~(stringFormat & eTextFormat.EndEllipsis);
	}

	protected override void PaintState(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r, bool isMouseDown)
	{
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		if (r.IsEmpty || !_192F._1930(button, pa) || r.Width == 0 || r.Height == 0)
		{
			return;
		}
		if (!(button is TabFormItem tabFormItem) || IsOnMenu(button, pa))
		{
			base.PaintState(button, pa, image, r, isMouseDown);
			return;
		}
		_ = pa.IsOnMenu;
		TabFormItemColorTable tabFormItemColorTable = ጧ(tabFormItem);
		if (tabFormItemColorTable == null)
		{
			return;
		}
		TabFormItemStateColorTable tabFormItemStateColorTable = ῡ(tabFormItemColorTable, tabFormItem);
		if (tabFormItemStateColorTable == null)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		Region clip = graphics.Clip;
		try
		{
			Rectangle rectangle = _220B(tabFormItem);
			rectangle.Inflate(1, 0);
			graphics.SetClip(rectangle, (CombineMode)1);
			eTabFormStripControlDock tabAlignment = tabFormItem.TabAlignment;
			tabFormItem.TabPath = ᔹ(r, tabAlignment, ᔻ: true);
			if ((tabFormItemStateColorTable.BackColors != null && tabFormItemStateColorTable.BackColors.Length > 0) || (tabFormItem.BackColors != null && tabFormItem.BackColors.Length > 0))
			{
				Brush val = DisplayHelp.CreateBrush(r, (tabFormItem.BackColors != null && tabFormItem.BackColors.Length > 0) ? tabFormItem.BackColors : tabFormItemStateColorTable.BackColors, tabFormItemStateColorTable.BackColorsGradientAngle, tabFormItemStateColorTable.BackColorsPositions);
				try
				{
					GraphicsPath val2 = ᔹ(r, tabAlignment, ᔻ: true);
					try
					{
						graphics.FillPath(Brushes.White, val2);
						graphics.FillPath(val, val2);
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
			if (tabFormItemStateColorTable.BorderColors.Length > 0)
			{
				Rectangle ल = r;
				Color[] borderColors = tabFormItemStateColorTable.BorderColors;
				foreach (Color color in borderColors)
				{
					GraphicsPath val3 = ᔹ(ल, tabAlignment, ᔻ: false);
					try
					{
						Pen val4 = new Pen(color, (float)Dpi.Width1);
						try
						{
							graphics.DrawPath(val4, val3);
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
					ल.Inflate(-Dpi.Width1, -Dpi.Width1);
				}
			}
			if (tabFormItem.CloseButtonVisible)
			{
				int num = Dpi.Width(TabFormItem._1370);
				Rectangle rectangle2 = new Rectangle(r.Right - (TabFormItem._136F + TabFormItem._136D - TabFormItem.ܭ), r.Y + (r.Height - num) / 2 + Dpi.Height1, num, num);
				TabCloseButtonStateColorTable tabCloseButtonStateColorTable = _220A(tabFormItemColorTable, tabFormItem);
				if (tabCloseButtonStateColorTable.BackColors.Length > 0)
				{
					Brush val5 = DisplayHelp.CreateBrush(rectangle2, tabCloseButtonStateColorTable.BackColors, tabCloseButtonStateColorTable.BackColorsGradientAngle, tabCloseButtonStateColorTable.BackColorsPositions);
					try
					{
						graphics.FillEllipse(val5, rectangle2);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				if (!tabCloseButtonStateColorTable.BorderColor.IsEmpty)
				{
					Pen val6 = new Pen(tabCloseButtonStateColorTable.BorderColor, (float)Dpi.Width1);
					try
					{
						graphics.DrawEllipse(val6, rectangle2);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				Color color2 = tabFormItemStateColorTable.TextColor;
				if (!tabCloseButtonStateColorTable.ForeColor.IsEmpty)
				{
					color2 = tabCloseButtonStateColorTable.ForeColor;
				}
				Pen val7 = new Pen(color2, (float)Dpi.Width2);
				try
				{
					Rectangle rectangle3 = rectangle2;
					rectangle3.Inflate(-Dpi.Width3, -Dpi.Width3);
					graphics.DrawLine(val7, rectangle3.X, rectangle3.Y, rectangle3.Right, rectangle3.Bottom);
					graphics.DrawLine(val7, rectangle3.Right, rectangle3.Y, rectangle3.X, rectangle3.Bottom);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
				tabFormItem._1369 = rectangle2;
			}
			else
			{
				tabFormItem._1369 = Rectangle.Empty;
			}
			if (tabFormItem.Checked)
			{
				graphics.ResetClip();
				Pen val8 = new Pen(tabFormItemStateColorTable.BorderColors[0], (float)Dpi.Width1);
				try
				{
					Rectangle rect = _220B(tabFormItem);
					rect.Inflate(-Dpi.Width2, 0);
					TabFormItemsSimpleContainer tabFormItemsSimpleContainer = tabFormItem.Parent as TabFormItemsSimpleContainer;
					if (tabFormItemsSimpleContainer == null || tabFormItemsSimpleContainer.ClippingRectangle.IsEmpty || tabFormItemsSimpleContainer.ClippingRectangle.IntersectsWith(rect))
					{
						if (tabFormItemsSimpleContainer != null && !tabFormItemsSimpleContainer.ClippingRectangle.IsEmpty && tabFormItemsSimpleContainer.ClippingRectangle.IntersectsWith(rect) && rect.Right > tabFormItemsSimpleContainer.ClippingRectangle.Right)
						{
							rect.Width = Math.Max(1, tabFormItemsSimpleContainer.ClippingRectangle.Right - rect.X);
						}
						if (tabAlignment == eTabFormStripControlDock.Bottom)
						{
							graphics.DrawLine(val8, 0, rect.Y, rect.X, rect.Y);
							graphics.DrawLine(val8, rect.Right, rect.Y, pa.ContainerControl.Width, rect.Y);
						}
						else
						{
							graphics.DrawLine(val8, 0, rect.Bottom - 1, rect.X, rect.Bottom - 1);
							graphics.DrawLine(val8, rect.Right, rect.Bottom - 1, pa.ContainerControl.Width, rect.Bottom - 1);
						}
					}
					else if (tabAlignment == eTabFormStripControlDock.Bottom)
					{
						graphics.DrawLine(val8, 0, rect.Y, pa.ContainerControl.Width, rect.Y);
					}
					else
					{
						graphics.DrawLine(val8, 0, rect.Bottom - 1, pa.ContainerControl.Width, rect.Bottom - 1);
					}
				}
				finally
				{
					((IDisposable)val8)?.Dispose();
				}
			}
			graphics.Clip = clip;
		}
		finally
		{
			if (clip != null)
			{
				clip.Dispose();
			}
		}
	}

	private TabCloseButtonStateColorTable _220A(TabFormItemColorTable _21EE, TabFormItem ᆰ)
	{
		if (ᆰ._1368 == eButtonState.MouseDownLeft)
		{
			return _21EE.CloseButton.Pressed;
		}
		if (ᆰ._1368 == eButtonState.MouseOver)
		{
			return _21EE.CloseButton.MouseOver;
		}
		return _21EE.CloseButton.Normal;
	}

	private Rectangle _220B(TabFormItem ᆰ)
	{
		Rectangle displayRectangle = ᆰ.DisplayRectangle;
		GraphicsPath val = ᔹ(displayRectangle, ᆰ.TabAlignment, ᔻ: false);
		try
		{
			return Rectangle.Round(val.GetBounds());
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private GraphicsPath ᔹ(Rectangle ल, eTabFormStripControlDock _0736, bool ᔻ)
	{
		return _220C(ल);
	}

	private GraphicsPath _220C(Rectangle ᔸ)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		Rectangle rectangle = ᔸ;
		rectangle.Width -= Dpi.Width1;
		int num = Math.Min(20, rectangle.Height);
		float num2 = (float)num / 20f;
		int num3 = (int)(90f * num2) - 25;
		int num4 = (int)((float)TabFormItem._136F / 2f * num2);
		GraphicsPath val = new GraphicsPath();
		int _136E = TabFormItem._136E;
		Rectangle rectangle2 = new Rectangle(rectangle.X - TabFormItem._136D, rectangle.Bottom - _136E, _136E, _136E);
		val.AddArc(rectangle2, 90f, (float)(-num3));
		int ܭ = TabFormItem.ܭ;
		rectangle2 = new Rectangle(rectangle.X + TabFormItem._136D + ܭ, rectangle.Y, _136E, _136E);
		val.AddArc(rectangle2, (float)(270 - num3), (float)num3);
		rectangle2 = new Rectangle(rectangle.Right - (_136E + ܭ + num4), rectangle.Y, _136E, _136E);
		val.AddArc(rectangle2, 270f, (float)num3);
		rectangle2 = new Rectangle(rectangle.Right - (_136E - ܭ * 2), rectangle.Bottom - _136E, _136E, _136E);
		val.AddArc(rectangle2, (float)(90 + num3), (float)(-num3));
		return val;
	}

	protected override Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
	{
		if (!_192F._1930(button, pa) || !(button is TabFormItem))
		{
			return base.GetTextColor(button, pa);
		}
		TabFormItem tabFormItem = button as TabFormItem;
		if (!tabFormItem.ForeColor.IsEmpty)
		{
			return tabFormItem.ForeColor;
		}
		Color result = Color.Empty;
		TabFormItemStateColorTable tabFormItemStateColorTable = ῡ(ጧ(tabFormItem), tabFormItem);
		if (tabFormItemStateColorTable != null)
		{
			result = tabFormItemStateColorTable.TextColor;
		}
		if (result.IsEmpty)
		{
			return base.GetTextColor(button, pa);
		}
		return result;
	}

	private TabFormItemColorTable ጧ(TabFormItem ᆰ)
	{
		return base.ᘽ.TabFormItemColorTables[ᆰ.ຽ()];
	}

	internal static TabFormItemStateColorTable ῡ(TabFormItemColorTable _21EE, TabFormItem ᆰ)
	{
		if (ᆰ.CustomColorTable != null)
		{
			_21EE = ᆰ.CustomColorTable;
		}
		if (_21EE == null)
		{
			return null;
		}
		TabFormItemStateColorTable tabFormItemStateColorTable = null;
		if (!ᆰ.ᎇ)
		{
			return _21EE.Default;
		}
		if (!ᆰ.GetEnabled())
		{
			return _21EE.Disabled;
		}
		if (ᆰ.Checked)
		{
			return _21EE.Selected;
		}
		if (ᆰ.IsMouseOver)
		{
			return _21EE.MouseOver;
		}
		return _21EE.Default;
	}
}
internal class _220D : ᯊ
{
	private int _220E => TabFormItem._136F / 2;

	protected override void PaintState(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r, bool isMouseDown)
	{
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		if (r.IsEmpty || !_192F._1930(button, pa) || r.Width == 0 || r.Height == 0)
		{
			return;
		}
		if (!(button is NewTabFormItem newTabFormItem) || IsOnMenu(button, pa))
		{
			base.PaintState(button, pa, image, r, isMouseDown);
			return;
		}
		_ = pa.IsOnMenu;
		TabFormItemColorTable tabFormItemColorTable = ጧ(newTabFormItem);
		if (tabFormItemColorTable == null)
		{
			return;
		}
		TabFormItemStateColorTable tabFormItemStateColorTable = ῡ(tabFormItemColorTable, newTabFormItem);
		if (tabFormItemStateColorTable == null)
		{
			return;
		}
		r.X += _220E;
		r.Width -= _220E;
		Graphics graphics = pa.Graphics;
		Region clip = graphics.Clip;
		try
		{
			Rectangle rectangle = _220B(newTabFormItem);
			rectangle.Inflate(2, 1);
			graphics.SetClip(rectangle, (CombineMode)1);
			if ((tabFormItemStateColorTable.BackColors != null && tabFormItemStateColorTable.BackColors.Length > 0) || (newTabFormItem.BackColors != null && newTabFormItem.BackColors.Length > 0))
			{
				Brush val = DisplayHelp.CreateBrush(r, (newTabFormItem.BackColors != null && newTabFormItem.BackColors.Length > 0) ? newTabFormItem.BackColors : tabFormItemStateColorTable.BackColors, tabFormItemStateColorTable.BackColorsGradientAngle, tabFormItemStateColorTable.BackColorsPositions);
				try
				{
					GraphicsPath val2 = ᔹ(r);
					graphics.FillPath(val, val2);
					newTabFormItem.TabPath = val2;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			if (tabFormItemStateColorTable.BorderColors.Length > 0)
			{
				Rectangle ल = r;
				Color[] borderColors = tabFormItemStateColorTable.BorderColors;
				foreach (Color color in borderColors)
				{
					GraphicsPath val3 = ᔹ(ल);
					try
					{
						Pen val4 = new Pen(color, (float)Dpi.Width1);
						try
						{
							val4.LineJoin = (LineJoin)2;
							graphics.DrawPath(val4, val3);
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
					ल.Inflate(-Dpi.Width1, -Dpi.Width1);
				}
			}
			graphics.Clip = clip;
		}
		finally
		{
			if (clip != null)
			{
				clip.Dispose();
			}
		}
	}

	private Rectangle _220B(NewTabFormItem ᆰ)
	{
		Rectangle displayRectangle = ᆰ.DisplayRectangle;
		displayRectangle.X += _220E;
		displayRectangle.Width -= _220E;
		GraphicsPath val = ᔹ(displayRectangle);
		try
		{
			return Rectangle.Round(val.GetBounds());
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private GraphicsPath ᔹ(Rectangle ल)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddLine(2, 0, 5, 14);
		val.AddLine(31, 14, 28, 0);
		val.CloseAllFigures();
		Matrix val2 = new Matrix();
		val2.Translate((float)ल.X, (float)ल.Y);
		if (Dpi.Factor.Width > 1f)
		{
			val2.Scale(Dpi.Factor.Width, Dpi.Factor.Width);
		}
		val.Transform(val2);
		return val;
	}

	private TabFormItemColorTable ጧ(NewTabFormItem ᆰ)
	{
		return base.ᘽ.TabFormItemColorTables[ᆰ.ຽ()];
	}

	internal static TabFormItemStateColorTable ῡ(TabFormItemColorTable _21EE, NewTabFormItem ᆰ)
	{
		if (_21EE == null)
		{
			return null;
		}
		TabFormItemStateColorTable tabFormItemStateColorTable = null;
		if (!ᆰ.ᎇ)
		{
			return _21EE.Default;
		}
		if (!ᆰ.GetEnabled())
		{
			return _21EE.Disabled;
		}
		if (ᆰ.Checked)
		{
			return _21EE.Selected;
		}
		if (ᆰ.IsMouseOver)
		{
			return _21EE.MouseOver;
		}
		return _21EE.Default;
	}
}
internal class _2211 : ᯊ
{
	public override Rectangle GetImageRectangle(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		Rectangle empty = Rectangle.Empty;
		IsOnMenu(button, pa);
		if (image != null)
		{
			empty.Width = image.ܐ + 16;
			empty.Height = image._0711 + 16;
			empty.X = button.DisplayRectangle.X + (button.DisplayRectangle.Width - empty.Width) / 2;
			empty.Y = button.DisplayRectangle.Y + 3;
		}
		return empty;
	}

	public override void PaintButtonImage(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle imagebounds)
	{
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		if (imagebounds.Width <= 0 || imagebounds.Height <= 0)
		{
			return;
		}
		if (button is _220F { _21F1: not null } _220F)
		{
			ElementStyle elementStyle = _220F._21F1.એ();
			ElementStyle titleStyle = _220F._21F1.TitleStyle;
			int num = 3;
			if (elementStyle.BackColorBlend.Count > 0)
			{
				DisplayHelp.FillRoundedRectangle(pa.Graphics, imagebounds, num, elementStyle.BackColorBlend[0].Color, elementStyle.BackColorBlend[elementStyle.BackColorBlend.Count - 1].Color, _220F._21F1.BackgroundStyle.BackColorGradientAngle);
			}
			else
			{
				DisplayHelp.FillRoundedRectangle(pa.Graphics, imagebounds, num, elementStyle.BackColor, elementStyle.BackColor2, elementStyle.BackColorGradientAngle);
			}
			if (!button.Expanded)
			{
				DisplayHelp.FillRectangle(pa.Graphics, new Rectangle(imagebounds.X + 1, imagebounds.Bottom - 8, imagebounds.Width - 2, 7), titleStyle.BackColor, titleStyle.BackColor2, titleStyle.BackColorGradientAngle);
			}
			if (!elementStyle.BorderColor.IsEmpty)
			{
				GraphicsPath val = new GraphicsPath();
				try
				{
					val.AddLine(imagebounds.X, imagebounds.Bottom - 8, imagebounds.Right, imagebounds.Bottom - 8);
					ElementStyleDisplay.AddCornerArc(val, imagebounds, num, eCornerArc.BottomRight);
					ElementStyleDisplay.AddCornerArc(val, imagebounds, num, eCornerArc.BottomLeft);
					val.CloseAllFigures();
					SolidBrush val2 = new SolidBrush(Color.FromArgb(192, elementStyle.BorderColor));
					try
					{
						pa.Graphics.FillPath((Brush)(object)val2, val);
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
			DisplayHelp.DrawRoundGradientRectangle(pa.Graphics, imagebounds, elementStyle.BorderColor, elementStyle.BorderColor2, elementStyle.BorderGradientAngle, 1, num);
			imagebounds.X += (imagebounds.Width - image.ܐ) / 2;
			imagebounds.Y += 4;
			imagebounds.Width = image.ܐ;
			imagebounds.Height = image._0711;
			image.ᇗ(pa.Graphics, imagebounds);
		}
		else
		{
			base.PaintButtonImage(button, pa, image, imagebounds);
		}
	}

	protected override Rectangle GetTextRectangle(ButtonItem button, ItemPaintArgs pa, eTextFormat stringFormat, ᇎ image)
	{
		Rectangle textRectangle = base.GetTextRectangle(button, pa, stringFormat, image);
		textRectangle.Offset(0, 12);
		return textRectangle;
	}

	protected override void PaintState(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r, bool isMouseDown)
	{
	}
}
internal class _2212 : ᯊ
{
	protected override void PaintStateBackground(ButtonItem button, Graphics g, Office2007ButtonItemStateColorTable stateColors, Rectangle r, IShapeDescriptor shape, bool isDefault, bool paintBorder)
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		if (stateColors != null && stateColors.Background != null && !stateColors.Background.IsEmpty)
		{
			Rectangle rectangle = r;
			ShadowPaintInfo shadowPaintInfo = new ShadowPaintInfo();
			shadowPaintInfo.Graphics = g;
			shadowPaintInfo.Rectangle = rectangle;
			ShadowPainter.Paint3(shadowPaintInfo);
			r.Width -= 4;
			r.Height -= 4;
			r.Offset(1, 1);
			DisplayHelp.FillRectangle(g, r, stateColors.Background);
			if (stateColors.BottomBackgroundHighlight != null && !stateColors.BottomBackgroundHighlight.IsEmpty)
			{
				Rectangle rectangle2 = new Rectangle(r.X, r.Y + r.Height / 2 - 2, r.Width, r.Height + 4);
				GraphicsPath val = new GraphicsPath();
				val.AddEllipse(rectangle2);
				PathGradientBrush val2 = new PathGradientBrush(val);
				val2.CenterColor = stateColors.BottomBackgroundHighlight.Start;
				val2.SurroundColors = new Color[1] { stateColors.BottomBackgroundHighlight.End };
				val2.CenterPoint = new PointF(rectangle2.X + rectangle2.Width / 2, r.Bottom);
				Blend val3 = new Blend();
				val3.Factors = new float[3] { 0f, 0.5f, 0.6f };
				val3.Positions = new float[3] { 0f, 0.4f, 1f };
				val2.Blend = val3;
				g.FillRectangle((Brush)(object)val2, r);
				((Brush)val2).Dispose();
				val.Dispose();
			}
			DisplayHelp.DrawGradientRectangle(g, r, stateColors.OuterBorder, 1);
			r.Inflate(-1, -1);
			DisplayHelp.DrawGradientRectangle(g, r, stateColors.InnerBorder, 1);
		}
	}
}
internal class _2213 : ᯊ
{
	public override Office2007ButtonItemColorTable GetColorTable(ButtonItem button, _21F0 buttonCont)
	{
		Office2007ColorTable office2007ColorTable = base.ᘽ;
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = null;
		if (button.CustomColorName != "")
		{
			office2007ButtonItemColorTable = office2007ColorTable.ApplicationButtonColors[button.CustomColorName];
		}
		if (office2007ButtonItemColorTable == null)
		{
			office2007ButtonItemColorTable = office2007ColorTable.ApplicationButtonColors[button.ຽ()];
		}
		if (office2007ButtonItemColorTable == null && office2007ColorTable.ApplicationButtonColors.Count > 0)
		{
			office2007ButtonItemColorTable = office2007ColorTable.ApplicationButtonColors[0];
		}
		if (office2007ButtonItemColorTable == null)
		{
			office2007ButtonItemColorTable = Office2010ColorTable._21AF();
		}
		return office2007ButtonItemColorTable;
	}

	public override void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, ᇎ image)
	{
		Rectangle displayRectangle = button.DisplayRectangle;
		displayRectangle.Inflate(-1, -1);
		displayRectangle.Width--;
		Office2007ButtonItemColorTable colorTable = GetColorTable(button, _21F0._1BF3);
		Color color = Color.FromArgb(32, Color.Black);
		if (colorTable.Default != null && colorTable.Default.OuterBorder != null && !colorTable.Default.OuterBorder.IsEmpty)
		{
			color = colorTable.Default.OuterBorder.Start;
		}
		if (pa.GlassEnabled)
		{
			ThemeTextFormat _21FD = (ThemeTextFormat)1048613;
			_21F8._2201(pa.Graphics, button.Text, pa.Font, displayRectangle, _21FD, textColor, color, _21FE: true);
			return;
		}
		eTextFormat format = eTextFormat.HidePrefix | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
		displayRectangle.Offset(1, 1);
		TextDrawing.DrawString(pa.Graphics, button.Text, pa.Font, color, displayRectangle, format);
		displayRectangle.Offset(-1, -1);
		TextDrawing.DrawString(pa.Graphics, button.Text, pa.Font, textColor, displayRectangle, format);
	}

	protected override Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
	{
		return base.GetTextColor(button, pa, GetColorTable(button, _21F0._1BF3));
	}

	public override void PaintButtonImage(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle imagebounds)
	{
		if (string.IsNullOrEmpty(button.Text))
		{
			Rectangle displayRectangle = button.DisplayRectangle;
			displayRectangle.X += (displayRectangle.Width - imagebounds.Width) / 2;
			displayRectangle.Y += (displayRectangle.Height - imagebounds.Height) / 2;
			displayRectangle.Width = imagebounds.Width;
			displayRectangle.Height = imagebounds.Height;
			base.PaintButtonImage(button, pa, image, displayRectangle);
		}
	}

	public override void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		Office2007ButtonItemColorTable colorTable = GetColorTable(button, _21F0._1BF3);
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = colorTable.Default;
		if (button.IsMouseDown)
		{
			office2007ButtonItemStateColorTable = colorTable.Pressed;
		}
		else if (button.IsMouseOver)
		{
			office2007ButtonItemStateColorTable = colorTable.MouseOver;
		}
		Rectangle displayRectangle = button.DisplayRectangle;
		displayRectangle.Width--;
		Graphics graphics = pa.Graphics;
		GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(displayRectangle, 2, 2, 0, 0);
		try
		{
			DisplayHelp.FillPath(graphics, roundedRectanglePath, office2007ButtonItemStateColorTable.Background);
			if (office2007ButtonItemStateColorTable.BottomBackgroundHighlight != null && !office2007ButtonItemStateColorTable.BottomBackgroundHighlight.IsEmpty)
			{
				Rectangle rectangle = new Rectangle(displayRectangle.X - 12, displayRectangle.Y + displayRectangle.Height / 2 - 4, displayRectangle.Width + 24, displayRectangle.Height + 4);
				GraphicsPath val = new GraphicsPath();
				try
				{
					val.AddEllipse(rectangle);
					PathGradientBrush val2 = new PathGradientBrush(val);
					try
					{
						val2.CenterColor = office2007ButtonItemStateColorTable.BottomBackgroundHighlight.Start;
						val2.SurroundColors = new Color[1] { office2007ButtonItemStateColorTable.BottomBackgroundHighlight.End };
						val2.CenterPoint = new PointF(rectangle.X + rectangle.Width / 2, displayRectangle.Bottom - 1);
						graphics.FillRectangle((Brush)(object)val2, displayRectangle);
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
			if (office2007ButtonItemStateColorTable.InnerBorder != null && !office2007ButtonItemStateColorTable.InnerBorder.IsEmpty)
			{
				Rectangle bounds = displayRectangle;
				bounds.Y++;
				bounds.Width--;
				bounds.X++;
				DisplayHelp.DrawGradientRectangle(graphics, bounds, office2007ButtonItemStateColorTable.InnerBorder, 1);
				Pen val3 = new Pen(ColorScheme.GetColor(10, 16777215));
				try
				{
					graphics.DrawLine(val3, bounds.X + 1, bounds.Y + 1, bounds.X + 1, bounds.Bottom - 1);
					graphics.DrawLine(val3, bounds.Right - 2, bounds.Y + 1, bounds.Right - 2, bounds.Bottom - 1);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			DisplayHelp.DrawGradientPathBorder(graphics, roundedRectanglePath, office2007ButtonItemStateColorTable.OuterBorder, 1);
		}
		finally
		{
			((IDisposable)roundedRectanglePath)?.Dispose();
		}
	}
}
internal class _245E : ᙓ
{
	private Office2007ColorTable __137C;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
		}
	}

	public virtual void Paint(ListBoxItemRendererEventArgs e)
	{
	}
}
internal class _245F : _245E
{
	private ᢏ __2461;

	public ᢏ _2460
	{
		get
		{
			return __2461;
		}
		set
		{
			__2461 = value;
		}
	}

	public override void Paint(ListBoxItemRendererEventArgs e)
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.က.Graphics;
		ListBoxItem item = e.Item;
		ListBoxAdv listBoxAdv = e.က.ContainerControl as ListBoxAdv;
		bool flag = false;
		bool flag2 = true;
		if (listBoxAdv != null)
		{
			flag = listBoxAdv.CheckBoxesVisible;
			flag2 = listBoxAdv.UseMnemonic;
		}
		OfficeListBoxItemColorTable listBoxItem = base.ᘽ.ListBoxItem;
		OfficeListBoxItemStateColorTable officeListBoxItemStateColorTable = (item.IsSelected ? listBoxItem.Selected : listBoxItem.Default);
		if (item.HotTracking && item.IsMouseOver)
		{
			officeListBoxItemStateColorTable = listBoxItem.MouseOver;
		}
		Rectangle bounds = item.Bounds;
		if (flag)
		{
			Office2007CheckBoxStateColorTable ዅ = ᢗ(item.CheckBoxMouseState);
			Size size = Dpi.Size(item.ܦ);
			Rectangle rectangle = new Rectangle(bounds.X + 2, bounds.Y + (bounds.Height - size.Height) / 2, size.Width, size.Height);
			__2461.ᢓ(graphics, rectangle, ዅ, item.CheckState);
			int num = Dpi.Width(6);
			bounds.X += size.Width + num - 1;
			bounds.Width -= size.Width + num - 1;
			item._1758 = rectangle;
		}
		if ((officeListBoxItemStateColorTable.BackColors != null && officeListBoxItemStateColorTable.BackColors.Length > 0) || (item.BackColors != null && item.BackColors.Length > 0))
		{
			Brush val = DisplayHelp.CreateBrush(bounds, (item.BackColors != null && item.BackColors.Length > 0) ? item.BackColors : officeListBoxItemStateColorTable.BackColors, officeListBoxItemStateColorTable.BackColorsGradientAngle, officeListBoxItemStateColorTable.BackColorsPositions);
			try
			{
				graphics.FillRectangle(val, bounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		Color textColor = officeListBoxItemStateColorTable.TextColor;
		if (!string.IsNullOrEmpty(item.SymbolRealized))
		{
			Color color = item.SymbolColor;
			if (color.IsEmpty)
			{
				color = textColor;
			}
			TextDrawing.DrawStringLegacy(graphics, item.SymbolRealized, Symbols.GetFont(item.SymbolSize, item.SymbolSet), color, new Rectangle(bounds.X, bounds.Y + bounds.Height / 2, 0, 0), eTextFormat.VerticalCenter);
			int num2 = item.ᬦ.Width + Dpi.Width(4);
			bounds.Width -= num2;
			bounds.X += num2;
		}
		else if (item.Image != null)
		{
			Size size2 = Dpi.ImageSize(item.Image.Size);
			graphics.DrawImage(item.Image, new Rectangle(bounds.X, bounds.Y + (bounds.Height - size2.Height) / 2, size2.Width, size2.Height));
			int num3 = size2.Width + Dpi.Width(4);
			bounds.Width -= num3;
			bounds.X += num3;
		}
		if (string.IsNullOrEmpty(item.Text))
		{
			return;
		}
		if (flag)
		{
			bounds.X++;
			bounds.Width--;
		}
		if (!item.TextColor.IsEmpty)
		{
			textColor = item.TextColor;
		}
		Font font = e.က.Font;
		if (item.TextMarkupBody == null)
		{
			eTextFormat eTextFormat = eTextFormat.VerticalCenter;
			if (item.TextAlignment == eButtonTextAlignment.Center)
			{
				eTextFormat |= eTextFormat.HorizontalCenter;
			}
			else if (item.TextAlignment == eButtonTextAlignment.Right)
			{
				eTextFormat |= eTextFormat.Right;
			}
			if (!flag2)
			{
				eTextFormat |= eTextFormat.NoPrefix;
			}
			TextDrawing.DrawString(graphics, item.Text, font, textColor, bounds, eTextFormat);
		}
		else
		{
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, textColor, e.က.RightToLeft);
			markupDrawContext.HotKeyPrefixVisible = false;
			markupDrawContext.ContextObject = item;
			Rectangle bounds2 = new Rectangle(bounds.X, bounds.Y + (bounds.Height - item.TextMarkupBody.Bounds.Height) / 2, item.TextMarkupBody.Bounds.Width, item.TextMarkupBody.Bounds.Height);
			item.TextMarkupBody.Bounds = bounds2;
			item.TextMarkupBody.Render(markupDrawContext);
		}
	}

	private Office2007CheckBoxStateColorTable ᢗ(eMouseState య)
	{
		return య switch
		{
			eMouseState.Down => base.ᘽ.CheckBoxItem.Pressed, 
			eMouseState.Hot => base.ᘽ.CheckBoxItem.MouseOver, 
			_ => base.ᘽ.CheckBoxItem.Default, 
		};
	}
}
internal class _24A7 : ᙓ
{
	private Office2007ColorTable __137C;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
		}
	}

	public virtual void Paint(StepItemRendererEventArgs e)
	{
	}
}
internal class _24A8 : _24A7
{
	public override void Paint(StepItemRendererEventArgs e)
	{
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		StepItem item = e.Item;
		OfficeStepItemColorTable stepItem = base.ᘽ.StepItem;
		OfficeStepItemStateColorTable officeStepItemStateColorTable = stepItem.Default;
		if (item.HotTracking && item.IsMouseDown)
		{
			officeStepItemStateColorTable = stepItem.Pressed;
		}
		else if (item.HotTracking && item.IsMouseOver)
		{
			officeStepItemStateColorTable = stepItem.MouseOver;
		}
		OfficeStepItemStateColorTable progress = stepItem.Progress;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle bounds = item.Bounds;
		int num = item._24A5();
		Graphics graphics = e.က.Graphics;
		GraphicsPath val2 = (item.ItemPath = ഗ(item, num));
		Brush val3 = DisplayHelp.CreateBrush(val2.GetBounds(), (item.BackColors != null && item.BackColors.Length > 0) ? item.BackColors : officeStepItemStateColorTable.BackColors, officeStepItemStateColorTable.BackColorsGradientAngle, officeStepItemStateColorTable.BackColorsPositions);
		try
		{
			graphics.FillPath(val3, val2);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		if (item.Value > item.Minimum)
		{
			float num2 = Math.Min(1f, (float)item.Value / (float)(item.Maximum - item.Minimum));
			if (num2 > 0f)
			{
				rectangle = item.Bounds;
				rectangle.Width = (int)((float)rectangle.Width * num2);
			}
			if (!rectangle.IsEmpty)
			{
				Region clip = graphics.Clip;
				graphics.SetClip(rectangle, (CombineMode)1);
				Brush val4 = DisplayHelp.CreateBrush(val2.GetBounds(), (item.ProgressColors != null && item.ProgressColors.Length > 0) ? item.ProgressColors : progress.BackColors, progress.BackColorsGradientAngle, progress.BackColorsPositions);
				try
				{
					graphics.FillPath(val4, val2);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				graphics.Clip = clip;
				clip.Dispose();
			}
		}
		if (officeStepItemStateColorTable.BorderColors.Length > 0)
		{
			for (int num3 = officeStepItemStateColorTable.BorderColors.Length - 1; num3 > 0; num3--)
			{
				Rectangle bounds2 = item.Bounds;
				bounds2.Inflate(-num3, -num3);
				GraphicsPath val5 = ഗ(item, num, bounds2);
				try
				{
					Pen val6 = new Pen(officeStepItemStateColorTable.BorderColors[num3]);
					try
					{
						graphics.DrawPath(val6, val5);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			Pen val7 = new Pen(officeStepItemStateColorTable.BorderColors[0]);
			try
			{
				graphics.DrawPath(val7, val2);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		bounds.X += item.Padding.Left;
		bounds.Y += item.Padding.Top;
		bounds.Width -= item.Padding.Horizontal;
		bounds.Height -= item.Padding.Vertical;
		if (!item._249E)
		{
			bounds.X += num;
			bounds.Width -= num;
		}
		Color textColor = officeStepItemStateColorTable.TextColor;
		if (!string.IsNullOrEmpty(item.SymbolRealized))
		{
			Color color = item.SymbolColor;
			if (color.IsEmpty)
			{
				color = textColor;
			}
			TextDrawing.DrawStringLegacy(graphics, item.SymbolRealized, Symbols.GetFont(item.SymbolSize, item.SymbolSet), color, new Rectangle(bounds.X, bounds.Y + bounds.Height / 2, 0, 0), eTextFormat.VerticalCenter);
			int num4 = item.ᬦ.Width + item.ImageTextSpacing;
			bounds.Width -= num4;
			bounds.X += num4;
		}
		else if (item.Image != null)
		{
			graphics.DrawImage(item.Image, new Rectangle(bounds.X, bounds.Y + (bounds.Height - item.Image.Height) / 2, item.Image.Width, item.Image.Height));
			int num5 = item.Image.Width + item.ImageTextSpacing;
			bounds.Width -= num5;
			bounds.X += num5;
		}
		if (string.IsNullOrEmpty(item.Text))
		{
			return;
		}
		if (!item.TextColor.IsEmpty)
		{
			textColor = item.TextColor;
		}
		Font font = e.က.Font;
		if (item.TextMarkupBody == null)
		{
			eTextFormat eTextFormat = eTextFormat.VerticalCenter;
			if (item.TextAlignment == eButtonTextAlignment.Center)
			{
				eTextFormat |= eTextFormat.HorizontalCenter;
				if (!item._249F)
				{
					bounds.Width -= num;
				}
			}
			else if (item.TextAlignment == eButtonTextAlignment.Right)
			{
				eTextFormat |= eTextFormat.Right;
				if (!item._249F)
				{
					bounds.Width -= num;
				}
			}
			TextDrawing.DrawString(graphics, item.Text, font, textColor, bounds, eTextFormat);
		}
		else
		{
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, textColor, e.က.RightToLeft);
			markupDrawContext.HotKeyPrefixVisible = false;
			markupDrawContext.ContextObject = item;
			Rectangle bounds3 = new Rectangle(bounds.X, bounds.Y + (bounds.Height - item.TextMarkupBody.Bounds.Height) / 2, item.TextMarkupBody.Bounds.Width, item.TextMarkupBody.Bounds.Height);
			item.TextMarkupBody.Bounds = bounds3;
			item.TextMarkupBody.Render(markupDrawContext);
		}
	}

	private GraphicsPath ഗ(StepItem _06EC, int _24A9)
	{
		Rectangle bounds = _06EC.Bounds;
		return ഗ(_06EC, _24A9, bounds);
	}

	private GraphicsPath ഗ(StepItem _06EC, int _24A9, Rectangle ल)
	{
		ल.Width--;
		ल.Height--;
		if (_06EC._249E)
		{
			return _24AC(ल, _24A9);
		}
		if (_06EC._249F)
		{
			return _24AB(ल, _24A9);
		}
		return _24AA(ल, _24A9);
	}

	private GraphicsPath _24AA(Rectangle ल, int _24A9)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddLine(ल.X, ल.Y, ल.X + _24A9, ल.Y + ल.Height / 2);
		val.AddLine(ल.X + _24A9, ल.Y + ल.Height / 2, ल.X, ल.Bottom);
		val.AddLine(ल.X, ल.Bottom, ल.Right - _24A9, ल.Bottom);
		val.AddLine(ल.Right - _24A9, ल.Bottom, ल.Right, ल.Y + ल.Height / 2);
		val.AddLine(ल.Right, ल.Y + ल.Height / 2, ल.Right - _24A9, ल.Y);
		val.CloseAllFigures();
		return val;
	}

	private GraphicsPath _24AB(Rectangle ल, int _24A9)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		ᒇ ᒇ = ElementStyleDisplay.സ(ल, 2, eCornerArc.TopRight);
		val.AddArc(ᒇ.ᒈ, ᒇ.ᒉ, ᒇ.ܐ, ᒇ._0711, ᒇ.ᒊ, ᒇ.ᒋ);
		ᒇ = ElementStyleDisplay.സ(ल, 2, eCornerArc.BottomRight);
		val.AddArc(ᒇ.ᒈ, ᒇ.ᒉ, ᒇ.ܐ, ᒇ._0711, ᒇ.ᒊ, ᒇ.ᒋ);
		val.AddLine(ल.X, ल.Bottom, ल.X + _24A9, ल.Y + ल.Height / 2);
		val.AddLine(ल.X + _24A9, ल.Y + ल.Height / 2, ल.X, ल.Y);
		val.CloseAllFigures();
		return val;
	}

	private GraphicsPath _24AC(Rectangle ल, int _24A9)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		ᒇ ᒇ = ElementStyleDisplay.സ(ल, 2, eCornerArc.BottomLeft);
		val.AddArc(ᒇ.ᒈ, ᒇ.ᒉ, ᒇ.ܐ, ᒇ._0711, ᒇ.ᒊ, ᒇ.ᒋ);
		ᒇ = ElementStyleDisplay.സ(ल, 2, eCornerArc.TopLeft);
		val.AddArc(ᒇ.ᒈ, ᒇ.ᒉ, ᒇ.ܐ, ᒇ._0711, ᒇ.ᒊ, ᒇ.ᒋ);
		val.AddLine(ल.Right - _24A9, ल.Y, ल.Right, ल.Y + ल.Height / 2);
		val.AddLine(ल.Right, ल.Y + ल.Height / 2, ल.Right - _24A9, ल.Bottom);
		val.CloseAllFigures();
		return val;
	}
}
internal class _24BD : _24BE, ᙓ
{
	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void Paint(ProgressBarItemRenderEventArgs e)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		Rectangle displayRectangle = e.ProgressBarItem.DisplayRectangle;
		if (displayRectangle.Width <= 0 || displayRectangle.Height <= 0)
		{
			return;
		}
		ProgressBarItem progressBarItem = e.ProgressBarItem;
		Office2007ProgressBarColorTable office2007ProgressBarColorTable = ຊ.ProgressBarItem;
		if (progressBarItem.ColorTable == eProgressBarItemColor.Paused)
		{
			office2007ProgressBarColorTable = ຊ.ProgressBarItemPaused;
		}
		else if (progressBarItem.ColorTable == eProgressBarItemColor.Error)
		{
			office2007ProgressBarColorTable = ຊ.ProgressBarItemError;
		}
		Graphics graphics = e.Graphics;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)0;
		if (StyleManager.IsMetro(progressBarItem.EffectiveStyle))
		{
			DisplayHelp.DrawRectangle(graphics, office2007ProgressBarColorTable.OuterBorder, displayRectangle);
		}
		else
		{
			DisplayHelp.DrawRoundedRectangle(graphics, office2007ProgressBarColorTable.OuterBorder, displayRectangle, 2);
		}
		displayRectangle.Inflate(-Dpi.Width1, -Dpi.Height1);
		Brush val = DisplayHelp.CreateBrush(displayRectangle, office2007ProgressBarColorTable.BackgroundColors);
		if (val != null)
		{
			graphics.FillRectangle(val, displayRectangle);
			val.Dispose();
		}
		graphics.SmoothingMode = smoothingMode;
		if (!office2007ProgressBarColorTable.InnerBorder.IsEmpty)
		{
			DisplayHelp.DrawRoundedRectangle(graphics, office2007ProgressBarColorTable.InnerBorder, displayRectangle, 2);
		}
		displayRectangle.Inflate(-Dpi.Width1, -Dpi.Height1);
		Region clip = graphics.Clip;
		try
		{
			graphics.SetClip(displayRectangle, (CombineMode)1);
			Rectangle rectangle = displayRectangle;
			if (progressBarItem.ProgressType == eProgressItemType.Marquee)
			{
				if (progressBarItem.Orientation == eOrientation.Horizontal)
				{
					rectangle.Width = (int)((double)rectangle.Width * 0.3);
					rectangle.X += displayRectangle.Width * progressBarItem.ṓ / 100 - rectangle.Width / 2;
				}
				else if (progressBarItem.Orientation == eOrientation.Vertical)
				{
					rectangle.Height = (int)((double)rectangle.Height * 0.3);
					rectangle.Y += displayRectangle.Height * progressBarItem.ṓ / 100 - rectangle.Height / 2;
				}
			}
			else if (progressBarItem.Orientation == eOrientation.Horizontal)
			{
				rectangle.Width = (int)((float)rectangle.Width * ((float)(progressBarItem.Value - progressBarItem.Minimum) / (float)(progressBarItem.Maximum - progressBarItem.Minimum)));
			}
			else if (progressBarItem.Orientation == eOrientation.Vertical)
			{
				int num = (int)((float)rectangle.Height * ((float)(progressBarItem.Value - progressBarItem.Minimum) / (float)(progressBarItem.Maximum - progressBarItem.Minimum)));
				rectangle.Y = rectangle.Bottom - num;
				rectangle.Height = num;
			}
			if (rectangle.Width <= 0 || rectangle.Height <= 0)
			{
				return;
			}
			val = DisplayHelp.CreateBrush(rectangle, office2007ProgressBarColorTable.Chunk);
			if (val != null)
			{
				graphics.SmoothingMode = (SmoothingMode)3;
				graphics.FillRectangle(val, rectangle);
				val.Dispose();
				graphics.SmoothingMode = smoothingMode;
			}
			GradientColorTable chunkOverlay = office2007ProgressBarColorTable.ChunkOverlay;
			if (progressBarItem.Orientation == eOrientation.Horizontal)
			{
				chunkOverlay.LinearGradientAngle = 90;
			}
			else
			{
				chunkOverlay.LinearGradientAngle = 0;
			}
			val = DisplayHelp.CreateBrush(rectangle, chunkOverlay);
			if (val != null)
			{
				graphics.FillRectangle(val, rectangle);
				if (progressBarItem.ProgressType == eProgressItemType.Marquee)
				{
					if (progressBarItem.Orientation == eOrientation.Horizontal && rectangle.Right > displayRectangle.Right + 4)
					{
						rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y, rectangle.Right - displayRectangle.Right - 4, displayRectangle.Height);
					}
					else if (progressBarItem.Orientation == eOrientation.Vertical && rectangle.Bottom > displayRectangle.Bottom + 4)
					{
						rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y, displayRectangle.Height, rectangle.Bottom - displayRectangle.Bottom - 4);
					}
					graphics.FillRectangle(val, rectangle);
				}
				val.Dispose();
			}
			if (progressBarItem.Orientation == eOrientation.Horizontal && rectangle.Right + 4 <= displayRectangle.Right)
			{
				rectangle.X = rectangle.Right;
				rectangle.Width = 4;
				val = DisplayHelp.CreateBrush(rectangle, office2007ProgressBarColorTable.ChunkShadow);
				if (val != null)
				{
					graphics.FillRectangle(val, rectangle);
					val.Dispose();
				}
			}
			else if (progressBarItem.Orientation == eOrientation.Vertical && rectangle.Y - 3 >= displayRectangle.Y)
			{
				rectangle.Y -= 3;
				rectangle.Height = 3;
				val = DisplayHelp.CreateBrush(rectangle, office2007ProgressBarColorTable.ChunkShadow.Colors, office2007ProgressBarColorTable.ChunkShadow.LinearGradientAngle - 90, office2007ProgressBarColorTable.ChunkShadow.GradientType);
				if (val != null)
				{
					graphics.FillRectangle(val, rectangle);
					val.Dispose();
				}
			}
		}
		finally
		{
			graphics.Clip = clip;
		}
	}
}
internal abstract class _24BE
{
	public abstract void Paint(ProgressBarItemRenderEventArgs e);
}
internal class _24BF : _24C0, ᙓ
{
	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void PaintButtonBackground(NavPaneRenderEventArgs e)
	{
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		Office2007NavigationPaneColorTable navigationPane = ຊ.NavigationPane;
		Brush val = DisplayHelp.CreateBrush(bounds, navigationPane.ButtonBackground);
		try
		{
			graphics.FillRectangle(val, bounds);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
internal abstract class _24C0
{
	public abstract void PaintButtonBackground(NavPaneRenderEventArgs e);
}
internal class _24C1
{
	public virtual void Paint(SliderItemRendererEventArgs e)
	{
	}
}
internal class _24DE
{
	public virtual void PaintSideBar(SideBarRendererEventArgs e)
	{
	}

	public virtual void PaintSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
	{
	}
}
internal class _24E7
{
	public virtual void Paint(RangeSliderItemRendererEventArgs e)
	{
	}
}
internal class _250F : ᣒ, ᙓ
{
	private static readonly int _2510 = 2;

	private Office2007ColorTable __137C;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
		}
	}

	public override void Paint(SwitchButtonRenderEventArgs e)
	{
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_0949: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Expected O, but got Unknown
		//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
		SwitchButtonItem switchButtonItem = e.SwitchButtonItem;
		bool enabled = switchButtonItem.Enabled;
		SwitchButtonColorTable switchButtonColorTable = (enabled ? ᘽ.SwitchButton.Default : ᘽ.SwitchButton.Disabled);
		if (switchButtonColorTable == null)
		{
			switchButtonColorTable = new SwitchButtonColorTable();
		}
		Rectangle bounds = switchButtonItem.Bounds;
		int num = Dpi.Width(switchButtonItem.ButtonWidth);
		Padding padding = Dpi.Size(switchButtonItem.Margin);
		if (e.က != null && e.က.ContainerControl is DevComponents.AdvTree.AdvTree)
		{
			if (switchButtonItem.ItemAlignment == eItemAlignment.Far)
			{
				bounds.X = bounds.Right - padding.Right - num;
			}
			else if (switchButtonItem.ItemAlignment == eItemAlignment.Center)
			{
				bounds.X += (bounds.Width - num) / 2;
			}
		}
		else
		{
			bounds.X = bounds.Right - padding.Right - num;
		}
		bounds.Width = num;
		int num2 = Dpi.Height(switchButtonItem.ButtonHeight);
		bounds.Y += padding.Top + (bounds.Height - padding.Vertical - num2) / 2;
		bounds.Height = num2;
		switchButtonItem.ButtonBounds = bounds;
		bool flag = e.က != null && e.က.GlassEnabled && ((switchButtonItem.Parent is ᱭ && !(e.က.ContainerControl is Ꮮ)) || (switchButtonItem.Parent is ය && switchButtonItem.EffectiveStyle == eDotNetBarStyle.Office2010));
		Graphics graphics = e.Graphics;
		if (switchButtonItem.TextVisible && !string.IsNullOrEmpty(switchButtonItem.Text))
		{
			Rectangle bounds2 = switchButtonItem.Bounds;
			Padding padding2 = Dpi.Size(switchButtonItem.TextPadding);
			bounds2.Width -= num + padding.Right + padding2.Horizontal;
			bounds2.Y += padding2.Top;
			bounds2.X += padding2.Left;
			bounds2.Height -= padding2.Vertical;
			bool rightToLeft = e.RightToLeft;
			Color color = ((switchButtonItem.TextColor.IsEmpty || !enabled) ? switchButtonColorTable.TextColor : switchButtonItem.TextColor);
			Font font = e.Font;
			eTextFormat eTextFormat = eTextFormat.VerticalCenter;
			if (switchButtonItem.TextMarkupBody != null)
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, color, rightToLeft);
				markupDrawContext.HotKeyPrefixVisible = (eTextFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
				if ((eTextFormat & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
				{
					bounds2.Y = switchButtonItem.TopInternal + (switchButtonItem.Bounds.Height - switchButtonItem.TextMarkupBody.Bounds.Height) / 2;
				}
				else if ((eTextFormat & eTextFormat.Bottom) == eTextFormat.Bottom)
				{
					bounds2.Y += switchButtonItem.TextMarkupBody.Bounds.Height - bounds2.Height + 1;
				}
				bounds2.Height = switchButtonItem.TextMarkupBody.Bounds.Height;
				switchButtonItem.TextMarkupBody.Bounds = bounds2;
				switchButtonItem.TextMarkupBody.Render(markupDrawContext);
			}
			else if (flag)
			{
				if (!e.က.ໟ)
				{
					_21F8._21FC(graphics, switchButtonItem.Text, font, bounds2, TextDrawing.GetTextFormat(eTextFormat));
				}
			}
			else
			{
				TextDrawing.DrawString(graphics, switchButtonItem.Text, font, color, bounds2, eTextFormat);
			}
		}
		bool value = switchButtonItem.Value;
		string offText = switchButtonItem.OffText;
		string onText = switchButtonItem.OnText;
		Font font2 = (Font)((switchButtonItem.SwitchFont != null) ? ((object)switchButtonItem.SwitchFont) : ((object)new Font(e.Font, (FontStyle)1)));
		Color color2 = ((switchButtonItem.OffTextColor.IsEmpty || !enabled) ? switchButtonColorTable.OffTextColor : switchButtonItem.OffTextColor);
		Color color3 = ((switchButtonItem.OnTextColor.IsEmpty || !enabled) ? switchButtonColorTable.OnTextColor : switchButtonItem.OnTextColor);
		int num3 = Dpi.Width(switchButtonItem.SwitchWidth);
		int num4 = Math.Min(bounds.X + switchButtonItem.ᢪ, bounds.Right);
		if (value)
		{
			num4 = Math.Max(bounds.Right - num3 - switchButtonItem.ᢪ, bounds.X);
		}
		Color color4 = ((switchButtonItem.BorderColor.IsEmpty || !enabled) ? switchButtonColorTable.BorderColor : switchButtonItem.BorderColor);
		Color color5 = ((switchButtonItem.OffBackColor.IsEmpty || !enabled) ? switchButtonColorTable.OffBackColor : switchButtonItem.OffBackColor);
		Color color6 = ((switchButtonItem.OnBackColor.IsEmpty || !enabled) ? switchButtonColorTable.OnBackColor : switchButtonItem.OnBackColor);
		Rectangle r = bounds;
		r.Inflate(-1, -1);
		GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(r, _2510 - 1);
		Region clip = graphics.Clip;
		graphics.SetClip(roundedRectanglePath, (CombineMode)1);
		roundedRectanglePath.Dispose();
		Rectangle bounds3 = (switchButtonItem.OnPartBounds = new Rectangle(num4 - (bounds.Width - num3), bounds.Y, bounds.Width - num3 + 2, bounds.Height));
		DisplayHelp.FillRoundedRectangle(graphics, bounds3, _2510 - 1, color6);
		DisplayHelp.FillRoundedRectangle(graphics, bounds3, _2510 - 1, Color.FromArgb((color6.GetBrightness() > 0.9f) ? 8 : 24, Color.Black), Color.FromArgb(32, Color.White));
		if (flag && BarUtilities.UseTextRenderer)
		{
			TextDrawing.DrawStringLegacy(graphics, onText, font2, color3, bounds3, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
		else
		{
			TextDrawing.DrawString(graphics, onText, font2, color3, bounds3, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
		Rectangle bounds4 = (switchButtonItem.OffPartBounds = new Rectangle(num4 + num3 - 2, bounds.Y, bounds.Width - num3 + 2, bounds.Height));
		DisplayHelp.FillRoundedRectangle(graphics, bounds4, _2510, color5);
		DisplayHelp.FillRoundedRectangle(graphics, bounds4, _2510, Color.FromArgb((color5.GetBrightness() > 0.9f) ? 12 : 32, Color.Black), Color.FromArgb(64, Color.White));
		if (flag && BarUtilities.UseTextRenderer)
		{
			TextDrawing.DrawStringLegacy(graphics, offText, font2, color2, bounds4, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
		else
		{
			TextDrawing.DrawString(graphics, offText, font2, color2, bounds4, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
		graphics.Clip = clip;
		clip.Dispose();
		DisplayHelp.DrawRoundedRectangle(graphics, color4, bounds, _2510);
		new LinearGradientColorTable(Color.FromArgb(96, Color.DarkGray), Color.Transparent, 90);
		Rectangle rectangle3 = bounds;
		rectangle3.Inflate(-_2510 / 2, -1);
		DisplayHelp.DrawLine(graphics, rectangle3.X, rectangle3.Y, rectangle3.Right, rectangle3.Y, Color.FromArgb(24, Color.Black), 1);
		rectangle3.Y++;
		DisplayHelp.DrawLine(graphics, rectangle3.X, rectangle3.Y, rectangle3.Right, rectangle3.Y, Color.FromArgb(8, Color.Black), 1);
		Rectangle bounds5 = (switchButtonItem.SwitchBounds = new Rectangle(num4, bounds.Y, num3, bounds.Height));
		Color color7 = ((switchButtonItem.SwitchBorderColor.IsEmpty || !enabled) ? switchButtonColorTable.SwitchBorderColor : switchButtonItem.SwitchBorderColor);
		Color fillColor = ((switchButtonItem.SwitchBackColor.IsEmpty || !enabled) ? switchButtonColorTable.SwitchBackColor : switchButtonItem.SwitchBackColor);
		if (!switchButtonColorTable.SwitchOnBackColor.IsEmpty && !switchButtonItem.Value && switchButtonItem.SwitchBackColor.IsEmpty)
		{
			fillColor = switchButtonColorTable.SwitchOnBackColor;
		}
		if (bounds5.Right + 2 < bounds.Right)
		{
			DisplayHelp.DrawLine(graphics, bounds5.Right, bounds5.Y + 3, bounds5.Right, bounds5.Bottom - 2, Color.FromArgb(24, Color.Black), 1);
			DisplayHelp.DrawLine(graphics, bounds5.Right + 1, bounds5.Y + 3, bounds5.Right + 1, bounds5.Bottom - 2, Color.FromArgb(8, Color.Black), 1);
		}
		DisplayHelp.DrawRoundedRectangle(graphics, color7, fillColor, bounds5, _2510);
		DisplayHelp.FillRoundedRectangle(graphics, bounds5, _2510, Color.FromArgb((fillColor.GetBrightness() > 0.9f) ? 42 : 72, Color.Black), Color.FromArgb(64, Color.White));
		DisplayHelp.DrawLine(graphics, bounds5.X + 2, bounds5.Y + 1, bounds5.Right - 3, bounds5.Y + 1, Color.FromArgb(164, Color.White), 1);
		if (switchButtonItem.IsReadOnly && switchButtonItem.ShowReadOnlyMarker)
		{
			Color readOnlyMarkerColor = switchButtonItem.ReadOnlyMarkerColor;
			Rectangle rectangle5 = new Rectangle(bounds5.X + (bounds5.Width - 7) / 2, bounds5.Y + (bounds5.Height - 10) / 2, 7, 10);
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)3;
			SolidBrush val = new SolidBrush(readOnlyMarkerColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val, new Rectangle(rectangle5.X, rectangle5.Y + 4, rectangle5.Width, rectangle5.Height - 4));
				graphics.FillRectangle(Brushes.White, new Rectangle(rectangle5.X + 3, rectangle5.Y + 5, 1, 2));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			Pen val2 = new Pen(readOnlyMarkerColor, 1f);
			try
			{
				graphics.DrawLine(val2, rectangle5.X + 2, rectangle5.Y, rectangle5.X + 4, rectangle5.Y);
				graphics.DrawLine(val2, rectangle5.X + 1, rectangle5.Y + 1, rectangle5.X + 1, rectangle5.Y + 3);
				graphics.DrawLine(val2, rectangle5.X + 5, rectangle5.Y + 1, rectangle5.X + 5, rectangle5.Y + 3);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			graphics.SmoothingMode = smoothingMode;
		}
	}
}
internal class _2511 : _2514
{
	public override void Paint(SideNavItemRendererEventArgs e)
	{
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		Graphics graphics = e.က.Graphics;
		SideNavItem item = e.Item;
		_ = e.က.ContainerControl;
		SideNavItemColorTable sideNavItem = base.ᘽ.SideNav.SideNavItem;
		SideNavItemStateColorTable sideNavItemStateColorTable = (item.IsMouseOver ? sideNavItem.MouseOver : sideNavItem.Default);
		if (item.Checked)
		{
			sideNavItemStateColorTable = sideNavItem.Selected;
		}
		else if (item.IsMouseDown)
		{
			sideNavItemStateColorTable = sideNavItem.Pressed;
		}
		Rectangle bounds = item.Bounds;
		Rectangle rectangle = item._136B;
		Rectangle rectangle2 = item._136A;
		ᇎ ᇎ = item._0EC5();
		if ((sideNavItemStateColorTable.BackColors != null && sideNavItemStateColorTable.BackColors.Length > 0) || (item.BackColors != null && item.BackColors.Length > 0))
		{
			Brush val = DisplayHelp.CreateBrush(bounds, (item.BackColors != null && item.BackColors.Length > 0) ? item.BackColors : sideNavItemStateColorTable.BackColors, sideNavItemStateColorTable.BackColorsGradientAngle, sideNavItemStateColorTable.BackColorsPositions);
			try
			{
				DisplayHelp.FillRoundedRectangle(graphics, val, bounds, sideNavItemStateColorTable.CornerRadius);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		Region val2 = null;
		if (bounds.Y == 0 && item.Checked)
		{
			val2 = graphics.Clip;
			graphics.SetClip(new Rectangle(bounds.X, bounds.Y, bounds.Width, 1), (CombineMode)4);
		}
		if (item.BorderColors != null && item.BorderColors.Length > 0)
		{
			DisplayHelp.DrawRoundedRectangle(graphics, bounds, item.BorderColors, sideNavItemStateColorTable.CornerRadius);
		}
		else if (sideNavItemStateColorTable.BorderColors != null && sideNavItemStateColorTable.BorderColors.Length > 0)
		{
			DisplayHelp.DrawRoundedRectangle(graphics, bounds, sideNavItemStateColorTable.BorderColors, sideNavItemStateColorTable.CornerRadius);
		}
		if (bounds.Y == 0 && item.Checked)
		{
			graphics.Clip = val2;
			val2.Dispose();
		}
		Color color = sideNavItemStateColorTable.TextColor;
		bool flag = false;
		if ((ᇎ != null || !string.IsNullOrEmpty(item.SymbolRealized)) && item.ButtonStyle != eButtonStyle.TextOnlyAlways)
		{
			if (rectangle2.IsEmpty)
			{
				rectangle2 = _2512(item, ᇎ);
			}
			if (rectangle.IsEmpty)
			{
				rectangle = ഷ(item, ᇎ, rectangle2);
			}
			flag = true;
		}
		else if (rectangle.IsEmpty)
		{
			rectangle = bounds;
			bounds.X += 2;
			bounds.Width -= 2;
		}
		if (!string.IsNullOrEmpty(item.SymbolRealized))
		{
			Color color2 = item.SymbolColor;
			if (color2.IsEmpty)
			{
				color2 = color;
			}
			TextDrawing.DrawStringLegacy(graphics, item.SymbolRealized, Symbols.GetFont(item.SymbolSize, item.SymbolSet), color2, new Rectangle(rectangle2.X, rectangle2.Y + rectangle2.Height / 2, 0, 0), eTextFormat.VerticalCenter);
		}
		else if (ᇎ != null)
		{
			if (!item.IsMouseOver && item.HotTrackingStyle == eHotTrackingStyle.Color)
			{
				ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
				{
					new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
					new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
					new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0.2f, 0.2f, 0.2f, 0f, 1f }
				});
				ImageAttributes val3 = new ImageAttributes();
				val3.SetColorMatrix(colorMatrix);
				ᇎ.ᇗ(graphics, rectangle2, 0, 0, ᇎ.ᇕ, ᇎ.ᇔ, (GraphicsUnit)2, val3);
			}
			else
			{
				ᇎ.ᇗ(graphics, rectangle2);
			}
		}
		item._136A = rectangle2;
		if (!string.IsNullOrEmpty(item.Text) && (item.ButtonStyle != 0 || !flag))
		{
			if (!item.ForeColor.IsEmpty)
			{
				color = item.ForeColor;
			}
			Font font = e.က.Font;
			if (item.TextMarkupBody == null)
			{
				eTextFormat format = eTextFormat.VerticalCenter;
				TextDrawing.DrawString(graphics, item.Text, font, color, rectangle, format);
			}
			else
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, color, e.က.RightToLeft);
				markupDrawContext.HotKeyPrefixVisible = false;
				markupDrawContext.ContextObject = item;
				Rectangle bounds2 = new Rectangle(rectangle.X, rectangle.Y + (rectangle.Height - item.TextMarkupBody.Bounds.Height) / 2, item.TextMarkupBody.Bounds.Width, item.TextMarkupBody.Bounds.Height);
				item.TextMarkupBody.Bounds = bounds2;
				item.TextMarkupBody.Render(markupDrawContext);
			}
		}
		item._136B = rectangle;
	}

	private Rectangle _2512(SideNavItem _06EC, ᇎ ແ)
	{
		Rectangle result = Rectangle.Empty;
		if (ແ != null || !string.IsNullOrEmpty(_06EC.SymbolRealized))
		{
			Size imageSize = _06EC.ImageSize;
			if (_06EC.ImagePosition == eImagePosition.Top || _06EC.ImagePosition == eImagePosition.Bottom)
			{
				result = new Rectangle(_06EC._0E60.X, _06EC._0E60.Y, _06EC.DisplayRectangle.Width, _06EC._0E60.Height);
			}
			else if (_06EC.ImagePosition == eImagePosition.Left)
			{
				if (_06EC.ButtonStyle == eButtonStyle.Default)
				{
					return new Rectangle(_06EC.Bounds.X + (_06EC.Bounds.Width - imageSize.Width) / 2, _06EC.Bounds.Y + (_06EC.Bounds.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
				}
				result = new Rectangle(_06EC._0E60.X + 4, _06EC._0E60.Y, _06EC._0E60.Width, _06EC._0E60.Height);
			}
			else if (_06EC.ImagePosition == eImagePosition.Right)
			{
				result = new Rectangle(_06EC._0E60.X + _06EC.ImagePaddingHorizontal + 4, _06EC._0E60.Y, _06EC._0E60.Width, _06EC._0E60.Height);
			}
			result.Offset(_06EC.DisplayRectangle.Left, _06EC.DisplayRectangle.Top);
			result.Offset((result.Width - imageSize.Width) / 2, (result.Height - imageSize.Height) / 2);
			result.Width = imageSize.Width;
			result.Height = imageSize.Height;
		}
		return result;
	}

	private Rectangle ഷ(SideNavItem _06EC, ᇎ ແ, Rectangle _2513)
	{
		Rectangle displayRectangle = _06EC.DisplayRectangle;
		Rectangle result = _06EC.TextDrawRect;
		if (_06EC.ImagePosition == eImagePosition.Top || _06EC.ImagePosition == eImagePosition.Bottom)
		{
			result = new Rectangle(1, result.Y, displayRectangle.Width - 2, result.Height);
		}
		result.Offset(displayRectangle.Left, displayRectangle.Top);
		if (_06EC.ImagePosition == eImagePosition.Left)
		{
			result.X = _2513.Right + _06EC.ImagePaddingHorizontal;
		}
		if (result.Right > displayRectangle.Right)
		{
			result.Width = displayRectangle.Right - result.Left;
		}
		return result;
	}
}
internal class _2514 : ᙓ
{
	private Office2007ColorTable __137C;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
		}
	}

	public virtual void Paint(SideNavItemRendererEventArgs e)
	{
	}
}
internal abstract class _2515
{
	public abstract void Paint(TabFormStripPainterArgs renderingInfo);
}
internal abstract class _2516
{
	public abstract void Paint(TabFormPainterArgs args);
}
internal class _2519 : _2515, ᙓ
{
	private Office2007ColorTable __137C;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
		}
	}

	public override void Paint(TabFormStripPainterArgs renderingInfo)
	{
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Invalid comparison between Unknown and I4
		TabFormStripControl tabFormStrip = renderingInfo.TabFormStrip;
		Graphics graphics = renderingInfo.Graphics;
		_ = ((Control)tabFormStrip).ClientRectangle;
		bool flag = true;
		Form val = ((Control)tabFormStrip).FindForm();
		if (val != null && ((val != Form.ActiveForm && val.MdiParent == null) || (val.MdiParent != null && val.MdiParent.ActiveMdiChild != val)))
		{
			flag = false;
		}
		TabFormControl tabFormControl = ((Control)tabFormStrip).Parent as TabFormControl;
		TabFormColorTable tabFormColorTable = ᘽ.TabForm;
		if (tabFormControl != null && tabFormControl.ColorTable != null)
		{
			tabFormColorTable = tabFormControl.ColorTable;
		}
		TabFormStateColorTable tabFormStateColorTable = (flag ? tabFormColorTable.Active : tabFormColorTable.Inactive);
		if (tabFormStateColorTable.CaptionBackColors.Length > 0)
		{
			Rectangle clientRectangle = ((Control)tabFormStrip).ClientRectangle;
			clientRectangle.Inflate(1, 1);
			Brush val2 = DisplayHelp.CreateBrush(clientRectangle, tabFormStateColorTable.CaptionBackColors, tabFormStateColorTable.CaptionBackColorsGradientAngle, tabFormStateColorTable.CaptionBackColorsPositions);
			try
			{
				graphics.FillRectangle(val2, clientRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (!tabFormStrip.CaptionVisible)
		{
			return;
		}
		if (tabFormStrip._0D65.IsEmpty || tabFormStrip._൦.IsEmpty)
		{
			_251A(tabFormStrip, renderingInfo);
		}
		Color captionText = tabFormStateColorTable.CaptionText;
		eTextFormat captionTextFormat = tabFormColorTable.CaptionTextFormat;
		Font val3 = SystemFonts.DefaultFont;
		bool flag2 = true;
		if (tabFormStrip.CaptionFont != null)
		{
			val3.Dispose();
			val3 = tabFormStrip.CaptionFont;
			flag2 = false;
		}
		string text = tabFormStrip.TitleText;
		if (string.IsNullOrEmpty(text) && val != null)
		{
			text = ((Control)val).Text;
		}
		bool flag3 = tabFormStrip._0D62 != null;
		Rectangle rectangle = tabFormStrip._0D65;
		rectangle.X += 12;
		rectangle.Width -= 12;
		if (StyleManager.Style == eStyle.OfficeMobile2014)
		{
			rectangle.Y -= 2;
			rectangle.Height -= 2;
			if (!tabFormStrip._൦.IsEmpty && rectangle.Width > tabFormStrip._൦.Width)
			{
				rectangle.X += tabFormStrip._൦.Width / 2 + 18;
				rectangle.Width -= tabFormStrip._൦.Width / 2 + 18;
			}
		}
		if (!flag3)
		{
			TextDrawing.DrawString(graphics, text, val3, captionText, rectangle, captionTextFormat);
		}
		else
		{
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, val3, captionText, (int)((Control)tabFormStrip).RightToLeft == 1, rectangle, hotKeyPrefixVisible: false);
			markupDrawContext.AllowMultiLine = false;
			markupDrawContext.IgnoreFormattingColors = !flag;
			_0CB4 obj = tabFormStrip._0D62;
			if (tabFormStrip.අ != rectangle)
			{
				obj.Measure(rectangle.Size, markupDrawContext);
				obj._0CC6(rectangle, markupDrawContext);
				tabFormStrip.අ = rectangle;
				Rectangle bounds = obj.Bounds;
				if (bounds.Width < rectangle.Width)
				{
					bounds.Offset((rectangle.Width - bounds.Width) / 2, 0);
				}
				if (bounds.Height < rectangle.Height)
				{
					bounds.Offset(0, (rectangle.Height - bounds.Height) / 2);
				}
				obj.Bounds = bounds;
			}
			Region clip = graphics.Clip;
			graphics.SetClip(rectangle, (CombineMode)1);
			obj.Render(markupDrawContext);
			graphics.Clip = clip;
			if (clip != null)
			{
				clip.Dispose();
			}
		}
		if (flag2)
		{
			val3.Dispose();
		}
	}

	private void _251A(TabFormStripControl _251B, TabFormStripPainterArgs _251C)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if (!_251B.CaptionVisible)
		{
			return;
		}
		bool flag = (int)((Control)_251B).RightToLeft == 1;
		((Control)_251B).FindForm();
		int num = 0;
		int num2 = 0;
		for (int num3 = _251B.CaptionContainerItem.SubItems.Count - 1; num3 >= 0; num3--)
		{
			BaseItem baseItem = _251B.CaptionContainerItem.SubItems[num3];
			if (baseItem.Visible && baseItem.Displayed)
			{
				if (baseItem.ItemAlignment == eItemAlignment.Near && baseItem.Visible && num3 > 0)
				{
					num = ((!flag) ? baseItem.DisplayRectangle.Right : baseItem.DisplayRectangle.X);
					break;
				}
				if (baseItem.ItemAlignment == eItemAlignment.Far && baseItem.Visible)
				{
					num2 = ((!flag) ? baseItem.DisplayRectangle.X : baseItem.DisplayRectangle.Right);
				}
			}
		}
		if (_251B.CaptionContainerItem is ᱭ && ((ᱭ)_251B.CaptionContainerItem)._0DBC != null)
		{
			num = ((!flag) ? ((ᱭ)_251B.CaptionContainerItem)._0DBC.DisplayRectangle.Right : ((ᱭ)_251B.CaptionContainerItem)._0DBC.DisplayRectangle.X);
		}
		Rectangle rectangle2 = (_251B._0D65 = new Rectangle(num, 2, _251B.CaptionContainerItem.WidthInternal - num - (_251B.CaptionContainerItem.WidthInternal - num2), _251B.ඛ()));
		if (num2 > 0)
		{
			if (flag)
			{
				_251B._൦ = new Rectangle(rectangle2.X, rectangle2.Y, num2, rectangle2.Height);
			}
			else
			{
				_251B._൦ = new Rectangle(num2, rectangle2.Y, _251B.CaptionContainerItem.WidthInternal - num2, rectangle2.Height);
			}
		}
	}
}
internal class _251D : _24C1, ᙓ
{
	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	protected virtual Office2007SliderColorTable GetColorTable(SliderItem item, Control container)
	{
		if (item.ColorTable != null)
		{
			return item.ColorTable;
		}
		if (container == null)
		{
			return ຊ.Slider;
		}
		Office2007ColorTable office2007ColorTable = ຊ;
		string contextualKey = Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), ((object)container).GetType());
		object value = null;
		if (container is Bar && office2007ColorTable.ContextualTables.TryGetValue(contextualKey + "+" + ((Bar)(object)container).BarType, out value))
		{
			return (Office2007SliderColorTable)value;
		}
		if (office2007ColorTable.ContextualTables.TryGetValue(contextualKey, out value))
		{
			return (Office2007SliderColorTable)value;
		}
		return ຊ.Slider;
	}

	public override void Paint(SliderItemRendererEventArgs e)
	{
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07aa: Expected O, but got Unknown
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Expected O, but got Unknown
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		SliderItem sliderItem = e.SliderItem;
		Office2007SliderColorTable colorTable = GetColorTable(sliderItem, e.က.ContainerControl);
		Office2007SliderStateColorTable office2007SliderStateColorTable = null;
		Office2007SliderStateColorTable office2007SliderStateColorTable2 = null;
		Office2007SliderStateColorTable office2007SliderStateColorTable3 = null;
		eDotNetBarStyle effectiveStyle = sliderItem.EffectiveStyle;
		if (sliderItem.GetEnabled())
		{
			office2007SliderStateColorTable = colorTable.Default;
			office2007SliderStateColorTable2 = colorTable.Default;
			office2007SliderStateColorTable3 = colorTable.Default;
			if (colorTable.DecreaseButtonPart != null)
			{
				office2007SliderStateColorTable = colorTable.DecreaseButtonPart.Default;
			}
			if (colorTable.IncreaseButtonPart != null)
			{
				office2007SliderStateColorTable2 = colorTable.IncreaseButtonPart.Default;
			}
			if (colorTable.TrackPart != null)
			{
				office2007SliderStateColorTable3 = colorTable.TrackPart.Default;
			}
			if (sliderItem.MouseDownPart == eSliderPart.DecreaseButton)
			{
				office2007SliderStateColorTable = ((colorTable.DecreaseButtonPart == null) ? colorTable.Pressed : colorTable.DecreaseButtonPart.Pressed);
			}
			else if (sliderItem.MouseDownPart == eSliderPart.IncreaseButton)
			{
				office2007SliderStateColorTable2 = ((colorTable.IncreaseButtonPart == null) ? colorTable.Pressed : colorTable.IncreaseButtonPart.Pressed);
			}
			else if (sliderItem.MouseDownPart == eSliderPart.TrackArea)
			{
				office2007SliderStateColorTable3 = ((colorTable.TrackPart == null) ? colorTable.Pressed : colorTable.TrackPart.Pressed);
			}
			else if (sliderItem.ᘅ == eSliderPart.DecreaseButton)
			{
				office2007SliderStateColorTable = ((colorTable.DecreaseButtonPart == null) ? colorTable.MouseOver : colorTable.DecreaseButtonPart.MouseOver);
			}
			else if (sliderItem.ᘅ == eSliderPart.IncreaseButton)
			{
				office2007SliderStateColorTable2 = ((colorTable.IncreaseButtonPart == null) ? colorTable.MouseOver : colorTable.IncreaseButtonPart.MouseOver);
			}
			else if (sliderItem.ᘅ == eSliderPart.TrackArea)
			{
				office2007SliderStateColorTable3 = ((colorTable.TrackPart == null) ? colorTable.MouseOver : colorTable.TrackPart.MouseOver);
			}
		}
		else
		{
			office2007SliderStateColorTable = colorTable.Disabled;
			office2007SliderStateColorTable2 = colorTable.Disabled;
			office2007SliderStateColorTable3 = colorTable.Disabled;
		}
		Rectangle displayRectangle = sliderItem.DisplayRectangle;
		Rectangle _24DC = sliderItem._24C2;
		Graphics graphics = e.Graphics;
		string text = _2498(sliderItem);
		if (!_24DC.IsEmpty && text.Length > 0)
		{
			_24DC.Offset(displayRectangle.Location);
			eTextFormat eTextFormat = eTextFormat.WordEllipsis;
			if (sliderItem.LabelPosition == eSliderLabelPosition.Left)
			{
				eTextFormat |= eTextFormat.VerticalCenter;
			}
			else if (sliderItem.LabelPosition == eSliderLabelPosition.Right)
			{
				eTextFormat |= eTextFormat.Right | eTextFormat.VerticalCenter;
			}
			else if (sliderItem.LabelPosition == eSliderLabelPosition.Top)
			{
				eTextFormat |= eTextFormat.HorizontalCenter | eTextFormat.WordBreak;
			}
			else if (sliderItem.LabelPosition == eSliderLabelPosition.Bottom)
			{
				eTextFormat |= eTextFormat.Bottom | eTextFormat.HorizontalCenter | eTextFormat.WordBreak;
			}
			Color color = office2007SliderStateColorTable3.LabelColor;
			if (e.က.ContainerControl is Slider)
			{
				color = office2007SliderStateColorTable3.SliderLabelColor;
			}
			if (!sliderItem.TextColor.IsEmpty)
			{
				color = sliderItem.TextColor;
			}
			if (sliderItem.TextMarkupBody == null)
			{
				if (e.က != null && e.က.GlassEnabled && sliderItem.Parent is ᱭ && !(e.က.ContainerControl is Ꮮ))
				{
					if (!e.က.ໟ)
					{
						_21F8._21FC(graphics, _2498(sliderItem), e.က.Font, _24DC, TextDrawing.GetTextFormat(eTextFormat));
					}
				}
				else if (sliderItem.SliderOrientation == eOrientation.Vertical)
				{
					graphics.RotateTransform(90f);
					TextDrawing.DrawString(graphics, _2498(sliderItem), e.က.Font, color, new Rectangle(_24DC.Top, -_24DC.Right, _24DC.Height, _24DC.Width), eTextFormat);
					graphics.ResetTransform();
				}
				else
				{
					TextDrawing.DrawString(graphics, _2498(sliderItem), e.က.Font, color, _24DC, eTextFormat);
				}
			}
			else if (sliderItem.SliderOrientation == eOrientation.Vertical)
			{
				graphics.RotateTransform(90f);
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, e.က.Font, color, e.က.RightToLeft);
				markupDrawContext.HotKeyPrefixVisible = (eTextFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
				sliderItem.TextMarkupBody.Bounds = new Rectangle(_24DC.Top, -_24DC.Right, sliderItem.TextMarkupBody.Bounds.Width, sliderItem.TextMarkupBody.Bounds.Height);
				sliderItem.TextMarkupBody.Render(markupDrawContext);
				graphics.ResetTransform();
			}
			else
			{
				MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(graphics, e.က.Font, color, e.က.RightToLeft);
				markupDrawContext2.HotKeyPrefixVisible = (eTextFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
				sliderItem.TextMarkupBody.Bounds = _24DC;
				sliderItem.TextMarkupBody.Render(markupDrawContext2);
			}
		}
		_24DC = sliderItem._24C5;
		int width = Dpi.Width1;
		if (!_24DC.IsEmpty)
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)3;
			_24DC.Offset(displayRectangle.Location);
			if (sliderItem.SliderOrientation == eOrientation.Horizontal)
			{
				int num = _24DC.Y + _24DC.Height / 2 - 1;
				int num2 = _24DC.X + _24DC.Width / 2;
				if (!office2007SliderStateColorTable3.TrackLineColor.IsEmpty)
				{
					Pen val = new Pen(office2007SliderStateColorTable3.TrackLineColor, (float)width);
					try
					{
						graphics.DrawLine(val, _24DC.X - 1, num, _24DC.Right, num);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				if (!office2007SliderStateColorTable3.TrackLineLightColor.IsEmpty)
				{
					num++;
					num2++;
					Pen val2 = new Pen(office2007SliderStateColorTable3.TrackLineLightColor, (float)width);
					try
					{
						if (sliderItem.TrackMarker)
						{
							graphics.DrawLine(val2, num2, num - 3, num2, num + 3);
						}
						graphics.DrawLine(val2, _24DC.X - 1, num, _24DC.Right, num);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				if (!office2007SliderStateColorTable3.TrackLineColor.IsEmpty && sliderItem.TrackMarker)
				{
					num--;
					num2--;
					Pen val3 = new Pen(office2007SliderStateColorTable3.TrackLineColor, (float)width);
					try
					{
						graphics.DrawLine(val3, num2, num - 3, num2, num + 3);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				graphics.SmoothingMode = smoothingMode;
				int num3 = 0;
				num3 = ((!e.က.RightToLeft) ? ((int)((float)(_24DC.Width - Dpi.Width11) * ((sliderItem.Maximum - sliderItem.Minimum == 0) ? 0f : ((float)(sliderItem.Value - sliderItem.Minimum) / (float)(sliderItem.Maximum - sliderItem.Minimum))))) : ((int)((float)(_24DC.Width - Dpi.Width11) * (1f - ((sliderItem.Maximum - sliderItem.Minimum == 0) ? 0f : ((float)(sliderItem.Value - sliderItem.Minimum) / (float)(sliderItem.Maximum - sliderItem.Minimum)))))));
				Rectangle rectangle = _24DC;
				rectangle.X += num3;
				rectangle.Width = Dpi.Width11;
				rectangle.Height = Dpi.Height15;
				rectangle.Y += (_24DC.Height - rectangle.Height) / 2;
				_251E(office2007SliderStateColorTable3, rectangle, graphics, eSliderPart.TrackArea, sliderItem.SliderOrientation, effectiveStyle);
				sliderItem._24C6 = rectangle;
			}
			else
			{
				int num4 = _24DC.X + _24DC.Width / 2 - 1;
				int num5 = _24DC.Y + _24DC.Height / 2;
				if (!office2007SliderStateColorTable3.TrackLineColor.IsEmpty)
				{
					Pen val4 = new Pen(office2007SliderStateColorTable3.TrackLineColor, (float)width);
					try
					{
						graphics.DrawLine(val4, num4, _24DC.Y - 1, num4, _24DC.Bottom);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				if (!office2007SliderStateColorTable3.TrackLineLightColor.IsEmpty)
				{
					num4++;
					num5++;
					Pen val5 = new Pen(office2007SliderStateColorTable3.TrackLineLightColor, (float)width);
					try
					{
						if (sliderItem.TrackMarker)
						{
							graphics.DrawLine(val5, num4 - 3, num5, num4 + 3, num5);
						}
						graphics.DrawLine(val5, num4, _24DC.Y - 1, num4, _24DC.Bottom);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				if (!office2007SliderStateColorTable3.TrackLineColor.IsEmpty && sliderItem.TrackMarker)
				{
					num4--;
					num5--;
					Pen val6 = new Pen(office2007SliderStateColorTable3.TrackLineColor, (float)width);
					try
					{
						graphics.DrawLine(val6, num4 - 3, num5, num4 + 3, num5);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				graphics.SmoothingMode = smoothingMode;
				int num6 = 0;
				num6 = _24DC.Height - 16 - (int)((float)(_24DC.Height - 11) * ((sliderItem.Maximum - sliderItem.Minimum == 0) ? 0f : ((float)(sliderItem.Value - sliderItem.Minimum) / (float)(sliderItem.Maximum - sliderItem.Minimum))));
				Rectangle rectangle2 = _24DC;
				rectangle2.Y += num6;
				rectangle2.Width = Dpi.Width15;
				rectangle2.Height = Dpi.Height11;
				rectangle2.X += (_24DC.Width - rectangle2.Width) / 2;
				_251E(office2007SliderStateColorTable3, rectangle2, graphics, eSliderPart.TrackArea, sliderItem.SliderOrientation, effectiveStyle);
				sliderItem._24C6 = rectangle2;
			}
		}
		_24DC = sliderItem._24C3;
		if (!_24DC.IsEmpty)
		{
			_24DC.Offset(displayRectangle.Location);
			_24DC.Width--;
			_24DC.Height--;
			_251E(office2007SliderStateColorTable, _24DC, graphics, eSliderPart.DecreaseButton, sliderItem.SliderOrientation, effectiveStyle);
		}
		_24DC = sliderItem._24C4;
		if (!_24DC.IsEmpty)
		{
			_24DC.Offset(displayRectangle.Location);
			_24DC.Width--;
			_24DC.Height--;
			_251E(office2007SliderStateColorTable2, _24DC, graphics, eSliderPart.IncreaseButton, sliderItem.SliderOrientation, effectiveStyle);
		}
		base.Paint(e);
	}

	private string _2498(SliderItem _06EC)
	{
		return _06EC.Text;
	}

	private void _251E(Office2007SliderStateColorTable ዅ, Rectangle ल, Graphics _٥, eSliderPart _070B, eOrientation _0DE3, eDotNetBarStyle _19CF)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Expected O, but got Unknown
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Expected O, but got Unknown
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_058f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Expected O, but got Unknown
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Expected O, but got Unknown
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Expected O, but got Unknown
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Expected O, but got Unknown
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Expected O, but got Unknown
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Expected O, but got Unknown
		//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
		if (ल.Width <= 0 || ल.Height <= 0)
		{
			return;
		}
		switch (_070B)
		{
		case eSliderPart.IncreaseButton:
		case eSliderPart.DecreaseButton:
		{
			ल.Inflate(-1, -1);
			SmoothingMode smoothingMode3 = _٥.SmoothingMode;
			_٥.SmoothingMode = (SmoothingMode)4;
			if (ዅ.PartBackground.Colors.Count > 0)
			{
				Brush val10 = DisplayHelp.CreateBrush(ल, ዅ.PartBackground);
				try
				{
					_٥.FillEllipse(val10, ल);
				}
				finally
				{
					((IDisposable)val10)?.Dispose();
				}
			}
			if (!ዅ.PartBorderColor.IsEmpty)
			{
				Pen val11 = new Pen(ዅ.PartBorderColor, 1f);
				try
				{
					_٥.DrawEllipse(val11, ल);
				}
				finally
				{
					((IDisposable)val11)?.Dispose();
				}
			}
			ल.Inflate(1, 1);
			if (!ዅ.PartBorderLightColor.IsEmpty)
			{
				Pen val12 = new Pen(ዅ.PartBorderLightColor, 1f);
				try
				{
					_٥.DrawEllipse(val12, ल);
				}
				finally
				{
					((IDisposable)val12)?.Dispose();
				}
			}
			_٥.SmoothingMode = (SmoothingMode)3;
			switch (_070B)
			{
			case eSliderPart.DecreaseButton:
			{
				Size size2 = new Size(Dpi.Width9, Dpi.Height3);
				Rectangle rectangle3 = new Rectangle(ल.X + (ल.Width - size2.Width) / 2, ल.Y + (ल.Height - size2.Height) / 2, size2.Width, size2.Height);
				if (!ዅ.PartForeLightColor.IsEmpty)
				{
					Pen val15 = new Pen(ዅ.PartForeLightColor, 1f);
					try
					{
						_٥.DrawRectangle(val15, rectangle3);
					}
					finally
					{
						((IDisposable)val15)?.Dispose();
					}
				}
				if (!ዅ.PartForeColor.IsEmpty)
				{
					rectangle3.Offset(1, 1);
					rectangle3.Width--;
					rectangle3.Height--;
					SolidBrush val16 = new SolidBrush(ዅ.PartForeColor);
					try
					{
						_٥.FillRectangle((Brush)(object)val16, rectangle3);
					}
					finally
					{
						((IDisposable)val16)?.Dispose();
					}
				}
				break;
			}
			case eSliderPart.IncreaseButton:
			{
				Size size = new Size(Dpi.Width8, Dpi.Height8);
				Rectangle rectangle2 = new Rectangle(ल.X + (ल.Width - size.Width) / 2, ल.Y + (ल.Height - size.Height) / 2, size.Width, size.Height);
				if (!ዅ.TrackLineLightColor.IsEmpty)
				{
					SolidBrush val13 = new SolidBrush(ዅ.PartForeLightColor);
					try
					{
						_٥.FillRectangle((Brush)(object)val13, new Rectangle(rectangle2.X, rectangle2.Y + Dpi.Height3, rectangle2.Width + Dpi.Width2, Dpi.Height4));
						_٥.FillRectangle((Brush)(object)val13, new Rectangle(rectangle2.X + Dpi.Width3, rectangle2.Y, Dpi.Width4, Dpi.Height3));
						_٥.FillRectangle((Brush)(object)val13, new Rectangle(rectangle2.X + Dpi.Width3, rectangle2.Bottom - Dpi.Height1, Dpi.Width4, Dpi.Height3));
					}
					finally
					{
						((IDisposable)val13)?.Dispose();
					}
				}
				if (!ዅ.TrackLineColor.IsEmpty)
				{
					SolidBrush val14 = new SolidBrush(ዅ.PartForeColor);
					try
					{
						_٥.FillRectangle((Brush)(object)val14, new Rectangle(rectangle2.X + Dpi.Width1, rectangle2.Y + Dpi.Width4, rectangle2.Width, Dpi.Width2));
						_٥.FillRectangle((Brush)(object)val14, new Rectangle(rectangle2.X + Dpi.Width4, rectangle2.Y + Dpi.Height1, Dpi.Width2, Dpi.Height4));
						_٥.FillRectangle((Brush)(object)val14, new Rectangle(rectangle2.X + Dpi.Width4, rectangle2.Bottom - Dpi.Width2, Dpi.Width2, Dpi.Height3));
					}
					finally
					{
						((IDisposable)val14)?.Dispose();
					}
				}
				break;
			}
			}
			_٥.SmoothingMode = smoothingMode3;
			break;
		}
		case eSliderPart.TrackArea:
			if (_0DE3 == eOrientation.Vertical)
			{
				Matrix val = new Matrix();
				val.RotateAt(-90f, new PointF(ल.X, ल.Bottom));
				val.Translate((float)ल.Height, (float)(ल.Width - ल.Height), (MatrixOrder)1);
				_٥.Transform = val;
			}
			if (StyleManager.IsMetro(_19CF))
			{
				if (!ዅ.PartBackground.IsEmpty)
				{
					SmoothingMode smoothingMode = _٥.SmoothingMode;
					_٥.SmoothingMode = (SmoothingMode)0;
					Rectangle rectangle = new Rectangle(ल.X + (ल.Width - Dpi.Width3) / 2, ल.Y + Dpi.Width2, Dpi.Width3, ल.Height - Dpi.Width4);
					Brush val2 = DisplayHelp.CreateBrush(rectangle, ዅ.PartBackground);
					try
					{
						_٥.FillRectangle(val2, rectangle);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					_٥.SmoothingMode = smoothingMode;
				}
			}
			else
			{
				GraphicsPath val3 = new GraphicsPath();
				try
				{
					int width = Dpi.Width11;
					val3.AddLine(ल.X, ल.Y, ल.X + width, ल.Y);
					val3.AddLine(ल.X + width, ल.Y, ल.X + width, ल.Y + Dpi.Height9);
					val3.AddLine(ल.X + width, ल.Y + Dpi.Height9, ल.X + Dpi.Width6, ल.Y + Dpi.Height15);
					val3.AddLine(ल.X + Dpi.Width5, ल.Y + Dpi.Height15, ल.X, ल.Y + Dpi.Width10);
					val3.CloseAllFigures();
					SolidBrush val4 = new SolidBrush(ዅ.PartBorderLightColor);
					try
					{
						_٥.FillPath((Brush)(object)val4, val3);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				SmoothingMode smoothingMode2 = _٥.SmoothingMode;
				_٥.SmoothingMode = (SmoothingMode)4;
				ल.Offset(1, 1);
				GraphicsPath val5 = new GraphicsPath();
				try
				{
					int width2 = Dpi.Width8;
					val5.AddLine(ल.X, ल.Y, ल.X + width2, ल.Y);
					val5.AddLine(ल.X + width2, ल.Y + width2, ल.X + width2 / 2, ल.Y + Dpi.Height12);
					val5.AddLine(ल.X, ल.Y + width2, ल.X, ल.Y);
					val5.CloseAllFigures();
					if (ዅ.PartBackground.Colors.Count > 0)
					{
						Brush val6 = DisplayHelp.CreateBrush(Rectangle.Ceiling(val5.GetBounds()), ዅ.PartBackground);
						try
						{
							_٥.FillPath(val6, val5);
						}
						finally
						{
							((IDisposable)val6)?.Dispose();
						}
					}
					Pen val7 = new Pen(ዅ.PartBorderColor, 1f);
					try
					{
						_٥.DrawPath(val7, val5);
					}
					finally
					{
						((IDisposable)val7)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				Pen val8 = new Pen(Color.FromArgb(200, ዅ.PartForeColor), 1f);
				try
				{
					_٥.DrawLine(val8, ल.X + Dpi.Width4, ल.Y + Dpi.Height3, ल.X + Dpi.Width4, ल.Y + Dpi.Height8);
				}
				finally
				{
					((IDisposable)val8)?.Dispose();
				}
				Pen val9 = new Pen(ዅ.PartForeLightColor, 1f);
				try
				{
					_٥.DrawLine(val9, ल.X + Dpi.Width5, ल.Y + Dpi.Height4, ल.X + Dpi.Width5, ल.Y + Dpi.Height7);
				}
				finally
				{
					((IDisposable)val9)?.Dispose();
				}
				_٥.SmoothingMode = smoothingMode2;
			}
			if (_0DE3 == eOrientation.Vertical)
			{
				_٥.ResetTransform();
			}
			break;
		}
	}
}
internal class _2525 : _24DE, ᙓ
{
	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void PaintSideBar(SideBarRendererEventArgs e)
	{
		Graphics graphics = e.Graphics;
		SideBar sideBar = e.SideBar;
		Office2007SideBarColorTable sideBar2 = ຊ.SideBar;
		LinearGradientColorTable background = sideBar2.Background;
		Rectangle r = new Rectangle(Point.Empty, ((Control)sideBar).Size);
		DisplayHelp.FillRectangle(graphics, r, background);
		Color border = sideBar2.Border;
		DisplayHelp.DrawRectangle(graphics, border, r);
		base.PaintSideBar(e);
	}

	public override void PaintSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
	{
		Graphics graphics = e.Graphics;
		SideBarPanelItem sideBarPanelItem = e.SideBarPanelItem;
		Office2007SideBarColorTable sideBar = ຊ.SideBar;
		GradientColorTable gradientColorTable = (sideBarPanelItem.Expanded ? sideBar.SideBarPanelItemExpanded : sideBar.SideBarPanelItemDefault);
		if (sideBarPanelItem.IsMouseDown)
		{
			gradientColorTable = sideBar.SideBarPanelItemPressed;
		}
		else if (sideBarPanelItem.IsMouseOver)
		{
			gradientColorTable = sideBar.SideBarPanelItemMouseOver;
		}
		if (!sideBarPanelItem.BackgroundStyle.BackColor1.IsEmpty || sideBarPanelItem.BackgroundStyle.BackgroundImage != null)
		{
			sideBarPanelItem.BackgroundStyle.Paint(graphics, sideBarPanelItem.DisplayRectangle);
		}
		Rectangle panelRect = sideBarPanelItem.PanelRect;
		Rectangle displayRectangle = sideBarPanelItem.DisplayRectangle;
		panelRect.Offset(displayRectangle.Location);
		if (gradientColorTable != null)
		{
			Brush val = DisplayHelp.CreateBrush(panelRect, gradientColorTable);
			try
			{
				graphics.FillRectangle(val, panelRect);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (sideBarPanelItem.Expanded)
		{
			DisplayHelp.DrawLine(graphics, panelRect.X, panelRect.Bottom, panelRect.Right, panelRect.Bottom, sideBar.Border, 1);
		}
		DisplayHelp.DrawLine(graphics, displayRectangle.X, displayRectangle.Bottom, displayRectangle.Right, displayRectangle.Bottom, sideBar.Border, 1);
		Font font = sideBarPanelItem._໘();
		ᇎ ᇎ = sideBarPanelItem._0EC5();
		if (ᇎ != null)
		{
			panelRect.X += 2;
			panelRect.Width -= 2;
			ᇎ.ᇗ(graphics, new Rectangle(panelRect.X, panelRect.Y + (panelRect.Height - ᇎ._0711) / 2, ᇎ.ܐ, ᇎ._0711));
			panelRect.X += ᇎ.ܐ + 4;
			panelRect.Width -= ᇎ.ܐ + 8;
		}
		Rectangle rectangle = Rectangle.Empty;
		if (sideBarPanelItem.Text.Length > 0)
		{
			Color color = sideBar.SideBarPanelItemText;
			if (!sideBarPanelItem.ForeColor.IsEmpty)
			{
				color = sideBarPanelItem.ForeColor;
			}
			if (sideBarPanelItem.IsMouseOver && !sideBarPanelItem.HotForeColor.IsEmpty)
			{
				color = sideBarPanelItem.HotForeColor;
			}
			TextDrawing.DrawString(graphics, sideBarPanelItem.Text, font, color, panelRect, sideBarPanelItem.ಲ());
			rectangle = panelRect;
		}
		if (sideBarPanelItem.Focused)
		{
			if (sideBarPanelItem.DesignMode)
			{
				Rectangle panelRect2 = sideBarPanelItem.PanelRect;
				panelRect2.Offset(sideBarPanelItem.DisplayRectangle.Location);
				panelRect2.Inflate(-2, -2);
				_1A6B._1A6C(graphics, panelRect2, e.က.Colors.ItemDesignTimeBorder);
			}
			else if (!rectangle.IsEmpty)
			{
				ControlPaint.DrawFocusRectangle(graphics, rectangle);
			}
		}
		base.PaintSideBarPanelItem(e);
	}
}
internal class _2526 : _24E7, ᙓ
{
	private Office2007ColorTable __137C;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
		}
	}

	public override void Paint(RangeSliderItemRendererEventArgs e)
	{
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Expected O, but got Unknown
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Expected O, but got Unknown
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Expected O, but got Unknown
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Expected O, but got Unknown
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Expected O, but got Unknown
		RangeSliderItem sliderItem = e.SliderItem;
		ItemPaintArgs က = e.က;
		Graphics graphics = e.Graphics;
		Office2010RangeSliderColorTable colorTable = GetColorTable(sliderItem, က.ContainerControl);
		eOrientation sliderOrientation = sliderItem.SliderOrientation;
		RangeValue rangeValue = sliderItem._2508();
		int min = rangeValue.Min;
		int max = rangeValue.Max;
		sliderItem._2507();
		if (colorTable == null)
		{
			graphics.FillRectangle(Brushes.Gray, sliderItem.Bounds);
			return;
		}
		if (sliderItem.RangeLineHeight > 0)
		{
			Rectangle rectangle = ((sliderItem.SliderOrientation != 0) ? new Rectangle(sliderItem.RangeButtonMinBounds.X + (sliderItem.RangeButtonMinBounds.Width - sliderItem.RangeLineHeight) / 2, sliderItem.TicksBounds.Y, sliderItem.RangeLineHeight, sliderItem.TicksBounds.Height) : new Rectangle(sliderItem.TicksBounds.X, sliderItem.RangeButtonMinBounds.Y + (sliderItem.RangeButtonMinBounds.Height - sliderItem.RangeLineHeight) / 2 + 1, sliderItem.TicksBounds.Width, sliderItem.RangeLineHeight));
			if (colorTable.RangeBackground != null)
			{
				colorTable.RangeBackground.LinearGradientAngle = ((sliderItem.SliderOrientation == eOrientation.Horizontal) ? 90 : 180);
				Rectangle bounds = rectangle;
				bounds.Offset(sliderItem.Bounds.Location);
				bounds.Height--;
				Brush val = DisplayHelp.CreateBrush(bounds, colorTable.RangeBackground);
				try
				{
					DisplayHelp.FillRoundedRectangle(graphics, val, bounds, colorTable.RangeCornerRadius);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				bounds.Height++;
				if (!colorTable.RangeLineColor.IsEmpty)
				{
					DisplayHelp.DrawRoundedRectangle(graphics, colorTable.RangeLineColor, bounds, colorTable.RangeCornerRadius);
				}
			}
			if (colorTable.RangeValueBackground != null)
			{
				if (sliderItem.SliderOrientation == eOrientation.Horizontal)
				{
					rectangle.X = sliderItem.RangeButtonMinBounds.Right;
					rectangle.Width = sliderItem.RangeButtonMaxBounds.X - sliderItem.RangeButtonMinBounds.Right;
				}
				else
				{
					rectangle.Y = sliderItem.RangeButtonMinBounds.Bottom;
					rectangle.Height = sliderItem.RangeButtonMaxBounds.Y - sliderItem.RangeButtonMinBounds.Bottom;
				}
				Rectangle rectangle2 = rectangle;
				rectangle2.Offset(sliderItem.Bounds.Location);
				colorTable.RangeValueBackground.LinearGradientAngle = ((sliderItem.SliderOrientation == eOrientation.Horizontal) ? 90 : 180);
				rectangle2.Height--;
				if (sliderItem.RangeValueColor.IsEmpty)
				{
					Brush val2 = DisplayHelp.CreateBrush(rectangle2, colorTable.RangeValueBackground);
					try
					{
						graphics.FillRectangle(val2, rectangle2);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				else
				{
					SolidBrush val3 = new SolidBrush(sliderItem.RangeValueColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val3, rectangle2);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				rectangle2.Height++;
				if (!colorTable.RangeValueLineColor.IsEmpty)
				{
					Pen val4 = new Pen(colorTable.RangeValueLineColor);
					try
					{
						if (sliderItem.SliderOrientation == eOrientation.Horizontal)
						{
							graphics.DrawLine(val4, rectangle2.X, rectangle2.Y, rectangle2.Right, rectangle2.Y);
							graphics.DrawLine(val4, rectangle2.X, rectangle2.Bottom - 1, rectangle2.Right, rectangle2.Bottom - 1);
						}
						else
						{
							graphics.DrawLine(val4, rectangle2.X, rectangle2.Y, rectangle2.X, rectangle2.Bottom);
							graphics.DrawLine(val4, rectangle2.Right - 1, rectangle2.Y, rectangle2.Right - 1, rectangle2.Bottom);
						}
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
			}
		}
		if (sliderItem.TicksVisible && !colorTable.TickLineColor.IsEmpty)
		{
			Rectangle ticksBounds = sliderItem.TicksBounds;
			ticksBounds.Offset(sliderItem.Bounds.Location);
			int ticksStep = sliderItem.TicksStep;
			if (sliderOrientation == eOrientation.Horizontal)
			{
				_2527(graphics, colorTable, min, max, ticksBounds, ticksStep);
				if (!sliderItem.TicksBounds2.IsEmpty)
				{
					ticksBounds = sliderItem.TicksBounds2;
					ticksBounds.Offset(sliderItem.Bounds.Location);
					_2527(graphics, colorTable, min, max, ticksBounds, ticksStep);
				}
			}
			else
			{
				_252A(graphics, colorTable, min, max, ticksBounds, ticksStep);
				if (!sliderItem.TicksBounds2.IsEmpty)
				{
					ticksBounds = sliderItem.TicksBounds2;
					ticksBounds.Offset(sliderItem.Bounds.Location);
					_252A(graphics, colorTable, min, max, ticksBounds, ticksStep);
				}
			}
		}
		graphics.ResetClip();
		if (sliderItem.MinRangeSliderImage != null)
		{
			Rectangle rangeButtonMinBounds = sliderItem.RangeButtonMinBounds;
			rangeButtonMinBounds.Offset(sliderItem.Bounds.Location);
			graphics.DrawImage(sliderItem.MinRangeSliderImage, rangeButtonMinBounds);
		}
		else
		{
			Office2010RangeChangePartColorTable office2010RangeChangePartColorTable = colorTable.MinRangeSlider.Default;
			if (!sliderItem.GetEnabled() && colorTable.MinRangeSlider.Disabled != null)
			{
				office2010RangeChangePartColorTable = colorTable.MinRangeSlider.Disabled;
			}
			else if (sliderItem.MouseDownPart == eRangeSliderPart.MinRangeSlider && colorTable.MinRangeSlider.Pressed != null)
			{
				office2010RangeChangePartColorTable = colorTable.MinRangeSlider.Pressed;
			}
			else if (sliderItem.MouseOverPart == eRangeSliderPart.MinRangeSlider && colorTable.MinRangeSlider.MouseOver != null)
			{
				office2010RangeChangePartColorTable = colorTable.MinRangeSlider.MouseOver;
			}
			if (office2010RangeChangePartColorTable.Background != null)
			{
				office2010RangeChangePartColorTable.Background.LinearGradientAngle = ((sliderItem.SliderOrientation == eOrientation.Horizontal) ? 90 : 0);
			}
			Rectangle rangeButtonMinBounds2 = sliderItem.RangeButtonMinBounds;
			rangeButtonMinBounds2.Offset(sliderItem.Bounds.Location);
			GraphicsPath val5 = _252B(rangeButtonMinBounds2, sliderItem.TicksPosition, sliderItem.SliderOrientation);
			try
			{
				Brush val6 = DisplayHelp.CreateBrush(rangeButtonMinBounds2, office2010RangeChangePartColorTable.Background);
				try
				{
					graphics.FillPath(val6, val5);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
				if (!office2010RangeChangePartColorTable.BorderLightColor.IsEmpty)
				{
					rangeButtonMinBounds2.Inflate(-1, -1);
					GraphicsPath val7 = _252B(rangeButtonMinBounds2, sliderItem.TicksPosition, sliderItem.SliderOrientation);
					try
					{
						Pen val8 = new Pen(office2010RangeChangePartColorTable.BorderLightColor, 1f);
						try
						{
							graphics.DrawPath(val8, val5);
						}
						finally
						{
							((IDisposable)val8)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val7)?.Dispose();
					}
				}
				if (!office2010RangeChangePartColorTable.BorderColor.IsEmpty)
				{
					Pen val9 = new Pen(office2010RangeChangePartColorTable.BorderColor, 1f);
					try
					{
						graphics.DrawPath(val9, val5);
					}
					finally
					{
						((IDisposable)val9)?.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		if (sliderItem.MaxRangeSliderImage != null)
		{
			Rectangle rangeButtonMaxBounds = sliderItem.RangeButtonMaxBounds;
			rangeButtonMaxBounds.Offset(sliderItem.Bounds.Location);
			graphics.DrawImage(sliderItem.MaxRangeSliderImage, rangeButtonMaxBounds);
			return;
		}
		Office2010RangeChangePartColorTable office2010RangeChangePartColorTable2 = colorTable.MaxRangeSlider.Default;
		if (!sliderItem.GetEnabled() && colorTable.MaxRangeSlider.Disabled != null)
		{
			office2010RangeChangePartColorTable2 = colorTable.MaxRangeSlider.Disabled;
		}
		else if (sliderItem.MouseDownPart == eRangeSliderPart.MaxRangeSlider && colorTable.MaxRangeSlider.Pressed != null)
		{
			office2010RangeChangePartColorTable2 = colorTable.MaxRangeSlider.Pressed;
		}
		else if (sliderItem.MouseOverPart == eRangeSliderPart.MaxRangeSlider && colorTable.MaxRangeSlider.MouseOver != null)
		{
			office2010RangeChangePartColorTable2 = colorTable.MaxRangeSlider.MouseOver;
		}
		if (office2010RangeChangePartColorTable2.Background != null)
		{
			office2010RangeChangePartColorTable2.Background.LinearGradientAngle = ((sliderItem.SliderOrientation == eOrientation.Horizontal) ? 90 : 0);
		}
		Rectangle rangeButtonMaxBounds2 = sliderItem.RangeButtonMaxBounds;
		rangeButtonMaxBounds2.Offset(sliderItem.Bounds.Location);
		GraphicsPath val10 = _252D(rangeButtonMaxBounds2, sliderItem.TicksPosition, sliderItem.SliderOrientation);
		try
		{
			Brush val11 = DisplayHelp.CreateBrush(rangeButtonMaxBounds2, office2010RangeChangePartColorTable2.Background);
			try
			{
				graphics.FillPath(val11, val10);
			}
			finally
			{
				((IDisposable)val11)?.Dispose();
			}
			if (!office2010RangeChangePartColorTable2.BorderLightColor.IsEmpty)
			{
				rangeButtonMaxBounds2.Inflate(-1, -1);
				GraphicsPath val12 = _252D(rangeButtonMaxBounds2, sliderItem.TicksPosition, sliderItem.SliderOrientation);
				try
				{
					Pen val13 = new Pen(office2010RangeChangePartColorTable2.BorderLightColor, 1f);
					try
					{
						graphics.DrawPath(val13, val10);
					}
					finally
					{
						((IDisposable)val13)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val12)?.Dispose();
				}
			}
			if (!office2010RangeChangePartColorTable2.BorderColor.IsEmpty)
			{
				Pen val14 = new Pen(office2010RangeChangePartColorTable2.BorderColor, 1f);
				try
				{
					graphics.DrawPath(val14, val10);
					return;
				}
				finally
				{
					((IDisposable)val14)?.Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)val10)?.Dispose();
		}
	}

	private static void _2527(Graphics _٥, Office2010RangeSliderColorTable ቯ, int _2504, int _2505, Rectangle _2528, int _2529)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = _٥.SmoothingMode;
		_٥.SmoothingMode = (SmoothingMode)3;
		if (_2528.Width / 2 < Math.Abs(_2505 - _2504) / _2529)
		{
			_2529 = (int)Math.Max(2.0, Math.Ceiling((float)Math.Abs(_2505 - _2504) / ((float)_2528.Width / 2f)));
		}
		int num = (int)RangeSliderItem._2502(_2528.Width, _2504, _2505, _2529);
		int num2 = _2528.X;
		Pen val = new Pen(ቯ.TickLineColor);
		try
		{
			for (int i = _2504; i <= _2505; i += _2529)
			{
				_٥.DrawLine(val, num2, _2528.Y, num2, _2528.Bottom - 1);
				num2 += num;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		_٥.SmoothingMode = smoothingMode;
	}

	private static void _252A(Graphics _٥, Office2010RangeSliderColorTable ቯ, int _2504, int _2505, Rectangle _2528, int _2529)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = _٥.SmoothingMode;
		_٥.SmoothingMode = (SmoothingMode)3;
		if (_2528.Height / 2 < Math.Abs(_2505 - _2504) / _2529)
		{
			_2529 = Math.Max(2, Math.Abs(_2505 - _2504) / (_2528.Height / 2));
		}
		int num = _2528.Y;
		int num2 = (int)RangeSliderItem._2502(_2528.Height, _2504, _2505, _2529);
		Pen val = new Pen(ቯ.TickLineColor);
		try
		{
			for (int i = _2504; i <= _2505; i += _2529)
			{
				_٥.DrawLine(val, _2528.X, num, _2528.Right - 1, num);
				num += num2;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		_٥.SmoothingMode = smoothingMode;
	}

	private GraphicsPath _252B(Rectangle ल, eTicksPosition _೫, eOrientation _252C)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		if (_252C == eOrientation.Horizontal)
		{
			switch (_೫)
			{
			case eTicksPosition.Top:
				val.AddLine(ल.Right, ल.Y, ल.Right, ल.Bottom);
				val.AddLine(ल.Right, ल.Bottom, ल.X, ल.Bottom);
				val.AddLine(ल.X, ल.Bottom, ल.X, ल.Y + ल.Width);
				val.CloseAllFigures();
				break;
			case eTicksPosition.Bottom:
				val.AddLine(ल.Right, ल.Bottom, ल.Right, ल.Y);
				val.AddLine(ल.Right, ल.Y, ल.X, ल.Y);
				val.AddLine(ल.X, ल.Y, ल.X, ल.Bottom - ल.Width);
				val.CloseAllFigures();
				break;
			default:
				val.AddRectangle(ल);
				break;
			}
		}
		else
		{
			switch (_೫)
			{
			case eTicksPosition.Top:
				val.AddLine(ल.X, ल.Bottom, ल.Right, ल.Bottom);
				val.AddLine(ल.Right, ल.Bottom, ल.Right, ल.Y);
				val.AddLine(ल.Right, ल.Y, ल.X + ल.Height, ल.Y);
				val.CloseAllFigures();
				break;
			case eTicksPosition.Bottom:
				val.AddLine(ल.Right, ल.Bottom, ल.X, ल.Bottom);
				val.AddLine(ल.X, ल.Bottom, ल.X, ल.Y);
				val.AddLine(ल.X, ल.Y, ल.Right - ल.Height, ल.Y);
				val.CloseAllFigures();
				break;
			default:
				val.AddRectangle(ल);
				break;
			}
		}
		return val;
	}

	private GraphicsPath _252D(Rectangle ल, eTicksPosition _೫, eOrientation _252C)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		if (_252C == eOrientation.Horizontal)
		{
			switch (_೫)
			{
			case eTicksPosition.Top:
				val.AddLine(ल.X, ल.Y, ल.X, ल.Bottom);
				val.AddLine(ल.X, ल.Bottom, ल.Right, ल.Bottom);
				val.AddLine(ल.Right, ल.Bottom, ल.Right, ल.Y + ल.Width);
				val.CloseAllFigures();
				break;
			case eTicksPosition.Bottom:
				val.AddLine(ल.X, ल.Bottom, ल.X, ल.Y);
				val.AddLine(ल.X, ल.Y, ल.Right, ल.Y);
				val.AddLine(ल.Right, ल.Y, ल.Right, ल.Bottom - ल.Width);
				val.CloseAllFigures();
				break;
			default:
				val.AddRectangle(ल);
				break;
			}
		}
		else
		{
			switch (_೫)
			{
			case eTicksPosition.Top:
				val.AddLine(ल.X, ल.Y, ल.Right, ल.Y);
				val.AddLine(ल.Right, ल.Y, ल.Right, ल.Bottom);
				val.AddLine(ल.Right, ल.Bottom, ल.X + ल.Height, ल.Bottom);
				val.CloseAllFigures();
				break;
			case eTicksPosition.Bottom:
				val.AddLine(ल.Right, ल.Y, ल.X, ल.Y);
				val.AddLine(ल.X, ल.Y, ल.X, ल.Bottom);
				val.AddLine(ल.X, ल.Bottom, ल.Right - ल.Height, ल.Bottom);
				val.CloseAllFigures();
				break;
			default:
				val.AddRectangle(ल);
				break;
			}
		}
		return val;
	}

	protected virtual Office2010RangeSliderColorTable GetColorTable(RangeSliderItem item, Control container)
	{
		if (container == null)
		{
			return __137C.RangeSlider;
		}
		Office2007ColorTable office2007ColorTable = __137C;
		string contextualKey = Office2007ColorTable.GetContextualKey(typeof(Office2010RangeSliderColorTable), ((object)container).GetType());
		object value = null;
		if (container is Bar && office2007ColorTable.ContextualTables.TryGetValue(contextualKey + "+" + ((Bar)(object)container).BarType, out value))
		{
			return (Office2010RangeSliderColorTable)value;
		}
		if (office2007ColorTable.ContextualTables.TryGetValue(contextualKey, out value))
		{
			return (Office2010RangeSliderColorTable)value;
		}
		return __137C.RangeSlider;
	}
}
internal class _252E : _2516, ᙓ
{
	private Office2007ColorTable __137C;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
		}
	}

	public override void Paint(TabFormPainterArgs args)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		TabParentForm tabParentForm = args.TabParentForm;
		Graphics graphics = args.Graphics;
		TabFormColorTable tabFormColorTable = Ꮡ();
		if (tabParentForm.FormTabsControl != null && tabParentForm.FormTabsControl.ColorTable != null)
		{
			tabFormColorTable = tabParentForm.FormTabsControl.ColorTable;
		}
		Thickness borderThickness = tabParentForm.BorderThickness;
		BorderColors ዾ = tabParentForm.BorderColor;
		if (borderThickness.IsZero && ዾ.IsEmpty)
		{
			borderThickness = tabFormColorTable.BorderThickness;
			ዾ = (tabParentForm.IsActive ? tabFormColorTable.Active.BorderColors : tabFormColorTable.Inactive.BorderColors);
		}
		SolidBrush val = new SolidBrush(tabFormColorTable.BackColor.IsEmpty ? ((Control)tabParentForm).BackColor : tabFormColorTable.BackColor);
		try
		{
			graphics.FillRectangle((Brush)(object)val, new Rectangle(0, 0, ((Control)tabParentForm).Width, ((Control)tabParentForm).Height));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (!borderThickness.IsZero && !ዾ.IsEmpty)
		{
			RectangleF ശ = new RectangleF(0f, 0f, ((Control)tabParentForm).Width, ((Control)tabParentForm).Height);
			_252F._२(graphics, ശ, borderThickness, ዾ);
		}
	}

	private TabFormColorTable Ꮡ()
	{
		return __137C.TabForm;
	}
}
internal static class _26E2
{
	internal static void _26E3(Graphics _٥, Rectangle _26E4, Color _26E5, Color _065B, bool _20B4)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Expected O, but got Unknown
		int num = 1;
		Point point = new Point(_26E4.Right, _26E4.Bottom);
		if (_20B4)
		{
			num = -1;
			point = new Point(0, _26E4.Bottom - Dpi.Width2);
		}
		Pen val = new Pen(_26E5, (float)Dpi.Width1);
		try
		{
			_٥.DrawLine(val, point.X - Dpi.Width2 * num, point.Y - Dpi.Height2, point.X - Dpi.Width3 * num, point.Y - Dpi.Height2);
			_٥.DrawLine(val, point.X - Dpi.Width6 * num, point.Y - Dpi.Height2, point.X - Dpi.Width7 * num, point.Y - Dpi.Height2);
			_٥.DrawLine(val, point.X - Dpi.Width10 * num, point.Y - Dpi.Height2, point.X - Dpi.Width11 * num, point.Y - Dpi.Height2);
			_٥.DrawLine(val, point.X - Dpi.Width2 * num, point.Y - Dpi.Height2, point.X - Dpi.Width2 * num, point.Y - Dpi.Height3);
			_٥.DrawLine(val, point.X - Dpi.Width6 * num, point.Y - Dpi.Height2, point.X - Dpi.Width6 * num, point.Y - Dpi.Height3);
			_٥.DrawLine(val, point.X - Dpi.Width10 * num, point.Y - Dpi.Height2, point.X - Dpi.Width10 * num, point.Y - Dpi.Height3);
			_٥.DrawLine(val, point.X - Dpi.Width2 * num, point.Y - Dpi.Height6, point.X - Dpi.Width3 * num, point.Y - Dpi.Height6);
			_٥.DrawLine(val, point.X - Dpi.Width6 * num, point.Y - Dpi.Height6, point.X - Dpi.Width7 * num, point.Y - Dpi.Height6);
			_٥.DrawLine(val, point.X - Dpi.Width2 * num, point.Y - Dpi.Height6, point.X - Dpi.Width2 * num, point.Y - Dpi.Height7);
			_٥.DrawLine(val, point.X - Dpi.Width6 * num, point.Y - Dpi.Height6, point.X - Dpi.Width6 * num, point.Y - Dpi.Height7);
			_٥.DrawLine(val, point.X - Dpi.Width2 * num, point.Y - Dpi.Height10, point.X - Dpi.Width3 * num, point.Y - Dpi.Height10);
			_٥.DrawLine(val, point.X - Dpi.Width2 * num, point.Y - Dpi.Height10, point.X - Dpi.Width2 * num, point.Y - Dpi.Height11);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(_065B, (float)Dpi.Width1);
		try
		{
			_٥.DrawRectangle(val2, point.X - Dpi.Width4 * num, point.Y - Dpi.Height4, Dpi.Width1, Dpi.Width1);
			_٥.DrawRectangle(val2, point.X - Dpi.Width8 * num, point.Y - Dpi.Height4, Dpi.Width1, Dpi.Width1);
			_٥.DrawRectangle(val2, point.X - Dpi.Width12 * num, point.Y - Dpi.Height4, Dpi.Width1, Dpi.Width1);
			_٥.DrawRectangle(val2, point.X - Dpi.Width4 * num, point.Y - Dpi.Height8, Dpi.Width1, Dpi.Width1);
			_٥.DrawRectangle(val2, point.X - Dpi.Width8 * num, point.Y - Dpi.Height8, Dpi.Width1, Dpi.Width1);
			_٥.DrawRectangle(val2, point.X - Dpi.Width4 * num, point.Y - Dpi.Height12, Dpi.Width1, Dpi.Width1);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
