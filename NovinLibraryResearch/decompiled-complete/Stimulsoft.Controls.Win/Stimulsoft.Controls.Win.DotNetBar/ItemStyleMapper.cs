using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class ItemStyleMapper
{
	private ElementStyle m_ElementStyle;

	private ColorExMapper m_BackColor1;

	private ColorExMapper m_BackColor2;

	private ColorExMapper m_ForeColor;

	private ColorExMapper m_BorderColor;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ColorExMapper BackColor1 => m_BackColor1;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ColorExMapper BackColor2 => m_BackColor2;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ColorExMapper ForeColor => m_ForeColor;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int GradientAngle
	{
		get
		{
			return m_ElementStyle.BackColorGradientAngle;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["BackColorGradientAngle"].SetValue(m_ElementStyle, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Font Font
	{
		get
		{
			return m_ElementStyle.Font;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["Font"].SetValue(m_ElementStyle, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool WordWrap
	{
		get
		{
			return m_ElementStyle.WordWrap;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["WordWrap"].SetValue(m_ElementStyle, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StringAlignment Alignment
	{
		get
		{
			return (StringAlignment)m_ElementStyle.TextAlignment;
		}
		set
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected I4, but got Unknown
			TypeDescriptor.GetProperties(m_ElementStyle)["TextAlignment"].SetValue(m_ElementStyle, (eStyleTextAlignment)value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StringAlignment LineAlignment
	{
		get
		{
			return (StringAlignment)m_ElementStyle.TextLineAlignment;
		}
		set
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected I4, but got Unknown
			TypeDescriptor.GetProperties(m_ElementStyle)["TextLineAlignment"].SetValue(m_ElementStyle, (eStyleTextAlignment)value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StringTrimming TextTrimming
	{
		get
		{
			return (StringTrimming)m_ElementStyle.TextTrimming;
		}
		set
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected I4, but got Unknown
			TypeDescriptor.GetProperties(m_ElementStyle)["TextTrimming"].SetValue(m_ElementStyle, (eStyleTextTrimming)value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Image BackgroundImage
	{
		get
		{
			return m_ElementStyle.BackgroundImage;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["BackgroundImage"].SetValue(m_ElementStyle, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eBackgroundImagePosition BackgroundImagePosition
	{
		get
		{
			return (eBackgroundImagePosition)m_ElementStyle.BackgroundImagePosition;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["BackgroundImagePosition"].SetValue(m_ElementStyle, (eStyleBackgroundImage)value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public byte BackgroundImageAlpha
	{
		get
		{
			return m_ElementStyle.BackgroundImageAlpha;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["BackgroundImageAlpha"].SetValue(m_ElementStyle, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eCornerType CornerType
	{
		get
		{
			return m_ElementStyle.CornerType;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["CornerType"].SetValue(m_ElementStyle, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int CornerDiameter
	{
		get
		{
			return m_ElementStyle.CornerDiameter;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["CornerDiameter"].SetValue(m_ElementStyle, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
			TypeDescriptor.GetProperties(m_ElementStyle)["BorderLeft"].SetValue(m_ElementStyle, eStyleBorderType2);
			TypeDescriptor.GetProperties(m_ElementStyle)["BorderTop"].SetValue(m_ElementStyle, eStyleBorderType2);
			TypeDescriptor.GetProperties(m_ElementStyle)["BorderRight"].SetValue(m_ElementStyle, eStyleBorderType2);
			TypeDescriptor.GetProperties(m_ElementStyle)["BorderBottom"].SetValue(m_ElementStyle, eStyleBorderType2);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
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
				TypeDescriptor.GetProperties(m_ElementStyle)["BorderLeft"].SetValue(m_ElementStyle, eStyleBorderType.Solid);
			}
			else
			{
				TypeDescriptor.GetProperties(m_ElementStyle)["BorderLeft"].SetValue(m_ElementStyle, eStyleBorderType.None);
			}
			if ((value & eBorderSide.Right) == eBorderSide.Right)
			{
				TypeDescriptor.GetProperties(m_ElementStyle)["BorderRight"].SetValue(m_ElementStyle, eStyleBorderType.Solid);
			}
			else
			{
				TypeDescriptor.GetProperties(m_ElementStyle)["BorderRight"].SetValue(m_ElementStyle, eStyleBorderType.None);
			}
			if ((value & eBorderSide.Top) == eBorderSide.Top)
			{
				TypeDescriptor.GetProperties(m_ElementStyle)["BorderTop"].SetValue(m_ElementStyle, eStyleBorderType.Solid);
			}
			else
			{
				TypeDescriptor.GetProperties(m_ElementStyle)["BorderTop"].SetValue(m_ElementStyle, eStyleBorderType.None);
			}
			if ((value & eBorderSide.Bottom) == eBorderSide.Bottom)
			{
				TypeDescriptor.GetProperties(m_ElementStyle)["BorderBottom"].SetValue(m_ElementStyle, eStyleBorderType.Solid);
			}
			else
			{
				TypeDescriptor.GetProperties(m_ElementStyle)["BorderBottom"].SetValue(m_ElementStyle, eStyleBorderType.None);
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ColorExMapper BorderColor => m_BorderColor;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int BorderWidth
	{
		get
		{
			return m_ElementStyle.BorderLeftWidth;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["BorderLeftWidth"].SetValue(m_ElementStyle, value);
			TypeDescriptor.GetProperties(m_ElementStyle)["BorderTopWidth"].SetValue(m_ElementStyle, value);
			TypeDescriptor.GetProperties(m_ElementStyle)["BorderRightWidth"].SetValue(m_ElementStyle, value);
			TypeDescriptor.GetProperties(m_ElementStyle)["BorderBottomWidth"].SetValue(m_ElementStyle, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int MarginLeft
	{
		get
		{
			return m_ElementStyle.MarginLeft;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["MarginLeft"].SetValue(m_ElementStyle, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int MarginRight
	{
		get
		{
			return m_ElementStyle.MarginRight;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["MarginRight"].SetValue(m_ElementStyle, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int MarginTop
	{
		get
		{
			return m_ElementStyle.MarginTop;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["MarginTop"].SetValue(m_ElementStyle, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int MarginBottom
	{
		get
		{
			return m_ElementStyle.MarginBottom;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)["MarginBottom"].SetValue(m_ElementStyle, value);
		}
	}

	public ItemStyleMapper(ElementStyle style)
	{
		m_ElementStyle = style;
		m_BackColor1 = new ColorExMapper("BackColor", style);
		m_BackColor2 = new ColorExMapper("BackColor2", style);
		m_ForeColor = new ColorExMapper("TextColor", style);
		m_BorderColor = new ColorExMapper("BorderColor", style);
	}
}
