using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[TypeConverter(typeof(BackgroundColorBlendConverter))]
public class BackgroundColorBlend
{
	private Color m_Color = Color.Empty;

	private float m_Position;

	[Browsable(true)]
	[Description("Indicates the Color to use in multicolor gradient blend at specified position.")]
	public Color Color
	{
		get
		{
			return m_Color;
		}
		set
		{
			m_Color = value;
			OnColorBlendChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue(0f)]
	[Description("")]
	public float Position
	{
		get
		{
			return m_Position;
		}
		set
		{
			m_Position = value;
			OnColorBlendChanged();
		}
	}

	public BackgroundColorBlend()
	{
	}

	public BackgroundColorBlend(Color color, float position)
	{
		m_Color = color;
		m_Position = position;
	}

	public BackgroundColorBlend(int color, float position)
	{
		m_Color = ColorScheme.GetColor(color);
		m_Position = position;
	}

	private bool ShouldSerializeColor()
	{
		return !m_Color.IsEmpty;
	}

	private void OnColorBlendChanged()
	{
	}
}
