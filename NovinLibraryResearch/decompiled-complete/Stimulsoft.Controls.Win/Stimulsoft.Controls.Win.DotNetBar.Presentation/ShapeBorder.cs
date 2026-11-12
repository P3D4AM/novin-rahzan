using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Presentation;

internal class ShapeBorder
{
	public int Width;

	public Color Color1 = Color.Empty;

	public Color Color2 = Color.Empty;

	public int GradientAngle = 90;

	public ShapeBorder()
	{
	}

	public ShapeBorder(int borderWidth)
	{
		Width = borderWidth;
	}

	public ShapeBorder(LinearGradientColorTable table)
	{
		Color1 = table.Start;
		Color2 = table.End;
		Width = 1;
	}

	public ShapeBorder(LinearGradientColorTable table, int width)
	{
		Color1 = table.Start;
		Color2 = table.End;
		Width = width;
	}

	public void Apply(LinearGradientColorTable table)
	{
		if (table == null)
		{
			Color1 = Color.Empty;
			Color2 = Color.Empty;
		}
		else
		{
			Color1 = table.Start;
			Color2 = table.End;
		}
	}

	public ShapeBorder(Color color1, Color color2, int width)
	{
		Color1 = color1;
		Color2 = color2;
		Width = width;
	}

	public ShapeBorder(Color color1, int width)
	{
		Color1 = color1;
		Color2 = Color.Empty;
		Width = width;
	}
}
