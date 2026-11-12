using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class ElementStyle : IComponent, IDisposable
{
	private const int DEFAULT_CORNER_DIAMETER = 8;

	private Color m_BackColor = Color.Empty;

	private eColorSchemePart m_BackColorSchemePart = eColorSchemePart.None;

	private Color m_BackColor2 = Color.Empty;

	private eColorSchemePart m_BackColor2SchemePart = eColorSchemePart.None;

	private int m_BackColorGradientAngle;

	private Image m_BackgroundImage;

	private eStyleBackgroundImage m_BackgroundImagePosition;

	private byte m_BackgroundImageAlpha = byte.MaxValue;

	private eGradientType m_BackColorGradientType;

	private Font m_Font;

	private bool m_WordWrap;

	private eStyleTextAlignment m_TextAlignment;

	private eStyleTextAlignment m_TextLineAlignment = eStyleTextAlignment.Center;

	private eStyleTextTrimming m_TextTrimming = eStyleTextTrimming.EllipsisCharacter;

	private Color m_TextColor = Color.Empty;

	private eColorSchemePart m_TextColorSchemePart = eColorSchemePart.None;

	private Color m_TextShadowColor = Color.Empty;

	private eColorSchemePart m_TextShadowColorSchemePart = eColorSchemePart.None;

	private Point m_TextShadowOffset = Point.Empty;

	private int m_MarginLeft;

	private int m_MarginRight;

	private int m_MarginTop;

	private int m_MarginBottom;

	private int m_PaddingLeft;

	private int m_PaddingRight;

	private int m_PaddingTop;

	private int m_PaddingBottom;

	private eStyleBorderType m_BorderLeft;

	private eStyleBorderType m_BorderRight;

	private eStyleBorderType m_BorderTop;

	private eStyleBorderType m_BorderBottom;

	private Color m_BorderColor = Color.Empty;

	private eColorSchemePart m_BorderColorSchemePart = eColorSchemePart.None;

	private Color m_BorderColor2 = Color.Empty;

	private eColorSchemePart m_BorderColor2SchemePart = eColorSchemePart.None;

	private int m_BorderGradientAngle = 90;

	private Color m_BorderColorLight = Color.Empty;

	private eColorSchemePart m_BorderColorLightSchemePart = eColorSchemePart.None;

	private Color m_BorderColorLight2 = Color.Empty;

	private eColorSchemePart m_BorderColorLight2SchemePart = eColorSchemePart.None;

	private int m_BorderLightGradientAngle = 90;

	private Color m_BorderLeftColor = Color.Empty;

	private eColorSchemePart m_BorderLeftColorSchemePart = eColorSchemePart.None;

	private Color m_BorderRightColor = Color.Empty;

	private eColorSchemePart m_BorderRightColorSchemePart = eColorSchemePart.None;

	private Color m_BorderTopColor = Color.Empty;

	private eColorSchemePart m_BorderTopColorSchemePart = eColorSchemePart.None;

	private Color m_BorderBottomColor = Color.Empty;

	private eColorSchemePart m_BorderBottomColorSchemePart = eColorSchemePart.None;

	private int m_BorderLeftWidth;

	private int m_BorderRightWidth;

	private int m_BorderTopWidth;

	private int m_BorderBottomWidth;

	private eCornerType m_CornerType = eCornerType.Square;

	private eCornerType m_CornerTypeTopLeft;

	private eCornerType m_CornerTypeTopRight;

	private eCornerType m_CornerTypeBottomLeft;

	private eCornerType m_CornerTypeBottomRight;

	private int m_CornerDiameter = 8;

	private Size m_Size = Size.Empty;

	private bool m_SizeChanged = true;

	private string m_Name = "";

	private string m_Description = "";

	private int m_MaximumHeight;

	private ISite m_ComponentSite;

	private bool m_DesignMode;

	private ColorScheme m_ColorScheme;

	private BackgroundColorBlendCollection m_BackColorBlend = new BackgroundColorBlendCollection();

	private string m_Class = "";

	private bool m_FreezeEvents;

	private eOrientation m_TextOrientation;

	private bool _UseMnemonic;

	private bool _HideMnemonic;

	internal bool _DisposeBackgroundImage;

	private int m_MaximumWidth;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Collection that defines the multicolor gradient background.")]
	public BackgroundColorBlendCollection BackColorBlend => m_BackColorBlend;

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Background")]
	[Description("Gets or sets the background color for UI element.")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ColorSchemeColorConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	public Color BackColor
	{
		get
		{
			return GetColor(m_BackColor, m_BackColorSchemePart);
		}
		set
		{
			if (value != BackColor && ((m_BackColorSchemePart != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BackColorSchemePart = eColorSchemePart.None;
			}
			m_BackColor = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BackColorSchemePart
	{
		get
		{
			return m_BackColorSchemePart;
		}
		set
		{
			m_BackColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Background")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Gets or sets the target gradient background color for UI element.")]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ColorSchemeColorConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	public Color BackColor2
	{
		get
		{
			return GetColor(m_BackColor2, m_BackColor2SchemePart);
		}
		set
		{
			if (value != BackColor2 && ((m_BackColor2SchemePart != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BackColor2SchemePart = eColorSchemePart.None;
			}
			m_BackColor2 = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BackColor2SchemePart
	{
		get
		{
			return m_BackColor2SchemePart;
		}
		set
		{
			m_BackColor2SchemePart = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Background")]
	[DefaultValue(0)]
	[Description("Gets or sets the background gradient angle.")]
	public int BackColorGradientAngle
	{
		get
		{
			return m_BackColorGradientAngle;
		}
		set
		{
			if (m_BackColorGradientAngle != value)
			{
				m_BackColorGradientAngle = value;
				OnStyleChanged();
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
			return m_BackColorGradientType;
		}
		set
		{
			if (m_BackColorGradientType != value)
			{
				m_BackColorGradientType = value;
				OnStyleChanged();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Background")]
	[DefaultValue(null)]
	[Description("Specifies background image.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public Image BackgroundImage
	{
		get
		{
			return m_BackgroundImage;
		}
		set
		{
			m_BackgroundImage = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Background")]
	[DefaultValue(eStyleBackgroundImage.Stretch)]
	[Description("Specifies background image position when container is larger than image.")]
	public eStyleBackgroundImage BackgroundImagePosition
	{
		get
		{
			return m_BackgroundImagePosition;
		}
		set
		{
			if (m_BackgroundImagePosition != value)
			{
				m_BackgroundImagePosition = value;
				OnStyleChanged();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Background")]
	[DefaultValue(byte.MaxValue)]
	[Description("Specifies the transparency of background image.")]
	public byte BackgroundImageAlpha
	{
		get
		{
			return m_BackgroundImageAlpha;
		}
		set
		{
			if (m_BackgroundImageAlpha != value)
			{
				m_BackgroundImageAlpha = value;
				OnStyleChanged();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Text Properties")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Gets or sets the text color displayed in this UI element.")]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ColorSchemeColorConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	public Color TextColor
	{
		get
		{
			return GetColor(m_TextColor, m_TextColorSchemePart);
		}
		set
		{
			if (value != TextColor && ((m_TextColorSchemePart != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				TextColorSchemePart = eColorSchemePart.None;
			}
			m_TextColor = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart TextColorSchemePart
	{
		get
		{
			return m_TextColorSchemePart;
		}
		set
		{
			m_TextColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Text Properties")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates text shadow color.")]
	[TypeConverter(typeof(ColorConverter))]
	public Color TextShadowColor
	{
		get
		{
			return GetColor(m_TextShadowColor, m_TextShadowColorSchemePart);
		}
		set
		{
			if ((m_TextShadowColorSchemePart != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty)
			{
				TextShadowColorSchemePart = eColorSchemePart.None;
			}
			m_TextShadowColor = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart TextShadowColorSchemePart
	{
		get
		{
			return m_TextShadowColorSchemePart;
		}
		set
		{
			m_TextShadowColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Text Properties")]
	[Description("Indicates text shadow offset in pixels.")]
	public Point TextShadowOffset
	{
		get
		{
			return m_TextShadowOffset;
		}
		set
		{
			m_TextShadowOffset = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Text Properties")]
	[DefaultValue(null)]
	[Description("Gets or sets the Font used to draw this the text.")]
	public Font Font
	{
		get
		{
			return m_Font;
		}
		set
		{
			m_Font = value;
			OnStyleChanged();
			OnSizePropertyChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Text Formatting")]
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
			m_WordWrap = value;
			OnStyleChanged();
			OnSizePropertyChanged();
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
			return m_TextAlignment;
		}
		set
		{
			m_TextAlignment = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Text Formatting")]
	[DefaultValue(eStyleTextAlignment.Center)]
	[Description("Specifies alignment of the text.")]
	public eStyleTextAlignment TextLineAlignment
	{
		get
		{
			return m_TextLineAlignment;
		}
		set
		{
			m_TextLineAlignment = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Text Formatting")]
	[DefaultValue(eStyleTextTrimming.EllipsisCharacter)]
	[Description("Specifies how to trim characters when text does not fit.")]
	public eStyleTextTrimming TextTrimming
	{
		get
		{
			return m_TextTrimming;
		}
		set
		{
			if (m_TextTrimming != value)
			{
				m_TextTrimming = value;
				OnStyleChanged();
			}
		}
	}

	[Browsable(false)]
	public int MarginHorizontal => m_MarginLeft + m_MarginRight;

	[Browsable(false)]
	public int MarginVertical => m_MarginTop + m_MarginBottom;

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Margins")]
	[DefaultValue(0)]
	[Description("Specifies left margin.")]
	public int MarginLeft
	{
		get
		{
			return m_MarginLeft;
		}
		set
		{
			m_MarginLeft = value;
			OnStyleChanged();
			OnSizePropertyChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Margins")]
	[DefaultValue(0)]
	[Description("Specifies right margin.")]
	public int MarginRight
	{
		get
		{
			return m_MarginRight;
		}
		set
		{
			m_MarginRight = value;
			OnStyleChanged();
			OnSizePropertyChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Margins")]
	[DefaultValue(0)]
	[Description("Specifies top margin.")]
	public int MarginTop
	{
		get
		{
			return m_MarginTop;
		}
		set
		{
			m_MarginTop = value;
			OnStyleChanged();
			OnSizePropertyChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Margins")]
	[DefaultValue(0)]
	[Description("Specifies bottom margin.")]
	public int MarginBottom
	{
		get
		{
			return m_MarginBottom;
		}
		set
		{
			m_MarginBottom = value;
			OnStyleChanged();
			OnSizePropertyChanged();
		}
	}

	internal bool SizeChanged
	{
		get
		{
			return m_SizeChanged;
		}
		set
		{
			m_SizeChanged = value;
		}
	}

	[Browsable(false)]
	public Size Size => m_Size;

	[DefaultValue(eStyleBorderType.None)]
	[Browsable(true)]
	[Category("Border")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Indicates the border type for all sides of the element.")]
	public eStyleBorderType Border
	{
		get
		{
			return m_BorderTop;
		}
		set
		{
			if (DesignMode)
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
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("Border")]
	[Description("Indicates border width in pixels.")]
	public int BorderWidth
	{
		get
		{
			return m_BorderTopWidth;
		}
		set
		{
			if (DesignMode)
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
			OnStyleChanged();
			OnSizePropertyChanged();
		}
	}

	[DevCoSerialize]
	[DefaultValue(eStyleBorderType.None)]
	[Browsable(true)]
	[Category("Border")]
	[Description("Indicates the border type for top side of the element.")]
	public eStyleBorderType BorderTop
	{
		get
		{
			return m_BorderTop;
		}
		set
		{
			if (m_BorderTop != value)
			{
				m_BorderTop = value;
				OnStyleChanged();
				OnSizePropertyChanged();
			}
		}
	}

	[DevCoSerialize]
	[DefaultValue(eStyleBorderType.None)]
	[Browsable(true)]
	[Category("Border")]
	[Description("Indicates the border type for bottom side of the element.")]
	public eStyleBorderType BorderBottom
	{
		get
		{
			return m_BorderBottom;
		}
		set
		{
			if (m_BorderBottom != value)
			{
				m_BorderBottom = value;
				OnStyleChanged();
				OnSizePropertyChanged();
			}
		}
	}

	[DevCoSerialize]
	[DefaultValue(eStyleBorderType.None)]
	[Browsable(true)]
	[Category("Border")]
	[Description("Indicates the border type for left side of the element.")]
	public eStyleBorderType BorderLeft
	{
		get
		{
			return m_BorderLeft;
		}
		set
		{
			if (m_BorderLeft != value)
			{
				m_BorderLeft = value;
				OnStyleChanged();
				OnSizePropertyChanged();
			}
		}
	}

	[DevCoSerialize]
	[DefaultValue(eStyleBorderType.None)]
	[Browsable(true)]
	[Category("Border")]
	[Description("Indicates the border type for right side of the element.")]
	public eStyleBorderType BorderRight
	{
		get
		{
			return m_BorderRight;
		}
		set
		{
			if (m_BorderRight != value)
			{
				m_BorderRight = value;
				OnStyleChanged();
				OnSizePropertyChanged();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Border")]
	[Description("Indicates border width in pixels.")]
	public int BorderTopWidth
	{
		get
		{
			return m_BorderTopWidth;
		}
		set
		{
			if (m_BorderTopWidth != value)
			{
				m_BorderTopWidth = value;
				OnStyleChanged();
				OnSizePropertyChanged();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Border")]
	[Description("Indicates border width in pixels.")]
	public int BorderBottomWidth
	{
		get
		{
			return m_BorderBottomWidth;
		}
		set
		{
			if (m_BorderBottomWidth != value)
			{
				m_BorderBottomWidth = value;
				OnStyleChanged();
				OnSizePropertyChanged();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Border")]
	[Description("Indicates border width in pixels.")]
	public int BorderLeftWidth
	{
		get
		{
			return m_BorderLeftWidth;
		}
		set
		{
			if (m_BorderLeftWidth != value)
			{
				m_BorderLeftWidth = value;
				OnStyleChanged();
				OnSizePropertyChanged();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Border")]
	[Description("Indicates border width in pixels.")]
	public int BorderRightWidth
	{
		get
		{
			return m_BorderRightWidth;
		}
		set
		{
			if (m_BorderRightWidth != value)
			{
				m_BorderRightWidth = value;
				OnStyleChanged();
				OnSizePropertyChanged();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Border")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates border color for all sides. Specifing the color for the side will override this value.")]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ColorSchemeColorConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	public Color BorderColor
	{
		get
		{
			return GetColor(m_BorderColor, m_BorderColorSchemePart);
		}
		set
		{
			if (value != BorderColor && ((m_BorderColorSchemePart != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderColorSchemePart = eColorSchemePart.None;
			}
			m_BorderColor = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BorderColorSchemePart
	{
		get
		{
			return m_BorderColorSchemePart;
		}
		set
		{
			m_BorderColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Border")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates target background gradient color for border on all sides. ")]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ColorSchemeColorConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	public Color BorderColor2
	{
		get
		{
			return GetColor(m_BorderColor2, m_BorderColor2SchemePart);
		}
		set
		{
			if (value != BorderColor2 && ((m_BorderColor2SchemePart != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderColor2SchemePart = eColorSchemePart.None;
			}
			m_BorderColor2 = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BorderColor2SchemePart
	{
		get
		{
			return m_BorderColor2SchemePart;
		}
		set
		{
			m_BorderColor2SchemePart = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Background")]
	[DefaultValue(90)]
	[Description("Gets or sets the border gradient angle.")]
	public int BorderGradientAngle
	{
		get
		{
			return m_BorderGradientAngle;
		}
		set
		{
			if (m_BorderGradientAngle != value)
			{
				m_BorderGradientAngle = value;
				OnStyleChanged();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Border")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates color for light border part when etched border is used.")]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ColorSchemeColorConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	public Color BorderColorLight
	{
		get
		{
			return GetColor(m_BorderColorLight, m_BorderColorLightSchemePart);
		}
		set
		{
			if (value != BorderColorLight && ((m_BorderColorLightSchemePart != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderColorLightSchemePart = eColorSchemePart.None;
			}
			m_BorderColorLight = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BorderColorLightSchemePart
	{
		get
		{
			return m_BorderColorLightSchemePart;
		}
		set
		{
			m_BorderColorLightSchemePart = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Border")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates target background gradient color for border on all sides. ")]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ColorSchemeColorConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	public Color BorderColorLight2
	{
		get
		{
			return GetColor(m_BorderColorLight2, m_BorderColorLight2SchemePart);
		}
		set
		{
			if (value != BorderColorLight2 && ((m_BorderColorLight2SchemePart != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderColorLight2SchemePart = eColorSchemePart.None;
			}
			m_BorderColorLight2 = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BorderColorLight2SchemePart
	{
		get
		{
			return m_BorderColorLight2SchemePart;
		}
		set
		{
			m_BorderColorLight2SchemePart = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Background")]
	[DefaultValue(90)]
	[Description("Gets or sets the border gradient angle.")]
	public int BorderLightGradientAngle
	{
		get
		{
			return m_BorderLightGradientAngle;
		}
		set
		{
			if (m_BorderLightGradientAngle != value)
			{
				m_BorderLightGradientAngle = value;
				OnStyleChanged();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Border")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates background color for the left side border.")]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ColorSchemeColorConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	public Color BorderLeftColor
	{
		get
		{
			return GetColor(m_BorderLeftColor, m_BorderLeftColorSchemePart);
		}
		set
		{
			if (value != BorderLeftColor && ((m_BorderLeftColorSchemePart != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderLeftColorSchemePart = eColorSchemePart.None;
			}
			m_BorderLeftColor = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BorderLeftColorSchemePart
	{
		get
		{
			return m_BorderLeftColorSchemePart;
		}
		set
		{
			m_BorderLeftColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Border")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates background color for the right side border.")]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ColorSchemeColorConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	public Color BorderRightColor
	{
		get
		{
			return GetColor(m_BorderRightColor, m_BorderRightColorSchemePart);
		}
		set
		{
			if (value != BorderRightColor && ((m_BorderRightColorSchemePart != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderRightColorSchemePart = eColorSchemePart.None;
			}
			m_BorderRightColor = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BorderRightColorSchemePart
	{
		get
		{
			return m_BorderRightColorSchemePart;
		}
		set
		{
			m_BorderRightColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Border")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates background color for the top side border.")]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ColorSchemeColorConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	public Color BorderTopColor
	{
		get
		{
			return GetColor(m_BorderTopColor, m_BorderTopColorSchemePart);
		}
		set
		{
			if (value != BorderTopColor && ((m_BorderTopColorSchemePart != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderTopColorSchemePart = eColorSchemePart.None;
			}
			m_BorderTopColor = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BorderTopColorSchemePart
	{
		get
		{
			return m_BorderTopColorSchemePart;
		}
		set
		{
			m_BorderTopColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Border")]
	[Editor(typeof(ColorTypeEditor), typeof(UITypeEditor))]
	[Description("Indicates background color for the bottom side border.")]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ColorSchemeColorConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	public Color BorderBottomColor
	{
		get
		{
			return GetColor(m_BorderBottomColor, m_BorderBottomColorSchemePart);
		}
		set
		{
			if (value != BorderBottomColor && ((m_BorderBottomColorSchemePart != eColorSchemePart.None && !value.IsEmpty) || value.IsEmpty))
			{
				BorderBottomColorSchemePart = eColorSchemePart.None;
			}
			m_BorderBottomColor = value;
			OnStyleChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(false)]
	[DefaultValue(eColorSchemePart.None)]
	public eColorSchemePart BorderBottomColorSchemePart
	{
		get
		{
			return m_BorderBottomColorSchemePart;
		}
		set
		{
			m_BorderBottomColorSchemePart = value;
			OnStyleChanged();
		}
	}

	[Browsable(false)]
	public int PaddingHorizontal => m_PaddingLeft + m_PaddingRight;

	[Browsable(false)]
	public int PaddingVertical => m_PaddingTop + m_PaddingBottom;

	[Browsable(true)]
	[DefaultValue(0)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("PaddingTop")]
	[Description("Indicates padding space in pixels for all 4 sides of the box.")]
	public int Padding
	{
		get
		{
			return m_PaddingTop;
		}
		set
		{
			if (DesignMode)
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
			OnStyleChanged();
			OnSizePropertyChanged();
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
			return m_PaddingTop;
		}
		set
		{
			m_PaddingTop = value;
			OnStyleChanged();
			OnSizePropertyChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Padding")]
	[DefaultValue(0)]
	[Description("Indicates the amount of space to insert between the bottom border of the element and the content.")]
	public int PaddingBottom
	{
		get
		{
			return m_PaddingBottom;
		}
		set
		{
			m_PaddingBottom = value;
			OnStyleChanged();
			OnSizePropertyChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Padding")]
	[DefaultValue(0)]
	[Description("Indicates the amount of space to insert between the left border of the element and the content.")]
	public int PaddingLeft
	{
		get
		{
			return m_PaddingLeft;
		}
		set
		{
			m_PaddingLeft = value;
			OnStyleChanged();
			OnSizePropertyChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Padding")]
	[DefaultValue(0)]
	[Description("Indicates the amount of space to insert between the right border of the element and the content.")]
	public int PaddingRight
	{
		get
		{
			return m_PaddingRight;
		}
		set
		{
			m_PaddingRight = value;
			OnStyleChanged();
			OnSizePropertyChanged();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[DefaultValue("")]
	[Category("Design")]
	[Description("Indicates name of the style.")]
	public string Name
	{
		get
		{
			if (m_ComponentSite != null)
			{
				m_Name = m_ComponentSite.Name;
			}
			return m_Name;
		}
		set
		{
			if (m_ComponentSite != null)
			{
				m_ComponentSite.Name = value;
			}
			if (value == null)
			{
				m_Name = "";
			}
			else
			{
				m_Name = value;
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Design")]
	[Description("Indicates the class style belongs to.")]
	[Localizable(false)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ElementStyleClassTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string Class
	{
		get
		{
			return m_Class;
		}
		set
		{
			if (value == null)
			{
				throw new NullReferenceException("null is not valid value for this property");
			}
			if (m_Class != value)
			{
				m_Class = value;
				OnStyleChanged();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[DefaultValue("")]
	[Category("Design")]
	[Description("Indicates description of the style.")]
	public string Description
	{
		get
		{
			return m_Description;
		}
		set
		{
			m_Description = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual ISite Site
	{
		get
		{
			return m_ComponentSite;
		}
		set
		{
			m_ComponentSite = value;
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Text Formatting")]
	[DefaultValue(0)]
	[Description("Indicates the maximum height of the element. This property should be used in conjunction with the WordWrap property to limit the size of text bounding box.")]
	public int MaximumHeight
	{
		get
		{
			return m_MaximumHeight;
		}
		set
		{
			if (m_MaximumHeight != value)
			{
				m_MaximumHeight = value;
				OnStyleChanged();
				OnSizePropertyChanged();
			}
		}
	}

	internal StringFormat StringFormat
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
			val.Alignment = (StringAlignment)m_TextAlignment;
			val.LineAlignment = (StringAlignment)m_TextLineAlignment;
			val.Trimming = (StringTrimming)m_TextTrimming;
			if (m_WordWrap)
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
	internal eOrientation TextOrientation
	{
		get
		{
			return m_TextOrientation;
		}
		set
		{
			if (m_TextOrientation != value)
			{
				m_TextOrientation = value;
				OnStyleChanged();
			}
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether the control interprets an ampersand character (&amp;) in the control's Text property to be an access key prefix character.")]
	public bool UseMnemonic
	{
		get
		{
			return _UseMnemonic;
		}
		set
		{
			_UseMnemonic = value;
			OnStyleChanged();
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether control hides the underline of the letter prefixed by ampersand character when UseMnemonic=true")]
	public bool HideMnemonic
	{
		get
		{
			return _HideMnemonic;
		}
		set
		{
			_HideMnemonic = value;
		}
	}

	internal eTextFormat TextFormat
	{
		get
		{
			eTextFormat eTextFormat2 = eTextFormat.Default;
			if (m_TextAlignment == eStyleTextAlignment.Center)
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			else if (m_TextAlignment == eStyleTextAlignment.Far)
			{
				eTextFormat2 |= eTextFormat.Right;
			}
			if (m_TextLineAlignment == eStyleTextAlignment.Center)
			{
				eTextFormat2 |= eTextFormat.VerticalCenter;
			}
			else if (m_TextLineAlignment == eStyleTextAlignment.Far)
			{
				eTextFormat2 |= eTextFormat.Bottom;
			}
			eTextFormat2 = ((!m_WordWrap) ? (eTextFormat2 | eTextFormat.SingleLine) : (eTextFormat2 | eTextFormat.WordBreak));
			if (m_TextTrimming != 0)
			{
				eTextFormat2 |= eTextFormat.EndEllipsis;
			}
			if (_HideMnemonic)
			{
				eTextFormat2 |= eTextFormat.HidePrefix;
			}
			if (!_UseMnemonic && !_HideMnemonic)
			{
				eTextFormat2 |= eTextFormat.NoPrefix;
			}
			if (m_TextOrientation == eOrientation.Vertical)
			{
				eTextFormat2 |= eTextFormat.Vertical;
			}
			return eTextFormat2;
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Border")]
	[Description("Indicates border corner type.")]
	public eCornerType CornerType
	{
		get
		{
			return m_CornerType;
		}
		set
		{
			if (m_CornerType != value)
			{
				m_CornerType = value;
				if (m_CornerType == eCornerType.Inherit)
				{
					m_CornerType = eCornerType.Square;
				}
				OnStyleChanged();
			}
		}
	}

	[DevCoSerialize]
	[DefaultValue(eCornerType.Inherit)]
	[Browsable(true)]
	[Category("Border")]
	[Description("Indicates top left border corner type.")]
	public eCornerType CornerTypeTopLeft
	{
		get
		{
			return m_CornerTypeTopLeft;
		}
		set
		{
			if (m_CornerTypeTopLeft != value)
			{
				m_CornerTypeTopLeft = value;
				OnStyleChanged();
			}
		}
	}

	[DevCoSerialize]
	[DefaultValue(eCornerType.Inherit)]
	[Browsable(true)]
	[Category("Border")]
	[Description("Indicates top right border corner type.")]
	public eCornerType CornerTypeTopRight
	{
		get
		{
			return m_CornerTypeTopRight;
		}
		set
		{
			if (m_CornerTypeTopRight != value)
			{
				m_CornerTypeTopRight = value;
				OnStyleChanged();
			}
		}
	}

	[DevCoSerialize]
	[DefaultValue(eCornerType.Inherit)]
	[Browsable(true)]
	[Category("Border")]
	[Description("Indicates bottom left border corner type.")]
	public eCornerType CornerTypeBottomLeft
	{
		get
		{
			return m_CornerTypeBottomLeft;
		}
		set
		{
			if (m_CornerTypeBottomLeft != value)
			{
				m_CornerTypeBottomLeft = value;
				OnStyleChanged();
			}
		}
	}

	[DevCoSerialize]
	[DefaultValue(eCornerType.Inherit)]
	[Browsable(true)]
	[Category("Border")]
	[Description("Indicates bottom right border corner type.")]
	public eCornerType CornerTypeBottomRight
	{
		get
		{
			return m_CornerTypeBottomRight;
		}
		set
		{
			if (m_CornerTypeBottomRight != value)
			{
				m_CornerTypeBottomRight = value;
				OnStyleChanged();
			}
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Border")]
	[DefaultValue(8)]
	[Description("Indicates diameter in pixels of the corner type rounded or diagonal.")]
	public int CornerDiameter
	{
		get
		{
			return m_CornerDiameter;
		}
		set
		{
			if (m_CornerDiameter != value)
			{
				m_CornerDiameter = value;
				OnStyleChanged();
			}
		}
	}

	internal bool PaintLeftBorder
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

	internal bool PaintRightBorder
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

	internal bool PaintTopBorder
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

	internal bool PaintBottomBorder
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

	internal bool PaintBorder => PaintTopBorder | PaintBottomBorder | PaintLeftBorder | PaintRightBorder;

	internal bool PaintAllBorders
	{
		get
		{
			if (PaintTopBorder && PaintBottomBorder && PaintLeftBorder)
			{
				return PaintRightBorder;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
			return false;
		}
	}

	internal bool FreezeEvents
	{
		get
		{
			return m_FreezeEvents;
		}
		set
		{
			m_FreezeEvents = value;
		}
	}

	internal bool DesignMode
	{
		get
		{
			return m_DesignMode;
		}
		set
		{
			m_DesignMode = value;
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[Category("Text Formatting")]
	[DefaultValue(0)]
	[Description("Indicates the maximum width of the element. This property should be used in conjunction with the WordWrap property to limit the size of text bounding box.")]
	public int MaximumWidth
	{
		get
		{
			return m_MaximumWidth;
		}
		set
		{
			if (m_MaximumWidth != value)
			{
				m_MaximumWidth = value;
				OnStyleChanged();
				OnSizePropertyChanged();
			}
		}
	}

	public event EventHandler StyleChanged;

	[Description("Occurs when component is Disposed.")]
	public event EventHandler Disposed;

	public ElementStyle()
	{
		m_BackColorBlend.Parent = this;
	}

	public ElementStyle(string styleClass)
	{
		m_Class = styleClass;
	}

	public ElementStyle(Color textColor)
	{
		m_BackColorBlend.Parent = this;
		TextColor = textColor;
	}

	public ElementStyle(Color textColor, Color backColor)
	{
		m_BackColorBlend.Parent = this;
		TextColor = textColor;
		BackColor = backColor;
	}

	public ElementStyle(Color textColor, Color backColor1, Color backColor2)
	{
		m_BackColorBlend.Parent = this;
		TextColor = textColor;
		BackColor = backColor1;
		BackColor2 = backColor2;
		BackColorGradientAngle = 90;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		if (!m_BackColor.IsEmpty)
		{
			return m_BackColorSchemePart == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor()
	{
		m_BackColor = Color.Empty;
		OnStyleChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor2()
	{
		if (!m_BackColor2.IsEmpty)
		{
			return m_BackColor2SchemePart == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor2()
	{
		m_BackColor2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundImage()
	{
		m_BackgroundImage = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		if (!m_TextColor.IsEmpty)
		{
			return m_TextColorSchemePart == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		m_TextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextShadowColor()
	{
		if (!m_TextShadowColor.IsEmpty)
		{
			return m_TextShadowColorSchemePart == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextShadowColor()
	{
		m_TextShadowColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextShadowOffset()
	{
		return !m_TextShadowOffset.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextShadowOffset()
	{
		m_TextShadowOffset = Point.Empty;
	}

	internal void SetSize(Size size)
	{
		m_Size = size;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		if (!m_BorderColor.IsEmpty)
		{
			return m_BorderColorSchemePart == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		m_BorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor2()
	{
		if (!m_BorderColor2.IsEmpty)
		{
			return m_BorderColor2SchemePart == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetBorderColor2()
	{
		m_BorderColor2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColorLight()
	{
		if (!m_BorderColorLight.IsEmpty)
		{
			return m_BorderColorLightSchemePart == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetBorderColorLight()
	{
		m_BorderColorLight = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColorLight2()
	{
		if (!m_BorderColorLight2.IsEmpty)
		{
			return m_BorderColorLight2SchemePart == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetBorderColorLight2()
	{
		m_BorderColorLight2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderLeftColor()
	{
		if (!m_BorderLeftColor.IsEmpty)
		{
			return m_BorderLeftColorSchemePart == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderLeftColor()
	{
		m_BorderLeftColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderRightColor()
	{
		if (!m_BorderRightColor.IsEmpty)
		{
			return m_BorderRightColorSchemePart == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderRightColor()
	{
		m_BorderRightColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderTopColor()
	{
		if (!m_BorderTopColor.IsEmpty)
		{
			return m_BorderTopColorSchemePart == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderTopColor()
	{
		m_BorderTopColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderBottomColor()
	{
		if (!m_BorderBottomColor.IsEmpty)
		{
			return m_BorderBottomColorSchemePart == eColorSchemePart.None;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderBottomColor()
	{
		m_BorderBottomColor = Color.Empty;
	}

	public virtual void Dispose()
	{
		if (this.Disposed != null)
		{
			this.Disposed(this, EventArgs.Empty);
		}
		if ((BarUtilities.DisposeItemImages && !DesignMode) || _DisposeBackgroundImage)
		{
			BarUtilities.DisposeImage(ref m_BackgroundImage);
		}
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
			elementStyle._DisposeBackgroundImage = true;
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
		elementStyle.Class = m_Class;
		elementStyle.UseMnemonic = _UseMnemonic;
		elementStyle.HideMnemonic = _HideMnemonic;
		foreach (BackgroundColorBlend item in m_BackColorBlend)
		{
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(item.Color, item.Position));
		}
		return elementStyle;
	}

	public void Reset()
	{
		m_BackColor = Color.Empty;
		m_BackColorSchemePart = eColorSchemePart.None;
		m_BackColor2 = Color.Empty;
		m_BackColor2SchemePart = eColorSchemePart.None;
		m_BackColorGradientAngle = 0;
		m_BackColorGradientType = eGradientType.Linear;
		m_BackgroundImage = null;
		m_BackgroundImagePosition = eStyleBackgroundImage.Stretch;
		m_BackgroundImageAlpha = byte.MaxValue;
		m_BackColorBlend.Clear();
		m_Font = null;
		m_WordWrap = false;
		m_TextAlignment = eStyleTextAlignment.Near;
		m_TextLineAlignment = eStyleTextAlignment.Center;
		m_TextTrimming = eStyleTextTrimming.EllipsisCharacter;
		m_TextColor = Color.Empty;
		m_TextColorSchemePart = eColorSchemePart.None;
		m_TextShadowColor = Color.Empty;
		m_TextShadowColorSchemePart = eColorSchemePart.None;
		m_TextShadowOffset = Point.Empty;
		m_MarginLeft = 0;
		m_MarginRight = 0;
		m_MarginTop = 0;
		m_MarginBottom = 0;
		m_PaddingLeft = 0;
		m_PaddingRight = 0;
		m_PaddingTop = 0;
		m_PaddingBottom = 0;
		m_BorderLeft = eStyleBorderType.None;
		m_BorderRight = eStyleBorderType.None;
		m_BorderTop = eStyleBorderType.None;
		m_BorderBottom = eStyleBorderType.None;
		m_BorderColor = Color.Empty;
		m_BorderColorSchemePart = eColorSchemePart.None;
		m_BorderColor2 = Color.Empty;
		m_BorderColor2SchemePart = eColorSchemePart.None;
		m_BorderGradientAngle = 90;
		m_BorderColorLight = Color.Empty;
		m_BorderColorLightSchemePart = eColorSchemePart.None;
		m_BorderColorLight2 = Color.Empty;
		m_BorderColorLight2SchemePart = eColorSchemePart.None;
		m_BorderLightGradientAngle = 90;
		m_BorderLeftColor = Color.Empty;
		m_BorderLeftColorSchemePart = eColorSchemePart.None;
		m_BorderRightColor = Color.Empty;
		m_BorderRightColorSchemePart = eColorSchemePart.None;
		m_BorderTopColor = Color.Empty;
		m_BorderTopColorSchemePart = eColorSchemePart.None;
		m_BorderBottomColor = Color.Empty;
		m_BorderBottomColorSchemePart = eColorSchemePart.None;
		m_BorderLeftWidth = 0;
		m_BorderRightWidth = 0;
		m_BorderTopWidth = 0;
		m_BorderBottomWidth = 0;
		m_CornerType = eCornerType.Square;
		m_CornerTypeTopLeft = eCornerType.Inherit;
		m_CornerTypeTopRight = eCornerType.Inherit;
		m_CornerTypeBottomLeft = eCornerType.Inherit;
		m_CornerTypeBottomRight = eCornerType.Inherit;
		m_CornerDiameter = 8;
		m_Class = "";
		m_MaximumHeight = 0;
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

	private void OnStyleChanged()
	{
		if (!m_FreezeEvents && this.StyleChanged != null)
		{
			this.StyleChanged(this, new EventArgs());
		}
	}

	private void OnSizePropertyChanged()
	{
		m_SizeChanged = true;
	}

	private Color GetColor(Color color, eColorSchemePart p)
	{
		if (p == eColorSchemePart.None)
		{
			return color;
		}
		ColorScheme colorScheme = GetColorScheme();
		if (colorScheme == null)
		{
			return color;
		}
		return p switch
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
			_ => (Color)colorScheme.GetType().GetProperty(p.ToString()).GetValue(colorScheme, null), 
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ColorScheme GetColorScheme()
	{
		return m_ColorScheme;
	}

	internal void SetColorScheme(ColorScheme cs)
	{
		m_ColorScheme = cs;
	}
}
