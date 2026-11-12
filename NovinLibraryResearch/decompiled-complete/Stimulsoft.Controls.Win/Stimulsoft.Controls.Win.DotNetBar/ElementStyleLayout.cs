using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ElementStyleLayout
{
	private static readonly eSpacePart LeftWhiteSpaceParts = eSpacePart.Padding | eSpacePart.Border | eSpacePart.Margin;

	private static readonly eSpacePart RightWhiteSpaceParts = eSpacePart.Padding | eSpacePart.Border | eSpacePart.Margin;

	public static void CalculateStyleSize(ElementStyle style, Font defaultFont)
	{
		int num = defaultFont.Height;
		bool dispose = false;
		ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(style, out dispose);
		if (elementStyle.Font != null)
		{
			num = elementStyle.Font.Height;
		}
		if (elementStyle.PaintTopBorder)
		{
			num += elementStyle.BorderTopWidth;
		}
		if (elementStyle.PaintBottomBorder)
		{
			num += elementStyle.BorderBottomWidth;
		}
		num += elementStyle.MarginTop + style.MarginBottom;
		num += elementStyle.PaddingTop + elementStyle.PaddingBottom;
		style.SetSize(new Size(0, num));
	}

	public static int HorizontalStyleWhiteSpace(ElementStyle es)
	{
		return LeftWhiteSpace(es) + RightWhiteSpace(es);
	}

	public static int VerticalStyleWhiteSpace(ElementStyle es)
	{
		return TopWhiteSpace(es) + BottomWhiteSpace(es);
	}

	public static int LeftWhiteSpace(ElementStyle es)
	{
		return StyleSpacing(es, LeftWhiteSpaceParts, eStyleSide.Left);
	}

	public static int LeftWhiteSpace(ElementStyle es, eSpacePart parts)
	{
		return StyleSpacing(es, parts, eStyleSide.Left);
	}

	public static int RightWhiteSpace(ElementStyle es)
	{
		return StyleSpacing(es, RightWhiteSpaceParts, eStyleSide.Right);
	}

	public static int RightWhiteSpace(ElementStyle es, eSpacePart parts)
	{
		return StyleSpacing(es, parts, eStyleSide.Right);
	}

	public static int TopWhiteSpace(ElementStyle es)
	{
		return StyleSpacing(es, eSpacePart.Padding | eSpacePart.Border | eSpacePart.Margin, eStyleSide.Top);
	}

	public static int TopWhiteSpace(ElementStyle es, eSpacePart parts)
	{
		return StyleSpacing(es, parts, eStyleSide.Top);
	}

	public static int BottomWhiteSpace(ElementStyle es)
	{
		return StyleSpacing(es, eSpacePart.Padding | eSpacePart.Border | eSpacePart.Margin, eStyleSide.Bottom);
	}

	public static int BottomWhiteSpace(ElementStyle es, eSpacePart parts)
	{
		return StyleSpacing(es, parts, eStyleSide.Bottom);
	}

	public static int StyleSpacing(ElementStyle style, eSpacePart part, eStyleSide side)
	{
		bool dispose = false;
		ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(style, out dispose);
		int num = 0;
		if ((part & eSpacePart.Margin) == eSpacePart.Margin)
		{
			switch (side)
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
		if ((part & eSpacePart.Padding) == eSpacePart.Padding)
		{
			switch (side)
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
		if ((part & eSpacePart.Border) == eSpacePart.Border)
		{
			switch (side)
			{
			case eStyleSide.Bottom:
				if (elementStyle.PaintBottomBorder)
				{
					num += elementStyle.BorderBottomWidth;
				}
				if (elementStyle.BorderBottom == eStyleBorderType.Etched || elementStyle.BorderBottom == eStyleBorderType.Double)
				{
					num += elementStyle.BorderBottomWidth;
				}
				break;
			case eStyleSide.Left:
				if (elementStyle.PaintLeftBorder)
				{
					num += elementStyle.BorderLeftWidth;
				}
				if (elementStyle.BorderLeft == eStyleBorderType.Etched || elementStyle.BorderLeft == eStyleBorderType.Double)
				{
					num += elementStyle.BorderLeftWidth;
				}
				break;
			case eStyleSide.Right:
				if (elementStyle.PaintRightBorder)
				{
					num += elementStyle.BorderRightWidth;
				}
				if (elementStyle.BorderRight == eStyleBorderType.Etched || elementStyle.BorderRight == eStyleBorderType.Double)
				{
					num += elementStyle.BorderRightWidth;
				}
				break;
			case eStyleSide.Top:
				if (elementStyle.PaintTopBorder)
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
		if (dispose)
		{
			elementStyle.Dispose();
		}
		return num;
	}

	public static Rectangle GetInnerRect(ElementStyle style, Rectangle bounds)
	{
		bounds.X += LeftWhiteSpace(style);
		bounds.Width -= HorizontalStyleWhiteSpace(style);
		bounds.Y += TopWhiteSpace(style);
		bounds.Height -= VerticalStyleWhiteSpace(style);
		return bounds;
	}
}
