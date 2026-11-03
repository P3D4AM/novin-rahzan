using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
[ComVisible(false)]
public class AutoHidePanel : Control
{
	private class ᆹ : IDisposable
	{
		public Rectangle ᆽ = new Rectangle(0, 0, 0, 0);

		public DockContainerItem _0CCA;

		public Size ᆾ = Size.Empty;

		public Size _0E5A = Size.Empty;

		private string ө = "";

		private ᇀ ӭ;

		private bool _ᆿ;

		public string ᆺ
		{
			get
			{
				if (_0CCA == null)
				{
					return ө;
				}
				return _0CCA.Text;
			}
		}

		public ᇎ ᆻ
		{
			get
			{
				if (_0CCA == null)
				{
					return null;
				}
				if (_0CCA.Image != null)
				{
					return new ᇎ(_0CCA.Image, dispose: false);
				}
				if (_0CCA.ImageIndex >= 0 && _0CCA.ImageList != null)
				{
					return new ᇎ(_0CCA.ImageList.Images[_0CCA.ImageIndex], dispose: false);
				}
				if (_0CCA.Icon != null)
				{
					return new ᇎ(_0CCA.Icon, dispose: false);
				}
				return null;
			}
		}

		public bool ᆼ
		{
			get
			{
				return _ᆿ;
			}
			set
			{
				_ᆿ = value;
			}
		}

		public ᆹ(ᇀ parent, DockContainerItem item)
		{
			_0CCA = item;
			ӭ = parent;
			item.TextChanged += _09D0;
			ᇎ ᇎ2 = ᆻ;
			if (ᇎ2 != null)
			{
				_0E5A = ᇎ2.ᇐ;
			}
		}

		public ᆹ(ᇀ parent, string text)
		{
			ө = text;
			ӭ = parent;
		}

		private void _09D0(object _٠, EventArgs ؿ)
		{
			object containerControl = ((BaseItem)_٠).ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Graphics val2 = BarFunctions.CreateGraphics(val);
				try
				{
					ᆾ = TextDrawing.MeasureStringLegacy(val2, ((BaseItem)_٠).Text, ((Control)ӭ.ކ).Font, Size.Empty, eTextFormat.Default);
					ᆾ.Width += 4;
				}
				finally
				{
					val2.Dispose();
				}
				ᇎ ᇎ2 = ᆻ;
				if (ᇎ2 != null)
				{
					_0E5A = ᇎ2.ᇐ;
				}
				else
				{
					_0E5A = Size.Empty;
				}
			}
			ӭ.ᇌ();
		}

		public void Dispose()
		{
			if (_0CCA != null)
			{
				_0CCA.TextChanged -= _09D0;
			}
		}
	}

	private class ᇀ : IDisposable
	{
		public ArrayList ᇃ = new ArrayList(10);

		public int ᇄ = -1;

		public Bar ᇅ;

		private AutoHidePanel ӭ;

		public static int ᇆ = 4;

		public static int ᇇ = 4;

		public static int ᇈ = 3;

		private Size ᇉ = Size.Empty;

		private int ᇊ;

		public AutoHidePanel ކ => ӭ;

		public Size ᇁ => ᇉ;

		public int ᇂ => ᇊ;

		public ᇀ(AutoHidePanel parent, Bar bar)
		{
			ӭ = parent;
			ᇅ = bar;
			ᇋ();
		}

		public void ᇋ()
		{
			foreach (ᆹ item in ᇃ)
			{
				item.Dispose();
			}
			ᇃ.Clear();
			Graphics val = BarFunctions.CreateGraphics((Control)(object)ᇅ);
			try
			{
				if (ᇅ.LayoutType == eLayoutType.DockContainer)
				{
					foreach (BaseItem item2 in ᇅ.Items)
					{
						if (item2 is DockContainerItem { Visible: not false } dockContainerItem)
						{
							ᆹ ᆹ2 = new ᆹ(this, dockContainerItem);
							ᇃ.Add(ᆹ2);
							ᆹ2.ᆾ = TextDrawing.MeasureStringLegacy(val, dockContainerItem.Text, ((Control)ӭ).Font, Size.Empty, eTextFormat.Default);
							ᆹ2.ᆾ.Width += 4;
						}
					}
				}
				if (ᇃ.Count == 0)
				{
					ᆹ ᆹ3 = new ᆹ(this, ((Control)ᇅ).Text);
					ᇃ.Add(ᆹ3);
					ᆹ3.ᆾ = TextDrawing.MeasureStringLegacy(val, ((Control)ᇅ).Text, ((Control)ӭ).Font, Size.Empty, eTextFormat.Default);
					ᆹ3.ᆾ.Width += 4;
				}
			}
			finally
			{
				val.Dispose();
			}
			ᇉ = ᇍ();
			if (ᇅ.SelectedDockTab >= 0)
			{
				ᇄ = ᇅ.SelectedDockTab;
			}
			else
			{
				ᇄ = 0;
			}
		}

		public void ᇌ()
		{
			ᇉ = ᇍ();
			((Control)ӭ).Refresh();
		}

		public void Dispose()
		{
			foreach (ᆹ item in ᇃ)
			{
				item.Dispose();
			}
			ᇃ.Clear();
			ᇅ = null;
			ӭ = null;
		}

		private Size ᇍ()
		{
			Size result = Size.Empty;
			Size empty = Size.Empty;
			bool flag = true;
			foreach (ᆹ item in ᇃ)
			{
				if (!item._0E5A.IsEmpty)
				{
					if (flag)
					{
						result.Width += item._0E5A.Width + ᇆ * 2;
						result.Height = item._0E5A.Height + ᇈ * 2;
					}
					else
					{
						result.Height += item._0E5A.Height + ᇆ * 2;
						result.Width = item._0E5A.Width + ᇈ * 2;
					}
				}
				if (flag)
				{
					if (item.ᆾ.Width > empty.Width)
					{
						empty.Width = item.ᆾ.Width;
					}
					if (item.ᆾ.Height > empty.Height)
					{
						empty.Height = item.ᆾ.Height;
					}
				}
				else
				{
					if (item.ᆾ.Width > empty.Height)
					{
						empty.Height = item.ᆾ.Width;
					}
					if (item.ᆾ.Height > empty.Width)
					{
						empty.Width = item.ᆾ.Height;
					}
				}
			}
			if (flag)
			{
				ᇊ = empty.Width;
				empty.Width += ᇆ;
				empty.Height += ᇇ * 2;
				result = new Size(result.Width + empty.Width, (result.Height > empty.Height) ? result.Height : empty.Height);
				if (StyleManager.IsMetro(StyleManager.Style))
				{
					result.Height += 6;
				}
			}
			else
			{
				ᇊ = empty.Height;
				empty.Height += ᇆ;
				empty.Width += ᇇ * 2;
				result = new Size((result.Width > empty.Height) ? result.Width : empty.Height, result.Height + empty.Width);
				if (StyleManager.IsMetro(StyleManager.Style))
				{
					result.Width += 6;
				}
			}
			if (ӭ != null && !ӭ.FixedSize.IsEmpty)
			{
				return ӭ.FixedSize;
			}
			return result;
		}
	}

	private class _28F4
	{
		protected AutoHidePanel _28F6;

		private ColorScheme լ;

		private int _2854 = 1;

		private bool _28F7;

		public bool _28F5
		{
			get
			{
				return _28F7;
			}
			set
			{
				_28F7 = value;
			}
		}

		public _28F4(AutoHidePanel autoHidePanel, ColorScheme colorScheme)
		{
			_28F6 = autoHidePanel;
			լ = colorScheme;
		}

		public void _139A(Graphics _٥, ArrayList ᆩ)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Invalid comparison between Unknown and I4
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Invalid comparison between Unknown and I4
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Invalid comparison between Unknown and I4
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Invalid comparison between Unknown and I4
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Invalid comparison between Unknown and I4
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Invalid comparison between Unknown and I4
			ArrayList arrayList = new ArrayList();
			Point point = new Point(0, 0);
			if ((int)((Control)_28F6).Dock == 3 || (int)((Control)_28F6).Dock == 4)
			{
				point.Y = 2;
				point.X = 1;
			}
			else
			{
				point.X = 2;
			}
			foreach (ᇀ item in ᆩ)
			{
				for (int i = 0; i < item.ᇃ.Count; i++)
				{
					bool flag = true;
					if (_28F7 && item.ᇄ != i && !item.ᇅ.AutoHideTabTextAlwaysVisible)
					{
						flag = false;
					}
					ᆹ ᆹ = item.ᇃ[i] as ᆹ;
					if ((int)((Control)_28F6).Dock == 3 || (int)((Control)_28F6).Dock == 4)
					{
						ᆹ.ᆽ = new Rectangle(point.X, point.Y, ((Control)_28F6).Width - 2, ᆹ._0E5A.Width + 2 + ((flag || ᆹ._0E5A.Width == 0) ? ᆹ.ᆾ.Width : 4));
						point.Y += ᆹ.ᆽ.Height + _2854;
					}
					else
					{
						ᆹ.ᆽ = new Rectangle(point.X, point.Y, ᆹ._0E5A.Width + 2 + ((flag || ᆹ._0E5A.Width == 0) ? ᆹ.ᆾ.Width : 4), ((Control)_28F6).Height);
						point.X += ᆹ.ᆽ.Width + _2854;
					}
					if (item.ᇄ == i)
					{
						arrayList.Add(ᆹ);
					}
					else
					{
						PaintTab(_٥, ᆹ, selected: false);
					}
				}
				if ((int)((Control)_28F6).Dock == 3 || (int)((Control)_28F6).Dock == 4)
				{
					point.Y += 2;
				}
				else
				{
					point.X += 2;
				}
			}
			foreach (ᆹ item2 in arrayList)
			{
				PaintTab(_٥, item2, selected: true);
			}
		}

		protected virtual void PaintTab(Graphics g, ᆹ tab, bool selected)
		{
			GraphicsPath tabItemPath = GetTabItemPath(tab);
			TabColors tabColors = GetTabColors(tab, selected);
			DrawTabItemBackground(tab, tabItemPath, tabColors, g);
			DrawTabText(tab, tabColors, g, selected);
		}

		protected virtual TabColors GetTabColors(ᆹ tab, bool selected)
		{
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Invalid comparison between Unknown and I4
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Invalid comparison between Unknown and I4
			TabColors tabColors = new TabColors();
			if (tab._0CCA != null && BarFunctions.IsOffice2007Style(tab._0CCA.EffectiveStyle))
			{
				if (tab._0CCA.PredefinedTabColor == eTabItemColor.Default)
				{
					if (selected && !լ.AutoHideSelectedTabBackground.IsEmpty)
					{
						tabColors.BackColor = լ.AutoHideSelectedTabBackground;
						tabColors.BackColor2 = լ.AutoHideSelectedTabBackground2;
						tabColors.BackColorGradientAngle = լ.AutoHideTabBackgroundGradientAngle;
						tabColors.TextColor = լ.AutoHideSelectedTabText;
						tabColors.BorderColor = լ.AutoHideSelectedTabBorder;
					}
					else if (!լ.AutoHideTabBackground.IsEmpty)
					{
						tabColors.BackColor = լ.AutoHideTabBackground;
						tabColors.BackColor2 = լ.AutoHideTabBackground2;
						tabColors.BackColorGradientAngle = լ.AutoHideTabBackgroundGradientAngle;
						tabColors.TextColor = լ.AutoHideTabText;
						tabColors.BorderColor = լ.AutoHideTabBorder;
					}
					else
					{
						tabColors.BackColor = լ.BarBackground;
						tabColors.BackColor2 = լ.BarBackground2;
						tabColors.BackColorGradientAngle = լ.BarBackgroundGradientAngle;
						tabColors.TextColor = լ.ItemText;
						tabColors.BorderColor = լ.BarDockedBorder;
					}
				}
				else
				{
					TabColorScheme.GetPredefinedColors(tab._0CCA.PredefinedTabColor, out var color, out var color2);
					tabColors.BackColor = color;
					tabColors.BackColor2 = color2;
					tabColors.BackColorGradientAngle = 90;
					tabColors.TextColor = լ.ItemText;
				}
				if (_28F6 != null && ((int)((Control)_28F6).Dock == 3 || (int)((Control)_28F6).Dock == 4))
				{
					tabColors.BackColorGradientAngle -= 90;
				}
			}
			else
			{
				tabColors.BackColor = լ.DockSiteBackColor;
				tabColors.BackColor2 = լ.DockSiteBackColor;
				tabColors.TextColor = SystemColors.ControlText;
				tabColors.BorderColor = SystemColors.ControlDarkDark;
			}
			return tabColors;
		}

		protected virtual void DrawTabItemBackground(ᆹ tab, GraphicsPath path, TabColors colors, Graphics g)
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			RectangleF bounds = path.GetBounds();
			Rectangle r = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
			if (colors.BackColor2.IsEmpty)
			{
				if (!colors.BackColor.IsEmpty)
				{
					SolidBrush val = new SolidBrush(colors.BackColor);
					try
					{
						g.FillPath((Brush)(object)val, path);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			else
			{
				SolidBrush val2 = new SolidBrush(Color.White);
				try
				{
					g.FillPath((Brush)(object)val2, path);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				LinearGradientBrush val3 = CreateTabGradientBrush(r, colors.BackColor, colors.BackColor2, colors.BackColorGradientAngle);
				try
				{
					g.FillPath((Brush)(object)val3, path);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			if (!colors.BorderColor.IsEmpty)
			{
				Pen val4 = new Pen(colors.BorderColor, 1f);
				try
				{
					g.DrawPath(val4, path);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
		}

		protected virtual void DrawTabText(ᆹ tab, TabColors colors, Graphics g, bool selected)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Invalid comparison between Unknown and I4
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Invalid comparison between Unknown and I4
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Invalid comparison between Unknown and I4
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Invalid comparison between Unknown and I4
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Invalid comparison between Unknown and I4
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Invalid comparison between Unknown and I4
			int num = 12;
			eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
			Rectangle bounds = tab.ᆽ;
			ᇎ ᆻ = tab.ᆻ;
			if (ᆻ != null && ᆻ.ܐ + 4 <= bounds.Width)
			{
				if ((int)((Control)_28F6).Dock == 1 || (int)((Control)_28F6).Dock == 2)
				{
					ᆻ.ᇗ(g, new Rectangle(bounds.X + 3, bounds.Y + (bounds.Height - ᆻ._0711) / 2, ᆻ.ܐ, ᆻ._0711));
					int num2 = ᆻ.ܐ + 2;
					bounds.X += num2;
					bounds.Width -= num2;
				}
				else
				{
					ᆻ.ᇗ(g, new Rectangle(bounds.X + (bounds.Width - ᆻ.ܐ) / 2, bounds.Y + 3, ᆻ.ܐ, ᆻ._0711));
					int num3 = ᆻ._0711 + 2;
					bounds.Y += num3;
					bounds.Height -= num3;
				}
			}
			bool flag = false;
			if (tab._0CCA != null && tab._0CCA.ContainerControl is Bar)
			{
				flag = ((Bar)tab._0CCA.ContainerControl).AutoHideTabTextAlwaysVisible;
			}
			if (!_28F7 || flag || selected || ᆻ == null)
			{
				Font font = ((Control)_28F6).Font;
				bounds.Inflate(-1, -1);
				if ((int)((Control)_28F6).Dock == 3 || (int)((Control)_28F6).Dock == 4)
				{
					bounds.Y += 2;
					g.RotateTransform(90f);
					bounds = new Rectangle(bounds.Top, -bounds.Right, bounds.Height, bounds.Width);
				}
				if (bounds.Width > num)
				{
					TextDrawing.DrawStringLegacy(g, tab.ᆺ, font, colors.TextColor, bounds, format);
				}
				if ((int)((Control)_28F6).Dock == 3 || (int)((Control)_28F6).Dock == 4)
				{
					g.ResetTransform();
				}
			}
		}

		protected virtual LinearGradientBrush CreateTabGradientBrush(Rectangle r, Color color1, Color color2, int gradientAngle)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			if (r.Width <= 0)
			{
				r.Width = 1;
			}
			if (r.Height <= 0)
			{
				r.Height = 1;
			}
			return new LinearGradientBrush(new Rectangle(r.X, r.Y - 1, r.Width, r.Height + 1), color1, color2, (float)gradientAngle);
		}

		protected virtual GraphicsPath GetTabItemPath(ᆹ tab)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Invalid comparison between Unknown and I4
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Invalid comparison between Unknown and I4
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Invalid comparison between Unknown and I4
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Invalid comparison between Unknown and I4
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Expected O, but got Unknown
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Invalid comparison between Unknown and I4
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Expected O, but got Unknown
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Expected O, but got Unknown
			Rectangle rectangle = tab.ᆽ;
			if ((int)((Control)_28F6).Dock == 3)
			{
				rectangle = new Rectangle(rectangle.X - (rectangle.Height - rectangle.Width), rectangle.Y, rectangle.Height, rectangle.Width);
			}
			else if ((int)((Control)_28F6).Dock == 4)
			{
				rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Height, rectangle.Width);
			}
			rectangle.Offset(0, 1);
			GraphicsPath val = new GraphicsPath();
			int num = 2;
			val.AddLine(rectangle.X, rectangle.Bottom, rectangle.X, rectangle.Y + num);
			val.AddLine(rectangle.X + num, rectangle.Y, rectangle.Right - num, rectangle.Y);
			val.AddLine(rectangle.Right, rectangle.Y + num, rectangle.Right, rectangle.Bottom);
			val.AddLine(rectangle.Right, rectangle.Bottom, rectangle.X, rectangle.Bottom);
			val.CloseAllFigures();
			if ((int)((Control)_28F6).Dock == 1)
			{
				Matrix val2 = new Matrix();
				val2.RotateAt(180f, new PointF(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2));
				val.Transform(val2);
			}
			else if ((int)((Control)_28F6).Dock == 4)
			{
				Matrix val3 = new Matrix();
				val3.RotateAt(-90f, new PointF(rectangle.X, rectangle.Bottom));
				val3.Translate((float)rectangle.Height, (float)(rectangle.Width - rectangle.Height), (MatrixOrder)1);
				val.Transform(val3);
			}
			else if ((int)((Control)_28F6).Dock == 3)
			{
				Matrix val4 = new Matrix();
				val4.RotateAt(90f, new PointF(rectangle.Right, rectangle.Bottom));
				val4.Translate((float)(-rectangle.Height), (float)(rectangle.Width - (rectangle.Height - 1)), (MatrixOrder)1);
				val.Transform(val4);
			}
			return val;
		}

		private GraphicsPath _2855(Rectangle ल)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			GraphicsPath val = new GraphicsPath();
			val.AddLine(ल.X - _2854, ल.Bottom, ल.X, ल.Y + 5);
			val.AddCurve(new Point[3]
			{
				new Point(ल.X, ल.Y + 5),
				new Point(ल.X + 2, ल.Y + 2),
				new Point(ल.X + 5, ल.Y)
			}, 0.9f);
			return val;
		}

		private GraphicsPath _2856(Rectangle ल)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			GraphicsPath val = new GraphicsPath();
			val.AddCurve(new Point[3]
			{
				new Point(ल.Right - 5, ल.Y),
				new Point(ल.Right - 2, ल.Y + 2),
				new Point(ल.Right, ल.Y + 5)
			}, 0.9f);
			val.AddLine(ल.Right, ल.Y + 5, ल.Right + _2854, ल.Bottom);
			return val;
		}
	}

	private class _28F8 : _28F4
	{
		private const int ủ = 6;

		public _28F8(AutoHidePanel autoHidePanel, ColorScheme colorScheme)
			: base(autoHidePanel, colorScheme)
		{
		}

		protected override GraphicsPath GetTabItemPath(ᆹ tab)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Invalid comparison between Unknown and I4
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Invalid comparison between Unknown and I4
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			Rectangle rectangle = tab.ᆽ;
			if ((int)((Control)_28F6).Dock == 3)
			{
				rectangle = new Rectangle(rectangle.X - (rectangle.Height - rectangle.Width), rectangle.Y, rectangle.Height, rectangle.Width);
			}
			else if ((int)((Control)_28F6).Dock == 4)
			{
				rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Height, rectangle.Width);
			}
			rectangle.Offset(0, 1);
			GraphicsPath val = new GraphicsPath();
			val.AddRectangle(rectangle);
			return val;
		}

		protected override void DrawTabItemBackground(ᆹ tab, GraphicsPath path, TabColors colors, Graphics g)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Invalid comparison between Unknown and I4
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Invalid comparison between Unknown and I4
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Invalid comparison between Unknown and I4
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Expected O, but got Unknown
			Rectangle ᆽ = tab.ᆽ;
			if (!colors.BackColor.IsEmpty)
			{
				SolidBrush val = new SolidBrush(colors.BackColor);
				try
				{
					g.FillPath((Brush)(object)val, path);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			if (!colors.BorderColor.IsEmpty)
			{
				Rectangle rectangle = new Rectangle(ᆽ.X, ᆽ.Y, 6, ᆽ.Height);
				if ((int)((Control)_28F6).Dock == 4)
				{
					rectangle = new Rectangle(ᆽ.Right - 6, ᆽ.Y, 6, ᆽ.Height);
				}
				else if ((int)((Control)_28F6).Dock == 1)
				{
					rectangle = new Rectangle(ᆽ.X, ᆽ.Y, ᆽ.Width, 6);
				}
				else if ((int)((Control)_28F6).Dock == 2)
				{
					rectangle = new Rectangle(ᆽ.X, ᆽ.Bottom - 6, ᆽ.Width, 6);
				}
				SolidBrush val2 = new SolidBrush(colors.BorderColor);
				try
				{
					g.FillRectangle((Brush)(object)val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}

		protected override TabColors GetTabColors(ᆹ tab, bool selected)
		{
			selected = tab.ᆼ;
			return base.GetTabColors(tab, selected);
		}
	}

	private ArrayList ᆩ = new ArrayList(10);

	private Timer ᆪ;

	private eDotNetBarStyle Ӷ;

	private bool ᆫ = true;

	private bool ᆬ = true;

	private int ᆭ = 800;

	private ColorScheme լ;

	private DotNetBarManager _0AC3;

	private Size _ᅫ = Size.Empty;

	private bool ե;

	private ᆹ _ᆮ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Size FixedSize
	{
		get
		{
			return _ᅫ;
		}
		set
		{
			_ᅫ = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			if (ե != value)
			{
				ե = value;
				((Control)this).Invalidate();
			}
		}
	}

	private bool _0AFB
	{
		get
		{
			if (Ӷ == eDotNetBarStyle.Office2003 || Ӷ == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(Ӷ))
			{
				return true;
			}
			return false;
		}
	}

	[Category("Behavior")]
	[Description("Indicates timeout in milliseconds for auto hide/show action.")]
	[DefaultValue(800)]
	public int AutoHideShowTimeout
	{
		get
		{
			return ᆭ;
		}
		set
		{
			ᆭ = value;
		}
	}

	internal DockContainerItem ᆧ
	{
		get
		{
			ᇀ ᇀ = ᆨ;
			if (ᇀ == null)
			{
				foreach (ᇀ item in ᆩ)
				{
					for (int i = 0; i < item.ᇃ.Count; i++)
					{
						ᆹ ᆹ = item.ᇃ[i] as ᆹ;
						if (item.ᇄ == i)
						{
							return ᆹ._0CCA;
						}
					}
				}
				return null;
			}
			try
			{
				ᆹ ᆹ2 = ᇀ.ᇃ[ᇀ.ᇄ] as ᆹ;
				return ᆹ2._0CCA;
			}
			catch
			{
				return null;
			}
		}
		set
		{
			foreach (ᇀ item in ᆩ)
			{
				foreach (ᆹ item2 in item.ᇃ)
				{
					if (item2._0CCA != value)
					{
						continue;
					}
					ᇀ ᇀ2 = ᆨ;
					if (ᇀ2 != null)
					{
						if (ᇀ2 != item)
						{
							if (ᇀ2.ᇅ.ଆ || !_0C45(ᇀ2.ᇅ))
							{
								return;
							}
						}
						else
						{
							if (item.ᇃ[item.ᇄ] == item2 && item.ᇅ.Visible)
							{
								return;
							}
							((ᆹ)ᇀ2.ᇃ[ᇀ2.ᇄ])._0CCA.Displayed = false;
						}
					}
					item.ᇄ = item.ᇃ.IndexOf(item2);
					((Control)this).Refresh();
					return;
				}
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether bars on auto-hide panel are displayed when mouse hovers over the tab.")]
	[Browsable(true)]
	public bool EnableHoverExpand
	{
		get
		{
			return ᆫ;
		}
		set
		{
			ᆫ = value;
		}
	}

	[Description("Indicates whether bars that have focus are collapsed automatically or not.")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool EnableFocusCollapse
	{
		get
		{
			return ᆬ;
		}
		set
		{
			ᆬ = value;
		}
	}

	private ᇀ ᆨ
	{
		get
		{
			foreach (ᇀ item in ᆩ)
			{
				if (item.ᇄ >= 0 && item.ᇅ.Visible)
				{
					return item;
				}
			}
			return null;
		}
	}

	public eDotNetBarStyle Style
	{
		get
		{
			return Ӷ;
		}
		set
		{
			Ӷ = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public ColorScheme ColorScheme
	{
		get
		{
			return լ;
		}
		set
		{
			լ = value;
		}
	}

	public AutoHidePanel()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			ᆵ();
			foreach (ᇀ item in ᆩ)
			{
				item.Dispose();
			}
			ᆩ.Clear();
		}
		((Control)this).Dispose(disposing);
	}

	internal void ၻ(DotNetBarManager ॡ)
	{
		_0AC3 = ॡ;
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Invalid comparison between Unknown and I4
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Invalid comparison between Unknown and I4
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f8: Invalid comparison between Unknown and I4
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a01: Invalid comparison between Unknown and I4
		//IL_1194: Unknown result type (might be due to invalid IL or missing references)
		//IL_119b: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Invalid comparison between Unknown and I4
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Invalid comparison between Unknown and I4
		//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Invalid comparison between Unknown and I4
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Invalid comparison between Unknown and I4
		//IL_0cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d03: Invalid comparison between Unknown and I4
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aac: Invalid comparison between Unknown and I4
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Invalid comparison between Unknown and I4
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Invalid comparison between Unknown and I4
		//IL_0fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fae: Invalid comparison between Unknown and I4
		//IL_10ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f3: Invalid comparison between Unknown and I4
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Invalid comparison between Unknown and I4
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f51: Invalid comparison between Unknown and I4
		eTextFormat format = eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		Graphics graphics = p.Graphics;
		ColorScheme colorScheme = null;
		if (((Control)this).BackColor == Color.Transparent || ((Control)this).BackgroundImage != null)
		{
			((Control)this).OnPaintBackground(p);
		}
		TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		if (ե)
		{
			graphics.TextRenderingHint = BarUtilities.AntiAliasTextRenderingHint;
			graphics.SmoothingMode = (SmoothingMode)4;
		}
		bool flag = BarFunctions.IsOffice2007Style(Ӷ);
		colorScheme = ((flag && լ == null && GlobalManager.Renderer is Office2007Renderer) ? ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors : ((_0AC3 != null && _0AC3.UseGlobalColorScheme) ? _0AC3.ColorScheme : ((լ == null) ? new ColorScheme(Ӷ) : լ)));
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Inflate(1, 1);
		if (colorScheme.AutoHidePanelBackgroundImage != null)
		{
			BarFunctions._0982(graphics, clientRectangle, colorScheme.AutoHidePanelBackgroundImage, eBackgroundImagePosition.Tile, 255);
		}
		if (!colorScheme.AutoHidePanelBackground.IsEmpty)
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, colorScheme.AutoHidePanelBackground, colorScheme.AutoHidePanelBackground2, 90);
		}
		else if (_0AFB)
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, colorScheme.BarBackground, colorScheme.BarBackground2, colorScheme.BarBackgroundGradientAngle);
		}
		else
		{
			SolidBrush val = new SolidBrush(ControlPaint.Light(((Control)this).BackColor));
			try
			{
				graphics.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (Ӷ == eDotNetBarStyle.VS2005 || flag)
		{
			_28F4 _28F4 = (StyleManager.IsMetro(Ӷ) ? new _28F8(this, colorScheme) : new _28F4(this, colorScheme));
			_28F4._28F5 = flag;
			_28F4._139A(graphics, ᆩ);
			if (ե)
			{
				graphics.TextRenderingHint = textRenderingHint;
				graphics.SmoothingMode = smoothingMode;
			}
			return;
		}
		int num = ((Control)this).Width - 2;
		int num2 = ((Control)this).Height - 2;
		Color empty = Color.Empty;
		SolidBrush val2 = null;
		Pen val3 = null;
		if (Ӷ == eDotNetBarStyle.Office2003)
		{
			empty = colorScheme.ItemText;
			val3 = new Pen(colorScheme.ItemHotBorder, 1f);
		}
		else if (Ӷ == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(Ӷ))
		{
			empty = colorScheme.ItemText;
			val3 = new Pen(colorScheme.MenuBorder, 1f);
		}
		else
		{
			empty = graphics.GetNearestColor(ControlPaint.DarkDark(((Control)this).BackColor));
			val2 = new SolidBrush(((Control)this).BackColor);
			val3 = new Pen(graphics.GetNearestColor(ControlPaint.Dark(((Control)this).BackColor)), 1f);
		}
		if ((int)((Control)this).Dock == 1 || (int)((Control)this).Dock == 2 || (int)((Control)this).Dock == 0)
		{
			int num3 = 2;
			foreach (ᇀ item in ᆩ)
			{
				for (int i = 0; i < item.ᇃ.Count; i++)
				{
					ᆹ ᆹ = item.ᇃ[i] as ᆹ;
					ᇎ ᆻ = ᆹ.ᆻ;
					ᆹ.ᆽ.X = num3;
					ᆹ.ᆽ.Y = 0;
					ᆹ.ᆽ.Height = num2;
					num3 += ᇀ.ᇆ;
					if (item.ᇄ == i || item.ᇅ.AutoHideTabTextAlwaysVisible)
					{
						if (ᆻ != null)
						{
							if ((int)((Control)this).Dock == 1)
							{
								if ((Ӷ == eDotNetBarStyle.Office2003 || Ӷ == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
								{
									LinearGradientBrush val4 = BarFunctions.ॴ(new Rectangle(ᆹ.ᆽ.X, 0, ᆻ.ܐ + item.ᇂ + ᇀ.ᇆ * 3, num2), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
									try
									{
										graphics.FillRectangle((Brush)(object)val4, ᆹ.ᆽ.X, 0, ᆻ.ܐ + item.ᇂ + ᇀ.ᇆ * 3, num2);
									}
									finally
									{
										((IDisposable)val4)?.Dispose();
									}
								}
								else if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, ᆹ.ᆽ.X, 0, ᆻ.ܐ + item.ᇂ + ᇀ.ᇆ * 3, num2);
								}
								ᆻ.ᇗ(graphics, new Rectangle(num3, (num2 - ᆻ._0711) / 2, ᆻ.ܐ, ᆻ._0711));
							}
							else
							{
								if ((Ӷ == eDotNetBarStyle.Office2003 || Ӷ == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
								{
									LinearGradientBrush val5 = BarFunctions.ॴ(new Rectangle(ᆹ.ᆽ.X, 2, ᆻ.ܐ + item.ᇂ + ᇀ.ᇆ * 3, num2), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
									try
									{
										graphics.FillRectangle((Brush)(object)val5, ᆹ.ᆽ.X, 2, ᆻ.ܐ + item.ᇂ + ᇀ.ᇆ * 3, num2);
									}
									finally
									{
										((IDisposable)val5)?.Dispose();
									}
								}
								else if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, ᆹ.ᆽ.X, 2, ᆻ.ܐ + item.ᇂ + ᇀ.ᇆ * 3, num2);
								}
								ᆻ.ᇗ(graphics, new Rectangle(num3, (num2 - ᆻ._0711) / 2 + 2, ᆻ.ܐ, ᆻ._0711));
							}
							num3 += ᆻ.ܐ + ᇀ.ᇆ;
						}
						else if ((int)((Control)this).Dock == 1)
						{
							if ((Ӷ == eDotNetBarStyle.Office2003 || Ӷ == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
							{
								LinearGradientBrush val6 = BarFunctions.ॴ(new Rectangle(ᆹ.ᆽ.X, 0, ᆹ.ᆾ.Width + ᇀ.ᇆ * 2, num2), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
								try
								{
									graphics.FillRectangle((Brush)(object)val6, ᆹ.ᆽ.X, 0, ᆹ.ᆾ.Width + ᇀ.ᇆ * 2, num2);
								}
								finally
								{
									((IDisposable)val6)?.Dispose();
								}
							}
							else if (val2 != null)
							{
								graphics.FillRectangle((Brush)(object)val2, ᆹ.ᆽ.X, 0, ᆹ.ᆾ.Width + ᇀ.ᇆ * 2, num2);
							}
						}
						else if ((Ӷ == eDotNetBarStyle.Office2003 || Ӷ == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
						{
							LinearGradientBrush val7 = BarFunctions.ॴ(new Rectangle(ᆹ.ᆽ.X, 2, ᆹ.ᆾ.Width + ᇀ.ᇆ * 2, num2), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
							try
							{
								graphics.FillRectangle((Brush)(object)val7, ᆹ.ᆽ.X, 2, ᆹ.ᆾ.Width + ᇀ.ᇆ * 2, num2);
							}
							finally
							{
								((IDisposable)val7)?.Dispose();
							}
						}
						else if (val2 != null)
						{
							graphics.FillRectangle((Brush)(object)val2, ᆹ.ᆽ.X, 2, ᆹ.ᆾ.Width + ᇀ.ᇆ * 2, num2);
						}
						TextDrawing.DrawStringLegacy(graphics, ᆹ.ᆺ, ((Control)this).Font, empty, new Rectangle(num3, ((int)((Control)this).Dock != 1) ? 2 : 0, (ᆻ == null) ? ᆹ.ᆾ.Width : item.ᇂ, num2), format);
						num3 += ((ᆻ == null) ? ᆹ.ᆾ.Width : item.ᇂ) + ᇀ.ᇆ;
						ᆹ.ᆽ.Width = num3 - ᆹ.ᆽ.X;
						if ((int)((Control)this).Dock == 1)
						{
							ᆹ.ᆽ.Offset(0, -1);
						}
						else
						{
							ᆹ.ᆽ.Offset(0, 1);
							ᆹ.ᆽ.Height += 2;
						}
						graphics.DrawRectangle(val3, ᆹ.ᆽ);
					}
					else
					{
						if (ᆻ != null)
						{
							if ((int)((Control)this).Dock == 1)
							{
								if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, ᆹ.ᆽ.X, 0, ᆻ.ܐ + ᇀ.ᇆ * 2, num2);
								}
								ᆻ.ᇗ(graphics, new Rectangle(num3, (num2 - ᆻ._0711) / 2, ᆻ.ܐ, ᆻ._0711));
							}
							else
							{
								if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, ᆹ.ᆽ.X, 2, ᆻ.ܐ + ᇀ.ᇆ * 2, num2);
								}
								ᆻ.ᇗ(graphics, new Rectangle(num3, (num2 - ᆻ._0711) / 2 + 2, ᆻ.ܐ, ᆻ._0711));
							}
							num3 += ᆻ.ܐ + ᇀ.ᇆ;
						}
						else
						{
							TextDrawing.DrawStringLegacy(graphics, ᆹ.ᆺ, ((Control)this).Font, empty, new Rectangle(num3, ((int)((Control)this).Dock != 1) ? 2 : 0, ᆹ.ᆾ.Width, num2), format);
							num3 += ᆹ.ᆾ.Width + ᇀ.ᇆ;
						}
						ᆹ.ᆽ.Width = num3 - ᆹ.ᆽ.X;
						if ((int)((Control)this).Dock == 1)
						{
							ᆹ.ᆽ.Y--;
						}
						else
						{
							ᆹ.ᆽ.Y++;
							ᆹ.ᆽ.Height += 2;
						}
						graphics.DrawRectangle(val3, ᆹ.ᆽ);
					}
					if (num3 > ((Control)this).Width)
					{
						break;
					}
				}
				num3 += 4;
				if (num3 > ((Control)this).Width)
				{
					break;
				}
			}
		}
		else if ((int)((Control)this).Dock == 3 || (int)((Control)this).Dock == 4)
		{
			int num4 = 2;
			foreach (ᇀ item2 in ᆩ)
			{
				for (int j = 0; j < item2.ᇃ.Count; j++)
				{
					ᆹ ᆹ2 = item2.ᇃ[j] as ᆹ;
					ᇎ ᆻ2 = ᆹ2.ᆻ;
					ᆹ2.ᆽ.X = 0;
					ᆹ2.ᆽ.Y = num4;
					ᆹ2.ᆽ.Width = num;
					num4 += ᇀ.ᇆ;
					if (item2.ᇄ == j || item2.ᇅ.AutoHideTabTextAlwaysVisible)
					{
						if (ᆻ2 != null)
						{
							if ((int)((Control)this).Dock == 3)
							{
								if ((Ӷ == eDotNetBarStyle.Office2003 || Ӷ == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
								{
									LinearGradientBrush val8 = BarFunctions.ॴ(new Rectangle(ᆹ2.ᆽ.X, ᆹ2.ᆽ.Y, num, ᆻ2._0711 + item2.ᇂ + ᇀ.ᇆ * 3), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
									try
									{
										graphics.FillRectangle((Brush)(object)val8, ᆹ2.ᆽ.X, ᆹ2.ᆽ.Y, num, ᆻ2._0711 + item2.ᇂ + ᇀ.ᇆ * 3);
									}
									finally
									{
										((IDisposable)val8)?.Dispose();
									}
								}
								else if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, ᆹ2.ᆽ.X, ᆹ2.ᆽ.Y, num, ᆻ2._0711 + item2.ᇂ + ᇀ.ᇆ * 3);
								}
								ᆻ2.ᇗ(graphics, new Rectangle((num - ᆻ2.ܐ - 2) / 2, num4, ᆻ2.ܐ, ᆻ2._0711));
							}
							else
							{
								if ((Ӷ == eDotNetBarStyle.Office2003 || Ӷ == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
								{
									LinearGradientBrush val9 = BarFunctions.ॴ(new Rectangle(2, ᆹ2.ᆽ.Y, num, ᆻ2._0711 + item2.ᇂ + ᇀ.ᇆ * 3), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
									try
									{
										graphics.FillRectangle((Brush)(object)val9, 2, ᆹ2.ᆽ.Y, num, ᆻ2._0711 + item2.ᇂ + ᇀ.ᇆ * 3);
									}
									finally
									{
										((IDisposable)val9)?.Dispose();
									}
								}
								else if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, 2, ᆹ2.ᆽ.Y, num, ᆻ2._0711 + item2.ᇂ + ᇀ.ᇆ * 3);
								}
								ᆻ2.ᇗ(graphics, new Rectangle((num - ᆻ2.ܐ + 2) / 2, num4, ᆻ2.ܐ, ᆻ2._0711));
							}
							num4 += ᆻ2._0711 + ᇀ.ᇆ;
						}
						else if ((int)((Control)this).Dock == 3)
						{
							if ((Ӷ == eDotNetBarStyle.Office2003 || Ӷ == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
							{
								LinearGradientBrush val10 = BarFunctions.ॴ(new Rectangle(ᆹ2.ᆽ.X, ᆹ2.ᆽ.Y, num, ᆹ2.ᆾ.Width + ᇀ.ᇆ * 2), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
								try
								{
									graphics.FillRectangle((Brush)(object)val10, ᆹ2.ᆽ.X, ᆹ2.ᆽ.Y, num, ᆹ2.ᆾ.Width + ᇀ.ᇆ * 2);
								}
								finally
								{
									((IDisposable)val10)?.Dispose();
								}
							}
							else if (val2 != null)
							{
								graphics.FillRectangle((Brush)(object)val2, ᆹ2.ᆽ.X, ᆹ2.ᆽ.Y, num, ᆹ2.ᆾ.Width + ᇀ.ᇆ * 2);
							}
						}
						else if ((Ӷ == eDotNetBarStyle.Office2003 || Ӷ == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
						{
							LinearGradientBrush val11 = BarFunctions.ॴ(new Rectangle(2, ᆹ2.ᆽ.Y, num, ᆹ2.ᆾ.Width + ᇀ.ᇆ * 2), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
							try
							{
								graphics.FillRectangle((Brush)(object)val11, 2, ᆹ2.ᆽ.Y, num, ᆹ2.ᆾ.Width + ᇀ.ᇆ * 2);
							}
							finally
							{
								((IDisposable)val11)?.Dispose();
							}
						}
						else if (val2 != null)
						{
							graphics.FillRectangle((Brush)(object)val2, 2, ᆹ2.ᆽ.Y, num, ᆹ2.ᆾ.Width + ᇀ.ᇆ * 2);
						}
						graphics.RotateTransform(90f);
						TextDrawing.DrawStringLegacy(graphics, ᆹ2.ᆺ, ((Control)this).Font, empty, new Rectangle(num4, -num, (ᆻ2 == null) ? ᆹ2.ᆾ.Width : item2.ᇂ, num), format);
						graphics.ResetTransform();
						num4 += ((ᆻ2 == null) ? ᆹ2.ᆾ.Width : item2.ᇂ) + ᇀ.ᇆ;
						if ((int)((Control)this).Dock == 3)
						{
							ᆹ2.ᆽ.Offset(-1, 0);
						}
						else
						{
							ᆹ2.ᆽ.Offset(1, 0);
						}
						ᆹ2.ᆽ.Height = num4 - ᆹ2.ᆽ.Y;
						graphics.DrawRectangle(val3, ᆹ2.ᆽ);
					}
					else
					{
						if (ᆻ2 != null)
						{
							if ((int)((Control)this).Dock == 3)
							{
								if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, ᆹ2.ᆽ.X, ᆹ2.ᆽ.Y, num, ᆻ2._0711 + ᇀ.ᇆ * 2);
								}
								ᆻ2.ᇗ(graphics, new Rectangle((num - ᆻ2.ܐ - 2) / 2, num4, ᆻ2.ܐ, ᆻ2._0711));
							}
							else
							{
								if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, 2, ᆹ2.ᆽ.Y, num, ᆻ2._0711 + ᇀ.ᇆ * 2);
								}
								ᆻ2.ᇗ(graphics, new Rectangle((num - ᆻ2.ܐ + 2) / 2, num4, ᆻ2.ܐ, ᆻ2._0711));
							}
							num4 += ᆻ2._0711 + ᇀ.ᇆ;
						}
						else
						{
							graphics.RotateTransform(90f);
							TextDrawing.DrawStringLegacy(graphics, ᆹ2.ᆺ, ((Control)this).Font, empty, new Rectangle(num4, -num, ᆹ2.ᆾ.Width, num), format);
							graphics.ResetTransform();
							num4 += ᆹ2.ᆾ.Width + ᇀ.ᇆ;
						}
						ᆹ2.ᆽ.Height = num4 - ᆹ2.ᆽ.Y;
						if ((int)((Control)this).Dock == 3)
						{
							ᆹ2.ᆽ.X--;
						}
						else
						{
							ᆹ2.ᆽ.X++;
						}
						graphics.DrawRectangle(val3, ᆹ2.ᆽ);
					}
					if (num4 > ((Control)this).Height)
					{
						break;
					}
				}
				num4 += 4;
				if (num4 > ((Control)this).Height)
				{
					break;
				}
			}
		}
		val3.Dispose();
		if (ե)
		{
			graphics.TextRenderingHint = textRenderingHint;
			graphics.SmoothingMode = smoothingMode;
		}
	}

	private void ᆯ(ᆹ ᆰ)
	{
		if (ᆰ != _ᆮ)
		{
			if (_ᆮ != null)
			{
				_ᆮ.ᆼ = false;
				((Control)this).Invalidate(_ᆮ.ᆽ);
			}
			_ᆮ = ᆰ;
			if (_ᆮ != null)
			{
				_ᆮ.ᆼ = true;
				((Control)this).Invalidate(_ᆮ.ᆽ);
			}
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		ᆹ ᆰ = ᆷ(e.X, e.Y);
		ᆯ(ᆰ);
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		ᆱ();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		ᆯ(null);
		if (ᆪ != null)
		{
			ᆪ.Interval = ᆭ;
		}
	}

	private void ᆱ()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (ᆪ != null)
		{
			ᆪ.Start();
			return;
		}
		ᆪ = new Timer();
		ᆪ.Interval = ᆭ;
		ᆪ.Tick += ᆴ;
		ᆪ.Start();
	}

	internal void ᆲ()
	{
		if (ᆪ != null)
		{
			ᆪ.Stop();
		}
	}

	internal void ᆳ()
	{
		if (ᆪ != null)
		{
			ᆪ.Start();
		}
	}

	private void ᆴ(object _٠, EventArgs ؿ)
	{
		Point pt = ((Control)this).PointToClient(Control.MousePosition);
		if (((Control)this).ClientRectangle.Contains(pt))
		{
			IntPtr activeWindow = _089F.GetActiveWindow();
			Form val = ((Control)this).FindForm();
			if (val != null && ((Control)val).Handle != activeWindow)
			{
				Control val2 = (Control)(object)val;
				bool flag = true;
				while (val2.Parent != null)
				{
					val2 = val2.Parent;
					if (val2.Handle == activeWindow)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return;
				}
			}
			if (!ᆫ)
			{
				return;
			}
			if (val != null)
			{
				Point mousePosition = Control.MousePosition;
				Point point = ((Control)val).PointToClient(mousePosition);
				Control childAtPoint = ((Control)val).GetChildAtPoint(point, (GetChildAtPointSkip)5);
				while (childAtPoint != null && !(childAtPoint is AutoHidePanel))
				{
					point = childAtPoint.PointToClient(mousePosition);
					childAtPoint = childAtPoint.GetChildAtPoint(point, (GetChildAtPointSkip)5);
				}
				if (!(childAtPoint is AutoHidePanel))
				{
					ᆲ();
					return;
				}
			}
			SelectPanel(pt.X, pt.Y);
			return;
		}
		ᇀ ᇀ = ᆨ;
		if (ᇀ != null && !ᇀ.ᇅ.ଆ && ᇀ.ᇅ.Visible && (ᆬ || !ᆶ(ᇀ.ᇅ)))
		{
			pt = ((Control)ᇀ.ᇅ).PointToClient(Control.MousePosition);
			if (!((Control)ᇀ.ᇅ).ClientRectangle.Contains(pt) && _0C45(ᇀ.ᇅ))
			{
				ᆵ();
			}
		}
	}

	private void ᆵ()
	{
		if (ᆪ != null)
		{
			ᆪ.Stop();
			((Component)(object)ᆪ).Dispose();
			ᆪ = null;
		}
	}

	private bool ᆶ(Bar ච)
	{
		if (ච == null)
		{
			return false;
		}
		Form val = ((Control)this).FindForm();
		if (val == null)
		{
			return false;
		}
		Control val2 = ((ContainerControl)val).ActiveControl;
		if (val2 == null)
		{
			return false;
		}
		while (val2 != null)
		{
			if (ච == val2)
			{
				return true;
			}
			val2 = val2.Parent;
		}
		return false;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		((Control)this).OnMouseDown(e);
		if ((int)e.Button == 1048576)
		{
			SelectPanel(e.X, e.Y);
		}
	}

	public void SelectPanel(int x, int y)
	{
		foreach (ᇀ item in ᆩ)
		{
			foreach (ᆹ item2 in item.ᇃ)
			{
				if (!item2.ᆽ.Contains(x, y))
				{
					continue;
				}
				ᇀ ᇀ2 = ᆨ;
				if (ᇀ2 != null)
				{
					if (ᇀ2 != item)
					{
						if (ᇀ2.ᇅ.ଆ || !_0C45(ᇀ2.ᇅ))
						{
							return;
						}
					}
					else if (item.ᇄ < item.ᇃ.Count)
					{
						if (item.ᇃ[item.ᇄ] == item2 && item.ᇅ.Visible)
						{
							return;
						}
						((ᆹ)ᇀ2.ᇃ[ᇀ2.ᇄ])._0CCA.Displayed = false;
					}
				}
				if (item.ᇅ.VisibleItemCount == 0)
				{
					throw new InvalidOperationException("Bar.Items collection must contain at least one visible DockContainerItem object so auto-hide functionality can function properly.");
				}
				item.ᇄ = item.ᇃ.IndexOf(item2);
				if (!((Control)item.ᇅ).IsDisposed)
				{
					item.ᇅ.SelectedDockTab = item.ᇅ.Items.IndexOf(item2._0CCA);
					PopupManager.CloseAllPopups();
					_0C44(item.ᇅ);
					((Control)this).Refresh();
				}
				return;
			}
		}
	}

	public DockContainerItem HitTest(int x, int y)
	{
		return ᆷ(x, y)?._0CCA;
	}

	private ᆹ ᆷ(int و, int ى)
	{
		foreach (ᇀ item in ᆩ)
		{
			foreach (ᆹ item2 in item.ᇃ)
			{
				if (item2.ᆽ.Contains(و, ى))
				{
					return item2;
				}
			}
		}
		return null;
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((Control)this).OnFontChanged(e);
		ᆸ();
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		((Control)this).Font = SystemFonts.DefaultFont;
	}

	private void ᆸ()
	{
		foreach (ᇀ item in ᆩ)
		{
			item.ᇋ();
		}
		((Control)this).Invalidate();
	}

	internal void _0C3A(Bar ච)
	{
		ᇀ ᇀ = null;
		foreach (ᇀ item in ᆩ)
		{
			if (item.ᇅ == ච)
			{
				ᇀ = item;
				break;
			}
		}
		ᇀ ᇀ3 = ᆨ;
		if (ᇀ3 == ᇀ)
		{
			return;
		}
		if (ᇀ3 != null)
		{
			if (ᇀ3.ᇅ.ଆ)
			{
				return;
			}
			if (!_0C45(ᇀ3.ᇅ))
			{
				ᆱ();
				return;
			}
		}
		ᇀ.ᇅ.SelectedDockTab = ᇀ.ᇄ;
		_0C44(ᇀ.ᇅ);
		((Control)this).Refresh();
		ᆱ();
	}

	internal void స(Bar ච)
	{
		ᇀ ᇀ = ᆨ;
		if (ᇀ.ᇅ == ච && ᇀ.ᇅ.Visible && _0C45(ᇀ.ᇅ))
		{
			ᆵ();
		}
	}

	private bool _0C45(Bar ච)
	{
		return ච._0C45();
	}

	private bool _0C44(Bar ච)
	{
		if (((Control)ච).Enabled)
		{
			return ච._0C44();
		}
		return false;
	}

	public void SetBarPosition(Bar bar, int iIndex)
	{
		if (iIndex >= ᆩ.Count)
		{
			return;
		}
		for (int i = 0; i < ᆩ.Count; i++)
		{
			if (((ᇀ)ᆩ[i]).ᇅ == bar)
			{
				if (i != iIndex)
				{
					ᇀ value = (ᇀ)ᆩ[i];
					ᆩ.RemoveAt(i);
					ᆩ.Insert(iIndex, value);
					((Control)this).Refresh();
				}
				break;
			}
		}
	}

	public void AddBar(Bar bar)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Invalid comparison between Unknown and I4
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Invalid comparison between Unknown and I4
		if (bar.Style == eDotNetBarStyle.Office2003)
		{
			Ӷ = eDotNetBarStyle.Office2003;
		}
		else if (bar.Style == eDotNetBarStyle.VS2005)
		{
			Ӷ = eDotNetBarStyle.VS2005;
		}
		else if (bar.Style == eDotNetBarStyle.Office2007)
		{
			Ӷ = eDotNetBarStyle.Office2007;
		}
		else if (bar.Style == eDotNetBarStyle.Office2010)
		{
			Ӷ = eDotNetBarStyle.Office2010;
		}
		else if (bar.Style == eDotNetBarStyle.Windows7)
		{
			Ӷ = eDotNetBarStyle.Windows7;
		}
		else if (bar.Style == eDotNetBarStyle.StyleManagerControlled)
		{
			Ӷ = bar.ItemsContainer.EffectiveStyle;
		}
		else
		{
			Ӷ = eDotNetBarStyle.OfficeXP;
		}
		ᇀ ᇀ = new ᇀ(this, bar);
		ᆩ.Add(ᇀ);
		if ((int)((Control)this).Dock == 4 || (int)((Control)this).Dock == 3)
		{
			if (ᇀ.ᇁ.Height > ((Control)this).Width)
			{
				((Control)this).Width = ᇀ.ᇁ.Height;
			}
		}
		else if (ᇀ.ᇁ.Height > ((Control)this).Height)
		{
			((Control)this).Height = ᇀ.ᇁ.Height;
		}
		if (!((Control)this).Visible)
		{
			BarFunctions.SetControlVisible((Control)(object)this, visible: true);
		}
		else
		{
			((Control)this).Refresh();
		}
		((Control)this).Parent.PerformLayout();
		((Control)this).Parent.Update();
	}

	public void RemoveBar(Bar bar)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		foreach (ᇀ item in ᆩ)
		{
			if (item.ᇅ != bar)
			{
				continue;
			}
			ᆩ.Remove(item);
			item.Dispose();
			if (ᆩ.Count == 0)
			{
				((Control)this).Visible = false;
				if ((int)((Control)this).Dock == 4 || (int)((Control)this).Dock == 3)
				{
					((Control)this).Width = 0;
				}
				else
				{
					((Control)this).Height = 0;
				}
			}
			else
			{
				((Control)this).Refresh();
			}
			break;
		}
		if (ᆩ.Count == 0)
		{
			ᆵ();
		}
	}

	public void RefreshBar(Bar bar)
	{
		foreach (ᇀ item in ᆩ)
		{
			if (item.ᇅ == bar)
			{
				item.ᇋ();
				break;
			}
		}
		((Control)this).Refresh();
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		((Control)this).OnHandleDestroyed(e);
		ᆵ();
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		ᇀ ᇀ = ᆨ;
		if (ᇀ != null && ᇀ.ᇅ.Visible)
		{
			ᇀ.ᇅ._0C43();
		}
	}
}
