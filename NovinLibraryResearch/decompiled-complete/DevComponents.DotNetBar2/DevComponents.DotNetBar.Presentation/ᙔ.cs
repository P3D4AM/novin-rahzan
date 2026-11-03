using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Presentation;

internal class ᙔ
{
	public int ܐ;

	public Color ᙕ = Color.Empty;

	public Color ᙖ = Color.Empty;

	public int ᙗ = 90;

	public ᙔ()
	{
	}

	public ᙔ(int borderWidth)
	{
		ܐ = borderWidth;
	}

	public ᙔ(LinearGradientColorTable table)
	{
		ᙕ = table.Start;
		ᙖ = table.End;
		ܐ = 1;
	}

	public ᙔ(LinearGradientColorTable table, int width)
	{
		ᙕ = table.Start;
		ᙖ = table.End;
		ܐ = width;
	}

	public void ᙘ(LinearGradientColorTable ቯ)
	{
		if (ቯ == null)
		{
			ᙕ = Color.Empty;
			ᙖ = Color.Empty;
		}
		else
		{
			ᙕ = ቯ.Start;
			ᙖ = ቯ.End;
		}
	}

	public ᙔ(Color color1, Color color2, int width)
	{
		ᙕ = color1;
		ᙖ = color2;
		ܐ = width;
	}

	public ᙔ(Color color1, int width)
	{
		ᙕ = color1;
		ᙖ = Color.Empty;
		ܐ = width;
	}
}
