using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class SimpleElementLayout
{
	private enum eVerticalAlign
	{
		Top,
		Middle,
		Bottom
	}

	private enum eHorizontalAlign
	{
		Left,
		Center,
		Right
	}

	internal static void LayoutSimpleElement(SimpleElementLayoutInfo info)
	{
		Size size = Size.Empty;
		Font font = info.Font;
		int num = 0;
		if (info.LayoutStyle.Font != null)
		{
			font = info.LayoutStyle.Font;
		}
		if (info.Element.FixedWidth == 0)
		{
			if (info.Element.TextVisible)
			{
				string text = info.Element.Text;
				if (text != "")
				{
					size = TextDrawing.MeasureString(info.Graphics, text, font);
					if (info.LayoutStyle != null && !info.LayoutStyle.TextShadowColor.IsEmpty && !info.LayoutStyle.TextShadowOffset.IsEmpty)
					{
						size.Height += info.LayoutStyle.TextShadowOffset.Y;
					}
				}
			}
		}
		else
		{
			int num2 = info.Element.FixedWidth - ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle);
			if (info.Element.ImageVisible)
			{
				num2 -= info.Element.ImageLayoutSize.Width;
			}
			if (info.Element.TextVisible)
			{
				int num3 = font.Height;
				if (info.LayoutStyle != null && !info.LayoutStyle.TextShadowColor.IsEmpty && !info.LayoutStyle.TextShadowOffset.IsEmpty)
				{
					num3 += info.LayoutStyle.TextShadowOffset.Y;
				}
				if (info.LayoutStyle.WordWrap)
				{
					num3 = info.LayoutStyle.MaximumHeight - info.LayoutStyle.MarginTop - info.LayoutStyle.MarginBottom - info.LayoutStyle.PaddingTop - info.LayoutStyle.PaddingBottom;
					if (num2 > 0)
					{
						size = ((num3 <= 0) ? TextDrawing.MeasureString(info.Graphics, info.Element.Text, font, num2, info.LayoutStyle.TextFormat) : TextDrawing.MeasureString(info.Graphics, info.Element.Text, font, new Size(num2, num3), info.LayoutStyle.TextFormat));
					}
				}
				else
				{
					size = new Size(num2, num3);
				}
			}
		}
		if (info.Element.TextVisible && !info.LayoutStyle.TextShadowColor.IsEmpty)
		{
			size.Height += info.LayoutStyle.TextShadowOffset.Y;
		}
		num = size.Height;
		if (info.VerticalPartAlignment)
		{
			if (info.Element.ImageVisible && info.Element.ImageLayoutSize.Height > 0)
			{
				num += info.Element.ImageLayoutSize.Height;
			}
		}
		else if (info.Element.ImageVisible && info.Element.ImageLayoutSize.Height > num)
		{
			num = info.Element.ImageLayoutSize.Height;
		}
		Rectangle boundingRectangle = new Rectangle(info.Left + ElementStyleLayout.LeftWhiteSpace(info.LayoutStyle), info.Top + ElementStyleLayout.TopWhiteSpace(info.LayoutStyle), info.Element.FixedWidth - ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle), num);
		if (boundingRectangle.Width == 0)
		{
			if (info.VerticalPartAlignment)
			{
				if (info.Element.TextVisible)
				{
					boundingRectangle.Width = size.Width;
				}
				if (info.Element.ImageVisible && info.Element.ImageLayoutSize.Width > boundingRectangle.Width)
				{
					boundingRectangle.Width = info.Element.ImageLayoutSize.Width + info.Element.ImageTextSpacing;
				}
			}
			else
			{
				if (info.Element.TextVisible)
				{
					boundingRectangle.Width = size.Width;
				}
				if (info.Element.ImageVisible && info.Element.ImageLayoutSize.Width > 0)
				{
					boundingRectangle.Width += info.Element.ImageLayoutSize.Width + info.Element.ImageTextSpacing;
				}
			}
		}
		Rectangle bounds = new Rectangle(info.Left, info.Top, info.Element.FixedWidth, boundingRectangle.Height + info.LayoutStyle.MarginTop + info.LayoutStyle.MarginBottom + info.LayoutStyle.PaddingTop + info.LayoutStyle.PaddingBottom);
		if (bounds.Width == 0)
		{
			bounds.Width = boundingRectangle.Width + ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle);
		}
		info.Element.Bounds = bounds;
		if (info.Element.ImageVisible && !info.Element.ImageLayoutSize.IsEmpty)
		{
			eVerticalAlign imageVerticalAlign = GetImageVerticalAlign(info.Element.ImageAlignment);
			eHorizontalAlign imageHorizontalAlign = GetImageHorizontalAlign(info.Element.ImageAlignment, info.LeftToRight);
			if (info.VerticalPartAlignment)
			{
				info.Element.ImageBounds = AlignContentVertical(info.Element.ImageLayoutSize, ref boundingRectangle, imageHorizontalAlign, imageVerticalAlign, info.Element.ImageTextSpacing);
			}
			else
			{
				info.Element.ImageBounds = AlignContent(info.Element.ImageLayoutSize, ref boundingRectangle, imageHorizontalAlign, imageVerticalAlign, info.Element.ImageTextSpacing);
			}
		}
		else
		{
			info.Element.ImageBounds = Rectangle.Empty;
		}
		if (!size.IsEmpty)
		{
			info.Element.TextBounds = boundingRectangle;
		}
		else
		{
			info.Element.TextBounds = Rectangle.Empty;
		}
	}

	private static Rectangle AlignContent(Size contentSize, ref Rectangle boundingRectangle, eHorizontalAlign horizAlign, eVerticalAlign vertAlign, int contentSpacing)
	{
		Rectangle result = new Rectangle(Point.Empty, contentSize);
		if (horizAlign == eHorizontalAlign.Right)
		{
			result.X = boundingRectangle.Right - result.Width;
			boundingRectangle.Width -= result.Width + contentSpacing;
		}
		else
		{
			result.X = boundingRectangle.X;
			boundingRectangle.X = result.Right + contentSpacing;
			boundingRectangle.Width -= result.Width + contentSpacing;
		}
		switch (vertAlign)
		{
		case eVerticalAlign.Top:
			result.Y = boundingRectangle.Y;
			break;
		case eVerticalAlign.Middle:
			result.Y = boundingRectangle.Y + (boundingRectangle.Height - result.Height) / 2;
			break;
		case eVerticalAlign.Bottom:
			result.Y = boundingRectangle.Bottom - result.Height;
			break;
		}
		return result;
	}

	private static Rectangle AlignContentVertical(Size contentSize, ref Rectangle boundingRectangle, eHorizontalAlign horizAlign, eVerticalAlign vertAlign, int contentSpacing)
	{
		Rectangle result = new Rectangle(Point.Empty, contentSize);
		switch (horizAlign)
		{
		case eHorizontalAlign.Left:
			result.X = boundingRectangle.X;
			break;
		case eHorizontalAlign.Right:
			result.X = boundingRectangle.Right - result.Width;
			break;
		case eHorizontalAlign.Center:
			result.X = boundingRectangle.X + (boundingRectangle.Width - result.Width) / 2;
			break;
		}
		if (vertAlign == eVerticalAlign.Bottom)
		{
			result.Y = boundingRectangle.Bottom - result.Height;
			boundingRectangle.Height -= result.Height + contentSpacing;
		}
		else
		{
			result.Y = boundingRectangle.Y;
			boundingRectangle.Y = result.Bottom + contentSpacing;
			boundingRectangle.Height -= result.Height + contentSpacing;
		}
		return result;
	}

	private static eHorizontalAlign GetImageHorizontalAlign(eSimplePartAlignment align, bool leftToRight)
	{
		if (((align == eSimplePartAlignment.NearBottom || align == eSimplePartAlignment.NearCenter || align == eSimplePartAlignment.NearTop) && leftToRight) || ((align == eSimplePartAlignment.FarBottom || align == eSimplePartAlignment.FarCenter || align == eSimplePartAlignment.FarTop) && !leftToRight))
		{
			return eHorizontalAlign.Left;
		}
		if (align == eSimplePartAlignment.CenterBottom || align == eSimplePartAlignment.CenterTop)
		{
			return eHorizontalAlign.Center;
		}
		return eHorizontalAlign.Right;
	}

	private static eVerticalAlign GetImageVerticalAlign(eSimplePartAlignment align)
	{
		eVerticalAlign result = eVerticalAlign.Middle;
		switch (align)
		{
		case eSimplePartAlignment.NearBottom:
		case eSimplePartAlignment.CenterBottom:
		case eSimplePartAlignment.FarBottom:
			result = eVerticalAlign.Bottom;
			break;
		case eSimplePartAlignment.NearTop:
		case eSimplePartAlignment.CenterTop:
		case eSimplePartAlignment.FarTop:
			result = eVerticalAlign.Top;
			break;
		}
		return result;
	}
}
