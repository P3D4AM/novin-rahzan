using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[TypeConverter(typeof(BackgroundColorBlendConverter))]
public class BackgroundColorBlend
{
	private Color ൠ = Color.Empty;

	private float ቶ;

	[Description("Indicates the Color to use in multicolor gradient blend at specified position.")]
	[Browsable(true)]
	public Color Color
	{
		get
		{
			return ൠ;
		}
		set
		{
			ൠ = value;
			ቷ();
		}
	}

	[DefaultValue(0f)]
	[Browsable(true)]
	[Description("")]
	public float Position
	{
		get
		{
			return ቶ;
		}
		set
		{
			ቶ = value;
			ቷ();
		}
	}

	public BackgroundColorBlend()
	{
	}

	public BackgroundColorBlend(Color color, float position)
	{
		ൠ = color;
		ቶ = position;
	}

	public BackgroundColorBlend(int color, float position)
	{
		ൠ = ColorScheme.GetColor(color);
		ቶ = position;
	}

	private bool ShouldSerializeColor()
	{
		return !ൠ.IsEmpty;
	}

	private void ቷ()
	{
	}
}
