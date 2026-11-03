using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
public class ColorExMapper
{
	private ElementStyle ᵛ;

	private string ᵜ = "";

	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Color Color
	{
		get
		{
			return (Color)TypeDescriptor.GetProperties(ᵛ)[ᵜ].GetValue(ᵛ);
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)[ᵜ].SetValue(ᵛ, value);
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public byte Alpha
	{
		get
		{
			return ((Color)TypeDescriptor.GetProperties(ᵛ)[ᵜ].GetValue(ᵛ)).A;
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)[ᵜ].SetValue(ᵛ, Color.FromArgb(value, Color));
		}
	}

	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public eColorSchemePart ColorSchemePart
	{
		get
		{
			return (eColorSchemePart)TypeDescriptor.GetProperties(ᵛ)[ᵜ + "SchemePart"].GetValue(ᵛ);
		}
		set
		{
			TypeDescriptor.GetProperties(ᵛ)[ᵜ + "SchemePart"].SetValue(ᵛ, value);
		}
	}

	public ColorExMapper(string colorProperty, ElementStyle style)
	{
		ᵜ = colorProperty;
		ᵛ = style;
	}
}
