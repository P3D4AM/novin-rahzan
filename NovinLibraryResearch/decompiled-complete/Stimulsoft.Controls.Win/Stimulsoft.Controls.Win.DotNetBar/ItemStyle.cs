using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class ItemStyle : ICloneable
{
	private const eBorderSide DEFAULT_BORDERSIDE = eBorderSide.All;

	private const int DEFAULT_CORNER_DIAMETER = 8;

	private ColorEx m_BackColor1 = ColorEx.Empty;

	private ColorEx m_BackColor2 = ColorEx.Empty;

	private int m_GradientAngle;

	private ColorEx m_ForeColor = ColorEx.Empty;

	private Font m_Font;

	private Image m_BackgroundImage;

	private eBackgroundImagePosition m_BackgroundImagePosition;

	private byte m_BackgroundImageAlpha = byte.MaxValue;

	private bool m_WordWrap;

	private StringAlignment m_Alignment;

	private StringAlignment m_LineAlignment = (StringAlignment)1;

	private StringTrimming m_TextTrimming = (StringTrimming)3;

	private eBorderType m_Border;

	private eBorderSide m_BorderSide = eBorderSide.All;

	private int m_BorderWidth = 1;

	private ColorEx m_BorderColor = ColorEx.Empty;

	private bool m_Custom;

	private DashStyle m_BorderDashStyle;

	private int m_MarginLeft;

	private int m_MarginRight;

	private int m_MarginTop;

	private int m_MarginBottom;

	private bool m_VerticalText;

	private eCornerType m_CornerType = eCornerType.Square;

	private int m_CornerDiameter = 8;

	private bool _UseMnemonic = true;

	internal bool Custom
	{
		get
		{
			return m_Custom | m_BackColor1.Custom | m_BackColor2.Custom | m_ForeColor.Custom;
		}
		set
		{
			m_Custom = value;
			m_BackColor1.Custom = value;
			m_BackColor2.Custom = value;
			m_ForeColor.Custom = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool VerticalText
	{
		get
		{
			return m_VerticalText;
		}
		set
		{
			if (m_VerticalText != value)
			{
				m_VerticalText = value;
				InvokeVisualPropertyChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets a background color or starting color for gradient background.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ColorEx BackColor1 => m_BackColor1;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets a background color or ending color for gradient background.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ColorEx BackColor2 => m_BackColor2;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets a text color.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ColorEx ForeColor => m_ForeColor;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(0)]
	[Description("Gets or sets the gradient angle.")]
	public int GradientAngle
	{
		get
		{
			return m_GradientAngle;
		}
		set
		{
			if (m_GradientAngle != value)
			{
				m_GradientAngle = value;
				m_Custom = true;
				InvokeVisualPropertyChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(null)]
	[Description("Gets or sets the style Font")]
	public Font Font
	{
		get
		{
			return m_Font;
		}
		set
		{
			m_Font = value;
			m_Custom = true;
			InvokeVisualPropertyChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
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
			}
			m_Custom = true;
			InvokeVisualPropertyChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Specifies alignment of the text.")]
	public StringAlignment Alignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_Alignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (m_Alignment != value)
			{
				m_Alignment = value;
				m_Custom = true;
				InvokeVisualPropertyChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Specifies alignment of the text.")]
	public StringAlignment LineAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_LineAlignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (m_LineAlignment != value)
			{
				m_LineAlignment = value;
				m_Custom = true;
				InvokeVisualPropertyChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Specifies how to trim characters when text does not fit.")]
	public StringTrimming TextTrimming
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_TextTrimming;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (m_TextTrimming != value)
			{
				m_TextTrimming = value;
				m_Custom = true;
				InvokeVisualPropertyChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
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
			m_Custom = true;
			InvokeVisualPropertyChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(eBackgroundImagePosition.Stretch)]
	[Description("Specifies background image position when container is larger than image.")]
	public eBackgroundImagePosition BackgroundImagePosition
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
				m_Custom = true;
				InvokeVisualPropertyChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
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
				m_Custom = true;
				InvokeVisualPropertyChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(eCornerType.Square)]
	[Description("Indicates corner type.")]
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
				m_Custom = true;
				InvokeVisualPropertyChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
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
				m_Custom = true;
				InvokeVisualPropertyChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(eBorderType.None)]
	[Description("Specifies the border type.")]
	public eBorderType Border
	{
		get
		{
			return m_Border;
		}
		set
		{
			if (m_Border != value)
			{
				m_Border = value;
				m_Custom = true;
				InvokeVisualPropertyChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Indicates dash style for single line border type.")]
	public DashStyle BorderDashStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_BorderDashStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			m_BorderDashStyle = value;
			InvokeVisualPropertyChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
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
				m_Custom = true;
				InvokeVisualPropertyChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Specifies the border color.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ColorEx BorderColor => m_BorderColor;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(1)]
	[Description("Specifies the line tickness of single line border.")]
	public int BorderWidth
	{
		get
		{
			return m_BorderWidth;
		}
		set
		{
			m_BorderWidth = value;
			InvokeVisualPropertyChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(0)]
	[Description("Specifies left text margin.")]
	public int MarginLeft
	{
		get
		{
			return m_MarginLeft;
		}
		set
		{
			m_MarginLeft = value;
			InvokeVisualPropertyChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(0)]
	[Description("Specifies right text margin.")]
	public int MarginRight
	{
		get
		{
			return m_MarginRight;
		}
		set
		{
			m_MarginRight = value;
			InvokeVisualPropertyChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(0)]
	[Description("Specifies top text margin.")]
	public int MarginTop
	{
		get
		{
			return m_MarginTop;
		}
		set
		{
			m_MarginTop = value;
			InvokeVisualPropertyChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(0)]
	[Description("Specifies bottom text margin.")]
	public int MarginBottom
	{
		get
		{
			return m_MarginBottom;
		}
		set
		{
			m_MarginBottom = value;
			InvokeVisualPropertyChanged();
		}
	}

	internal StringFormat StringFormat
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			StringFormat val = BarFunctions.CreateStringFormat();
			val.Alignment = m_Alignment;
			val.LineAlignment = m_LineAlignment;
			if (m_WordWrap)
			{
				val.FormatFlags = (StringFormatFlags)(val.FormatFlags & ~(val.FormatFlags & 0x1000));
			}
			else
			{
				val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 0x1000);
			}
			val.Trimming = m_TextTrimming;
			val.HotkeyPrefix = (HotkeyPrefix)1;
			return val;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether the control interprets an ampersand character (&) in the control's Text property to be an access key prefix character.")]
	public bool UseMnemonic
	{
		get
		{
			return _UseMnemonic;
		}
		set
		{
			_UseMnemonic = value;
		}
	}

	internal ThemeTextFormat ThemeStringFormat
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected I4, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected I4, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected I4, but got Unknown
			ThemeTextFormat themeTextFormat = ThemeTextFormat.HidePrefix;
			StringAlignment alignment = m_Alignment;
			switch ((int)alignment)
			{
			case 1:
				themeTextFormat |= ThemeTextFormat.Center;
				break;
			case 0:
				themeTextFormat |= (SystemInformation.RightAlignedMenus ? ThemeTextFormat.Right : ThemeTextFormat.Top);
				break;
			case 2:
				themeTextFormat |= ((!SystemInformation.RightAlignedMenus) ? ThemeTextFormat.Right : ThemeTextFormat.Top);
				break;
			}
			StringAlignment lineAlignment = m_LineAlignment;
			switch ((int)lineAlignment)
			{
			case 1:
				themeTextFormat |= (m_WordWrap ? ThemeTextFormat.Bottom : ThemeTextFormat.VCenter);
				break;
			case 0:
				themeTextFormat |= ThemeTextFormat.Top;
				break;
			case 2:
				themeTextFormat |= ThemeTextFormat.Bottom;
				break;
			}
			themeTextFormat = ((!m_WordWrap) ? (themeTextFormat | ThemeTextFormat.SingleLine) : (themeTextFormat | ThemeTextFormat.WordBreak));
			StringTrimming textTrimming = m_TextTrimming;
			switch (textTrimming - 1)
			{
			case 0:
				themeTextFormat |= ThemeTextFormat.NoFullWidthCharBreak;
				break;
			case 2:
				themeTextFormat |= ThemeTextFormat.EndEllipsis;
				break;
			case 4:
				themeTextFormat |= ThemeTextFormat.PathElliosis;
				break;
			case 3:
				themeTextFormat |= ThemeTextFormat.WordEllipsis;
				break;
			case 1:
				themeTextFormat |= ThemeTextFormat.WordEllipsis;
				break;
			}
			return themeTextFormat;
		}
	}

	internal event EventHandler VisualPropertyChanged;

	public ItemStyle()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		m_BackColor1.ColorChanged += ColorChanged;
		m_BackColor2.ColorChanged += ColorChanged;
		m_ForeColor.ColorChanged += ColorChanged;
		m_BorderColor.ColorChanged += ColorChanged;
	}

	public object Clone()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		ItemStyle itemStyle = new ItemStyle();
		itemStyle.Alignment = Alignment;
		itemStyle.BackColor1.SetColor(BackColor1.Color);
		itemStyle.BackColor1.Alpha = BackColor1.Alpha;
		itemStyle.BackColor1.SetColorSchemePart(BackColor1.ColorSchemePart);
		itemStyle.BackColor2.SetColor(BackColor2.Color);
		itemStyle.BackColor2.Alpha = BackColor2.Alpha;
		itemStyle.BackColor2.SetColorSchemePart(BackColor2.ColorSchemePart);
		if (BackgroundImage != null)
		{
			object obj = BackgroundImage.Clone();
			itemStyle.BackgroundImage = (Image)((obj is Image) ? obj : null);
		}
		itemStyle.BackgroundImageAlpha = BackgroundImageAlpha;
		itemStyle.BackgroundImagePosition = BackgroundImagePosition;
		itemStyle.Border = Border;
		itemStyle.BorderDashStyle = BorderDashStyle;
		itemStyle.BorderColor.SetColor(BorderColor.Color);
		itemStyle.BorderColor.Alpha = BorderColor.Alpha;
		itemStyle.BorderColor.SetColorSchemePart(BorderColor.ColorSchemePart);
		itemStyle.BorderWidth = BorderWidth;
		if (Font != null)
		{
			object obj2 = Font.Clone();
			itemStyle.Font = (Font)((obj2 is Font) ? obj2 : null);
		}
		itemStyle.ForeColor.SetColor(ForeColor.Color);
		itemStyle.ForeColor.Alpha = ForeColor.Alpha;
		itemStyle.ForeColor.SetColorSchemePart(ForeColor.ColorSchemePart);
		itemStyle.GradientAngle = GradientAngle;
		itemStyle.LineAlignment = LineAlignment;
		itemStyle.BorderSide = BorderSide;
		itemStyle.TextTrimming = TextTrimming;
		itemStyle.WordWrap = WordWrap;
		itemStyle.Custom = Custom;
		itemStyle.MarginBottom = MarginBottom;
		itemStyle.MarginLeft = MarginLeft;
		itemStyle.MarginRight = MarginRight;
		itemStyle.MarginTop = MarginTop;
		itemStyle.CornerType = CornerType;
		itemStyle.CornerDiameter = CornerDiameter;
		itemStyle.VerticalText = VerticalText;
		itemStyle.UseMnemonic = _UseMnemonic;
		return itemStyle;
	}

	internal void ApplyStyle(ItemStyle style)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Invalid comparison between Unknown and I4
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Invalid comparison between Unknown and I4
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		if (style != null)
		{
			m_Alignment = style.Alignment;
			if (!style.BackColor1.IsEmpty)
			{
				m_BackColor1 = style.BackColor1;
			}
			if (!style.BackColor2.IsEmpty)
			{
				m_BackColor2 = style.BackColor2;
			}
			if (style.BackgroundImage != null)
			{
				m_BackgroundImage = style.BackgroundImage;
			}
			if (style.BackgroundImageAlpha != byte.MaxValue)
			{
				m_BackgroundImageAlpha = style.BackgroundImageAlpha;
			}
			if (style.BackgroundImagePosition != 0)
			{
				m_BackgroundImagePosition = style.BackgroundImagePosition;
			}
			if (style.Border != 0)
			{
				m_Border = style.Border;
			}
			if ((int)style.BorderDashStyle != 0)
			{
				m_BorderDashStyle = style.BorderDashStyle;
			}
			if (!style.BorderColor.IsEmpty)
			{
				m_BorderColor = style.BorderColor;
			}
			if (style.BorderSide != eBorderSide.All)
			{
				m_BorderSide = style.BorderSide;
			}
			if (style.BorderWidth != 1)
			{
				m_BorderWidth = style.BorderWidth;
			}
			if (style.Font != null)
			{
				m_Font = style.Font;
			}
			if (!style.ForeColor.IsEmpty)
			{
				m_ForeColor = style.ForeColor;
			}
			if ((int)style.LineAlignment != 1)
			{
				m_LineAlignment = style.LineAlignment;
			}
			if ((int)style.TextTrimming != 3)
			{
				m_TextTrimming = style.TextTrimming;
			}
			if (style.WordWrap)
			{
				m_WordWrap = style.WordWrap;
			}
			if (style.MarginBottom != 0)
			{
				m_MarginBottom = style.MarginBottom;
			}
			if (style.MarginLeft != 0)
			{
				m_MarginLeft = style.MarginLeft;
			}
			if (style.MarginRight != 0)
			{
				m_MarginRight = style.MarginRight;
			}
			if (style.MarginTop != 0)
			{
				m_MarginTop = style.MarginTop;
			}
			if (style.CornerType != eCornerType.Square)
			{
				m_CornerType = style.CornerType;
			}
			if (style.CornerDiameter != 8)
			{
				m_CornerDiameter = style.CornerDiameter;
			}
			if (style.VerticalText)
			{
				m_VerticalText = style.VerticalText;
			}
			if (!style.UseMnemonic)
			{
				_UseMnemonic = style.UseMnemonic;
			}
		}
	}

	internal void ApplyColorScheme(ColorScheme cs)
	{
		if (BackColor1.ColorSchemePart != eColorSchemePart.Custom && BackColor1.ColorSchemePart != eColorSchemePart.None)
		{
			BackColor1.SetColor((Color)cs.GetType().GetProperty(BackColor1.ColorSchemePart.ToString()).GetValue(cs, null));
		}
		if (BackColor2.ColorSchemePart != eColorSchemePart.Custom && BackColor2.ColorSchemePart != eColorSchemePart.None)
		{
			BackColor2.SetColor((Color)cs.GetType().GetProperty(BackColor2.ColorSchemePart.ToString()).GetValue(cs, null));
		}
		if (BorderColor.ColorSchemePart != eColorSchemePart.Custom && BorderColor.ColorSchemePart != eColorSchemePart.None)
		{
			BorderColor.SetColor((Color)cs.GetType().GetProperty(BorderColor.ColorSchemePart.ToString()).GetValue(cs, null));
		}
		if (ForeColor.ColorSchemePart != eColorSchemePart.Custom && ForeColor.ColorSchemePart != eColorSchemePart.None)
		{
			ForeColor.SetColor((Color)cs.GetType().GetProperty(ForeColor.ColorSchemePart.ToString()).GetValue(cs, null));
		}
	}

	private void ColorChanged(object sender, EventArgs e)
	{
		InvokeVisualPropertyChanged();
	}

	private void InvokeVisualPropertyChanged()
	{
		if (this.VisualPropertyChanged != null)
		{
			this.VisualPropertyChanged(this, new EventArgs());
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor1()
	{
		return !m_BackColor1.IsEmpty;
	}

	public void ResetBackColor1()
	{
		m_BackColor1.ColorChanged -= ColorChanged;
		m_BackColor1 = ColorEx.Empty;
		m_BackColor1.ColorChanged += ColorChanged;
	}

	internal void SetBackColor1(ColorEx c)
	{
		m_BackColor1 = c;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor2()
	{
		return !m_BackColor2.IsEmpty;
	}

	public void ResetBackColor2()
	{
		m_BackColor2.ColorChanged -= ColorChanged;
		m_BackColor2 = ColorEx.Empty;
		m_BackColor2.ColorChanged += ColorChanged;
	}

	internal void SetBackColor2(ColorEx c)
	{
		m_BackColor2 = c;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeForeColor()
	{
		return !m_ForeColor.IsEmpty;
	}

	internal Region GetRegion(Rectangle clientRectangle)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		if (m_CornerType == eCornerType.Rounded && m_CornerDiameter > 1)
		{
			return Region.FromHrgn(WinApi.CreateRoundRectRgn(clientRectangle.X, clientRectangle.Y, clientRectangle.Width, clientRectangle.Height, m_CornerDiameter, m_CornerDiameter));
		}
		clientRectangle.Width++;
		clientRectangle.Height++;
		GraphicsPath path = GetPath(clientRectangle);
		Region val = new Region();
		val.MakeEmpty();
		val.Union(path);
		if (m_Border != 0 && m_BorderWidth > 0 && m_CornerType != eCornerType.Square && !m_BorderColor.IsEmpty)
		{
			Pen val2 = new Pen(Color.Black, (float)m_BorderWidth);
			try
			{
				path.Widen(val2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			Region val3 = new Region(path);
			val.Union(path);
		}
		else if (m_CornerType == eCornerType.Rounded)
		{
			path.Widen(SystemPens.Control);
			Region val4 = new Region(path);
			val.Union(path);
		}
		return val;
	}

	private GraphicsPath GetPath(Rectangle clientRectangle)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		GraphicsPath val = null;
		if (m_CornerType != eCornerType.Square)
		{
			clientRectangle.Width--;
			clientRectangle.Height--;
		}
		switch (m_CornerType)
		{
		case eCornerType.Square:
			val = new GraphicsPath();
			val.AddRectangle(clientRectangle);
			break;
		case eCornerType.Rounded:
			val = DisplayHelp.GetRoundedRectanglePath(clientRectangle, m_CornerDiameter);
			break;
		case eCornerType.Diagonal:
			val = new GraphicsPath();
			val.AddLine(clientRectangle.X, clientRectangle.Bottom - m_CornerDiameter, clientRectangle.X, clientRectangle.Y + m_CornerDiameter);
			val.AddLine(clientRectangle.X + m_CornerDiameter, clientRectangle.Y, clientRectangle.Right - m_CornerDiameter, clientRectangle.Y);
			val.AddLine(clientRectangle.Right, clientRectangle.Y + m_CornerDiameter, clientRectangle.Right, clientRectangle.Bottom - m_CornerDiameter);
			val.AddLine(clientRectangle.Right - m_CornerDiameter, clientRectangle.Bottom, clientRectangle.X + m_CornerDiameter, clientRectangle.Bottom);
			val.CloseAllFigures();
			break;
		}
		return val;
	}

	public void ResetBackgroundImage()
	{
		m_BackgroundImage = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return !m_BorderColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		m_BorderColor.ColorChanged -= ColorChanged;
		m_BorderColor = ColorEx.Empty;
		m_BorderColor.ColorChanged += ColorChanged;
	}

	public void Paint(Graphics g, Rectangle r, string text, Rectangle textRect, Font font)
	{
		Paint(g, r, text, textRect, font, null);
	}

	private void AdjustRectangleForBorder(ref Rectangle rPath)
	{
		if (m_Border != 0 && !m_BorderColor.IsEmpty)
		{
			int num = 1;
			if (m_BorderWidth > 1)
			{
				num = m_BorderWidth / 2;
			}
			else if (m_BorderWidth == 0)
			{
				num = 0;
			}
			if ((m_BorderSide & eBorderSide.Top) == eBorderSide.Top)
			{
				rPath.Y += num;
				rPath.Height -= num;
			}
			if ((m_BorderSide & eBorderSide.Bottom) == eBorderSide.Bottom)
			{
				rPath.Height -= num;
			}
			if ((m_BorderSide & eBorderSide.Left) == eBorderSide.Left)
			{
				rPath.X += num;
				rPath.Width -= num;
			}
			if ((m_BorderSide & eBorderSide.Right) == eBorderSide.Right)
			{
				rPath.Width -= num;
			}
		}
	}

	public void Paint(Graphics g, Rectangle r, string text, Rectangle textRect, Font font, Point[] borderShape)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Expected O, but got Unknown
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		if (r.Width == 0 || r.Height == 0)
		{
			return;
		}
		Rectangle rPath = r;
		if ((int)g.SmoothingMode == 4)
		{
			rPath.Inflate(1, 1);
		}
		AdjustRectangleForBorder(ref rPath);
		GraphicsPath path = GetPath(rPath);
		if (!m_BackColor1.IsEmpty)
		{
			if (!m_BackColor2.IsEmpty)
			{
				Color color = Color.FromArgb(m_BackColor1.Alpha, m_BackColor1.Color);
				Color color2 = Color.FromArgb(m_BackColor2.Alpha, m_BackColor2.Color);
				Rectangle r2 = r;
				LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(r2, color, color2, m_GradientAngle, isAngleScalable: false);
				g.FillPath((Brush)(object)val, path);
				((Brush)val).Dispose();
			}
			else if (m_BackColor1.Color != Color.Transparent)
			{
				Color color3 = Color.FromArgb(m_BackColor1.Alpha, m_BackColor1.Color);
				SolidBrush val2 = new SolidBrush(color3);
				g.FillPath((Brush)(object)val2, path);
				((Brush)val2).Dispose();
			}
		}
		path.Dispose();
		Rectangle rPath2 = r;
		rPath2.Width--;
		rPath2.Height--;
		AdjustRectangleForBorder(ref rPath2);
		path = GetPath(rPath2);
		if (m_BackgroundImage != null)
		{
			Region clip = g.Clip;
			GraphicsPath path2 = GetPath(r);
			try
			{
				g.SetClip(path2);
				BarFunctions.PaintBackgroundImage(g, r, m_BackgroundImage, m_BackgroundImagePosition, m_BackgroundImageAlpha);
			}
			finally
			{
				((IDisposable)path2)?.Dispose();
			}
			g.Clip = clip;
			if (clip != null)
			{
				clip.Dispose();
			}
		}
		if (m_Border != 0 && !m_BorderColor.IsEmpty)
		{
			if (m_CornerType != eCornerType.Square)
			{
				if ((int)m_BorderDashStyle == 0)
				{
					g.SmoothingMode = (SmoothingMode)2;
				}
				Pen val3 = new Pen(m_BorderColor.GetCompositeColor(), (float)m_BorderWidth);
				try
				{
					val3.DashStyle = m_BorderDashStyle;
					g.DrawPath(val3, path);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				if ((int)m_BorderDashStyle == 0)
				{
					g.SmoothingMode = (SmoothingMode)0;
				}
			}
			else if (m_Border == eBorderType.SingleLine && borderShape != null && borderShape.Length != 0)
			{
				Pen val4 = new Pen(m_BorderColor.GetCompositeColor(), (float)m_BorderWidth);
				try
				{
					val4.DashStyle = m_BorderDashStyle;
					g.DrawLines(val4, borderShape);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			else
			{
				switch (m_Border)
				{
				case eBorderType.SingleLine:
				case eBorderType.DoubleLine:
				case eBorderType.Etched:
				case eBorderType.Bump:
					BarFunctions.DrawBorder(g, m_Border, r, m_BorderColor.IsEmpty ? SystemColors.ControlText : Color.FromArgb(m_BorderColor.Alpha, m_BorderColor.Color), m_BorderSide, m_BorderDashStyle, m_BorderWidth);
					break;
				case eBorderType.Raised:
					BarFunctions.DrawBorder3D(side: (m_BorderSide != eBorderSide.All) ? ((Border3DSide)((((m_BorderSide & eBorderSide.Left) != 0) ? 1 : 0) | (((m_BorderSide & eBorderSide.Right) != 0) ? 4 : 0) | (((m_BorderSide & eBorderSide.Top) != 0) ? 2 : 0) | (((m_BorderSide & eBorderSide.Bottom) != 0) ? 8 : 0))) : ((Border3DSide)2063), g: g, r: r, style: (Border3DStyle)4, baseColor: m_BorderColor.IsEmpty ? m_BackColor1.Color : Color.FromArgb(m_BorderColor.Alpha, m_BorderColor.Color), bFillInner: false);
					break;
				case eBorderType.RaisedInner:
				{
					Pen val5 = new Pen(Color.White);
					try
					{
						if ((m_BorderSide & eBorderSide.Top) == eBorderSide.Top)
						{
							g.DrawLine(val5, r.X, r.Y, r.Right, r.Y);
						}
						if ((m_BorderSide & eBorderSide.Left) == eBorderSide.Left)
						{
							g.DrawLine(val5, r.X, r.Y, r.X, r.Bottom);
						}
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
					Pen val6 = new Pen(m_BorderColor.GetCompositeColor());
					try
					{
						if ((m_BorderSide & eBorderSide.Right) == eBorderSide.Right)
						{
							g.DrawLine(val6, r.Right - 1, r.Y, r.Right - 1, r.Bottom);
						}
						if ((m_BorderSide & eBorderSide.Bottom) == eBorderSide.Bottom)
						{
							g.DrawLine(val6, r.X, r.Bottom - 1, r.Right, r.Bottom - 1);
						}
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
					break;
				}
				case eBorderType.Sunken:
					BarFunctions.DrawBorder3D(side: (m_BorderSide != eBorderSide.All) ? ((Border3DSide)((((m_BorderSide & eBorderSide.Left) != 0) ? 1 : 0) | (((m_BorderSide & eBorderSide.Right) != 0) ? 4 : 0) | (((m_BorderSide & eBorderSide.Top) != 0) ? 2 : 0) | (((m_BorderSide & eBorderSide.Bottom) != 0) ? 8 : 0))) : ((Border3DSide)2063), g: g, r: r, style: (Border3DStyle)2, baseColor: m_BorderColor.IsEmpty ? m_BackColor1.Color : Color.FromArgb(m_BorderColor.Alpha, m_BorderColor.Color), bFillInner: false);
					break;
				}
			}
		}
		if (text != "" && !m_ForeColor.IsEmpty)
		{
			if (m_Font != null)
			{
				font = m_Font;
			}
			Color color4 = Color.FromArgb(m_ForeColor.Alpha, m_ForeColor.Color);
			if (m_MarginLeft != 0)
			{
				textRect.X += m_MarginLeft;
				textRect.Width -= m_MarginLeft;
			}
			if (m_MarginRight != 0)
			{
				textRect.Width -= m_MarginRight;
			}
			if (m_MarginTop != 0)
			{
				textRect.Y += m_MarginTop;
				textRect.Height -= m_MarginTop;
			}
			if (m_MarginBottom != 0)
			{
				textRect.Height -= m_MarginBottom;
			}
			if (textRect.Width > 0 && textRect.Height > 0)
			{
				if (m_VerticalText)
				{
					g.RotateTransform(-90f);
					TextDrawing.DrawStringLegacy(g, text, font, color4, new Rectangle(-textRect.Bottom, textRect.X, textRect.Height, textRect.Width), GetTextFormat());
					g.ResetTransform();
				}
				else
				{
					TextDrawing.DrawString(g, text, font, color4, textRect, GetTextFormat());
				}
			}
		}
		if (path != null)
		{
			path.Dispose();
		}
		if (m_CornerType != eCornerType.Square)
		{
			g.ResetClip();
		}
	}

	public void Paint(Graphics g, Rectangle r)
	{
		Paint(g, r, "", Rectangle.Empty, Control.DefaultFont);
	}

	public void PaintText(Graphics g, string text, Rectangle textRect, Font font)
	{
		if (!(text != ""))
		{
			return;
		}
		if (m_Font != null)
		{
			font = m_Font;
		}
		Color color = Color.FromArgb(m_ForeColor.Alpha, m_ForeColor.Color);
		if (m_MarginLeft != 0)
		{
			textRect.X += m_MarginLeft;
			textRect.Width -= m_MarginLeft;
		}
		if (m_MarginRight != 0)
		{
			textRect.Width -= m_MarginRight;
		}
		if (m_MarginTop != 0)
		{
			textRect.Y += m_MarginTop;
			textRect.Height -= m_MarginTop;
		}
		if (m_MarginBottom != 0)
		{
			textRect.Height -= m_MarginBottom;
		}
		if (textRect.Width > 0 && textRect.Height > 0)
		{
			if (m_VerticalText)
			{
				g.RotateTransform(-90f);
				TextDrawing.DrawStringLegacy(g, text, font, color, new Rectangle(-textRect.Bottom, textRect.X, textRect.Height, textRect.Width), GetTextFormat());
				g.ResetTransform();
			}
			else
			{
				TextDrawing.DrawString(g, text, font, color, textRect, GetTextFormat());
			}
		}
	}

	internal eTextFormat GetTextFormat()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		eTextFormat eTextFormat2 = eTextFormat.Default;
		if ((int)m_Alignment == 1)
		{
			eTextFormat2 |= eTextFormat.HorizontalCenter;
		}
		else if ((int)m_Alignment == 2)
		{
			eTextFormat2 |= eTextFormat.Right;
		}
		if ((int)m_LineAlignment == 1)
		{
			eTextFormat2 |= eTextFormat.VerticalCenter;
		}
		else if ((int)m_LineAlignment == 2)
		{
			eTextFormat2 |= eTextFormat.Bottom;
		}
		eTextFormat2 = (m_WordWrap ? (eTextFormat2 | eTextFormat.WordBreak) : (eTextFormat2 | eTextFormat.SingleLine));
		if ((int)m_TextTrimming != 0)
		{
			eTextFormat2 |= eTextFormat.EndEllipsis;
		}
		if (!_UseMnemonic)
		{
			eTextFormat2 |= eTextFormat.NoPrefix;
		}
		return eTextFormat2;
	}

	internal void Serialize(XmlElement styleElement)
	{
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Expected I4, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Invalid comparison between Unknown and I4
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected I4, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Invalid comparison between Unknown and I4
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected I4, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Expected I4, but got Unknown
		if (!m_BackColor1.IsEmpty)
		{
			if (m_BackColor1.ColorSchemePart == eColorSchemePart.Custom)
			{
				styleElement.SetAttribute("bc1", BarFunctions.ColorToString(m_BackColor1.Color));
				styleElement.SetAttribute("bc1a", m_BackColor1.Alpha.ToString());
			}
			else
			{
				styleElement.SetAttribute("bc1csp", m_BackColor1.ColorSchemePart.ToString());
			}
		}
		if (!m_BackColor2.IsEmpty)
		{
			if (m_BackColor2.ColorSchemePart == eColorSchemePart.Custom)
			{
				styleElement.SetAttribute("bc2", BarFunctions.ColorToString(m_BackColor2.Color));
				styleElement.SetAttribute("bc2a", m_BackColor2.Alpha.ToString());
			}
			else
			{
				styleElement.SetAttribute("bc2csp", m_BackColor2.ColorSchemePart.ToString());
			}
		}
		if (!m_ForeColor.IsEmpty)
		{
			if (m_ForeColor.ColorSchemePart == eColorSchemePart.Custom)
			{
				styleElement.SetAttribute("fc", BarFunctions.ColorToString(m_ForeColor.Color));
				styleElement.SetAttribute("fca", m_ForeColor.Alpha.ToString());
			}
			else
			{
				styleElement.SetAttribute("fccsp", m_ForeColor.ColorSchemePart.ToString());
			}
		}
		if (!m_BorderColor.IsEmpty)
		{
			if (m_BorderColor.ColorSchemePart == eColorSchemePart.Custom)
			{
				styleElement.SetAttribute("borderc", BarFunctions.ColorToString(m_BorderColor.Color));
				styleElement.SetAttribute("borderca", m_BorderColor.Alpha.ToString());
			}
			else
			{
				styleElement.SetAttribute("bordercsp", m_BorderColor.ColorSchemePart.ToString());
			}
		}
		styleElement.SetAttribute("ga", m_GradientAngle.ToString());
		if (m_Font != null)
		{
			styleElement.SetAttribute("fontname", m_Font.Name);
			styleElement.SetAttribute("fontemsize", XmlConvert.ToString(m_Font.Size));
			styleElement.SetAttribute("fontstyle", XmlConvert.ToString((int)m_Font.Style));
		}
		if (m_BackgroundImage != null)
		{
			XmlElement xmlElement = styleElement.OwnerDocument.CreateElement("backimage");
			styleElement.AppendChild(xmlElement);
			BarFunctions.SerializeImage(m_BackgroundImage, xmlElement);
			int backgroundImagePosition = (int)m_BackgroundImagePosition;
			xmlElement.SetAttribute("pos", backgroundImagePosition.ToString());
			xmlElement.SetAttribute("alpha", m_BackgroundImageAlpha.ToString());
		}
		if (m_WordWrap)
		{
			styleElement.SetAttribute("wordwrap", XmlConvert.ToString(m_WordWrap));
		}
		if ((int)m_Alignment != 0)
		{
			styleElement.SetAttribute("align", XmlConvert.ToString((int)m_Alignment));
		}
		if ((int)m_LineAlignment != 1)
		{
			styleElement.SetAttribute("valign", XmlConvert.ToString((int)m_LineAlignment));
		}
		if ((int)m_TextTrimming != 3)
		{
			styleElement.SetAttribute("trim", XmlConvert.ToString((int)m_TextTrimming));
		}
		if (m_Border != 0)
		{
			styleElement.SetAttribute("border", XmlConvert.ToString((int)m_Border));
		}
		if (m_BorderWidth != 1)
		{
			styleElement.SetAttribute("borderw", m_BorderWidth.ToString());
		}
		if (m_CornerDiameter != 8)
		{
			styleElement.SetAttribute("cornerdiameter", m_CornerDiameter.ToString());
		}
		if (m_CornerType != eCornerType.Square)
		{
			styleElement.SetAttribute("cornertype", XmlConvert.ToString((int)m_CornerType));
		}
	}

	internal void Deserialize(XmlElement styleElement)
	{
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Expected O, but got Unknown
		m_BackColor1 = ColorEx.Empty;
		if (styleElement.HasAttribute("bc1"))
		{
			m_BackColor1.Color = BarFunctions.ColorFromString(styleElement.GetAttribute("bc1"));
			m_BackColor1.Alpha = XmlConvert.ToByte(styleElement.GetAttribute("bc1a"));
		}
		else if (styleElement.HasAttribute("bc1csp"))
		{
			m_BackColor1.ColorSchemePart = (eColorSchemePart)Enum.Parse(typeof(eColorSchemePart), styleElement.GetAttribute("bc1csp"));
		}
		m_BackColor2 = ColorEx.Empty;
		if (styleElement.HasAttribute("bc2"))
		{
			m_BackColor2.Color = BarFunctions.ColorFromString(styleElement.GetAttribute("bc2"));
			m_BackColor2.Alpha = XmlConvert.ToByte(styleElement.GetAttribute("bc2a"));
		}
		else if (styleElement.HasAttribute("bc2csp"))
		{
			m_BackColor2.ColorSchemePart = (eColorSchemePart)Enum.Parse(typeof(eColorSchemePart), styleElement.GetAttribute("bc2csp"));
		}
		m_ForeColor = ColorEx.Empty;
		if (styleElement.HasAttribute("fc"))
		{
			m_ForeColor.Color = BarFunctions.ColorFromString(styleElement.GetAttribute("fc"));
			m_ForeColor.Alpha = XmlConvert.ToByte(styleElement.GetAttribute("fca"));
		}
		else if (styleElement.HasAttribute("fccsp"))
		{
			m_ForeColor.ColorSchemePart = (eColorSchemePart)Enum.Parse(typeof(eColorSchemePart), styleElement.GetAttribute("fccsp"));
		}
		m_BorderColor = ColorEx.Empty;
		if (styleElement.HasAttribute("borderc"))
		{
			m_BorderColor.Color = BarFunctions.ColorFromString(styleElement.GetAttribute("borderc"));
			m_BorderColor.Alpha = XmlConvert.ToByte(styleElement.GetAttribute("borderca"));
		}
		else if (styleElement.HasAttribute("bordercsp"))
		{
			m_BorderColor.ColorSchemePart = (eColorSchemePart)Enum.Parse(typeof(eColorSchemePart), styleElement.GetAttribute("bordercsp"));
		}
		m_GradientAngle = XmlConvert.ToInt16(styleElement.GetAttribute("ga"));
		m_Font = null;
		if (styleElement.HasAttribute("fontname"))
		{
			string attribute = styleElement.GetAttribute("fontname");
			float num = XmlConvert.ToSingle(styleElement.GetAttribute("fontemsize"));
			FontStyle val = (FontStyle)XmlConvert.ToInt32(styleElement.GetAttribute("fontstyle"));
			try
			{
				m_Font = new Font(attribute, num, val);
			}
			catch (Exception)
			{
				m_Font = null;
			}
		}
		m_BackgroundImage = null;
		foreach (XmlElement childNode in styleElement.ChildNodes)
		{
			if (childNode.Name == "backimage")
			{
				m_BackgroundImage = BarFunctions.DeserializeImage(childNode);
				m_BackgroundImagePosition = (eBackgroundImagePosition)XmlConvert.ToInt32(childNode.GetAttribute("pos"));
				m_BackgroundImageAlpha = XmlConvert.ToByte(childNode.GetAttribute("alpha"));
			}
		}
		m_WordWrap = false;
		if (styleElement.HasAttribute("wordwrap"))
		{
			m_WordWrap = XmlConvert.ToBoolean(styleElement.GetAttribute("wordwrap"));
		}
		m_Alignment = (StringAlignment)0;
		if (styleElement.HasAttribute("align"))
		{
			m_Alignment = (StringAlignment)XmlConvert.ToInt32(styleElement.GetAttribute("align"));
		}
		m_LineAlignment = (StringAlignment)1;
		if (styleElement.HasAttribute("valign"))
		{
			m_LineAlignment = (StringAlignment)XmlConvert.ToInt32(styleElement.GetAttribute("valign"));
		}
		m_TextTrimming = (StringTrimming)3;
		if (styleElement.HasAttribute("trim"))
		{
			m_TextTrimming = (StringTrimming)XmlConvert.ToInt32(styleElement.GetAttribute("trim"));
		}
		m_Border = eBorderType.None;
		if (styleElement.HasAttribute("border"))
		{
			m_Border = (eBorderType)XmlConvert.ToInt32(styleElement.GetAttribute("border"));
		}
		m_BorderWidth = 1;
		if (styleElement.HasAttribute("borderw"))
		{
			m_BorderWidth = XmlConvert.ToInt32(styleElement.GetAttribute("borderw"));
		}
		m_CornerType = eCornerType.Square;
		if (styleElement.HasAttribute("cornertype"))
		{
			m_CornerType = (eCornerType)XmlConvert.ToInt32(styleElement.GetAttribute("cornertype"));
		}
		m_CornerDiameter = 8;
		if (styleElement.HasAttribute("cornerdiameter"))
		{
			m_CornerDiameter = XmlConvert.ToInt32(styleElement.GetAttribute("cornerdiameter"));
		}
	}
}
