using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[ComVisible(false)]
public class AutoHidePanel : Control
{
	private class DockItemTab : IDisposable
	{
		public Rectangle DisplayRectangle = new Rectangle(0, 0, 0, 0);

		public DockContainerItem Item;

		public Size TextSize = Size.Empty;

		public Size IconSize = Size.Empty;

		private string m_Text = "";

		private PanelBar m_Parent;

		public string Text
		{
			get
			{
				if (Item == null)
				{
					return m_Text;
				}
				return Item.Text;
			}
		}

		public CompositeImage Icon
		{
			get
			{
				if (Item == null)
				{
					return null;
				}
				if (Item.Image != null)
				{
					return new CompositeImage(Item.Image, dispose: false);
				}
				if (Item.ImageIndex >= 0 && Item.ImageList != null)
				{
					return new CompositeImage(Item.ImageList.Images[Item.ImageIndex], dispose: false);
				}
				if (Item.Icon != null)
				{
					return new CompositeImage(Item.Icon, dispose: false);
				}
				return null;
			}
		}

		public DockItemTab(PanelBar parent, DockContainerItem item)
		{
			Item = item;
			m_Parent = parent;
			item.TextChanged += ItemTextChanged;
			CompositeImage icon = Icon;
			if (icon != null)
			{
				IconSize = icon.Size;
			}
		}

		public DockItemTab(PanelBar parent, string text)
		{
			m_Text = text;
			m_Parent = parent;
		}

		private void ItemTextChanged(object sender, EventArgs e)
		{
			object containerControl = ((BaseItem)sender).ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Graphics val2 = BarFunctions.CreateGraphics(val);
				try
				{
					TextSize = TextDrawing.MeasureStringLegacy(val2, ((BaseItem)sender).Text, ((Control)m_Parent.Parent).Font, Size.Empty, eTextFormat.Default);
					TextSize.Width += 4;
				}
				finally
				{
					val2.Dispose();
				}
				CompositeImage icon = Icon;
				if (icon != null)
				{
					IconSize = icon.Size;
				}
				else
				{
					IconSize = Size.Empty;
				}
			}
			m_Parent.Refresh();
		}

		public void Dispose()
		{
			if (Item != null)
			{
				Item.TextChanged -= ItemTextChanged;
			}
		}
	}

	private class PanelBar : IDisposable
	{
		public ArrayList Tabs = new ArrayList(10);

		public int ActiveTab = -1;

		public Bar BoundBar;

		private AutoHidePanel m_Parent;

		public static int TabPadding = 4;

		public static int TabTextVPadding = 4;

		public static int TabIconVPadding = 3;

		private Size m_PanelSize = Size.Empty;

		private int m_MaxTextWidth;

		public AutoHidePanel Parent => m_Parent;

		public Size PanelSize => m_PanelSize;

		public int MaxTextWidth => m_MaxTextWidth;

		public PanelBar(AutoHidePanel parent, Bar bar)
		{
			m_Parent = parent;
			BoundBar = bar;
			ReloadDockItems();
		}

		public void ReloadDockItems()
		{
			foreach (DockItemTab tab in Tabs)
			{
				tab.Dispose();
			}
			Tabs.Clear();
			Graphics val = BarFunctions.CreateGraphics((Control)(object)BoundBar);
			try
			{
				if (BoundBar.LayoutType == eLayoutType.DockContainer)
				{
					foreach (BaseItem item in BoundBar.Items)
					{
						if (item is DockContainerItem { Visible: not false } dockContainerItem)
						{
							DockItemTab dockItemTab2 = new DockItemTab(this, dockContainerItem);
							Tabs.Add(dockItemTab2);
							dockItemTab2.TextSize = TextDrawing.MeasureStringLegacy(val, dockContainerItem.Text, ((Control)m_Parent).Font, Size.Empty, eTextFormat.Default);
							dockItemTab2.TextSize.Width += 4;
						}
					}
				}
				if (Tabs.Count == 0)
				{
					DockItemTab dockItemTab3 = new DockItemTab(this, ((Control)BoundBar).Text);
					Tabs.Add(dockItemTab3);
					dockItemTab3.TextSize = TextDrawing.MeasureStringLegacy(val, ((Control)BoundBar).Text, ((Control)m_Parent).Font, Size.Empty, eTextFormat.Default);
					dockItemTab3.TextSize.Width += 4;
				}
			}
			finally
			{
				val.Dispose();
			}
			m_PanelSize = CalcPanelSize();
			if (BoundBar.SelectedDockTab >= 0)
			{
				ActiveTab = BoundBar.SelectedDockTab;
			}
			else
			{
				ActiveTab = 0;
			}
		}

		public void Refresh()
		{
			m_PanelSize = CalcPanelSize();
			((Control)m_Parent).Refresh();
		}

		public void Dispose()
		{
			foreach (DockItemTab tab in Tabs)
			{
				tab.Dispose();
			}
			Tabs.Clear();
			BoundBar = null;
			m_Parent = null;
		}

		private Size CalcPanelSize()
		{
			Size result = Size.Empty;
			Size empty = Size.Empty;
			bool flag = true;
			foreach (DockItemTab tab in Tabs)
			{
				if (!tab.IconSize.IsEmpty)
				{
					if (flag)
					{
						result.Width += tab.IconSize.Width + TabPadding * 2;
						result.Height = tab.IconSize.Height + TabIconVPadding * 2;
					}
					else
					{
						result.Height += tab.IconSize.Height + TabPadding * 2;
						result.Width = tab.IconSize.Width + TabIconVPadding * 2;
					}
				}
				if (flag)
				{
					if (tab.TextSize.Width > empty.Width)
					{
						empty.Width = tab.TextSize.Width;
					}
					if (tab.TextSize.Height > empty.Height)
					{
						empty.Height = tab.TextSize.Height;
					}
				}
				else
				{
					if (tab.TextSize.Width > empty.Height)
					{
						empty.Height = tab.TextSize.Width;
					}
					if (tab.TextSize.Height > empty.Width)
					{
						empty.Width = tab.TextSize.Height;
					}
				}
			}
			if (flag)
			{
				m_MaxTextWidth = empty.Width;
				empty.Width += TabPadding;
				empty.Height += TabTextVPadding * 2;
				result = new Size(result.Width + empty.Width, (result.Height > empty.Height) ? result.Height : empty.Height);
			}
			else
			{
				m_MaxTextWidth = empty.Height;
				empty.Height += TabPadding;
				empty.Width += TabTextVPadding * 2;
				result = new Size((result.Width > empty.Height) ? result.Width : empty.Height, result.Height + empty.Width);
			}
			if (m_Parent != null && !m_Parent.FixedSize.IsEmpty)
			{
				return m_Parent.FixedSize;
			}
			return result;
		}
	}

	private class VS2005TabDisplay
	{
		private AutoHidePanel m_AutoHidePanel;

		private ColorScheme m_ColorScheme;

		private int m_xTabOffset = 1;

		private bool m_DisplayTextForActiveTabOnly;

		public bool DisplayTextForActiveTabOnly
		{
			get
			{
				return m_DisplayTextForActiveTabOnly;
			}
			set
			{
				m_DisplayTextForActiveTabOnly = value;
			}
		}

		public VS2005TabDisplay(AutoHidePanel autoHidePanel, ColorScheme colorScheme)
		{
			m_AutoHidePanel = autoHidePanel;
			m_ColorScheme = colorScheme;
		}

		public void Paint(Graphics g, ArrayList m_Panels)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Invalid comparison between Unknown and I4
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Invalid comparison between Unknown and I4
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Invalid comparison between Unknown and I4
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Invalid comparison between Unknown and I4
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Invalid comparison between Unknown and I4
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Invalid comparison between Unknown and I4
			ArrayList arrayList = new ArrayList();
			Point point = new Point(0, 0);
			if ((int)((Control)m_AutoHidePanel).Dock == 3 || (int)((Control)m_AutoHidePanel).Dock == 4)
			{
				point.Y = 2;
				point.X = 1;
			}
			else
			{
				point.X = 2;
			}
			foreach (PanelBar m_Panel in m_Panels)
			{
				for (int i = 0; i < m_Panel.Tabs.Count; i++)
				{
					bool flag = true;
					if (m_DisplayTextForActiveTabOnly && m_Panel.ActiveTab != i && !m_Panel.BoundBar.AutoHideTabTextAlwaysVisible)
					{
						flag = false;
					}
					DockItemTab dockItemTab = m_Panel.Tabs[i] as DockItemTab;
					if ((int)((Control)m_AutoHidePanel).Dock == 3 || (int)((Control)m_AutoHidePanel).Dock == 4)
					{
						dockItemTab.DisplayRectangle = new Rectangle(point.X, point.Y, ((Control)m_AutoHidePanel).Width - 2, dockItemTab.IconSize.Width + 2 + ((flag || dockItemTab.IconSize.Width == 0) ? dockItemTab.TextSize.Width : 4));
						point.Y += dockItemTab.DisplayRectangle.Height + m_xTabOffset;
					}
					else
					{
						dockItemTab.DisplayRectangle = new Rectangle(point.X, point.Y, dockItemTab.IconSize.Width + 2 + ((flag || dockItemTab.IconSize.Width == 0) ? dockItemTab.TextSize.Width : 4), ((Control)m_AutoHidePanel).Height);
						point.X += dockItemTab.DisplayRectangle.Width + m_xTabOffset;
					}
					if (m_Panel.ActiveTab == i)
					{
						arrayList.Add(dockItemTab);
					}
					else
					{
						PaintTab(g, dockItemTab, selected: false);
					}
				}
				if ((int)((Control)m_AutoHidePanel).Dock == 3 || (int)((Control)m_AutoHidePanel).Dock == 4)
				{
					point.Y += 2;
				}
				else
				{
					point.X += 2;
				}
			}
			foreach (DockItemTab item in arrayList)
			{
				PaintTab(g, item, selected: true);
			}
		}

		protected virtual void PaintTab(Graphics g, DockItemTab tab, bool selected)
		{
			GraphicsPath tabItemPath = GetTabItemPath(tab);
			TabColors tabColors = GetTabColors(tab, selected);
			DrawTabItemBackground(tab, tabItemPath, tabColors, g);
			DrawTabText(tab, tabColors, g, selected);
		}

		private TabColors GetTabColors(DockItemTab tab, bool selected)
		{
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Invalid comparison between Unknown and I4
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Invalid comparison between Unknown and I4
			TabColors tabColors = new TabColors();
			if (tab.Item != null && BarFunctions.IsOffice2007Style(tab.Item.EffectiveStyle))
			{
				if (tab.Item.PredefinedTabColor == eTabItemColor.Default)
				{
					if (selected && !m_ColorScheme.AutoHideSelectedTabBackground.IsEmpty)
					{
						tabColors.BackColor = m_ColorScheme.AutoHideSelectedTabBackground;
						tabColors.BackColor2 = m_ColorScheme.AutoHideSelectedTabBackground2;
						tabColors.BackColorGradientAngle = m_ColorScheme.AutoHideTabBackgroundGradientAngle;
						tabColors.TextColor = m_ColorScheme.AutoHideSelectedTabText;
						tabColors.BorderColor = m_ColorScheme.AutoHideSelectedTabBorder;
					}
					else if (!m_ColorScheme.AutoHideTabBackground.IsEmpty)
					{
						tabColors.BackColor = m_ColorScheme.AutoHideTabBackground;
						tabColors.BackColor2 = m_ColorScheme.AutoHideTabBackground2;
						tabColors.BackColorGradientAngle = m_ColorScheme.AutoHideTabBackgroundGradientAngle;
						tabColors.TextColor = m_ColorScheme.AutoHideTabText;
						tabColors.BorderColor = m_ColorScheme.AutoHideTabBorder;
					}
					else
					{
						tabColors.BackColor = m_ColorScheme.BarBackground;
						tabColors.BackColor2 = m_ColorScheme.BarBackground2;
						tabColors.BackColorGradientAngle = m_ColorScheme.BarBackgroundGradientAngle;
						tabColors.TextColor = m_ColorScheme.ItemText;
						tabColors.BorderColor = m_ColorScheme.BarDockedBorder;
					}
				}
				else
				{
					TabColorScheme.GetPredefinedColors(tab.Item.PredefinedTabColor, out var color, out var color2);
					tabColors.BackColor = color;
					tabColors.BackColor2 = color2;
					tabColors.BackColorGradientAngle = 90;
					tabColors.TextColor = m_ColorScheme.ItemText;
				}
				if (m_AutoHidePanel != null && ((int)((Control)m_AutoHidePanel).Dock == 3 || (int)((Control)m_AutoHidePanel).Dock == 4))
				{
					tabColors.BackColorGradientAngle -= 90;
				}
			}
			else
			{
				tabColors.BackColor = m_ColorScheme.DockSiteBackColor;
				tabColors.BackColor2 = m_ColorScheme.DockSiteBackColor;
				tabColors.TextColor = SystemColors.ControlText;
				tabColors.BorderColor = SystemColors.ControlDarkDark;
			}
			return tabColors;
		}

		protected virtual void DrawTabItemBackground(DockItemTab tab, GraphicsPath path, TabColors colors, Graphics g)
		{
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
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

		protected virtual void DrawTabText(DockItemTab tab, TabColors colors, Graphics g, bool selected)
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
			Rectangle bounds = tab.DisplayRectangle;
			CompositeImage icon = tab.Icon;
			if (icon != null && icon.Width + 4 <= bounds.Width)
			{
				if ((int)((Control)m_AutoHidePanel).Dock == 1 || (int)((Control)m_AutoHidePanel).Dock == 2)
				{
					icon.DrawImage(g, new Rectangle(bounds.X + 3, bounds.Y + (bounds.Height - icon.Height) / 2, icon.Width, icon.Height));
					int num2 = icon.Width + 2;
					bounds.X += num2;
					bounds.Width -= num2;
				}
				else
				{
					icon.DrawImage(g, new Rectangle(bounds.X + (bounds.Width - icon.Width) / 2, bounds.Y + 3, icon.Width, icon.Height));
					int num3 = icon.Height + 2;
					bounds.Y += num3;
					bounds.Height -= num3;
				}
			}
			bool flag = false;
			if (tab.Item != null && tab.Item.ContainerControl is Bar)
			{
				flag = ((Bar)tab.Item.ContainerControl).AutoHideTabTextAlwaysVisible;
			}
			if (!m_DisplayTextForActiveTabOnly || flag || selected || icon == null)
			{
				Font font = ((Control)m_AutoHidePanel).Font;
				bounds.Inflate(-1, -1);
				if ((int)((Control)m_AutoHidePanel).Dock == 3 || (int)((Control)m_AutoHidePanel).Dock == 4)
				{
					bounds.Y += 2;
					g.RotateTransform(90f);
					bounds = new Rectangle(bounds.Top, -bounds.Right, bounds.Height, bounds.Width);
				}
				if (bounds.Width > num)
				{
					TextDrawing.DrawStringLegacy(g, tab.Text, font, colors.TextColor, bounds, format);
				}
				if ((int)((Control)m_AutoHidePanel).Dock == 3 || (int)((Control)m_AutoHidePanel).Dock == 4)
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

		protected GraphicsPath GetTabItemPath(DockItemTab tab)
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
			Rectangle rectangle = tab.DisplayRectangle;
			if ((int)((Control)m_AutoHidePanel).Dock == 3)
			{
				rectangle = new Rectangle(rectangle.X - (rectangle.Height - rectangle.Width), rectangle.Y, rectangle.Height, rectangle.Width);
			}
			else if ((int)((Control)m_AutoHidePanel).Dock == 4)
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
			if ((int)((Control)m_AutoHidePanel).Dock == 1)
			{
				Matrix val2 = new Matrix();
				val2.RotateAt(180f, new PointF(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2));
				val.Transform(val2);
			}
			else if ((int)((Control)m_AutoHidePanel).Dock == 4)
			{
				Matrix val3 = new Matrix();
				val3.RotateAt(-90f, new PointF(rectangle.X, rectangle.Bottom));
				val3.Translate((float)rectangle.Height, (float)(rectangle.Width - rectangle.Height), (MatrixOrder)1);
				val.Transform(val3);
			}
			else if ((int)((Control)m_AutoHidePanel).Dock == 3)
			{
				Matrix val4 = new Matrix();
				val4.RotateAt(90f, new PointF(rectangle.Right, rectangle.Bottom));
				val4.Translate((float)(-rectangle.Height), (float)(rectangle.Width - (rectangle.Height - 1)), (MatrixOrder)1);
				val.Transform(val4);
			}
			return val;
		}

		private GraphicsPath GetLeftLine(Rectangle r)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			GraphicsPath val = new GraphicsPath();
			val.AddLine(r.X - m_xTabOffset, r.Bottom, r.X, r.Y + 5);
			val.AddCurve(new Point[3]
			{
				new Point(r.X, r.Y + 5),
				new Point(r.X + 2, r.Y + 2),
				new Point(r.X + 5, r.Y)
			}, 0.9f);
			return val;
		}

		private GraphicsPath GetRightLine(Rectangle r)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			GraphicsPath val = new GraphicsPath();
			val.AddCurve(new Point[3]
			{
				new Point(r.Right - 5, r.Y),
				new Point(r.Right - 2, r.Y + 2),
				new Point(r.Right, r.Y + 5)
			}, 0.9f);
			val.AddLine(r.Right, r.Y + 5, r.Right + m_xTabOffset, r.Bottom);
			return val;
		}
	}

	private ArrayList m_Panels = new ArrayList(10);

	private Timer m_Timer;

	private eDotNetBarStyle m_Style;

	private bool m_EnableHoverExpand = true;

	private bool m_EnableFocusCollapse = true;

	private int m_AutoHideShowTimeout = 800;

	private ColorScheme m_ColorScheme;

	private DotNetBarManager m_Owner;

	private Size _FixedSize = Size.Empty;

	private bool m_AntiAlias;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Size FixedSize
	{
		get
		{
			return _FixedSize;
		}
		set
		{
			_FixedSize = value;
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
			return m_AntiAlias;
		}
		set
		{
			if (m_AntiAlias != value)
			{
				m_AntiAlias = value;
				((Control)this).Invalidate();
			}
		}
	}

	private bool IsGradientStyle
	{
		get
		{
			if (m_Style == eDotNetBarStyle.Office2003 || m_Style == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_Style))
			{
				return true;
			}
			return false;
		}
	}

	[DefaultValue(800)]
	[Category("Behavior")]
	[Description("Indicates timeout in milliseconds for auto hide/show action.")]
	public int AutoHideShowTimeout
	{
		get
		{
			return m_AutoHideShowTimeout;
		}
		set
		{
			m_AutoHideShowTimeout = value;
		}
	}

	internal DockContainerItem SelectedDockContainerItem
	{
		get
		{
			PanelBar activePanelBar = ActivePanelBar;
			if (activePanelBar == null)
			{
				foreach (PanelBar panel in m_Panels)
				{
					for (int i = 0; i < panel.Tabs.Count; i++)
					{
						DockItemTab dockItemTab = panel.Tabs[i] as DockItemTab;
						if (panel.ActiveTab == i)
						{
							return dockItemTab.Item;
						}
					}
				}
				return null;
			}
			try
			{
				DockItemTab dockItemTab2 = activePanelBar.Tabs[activePanelBar.ActiveTab] as DockItemTab;
				return dockItemTab2.Item;
			}
			catch
			{
				return null;
			}
		}
		set
		{
			foreach (PanelBar panel in m_Panels)
			{
				foreach (DockItemTab tab in panel.Tabs)
				{
					if (tab.Item != value)
					{
						continue;
					}
					PanelBar activePanelBar = ActivePanelBar;
					if (activePanelBar != null)
					{
						if (activePanelBar != panel)
						{
							if (activePanelBar.BoundBar.IsSizingWindow || !AnimateHide(activePanelBar.BoundBar))
							{
								return;
							}
						}
						else
						{
							if (panel.Tabs[panel.ActiveTab] == tab && panel.BoundBar.Visible)
							{
								return;
							}
							((DockItemTab)activePanelBar.Tabs[activePanelBar.ActiveTab]).Item.Displayed = false;
						}
					}
					panel.ActiveTab = panel.Tabs.IndexOf(tab);
					((Control)this).Refresh();
					return;
				}
			}
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether bars on auto-hide panel are displayed when mouse hovers over the tab.")]
	public bool EnableHoverExpand
	{
		get
		{
			return m_EnableHoverExpand;
		}
		set
		{
			m_EnableHoverExpand = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether bars that have focus are collapsed automatically or not.")]
	public bool EnableFocusCollapse
	{
		get
		{
			return m_EnableFocusCollapse;
		}
		set
		{
			m_EnableFocusCollapse = value;
		}
	}

	private PanelBar ActivePanelBar
	{
		get
		{
			foreach (PanelBar panel in m_Panels)
			{
				if (panel.ActiveTab >= 0 && panel.BoundBar.Visible)
				{
					return panel;
				}
			}
			return null;
		}
	}

	public eDotNetBarStyle Style
	{
		get
		{
			return m_Style;
		}
		set
		{
			m_Style = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public ColorScheme ColorScheme
	{
		get
		{
			return m_ColorScheme;
		}
		set
		{
			m_ColorScheme = value;
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
		((Control)this).Font = SystemFonts.DefaultFont;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			DestroyTimer();
			foreach (PanelBar panel in m_Panels)
			{
				panel.Dispose();
			}
			m_Panels.Clear();
		}
		((Control)this).Dispose(disposing);
	}

	internal void SetOwner(DotNetBarManager owner)
	{
		m_Owner = owner;
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
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Invalid comparison between Unknown and I4
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Invalid comparison between Unknown and I4
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Invalid comparison between Unknown and I4
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f3: Invalid comparison between Unknown and I4
		//IL_118d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Invalid comparison between Unknown and I4
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Invalid comparison between Unknown and I4
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Invalid comparison between Unknown and I4
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fd: Invalid comparison between Unknown and I4
		//IL_0cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf5: Invalid comparison between Unknown and I4
		//IL_0a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9e: Invalid comparison between Unknown and I4
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Invalid comparison between Unknown and I4
		//IL_0928: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Invalid comparison between Unknown and I4
		//IL_0f97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9d: Invalid comparison between Unknown and I4
		//IL_10dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e2: Invalid comparison between Unknown and I4
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a7: Invalid comparison between Unknown and I4
		//IL_0f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f40: Invalid comparison between Unknown and I4
		eTextFormat format = eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		Graphics graphics = p.Graphics;
		ColorScheme colorScheme = null;
		if (((Control)this).BackColor == Color.Transparent || ((Control)this).BackgroundImage != null)
		{
			((Control)this).OnPaintBackground(p);
		}
		TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		if (m_AntiAlias)
		{
			graphics.TextRenderingHint = BarUtilities.AntiAliasTextRenderingHint;
			graphics.SmoothingMode = (SmoothingMode)4;
		}
		bool flag = BarFunctions.IsOffice2007Style(m_Style);
		colorScheme = ((flag && m_ColorScheme == null && GlobalManager.Renderer is Office2007Renderer) ? ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors : ((m_Owner != null && m_Owner.UseGlobalColorScheme) ? m_Owner.ColorScheme : ((m_ColorScheme == null) ? new ColorScheme(m_Style) : m_ColorScheme)));
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Inflate(1, 1);
		if (colorScheme.AutoHidePanelBackgroundImage != null)
		{
			BarFunctions.PaintBackgroundImage(graphics, clientRectangle, colorScheme.AutoHidePanelBackgroundImage, eBackgroundImagePosition.Tile, 255);
		}
		if (!colorScheme.AutoHidePanelBackground.IsEmpty)
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, colorScheme.AutoHidePanelBackground, colorScheme.AutoHidePanelBackground2, 90);
		}
		else if (IsGradientStyle)
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
		if (m_Style == eDotNetBarStyle.VS2005 || flag)
		{
			VS2005TabDisplay vS2005TabDisplay = new VS2005TabDisplay(this, colorScheme);
			vS2005TabDisplay.DisplayTextForActiveTabOnly = flag;
			vS2005TabDisplay.Paint(graphics, m_Panels);
			if (m_AntiAlias)
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
		if (m_Style == eDotNetBarStyle.Office2003)
		{
			empty = colorScheme.ItemText;
			val3 = new Pen(colorScheme.ItemHotBorder, 1f);
		}
		else if (m_Style == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(m_Style))
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
			foreach (PanelBar panel in m_Panels)
			{
				for (int i = 0; i < panel.Tabs.Count; i++)
				{
					DockItemTab dockItemTab = panel.Tabs[i] as DockItemTab;
					CompositeImage icon = dockItemTab.Icon;
					dockItemTab.DisplayRectangle.X = num3;
					dockItemTab.DisplayRectangle.Y = 0;
					dockItemTab.DisplayRectangle.Height = num2;
					num3 += PanelBar.TabPadding;
					if (panel.ActiveTab == i || panel.BoundBar.AutoHideTabTextAlwaysVisible)
					{
						if (icon != null)
						{
							if ((int)((Control)this).Dock == 1)
							{
								if ((m_Style == eDotNetBarStyle.Office2003 || m_Style == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
								{
									LinearGradientBrush val4 = BarFunctions.CreateLinearGradientBrush(new Rectangle(dockItemTab.DisplayRectangle.X, 0, icon.Width + panel.MaxTextWidth + PanelBar.TabPadding * 3, num2), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
									try
									{
										graphics.FillRectangle((Brush)(object)val4, dockItemTab.DisplayRectangle.X, 0, icon.Width + panel.MaxTextWidth + PanelBar.TabPadding * 3, num2);
									}
									finally
									{
										((IDisposable)val4)?.Dispose();
									}
								}
								else if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, dockItemTab.DisplayRectangle.X, 0, icon.Width + panel.MaxTextWidth + PanelBar.TabPadding * 3, num2);
								}
								icon.DrawImage(graphics, new Rectangle(num3, (num2 - icon.Height) / 2, icon.Width, icon.Height));
							}
							else
							{
								if ((m_Style == eDotNetBarStyle.Office2003 || m_Style == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
								{
									LinearGradientBrush val5 = BarFunctions.CreateLinearGradientBrush(new Rectangle(dockItemTab.DisplayRectangle.X, 2, icon.Width + panel.MaxTextWidth + PanelBar.TabPadding * 3, num2), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
									try
									{
										graphics.FillRectangle((Brush)(object)val5, dockItemTab.DisplayRectangle.X, 2, icon.Width + panel.MaxTextWidth + PanelBar.TabPadding * 3, num2);
									}
									finally
									{
										((IDisposable)val5)?.Dispose();
									}
								}
								else if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, dockItemTab.DisplayRectangle.X, 2, icon.Width + panel.MaxTextWidth + PanelBar.TabPadding * 3, num2);
								}
								icon.DrawImage(graphics, new Rectangle(num3, (num2 - icon.Height) / 2 + 2, icon.Width, icon.Height));
							}
							num3 += icon.Width + PanelBar.TabPadding;
						}
						else if ((int)((Control)this).Dock == 1)
						{
							if ((m_Style == eDotNetBarStyle.Office2003 || m_Style == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
							{
								LinearGradientBrush val6 = BarFunctions.CreateLinearGradientBrush(new Rectangle(dockItemTab.DisplayRectangle.X, 0, dockItemTab.TextSize.Width + PanelBar.TabPadding * 2, num2), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
								try
								{
									graphics.FillRectangle((Brush)(object)val6, dockItemTab.DisplayRectangle.X, 0, dockItemTab.TextSize.Width + PanelBar.TabPadding * 2, num2);
								}
								finally
								{
									((IDisposable)val6)?.Dispose();
								}
							}
							else if (val2 != null)
							{
								graphics.FillRectangle((Brush)(object)val2, dockItemTab.DisplayRectangle.X, 0, dockItemTab.TextSize.Width + PanelBar.TabPadding * 2, num2);
							}
						}
						else if ((m_Style == eDotNetBarStyle.Office2003 || m_Style == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
						{
							LinearGradientBrush val7 = BarFunctions.CreateLinearGradientBrush(new Rectangle(dockItemTab.DisplayRectangle.X, 2, dockItemTab.TextSize.Width + PanelBar.TabPadding * 2, num2), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
							try
							{
								graphics.FillRectangle((Brush)(object)val7, dockItemTab.DisplayRectangle.X, 2, dockItemTab.TextSize.Width + PanelBar.TabPadding * 2, num2);
							}
							finally
							{
								((IDisposable)val7)?.Dispose();
							}
						}
						else if (val2 != null)
						{
							graphics.FillRectangle((Brush)(object)val2, dockItemTab.DisplayRectangle.X, 2, dockItemTab.TextSize.Width + PanelBar.TabPadding * 2, num2);
						}
						TextDrawing.DrawStringLegacy(graphics, dockItemTab.Text, ((Control)this).Font, empty, new Rectangle(num3, ((int)((Control)this).Dock != 1) ? 2 : 0, (icon == null) ? dockItemTab.TextSize.Width : panel.MaxTextWidth, num2), format);
						num3 += ((icon == null) ? dockItemTab.TextSize.Width : panel.MaxTextWidth) + PanelBar.TabPadding;
						dockItemTab.DisplayRectangle.Width = num3 - dockItemTab.DisplayRectangle.X;
						if ((int)((Control)this).Dock == 1)
						{
							dockItemTab.DisplayRectangle.Offset(0, -1);
						}
						else
						{
							dockItemTab.DisplayRectangle.Offset(0, 1);
							dockItemTab.DisplayRectangle.Height += 2;
						}
						graphics.DrawRectangle(val3, dockItemTab.DisplayRectangle);
					}
					else
					{
						if (icon != null)
						{
							if ((int)((Control)this).Dock == 1)
							{
								if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, dockItemTab.DisplayRectangle.X, 0, icon.Width + PanelBar.TabPadding * 2, num2);
								}
								icon.DrawImage(graphics, new Rectangle(num3, (num2 - icon.Height) / 2, icon.Width, icon.Height));
							}
							else
							{
								if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, dockItemTab.DisplayRectangle.X, 2, icon.Width + PanelBar.TabPadding * 2, num2);
								}
								icon.DrawImage(graphics, new Rectangle(num3, (num2 - icon.Height) / 2 + 2, icon.Width, icon.Height));
							}
							num3 += icon.Width + PanelBar.TabPadding;
						}
						else
						{
							TextDrawing.DrawStringLegacy(graphics, dockItemTab.Text, ((Control)this).Font, empty, new Rectangle(num3, ((int)((Control)this).Dock != 1) ? 2 : 0, dockItemTab.TextSize.Width, num2), format);
							num3 += dockItemTab.TextSize.Width + PanelBar.TabPadding;
						}
						dockItemTab.DisplayRectangle.Width = num3 - dockItemTab.DisplayRectangle.X;
						if ((int)((Control)this).Dock == 1)
						{
							dockItemTab.DisplayRectangle.Y--;
						}
						else
						{
							dockItemTab.DisplayRectangle.Y++;
							dockItemTab.DisplayRectangle.Height += 2;
						}
						graphics.DrawRectangle(val3, dockItemTab.DisplayRectangle);
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
			foreach (PanelBar panel2 in m_Panels)
			{
				for (int j = 0; j < panel2.Tabs.Count; j++)
				{
					DockItemTab dockItemTab2 = panel2.Tabs[j] as DockItemTab;
					CompositeImage icon2 = dockItemTab2.Icon;
					dockItemTab2.DisplayRectangle.X = 0;
					dockItemTab2.DisplayRectangle.Y = num4;
					dockItemTab2.DisplayRectangle.Width = num;
					num4 += PanelBar.TabPadding;
					if (panel2.ActiveTab == j || panel2.BoundBar.AutoHideTabTextAlwaysVisible)
					{
						if (icon2 != null)
						{
							if ((int)((Control)this).Dock == 3)
							{
								if ((m_Style == eDotNetBarStyle.Office2003 || m_Style == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
								{
									LinearGradientBrush val8 = BarFunctions.CreateLinearGradientBrush(new Rectangle(dockItemTab2.DisplayRectangle.X, dockItemTab2.DisplayRectangle.Y, num, icon2.Height + panel2.MaxTextWidth + PanelBar.TabPadding * 3), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
									try
									{
										graphics.FillRectangle((Brush)(object)val8, dockItemTab2.DisplayRectangle.X, dockItemTab2.DisplayRectangle.Y, num, icon2.Height + panel2.MaxTextWidth + PanelBar.TabPadding * 3);
									}
									finally
									{
										((IDisposable)val8)?.Dispose();
									}
								}
								else if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, dockItemTab2.DisplayRectangle.X, dockItemTab2.DisplayRectangle.Y, num, icon2.Height + panel2.MaxTextWidth + PanelBar.TabPadding * 3);
								}
								icon2.DrawImage(graphics, new Rectangle((num - icon2.Width - 2) / 2, num4, icon2.Width, icon2.Height));
							}
							else
							{
								if ((m_Style == eDotNetBarStyle.Office2003 || m_Style == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
								{
									LinearGradientBrush val9 = BarFunctions.CreateLinearGradientBrush(new Rectangle(2, dockItemTab2.DisplayRectangle.Y, num, icon2.Height + panel2.MaxTextWidth + PanelBar.TabPadding * 3), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
									try
									{
										graphics.FillRectangle((Brush)(object)val9, 2, dockItemTab2.DisplayRectangle.Y, num, icon2.Height + panel2.MaxTextWidth + PanelBar.TabPadding * 3);
									}
									finally
									{
										((IDisposable)val9)?.Dispose();
									}
								}
								else if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, 2, dockItemTab2.DisplayRectangle.Y, num, icon2.Height + panel2.MaxTextWidth + PanelBar.TabPadding * 3);
								}
								icon2.DrawImage(graphics, new Rectangle((num - icon2.Width + 2) / 2, num4, icon2.Width, icon2.Height));
							}
							num4 += icon2.Height + PanelBar.TabPadding;
						}
						else if ((int)((Control)this).Dock == 3)
						{
							if ((m_Style == eDotNetBarStyle.Office2003 || m_Style == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
							{
								LinearGradientBrush val10 = BarFunctions.CreateLinearGradientBrush(new Rectangle(dockItemTab2.DisplayRectangle.X, dockItemTab2.DisplayRectangle.Y, num, dockItemTab2.TextSize.Width + PanelBar.TabPadding * 2), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
								try
								{
									graphics.FillRectangle((Brush)(object)val10, dockItemTab2.DisplayRectangle.X, dockItemTab2.DisplayRectangle.Y, num, dockItemTab2.TextSize.Width + PanelBar.TabPadding * 2);
								}
								finally
								{
									((IDisposable)val10)?.Dispose();
								}
							}
							else if (val2 != null)
							{
								graphics.FillRectangle((Brush)(object)val2, dockItemTab2.DisplayRectangle.X, dockItemTab2.DisplayRectangle.Y, num, dockItemTab2.TextSize.Width + PanelBar.TabPadding * 2);
							}
						}
						else if ((m_Style == eDotNetBarStyle.Office2003 || m_Style == eDotNetBarStyle.VS2005) && !colorScheme.ItemPressedBackground2.IsEmpty)
						{
							LinearGradientBrush val11 = BarFunctions.CreateLinearGradientBrush(new Rectangle(2, dockItemTab2.DisplayRectangle.Y, num, dockItemTab2.TextSize.Width + PanelBar.TabPadding * 2), colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2, colorScheme.ItemPressedBackgroundGradientAngle);
							try
							{
								graphics.FillRectangle((Brush)(object)val11, 2, dockItemTab2.DisplayRectangle.Y, num, dockItemTab2.TextSize.Width + PanelBar.TabPadding * 2);
							}
							finally
							{
								((IDisposable)val11)?.Dispose();
							}
						}
						else if (val2 != null)
						{
							graphics.FillRectangle((Brush)(object)val2, 2, dockItemTab2.DisplayRectangle.Y, num, dockItemTab2.TextSize.Width + PanelBar.TabPadding * 2);
						}
						graphics.RotateTransform(90f);
						TextDrawing.DrawStringLegacy(graphics, dockItemTab2.Text, ((Control)this).Font, empty, new Rectangle(num4, -num, (icon2 == null) ? dockItemTab2.TextSize.Width : panel2.MaxTextWidth, num), format);
						graphics.ResetTransform();
						num4 += ((icon2 == null) ? dockItemTab2.TextSize.Width : panel2.MaxTextWidth) + PanelBar.TabPadding;
						if ((int)((Control)this).Dock == 3)
						{
							dockItemTab2.DisplayRectangle.Offset(-1, 0);
						}
						else
						{
							dockItemTab2.DisplayRectangle.Offset(1, 0);
						}
						dockItemTab2.DisplayRectangle.Height = num4 - dockItemTab2.DisplayRectangle.Y;
						graphics.DrawRectangle(val3, dockItemTab2.DisplayRectangle);
					}
					else
					{
						if (icon2 != null)
						{
							if ((int)((Control)this).Dock == 3)
							{
								if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, dockItemTab2.DisplayRectangle.X, dockItemTab2.DisplayRectangle.Y, num, icon2.Height + PanelBar.TabPadding * 2);
								}
								icon2.DrawImage(graphics, new Rectangle((num - icon2.Width - 2) / 2, num4, icon2.Width, icon2.Height));
							}
							else
							{
								if (val2 != null)
								{
									graphics.FillRectangle((Brush)(object)val2, 2, dockItemTab2.DisplayRectangle.Y, num, icon2.Height + PanelBar.TabPadding * 2);
								}
								icon2.DrawImage(graphics, new Rectangle((num - icon2.Width + 2) / 2, num4, icon2.Width, icon2.Height));
							}
							num4 += icon2.Height + PanelBar.TabPadding;
						}
						else
						{
							graphics.RotateTransform(90f);
							TextDrawing.DrawStringLegacy(graphics, dockItemTab2.Text, ((Control)this).Font, empty, new Rectangle(num4, -num, dockItemTab2.TextSize.Width, num), format);
							graphics.ResetTransform();
							num4 += dockItemTab2.TextSize.Width + PanelBar.TabPadding;
						}
						dockItemTab2.DisplayRectangle.Height = num4 - dockItemTab2.DisplayRectangle.Y;
						if ((int)((Control)this).Dock == 3)
						{
							dockItemTab2.DisplayRectangle.X--;
						}
						else
						{
							dockItemTab2.DisplayRectangle.X++;
						}
						graphics.DrawRectangle(val3, dockItemTab2.DisplayRectangle);
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
		if (m_AntiAlias)
		{
			graphics.TextRenderingHint = textRenderingHint;
			graphics.SmoothingMode = smoothingMode;
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		CreateTimer();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (m_Timer != null)
		{
			m_Timer.Interval = m_AutoHideShowTimeout;
		}
	}

	private void CreateTimer()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (m_Timer != null)
		{
			m_Timer.Start();
			return;
		}
		m_Timer = new Timer();
		m_Timer.Interval = m_AutoHideShowTimeout;
		m_Timer.Tick += TimerTick;
		m_Timer.Start();
	}

	internal void StopTimer()
	{
		if (m_Timer != null)
		{
			m_Timer.Stop();
		}
	}

	internal void StartTimer()
	{
		if (m_Timer != null)
		{
			m_Timer.Start();
		}
	}

	private void TimerTick(object sender, EventArgs e)
	{
		Point pt = ((Control)this).PointToClient(Control.MousePosition);
		if (((Control)this).ClientRectangle.Contains(pt))
		{
			IntPtr activeWindow = NativeFunctions.GetActiveWindow();
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
			if (m_EnableHoverExpand)
			{
				SelectPanel(pt.X, pt.Y);
			}
			return;
		}
		PanelBar activePanelBar = ActivePanelBar;
		if (activePanelBar != null && !activePanelBar.BoundBar.IsSizingWindow && activePanelBar.BoundBar.Visible && (m_EnableFocusCollapse || !IsBarActive(activePanelBar.BoundBar)))
		{
			pt = ((Control)activePanelBar.BoundBar).PointToClient(Control.MousePosition);
			if (!((Control)activePanelBar.BoundBar).ClientRectangle.Contains(pt) && AnimateHide(activePanelBar.BoundBar))
			{
				DestroyTimer();
			}
		}
	}

	private void DestroyTimer()
	{
		if (m_Timer != null)
		{
			m_Timer.Stop();
			((Component)(object)m_Timer).Dispose();
			m_Timer = null;
		}
	}

	private bool IsBarActive(Bar bar)
	{
		if (bar == null)
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
			if (bar == val2)
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
		foreach (PanelBar panel in m_Panels)
		{
			foreach (DockItemTab tab in panel.Tabs)
			{
				if (!tab.DisplayRectangle.Contains(x, y))
				{
					continue;
				}
				PanelBar activePanelBar = ActivePanelBar;
				if (activePanelBar != null)
				{
					if (activePanelBar != panel)
					{
						if (activePanelBar.BoundBar.IsSizingWindow || !AnimateHide(activePanelBar.BoundBar))
						{
							return;
						}
					}
					else if (panel.ActiveTab < panel.Tabs.Count)
					{
						if (panel.Tabs[panel.ActiveTab] == tab && panel.BoundBar.Visible)
						{
							return;
						}
						((DockItemTab)activePanelBar.Tabs[activePanelBar.ActiveTab]).Item.Displayed = false;
					}
				}
				if (panel.BoundBar.VisibleItemCount != 0)
				{
					panel.ActiveTab = panel.Tabs.IndexOf(tab);
					if (!((Control)panel.BoundBar).IsDisposed)
					{
						panel.BoundBar.SelectedDockTab = panel.BoundBar.Items.IndexOf(tab.Item);
						AnimateShow(panel.BoundBar);
						((Control)this).Refresh();
					}
				}
				return;
			}
		}
	}

	public DockContainerItem HitTest(int x, int y)
	{
		foreach (PanelBar panel in m_Panels)
		{
			foreach (DockItemTab tab in panel.Tabs)
			{
				if (tab.DisplayRectangle.Contains(x, y))
				{
					return tab.Item;
				}
			}
		}
		return null;
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((Control)this).OnFontChanged(e);
		RefreshPanels();
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		((Control)this).Font = SystemFonts.DefaultFont;
	}

	private void RefreshPanels()
	{
		foreach (PanelBar panel in m_Panels)
		{
			panel.ReloadDockItems();
		}
		((Control)this).Invalidate();
	}

	internal void ShowBar(Bar bar)
	{
		PanelBar panelBar = null;
		foreach (PanelBar panel in m_Panels)
		{
			if (panel.BoundBar == bar)
			{
				panelBar = panel;
				break;
			}
		}
		PanelBar activePanelBar = ActivePanelBar;
		if (activePanelBar == panelBar)
		{
			return;
		}
		if (activePanelBar != null)
		{
			if (activePanelBar.BoundBar.IsSizingWindow)
			{
				return;
			}
			if (!AnimateHide(activePanelBar.BoundBar))
			{
				CreateTimer();
				return;
			}
		}
		panelBar.BoundBar.SelectedDockTab = panelBar.ActiveTab;
		AnimateShow(panelBar.BoundBar);
		((Control)this).Refresh();
		CreateTimer();
	}

	internal void HideBar(Bar bar)
	{
		PanelBar activePanelBar = ActivePanelBar;
		if (activePanelBar.BoundBar == bar && activePanelBar.BoundBar.Visible && AnimateHide(activePanelBar.BoundBar))
		{
			DestroyTimer();
		}
	}

	private bool AnimateHide(Bar bar)
	{
		return bar.AnimateHide();
	}

	private bool AnimateShow(Bar bar)
	{
		if (((Control)bar).Enabled)
		{
			return bar.AnimateShow();
		}
		return false;
	}

	public void SetBarPosition(Bar bar, int iIndex)
	{
		if (iIndex >= m_Panels.Count)
		{
			return;
		}
		for (int i = 0; i < m_Panels.Count; i++)
		{
			if (((PanelBar)m_Panels[i]).BoundBar == bar)
			{
				if (i != iIndex)
				{
					PanelBar value = (PanelBar)m_Panels[i];
					m_Panels.RemoveAt(i);
					m_Panels.Insert(iIndex, value);
					((Control)this).Refresh();
				}
				break;
			}
		}
	}

	public void AddBar(Bar bar)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Invalid comparison between Unknown and I4
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Invalid comparison between Unknown and I4
		if (bar.Style == eDotNetBarStyle.Office2003)
		{
			m_Style = eDotNetBarStyle.Office2003;
		}
		else if (bar.Style == eDotNetBarStyle.VS2005)
		{
			m_Style = eDotNetBarStyle.VS2005;
		}
		else if (bar.Style == eDotNetBarStyle.Office2007)
		{
			m_Style = eDotNetBarStyle.Office2007;
		}
		else if (bar.Style == eDotNetBarStyle.Office2010)
		{
			m_Style = eDotNetBarStyle.Office2010;
		}
		else if (bar.Style == eDotNetBarStyle.Windows7)
		{
			m_Style = eDotNetBarStyle.Windows7;
		}
		else if (bar.Style == eDotNetBarStyle.StyleManagerControlled)
		{
			m_Style = bar.ItemsContainer.EffectiveStyle;
		}
		else
		{
			m_Style = eDotNetBarStyle.OfficeXP;
		}
		PanelBar panelBar = new PanelBar(this, bar);
		m_Panels.Add(panelBar);
		if ((int)((Control)this).Dock == 4 || (int)((Control)this).Dock == 3)
		{
			if (panelBar.PanelSize.Height > ((Control)this).Width)
			{
				((Control)this).Width = panelBar.PanelSize.Height;
			}
		}
		else if (panelBar.PanelSize.Height > ((Control)this).Height)
		{
			((Control)this).Height = panelBar.PanelSize.Height;
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
		foreach (PanelBar panel in m_Panels)
		{
			if (panel.BoundBar != bar)
			{
				continue;
			}
			m_Panels.Remove(panel);
			panel.Dispose();
			if (m_Panels.Count == 0)
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
		if (m_Panels.Count == 0)
		{
			DestroyTimer();
		}
	}

	public void RefreshBar(Bar bar)
	{
		foreach (PanelBar panel in m_Panels)
		{
			if (panel.BoundBar == bar)
			{
				panel.ReloadDockItems();
				break;
			}
		}
		((Control)this).Refresh();
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		((Control)this).OnHandleDestroyed(e);
		DestroyTimer();
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		PanelBar activePanelBar = ActivePanelBar;
		if (activePanelBar != null && activePanelBar.BoundBar.Visible)
		{
			activePanelBar.BoundBar.SetAutoHideSize();
		}
	}
}
