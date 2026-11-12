using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ExplorerBarGroupItem : ImageItem, IDesignTimeProvider
{
	private Rectangle m_ExpandButtonRect = Rectangle.Empty;

	private bool m_ExpandButtonHot;

	private int m_Margin = 2;

	private bool m_MouseOver;

	private bool m_MouseDown;

	private Rectangle m_PanelRect = Rectangle.Empty;

	private ItemStyleMapper m_HeaderStyle;

	private ItemStyleMapper m_HeaderHotStyle;

	private ItemStyleMapper m_BackgroundStyle;

	private ElementStyle m_TitleStyle;

	private ElementStyle m_TitleHotStyle;

	private ElementStyle m_BackStyle;

	private eExplorerBarStockStyle m_StockStyle = eExplorerBarStockStyle.Custom;

	private bool m_ExpandButtonVisible = true;

	private Point m_MouseDownPt;

	private const int EXPAND_MARGIN = 25;

	private Image m_Image;

	private int m_ImageIndex = -1;

	private Image m_ImageCachedIdx;

	private int m_SubItemsMargin = 4;

	private bool m_XPSpecialGroup;

	private Color m_ExpandBorderColor = Color.DarkGray;

	private Color m_ExpandBackColor = Color.White;

	private Color m_ExpandForeColor = SystemColors.ActiveCaption;

	private Color m_ExpandHotBorderColor = Color.DarkGray;

	private Color m_ExpandHotBackColor = Color.White;

	private Color m_ExpandHotForeColor = SystemColors.ActiveCaption;

	private bool m_HeaderExpands = true;

	private bool m_WordWrapSubItems = true;

	private const string INFO_EMPTYPANEL = "Right-click header and choose one of Create commands or use SubItems collection to create new buttons. You can also drag & drop controls on this panel.";

	private bool m_DropShadow = true;

	private ShadowPaintInfo m_ShadowPaintInfo;

	private bool m_IsPainting;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("Appearance")]
	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			m_ThemeAware = value;
			if (m_SubItems == null)
			{
				return;
			}
			foreach (BaseItem subItem in m_SubItems)
			{
				if (!(subItem is ButtonItem))
				{
					subItem.ThemeAware = value;
				}
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Gets or sets whether expand button is visible.")]
	public bool ExpandButtonVisible
	{
		get
		{
			return m_ExpandButtonVisible;
		}
		set
		{
			m_ExpandButtonVisible = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether group is expanded or not.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			base.Expanded = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether drop shadow is displayed when non-themed display is used.")]
	public bool DropShadow
	{
		get
		{
			return m_DropShadow;
		}
		set
		{
			m_DropShadow = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets a normal item style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle TitleStyle => m_TitleStyle;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets a mouse over item style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle TitleHotStyle => m_TitleHotStyle;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets group background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackStyle => m_BackStyle;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("This property is obsolete. Use TitleStyle property instead")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ItemStyleMapper HeaderStyle => m_HeaderStyle;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("This property is obsolete. Use TitleHotStyle property instead")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ItemStyleMapper HeaderHotStyle => m_HeaderHotStyle;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("This property is obsolete. Use BackStyle property instead")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ItemStyleMapper BackgroundStyle => m_BackgroundStyle;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Determines whether clicking the header of the control expands the item.")]
	public bool HeaderExpands
	{
		get
		{
			return m_HeaderExpands;
		}
		set
		{
			m_HeaderExpands = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Applies the stock style to the object.")]
	[DefaultValue(eExplorerBarStockStyle.Custom)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public eExplorerBarStockStyle StockStyle
	{
		get
		{
			return m_StockStyle;
		}
		set
		{
			m_StockStyle = value;
			if (m_StockStyle == eExplorerBarStockStyle.Custom)
			{
				return;
			}
			BarFunctions.SetExplorerBarStyle(this, m_StockStyle);
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is ButtonItem)
				{
					BarFunctions.SetExplorerBarStyle(subItem as ButtonItem, m_StockStyle);
				}
			}
			if (m_StockStyle == eExplorerBarStockStyle.BlueSpecial || m_StockStyle == eExplorerBarStockStyle.OliveGreenSpecial || m_StockStyle == eExplorerBarStockStyle.SilverSpecial)
			{
				XPSpecialGroup = true;
			}
			else if (m_StockStyle != eExplorerBarStockStyle.SystemColors)
			{
				XPSpecialGroup = false;
			}
			Refresh();
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(eDotNetBarStyle.OfficeXP)]
	[Category("Appearance")]
	[Description("Determines the display of the item when shown.")]
	public override eDotNetBarStyle Style
	{
		get
		{
			return base.Style;
		}
		set
		{
			base.Style = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Specifies expand button border color.")]
	public Color ExpandBorderColor
	{
		get
		{
			return m_ExpandBorderColor;
		}
		set
		{
			m_ExpandBorderColor = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Specifies expand button back color.")]
	public Color ExpandBackColor
	{
		get
		{
			return m_ExpandBackColor;
		}
		set
		{
			m_ExpandBackColor = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Specifies expand button fore color.")]
	public Color ExpandForeColor
	{
		get
		{
			return m_ExpandForeColor;
		}
		set
		{
			m_ExpandForeColor = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Specifies hot expand button border color.")]
	public Color ExpandHotBorderColor
	{
		get
		{
			return m_ExpandHotBorderColor;
		}
		set
		{
			m_ExpandHotBorderColor = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Specifies hot expand button back color.")]
	public Color ExpandHotBackColor
	{
		get
		{
			return m_ExpandHotBackColor;
		}
		set
		{
			m_ExpandHotBackColor = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Specifies hot expand button fore color.")]
	public Color ExpandHotForeColor
	{
		get
		{
			return m_ExpandHotForeColor;
		}
		set
		{
			m_ExpandHotForeColor = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(eItemAlignment.Near)]
	[Category("Appearance")]
	[Description("Determines alignment of the item inside the container.")]
	public override eItemAlignment ItemAlignment
	{
		get
		{
			return base.ItemAlignment;
		}
		set
		{
			base.ItemAlignment = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
	public override bool Stretch
	{
		get
		{
			return base.Stretch;
		}
		set
		{
			base.Stretch = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	public override bool AutoCollapseOnClick
	{
		get
		{
			return base.AutoCollapseOnClick;
		}
		set
		{
			base.AutoCollapseOnClick = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Determines whether sub-items are displayed.")]
	public override bool ShowSubItems
	{
		get
		{
			return base.ShowSubItems;
		}
		set
		{
			base.ShowSubItems = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Design")]
	[Description("Indicates item category used to group similar items at design-time.")]
	public override string Category
	{
		get
		{
			return base.Category;
		}
		set
		{
			base.Category = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Design")]
	[Description("Indicates description of the item that is displayed during design.")]
	public override string Description
	{
		get
		{
			return base.Description;
		}
		set
		{
			base.Description = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether this item is beginning of the group.")]
	public override bool BeginGroup
	{
		get
		{
			return base.BeginGroup;
		}
		set
		{
			base.BeginGroup = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether is item enabled.")]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ExplorerBarGroupItemEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Data")]
	[Description("Collection of sub items.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public override SubItemsCollection SubItems => base.SubItems;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle PanelRect => m_PanelRect;

	internal bool UseThemes => base.IsThemed;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ImageList ImageList
	{
		get
		{
			if (GetOwner() is IOwner owner)
			{
				return owner.ImagesMedium;
			}
			return null;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed on the face of the item.")]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return m_Image;
		}
		set
		{
			NeedRecalcSize = true;
			m_Image = value;
			OnImageChanged();
			Refresh();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether XP themed special group colors are used for drawing.")]
	[DefaultValue(false)]
	public bool XPSpecialGroup
	{
		get
		{
			return m_XPSpecialGroup;
		}
		set
		{
			m_XPSpecialGroup = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(4)]
	[Description("Indicates margin in pixels between the edge of the container and the items contained inside of it")]
	public int SubItemsMargin
	{
		get
		{
			return m_SubItemsMargin;
		}
		set
		{
			m_SubItemsMargin = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether text on sub items is wrapped on new line if it cannot fit the space available.")]
	[DefaultValue(true)]
	public bool WordWrapSubItems
	{
		get
		{
			return m_WordWrapSubItems;
		}
		set
		{
			m_WordWrapSubItems = value;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is ButtonItem)
				{
					((ButtonItem)subItem)._FitContainer = m_WordWrapSubItems;
				}
			}
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image list image index of the image that will be displayed on the face of the item.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ImageIndexEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	public int ImageIndex
	{
		get
		{
			return m_ImageIndex;
		}
		set
		{
			m_ImageCachedIdx = null;
			if (m_ImageIndex != value)
			{
				m_ImageIndex = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "ImageIndex");
				}
				if (m_Parent != null)
				{
					OnImageChanged();
					NeedRecalcSize = true;
					Refresh();
				}
			}
		}
	}

	public ExplorerBarGroupItem()
		: this("", "")
	{
	}

	public ExplorerBarGroupItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public ExplorerBarGroupItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		m_IsContainer = true;
		m_AllowOnlyOneSubItemExpanded = false;
		m_TitleStyle = new ElementStyle();
		m_TitleStyle.StyleChanged += VisualPropertyChanged;
		m_TitleHotStyle = new ElementStyle();
		m_TitleHotStyle.StyleChanged += VisualPropertyChanged;
		m_BackStyle = new ElementStyle();
		m_BackStyle.StyleChanged += VisualPropertyChanged;
		m_HeaderStyle = new ItemStyleMapper(m_TitleStyle);
		m_HeaderHotStyle = new ItemStyleMapper(m_TitleHotStyle);
		m_BackgroundStyle = new ItemStyleMapper(m_BackStyle);
		SubItemsImageSize = new Size(12, 12);
		ImageSize = new Size(12, 12);
		BarFunctions.SetExplorerBarStyle(this, m_StockStyle);
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.DisposeImage(ref m_Image);
			BarUtilities.DisposeImage(ref m_ImageCachedIdx);
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		ExplorerBarGroupItem explorerBarGroupItem = new ExplorerBarGroupItem();
		CopyToItem(explorerBarGroupItem);
		return explorerBarGroupItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		ExplorerBarGroupItem objCopy = copy as ExplorerBarGroupItem;
		base.CopyToItem(objCopy);
	}

	private void VisualPropertyChanged(object sender, EventArgs e)
	{
		VisualPropertyChanged();
	}

	internal void VisualPropertyChanged()
	{
		OnAppearanceChanged();
	}

	public override void RecalcSize()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (!BaseItem.IsHandleValid(val))
		{
			return;
		}
		Graphics val2 = Graphics.FromHwnd(val.Handle);
		SizeF empty = SizeF.Empty;
		Image image = GetImage();
		string text = m_Text;
		if (text == "")
		{
			text = " ";
		}
		int num = m_Rect.Width - m_Margin * 2;
		if (m_ExpandButtonVisible)
		{
			num -= 25;
		}
		if (image != null)
		{
			num -= image.Width + m_Margin * 2;
		}
		if (num <= 0)
		{
			num = 1;
		}
		bool disposeFont = false;
		Font font = GetFont(out disposeFont);
		empty = TextDrawing.MeasureString(val2, text, font, num, m_TitleStyle.TextFormat);
		if (disposeFont)
		{
			font.Dispose();
		}
		val2.Dispose();
		m_Rect.Height = 23;
		if (m_Rect.Height < (int)empty.Height + m_Margin * 2)
		{
			m_Rect.Height = (int)empty.Height + m_Margin * 2;
		}
		if (image != null)
		{
			int height = m_Rect.Height;
			if (image.Height > m_Rect.Height)
			{
				m_Rect.Height = image.Height + m_Margin * 2;
			}
			m_PanelRect = new Rectangle(0, m_Rect.Height - height, m_Rect.Width, height);
		}
		else
		{
			m_PanelRect = new Rectangle(0, 0, m_Rect.Width, m_Rect.Height);
		}
		if (m_ExpandButtonVisible)
		{
			m_ExpandButtonRect = new Rectangle(m_PanelRect.Right - 25, m_PanelRect.Y, 25, m_PanelRect.Height);
		}
		if (Expanded)
		{
			if (m_SubItems != null)
			{
				int num2 = m_Rect.Bottom + 1;
				int leftInternal = m_Rect.Left + m_SubItemsMargin;
				int num3 = -1;
				foreach (BaseItem subItem in m_SubItems)
				{
					num3++;
					if (!subItem.Visible)
					{
						subItem.Displayed = false;
						continue;
					}
					subItem.WidthInternal = m_Rect.Width - m_SubItemsMargin * 2;
					subItem.RecalcSize();
					subItem.WidthInternal = m_Rect.Width - m_SubItemsMargin * 2;
					if (subItem.BeginGroup)
					{
						num2 += 3;
					}
					subItem.TopInternal = num2;
					subItem.LeftInternal = leftInternal;
					num2 += subItem.HeightInternal;
					subItem.Displayed = true;
				}
				m_Rect.Height = num2 - m_Rect.Top + 2;
			}
		}
		else
		{
			foreach (BaseItem subItem2 in m_SubItems)
			{
				subItem2.Displayed = false;
			}
		}
		if (Expanded && DesignMode && SubItems.Count == 0 && Parent != null && Parent.SubItems.Count == 1)
		{
			m_Rect.Height += 64;
		}
		base.RecalcSize();
	}

	private Image GetImage()
	{
		if (m_Image != null)
		{
			return m_Image;
		}
		if (m_ImageIndex >= 0)
		{
			return GetImageFromImageList(m_ImageIndex);
		}
		return null;
	}

	private Image GetImageFromImageList(int ImageIndex)
	{
		if (ImageIndex >= 0)
		{
			IOwner owner = null;
			Bar bar = null;
			if (owner == null)
			{
				owner = GetOwner() as IOwner;
			}
			if (bar == null)
			{
				bar = ContainerControl as Bar;
			}
			if (owner != null)
			{
				try
				{
					if (owner.ImagesMedium != null)
					{
						if (m_ImageCachedIdx == null)
						{
							m_ImageCachedIdx = owner.ImagesMedium.Images[ImageIndex];
						}
						return m_ImageCachedIdx;
					}
				}
				catch (Exception)
				{
					return null;
				}
			}
		}
		return null;
	}

	private ShadowPaintInfo GetShadowPaintInfo()
	{
		if (m_ShadowPaintInfo == null)
		{
			m_ShadowPaintInfo = new ShadowPaintInfo();
		}
		m_ShadowPaintInfo.Size = 3;
		return m_ShadowPaintInfo;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Expected O, but got Unknown
		if (DisplayRectangle.Width <= 0 || DisplayRectangle.Height <= 0 || m_IsPainting)
		{
			return;
		}
		m_IsPainting = true;
		try
		{
			if (base.IsThemed)
			{
				PaintThemed(pa);
				DrawInsertMarker(pa.Graphics);
			}
			else
			{
				if (SuspendLayout)
				{
					return;
				}
				Graphics graphics = pa.Graphics;
				if (m_NeedRecalcSize)
				{
					RecalcSize();
				}
				Rectangle panelRect = m_PanelRect;
				panelRect.Offset(m_Rect.X, m_Rect.Y);
				bool disposeFont = false;
				Font font = GetFont(out disposeFont);
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				Image image = GetImage();
				Rectangle rectangle = panelRect;
				rectangle.X += m_Margin;
				rectangle.X += m_Margin * 2;
				rectangle.Width -= m_Margin * 3;
				if (image != null)
				{
					rectangle.Width -= image.Width + m_Margin;
					rectangle.X += image.Width + m_Margin;
				}
				if (m_ExpandButtonVisible)
				{
					rectangle.Width -= 25;
				}
				ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo();
				elementStyleDisplayInfo.Bounds = panelRect;
				elementStyleDisplayInfo.Graphics = graphics;
				if (m_MouseOver)
				{
					elementStyleDisplayInfo.Style = m_TitleHotStyle;
				}
				else
				{
					elementStyleDisplayInfo.Style = m_TitleStyle;
				}
				ElementStyleDisplay.Paint(elementStyleDisplayInfo);
				if (elementStyleDisplayInfo.Style.Font != null)
				{
					font = elementStyleDisplayInfo.Style.Font;
				}
				if (pa.RightToLeft)
				{
					TextDrawing.DrawString(graphics, m_Text, font, elementStyleDisplayInfo.Style.TextColor, GetRtlRectangle(m_PanelRect, rectangle), elementStyleDisplayInfo.Style.TextFormat | eTextFormat.RightToLeft);
				}
				else
				{
					TextDrawing.DrawString(graphics, m_Text, font, elementStyleDisplayInfo.Style.TextColor, rectangle, elementStyleDisplayInfo.Style.TextFormat);
				}
				if (m_ExpandButtonVisible)
				{
					Rectangle expandButtonRect = m_ExpandButtonRect;
					expandButtonRect.Offset(m_Rect.X, m_Rect.Y);
					if (pa.RightToLeft)
					{
						PaintExpandButton(pa, GetRtlRectangle(m_Rect, expandButtonRect), m_MouseOver, m_MouseDown, m_Expanded);
					}
					else
					{
						PaintExpandButton(pa, expandButtonRect, m_MouseOver, m_MouseDown, m_Expanded);
					}
				}
				int cornerDiameter = m_TitleStyle.CornerDiameter;
				Rectangle bounds = new Rectangle(m_Rect.X, m_PanelRect.Bottom + m_Rect.Y, m_Rect.Width, m_Rect.Height - m_PanelRect.Bottom);
				if (bounds.Width > 0 && bounds.Height > 0)
				{
					elementStyleDisplayInfo.Style = m_BackStyle;
					elementStyleDisplayInfo.Bounds = bounds;
					ElementStyleDisplay.Paint(elementStyleDisplayInfo);
					if (m_DropShadow)
					{
						ShadowPaintInfo shadowPaintInfo = GetShadowPaintInfo();
						shadowPaintInfo.Rectangle = new Rectangle(m_Rect.X, m_Rect.Y + m_PanelRect.Top + cornerDiameter, m_Rect.Width, m_Rect.Height - m_PanelRect.Top - cornerDiameter);
						shadowPaintInfo.Graphics = graphics;
						ShadowPainter.Paint(shadowPaintInfo);
					}
				}
				else if (m_DropShadow)
				{
					panelRect.Y += cornerDiameter;
					panelRect.Height -= cornerDiameter;
					ShadowPaintInfo shadowPaintInfo2 = GetShadowPaintInfo();
					shadowPaintInfo2.Rectangle = panelRect;
					shadowPaintInfo2.Graphics = graphics;
					ShadowPainter.Paint(shadowPaintInfo2);
				}
				if (image != null)
				{
					if (pa.RightToLeft)
					{
						graphics.DrawImage(image, GetRtlRectangle(m_PanelRect, new Rectangle(panelRect.Left + m_Margin, panelRect.Bottom - m_Margin - image.Height, image.Width, image.Height)));
					}
					else
					{
						graphics.DrawImage(image, panelRect.Left + m_Margin, panelRect.Bottom - m_Margin - image.Height, image.Width, image.Height);
					}
				}
				if (Focused)
				{
					if (DesignMode)
					{
						Rectangle r = panelRect;
						r.Inflate(-1, -1);
						if (pa.RightToLeft)
						{
							r = GetRtlRectangle(m_PanelRect, r);
						}
						DesignTime.DrawDesignTimeSelection(graphics, panelRect, pa.Colors.ItemDesignTimeBorder);
					}
					else
					{
						Rectangle rectangle2 = rectangle;
						rectangle2.Inflate(0, -1);
						rectangle2.Width -= 2;
						rectangle2.X -= 2;
						if (pa.RightToLeft)
						{
							rectangle2 = GetRtlRectangle(m_PanelRect, rectangle2);
						}
						ControlPaint.DrawFocusRectangle(graphics, rectangle2);
					}
				}
				if ((Expanded || (Parent is ExplorerBarContainerItem && ((ExplorerBarContainerItem)Parent)._Animating)) && m_SubItems != null)
				{
					panelRect = new Rectangle(m_Rect.X, m_Rect.Y + panelRect.Height + 1, m_Rect.Width, m_Rect.Height - panelRect.Height - 1);
					for (int i = 0; i < m_SubItems.Count; i++)
					{
						BaseItem baseItem = m_SubItems[i];
						if (!baseItem.Displayed || !baseItem.Visible)
						{
							continue;
						}
						if (baseItem.BeginGroup)
						{
							Pen val2 = new Pen(pa.Colors.ItemSeparator, 1f);
							try
							{
								graphics.DrawLine(val2, baseItem.LeftInternal + 2, baseItem.TopInternal - 2, baseItem.DisplayRectangle.Right - 4, baseItem.TopInternal - 2);
							}
							finally
							{
								((IDisposable)val2)?.Dispose();
							}
						}
						baseItem.Paint(pa);
					}
				}
				if (disposeFont)
				{
					font.Dispose();
				}
				DrawInsertMarker(pa.Graphics);
				PaintInfoText(pa);
			}
		}
		finally
		{
			m_IsPainting = false;
		}
	}

	private Rectangle GetRtlRectangle(Rectangle bounds, Rectangle r)
	{
		return new Rectangle(bounds.Right - (r.Width + (r.X - bounds.X)), r.Y, r.Width, r.Height);
	}

	private void PaintThemed(ItemPaintArgs pa)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		if (SuspendLayout)
		{
			return;
		}
		if (m_NeedRecalcSize)
		{
			RecalcSize();
		}
		Bitmap val = new Bitmap(DisplayRectangle.Width, DisplayRectangle.Height, pa.Graphics);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		ThemeExplorerBar themeExplorerBar = pa.ThemeExplorerBar;
		Rectangle panelRect = m_PanelRect;
		ThemeExplorerBarParts themeExplorerBarParts = ThemeExplorerBarParts.NormalGroupHead;
		ThemeExplorerBarStates normalGroupHeadNormal = ThemeExplorerBarStates.NormalGroupHeadNormal;
		if (m_XPSpecialGroup)
		{
			themeExplorerBarParts = ThemeExplorerBarParts.SpecialGroupHead;
		}
		themeExplorerBar.DrawBackground(val2, themeExplorerBarParts, normalGroupHeadNormal, panelRect);
		Rectangle r = new Rectangle(0, m_PanelRect.Bottom, m_Rect.Width + (m_XPSpecialGroup ? 0 : 0), m_Rect.Height - m_PanelRect.Bottom);
		if (r.Width > 0 && r.Height > 0)
		{
			ThemeExplorerBarParts part;
			ThemeExplorerBarStates state;
			if (m_XPSpecialGroup)
			{
				part = ThemeExplorerBarParts.SpecialGroupBackground;
				state = ThemeExplorerBarStates.SpecialGroupBackgroundNormal;
			}
			else
			{
				part = ThemeExplorerBarParts.NormalGroupBackground;
				state = ThemeExplorerBarStates.NormalGroupBackgroundNormal;
			}
			themeExplorerBar.DrawBackground(val2, part, state, r);
		}
		Image image = GetImage();
		Rectangle rectangle = panelRect;
		rectangle.X += m_Margin;
		rectangle.X += m_Margin * 2;
		rectangle.Width -= m_Margin * 5;
		if (image != null)
		{
			rectangle.Width -= image.Width + m_Margin;
			rectangle.X += image.Width + m_Margin;
		}
		if (m_ExpandButtonVisible)
		{
			rectangle.Width -= 25;
		}
		bool disposeFont = false;
		Font font = GetFont(out disposeFont);
		ElementStyle elementStyle = m_TitleStyle;
		if (m_MouseOver)
		{
			elementStyle = m_TitleHotStyle;
		}
		if (elementStyle.Font != null)
		{
			font = elementStyle.Font;
		}
		if (pa.RightToLeft)
		{
			TextDrawing.DrawStringLegacy(val2, m_Text, font, elementStyle.TextColor, GetRtlRectangle(m_PanelRect, rectangle), elementStyle.TextFormat | eTextFormat.RightToLeft);
		}
		else
		{
			TextDrawing.DrawStringLegacy(val2, m_Text, font, elementStyle.TextColor, rectangle, elementStyle.TextFormat);
		}
		if (m_ExpandButtonVisible)
		{
			ThemeExplorerBarParts themeExplorerBarParts2 = themeExplorerBarParts;
			Rectangle expandButtonRect = m_ExpandButtonRect;
			if (m_XPSpecialGroup)
			{
				if (Expanded)
				{
					themeExplorerBarParts = ThemeExplorerBarParts.SpecialGroupCollapse;
					normalGroupHeadNormal = ((!m_ExpandButtonHot) ? ThemeExplorerBarStates.SpecialGroupCollapseNormal : ThemeExplorerBarStates.SpecialGroupCollapseHot);
				}
				else
				{
					themeExplorerBarParts = ThemeExplorerBarParts.SpecialGroupExpand;
					normalGroupHeadNormal = ((!m_ExpandButtonHot) ? ThemeExplorerBarStates.SpecialGroupExpandNormal : ThemeExplorerBarStates.SpecialGroupExpandHot);
				}
			}
			else if (Expanded)
			{
				themeExplorerBarParts = ThemeExplorerBarParts.NormalGroupCollapse;
				normalGroupHeadNormal = ((!m_ExpandButtonHot) ? ThemeExplorerBarStates.NormalGroupCollapseNormal : ThemeExplorerBarStates.NormalGroupCollapseHot);
			}
			else
			{
				themeExplorerBarParts = ThemeExplorerBarParts.NormalGroupExpand;
				normalGroupHeadNormal = ((!m_ExpandButtonHot) ? ThemeExplorerBarStates.NormalGroupExpandNormal : ThemeExplorerBarStates.NormalGroupExpandHot);
			}
			if (pa.RightToLeft)
			{
				themeExplorerBar.DrawBackground(val2, themeExplorerBarParts, normalGroupHeadNormal, GetRtlRectangle(m_Rect, expandButtonRect));
			}
			else
			{
				themeExplorerBar.DrawBackground(val2, themeExplorerBarParts, normalGroupHeadNormal, expandButtonRect);
			}
		}
		if (image != null)
		{
			if (pa.RightToLeft)
			{
				val2.DrawImage(image, GetRtlRectangle(m_PanelRect, new Rectangle(panelRect.Left + m_Margin, panelRect.Bottom - m_Margin - image.Height, image.Width, image.Height)));
			}
			else
			{
				val2.DrawImage(image, panelRect.Left + m_Margin, panelRect.Bottom - m_Margin - image.Height, image.Width, image.Height);
			}
		}
		if (Focused)
		{
			if (DesignMode)
			{
				Rectangle r2 = panelRect;
				r2.Inflate(-1, -1);
				if (pa.RightToLeft)
				{
					r2 = GetRtlRectangle(m_PanelRect, r2);
				}
				DesignTime.DrawDesignTimeSelection(val2, panelRect, pa.Colors.ItemDesignTimeBorder);
			}
			else
			{
				Rectangle rectangle2 = rectangle;
				rectangle2.Inflate(0, -1);
				rectangle2.Width -= 2;
				rectangle2.X -= 2;
				if (pa.RightToLeft)
				{
					rectangle2 = GetRtlRectangle(m_PanelRect, rectangle2);
				}
				ControlPaint.DrawFocusRectangle(val2, rectangle2);
			}
		}
		val2.Dispose();
		val2 = pa.Graphics;
		val2.DrawImage((Image)(object)val, DisplayRectangle.X, DisplayRectangle.Y, ((Image)val).Width, ((Image)val).Height);
		((Image)val).Dispose();
		if (disposeFont)
		{
			font.Dispose();
		}
		if ((Expanded || (Parent is ExplorerBarContainerItem && ((ExplorerBarContainerItem)Parent)._Animating)) && m_SubItems != null)
		{
			panelRect = new Rectangle(m_Rect.X, m_Rect.Y + panelRect.Height + 1, m_Rect.Width, m_Rect.Height - panelRect.Height - 1);
			for (int i = 0; i < m_SubItems.Count; i++)
			{
				BaseItem baseItem = m_SubItems[i];
				if (baseItem.Displayed && baseItem.Visible)
				{
					baseItem.Paint(pa);
				}
			}
		}
		PaintInfoText(pa);
	}

	private void PaintInfoText(ItemPaintArgs pa)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		if (m_SubItems != null && m_SubItems.Count == 0 && DesignMode)
		{
			Rectangle rect = m_Rect;
			if (!m_PanelRect.IsEmpty)
			{
				rect.Y += m_PanelRect.Height;
				rect.Height -= m_PanelRect.Height;
			}
			string text = "Right-click header and choose one of Create commands or use SubItems collection to create new buttons. You can also drag & drop controls on this panel.";
			rect.Inflate(-1, -1);
			eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
			Font val = new Font(SystemFonts.DefaultFont.FontFamily, 7f);
			TextDrawing.DrawString(pa.Graphics, text, val, SystemColors.ControlText, rect, format);
			val.Dispose();
		}
	}

	private void PaintThemeExpandButton()
	{
		if (!m_ExpandButtonVisible)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (!BaseItem.IsHandleValid(val))
		{
			return;
		}
		Graphics val2 = BarFunctions.CreateGraphics(val);
		try
		{
			ThemeExplorerBar themeExplorerBar = null;
			bool flag = false;
			if (val is IThemeCache)
			{
				themeExplorerBar = ((IThemeCache)val).ThemeExplorerBar;
			}
			else
			{
				flag = true;
				themeExplorerBar = new ThemeExplorerBar(val);
			}
			ThemeExplorerBarParts normalGroupCollapse = ThemeExplorerBarParts.NormalGroupCollapse;
			ThemeExplorerBarStates normalGroupCollapseNormal = ThemeExplorerBarStates.NormalGroupCollapseNormal;
			Rectangle expandButtonRect = m_ExpandButtonRect;
			expandButtonRect.Offset(m_Rect.X, m_Rect.Y);
			if (m_XPSpecialGroup)
			{
				if (Expanded)
				{
					normalGroupCollapse = ThemeExplorerBarParts.SpecialGroupCollapse;
					normalGroupCollapseNormal = ((!m_ExpandButtonHot) ? ThemeExplorerBarStates.SpecialGroupCollapseNormal : ThemeExplorerBarStates.SpecialGroupCollapseHot);
				}
				else
				{
					normalGroupCollapse = ThemeExplorerBarParts.SpecialGroupExpand;
					normalGroupCollapseNormal = ((!m_ExpandButtonHot) ? ThemeExplorerBarStates.SpecialGroupExpandNormal : ThemeExplorerBarStates.SpecialGroupExpandHot);
				}
			}
			else if (Expanded)
			{
				normalGroupCollapse = ThemeExplorerBarParts.NormalGroupCollapse;
				normalGroupCollapseNormal = ((!m_ExpandButtonHot) ? ThemeExplorerBarStates.NormalGroupCollapseNormal : ThemeExplorerBarStates.NormalGroupCollapseHot);
			}
			else
			{
				normalGroupCollapse = ThemeExplorerBarParts.NormalGroupExpand;
				normalGroupCollapseNormal = ((!m_ExpandButtonHot) ? ThemeExplorerBarStates.NormalGroupExpandNormal : ThemeExplorerBarStates.NormalGroupExpandHot);
			}
			themeExplorerBar.DrawBackground(val2, normalGroupCollapse, normalGroupCollapseNormal, expandButtonRect);
			if (flag)
			{
				themeExplorerBar.Dispose();
			}
		}
		finally
		{
			if (val2 != null)
			{
				val2.SmoothingMode = (SmoothingMode)0;
				val2.TextRenderingHint = (TextRenderingHint)0;
				val2.Dispose();
			}
		}
	}

	private void PaintExpandButton()
	{
		if (m_ExpandButtonVisible)
		{
			Rectangle expandButtonRect = m_ExpandButtonRect;
			expandButtonRect.Offset(m_Rect.X, m_Rect.Y);
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Invalidate(expandButtonRect);
				val.Update();
			}
		}
	}

	private void PaintExpandButton(ItemPaintArgs pa, Rectangle r, bool bHot, bool bPressed, bool bExpanded)
	{
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		if (pa.ContainerControl is ExplorerBar explorerBar && ((explorerBar.GroupButtonExpandNormal != null && !bExpanded) || (explorerBar.GroupButtonCollapseNormal != null && bExpanded)))
		{
			Image val = explorerBar.GroupButtonExpandNormal;
			if (bExpanded)
			{
				val = explorerBar.GroupButtonCollapseNormal;
			}
			if (bPressed)
			{
				if (!bExpanded && explorerBar.GroupButtonExpandPressed != null)
				{
					val = explorerBar.GroupButtonExpandPressed;
				}
				else if (bExpanded && explorerBar.GroupButtonCollapsePressed != null)
				{
					val = explorerBar.GroupButtonCollapsePressed;
				}
			}
			else if (bHot)
			{
				if (!bExpanded && explorerBar.GroupButtonExpandHot != null)
				{
					val = explorerBar.GroupButtonExpandHot;
				}
				else if (bExpanded && explorerBar.GroupButtonCollapseHot != null)
				{
					val = explorerBar.GroupButtonCollapseHot;
				}
			}
			Rectangle rectangle = r;
			rectangle.Y += (rectangle.Height - val.Height) / 2;
			graphics.DrawImageUnscaled(val, rectangle);
			return;
		}
		Pen val2 = new Pen(bHot ? m_ExpandHotBorderColor : m_ExpandBorderColor, 1f);
		try
		{
			if (r.Width > 16)
			{
				r.Offset((r.Width - 16) / 2, 0);
			}
			if (r.Height > 16)
			{
				r.Offset(0, (r.Height - 16) / 2);
			}
			r.Width = 16;
			r.Height = 16;
			Brush val3 = (Brush)new SolidBrush(bHot ? m_ExpandHotBackColor : m_ExpandBackColor);
			graphics.FillEllipse(val3, r);
			val3.Dispose();
			graphics.DrawEllipse(val2, r);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		Pen val4 = new Pen(bHot ? m_ExpandHotForeColor : m_ExpandForeColor, 1f);
		try
		{
			if (bExpanded)
			{
				Point point = new Point(r.X + 8, r.Y + 4);
				graphics.DrawLine(val4, point.X, point.Y, point.X - 3, point.Y + 3);
				graphics.DrawLine(val4, point.X, point.Y, point.X + 3, point.Y + 3);
				graphics.DrawLine(val4, point.X, point.Y + 1, point.X - 3, point.Y + 4);
				graphics.DrawLine(val4, point.X, point.Y + 1, point.X + 3, point.Y + 4);
				point.Y += 4;
				graphics.DrawLine(val4, point.X, point.Y, point.X - 3, point.Y + 3);
				graphics.DrawLine(val4, point.X, point.Y, point.X + 3, point.Y + 3);
				graphics.DrawLine(val4, point.X, point.Y + 1, point.X - 3, point.Y + 4);
				graphics.DrawLine(val4, point.X, point.Y + 1, point.X + 3, point.Y + 4);
			}
			else
			{
				Point point2 = new Point(r.X + 8, r.Y + 8);
				graphics.DrawLine(val4, point2.X, point2.Y, point2.X - 3, point2.Y - 3);
				graphics.DrawLine(val4, point2.X, point2.Y, point2.X + 3, point2.Y - 3);
				graphics.DrawLine(val4, point2.X, point2.Y - 1, point2.X - 3, point2.Y - 4);
				graphics.DrawLine(val4, point2.X, point2.Y - 1, point2.X + 3, point2.Y - 4);
				point2.Y += 4;
				graphics.DrawLine(val4, point2.X, point2.Y, point2.X - 3, point2.Y - 3);
				graphics.DrawLine(val4, point2.X, point2.Y, point2.X + 3, point2.Y - 3);
				graphics.DrawLine(val4, point2.X, point2.Y - 1, point2.X - 3, point2.Y - 4);
				graphics.DrawLine(val4, point2.X, point2.Y - 1, point2.X + 3, point2.Y - 4);
			}
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
	}

	public override void SubItemSizeChanged(BaseItem objChildItem)
	{
		NeedRecalcSize = true;
	}

	private void PaintArrow(Graphics g, Rectangle rect, Color c, bool up)
	{
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		Point[] array = new Point[3];
		if (up)
		{
			array[0].X = rect.Left + (rect.Width - 9) / 2;
			array[0].Y = rect.Top + rect.Height / 2 + 1;
			array[1].X = array[0].X + 8;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + 4;
			array[2].Y = array[0].Y - 5;
		}
		else
		{
			array[0].X = rect.Left + (rect.Width - 7) / 2;
			array[0].Y = rect.Top + (rect.Height - 4) / 2;
			array[1].X = array[0].X + 7;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + 3;
			array[2].Y = array[0].Y + 4;
		}
		g.FillPolygon((Brush)new SolidBrush(c), array);
	}

	protected virtual Font GetFont(out bool disposeFont)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		disposeFont = false;
		if (TitleStyle.Font != null)
		{
			return TitleStyle.Font;
		}
		Font val = null;
		object containerControl = ContainerControl;
		Control val2 = (Control)((containerControl is Control) ? containerControl : null);
		if (val2 != null)
		{
			val = val2.Font;
		}
		else
		{
			val = SystemFonts.DefaultFont;
			disposeFont = true;
		}
		if (!val.Bold)
		{
			val = new Font(val, (FontStyle)1);
			disposeFont = true;
		}
		return val;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576 && (Math.Abs(objArg.X - m_MouseDownPt.X) >= 4 || Math.Abs(objArg.Y - m_MouseDownPt.Y) >= 4) && ContainerControl is ExplorerBar { AllowUserCustomize: not false } && CanCustomize)
		{
			BaseItem hotSubItem = m_HotSubItem;
			if (hotSubItem != null && hotSubItem.CanCustomize && !hotSubItem.SystemItem && GetOwner() is IOwner { DragItem: null } owner)
			{
				owner.StartItemDrag(hotSubItem);
				return;
			}
		}
		Rectangle panelRect = m_PanelRect;
		panelRect.Offset(m_Rect.Location);
		if (panelRect.Contains(objArg.X, objArg.Y))
		{
			if (!m_MouseOver)
			{
				m_MouseOver = true;
				if (m_HeaderExpands)
				{
					Cursor = Cursors.Hand;
				}
				Refresh();
			}
		}
		else if (m_MouseOver)
		{
			if (m_HeaderExpands)
			{
				Cursor = Cursors.Default;
			}
			m_MouseOver = false;
			Refresh();
		}
		Rectangle expandButtonRect = m_ExpandButtonRect;
		expandButtonRect.Offset(m_Rect.Location);
		if (m_ExpandButtonVisible && expandButtonRect.Contains(objArg.X, objArg.Y))
		{
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseLeave();
				m_HotSubItem = null;
			}
			m_ExpandButtonHot = true;
			PaintExpandButton();
		}
		else
		{
			if (m_ExpandButtonHot)
			{
				m_ExpandButtonHot = false;
				PaintExpandButton();
			}
			base.InternalMouseMove(objArg);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseLeave()
	{
		Cursor = Cursors.Default;
		base.InternalMouseLeave();
		m_MouseOver = false;
		m_MouseDown = false;
		if (m_ExpandButtonHot)
		{
			m_ExpandButtonHot = false;
		}
		Refresh();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Invalid comparison between Unknown and I4
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Invalid comparison between Unknown and I4
		Rectangle panelRect = m_PanelRect;
		panelRect.Offset(m_Rect.Location);
		m_MouseDownPt = new Point(objArg.X, objArg.Y);
		if (DesignMode)
		{
			if (ItemAtLocation(objArg.X, objArg.Y) == null && !panelRect.Contains(objArg.X, objArg.Y) && Expanded)
			{
				if (GetOwner() is IOwner owner)
				{
					owner.SetFocusItem(null);
				}
			}
			else if (!panelRect.Contains(objArg.X, objArg.Y))
			{
				base.InternalMouseDown(objArg);
			}
			return;
		}
		Rectangle expandButtonRect = m_ExpandButtonRect;
		expandButtonRect.Offset(m_Rect.X, m_Rect.Y);
		if (m_ExpandButtonVisible && (int)objArg.Button == 1048576 && expandButtonRect.Contains(objArg.X, objArg.Y))
		{
			ExpandButtonClick();
			return;
		}
		base.InternalMouseDown(objArg);
		if ((int)objArg.Button == 1048576 && panelRect.Contains(objArg.X, objArg.Y))
		{
			m_MouseDown = true;
			if (m_HeaderExpands)
			{
				Expanded = !Expanded;
			}
			else
			{
				Refresh();
			}
		}
	}

	internal void ExpandButtonClick()
	{
		Expanded = !Expanded;
	}

	protected internal override void OnExpandChange()
	{
		base.OnExpandChange();
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible)
			{
				subItem.Displayed = Expanded;
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576)
		{
			m_MouseDown = false;
			Refresh();
		}
		base.InternalMouseUp(objArg);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		base.InternalClick(mb, mpos);
		if (GetOwner() is IOwner)
		{
			_ = ((IOwner)GetOwner()).DragInProgress;
		}
	}

	protected internal override void OnSubItemsClear()
	{
		base.OnSubItemsClear();
		NeedRecalcSize = true;
		if (DesignMode)
		{
			Refresh();
		}
	}

	protected override void OnTopLocationChanged(int oldValue)
	{
		int num = m_Rect.Top - oldValue;
		if (m_SubItems == null)
		{
			return;
		}
		foreach (BaseItem subItem in m_SubItems)
		{
			if (subItem.Visible)
			{
				subItem.TopInternal += num;
			}
		}
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		base.OnContainerChanged(objOldContainer);
		if (ContainerControl is ExplorerBar)
		{
			ExplorerBar explorerBar = ContainerControl as ExplorerBar;
			m_TitleStyle.SetColorScheme(explorerBar.ColorScheme);
			m_TitleHotStyle.SetColorScheme(explorerBar.ColorScheme);
			m_BackStyle.SetColorScheme(explorerBar.ColorScheme);
		}
	}

	protected internal override void OnVisibleChanged(bool bVisible)
	{
		base.OnVisibleChanged(bVisible);
		if (bVisible)
		{
			return;
		}
		foreach (BaseItem subItem in SubItems)
		{
			subItem.Displayed = false;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandBorderColor()
	{
		if (m_ExpandBorderColor != Color.DarkGray)
		{
			return m_StockStyle == eExplorerBarStockStyle.Custom;
		}
		return false;
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandBackColor()
	{
		if (m_ExpandBackColor != Color.White)
		{
			return m_StockStyle == eExplorerBarStockStyle.Custom;
		}
		return false;
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandForeColor()
	{
		if (m_ExpandForeColor != SystemColors.ActiveCaption)
		{
			return m_StockStyle == eExplorerBarStockStyle.Custom;
		}
		return false;
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandHotBorderColor()
	{
		if (m_ExpandHotBorderColor != Color.DarkGray)
		{
			return m_StockStyle == eExplorerBarStockStyle.Custom;
		}
		return false;
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandHotBackColor()
	{
		if (m_ExpandHotBackColor != Color.White)
		{
			return m_StockStyle == eExplorerBarStockStyle.Custom;
		}
		return false;
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandHotForeColor()
	{
		if (m_ExpandHotForeColor != SystemColors.ActiveCaption)
		{
			return m_StockStyle == eExplorerBarStockStyle.Custom;
		}
		return false;
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("stockstyle", XmlConvert.ToString((int)m_StockStyle));
		itemXmlElement.SetAttribute("expvisible", XmlConvert.ToString(m_ExpandButtonVisible));
		itemXmlElement.SetAttribute("subitemsmargin", XmlConvert.ToString(m_SubItemsMargin));
		itemXmlElement.SetAttribute("xpspecial", XmlConvert.ToString(m_XPSpecialGroup));
		itemXmlElement.SetAttribute("expborder", BarFunctions.ColorToString(m_ExpandBorderColor));
		itemXmlElement.SetAttribute("expbc", BarFunctions.ColorToString(m_ExpandBackColor));
		itemXmlElement.SetAttribute("expfc", BarFunctions.ColorToString(m_ExpandForeColor));
		itemXmlElement.SetAttribute("exphborder", BarFunctions.ColorToString(m_ExpandHotBorderColor));
		itemXmlElement.SetAttribute("exphbc", BarFunctions.ColorToString(m_ExpandHotBackColor));
		itemXmlElement.SetAttribute("exphfc", BarFunctions.ColorToString(m_ExpandHotForeColor));
		itemXmlElement.SetAttribute("headerexp", XmlConvert.ToString(m_HeaderExpands));
		itemXmlElement.SetAttribute("expanded", XmlConvert.ToString(m_Expanded));
		if (m_StockStyle == eExplorerBarStockStyle.Custom)
		{
			XmlElement xmlElement = itemXmlElement.OwnerDocument.CreateElement("backstyle");
			itemXmlElement.AppendChild(xmlElement);
			SerializeElementStyle(m_BackStyle, xmlElement);
		}
		if (m_StockStyle == eExplorerBarStockStyle.Custom)
		{
			XmlElement xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("headerhotstyle");
			itemXmlElement.AppendChild(xmlElement2);
			SerializeElementStyle(m_TitleHotStyle, xmlElement2);
		}
		if (m_StockStyle == eExplorerBarStockStyle.Custom)
		{
			XmlElement xmlElement3 = itemXmlElement.OwnerDocument.CreateElement("headerstyle");
			itemXmlElement.AppendChild(xmlElement3);
			SerializeElementStyle(m_TitleStyle, xmlElement3);
		}
		if (m_ImageIndex != -1)
		{
			itemXmlElement.SetAttribute("imageindex", XmlConvert.ToString(m_ImageIndex));
		}
		else if (m_Image != null)
		{
			XmlElement xmlElement4 = itemXmlElement.OwnerDocument.CreateElement("image");
			itemXmlElement.AppendChild(xmlElement4);
			BarFunctions.SerializeImage(m_Image, xmlElement4);
		}
	}

	private void SerializeElementStyle(ElementStyle style, XmlElement xmlElement)
	{
		ElementSerializer.Serialize(style, xmlElement);
	}

	private void DeserializeElementStyle(ElementStyle style, XmlElement xmlElement)
	{
		ElementSerializer.Deserialize(style, xmlElement);
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		m_StockStyle = (eExplorerBarStockStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("stockstyle"));
		m_ExpandButtonVisible = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("expvisible"));
		m_SubItemsMargin = XmlConvert.ToInt32(itemXmlElement.GetAttribute("subitemsmargin"));
		m_XPSpecialGroup = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("xpspecial"));
		m_ExpandBorderColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("expborder"));
		m_ExpandBackColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("expbc"));
		m_ExpandForeColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("expfc"));
		m_ExpandHotBorderColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("exphborder"));
		m_ExpandHotBackColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("exphbc"));
		m_ExpandHotForeColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("exphfc"));
		m_HeaderExpands = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("headerexp"));
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			switch (childNode.Name)
			{
			case "backstyle":
				DeserializeElementStyle(m_BackStyle, childNode);
				break;
			case "headerhotstyle":
				DeserializeElementStyle(m_TitleHotStyle, childNode);
				break;
			case "headerstyle":
				DeserializeElementStyle(m_TitleStyle, childNode);
				break;
			case "image":
				m_Image = BarFunctions.DeserializeImage(childNode);
				break;
			}
		}
		if (itemXmlElement.HasAttribute("imageindex"))
		{
			ImageIndex = XmlConvert.ToInt32(itemXmlElement.GetAttribute("imageindex"));
		}
		RefreshImageSize();
		Expanded = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("expanded"));
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		NeedRecalcSize = true;
		if (item is ButtonItem)
		{
			((ButtonItem)item)._FitContainer = m_WordWrapSubItems;
		}
		base.OnItemAdded(item);
		if (!Expanded)
		{
			if (!item.Displayed)
			{
				item.Displayed = true;
			}
			item.Displayed = false;
		}
		if (item is ButtonItem)
		{
			ButtonItem buttonItem = item as ButtonItem;
			buttonItem.ThemeAware = false;
			if (buttonItem.ForeColor.IsEmpty && buttonItem.HotForeColor.IsEmpty)
			{
				BarFunctions.SetExplorerBarStyle(item as ButtonItem, m_StockStyle);
			}
		}
		item.NeedRecalcSize = true;
		if (DesignMode)
		{
			Refresh();
		}
	}

	protected internal override void OnBeforeItemRemoved(BaseItem item)
	{
		if (item is ButtonItem)
		{
			((ButtonItem)item)._FitContainer = false;
		}
		base.OnBeforeItemRemoved(item);
	}

	public override void ShowToolTip()
	{
		if (DesignMode || !Visible || !Displayed)
		{
			return;
		}
		IOwner owner = GetOwner() as IOwner;
		if (owner != null && !owner.ShowToolTips)
		{
			return;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null)
		{
			return;
		}
		Rectangle panelRect = m_PanelRect;
		panelRect.Offset(m_Rect.X, m_Rect.Y);
		if (!panelRect.Contains(val.PointToClient(Control.MousePosition)))
		{
			return;
		}
		OnTooltip(bShow: true);
		if (m_Tooltip != "")
		{
			if (m_ToolTipWnd == null)
			{
				m_ToolTipWnd = new ToolTip();
			}
			m_ToolTipWnd.Text = m_Tooltip;
			if (owner != null && owner.ShowShortcutKeysInToolTips && Shortcuts.Count > 0)
			{
				ToolTip toolTipWnd = m_ToolTipWnd;
				toolTipWnd.Text = toolTipWnd.Text + " (" + base.ShortcutString + ")";
			}
			GetIOwnerItemEvents()?.InvokeToolTipShowing(this, new EventArgs());
			m_ToolTipWnd.ShowToolTip();
		}
	}

	public override void Refresh()
	{
		if (SuspendLayout)
		{
			return;
		}
		if (!Expanded)
		{
			base.Refresh();
		}
		else
		{
			if ((!Visible && !base.IsOnCustomizeMenu) || !Displayed)
			{
				return;
			}
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val == null || !BaseItem.IsHandleValid(val) || val is ItemsListBox)
			{
				return;
			}
			if (m_NeedRecalcSize)
			{
				RecalcSize();
				if (m_Parent != null)
				{
					m_Parent.SubItemSizeChanged(this);
				}
			}
			Rectangle panelRect = m_PanelRect;
			panelRect.Inflate(4, 4);
			val.Invalidate(m_Rect, false);
		}
	}

	protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem dragItem)
	{
		return GetContainerInsertPosition(pScreen, dragItem);
	}

	void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool Before)
	{
		DesignTimeProviderContainer.DrawReversibleMarker(this, iPos, Before);
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
		DesignTimeProviderContainer.InsertItemAt(this, objItem, iPos, Before);
	}

	private int GetAppendPosition(BaseItem objParent)
	{
		int result = -1;
		int num = objParent.SubItems.Count - 1;
		while (num >= 0 && objParent.SubItems[num].SystemItem)
		{
			result = num;
			num--;
		}
		return result;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void SetDesignTimeDefaults(ButtonItem button, eExplorerBarStockStyle e)
	{
		BarFunctions.SetExplorerBarStyle(button, e);
		button.Text = "New Button";
		button.ButtonStyle = eButtonStyle.ImageAndText;
		button.ImagePosition = eImagePosition.Left;
		button.HotTrackingStyle = eHotTrackingStyle.None;
		button.HotFontUnderline = true;
		button.Cursor = Cursors.Hand;
	}

	public void SetDefaultAppearance()
	{
		StockStyle = eExplorerBarStockStyle.SystemColors;
		BackStyle.BackColorSchemePart = eColorSchemePart.BarBackground;
		BackStyle.BorderLeft = eStyleBorderType.Solid;
		BackStyle.BorderRight = eStyleBorderType.Solid;
		BackStyle.BorderTop = eStyleBorderType.Solid;
		BackStyle.BorderBottom = eStyleBorderType.Solid;
		BackStyle.BorderLeftColor = SystemColors.Window;
		BackStyle.BorderRightColor = SystemColors.Window;
		BackStyle.BorderTopColor = SystemColors.Window;
		BackStyle.BorderBottomColor = SystemColors.Window;
		BackStyle.BorderLeftWidth = 1;
		BackStyle.BorderRightWidth = 1;
		BackStyle.BorderTopWidth = 1;
		BackStyle.BorderBottomWidth = 1;
		ExpandBackColor = SystemColors.Window;
		ExpandBorderColor = SystemColors.InactiveCaption;
		ExpandForeColor = SystemColors.Highlight;
		ExpandHotBackColor = SystemColors.Window;
		ExpandHotForeColor = SystemColors.ActiveCaption;
		ExpandHotBorderColor = SystemColors.ActiveCaption;
		TitleHotStyle.BackColor = SystemColors.Window;
		TitleHotStyle.BackColor2 = SystemColors.InactiveCaption;
		TitleHotStyle.TextColor = SystemColors.ActiveCaption;
		TitleStyle.BackColor = SystemColors.Window;
		TitleStyle.BackColor2SchemePart = eColorSchemePart.BarBackground2;
		TitleStyle.TextColor = SystemColors.ControlText;
		ThemeAware = true;
	}
}
