using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class Office2010RangeChangePartColorTable
{
	public GradientColorTable Background;

	public Color BorderColor = Color.Empty;

	public Color BorderLightColor = Color.Empty;

	public Office2010RangeChangePartColorTable()
	{
	}

	public Office2010RangeChangePartColorTable(GradientColorTable background, Color borderColor, Color borderLightColor)
	{
		Background = background;
		BorderColor = borderColor;
		BorderLightColor = borderLightColor;
	}
}
