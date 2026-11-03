using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using DevComponents.AdvTree;

namespace DevComponents.DotNetBar;

[TypeConverter(typeof(ExpandableObjectConverter))]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class ElementStyle : IComponent, IDisposable
{
	private const int އ = 8;

	private EventHandler ވ;

	private EventHandler މ;

	private Color ފ = Color.Empty;

	private eColorSchemePart ދ = eColorSchemePart.None;

	private Color ތ = Color.Empty;

	private eColorSchemePart ލ = eColorSchemePart.None;

	private int ގ;

	private Image ޏ;

	private eStyleBackgroundImage ސ;

	private byte ޑ = byte.MaxValue;

	private eGradientType ޒ;

	private Font ޓ;

	private bool ݥ;

	private eStyleTextAlignment ޔ;

	private eStyleTextAlignment ޕ = eStyleTextAlignment.Center;

	private eStyleTextTrimming ޖ = eStyleTextTrimming.EllipsisCharacter;

	private Color ޗ = Color.Empty;

	private eColorSchemePart ޘ = eColorSchemePart.None;

	private Color ޙ = Color.Empty;

	private eColorSchemePart ޚ = eColorSchemePart.None;

	private Point ޛ = Point.Empty;

	private int ޜ;

	private int ޝ;

	private int ޞ;

	private int ޟ;

	private int ޠ;

	private int ޡ;

	private int ޢ;

	private int ޣ;

	private eStyleBorderType ޤ;

	private eStyleBorderType ޥ;

	private eStyleBorderType _07A6;

	private eStyleBorderType _07A7;

	private Color _07A8 = Color.Empty;

	private eColorSchemePart _07A9 = eColorSchemePart.None;

	private Color _07AA = Color.Empty;

	private eColorSchemePart _07AB = eColorSchemePart.None;

	private int _07AC = 90;

	private Color _07AD = Color.Empty;

	private eColorSchemePart _07AE = eColorSchemePart.None;

	private Color _07AF = Color.Empty;

	private eColorSchemePart _07B0 = eColorSchemePart.None;

	private int ޱ = 90;

	private Color _07B2 = Color.Empty;

	private eColorSchemePart _07B3 = eColorSchemePart.None;

	private Color _07B4 = Color.Empty;

	private eColorSchemePart _07B5 = eColorSchemePart.None;

	private Color _07B6 = Color.Empty;

	private eColorSchemePart _07B7 = eColorSchemePart.None;

	private Color _07B8 = Color.Empty;

	private eColorSchemePart _07B9 = eColorSchemePart.None;

	private int _07BA;

	private int _07BB;

	private int _07BC;

	private int _07BD;

	private eCornerType _07BE = eCornerType.Square;

	private eCornerType _07BF;

	private eCornerType _߀;

	private eCornerType _߁;

	private eCornerType _߂;

	private int _߃ = 8;

	private Size _߄ = Size.Empty;

	private bool ӱ = true;

	private string Ӿ = "";

	private string _߅ = "";

	private int _߆;

	private ISite _߇;

	private bool _߈;

	private ColorScheme լ;

	private BackgroundColorBlendCollection _߉ = new BackgroundColorBlendCollection();

	private string ߊ = "";

	private bool ߋ;

	private eOrientation ߌ;

	private bool _ߍ;

	private bool _ߎ;

	internal bool _ߏ;

	private int ߐ;

	private ElementStyleCollection ӭ;

	private DevComponents.AdvTree.AdvTree Ԛ;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Collection that defines the multicolor gradient background.")]
	public BackgroundColorBlendCollection BackColorBlend => _߉;

	[DevCoSerialize]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Browsable(true)]
	[Description("Gets or sets the background color for UI element.")]
	[Category("Background")]
	[TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public Color BackColor
	{
		get
		{
			return _065A(ފ, ދ);
		}
		set
		{
			if (value != BackColor && ((ދ != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BackColorSchemePart = eColorSchemePart.None;
			}
			ފ = value;
			ߓ();
		}
	}

	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	[DevCoSerialize]
	public eColorSchemePart BackColorSchemePart
	{
		get
		{
			return ދ;
		}
		set
		{
			ދ = value;
			ߓ();
		}
	}

	[Description("Gets or sets the target gradient background color for UI element.")]
	[DevCoSerialize]
	[Browsable(true)]
	[TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[Category("Background")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	public Color BackColor2
	{
		get
		{
			return _065A(ތ, ލ);
		}
		set
		{
			if (value != BackColor2 && ((ލ != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BackColor2SchemePart = eColorSchemePart.None;
			}
			ތ = value;
			ߓ();
		}
	}

	[DefaultValue(eColorSchemePart.None)]
	[Browsable(false)]
	[DevCoSerialize]
	public eColorSchemePart BackColor2SchemePart
	{
		get
		{
			return ލ;
		}
		set
		{
			ލ = value;
			ߓ();
		}
	}

	[Category("Background")]
	[DefaultValue(0)]
	[DevCoSerialize]
	[Browsable(true)]
	[Description("Gets or sets the background gradient angle.")]
	public int BackColorGradientAngle
	{
		get
		{
			return ގ;
		}
		set
		{
			if (ގ != value)
			{
				ގ = value;
				ߓ();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Background")]
	[DefaultValue(eGradientType.Linear)]
	[Description("Indicates background gradient fill type.")]
	public eGradientType BackColorGradientType
	{
		get
		{
			return ޒ;
		}
		set
		{
			if (ޒ != value)
			{
				ޒ = value;
				ߓ();
			}
		}
	}

	[DevCoSerialize]
	[Category("Background")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[DefaultValue(null)]
	[Description("Specifies background image.")]
	public Image BackgroundImage
	{
		get
		{
			return ޏ;
		}
		set
		{
			ޏ = value;
			ߓ();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[DefaultValue(eStyleBackgroundImage.Stretch)]
	[Description("Specifies background image position when container is larger than image.")]
	[Category("Background")]
	public eStyleBackgroundImage BackgroundImagePosition
	{
		get
		{
			return ސ;
		}
		set
		{
			if (ސ != value)
			{
				ސ = value;
				ߓ();
			}
		}
	}

	[Browsable(true)]
	[Category("Background")]
	[DevCoSerialize]
	[DefaultValue(byte.MaxValue)]
	[Description("Specifies the transparency of background image.")]
	public byte BackgroundImageAlpha
	{
		get
		{
			return ޑ;
		}
		set
		{
			if (ޑ != value)
			{
				ޑ = value;
				ߓ();
			}
		}
	}

	[TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[DevCoSerialize]
	[Description("Gets or sets the text color displayed in this UI element.")]
	[Browsable(true)]
	[Category("Text Properties")]
	public Color TextColor
	{
		get
		{
			return _065A(ޗ, ޘ);
		}
		set
		{
			if (value != TextColor && ((ޘ != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				TextColorSchemePart = eColorSchemePart.None;
			}
			ޗ = value;
			ߓ();
		}
	}

	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	[DevCoSerialize]
	public eColorSchemePart TextColorSchemePart
	{
		get
		{
			return ޘ;
		}
		set
		{
			ޘ = value;
			ߓ();
		}
	}

	[Browsable(true)]
	[Description("Indicates text shadow color.")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[DevCoSerialize]
	[Category("Text Properties")]
	[TypeConverter(typeof(ColorConverter))]
	public Color TextShadowColor
	{
		get
		{
			return _065A(ޙ, ޚ);
		}
		set
		{
			if ((ޚ != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty)
			{
				TextShadowColorSchemePart = eColorSchemePart.None;
			}
			ޙ = value;
			ߓ();
		}
	}

	[DevCoSerialize]
	[DefaultValue(eColorSchemePart.None)]
	[Browsable(false)]
	public eColorSchemePart TextShadowColorSchemePart
	{
		get
		{
			return ޚ;
		}
		set
		{
			ޚ = value;
			ߓ();
		}
	}

	[Description("Indicates text shadow offset in pixels.")]
	[Browsable(true)]
	[Category("Text Properties")]
	[DevCoSerialize]
	public Point TextShadowOffset
	{
		get
		{
			return ޛ;
		}
		set
		{
			ޛ = value;
			ߓ();
		}
	}

	[DevCoSerialize]
	[Category("Text Properties")]
	[Browsable(true)]
	[Description("Gets or sets the Font used to draw this the text.")]
	[DefaultValue(null)]
	public Font Font
	{
		get
		{
			return ޓ;
		}
		set
		{
			ޓ = value;
			ߓ();
			ߔ();
		}
	}

	[Browsable(true)]
	[Description("Gets or sets a value that determines whether text is displayed in multiple lines or one long line.")]
	[Category("Text Formatting")]
	[DevCoSerialize]
	[DefaultValue(false)]
	public bool WordWrap
	{
		get
		{
			return ݥ;
		}
		set
		{
			ݥ = value;
			ߓ();
			ߔ();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Text Formatting")]
	[DefaultValue(eStyleTextAlignment.Near)]
	[Description("Specifies alignment of the text.")]
	public eStyleTextAlignment TextAlignment
	{
		get
		{
			return ޔ;
		}
		set
		{
			ޔ = value;
			ߓ();
		}
	}

	[Description("Specifies alignment of the text.")]
	[Browsable(true)]
	[DevCoSerialize]
	[DefaultValue(eStyleTextAlignment.Center)]
	[Category("Text Formatting")]
	public eStyleTextAlignment TextLineAlignment
	{
		get
		{
			return ޕ;
		}
		set
		{
			ޕ = value;
			ߓ();
		}
	}

	[Category("Text Formatting")]
	[DefaultValue(eStyleTextTrimming.EllipsisCharacter)]
	[DevCoSerialize]
	[Description("Specifies how to trim characters when text does not fit.")]
	[Browsable(true)]
	public eStyleTextTrimming TextTrimming
	{
		get
		{
			return ޖ;
		}
		set
		{
			if (ޖ != value)
			{
				ޖ = value;
				ߓ();
			}
		}
	}

	[Browsable(false)]
	public int MarginHorizontal => ޜ + ޝ;

	[Browsable(false)]
	public int MarginVertical => ޞ + ޟ;

	[Browsable(true)]
	[Category("Margins")]
	[DefaultValue(0)]
	[Description("Specifies left margin.")]
	[DevCoSerialize]
	public int MarginLeft
	{
		get
		{
			return ޜ;
		}
		set
		{
			ޜ = value;
			ߓ();
			ߔ();
		}
	}

	[DefaultValue(0)]
	[Browsable(true)]
	[Description("Specifies right margin.")]
	[Category("Margins")]
	[DevCoSerialize]
	public int MarginRight
	{
		get
		{
			return ޝ;
		}
		set
		{
			ޝ = value;
			ߓ();
			ߔ();
		}
	}

	[DefaultValue(0)]
	[Category("Margins")]
	[Browsable(true)]
	[Description("Specifies top margin.")]
	[DevCoSerialize]
	public int MarginTop
	{
		get
		{
			return ޞ;
		}
		set
		{
			ޞ = value;
			ߓ();
			ߔ();
		}
	}

	[DevCoSerialize]
	[Description("Specifies bottom margin.")]
	[Browsable(true)]
	[Category("Margins")]
	[DefaultValue(0)]
	public int MarginBottom
	{
		get
		{
			return ޟ;
		}
		set
		{
			ޟ = value;
			ߓ();
			ߔ();
		}
	}

	internal bool ݺ
	{
		get
		{
			return ӱ;
		}
		set
		{
			ӱ = value;
		}
	}

	[Browsable(false)]
	public Size Size => _߄;

	[Description("Indicates the border type for all sides of the element.")]
	[DefaultValue(eStyleBorderType.None)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(true)]
	[Category("Border")]
	public eStyleBorderType Border
	{
		get
		{
			return _07A6;
		}
		set
		{
			if (ޅ)
			{
				TypeDescriptor.GetProperties(this)["BorderLeft"].SetValue(this, value);
				TypeDescriptor.GetProperties(this)["BorderRight"].SetValue(this, value);
				TypeDescriptor.GetProperties(this)["BorderTop"].SetValue(this, value);
				TypeDescriptor.GetProperties(this)["BorderBottom"].SetValue(this, value);
			}
			else
			{
				BorderLeft = value;
				BorderRight = value;
				BorderTop = value;
				BorderBottom = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates border width in pixels.")]
	[Category("Border")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int BorderWidth
	{
		get
		{
			return _07BC;
		}
		set
		{
			if (ޅ)
			{
				TypeDescriptor.GetProperties(this)["BorderLeftWidth"].SetValue(this, value);
				TypeDescriptor.GetProperties(this)["BorderRightWidth"].SetValue(this, value);
				TypeDescriptor.GetProperties(this)["BorderTopWidth"].SetValue(this, value);
				TypeDescriptor.GetProperties(this)["BorderBottomWidth"].SetValue(this, value);
			}
			else
			{
				BorderLeftWidth = value;
				BorderRightWidth = value;
				BorderTopWidth = value;
				BorderBottomWidth = value;
			}
			ߓ();
			ߔ();
		}
	}

	[DevCoSerialize]
	[Description("Indicates the border type for top side of the element.")]
	[DefaultValue(eStyleBorderType.None)]
	[Category("Border")]
	[Browsable(true)]
	public eStyleBorderType BorderTop
	{
		get
		{
			return _07A6;
		}
		set
		{
			if (_07A6 != value)
			{
				_07A6 = value;
				ߓ();
				ߔ();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates the border type for bottom side of the element.")]
	[Category("Border")]
	[DefaultValue(eStyleBorderType.None)]
	[DevCoSerialize]
	public eStyleBorderType BorderBottom
	{
		get
		{
			return _07A7;
		}
		set
		{
			if (_07A7 != value)
			{
				_07A7 = value;
				ߓ();
				ߔ();
			}
		}
	}

	[Description("Indicates the border type for left side of the element.")]
	[Category("Border")]
	[DevCoSerialize]
	[DefaultValue(eStyleBorderType.None)]
	[Browsable(true)]
	public eStyleBorderType BorderLeft
	{
		get
		{
			return ޤ;
		}
		set
		{
			if (ޤ != value)
			{
				ޤ = value;
				ߓ();
				ߔ();
			}
		}
	}

	[Browsable(true)]
	[Category("Border")]
	[DefaultValue(eStyleBorderType.None)]
	[Description("Indicates the border type for right side of the element.")]
	[DevCoSerialize]
	public eStyleBorderType BorderRight
	{
		get
		{
			return ޥ;
		}
		set
		{
			if (ޥ != value)
			{
				ޥ = value;
				ߓ();
				ߔ();
			}
		}
	}

	[Category("Border")]
	[DefaultValue(0)]
	[DevCoSerialize]
	[Browsable(true)]
	[Description("Indicates border width in pixels.")]
	public int BorderTopWidth
	{
		get
		{
			return _07BC;
		}
		set
		{
			if (_07BC != value)
			{
				_07BC = value;
				ߓ();
				ߔ();
			}
		}
	}

	[Category("Border")]
	[DefaultValue(0)]
	[Description("Indicates border width in pixels.")]
	[DevCoSerialize]
	[Browsable(true)]
	public int BorderBottomWidth
	{
		get
		{
			return _07BD;
		}
		set
		{
			if (_07BD != value)
			{
				_07BD = value;
				ߓ();
				ߔ();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Border")]
	[DefaultValue(0)]
	[Description("Indicates border width in pixels.")]
	public int BorderLeftWidth
	{
		get
		{
			return _07BA;
		}
		set
		{
			if (_07BA != value)
			{
				_07BA = value;
				ߓ();
				ߔ();
			}
		}
	}

	[Category("Border")]
	[Browsable(true)]
	[Description("Indicates border width in pixels.")]
	[DefaultValue(0)]
	[DevCoSerialize]
	public int BorderRightWidth
	{
		get
		{
			return _07BB;
		}
		set
		{
			if (_07BB != value)
			{
				_07BB = value;
				ߓ();
				ߔ();
			}
		}
	}

	[DevCoSerialize]
	[TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[Browsable(true)]
	[Description("Indicates border color for all sides. Specifing the color for the side will override this value.")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Category("Border")]
	public Color BorderColor
	{
		get
		{
			return _065A(_07A8, _07A9);
		}
		set
		{
			if (value != BorderColor && ((_07A9 != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderColorSchemePart = eColorSchemePart.None;
			}
			_07A8 = value;
			ߓ();
		}
	}

	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	[DevCoSerialize]
	public eColorSchemePart BorderColorSchemePart
	{
		get
		{
			return _07A9;
		}
		set
		{
			_07A9 = value;
			ߓ();
		}
	}

	[Category("Border")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates target background gradient color for border on all sides. ")]
	[DevCoSerialize]
	[Browsable(true)]
	[TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public Color BorderColor2
	{
		get
		{
			return _065A(_07AA, _07AB);
		}
		set
		{
			if (value != BorderColor2 && ((_07AB != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderColor2SchemePart = eColorSchemePart.None;
			}
			_07AA = value;
			ߓ();
		}
	}

	[DefaultValue(eColorSchemePart.None)]
	[DevCoSerialize]
	[Browsable(false)]
	public eColorSchemePart BorderColor2SchemePart
	{
		get
		{
			return _07AB;
		}
		set
		{
			_07AB = value;
			ߓ();
		}
	}

	[Description("Gets or sets the border gradient angle.")]
	[DefaultValue(90)]
	[Category("Background")]
	[DevCoSerialize]
	[Browsable(true)]
	public int BorderGradientAngle
	{
		get
		{
			return _07AC;
		}
		set
		{
			if (_07AC != value)
			{
				_07AC = value;
				ߓ();
			}
		}
	}

	[Category("Border")]
	[TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[Browsable(true)]
	[DevCoSerialize]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates color for light border part when etched border is used.")]
	public Color BorderColorLight
	{
		get
		{
			return _065A(_07AD, _07AE);
		}
		set
		{
			if (value != BorderColorLight && ((_07AE != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderColorLightSchemePart = eColorSchemePart.None;
			}
			_07AD = value;
			ߓ();
		}
	}

	[DevCoSerialize]
	[DefaultValue(eColorSchemePart.None)]
	[Browsable(false)]
	public eColorSchemePart BorderColorLightSchemePart
	{
		get
		{
			return _07AE;
		}
		set
		{
			_07AE = value;
			ߓ();
		}
	}

	[TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[Description("Indicates target background gradient color for border on all sides. ")]
	[DevCoSerialize]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Category("Border")]
	[Browsable(true)]
	public Color BorderColorLight2
	{
		get
		{
			return _065A(_07AF, _07B0);
		}
		set
		{
			if (value != BorderColorLight2 && ((_07B0 != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderColorLight2SchemePart = eColorSchemePart.None;
			}
			_07AF = value;
			ߓ();
		}
	}

	[DefaultValue(eColorSchemePart.None)]
	[Browsable(false)]
	[DevCoSerialize]
	public eColorSchemePart BorderColorLight2SchemePart
	{
		get
		{
			return _07B0;
		}
		set
		{
			_07B0 = value;
			ߓ();
		}
	}

	[DefaultValue(90)]
	[Description("Gets or sets the border gradient angle.")]
	[Browsable(true)]
	[Category("Background")]
	[DevCoSerialize]
	public int BorderLightGradientAngle
	{
		get
		{
			return ޱ;
		}
		set
		{
			if (ޱ != value)
			{
				ޱ = value;
				ߓ();
			}
		}
	}

	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates background color for the left side border.")]
	[DevCoSerialize]
	[Category("Border")]
	[Browsable(true)]
	[TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public Color BorderLeftColor
	{
		get
		{
			return _065A(_07B2, _07B3);
		}
		set
		{
			if (value != BorderLeftColor && ((_07B3 != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderLeftColorSchemePart = eColorSchemePart.None;
			}
			_07B2 = value;
			ߓ();
		}
	}

	[DefaultValue(eColorSchemePart.None)]
	[Browsable(false)]
	[DevCoSerialize]
	public eColorSchemePart BorderLeftColorSchemePart
	{
		get
		{
			return _07B3;
		}
		set
		{
			_07B3 = value;
			ߓ();
		}
	}

	[TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[Category("Border")]
	[Description("Indicates background color for the right side border.")]
	[Browsable(true)]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[DevCoSerialize]
	public Color BorderRightColor
	{
		get
		{
			return _065A(_07B4, _07B5);
		}
		set
		{
			if (value != BorderRightColor && ((_07B5 != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderRightColorSchemePart = eColorSchemePart.None;
			}
			_07B4 = value;
			ߓ();
		}
	}

	[Browsable(false)]
	[DevCoSerialize]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BorderRightColorSchemePart
	{
		get
		{
			return _07B5;
		}
		set
		{
			_07B5 = value;
			ߓ();
		}
	}

	[Category("Border")]
	[TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[Description("Indicates background color for the top side border.")]
	[Browsable(true)]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[DevCoSerialize]
	public Color BorderTopColor
	{
		get
		{
			return _065A(_07B6, _07B7);
		}
		set
		{
			if (value != BorderTopColor && ((_07B7 != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderTopColorSchemePart = eColorSchemePart.None;
			}
			_07B6 = value;
			ߓ();
		}
	}

	[Browsable(false)]
	[DevCoSerialize]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BorderTopColorSchemePart
	{
		get
		{
			return _07B7;
		}
		set
		{
			_07B7 = value;
			ߓ();
		}
	}

	[Browsable(true)]
	[DevCoSerialize]
	[TypeConverter("DevComponents.DotNetBar.Design.ColorSchemeColorConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[Category("Border")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates background color for the bottom side border.")]
	public Color BorderBottomColor
	{
		get
		{
			return _065A(_07B8, _07B9);
		}
		set
		{
			if (value != BorderBottomColor && ((_07B9 != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderBottomColorSchemePart = eColorSchemePart.None;
			}
			_07B8 = value;
			ߓ();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BorderBottomColorSchemePart
	{
		get
		{
			return _07B9;
		}
		set
		{
			_07B9 = value;
			ߓ();
		}
	}

	[Browsable(false)]
	public int PaddingHorizontal => ޠ + ޡ;

	[Browsable(false)]
	public int PaddingVertical => ޢ + ޣ;

	[Browsable(true)]
	[Category("PaddingTop")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Indicates padding space in pixels for all 4 sides of the box.")]
	[DefaultValue(0)]
	public int Padding
	{
		get
		{
			return ޢ;
		}
		set
		{
			if (ޅ)
			{
				TypeDescriptor.GetProperties(this)["PaddingLeft"].SetValue(this, value);
				TypeDescriptor.GetProperties(this)["PaddingRight"].SetValue(this, value);
				TypeDescriptor.GetProperties(this)["PaddingTop"].SetValue(this, value);
				TypeDescriptor.GetProperties(this)["PaddingBottom"].SetValue(this, value);
			}
			else
			{
				PaddingLeft = value;
				PaddingRight = value;
				PaddingTop = value;
				PaddingBottom = value;
			}
			ߓ();
			ߔ();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Padding")]
	[DefaultValue(0)]
	[Description("Indicates the amount of space to insert between the top border of the element and the content.")]
	public int PaddingTop
	{
		get
		{
			return ޢ;
		}
		set
		{
			ޢ = value;
			ߓ();
			ߔ();
		}
	}

	[DefaultValue(0)]
	[Description("Indicates the amount of space to insert between the bottom border of the element and the content.")]
	[Browsable(true)]
	[DevCoSerialize]
	[Category("Padding")]
	public int PaddingBottom
	{
		get
		{
			return ޣ;
		}
		set
		{
			ޣ = value;
			ߓ();
			ߔ();
		}
	}

	[Category("Padding")]
	[Browsable(true)]
	[DefaultValue(0)]
	[DevCoSerialize]
	[Description("Indicates the amount of space to insert between the left border of the element and the content.")]
	public int PaddingLeft
	{
		get
		{
			return ޠ;
		}
		set
		{
			ޠ = value;
			ߓ();
			ߔ();
		}
	}

	[Category("Padding")]
	[Description("Indicates the amount of space to insert between the right border of the element and the content.")]
	[Browsable(true)]
	[DevCoSerialize]
	[DefaultValue(0)]
	public int PaddingRight
	{
		get
		{
			return ޡ;
		}
		set
		{
			ޡ = value;
			ߓ();
			ߔ();
		}
	}

	[Browsable(true)]
	[Description("Indicates name of the style.")]
	[DefaultValue("")]
	[DevCoSerialize]
	[Category("Design")]
	public string Name
	{
		get
		{
			if (_߇ != null)
			{
				Ӿ = _߇.Name;
			}
			return Ӿ;
		}
		set
		{
			if (_߇ != null)
			{
				_߇.Name = value;
			}
			if (value == null)
			{
				Ӿ = "";
			}
			else
			{
				Ӿ = value;
			}
		}
	}

	[DevCoSerialize]
	[DefaultValue("")]
	[Localizable(false)]
	[Description("Indicates the class style belongs to.")]
	[Category("Design")]
	[Editor("DevComponents.DotNetBar.Design.ElementStyleClassTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	public string Class
	{
		get
		{
			return ߊ;
		}
		set
		{
			if (value == null)
			{
				throw new NullReferenceException("null is not valid value for this property");
			}
			if (ߊ != value)
			{
				ߊ = value;
				ߓ();
			}
		}
	}

	[DefaultValue("")]
	[DevCoSerialize]
	[Browsable(true)]
	[Category("Design")]
	[Description("Indicates description of the style.")]
	public string Description
	{
		get
		{
			return _߅;
		}
		set
		{
			_߅ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual ISite Site
	{
		get
		{
			return _߇;
		}
		set
		{
			_߇ = value;
		}
	}

	[DefaultValue(0)]
	[Description("Indicates the maximum height of the element. This property should be used in conjunction with the WordWrap property to limit the size of text bounding box.")]
	[Browsable(true)]
	[Category("Text Formatting")]
	[DevCoSerialize]
	public int MaximumHeight
	{
		get
		{
			return _߆;
		}
		set
		{
			if (_߆ != value)
			{
				_߆ = value;
				ߓ();
				ߔ();
			}
		}
	}

	internal StringFormat ݻ
	{
		get
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			StringFormat val = new StringFormat(StringFormat.GenericDefault);
			val.Alignment = (StringAlignment)ޔ;
			val.LineAlignment = (StringAlignment)ޕ;
			val.Trimming = (StringTrimming)ޖ;
			if (ݥ)
			{
				val.FormatFlags = (StringFormatFlags)(val.FormatFlags & ~(val.FormatFlags & 0x1000));
			}
			else
			{
				val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 0x1000);
			}
			return val;
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	internal eOrientation ݼ
	{
		get
		{
			return ߌ;
		}
		set
		{
			if (ߌ != value)
			{
				ߌ = value;
				ߓ();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether the control interprets an ampersand character (&amp;) in the control's Text property to be an access key prefix character.")]
	[Category("Appearance")]
	public bool UseMnemonic
	{
		get
		{
			return _ߍ;
		}
		set
		{
			_ߍ = value;
			ߓ();
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether control hides the underline of the letter prefixed by ampersand character when UseMnemonic=true")]
	[Category("Appearance")]
	public bool HideMnemonic
	{
		get
		{
			return _ߎ;
		}
		set
		{
			_ߎ = value;
		}
	}

	internal eTextFormat ݽ
	{
		get
		{
			eTextFormat eTextFormat2 = eTextFormat.Default;
			if (ޔ == eStyleTextAlignment.Center)
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			else if (ޔ == eStyleTextAlignment.Far)
			{
				eTextFormat2 |= eTextFormat.Right;
			}
			if (ޕ == eStyleTextAlignment.Center)
			{
				eTextFormat2 |= eTextFormat.VerticalCenter;
			}
			else if (ޕ == eStyleTextAlignment.Far)
			{
				eTextFormat2 |= eTextFormat.Bottom;
			}
			eTextFormat2 = ((!ݥ) ? (eTextFormat2 | eTextFormat.SingleLine) : (eTextFormat2 | eTextFormat.WordBreak));
			if (ޖ != 0)
			{
				eTextFormat2 |= eTextFormat.EndEllipsis;
			}
			if (_ߎ)
			{
				eTextFormat2 |= eTextFormat.HidePrefix;
			}
			if (!_ߍ && !_ߎ)
			{
				eTextFormat2 |= eTextFormat.NoPrefix;
			}
			if (ߌ == eOrientation.Vertical)
			{
				eTextFormat2 |= eTextFormat.Vertical;
			}
			return eTextFormat2;
		}
	}

	[Browsable(true)]
	[DevCoSerialize]
	[Category("Border")]
	[Description("Indicates border corner type.")]
	public eCornerType CornerType
	{
		get
		{
			return _07BE;
		}
		set
		{
			if (_07BE != value)
			{
				_07BE = value;
				if (_07BE == eCornerType.Inherit)
				{
					_07BE = eCornerType.Square;
				}
				ߓ();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Border")]
	[Description("Indicates top left border corner type.")]
	[DefaultValue(eCornerType.Inherit)]
	public eCornerType CornerTypeTopLeft
	{
		get
		{
			return _07BF;
		}
		set
		{
			if (_07BF != value)
			{
				_07BF = value;
				ߓ();
			}
		}
	}

	[Category("Border")]
	[DefaultValue(eCornerType.Inherit)]
	[Description("Indicates top right border corner type.")]
	[Browsable(true)]
	[DevCoSerialize]
	public eCornerType CornerTypeTopRight
	{
		get
		{
			return _߀;
		}
		set
		{
			if (_߀ != value)
			{
				_߀ = value;
				ߓ();
			}
		}
	}

	[Category("Border")]
	[DefaultValue(eCornerType.Inherit)]
	[DevCoSerialize]
	[Description("Indicates bottom left border corner type.")]
	[Browsable(true)]
	public eCornerType CornerTypeBottomLeft
	{
		get
		{
			return _߁;
		}
		set
		{
			if (_߁ != value)
			{
				_߁ = value;
				ߓ();
			}
		}
	}

	[Description("Indicates bottom right border corner type.")]
	[DefaultValue(eCornerType.Inherit)]
	[DevCoSerialize]
	[Category("Border")]
	[Browsable(true)]
	public eCornerType CornerTypeBottomRight
	{
		get
		{
			return _߂;
		}
		set
		{
			if (_߂ != value)
			{
				_߂ = value;
				ߓ();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(8)]
	[Description("Indicates diameter in pixels of the corner type rounded or diagonal.")]
	[DevCoSerialize]
	[Category("Border")]
	public int CornerDiameter
	{
		get
		{
			return _߃;
		}
		set
		{
			if (_߃ != value)
			{
				_߃ = value;
				ߓ();
			}
		}
	}

	internal bool ݾ
	{
		get
		{
			if (BorderLeft != 0 && BorderLeftWidth > 0 && (!BorderColor.IsEmpty || !BorderLeftColor.IsEmpty || BorderColorSchemePart != eColorSchemePart.None || BorderLeftColorSchemePart != eColorSchemePart.None))
			{
				return true;
			}
			return false;
		}
	}

	internal bool ݿ
	{
		get
		{
			if (BorderRight != 0 && BorderRightWidth > 0 && (!BorderColor.IsEmpty || !BorderRightColor.IsEmpty || BorderColorSchemePart != eColorSchemePart.None || BorderRightColorSchemePart != eColorSchemePart.None))
			{
				return true;
			}
			return false;
		}
	}

	internal bool ހ
	{
		get
		{
			if (BorderTop != 0 && BorderTopWidth > 0 && (!BorderColor.IsEmpty || !BorderTopColor.IsEmpty || BorderColorSchemePart != eColorSchemePart.None || BorderTopColorSchemePart != eColorSchemePart.None))
			{
				return true;
			}
			return false;
		}
	}

	internal bool ށ
	{
		get
		{
			if (BorderBottom != 0 && BorderBottomWidth > 0 && (!BorderColor.IsEmpty || !BorderBottomColor.IsEmpty || BorderColorSchemePart != eColorSchemePart.None || BorderBottomColorSchemePart != eColorSchemePart.None))
			{
				return true;
			}
			return false;
		}
	}

	internal bool ނ => ހ | ށ | ݾ | ݿ;

	internal bool ރ
	{
		get
		{
			if (ހ && ށ && ݾ)
			{
				return ݿ;
			}
			return false;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual bool Custom
	{
		get
		{
			if (!BackColor.IsEmpty)
			{
				return true;
			}
			if (!BackColor2.IsEmpty)
			{
				return true;
			}
			if (BackColorBlend.Count > 0)
			{
				return true;
			}
			if (BackColorGradientAngle != 0)
			{
				return true;
			}
			if (BackColorGradientType != 0)
			{
				return true;
			}
			if (BackgroundImage != null)
			{
				return true;
			}
			if (BackgroundImageAlpha != byte.MaxValue)
			{
				return true;
			}
			if (BackgroundImagePosition != 0)
			{
				return true;
			}
			if (BorderBottom != 0)
			{
				return true;
			}
			if (!BorderBottomColor.IsEmpty)
			{
				return true;
			}
			if (BorderBottomWidth != 0)
			{
				return true;
			}
			if (!BorderColor.IsEmpty)
			{
				return true;
			}
			if (BorderLeft != 0)
			{
				return true;
			}
			if (!BorderLeftColor.IsEmpty)
			{
				return true;
			}
			if (BorderLeftWidth != 0)
			{
				return true;
			}
			if (BorderTop != 0)
			{
				return true;
			}
			if (!BorderTopColor.IsEmpty)
			{
				return true;
			}
			if (BorderTopWidth != 0)
			{
				return true;
			}
			if (BorderRight != 0)
			{
				return true;
			}
			if (!BorderRightColor.IsEmpty)
			{
				return true;
			}
			if (BorderRightWidth != 0)
			{
				return true;
			}
			if (CornerDiameter != 8)
			{
				return true;
			}
			if (CornerType != eCornerType.Square)
			{
				return true;
			}
			if (CornerTypeTopLeft != 0)
			{
				return true;
			}
			if (CornerTypeTopRight != 0)
			{
				return true;
			}
			if (CornerTypeBottomLeft != 0)
			{
				return true;
			}
			if (CornerTypeBottomRight != 0)
			{
				return true;
			}
			if (Font != null)
			{
				return true;
			}
			if (MarginBottom != 0)
			{
				return true;
			}
			if (MarginLeft != 0)
			{
				return true;
			}
			if (MarginRight != 0)
			{
				return true;
			}
			if (MarginTop != 0)
			{
				return true;
			}
			if (MaximumHeight != 0)
			{
				return true;
			}
			if (PaddingBottom != 0)
			{
				return true;
			}
			if (PaddingLeft != 0)
			{
				return true;
			}
			if (PaddingRight != 0)
			{
				return true;
			}
			if (PaddingTop != 0)
			{
				return true;
			}
			if (TextAlignment != 0)
			{
				return true;
			}
			if (!TextColor.IsEmpty)
			{
				return true;
			}
			if (TextLineAlignment != eStyleTextAlignment.Center)
			{
				return true;
			}
			if (TextTrimming != eStyleTextTrimming.EllipsisCharacter)
			{
				return true;
			}
			if (WordWrap)
			{
				return true;
			}
			if (!TextShadowColor.IsEmpty)
			{
				return true;
			}
			if (!TextShadowOffset.IsEmpty)
			{
				return true;
			}
			if (MaximumWidth != 0)
			{
				return true;
			}
			if (!string.IsNullOrEmpty(Class))
			{
				return true;
			}
			return false;
		}
	}

	internal bool ބ
	{
		get
		{
			return ߋ;
		}
		set
		{
			ߋ = value;
		}
	}

	internal bool ޅ
	{
		get
		{
			return _߈;
		}
		set
		{
			_߈ = value;
		}
	}

	[Category("Text Formatting")]
	[DefaultValue(0)]
	[DevCoSerialize]
	[Browsable(true)]
	[Description("Indicates the maximum width of the element. This property should be used in conjunction with the WordWrap property to limit the size of text bounding box.")]
	public int MaximumWidth
	{
		get
		{
			return ߐ;
		}
		set
		{
			if (ߐ != value)
			{
				ߐ = value;
				ߓ();
				ߔ();
			}
		}
	}

	internal ElementStyleCollection ކ
	{
		get
		{
			return ӭ;
		}
		set
		{
			ӭ = value;
		}
	}

	internal DevComponents.AdvTree.AdvTree Ԙ
	{
		get
		{
			return Ԛ;
		}
		set
		{
			Ԛ = value;
		}
	}

	public event EventHandler StyleChanged
	{
		add
		{
			EventHandler eventHandler = ވ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ވ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ވ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ވ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when component is Disposed.")]
	public event EventHandler Disposed
	{
		add
		{
			EventHandler eventHandler = މ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref މ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = މ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref މ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public ElementStyle()
	{
		_߉.ކ = this;
	}

	public ElementStyle(string styleClass)
	{
		ߊ = styleClass;
	}

	public ElementStyle(Color textColor)
	{
		_߉.ކ = this;
		TextColor = textColor;
	}

	public ElementStyle(Color textColor, Color backColor)
	{
		_߉.ކ = this;
		TextColor = textColor;
		BackColor = backColor;
	}

	public ElementStyle(Color textColor, Color backColor1, Color backColor2)
	{
		_߉.ކ = this;
		TextColor = textColor;
		BackColor = backColor1;
		BackColor2 = backColor2;
		BackColorGradientAngle = 90;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		if (!ފ.IsEmpty)
		{
			return ދ == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor()
	{
		ފ = Color.Empty;
		ߓ();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor2()
	{
		if (!ތ.IsEmpty)
		{
			return ލ == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor2()
	{
		ތ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundImage()
	{
		ޏ = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		if (!ޗ.IsEmpty)
		{
			return ޘ == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		ޗ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextShadowColor()
	{
		if (!ޙ.IsEmpty)
		{
			return ޚ == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextShadowColor()
	{
		ޙ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextShadowOffset()
	{
		return !ޛ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextShadowOffset()
	{
		ޛ = Point.Empty;
	}

	internal void ߑ(Size ߒ)
	{
		_߄ = ߒ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		if (!_07A8.IsEmpty)
		{
			return _07A9 == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		_07A8 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor2()
	{
		if (!_07AA.IsEmpty)
		{
			return _07AB == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetBorderColor2()
	{
		_07AA = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColorLight()
	{
		if (!_07AD.IsEmpty)
		{
			return _07AE == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetBorderColorLight()
	{
		_07AD = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColorLight2()
	{
		if (!_07AF.IsEmpty)
		{
			return _07B0 == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetBorderColorLight2()
	{
		_07AF = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderLeftColor()
	{
		if (!_07B2.IsEmpty)
		{
			return _07B3 == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderLeftColor()
	{
		_07B2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderRightColor()
	{
		if (!_07B4.IsEmpty)
		{
			return _07B5 == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderRightColor()
	{
		_07B4 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderTopColor()
	{
		if (!_07B6.IsEmpty)
		{
			return _07B7 == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderTopColor()
	{
		_07B6 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderBottomColor()
	{
		if (!_07B8.IsEmpty)
		{
			return _07B9 == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderBottomColor()
	{
		_07B8 = Color.Empty;
	}

	public virtual void Dispose()
	{
		if (މ != null)
		{
			މ(this, EventArgs.Empty);
		}
		if ((BarUtilities.DisposeItemImages && !ޅ) || _ߏ)
		{
			BarUtilities.ᆟ(ref ޏ);
		}
		_߉.ކ = null;
	}

	public void ApplyFontStyle(ElementStyle style)
	{
		if (style.Font != null)
		{
			Font = style.Font;
		}
		if (style.MarginBottom != 0)
		{
			MarginBottom = style.MarginBottom;
		}
		if (style.MarginLeft != 0)
		{
			MarginLeft = style.MarginLeft;
		}
		if (style.MarginRight != 0)
		{
			MarginRight = style.MarginRight;
		}
		if (style.MarginTop != 0)
		{
			MarginTop = style.MarginTop;
		}
		if (style.MaximumHeight != 0)
		{
			MaximumHeight = style.MaximumHeight;
		}
		if (style.MaximumWidth != 0)
		{
			MaximumWidth = style.MaximumWidth;
		}
		if (style.PaddingBottom != 0)
		{
			PaddingBottom = style.PaddingBottom;
		}
		if (style.PaddingLeft != 0)
		{
			PaddingLeft = style.PaddingLeft;
		}
		if (style.PaddingRight != 0)
		{
			PaddingRight = style.PaddingRight;
		}
		if (style.PaddingTop != 0)
		{
			PaddingTop = style.PaddingTop;
		}
		if (style.TextAlignment != TextAlignment)
		{
			TextAlignment = style.TextAlignment;
		}
		if (!style.TextColor.IsEmpty)
		{
			TextColor = style.TextColor;
		}
		else if (style.TextColorSchemePart != eColorSchemePart.None)
		{
			TextColorSchemePart = style.TextColorSchemePart;
		}
		if (style.TextLineAlignment != TextLineAlignment)
		{
			TextLineAlignment = style.TextLineAlignment;
		}
		if (style.TextTrimming == eStyleTextTrimming.EllipsisCharacter)
		{
			TextTrimming = style.TextTrimming;
		}
		if (style.WordWrap)
		{
			WordWrap = style.WordWrap;
		}
		if (!style.TextShadowColor.IsEmpty)
		{
			TextShadowColor = style.TextShadowColor;
		}
		if (!style.TextShadowOffset.IsEmpty)
		{
			TextShadowOffset = style.TextShadowOffset;
		}
	}

	public virtual void ApplyStyle(ElementStyle style)
	{
		if (!style.BackColor.IsEmpty)
		{
			BackColor = style.BackColor;
		}
		else if (style.BackColorSchemePart != eColorSchemePart.None)
		{
			BackColorSchemePart = style.BackColorSchemePart;
		}
		if (!style.BackColor2.IsEmpty)
		{
			BackColor2 = style.BackColor2;
		}
		else if (style.BackColor2SchemePart != eColorSchemePart.None)
		{
			BackColor2SchemePart = style.BackColor2SchemePart;
		}
		if (style.BackColorGradientAngle != 0)
		{
			BackColorGradientAngle = style.BackColorGradientAngle;
		}
		BackColorGradientType = style.BackColorGradientType;
		if (style.BackgroundImage != null)
		{
			BackgroundImage = style.BackgroundImage;
		}
		if (style.BackgroundImageAlpha != byte.MaxValue)
		{
			BackgroundImageAlpha = style.BackgroundImageAlpha;
		}
		if (style.BackgroundImagePosition != 0)
		{
			BackgroundImagePosition = style.BackgroundImagePosition;
		}
		if (style.BorderBottom != 0)
		{
			BorderBottom = style.BorderBottom;
		}
		if (!style.BorderBottomColor.IsEmpty)
		{
			BorderBottomColor = style.BorderBottomColor;
		}
		else if (style.BorderBottomColorSchemePart != eColorSchemePart.None)
		{
			BorderBottomColorSchemePart = style.BorderBottomColorSchemePart;
		}
		if (style.BorderBottomWidth != 0)
		{
			BorderBottomWidth = style.BorderBottomWidth;
		}
		if (!style.BorderColor.IsEmpty)
		{
			BorderColor = style.BorderColor;
		}
		else if (style.BorderColorSchemePart != eColorSchemePart.None)
		{
			BorderColorSchemePart = style.BorderColorSchemePart;
		}
		if (!style.BorderColor2.IsEmpty || !style.BorderColor.IsEmpty)
		{
			BorderColor2 = style.BorderColor2;
		}
		else if (style.BorderColor2SchemePart != eColorSchemePart.None)
		{
			BorderColor2SchemePart = style.BorderColor2SchemePart;
		}
		if (style.BorderGradientAngle != 90)
		{
			BorderGradientAngle = style.BorderGradientAngle;
		}
		if (!style.BorderColorLight.IsEmpty)
		{
			BorderColorLight = style.BorderColorLight;
		}
		else if (style.BorderColorLightSchemePart != eColorSchemePart.None)
		{
			BorderColorLightSchemePart = style.BorderColorLightSchemePart;
		}
		if (!style.BorderColorLight2.IsEmpty || !style.BorderColorLight.IsEmpty)
		{
			BorderColorLight2 = style.BorderColorLight2;
		}
		else if (style.BorderColorLight2SchemePart != eColorSchemePart.None)
		{
			BorderColorLight2SchemePart = style.BorderColorLight2SchemePart;
		}
		if (style.BorderLightGradientAngle != 90)
		{
			BorderLightGradientAngle = style.BorderLightGradientAngle;
		}
		if (style.BorderLeft != 0)
		{
			BorderLeft = style.BorderLeft;
		}
		if (!style.BorderLeftColor.IsEmpty)
		{
			BorderLeftColor = style.BorderLeftColor;
		}
		else if (style.BorderLeftColorSchemePart != eColorSchemePart.None)
		{
			BorderLeftColorSchemePart = style.BorderLeftColorSchemePart;
		}
		if (style.BorderLeftWidth != 0)
		{
			BorderLeftWidth = style.BorderLeftWidth;
		}
		if (style.BorderTop != 0)
		{
			BorderTop = style.BorderTop;
		}
		else if (style.BorderTopColorSchemePart != eColorSchemePart.None)
		{
			BorderTopColorSchemePart = style.BorderTopColorSchemePart;
		}
		if (!style.BorderTopColor.IsEmpty)
		{
			BorderTopColor = style.BorderTopColor;
		}
		if (style.BorderTopWidth != 0)
		{
			BorderTopWidth = style.BorderTopWidth;
		}
		if (style.BorderRight != 0)
		{
			BorderRight = style.BorderRight;
		}
		else if (style.BorderRightColorSchemePart != eColorSchemePart.None)
		{
			BorderRightColorSchemePart = style.BorderRightColorSchemePart;
		}
		if (!style.BorderRightColor.IsEmpty)
		{
			BorderRightColor = style.BorderRightColor;
		}
		if (style.BorderRightWidth != 0)
		{
			BorderRightWidth = style.BorderRightWidth;
		}
		if (style.CornerDiameter != 8)
		{
			CornerDiameter = style.CornerDiameter;
		}
		if (style.CornerType != eCornerType.Square)
		{
			CornerType = style.CornerType;
		}
		if (style.CornerTypeTopLeft != 0)
		{
			CornerTypeTopLeft = style.CornerTypeTopLeft;
		}
		if (style.CornerTypeTopRight != 0)
		{
			CornerTypeTopRight = style.CornerTypeTopRight;
		}
		if (style.CornerTypeBottomLeft != 0)
		{
			CornerTypeBottomLeft = style.CornerTypeBottomLeft;
		}
		if (style.CornerTypeBottomRight != 0)
		{
			CornerTypeBottomRight = style.CornerTypeBottomRight;
		}
		if (style.Font != null)
		{
			Font = style.Font;
		}
		if (style.MarginBottom != 0)
		{
			MarginBottom = style.MarginBottom;
		}
		if (style.MarginLeft != 0)
		{
			MarginLeft = style.MarginLeft;
		}
		if (style.MarginRight != 0)
		{
			MarginRight = style.MarginRight;
		}
		if (style.MarginTop != 0)
		{
			MarginTop = style.MarginTop;
		}
		if (style.MaximumWidth != 0)
		{
			MaximumWidth = style.MaximumWidth;
		}
		if (style.MaximumHeight != 0)
		{
			MaximumHeight = style.MaximumHeight;
		}
		if (style.PaddingBottom != 0)
		{
			PaddingBottom = style.PaddingBottom;
		}
		if (style.PaddingLeft != 0)
		{
			PaddingLeft = style.PaddingLeft;
		}
		if (style.PaddingRight != 0)
		{
			PaddingRight = style.PaddingRight;
		}
		if (style.PaddingTop != 0)
		{
			PaddingTop = style.PaddingTop;
		}
		if (style.TextAlignment != TextAlignment)
		{
			TextAlignment = style.TextAlignment;
		}
		if (!style.TextColor.IsEmpty)
		{
			TextColor = style.TextColor;
		}
		else if (style.TextColorSchemePart != eColorSchemePart.None)
		{
			TextColorSchemePart = style.TextColorSchemePart;
		}
		if (style.TextLineAlignment != TextLineAlignment)
		{
			TextLineAlignment = style.TextLineAlignment;
		}
		if (style.TextTrimming != eStyleTextTrimming.EllipsisCharacter)
		{
			TextTrimming = style.TextTrimming;
		}
		if (style.WordWrap)
		{
			WordWrap = style.WordWrap;
		}
		if (!style.TextShadowColor.IsEmpty)
		{
			TextShadowColor = style.TextShadowColor;
		}
		if (!style.TextShadowOffset.IsEmpty)
		{
			TextShadowOffset = style.TextShadowOffset;
		}
		if (style.UseMnemonic)
		{
			UseMnemonic = style.UseMnemonic;
		}
		if (style.HideMnemonic)
		{
			HideMnemonic = style.HideMnemonic;
		}
		if (style.BackColorBlend.Count > 0)
		{
			BackColorBlend.Clear();
			foreach (BackgroundColorBlend item in style.BackColorBlend)
			{
				BackColorBlend.Add(new BackgroundColorBlend(item.Color, item.Position));
			}
		}
		else if (!style.BackColor.IsEmpty || !style.BackColor2.IsEmpty)
		{
			BackColorBlend.Clear();
		}
		if (style.GetColorScheme() != null)
		{
			SetColorScheme(style.GetColorScheme());
		}
	}

	public ElementStyle Copy()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.BackColor = BackColor;
		elementStyle.BackColorSchemePart = BackColorSchemePart;
		elementStyle.BackColor2 = BackColor2;
		elementStyle.BackColor2SchemePart = BackColor2SchemePart;
		elementStyle.BackColorGradientAngle = BackColorGradientAngle;
		elementStyle.BackColorGradientType = BackColorGradientType;
		if (BackgroundImage != null)
		{
			elementStyle.BackgroundImage = (Image)BackgroundImage.Clone();
			elementStyle._ߏ = true;
		}
		elementStyle.BackgroundImageAlpha = BackgroundImageAlpha;
		elementStyle.BackgroundImagePosition = BackgroundImagePosition;
		elementStyle.BorderColor = BorderColor;
		elementStyle.BorderColorSchemePart = BorderColorSchemePart;
		elementStyle.BorderColor2 = BorderColor2;
		elementStyle.BorderColor2SchemePart = BorderColor2SchemePart;
		elementStyle.BorderGradientAngle = BorderGradientAngle;
		elementStyle.BorderColorLight = BorderColorLight;
		elementStyle.BorderColorLightSchemePart = BorderColorLightSchemePart;
		elementStyle.BorderColorLight2 = BorderColorLight2;
		elementStyle.BorderColorLight2SchemePart = BorderColorLight2SchemePart;
		elementStyle.BorderLightGradientAngle = BorderLightGradientAngle;
		elementStyle.BorderBottom = BorderBottom;
		elementStyle.BorderBottomColor = BorderBottomColor;
		elementStyle.BorderBottomColorSchemePart = BorderBottomColorSchemePart;
		elementStyle.BorderBottomWidth = BorderBottomWidth;
		elementStyle.BorderLeft = BorderLeft;
		elementStyle.BorderLeftWidth = BorderLeftWidth;
		elementStyle.BorderLeftColor = BorderLeftColor;
		elementStyle.BorderLeftColorSchemePart = BorderLeftColorSchemePart;
		elementStyle.BorderRight = BorderRight;
		elementStyle.BorderRightWidth = BorderRightWidth;
		elementStyle.BorderRightColor = BorderRightColor;
		elementStyle.BorderRightColorSchemePart = BorderRightColorSchemePart;
		elementStyle.BorderTop = BorderTop;
		elementStyle.BorderTopWidth = BorderTopWidth;
		elementStyle.BorderBottomColor = BorderBottomColor;
		elementStyle.BorderBottomColorSchemePart = BorderBottomColorSchemePart;
		elementStyle.CornerDiameter = CornerDiameter;
		elementStyle.CornerType = CornerType;
		elementStyle.CornerTypeTopLeft = CornerTypeTopLeft;
		elementStyle.CornerTypeTopRight = CornerTypeTopRight;
		elementStyle.CornerTypeBottomLeft = CornerTypeBottomLeft;
		elementStyle.CornerTypeBottomRight = CornerTypeBottomRight;
		elementStyle.Font = Font;
		elementStyle.MarginBottom = MarginBottom;
		elementStyle.MarginLeft = MarginLeft;
		elementStyle.MarginRight = MarginRight;
		elementStyle.MarginTop = MarginTop;
		elementStyle.PaddingBottom = PaddingBottom;
		elementStyle.PaddingLeft = PaddingLeft;
		elementStyle.PaddingRight = PaddingRight;
		elementStyle.PaddingTop = PaddingTop;
		elementStyle.TextAlignment = TextAlignment;
		elementStyle.TextColor = TextColor;
		elementStyle.TextColorSchemePart = TextColorSchemePart;
		elementStyle.TextLineAlignment = TextLineAlignment;
		elementStyle.TextTrimming = TextTrimming;
		elementStyle.WordWrap = WordWrap;
		elementStyle.Description = Description;
		elementStyle.TextShadowColor = TextShadowColor;
		elementStyle.TextShadowColorSchemePart = TextShadowColorSchemePart;
		elementStyle.TextShadowOffset = TextShadowOffset;
		elementStyle.Class = ߊ;
		elementStyle.UseMnemonic = _ߍ;
		elementStyle.HideMnemonic = _ߎ;
		elementStyle.MaximumWidth = MaximumWidth;
		elementStyle.MaximumHeight = MaximumHeight;
		foreach (BackgroundColorBlend item in _߉)
		{
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(item.Color, item.Position));
		}
		return elementStyle;
	}

	public void Reset()
	{
		ފ = Color.Empty;
		ދ = eColorSchemePart.None;
		ތ = Color.Empty;
		ލ = eColorSchemePart.None;
		ގ = 0;
		ޒ = eGradientType.Linear;
		ޏ = null;
		ސ = eStyleBackgroundImage.Stretch;
		ޑ = byte.MaxValue;
		_߉.Clear();
		ޓ = null;
		ݥ = false;
		ޔ = eStyleTextAlignment.Near;
		ޕ = eStyleTextAlignment.Center;
		ޖ = eStyleTextTrimming.EllipsisCharacter;
		ޗ = Color.Empty;
		ޘ = eColorSchemePart.None;
		ޙ = Color.Empty;
		ޚ = eColorSchemePart.None;
		ޛ = Point.Empty;
		ޜ = 0;
		ޝ = 0;
		ޞ = 0;
		ޟ = 0;
		ޠ = 0;
		ޡ = 0;
		ޢ = 0;
		ޣ = 0;
		ޤ = eStyleBorderType.None;
		ޥ = eStyleBorderType.None;
		_07A6 = eStyleBorderType.None;
		_07A7 = eStyleBorderType.None;
		_07A8 = Color.Empty;
		_07A9 = eColorSchemePart.None;
		_07AA = Color.Empty;
		_07AB = eColorSchemePart.None;
		_07AC = 90;
		_07AD = Color.Empty;
		_07AE = eColorSchemePart.None;
		_07AF = Color.Empty;
		_07B0 = eColorSchemePart.None;
		ޱ = 90;
		_07B2 = Color.Empty;
		_07B3 = eColorSchemePart.None;
		_07B4 = Color.Empty;
		_07B5 = eColorSchemePart.None;
		_07B6 = Color.Empty;
		_07B7 = eColorSchemePart.None;
		_07B8 = Color.Empty;
		_07B9 = eColorSchemePart.None;
		_07BA = 0;
		_07BB = 0;
		_07BC = 0;
		_07BD = 0;
		_07BE = eCornerType.Square;
		_07BF = eCornerType.Inherit;
		_߀ = eCornerType.Inherit;
		_߁ = eCornerType.Inherit;
		_߂ = eCornerType.Inherit;
		_߃ = 8;
		ߊ = "";
		_߆ = 0;
		ߐ = 0;
	}

	public static ElementStyle GetDefaultCellStyle(ElementStyle defaultNodeStyle)
	{
		ElementStyle elementStyle = new ElementStyle();
		if (defaultNodeStyle != null)
		{
			elementStyle.TextColor = defaultNodeStyle.TextColor;
			elementStyle.Font = defaultNodeStyle.Font;
			elementStyle.TextAlignment = defaultNodeStyle.TextAlignment;
			elementStyle.TextLineAlignment = defaultNodeStyle.TextLineAlignment;
			elementStyle.TextTrimming = defaultNodeStyle.TextTrimming;
			elementStyle.WordWrap = defaultNodeStyle.WordWrap;
		}
		if (elementStyle.TextColor.IsEmpty)
		{
			elementStyle.TextColor = SystemColors.ControlText;
		}
		return elementStyle;
	}

	public static ElementStyle GetDefaultDisabledCellStyle()
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.TextColor = SystemColors.GrayText;
		return elementStyle;
	}

	public static ElementStyle GetDefaultSelectedCellStyle()
	{
		return new ElementStyle();
	}

	public static void SetColorsAlpha(ElementStyle style, int alpha)
	{
		if (!style.TextColor.IsEmpty)
		{
			style.TextColor = Color.FromArgb(alpha, style.TextColor);
		}
		if (!style.BackColor.IsEmpty)
		{
			style.BackColor = Color.FromArgb(alpha, style.BackColor);
		}
		if (!style.BackColor2.IsEmpty)
		{
			style.BackColor2 = Color.FromArgb(alpha, style.BackColor2);
		}
		if (!style.BorderColor.IsEmpty)
		{
			style.BorderColor = Color.FromArgb(alpha, style.BorderColor);
		}
		if (!style.BorderBottomColor.IsEmpty)
		{
			style.BorderBottomColor = Color.FromArgb(alpha, style.BorderBottomColor);
		}
		if (!style.BorderLeftColor.IsEmpty)
		{
			style.BorderLeftColor = Color.FromArgb(alpha, style.BorderLeftColor);
		}
		if (!style.BorderRightColor.IsEmpty)
		{
			style.BorderRightColor = Color.FromArgb(alpha, style.BorderRightColor);
		}
		if (!style.BorderRightColor.IsEmpty)
		{
			style.BorderRightColor = Color.FromArgb(alpha, style.BorderRightColor);
		}
		if (!style.BorderTopColor.IsEmpty)
		{
			style.BorderTopColor = Color.FromArgb(alpha, style.BorderTopColor);
		}
	}

	private void ߓ()
	{
		if (!ߋ && ވ != null)
		{
			ވ(this, new EventArgs());
		}
	}

	private void ߔ()
	{
		ӱ = true;
	}

	private Color _065A(Color _065B, eColorSchemePart _065C)
	{
		if (_065C == eColorSchemePart.None)
		{
			return _065B;
		}
		ColorScheme colorScheme = GetColorScheme();
		if (colorScheme == null)
		{
			return _065B;
		}
		return _065C switch
		{
			eColorSchemePart.PanelBackground => colorScheme.PanelBackground, 
			eColorSchemePart.PanelBackground2 => colorScheme.PanelBackground2, 
			eColorSchemePart.PanelBorder => colorScheme.PanelBorder, 
			eColorSchemePart.PanelText => colorScheme.PanelText, 
			eColorSchemePart.BarBackground => colorScheme.BarBackground, 
			eColorSchemePart.BarBackground2 => colorScheme.BarBackground2, 
			eColorSchemePart.BarCaptionBackground => colorScheme.BarCaptionBackground, 
			eColorSchemePart.BarCaptionBackground2 => colorScheme.BarCaptionBackground2, 
			eColorSchemePart.BarCaptionInactiveBackground => colorScheme.BarCaptionInactiveBackground, 
			eColorSchemePart.BarCaptionInactiveBackground2 => colorScheme.BarCaptionInactiveBackground2, 
			eColorSchemePart.BarCaptionInactiveText => colorScheme.BarCaptionInactiveText, 
			eColorSchemePart.BarCaptionText => colorScheme.BarCaptionText, 
			eColorSchemePart.BarDockedBorder => colorScheme.BarDockedBorder, 
			eColorSchemePart.BarFloatingBorder => colorScheme.BarFloatingBorder, 
			eColorSchemePart.BarPopupBackground => colorScheme.BarPopupBackground, 
			eColorSchemePart.BarPopupBorder => colorScheme.BarPopupBorder, 
			eColorSchemePart.BarStripeColor => colorScheme.BarStripeColor, 
			eColorSchemePart.CustomizeBackground => colorScheme.CustomizeBackground, 
			eColorSchemePart.CustomizeBackground2 => colorScheme.CustomizeBackground2, 
			eColorSchemePart.CustomizeText => colorScheme.CustomizeText, 
			eColorSchemePart.DockSiteBackColor => colorScheme.DockSiteBackColor, 
			eColorSchemePart.DockSiteBackColor2 => colorScheme.DockSiteBackColor2, 
			eColorSchemePart.ExplorerBarBackground => colorScheme.ExplorerBarBackground, 
			eColorSchemePart.ExplorerBarBackground2 => colorScheme.ExplorerBarBackground2, 
			eColorSchemePart.ItemBackground => colorScheme.ItemBackground, 
			eColorSchemePart.ItemBackground2 => colorScheme.ItemBackground2, 
			eColorSchemePart.ItemCheckedBackground => colorScheme.ItemCheckedBackground, 
			eColorSchemePart.ItemCheckedBackground2 => colorScheme.ItemCheckedBackground2, 
			eColorSchemePart.ItemCheckedBorder => colorScheme.ItemCheckedBorder, 
			eColorSchemePart.ItemCheckedText => colorScheme.ItemCheckedText, 
			eColorSchemePart.ItemDesignTimeBorder => colorScheme.ItemDesignTimeBorder, 
			eColorSchemePart.ItemDisabledBackground => colorScheme.ItemDisabledBackground, 
			eColorSchemePart.ItemDisabledText => colorScheme.ItemDisabledText, 
			eColorSchemePart.ItemExpandedBackground => colorScheme.ItemExpandedBackground, 
			eColorSchemePart.ItemExpandedBackground2 => colorScheme.ItemExpandedBackground2, 
			eColorSchemePart.ItemExpandedBorder => colorScheme.ItemExpandedBorder, 
			eColorSchemePart.ItemExpandedShadow => colorScheme.ItemExpandedShadow, 
			eColorSchemePart.ItemExpandedText => colorScheme.ItemExpandedText, 
			eColorSchemePart.ItemHotBackground => colorScheme.ItemHotBackground, 
			eColorSchemePart.ItemHotBackground2 => colorScheme.ItemHotBackground2, 
			eColorSchemePart.ItemHotBorder => colorScheme.ItemHotBorder, 
			eColorSchemePart.ItemHotText => colorScheme.ItemHotText, 
			eColorSchemePart.ItemPressedBackground => colorScheme.ItemPressedBackground, 
			eColorSchemePart.ItemPressedBackground2 => colorScheme.ItemPressedBackground2, 
			eColorSchemePart.ItemPressedBorder => colorScheme.ItemPressedBorder, 
			eColorSchemePart.ItemPressedText => colorScheme.ItemPressedText, 
			eColorSchemePart.ItemSeparator => colorScheme.ItemSeparator, 
			eColorSchemePart.ItemSeparatorShade => colorScheme.ItemSeparatorShade, 
			eColorSchemePart.ItemText => colorScheme.ItemText, 
			eColorSchemePart.MenuBackground => colorScheme.MenuBackground, 
			eColorSchemePart.MenuBackground2 => colorScheme.MenuBackground2, 
			eColorSchemePart.MenuBarBackground => colorScheme.MenuBarBackground, 
			eColorSchemePart.MenuBarBackground2 => colorScheme.MenuBarBackground2, 
			eColorSchemePart.MenuBorder => colorScheme.MenuBorder, 
			eColorSchemePart.MenuSide => colorScheme.MenuSide, 
			eColorSchemePart.MenuSide2 => colorScheme.MenuSide2, 
			eColorSchemePart.MenuUnusedBackground => colorScheme.MenuUnusedBackground, 
			eColorSchemePart.MenuUnusedSide => colorScheme.MenuUnusedSide, 
			eColorSchemePart.MenuUnusedSide2 => colorScheme.MenuUnusedSide2, 
			_ => (Color)colorScheme.GetType().GetProperty(_065C.ToString()).GetValue(colorScheme, null), 
		};
	}

	public override string ToString()
	{
		string text = Name;
		if (Description != "")
		{
			text = text + " " + Description;
		}
		return text;
	}

	internal ColorScheme GetColorScheme()
	{
		if (ӭ != null && ӭ.Ԙ != null)
		{
			return ӭ.Ԙ.ԫ;
		}
		if (Ԛ != null)
		{
			return Ԛ.ԫ;
		}
		return լ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetColorScheme(ColorScheme cs)
	{
		լ = cs;
	}
}
