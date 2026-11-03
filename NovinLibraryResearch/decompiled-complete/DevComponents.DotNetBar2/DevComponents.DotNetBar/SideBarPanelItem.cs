using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.SideBarPanelItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
public class SideBarPanelItem : ImageItem, IDesignTimeProvider
{
	private const string _193D = "Right-click header and choose Add New Button or use SubItems collection to create new buttons.";

	private const int _193E = 26;

	private const int _193F = 2;

	private int _1940;

	private bool _1941;

	private Rectangle[] _1942 = new Rectangle[2]
	{
		Rectangle.Empty,
		Rectangle.Empty
	};

	private int _1943 = -1;

	private int ᇬ = 3;

	private bool ݡ;

	private bool ݠ;

	private Rectangle _1944 = Rectangle.Empty;

	private Color _0D02 = Color.Empty;

	private Color _0E79 = Color.Empty;

	private bool _0E7A;

	private bool _0E7B;

	private bool _0E7C;

	private eBarImageSize _1945;

	private Point _0AC2;

	private SideBarPanelControlHost _᥆;

	private ItemStyle _0599 = new ItemStyle();

	private ItemStyle _᥇;

	private ItemStyle _᥈;

	private ItemStyle _᥉;

	private ItemStyle _᥊;

	private ItemStyle _᥋;

	private ItemStyle _᥌;

	private bool ݥ;

	private Image _0E66;

	private int դ = -1;

	private Image _0E68;

	private int _0E69 = -1;

	private Image _0E6E;

	private int _0E6F = -1;

	private Icon ຈ;

	private Image ຂ;

	private ItemPaintArgs __0E83;

	private eSideBarAppearance _᥍;

	private bool _᥎;

	private DateTime _᥏ = DateTime.MinValue;

	private Rectangle ᥐ = Rectangle.Empty;

	private bool ᥑ = true;

	private eSideBarLayoutType _0DC8;

	private eStyleTextAlignment ޔ = eStyleTextAlignment.Center;

	[Browsable(false)]
	public bool IsMouseOver => ݡ;

	[Browsable(false)]
	public bool IsMouseDown => ݠ;

	[Description("Specifies alignment of the text.")]
	[Browsable(true)]
	[DevCoSerialize]
	[DefaultValue(eStyleTextAlignment.Center)]
	[Category("Appearance")]
	public eStyleTextAlignment TextAlignment
	{
		get
		{
			return ޔ;
		}
		set
		{
			ޔ = value;
			OnAppearanceChanged();
		}
	}

	[Description("Gets or sets the layout type for the items.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(eSideBarLayoutType.Default)]
	[Category("Appearance")]
	public eSideBarLayoutType LayoutType
	{
		get
		{
			return _0DC8;
		}
		set
		{
			if (_0DC8 != value)
			{
				_0DC8 = value;
				NeedRecalcSize = true;
				if (DesignMode)
				{
					Refresh();
				}
			}
		}
	}

	[Description("The text contained in the item.")]
	[Localizable(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
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
				ᇬ = 0;
			}
			else
			{
				ᇬ = 3;
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

	[Description("Gets or sets group background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public ItemStyle BackgroundStyle => _0599;

	internal bool _1939
	{
		get
		{
			if (_᥇ != null)
			{
				return true;
			}
			return false;
		}
	}

	[Category("Style")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets the item header style. Applies only when SideBar.Appearance is set to Flat.")]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	public ItemStyle HeaderStyle
	{
		get
		{
			if (_᥇ == null)
			{
				ᥚ(new ItemStyle());
			}
			return _᥇;
		}
	}

	[Category("Style")]
	[Description("Gets or sets the item header style when mouse is over the header. Applies only when SideBar.Appearance is set to Flat.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	public ItemStyle HeaderHotStyle
	{
		get
		{
			if (_᥈ == null)
			{
				ᥛ(new ItemStyle());
			}
			return _᥈;
		}
	}

	[Category("Style")]
	[Description("Gets or sets the item header style when left mouse button is pressed on header. Applies only when SideBar.Appearance is set to Flat.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	public ItemStyle HeaderMouseDownStyle
	{
		get
		{
			if (_᥉ == null)
			{
				ᥜ(new ItemStyle());
			}
			return _᥉;
		}
	}

	[Category("Style")]
	[Description("Gets or sets the item header side style. Applies only when SideBar.Appearance is set to Flat.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	public ItemStyle HeaderSideStyle
	{
		get
		{
			if (_᥊ == null)
			{
				ᥝ(new ItemStyle());
			}
			return _᥊;
		}
	}

	[Category("Style")]
	[Description("Gets or sets the item header side style when mouse is over the header. Applies only when SideBar.Appearance is set to Flat.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[NotifyParentProperty(true)]
	[DevCoBrowsable(true)]
	public ItemStyle HeaderSideHotStyle
	{
		get
		{
			if (_᥋ == null)
			{
				ᥞ(new ItemStyle());
			}
			return _᥋;
		}
	}

	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets the item header side style when left mouse button is pressed on header. Applies only when SideBar.Appearance is set to Flat.")]
	[Category("Style")]
	public ItemStyle HeaderSideMouseDownStyle
	{
		get
		{
			if (_᥌ == null)
			{
				ᥟ(new ItemStyle());
			}
			return _᥌;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eSideBarAppearance Appearance
	{
		get
		{
			return _᥍;
		}
		set
		{
			if (_᥍ != value)
			{
				_᥍ = value;
				NeedRecalcSize = true;
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Description("Specifies that text font is bold when text is rendered.")]
	public bool FontBold
	{
		get
		{
			return _0E7C;
		}
		set
		{
			if (_0E7C != value)
			{
				_0E7C = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "FontBold");
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
	[DefaultValue(false)]
	[Description("Specifies that text font is bold when mouse is over the item.")]
	[Category("Appearance")]
	public bool HotFontBold
	{
		get
		{
			return _0E7B;
		}
		set
		{
			if (_0E7B != value)
			{
				_0E7B = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "HotFontBold");
				}
				if (Displayed && ݡ)
				{
					Refresh();
				}
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Description("Specifies that text font is underlined when mouse is over the item.")]
	[Category("Appearance")]
	public bool HotFontUnderline
	{
		get
		{
			return _0E7A;
		}
		set
		{
			if (_0E7A != value)
			{
				_0E7A = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "HotFontUnderline");
				}
				if (Displayed && ݡ)
				{
					Refresh();
				}
			}
			OnAppearanceChanged();
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("The foreground color used to display text when mouse is over the item.")]
	[DevCoBrowsable(true)]
	public Color HotForeColor
	{
		get
		{
			return _0E79;
		}
		set
		{
			if (_0E79 != value)
			{
				_0E79 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "HotForeColor");
				}
				if (Displayed && ݡ)
				{
					Refresh();
				}
			}
			OnAppearanceChanged();
		}
	}

	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("The foreground color used to display text.")]
	public Color ForeColor
	{
		get
		{
			return _0D02;
		}
		set
		{
			if (_0D02 != value)
			{
				_0D02 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "ForeColor");
				}
				if (Displayed)
				{
					Refresh();
				}
			}
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(false)]
	[Description("Determines alignment of the item inside the container.")]
	[Browsable(false)]
	[Category("Appearance")]
	[DefaultValue(eItemAlignment.Near)]
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

	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
	[Browsable(false)]
	[Category("Appearance")]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
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

	[Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
	[Category("Behavior")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DefaultValue(true)]
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

	[Description("Determines whether sub-items are displayed.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	[DefaultValue("")]
	[Category("Design")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
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

	[DefaultValue("")]
	[Category("Design")]
	[DevCoBrowsable(false)]
	[Description("Indicates description of the item that is displayed during design.")]
	[Browsable(false)]
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

	[Description("Indicates whether this item is beginning of the group.")]
	[Category("Appearance")]
	[DefaultValue(false)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	[Category("Data")]
	[Description("Collection of sub items.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor("DevComponents.DotNetBar.Design.SideBarPanelItemEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DevCoBrowsable(false)]
	[Browsable(true)]
	public override SubItemsCollection SubItems => base.SubItems;

	[DefaultValue(eBarImageSize.Default)]
	[Category("Behavior")]
	[Description("Specifies the Image size that will be used by items on this bar.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public eBarImageSize ItemImageSize
	{
		get
		{
			return _1945;
		}
		set
		{
			_1945 = value;
			eImagePosition imagePosition = eImagePosition.Left;
			if (_1945 != 0)
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
			_1940 = 0;
			RefreshImageSize();
			RecalcSize();
			Refresh();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Rectangle PanelRect => _1944;

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int TopItemIndex
	{
		get
		{
			return _1940;
		}
		set
		{
			_1940 = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(false)]
	[Description("Indicates a value that determines whether text is displayed in multiple lines or one long line.")]
	[Category("Layout")]
	[Browsable(true)]
	public bool WordWrap
	{
		get
		{
			return ݥ;
		}
		set
		{
			ݥ = value;
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem is ButtonItem)
				{
					((ButtonItem)subItem)._ງ = ݥ;
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

	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Description("Specifies whether scroll buttons are displayed when content of the panel exceeds it's height.")]
	[Category("Behavior")]
	[Browsable(true)]
	public bool EnableScrollButtons
	{
		get
		{
			return ᥑ;
		}
		set
		{
			ᥑ = value;
			if (!ᥑ)
			{
				NeedRecalcSize = true;
				if (DesignMode)
				{
					Refresh();
				}
			}
		}
	}

	internal bool _193A
	{
		get
		{
			if (ᥑ)
			{
				if (_1942[0].IsEmpty)
				{
					return !_1942[1].IsEmpty;
				}
				return true;
			}
			return false;
		}
	}

	internal bool _193B => _1941;

	internal bool _193C => base.IsThemed;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the Button icon. Icons support multiple image sizes and alpha blending.")]
	[Browsable(true)]
	[DefaultValue(null)]
	public Icon Icon
	{
		get
		{
			return ຈ;
		}
		set
		{
			NeedRecalcSize = true;
			ຈ = value;
			OnImageChanged();
			Refresh();
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed on the face of the item.")]
	[DefaultValue(null)]
	[Browsable(true)]
	public Image Image
	{
		get
		{
			return _0E66;
		}
		set
		{
			NeedRecalcSize = true;
			_0E66 = value;
			OnImageChanged();
			Refresh();
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(-1)]
	[Description("The image list image index of the image that will be displayed on the face of the item.")]
	[DevCoBrowsable(true)]
	[TypeConverter(typeof(ImageIndexConverter))]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public int ImageIndex
	{
		get
		{
			return դ;
		}
		set
		{
			ຂ = null;
			if (դ != value)
			{
				դ = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "ImageIndex");
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

	[Category("Appearance")]
	[Description("The image that will be displayed when mouse hovers over the item.")]
	[Browsable(true)]
	[DefaultValue(null)]
	[DevCoBrowsable(true)]
	public Image HoverImage
	{
		get
		{
			return _0E68;
		}
		set
		{
			_0E68 = value;
			OnAppearanceChanged();
		}
	}

	[Description("The image list image index of the image that will be displayed when mouse hovers over the item.")]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(-1)]
	[Browsable(true)]
	public int HoverImageIndex
	{
		get
		{
			return _0E69;
		}
		set
		{
			if (_0E69 != value)
			{
				_0E69 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "HoverImageIndex");
				}
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("The image that will be displayed when item is pressed.")]
	[Browsable(true)]
	public Image PressedImage
	{
		get
		{
			return _0E6E;
		}
		set
		{
			_0E6E = value;
			OnAppearanceChanged();
		}
	}

	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[TypeConverter(typeof(ImageIndexConverter))]
	[Description("The image list image index of the image that will be displayed when item is pressed.")]
	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(-1)]
	[Browsable(true)]
	public int PressedImageIndex
	{
		get
		{
			return _0E6F;
		}
		set
		{
			if (_0E6F != value)
			{
				_0E6F = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "PressedImageIndex");
				}
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	private Size _0E5A
	{
		get
		{
			Size result = new Size(16, 16);
			IBarImageSize barImageSize = null;
			if (__0E83 != null)
			{
				barImageSize = __0E83.ContainerControl as IBarImageSize;
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
		_0599.ઐ += ઐ;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.ᆟ(ref _0E66);
			BarUtilities.ᆟ(ref ຂ);
			BarUtilities.ᆟ(ref ຈ);
			BarUtilities.ᆟ(ref _0E68);
			BarUtilities.ᆟ(ref _0E6E);
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
		sideBarPanelItem.HotFontBold = _0E7B;
		sideBarPanelItem.HotFontUnderline = _0E7A;
		sideBarPanelItem.HotForeColor = _0E79;
		sideBarPanelItem.ForeColor = _0D02;
		sideBarPanelItem.ItemImageSize = _1945;
		sideBarPanelItem.ᥘ(_0599.Clone() as ItemStyle);
	}

	private void ઐ(object _٠, EventArgs ؿ)
	{
		ᥒ();
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	internal void ᥒ()
	{
		if (ContainerControl is SideBar sideBar)
		{
			if (_0599 != null)
			{
				_0599.ᗅ(sideBar.ColorScheme);
			}
			if (_᥇ != null)
			{
				_᥇.ᗅ(sideBar.ColorScheme);
			}
			if (_᥈ != null)
			{
				_᥈.ᗅ(sideBar.ColorScheme);
			}
			if (_᥉ != null)
			{
				_᥉.ᗅ(sideBar.ColorScheme);
			}
			if (_᥊ != null)
			{
				_᥊.ᗅ(sideBar.ColorScheme);
			}
			if (_᥋ != null)
			{
				_᥋.ᗅ(sideBar.ColorScheme);
			}
			if (_᥌ != null)
			{
				_᥌.ᗅ(sideBar.ColorScheme);
			}
		}
	}

	public override void RecalcSize()
	{
		_1941 = false;
		object containerControl = ContainerControl;
		Control objCtrl = (Control)((containerControl is Control) ? containerControl : null);
		if (!BaseItem.IsHandleValid(objCtrl))
		{
			return;
		}
		ᇎ ᇎ2 = _0EC5();
		Size size = Size.Empty;
		if (ᇎ2 != null)
		{
			size = ᇎ2.ᇐ;
			size.Width += 4;
			size.Height += 4;
		}
		ᇎ2 = null;
		Size size2 = Size.Empty;
		string text = m_Text;
		Graphics val = BarFunctions.CreateGraphics(objCtrl);
		try
		{
			if (text != "")
			{
				size2 = TextDrawing.MeasureString(val, text, _໘(), m_Rect.Width, ಲ());
			}
		}
		finally
		{
			val.Dispose();
		}
		if (size.Height > size2.Height + ᇬ * 2)
		{
			_1944 = new Rectangle(0, 0, m_Rect.Width, size.Height);
		}
		else
		{
			_1944 = new Rectangle(0, 0, m_Rect.Width, size2.Height + ᇬ * 2);
			if (_᥍ == eSideBarAppearance.Flat)
			{
				_1944.Height += 4;
			}
		}
		if (Expanded)
		{
			if (m_Rect.Height < _1944.Height)
			{
				m_Rect.Height = _1944.Height;
			}
			int num = m_Rect.Width;
			bool flag = false;
			if (_0DC8 == eSideBarLayoutType.MultiColumn && _1940 > 0)
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
					num3 = m_Rect.Top + _1944.Height + 1;
					int num5 = m_Rect.Left;
					num4 = m_Rect.Bottom;
					if (_᥆ != null)
					{
						num3 = 0;
						num5 = 0;
						num4 = m_Rect.Height - (_1944.Height + 1);
					}
					int num6 = -1;
					int num7 = num3;
					int num8 = m_Rect.Width;
					if (_᥍ == eSideBarAppearance.Flat)
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
						if (_᥆ != null)
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
							if (ᥑ && subItem.Visible)
							{
								_1941 = true;
							}
							if (subItem.Visible && num3 > num4)
							{
								subItem.RecalcSize();
								num7 += subItem.HeightInternal;
							}
							subItem.Displayed = false;
							continue;
						}
						if (num6 < _1940 && !flag)
						{
							subItem.WidthInternal = num8;
							subItem.RecalcSize();
							num7 += subItem.HeightInternal;
							subItem.Displayed = false;
							continue;
						}
						if (_0DC8 == eSideBarLayoutType.MultiColumn)
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
					if (_0DC8 == eSideBarLayoutType.MultiColumn && num10 > 0)
					{
						num7 += num10;
						num3 += num10;
					}
					if (flag)
					{
						int topInternal = SubItems[_1940].TopInternal;
						int num11 = _1940;
						while (num11 > 0)
						{
							num11--;
							if (SubItems[num11].Visible && SubItems[num11].TopInternal < topInternal)
							{
								num11++;
								break;
							}
						}
						_1940 = num11;
						flag = false;
						num2 = 2;
					}
					else if (num7 > 0 && num7 < num4 && _1940 > 0)
					{
						_1940 = 0;
						_1941 = false;
						num2++;
					}
				}
				while (num2 == 2);
				if (num3 > num4 && ᥑ)
				{
					_1941 = true;
				}
			}
			if (_᥆ != null)
			{
				Size empty = Size.Empty;
				empty = ((_᥍ != 0) ? new Size(num, m_Rect.Height - _1944.Height) : new Size(m_Rect.Width, m_Rect.Height - (_1944.Height + 1)));
				Point empty2 = Point.Empty;
				empty2 = ((_᥍ != 0) ? new Point(m_Rect.Right - num, m_Rect.Top + _1944.Height) : new Point(m_Rect.Left, m_Rect.Top + _1944.Height + 1));
				((Control)_᥆).Size = empty;
				((Control)_᥆).Location = empty2;
				((Control)_᥆).Visible = true;
			}
		}
		else
		{
			if (_᥆ != null)
			{
				((Control)_᥆).Visible = false;
			}
			m_Rect.Height = _1944.Height;
			foreach (BaseItem subItem2 in m_SubItems)
			{
				subItem2.Displayed = false;
			}
		}
		if (_᥆ != null && Expanded)
		{
			_᥆.SetupScrollButtons();
		}
		base.RecalcSize();
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		if (ݥ)
		{
			pa.ButtonStringFormat &= ~(pa.ButtonStringFormat & eTextFormat.SingleLine);
			pa.ButtonStringFormat |= eTextFormat.WordBreak;
		}
		__0E83 = pa;
		Rectangle rect = m_Rect;
		Font font = _໘();
		Graphics graphics = pa.Graphics;
		if (m_NeedRecalcSize)
		{
			RecalcSize();
		}
		if (BarFunctions.IsOffice2007Style(EffectiveStyle))
		{
			SideBarPanelItemRendererEventArgs sideBarPanelItemRendererEventArgs = new SideBarPanelItemRendererEventArgs(this, pa.Graphics);
			sideBarPanelItemRendererEventArgs.က = pa;
			pa.ໝ.DrawSideBarPanelItem(sideBarPanelItemRendererEventArgs);
			rect = _1944;
			rect.Offset(m_Rect.X, m_Rect.Y);
		}
		else
		{
			if (base.IsThemed)
			{
				_0DF4(pa);
				if (ݥ)
				{
					pa.ButtonStringFormat |= eTextFormat.SingleLine;
				}
				__0E83 = null;
				return;
			}
			if (SuspendLayout)
			{
				if (ݥ)
				{
					pa.ButtonStringFormat |= eTextFormat.SingleLine;
				}
				__0E83 = null;
				return;
			}
			ᇎ ᇎ2 = _0EC5();
			Rectangle rectangle = Rectangle.Empty;
			if (_᥍ == eSideBarAppearance.Traditional)
			{
				Color ॱ = SystemColors.Control;
				Color color = _0D02;
				if (color.IsEmpty)
				{
					color = pa.Colors.ItemText;
				}
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (!_0599.BackColor1.IsEmpty || _0599.BackgroundImage != null)
				{
					_0599.Paint(graphics, m_Rect);
					ॱ = _0599.BackColor1.Color;
				}
				if (val != null)
				{
					ॱ = val.BackColor;
					if (_0D02 == SystemColors.ControlText)
					{
						color = val.ForeColor;
					}
				}
				if (ݡ && !_0E79.IsEmpty)
				{
					color = _0E79;
				}
				rect = _1944;
				rect.Offset(m_Rect.X, m_Rect.Y);
				if (m_Text != "")
				{
					if (ݠ)
					{
						BarFunctions._८(graphics, rect.X, rect.Y, rect.Width, rect.Height, (Border3DStyle)10, ॱ);
					}
					else if (ݡ)
					{
						BarFunctions._८(graphics, rect.X, rect.Y, rect.Width, rect.Height, (Border3DStyle)5, ॱ);
					}
					else
					{
						BarFunctions._८(graphics, rect.X, rect.Y, rect.Width, rect.Height, (Border3DStyle)4, ॱ);
					}
				}
				rect.Inflate(-2, -2);
				if (ᇎ2 != null)
				{
					rect.X += 2;
					rect.Width -= 2;
					ᇎ2.ᇗ(graphics, new Rectangle(rect.X, rect.Y + (rect.Height - ᇎ2._0711) / 2, ᇎ2.ܐ, ᇎ2._0711));
					rect.X += ᇎ2.ܐ + 4;
					rect.Width -= ᇎ2.ܐ + 8;
				}
				if (m_Text != "")
				{
					TextDrawing.DrawString(graphics, m_Text, font, color, rect, ಲ());
					rectangle = rect;
				}
			}
			else
			{
				int num = 26;
				if (ᇎ2 != null)
				{
					num = ᇎ2.ܐ + 4;
				}
				if (num >= m_Rect.Width)
				{
					num = 0;
				}
				rect.Offset(num, 0);
				rect.Width -= num;
				if (!_0599.BackColor1.IsEmpty || _0599.BackgroundImage != null)
				{
					_0599.Paint(graphics, rect);
				}
				rect = _1944;
				rect.Offset(m_Rect.Location);
				if (num > 0)
				{
					Rectangle r = new Rectangle(rect.X, rect.Y, num + 1, rect.Height);
					rect.Offset(num + 1, 0);
					rect.Width -= num + 1;
					if (_᥊ != null)
					{
						ItemStyle itemStyle = _᥊.Clone() as ItemStyle;
						if (ݠ && !Expanded)
						{
							itemStyle.ᗄ(_᥌);
						}
						else if (ݡ && !Expanded)
						{
							itemStyle.ᗄ(_᥋);
						}
						itemStyle.Paint(graphics, r);
						ᇎ2?.ᇗ(graphics, new Rectangle(r.X + (r.Width - ᇎ2.ܐ) / 2, r.Y + (r.Height - ᇎ2._0711) / 2, ᇎ2.ܐ, ᇎ2._0711));
					}
				}
				if (_᥇ != null)
				{
					ItemStyle itemStyle2 = _᥇.Clone() as ItemStyle;
					if (ݠ && !Expanded)
					{
						itemStyle2.ᗄ(_᥉);
					}
					else if (ݡ && !Expanded)
					{
						itemStyle2.ᗄ(_᥈);
					}
					else if (Expanded && !_0599.ForeColor.IsEmpty)
					{
						itemStyle2.ForeColor.Color = _0599.ForeColor.Color;
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
			ᇎ2 = null;
			if (Focused)
			{
				if (DesignMode)
				{
					Rectangle ल = _1944;
					ल.Offset(m_Rect.X, m_Rect.Y);
					ल.Inflate(-2, -2);
					_1A6B._1A6C(graphics, ल, pa.Colors.ItemDesignTimeBorder);
				}
				else if (!rectangle.IsEmpty)
				{
					Rectangle rectangle2 = rectangle;
					ControlPaint.DrawFocusRectangle(graphics, rectangle2);
				}
			}
		}
		if (Expanded && m_SubItems != null && _᥆ == null)
		{
			rect = new Rectangle(m_Rect.X, m_Rect.Y + rect.Height + 1, m_Rect.Width, m_Rect.Height - rect.Height - 1);
			graphics.SetClip(rect);
			for (int i = _1940; i < m_SubItems.Count; i++)
			{
				BaseItem baseItem = m_SubItems[i];
				if (!baseItem.Displayed || !baseItem.Visible)
				{
					continue;
				}
				if (baseItem.BeginGroup && _0DC8 == eSideBarLayoutType.Default)
				{
					Color color2 = SystemColors.ControlDark;
					if (_᥇ != null && !_᥇.BorderColor.IsEmpty)
					{
						color2 = _᥇.BorderColor.Color;
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
		if (_᥆ == null)
		{
			if (_1940 > 0 && ᥑ)
			{
				if (BarFunctions.IsOffice2007Style(EffectiveStyle))
				{
					Office2007SideBarColorTable sideBar = ((Office2007Renderer)pa.ໝ).ColorTable.SideBar;
					GradientColorTable sideBarPanelItemDefault = sideBar.SideBarPanelItemDefault;
					if (sideBarPanelItemDefault != null)
					{
						ref Rectangle reference = ref _1942[0];
						reference = new Rectangle(rect.Right - Dpi.Width18, rect.Top + Dpi.Height4, Dpi.Width16, Dpi.Height16);
						Brush val3 = DisplayHelp.CreateBrush(rect, sideBarPanelItemDefault);
						try
						{
							graphics.FillRectangle(val3, _1942[0]);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
						DisplayHelp.DrawRectangle(graphics, sideBar.Border, _1942[0]);
						ᥕ(graphics, _1942[0], sideBar.SideBarPanelItemText, ᥖ: true);
					}
				}
				else if (_᥍ == eSideBarAppearance.Traditional || _᥇ == null)
				{
					ref Rectangle reference2 = ref _1942[0];
					reference2 = new Rectangle(rect.Right - Dpi.Width18, rect.Top + Dpi.Height4, Dpi.Width16, Dpi.Height16);
					BarFunctions._८(graphics, _1942[0], (Border3DStyle)5);
					ᥕ(graphics, _1942[0], SystemColors.ControlText, ᥖ: true);
				}
				else
				{
					ref Rectangle reference3 = ref _1942[0];
					reference3 = new Rectangle(rect.Right - Dpi.Width16, rect.Top + Dpi.Height2, Dpi.Width14, Dpi.Width14);
					ItemStyle itemStyle3 = _᥇.Clone() as ItemStyle;
					itemStyle3.BorderSide = eBorderSide.All;
					itemStyle3.Paint(graphics, _1942[0]);
					ᥕ(graphics, _1942[0], SystemColors.ControlText, ᥖ: true);
				}
			}
			else
			{
				ref Rectangle reference4 = ref _1942[0];
				reference4 = Rectangle.Empty;
			}
			if (_1941)
			{
				if (BarFunctions.IsOffice2007Style(EffectiveStyle))
				{
					Office2007SideBarColorTable sideBar2 = ((Office2007Renderer)pa.ໝ).ColorTable.SideBar;
					GradientColorTable sideBarPanelItemDefault2 = sideBar2.SideBarPanelItemDefault;
					if (sideBarPanelItemDefault2 != null)
					{
						ref Rectangle reference5 = ref _1942[1];
						reference5 = new Rectangle(m_Rect.Right - Dpi.Width18, m_Rect.Bottom - Dpi.Height18, Dpi.Width16, Dpi.Height16);
						Brush val4 = DisplayHelp.CreateBrush(rect, sideBarPanelItemDefault2);
						try
						{
							graphics.FillRectangle(val4, _1942[1]);
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
						DisplayHelp.DrawRectangle(graphics, sideBar2.Border, _1942[1]);
						ᥕ(graphics, _1942[1], sideBar2.SideBarPanelItemText, ᥖ: false);
					}
				}
				else if (_᥍ == eSideBarAppearance.Traditional || _᥇ == null)
				{
					ref Rectangle reference6 = ref _1942[1];
					reference6 = new Rectangle(m_Rect.Right - Dpi.Width18, m_Rect.Bottom - Dpi.Height18, Dpi.Width16, Dpi.Height16);
					BarFunctions._८(graphics, _1942[1], (Border3DStyle)5);
					ᥕ(graphics, _1942[1], SystemColors.ControlText, ᥖ: false);
				}
				else
				{
					ref Rectangle reference7 = ref _1942[1];
					reference7 = new Rectangle(m_Rect.Right - Dpi.Width16, m_Rect.Bottom - Dpi.Height16, Dpi.Width14, Dpi.Height14);
					ItemStyle itemStyle4 = _᥇.Clone() as ItemStyle;
					itemStyle4.BorderSide = eBorderSide.All;
					itemStyle4.Paint(graphics, _1942[1]);
					ᥕ(graphics, _1942[1], SystemColors.ControlText, ᥖ: false);
				}
			}
			else
			{
				ref Rectangle reference8 = ref _1942[1];
				reference8 = Rectangle.Empty;
			}
		}
		if (ݥ)
		{
			pa.ButtonStringFormat |= eTextFormat.SingleLine;
		}
		ᥓ(pa);
		__0E83 = null;
	}

	private void _0DF4(ItemPaintArgs _0BD2)
	{
		if (SuspendLayout)
		{
			return;
		}
		Graphics graphics = _0BD2.Graphics;
		if (m_NeedRecalcSize)
		{
			RecalcSize();
		}
		_0A7C obj = _0BD2._0A7C;
		Rectangle rectangle = _1944;
		rectangle.Offset(m_Rect.X, m_Rect.Y);
		Font val = _໘();
		_0F7B _070B = _0F7B._0F7D;
		_0F80 య = _0F80._0F81;
		if (m_Text != "")
		{
			if (ݠ)
			{
				య = _0F80._0F83;
				_070B = _0F7B._0F7C;
			}
			else if (ݡ)
			{
				య = _0F80._0F82;
				_070B = _0F7B._0F7C;
			}
			obj._0EE5(graphics, _070B, య, rectangle);
		}
		ᇎ ᇎ2 = _0EC5();
		rectangle.Inflate(-2, -2);
		if (ᇎ2 != null)
		{
			rectangle.X += 2;
			rectangle.Width -= 2;
			ᇎ2.ᇗ(graphics, new Rectangle(rectangle.X, rectangle.Y + (rectangle.Height - ᇎ2._0711) / 2, ᇎ2.ܐ, ᇎ2._0711));
			rectangle.X += ᇎ2.ܐ + 4;
			rectangle.Width -= ᇎ2.ܐ + 4;
			rectangle.Inflate(0, -2);
		}
		if (m_Text != "")
		{
			obj._0EE6(graphics, m_Text, val, rectangle, _070B, య, (ThemeTextFormat)32773, !m_Enabled);
		}
		if (DesignMode && Focused)
		{
			Rectangle ल = _1944;
			ल.Offset(m_Rect.X, m_Rect.Y);
			ल.Inflate(-2, -2);
			_1A6B._1A6C(graphics, ल, _0BD2.Colors.ItemDesignTimeBorder);
		}
		if (Expanded && m_SubItems != null && _᥆ == null)
		{
			rectangle = new Rectangle(m_Rect.X, m_Rect.Y + rectangle.Height + 1, m_Rect.Width, m_Rect.Height - rectangle.Height - 1);
			graphics.SetClip(rectangle);
			IntPtr hdc = graphics.GetHdc();
			int num = 0;
			try
			{
				num = _089F.CreateRectRgn(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
				if (num != 0)
				{
					_089F.SelectClipRgn(hdc, num);
				}
			}
			finally
			{
				graphics.ReleaseHdc(hdc);
			}
			for (int i = _1940; i < m_SubItems.Count; i++)
			{
				BaseItem baseItem = m_SubItems[i];
				if (baseItem.Displayed && baseItem.Visible)
				{
					baseItem.Paint(_0BD2);
				}
			}
			graphics.ResetClip();
			if (num != 0)
			{
				hdc = graphics.GetHdc();
				try
				{
					_089F.SelectClipRgn(hdc, 0);
					_0EFE.DeleteObject(num);
				}
				finally
				{
					graphics.ReleaseHdc(hdc);
				}
			}
		}
		if (_᥆ == null)
		{
			if (_1940 > 0)
			{
				_0A7D obj2 = _0BD2._0A7D;
				ref Rectangle reference = ref _1942[0];
				reference = new Rectangle(rectangle.Right - Dpi.Width17, rectangle.Top + Dpi.Height4, Dpi.Width17, Dpi.Height16);
				obj2._0EE5(graphics, _0F86._0F87, _0F90._0F91, _1942[0]);
			}
			else
			{
				ref Rectangle reference2 = ref _1942[0];
				reference2 = Rectangle.Empty;
			}
			if (_1941)
			{
				_0A7D obj3 = _0BD2._0A7D;
				ref Rectangle reference3 = ref _1942[1];
				reference3 = new Rectangle(m_Rect.Right - Dpi.Width17, m_Rect.Bottom - Dpi.Height18, Dpi.Width17, Dpi.Height16);
				obj3._0EE5(graphics, _0F86._0F87, _0F90._0F95, _1942[1]);
			}
			else
			{
				ref Rectangle reference4 = ref _1942[1];
				reference4 = Rectangle.Empty;
			}
		}
		ᥓ(_0BD2);
	}

	private void ᥓ(ItemPaintArgs _0BD2)
	{
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		if (m_SubItems == null || m_SubItems.Count != 0 || !DesignMode || !Expanded)
		{
			return;
		}
		Rectangle rect = m_Rect;
		if (!_1944.IsEmpty)
		{
			rect.Y += _1944.Height;
			rect.Height -= _1944.Height;
		}
		string text = "Right-click header and choose Add New Button or use SubItems collection to create new buttons.";
		rect.Inflate(-1, -1);
		if (_᥍ == eSideBarAppearance.Flat)
		{
			ᇎ ᇎ2 = _0EC5();
			int num = 26;
			if (ᇎ2 != null)
			{
				num = ᇎ2.ܐ + 4;
			}
			if (num >= m_Rect.Width)
			{
				num = 0;
			}
			rect.Offset(num, 0);
			rect.Width -= num;
		}
		Font val = _໘();
		if (val != null)
		{
			Font val2 = new Font(val.FontFamily, val.Size - 1f);
			try
			{
				eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
				TextDrawing.DrawString(_0BD2.Graphics, text, val2, SystemColors.ControlDark, rect, format);
			}
			finally
			{
				val2.Dispose();
			}
		}
	}

	private void ᥔ()
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
			_0A7D obj = null;
			bool flag = false;
			if (val is ል)
			{
				obj = ((ል)val)._0A7D;
			}
			else if (val is Bar)
			{
				obj = ((Bar)(object)val)._0A7D;
			}
			else
			{
				flag = true;
				obj = new _0A7D(val);
			}
			_0F86 _070B = _0F86._0F87;
			_0F90 య = _0F90._0F91;
			if (_1940 > 0)
			{
				if (_1943 == 0)
				{
					య = _0F90._0F92;
				}
				obj._0EE5(val2, _070B, య, _1942[0]);
			}
			if (_1941)
			{
				య = _0F90._0F95;
				if (_1943 == 1)
				{
					య = _0F90._0F96;
				}
				obj._0EE5(val2, _070B, య, _1942[1]);
			}
			if (flag)
			{
				obj.Dispose();
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

	private void ᥕ(Graphics _٥, Rectangle भ, Color _0BDE, bool ᥖ)
	{
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		Point[] array = new Point[3];
		if (ᥖ)
		{
			array[0].X = भ.Left + (भ.Width - 9) / 2;
			array[0].Y = भ.Top + भ.Height / 2 + 1;
			array[1].X = array[0].X + 8;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + 4;
			array[2].Y = array[0].Y - 5;
		}
		else
		{
			array[0].X = भ.Left + (भ.Width - 7) / 2;
			array[0].Y = भ.Top + (भ.Height - 4) / 2;
			array[1].X = array[0].X + 7;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + 3;
			array[2].Y = array[0].Y + 4;
		}
		_٥.FillPolygon((Brush)new SolidBrush(_0BDE), array);
	}

	internal virtual Font _໘()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		Font val = SystemFonts.DefaultFont;
		object containerControl = ContainerControl;
		Control val2 = (Control)((containerControl is Control) ? containerControl : null);
		if (val2 != null)
		{
			val = val2.Font;
		}
		if (ݡ && (_0E7B || _0E7A))
		{
			FontStyle val3 = (FontStyle)0;
			if (_0E7B)
			{
				val3 = (FontStyle)(val3 | 1);
			}
			if (_0E7A)
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
		else if (_0E7C)
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

	internal eTextFormat ಲ()
	{
		eTextFormat eTextFormat2 = eTextFormat.EndEllipsis | eTextFormat.ExternalLeading | eTextFormat.VerticalCenter;
		if (ޔ == eStyleTextAlignment.Center)
		{
			eTextFormat2 |= eTextFormat.HorizontalCenter;
		}
		else if (ޔ == eStyleTextAlignment.Far)
		{
			eTextFormat2 |= eTextFormat.Right;
		}
		if (!ݥ)
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
		if ((int)objArg.Button == 1048576 && (Math.Abs(objArg.X - _0AC2.X) >= 4 || Math.Abs(objArg.Y - _0AC2.Y) >= 4) && ContainerControl is SideBar { AllowUserCustomize: not false } && CanCustomize)
		{
			BaseItem hotSubItem = m_HotSubItem;
			if (hotSubItem != null && hotSubItem.CanCustomize && !hotSubItem.SystemItem && GetOwner() is IOwner { DragItem: null } owner)
			{
				owner.StartItemDrag(hotSubItem);
				return;
			}
		}
		bool flag = false;
		if (_1942[0].Contains(objArg.X, objArg.Y) || _1942[1].Contains(objArg.X, objArg.Y))
		{
			if (m_HotSubItem != null)
			{
				m_HotSubItem.InternalMouseLeave();
				m_HotSubItem = null;
			}
			if (_1942[0].Contains(objArg.X, objArg.Y))
			{
				_1943 = 0;
			}
			else
			{
				_1943 = 1;
			}
			if (base.IsThemed)
			{
				ᥔ();
			}
			return;
		}
		if (_1943 >= 0)
		{
			_1943 = -1;
			flag = true;
		}
		if (_᥆ != null)
		{
			m_IsContainer = false;
			base.InternalMouseMove(objArg);
			m_IsContainer = true;
		}
		else
		{
			base.InternalMouseMove(objArg);
		}
		Rectangle rectangle = _1944;
		rectangle.Offset(m_Rect.Location);
		if (rectangle.Contains(objArg.X, objArg.Y))
		{
			ݡ = true;
			Refresh();
		}
		else if (ݡ)
		{
			ݡ = false;
			Refresh();
		}
		else if (flag && base.IsThemed)
		{
			ᥔ();
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		ݡ = false;
		ݠ = false;
		if (_1943 >= 0)
		{
			_1943 = -1;
		}
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Invalid comparison between Unknown and I4
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Invalid comparison between Unknown and I4
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Invalid comparison between Unknown and I4
		Rectangle rectangle = _1944;
		rectangle.Offset(m_Rect.Location);
		_0AC2 = new Point(objArg.X, objArg.Y);
		if (DesignMode)
		{
			if (ItemAtLocation(objArg.X, objArg.Y) == null && !rectangle.Contains(objArg.X, objArg.Y) && Expanded)
			{
				if (GetOwner() is IOwner owner)
				{
					owner.SetFocusItem(null);
				}
			}
			else if (!rectangle.Contains(objArg.X, objArg.Y))
			{
				base.InternalMouseDown(objArg);
			}
			return;
		}
		if ((int)objArg.Button == 1048576 && _1942[0].Contains(objArg.X, objArg.Y))
		{
			ᥗ(ᥖ: true);
			return;
		}
		if ((int)objArg.Button == 1048576 && _1942[1].Contains(objArg.X, objArg.Y))
		{
			ᥗ(ᥖ: false);
			return;
		}
		if ((int)objArg.Button == 1048576 && rectangle.Contains(objArg.X, objArg.Y))
		{
			ݠ = true;
			Refresh();
		}
		if (_᥏ != DateTime.MinValue)
		{
			if (DateTime.Now.Subtract(_᥏).TotalMilliseconds < 1000.0 && ᥐ.Contains(objArg.X, objArg.Y))
			{
				return;
			}
			_᥏ = DateTime.MinValue;
		}
		base.InternalMouseDown(objArg);
	}

	internal void _੨(MouseEventArgs ؿ)
	{
		if (_193A)
		{
			ᥗ(ؿ.Delta < 0);
		}
	}

	internal void ᥗ(bool ᥖ)
	{
		if (ᥖ)
		{
			if (_1940 > 0)
			{
				ᥐ = _1942[0];
				if (_0DC8 == eSideBarLayoutType.MultiColumn)
				{
					while (_1940 > 0)
					{
						_1940--;
						if (SubItems[_1940].Visible)
						{
							break;
						}
					}
				}
				else
				{
					_1940--;
				}
				NeedRecalcSize = true;
				Refresh();
				_᥎ = true;
			}
		}
		else if (!_1942[1].IsEmpty)
		{
			ᥐ = _1942[1];
			if (_0DC8 == eSideBarLayoutType.MultiColumn)
			{
				int topInternal = SubItems[_1940].TopInternal;
				while (_1940 + 1 < SubItems.Count)
				{
					_1940++;
					if (SubItems[_1940].Visible && SubItems[_1940].TopInternal > topInternal)
					{
						break;
					}
				}
			}
			else
			{
				_1940++;
			}
			NeedRecalcSize = true;
			Refresh();
			_᥎ = true;
		}
		if (_᥆ != null)
		{
			((Control)_᥆).Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576)
		{
			ݠ = false;
			Refresh();
		}
		if (_᥎)
		{
			_᥎ = false;
			_᥏ = DateTime.Now;
		}
		else if (_᥏ != DateTime.MinValue)
		{
			return;
		}
		Rectangle rectangle = _1944;
		rectangle.Offset(m_Rect.Location);
		base.InternalMouseUp(objArg);
		if ((!(GetOwner() is IOwner) || !((IOwner)GetOwner()).DragInProgress) && !Expanded && (int)objArg.Button == 1048576 && rectangle.Contains(objArg.X, objArg.Y))
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
		_1940 = 0;
	}

	public void ResetBackgroundStyle()
	{
		ᥘ(new ItemStyle());
	}

	internal void ᥘ(ItemStyle ᥙ)
	{
		if (_0599 != null)
		{
			_0599.ઐ -= ઐ;
		}
		_0599 = ᥙ;
		if (_0599 != null)
		{
			_0599.ઐ += ઐ;
		}
	}

	public void ResetHeaderStyle()
	{
		ᥚ(null);
	}

	internal void ᥚ(ItemStyle _0657)
	{
		if (_᥇ != null)
		{
			_᥇.ઐ -= ઐ;
		}
		_᥇ = _0657;
		if (_᥇ != null)
		{
			_᥇.ઐ += ઐ;
		}
	}

	public void ResetHeaderHotStyle()
	{
		ᥛ(null);
	}

	internal void ᥛ(ItemStyle _0657)
	{
		if (_᥈ != null)
		{
			_᥈.ઐ -= ઐ;
		}
		_᥈ = _0657;
		if (_᥈ != null)
		{
			_᥈.ઐ += ઐ;
		}
	}

	public void ResetHeaderMouseDownStyle()
	{
		ᥜ(null);
	}

	internal void ᥜ(ItemStyle _0657)
	{
		if (_᥉ != null)
		{
			_᥉.ઐ -= ઐ;
		}
		_᥉ = _0657;
		if (_᥉ != null)
		{
			_᥉.ઐ += ઐ;
		}
	}

	public void ResetHeaderSideStyle()
	{
		ᥝ(null);
	}

	internal void ᥝ(ItemStyle _0657)
	{
		if (_᥊ != null)
		{
			_᥊.ઐ -= ઐ;
		}
		_᥊ = _0657;
		if (_᥊ != null)
		{
			_᥊.ઐ += ઐ;
		}
	}

	public void ResetHeaderSideHotStyle()
	{
		ᥞ(null);
	}

	internal void ᥞ(ItemStyle _0657)
	{
		if (_᥋ != null)
		{
			_᥋.ઐ -= ઐ;
		}
		_᥋ = _0657;
		if (_᥋ != null)
		{
			_᥋.ઐ += ઐ;
		}
	}

	public void ResetHeaderSideMouseDownStyle()
	{
		ᥟ(null);
	}

	internal void ᥟ(ItemStyle _0657)
	{
		if (_᥌ != null)
		{
			_᥌.ઐ -= ઐ;
		}
		_᥌ = _0657;
		if (_᥌ != null)
		{
			_᥌.ઐ += ઐ;
		}
	}

	public bool ShouldSerializeHotForeColor()
	{
		if (_0E79 != Color.Empty)
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
		return !_0D02.IsEmpty;
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (_0D02 != SystemColors.ControlText && !_0D02.IsEmpty)
		{
			itemXmlElement.SetAttribute("forecolor", BarFunctions.ColorToString(_0D02));
		}
		if (_0E7B)
		{
			itemXmlElement.SetAttribute("hotfb", XmlConvert.ToString(_0E7B));
		}
		if (_0E7A)
		{
			itemXmlElement.SetAttribute("hotfu", XmlConvert.ToString(_0E7A));
		}
		if (!_0E79.IsEmpty)
		{
			itemXmlElement.SetAttribute("hotclr", BarFunctions.ColorToString(_0E79));
		}
		if (_1945 != 0)
		{
			itemXmlElement.SetAttribute("itemimagesize", XmlConvert.ToString((int)_1945));
		}
		if (!ᥑ)
		{
			itemXmlElement.SetAttribute("enablescrollbuttons", XmlConvert.ToString(ᥑ));
		}
		XmlElement xmlElement = null;
		XmlElement xmlElement2 = null;
		if (_0E66 != null || դ >= 0 || _0E68 != null || _0E69 >= 0 || _0E6E != null || _0E6F >= 0 || ຈ != null)
		{
			xmlElement = itemXmlElement.OwnerDocument.CreateElement("images");
			itemXmlElement.AppendChild(xmlElement);
			if (դ >= 0)
			{
				xmlElement.SetAttribute("imageindex", XmlConvert.ToString(դ));
			}
			if (_0E69 >= 0)
			{
				xmlElement.SetAttribute("hoverimageindex", XmlConvert.ToString(_0E69));
			}
			if (_0E6F >= 0)
			{
				xmlElement.SetAttribute("pressedimageindex", XmlConvert.ToString(_0E6F));
			}
			if (_0E66 != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "default");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(_0E66, xmlElement2);
			}
			if (_0E68 != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "hover");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(_0E68, xmlElement2);
			}
			if (_0E6E != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "pressed");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(_0E6E, xmlElement2);
			}
			if (ຈ != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "icon");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeIcon(ຈ, xmlElement2);
			}
		}
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		if (itemXmlElement.HasAttribute("forecolor"))
		{
			_0D02 = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("forecolor"));
		}
		else
		{
			_0D02 = Color.Empty;
		}
		if (itemXmlElement.HasAttribute("hotclr"))
		{
			_0E79 = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("hotclr"));
		}
		else
		{
			_0E79 = Color.Empty;
		}
		if (itemXmlElement.HasAttribute("hotfb"))
		{
			_0E7B = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("hotfb"));
		}
		else
		{
			_0E7B = false;
		}
		if (itemXmlElement.HasAttribute("hotfu"))
		{
			_0E7A = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("hotfu"));
		}
		else
		{
			_0E7A = false;
		}
		if (itemXmlElement.HasAttribute("itemimagesize"))
		{
			_1945 = (eBarImageSize)XmlConvert.ToInt32(itemXmlElement.GetAttribute("itemimagesize"));
		}
		else
		{
			_1945 = eBarImageSize.Default;
		}
		if (itemXmlElement.HasAttribute("enablescrollbuttons"))
		{
			ᥑ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("enablescrollbuttons"));
		}
		else
		{
			ᥑ = true;
		}
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			if (!(childNode.Name == "images"))
			{
				continue;
			}
			if (childNode.HasAttribute("imageindex"))
			{
				դ = XmlConvert.ToInt32(childNode.GetAttribute("imageindex"));
			}
			if (childNode.HasAttribute("hoverimageindex"))
			{
				_0E69 = XmlConvert.ToInt32(childNode.GetAttribute("hoverimageindex"));
			}
			if (childNode.HasAttribute("pressedimageindex"))
			{
				_0E6F = XmlConvert.ToInt32(childNode.GetAttribute("pressedimageindex"));
			}
			foreach (XmlElement childNode2 in childNode.ChildNodes)
			{
				switch (childNode2.GetAttribute("type"))
				{
				case "default":
					_0E66 = BarFunctions.DeserializeImage(childNode2);
					դ = -1;
					break;
				case "icon":
					ຈ = BarFunctions.DeserializeIcon(childNode2);
					դ = -1;
					break;
				case "hover":
					_0E68 = BarFunctions.DeserializeImage(childNode2);
					_0E69 = -1;
					break;
				case "pressed":
					_0E6E = BarFunctions.DeserializeImage(childNode2);
					_0E6F = -1;
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
			ᥠ();
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
		if (_᥆ != null)
		{
			item.ContainerControl = _᥆;
		}
		if (item is ButtonItem)
		{
			((ButtonItem)item)._ງ = ݥ;
		}
	}

	protected internal override void OnBeforeItemRemoved(BaseItem item)
	{
		if (item is ButtonItem)
		{
			((ButtonItem)item)._ງ = false;
		}
		base.OnBeforeItemRemoved(item);
	}

	private void ᥠ()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null && _᥆ == null)
		{
			_᥆ = new SideBarPanelControlHost(this);
			((Control)_᥆).Visible = false;
			val.Controls.Add((Control)(object)_᥆);
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
		if (val == null || !_1944.Contains(val.PointToClient(Control.MousePosition)))
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
		if (_᥆ == null || !Expanded)
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
			if (val == null || !BaseItem.IsHandleValid(val) || val is ᆓ)
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
			Rectangle rectangle = _1944;
			rectangle.Inflate(4, 4);
			val.Invalidate(m_Rect, false);
		}
	}

	internal void _0EBE(int _0EBF)
	{
		դ = _0EBF;
	}

	internal ᇎ _0EC5()
	{
		if (ݠ)
		{
			return _0EC5(_1937._0F78);
		}
		if (ݡ)
		{
			return _0EC5(_1937._1938);
		}
		return _0EC5(_1937.ሬ);
	}

	private ᇎ _0EC5(_1937 య)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Image val = null;
		if (ຈ != null)
		{
			Size size = _0E5A;
			Icon val2 = null;
			try
			{
				val2 = new Icon(ຈ, size);
			}
			catch
			{
				val2 = null;
			}
			if (val2 == null)
			{
				return new ᇎ(ຈ, dispose: false);
			}
			return new ᇎ(val2, dispose: true);
		}
		if (య == _1937._1938 && (_0E68 != null || _0E69 >= 0))
		{
			if (_0E68 != null)
			{
				return new ᇎ(_0E68, dispose: false);
			}
			if (_0E69 >= 0)
			{
				val = _0EC9(_0E69);
				if (val != null)
				{
					return new ᇎ(val, dispose: false);
				}
				return null;
			}
		}
		if (య == _1937._0F78 && (_0E6E != null || _0E6F >= 0))
		{
			if (_0E6E != null)
			{
				return new ᇎ(_0E6E, dispose: false);
			}
			if (_0E6F >= 0)
			{
				val = _0EC9(_0E6F);
				if (val != null)
				{
					return new ᇎ(val, dispose: false);
				}
				return null;
			}
		}
		if (_0E66 != null)
		{
			return new ᇎ(_0E66, dispose: false);
		}
		if (դ >= 0)
		{
			val = _0EC9(դ);
			if (val != null)
			{
				return new ᇎ(val, dispose: false);
			}
		}
		return null;
	}

	private Image _0EC9(int _0ECA)
	{
		if (_0ECA >= 0)
		{
			IOwner owner = null;
			IBarImageSize barImageSize = null;
			if (__0E83 != null)
			{
				owner = __0E83.Owner;
				barImageSize = __0E83.ContainerControl as IBarImageSize;
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
							return owner.ImagesMedium.Images[_0ECA];
						}
						if (barImageSize.ImageSize == eBarImageSize.Large && owner.ImagesLarge != null)
						{
							return owner.ImagesLarge.Images[_0ECA];
						}
						if (owner.Images != null)
						{
							if (_0ECA == դ)
							{
								if (ຂ == null)
								{
									ຂ = owner.Images.Images[_0ECA];
								}
								return ຂ;
							}
							return owner.Images.Images[_0ECA];
						}
					}
					else if (m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize != 0)
					{
						eBarImageSize itemImageSize = ((SideBarPanelItem)m_Parent).ItemImageSize;
						if (itemImageSize == eBarImageSize.Medium && owner.ImagesMedium != null)
						{
							return owner.ImagesMedium.Images[_0ECA];
						}
						if (itemImageSize == eBarImageSize.Large && owner.ImagesLarge != null)
						{
							return owner.ImagesLarge.Images[_0ECA];
						}
						if (owner.Images != null)
						{
							return owner.Images.Images[_0ECA];
						}
					}
					else if (owner.Images != null)
					{
						if (_0ECA == դ)
						{
							if (ຂ == null)
							{
								ຂ = owner.Images.Images[_0ECA];
							}
							return ຂ;
						}
						return owner.Images.Images[_0ECA];
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

	private InsertPosition ᆉ(Point ॺ, BaseItem _0C78)
	{
		InsertPosition insertPosition = null;
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || !CanCustomize)
		{
			return null;
		}
		Point pt = val.PointToClient(ॺ);
		Rectangle displayRectangle = DisplayRectangle;
		if (displayRectangle.Contains(pt))
		{
			Rectangle rectangle = displayRectangle;
			rectangle.Size = _1944.Size;
			rectangle.X += _1944.Left;
			rectangle.Y += _1944.Top;
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
				insertPosition = designTimeProvider.GetInsertPosition(ॺ, _0C78);
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
				if (baseItem == _0C78)
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
				if (subItem != _0C78 && subItem is IDesignTimeProvider designTimeProvider2)
				{
					insertPosition = designTimeProvider2.GetInsertPosition(ॺ, _0C78);
					if (insertPosition != null)
					{
						break;
					}
				}
			}
		}
		return insertPosition;
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point ॺ, BaseItem _0C78)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆉ
		return this.ᆉ(ॺ, _0C78);
	}

	private void ᆋ(int ᆌ, bool ᆍ)
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null)
		{
			return;
		}
		BaseItem baseItem = null;
		if (ᆌ >= 0)
		{
			baseItem = SubItems[ᆌ];
		}
		if (baseItem != null)
		{
			if (baseItem.DesignInsertMarker != 0)
			{
				baseItem.DesignInsertMarker = ᥡ.ᘶ;
			}
			else if (ᆍ)
			{
				baseItem.DesignInsertMarker = ᥡ.ᆍ;
			}
			else
			{
				baseItem.DesignInsertMarker = ᥡ.ᥢ;
			}
		}
		else
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			displayRectangle.Offset(_1944.X, _1944.Bottom);
			new Rectangle(displayRectangle.Left + 2, displayRectangle.Top + 2, displayRectangle.Width - 4, 1);
			new Rectangle(displayRectangle.Left + 1, displayRectangle.Top, 1, 5);
			new Rectangle(displayRectangle.Right - 2, displayRectangle.Top, 1, 5);
		}
	}

	void IDesignTimeProvider.DrawReversibleMarker(int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆋ
		this.ᆋ(ᆌ, ᆍ);
	}

	private void ᆎ(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		if (ExpandedItem() != null)
		{
			ExpandedItem().Expanded = false;
		}
		if (!ᆍ)
		{
			if (ᆌ + 1 >= SubItems.Count)
			{
				SubItems.Add(ਤ, _෭(this));
			}
			else
			{
				SubItems.Add(ਤ, ᆌ + 1);
			}
		}
		else if (ᆌ >= SubItems.Count)
		{
			SubItems.Add(ਤ, _෭(this));
		}
		else
		{
			SubItems.Add(ਤ, ᆌ);
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

	void IDesignTimeProvider.InsertItemAt(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆎ
		this.ᆎ(ਤ, ᆌ, ᆍ);
	}

	private int _෭(BaseItem _෮)
	{
		int result = -1;
		int num = _෮.SubItems.Count - 1;
		while (num >= 0 && _෮.SubItems[num].SystemItem)
		{
			result = num;
			num--;
		}
		return result;
	}
}
