using System.Drawing;

namespace DevComponents.DotNetBar;

internal class ᝈ
{
	private static readonly eSpacePart ᝉ = eSpacePart.Padding | eSpacePart.Border | eSpacePart.Margin;

	private static readonly eSpacePart ᝊ = eSpacePart.Padding | eSpacePart.Border | eSpacePart.Margin;

	public static void ᝋ(ElementStyle _0657, Font ᝌ)
	{
		int num = ᝌ.Height;
		bool ല = false;
		ElementStyle elementStyle = ElementStyleDisplay.റ(_0657, out ല);
		if (elementStyle.Font != null)
		{
			num = elementStyle.Font.Height;
		}
		if (elementStyle.ހ)
		{
			num += elementStyle.BorderTopWidth;
		}
		if (elementStyle.ށ)
		{
			num += elementStyle.BorderBottomWidth;
		}
		num += elementStyle.MarginTop + _0657.MarginBottom;
		num += elementStyle.PaddingTop + elementStyle.PaddingBottom;
		_0657.ߑ(new Size(0, num));
	}

	public static int ᝍ(ElementStyle ᝎ)
	{
		return ᝐ(ᝎ) + _1752(ᝎ);
	}

	public static int ᝏ(ElementStyle ᝎ)
	{
		return _1753(ᝎ) + _1754(ᝎ);
	}

	public static int ᝐ(ElementStyle ᝎ)
	{
		return _1755(ᝎ, ᝉ, eStyleSide.Left);
	}

	public static int ᝐ(ElementStyle ᝎ, eSpacePart ᝑ)
	{
		return _1755(ᝎ, ᝑ, eStyleSide.Left);
	}

	public static int _1752(ElementStyle ᝎ)
	{
		return _1755(ᝎ, ᝊ, eStyleSide.Right);
	}

	public static int _1752(ElementStyle ᝎ, eSpacePart ᝑ)
	{
		return _1755(ᝎ, ᝑ, eStyleSide.Right);
	}

	public static int _1753(ElementStyle ᝎ)
	{
		return _1755(ᝎ, eSpacePart.Padding | eSpacePart.Border | eSpacePart.Margin, eStyleSide.Top);
	}

	public static int _1753(ElementStyle ᝎ, eSpacePart ᝑ)
	{
		return _1755(ᝎ, ᝑ, eStyleSide.Top);
	}

	public static int _1754(ElementStyle ᝎ)
	{
		return _1755(ᝎ, eSpacePart.Padding | eSpacePart.Border | eSpacePart.Margin, eStyleSide.Bottom);
	}

	public static int _1754(ElementStyle ᝎ, eSpacePart ᝑ)
	{
		return _1755(ᝎ, ᝑ, eStyleSide.Bottom);
	}

	public static int _1755(ElementStyle _0657, eSpacePart _070B, eStyleSide _५)
	{
		bool ല = false;
		ElementStyle elementStyle = ElementStyleDisplay.റ(_0657, out ല);
		int num = 0;
		if ((_070B & eSpacePart.Margin) == eSpacePart.Margin)
		{
			switch (_५)
			{
			case eStyleSide.Bottom:
				num += elementStyle.MarginBottom;
				break;
			case eStyleSide.Left:
				num += elementStyle.MarginLeft;
				break;
			case eStyleSide.Right:
				num += elementStyle.MarginRight;
				break;
			case eStyleSide.Top:
				num += elementStyle.MarginTop;
				break;
			}
		}
		if ((_070B & eSpacePart.Padding) == eSpacePart.Padding)
		{
			switch (_५)
			{
			case eStyleSide.Bottom:
				num += elementStyle.PaddingBottom;
				break;
			case eStyleSide.Left:
				num += elementStyle.PaddingLeft;
				break;
			case eStyleSide.Right:
				num += elementStyle.PaddingRight;
				break;
			case eStyleSide.Top:
				num += elementStyle.PaddingTop;
				break;
			}
		}
		if ((_070B & eSpacePart.Border) == eSpacePart.Border)
		{
			switch (_५)
			{
			case eStyleSide.Bottom:
				if (elementStyle.ށ)
				{
					num += elementStyle.BorderBottomWidth;
				}
				if (elementStyle.BorderBottom == eStyleBorderType.Etched || elementStyle.BorderBottom == eStyleBorderType.Double)
				{
					num += elementStyle.BorderBottomWidth;
				}
				break;
			case eStyleSide.Left:
				if (elementStyle.ݾ)
				{
					num += elementStyle.BorderLeftWidth;
				}
				if (elementStyle.BorderLeft == eStyleBorderType.Etched || elementStyle.BorderLeft == eStyleBorderType.Double)
				{
					num += elementStyle.BorderLeftWidth;
				}
				break;
			case eStyleSide.Right:
				if (elementStyle.ݿ)
				{
					num += elementStyle.BorderRightWidth;
				}
				if (elementStyle.BorderRight == eStyleBorderType.Etched || elementStyle.BorderRight == eStyleBorderType.Double)
				{
					num += elementStyle.BorderRightWidth;
				}
				break;
			case eStyleSide.Top:
				if (elementStyle.ހ)
				{
					num += elementStyle.BorderTopWidth;
				}
				if (elementStyle.BorderTop == eStyleBorderType.Etched || elementStyle.BorderTop == eStyleBorderType.Double)
				{
					num += elementStyle.BorderTopWidth;
				}
				break;
			}
		}
		if (ല)
		{
			elementStyle.Dispose();
		}
		return num;
	}

	public static Rectangle _1756(ElementStyle _0657, Rectangle ശ)
	{
		ശ.X += ᝐ(_0657);
		ശ.Width -= ᝍ(_0657);
		ശ.Y += _1753(_0657);
		ശ.Height -= ᝏ(_0657);
		return ശ;
	}
}
