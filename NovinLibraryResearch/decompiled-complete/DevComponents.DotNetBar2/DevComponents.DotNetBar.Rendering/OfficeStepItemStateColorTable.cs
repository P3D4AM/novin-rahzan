using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class OfficeStepItemStateColorTable
{
	public Color[] BackColors = new Color[0];

	public int BackColorsGradientAngle = 90;

	public float[] BackColorsPositions = new float[0];

	public Color TextColor = Color.Black;

	public Color[] BorderColors = new Color[0];

	public OfficeStepItemStateColorTable()
	{
	}

	public OfficeStepItemStateColorTable(Color[] backColors, Color textColor, Color[] borderColors)
	{
		BackColors = backColors;
		TextColor = textColor;
		BorderColors = borderColors;
	}

	public OfficeStepItemStateColorTable(Color[] backColors, int backColorsGradientAngle, float[] backColorsPositions, Color textColor, Color[] borderColors)
	{
		BackColors = backColors;
		BackColorsGradientAngle = backColorsGradientAngle;
		BackColorsPositions = backColorsPositions;
		TextColor = textColor;
		BorderColors = borderColors;
	}
}
