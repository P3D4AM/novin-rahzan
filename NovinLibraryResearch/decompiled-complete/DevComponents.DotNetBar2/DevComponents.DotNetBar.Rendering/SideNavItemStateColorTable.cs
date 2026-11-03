using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class SideNavItemStateColorTable
{
	public Color TextColor = Color.Black;

	public Color[] BackColors = new Color[0];

	public int BackColorsGradientAngle = 90;

	public float[] BackColorsPositions = new float[0];

	public Color[] BorderColors = new Color[0];

	public int CornerRadius;

	public SideNavItemStateColorTable()
	{
	}

	public SideNavItemStateColorTable(Color textColor)
	{
		TextColor = textColor;
	}

	public SideNavItemStateColorTable(Color textColor, Color[] backColors, Color[] borderColors)
	{
		TextColor = textColor;
		BackColors = backColors;
		BorderColors = borderColors;
	}

	public SideNavItemStateColorTable(Color textColor, Color[] backColors, Color[] borderColors, int cornerRadius)
	{
		TextColor = textColor;
		BackColors = backColors;
		BorderColors = borderColors;
		CornerRadius = cornerRadius;
	}
}
