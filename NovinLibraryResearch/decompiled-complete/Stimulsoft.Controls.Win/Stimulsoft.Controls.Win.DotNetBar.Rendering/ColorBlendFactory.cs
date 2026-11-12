using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public class ColorBlendFactory : ColorFactory
{
	private Color m_BlendColor = Color.Empty;

	public ColorBlendFactory(Color blendColor)
	{
		m_BlendColor = blendColor;
	}

	public override Color GetColor(int rgb)
	{
		if (rgb == -1)
		{
			return Color.Empty;
		}
		return Color.FromArgb(SoftLight((rgb & 0xFF0000) >> 16, m_BlendColor.R), SoftLight((rgb & 0xFF00) >> 8, m_BlendColor.G), SoftLight(rgb & 0xFF, m_BlendColor.B));
	}

	public override Color GetColor(int alpha, int rgb)
	{
		if (rgb == -1)
		{
			return Color.Empty;
		}
		return Color.FromArgb(alpha, SoftLight((rgb & 0xFF0000) >> 16, m_BlendColor.R), SoftLight((rgb & 0xFF00) >> 8, m_BlendColor.G), SoftLight(rgb & 0xFF, m_BlendColor.B));
	}

	public override Color GetColor(Color c)
	{
		if (c.IsEmpty)
		{
			return Color.Empty;
		}
		return Color.FromArgb(c.A, SoftLight(c.R, m_BlendColor.R), SoftLight(c.G, m_BlendColor.G), SoftLight(c.B, m_BlendColor.B));
	}

	public override Color GetColor(int alpha, Color c)
	{
		if (c.IsEmpty)
		{
			return Color.Empty;
		}
		return Color.FromArgb(alpha, SoftLight(c.R, m_BlendColor.R), SoftLight(c.G, m_BlendColor.G), SoftLight(c.B, m_BlendColor.B));
	}

	public override Color GetColor(string argb)
	{
		return GetColor(ColorScheme.GetColor(argb));
	}

	internal static int SoftLight(int a, int b)
	{
		int num = 0;
		num = a * b / 255;
		return num + a * (255 - (255 - a) * (255 - b) / 255 - num) / 255;
	}

	internal static Color SoftLight(Color c, Color light)
	{
		return Color.FromArgb(SoftLight(c.R, light.R), SoftLight(c.G, light.G), SoftLight(c.B, light.B));
	}
}
