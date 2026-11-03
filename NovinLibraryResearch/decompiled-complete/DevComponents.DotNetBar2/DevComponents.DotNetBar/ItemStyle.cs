using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;

namespace DevComponents.DotNetBar;

[TypeConverter(typeof(ExpandableObjectConverter))]
[ToolboxItem(false)]
public class ItemStyle : ICloneable
{
	private const eBorderSide ᖺ = eBorderSide.All;

	private const int އ = 8;

	private ColorEx ᖻ = ColorEx.Empty;

	private ColorEx ތ = ColorEx.Empty;

	private int ᖼ;

	private ColorEx _0D02 = ColorEx.Empty;

	private Font ޓ;

	private Image ޏ;

	private eBackgroundImagePosition ސ;

	private byte ޑ = byte.MaxValue;

	private bool ݥ;

	private StringAlignment ᓹ;

	private StringAlignment ᖽ = (StringAlignment)1;

	private StringTrimming ޖ = (StringTrimming)3;

	private eBorderType ᖾ;

	private eBorderSide ᖿ = eBorderSide.All;

	private int ᗀ = 1;

	private ColorEx _07A8 = ColorEx.Empty;

	private bool ᗁ;

	private DashStyle ᗂ;

	private int ޜ;

	private int ޝ;

	private int ޞ;

	private int ޟ;

	private bool ᗃ;

	private eCornerType _07BE = eCornerType.Square;

	private int _߃ = 8;

	private bool _ߍ = true;

	internal bool ᖸ
	{
		get
		{
			return ᗁ | ᖻ.ᖸ | ތ.ᖸ | _0D02.ᖸ;
		}
		set
		{
			ᗁ = value;
			ᖻ.ᖸ = value;
			ތ.ᖸ = value;
			_0D02.ᖸ = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(false)]
	public bool VerticalText
	{
		get
		{
			return ᗃ;
		}
		set
		{
			if (ᗃ != value)
			{
				ᗃ = value;
				ᗇ();
			}
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[Description("Gets or sets a background color or starting color for gradient background.")]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ColorEx BackColor1 => ᖻ;

	[Category("Style")]
	[NotifyParentProperty(true)]
	[Description("Gets or sets a background color or ending color for gradient background.")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(true)]
	public ColorEx BackColor2 => ތ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Description("Gets or sets a text color.")]
	[Browsable(true)]
	[Category("Style")]
	public ColorEx ForeColor => _0D02;

	[NotifyParentProperty(true)]
	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Style")]
	[DevCoBrowsable(true)]
	[Description("Gets or sets the gradient angle.")]
	public int GradientAngle
	{
		get
		{
			return ᖼ;
		}
		set
		{
			if (ᖼ != value)
			{
				ᖼ = value;
				ᗁ = true;
				ᗇ();
			}
		}
	}

	[DefaultValue(null)]
	[Category("Style")]
	[Browsable(true)]
	[Description("Gets or sets the style Font")]
	[NotifyParentProperty(true)]
	[DevCoBrowsable(true)]
	public Font Font
	{
		get
		{
			return ޓ;
		}
		set
		{
			ޓ = value;
			ᗁ = true;
			ᗇ();
		}
	}

	[Category("Style")]
	[DefaultValue(false)]
	[NotifyParentProperty(true)]
	[Browsable(true)]
	[Description("Gets or sets a value that determines whether text is displayed in multiple lines or one long line.")]
	[DevCoBrowsable(true)]
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
			}
			ᗁ = true;
			ᗇ();
		}
	}

	[Category("Style")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Specifies alignment of the text.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public StringAlignment Alignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ᓹ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (ᓹ != value)
			{
				ᓹ = value;
				ᗁ = true;
				ᗇ();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[NotifyParentProperty(true)]
	[Description("Specifies alignment of the text.")]
	[Category("Style")]
	public StringAlignment LineAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ᖽ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (ᖽ != value)
			{
				ᖽ = value;
				ᗁ = true;
				ᗇ();
			}
		}
	}

	[Category("Style")]
	[NotifyParentProperty(true)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Specifies how to trim characters when text does not fit.")]
	public StringTrimming TextTrimming
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ޖ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (ޖ != value)
			{
				ޖ = value;
				ᗁ = true;
				ᗇ();
			}
		}
	}

	[Description("Specifies background image.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[NotifyParentProperty(true)]
	[DefaultValue(null)]
	[Category("Style")]
	public Image BackgroundImage
	{
		get
		{
			return ޏ;
		}
		set
		{
			ޏ = value;
			ᗁ = true;
			ᗇ();
		}
	}

	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[DefaultValue(eBackgroundImagePosition.Stretch)]
	[Browsable(true)]
	[Category("Style")]
	[Description("Specifies background image position when container is larger than image.")]
	public eBackgroundImagePosition BackgroundImagePosition
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
				ᗁ = true;
				ᗇ();
			}
		}
	}

	[Category("Style")]
	[DefaultValue(byte.MaxValue)]
	[Browsable(true)]
	[NotifyParentProperty(true)]
	[DevCoBrowsable(true)]
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
				ᗁ = true;
				ᗇ();
			}
		}
	}

	[NotifyParentProperty(true)]
	[DefaultValue(eCornerType.Square)]
	[Category("Style")]
	[Description("Indicates corner type.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
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
				ᗁ = true;
				ᗇ();
			}
		}
	}

	[Category("Style")]
	[DefaultValue(8)]
	[Description("Indicates diameter in pixels of the corner type rounded or diagonal.")]
	[NotifyParentProperty(true)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
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
				ᗁ = true;
				ᗇ();
			}
		}
	}

	[Category("Style")]
	[Browsable(true)]
	[NotifyParentProperty(true)]
	[DefaultValue(eBorderType.None)]
	[DevCoBrowsable(true)]
	[Description("Specifies the border type.")]
	public eBorderType Border
	{
		get
		{
			return ᖾ;
		}
		set
		{
			if (ᖾ != value)
			{
				ᖾ = value;
				ᗁ = true;
				ᗇ();
			}
		}
	}

	[NotifyParentProperty(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[DevCoBrowsable(true)]
	[Description("Indicates dash style for single line border type.")]
	[Category("Style")]
	[Browsable(true)]
	public DashStyle BorderDashStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ᗂ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			ᗂ = value;
			ᗇ();
		}
	}

	[NotifyParentProperty(true)]
	[DefaultValue(eBorderSide.All)]
	[Description("Specifies border sides that are displayed.")]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Browsable(true)]
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
				ᗁ = true;
				ᗇ();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Specifies the border color.")]
	public ColorEx BorderColor => _07A8;

	[NotifyParentProperty(true)]
	[DefaultValue(1)]
	[Description("Specifies the line tickness of single line border.")]
	[Browsable(true)]
	[Category("Style")]
	[DevCoBrowsable(true)]
	public int BorderWidth
	{
		get
		{
			return ᗀ;
		}
		set
		{
			ᗀ = value;
			ᗇ();
		}
	}

	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(0)]
	[Description("Specifies left text margin.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public int MarginLeft
	{
		get
		{
			return ޜ;
		}
		set
		{
			ޜ = value;
			ᗇ();
		}
	}

	[DefaultValue(0)]
	[NotifyParentProperty(true)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Style")]
	[Description("Specifies right text margin.")]
	public int MarginRight
	{
		get
		{
			return ޝ;
		}
		set
		{
			ޝ = value;
			ᗇ();
		}
	}

	[NotifyParentProperty(true)]
	[Category("Style")]
	[DefaultValue(0)]
	[DevCoBrowsable(true)]
	[Description("Specifies top text margin.")]
	[Browsable(true)]
	public int MarginTop
	{
		get
		{
			return ޞ;
		}
		set
		{
			ޞ = value;
			ᗇ();
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[NotifyParentProperty(true)]
	[DefaultValue(0)]
	[Description("Specifies bottom text margin.")]
	[DevCoBrowsable(true)]
	public int MarginBottom
	{
		get
		{
			return ޟ;
		}
		set
		{
			ޟ = value;
			ᗇ();
		}
	}

	internal StringFormat ݻ
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
			val.Alignment = ᓹ;
			val.LineAlignment = ᖽ;
			if (ݥ)
			{
				val.FormatFlags = (StringFormatFlags)(val.FormatFlags & ~(val.FormatFlags & 0x1000));
			}
			else
			{
				val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 0x1000);
			}
			val.Trimming = ޖ;
			val.HotkeyPrefix = (HotkeyPrefix)1;
			return val;
		}
	}

	[Description("Indicates whether the control interprets an ampersand character (&) in the control's Text property to be an access key prefix character.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool UseMnemonic
	{
		get
		{
			return _ߍ;
		}
		set
		{
			_ߍ = value;
		}
	}

	internal ThemeTextFormat ᖹ
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
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected I4, but got Unknown
			ThemeTextFormat themeTextFormat = ThemeTextFormat.HidePrefix;
			StringAlignment val = ᓹ;
			switch ((int)val)
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
			StringAlignment val2 = ᖽ;
			switch ((int)val2)
			{
			case 1:
				themeTextFormat |= (ݥ ? ThemeTextFormat.Bottom : ThemeTextFormat.VCenter);
				break;
			case 0:
				themeTextFormat = themeTextFormat;
				break;
			case 2:
				themeTextFormat |= ThemeTextFormat.Bottom;
				break;
			}
			themeTextFormat = ((!ݥ) ? (themeTextFormat | ThemeTextFormat.SingleLine) : (themeTextFormat | ThemeTextFormat.WordBreak));
			StringTrimming val3 = ޖ;
			switch (val3 - 1)
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

	internal event EventHandler ઐ;

	public ItemStyle()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		ᖻ.ᗆ += ᗆ;
		ތ.ᗆ += ᗆ;
		_0D02.ᗆ += ᗆ;
		_07A8.ᗆ += ᗆ;
	}

	public object Clone()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		ItemStyle itemStyle = new ItemStyle();
		itemStyle.Alignment = Alignment;
		itemStyle.BackColor1.ᗒ(BackColor1.Color);
		itemStyle.BackColor1.Alpha = BackColor1.Alpha;
		itemStyle.BackColor1.ᗓ(BackColor1.ColorSchemePart);
		itemStyle.BackColor2.ᗒ(BackColor2.Color);
		itemStyle.BackColor2.Alpha = BackColor2.Alpha;
		itemStyle.BackColor2.ᗓ(BackColor2.ColorSchemePart);
		if (BackgroundImage != null)
		{
			object obj = BackgroundImage.Clone();
			itemStyle.BackgroundImage = (Image)((obj is Image) ? obj : null);
		}
		itemStyle.BackgroundImageAlpha = BackgroundImageAlpha;
		itemStyle.BackgroundImagePosition = BackgroundImagePosition;
		itemStyle.Border = Border;
		itemStyle.BorderDashStyle = BorderDashStyle;
		itemStyle.BorderColor.ᗒ(BorderColor.Color);
		itemStyle.BorderColor.Alpha = BorderColor.Alpha;
		itemStyle.BorderColor.ᗓ(BorderColor.ColorSchemePart);
		itemStyle.BorderWidth = BorderWidth;
		if (Font != null)
		{
			object obj2 = Font.Clone();
			itemStyle.Font = (Font)((obj2 is Font) ? obj2 : null);
		}
		itemStyle.ForeColor.ᗒ(ForeColor.Color);
		itemStyle.ForeColor.Alpha = ForeColor.Alpha;
		itemStyle.ForeColor.ᗓ(ForeColor.ColorSchemePart);
		itemStyle.GradientAngle = GradientAngle;
		itemStyle.LineAlignment = LineAlignment;
		itemStyle.BorderSide = BorderSide;
		itemStyle.TextTrimming = TextTrimming;
		itemStyle.WordWrap = WordWrap;
		itemStyle.ᖸ = ᖸ;
		itemStyle.MarginBottom = MarginBottom;
		itemStyle.MarginLeft = MarginLeft;
		itemStyle.MarginRight = MarginRight;
		itemStyle.MarginTop = MarginTop;
		itemStyle.CornerType = CornerType;
		itemStyle.CornerDiameter = CornerDiameter;
		itemStyle.VerticalText = VerticalText;
		itemStyle.UseMnemonic = _ߍ;
		return itemStyle;
	}

	internal void ᗄ(ItemStyle _0657)
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
		if (_0657 != null)
		{
			ᓹ = _0657.Alignment;
			if (!_0657.BackColor1.IsEmpty)
			{
				ᖻ = _0657.BackColor1;
			}
			if (!_0657.BackColor2.IsEmpty)
			{
				ތ = _0657.BackColor2;
			}
			if (_0657.BackgroundImage != null)
			{
				ޏ = _0657.BackgroundImage;
			}
			if (_0657.BackgroundImageAlpha != byte.MaxValue)
			{
				ޑ = _0657.BackgroundImageAlpha;
			}
			if (_0657.BackgroundImagePosition != 0)
			{
				ސ = _0657.BackgroundImagePosition;
			}
			if (_0657.Border != 0)
			{
				ᖾ = _0657.Border;
			}
			if ((int)_0657.BorderDashStyle != 0)
			{
				ᗂ = _0657.BorderDashStyle;
			}
			if (!_0657.BorderColor.IsEmpty)
			{
				_07A8 = _0657.BorderColor;
			}
			if (_0657.BorderSide != eBorderSide.All)
			{
				ᖿ = _0657.BorderSide;
			}
			if (_0657.BorderWidth != 1)
			{
				ᗀ = _0657.BorderWidth;
			}
			if (_0657.Font != null)
			{
				ޓ = _0657.Font;
			}
			if (!_0657.ForeColor.IsEmpty)
			{
				_0D02 = _0657.ForeColor;
			}
			if ((int)_0657.LineAlignment != 1)
			{
				ᖽ = _0657.LineAlignment;
			}
			if ((int)_0657.TextTrimming != 3)
			{
				ޖ = _0657.TextTrimming;
			}
			if (_0657.WordWrap)
			{
				ݥ = _0657.WordWrap;
			}
			if (_0657.MarginBottom != 0)
			{
				ޟ = _0657.MarginBottom;
			}
			if (_0657.MarginLeft != 0)
			{
				ޜ = _0657.MarginLeft;
			}
			if (_0657.MarginRight != 0)
			{
				ޝ = _0657.MarginRight;
			}
			if (_0657.MarginTop != 0)
			{
				ޞ = _0657.MarginTop;
			}
			if (_0657.CornerType != eCornerType.Square)
			{
				_07BE = _0657.CornerType;
			}
			if (_0657.CornerDiameter != 8)
			{
				_߃ = _0657.CornerDiameter;
			}
			if (_0657.VerticalText)
			{
				ᗃ = _0657.VerticalText;
			}
			if (!_0657.UseMnemonic)
			{
				_ߍ = _0657.UseMnemonic;
			}
		}
	}

	internal void ᗅ(ColorScheme ጎ)
	{
		if (BackColor1.ColorSchemePart != eColorSchemePart.Custom && BackColor1.ColorSchemePart != eColorSchemePart.None)
		{
			BackColor1.ᗒ((Color)ጎ.GetType().GetProperty(BackColor1.ColorSchemePart.ToString()).GetValue(ጎ, null));
		}
		if (BackColor2.ColorSchemePart != eColorSchemePart.Custom && BackColor2.ColorSchemePart != eColorSchemePart.None)
		{
			BackColor2.ᗒ((Color)ጎ.GetType().GetProperty(BackColor2.ColorSchemePart.ToString()).GetValue(ጎ, null));
		}
		if (BorderColor.ColorSchemePart != eColorSchemePart.Custom && BorderColor.ColorSchemePart != eColorSchemePart.None)
		{
			BorderColor.ᗒ((Color)ጎ.GetType().GetProperty(BorderColor.ColorSchemePart.ToString()).GetValue(ጎ, null));
		}
		if (ForeColor.ColorSchemePart != eColorSchemePart.Custom && ForeColor.ColorSchemePart != eColorSchemePart.None)
		{
			ForeColor.ᗒ((Color)ጎ.GetType().GetProperty(ForeColor.ColorSchemePart.ToString()).GetValue(ጎ, null));
		}
	}

	private void ᗆ(object _٠, EventArgs ؿ)
	{
		ᗇ();
	}

	private void ᗇ()
	{
		if (this.ઐ != null)
		{
			this.ઐ(this, new EventArgs());
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor1()
	{
		return !ᖻ.IsEmpty;
	}

	public void ResetBackColor1()
	{
		ᖻ.ᗆ -= ᗆ;
		ᖻ = ColorEx.Empty;
		ᖻ.ᗆ += ᗆ;
	}

	internal void ᗈ(ColorEx _0BDE)
	{
		ᖻ = _0BDE;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor2()
	{
		return !ތ.IsEmpty;
	}

	public void ResetBackColor2()
	{
		ތ.ᗆ -= ᗆ;
		ތ = ColorEx.Empty;
		ތ.ᗆ += ᗆ;
	}

	internal void ᗉ(ColorEx _0BDE)
	{
		ތ = _0BDE;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeForeColor()
	{
		return !_0D02.IsEmpty;
	}

	internal Region ᗊ(Rectangle ᗋ)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		if (_07BE == eCornerType.Rounded && _߃ > 1)
		{
			IntPtr intPtr = _0EFE.CreateRoundRectRgn(ᗋ.X, ᗋ.Y, ᗋ.Width, ᗋ.Height, _߃, _߃);
			Region result = Region.FromHrgn(intPtr);
			_0EFE.DeleteObject(intPtr);
			return result;
		}
		ᗋ.Width++;
		ᗋ.Height++;
		GraphicsPath val = ഗ(ᗋ);
		Region val2 = new Region();
		val2.MakeEmpty();
		val2.Union(val);
		if (ᖾ != 0 && ᗀ > 0 && _07BE != eCornerType.Square && !_07A8.IsEmpty)
		{
			Pen val3 = new Pen(Color.Black, (float)ᗀ);
			try
			{
				val.Widen(val3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			new Region(val);
			val2.Union(val);
		}
		else if (_07BE == eCornerType.Rounded)
		{
			val.Widen(SystemPens.Control);
			new Region(val);
			val2.Union(val);
		}
		return val2;
	}

	private GraphicsPath ഗ(Rectangle ᗋ)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		GraphicsPath val = null;
		if (_07BE != eCornerType.Square)
		{
			ᗋ.Width--;
			ᗋ.Height--;
		}
		switch (_07BE)
		{
		case eCornerType.Square:
			val = new GraphicsPath();
			val.AddRectangle(ᗋ);
			break;
		case eCornerType.Rounded:
			val = DisplayHelp.GetRoundedRectanglePath(ᗋ, _߃);
			break;
		case eCornerType.Diagonal:
			val = new GraphicsPath();
			val.AddLine(ᗋ.X, ᗋ.Bottom - _߃, ᗋ.X, ᗋ.Y + _߃);
			val.AddLine(ᗋ.X + _߃, ᗋ.Y, ᗋ.Right - _߃, ᗋ.Y);
			val.AddLine(ᗋ.Right, ᗋ.Y + _߃, ᗋ.Right, ᗋ.Bottom - _߃);
			val.AddLine(ᗋ.Right - _߃, ᗋ.Bottom, ᗋ.X + _߃, ᗋ.Bottom);
			val.CloseAllFigures();
			break;
		}
		return val;
	}

	public void ResetBackgroundImage()
	{
		ޏ = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return !_07A8.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		_07A8.ᗆ -= ᗆ;
		_07A8 = ColorEx.Empty;
		_07A8.ᗆ += ᗆ;
	}

	public void Paint(Graphics g, Rectangle r, string text, Rectangle textRect, Font font)
	{
		Paint(g, r, text, textRect, font, null);
	}

	private void ᗌ(ref Rectangle ᗍ)
	{
		if (ᖾ != 0 && !_07A8.IsEmpty)
		{
			int num = 1;
			if (ᗀ > 1)
			{
				num = ᗀ / 2;
			}
			else if (ᗀ == 0)
			{
				num = 0;
			}
			if ((ᖿ & eBorderSide.Top) == eBorderSide.Top)
			{
				ᗍ.Y += num;
				ᗍ.Height -= num;
			}
			if ((ᖿ & eBorderSide.Bottom) == eBorderSide.Bottom)
			{
				ᗍ.Height -= num;
			}
			if ((ᖿ & eBorderSide.Left) == eBorderSide.Left)
			{
				ᗍ.X += num;
				ᗍ.Width -= num;
			}
			if ((ᖿ & eBorderSide.Right) == eBorderSide.Right)
			{
				ᗍ.Width -= num;
			}
		}
	}

	public void Paint(Graphics g, Rectangle r, string text, Rectangle textRect, Font font, Point[] borderShape)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Expected O, but got Unknown
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		if (r.Width == 0 || r.Height == 0)
		{
			return;
		}
		Rectangle ᗍ = r;
		if ((int)g.SmoothingMode == 4)
		{
			ᗍ.Inflate(1, 1);
		}
		ᗌ(ref ᗍ);
		GraphicsPath val = ഗ(ᗍ);
		if (!ᖻ.IsEmpty)
		{
			if (!ތ.IsEmpty)
			{
				Color ॵ = Color.FromArgb(ᖻ.Alpha, ᖻ.Color);
				Color ॶ = Color.FromArgb(ތ.Alpha, ތ.Color);
				Rectangle ल = r;
				LinearGradientBrush val2 = BarFunctions.ॴ(ल, ॵ, ॶ, ᖼ, ॸ: false);
				g.FillPath((Brush)(object)val2, val);
				((Brush)val2).Dispose();
			}
			else if (ᖻ.Color != Color.Transparent)
			{
				Color color = Color.FromArgb(ᖻ.Alpha, ᖻ.Color);
				SolidBrush val3 = new SolidBrush(color);
				g.FillPath((Brush)(object)val3, val);
				((Brush)val3).Dispose();
			}
		}
		val.Dispose();
		Rectangle ᗍ2 = r;
		ᗍ2.Width--;
		ᗍ2.Height--;
		ᗌ(ref ᗍ2);
		val = ഗ(ᗍ2);
		if (ޏ != null)
		{
			Region clip = g.Clip;
			GraphicsPath val4 = ഗ(r);
			try
			{
				g.SetClip(val4);
				BarFunctions._0982(g, r, ޏ, ސ, ޑ);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			g.Clip = clip;
			if (clip != null)
			{
				clip.Dispose();
			}
		}
		if (ᖾ != 0 && !_07A8.IsEmpty)
		{
			if (_07BE != eCornerType.Square)
			{
				if ((int)ᗂ == 0)
				{
					g.SmoothingMode = (SmoothingMode)2;
				}
				Pen val5 = new Pen(_07A8.GetCompositeColor(), (float)ᗀ);
				try
				{
					val5.DashStyle = ᗂ;
					g.DrawPath(val5, val);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				if ((int)ᗂ == 0)
				{
					g.SmoothingMode = (SmoothingMode)0;
				}
			}
			else if (ᖾ == eBorderType.SingleLine && borderShape != null && borderShape.Length > 0)
			{
				Pen val6 = new Pen(_07A8.GetCompositeColor(), (float)ᗀ);
				try
				{
					val6.DashStyle = ᗂ;
					g.DrawLines(val6, borderShape);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			else
			{
				switch (ᖾ)
				{
				case eBorderType.SingleLine:
				case eBorderType.DoubleLine:
				case eBorderType.Etched:
				case eBorderType.Bump:
					BarFunctions._२(g, ᖾ, r, _07A8.IsEmpty ? SystemColors.ControlText : Color.FromArgb(_07A8.Alpha, _07A8.Color), ᖿ, ᗂ, ᗀ);
					break;
				case eBorderType.Raised:
					BarFunctions._८((ᖿ != eBorderSide.All) ? ((Border3DSide)((((ᖿ & eBorderSide.Left) != 0) ? 1 : 0) | (((ᖿ & eBorderSide.Right) != 0) ? 4 : 0) | (((ᖿ & eBorderSide.Top) != 0) ? 2 : 0) | (((ᖿ & eBorderSide.Bottom) != 0) ? 8 : 0))) : ((Border3DSide)2063), g, ल: r, (Border3DStyle)4, ॲ: _07A8.IsEmpty ? ᖻ.Color : Color.FromArgb(_07A8.Alpha, _07A8.Color), ॳ: false);
					break;
				case eBorderType.RaisedInner:
				{
					Pen val7 = new Pen(Color.White);
					try
					{
						if ((ᖿ & eBorderSide.Top) == eBorderSide.Top)
						{
							g.DrawLine(val7, r.X, r.Y, r.Right, r.Y);
						}
						if ((ᖿ & eBorderSide.Left) == eBorderSide.Left)
						{
							g.DrawLine(val7, r.X, r.Y, r.X, r.Bottom);
						}
					}
					finally
					{
						((IDisposable)val7)?.Dispose();
					}
					Pen val8 = new Pen(_07A8.GetCompositeColor());
					try
					{
						if ((ᖿ & eBorderSide.Right) == eBorderSide.Right)
						{
							g.DrawLine(val8, r.Right - 1, r.Y, r.Right - 1, r.Bottom);
						}
						if ((ᖿ & eBorderSide.Bottom) == eBorderSide.Bottom)
						{
							g.DrawLine(val8, r.X, r.Bottom - 1, r.Right, r.Bottom - 1);
						}
					}
					finally
					{
						((IDisposable)val8)?.Dispose();
					}
					break;
				}
				case eBorderType.Sunken:
					BarFunctions._८((ᖿ != eBorderSide.All) ? ((Border3DSide)((((ᖿ & eBorderSide.Left) != 0) ? 1 : 0) | (((ᖿ & eBorderSide.Right) != 0) ? 4 : 0) | (((ᖿ & eBorderSide.Top) != 0) ? 2 : 0) | (((ᖿ & eBorderSide.Bottom) != 0) ? 8 : 0))) : ((Border3DSide)2063), g, ल: r, (Border3DStyle)2, ॲ: _07A8.IsEmpty ? ᖻ.Color : Color.FromArgb(_07A8.Alpha, _07A8.Color), ॳ: false);
					break;
				}
			}
		}
		if (text != "" && !_0D02.IsEmpty)
		{
			if (ޓ != null)
			{
				font = ޓ;
			}
			Color color2 = Color.FromArgb(_0D02.Alpha, _0D02.Color);
			if (ޜ != 0)
			{
				textRect.X += ޜ;
				textRect.Width -= ޜ;
			}
			if (ޝ != 0)
			{
				textRect.Width -= ޝ;
			}
			if (ޞ != 0)
			{
				textRect.Y += ޞ;
				textRect.Height -= ޞ;
			}
			if (ޟ != 0)
			{
				textRect.Height -= ޟ;
			}
			if (textRect.Width > 0 && textRect.Height > 0)
			{
				if (ᗃ)
				{
					g.RotateTransform(-90f);
					TextDrawing.DrawStringLegacy(g, text, font, color2, new Rectangle(-textRect.Bottom, textRect.X, textRect.Height, textRect.Width), ᗎ());
					g.ResetTransform();
				}
				else
				{
					TextDrawing.DrawString(g, text, font, color2, textRect, ᗎ());
				}
			}
		}
		if (val != null)
		{
			val.Dispose();
		}
		if (_07BE != eCornerType.Square)
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
		if (ޓ != null)
		{
			font = ޓ;
		}
		Color color = Color.FromArgb(_0D02.Alpha, _0D02.Color);
		if (ޜ != 0)
		{
			textRect.X += ޜ;
			textRect.Width -= ޜ;
		}
		if (ޝ != 0)
		{
			textRect.Width -= ޝ;
		}
		if (ޞ != 0)
		{
			textRect.Y += ޞ;
			textRect.Height -= ޞ;
		}
		if (ޟ != 0)
		{
			textRect.Height -= ޟ;
		}
		if (textRect.Width > 0 && textRect.Height > 0)
		{
			if (ᗃ)
			{
				g.RotateTransform(-90f);
				TextDrawing.DrawStringLegacy(g, text, font, color, new Rectangle(-textRect.Bottom, textRect.X, textRect.Height, textRect.Width), ᗎ());
				g.ResetTransform();
			}
			else
			{
				TextDrawing.DrawString(g, text, font, color, textRect, ᗎ());
			}
		}
	}

	internal eTextFormat ᗎ()
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
		if ((int)ᓹ == 1)
		{
			eTextFormat2 |= eTextFormat.HorizontalCenter;
		}
		else if ((int)ᓹ == 2)
		{
			eTextFormat2 |= eTextFormat.Right;
		}
		if ((int)ᖽ == 1)
		{
			eTextFormat2 |= eTextFormat.VerticalCenter;
		}
		else if ((int)ᖽ == 2)
		{
			eTextFormat2 |= eTextFormat.Bottom;
		}
		eTextFormat2 = (ݥ ? (eTextFormat2 | eTextFormat.WordBreak) : (eTextFormat2 | eTextFormat.SingleLine));
		if ((int)ޖ != 0)
		{
			eTextFormat2 |= eTextFormat.EndEllipsis;
		}
		if (!_ߍ)
		{
			eTextFormat2 |= eTextFormat.NoPrefix;
		}
		return eTextFormat2;
	}

	internal void ణ(XmlElement ᗏ)
	{
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Expected I4, but got Unknown
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Invalid comparison between Unknown and I4
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected I4, but got Unknown
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Invalid comparison between Unknown and I4
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected I4, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Expected I4, but got Unknown
		if (!ᖻ.IsEmpty)
		{
			if (ᖻ.ColorSchemePart == eColorSchemePart.Custom)
			{
				ᗏ.SetAttribute("bc1", BarFunctions.ColorToString(ᖻ.Color));
				ᗏ.SetAttribute("bc1a", ᖻ.Alpha.ToString());
			}
			else
			{
				ᗏ.SetAttribute("bc1csp", ᖻ.ColorSchemePart.ToString());
			}
		}
		if (!ތ.IsEmpty)
		{
			if (ތ.ColorSchemePart == eColorSchemePart.Custom)
			{
				ᗏ.SetAttribute("bc2", BarFunctions.ColorToString(ތ.Color));
				ᗏ.SetAttribute("bc2a", ތ.Alpha.ToString());
			}
			else
			{
				ᗏ.SetAttribute("bc2csp", ތ.ColorSchemePart.ToString());
			}
		}
		if (!_0D02.IsEmpty)
		{
			if (_0D02.ColorSchemePart == eColorSchemePart.Custom)
			{
				ᗏ.SetAttribute("fc", BarFunctions.ColorToString(_0D02.Color));
				ᗏ.SetAttribute("fca", _0D02.Alpha.ToString());
			}
			else
			{
				ᗏ.SetAttribute("fccsp", _0D02.ColorSchemePart.ToString());
			}
		}
		if (!_07A8.IsEmpty)
		{
			if (_07A8.ColorSchemePart == eColorSchemePart.Custom)
			{
				ᗏ.SetAttribute("borderc", BarFunctions.ColorToString(_07A8.Color));
				ᗏ.SetAttribute("borderca", _07A8.Alpha.ToString());
			}
			else
			{
				ᗏ.SetAttribute("bordercsp", _07A8.ColorSchemePart.ToString());
			}
		}
		ᗏ.SetAttribute("ga", ᖼ.ToString());
		if (ޓ != null)
		{
			ᗏ.SetAttribute("fontname", ޓ.Name);
			ᗏ.SetAttribute("fontemsize", XmlConvert.ToString(ޓ.Size));
			ᗏ.SetAttribute("fontstyle", XmlConvert.ToString((int)ޓ.Style));
		}
		if (ޏ != null)
		{
			XmlElement xmlElement = ᗏ.OwnerDocument.CreateElement("backimage");
			ᗏ.AppendChild(xmlElement);
			BarFunctions.SerializeImage(ޏ, xmlElement);
			int num = (int)ސ;
			xmlElement.SetAttribute("pos", num.ToString());
			xmlElement.SetAttribute("alpha", ޑ.ToString());
		}
		if (ݥ)
		{
			ᗏ.SetAttribute("wordwrap", XmlConvert.ToString(ݥ));
		}
		if ((int)ᓹ != 0)
		{
			ᗏ.SetAttribute("align", XmlConvert.ToString((int)ᓹ));
		}
		if ((int)ᖽ != 1)
		{
			ᗏ.SetAttribute("valign", XmlConvert.ToString((int)ᖽ));
		}
		if ((int)ޖ != 3)
		{
			ᗏ.SetAttribute("trim", XmlConvert.ToString((int)ޖ));
		}
		if (ᖾ != 0)
		{
			ᗏ.SetAttribute("border", XmlConvert.ToString((int)ᖾ));
		}
		if (ᗀ != 1)
		{
			ᗏ.SetAttribute("borderw", ᗀ.ToString());
		}
		if (_߃ != 8)
		{
			ᗏ.SetAttribute("cornerdiameter", _߃.ToString());
		}
		if (_07BE != eCornerType.Square)
		{
			ᗏ.SetAttribute("cornertype", XmlConvert.ToString((int)_07BE));
		}
	}

	internal void ద(XmlElement ᗏ)
	{
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Expected O, but got Unknown
		ᖻ = ColorEx.Empty;
		if (ᗏ.HasAttribute("bc1"))
		{
			ᖻ.Color = BarFunctions.ColorFromString(ᗏ.GetAttribute("bc1"));
			ᖻ.Alpha = XmlConvert.ToByte(ᗏ.GetAttribute("bc1a"));
		}
		else if (ᗏ.HasAttribute("bc1csp"))
		{
			ᖻ.ColorSchemePart = (eColorSchemePart)Enum.Parse(typeof(eColorSchemePart), ᗏ.GetAttribute("bc1csp"));
		}
		ތ = ColorEx.Empty;
		if (ᗏ.HasAttribute("bc2"))
		{
			ތ.Color = BarFunctions.ColorFromString(ᗏ.GetAttribute("bc2"));
			ތ.Alpha = XmlConvert.ToByte(ᗏ.GetAttribute("bc2a"));
		}
		else if (ᗏ.HasAttribute("bc2csp"))
		{
			ތ.ColorSchemePart = (eColorSchemePart)Enum.Parse(typeof(eColorSchemePart), ᗏ.GetAttribute("bc2csp"));
		}
		_0D02 = ColorEx.Empty;
		if (ᗏ.HasAttribute("fc"))
		{
			_0D02.Color = BarFunctions.ColorFromString(ᗏ.GetAttribute("fc"));
			_0D02.Alpha = XmlConvert.ToByte(ᗏ.GetAttribute("fca"));
		}
		else if (ᗏ.HasAttribute("fccsp"))
		{
			_0D02.ColorSchemePart = (eColorSchemePart)Enum.Parse(typeof(eColorSchemePart), ᗏ.GetAttribute("fccsp"));
		}
		_07A8 = ColorEx.Empty;
		if (ᗏ.HasAttribute("borderc"))
		{
			_07A8.Color = BarFunctions.ColorFromString(ᗏ.GetAttribute("borderc"));
			_07A8.Alpha = XmlConvert.ToByte(ᗏ.GetAttribute("borderca"));
		}
		else if (ᗏ.HasAttribute("bordercsp"))
		{
			_07A8.ColorSchemePart = (eColorSchemePart)Enum.Parse(typeof(eColorSchemePart), ᗏ.GetAttribute("bordercsp"));
		}
		ᖼ = XmlConvert.ToInt16(ᗏ.GetAttribute("ga"));
		ޓ = null;
		if (ᗏ.HasAttribute("fontname"))
		{
			string attribute = ᗏ.GetAttribute("fontname");
			float num = XmlConvert.ToSingle(ᗏ.GetAttribute("fontemsize"));
			FontStyle val = (FontStyle)XmlConvert.ToInt32(ᗏ.GetAttribute("fontstyle"));
			try
			{
				ޓ = new Font(attribute, num, val);
			}
			catch (Exception)
			{
				ޓ = null;
			}
		}
		ޏ = null;
		foreach (XmlElement childNode in ᗏ.ChildNodes)
		{
			if (childNode.Name == "backimage")
			{
				ޏ = BarFunctions.DeserializeImage(childNode);
				ސ = (eBackgroundImagePosition)XmlConvert.ToInt32(childNode.GetAttribute("pos"));
				ޑ = XmlConvert.ToByte(childNode.GetAttribute("alpha"));
			}
		}
		ݥ = false;
		if (ᗏ.HasAttribute("wordwrap"))
		{
			ݥ = XmlConvert.ToBoolean(ᗏ.GetAttribute("wordwrap"));
		}
		ᓹ = (StringAlignment)0;
		if (ᗏ.HasAttribute("align"))
		{
			ᓹ = (StringAlignment)XmlConvert.ToInt32(ᗏ.GetAttribute("align"));
		}
		ᖽ = (StringAlignment)1;
		if (ᗏ.HasAttribute("valign"))
		{
			ᖽ = (StringAlignment)XmlConvert.ToInt32(ᗏ.GetAttribute("valign"));
		}
		ޖ = (StringTrimming)3;
		if (ᗏ.HasAttribute("trim"))
		{
			ޖ = (StringTrimming)XmlConvert.ToInt32(ᗏ.GetAttribute("trim"));
		}
		ᖾ = eBorderType.None;
		if (ᗏ.HasAttribute("border"))
		{
			ᖾ = (eBorderType)XmlConvert.ToInt32(ᗏ.GetAttribute("border"));
		}
		ᗀ = 1;
		if (ᗏ.HasAttribute("borderw"))
		{
			ᗀ = XmlConvert.ToInt32(ᗏ.GetAttribute("borderw"));
		}
		_07BE = eCornerType.Square;
		if (ᗏ.HasAttribute("cornertype"))
		{
			_07BE = (eCornerType)XmlConvert.ToInt32(ᗏ.GetAttribute("cornertype"));
		}
		_߃ = 8;
		if (ᗏ.HasAttribute("cornerdiameter"))
		{
			_߃ = XmlConvert.ToInt32(ᗏ.GetAttribute("cornerdiameter"));
		}
	}
}
