using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class RibbonPanel : PanelControl, IKeyTipsControl
{
	private class XPositionComparer : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (x is Control && y is Control)
			{
				return ((Control)x).Left - ((Control)y).Left;
			}
			return new CaseInsensitiveComparer().Compare(x, y);
		}
	}

	private class ResizeOrderComparer : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			if (x is RibbonBar && y is RibbonBar)
			{
				return ((RibbonBar)x).ResizeOrderIndex - ((RibbonBar)y).ResizeOrderIndex;
			}
			return new CaseInsensitiveComparer().Compare(x, y);
		}
	}

	private const string INFO_TEXT = "Drop RibbonBar or other controls here. Drag and Drop tabs and items to re-order.";

	private RibbonTabItem m_RibbonTabItem;

	private bool m_UseCustomStyle;

	private bool m_DefaultLayout;

	private bool m_StretchLastRibbonBar;

	private ElementStyle m_DefaultStyle = new ElementStyle();

	private RightToLeft m_RightToLeft;

	private bool m_InternalLayoutSuspend;

	private ButtonX m_ButtonScrollRight;

	private ButtonX m_ButtonScrollLeft;

	private Rectangle m_ViewBounds = Rectangle.Empty;

	private int m_ScrollOffset;

	private bool m_PopupMode;

	private RibbonControl m_RibbonControl;

	private bool m_ShowKeyTips;

	private string m_KeyTipsKeysStack = "";

	internal bool IsPopupMode => m_PopupMode;

	internal bool InternalLayoutSuspend
	{
		get
		{
			return m_InternalLayoutSuspend;
		}
		set
		{
			m_InternalLayoutSuspend = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Layout")]
	[Description("Indicates whether default control layout is used instead of Rendering layout for RibbonBar controls positioning.")]
	public bool DefaultLayout
	{
		get
		{
			return m_DefaultLayout;
		}
		set
		{
			m_DefaultLayout = value;
			((Control)this).PerformLayout();
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether style of the panel is managed by tab control automatically. Set this to true if you would like to control style of the panel.")]
	public bool UseCustomStyle
	{
		get
		{
			return m_UseCustomStyle;
		}
		set
		{
			m_UseCustomStyle = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public RibbonTabItem RibbonTabItem
	{
		get
		{
			return m_RibbonTabItem;
		}
		set
		{
			m_RibbonTabItem = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Layout")]
	[Description("Whether last RibbonBar is stretched to fill available space inside of the panel.")]
	public bool StretchLastRibbonBar
	{
		get
		{
			return m_StretchLastRibbonBar;
		}
		set
		{
			m_StretchLastRibbonBar = value;
			((Control)this).PerformLayout();
		}
	}

	[Browsable(false)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public override DockStyle Dock
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Dock;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Dock = value;
		}
	}

	[Browsable(false)]
	public Size Size
	{
		get
		{
			return ((Control)this).Size;
		}
		set
		{
			((Control)this).Size = value;
		}
	}

	[Browsable(false)]
	public Point Location
	{
		get
		{
			return ((Control)this).Location;
		}
		set
		{
			((Control)this).Location = value;
		}
	}

	[Browsable(false)]
	public bool Visible
	{
		get
		{
			return ((Control)this).Visible;
		}
		set
		{
			((Control)this).Visible = value;
		}
	}

	[Browsable(false)]
	public override AnchorStyles Anchor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Anchor;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Anchor = value;
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
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

	bool IKeyTipsControl.ShowKeyTips
	{
		get
		{
			return m_ShowKeyTips;
		}
		set
		{
			m_ShowKeyTips = value;
			Control[] array = (Control[])(object)new Control[((ArrangedElementCollection)((Control)this).Controls).Count];
			((ArrangedElementCollection)((Control)this).Controls).CopyTo((Array)array, 0);
			Control[] array2 = array;
			foreach (Control val in array2)
			{
				if (val is IKeyTipsControl && val.Enabled && (val.Visible || !m_ShowKeyTips))
				{
					((IKeyTipsControl)val).ShowKeyTips = m_ShowKeyTips;
				}
			}
		}
	}

	string IKeyTipsControl.KeyTipsKeysStack
	{
		get
		{
			return m_KeyTipsKeysStack;
		}
		set
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			m_KeyTipsKeysStack = value;
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is IKeyTipsControl && val.Visible && val.Enabled)
				{
					((IKeyTipsControl)val).KeyTipsKeysStack = m_KeyTipsKeysStack;
				}
			}
		}
	}

	public RibbonPanel()
	{
		((Control)this).BackColor = SystemColors.Control;
	}

	protected override ElementStyle GetStyle()
	{
		if (!Style.Custom)
		{
			return GetDefaultStyle();
		}
		return base.GetStyle();
	}

	internal RibbonControl GetRibbonControl()
	{
		if (m_PopupMode)
		{
			return m_RibbonControl;
		}
		return ((Control)this).Parent as RibbonControl;
	}

	internal void SetPopupMode(bool popupMode, RibbonControl rc)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		m_PopupMode = popupMode;
		Padding padding;
		if (m_PopupMode)
		{
			m_RibbonControl = rc;
			padding = ((Control)this).Padding;
			if (((Padding)(ref padding)).Bottom > 0)
			{
				int height = ((Control)this).Height;
				padding = ((Control)this).Padding;
				((Control)this).Height = height + ((Padding)(ref padding)).Bottom;
				padding = ((Control)this).Padding;
				int left = ((Padding)(ref padding)).Left;
				padding = ((Control)this).Padding;
				int bottom = ((Padding)(ref padding)).Bottom;
				padding = ((Control)this).Padding;
				int right = ((Padding)(ref padding)).Right;
				padding = ((Control)this).Padding;
				((Control)this).Padding = new Padding(left, bottom, right, ((Padding)(ref padding)).Bottom);
			}
		}
		else
		{
			padding = ((Control)this).Padding;
			if (((Padding)(ref padding)).Top > 0)
			{
				int height2 = ((Control)this).Height;
				padding = ((Control)this).Padding;
				((Control)this).Height = height2 - ((Padding)(ref padding)).Top;
				padding = ((Control)this).Padding;
				int left2 = ((Padding)(ref padding)).Left;
				padding = ((Control)this).Padding;
				int right2 = ((Padding)(ref padding)).Right;
				padding = ((Control)this).Padding;
				((Control)this).Padding = new Padding(left2, 0, right2, ((Padding)(ref padding)).Bottom);
			}
			m_RibbonControl = null;
		}
	}

	private ElementStyle GetDefaultStyle()
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && base.ColorSchemeStyle != ribbonControl.Style)
		{
			base.ColorSchemeStyle = ribbonControl.Style;
		}
		m_DefaultStyle.SetColorScheme(base.ColorScheme);
		if (BarFunctions.IsOffice2007Style(base.EffectiveColorSchemeStyle) || (ribbonControl != null && BarFunctions.IsOffice2007Style(ribbonControl.EffectiveStyle)))
		{
			m_DefaultStyle.Reset();
			Office2007ColorTable office2007ColorTable = null;
			if (ribbonControl != null)
			{
				office2007ColorTable = ribbonControl.GetOffice2007ColorTable();
			}
			else
			{
				if (!(GlobalManager.Renderer is Office2007Renderer))
				{
					return Style;
				}
				office2007ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			}
			if (base.EffectiveColorSchemeStyle == eDotNetBarStyle.Metro || (ribbonControl != null && ribbonControl.EffectiveStyle == eDotNetBarStyle.Metro))
			{
				m_DefaultStyle.Border = eStyleBorderType.Solid;
			}
			else
			{
				m_DefaultStyle.Border = eStyleBorderType.Double;
			}
			if (ribbonControl == null || !ribbonControl.IsPopupMode)
			{
				m_DefaultStyle.BorderTop = eStyleBorderType.None;
			}
			m_DefaultStyle.BorderWidth = 1;
			m_DefaultStyle.CornerDiameter = office2007ColorTable.RibbonControl.CornerSize;
			m_DefaultStyle.CornerType = eCornerType.Rounded;
			if (!m_PopupMode)
			{
				m_DefaultStyle.CornerTypeTopLeft = eCornerType.Square;
				m_DefaultStyle.CornerTypeTopRight = eCornerType.Square;
			}
			else if (office2007ColorTable.InitialColorScheme == eOffice2007ColorScheme.Black)
			{
				m_DefaultStyle.BorderTop = eStyleBorderType.None;
			}
			if (ribbonControl != null && (ribbonControl.EffectiveStyle == eDotNetBarStyle.Office2010 || ribbonControl.EffectiveStyle == eDotNetBarStyle.Metro))
			{
				m_DefaultStyle.CornerTypeBottomRight = eCornerType.Square;
				m_DefaultStyle.CornerTypeBottomLeft = eCornerType.Square;
				if (ribbonControl.RibbonStrip.IsGlassEnabled)
				{
					m_DefaultStyle.BorderLeftWidth = 0;
					m_DefaultStyle.BorderRightWidth = 0;
					m_DefaultStyle.BorderLeft = eStyleBorderType.None;
					m_DefaultStyle.BorderRight = eStyleBorderType.None;
				}
				else if (ribbonControl.EffectiveStyle == eDotNetBarStyle.Metro)
				{
					m_DefaultStyle.BorderLeft = eStyleBorderType.None;
					m_DefaultStyle.BorderRight = eStyleBorderType.None;
				}
				else
				{
					m_DefaultStyle.BorderLeft = eStyleBorderType.Solid;
					m_DefaultStyle.BorderRight = eStyleBorderType.Solid;
				}
			}
			else if (ribbonControl != null && ribbonControl.EffectiveStyle == eDotNetBarStyle.Windows7)
			{
				m_DefaultStyle.CornerType = eCornerType.Square;
				m_DefaultStyle.CornerTypeBottomRight = eCornerType.Square;
				m_DefaultStyle.CornerTypeBottomLeft = eCornerType.Square;
				m_DefaultStyle.CornerTypeTopRight = eCornerType.Square;
				m_DefaultStyle.CornerTypeTopLeft = eCornerType.Square;
				m_DefaultStyle.CornerDiameter = 0;
				if (ribbonControl.RibbonStrip.IsGlassEnabled)
				{
					m_DefaultStyle.BorderLeft = eStyleBorderType.None;
					m_DefaultStyle.BorderRight = eStyleBorderType.None;
				}
			}
			m_DefaultStyle.BorderColor = office2007ColorTable.RibbonControl.OuterBorder.Start;
			m_DefaultStyle.BorderColor2 = office2007ColorTable.RibbonControl.OuterBorder.End;
			if (office2007ColorTable.RibbonControl.InnerBorder != null)
			{
				m_DefaultStyle.BorderColorLight = office2007ColorTable.RibbonControl.InnerBorder.Start;
				m_DefaultStyle.BorderColorLight2 = office2007ColorTable.RibbonControl.InnerBorder.End;
				m_DefaultStyle.BorderBottom = eStyleBorderType.Double;
			}
			else
			{
				m_DefaultStyle.BorderBottom = eStyleBorderType.Solid;
			}
			m_DefaultStyle.BorderWidth = 1;
			m_DefaultStyle.BackColorGradientAngle = 90;
			m_DefaultStyle.BackColor = office2007ColorTable.RibbonBar.Default.TopBackground.Start;
			if (office2007ColorTable.RibbonControl.PanelTopBackgroundHeight == 0f && office2007ColorTable.RibbonControl.PanelBottomBackground == null)
			{
				m_DefaultStyle.BackColor = office2007ColorTable.RibbonControl.PanelTopBackground.Start;
				m_DefaultStyle.BackColor2 = office2007ColorTable.RibbonControl.PanelTopBackground.End;
			}
			else
			{
				m_DefaultStyle.BackColorBlend.Add(new BackgroundColorBlend(office2007ColorTable.RibbonControl.PanelTopBackground.Start, 0f));
				m_DefaultStyle.BackColorBlend.Add(new BackgroundColorBlend(office2007ColorTable.RibbonControl.PanelTopBackground.End, office2007ColorTable.RibbonControl.PanelTopBackgroundHeight));
				m_DefaultStyle.BackColorBlend.Add(new BackgroundColorBlend(office2007ColorTable.RibbonControl.PanelBottomBackground.Start, office2007ColorTable.RibbonControl.PanelTopBackgroundHeight));
				m_DefaultStyle.BackColorBlend.Add(new BackgroundColorBlend(office2007ColorTable.RibbonControl.PanelBottomBackground.End, 1f));
			}
		}
		else
		{
			ApplyLabelStyle(m_DefaultStyle);
			m_DefaultStyle.BorderBottom = eStyleBorderType.Solid;
		}
		return m_DefaultStyle;
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		if (!(e.Control is RibbonBar))
		{
			m_DefaultLayout = false;
		}
		base.OnControlAdded(e);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	protected override void OnLayout(LayoutEventArgs levent)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Invalid comparison between Unknown and I4
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		if (m_DefaultLayout || ((Component)(object)this).DesignMode)
		{
			((ScrollableControl)this).OnLayout(levent);
		}
		else
		{
			if (((Control)this).ClientRectangle.Width == 0 || ((Control)this).ClientRectangle.Height == 0 || !((Control)this).IsHandleCreated)
			{
				return;
			}
			Form val = ((Control)this).FindForm();
			if (val != null && (int)val.WindowState == 1)
			{
				return;
			}
			if (m_RightToLeft != ((Control)this).RightToLeft)
			{
				m_RightToLeft = ((Control)this).RightToLeft;
				Rectangle layoutRectangle = GetLayoutRectangle();
				foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
				{
					Control val2 = item;
					if (val2 is RibbonBar)
					{
						val2.Left = layoutRectangle.Width - val2.Width - val2.Left;
					}
				}
			}
			if (!m_InternalLayoutSuspend)
			{
				LayoutRibbons();
			}
		}
	}

	private Rectangle GetLayoutRectangle()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.X += ((ScrollableControl)this).DockPadding.Left;
		clientRectangle.Width -= ((ScrollableControl)this).DockPadding.Left + ((ScrollableControl)this).DockPadding.Right;
		clientRectangle.Y += ((ScrollableControl)this).DockPadding.Top;
		clientRectangle.Height -= ((ScrollableControl)this).DockPadding.Top + (((base.EffectiveColorSchemeStyle == eDotNetBarStyle.Office2010 || base.EffectiveColorSchemeStyle == eDotNetBarStyle.Metro) && ((ScrollableControl)this).DockPadding.Bottom == 3) ? 2 : ((ScrollableControl)this).DockPadding.Bottom);
		if (base.EffectiveColorSchemeStyle == eDotNetBarStyle.Metro && ((ScrollableControl)this).DockPadding.All == 0)
		{
			clientRectangle.Y += StiScale2.I3;
			clientRectangle.Height -= StiScale2.I6;
		}
		return clientRectangle;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void LayoutRibbons()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		int i = StiScale2.I2;
		Rectangle layoutRectangle = GetLayoutRectangle();
		int num = -i;
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		ArrayList arrayList3 = new ArrayList();
		Control val = null;
		int num2 = 0;
		bool flag = false;
		RibbonBar ribbonBar = null;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val2 = item;
			if (val2 is RibbonBar ribbonBar2 && ((Control)ribbonBar2).Visible)
			{
				int width = ribbonBar2.GetContentBasedSize().Width;
				num += width + i;
				if (ribbonBar2.GalleryStretch != null && ribbonBar2.GalleryStretch.Visible)
				{
					ribbonBar = ribbonBar2;
				}
				if (ribbonBar2.ResizeOrderIndex != 0)
				{
					flag = true;
				}
				arrayList.Add(ribbonBar2);
				arrayList2.Add(ribbonBar2);
				if (ribbonBar2.OverflowState)
				{
					arrayList3.Add(ribbonBar2);
				}
				if (((Control)ribbonBar2).Left > num2)
				{
					num2 = ((Control)ribbonBar2).Left;
					val = val2;
				}
			}
		}
		arrayList2.Sort(new XPositionComparer());
		if (flag)
		{
			arrayList.Sort(new ResizeOrderComparer());
			arrayList3.Sort(new ResizeOrderComparer());
		}
		else
		{
			arrayList.Sort(new XPositionComparer());
			arrayList3.Sort(new XPositionComparer());
		}
		int lastControlIncrease = 0;
		if (num > layoutRectangle.Width)
		{
			int num3 = num - layoutRectangle.Width;
			if (ribbonBar != null && !ribbonBar.GalleryStretch.IsAtMinimumSize)
			{
				int width2 = ribbonBar.GetContentBasedSize().Width;
				if (ribbonBar.GalleryStretch.WidthInternal - ribbonBar.GalleryStretch.MinimumSize.Width >= num3)
				{
					((Control)ribbonBar).Size = new Size(width2 - num3, layoutRectangle.Height);
				}
				else
				{
					((Control)ribbonBar).Size = new Size(width2 - (ribbonBar.GalleryStretch.WidthInternal - ribbonBar.GalleryStretch.MinimumSize.Width), layoutRectangle.Height);
				}
				num3 -= width2 - ribbonBar.GetContentBasedSize().Width;
			}
			if (num3 > 0)
			{
				int num4 = arrayList.Count - 1;
				for (int num5 = num4; num5 >= 0; num5--)
				{
					RibbonBar ribbonBar3 = arrayList[num5] as RibbonBar;
					int width3 = ribbonBar3.GetContentBasedSize().Width;
					if (width3 > num3 && ribbonBar3.AutoSizeItems)
					{
						((Control)ribbonBar3).Size = new Size(width3 - num3, layoutRectangle.Height);
					}
					else
					{
						((Control)ribbonBar3).Size = new Size(1, layoutRectangle.Height);
					}
					num3 -= width3 - ribbonBar3.GetContentBasedSize().Width;
					if (num3 <= 0)
					{
						break;
					}
				}
			}
			if (num3 != 0)
			{
				lastControlIncrease = Math.Abs(num3);
			}
		}
		else if (num < layoutRectangle.Width)
		{
			int num6 = layoutRectangle.Width - num;
			ArrayList arrayList4 = new ArrayList();
			arrayList4.AddRange(arrayList3.ToArray());
			foreach (RibbonBar item2 in arrayList4)
			{
				int width4 = item2.GetContentBasedSize().Width;
				((Control)item2).Size = new Size(width4 + num6, layoutRectangle.Height);
				num6 -= item2.GetContentBasedSize().Width - width4;
				if (num6 >= 0 && !item2.OverflowState)
				{
					arrayList3.Remove(item2);
				}
				if (num6 <= 0 || item2.GetContentBasedSize().Width - width4 == 0)
				{
					break;
				}
			}
			if (num6 > 0 && arrayList3.Count == 0)
			{
				int num7 = arrayList.Count - 1;
				for (int num8 = num7; num8 >= 0; num8--)
				{
					RibbonBar ribbonBar5 = arrayList[num8] as RibbonBar;
					if (!ribbonBar5.OverflowState && !arrayList4.Contains(ribbonBar5))
					{
						int width5 = ribbonBar5.GetContentBasedSize().Width;
						if (width5 != ((Control)ribbonBar5).Width || ((Control)ribbonBar5).Height != layoutRectangle.Height || !ribbonBar5.LastReducedSize.IsEmpty || !ribbonBar5.BeforeOverflowSize.IsEmpty || ribbonBar5.GalleryStretch != null)
						{
							if (((Control)ribbonBar5).Width == width5 + num6)
							{
								if (ribbonBar5 != val || !m_StretchLastRibbonBar)
								{
									num6 = 0;
								}
								break;
							}
							((Control)ribbonBar5).Size = new Size(width5 + num6, layoutRectangle.Height);
							num6 -= ribbonBar5.GetContentBasedSize().Width - width5;
							if (num6 <= 0)
							{
								break;
							}
						}
					}
				}
			}
			if (num6 > 0)
			{
				lastControlIncrease = num6;
			}
		}
		RepositionRibbons(arrayList2, layoutRectangle, i, val, lastControlIncrease);
	}

	private void RepositionRibbons(ArrayList layoutOrder, Rectangle layoutRectangle, int spacing, Control lastVisibleControl, int lastControlIncrease)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		Point location = layoutRectangle.Location;
		int num = 0;
		if ((int)((Control)this).RightToLeft == 1)
		{
			int num2 = 0;
			foreach (RibbonBar item in layoutOrder)
			{
				num2 += item.GetContentBasedSize().Width + spacing;
			}
			if (num2 < layoutRectangle.Width)
			{
				num = spacing + layoutRectangle.Width - num2;
			}
		}
		Rectangle rectangle = Rectangle.Empty;
		location.X += m_ScrollOffset;
		foreach (RibbonBar item2 in layoutOrder)
		{
			Size contentBasedSize = item2.GetContentBasedSize();
			if (item2 == lastVisibleControl && m_StretchLastRibbonBar && !item2.OverflowState)
			{
				contentBasedSize.Width += lastControlIncrease;
			}
			Rectangle rectangle2 = new Rectangle(location.X + num, location.Y, contentBasedSize.Width, layoutRectangle.Height);
			if (((Component)(object)this).DesignMode)
			{
				TypeDescriptor.GetProperties(item2)["Bounds"].SetValue(item2, rectangle2);
			}
			else
			{
				((Control)item2).Bounds = rectangle2;
			}
			rectangle = Rectangle.Union(rectangle2, rectangle);
			location.X += ((Control)item2).Width + spacing;
		}
		m_ViewBounds = rectangle;
		if (m_ViewBounds.Right > ((Control)this).Width)
		{
			if (m_ButtonScrollRight == null)
			{
				m_ButtonScrollRight = new ButtonX();
				((Control)m_ButtonScrollRight).Text = "<expand direction=\"right\"/>";
				m_ButtonScrollRight.ColorTable = eButtonColor.OrangeWithBackground;
				((Control)m_ButtonScrollRight).Click += ScrollPanelRightClick;
			}
			((Control)m_ButtonScrollRight).Bounds = new Rectangle(((Control)this).Width - 12, 0, 12, ((Control)this).Height - 2);
			((Control)this).Controls.Add((Control)(object)m_ButtonScrollRight);
			((Control)m_ButtonScrollRight).BringToFront();
		}
		else if (m_ButtonScrollRight != null)
		{
			((Control)m_ButtonScrollRight).Visible = false;
			((Control)this).Controls.Remove((Control)(object)m_ButtonScrollRight);
			((Component)(object)m_ButtonScrollRight).Dispose();
			m_ButtonScrollRight = null;
		}
		if (m_ScrollOffset < 0)
		{
			if (m_ButtonScrollLeft == null)
			{
				m_ButtonScrollLeft = new ButtonX();
				((Control)m_ButtonScrollLeft).Text = "<expand direction=\"left\"/>";
				m_ButtonScrollLeft.ColorTable = eButtonColor.OrangeWithBackground;
				((Control)m_ButtonScrollLeft).Click += ScrollPanelLeftClick;
			}
			((Control)m_ButtonScrollLeft).Bounds = new Rectangle(0, 0, 12, ((Control)this).Height - 2);
			((Control)this).Controls.Add((Control)(object)m_ButtonScrollLeft);
			((Control)m_ButtonScrollLeft).BringToFront();
		}
		else if (m_ButtonScrollLeft != null)
		{
			((Control)m_ButtonScrollLeft).Visible = false;
			((Control)this).Controls.Remove((Control)(object)m_ButtonScrollLeft);
			((Component)(object)m_ButtonScrollLeft).Dispose();
			m_ButtonScrollLeft = null;
		}
	}

	private void ScrollPanelRightClick(object sender, EventArgs e)
	{
		ScrollRight();
	}

	public void ScrollRight()
	{
		if (m_ScrollOffset + m_ViewBounds.Width > ((Control)this).Width)
		{
			SetScrollOffset(-Math.Min(((Control)this).Width, m_ViewBounds.Right - ((Control)this).Width + 2));
		}
	}

	private void ScrollPanelLeftClick(object sender, EventArgs e)
	{
		ScrollLeft();
	}

	public void ScrollLeft()
	{
		SetScrollOffset(Math.Min(Math.Abs(m_ScrollOffset), ((Control)this).Width));
	}

	public void ResetScrollPosition()
	{
		SetScrollOffset(-m_ScrollOffset);
	}

	private void SetScrollOffset(int offset)
	{
		m_ScrollOffset += offset;
		((Control)this).PerformLayout();
	}

	private Rectangle GetThemedRect(Rectangle r)
	{
		r.Y -= 6;
		r.Height += 6;
		return r;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		bool flag = true;
		if (DrawThemedPane && BarFunctions.ThemedOS)
		{
			Rectangle themedRect = GetThemedRect(((Control)this).ClientRectangle);
			eTabStripAlignment eTabStripAlignment2 = eTabStripAlignment.Top;
			Rectangle r = new Rectangle(0, 0, themedRect.Width, themedRect.Height);
			if (eTabStripAlignment2 == eTabStripAlignment.Right || eTabStripAlignment2 == eTabStripAlignment.Left)
			{
				r = new Rectangle(0, 0, r.Height, r.Width);
			}
			if (m_ThemeCachedBitmap == null || ((Image)m_ThemeCachedBitmap).Size != r.Size)
			{
				DisposeThemeCachedBitmap();
				Bitmap val = new Bitmap(r.Width, r.Height, e.Graphics);
				try
				{
					Graphics val2 = Graphics.FromImage((Image)(object)val);
					try
					{
						SolidBrush val3 = new SolidBrush(Color.Transparent);
						try
						{
							val2.FillRectangle((Brush)(object)val3, 0, 0, ((Image)val).Width, ((Image)val).Height);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
						base.ThemeTab.DrawBackground(val2, ThemeTabParts.Pane, ThemeTabStates.Normal, r);
					}
					finally
					{
						val2.Dispose();
					}
				}
				finally
				{
					switch (eTabStripAlignment2)
					{
					case eTabStripAlignment.Left:
						((Image)val).RotateFlip((RotateFlipType)3);
						break;
					case eTabStripAlignment.Right:
						((Image)val).RotateFlip((RotateFlipType)1);
						break;
					case eTabStripAlignment.Bottom:
						((Image)val).RotateFlip((RotateFlipType)2);
						break;
					}
					e.Graphics.DrawImageUnscaled((Image)(object)val, themedRect.X, themedRect.Y);
					m_ThemeCachedBitmap = val;
				}
			}
			else
			{
				e.Graphics.DrawImageUnscaled((Image)(object)m_ThemeCachedBitmap, themedRect.X, themedRect.Y);
			}
			flag = false;
		}
		if (flag)
		{
			base.OnPaint(e);
		}
		if (((Component)(object)this).DesignMode && ((ArrangedElementCollection)((Control)this).Controls).Count == 0 && ((Control)this).Text == "")
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(-2, -2);
			StringFormat val4 = BarFunctions.CreateStringFormat();
			val4.Alignment = (StringAlignment)1;
			val4.LineAlignment = (StringAlignment)1;
			val4.Trimming = (StringTrimming)3;
			Font val5 = new Font(((Control)this).Font, (FontStyle)1);
			e.Graphics.DrawString("Drop RibbonBar or other controls here. Drag and Drop tabs and items to re-order.", val5, (Brush)new SolidBrush(ControlPaint.Dark(Style.BackColor)), (RectangleF)clientRectangle, val4);
			val5.Dispose();
			val4.Dispose();
		}
		if (((Control)this).Parent is RibbonControl)
		{
			((RibbonControl)(object)((Control)this).Parent).RibbonStrip.InvalidateKeyTipsCanvas();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		m_ScrollOffset = 0;
		DisposeThemeCachedBitmap();
		base.OnResize(e);
	}

	bool IKeyTipsControl.ProcessMnemonicEx(char charCode)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
		{
			return false;
		}
		Control[] array = (Control[])(object)new Control[((ArrangedElementCollection)((Control)this).Controls).Count];
		((ArrangedElementCollection)((Control)this).Controls).CopyTo((Array)array, 0);
		ArrayList arrayList = new ArrayList(array);
		arrayList.Sort(new XPositionComparer());
		foreach (Control item in arrayList)
		{
			Control val = item;
			if (val is IKeyTipsControl keyTipsControl && val.Visible && val.Enabled)
			{
				string keyTipsKeysStack = keyTipsControl.KeyTipsKeysStack;
				if (keyTipsControl.ProcessMnemonicEx(charCode))
				{
					return true;
				}
				if (keyTipsControl.KeyTipsKeysStack != keyTipsKeysStack)
				{
					((IKeyTipsControl)this).KeyTipsKeysStack = keyTipsControl.KeyTipsKeysStack;
					return false;
				}
			}
		}
		return false;
	}
}
