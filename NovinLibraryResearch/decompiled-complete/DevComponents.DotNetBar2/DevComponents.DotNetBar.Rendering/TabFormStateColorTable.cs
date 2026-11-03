using System.Drawing;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar.Rendering;

public class TabFormStateColorTable
{
	public Color[] CaptionBackColors = new Color[0];

	public int CaptionBackColorsGradientAngle = 90;

	public float[] CaptionBackColorsPositions = new float[0];

	public Color CaptionText = Color.Empty;

	public BorderColors BorderColors = new BorderColors(ColorScheme.GetColor("696969"));

	public TabFormStateColorTable()
	{
	}

	public TabFormStateColorTable(Color[] captionBackColors, Color captionText, BorderColors borderColors)
	{
		CaptionBackColors = captionBackColors;
		CaptionText = captionText;
		BorderColors = borderColors;
	}
}
