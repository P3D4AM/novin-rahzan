using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ComVisible(false)]
public class RibbonStrip : ItemControl
{
	private const int Office2010GlassExcludeTopPart = 29;

	private RibbonStripContainerItem m_StripContainer;

	private RibbonTabItemGroupCollection m_Groups;

	private int m_TabGroupHeight = StiScale2.YYI(14.0);

	private bool m_TabGroupsVisible;

	private Font m_DefaultGroupFont;

	private Font m_DefaultGroupFontAuto;

	private int m_CaptionHeight;

	private bool m_CaptionVisible;

	private bool m_AutoExpand = true;

	private int m_RibbonStripIndent = StiScale2.YYI(46.0);

	private Rectangle m_CaptionBounds = Rectangle.Empty;

	private Rectangle m_QuickToolbarBounds = Rectangle.Empty;

	private Rectangle m_SystemCaptionItemBounds = Rectangle.Empty;

	private Rectangle[] m_CaptionTextBounds;

	private Font m_CaptionFont;

	private bool m_CanCustomize = true;

	private ElementStyle m_DefaultBackgroundStyle = new ElementStyle();

	private bool m_KeyTipsEnabled = true;

	private string m_TitleText = "";

	private bool m_CanSupportGlass;

	private BodyElement m_TitleTextMarkup;

	internal Rectangle TitleTextMarkupLastArrangeBounds = Rectangle.Empty;

	private bool _TabGroupPaintSuspended;

	private bool m_AutoKeyboardExpand = true;

	private bool m_AltFocus;

	private IKeyTipsControl m_AltFocusedControl;

	private bool _IgnoreF10Key;

	private bool m_MouseDownOnCaption;

	public static int OffsetCaptionContainer { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CanSupportGlass
	{
		get
		{
			return m_CanSupportGlass;
		}
		set
		{
			m_CanSupportGlass = value;
		}
	}

	internal override bool IsGlassEnabled
	{
		get
		{
			bool isGlassEnabled = base.IsGlassEnabled;
			if (!m_CanSupportGlass || !isGlassEnabled)
			{
				return false;
			}
			Form val = ((Control)this).FindForm();
			if (val is OfficeForm)
			{
				return false;
			}
			if (val is RibbonForm)
			{
				return ((RibbonForm)(object)val).IsGlassEnabled;
			}
			return isGlassEnabled;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Category("Appearance")]
	[Description("Indicates text displayed on Ribbon Title instead of the Form.Text property.")]
	public string TitleText
	{
		get
		{
			return m_TitleText;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			m_TitleText = value;
			m_TitleTextMarkup = null;
			if (MarkupParser.IsMarkup(ref m_TitleText))
			{
				m_TitleTextMarkup = MarkupParser.Parse(m_TitleText);
				if (m_TitleTextMarkup != null)
				{
					m_TitleTextMarkup.HyperLinkClick += InternalTitleTextMarkupLinkClick;
				}
				TitleTextMarkupLastArrangeBounds = Rectangle.Empty;
				((Control)this).Invalidate();
			}
		}
	}

	internal BodyElement TitleTextMarkupBody => m_TitleTextMarkup;

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether KeyTips functionality is enabled.")]
	public bool KeyTipsEnabled
	{
		get
		{
			return m_KeyTipsEnabled;
		}
		set
		{
			m_KeyTipsEnabled = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Customization")]
	[Description("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled.")]
	public bool CanCustomize
	{
		get
		{
			return m_CanCustomize;
		}
		set
		{
			m_CanCustomize = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Indicates explicit height of the caption provided by control")]
	public int CaptionHeight
	{
		get
		{
			return m_CaptionHeight;
		}
		set
		{
			m_CaptionHeight = value;
			m_StripContainer.NeedRecalcSize = true;
		}
	}

	internal bool HasVisibleTabs
	{
		get
		{
			foreach (BaseItem item in Items)
			{
				if (item.Visible)
				{
					return true;
				}
			}
			return false;
		}
	}

	internal Rectangle[] CaptionTextBounds
	{
		get
		{
			return m_CaptionTextBounds;
		}
		set
		{
			m_CaptionTextBounds = value;
		}
	}

	internal Rectangle CaptionBounds
	{
		get
		{
			return m_CaptionBounds;
		}
		set
		{
			m_CaptionBounds = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle QuickToolbarBounds
	{
		get
		{
			return m_QuickToolbarBounds;
		}
		set
		{
			m_QuickToolbarBounds = value;
		}
	}

	internal Rectangle SystemCaptionItemBounds
	{
		get
		{
			return m_SystemCaptionItemBounds;
		}
		set
		{
			m_SystemCaptionItemBounds = value;
		}
	}

	public SystemCaptionItem SystemCaptionItem => m_StripContainer.SystemCaptionItem;

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether custom caption line provided by the control is visible.")]
	public bool CaptionVisible
	{
		get
		{
			return m_CaptionVisible;
		}
		set
		{
			m_CaptionVisible = value;
			OnCaptionVisibleChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("")]
	public Font CaptionFont
	{
		get
		{
			return m_CaptionFont;
		}
		set
		{
			m_CaptionFont = value;
			m_StripContainer.NeedRecalcSize = true;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(46)]
	[Category("Layout")]
	[Description("Indicates indent of the ribbon strip.")]
	public int RibbonStripIndent
	{
		get
		{
			return m_RibbonStripIndent;
		}
		set
		{
			m_RibbonStripIndent = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(14)]
	[Category("Tab Groups")]
	[Description("Indicates height in pixels of tab group line that is displayed above the RibbonTabItem objects that have group assigned.")]
	public int TabGroupHeight
	{
		get
		{
			return m_TabGroupHeight;
		}
		set
		{
			m_TabGroupHeight = value;
			m_StripContainer.NeedRecalcSize = true;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Category("Tab Groups")]
	[Description("Indicates height in pixels of tab group line that is displayed above the RibbonTabItem objects that have group assigned.")]
	public bool TabGroupsVisible
	{
		get
		{
			return m_TabGroupsVisible;
		}
		set
		{
			m_TabGroupsVisible = value;
			m_StripContainer.NeedRecalcSize = true;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.RibbonTabItemGroupCollectionEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Tab Groups")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public RibbonTabItemGroupCollection TabGroups => m_Groups;

	internal eDotNetBarStyle EffectiveStyle => m_StripContainer.EffectiveStyle;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	public eDotNetBarStyle Style
	{
		get
		{
			return m_StripContainer.Style;
		}
		set
		{
			base.ColorScheme.Style = value;
			m_StripContainer.Style = value;
			((Control)this).Invalidate();
			RecalcLayout();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => m_StripContainer.RibbonStripContainer.SubItems;

	[Browsable(false)]
	public RibbonTabItem SelectedRibbonTabItem
	{
		get
		{
			foreach (BaseItem item in Items)
			{
				if (item is RibbonTabItem && ((RibbonTabItem)item).Checked)
				{
					return (RibbonTabItem)item;
				}
			}
			return null;
		}
	}

	internal bool IsMaximized
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				return (int)val.WindowState == 2;
			}
			return false;
		}
	}

	internal bool TabGroupPaintSuspended
	{
		get
		{
			return _TabGroupPaintSuspended;
		}
		set
		{
			if (_TabGroupPaintSuspended != value)
			{
				_TabGroupPaintSuspended = value;
				((Control)this).Invalidate();
				Form val = ((Control)this).FindForm();
				if (val != null)
				{
					((Control)val).Invalidate();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Tab Groups")]
	[DefaultValue(null)]
	public Font DefaultGroupFont
	{
		get
		{
			return m_DefaultGroupFont;
		}
		set
		{
			m_DefaultGroupFont = value;
			if (((Component)(object)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	internal bool AutoExpand
	{
		get
		{
			return m_AutoExpand;
		}
		set
		{
			m_AutoExpand = value;
		}
	}

	internal bool AutoKeyboardExpand
	{
		get
		{
			return m_AutoKeyboardExpand;
		}
		set
		{
			m_AutoKeyboardExpand = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(false)]
	public bool IgnoreF10Key
	{
		get
		{
			return _IgnoreF10Key;
		}
		set
		{
			_IgnoreF10Key = value;
		}
	}

	[Browsable(false)]
	public bool IsInKeyTipsMode => m_AltFocus | ShowKeyTips;

	internal bool MouseDownOnCaption => m_MouseDownOnCaption;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection QuickToolbarItems => m_StripContainer.CaptionContainer.SubItems;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public GenericItemContainer CaptionContainerItem => m_StripContainer.CaptionContainer;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public GenericItemContainer StripContainerItem => m_StripContainer.RibbonStripContainer;

	[Description("Occurs when text markup link from TitleText markup is clicked.")]
	public event MarkupLinkClickEventHandler TitleTextMarkupLinkClick;

	public RibbonStrip()
	{
		((Control)this).SetStyle((ControlStyles)4096, true);
		m_StripContainer = new RibbonStripContainerItem(this);
		m_StripContainer.GlobalItem = false;
		m_StripContainer.ContainerControl = this;
		m_StripContainer.Displayed = true;
		m_StripContainer.SetOwner(this);
		m_StripContainer.Style = eDotNetBarStyle.Office2003;
		SetBaseItemContainer(m_StripContainer);
		base.ColorScheme.Style = eDotNetBarStyle.Office2003;
		((Control)this).AutoSize = true;
		m_Groups = new RibbonTabItemGroupCollection();
		m_Groups.Owner = this;
		m_StripContainer.SystemCaptionItem.Click += SystemCaptionClick;
	}

	private void InternalTitleTextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is HyperLink hyperLink && this.TitleTextMarkupLinkClick != null)
		{
			this.TitleTextMarkupLinkClick(this, new MarkupLinkClickEventArgs(hyperLink.Name, hyperLink.HRef));
		}
	}

	private void OnCaptionVisibleChanged()
	{
		m_StripContainer.NeedRecalcSize = true;
		RecalcLayout();
	}

	internal override ItemPaintArgs GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = base.GetItemPaintArgs(g);
		if (((Control)this).Parent is RibbonControl ribbonControl)
		{
			itemPaintArgs.ControlExpanded = ribbonControl.Expanded || ribbonControl.IsPopupMode;
		}
		return itemPaintArgs;
	}

	protected override void WndProc(ref Message m)
	{
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Invalid comparison between Unknown and I4
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Invalid comparison between Unknown and I4
		if (((Message)(ref m)).Msg == 132)
		{
			int x = WinApi.LOWORD(((Message)(ref m)).LParam);
			int y = WinApi.HIWORD(((Message)(ref m)).LParam);
			Point pt = ((Control)this).PointToClient(new Point(x, y));
			if (IsGlassEnabled && CaptionVisible)
			{
				Rectangle rectangle = new Rectangle(((Control)this).Width - SystemInformation.CaptionButtonSize.Width * 3, 0, SystemInformation.CaptionButtonSize.Width * 3, SystemInformation.CaptionButtonSize.Height + 6);
				if (rectangle.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (new Rectangle(0, 0, ((Control)this).Width, 4).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (BarFunctions.IsWindows7 && IsMaximized)
				{
					Rectangle[] captionTextBounds = m_CaptionTextBounds;
					if (captionTextBounds != null)
					{
						Rectangle[] array = captionTextBounds;
						foreach (Rectangle rectangle2 in array)
						{
							if (rectangle2.Contains(pt))
							{
								((Message)(ref m)).Result = new IntPtr(-1);
								return;
							}
						}
					}
				}
			}
			else if (CaptionVisible && !IsMaximized && new Rectangle(0, 0, ((Control)this).Width, 4).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if (m_CaptionVisible && (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Metro) && ((pt.X < 28 - RibbonForm.OffsetHitZone && (int)((Control)this).RightToLeft == 0) || (pt.X > ((Control)this).Width - 28 - RibbonForm.OffsetHitZone && (int)((Control)this).RightToLeft == 1)) && pt.Y < 28)
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
			if (m_CaptionVisible && effectiveStyle == eDotNetBarStyle.Windows7 && ((pt.X < 28 - RibbonForm.OffsetHitZone && (int)((Control)this).RightToLeft == 0) || (pt.X > ((Control)this).Width - 28 - RibbonForm.OffsetHitZone && (int)((Control)this).RightToLeft == 1)) && pt.Y < 28)
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
		}
		base.WndProc(ref m);
	}

	protected override void ClearBackground(ItemPaintArgs pa)
	{
		if (IsGlassEnabled)
		{
			if (((Control)this).FindForm() is RibbonForm ribbonForm)
			{
				pa.Graphics.Clear(Color.Transparent);
				pa.Graphics.SetClip(new Rectangle(0, 0, ((Control)this).Width, ribbonForm.GlassHeight - 1), (CombineMode)4);
			}
			base.ClearBackground(pa);
			pa.Graphics.ResetClip();
		}
		else
		{
			base.ClearBackground(pa);
		}
	}

	protected override Rectangle GetPaintControlBackgroundRectangle()
	{
		if (IsGlassEnabled && (EffectiveStyle == eDotNetBarStyle.Office2010 || EffectiveStyle == eDotNetBarStyle.Metro))
		{
			Rectangle paintControlBackgroundRectangle = base.GetPaintControlBackgroundRectangle();
			paintControlBackgroundRectangle.Y += 29;
			paintControlBackgroundRectangle.Height -= 29;
			return paintControlBackgroundRectangle;
		}
		return base.GetPaintControlBackgroundRectangle();
	}

	protected override void PaintControlBackground(ItemPaintArgs pa)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		bool flag = false;
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		bool isGlassEnabled = IsGlassEnabled;
		if (isGlassEnabled && ((Control)this).FindForm() is RibbonForm ribbonForm)
		{
			pa.Graphics.SetClip(new Rectangle(0, 0, ((Control)this).Width, ribbonForm.GlassHeight - (((effectiveStyle != eDotNetBarStyle.Office2010 && effectiveStyle != eDotNetBarStyle.Metro) || !pa.GlassEnabled) ? 1 : 29)), (CombineMode)4);
			flag = true;
		}
		if (isGlassEnabled && (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Metro))
		{
			ElementStyle backgroundStyle = GetBackgroundStyle();
			if (backgroundStyle != null)
			{
				Rectangle rectangle = new Rectangle(0, 27, ((Control)this).Width, ((Control)this).Height + 8);
				GraphicsPath val = new GraphicsPath();
				try
				{
					val.AddEllipse(rectangle);
					PathGradientBrush val2 = new PathGradientBrush(val);
					try
					{
						val2.CenterColor = backgroundStyle.BackColor;
						val2.SurroundColors = new Color[1] { backgroundStyle.BackColor2 };
						val2.CenterPoint = new PointF(rectangle.Width / 2, rectangle.Bottom - 14);
						Blend val3 = new Blend();
						val3.Factors = new float[3] { 0f, 0.85f, 1f };
						val3.Positions = new float[3] { 0f, 0.65f, 1f };
						val2.Blend = val3;
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
		}
		else
		{
			base.PaintControlBackground(pa);
		}
		if (flag)
		{
			pa.Graphics.ResetClip();
		}
		m_QuickToolbarBounds = Rectangle.Empty;
		m_CaptionBounds = Rectangle.Empty;
		m_SystemCaptionItemBounds = Rectangle.Empty;
		BaseRenderer renderer = GetRenderer();
		if (renderer != null && ((Control)this).Parent is RibbonControl)
		{
			RibbonControlRendererEventArgs ribbonControlRendererEventArgs = new RibbonControlRendererEventArgs(pa.Graphics, ((Control)this).Parent as RibbonControl, pa.GlassEnabled);
			ribbonControlRendererEventArgs.ItemPaintArgs = pa;
			renderer.DrawRibbonControlBackground(ribbonControlRendererEventArgs);
			if (m_CaptionVisible)
			{
				renderer.DrawQuickAccessToolbarBackground(new RibbonControlRendererEventArgs(pa.Graphics, ((Control)this).Parent as RibbonControl, pa.GlassEnabled));
			}
		}
		if (m_TabGroupsVisible)
		{
			PaintTabGroups(pa);
		}
		if (renderer != null && m_CaptionVisible)
		{
			RibbonControlRendererEventArgs ribbonControlRendererEventArgs2 = new RibbonControlRendererEventArgs(pa.Graphics, ((Control)this).Parent as RibbonControl, pa.GlassEnabled);
			ribbonControlRendererEventArgs2.ItemPaintArgs = pa;
			renderer.DrawRibbonFormCaptionText(ribbonControlRendererEventArgs2);
		}
	}

	internal void PaintTabGroups(ItemPaintArgs pa)
	{
		if (m_Groups.Count == 0 || _TabGroupPaintSuspended)
		{
			return;
		}
		RibbonTabItemGroup ribbonTabItemGroup = null;
		Rectangle rect = Rectangle.Empty;
		Rectangle groupBounds = Rectangle.Empty;
		int num = 0;
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if (m_CaptionVisible)
		{
			num = ((effectiveStyle != eDotNetBarStyle.Office2010 && effectiveStyle != eDotNetBarStyle.Metro) ? (num + ((pa.GlassEnabled && effectiveStyle == eDotNetBarStyle.Windows7) ? 4 : 3)) : (num + 1));
		}
		foreach (RibbonTabItemGroup tabGroup in TabGroups)
		{
			tabGroup.DisplayPositions.Clear();
		}
		foreach (BaseItem item in Items)
		{
			if (item is RibbonTabItem)
			{
				RibbonTabItem ribbonTabItem = item as RibbonTabItem;
				if (ribbonTabItem.Group != ribbonTabItemGroup)
				{
					if (ribbonTabItem.Visible)
					{
						if (ribbonTabItemGroup != null)
						{
							PaintGroup(pa, ribbonTabItemGroup, rect, groupBounds);
						}
						ribbonTabItemGroup = ribbonTabItem.Group;
						if (ribbonTabItemGroup != null)
						{
							ribbonTabItemGroup.DisplayPositions.Clear();
							rect = new Rectangle(ribbonTabItem.DisplayRectangle.X, num, ribbonTabItem.DisplayRectangle.Width, (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Metro) ? ribbonTabItem.DisplayRectangle.Y : (ribbonTabItem.DisplayRectangle.Y - 4));
							groupBounds = new Rectangle(ribbonTabItem.DisplayRectangle.X, num, ribbonTabItem.DisplayRectangle.Width, ribbonTabItem.DisplayRectangle.Bottom - ((effectiveStyle == eDotNetBarStyle.Metro) ? 1 : m_TabGroupHeight));
						}
					}
				}
				else if (ribbonTabItemGroup != null)
				{
					if (ribbonTabItem.Visible)
					{
						rect.Width += ribbonTabItem.DisplayRectangle.Right - rect.Right;
						groupBounds.Width += ribbonTabItem.DisplayRectangle.Right - groupBounds.Right;
					}
				}
				else
				{
					rect = Rectangle.Empty;
					groupBounds = Rectangle.Empty;
				}
			}
			else if (ribbonTabItemGroup != null)
			{
				PaintGroup(pa, ribbonTabItemGroup, rect, groupBounds);
			}
		}
		if (ribbonTabItemGroup != null)
		{
			PaintGroup(pa, ribbonTabItemGroup, rect, groupBounds);
		}
	}

	private void PaintGroup(ItemPaintArgs pa, RibbonTabItemGroup group, Rectangle rect, Rectangle groupBounds)
	{
		if ((!m_CaptionVisible || m_QuickToolbarBounds.IsEmpty || !rect.IntersectsWith(m_QuickToolbarBounds)) && !m_SystemCaptionItemBounds.IntersectsWith(groupBounds))
		{
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Metro)
			{
				group.DisplayPositions.Add(new Rectangle(rect.X, rect.Y, rect.Width + 1, rect.Height));
			}
			else
			{
				group.DisplayPositions.Add(rect);
			}
			if (BarFunctions.IsOffice2007Style(effectiveStyle))
			{
				RibbonTabGroupRendererEventArgs e = new RibbonTabGroupRendererEventArgs(pa.Graphics, group, rect, groupBounds, GetDefaultGroupFont(), pa, effectiveStyle);
				BaseRenderer renderer = GetRenderer();
				renderer.DrawRibbonTabGroup(e);
			}
			else
			{
				ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(group.Style, pa.Graphics, rect);
				ElementStyleDisplay.Paint(e2);
				ElementStyleDisplay.PaintText(e2, group.GroupTitle, GetDefaultGroupFont());
			}
		}
	}

	private Font GetDefaultGroupFont()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (m_DefaultGroupFont == null)
		{
			try
			{
				if (m_DefaultGroupFontAuto == null)
				{
					m_DefaultGroupFontAuto = new Font(((Control)this).Font.Name, ((Control)this).Font.SizeInPoints - 1f);
				}
			}
			catch
			{
				return ((Control)this).Font;
			}
			return m_DefaultGroupFontAuto;
		}
		return m_DefaultGroupFont;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDefaultGroupFont()
	{
		TypeDescriptor.GetProperties(this)["DefaultGroupFont"].SetValue(this, null);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		base.OnFontChanged(e);
		m_DefaultGroupFont = null;
	}

	protected override ElementStyle GetBackgroundStyle()
	{
		if (base.BackgroundStyle.Custom)
		{
			return base.GetBackgroundStyle();
		}
		return m_DefaultBackgroundStyle;
	}

	internal void InitDefaultStyles()
	{
		m_DefaultBackgroundStyle.SetColorScheme(GetColorScheme());
		if (((Control)this).Parent is RibbonControl)
		{
			RibbonPredefinedColorSchemes.ApplyRibbonElementStyle(m_DefaultBackgroundStyle, (RibbonControl)(object)((Control)this).Parent);
		}
	}

	internal ElementStyle InternalGetBackgroundStyle()
	{
		return GetBackgroundStyle();
	}

	internal Rectangle GetItemContainerBounds()
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (m_TabGroupsVisible)
		{
			itemContainerRectangle.Y += m_TabGroupHeight + 1;
		}
		if (m_CaptionVisible)
		{
			itemContainerRectangle.Y += GetAbsoluteCaptionHeight();
		}
		if (m_CaptionVisible && m_RibbonStripIndent > 0)
		{
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if (effectiveStyle != eDotNetBarStyle.Office2010 && effectiveStyle != eDotNetBarStyle.Windows7 && effectiveStyle != eDotNetBarStyle.Metro)
			{
				if ((int)((Control)this).RightToLeft == 0)
				{
					itemContainerRectangle.X += m_RibbonStripIndent;
				}
				itemContainerRectangle.Width -= m_RibbonStripIndent;
			}
		}
		return itemContainerRectangle;
	}

	internal Rectangle GetCaptionContainerBounds()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (IsGlassEnabled)
		{
			itemContainerRectangle.Y += 3;
		}
		return new Rectangle(itemContainerRectangle.X + OffsetCaptionContainer, itemContainerRectangle.Y, itemContainerRectangle.Width - OffsetCaptionContainer, GetAbsoluteCaptionHeight());
	}

	private int GetCaptionHeight()
	{
		if (m_CaptionHeight == 0)
		{
			int num = StiScale2.YYI(16.0);
			if (IsGlassEnabled && EffectiveStyle != eDotNetBarStyle.Office2010 && EffectiveStyle != eDotNetBarStyle.Windows7 && EffectiveStyle != eDotNetBarStyle.Metro)
			{
				num = num;
			}
			else if (!IsGlassEnabled && EffectiveStyle == eDotNetBarStyle.Office2010)
			{
				num -= 2;
			}
			else if (!IsGlassEnabled && EffectiveStyle == eDotNetBarStyle.Windows7)
			{
				num -= 2;
			}
			else if (!IsGlassEnabled && EffectiveStyle == eDotNetBarStyle.Metro)
			{
				num -= 2;
			}
			return num;
		}
		return m_CaptionHeight;
	}

	internal int GetAbsoluteCaptionHeight()
	{
		return GetCaptionHeight() + ((!m_TabGroupsVisible) ? m_TabGroupHeight : 0);
	}

	internal int GetTotalCaptionHeight()
	{
		return GetCaptionHeight() + m_TabGroupHeight + ((EffectiveStyle == eDotNetBarStyle.Office2010 || EffectiveStyle == eDotNetBarStyle.Windows7 || EffectiveStyle == eDotNetBarStyle.Metro) ? 0 : 0);
	}

	internal int GetGlassCaptionHeight()
	{
		if (EffectiveStyle == eDotNetBarStyle.Office2010 || EffectiveStyle == eDotNetBarStyle.Metro)
		{
			return ((Control)this).Height;
		}
		return GetTotalCaptionHeight();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		InitDefaultStyles();
		base.OnPaint(e);
	}

	protected override void RecalcSize()
	{
		InitDefaultStyles();
		base.RecalcSize();
	}

	public override int GetAutoSizeHeight()
	{
		int num = base.GetAutoSizeHeight();
		if (BarFunctions.IsOffice2007Style(EffectiveStyle))
		{
			BaseRenderer renderer = GetRenderer();
			if (renderer is Office2007Renderer)
			{
				num += ((Office2007Renderer)renderer).ColorTable.RibbonControl.CornerSize;
			}
		}
		return num;
	}

	protected override bool OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		displayRectangle.Location = ((Control)this).PointToScreen(displayRectangle.Location);
		RibbonControl ribbonControl = ((Control)this).Parent as RibbonControl;
		if (((Control)this).Parent is RibbonControl)
		{
			displayRectangle = ((Control)this).Parent.DisplayRectangle;
			displayRectangle.Location = ((Control)this).Parent.PointToScreen(displayRectangle.Location);
			if (ribbonControl.SelectedRibbonTabItem != null && ribbonControl.SelectedRibbonTabItem.Panel != null && ((IKeyTipsControl)ribbonControl.SelectedRibbonTabItem.Panel).ShowKeyTips)
			{
				return false;
			}
		}
		if (ribbonControl != null && !ribbonControl.MouseWheelTabScrollEnabled)
		{
			return false;
		}
		Point mousePosition = Control.MousePosition;
		bool flag = true;
		if (ribbonControl != null)
		{
			flag = ribbonControl.Expanded;
		}
		bool flag2 = true;
		Form val = ((Control)this).FindForm();
		if (val != null && !BarFunctions.IsFormActive(val))
		{
			flag2 = false;
		}
		if (flag2 && displayRectangle.Contains(mousePosition) && !ShowKeyTips && flag && Items.Count > 0)
		{
			IntPtr intPtr = NativeFunctions.WindowFromPoint(new NativeFunctions.POINT(mousePosition));
			Control val2 = Control.FromChildHandle(intPtr);
			if (val2 == null)
			{
				val2 = Control.FromHandle(intPtr);
			}
			if (val2 is RibbonBar || val2 is RibbonStrip || val2 is RibbonControl || val2 is RibbonPanel)
			{
				RibbonTabItem selectedRibbonTabItem = SelectedRibbonTabItem;
				int num = 0;
				int num2 = Items.Count - 1;
				int num3 = 1;
				if (wParam.ToInt64() > 0)
				{
					num3 = -1;
					num2 = 0;
				}
				if (selectedRibbonTabItem != null)
				{
					num = Items.IndexOf(selectedRibbonTabItem) + num3;
					if (num3 < 0 && num < 0)
					{
						return false;
					}
					if (num == Items.Count)
					{
						num = 0;
					}
					else if (num < 0)
					{
						num = Items.Count - 1;
					}
				}
				int num4 = num - num3;
				do
				{
					num4 += num3;
					if (num4 < 0 || num4 > Items.Count - 1)
					{
						break;
					}
					if (Items[num4] is RibbonTabItem && Items[num4].Visible)
					{
						((RibbonTabItem)Items[num4]).Checked = true;
						return true;
					}
				}
				while (num4 != num2);
			}
		}
		return false;
	}

	private ArrayList GetAllRibbonBars()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		ArrayList arrayList = new ArrayList();
		if (!(((Control)this).Parent is RibbonControl))
		{
			return arrayList;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
		{
			Control c = item;
			GetAllRibbonBars(c, arrayList);
		}
		RibbonControl ribbonControl = ((Control)this).Parent as RibbonControl;
		if (!ribbonControl.Expanded && SelectedRibbonTabItem != null && SelectedRibbonTabItem.Panel != null && ((Control)SelectedRibbonTabItem.Panel).Parent != ribbonControl)
		{
			GetAllRibbonBars((Control)(object)SelectedRibbonTabItem.Panel, arrayList);
		}
		return arrayList;
	}

	private void GetAllRibbonBars(Control c, ArrayList list)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		if (c is RibbonBar)
		{
			list.Add(c);
		}
		foreach (Control item in (ArrangedElementCollection)c.Controls)
		{
			Control c2 = item;
			GetAllRibbonBars(c2, list);
		}
	}

	public override ArrayList GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByName(GetBaseItemContainer(), ItemName, arrayList);
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.QatPositionedBelowRibbon)
		{
			BarFunctions.GetSubItemsByName(ribbonControl.QatToolbar.GetBaseItemContainer(), ItemName, arrayList);
		}
		ArrayList allRibbonBars = GetAllRibbonBars();
		foreach (RibbonBar item in allRibbonBars)
		{
			BarFunctions.GetSubItemsByName(item.GetBaseItemContainer(), ItemName, arrayList);
		}
		if (ribbonControl != null && ribbonControl.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByName(ribbonControl.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList);
		}
		return arrayList;
	}

	public override ArrayList GetItems(string ItemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(GetBaseItemContainer(), ItemName, arrayList, itemType);
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.QatPositionedBelowRibbon)
		{
			BarFunctions.GetSubItemsByNameAndType(ribbonControl.QatToolbar.GetBaseItemContainer(), ItemName, arrayList, itemType);
		}
		ArrayList allRibbonBars = GetAllRibbonBars();
		foreach (RibbonBar item in allRibbonBars)
		{
			BarFunctions.GetSubItemsByNameAndType(item.GetBaseItemContainer(), ItemName, arrayList, itemType);
		}
		if (ribbonControl != null && ribbonControl.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByNameAndType(ribbonControl.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList, itemType);
		}
		return arrayList;
	}

	public override ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(GetBaseItemContainer(), ItemName, arrayList, itemType, useGlobalName);
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.QatPositionedBelowRibbon)
		{
			BarFunctions.GetSubItemsByNameAndType(ribbonControl.QatToolbar.GetBaseItemContainer(), ItemName, arrayList, itemType, useGlobalName);
		}
		ArrayList allRibbonBars = GetAllRibbonBars();
		foreach (RibbonBar item in allRibbonBars)
		{
			BarFunctions.GetSubItemsByNameAndType(item.GetBaseItemContainer(), ItemName, arrayList, itemType, useGlobalName);
		}
		if (ribbonControl != null && ribbonControl.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByNameAndType(ribbonControl.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList, itemType, useGlobalName);
		}
		return arrayList;
	}

	public override BaseItem GetItem(string ItemName)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(GetBaseItemContainer(), ItemName);
		if (subItemByName != null)
		{
			return subItemByName;
		}
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.QatPositionedBelowRibbon)
		{
			subItemByName = BarFunctions.GetSubItemByName(ribbonControl.QatToolbar.GetBaseItemContainer(), ItemName);
			if (subItemByName != null)
			{
				return subItemByName;
			}
		}
		ArrayList allRibbonBars = GetAllRibbonBars();
		foreach (RibbonBar item in allRibbonBars)
		{
			subItemByName = ((!item.OverflowState || item.OverflowRibbonBar == null) ? BarFunctions.GetSubItemByName(item.GetBaseItemContainer(), ItemName) : BarFunctions.GetSubItemByName(item.OverflowRibbonBar.GetBaseItemContainer(), ItemName));
			if (subItemByName == null)
			{
				foreach (BaseItem item2 in item.Items)
				{
					if (item2.Name == ItemName)
					{
						return item2;
					}
					BaseItem subItemByName2 = BarFunctions.GetSubItemByName(item2, ItemName);
					if (subItemByName2 != null)
					{
						return subItemByName2;
					}
				}
			}
			if (subItemByName != null)
			{
				return subItemByName;
			}
		}
		if (ribbonControl != null && ribbonControl.GlobalContextMenuBar != null)
		{
			return BarFunctions.GetSubItemByName(ribbonControl.GlobalContextMenuBar.ItemsContainer, ItemName);
		}
		return null;
	}

	internal void OnRibbonBarShowKeyTips(RibbonBar bar)
	{
		if (!m_AltFocus)
		{
			m_AltFocus = true;
		}
		if (m_AltFocusedControl != ((Control)bar).Parent && ((Control)bar).Parent is RibbonPanel)
		{
			if (m_AltFocusedControl != null)
			{
				m_AltFocusedControl.ShowKeyTips = false;
			}
			m_AltFocusedControl = ((Control)bar).Parent as IKeyTipsControl;
			if (m_AltFocusedControl != null)
			{
				m_AltFocusedControl.ShowKeyTips = true;
			}
		}
	}

	protected override void OnShowKeyTipsChanged()
	{
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.QatPositionedBelowRibbon)
		{
			ribbonControl.QatToolbar.ShowKeyTips = ShowKeyTips;
		}
		base.OnShowKeyTipsChanged();
		if (ShowKeyTips && !m_AltFocus)
		{
			GiveKeyTipsAltFocus();
		}
	}

	internal void BackstageTabClosed(SuperTabControl tabControl)
	{
		if (m_AltFocusedControl == tabControl || m_AltFocusedControl == tabControl.TabStrip)
		{
			m_AltFocusedControl = null;
		}
	}

	protected override bool OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Invalid comparison between Unknown and I4
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected I4, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Invalid comparison between Unknown and I4
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Invalid comparison between Unknown and I4
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Invalid comparison between Unknown and I4
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Invalid comparison between Unknown and I4
		int num = WinApi.ToInt(wParam);
		if (num == 27)
		{
			GetRibbonControl()?.OnEscapeKeyDown();
		}
		if (m_AltFocus)
		{
			if (base.HasRegisteredPopups)
			{
				bool flag = base.OnKeyDown(hWnd, wParam, lParam);
				if (!base.HasRegisteredPopups)
				{
					ReleaseAltFocus();
				}
			}
			Keys val = (Keys)NativeFunctions.MapVirtualKey((uint)(int)wParam, 2u);
			if ((int)val == 0)
			{
				val = (Keys)num;
			}
			if ((int)val == 27)
			{
				bool flag2 = false;
				if (!ShowKeyTips && m_AltFocusedControl != null && m_AltFocusedControl is RibbonPanel)
				{
					flag2 = true;
				}
				ReleaseAltFocus();
				if (flag2)
				{
					GiveKeyTipsAltFocus();
				}
				return base.OnKeyDown(hWnd, wParam, lParam);
			}
			char c = CharFromInt((int)val);
			if (c == '\0')
			{
				ReleaseAltFocus();
				return true;
			}
			if ((int)val == 32 || (int)val == 40 || (int)val == 39 || (int)val == 9)
			{
				ReleaseAltFocus();
				return false;
			}
			((Control)this).ProcessMnemonic(c);
			return true;
		}
		if (num == 112 && (int)Control.ModifierKeys == 131072)
		{
			Form f = ((Control)this).FindForm();
			if (BarFunctions.IsFormActive(f))
			{
				RibbonControl ribbonControl = GetRibbonControl();
				if (ribbonControl.AutoKeyboardExpand)
				{
					ribbonControl.Expanded = !ribbonControl.Expanded;
				}
			}
		}
		return base.OnKeyDown(hWnd, wParam, lParam);
	}

	private char CharFromInt(int key)
	{
		char[] array = new char[1];
		byte[] array2 = new byte[1];
		try
		{
			array2[0] = Convert.ToByte(key);
			Encoding.Default.GetDecoder().GetChars(array2, 0, 1, array, 0);
		}
		catch (Exception)
		{
			return '\0';
		}
		return array[0];
	}

	public bool InvokeSysKeyUpKeyTips()
	{
		return OnSysKeyUp(((Control)this).Handle, new IntPtr(18), new IntPtr(121));
	}

	protected override bool OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		int num = WinApi.ToInt(wParam);
		if (num == 18 || (num == 121 && !_IgnoreF10Key))
		{
			bool flag = true;
			if (m_AltFocus)
			{
				if (num == 18 || (num == 121 && !_IgnoreF10Key))
				{
					flag = false;
				}
				ReleaseAltFocus();
			}
			else
			{
				Form val = ((Control)this).FindForm();
				if (val == Form.ActiveForm)
				{
					flag = false;
					ActivateKeyTips();
				}
			}
			if (!flag)
			{
				return true;
			}
		}
		return base.OnSysKeyUp(hWnd, wParam, lParam);
	}

	private void ActivateKeyTips()
	{
		if (GetApplicationButton() is ApplicationButton { BackstageTab: not null, Expanded: not false } applicationButton)
		{
			if (m_AltFocusedControl != null)
			{
				m_AltFocusedControl.ShowKeyTips = false;
			}
			m_AltFocusedControl = applicationButton.BackstageTab.TabStrip;
			if (!m_AltFocusedControl.ShowKeyTips)
			{
				m_AltFocusedControl.ShowKeyTips = true;
			}
		}
		else
		{
			GiveKeyTipsAltFocus();
		}
	}

	private void GiveKeyTipsAltFocus()
	{
		if (m_KeyTipsEnabled)
		{
			m_AltFocus = true;
			ShowKeyTips = true;
			SetupActiveWindowTimer();
		}
	}

	private void ReleaseAltFocus()
	{
		if (m_AltFocus)
		{
			ReleaseActiveWindowTimer();
			if (!ShowKeyTips && m_AltFocusedControl != null)
			{
				m_AltFocusedControl.ShowKeyTips = false;
				m_AltFocusedControl = null;
			}
			ShowKeyTips = false;
		}
		m_AltFocus = false;
	}

	public void ExitKeyTipsMode()
	{
		ReleaseAltFocus();
	}

	internal void OnChildItemClick(BaseItem item)
	{
		if (ShowKeyTips || (m_AltFocusedControl != null && m_AltFocusedControl.ShowKeyTips))
		{
			ShowKeyTips = false;
			ReleaseAltFocus();
		}
	}

	protected override bool OnSysMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (ShowKeyTips || (m_AltFocusedControl != null && m_AltFocusedControl.ShowKeyTips))
		{
			ShowKeyTips = false;
			ReleaseAltFocus();
		}
		GetRibbonControl()?.OnSysMouseDown(hWnd, wParam, lParam);
		return base.OnSysMouseDown(hWnd, wParam, lParam);
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		if (m_AltFocusedControl != null)
		{
			if (m_AltFocusedControl.ProcessMnemonicEx(charCode))
			{
				ReleaseAltFocus();
				return true;
			}
			return false;
		}
		if (!m_AltFocus && (Control.ModifierKeys & 0x40000) != 262144)
		{
			return false;
		}
		return base.ProcessMnemonic(charCode);
	}

	public override bool ProcessMnemonicEx(char charCode)
	{
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		BaseItem baseItem = null;
		if (m_CaptionVisible)
		{
			baseItem = GetItemForMnemonic(m_StripContainer.CaptionContainer, charCode, deepScan: false, stackKeys: true);
			if (baseItem != null && ProcessItemMnemonicKey(baseItem))
			{
				ReleaseAltFocus();
				return true;
			}
		}
		RibbonControl ribbonControl = GetRibbonControl();
		if (ribbonControl != null && ribbonControl.QatPositionedBelowRibbon && ribbonControl.QatToolbar.ProcessMnemonicEx(charCode))
		{
			ReleaseAltFocus();
			return true;
		}
		if (baseItem == null)
		{
			baseItem = GetItemForMnemonic(GetBaseItemContainer(), charCode, deepScan: true, stackKeys: true);
		}
		if (baseItem is RibbonTabItem && baseItem.Visible)
		{
			if (!m_AltFocus)
			{
				GiveKeyTipsAltFocus();
			}
			ShowKeyTips = false;
			if (baseItem != SelectedRibbonTabItem)
			{
				baseItem.RaiseClick();
			}
			else if (SelectedRibbonTabItem != null && ribbonControl != null && !ribbonControl.Expanded && !ribbonControl.IsPopupMode)
			{
				ribbonControl.RibbonTabItemClick(SelectedRibbonTabItem);
			}
			if (SelectedRibbonTabItem != null && m_KeyTipsEnabled)
			{
				if (m_AltFocusedControl != null)
				{
					m_AltFocusedControl.ShowKeyTips = false;
				}
				m_AltFocusedControl = SelectedRibbonTabItem.Panel;
				if (m_AltFocusedControl != null)
				{
					m_AltFocusedControl.ShowKeyTips = true;
				}
			}
			return true;
		}
		if (baseItem != null && ProcessItemMnemonicKey(baseItem))
		{
			ShowKeyTips = false;
			if (baseItem is ApplicationButton)
			{
				ApplicationButton applicationButton = (ApplicationButton)baseItem;
				if (applicationButton.BackstageTab != null)
				{
					if (m_AltFocusedControl != null)
					{
						m_AltFocusedControl.ShowKeyTips = false;
					}
					m_AltFocusedControl = applicationButton.BackstageTab.TabStrip;
					if (!m_AltFocusedControl.ShowKeyTips)
					{
						m_AltFocusedControl.ShowKeyTips = true;
					}
				}
			}
			return true;
		}
		if (m_AltFocus && SelectedRibbonTabItem != null && SelectedRibbonTabItem.Panel != null)
		{
			Control panel = (Control)(object)SelectedRibbonTabItem.Panel;
			foreach (Control item in (ArrangedElementCollection)panel.Controls)
			{
				Control val = item;
				if (val is RibbonBar && val.Visible && Control.IsMnemonic(charCode, val.Text))
				{
					ShowKeyTips = false;
					((RibbonBar)(object)val).ShowKeyTips = true;
					if (m_AltFocusedControl != null)
					{
						m_AltFocusedControl.ShowKeyTips = false;
					}
					m_AltFocusedControl = (IKeyTipsControl)val;
					return true;
				}
			}
		}
		return false;
	}

	protected override void OnActiveWindowChanged()
	{
		ReleaseAltFocus();
		base.OnActiveWindowChanged();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return ((ContainerControl)this).ProcessCmdKey(ref msg, keyData);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return base.ProcessDialogKey(keyData);
	}

	protected override void OnPopupItemRightClick(BaseItem item)
	{
		GetRibbonControl()?.ShowCustomizeContextMenu(item, ribbonStrip: false);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (m_TitleTextMarkup != null)
		{
			m_TitleTextMarkup.MouseLeave((Control)(object)this);
		}
		base.OnMouseLeave(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Invalid comparison between Unknown and I4
		m_MouseDownOnCaption = false;
		if (m_CaptionVisible)
		{
			m_MouseDownOnCaption = HitTestCaption(new Point(e.X, e.Y));
			if ((int)e.Button == 2097152 && m_MouseDownOnCaption)
			{
				Form val = ((Control)this).FindForm();
				if (val != null)
				{
					Point mousePosition = Control.MousePosition;
					byte[] bytes = BitConverter.GetBytes(mousePosition.X);
					byte[] bytes2 = BitConverter.GetBytes(mousePosition.Y);
					byte[] value = new byte[4]
					{
						bytes[0],
						bytes[1],
						bytes2[0],
						bytes2[1]
					};
					int lParam = BitConverter.ToInt32(value, 0);
					((Control)this).Capture = false;
					NativeFunctions.SendMessage(((Control)val).Handle, 274, NativeFunctions.TrackPopupMenu(NativeFunctions.GetSystemMenu(((Control)val).Handle, bRevert: false), 256u, Control.MousePosition.X, Control.MousePosition.Y, 0, ((Control)val).Handle, IntPtr.Zero), lParam);
					return;
				}
			}
			if (m_TitleTextMarkup != null)
			{
				m_TitleTextMarkup.MouseDown((Control)(object)this, e);
			}
			e = TranslateMouseEventArgs(e);
		}
		if ((int)e.Button == 2097152 && QuickToolbarBounds.Contains(e.X, e.Y))
		{
			GetRibbonControl()?.OnRibbonStripRightClick(this, e.X, e.Y);
		}
		else if ((int)e.Button == 1048576 && m_CaptionVisible && m_TabGroupsVisible && !m_StripContainer.RibbonStripContainer.DisplayRectangle.Contains(e.X, e.Y))
		{
			foreach (RibbonTabItemGroup tabGroup in TabGroups)
			{
				if (!tabGroup.Visible)
				{
					continue;
				}
				foreach (Rectangle displayPosition in tabGroup.DisplayPositions)
				{
					if (displayPosition.Contains(e.X, e.Y))
					{
						if (!tabGroup.IsTabFromGroupSelected)
						{
							tabGroup.SelectFirstTab();
							return;
						}
						break;
					}
				}
			}
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (m_CaptionVisible)
		{
			if (m_TitleTextMarkup != null)
			{
				m_TitleTextMarkup.MouseUp((Control)(object)this, e);
			}
			e = TranslateMouseEventArgs(e);
		}
		m_MouseDownOnCaption = false;
		base.OnMouseUp(e);
	}

	internal BaseItem GetApplicationButton()
	{
		if (!CaptionVisible)
		{
			return null;
		}
		BaseItem captionContainerItem = CaptionContainerItem;
		if ((EffectiveStyle == eDotNetBarStyle.Office2010 || EffectiveStyle == eDotNetBarStyle.Metro) && Items.Count > 0)
		{
			foreach (BaseItem item in Items)
			{
				if (item is ApplicationButton)
				{
					return item;
				}
			}
		}
		if (captionContainerItem.SubItems.Count > 0 && captionContainerItem.SubItems[0] is ApplicationButton)
		{
			return captionContainerItem.SubItems[0];
		}
		if (captionContainerItem.SubItems.Count > 0 && captionContainerItem.SubItems[0] is ButtonItem && ((ButtonItem)captionContainerItem.SubItems[0]).HotTrackingStyle == eHotTrackingStyle.Image)
		{
			return captionContainerItem.SubItems[0];
		}
		return null;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (m_CaptionVisible)
		{
			if ((int)e.Button == 1048576 && m_MouseDownOnCaption && HitTestCaption(new Point(e.X, e.Y)))
			{
				Form val = ((Control)this).FindForm();
				if (val != null && ((int)val.WindowState == 0 || BarFunctions.IsWindows7))
				{
					if (GetApplicationButton() is PopupItem { Expanded: not false } popupItem)
					{
						popupItem.Expanded = false;
					}
					Point mousePosition = Control.MousePosition;
					byte[] bytes = BitConverter.GetBytes(mousePosition.X);
					byte[] bytes2 = BitConverter.GetBytes(mousePosition.Y);
					byte[] value = new byte[4]
					{
						bytes[0],
						bytes[1],
						bytes2[0],
						bytes2[1]
					};
					int lParam = BitConverter.ToInt32(value, 0);
					((Control)this).Capture = false;
					NativeFunctions.SendMessage(((Control)val).Handle, 274, 61458, lParam);
					m_MouseDownOnCaption = false;
					return;
				}
			}
			if (m_TitleTextMarkup != null)
			{
				m_TitleTextMarkup.MouseMove((Control)(object)this, e);
			}
			e = TranslateMouseEventArgs(e);
		}
		base.OnMouseMove(e);
	}

	protected override void InternalOnClick(MouseButtons mb, Point mousePos)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (m_CaptionVisible)
		{
			MouseEventArgs e = new MouseEventArgs(mb, 0, mousePos.X, mousePos.Y, 0);
			e = TranslateMouseEventArgs(e);
			mousePos = new Point(e.X, e.Y);
		}
		base.InternalOnClick(mb, mousePos);
	}

	private MouseEventArgs TranslateMouseEventArgs(MouseEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		if (e.Y <= 6)
		{
			Form val = ((Control)this).FindForm();
			if (val != null && (int)val.WindowState == 2 && val is RibbonForm)
			{
				if (e.X > 4)
				{
					e = ((e.X < ((Control)this).Width - 6) ? new MouseEventArgs(e.Button, e.Clicks, e.X, CaptionContainerItem.TopInternal + 5, e.Delta) : new MouseEventArgs(e.Button, e.Clicks, SystemCaptionItem.DisplayRectangle.Right - 4, SystemCaptionItem.DisplayRectangle.Top + 4, e.Delta));
				}
				else
				{
					BaseItem applicationButton = GetApplicationButton();
					if (applicationButton != null)
					{
						e = new MouseEventArgs(e.Button, e.Clicks, applicationButton.LeftInternal + 1, applicationButton.TopInternal + 1, e.Delta);
					}
				}
			}
		}
		return e;
	}

	protected override void OnClick(EventArgs e)
	{
		if (m_CaptionVisible && m_TitleTextMarkup != null)
		{
			m_TitleTextMarkup.Click((Control)(object)this);
		}
		base.OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		if (m_CaptionVisible)
		{
			Point p = ((Control)this).PointToClient(Control.MousePosition);
			if (HitTestCaption(p))
			{
				Form val = ((Control)this).FindForm();
				if (val != null && val.MaximizeBox && ((int)val.FormBorderStyle == 4 || (int)val.FormBorderStyle == 6))
				{
					if ((int)val.WindowState == 0)
					{
						NativeFunctions.PostMessage(((Control)val).Handle, 274, 61488, 0);
					}
					else if ((int)val.WindowState == 2)
					{
						NativeFunctions.PostMessage(((Control)val).Handle, 274, 61728, 0);
					}
				}
			}
		}
		base.OnDoubleClick(e);
	}

	protected bool HitTestCaption(Point p)
	{
		if (m_CaptionBounds.Contains(p) && m_CaptionTextBounds != null)
		{
			Rectangle[] captionTextBounds = m_CaptionTextBounds;
			foreach (Rectangle rectangle in captionTextBounds)
			{
				if (rectangle.Contains(p))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void SystemCaptionClick(object sender, EventArgs e)
	{
		SystemCaptionItem systemCaptionItem = sender as SystemCaptionItem;
		Form val = ((Control)this).FindForm();
		if (val != null && systemCaptionItem.LastButtonClick == systemCaptionItem.MouseDownButton)
		{
			if (systemCaptionItem.LastButtonClick == SystemButton.Minimize)
			{
				NativeFunctions.PostMessage(((Control)val).Handle, 274, 61472, 0);
			}
			else if (systemCaptionItem.LastButtonClick == SystemButton.Maximize)
			{
				NativeFunctions.PostMessage(((Control)val).Handle, 274, 61488, 0);
			}
			else if (systemCaptionItem.LastButtonClick == SystemButton.Restore)
			{
				NativeFunctions.PostMessage(((Control)val).Handle, 274, 61728, 0);
			}
			else if (systemCaptionItem.LastButtonClick == SystemButton.Close)
			{
				NativeFunctions.PostMessage(((Control)val).Handle, 274, 61536, 0);
			}
			else if (systemCaptionItem.LastButtonClick == SystemButton.Help)
			{
				NativeFunctions.PostMessage(((Control)val).Handle, 274, 61824, 0);
			}
		}
	}

	protected override void OnResize(EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		base.OnResize(e);
		if (!m_CaptionVisible)
		{
			return;
		}
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			if ((int)val.WindowState == 2 || (int)val.WindowState == 1)
			{
				m_StripContainer.SystemCaptionItem.RestoreEnabled = true;
			}
			else
			{
				m_StripContainer.SystemCaptionItem.RestoreEnabled = false;
			}
		}
	}

	protected override void PaintKeyTips(Graphics g)
	{
		if (ShowKeyTips)
		{
			KeyTipsRendererEventArgs e = new KeyTipsRendererEventArgs(g, Rectangle.Empty, "", GetKeyTipFont(), null);
			BaseRenderer renderer = GetRenderer();
			PaintContainerKeyTips(m_StripContainer.RibbonStripContainer, renderer, e);
			if (m_CaptionVisible)
			{
				PaintContainerKeyTips(m_StripContainer.CaptionContainer, renderer, e);
			}
		}
	}

	protected override Rectangle GetKeyTipRectangle(Graphics g, BaseItem item, Font font, string keyTip)
	{
		Rectangle keyTipRectangle = base.GetKeyTipRectangle(g, item, font, keyTip);
		if (QuickToolbarItems.Contains(item))
		{
			keyTipRectangle.Y += 4;
		}
		return keyTipRectangle;
	}

	internal void ClearMDIChildSystemItems(bool bRecalcLayout)
	{
		if (m_StripContainer.RibbonStripContainer == null)
		{
			return;
		}
		bool flag = false;
		try
		{
			if (Items.Contains("dotnetbarsysiconitem"))
			{
				Items.Remove("dotnetbarsysiconitem");
				flag = true;
			}
			if (Items.Contains("dotnetbarsysmenuitem"))
			{
				Items.Remove("dotnetbarsysmenuitem");
				flag = true;
			}
			if (bRecalcLayout && flag)
			{
				RecalcLayout();
			}
		}
		catch (Exception)
		{
		}
	}

	internal void ShowMDIChildSystemItems(Form objMdiChild, bool bRecalcLayout)
	{
		ClearMDIChildSystemItems(bRecalcLayout);
		if (objMdiChild != null)
		{
			MDISystemItem mDISystemItem = new MDISystemItem("dotnetbarsysmenuitem");
			if (!objMdiChild.ControlBox)
			{
				mDISystemItem.CloseEnabled = false;
			}
			if (!objMdiChild.MinimizeBox)
			{
				mDISystemItem.MinimizeEnabled = false;
			}
			if (!objMdiChild.MaximizeBox)
			{
				mDISystemItem.RestoreEnabled = false;
			}
			mDISystemItem.ItemAlignment = eItemAlignment.Far;
			mDISystemItem.Click += MDISysItemClick;
			Items.Add(mDISystemItem);
			if (bRecalcLayout)
			{
				RecalcLayout();
			}
		}
	}

	private void MDISysItemClick(object sender, EventArgs e)
	{
		MDISystemItem mDISystemItem = sender as MDISystemItem;
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			val = val.ActiveMdiChild;
		}
		if (val == null)
		{
			ClearMDIChildSystemItems(bRecalcLayout: true);
		}
		else if (mDISystemItem.LastButtonClick == SystemButton.Minimize)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, 61472, 0);
		}
		else if (mDISystemItem.LastButtonClick == SystemButton.Restore)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, 61728, 0);
		}
		else if (mDISystemItem.LastButtonClick == SystemButton.Close)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, 61536, 0);
		}
		else if (mDISystemItem.LastButtonClick == SystemButton.NextWindow)
		{
			NativeFunctions.PostMessage(((Control)val).Handle, 274, 61504, 0);
		}
	}

	internal void InvokeMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
	}

	internal void InvokeMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
	}

	internal void InvokeMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
	}

	internal void InvokeClick(EventArgs e)
	{
		((Control)this).OnClick(e);
	}
}
