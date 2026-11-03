using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class TabCloseButtonStateColorTable
{
	public Color[] BackColors = new Color[0];

	public int BackColorsGradientAngle = 90;

	public float[] BackColorsPositions = new float[0];

	public Color ForeColor = Color.Empty;

	public Color BorderColor = Color.Empty;

	public TabCloseButtonStateColorTable()
	{
	}

	public TabCloseButtonStateColorTable(Color[] backColors, Color foreColor, Color borderColor)
	{
		BackColors = backColors;
		ForeColor = foreColor;
		BorderColor = borderColor;
	}
}
