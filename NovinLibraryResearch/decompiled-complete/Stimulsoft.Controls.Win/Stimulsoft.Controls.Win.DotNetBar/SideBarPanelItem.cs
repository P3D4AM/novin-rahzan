using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class SideBarPanelItem : ImageItem, IDesignTimeProvider
{
	private int m_TopItemIndex;

	private bool m_ScrollDownButton;

	private Rectangle[] m_ScrollButtonRect = new Rectangle[2]
	{
		Rectangle.Empty,
		Rectangle.Empty
	};

	private int m_ScrollButtonHot = -1;

	private int m_Margin = 3;

	private bool m_MouseOver;

	private bool m_MouseDown;

	private Rectangle m_PanelRect = Rectangle.Empty;

	private Color m_ForeColor = Color.Empty;

	private Color m_HotForeColor = Color.Empty;

	private bool m_HotFontUnderline;

	private bool m_HotFontBold;

	private bool m_FontBold;

	private eBarImageSize m_ItemImageSize;

	private Point m_MouseDownPt;

	private SideBarPanelControlHost m_ControlHost;

	private ItemStyle m_BackgroundStyle = new ItemStyle();

	private ItemStyle m_HeaderStyle;

	private ItemStyle m_HeaderHotStyle;

	private ItemStyle m_HeaderMouseDownStyle;

	private ItemStyle m_HeaderSideStyle;

	private ItemStyle m_HeaderSideHotStyle;

	private ItemStyle m_HeaderSideMouseDownStyle;

	private bool m_WordWrap;

	private const string INFO_EMPTYPANEL = "Right-click header and choose Add New Button or use SubItems collection to create new buttons.";

	private Image m_Image;

	private int m_ImageIndex = -1;

	private Image m_HoverImage;

	private int m_HoverImageIndex = -1;

	private Image m_PressedImage;

	private int m_PressedImageIndex = -1;

	private Icon m_Icon;

	private Image m_ImageCachedIdx;

	private ItemPaintArgs _ItemPaintArgs;

	private eSideBarAppearance m_Appearance;

	private const int FLAT_NOIMAGESIDEWIDTH = 26;

	private const int IMAGE_MARGIN = 2;

	private bool m_IgnoreNextClick;

	private DateTime m_IgnoreClickDateTime = DateTime.MinValue;

	private Rectangle m_IgnoreScrollRect = Rectangle.Empty;

	private bool m_EnableScrollButtons = true;

	private eSideBarLayoutType m_LayoutType;

	private eStyleTextAlignment m_TextAlignment = eStyleTextAlignment.Center;

	[Browsable(false)]
	public bool IsMouseOver => m_MouseOver;

	[Browsable(false)]
	public bool IsMouseDown => m_MouseDown;

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eStyleTextAlignment.Center)]
	[Description("Specifies alignment of the text.")]
	public eStyleTextAlignment TextAlignment
	{
		get
		{
			return m_TextAlignment;
		}
		set
		{
			m_TextAlignment = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eSideBarLayoutType.Default)]
	[Category("Appearance")]
	[Description("Gets or sets the layout type for the items.")]
	public eSideBarLayoutType LayoutType
	{
		get
		{
			return m_LayoutType;
		}
		set
		{
			if (m_LayoutType != value)
			{
				m_LayoutType = value;
				NeedRecalcSize = true;
				if (DesignMode)
				{
					Refresh();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The text contained in the item.")]
	[Localizable(true)]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
			if (m_Text == "")
			{
				m_Margin = 0;
			}
			else
			{
				m_Margin = 3;
			}
			NeedRecalcSize = true;
			if (Parent != null)
			{
				Parent.NeedRecalcSize = true;
			}
			if (DesignMode)
			{
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets group background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle BackgroundStyle => m_BackgroundStyle;

	internal bool HasFlatStyle
	{
		get
		{
			if (m_HeaderStyle != null)
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets the item header style. Applies only when SideBar.Appearance is set to Flat.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle HeaderStyle
	{
		get
		{
			if (m_HeaderStyle == null)
			{
				SetHeaderStyle(new ItemStyle());
			}
			return m_HeaderStyle;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets the item header style when mouse is over the header. Applies only when SideBar.Appearance is set to Flat.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle HeaderHotStyle
	{
		get
		{
			if (m_HeaderHotStyle == null)
			{
				SetHeaderHotStyle(new ItemStyle());
			}
			return m_HeaderHotStyle;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets the item header style when left mouse button is pressed on header. Applies only when SideBar.Appearance is set to Flat.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle HeaderMouseDownStyle
	{
		get
		{
			if (m_HeaderMouseDownStyle == null)
			{
				SetHeaderMouseDownStyle(new ItemStyle());
			}
			return m_HeaderMouseDownStyle;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets the item header side style. Applies only when SideBar.Appearance is set to Flat.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle HeaderSideStyle
	{
		get
		{
			if (m_HeaderSideStyle == null)
			{
				SetHeaderSideStyle(new ItemStyle());
			}
			return m_HeaderSideStyle;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets the item header side style when mouse is over the header. Applies only when SideBar.Appearance is set to Flat.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle HeaderSideHotStyle
	{
		get
		{
			if (m_HeaderSideHotStyle == null)
			{
				SetHeaderSideHotStyle(new ItemStyle());
			}
			return m_HeaderSideHotStyle;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets the item header side style when left mouse button is pressed on header. Applies only when SideBar.Appearance is set to Flat.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle HeaderSideMouseDownStyle
	{
		get
		{
			if (m_HeaderSideMouseDownStyle == null)
			{
				SetHeaderSideMouseDownStyle(new ItemStyle());
			}
			return m_HeaderSideMouseDownStyle;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eSideBarAppearance Appearance
	{
		get
		{
			return m_Appearance;
		}
		set
		{
			if (m_Appearance != value)
			{
				m_Appearance = value;
				NeedRecalcSize = true;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies that text font is bold when text is rendered.")]
	[DefaultValue(false)]
	public bool FontBold
	{
		get
		{
			return m_FontBold;
		}
		set
		{
			if (m_FontBold != value)
			{
				m_FontBold = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "FontBold");
				}
				if (Displayed)
				{
					Refresh();
				}
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies that text font is bold when mouse is over the item.")]
	[DefaultValue(false)]
	public bool HotFontBold
	{
		get
		{
			return m_HotFontBold;
		}
		set
		{
			if (m_HotFontBold != value)
			{
				m_HotFontBold = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "HotFontBold");
				}
				if (Displayed && m_MouseOver)
				{
					Refresh();
				}
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies that text font is underlined when mouse is over the item.")]
	[DefaultValue(false)]
	public bool HotFontUnderline
	{
		get
		{
			return m_HotFontUnderline;
		}
		set
		{
			if (m_HotFontUnderline != value)
			{
				m_HotFontUnderline = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "HotFontUnderline");
				}
				if (Displayed && m_MouseOver)
				{
					Refresh();
				}
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The foreground color used to display text when mouse is over the item.")]
	public Color HotForeColor
	{
		get
		{
			return m_HotForeColor;
		}
		set
		{
			if (m_HotForeColor != value)
			{
				m_HotForeColor = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "HotForeColor");
				}
				if (Displayed && m_MouseOver)
				{
					Refresh();
				}
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The foreground color used to display text.")]
	public Color ForeColor
	{
		get
		{
			return m_ForeColor;
		}
		set
		{
			if (m_ForeColor != value)
			{
				m_ForeColor = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "ForeColor");
				}
				if (Displayed)
				{
					Refresh();
				}
			}
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

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.SideBarPanelItemEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Data")]
	[Description("Collection of sub items.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public override SubItemsCollection SubItems => base.SubItems;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eBarImageSize.Default)]
	[Category("Behavior")]
	[Description("Specifies the Image size that will be used by items on this bar.")]
	public eBarImageSize ItemImageSize
	{
		get
		{
			return m_ItemImageSize;
		}
		set
		{
			m_ItemImageSize = value;
			eImagePosition imagePosition = eImagePosition.Left;
			if (m_ItemImageSize != 0)
			{
				imagePosition = eImagePosition.Top;
			}
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is ButtonItem buttonItem)
				{
					buttonItem.ImagePosition = imagePosition;
				}
			}
			m_TopItemIndex = 0;
			RefreshImageSize();
			RecalcSize();
			Refresh();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle PanelRect => m_PanelRect;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int TopItemIndex
	{
		get
		{
			return m_TopItemIndex;
		}
		set
		{
			m_TopItemIndex = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Description("Indicates a value that determines whether text is displayed in multiple lines or one long line.")]
	[Category("Layout")]
	public bool WordWrap
	{
		get
		{
			return m_WordWrap;
		}
		set
		{
			m_WordWrap = value;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is ButtonItem)
				{
					((ButtonItem)subItem)._FitContainer = m_WordWrap;
				}
			}
			NeedRecalcSize = true;
			if (DesignMode && ContainerControl is SideBar sideBar)
			{
				sideBar.RecalcLayout();
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Specifies whether scroll buttons are displayed when content of the panel exceeds it's height.")]
	public bool EnableScrollButtons
	{
		get
		{
			return m_EnableScrollButtons;
		}
		set
		{
			m_EnableScrollButtons = value;
			if (!m_EnableScrollButtons)
			{
				NeedRecalcSize = true;
				if (DesignMode)
				{
					Refresh();
				}
			}
		}
	}

	internal bool HasScrollButtons
	{
		get
		{
			if (m_EnableScrollButtons)
			{
				if (m_ScrollButtonRect[0].IsEmpty)
				{
					return !m_ScrollButtonRect[1].IsEmpty;
				}
				return true;
			}
			return false;
		}
	}

	internal bool ScrollDownButton => m_ScrollDownButton;

	internal bool UseThemes => base.IsThemed;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ImageList ImageList
	{
		get
		{
			if (GetOwner() is IOwner owner)
			{
				return owner.Images;
			}
			return null;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the Button icon. Icons support multiple image sizes and alpha blending.")]
	[DefaultValue(null)]
	public Icon Icon
	{
		get
		{
			return m_Icon;
		}
		set
		{
			NeedRecalcSize = true;
			m_Icon = value;
			OnImageChanged();
			Refresh();
			OnAppearanceChanged();
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
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed when mouse hovers over the item.")]
	[DefaultValue(null)]
	public Image HoverImage
	{
		get
		{
			return m_HoverImage;
		}
		set
		{
			m_HoverImage = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image list image index of the image that will be displayed when mouse hovers over the item.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ImageIndexEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	public int HoverImageIndex
	{
		get
		{
			return m_HoverImageIndex;
		}
		set
		{
			if (m_HoverImageIndex != value)
			{
				m_HoverImageIndex = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "HoverImageIndex");
				}
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed when item is pressed.")]
	[DefaultValue(null)]
	public Image PressedImage
	{
		get
		{
			return m_PressedImage;
		}
		set
		{
			m_PressedImage = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image list image index of the image that will be displayed when item is pressed.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ImageIndexEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	public int PressedImageIndex
	{
		get
		{
			return m_PressedImageIndex;
		}
		set
		{
			if (m_PressedImageIndex != value)
			{
				m_PressedImageIndex = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "PressedImageIndex");
				}
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	private Size IconSize
	{
		get
		{
			Size result = new Size(16, 16);
			IBarImageSize barImageSize = null;
			if (_ItemPaintArgs != null)
			{
				barImageSize = _ItemPaintArgs.ContainerControl as IBarImageSize;
			}
			if (barImageSize == null)
			{
				barImageSize = ContainerControl as IBarImageSize;
			}
			try
			{
				if (barImageSize != null && barImageSize.ImageSize != 0)
				{
					if (barImageSize.ImageSize == eBarImageSize.Medium)
					{
						result = new Size(24, 24);
						return result;
					}
					if (barImageSize.ImageSize == eBarImageSize.Large)
					{
						result = new Size(32, 32);
						return result;
					}
				}
				else if (m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize != 0)
				{
					switch (((SideBarPanelItem)m_Parent).ItemImageSize)
					{
					case eBarImageSize.Medium:
						result = new Size(24, 24);
						return result;
					case eBarImageSize.Large:
						result = new Size(32, 32);
						return result;
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}
	}

	public SideBarPanelItem()
		: this("", "")
	{
	}

	public SideBarPanelItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public SideBarPanelItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		m_IsContainer = true;
		m_BackgroundStyle.VisualPropertyChanged += VisualPropertyChanged;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.DisposeImage(ref m_Image);
			BarUtilities.DisposeImage(ref m_ImageCachedIdx);
			BarUtilities.DisposeImage(ref m_Icon);
			BarUtilities.DisposeImage(ref m_HoverImage);
			BarUtilities.DisposeImage(ref m_PressedImage);
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		SideBarPanelItem sideBarPanelItem = new SideBarPanelItem();
		CopyToItem(sideBarPanelItem);
		return sideBarPanelItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		SideBarPanelItem sideBarPanelItem = copy as SideBarPanelItem;
		base.CopyToItem(sideBarPanelItem);
		sideBarPanelItem.HotFontBold = m_HotFontBold;
		sideBarPanelItem.HotFontUnderline = m_HotFontUnderline;
		sideBarPanelItem.HotForeColor = m_HotForeColor;
		sideBarPanelItem.ForeColor = m_ForeColor;
		sideBarPanelItem.ItemImageSize = m_ItemImageSize;
		sideBarPanelItem.SetBackgroundStyle(m_BackgroundStyle.Clone() as ItemStyle);
	}

	private void VisualPropertyChanged(object sender, EventArgs e)
	{
		RefreshItemStyleSystemColors();
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	internal void RefreshItemStyleSystemColors()
	{
		if (ContainerControl is SideBar sideBar)
		{
			if (m_BackgroundStyle != null)
			{
				m_BackgroundStyle.ApplyColorScheme(sideBar.ColorScheme);
			}
			if (m_HeaderStyle != null)
			{
				m_HeaderStyle.ApplyColorScheme(sideBar.ColorScheme);
			}
			if (m_HeaderHotStyle != null)
			{
				m_HeaderHotStyle.ApplyColorScheme(sideBar.ColorScheme);
			}
			if (m_HeaderMouseDownStyle != null)
			{
				m_HeaderMouseDownStyle.ApplyColorScheme(sideBar.ColorScheme);
			}
			if (m_HeaderSideStyle != null)
			{
				m_HeaderSideStyle.ApplyColorScheme(sideBar.ColorScheme);
			}
			if (m_HeaderSideHotStyle != null)
			{
				m_HeaderSideHotStyle.ApplyColorScheme(sideBar.ColorScheme);
			}
			if (m_HeaderSideMouseDownStyle != null)
			{
				m_HeaderSideMouseDownStyle.ApplyColorScheme(sideBar.ColorScheme);
			}
		}
	}

	public override void RecalcSize()
	{
		m_ScrollDownButton = false;
		object containerControl = ContainerControl;
		Control objCtrl = (Control)((containerControl is Control) ? containerControl : null);
		if (!BaseItem.IsHandleValid(objCtrl))
		{
			return;
		}
		CompositeImage image = GetImage();
		Size size = Size.Empty;
		if (image != null)
		{
			size = image.Size;
			size.Width += 4;
			size.Height += 4;
		}
		image = null;
		Size size2 = Size.Empty;
		string text = m_Text;
		Graphics val = BarFunctions.CreateGraphics(objCtrl);
		try
		{
			if (text != "")
			{
				size2 = TextDrawing.MeasureString(val, text, GetFont(), m_Rect.Width, GetStringFormat());
			}
		}
		finally
		{
			val.Dispose();
		}
		if (size.Height > size2.Height + m_Margin * 2)
		{
			m_PanelRect = new Rectangle(0, 0, m_Rect.Width, size.Height);
		}
		else
		{
			m_PanelRect = new Rectangle(0, 0, m_Rect.Width, size2.Height + m_Margin * 2);
			if (m_Appearance == eSideBarAppearance.Flat)
			{
				m_PanelRect.Height += 4;
			}
		}
		if (Expanded)
		{
			if (m_Rect.Height < m_PanelRect.Height)
			{
				m_Rect.Height = m_PanelRect.Height;
			}
			int num = m_Rect.Width;
			bool flag = false;
			if (m_LayoutType == eSideBarLayoutType.MultiColumn && m_TopItemIndex > 0)
			{
				flag = true;
			}
			if (m_SubItems != null)
			{
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				do
				{
					num2++;
					num3 = m_Rect.Top + m_PanelRect.Height + 1;
					int num5 = m_Rect.Left;
					num4 = m_Rect.Bottom;
					if (m_ControlHost != null)
					{
						num3 = 0;
						num5 = 0;
						num4 = m_Rect.Height - (m_PanelRect.Height + 1);
					}
					int num6 = -1;
					int num7 = num3;
					int num8 = m_Rect.Width;
					if (m_Appearance == eSideBarAppearance.Flat)
					{
						num8 = ((!size.IsEmpty) ? (num8 - size.Width) : (num8 - 26));
						if (num8 <= 0)
						{
							num8 = m_Rect.Width;
						}
						if (num8 > 16)
						{
							num8 -= 4;
							num5 += m_Rect.Width - num8 - 2;
						}
						else
						{
							num5 += m_Rect.Width - num8;
						}
						num = num8 + 4;
						if (m_ControlHost != null)
						{
							num5 = 2;
						}
					}
					int num9 = num5;
					int num10 = 0;
					foreach (BaseItem subItem in m_SubItems)
					{
						num6++;
						if (!subItem.Visible || (num3 > num4 && !flag))
						{
							if (m_EnableScrollButtons && subItem.Visible)
							{
								m_ScrollDownButton = true;
							}
							if (subItem.Visible && num3 > num4)
							{
								subItem.RecalcSize();
								num7 += subItem.HeightInternal;
							}
							subItem.Displayed = false;
							continue;
						}
						if (num6 < m_TopItemIndex && !flag)
						{
							subItem.WidthInternal = num8;
							subItem.RecalcSize();
							num7 += subItem.HeightInternal;
							subItem.Displayed = false;
							continue;
						}
						if (m_LayoutType == eSideBarLayoutType.MultiColumn)
						{
							subItem.WidthInternal = num8;
							subItem.RecalcSize();
							if (subItem.WidthInternal > num8 && num10 == 0)
							{
								subItem.WidthInternal = num8;
								num10 = subItem.HeightInternal;
							}
							else
							{
								if (num5 + subItem.WidthInternal - num9 > num8)
								{
									num5 = num9;
									num3 += num10;
									num7 += num10;
									num10 = 0;
								}
								if (subItem.HeightInternal > num10)
								{
									num10 = subItem.HeightInternal;
								}
								subItem.TopInternal = num3;
								subItem.LeftInternal = num5;
								num5 += subItem.WidthInternal;
							}
						}
						else
						{
							subItem.WidthInternal = num8;
							subItem.RecalcSize();
							subItem.WidthInternal = num8;
							if (DesignMode && subItem.IsWindowed && num3 + subItem.HeightInternal > num4)
							{
								num7 += subItem.HeightInternal;
								subItem.Displayed = false;
								continue;
							}
							if (subItem.BeginGroup)
							{
								num3 += 3;
								num7 += 3;
							}
							subItem.TopInternal = num3;
							subItem.LeftInternal = num5;
							num3 += subItem.HeightInternal;
							num7 += subItem.HeightInternal;
						}
						subItem.Displayed = true;
					}
					if (m_LayoutType == eSideBarLayoutType.MultiColumn && num10 > 0)
					{
						num7 += num10;
						num3 += num10;
					}
					if (flag)
					{
						int topInternal = SubItems[m_TopItemIndex].TopInternal;
						int num11 = m_TopItemIndex;
						while (num11 > 0)
						{
							num11--;
							if (SubItems[num11].Visible && SubItems[num11].TopInternal < topInternal)
							{
								num11++;
								break;
							}
						}
						m_TopItemIndex = num11;
						flag = false;
						num2 = 2;
					}
					else if (num7 > 0 && num7 < num4 && m_TopItemIndex > 0)
					{
						m_TopItemIndex = 0;
						m_ScrollDownButton = false;
						num2++;
					}
				}
				while (num2 == 2);
				if (num3 > num4 && m_EnableScrollButtons)
				{
					m_ScrollDownButton = true;
				}
			}
			if (m_ControlHost != null)
			{
				Size empty = Size.Empty;
				empty = ((m_Appearance != 0) ? new Size(num, m_Rect.Height - m_PanelRect.Height) : new Size(m_Rect.Width, m_Rect.Height - (m_PanelRect.Height + 1)));
				Point empty2 = Point.Empty;
				empty2 = ((m_Appearance != 0) ? new Point(m_Rect.Right - num, m_Rect.Top + m_PanelRect.Height) : new Point(m_Rect.Left, m_Rect.Top + m_PanelRect.Height + 1));
				((Control)m_ControlHost).Size = empty;
				((Control)m_ControlHost).Location = empty2;
				((Control)m_ControlHost).Visible = true;
			}
		}
		else
		{
			if (m_ControlHost != null)
			{
				((Control)m_ControlHost).Visible = false;
			}
			m_Rect.Height = m_PanelRect.Height;
			foreach (BaseItem subItem2 in m_SubItems)
			{
				subItem2.Displayed = false;
			}
		}
		if (m_ControlHost != null && Expanded)
		{
			m_ControlHost.SetupScrollButtons();
		}
		base.RecalcSize();
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		if (m_WordWrap)
		{
			pa.ButtonStringFormat &= ~(pa.ButtonStringFormat & eTextFormat.SingleLine);
			pa.ButtonStringFormat |= eTextFormat.WordBreak;
		}
		_ItemPaintArgs = pa;
		Rectangle rect = m_Rect;
		Font font = GetFont();
		Graphics graphics = pa.Graphics;
		if (m_NeedRecalcSize)
		{
			RecalcSize();
		}
		if (BarFunctions.IsOffice2007Style(EffectiveStyle))
		{
			SideBarPanelItemRendererEventArgs sideBarPanelItemRendererEventArgs = new SideBarPanelItemRendererEventArgs(this, pa.Graphics);
			sideBarPanelItemRendererEventArgs.ItemPaintArgs = pa;
			pa.Renderer.DrawSideBarPanelItem(sideBarPanelItemRendererEventArgs);
			rect = m_PanelRect;
			rect.Offset(m_Rect.X, m_Rect.Y);
		}
		else
		{
			if (base.IsThemed)
			{
				PaintThemed(pa);
				if (m_WordWrap)
				{
					pa.ButtonStringFormat |= eTextFormat.SingleLine;
				}
				_ItemPaintArgs = null;
				return;
			}
			if (SuspendLayout)
			{
				if (m_WordWrap)
				{
					pa.ButtonStringFormat |= eTextFormat.SingleLine;
				}
				_ItemPaintArgs = null;
				return;
			}
			CompositeImage image = GetImage();
			Rectangle rectangle = Rectangle.Empty;
			if (m_Appearance == eSideBarAppearance.Traditional)
			{
				Color clr = SystemColors.Control;
				Color color = m_ForeColor;
				if (color.IsEmpty)
				{
					color = pa.Colors.ItemText;
				}
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (!m_BackgroundStyle.BackColor1.IsEmpty || m_BackgroundStyle.BackgroundImage != null)
				{
					m_BackgroundStyle.Paint(graphics, m_Rect);
					clr = m_BackgroundStyle.BackColor1.Color;
				}
				if (val != null)
				{
					clr = val.BackColor;
					if (m_ForeColor == SystemColors.ControlText)
					{
						color = val.ForeColor;
					}
				}
				if (m_MouseOver && !m_HotForeColor.IsEmpty)
				{
					color = m_HotForeColor;
				}
				rect = m_PanelRect;
				rect.Offset(m_Rect.X, m_Rect.Y);
				if (m_Text != "")
				{
					if (m_MouseDown)
					{
						BarFunctions.DrawBorder3D(graphics, rect.X, rect.Y, rect.Width, rect.Height, (Border3DStyle)10, clr);
					}
					else if (m_MouseOver)
					{
						BarFunctions.DrawBorder3D(graphics, rect.X, rect.Y, rect.Width, rect.Height, (Border3DStyle)5, clr);
					}
					else
					{
						BarFunctions.DrawBorder3D(graphics, rect.X, rect.Y, rect.Width, rect.Height, (Border3DStyle)4, clr);
					}
				}
				rect.Inflate(-2, -2);
				if (image != null)
				{
					rect.X += 2;
					rect.Width -= 2;
					image.DrawImage(graphics, new Rectangle(rect.X, rect.Y + (rect.Height - image.Height) / 2, image.Width, image.Height));
					rect.X += image.Width + 4;
					rect.Width -= image.Width + 8;
				}
				if (m_Text != "")
				{
					TextDrawing.DrawString(graphics, m_Text, font, color, rect, GetStringFormat());
					rectangle = rect;
				}
			}
			else
			{
				int num = 26;
				if (image != null)
				{
					num = image.Width + 4;
				}
				if (num >= m_Rect.Width)
				{
					num = 0;
				}
				rect.Offset(num, 0);
				rect.Width -= num;
				if (!m_BackgroundStyle.BackColor1.IsEmpty || m_BackgroundStyle.BackgroundImage != null)
				{
					m_BackgroundStyle.Paint(graphics, rect);
				}
				rect = m_PanelRect;
				rect.Offset(m_Rect.Location);
				if (num > 0)
				{
					Rectangle r = new Rectangle(rect.X, rect.Y, num + 1, rect.Height);
					rect.Offset(num + 1, 0);
					rect.Width -= num + 1;
					if (m_HeaderSideStyle != null)
					{
						ItemStyle itemStyle = m_HeaderSideStyle.Clone() as ItemStyle;
						if (m_MouseDown && !Expanded)
						{
							itemStyle.ApplyStyle(m_HeaderSideMouseDownStyle);
						}
						else if (m_MouseOver && !Expanded)
						{
							itemStyle.ApplyStyle(m_HeaderSideHotStyle);
						}
						itemStyle.Paint(graphics, r);
						image?.DrawImage(graphics, new Rectangle(r.X + (r.Width - image.Width) / 2, r.Y + (r.Height - image.Height) / 2, image.Width, image.Height));
					}
				}
				if (m_HeaderStyle != null)
				{
					ItemStyle itemStyle2 = m_HeaderStyle.Clone() as ItemStyle;
					if (m_MouseDown && !Expanded)
					{
						itemStyle2.ApplyStyle(m_HeaderMouseDownStyle);
					}
					else if (m_MouseOver && !Expanded)
					{
						itemStyle2.ApplyStyle(m_HeaderHotStyle);
					}
					else if (Expanded && !m_BackgroundStyle.ForeColor.IsEmpty)
					{
						itemStyle2.ForeColor.Color = m_BackgroundStyle.ForeColor.Color;
					}
					rectangle = rect;
					rectangle.Offset(4, 0);
					rectangle.Width -= 4;
					if (Expanded)
					{
						itemStyle2.PaintText(graphics, m_Text, rectangle, font);
					}
					else
					{
						itemStyle2.Paint(graphics, rect, m_Text, rectangle, font);
					}
				}
			}
			image = null;
			if (Focused)
			{
				if (DesignMode)
				{
					Rectangle panelRect = m_PanelRect;
					panelRect.Offset(m_Rect.X, m_Rect.Y);
					panelRect.Inflate(-2, -2);
					DesignTime.DrawDesignTimeSelection(graphics, panelRect, pa.Colors.ItemDesignTimeBorder);
				}
				else if (!rectangle.IsEmpty)
				{
					Rectangle rectangle2 = rectangle;
					ControlPaint.DrawFocusRectangle(graphics, rectangle2);
				}
			}
		}
		if (Expanded && m_SubItems != null && m_ControlHost == null)
		{
			rect = new Rectangle(m_Rect.X, m_Rect.Y + rect.Height + 1, m_Rect.Width, m_Rect.Height - rect.Height - 1);
			graphics.SetClip(rect);
			for (int i = m_TopItemIndex; i < m_SubItems.Count; i++)
			{
				BaseItem baseItem = m_SubItems[i];
				if (!baseItem.Displayed || !baseItem.Visible)
				{
					continue;
				}
				if (baseItem.BeginGroup && m_LayoutType == eSideBarLayoutType.Default)
				{
					Color color2 = SystemColors.ControlDark;
					if (m_HeaderStyle != null && !m_HeaderStyle.BorderColor.IsEmpty)
					{
						color2 = m_HeaderStyle.BorderColor.Color;
					}
					Pen val2 = new Pen(color2, 1f);
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
			graphics.ResetClip();
		}
		if (m_ControlHost == null)
		{
			if (m_TopItemIndex > 0 && m_EnableScrollButtons)
			{
				if (BarFunctions.IsOffice2007Style(EffectiveStyle))
				{
					Office2007SideBarColorTable sideBar = ((Office2007Renderer)pa.Renderer).ColorTable.SideBar;
					GradientColorTable sideBarPanelItemDefault = sideBar.SideBarPanelItemDefault;
					if (sideBarPanelItemDefault != null)
					{
						m_ScrollButtonRect[0] = new Rectangle(rect.Right - 18, rect.Top + 4, 16, 16);
						Brush val3 = DisplayHelp.CreateBrush(rect, sideBarPanelItemDefault);
						try
						{
							graphics.FillRectangle(val3, m_ScrollButtonRect[0]);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
						DisplayHelp.DrawRectangle(graphics, sideBar.Border, m_ScrollButtonRect[0]);
						PaintArrow(graphics, m_ScrollButtonRect[0], sideBar.SideBarPanelItemText, up: true);
					}
				}
				else if (m_Appearance == eSideBarAppearance.Traditional || m_HeaderStyle == null)
				{
					m_ScrollButtonRect[0] = new Rectangle(rect.Right - 18, rect.Top + 4, 16, 16);
					BarFunctions.DrawBorder3D(graphics, m_ScrollButtonRect[0], (Border3DStyle)5);
					PaintArrow(graphics, m_ScrollButtonRect[0], SystemColors.ControlText, up: true);
				}
				else
				{
					m_ScrollButtonRect[0] = new Rectangle(rect.Right - 16, rect.Top + 2, 14, 14);
					ItemStyle itemStyle3 = m_HeaderStyle.Clone() as ItemStyle;
					itemStyle3.BorderSide = eBorderSide.All;
					itemStyle3.Paint(graphics, m_ScrollButtonRect[0]);
					PaintArrow(graphics, m_ScrollButtonRect[0], SystemColors.ControlText, up: true);
				}
			}
			else
			{
				m_ScrollButtonRect[0] = Rectangle.Empty;
			}
			if (m_ScrollDownButton)
			{
				if (BarFunctions.IsOffice2007Style(EffectiveStyle))
				{
					Office2007SideBarColorTable sideBar2 = ((Office2007Renderer)pa.Renderer).ColorTable.SideBar;
					GradientColorTable sideBarPanelItemDefault2 = sideBar2.SideBarPanelItemDefault;
					if (sideBarPanelItemDefault2 != null)
					{
						m_ScrollButtonRect[1] = new Rectangle(m_Rect.Right - 18, m_Rect.Bottom - 18, 16, 16);
						Brush val4 = DisplayHelp.CreateBrush(rect, sideBarPanelItemDefault2);
						try
						{
							graphics.FillRectangle(val4, m_ScrollButtonRect[1]);
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
						DisplayHelp.DrawRectangle(graphics, sideBar2.Border, m_ScrollButtonRect[1]);
						PaintArrow(graphics, m_ScrollButtonRect[1], sideBar2.SideBarPanelItemText, up: false);
					}
				}
				else if (m_Appearance == eSideBarAppearance.Traditional || m_HeaderStyle == null)
				{
					m_ScrollButtonRect[1] = new Rectangle(m_Rect.Right - 18, m_Rect.Bottom - 18, 16, 16);
					BarFunctions.DrawBorder3D(graphics, m_ScrollButtonRect[1], (Border3DStyle)5);
					PaintArrow(graphics, m_ScrollButtonRect[1], SystemColors.ControlText, up: false);
				}
				else
				{
					m_ScrollButtonRect[1] = new Rectangle(m_Rect.Right - 16, m_Rect.Bottom - 16, 14, 14);
					ItemStyle itemStyle4 = m_HeaderStyle.Clone() as ItemStyle;
					itemStyle4.BorderSide = eBorderSide.All;
					itemStyle4.Paint(graphics, m_ScrollButtonRect[1]);
					PaintArrow(graphics, m_ScrollButtonRect[1], SystemColors.ControlText, up: false);
				}
			}
			else
			{
				m_ScrollButtonRect[1] = Rectangle.Empty;
			}
		}
		if (m_WordWrap)
		{
			pa.ButtonStringFormat |= eTextFormat.SingleLine;
		}
		PaintInfoText(pa);
		_ItemPaintArgs = null;
	}

	private void PaintThemed(ItemPaintArgs pa)
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
		ThemeHeader themeHeader = pa.ThemeHeader;
		Rectangle rectangle = m_PanelRect;
		rectangle.Offset(m_Rect.X, m_Rect.Y);
		Font font = GetFont();
		ThemeHeaderParts part = ThemeHeaderParts.HeaderItemLeft;
		ThemeHeaderStates state = ThemeHeaderStates.ItemNormal;
		if (m_Text != "")
		{
			if (m_MouseDown)
			{
				state = ThemeHeaderStates.ItemPressed;
				part = ThemeHeaderParts.HeaderItem;
			}
			else if (m_MouseOver)
			{
				state = ThemeHeaderStates.ItemHot;
				part = ThemeHeaderParts.HeaderItem;
			}
			themeHeader.DrawBackground(graphics, part, state, rectangle);
		}
		CompositeImage image = GetImage();
		rectangle.Inflate(-2, -2);
		if (image != null)
		{
			rectangle.X += 2;
			rectangle.Width -= 2;
			image.DrawImage(graphics, new Rectangle(rectangle.X, rectangle.Y + (rectangle.Height - image.Height) / 2, image.Width, image.Height));
			rectangle.X += image.Width + 4;
			rectangle.Width -= image.Width + 4;
			rectangle.Inflate(0, -2);
		}
		if (m_Text != "")
		{
			themeHeader.DrawText(graphics, m_Text, font, rectangle, part, state, (ThemeTextFormat)32773, !m_Enabled);
		}
		if (DesignMode && Focused)
		{
			Rectangle panelRect = m_PanelRect;
			panelRect.Offset(m_Rect.X, m_Rect.Y);
			panelRect.Inflate(-2, -2);
			DesignTime.DrawDesignTimeSelection(graphics, panelRect, pa.Colors.ItemDesignTimeBorder);
		}
		if (Expanded && m_SubItems != null && m_ControlHost == null)
		{
			rectangle = new Rectangle(m_Rect.X, m_Rect.Y + rectangle.Height + 1, m_Rect.Width, m_Rect.Height - rectangle.Height - 1);
			graphics.SetClip(rectangle);
			IntPtr hdc = graphics.GetHdc();
			int num = 0;
			try
			{
				num = NativeFunctions.CreateRectRgn(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
				if (num != 0)
				{
					NativeFunctions.SelectClipRgn(hdc, num);
				}
			}
			finally
			{
				graphics.ReleaseHdc(hdc);
			}
			for (int i = m_TopItemIndex; i < m_SubItems.Count; i++)
			{
				BaseItem baseItem = m_SubItems[i];
				if (baseItem.Displayed && baseItem.Visible)
				{
					baseItem.Paint(pa);
				}
			}
			graphics.ResetClip();
			if (num != 0)
			{
				hdc = graphics.GetHdc();
				try
				{
					NativeFunctions.SelectClipRgn(hdc, 0);
					WinApi.DeleteObject(num);
				}
				finally
				{
					graphics.ReleaseHdc(hdc);
				}
			}
		}
		if (m_ControlHost == null)
		{
			if (m_TopItemIndex > 0)
			{
				ThemeScrollBar themeScrollBar = pa.ThemeScrollBar;
				m_ScrollButtonRect[0] = new Rectangle(rectangle.Right - 17, rectangle.Top + 4, 17, 16);
				themeScrollBar.DrawBackground(graphics, ThemeScrollBarParts.ArrowBtn, ThemeScrollBarStates.ArrowBtnUpNormal, m_ScrollButtonRect[0]);
			}
			else
			{
				m_ScrollButtonRect[0] = Rectangle.Empty;
			}
			if (m_ScrollDownButton)
			{
				ThemeScrollBar themeScrollBar2 = pa.ThemeScrollBar;
				m_ScrollButtonRect[1] = new Rectangle(m_Rect.Right - 17, m_Rect.Bottom - 18, 17, 16);
				themeScrollBar2.DrawBackground(graphics, ThemeScrollBarParts.ArrowBtn, ThemeScrollBarStates.ArrowBtnDownNormal, m_ScrollButtonRect[1]);
			}
			else
			{
				m_ScrollButtonRect[1] = Rectangle.Empty;
			}
		}
		PaintInfoText(pa);
	}

	private void PaintInfoText(ItemPaintArgs pa)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		if (m_SubItems == null || m_SubItems.Count != 0 || !DesignMode || !Expanded)
		{
			return;
		}
		Rectangle rect = m_Rect;
		if (!m_PanelRect.IsEmpty)
		{
			rect.Y += m_PanelRect.Height;
			rect.Height -= m_PanelRect.Height;
		}
		string text = "Right-click header and choose Add New Button or use SubItems collection to create new buttons.";
		rect.Inflate(-1, -1);
		if (m_Appearance == eSideBarAppearance.Flat)
		{
			CompositeImage image = GetImage();
			int num = 26;
			if (image != null)
			{
				num = image.Width + 4;
			}
			if (num >= m_Rect.Width)
			{
				num = 0;
			}
			rect.Offset(num, 0);
			rect.Width -= num;
		}
		Font font = GetFont();
		if (font != null)
		{
			Font val = new Font(font.FontFamily, font.Size - 1f);
			try
			{
				eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
				TextDrawing.DrawString(pa.Graphics, text, val, SystemColors.ControlDark, rect, format);
			}
			finally
			{
				val.Dispose();
			}
		}
	}

	private void PaintThemeScrollState()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (!BaseItem.IsHandleValid(val))
		{
			return;
		}
		Graphics val2 = BarFunctions.CreateGraphics(val);
		try
		{
			ThemeScrollBar themeScrollBar = null;
			bool flag = false;
			if (val is IThemeCache)
			{
				themeScrollBar = ((IThemeCache)val).ThemeScrollBar;
			}
			else if (val is Bar)
			{
				themeScrollBar = ((Bar)(object)val).ThemeScrollBar;
			}
			else
			{
				flag = true;
				themeScrollBar = new ThemeScrollBar(val);
			}
			ThemeScrollBarParts arrowBtn = ThemeScrollBarParts.ArrowBtn;
			ThemeScrollBarStates state = ThemeScrollBarStates.ArrowBtnUpNormal;
			if (m_TopItemIndex > 0)
			{
				if (m_ScrollButtonHot == 0)
				{
					state = ThemeScrollBarStates.ArrowBtnUpHot;
				}
				themeScrollBar.DrawBackground(val2, arrowBtn, state, m_ScrollButtonRect[0]);
			}
			if (m_ScrollDownButton)
			{
				state = ThemeScrollBarStates.ArrowBtnDownNormal;
				if (m_ScrollButtonHot == 1)
				{
					state = ThemeScrollBarStates.ArrowBtnDownHot;
				}
				themeScrollBar.DrawBackground(val2, arrowBtn, state, m_ScrollButtonRect[1]);
			}
			if (flag)
			{
				themeScrollBar.Dispose();
			}
		}
		finally
		{
			if (val2 != null)
			{
				val2.Dispose();
			}
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

	internal virtual Font GetFont()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		Font val = SystemFonts.DefaultFont;
		object containerControl = ContainerControl;
		Control val2 = (Control)((containerControl is Control) ? containerControl : null);
		if (val2 != null)
		{
			val = val2.Font;
		}
		if (m_MouseOver && (m_HotFontBold || m_HotFontUnderline))
		{
			FontStyle val3 = (FontStyle)0;
			if (m_HotFontBold)
			{
				val3 = (FontStyle)(val3 | 1);
			}
			if (m_HotFontUnderline)
			{
				val3 = (FontStyle)(val3 | 4);
			}
			try
			{
				val = new Font(val, val3);
			}
			catch
			{
				val = SystemFonts.DefaultFont;
			}
		}
		else if (m_FontBold)
		{
			FontStyle style = val.Style;
			style = (FontStyle)(style | 1);
			try
			{
				val = new Font(val, style);
			}
			catch
			{
				val = SystemFonts.DefaultFont;
			}
		}
		return val;
	}

	internal eTextFormat GetStringFormat()
	{
		eTextFormat eTextFormat2 = eTextFormat.EndEllipsis | eTextFormat.ExternalLeading | eTextFormat.VerticalCenter;
		if (m_TextAlignment == eStyleTextAlignment.Center)
		{
			eTextFormat2 |= eTextFormat.HorizontalCenter;
		}
		else if (m_TextAlignment == eStyleTextAlignment.Far)
		{
			eTextFormat2 |= eTextFormat.Right;
		}
		if (!m_WordWrap)
		{
			return eTextFormat2 | eTextFormat.SingleLine;
		}
		return eTextFormat2 | eTextFormat.WordBreak;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576 && (Math.Abs(objArg.X - m_MouseDownPt.X) >= 4 || Math.Abs(objArg.Y - m_MouseDownPt.Y) >= 4) && ContainerControl is SideBar { AllowUserCustomize: not false } && CanCustomize)
		{
			BaseItem hotSubItem = m_HotSubItem;
			if (hotSubItem != null && hotSubItem.CanCustomize && !hotSubItem.SystemItem && GetOwner() is IOwner { DragItem: null } owner)
			{
				owner.StartItemDrag(hotSubItem);
				return;
			}
		}
		bool flag = false;
		if (m_ScrollButtonRect[0].Contains(objArg.X, objArg.Y) || m_ScrollButtonRect[1].Contains(objArg.X, objArg.Y))
		{
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseLeave();
				m_HotSubItem = null;
			}
			if (m_ScrollButtonRect[0].Contains(objArg.X, objArg.Y))
			{
				m_ScrollButtonHot = 0;
			}
			else
			{
				m_ScrollButtonHot = 1;
			}
			if (base.IsThemed)
			{
				PaintThemeScrollState();
			}
			return;
		}
		if (m_ScrollButtonHot >= 0)
		{
			m_ScrollButtonHot = -1;
			flag = true;
		}
		if (m_ControlHost != null)
		{
			m_IsContainer = false;
			base.InternalMouseMove(objArg);
			m_IsContainer = true;
		}
		else
		{
			base.InternalMouseMove(objArg);
		}
		Rectangle panelRect = m_PanelRect;
		panelRect.Offset(m_Rect.Location);
		if (panelRect.Contains(objArg.X, objArg.Y))
		{
			m_MouseOver = true;
			Refresh();
		}
		else if (m_MouseOver)
		{
			m_MouseOver = false;
			Refresh();
		}
		else if (flag && base.IsThemed)
		{
			PaintThemeScrollState();
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		m_MouseOver = false;
		m_MouseDown = false;
		if (m_ScrollButtonHot >= 0)
		{
			m_ScrollButtonHot = -1;
		}
		Refresh();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Invalid comparison between Unknown and I4
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Invalid comparison between Unknown and I4
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Invalid comparison between Unknown and I4
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
		if ((int)objArg.Button == 1048576 && m_ScrollButtonRect[0].Contains(objArg.X, objArg.Y))
		{
			ScrollButtonClick(up: true);
			return;
		}
		if ((int)objArg.Button == 1048576 && m_ScrollButtonRect[1].Contains(objArg.X, objArg.Y))
		{
			ScrollButtonClick(up: false);
			return;
		}
		if ((int)objArg.Button == 1048576 && panelRect.Contains(objArg.X, objArg.Y))
		{
			m_MouseDown = true;
			Refresh();
		}
		if (m_IgnoreClickDateTime != DateTime.MinValue)
		{
			if (DateTime.Now.Subtract(m_IgnoreClickDateTime).TotalMilliseconds < 1000.0 && m_IgnoreScrollRect.Contains(objArg.X, objArg.Y))
			{
				return;
			}
			m_IgnoreClickDateTime = DateTime.MinValue;
		}
		base.InternalMouseDown(objArg);
	}

	internal void OnMouseWheel(MouseEventArgs e)
	{
		if (HasScrollButtons)
		{
			ScrollButtonClick(e.Delta < 0);
		}
	}

	internal void ScrollButtonClick(bool up)
	{
		if (up)
		{
			if (m_TopItemIndex > 0)
			{
				m_IgnoreScrollRect = m_ScrollButtonRect[0];
				if (m_LayoutType == eSideBarLayoutType.MultiColumn)
				{
					while (m_TopItemIndex > 0)
					{
						m_TopItemIndex--;
						if (SubItems[m_TopItemIndex].Visible)
						{
							break;
						}
					}
				}
				else
				{
					m_TopItemIndex--;
				}
				NeedRecalcSize = true;
				Refresh();
				m_IgnoreNextClick = true;
			}
		}
		else if (!m_ScrollButtonRect[1].IsEmpty)
		{
			m_IgnoreScrollRect = m_ScrollButtonRect[1];
			if (m_LayoutType == eSideBarLayoutType.MultiColumn)
			{
				int topInternal = SubItems[m_TopItemIndex].TopInternal;
				while (m_TopItemIndex + 1 < SubItems.Count)
				{
					m_TopItemIndex++;
					if (SubItems[m_TopItemIndex].Visible && SubItems[m_TopItemIndex].TopInternal > topInternal)
					{
						break;
					}
				}
			}
			else
			{
				m_TopItemIndex++;
			}
			NeedRecalcSize = true;
			Refresh();
			m_IgnoreNextClick = true;
		}
		if (m_ControlHost != null)
		{
			((Control)m_ControlHost).Refresh();
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576)
		{
			m_MouseDown = false;
			Refresh();
		}
		if (m_IgnoreNextClick)
		{
			m_IgnoreNextClick = false;
			m_IgnoreClickDateTime = DateTime.Now;
		}
		else if (m_IgnoreClickDateTime != DateTime.MinValue)
		{
			return;
		}
		Rectangle panelRect = m_PanelRect;
		panelRect.Offset(m_Rect.Location);
		base.InternalMouseUp(objArg);
		if ((!(GetOwner() is IOwner) || !((IOwner)GetOwner()).DragInProgress) && !Expanded && (int)objArg.Button == 1048576 && panelRect.Contains(objArg.X, objArg.Y))
		{
			Expanded = true;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		base.InternalClick(mb, mpos);
	}

	protected internal override void OnExpandChange()
	{
		base.OnExpandChange();
		m_TopItemIndex = 0;
	}

	public void ResetBackgroundStyle()
	{
		SetBackgroundStyle(new ItemStyle());
	}

	internal void SetBackgroundStyle(ItemStyle newStyle)
	{
		if (m_BackgroundStyle != null)
		{
			m_BackgroundStyle.VisualPropertyChanged -= VisualPropertyChanged;
		}
		m_BackgroundStyle = newStyle;
		if (m_BackgroundStyle != null)
		{
			m_BackgroundStyle.VisualPropertyChanged += VisualPropertyChanged;
		}
	}

	public void ResetHeaderStyle()
	{
		SetHeaderStyle(null);
	}

	internal void SetHeaderStyle(ItemStyle style)
	{
		if (m_HeaderStyle != null)
		{
			m_HeaderStyle.VisualPropertyChanged -= VisualPropertyChanged;
		}
		m_HeaderStyle = style;
		if (m_HeaderStyle != null)
		{
			m_HeaderStyle.VisualPropertyChanged += VisualPropertyChanged;
		}
	}

	public void ResetHeaderHotStyle()
	{
		SetHeaderHotStyle(null);
	}

	internal void SetHeaderHotStyle(ItemStyle style)
	{
		if (m_HeaderHotStyle != null)
		{
			m_HeaderHotStyle.VisualPropertyChanged -= VisualPropertyChanged;
		}
		m_HeaderHotStyle = style;
		if (m_HeaderHotStyle != null)
		{
			m_HeaderHotStyle.VisualPropertyChanged += VisualPropertyChanged;
		}
	}

	public void ResetHeaderMouseDownStyle()
	{
		SetHeaderMouseDownStyle(null);
	}

	internal void SetHeaderMouseDownStyle(ItemStyle style)
	{
		if (m_HeaderMouseDownStyle != null)
		{
			m_HeaderMouseDownStyle.VisualPropertyChanged -= VisualPropertyChanged;
		}
		m_HeaderMouseDownStyle = style;
		if (m_HeaderMouseDownStyle != null)
		{
			m_HeaderMouseDownStyle.VisualPropertyChanged += VisualPropertyChanged;
		}
	}

	public void ResetHeaderSideStyle()
	{
		SetHeaderSideStyle(null);
	}

	internal void SetHeaderSideStyle(ItemStyle style)
	{
		if (m_HeaderSideStyle != null)
		{
			m_HeaderSideStyle.VisualPropertyChanged -= VisualPropertyChanged;
		}
		m_HeaderSideStyle = style;
		if (m_HeaderSideStyle != null)
		{
			m_HeaderSideStyle.VisualPropertyChanged += VisualPropertyChanged;
		}
	}

	public void ResetHeaderSideHotStyle()
	{
		SetHeaderSideHotStyle(null);
	}

	internal void SetHeaderSideHotStyle(ItemStyle style)
	{
		if (m_HeaderSideHotStyle != null)
		{
			m_HeaderSideHotStyle.VisualPropertyChanged -= VisualPropertyChanged;
		}
		m_HeaderSideHotStyle = style;
		if (m_HeaderSideHotStyle != null)
		{
			m_HeaderSideHotStyle.VisualPropertyChanged += VisualPropertyChanged;
		}
	}

	public void ResetHeaderSideMouseDownStyle()
	{
		SetHeaderSideMouseDownStyle(null);
	}

	internal void SetHeaderSideMouseDownStyle(ItemStyle style)
	{
		if (m_HeaderSideMouseDownStyle != null)
		{
			m_HeaderSideMouseDownStyle.VisualPropertyChanged -= VisualPropertyChanged;
		}
		m_HeaderSideMouseDownStyle = style;
		if (m_HeaderSideMouseDownStyle != null)
		{
			m_HeaderSideMouseDownStyle.VisualPropertyChanged += VisualPropertyChanged;
		}
	}

	public bool ShouldSerializeHotForeColor()
	{
		if (m_HotForeColor != Color.Empty)
		{
			return true;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetForeColor()
	{
		ForeColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeForeColor()
	{
		return !m_ForeColor.IsEmpty;
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (m_ForeColor != SystemColors.ControlText && !m_ForeColor.IsEmpty)
		{
			itemXmlElement.SetAttribute("forecolor", BarFunctions.ColorToString(m_ForeColor));
		}
		if (m_HotFontBold)
		{
			itemXmlElement.SetAttribute("hotfb", XmlConvert.ToString(m_HotFontBold));
		}
		if (m_HotFontUnderline)
		{
			itemXmlElement.SetAttribute("hotfu", XmlConvert.ToString(m_HotFontUnderline));
		}
		if (!m_HotForeColor.IsEmpty)
		{
			itemXmlElement.SetAttribute("hotclr", BarFunctions.ColorToString(m_HotForeColor));
		}
		if (m_ItemImageSize != 0)
		{
			itemXmlElement.SetAttribute("itemimagesize", XmlConvert.ToString((int)m_ItemImageSize));
		}
		if (!m_EnableScrollButtons)
		{
			itemXmlElement.SetAttribute("enablescrollbuttons", XmlConvert.ToString(m_EnableScrollButtons));
		}
		XmlElement xmlElement = null;
		XmlElement xmlElement2 = null;
		if (m_Image != null || m_ImageIndex >= 0 || m_HoverImage != null || m_HoverImageIndex >= 0 || m_PressedImage != null || m_PressedImageIndex >= 0 || m_Icon != null)
		{
			xmlElement = itemXmlElement.OwnerDocument.CreateElement("images");
			itemXmlElement.AppendChild(xmlElement);
			if (m_ImageIndex >= 0)
			{
				xmlElement.SetAttribute("imageindex", XmlConvert.ToString(m_ImageIndex));
			}
			if (m_HoverImageIndex >= 0)
			{
				xmlElement.SetAttribute("hoverimageindex", XmlConvert.ToString(m_HoverImageIndex));
			}
			if (m_PressedImageIndex >= 0)
			{
				xmlElement.SetAttribute("pressedimageindex", XmlConvert.ToString(m_PressedImageIndex));
			}
			if (m_Image != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "default");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(m_Image, xmlElement2);
			}
			if (m_HoverImage != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "hover");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(m_HoverImage, xmlElement2);
			}
			if (m_PressedImage != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "pressed");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(m_PressedImage, xmlElement2);
			}
			if (m_Icon != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "icon");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeIcon(m_Icon, xmlElement2);
			}
		}
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (itemXmlElement.HasAttribute("forecolor"))
		{
			m_ForeColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("forecolor"));
		}
		else
		{
			m_ForeColor = Color.Empty;
		}
		if (itemXmlElement.HasAttribute("hotclr"))
		{
			m_HotForeColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("hotclr"));
		}
		else
		{
			m_HotForeColor = Color.Empty;
		}
		if (itemXmlElement.HasAttribute("hotfb"))
		{
			m_HotFontBold = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("hotfb"));
		}
		else
		{
			m_HotFontBold = false;
		}
		if (itemXmlElement.HasAttribute("hotfu"))
		{
			m_HotFontUnderline = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("hotfu"));
		}
		else
		{
			m_HotFontUnderline = false;
		}
		if (itemXmlElement.HasAttribute("itemimagesize"))
		{
			m_ItemImageSize = (eBarImageSize)XmlConvert.ToInt32(itemXmlElement.GetAttribute("itemimagesize"));
		}
		else
		{
			m_ItemImageSize = eBarImageSize.Default;
		}
		if (itemXmlElement.HasAttribute("enablescrollbuttons"))
		{
			m_EnableScrollButtons = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("enablescrollbuttons"));
		}
		else
		{
			m_EnableScrollButtons = true;
		}
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			if (!(childNode.Name == "images"))
			{
				continue;
			}
			if (childNode.HasAttribute("imageindex"))
			{
				m_ImageIndex = XmlConvert.ToInt32(childNode.GetAttribute("imageindex"));
			}
			if (childNode.HasAttribute("hoverimageindex"))
			{
				m_HoverImageIndex = XmlConvert.ToInt32(childNode.GetAttribute("hoverimageindex"));
			}
			if (childNode.HasAttribute("pressedimageindex"))
			{
				m_PressedImageIndex = XmlConvert.ToInt32(childNode.GetAttribute("pressedimageindex"));
			}
			foreach (XmlElement childNode2 in childNode.ChildNodes)
			{
				switch (childNode2.GetAttribute("type"))
				{
				case "default":
					m_Image = BarFunctions.DeserializeImage(childNode2);
					m_ImageIndex = -1;
					break;
				case "icon":
					m_Icon = BarFunctions.DeserializeIcon(childNode2);
					m_ImageIndex = -1;
					break;
				case "hover":
					m_HoverImage = BarFunctions.DeserializeImage(childNode2);
					m_HoverImageIndex = -1;
					break;
				case "pressed":
					m_PressedImage = BarFunctions.DeserializeImage(childNode2);
					m_PressedImageIndex = -1;
					break;
				}
			}
			break;
		}
		RefreshImageSize();
		NeedRecalcSize = true;
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		NeedRecalcSize = true;
		if (item.IsWindowed && !DesignMode)
		{
			CreateControlPanelHost();
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
		if (m_ControlHost != null)
		{
			item.ContainerControl = m_ControlHost;
		}
		if (item is ButtonItem)
		{
			((ButtonItem)item)._FitContainer = m_WordWrap;
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

	private void CreateControlPanelHost()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null && m_ControlHost == null)
		{
			m_ControlHost = new SideBarPanelControlHost(this);
			((Control)m_ControlHost).Visible = false;
			val.Controls.Add((Control)(object)m_ControlHost);
		}
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
		if (val == null || !m_PanelRect.Contains(val.PointToClient(Control.MousePosition)))
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
		if (m_ControlHost == null || !Expanded)
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

	internal void SetImageIndex(int iImageIndex)
	{
		m_ImageIndex = iImageIndex;
	}

	internal CompositeImage GetImage()
	{
		if (m_MouseDown)
		{
			return GetImage(ImageState.Pressed);
		}
		if (m_MouseOver)
		{
			return GetImage(ImageState.Hover);
		}
		return GetImage(ImageState.Default);
	}

	private CompositeImage GetImage(ImageState state)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Image val = null;
		if (m_Icon != null)
		{
			Size iconSize = IconSize;
			Icon val2 = null;
			try
			{
				val2 = new Icon(m_Icon, iconSize);
			}
			catch
			{
				val2 = null;
			}
			if (val2 == null)
			{
				return new CompositeImage(m_Icon, dispose: false);
			}
			return new CompositeImage(val2, dispose: true);
		}
		if (state == ImageState.Hover && (m_HoverImage != null || m_HoverImageIndex >= 0))
		{
			if (m_HoverImage != null)
			{
				return new CompositeImage(m_HoverImage, dispose: false);
			}
			if (m_HoverImageIndex >= 0)
			{
				val = GetImageFromImageList(m_HoverImageIndex);
				if (val != null)
				{
					return new CompositeImage(val, dispose: false);
				}
				return null;
			}
		}
		if (state == ImageState.Pressed && (m_PressedImage != null || m_PressedImageIndex >= 0))
		{
			if (m_PressedImage != null)
			{
				return new CompositeImage(m_PressedImage, dispose: false);
			}
			if (m_PressedImageIndex >= 0)
			{
				val = GetImageFromImageList(m_PressedImageIndex);
				if (val != null)
				{
					return new CompositeImage(val, dispose: false);
				}
				return null;
			}
		}
		if (m_Image != null)
		{
			return new CompositeImage(m_Image, dispose: false);
		}
		if (m_ImageIndex >= 0)
		{
			val = GetImageFromImageList(m_ImageIndex);
			if (val != null)
			{
				return new CompositeImage(val, dispose: false);
			}
		}
		return null;
	}

	private Image GetImageFromImageList(int ImageIndex)
	{
		if (ImageIndex >= 0)
		{
			IOwner owner = null;
			IBarImageSize barImageSize = null;
			if (_ItemPaintArgs != null)
			{
				owner = _ItemPaintArgs.Owner;
				barImageSize = _ItemPaintArgs.ContainerControl as IBarImageSize;
			}
			if (owner == null)
			{
				owner = GetOwner() as IOwner;
			}
			if (barImageSize == null)
			{
				barImageSize = ContainerControl as IBarImageSize;
			}
			if (owner != null)
			{
				try
				{
					if (barImageSize != null && barImageSize.ImageSize != 0)
					{
						if (barImageSize.ImageSize == eBarImageSize.Medium && owner.ImagesMedium != null)
						{
							return owner.ImagesMedium.Images[ImageIndex];
						}
						if (barImageSize.ImageSize == eBarImageSize.Large && owner.ImagesLarge != null)
						{
							return owner.ImagesLarge.Images[ImageIndex];
						}
						if (owner.Images != null)
						{
							if (ImageIndex == m_ImageIndex)
							{
								if (m_ImageCachedIdx == null)
								{
									m_ImageCachedIdx = owner.Images.Images[ImageIndex];
								}
								return m_ImageCachedIdx;
							}
							return owner.Images.Images[ImageIndex];
						}
					}
					else if (m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize != 0)
					{
						eBarImageSize itemImageSize = ((SideBarPanelItem)m_Parent).ItemImageSize;
						if (itemImageSize == eBarImageSize.Medium && owner.ImagesMedium != null)
						{
							return owner.ImagesMedium.Images[ImageIndex];
						}
						if (itemImageSize == eBarImageSize.Large && owner.ImagesLarge != null)
						{
							return owner.ImagesLarge.Images[ImageIndex];
						}
						if (owner.Images != null)
						{
							return owner.Images.Images[ImageIndex];
						}
					}
					else if (owner.Images != null)
					{
						if (ImageIndex == m_ImageIndex)
						{
							if (m_ImageCachedIdx == null)
							{
								m_ImageCachedIdx = owner.Images.Images[ImageIndex];
							}
							return m_ImageCachedIdx;
						}
						return owner.Images.Images[ImageIndex];
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

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem DragItem)
	{
		InsertPosition insertPosition = null;
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || !CanCustomize)
		{
			return null;
		}
		Point pt = val.PointToClient(pScreen);
		Rectangle displayRectangle = DisplayRectangle;
		if (displayRectangle.Contains(pt))
		{
			Rectangle rectangle = displayRectangle;
			rectangle.Size = m_PanelRect.Size;
			rectangle.X += m_PanelRect.Left;
			rectangle.Y += m_PanelRect.Top;
			if (rectangle.Contains(pt) && !Expanded)
			{
				Expanded = true;
				insertPosition = new InsertPosition();
				insertPosition.TargetProvider = this;
				if (SubItems.Count == 0)
				{
					insertPosition.Position = -1;
				}
				else
				{
					insertPosition.Position = 0;
					insertPosition.Before = true;
				}
				return insertPosition;
			}
			BaseItem baseItem = ExpandedItem();
			if (baseItem != null && baseItem is IDesignTimeProvider designTimeProvider)
			{
				insertPosition = designTimeProvider.GetInsertPosition(pScreen, DragItem);
				if (insertPosition != null)
				{
					return insertPosition;
				}
			}
			for (int i = 0; i < SubItems.Count; i++)
			{
				baseItem = SubItems[i];
				rectangle = baseItem.DisplayRectangle;
				rectangle.Inflate(2, 2);
				if (!baseItem.Visible || !baseItem.Displayed || !rectangle.Contains(pt))
				{
					continue;
				}
				if (baseItem.SystemItem && SubItems.Count != 1)
				{
					return null;
				}
				if (baseItem == DragItem)
				{
					return new InsertPosition();
				}
				insertPosition = new InsertPosition();
				insertPosition.TargetProvider = this;
				insertPosition.Position = i;
				if (pt.Y <= baseItem.TopInternal + baseItem.HeightInternal / 2 || baseItem.SystemItem)
				{
					insertPosition.Before = true;
				}
				if (GetOwner() is IOwner owner)
				{
					BaseItem baseItem2 = owner.GetExpandedItem();
					if (baseItem2 != null)
					{
						while (baseItem2.Parent != null)
						{
							baseItem2 = baseItem2.Parent;
						}
						BaseItem baseItem3 = baseItem;
						while (baseItem3.Parent != null)
						{
							baseItem3 = baseItem3.Parent;
						}
						if (baseItem2 != baseItem3)
						{
							owner.SetExpandedItem(null);
						}
					}
				}
				if (baseItem is PopupItem && (baseItem.SubItems.Count > 0 || baseItem.IsOnMenuBar))
				{
					if (!baseItem.Expanded)
					{
						baseItem.Expanded = true;
					}
				}
				else
				{
					BaseItem.CollapseSubItems(this);
				}
				break;
			}
			if (insertPosition == null)
			{
				insertPosition = ((SubItems.Count <= 1 || !SubItems[SubItems.Count - 1].SystemItem) ? new InsertPosition(SubItems.Count - 1, bBefore: false, this) : new InsertPosition(SubItems.Count - 2, bBefore: true, this));
			}
		}
		else
		{
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem != DragItem && subItem is IDesignTimeProvider designTimeProvider2)
				{
					insertPosition = designTimeProvider2.GetInsertPosition(pScreen, DragItem);
					if (insertPosition != null)
					{
						break;
					}
				}
			}
		}
		return insertPosition;
	}

	void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool Before)
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null)
		{
			return;
		}
		BaseItem baseItem = null;
		if (iPos >= 0)
		{
			baseItem = SubItems[iPos];
		}
		if (baseItem != null)
		{
			if (baseItem.DesignInsertMarker != 0)
			{
				baseItem.DesignInsertMarker = eDesignInsertPosition.None;
			}
			else if (Before)
			{
				baseItem.DesignInsertMarker = eDesignInsertPosition.Before;
			}
			else
			{
				baseItem.DesignInsertMarker = eDesignInsertPosition.After;
			}
		}
		else
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			displayRectangle.Offset(m_PanelRect.X, m_PanelRect.Bottom);
			Rectangle rectangle = new Rectangle(displayRectangle.Left + 2, displayRectangle.Top + 2, displayRectangle.Width - 4, 1);
			Rectangle rectangle2 = new Rectangle(displayRectangle.Left + 1, displayRectangle.Top, 1, 5);
			Rectangle rectangle3 = new Rectangle(displayRectangle.Right - 2, displayRectangle.Top, 1, 5);
		}
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
		if (ExpandedItem() != null)
		{
			ExpandedItem().Expanded = false;
		}
		if (!Before)
		{
			if (iPos + 1 >= SubItems.Count)
			{
				SubItems.Add(objItem, GetAppendPosition(this));
			}
			else
			{
				SubItems.Add(objItem, iPos + 1);
			}
		}
		else if (iPos >= SubItems.Count)
		{
			SubItems.Add(objItem, GetAppendPosition(this));
		}
		else
		{
			SubItems.Add(objItem, iPos);
		}
		if (ContainerControl is Bar)
		{
			((Bar)ContainerControl).RecalcLayout();
			return;
		}
		if (ContainerControl is MenuPanel)
		{
			((MenuPanel)ContainerControl).RecalcSize();
			return;
		}
		RecalcSize();
		Refresh();
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
}
