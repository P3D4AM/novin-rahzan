using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Ribbon;
using DevComponents.DotNetBar.TextMarkup;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.SimpleBaseItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
[DefaultEvent("Click")]
public class LabelItem : BaseItem, IBlock, IBlockExtended
{
	internal const eBorderSide ᖺ = eBorderSide.All;

	private eBorderType ᧆ;

	private Color ފ = Color.Empty;

	private Color _0D02 = Color.Empty;

	private Color _0B80 = SystemColors.ControlDark;

	private bool ᧇ;

	private StringAlignment ޔ;

	private StringAlignment ޕ = (StringAlignment)1;

	private Font ޓ;

	private bool ᧈ;

	private int ޠ;

	private int ޡ;

	private int ޢ;

	private int ޣ;

	private int ܘ;

	private int ܗ;

	private bool ݥ;

	private eImagePosition _0E71;

	private Image _0E66;

	private Image ຂ;

	private Image _0E6A;

	private Icon _0E6D;

	private Icon ຈ;

	private int դ = -1;

	private ItemPaintArgs __0E83;

	private eBorderSide ᖿ = eBorderSide.All;

	private bool ᧉ;

	internal bool _19CA;

	private MarkupLinkClickEventHandler Ӟ;

	private int _ᒘ = 2;

	private bool __0EB9 = true;

	private eColorSchemePart __0EB6 = eColorSchemePart.None;

	private bool __19CB;

	private eOrientation __19CC;

	private bool __19CD = true;

	private string _ລ = "";

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	private float _ຨ;

	private Color __0EA4 = Color.Empty;

	private bool __19CE;

	[Description("Specifies spacing between label image and text.")]
	[DefaultValue(2)]
	[Category("Appearance")]
	public int ImageTextSpacing
	{
		get
		{
			return _ᒘ;
		}
		set
		{
			if (value != _ᒘ)
			{
				int ᒘ = _ᒘ;
				_ᒘ = value;
				OnImageTextSpacingChanged(ᒘ, value);
			}
		}
	}

	protected override bool IsMarkupSupported => __0EB9;

	[DefaultValue(true)]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	[Category("Appearance")]
	public bool EnableMarkup
	{
		get
		{
			return __0EB9;
		}
		set
		{
			if (__0EB9 != value)
			{
				__0EB9 = value;
				NeedRecalcSize = true;
				OnTextChanged();
			}
		}
	}

	[Description("Indicates the type of the border drawn around the label.")]
	[DefaultValue(eBorderType.None)]
	[Browsable(true)]
	[Category("Appearance")]
	public eBorderType BorderType
	{
		get
		{
			return ᧆ;
		}
		set
		{
			if (ᧆ != value)
			{
				ᧆ = value;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[DefaultValue(eBorderSide.All)]
	[Browsable(true)]
	[Description("Specifies border sides that are displayed.")]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	public eBorderSide BorderSide
	{
		get
		{
			return ᖿ;
		}
		set
		{
			if (ᖿ != value)
			{
				ᖿ = value;
				Refresh();
			}
		}
	}

	[Description("Determines the background color of the label.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color BackColor
	{
		get
		{
			return ފ;
		}
		set
		{
			if (!(ފ == value))
			{
				ފ = value;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	internal eColorSchemePart _0E61
	{
		get
		{
			return __0EB6;
		}
		set
		{
			__0EB6 = value;
		}
	}

	[Description("Indicates text color.")]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public Color ForeColor
	{
		get
		{
			return _0D02;
		}
		set
		{
			if (!(_0D02 == value))
			{
				_0D02 = value;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Description("Indicates border line color when border is single line.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	public Color SingleLineColor
	{
		get
		{
			return _0B80;
		}
		set
		{
			if (!(_0B80 == value))
			{
				_0B80 = value;
				if (_0B80 != SystemColors.ControlDark)
				{
					ᧇ = true;
				}
				else
				{
					ᧇ = false;
				}
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Description("Indicates text alignment.")]
	[DevCoBrowsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Category("Layout")]
	public StringAlignment TextAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ޔ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (ޔ != value)
			{
				ޔ = value;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Browsable(true)]
	[Description("Indicates text line alignment.")]
	public StringAlignment TextLineAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ޕ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (ޕ != value)
			{
				ޕ = value;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates label font.")]
	public Font Font
	{
		get
		{
			return ޓ;
		}
		set
		{
			if (ޓ != value)
			{
				ޓ = value;
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[DefaultValue(0)]
	[Category("Layout")]
	[DevCoBrowsable(true)]
	[Description("Indicates left padding in pixels.")]
	[Browsable(true)]
	public int PaddingLeft
	{
		get
		{
			return ޠ;
		}
		set
		{
			if (ޠ != value)
			{
				ޠ = value;
				NeedRecalcSize = true;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Description("Indicates right padding in pixels.")]
	[Category("Layout")]
	[Browsable(true)]
	public int PaddingRight
	{
		get
		{
			return ޡ;
		}
		set
		{
			if (ޡ != value)
			{
				ޡ = value;
				NeedRecalcSize = true;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates top padding in pixels.")]
	[Category("Layout")]
	[DevCoBrowsable(true)]
	public int PaddingTop
	{
		get
		{
			return ޢ;
		}
		set
		{
			if (ޢ != value)
			{
				ޢ = value;
				NeedRecalcSize = true;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Description("Indicates bottom padding in pixels.")]
	[Category("Layout")]
	[Browsable(true)]
	public int PaddingBottom
	{
		get
		{
			return ޣ;
		}
		set
		{
			if (ޣ != value)
			{
				ޣ = value;
				NeedRecalcSize = true;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[DefaultValue(0)]
	[DevCoBrowsable(true)]
	[Description("Indicates the width of the label in pixels.")]
	[Browsable(true)]
	[Category("Layout")]
	public int Width
	{
		get
		{
			return ܘ;
		}
		set
		{
			if (ܘ != value)
			{
				ܘ = value;
				NeedRecalcSize = true;
				if (!_19CA)
				{
					Refresh();
					OnAppearanceChanged();
				}
			}
		}
	}

	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates height of the label in pixels.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public int Height
	{
		get
		{
			return ܗ;
		}
		set
		{
			if (ܗ != value)
			{
				ܗ = value;
				NeedRecalcSize = true;
				if (!_19CA)
				{
					Refresh();
					OnAppearanceChanged();
				}
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether label has divider style.")]
	[DevCoBrowsable(true)]
	public bool DividerStyle
	{
		get
		{
			return ᧈ;
		}
		set
		{
			if (ᧈ != value)
			{
				ᧈ = value;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets a value that determines whether text is displayed in multiple lines or one long line.")]
	public bool WordWrap
	{
		get
		{
			return ݥ;
		}
		set
		{
			if (ݥ != value)
			{
				ݥ = value;
				m_NeedRecalcSize = true;
			}
			OnAppearanceChanged();
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates whether label text is rendered using bold font unless the Font property is specifically set to different font.")]
	public bool FontBold
	{
		get
		{
			return __19CB;
		}
		set
		{
			if (__19CB != value)
			{
				bool oldValue = __19CB;
				__19CB = value;
				OnFontBoldChanged(value, oldValue);
			}
		}
	}

	internal bool ᧄ
	{
		get
		{
			return ᧉ;
		}
		set
		{
			ᧉ = value;
		}
	}

	[Description("The text contained in the item.")]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			if (m_Text == value)
			{
				return;
			}
			if (value == null)
			{
				m_Text = "";
			}
			else
			{
				m_Text = value;
			}
			m_AccessKey = _089F.य(m_Text);
			if (ShouldSyncProperties)
			{
				BarFunctions.ढ़(this, "Text");
			}
			OnTextChanged();
			if (Displayed && m_Parent != null && !SuspendLayout)
			{
				if (ܘ == 0 || ContainerControl is BaseItemControl)
				{
					NeedRecalcSize = true;
				}
				if (!(ContainerControl is LabelX))
				{
					Refresh();
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the label icon.")]
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
			ຈ = value;
			ᡁ();
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed on the face of the item.")]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return _0E66;
		}
		set
		{
			_0E66 = value;
			ᡁ();
			OnAppearanceChanged();
		}
	}

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

	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[Description("The image list image index of the image that will be displayed on the face of the item.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(-1)]
	[Category("Appearance")]
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
				ᡁ();
			}
			OnAppearanceChanged();
		}
	}

	[DefaultValue(eImagePosition.Left)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("The alignment of the image in relation to text displayed by this item.")]
	[DevCoBrowsable(true)]
	public eImagePosition ImagePosition
	{
		get
		{
			return _0E71;
		}
		set
		{
			if (_0E71 != value)
			{
				_0E71 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "ImagePosition");
				}
				NeedRecalcSize = true;
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

	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	[Description("Indicates text-orientation")]
	public eOrientation TextOrientation
	{
		get
		{
			return __19CC;
		}
		set
		{
			__19CC = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[Description("Indicates how vertical text is rotated when TextOrientation = Vertical.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool VerticalTextTopUp
	{
		get
		{
			return __19CD;
		}
		set
		{
			__19CD = value;
			if (__19CC == eOrientation.Vertical)
			{
				Refresh();
			}
		}
	}

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates the symbol displayed on face of the tile instead of the image. Setting the symbol overrides the image setting.")]
	[DefaultValue("")]
	[Category("Appearance")]
	public string Symbol
	{
		get
		{
			return _ລ;
		}
		set
		{
			if (value != _ລ)
			{
				string ລ = _ລ;
				_ລ = value;
				OnSymbolChanged(ລ, value);
			}
		}
	}

	[DefaultValue(eSymbolSet.Awesome)]
	[Browsable(false)]
	public eSymbolSet SymbolSet
	{
		get
		{
			return _ວ;
		}
		set
		{
			if (_ວ != value)
			{
				eSymbolSet ວ = _ວ;
				_ວ = value;
				OnSymbolSetChanged(ວ, value);
			}
		}
	}

	[DefaultValue(0f)]
	[Description("Indicates the size of the symbol in points.")]
	[Category("Appearance")]
	public float SymbolSize
	{
		get
		{
			return _ຨ;
		}
		set
		{
			if (value != _ຨ)
			{
				float ຨ = _ຨ;
				_ຨ = value;
				OnSymbolSizeChanged(ຨ, value);
			}
		}
	}

	[Description("Indicates color of the symbol.")]
	[Category("Columns")]
	public Color SymbolColor
	{
		get
		{
			return __0EA4;
		}
		set
		{
			__0EA4 = value;
			Refresh();
		}
	}

	[Description("Indicates whether label hosted in multi-line ItemContainer triggers new line switch after label is processed.")]
	[Category("Layout")]
	[DefaultValue(false)]
	public bool ContainerNewLineAfter
	{
		get
		{
			return __19CE;
		}
		set
		{
			__19CE = value;
		}
	}

	bool IBlockExtended.ᅜ => false;

	bool IBlockExtended.ᅝ => __19CE;

	bool IBlockExtended.ᅞ => true;

	bool IBlockExtended.ᧅ => false;

	[Description("Occurs when text markup link is clicked. Markup links can be created using a tag.")]
	public event MarkupLinkClickEventHandler MarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	protected virtual void OnImageTextSpacingChanged(int oldValue, int newValue)
	{
		NeedRecalcSize = true;
		Invalidate();
	}

	public LabelItem()
		: this("", "")
	{
	}

	public LabelItem(string sName)
		: this(sName, "")
	{
	}

	public LabelItem(string sName, string ItemText)
		: base(sName, ItemText)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		IsAccessible = false;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.ᆟ(ref _0E66);
			BarUtilities.ᆟ(ref ຂ);
			BarUtilities.ᆟ(ref ຈ);
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		LabelItem labelItem = new LabelItem(m_Name);
		CopyToItem(labelItem);
		return labelItem;
	}

	internal void _0EBA(LabelItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		LabelItem labelItem = copy as LabelItem;
		base.CopyToItem(labelItem);
		labelItem.BorderType = ᧆ;
		labelItem.BorderSide = ᖿ;
		labelItem.BackColor = ފ;
		labelItem.ForeColor = _0D02;
		if (ᧇ)
		{
			labelItem.SingleLineColor = _0B80;
		}
		labelItem.WidthInternal = WidthInternal;
		labelItem.HeightInternal = HeightInternal;
		labelItem.TextAlignment = TextAlignment;
		labelItem.TextLineAlignment = TextLineAlignment;
		labelItem.Font = ޓ;
		labelItem.PaddingBottom = ޣ;
		labelItem.PaddingLeft = ޠ;
		labelItem.PaddingRight = ޡ;
		labelItem.PaddingTop = ޢ;
		if (Image != null)
		{
			labelItem.Image = (Image)Image.Clone();
		}
		labelItem.ImageIndex = ImageIndex;
		labelItem.ImagePosition = ImagePosition;
		labelItem.WordWrap = WordWrap;
		labelItem.DividerStyle = DividerStyle;
		labelItem.EnableMarkup = EnableMarkup;
		labelItem.Width = Width;
		labelItem.Height = Height;
		labelItem.Symbol = _ລ;
		labelItem.SymbolSet = _ວ;
		labelItem.SymbolSize = _ຨ;
		labelItem.SymbolColor = __0EA4;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Invalid comparison between Unknown and I4
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Expected O, but got Unknown
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Expected O, but got Unknown
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Invalid comparison between Unknown and I4
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Invalid comparison between Unknown and I4
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Invalid comparison between Unknown and I4
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Invalid comparison between Unknown and I4
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Invalid comparison between Unknown and I4
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Invalid comparison between Unknown and I4
		//IL_0c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9f: Invalid comparison between Unknown and I4
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d36: Invalid comparison between Unknown and I4
		//IL_0d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d97: Invalid comparison between Unknown and I4
		if (SuspendLayout || !base.IsRendered)
		{
			return;
		}
		__0E83 = pa;
		Graphics graphics = pa.Graphics;
		ᇎ ᇎ2 = null;
		Size size = Size.Empty;
		bool flag = false;
		if (string.IsNullOrEmpty(SymbolRealized))
		{
			ᇎ2 = _0EC5();
			if (ᇎ2 != null)
			{
				size = ᇎ2.ᇐ;
			}
		}
		else
		{
			flag = true;
			Font font = Symbols.GetFont(SymbolSize, SymbolSet);
			size = TextDrawing.MeasureString(graphics, SymbolRealized, font);
			int num = (int)Math.Ceiling((float)font.FontFamily.GetCellDescent(font.Style) * font.Size / (float)font.FontFamily.GetEmHeight(font.Style));
			size.Height -= num;
		}
		Rectangle displayRectangle = DisplayRectangle;
		Rectangle rectangle = Rectangle.Empty;
		Border3DSide val = (ᧈ ? ((Border3DSide)2) : ((ᖿ != eBorderSide.All) ? ((Border3DSide)((((ᖿ & eBorderSide.Left) != 0) ? 1 : 0) | (((ᖿ & eBorderSide.Right) != 0) ? 4 : 0) | (((ᖿ & eBorderSide.Top) != 0) ? 2 : 0) | (((ᖿ & eBorderSide.Bottom) != 0) ? 8 : 0))) : ((Border3DSide)15)));
		if (!(pa.ContainerControl is LabelX))
		{
			DisplayHelp.FillRectangle(graphics, DisplayRectangle, ފ, Color.Empty);
		}
		if (ᧈ)
		{
			object containerControl = ContainerControl;
			Control val2 = (Control)((containerControl is Control) ? containerControl : null);
			RightToLeft val3 = (RightToLeft)0;
			if (val2 != null && (int)val2.RightToLeft == 1)
			{
				val3 = (RightToLeft)1;
			}
			bool disposeFont = false;
			Font font2 = GetFont(out disposeFont);
			Size size2 = TextDrawing.MeasureString(graphics, m_Text, font2, m_Rect.Width, ಲ());
			if (disposeFont)
			{
				font2.Dispose();
			}
			size2.Width += 4;
			if (size2.Width > displayRectangle.Width)
			{
				size2.Width = displayRectangle.Width;
			}
			rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y, size2.Width, size2.Height);
			if ((int)ޔ == 1)
			{
				rectangle.Offset((displayRectangle.Width - size2.Width) / 2, 0);
			}
			else if (((int)ޔ == 2 && (int)val3 == 0) || ((int)ޔ == 0 && (int)val3 == 1))
			{
				rectangle.Offset(displayRectangle.Width - rectangle.Width, 0);
			}
			if ((int)ޕ == 1)
			{
				rectangle.Offset(0, (displayRectangle.Height - rectangle.Height) / 2);
			}
			else if (((int)ޕ == 2 && (int)val3 == 0) || ((int)ޕ == 0 && (int)val3 == 1))
			{
				rectangle.Offset(0, displayRectangle.Height - rectangle.Height);
			}
		}
		switch (ᧆ)
		{
		case eBorderType.Bump:
			if (ᧈ)
			{
				ControlPaint.DrawBorder3D(graphics, m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Width, 2, (Border3DStyle)9, val);
				break;
			}
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)9, val);
			displayRectangle.Inflate(-2, -2);
			break;
		case eBorderType.Etched:
			if (ᧈ)
			{
				ControlPaint.DrawBorder3D(graphics, m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Width, 2, (Border3DStyle)6, val);
				break;
			}
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)6, val);
			displayRectangle.Inflate(-2, -2);
			break;
		case eBorderType.Raised:
		case eBorderType.RaisedInner:
			if (ᧈ)
			{
				ControlPaint.DrawBorder3D(graphics, m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Width, 2, (Border3DStyle)4, val);
				break;
			}
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)4, val);
			displayRectangle.Inflate(-2, -2);
			break;
		case eBorderType.Sunken:
			if (ᧈ)
			{
				ControlPaint.DrawBorder3D(graphics, m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Width, 2, (Border3DStyle)2, val);
				break;
			}
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)2, val);
			displayRectangle.Inflate(-2, -2);
			break;
		case eBorderType.SingleLine:
			if (ᧈ)
			{
				graphics.DrawLine(new Pen(_᧘(), 1f), m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Right, m_Rect.Y + m_Rect.Height / 2);
				break;
			}
			BarFunctions._२(graphics, eBorderType.SingleLine, m_Rect, _᧘(), ᖿ, (DashStyle)0, 1);
			displayRectangle.Inflate(-1, -1);
			break;
		case eBorderType.DoubleLine:
			BarFunctions._२(graphics, eBorderType.DoubleLine, m_Rect, _᧘());
			displayRectangle.Inflate(-2, -2);
			break;
		}
		if (ᧈ)
		{
			graphics.FillRectangle((Brush)new SolidBrush(ފ), rectangle);
		}
		eOrientation eOrientation2 = Orientation;
		if (__19CC == eOrientation.Vertical)
		{
			eOrientation2 = __19CC;
		}
		if (eOrientation2 == eOrientation.Horizontal)
		{
			displayRectangle.X += ޠ;
		}
		else
		{
			displayRectangle.Y += ޠ;
		}
		if (eOrientation2 == eOrientation.Horizontal)
		{
			displayRectangle.Width -= ޠ + ޡ;
		}
		else
		{
			displayRectangle.Width -= ޢ + ޣ;
		}
		displayRectangle.Y += ޢ;
		if (eOrientation2 == eOrientation.Horizontal)
		{
			displayRectangle.Height -= ޢ + ޣ;
		}
		else
		{
			displayRectangle.Height -= ޠ + ޡ;
		}
		Color color = _1933(pa);
		Color color2 = color;
		if (!SymbolColor.IsEmpty)
		{
			color2 = SymbolColor;
		}
		if (ᇎ2 != null || flag)
		{
			switch (_0E71)
			{
			case eImagePosition.Left:
				if (eOrientation2 == eOrientation.Horizontal)
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, SymbolRealized, Symbols.GetFont(SymbolSize, SymbolSet), color2, new Rectangle(displayRectangle.X + ImageTextSpacing, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, 0, 0), eTextFormat.Default);
					}
					else
					{
						ᇎ2.ᇗ(graphics, new Rectangle(displayRectangle.X + ImageTextSpacing, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, size.Width, size.Height));
					}
					displayRectangle.X += size.Width + ImageTextSpacing * 2;
					displayRectangle.Width -= size.Width + ImageTextSpacing * 2;
				}
				else
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, SymbolRealized, Symbols.GetFont(SymbolSize, SymbolSet), color2, new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Height) / 2, displayRectangle.Y + ImageTextSpacing, 0, 0), eTextFormat.Default);
					}
					else
					{
						ᇎ2.ᇗ(graphics, new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Height) / 2, displayRectangle.Y + ImageTextSpacing, size.Width, size.Height));
					}
					displayRectangle.Y += size.Height + ImageTextSpacing * 2;
					displayRectangle.Height -= size.Height + ImageTextSpacing * 2;
				}
				break;
			case eImagePosition.Right:
				if (eOrientation2 == eOrientation.Horizontal)
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, SymbolRealized, Symbols.GetFont(SymbolSize, SymbolSet), color2, new Rectangle(displayRectangle.Right - size.Width - ImageTextSpacing, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, 0, 0), eTextFormat.Default);
					}
					else
					{
						ᇎ2.ᇗ(graphics, new Rectangle(displayRectangle.Right - size.Width - ImageTextSpacing, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, size.Width, size.Height));
					}
					displayRectangle.Width -= size.Width + ImageTextSpacing * 2;
				}
				else
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, SymbolRealized, Symbols.GetFont(SymbolSize, SymbolSet), color2, new Rectangle(displayRectangle.X, displayRectangle.Bottom - size.Height - ImageTextSpacing, 0, 0), eTextFormat.Default);
					}
					else
					{
						ᇎ2.ᇗ(graphics, new Rectangle(displayRectangle.X, displayRectangle.Bottom - size.Height - ImageTextSpacing, size.Width, size.Height));
					}
					displayRectangle.Height -= size.Height + ImageTextSpacing * 2;
				}
				break;
			case eImagePosition.Top:
				if (eOrientation2 == eOrientation.Horizontal)
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, SymbolRealized, Symbols.GetFont(SymbolSize, SymbolSet), color2, new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Width) / 2, displayRectangle.Y, 0, 0), eTextFormat.Default);
					}
					else
					{
						ᇎ2.ᇗ(graphics, new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Width) / 2, displayRectangle.Y, size.Width, size.Height));
					}
					displayRectangle.Y += size.Height + ImageTextSpacing;
					displayRectangle.Height -= size.Height + ImageTextSpacing;
				}
				else
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, SymbolRealized, Symbols.GetFont(SymbolSize, SymbolSet), color2, new Rectangle(displayRectangle.X, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, 0, 0), eTextFormat.Default);
					}
					else
					{
						ᇎ2.ᇗ(graphics, new Rectangle(displayRectangle.X, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, size.Width, size.Height));
					}
					displayRectangle.X += size.Width + ImageTextSpacing;
					displayRectangle.Width -= size.Width + ImageTextSpacing;
				}
				break;
			case eImagePosition.Bottom:
				if (eOrientation2 == eOrientation.Horizontal)
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, SymbolRealized, Symbols.GetFont(SymbolSize, SymbolSet), color2, new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Width) / 2, displayRectangle.Bottom - size.Height, 0, 0), eTextFormat.Default);
					}
					else
					{
						ᇎ2.ᇗ(graphics, new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Width) / 2, displayRectangle.Bottom - size.Height, size.Width, size.Height));
					}
					displayRectangle.Height -= size.Height + ImageTextSpacing;
				}
				else
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, SymbolRealized, Symbols.GetFont(SymbolSize, SymbolSet), color2, new Rectangle(displayRectangle.Right - size.Width, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, 0, 0), eTextFormat.Default);
					}
					else
					{
						ᇎ2.ᇗ(graphics, new Rectangle(displayRectangle.Right - size.Width, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, size.Width, size.Height));
					}
					displayRectangle.Width -= size.Width + ImageTextSpacing;
				}
				break;
			}
		}
		if (displayRectangle.Height > 0 && displayRectangle.Width > 0)
		{
			if (eOrientation2 == eOrientation.Horizontal)
			{
				if ((int)ޔ == 2)
				{
					displayRectangle.Width--;
				}
				if (base.TextMarkupBody != null)
				{
					bool disposeFont2 = false;
					Font font3 = GetFont(out disposeFont2);
					MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font3, color, pa.RightToLeft);
					markupDrawContext.HotKeyPrefixVisible = (ಲ() & eTextFormat.NoPrefix) != eTextFormat.NoPrefix;
					if ((int)ޕ == 0)
					{
						base.TextMarkupBody.Bounds = new Rectangle(displayRectangle.Location, base.TextMarkupBody.Bounds.Size);
					}
					else if ((int)ޕ == 1)
					{
						base.TextMarkupBody.Bounds = new Rectangle(new Point(displayRectangle.X, displayRectangle.Y + (displayRectangle.Height - base.TextMarkupBody.Bounds.Height) / 2), base.TextMarkupBody.Bounds.Size);
					}
					else if ((int)ޕ == 2)
					{
						base.TextMarkupBody.Bounds = new Rectangle(new Point(displayRectangle.X, displayRectangle.Bottom - base.TextMarkupBody.Bounds.Height), base.TextMarkupBody.Bounds.Size);
					}
					Region clip = graphics.Clip;
					Rectangle rectangle2 = displayRectangle;
					rectangle2.Inflate(2, 2);
					graphics.SetClip(rectangle2, (CombineMode)0);
					base.TextMarkupBody.Render(markupDrawContext);
					if (disposeFont2)
					{
						font3.Dispose();
					}
					if (clip != null)
					{
						graphics.Clip = clip;
					}
					else
					{
						graphics.ResetClip();
					}
				}
				else
				{
					eTextFormat eTextFormat2 = ಲ();
					if (pa.RightToLeft)
					{
						eTextFormat2 |= eTextFormat.RightToLeft;
					}
					bool disposeFont3 = false;
					Font font4 = GetFont(out disposeFont3);
					if (pa.GlassEnabled && Parent is ᱭ && !(ContainerControl is Ꮮ))
					{
						if (!pa.ໟ)
						{
							_21F8._21FC(graphics, m_Text, font4, displayRectangle, TextDrawing.GetTextFormat(eTextFormat2), color, _21FE: true, _21FF: true, 7);
						}
					}
					else
					{
						if ((eTextFormat2 & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
						{
							displayRectangle.Inflate(0, 2);
						}
						TextDrawing.DrawString(graphics, m_Text, font4, color, displayRectangle, eTextFormat2);
					}
					if (disposeFont3)
					{
						font4.Dispose();
					}
				}
			}
			else
			{
				if (__19CD)
				{
					graphics.RotateTransform(90f);
				}
				else
				{
					graphics.RotateTransform(-90f);
				}
				bool disposeFont4 = false;
				Font font5 = GetFont(out disposeFont4);
				if (base.TextMarkupBody != null)
				{
					base.TextMarkupBody.Bounds = (__19CD ? new Rectangle(displayRectangle.Top, -displayRectangle.Right, base.TextMarkupBody.Bounds.Width + 2, base.TextMarkupBody.Bounds.Height) : new Rectangle(-displayRectangle.Bottom, displayRectangle.X, displayRectangle.Height + 1, displayRectangle.Width + 1));
					MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(graphics, font5, color, pa.RightToLeft);
					markupDrawContext2.HotKeyPrefixVisible = (ಲ() & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
					base.TextMarkupBody.Render(markupDrawContext2);
				}
				else
				{
					TextDrawing.DrawStringLegacy(graphics, m_Text, font5, color, __19CD ? new Rectangle(displayRectangle.Top, -displayRectangle.Right, displayRectangle.Height + 1, displayRectangle.Width + 1) : new Rectangle(-displayRectangle.Bottom, displayRectangle.X, displayRectangle.Height + 1, displayRectangle.Width + 1), ಲ());
				}
				if (disposeFont4)
				{
					font5.Dispose();
				}
				graphics.ResetTransform();
			}
		}
		DrawInsertMarker(graphics);
		if (DesignMode && Focused)
		{
			Rectangle rect = m_Rect;
			rect.Width--;
			rect.Height--;
			_1A6B._1A6C(graphics, rect, pa.Colors.ItemDesignTimeBorder);
		}
		__0E83 = null;
	}

	private Color _1933(ItemPaintArgs _0BD2)
	{
		Color _᧑ = _0D02;
		if (__0EB6 != eColorSchemePart.None)
		{
			_᧑ = (Color)_0BD2.Colors.GetType().GetProperty(__0EB6.ToString()).GetValue(_0BD2.Colors, null);
		}
		return _1933(_0BD2, EffectiveStyle, Enabled && GetEnabled(_0BD2.ContainerControl), _᧑);
	}

	internal static Color _1933(ItemPaintArgs _0BD2, eDotNetBarStyle _19CF, bool _᧐, Color _᧑)
	{
		Office2007ColorTable office2007ColorTable = null;
		if (_0BD2.ໝ is Office2007Renderer)
		{
			office2007ColorTable = ((Office2007Renderer)_0BD2.ໝ).ColorTable;
		}
		if (_᧑.IsEmpty && office2007ColorTable != null && _0BD2.ContainerControl != null && office2007ColorTable.LabelItemColors.Count > 0)
		{
			if (_0BD2.ContainerControl is Bar && ((Bar)(object)_0BD2.ContainerControl).BarType == eBarType.StatusBar && office2007ColorTable.LabelItemColors.TryGetValue(Office2007ColorTable.GetContextualKey(((object)_0BD2.ContainerControl).GetType(), "StatusBar"), out var value))
			{
				if (!_᧐)
				{
					return value.DisabledText;
				}
				return value.Text;
			}
			if (office2007ColorTable.LabelItemColors.TryGetValue(Office2007ColorTable.GetContextualKey(((object)_0BD2.ContainerControl).GetType()), out value))
			{
				if (!_᧐)
				{
					return value.DisabledText;
				}
				return value.Text;
			}
			if (office2007ColorTable.LabelItemColors.TryGetValue(string.Empty, out value))
			{
				if (!_᧐)
				{
					return value.DisabledText;
				}
				return value.Text;
			}
		}
		if (BarFunctions.IsOffice2007Style(_19CF) && _᧑.IsEmpty && office2007ColorTable != null)
		{
			if ((_0BD2.IsOnMenu || _0BD2.IsOnPopupBar) && office2007ColorTable.ButtonItemColors.Count > 0)
			{
				if (!_᧐)
				{
					return _0BD2.Colors.ItemDisabledText;
				}
				return office2007ColorTable.ButtonItemColors[0].Default.Text;
			}
			if (_0BD2.ContainerControl is RibbonStrip && office2007ColorTable.RibbonTabItemColors.Count > 0)
			{
				return office2007ColorTable.RibbonTabItemColors[0].Default.Text;
			}
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = office2007ColorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.Orange)];
			if (office2007ButtonItemColorTable != null && !office2007ButtonItemColorTable.Default.Text.IsEmpty)
			{
				if (!_᧐)
				{
					return _0BD2.Colors.ItemDisabledText;
				}
				return office2007ButtonItemColorTable.Default.Text;
			}
		}
		else if (!_᧐)
		{
			return _0BD2.Colors.ItemDisabledText;
		}
		if (_᧑.IsEmpty)
		{
			return _0BD2.Colors.ItemText;
		}
		return _᧑;
	}

	private Graphics _᧒()
	{
		object containerControl = ContainerControl;
		Control objCtrl = (Control)((containerControl is Control) ? containerControl : null);
		if (!BaseItem.IsHandleValid(objCtrl))
		{
			return null;
		}
		return BarFunctions.CreateGraphics(objCtrl);
	}

	public override void RecalcSize()
	{
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		if (SuspendLayout)
		{
			return;
		}
		if (base.TextMarkupBody != null)
		{
			_᧖();
		}
		else
		{
			Size size = Size.Empty;
			if (string.IsNullOrEmpty(SymbolRealized))
			{
				ᇎ ᇎ2 = _0EC5();
				if (ᇎ2 != null)
				{
					size = ᇎ2.ᇐ;
				}
			}
			eOrientation eOrientation2 = Orientation;
			if (__19CC == eOrientation.Vertical)
			{
				eOrientation2 = __19CC;
			}
			if (eOrientation2 == eOrientation.Horizontal || ܘ == 0)
			{
				if (ܘ == 0)
				{
					Graphics val = _᧒();
					if (val == null)
					{
						return;
					}
					try
					{
						if (!string.IsNullOrEmpty(SymbolRealized))
						{
							Font font = Symbols.GetFont(SymbolSize, SymbolSet);
							size = TextDrawing.MeasureString(val, "\uf00a", font);
							int num = (int)Math.Ceiling((float)font.FontFamily.GetCellDescent(font.Style) * font.Size / (float)font.FontFamily.GetEmHeight(font.Style));
							size.Height -= num;
						}
						Font val2 = null;
						bool disposeFont = false;
						val2 = ((ޓ == null) ? GetFont(out disposeFont) : ޓ);
						Size empty = Size.Empty;
						eTextFormat format = ಲ();
						if (m_Text != "")
						{
							string text = m_Text;
							if (text.EndsWith(" "))
							{
								text += ".";
							}
							int num2 = 0;
							if (ݥ && Stretch && WidthInternal > 16)
							{
								num2 = WidthInternal;
							}
							empty = ((eOrientation2 != eOrientation.Vertical || base.IsOnMenu) ? TextDrawing.MeasureString(val, text, val2, num2, format) : TextDrawing.MeasureStringLegacy(val, text, val2, new Size(num2, 0), format));
							empty.Width += Dpi.Width2;
							if (val2.Italic)
							{
								empty.Width += val2.Height / 3;
							}
						}
						else
						{
							empty = ((eOrientation2 != eOrientation.Vertical || base.IsOnMenu) ? TextDrawing.MeasureString(val, " ", val2, 0, format) : TextDrawing.MeasureStringLegacy(val, " ", val2, Size.Empty, format));
							empty.Width += 2;
						}
						if (Parent is ᱭ)
						{
							empty.Width += 2;
						}
						if (ᧆ != 0)
						{
							empty.Width += 4;
							empty.Height += 2;
							if (ᧆ != eBorderType.SingleLine)
							{
								empty.Height += 2;
							}
						}
						if (eOrientation2 == eOrientation.Horizontal)
						{
							if (Stretch && !(Parent is ItemContainer))
							{
								WidthInternal = 8;
							}
							else
							{
								WidthInternal = empty.Width + ޠ + ޡ;
							}
							HeightInternal = empty.Height + ޢ + ޣ;
							if (!size.IsEmpty)
							{
								if (_0E71 == eImagePosition.Left || _0E71 == eImagePosition.Right)
								{
									WidthInternal += size.Width + ImageTextSpacing * 2;
									HeightInternal = Math.Max(HeightInternal, size.Height + ImageTextSpacing);
								}
								else
								{
									HeightInternal += size.Height + ImageTextSpacing * 2;
								}
							}
						}
						else
						{
							if (Stretch && !(Parent is ItemContainer))
							{
								HeightInternal = 8;
							}
							else
							{
								HeightInternal = empty.Width + ޠ + ޡ;
							}
							WidthInternal = empty.Height + ޢ + ޣ;
							if (!size.IsEmpty)
							{
								if (_0E71 == eImagePosition.Left || _0E71 == eImagePosition.Right)
								{
									HeightInternal += size.Width + ImageTextSpacing * 2;
									WidthInternal = Math.Max(WidthInternal, size.Width + ImageTextSpacing);
								}
								else
								{
									WidthInternal += size.Height + ImageTextSpacing * 2;
									HeightInternal = Math.Max(size.Height + ImageTextSpacing, HeightInternal);
								}
							}
						}
						if (disposeFont)
						{
							val2.Dispose();
							val2 = null;
						}
					}
					finally
					{
						val.TextRenderingHint = (TextRenderingHint)0;
						val.SmoothingMode = (SmoothingMode)0;
						val.Dispose();
					}
				}
				else
				{
					WidthInternal = Dpi.Width(ܘ);
					if (ܗ == 0)
					{
						Font val3 = null;
						bool disposeFont2 = false;
						val3 = ((ޓ == null) ? GetFont(out disposeFont2) : ޓ);
						Graphics val4 = _᧒();
						if (val4 != null && !string.IsNullOrEmpty(SymbolRealized))
						{
							Font font2 = Symbols.GetFont(SymbolSize, SymbolSet);
							size = TextDrawing.MeasureString(val4, "\uf00a", font2);
							int num3 = (int)Math.Ceiling((float)font2.FontFamily.GetCellDescent(font2.Style) * font2.Size / (float)font2.FontFamily.GetEmHeight(font2.Style));
							size.Height -= num3;
						}
						if (WordWrap)
						{
							if (val4 != null)
							{
								int num4 = WidthInternal - (ޠ + ޡ);
								if (!size.IsEmpty && eOrientation2 == eOrientation.Horizontal && (_0E71 == eImagePosition.Left || _0E71 == eImagePosition.Right))
								{
									num4 -= size.Width + 4;
								}
								HeightInternal = TextDrawing.MeasureString(val4, Text, val3, num4).Height + 2;
							}
							else
							{
								HeightInternal = (int)Math.Ceiling(val3.GetHeight());
							}
						}
						else
						{
							HeightInternal = (int)Math.Ceiling(val3.GetHeight());
						}
						if (val4 != null)
						{
							val4.Dispose();
							val4 = null;
						}
						HeightInternal += ޢ + ޣ + (IsRightToLeft ? 2 : 0);
						if (ᧆ != 0)
						{
							HeightInternal += 2;
							if (ᧆ != eBorderType.SingleLine)
							{
								HeightInternal += 2;
							}
						}
						if (!size.IsEmpty)
						{
							if (eOrientation2 == eOrientation.Horizontal)
							{
								if (_0E71 == eImagePosition.Left || _0E71 == eImagePosition.Right)
								{
									HeightInternal = Math.Max(HeightInternal, size.Height);
								}
								else
								{
									HeightInternal += size.Height + 4;
								}
							}
							else if (_0E71 == eImagePosition.Left || _0E71 == eImagePosition.Right)
							{
								WidthInternal = Math.Max(HeightInternal, size.Height + 2);
							}
							else
							{
								WidthInternal += size.Height + 4;
							}
						}
						if (disposeFont2)
						{
							val3.Dispose();
							val3 = null;
						}
					}
					else
					{
						HeightInternal = Dpi.Height(ܗ);
					}
				}
			}
			else
			{
				WidthInternal = Dpi.Width(ܘ);
				HeightInternal = Dpi.Height(ܗ);
			}
		}
		base.RecalcSize();
	}

	protected override void OnExternalSizeChange()
	{
		if (base.TextMarkupBody != null)
		{
			if (__19CC == eOrientation.Horizontal)
			{
				_᧓(WidthInternal);
			}
			else
			{
				_᧓(HeightInternal + 1);
			}
		}
		base.OnExternalSizeChange();
	}

	private void _᧓(int _᧔)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Invalid comparison between Unknown and I4
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (!BaseItem.IsHandleValid(val))
		{
			return;
		}
		Graphics val2 = BarFunctions.CreateGraphics(val);
		try
		{
			Font val3 = null;
			bool disposeFont = false;
			val3 = ((ޓ == null) ? GetFont(out disposeFont) : ޓ);
			_0EC5();
			Size size = _᧕(val2, _᧔);
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(val2, val3, ForeColor, rightToLeft: false);
			markupDrawContext.HotKeyPrefixVisible = (ಲ() & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
			base.TextMarkupBody.Measure(size, markupDrawContext);
			markupDrawContext.RightToLeft = (int)val.RightToLeft == 1;
			if (_᧔ == 0 && ܘ == 0)
			{
				size = base.TextMarkupBody.Bounds.Size;
			}
			base.TextMarkupBody._0CC6(new Rectangle(Point.Empty, size), markupDrawContext);
			if (disposeFont)
			{
				val3.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private Size _᧕(Graphics _٥, int _᧔)
	{
		Size result = new Size((_᧔ == 0) ? Dpi.Width(ܘ) : _᧔, 1);
		if (__19CC == eOrientation.Vertical)
		{
			result = new Size((_᧔ == 0) ? Dpi.Height(ܗ) : _᧔, 1);
		}
		Size size = Size.Empty;
		if (!string.IsNullOrEmpty(_ລ))
		{
			size = _᧗(_٥);
		}
		else
		{
			ᇎ ᇎ2 = _0EC5();
			if (ᇎ2 != null)
			{
				size = ᇎ2.ᇐ;
			}
		}
		if (result.Width == 0)
		{
			result.Width = 1600;
		}
		else
		{
			result.Width -= ޠ + ޡ;
			if (!size.IsEmpty && (_0E71 == eImagePosition.Left || _0E71 == eImagePosition.Right))
			{
				result.Width -= size.Width + ImageTextSpacing * 2;
			}
			if (ᧆ != 0)
			{
				result.Width -= 4;
			}
		}
		return result;
	}

	private void _᧖()
	{
		_᧖(0);
	}

	private Size _᧗(Graphics _٥)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Size empty = Size.Empty;
		if (_٥ == null || string.IsNullOrEmpty(_ລ))
		{
			return empty;
		}
		Font font = Symbols.GetFont(SymbolSize, SymbolSet);
		empty = TextDrawing.MeasureString(_٥, "\uf00a", font);
		int num = (int)Math.Ceiling((float)font.FontFamily.GetCellDescent(font.Style) * font.Size / (float)font.FontFamily.GetEmHeight(font.Style));
		empty.Height -= num;
		return empty;
	}

	internal void _᧖(int ᖩ)
	{
		eOrientation eOrientation2 = Orientation;
		if (__19CC == eOrientation.Vertical)
		{
			eOrientation2 = __19CC;
		}
		_᧓(ᖩ);
		int num = base.TextMarkupBody.Bounds.Width;
		int num2 = base.TextMarkupBody.Bounds.Height;
		if (ᧆ != 0)
		{
			num += 4;
			num2 += 4;
		}
		Size size = Size.Empty;
		if (!string.IsNullOrEmpty(SymbolRealized))
		{
			Graphics val = _᧒();
			try
			{
				size = _᧗(val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			ᇎ ᇎ2 = _0EC5();
			if (ᇎ2 != null)
			{
				size = ᇎ2.ᇐ;
			}
		}
		if (!size.IsEmpty && ܘ == 0)
		{
			if (_0E71 == eImagePosition.Left || _0E71 == eImagePosition.Right)
			{
				num += size.Width + ImageTextSpacing * 2;
				num2 = Math.Max(num2, size.Height + ImageTextSpacing);
			}
			else
			{
				num2 += size.Height + ImageTextSpacing * 2;
				num = Math.Max(num, size.Width + ImageTextSpacing);
			}
		}
		if (ܘ == 0)
		{
			num += ޠ + ޡ;
			num2 += ޢ + ޣ;
			if (Stretch && !(Parent is ItemContainer))
			{
				num = 8;
			}
		}
		if (eOrientation2 == eOrientation.Vertical)
		{
			num += 2;
			int num3 = num;
			num = num2;
			num2 = num3;
		}
		if (ܘ == 0)
		{
			m_Rect.Width = num;
		}
		else
		{
			m_Rect.Width = Dpi.Width(ܘ);
		}
		if (ܗ == 0)
		{
			m_Rect.Height = num2;
		}
		else
		{
			m_Rect.Height = Dpi.Height(ܗ);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return !ފ.IsEmpty;
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSingleLineColor()
	{
		return _0B80 != SystemColors.ControlDark;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSingleLineColor()
	{
		SingleLineColor = SystemColors.ControlDark;
	}

	private Color _᧘()
	{
		if (ᧇ)
		{
			return _0B80;
		}
		Color result = _0B80;
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
		{
			if (BarFunctions.IsOffice2007Style(effectiveStyle))
			{
				if (ContainerControl is IRenderingSupport)
				{
					Office2007Renderer office2007Renderer = ((IRenderingSupport)ContainerControl).GetRenderer() as Office2007Renderer;
					if (office2007Renderer == null)
					{
						office2007Renderer = GlobalManager.Renderer as Office2007Renderer;
					}
					if (office2007Renderer != null)
					{
						result = office2007Renderer.ColorTable.Bar.ToolbarBottomBorder;
					}
				}
			}
			else
			{
				ColorScheme colorScheme = null;
				colorScheme = ((ContainerControl is Bar) ? ((Bar)ContainerControl).ColorScheme : ((!(ContainerControl is MenuPanel)) ? new ColorScheme(EffectiveStyle) : ((MenuPanel)ContainerControl).ColorScheme));
				if (colorScheme != null)
				{
					_0B80 = colorScheme.BarDockedBorder;
				}
				else
				{
					_0B80 = SystemColors.ControlDark;
				}
			}
		}
		return result;
	}

	protected virtual void OnFontBoldChanged(bool newValue, bool oldValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	protected virtual Font GetFont(out bool disposeFont)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		disposeFont = false;
		if (ޓ != null)
		{
			return ޓ;
		}
		Font val = null;
		object containerControl = ContainerControl;
		Control val2 = (Control)((containerControl is Control) ? containerControl : null);
		val = ((val2 == null) ? SystemFonts.DefaultFont : val2.Font);
		if (val != null && __19CB)
		{
			val = new Font(val, (FontStyle)1);
			disposeFont = true;
		}
		return val;
	}

	private eTextFormat ಲ()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		eTextFormat eTextFormat2 = eTextFormat.EndEllipsis | eTextFormat.ExpandTabs;
		if (!ᧉ)
		{
			eTextFormat2 |= eTextFormat.NoPrefix;
		}
		eTextFormat2 = (ݥ ? (eTextFormat2 | eTextFormat.WordBreak) : (eTextFormat2 | eTextFormat.SingleLine));
		if ((int)ޔ == 1)
		{
			eTextFormat2 |= eTextFormat.HorizontalCenter;
		}
		else if ((int)ޔ == 2)
		{
			eTextFormat2 |= eTextFormat.Right;
		}
		if ((int)ޕ == 1)
		{
			eTextFormat2 |= eTextFormat.VerticalCenter;
		}
		else if ((int)ޕ == 2)
		{
			eTextFormat2 |= eTextFormat.Bottom;
		}
		if (IsRightToLeft)
		{
			eTextFormat2 |= eTextFormat.RightToLeft;
		}
		return eTextFormat2;
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected I4, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected I4, but got Unknown
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Expected I4, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("bt", XmlConvert.ToString((int)ᧆ));
		if (ᖿ != eBorderSide.All)
		{
			itemXmlElement.SetAttribute("BorderSide", XmlConvert.ToString((int)ᖿ));
		}
		if (!ފ.IsEmpty)
		{
			itemXmlElement.SetAttribute("bc", BarFunctions.ColorToString(ފ));
		}
		itemXmlElement.SetAttribute("fc", BarFunctions.ColorToString(_0D02));
		if (ᧇ)
		{
			itemXmlElement.SetAttribute("sc", BarFunctions.ColorToString(_0B80));
		}
		itemXmlElement.SetAttribute("ta", XmlConvert.ToString((int)ޔ));
		itemXmlElement.SetAttribute("tla", XmlConvert.ToString((int)ޕ));
		itemXmlElement.SetAttribute("ds", XmlConvert.ToString(ᧈ));
		itemXmlElement.SetAttribute("pl", XmlConvert.ToString(ޠ));
		itemXmlElement.SetAttribute("pr", XmlConvert.ToString(ޡ));
		itemXmlElement.SetAttribute("pt", XmlConvert.ToString(ޢ));
		itemXmlElement.SetAttribute("pb", XmlConvert.ToString(ޣ));
		itemXmlElement.SetAttribute("w", XmlConvert.ToString(ܘ));
		itemXmlElement.SetAttribute("h", XmlConvert.ToString(ܗ));
		if (Font != null && (Font.Name != SystemInformation.MenuFont.Name || Font.Size != SystemInformation.MenuFont.Size || Font.Style != SystemInformation.MenuFont.Style))
		{
			itemXmlElement.SetAttribute("fontname", Font.Name);
			itemXmlElement.SetAttribute("fontemsize", XmlConvert.ToString(Font.Size));
			itemXmlElement.SetAttribute("fontstyle", XmlConvert.ToString((int)Font.Style));
		}
		XmlElement xmlElement = null;
		XmlElement xmlElement2 = null;
		if (_0E66 != null || դ >= 0 || ຈ != null)
		{
			xmlElement = itemXmlElement.OwnerDocument.CreateElement("images");
			itemXmlElement.AppendChild(xmlElement);
			if (դ >= 0)
			{
				xmlElement.SetAttribute("imageindex", XmlConvert.ToString(դ));
			}
			if (_0E66 != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "default");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(_0E66, xmlElement2);
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
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		ᧆ = (eBorderType)XmlConvert.ToInt32(itemXmlElement.GetAttribute("bt"));
		if (itemXmlElement.HasAttribute("BorderSide"))
		{
			ᖿ = (eBorderSide)XmlConvert.ToInt32(itemXmlElement.GetAttribute("BorderSide"));
		}
		else
		{
			ᖿ = eBorderSide.All;
		}
		if (itemXmlElement.HasAttribute("bc"))
		{
			ފ = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("bc"));
		}
		else
		{
			ފ = Color.Empty;
		}
		_0D02 = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("fc"));
		if (itemXmlElement.HasAttribute("sc"))
		{
			_0B80 = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("sc"));
			if (_0B80 != SystemColors.ControlDark)
			{
				ᧇ = true;
			}
		}
		else
		{
			ᧇ = false;
			_0B80 = SystemColors.ControlDark;
		}
		ޔ = (StringAlignment)XmlConvert.ToInt32(itemXmlElement.GetAttribute("ta"));
		ޕ = (StringAlignment)XmlConvert.ToInt32(itemXmlElement.GetAttribute("tla"));
		ᧈ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("ds"));
		ޠ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("pl"));
		ޡ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("pr"));
		ޢ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("pt"));
		ޣ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("pb"));
		ܘ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("w"));
		ܗ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("h"));
		if (itemXmlElement.HasAttribute("fontname"))
		{
			string attribute = itemXmlElement.GetAttribute("fontname");
			float num = XmlConvert.ToSingle(itemXmlElement.GetAttribute("fontemsize"));
			FontStyle val = (FontStyle)XmlConvert.ToInt32(itemXmlElement.GetAttribute("fontstyle"));
			try
			{
				Font = new Font(attribute, num, val);
			}
			catch (Exception)
			{
				Font = SystemFonts.DefaultFont;
			}
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
			{
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
					}
				}
				break;
			}
		}
	}

	private void ᡁ()
	{
		if (_0E6A != null)
		{
			_0E6A.Dispose();
			_0E6A = null;
		}
		if (_0E6D != null)
		{
			_0E6D.Dispose();
			_0E6D = null;
		}
		NeedRecalcSize = true;
		Refresh();
	}

	private ᇎ _0EC5()
	{
		if (GetEnabled())
		{
			return _0EC5(_1937.ሬ);
		}
		return _0EC5(_1937.ड़);
	}

	private ᇎ _0EC5(_1937 య)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		Image val = null;
		if (య == _1937.ड़)
		{
			if (_0E6A == null && _0E6D == null && (ຈ != null || _0E66 != null || դ >= 0))
			{
				_໖();
			}
			if (_0E6D != null)
			{
				return new ᇎ(new Icon(_0E6D, _0E5A), dispose: true);
			}
			if (_0E6A != null)
			{
				return new ᇎ(_0E6A, dispose: false);
			}
		}
		if (ຈ != null)
		{
			Size size = _0E5A;
			return new ᇎ(new Icon(ຈ, size), dispose: true);
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

	private void _໖()
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Invalid comparison between Unknown and I4
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Invalid comparison between Unknown and I4
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Invalid comparison between Unknown and I4
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		if (_0E66 == null && դ < 0 && ຈ == null)
		{
			return;
		}
		if (_0E6A != null)
		{
			_0E6A.Dispose();
		}
		_0E6A = null;
		if (_0E6D != null)
		{
			_0E6D.Dispose();
		}
		_0E6D = null;
		ᇎ ᇎ2 = _0EC5(_1937.ሬ);
		if (ᇎ2 == null)
		{
			return;
		}
		if (GetOwner() is IOwner && ((IOwner)GetOwner()).DisabledImagesGrayScale)
		{
			if (ᇎ2.ᇏ)
			{
				_0E6D = BarFunctions.ॽ(ᇎ2.ᆻ);
			}
			else
			{
				ref Image reference = ref _0E6A;
				Image ᇑ = ᇎ2.ᇑ;
				reference = (Image)(object)ᶣ.ᶧ((ᇑ is Bitmap) ? ᇑ : null);
			}
		}
		if (_0E6D == null && _0E6A == null)
		{
			PixelFormat val = (PixelFormat)2498570;
			if (!ᇎ2.ᇏ && ᇎ2.ᇑ != null)
			{
				val = ᇎ2.ᇑ.PixelFormat;
			}
			if ((int)val == 196865 || (int)val == 197634 || (int)val == 198659)
			{
				val = (PixelFormat)2498570;
			}
			Bitmap val2 = new Bitmap(ᇎ2.ܐ, ᇎ2._0711, val);
			_0E6A = (Image)new Bitmap(ᇎ2.ܐ, ᇎ2._0711, val);
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			SolidBrush val4 = new SolidBrush(Color.White);
			try
			{
				val3.FillRectangle((Brush)(object)val4, 0, 0, ᇎ2.ܐ, ᇎ2._0711);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			ᇎ2.ᇗ(val3, new Rectangle(0, 0, ᇎ2.ܐ, ᇎ2._0711));
			val3.Dispose();
			val3 = Graphics.FromImage(_0E6A);
			val2.MakeTransparent(Color.White);
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if ((effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)) && _089F.न >= 8)
			{
				float[][] array = new float[5][];
				float[] array2 = new float[5];
				array[0] = array2;
				float[] array3 = new float[5];
				array[1] = array3;
				float[] array4 = new float[5];
				array[2] = array4;
				array[3] = new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0f };
				float[] array5 = new float[5];
				array[4] = array5;
				ColorMatrix colorMatrix = new ColorMatrix(array);
				ImageAttributes val5 = new ImageAttributes();
				val5.ClearColorKey();
				val5.SetColorMatrix(colorMatrix);
				val3.DrawImage((Image)(object)val2, new Rectangle(0, 0, ((Image)val2).Width, ((Image)val2).Height), 0, 0, ((Image)val2).Width, ((Image)val2).Height, (GraphicsUnit)2, val5);
			}
			else
			{
				ControlPaint.DrawImageDisabled(val3, (Image)(object)val2, 0, 0, ColorFunctions.MenuBackColor(val3));
			}
			val3.Dispose();
			val3 = null;
			((Image)val2).Dispose();
			val2 = null;
			ᇎ2.Dispose();
		}
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

	protected override void OnCommandChanged()
	{
		if (!DesignMode && Command == null)
		{
			Text = "";
		}
		base.OnCommandChanged();
	}

	protected virtual void OnSymbolChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			__0EA6 = "";
		}
		else
		{
			__0EA6 = Symbols.GetSymbol(newValue);
		}
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnSymbolSizeChanged(float oldValue, float newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSymbolColor()
	{
		return !__0EA4.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSymbolColor()
	{
		SymbolColor = Color.Empty;
	}

	protected override void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is _0CFE obj)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
		base.TextMarkupLinkClick(sender, e);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}
}
