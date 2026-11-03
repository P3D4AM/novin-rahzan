using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class OfficeListBoxItemStateColorTable
{
	public Color TextColor = Color.Black;

	public Color[] BackColors = new Color[0];

	public int BackColorsGradientAngle = 90;

	public float[] BackColorsPositions = new float[0];

	public OfficeListBoxItemStateColorTable()
	{
	}

	public OfficeListBoxItemStateColorTable(Color textColor, Color[] backColors, int backColorsGradientAngle, float[] backColorsPositions)
	{
		TextColor = textColor;
		BackColors = backColors;
		BackColorsGradientAngle = backColorsGradientAngle;
		BackColorsPositions = backColorsPositions;
	}

	public OfficeListBoxItemStateColorTable(Color textColor)
	{
		TextColor = textColor;
	}
}
