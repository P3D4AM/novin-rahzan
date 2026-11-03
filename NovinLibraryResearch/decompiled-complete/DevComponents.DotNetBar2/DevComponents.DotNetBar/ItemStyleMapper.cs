using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
public class ItemStyleMapper
{
	private ElementStyle ᵛ;

	private ColorExMapper ᖻ;

	private ColorExMapper ތ;

	private ColorExMapper _0D02;

	private ColorExMapper _07A8;

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ColorExMapper BackColor1 => ᖻ;

	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public ColorExMapper BackColor2 => ތ;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ColorExMapper ForeColor => _0D02;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public int GradientAngle
	{
		get
		{
			return ᵛ.BackColorGradientAngle;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["BackColorGradientAngle"].SetValue(ᵛ, value);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public Font Font
	{
		get
		{
			return ᵛ.Font;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["Font"].SetValue(ᵛ, value);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	public bool WordWrap
	{
		get
		{
			return ᵛ.WordWrap;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["WordWrap"].SetValue(ᵛ, value);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public StringAlignment Alignment
	{
		get
		{
			return (StringAlignment)ᵛ.TextAlignment;
		}
		set
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected I4, but got Unknown
			TypeDescriptor.GetProperties(ᵛ)["TextAlignment"].SetValue(ᵛ, (eStyleTextAlignment)value);
		}
	}

	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public StringAlignment LineAlignment
	{
		get
		{
			return (StringAlignment)ᵛ.TextLineAlignment;
		}
		set
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected I4, but got Unknown
			TypeDescriptor.GetProperties(ᵛ)["TextLineAlignment"].SetValue(ᵛ, (eStyleTextAlignment)value);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public StringTrimming TextTrimming
	{
		get
		{
			return (StringTrimming)ᵛ.TextTrimming;
		}
		set
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected I4, but got Unknown
			TypeDescriptor.GetProperties(ᵛ)["TextTrimming"].SetValue(ᵛ, (eStyleTextTrimming)value);
		}
	}

	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Image BackgroundImage
	{
		get
		{
			return ᵛ.BackgroundImage;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["BackgroundImage"].SetValue(ᵛ, value);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public eBackgroundImagePosition BackgroundImagePosition
	{
		get
		{
			return (eBackgroundImagePosition)ᵛ.BackgroundImagePosition;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["BackgroundImagePosition"].SetValue(ᵛ, (eStyleBackgroundImage)value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public byte BackgroundImageAlpha
	{
		get
		{
			return ᵛ.BackgroundImageAlpha;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["BackgroundImageAlpha"].SetValue(ᵛ, value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	public eCornerType CornerType
	{
		get
		{
			return ᵛ.CornerType;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["CornerType"].SetValue(ᵛ, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int CornerDiameter
	{
		get
		{
			return ᵛ.CornerDiameter;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["CornerDiameter"].SetValue(ᵛ, value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DevCoBrowsable(false)]
	public eBorderType Border
	{
		get
		{
			return eBorderType.None;
		}
		set
		{
			eStyleBorderType eStyleBorderType2 = eStyleBorderType.None;
			if (value != 0)
			{
				eStyleBorderType2 = eStyleBorderType.Solid;
			}
			TypeDescriptor.GetProperties(ᵛ)["BorderLeft"].SetValue(ᵛ, eStyleBorderType2);
			TypeDescriptor.GetProperties(ᵛ)["BorderTop"].SetValue(ᵛ, eStyleBorderType2);
			TypeDescriptor.GetProperties(ᵛ)["BorderRight"].SetValue(ᵛ, eStyleBorderType2);
			TypeDescriptor.GetProperties(ᵛ)["BorderBottom"].SetValue(ᵛ, eStyleBorderType2);
		}
	}

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DashStyle BorderDashStyle
	{
		get
		{
			return (DashStyle)0;
		}
		set
		{
		}
	}

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eBorderSide BorderSide
	{
		get
		{
			return eBorderSide.None;
		}
		set
		{
			if ((value & eBorderSide.Left) == eBorderSide.Left)
			{
				TypeDescriptor.GetProperties(ᵛ)["BorderLeft"].SetValue(ᵛ, eStyleBorderType.Solid);
			}
			else
			{
				TypeDescriptor.GetProperties(ᵛ)["BorderLeft"].SetValue(ᵛ, eStyleBorderType.None);
			}
			if ((value & eBorderSide.Right) == eBorderSide.Right)
			{
				TypeDescriptor.GetProperties(ᵛ)["BorderRight"].SetValue(ᵛ, eStyleBorderType.Solid);
			}
			else
			{
				TypeDescriptor.GetProperties(ᵛ)["BorderRight"].SetValue(ᵛ, eStyleBorderType.None);
			}
			if ((value & eBorderSide.Top) == eBorderSide.Top)
			{
				TypeDescriptor.GetProperties(ᵛ)["BorderTop"].SetValue(ᵛ, eStyleBorderType.Solid);
			}
			else
			{
				TypeDescriptor.GetProperties(ᵛ)["BorderTop"].SetValue(ᵛ, eStyleBorderType.None);
			}
			if ((value & eBorderSide.Bottom) == eBorderSide.Bottom)
			{
				TypeDescriptor.GetProperties(ᵛ)["BorderBottom"].SetValue(ᵛ, eStyleBorderType.Solid);
			}
			else
			{
				TypeDescriptor.GetProperties(ᵛ)["BorderBottom"].SetValue(ᵛ, eStyleBorderType.None);
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ColorExMapper BorderColor => _07A8;

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int BorderWidth
	{
		get
		{
			return ᵛ.BorderLeftWidth;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["BorderLeftWidth"].SetValue(ᵛ, value);
			TypeDescriptor.GetProperties(ᵛ)["BorderTopWidth"].SetValue(ᵛ, value);
			TypeDescriptor.GetProperties(ᵛ)["BorderRightWidth"].SetValue(ᵛ, value);
			TypeDescriptor.GetProperties(ᵛ)["BorderBottomWidth"].SetValue(ᵛ, value);
		}
	}

	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int MarginLeft
	{
		get
		{
			return ᵛ.MarginLeft;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["MarginLeft"].SetValue(ᵛ, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int MarginRight
	{
		get
		{
			return ᵛ.MarginRight;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["MarginRight"].SetValue(ᵛ, value);
		}
	}

	[DevCoBrowsable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int MarginTop
	{
		get
		{
			return ᵛ.MarginTop;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["MarginTop"].SetValue(ᵛ, value);
		}
	}

	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int MarginBottom
	{
		get
		{
			return ᵛ.MarginBottom;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)["MarginBottom"].SetValue(ᵛ, value);
		}
	}

	public ItemStyleMapper(ElementStyle style)
	{
		ᵛ = style;
		ᖻ = new ColorExMapper("BackColor", style);
		ތ = new ColorExMapper("BackColor2", style);
		_0D02 = new ColorExMapper("TextColor", style);
		_07A8 = new ColorExMapper("BorderColor", style);
	}
}
