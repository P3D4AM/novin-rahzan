using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class ColorBlendFactory : ColorFactory
{
	private Color _12BF = Color.Empty;

	public ColorBlendFactory(Color blendColor)
	{
		_12BF = blendColor;
	}

	public override Color GetColor(int rgb)
	{
		if (rgb == -1)
		{
			return Color.Empty;
		}
		return Color.FromArgb(ዀ((rgb & 0xFF0000) >> 16, _12BF.R), ዀ((rgb & 0xFF00) >> 8, _12BF.G), ዀ(rgb & 0xFF, _12BF.B));
	}

	public override Color GetColor(int alpha, int rgb)
	{
		if (rgb == -1)
		{
			return Color.Empty;
		}
		return Color.FromArgb(alpha, ዀ((rgb & 0xFF0000) >> 16, _12BF.R), ዀ((rgb & 0xFF00) >> 8, _12BF.G), ዀ(rgb & 0xFF, _12BF.B));
	}

	public override Color GetColor(Color c)
	{
		if (c.IsEmpty)
		{
			return Color.Empty;
		}
		return Color.FromArgb(c.A, ዀ(c.R, _12BF.R), ዀ(c.G, _12BF.G), ዀ(c.B, _12BF.B));
	}

	public override Color GetColor(int alpha, Color c)
	{
		if (c.IsEmpty)
		{
			return Color.Empty;
		}
		return Color.FromArgb(alpha, ዀ(c.R, _12BF.R), ዀ(c.G, _12BF.G), ዀ(c.B, _12BF.B));
	}

	public override Color GetColor(string argb)
	{
		return GetColor(ColorScheme.GetColor(argb));
	}

	internal static int ዀ(int ሽ, int _0BCF)
	{
		int num = 0;
		num = ሽ * _0BCF / 255;
		return num + ሽ * (255 - (255 - ሽ) * (255 - _0BCF) / 255 - num) / 255;
	}

	internal static Color ዀ(Color _0BDE, Color _12C1)
	{
		return Color.FromArgb(ዀ(_0BDE.R, _12C1.R), ዀ(_0BDE.G, _12C1.G), ዀ(_0BDE.B, _12C1.B));
	}
}
