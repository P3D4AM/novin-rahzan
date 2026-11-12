using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[DefaultEvent("Click")]
public class LabelItem : BaseItem, IBlockExtended, IBlock
{
	internal const eBorderSide DEFAULT_BORDERSIDE = eBorderSide.All;

	private eBorderType m_BorderType;

	private Color m_BackColor = Color.Empty;

	private Color m_ForeColor = Color.Empty;

	private Color m_SingleLineColor = SystemColors.ControlDark;

	private bool m_SingleLineColorCustom;

	private StringAlignment m_TextAlignment;

	private StringAlignment m_TextLineAlignment = (StringAlignment)1;

	private Font m_Font;

	private bool m_DividerStyle;

	private int m_PaddingLeft;

	private int m_PaddingRight;

	private int m_PaddingTop;

	private int m_PaddingBottom;

	private int m_Width;

	private int m_Height;

	private bool m_WordWrap;

	private eImagePosition m_ImagePosition;

	private Image m_Image;

	private Image m_ImageCachedIdx;

	private Image m_DisabledImage;

	private Icon m_DisabledIcon;

	private Icon m_Icon;

	private int m_ImageIndex = -1;

	private ItemPaintArgs _ItemPaintArgs;

	private const int IMAGETEXT_SPACING = 2;

	private eBorderSide m_BorderSide = eBorderSide.All;

	private bool m_ShowPrefix;

	internal bool SuspendPaint;

	private bool _EnableMarkup = true;

	private eColorSchemePart _ForeColorColorSchemePart = eColorSchemePart.None;

	private eOrientation _TextOrientation;

	private bool _VerticalTextTopUp = true;

	private string _Symbol = "";

	private float _SymbolSize;

	private Color _SymbolColor = Color.Empty;

	private bool _ContainerNewLineAfter;

	protected override bool IsMarkupSupported => _EnableMarkup;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return _EnableMarkup;
		}
		set
		{
			if (_EnableMarkup != value)
			{
				_EnableMarkup = value;
				NeedRecalcSize = true;
				OnTextChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eBorderType.None)]
	[Category("Appearance")]
	[Description("Indicates the type of the border drawn around the label.")]
	public eBorderType BorderType
	{
		get
		{
			return m_BorderType;
		}
		set
		{
			if (m_BorderType != value)
			{
				m_BorderType = value;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(eBorderSide.All)]
	[Description("Specifies border sides that are displayed.")]
	public eBorderSide BorderSide
	{
		get
		{
			return m_BorderSide;
		}
		set
		{
			if (m_BorderSide != value)
			{
				m_BorderSide = value;
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Determines the background color of the label.")]
	public Color BackColor
	{
		get
		{
			return m_BackColor;
		}
		set
		{
			if (!(m_BackColor == value))
			{
				m_BackColor = value;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	internal eColorSchemePart ForeColorColorSchemePart
	{
		get
		{
			return _ForeColorColorSchemePart;
		}
		set
		{
			_ForeColorColorSchemePart = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates text color.")]
	public Color ForeColor
	{
		get
		{
			return m_ForeColor;
		}
		set
		{
			if (!(m_ForeColor == value))
			{
				m_ForeColor = value;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates border line color when border is single line.")]
	public Color SingleLineColor
	{
		get
		{
			return m_SingleLineColor;
		}
		set
		{
			if (!(m_SingleLineColor == value))
			{
				m_SingleLineColor = value;
				if (m_SingleLineColor != SystemColors.ControlDark)
				{
					m_SingleLineColorCustom = true;
				}
				else
				{
					m_SingleLineColorCustom = false;
				}
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates text alignment.")]
	public StringAlignment TextAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_TextAlignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (m_TextAlignment != value)
			{
				m_TextAlignment = value;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates text line alignment.")]
	public StringAlignment TextLineAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_TextLineAlignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (m_TextLineAlignment != value)
			{
				m_TextLineAlignment = value;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates label font.")]
	public Font Font
	{
		get
		{
			return m_Font;
		}
		set
		{
			if (m_Font != value)
			{
				m_Font = value;
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates left padding in pixels.")]
	public int PaddingLeft
	{
		get
		{
			return m_PaddingLeft;
		}
		set
		{
			if (m_PaddingLeft != value)
			{
				m_PaddingLeft = value;
				NeedRecalcSize = true;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates right padding in pixels.")]
	public int PaddingRight
	{
		get
		{
			return m_PaddingRight;
		}
		set
		{
			if (m_PaddingRight != value)
			{
				m_PaddingRight = value;
				NeedRecalcSize = true;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates top padding in pixels.")]
	public int PaddingTop
	{
		get
		{
			return m_PaddingTop;
		}
		set
		{
			if (m_PaddingTop != value)
			{
				m_PaddingTop = value;
				NeedRecalcSize = true;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates bottom padding in pixels.")]
	public int PaddingBottom
	{
		get
		{
			return m_PaddingBottom;
		}
		set
		{
			if (m_PaddingBottom != value)
			{
				m_PaddingBottom = value;
				NeedRecalcSize = true;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates the width of the label in pixels.")]
	public int Width
	{
		get
		{
			return m_Width;
		}
		set
		{
			if (m_Width != value)
			{
				m_Width = value;
				NeedRecalcSize = true;
				if (!SuspendPaint)
				{
					Refresh();
					OnAppearanceChanged();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates height of the label in pixels.")]
	public int Height
	{
		get
		{
			return m_Height;
		}
		set
		{
			if (m_Height != value)
			{
				m_Height = value;
				NeedRecalcSize = true;
				if (!SuspendPaint)
				{
					Refresh();
					OnAppearanceChanged();
				}
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether label has divider style.")]
	public bool DividerStyle
	{
		get
		{
			return m_DividerStyle;
		}
		set
		{
			if (m_DividerStyle != value)
			{
				m_DividerStyle = value;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[DefaultValue(false)]
	[Description("Gets or sets a value that determines whether text is displayed in multiple lines or one long line.")]
	public bool WordWrap
	{
		get
		{
			return m_WordWrap;
		}
		set
		{
			if (m_WordWrap != value)
			{
				m_WordWrap = value;
				m_NeedRecalcSize = true;
			}
			OnAppearanceChanged();
		}
	}

	internal bool ShowPrefix
	{
		get
		{
			return m_ShowPrefix;
		}
		set
		{
			m_ShowPrefix = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("The text contained in the item.")]
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
			m_AccessKey = NativeFunctions.GetAccessKey(m_Text);
			if (ShouldSyncProperties)
			{
				BarFunctions.SyncProperty(this, "Text");
			}
			if (Displayed && m_Parent != null && !SuspendLayout)
			{
				if (m_Width == 0)
				{
					NeedRecalcSize = true;
				}
				if (!(ContainerControl is LabelX))
				{
					Refresh();
				}
			}
			OnTextChanged();
			OnAppearanceChanged();
			if (ContainerControl is BaseItemControl)
			{
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the label icon.")]
	[DefaultValue(null)]
	public Icon Icon
	{
		get
		{
			return m_Icon;
		}
		set
		{
			m_Icon = value;
			OnImageChanged();
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
			m_Image = value;
			OnImageChanged();
			OnAppearanceChanged();
		}
	}

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
				OnImageChanged();
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The alignment of the image in relation to text displayed by this item.")]
	[DefaultValue(eImagePosition.Left)]
	public eImagePosition ImagePosition
	{
		get
		{
			return m_ImagePosition;
		}
		set
		{
			if (m_ImagePosition != value)
			{
				m_ImagePosition = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "ImagePosition");
				}
				NeedRecalcSize = true;
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

	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	[Description("Indicates text-orientation")]
	public eOrientation TextOrientation
	{
		get
		{
			return _TextOrientation;
		}
		set
		{
			_TextOrientation = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates how vertical text is rotated when TextOrientation = Vertical.")]
	public bool VerticalTextTopUp
	{
		get
		{
			return _VerticalTextTopUp;
		}
		set
		{
			_VerticalTextTopUp = value;
			if (_TextOrientation == eOrientation.Vertical)
			{
				Refresh();
			}
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the symbol displayed on face of the tile instead of the image. Setting the symbol overrides the image setting.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.SymbolTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string Symbol
	{
		get
		{
			return _Symbol;
		}
		set
		{
			if (value != _Symbol)
			{
				string symbol = _Symbol;
				_Symbol = value;
				OnSymbolChanged(symbol, value);
			}
		}
	}

	[DefaultValue(0f)]
	[Category("Appearance")]
	[Description("Indicates the size of the symbol in points.")]
	public float SymbolSize
	{
		get
		{
			return _SymbolSize;
		}
		set
		{
			if (value != _SymbolSize)
			{
				float symbolSize = _SymbolSize;
				_SymbolSize = value;
				OnSymbolSizeChanged(symbolSize, value);
			}
		}
	}

	[Category("Columns")]
	[Description("Indicates color of the symbol.")]
	public Color SymbolColor
	{
		get
		{
			return _SymbolColor;
		}
		set
		{
			_SymbolColor = value;
			Refresh();
		}
	}

	[DefaultValue(false)]
	[Category("Layout")]
	[Description("Indicates whether label hosted in multi-line ItemContainer triggers new line switch after label is processed.")]
	public bool ContainerNewLineAfter
	{
		get
		{
			return _ContainerNewLineAfter;
		}
		set
		{
			_ContainerNewLineAfter = value;
		}
	}

	bool IBlockExtended.IsBlockElement => false;

	bool IBlockExtended.IsNewLineAfterElement => _ContainerNewLineAfter;

	bool IBlockExtended.CanStartNewLine => true;

	bool IBlockExtended.IsBlockContainer => false;

	[Description("Occurs when text markup link is clicked. Markup links can be created using a tag.")]
	public event MarkupLinkClickEventHandler MarkupLinkClick;

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
			BarUtilities.DisposeImage(ref m_Image);
			BarUtilities.DisposeImage(ref m_ImageCachedIdx);
			BarUtilities.DisposeImage(ref m_Icon);
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		LabelItem labelItem = new LabelItem(m_Name);
		CopyToItem(labelItem);
		return labelItem;
	}

	internal void InternalCopyToItem(LabelItem copy)
	{
		CopyToItem(copy);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		LabelItem labelItem = copy as LabelItem;
		base.CopyToItem(labelItem);
		labelItem.BorderType = m_BorderType;
		labelItem.BorderSide = m_BorderSide;
		labelItem.BackColor = m_BackColor;
		labelItem.ForeColor = m_ForeColor;
		if (m_SingleLineColorCustom)
		{
			labelItem.SingleLineColor = m_SingleLineColor;
		}
		labelItem.WidthInternal = WidthInternal;
		labelItem.HeightInternal = HeightInternal;
		labelItem.TextAlignment = TextAlignment;
		labelItem.TextLineAlignment = TextLineAlignment;
		labelItem.Font = m_Font;
		labelItem.PaddingBottom = m_PaddingBottom;
		labelItem.PaddingLeft = m_PaddingLeft;
		labelItem.PaddingRight = m_PaddingRight;
		labelItem.PaddingTop = m_PaddingTop;
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
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Invalid comparison between Unknown and I4
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Invalid comparison between Unknown and I4
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Invalid comparison between Unknown and I4
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Invalid comparison between Unknown and I4
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Invalid comparison between Unknown and I4
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Invalid comparison between Unknown and I4
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Invalid comparison between Unknown and I4
		//IL_0bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdf: Invalid comparison between Unknown and I4
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c71: Invalid comparison between Unknown and I4
		//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd2: Invalid comparison between Unknown and I4
		if (SuspendLayout)
		{
			return;
		}
		_ItemPaintArgs = pa;
		Graphics graphics = pa.Graphics;
		CompositeImage compositeImage = null;
		Size size = Size.Empty;
		bool flag = false;
		if (string.IsNullOrEmpty(Symbol))
		{
			compositeImage = GetImage();
			if (compositeImage != null)
			{
				size = compositeImage.Size;
			}
		}
		else
		{
			flag = true;
			Font fontAwesome = Symbols.GetFontAwesome(SymbolSize);
			size = TextDrawing.MeasureString(graphics, Symbol, fontAwesome);
			int num = (int)Math.Ceiling((float)fontAwesome.FontFamily.GetCellDescent(fontAwesome.Style) * fontAwesome.Size / (float)fontAwesome.FontFamily.GetEmHeight(fontAwesome.Style));
			size.Height -= num;
		}
		Rectangle displayRectangle = DisplayRectangle;
		Rectangle rectangle = Rectangle.Empty;
		Border3DSide val = (m_DividerStyle ? ((Border3DSide)2) : ((m_BorderSide != eBorderSide.All) ? ((Border3DSide)((((m_BorderSide & eBorderSide.Left) != 0) ? 1 : 0) | (((m_BorderSide & eBorderSide.Right) != 0) ? 4 : 0) | (((m_BorderSide & eBorderSide.Top) != 0) ? 2 : 0) | (((m_BorderSide & eBorderSide.Bottom) != 0) ? 8 : 0))) : ((Border3DSide)15)));
		if (!(pa.ContainerControl is LabelX))
		{
			DisplayHelp.FillRectangle(graphics, DisplayRectangle, m_BackColor, Color.Empty);
		}
		if (m_DividerStyle)
		{
			object containerControl = ContainerControl;
			Control val2 = (Control)((containerControl is Control) ? containerControl : null);
			RightToLeft val3 = (RightToLeft)0;
			if (val2 != null && (int)val2.RightToLeft == 1)
			{
				val3 = (RightToLeft)1;
			}
			Size size2 = TextDrawing.MeasureString(graphics, m_Text, GetFont(), m_Rect.Width, GetStringFormat());
			size2.Width += 4;
			if (size2.Width > displayRectangle.Width)
			{
				size2.Width = displayRectangle.Width;
			}
			rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y, size2.Width, size2.Height);
			if ((int)m_TextAlignment == 1)
			{
				rectangle.Offset((displayRectangle.Width - size2.Width) / 2, 0);
			}
			else if (((int)m_TextAlignment == 2 && (int)val3 == 0) || ((int)m_TextAlignment == 0 && (int)val3 == 1))
			{
				rectangle.Offset(displayRectangle.Width - rectangle.Width, 0);
			}
			if ((int)m_TextLineAlignment == 1)
			{
				rectangle.Offset(0, (displayRectangle.Height - rectangle.Height) / 2);
			}
			else if (((int)m_TextLineAlignment == 2 && (int)val3 == 0) || ((int)m_TextLineAlignment == 0 && (int)val3 == 1))
			{
				rectangle.Offset(0, displayRectangle.Height - rectangle.Height);
			}
		}
		switch (m_BorderType)
		{
		case eBorderType.Bump:
			if (m_DividerStyle)
			{
				ControlPaint.DrawBorder3D(graphics, m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Width, 2, (Border3DStyle)9, val);
				break;
			}
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)9, val);
			displayRectangle.Inflate(-2, -2);
			break;
		case eBorderType.Etched:
			if (m_DividerStyle)
			{
				ControlPaint.DrawBorder3D(graphics, m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Width, 2, (Border3DStyle)6, val);
				break;
			}
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)6, val);
			displayRectangle.Inflate(-2, -2);
			break;
		case eBorderType.Raised:
		case eBorderType.RaisedInner:
			if (m_DividerStyle)
			{
				ControlPaint.DrawBorder3D(graphics, m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Width, 2, (Border3DStyle)4, val);
				break;
			}
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)4, val);
			displayRectangle.Inflate(-2, -2);
			break;
		case eBorderType.Sunken:
			if (m_DividerStyle)
			{
				ControlPaint.DrawBorder3D(graphics, m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Width, 2, (Border3DStyle)2, val);
				break;
			}
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)2, val);
			displayRectangle.Inflate(-2, -2);
			break;
		case eBorderType.SingleLine:
			if (m_DividerStyle)
			{
				graphics.DrawLine(new Pen(GetSingleLineColor(), 1f), m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Right, m_Rect.Y + m_Rect.Height / 2);
				break;
			}
			BarFunctions.DrawBorder(graphics, eBorderType.SingleLine, m_Rect, GetSingleLineColor(), m_BorderSide, (DashStyle)0, 1);
			displayRectangle.Inflate(-1, -1);
			break;
		case eBorderType.DoubleLine:
			BarFunctions.DrawBorder(graphics, eBorderType.DoubleLine, m_Rect, GetSingleLineColor());
			displayRectangle.Inflate(-2, -2);
			break;
		}
		if (m_DividerStyle)
		{
			graphics.FillRectangle((Brush)new SolidBrush(m_BackColor), rectangle);
		}
		eOrientation eOrientation2 = Orientation;
		if (_TextOrientation == eOrientation.Vertical)
		{
			eOrientation2 = _TextOrientation;
		}
		if (eOrientation2 == eOrientation.Horizontal)
		{
			displayRectangle.X += m_PaddingLeft;
		}
		else
		{
			displayRectangle.Y += m_PaddingLeft;
		}
		if (eOrientation2 == eOrientation.Horizontal)
		{
			displayRectangle.Width -= m_PaddingLeft + m_PaddingRight;
		}
		else
		{
			displayRectangle.Width -= m_PaddingTop + m_PaddingBottom;
		}
		displayRectangle.Y += m_PaddingTop;
		if (eOrientation2 == eOrientation.Horizontal)
		{
			displayRectangle.Height -= m_PaddingTop + m_PaddingBottom;
		}
		else
		{
			displayRectangle.Height -= m_PaddingLeft + m_PaddingRight;
		}
		Color textColor = GetTextColor(pa);
		Color color = textColor;
		if (!SymbolColor.IsEmpty)
		{
			color = SymbolColor;
		}
		if (compositeImage != null || flag)
		{
			switch (m_ImagePosition)
			{
			case eImagePosition.Left:
				if (eOrientation2 == eOrientation.Horizontal)
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, Symbol, Symbols.GetFontAwesome(SymbolSize), color, new Rectangle(displayRectangle.X + 2, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, 0, 0), eTextFormat.Default);
					}
					else
					{
						compositeImage.DrawImage(graphics, new Rectangle(displayRectangle.X + 2, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, size.Width, size.Height));
					}
					displayRectangle.X += size.Width + 4;
					displayRectangle.Width -= size.Width + 4;
				}
				else
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, Symbol, Symbols.GetFontAwesome(SymbolSize), color, new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Height) / 2, displayRectangle.Y + 2, 0, 0), eTextFormat.Default);
					}
					else
					{
						compositeImage.DrawImage(graphics, new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Height) / 2, displayRectangle.Y + 2, size.Width, size.Height));
					}
					displayRectangle.Y += size.Height + 4;
					displayRectangle.Height -= size.Height + 4;
				}
				break;
			case eImagePosition.Right:
				if (eOrientation2 == eOrientation.Horizontal)
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, Symbol, Symbols.GetFontAwesome(SymbolSize), color, new Rectangle(displayRectangle.Right - size.Width - 2, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, 0, 0), eTextFormat.Default);
					}
					else
					{
						compositeImage.DrawImage(graphics, new Rectangle(displayRectangle.Right - size.Width - 2, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, size.Width, size.Height));
					}
					displayRectangle.Width -= size.Width + 4;
				}
				else
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, Symbol, Symbols.GetFontAwesome(SymbolSize), color, new Rectangle(displayRectangle.X, displayRectangle.Bottom - size.Height - 2, 0, 0), eTextFormat.Default);
					}
					else
					{
						compositeImage.DrawImage(graphics, new Rectangle(displayRectangle.X, displayRectangle.Bottom - size.Height - 2, size.Width, size.Height));
					}
					displayRectangle.Height -= size.Height + 4;
				}
				break;
			case eImagePosition.Top:
				if (eOrientation2 == eOrientation.Horizontal)
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, Symbol, Symbols.GetFontAwesome(SymbolSize), color, new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Width) / 2, displayRectangle.Y, 0, 0), eTextFormat.Default);
					}
					else
					{
						compositeImage.DrawImage(graphics, new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Width) / 2, displayRectangle.Y, size.Width, size.Height));
					}
					displayRectangle.Y += size.Height + 2;
					displayRectangle.Height -= size.Height + 2;
				}
				else
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, Symbol, Symbols.GetFontAwesome(SymbolSize), color, new Rectangle(displayRectangle.X, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, 0, 0), eTextFormat.Default);
					}
					else
					{
						compositeImage.DrawImage(graphics, new Rectangle(displayRectangle.X, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, size.Width, size.Height));
					}
					displayRectangle.X += size.Width + 2;
					displayRectangle.Width -= size.Width + 2;
				}
				break;
			case eImagePosition.Bottom:
				if (eOrientation2 == eOrientation.Horizontal)
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, Symbol, Symbols.GetFontAwesome(SymbolSize), color, new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Width) / 2, displayRectangle.Bottom - size.Height, 0, 0), eTextFormat.Default);
					}
					else
					{
						compositeImage.DrawImage(graphics, new Rectangle(displayRectangle.X + (displayRectangle.Width - size.Width) / 2, displayRectangle.Bottom - size.Height, size.Width, size.Height));
					}
					displayRectangle.Height -= size.Height + 2;
				}
				else
				{
					if (flag)
					{
						TextDrawing.DrawStringLegacy(graphics, Symbol, Symbols.GetFontAwesome(SymbolSize), color, new Rectangle(displayRectangle.Right - size.Width, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, 0, 0), eTextFormat.Default);
					}
					else
					{
						compositeImage.DrawImage(graphics, new Rectangle(displayRectangle.Right - size.Width, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, size.Width, size.Height));
					}
					displayRectangle.Width -= size.Width + 2;
				}
				break;
			}
		}
		if (displayRectangle.Height > 0 && displayRectangle.Width > 0)
		{
			if (eOrientation2 == eOrientation.Horizontal)
			{
				if ((int)m_TextAlignment == 2)
				{
					displayRectangle.Width--;
				}
				if (base.TextMarkupBody != null)
				{
					MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, GetFont(), textColor, pa.RightToLeft);
					markupDrawContext.HotKeyPrefixVisible = (GetStringFormat() & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
					if ((int)m_TextLineAlignment == 0)
					{
						base.TextMarkupBody.Bounds = new Rectangle(displayRectangle.Location, base.TextMarkupBody.Bounds.Size);
					}
					else if ((int)m_TextLineAlignment == 1)
					{
						base.TextMarkupBody.Bounds = new Rectangle(new Point(displayRectangle.X, displayRectangle.Y + (displayRectangle.Height - base.TextMarkupBody.Bounds.Height) / 2), base.TextMarkupBody.Bounds.Size);
					}
					else if ((int)m_TextLineAlignment == 2)
					{
						base.TextMarkupBody.Bounds = new Rectangle(new Point(displayRectangle.X, displayRectangle.Bottom - base.TextMarkupBody.Bounds.Height), base.TextMarkupBody.Bounds.Size);
					}
					Region clip = graphics.Clip;
					Rectangle rectangle2 = displayRectangle;
					rectangle2.Inflate(2, 2);
					graphics.SetClip(rectangle2, (CombineMode)0);
					base.TextMarkupBody.Render(markupDrawContext);
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
					eTextFormat eTextFormat2 = GetStringFormat();
					if (pa.RightToLeft)
					{
						eTextFormat2 |= eTextFormat.RightToLeft;
					}
					if (pa.GlassEnabled && Parent is CaptionItemContainer && !(ContainerControl is QatToolbar))
					{
						if (!pa.CachedPaint)
						{
							Office2007RibbonControlPainter.PaintTextOnGlass(graphics, m_Text, GetFont(), displayRectangle, TextDrawing.GetTextFormat(eTextFormat2));
						}
					}
					else
					{
						TextDrawing.DrawString(graphics, m_Text, GetFont(), textColor, displayRectangle, eTextFormat2);
					}
				}
			}
			else
			{
				if (_VerticalTextTopUp)
				{
					graphics.RotateTransform(90f);
				}
				else
				{
					graphics.RotateTransform(-90f);
				}
				if (base.TextMarkupBody != null)
				{
					base.TextMarkupBody.Bounds = (_VerticalTextTopUp ? new Rectangle(displayRectangle.Top, -displayRectangle.Right, base.TextMarkupBody.Bounds.Width + 2, base.TextMarkupBody.Bounds.Height) : new Rectangle(-displayRectangle.Bottom, displayRectangle.X, displayRectangle.Height + 1, displayRectangle.Width + 1));
					MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(graphics, GetFont(), textColor, pa.RightToLeft);
					markupDrawContext2.HotKeyPrefixVisible = (GetStringFormat() & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
					base.TextMarkupBody.Render(markupDrawContext2);
				}
				else
				{
					TextDrawing.DrawStringLegacy(graphics, m_Text, GetFont(), textColor, _VerticalTextTopUp ? new Rectangle(displayRectangle.Top, -displayRectangle.Right, displayRectangle.Height + 1, displayRectangle.Width + 1) : new Rectangle(-displayRectangle.Bottom, displayRectangle.X, displayRectangle.Height + 1, displayRectangle.Width + 1), GetStringFormat());
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
			DesignTime.DrawDesignTimeSelection(graphics, rect, pa.Colors.ItemDesignTimeBorder);
		}
		_ItemPaintArgs = null;
	}

	private Color GetTextColor(ItemPaintArgs pa)
	{
		Color defaultColor = m_ForeColor;
		if (_ForeColorColorSchemePart != eColorSchemePart.None)
		{
			defaultColor = (Color)pa.Colors.GetType().GetProperty(_ForeColorColorSchemePart.ToString()).GetValue(pa.Colors, null);
		}
		return GetTextColor(pa, EffectiveStyle, Enabled && GetEnabled(pa.ContainerControl), defaultColor);
	}

	internal static Color GetTextColor(ItemPaintArgs pa, eDotNetBarStyle effectiveStyle, bool isEnabled, Color defaultColor)
	{
		Office2007ColorTable office2007ColorTable = null;
		if (pa.Renderer is Office2007Renderer)
		{
			office2007ColorTable = ((Office2007Renderer)pa.Renderer).ColorTable;
		}
		if (defaultColor.IsEmpty && office2007ColorTable != null && ((pa.ContainerControl is Bar && ((Bar)(object)pa.ContainerControl).BarType == eBarType.StatusBar) || !(pa.ContainerControl is Bar)) && office2007ColorTable.LabelItemColors.TryGetValue(((object)pa.ContainerControl).GetType(), out var value))
		{
			if (!isEnabled)
			{
				return value.DisabledText;
			}
			return value.Text;
		}
		if (BarFunctions.IsOffice2007Style(effectiveStyle) && defaultColor.IsEmpty && office2007ColorTable != null)
		{
			if ((pa.IsOnMenu || pa.IsOnPopupBar) && office2007ColorTable.ButtonItemColors.Count > 0)
			{
				if (!isEnabled)
				{
					return pa.Colors.ItemDisabledText;
				}
				return office2007ColorTable.ButtonItemColors[0].Default.Text;
			}
			if ((pa.ContainerControl is RibbonStrip || pa.ContainerControl is Bar) && office2007ColorTable.RibbonTabItemColors.Count > 0)
			{
				return office2007ColorTable.RibbonTabItemColors[0].Default.Text;
			}
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = office2007ColorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.Orange)];
			if (office2007ButtonItemColorTable != null && !office2007ButtonItemColorTable.Default.Text.IsEmpty)
			{
				if (!isEnabled)
				{
					return pa.Colors.ItemDisabledText;
				}
				return office2007ButtonItemColorTable.Default.Text;
			}
		}
		else if (!isEnabled)
		{
			return pa.Colors.ItemDisabledText;
		}
		if (defaultColor.IsEmpty)
		{
			return pa.Colors.ItemText;
		}
		return defaultColor;
	}

	private Graphics CreateGraphics()
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
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		if (SuspendLayout)
		{
			return;
		}
		if (base.TextMarkupBody != null)
		{
			RecalcSizeMarkup();
		}
		else
		{
			Size size = Size.Empty;
			if (string.IsNullOrEmpty(Symbol))
			{
				CompositeImage image = GetImage();
				if (image != null)
				{
					size = image.Size;
				}
			}
			eOrientation eOrientation2 = Orientation;
			if (_TextOrientation == eOrientation.Vertical)
			{
				eOrientation2 = _TextOrientation;
			}
			if (eOrientation2 == eOrientation.Horizontal || m_Width == 0)
			{
				if (m_Width == 0)
				{
					Graphics val = CreateGraphics();
					if (val == null)
					{
						return;
					}
					try
					{
						if (!string.IsNullOrEmpty(Symbol))
						{
							Font fontAwesome = Symbols.GetFontAwesome(SymbolSize);
							size = TextDrawing.MeasureString(val, "\uf00a", fontAwesome);
							int num = (int)Math.Ceiling((float)fontAwesome.FontFamily.GetCellDescent(fontAwesome.Style) * fontAwesome.Size / (float)fontAwesome.FontFamily.GetEmHeight(fontAwesome.Style));
							size.Height -= num;
						}
						Font val2 = null;
						val2 = ((m_Font == null) ? GetFont() : m_Font);
						Size empty = Size.Empty;
						eTextFormat stringFormat = GetStringFormat();
						if (m_Text != "")
						{
							string text = m_Text;
							if (text.EndsWith(" "))
							{
								text += ".";
							}
							int num2 = 0;
							if (m_WordWrap && Stretch && WidthInternal > 16)
							{
								num2 = WidthInternal;
							}
							empty = ((eOrientation2 != eOrientation.Vertical || base.IsOnMenu) ? TextDrawing.MeasureString(val, text, val2, num2, stringFormat) : TextDrawing.MeasureStringLegacy(val, text, val2, new Size(num2, 0), stringFormat));
							empty.Width += 2;
						}
						else
						{
							empty = ((eOrientation2 != eOrientation.Vertical || base.IsOnMenu) ? TextDrawing.MeasureString(val, " ", val2, 0, stringFormat) : TextDrawing.MeasureStringLegacy(val, " ", val2, Size.Empty, stringFormat));
							empty.Width += 2;
						}
						if (Parent is CaptionItemContainer)
						{
							empty.Width += 2;
						}
						if (m_BorderType != 0)
						{
							empty.Width += 4;
							empty.Height += 2;
							if (m_BorderType != eBorderType.SingleLine)
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
								WidthInternal = empty.Width + m_PaddingLeft + m_PaddingRight;
							}
							HeightInternal = empty.Height + m_PaddingTop + m_PaddingBottom;
							if (!size.IsEmpty)
							{
								if (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right)
								{
									WidthInternal += size.Width + 4;
									HeightInternal = Math.Max(HeightInternal, size.Height + 2);
								}
								else
								{
									HeightInternal += size.Height + 4;
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
								HeightInternal = empty.Width + m_PaddingLeft + m_PaddingRight;
							}
							WidthInternal = empty.Height + m_PaddingTop + m_PaddingBottom;
							if (!size.IsEmpty)
							{
								if (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right)
								{
									HeightInternal += size.Width + 4;
									WidthInternal = Math.Max(WidthInternal, size.Width + 2);
								}
								else
								{
									WidthInternal += size.Height + 4;
									HeightInternal = Math.Max(size.Height + 2, HeightInternal);
								}
							}
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
					WidthInternal = m_Width;
					if (m_Height == 0)
					{
						Font val3 = null;
						val3 = ((m_Font == null) ? GetFont() : m_Font);
						Graphics val4 = CreateGraphics();
						if (val4 != null && !string.IsNullOrEmpty(Symbol))
						{
							Font fontAwesome2 = Symbols.GetFontAwesome(SymbolSize);
							size = TextDrawing.MeasureString(val4, "\uf00a", fontAwesome2);
							int num3 = (int)Math.Ceiling((float)fontAwesome2.FontFamily.GetCellDescent(fontAwesome2.Style) * fontAwesome2.Size / (float)fontAwesome2.FontFamily.GetEmHeight(fontAwesome2.Style));
							size.Height -= num3;
						}
						if (WordWrap)
						{
							if (val4 != null)
							{
								int num4 = WidthInternal - (m_PaddingLeft + m_PaddingRight);
								if (!size.IsEmpty && eOrientation2 == eOrientation.Horizontal && (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right))
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
						HeightInternal += m_PaddingTop + m_PaddingBottom + (IsRightToLeft ? 2 : 0);
						if (m_BorderType != 0)
						{
							HeightInternal += 2;
							if (m_BorderType != eBorderType.SingleLine)
							{
								HeightInternal += 2;
							}
						}
						if (!size.IsEmpty)
						{
							if (eOrientation2 == eOrientation.Horizontal)
							{
								if (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right)
								{
									HeightInternal = Math.Max(HeightInternal, size.Height);
								}
								else
								{
									HeightInternal += size.Height + 4;
								}
							}
							else if (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right)
							{
								WidthInternal = Math.Max(HeightInternal, size.Height + 2);
							}
							else
							{
								WidthInternal += size.Height + 4;
							}
						}
					}
					else
					{
						HeightInternal = m_Height;
					}
				}
			}
			else
			{
				WidthInternal = m_Width;
				HeightInternal = m_Height;
			}
		}
		base.RecalcSize();
	}

	protected override void OnExternalSizeChange()
	{
		if (base.TextMarkupBody != null)
		{
			if (_TextOrientation == eOrientation.Horizontal)
			{
				ArrangeMarkup(WidthInternal);
			}
			else
			{
				ArrangeMarkup(HeightInternal + 1);
			}
		}
		base.OnExternalSizeChange();
	}

	private void ArrangeMarkup(int specifiedWidth)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
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
			val3 = ((m_Font == null) ? GetFont() : m_Font);
			CompositeImage image = GetImage();
			Size size = GetMarkupAvailableSize(specifiedWidth);
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(val2, val3, ForeColor, rightToLeft: false);
			markupDrawContext.HotKeyPrefixVisible = (GetStringFormat() & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
			base.TextMarkupBody.Measure(size, markupDrawContext);
			markupDrawContext.RightToLeft = (int)val.RightToLeft == 1;
			if (specifiedWidth == 0 && m_Width == 0)
			{
				size = base.TextMarkupBody.Bounds.Size;
			}
			base.TextMarkupBody.Arrange(new Rectangle(Point.Empty, size), markupDrawContext);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private Size GetMarkupAvailableSize(int specifiedWidth)
	{
		Size result = new Size((specifiedWidth == 0) ? m_Width : specifiedWidth, 1);
		if (_TextOrientation == eOrientation.Vertical)
		{
			result = new Size((specifiedWidth == 0) ? m_Height : specifiedWidth, 1);
		}
		CompositeImage image = GetImage();
		if (result.Width == 0)
		{
			result.Width = 1600;
		}
		else
		{
			result.Width -= m_PaddingLeft + m_PaddingRight;
			if (image != null && (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right))
			{
				result.Width -= image.Width + 4;
			}
			if (m_BorderType != 0)
			{
				result.Width -= 4;
			}
		}
		return result;
	}

	private void RecalcSizeMarkup()
	{
		RecalcSizeMarkup(0);
	}

	internal void RecalcSizeMarkup(int proposedWidth)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		eOrientation eOrientation2 = Orientation;
		if (_TextOrientation == eOrientation.Vertical)
		{
			eOrientation2 = _TextOrientation;
		}
		ArrangeMarkup(proposedWidth);
		int num = base.TextMarkupBody.Bounds.Width;
		int num2 = base.TextMarkupBody.Bounds.Height;
		if (m_BorderType != 0)
		{
			num += 4;
			num2 += 4;
		}
		Size size = Size.Empty;
		if (!string.IsNullOrEmpty(Symbol))
		{
			Graphics g = CreateGraphics();
			Font fontAwesome = Symbols.GetFontAwesome(SymbolSize);
			size = TextDrawing.MeasureString(g, "\uf00a", fontAwesome);
			int num3 = (int)Math.Ceiling((float)fontAwesome.FontFamily.GetCellDescent(fontAwesome.Style) * fontAwesome.Size / (float)fontAwesome.FontFamily.GetEmHeight(fontAwesome.Style));
			size.Height -= num3;
		}
		else
		{
			CompositeImage image = GetImage();
			if (image != null)
			{
				size = image.Size;
			}
		}
		if (!size.IsEmpty && m_Width == 0)
		{
			if (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right)
			{
				num += size.Width + 4;
				num2 = Math.Max(num2, size.Height + 2);
			}
			else
			{
				num2 += size.Height + 4;
				num = Math.Max(num, size.Width + 2);
			}
		}
		if (m_Width == 0)
		{
			num += m_PaddingLeft + m_PaddingRight;
			num2 += m_PaddingTop + m_PaddingBottom;
			if (Stretch && !(Parent is ItemContainer))
			{
				num = 8;
			}
		}
		if (eOrientation2 == eOrientation.Vertical)
		{
			num += 2;
			int num4 = num;
			num = num2;
			num2 = num4;
		}
		if (m_Width == 0)
		{
			m_Rect.Width = num;
		}
		else
		{
			m_Rect.Width = m_Width;
		}
		if (m_Height == 0)
		{
			m_Rect.Height = num2;
		}
		else
		{
			m_Rect.Height = m_Height;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return !m_BackColor.IsEmpty;
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSingleLineColor()
	{
		return m_SingleLineColor != SystemColors.ControlDark;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSingleLineColor()
	{
		SingleLineColor = SystemColors.ControlDark;
	}

	private Color GetSingleLineColor()
	{
		if (m_SingleLineColorCustom)
		{
			return m_SingleLineColor;
		}
		Color result = m_SingleLineColor;
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
					m_SingleLineColor = colorScheme.BarDockedBorder;
				}
				else
				{
					m_SingleLineColor = SystemColors.ControlDark;
				}
			}
		}
		return result;
	}

	protected virtual Font GetFont()
	{
		if (m_Font != null)
		{
			return m_Font;
		}
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			return val.Font;
		}
		return SystemFonts.DefaultFont;
	}

	private eTextFormat GetStringFormat()
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
		if (!m_ShowPrefix)
		{
			eTextFormat2 |= eTextFormat.NoPrefix;
		}
		eTextFormat2 = (m_WordWrap ? (eTextFormat2 | eTextFormat.WordBreak) : (eTextFormat2 | eTextFormat.SingleLine));
		if ((int)m_TextAlignment == 1)
		{
			eTextFormat2 |= eTextFormat.HorizontalCenter;
		}
		else if ((int)m_TextAlignment == 2)
		{
			eTextFormat2 |= eTextFormat.Right;
		}
		if ((int)m_TextLineAlignment == 1)
		{
			eTextFormat2 |= eTextFormat.VerticalCenter;
		}
		else if ((int)m_TextLineAlignment == 2)
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
		itemXmlElement.SetAttribute("bt", XmlConvert.ToString((int)m_BorderType));
		if (m_BorderSide != eBorderSide.All)
		{
			itemXmlElement.SetAttribute("BorderSide", XmlConvert.ToString((int)m_BorderSide));
		}
		if (!m_BackColor.IsEmpty)
		{
			itemXmlElement.SetAttribute("bc", BarFunctions.ColorToString(m_BackColor));
		}
		itemXmlElement.SetAttribute("fc", BarFunctions.ColorToString(m_ForeColor));
		if (m_SingleLineColorCustom)
		{
			itemXmlElement.SetAttribute("sc", BarFunctions.ColorToString(m_SingleLineColor));
		}
		itemXmlElement.SetAttribute("ta", XmlConvert.ToString((int)m_TextAlignment));
		itemXmlElement.SetAttribute("tla", XmlConvert.ToString((int)m_TextLineAlignment));
		itemXmlElement.SetAttribute("ds", XmlConvert.ToString(m_DividerStyle));
		itemXmlElement.SetAttribute("pl", XmlConvert.ToString(m_PaddingLeft));
		itemXmlElement.SetAttribute("pr", XmlConvert.ToString(m_PaddingRight));
		itemXmlElement.SetAttribute("pt", XmlConvert.ToString(m_PaddingTop));
		itemXmlElement.SetAttribute("pb", XmlConvert.ToString(m_PaddingBottom));
		itemXmlElement.SetAttribute("w", XmlConvert.ToString(m_Width));
		itemXmlElement.SetAttribute("h", XmlConvert.ToString(m_Height));
		if (Font != null && (Font.Name != SystemInformation.MenuFont.Name || Font.Size != SystemInformation.MenuFont.Size || Font.Style != SystemInformation.MenuFont.Style))
		{
			itemXmlElement.SetAttribute("fontname", Font.Name);
			itemXmlElement.SetAttribute("fontemsize", XmlConvert.ToString(Font.Size));
			itemXmlElement.SetAttribute("fontstyle", XmlConvert.ToString((int)Font.Style));
		}
		XmlElement xmlElement = null;
		XmlElement xmlElement2 = null;
		if (m_Image != null || m_ImageIndex >= 0 || m_Icon != null)
		{
			xmlElement = itemXmlElement.OwnerDocument.CreateElement("images");
			itemXmlElement.AppendChild(xmlElement);
			if (m_ImageIndex >= 0)
			{
				xmlElement.SetAttribute("imageindex", XmlConvert.ToString(m_ImageIndex));
			}
			if (m_Image != null)
			{
				xmlElement2 = itemXmlElement.OwnerDocument.CreateElement("image");
				xmlElement2.SetAttribute("type", "default");
				xmlElement.AppendChild(xmlElement2);
				BarFunctions.SerializeImage(m_Image, xmlElement2);
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
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		m_BorderType = (eBorderType)XmlConvert.ToInt32(itemXmlElement.GetAttribute("bt"));
		if (itemXmlElement.HasAttribute("BorderSide"))
		{
			m_BorderSide = (eBorderSide)XmlConvert.ToInt32(itemXmlElement.GetAttribute("BorderSide"));
		}
		else
		{
			m_BorderSide = eBorderSide.All;
		}
		if (itemXmlElement.HasAttribute("bc"))
		{
			m_BackColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("bc"));
		}
		else
		{
			m_BackColor = Color.Empty;
		}
		m_ForeColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("fc"));
		if (itemXmlElement.HasAttribute("sc"))
		{
			m_SingleLineColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("sc"));
			if (m_SingleLineColor != SystemColors.ControlDark)
			{
				m_SingleLineColorCustom = true;
			}
		}
		else
		{
			m_SingleLineColorCustom = false;
			m_SingleLineColor = SystemColors.ControlDark;
		}
		m_TextAlignment = (StringAlignment)XmlConvert.ToInt32(itemXmlElement.GetAttribute("ta"));
		m_TextLineAlignment = (StringAlignment)XmlConvert.ToInt32(itemXmlElement.GetAttribute("tla"));
		m_DividerStyle = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("ds"));
		m_PaddingLeft = XmlConvert.ToInt32(itemXmlElement.GetAttribute("pl"));
		m_PaddingRight = XmlConvert.ToInt32(itemXmlElement.GetAttribute("pr"));
		m_PaddingTop = XmlConvert.ToInt32(itemXmlElement.GetAttribute("pt"));
		m_PaddingBottom = XmlConvert.ToInt32(itemXmlElement.GetAttribute("pb"));
		m_Width = XmlConvert.ToInt32(itemXmlElement.GetAttribute("w"));
		m_Height = XmlConvert.ToInt32(itemXmlElement.GetAttribute("h"));
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
				m_ImageIndex = XmlConvert.ToInt32(childNode.GetAttribute("imageindex"));
			}
			{
				foreach (XmlElement childNode2 in childNode.ChildNodes)
				{
					string attribute2 = childNode2.GetAttribute("type");
					if (!(attribute2 == "default"))
					{
						if (attribute2 == "icon")
						{
							m_Icon = BarFunctions.DeserializeIcon(childNode2);
							m_ImageIndex = -1;
						}
					}
					else
					{
						m_Image = BarFunctions.DeserializeImage(childNode2);
						m_ImageIndex = -1;
					}
				}
				break;
			}
		}
	}

	private void OnImageChanged()
	{
		if (m_DisabledImage != null)
		{
			m_DisabledImage.Dispose();
			m_DisabledImage = null;
		}
		if (m_DisabledIcon != null)
		{
			m_DisabledIcon.Dispose();
			m_DisabledIcon = null;
		}
		NeedRecalcSize = true;
		Refresh();
	}

	private CompositeImage GetImage()
	{
		if (GetEnabled())
		{
			return GetImage(ImageState.Default);
		}
		return GetImage(ImageState.Disabled);
	}

	private CompositeImage GetImage(ImageState state)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		Image val = null;
		if (state == ImageState.Disabled)
		{
			if (m_DisabledImage == null && m_DisabledIcon == null && (m_Icon != null || m_Image != null || m_ImageIndex >= 0))
			{
				CreateDisabledImage();
			}
			if (m_DisabledIcon != null)
			{
				return new CompositeImage(new Icon(m_DisabledIcon, IconSize), dispose: true);
			}
			if (m_DisabledImage != null)
			{
				return new CompositeImage(m_DisabledImage, dispose: false);
			}
		}
		if (m_Icon != null)
		{
			Size iconSize = IconSize;
			return new CompositeImage(new Icon(m_Icon, iconSize), dispose: true);
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

	private void CreateDisabledImage()
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
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Expected O, but got Unknown
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		if (m_Image == null && m_ImageIndex < 0 && m_Icon == null)
		{
			return;
		}
		if (m_DisabledImage != null)
		{
			m_DisabledImage.Dispose();
		}
		m_DisabledImage = null;
		if (m_DisabledIcon != null)
		{
			m_DisabledIcon.Dispose();
		}
		m_DisabledIcon = null;
		CompositeImage image = GetImage(ImageState.Default);
		if (image == null)
		{
			return;
		}
		if (GetOwner() is IOwner && ((IOwner)GetOwner()).DisabledImagesGrayScale)
		{
			if (image.IsIcon)
			{
				m_DisabledIcon = BarFunctions.CreateDisabledIcon(image.Icon);
			}
			else
			{
				ref Image disabledImage = ref m_DisabledImage;
				Image image2 = image.Image;
				disabledImage = (Image)(object)ImageHelper.CreateGrayScaleImage((image2 is Bitmap) ? image2 : null);
			}
		}
		if (m_DisabledIcon == null && m_DisabledImage == null)
		{
			PixelFormat val = (PixelFormat)2498570;
			if (!image.IsIcon && image.Image != null)
			{
				val = image.Image.PixelFormat;
			}
			if ((int)val == 196865 || (int)val == 197634 || (int)val == 198659)
			{
				val = (PixelFormat)2498570;
			}
			Bitmap val2 = new Bitmap(image.Width, image.Height, val);
			m_DisabledImage = (Image)new Bitmap(image.Width, image.Height, val);
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			SolidBrush val4 = new SolidBrush(Color.White);
			try
			{
				val3.FillRectangle((Brush)(object)val4, 0, 0, image.Width, image.Height);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			image.DrawImage(val3, new Rectangle(0, 0, image.Width, image.Height));
			val3.Dispose();
			val3 = Graphics.FromImage(m_DisabledImage);
			val2.MakeTransparent(Color.White);
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if ((effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)) && NativeFunctions.ColorDepth >= 8)
			{
				ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
				{
					new float[5],
					new float[5],
					new float[5],
					new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0f },
					new float[5]
				});
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
			image.Dispose();
		}
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
		return !_SymbolColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSymbolColor()
	{
		SymbolColor = Color.Empty;
	}

	protected override void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is HyperLink hyperLink)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(hyperLink.Name, hyperLink.HRef));
		}
		base.TextMarkupLinkClick(sender, e);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (this.MarkupLinkClick != null)
		{
			this.MarkupLinkClick(this, e);
		}
	}
}
