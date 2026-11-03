using System.ComponentModel;
using System.Drawing;

namespace DevComponents.WinForms.Drawing;

[TypeConverter(typeof(ColorStopConverter))]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class ColorStop
{
	private Color __1360 = Color.Empty;

	private float __1361;

	[Browsable(true)]
	[Description("Indicates the Color to use in multicolor gradient blend at specified position.")]
	public Color Color
	{
		get
		{
			return __1360;
		}
		set
		{
			__1360 = value;
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
			return __1361;
		}
		set
		{
			__1361 = value;
			ቷ();
		}
	}

	public ColorStop()
	{
	}

	public ColorStop(Color color, float position)
	{
		__1360 = color;
		__1361 = position;
	}

	private bool ShouldSerializeColor()
	{
		return !__1360.IsEmpty;
	}

	private void ቷ()
	{
	}
}
