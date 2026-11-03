using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class TabFormItemStateColorTable
{
	public Color TextColor = Color.Black;

	public Color[] BackColors = new Color[0];

	public int BackColorsGradientAngle = 90;

	public float[] BackColorsPositions = new float[0];

	public Color[] BorderColors = new Color[0];

	public TabFormItemStateColorTable Clone()
	{
		TabFormItemStateColorTable tabFormItemStateColorTable = new TabFormItemStateColorTable();
		tabFormItemStateColorTable.BorderColors = BorderColors;
		tabFormItemStateColorTable.BackColors = BackColors;
		tabFormItemStateColorTable.TextColor = TextColor;
		tabFormItemStateColorTable.BackColorsGradientAngle = BackColorsGradientAngle;
		tabFormItemStateColorTable.BackColorsPositions = BackColorsPositions;
		return tabFormItemStateColorTable;
	}
}
