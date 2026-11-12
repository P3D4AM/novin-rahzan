using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class ColorExMapper
{
	private ElementStyle m_ElementStyle;

	private string m_ColorProperty = "";

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color Color
	{
		get
		{
			return (Color)TypeDescriptor.GetProperties(m_ElementStyle)[m_ColorProperty].GetValue(m_ElementStyle);
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)[m_ColorProperty].SetValue(m_ElementStyle, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public byte Alpha
	{
		get
		{
			return ((Color)TypeDescriptor.GetProperties(m_ElementStyle)[m_ColorProperty].GetValue(m_ElementStyle)).A;
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)[m_ColorProperty].SetValue(m_ElementStyle, Color.FromArgb(value, Color));
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eColorSchemePart ColorSchemePart
	{
		get
		{
			return (eColorSchemePart)TypeDescriptor.GetProperties(m_ElementStyle)[m_ColorProperty + "SchemePart"].GetValue(m_ElementStyle);
		}
		set
		{
			TypeDescriptor.GetProperties(m_ElementStyle)[m_ColorProperty + "SchemePart"].SetValue(m_ElementStyle, value);
		}
	}

	public ColorExMapper(string colorProperty, ElementStyle style)
	{
		m_ColorProperty = colorProperty;
		m_ElementStyle = style;
	}
}
