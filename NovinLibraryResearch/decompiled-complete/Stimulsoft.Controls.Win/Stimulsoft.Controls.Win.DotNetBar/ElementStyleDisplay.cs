using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ElementStyleDisplay
{
	private enum eBorderSide
	{
		Top,
		Bottom,
		Left,
		Right,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	internal struct BorderDrawInfo
	{
		public bool HasLeftBorder;

		public bool HasRightBorder;

		public bool HasTopBorder;

		public bool HasBottomBorder;

		public bool IsTwistedDoubleBorder;

		public BorderDrawInfo(bool hasLeftBorder, bool hasRightBorder, bool hasTopBorder, bool hasBottomBorder, bool isTwistedDoubleBorder)
		{
			HasLeftBorder = hasLeftBorder;
			HasRightBorder = hasRightBorder;
			HasTopBorder = hasTopBorder;
			HasBottomBorder = hasBottomBorder;
			IsTwistedDoubleBorder = isTwistedDoubleBorder;
		}
	}

	public static void PaintText(ElementStyleDisplayInfo e, string text, Font defaultFont)
	{
		PaintText(e, text, defaultFont, useDefaultFont: false);
	}

	public static void PaintText(ElementStyleDisplayInfo e, string text, Font defaultFont, bool useDefaultFont)
	{
		PaintText(e, text, defaultFont, useDefaultFont, e.Style.TextFormat);
	}

	public static void PaintText(ElementStyleDisplayInfo e, string text, Font defaultFont, bool useDefaultFont, eTextFormat textFormat)
	{
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		bool dispose = false;
		ElementStyle elementStyle = GetElementStyle(e.Style, out dispose);
		if (text == "" || bounds.IsEmpty)
		{
			if (dispose)
			{
				elementStyle.Dispose();
			}
			return;
		}
		Font val = elementStyle.Font;
		if (val == null || useDefaultFont)
		{
			val = defaultFont;
		}
		bounds.X += elementStyle.MarginLeft;
		bounds.Width -= elementStyle.MarginLeft + elementStyle.MarginRight;
		bounds.Y += elementStyle.MarginTop;
		bounds.Height -= elementStyle.MarginTop + elementStyle.MarginBottom;
		if (!elementStyle.TextShadowColor.IsEmpty && (double)Math.Abs(elementStyle.TextShadowColor.GetBrightness() - elementStyle.TextColor.GetBrightness()) > 0.2)
		{
			SolidBrush val2 = new SolidBrush(elementStyle.TextShadowColor);
			try
			{
				Rectangle bounds2 = bounds;
				bounds2.Offset(elementStyle.TextShadowOffset);
				TextDrawing.DrawString(e.Graphics, text, val, elementStyle.TextShadowColor, bounds2, textFormat);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Color color = elementStyle.TextColor;
		if (color.IsEmpty && elementStyle.TextShadowColor.IsEmpty)
		{
			color = SystemColors.ControlText;
		}
		if (!color.IsEmpty)
		{
			TextDrawing.DrawString(e.Graphics, text, val, color, bounds, textFormat);
		}
		if (dispose)
		{
			elementStyle.Dispose();
		}
	}

	public static Region GetStyleRegion(ElementStyleDisplayInfo e)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		if (e.Style.PaintBorder && e.Style.CornerType != eCornerType.Square)
		{
			bounds.Width--;
			bounds.Height--;
		}
		Region val = new Region();
		GraphicsPath backgroundPath = GetBackgroundPath(e.Style, bounds);
		try
		{
			val.MakeEmpty();
			val.Union(backgroundPath);
			if (e.Style.PaintBorder && (e.Style.CornerType == eCornerType.Rounded || e.Style.CornerType == eCornerType.Diagonal || e.Style.CornerTypeTopLeft == eCornerType.Rounded || e.Style.CornerTypeTopLeft == eCornerType.Diagonal || e.Style.CornerTypeTopRight == eCornerType.Rounded || e.Style.CornerTypeTopRight == eCornerType.Diagonal || e.Style.CornerTypeBottomLeft == eCornerType.Rounded || e.Style.CornerTypeBottomLeft == eCornerType.Diagonal || e.Style.CornerTypeBottomRight == eCornerType.Rounded || e.Style.CornerTypeBottomRight == eCornerType.Diagonal))
			{
				Pen val2 = new Pen(Color.Black, (float)((e.Style.BorderTopWidth <= 0) ? 1 : e.Style.BorderTopWidth));
				try
				{
					backgroundPath.Widen(val2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				val.Union(backgroundPath);
			}
		}
		finally
		{
			((IDisposable)backgroundPath)?.Dispose();
		}
		return val;
	}

	public static GraphicsPath GetInsideClip(ElementStyleDisplayInfo e)
	{
		Rectangle bounds = e.Bounds;
		if (e.Style.PaintBorder)
		{
			if (e.Style.PaintTopBorder)
			{
				bounds.Y += e.Style.BorderTopWidth;
				bounds.Height -= e.Style.BorderTopWidth;
				if (e.Style.BorderTop == eStyleBorderType.Etched || e.Style.BorderTop == eStyleBorderType.Double)
				{
					bounds.Y += e.Style.BorderTopWidth;
					bounds.Height -= e.Style.BorderTopWidth;
				}
			}
			if (e.Style.PaintBottomBorder)
			{
				bounds.Height -= e.Style.BorderBottomWidth;
				if (e.Style.BorderBottom == eStyleBorderType.Etched || e.Style.BorderBottom == eStyleBorderType.Double)
				{
					bounds.Height -= e.Style.BorderBottomWidth;
				}
			}
			if (e.Style.PaintLeftBorder)
			{
				bounds.X += e.Style.BorderLeftWidth;
				bounds.Width -= e.Style.BorderLeftWidth;
				if (e.Style.BorderLeft == eStyleBorderType.Etched || e.Style.BorderLeft == eStyleBorderType.Double)
				{
					bounds.X += e.Style.BorderLeftWidth;
					bounds.Width -= e.Style.BorderLeftWidth;
				}
			}
			if (e.Style.PaintRightBorder)
			{
				bounds.Width -= e.Style.BorderRightWidth;
				if (e.Style.BorderRight == eStyleBorderType.Etched || e.Style.BorderRight == eStyleBorderType.Double)
				{
					bounds.Width -= e.Style.BorderRightWidth;
				}
			}
		}
		return GetBackgroundPath(e.Style, bounds);
	}

	public static void Paint(ElementStyleDisplayInfo e)
	{
		PaintBackground(e);
		PaintBackgroundImage(e);
		PaintBorder(e);
	}

	private static eCornerType GetCornerType(eCornerType baseType, eCornerType specificType)
	{
		if (specificType != 0)
		{
			return specificType;
		}
		return baseType;
	}

	public static void PaintBorder(ElementStyleDisplayInfo e)
	{
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Expected O, but got Unknown
		bool dispose = false;
		ElementStyle elementStyle = GetElementStyle(e.Style, out dispose);
		Rectangle backgroundRectangle = GetBackgroundRectangle(elementStyle, e.Bounds);
		Rectangle borderRectangle = GetBorderRectangle(elementStyle, backgroundRectangle);
		try
		{
			if (borderRectangle.Width < 2 || borderRectangle.Height < 2)
			{
				return;
			}
			eCornerType cornerType = GetCornerType(elementStyle.CornerType, elementStyle.CornerTypeTopLeft);
			eCornerType cornerType2 = GetCornerType(elementStyle.CornerType, elementStyle.CornerTypeTopRight);
			eCornerType cornerType3 = GetCornerType(elementStyle.CornerType, elementStyle.CornerTypeBottomLeft);
			eCornerType cornerType4 = GetCornerType(elementStyle.CornerType, elementStyle.CornerTypeBottomRight);
			if (elementStyle.PaintBorder && elementStyle.Border == eStyleBorderType.Solid && cornerType == eCornerType.Rounded && cornerType2 == eCornerType.Rounded && cornerType3 == eCornerType.Rounded && cornerType4 == eCornerType.Rounded && elementStyle.BorderLeftWidth == elementStyle.BorderWidth && elementStyle.BorderRightWidth == elementStyle.BorderWidth && elementStyle.BorderTopWidth == elementStyle.BorderWidth && elementStyle.BorderBottomWidth == elementStyle.BorderWidth)
			{
				if (elementStyle.BorderWidth == 1)
				{
					borderRectangle.Width++;
					borderRectangle.Height++;
				}
				DisplayHelp.DrawRoundGradientRectangle(e.Graphics, borderRectangle, elementStyle.BorderColor, elementStyle.BorderColor2, elementStyle.BorderGradientAngle, elementStyle.BorderWidth, elementStyle.CornerDiameter);
				return;
			}
			if (elementStyle.PaintAllBorders && elementStyle.Border == eStyleBorderType.Solid && cornerType == eCornerType.Square && cornerType2 == eCornerType.Square && cornerType3 == eCornerType.Square && cornerType4 == eCornerType.Square && elementStyle.BorderLeftWidth == elementStyle.BorderWidth && elementStyle.BorderRightWidth == elementStyle.BorderWidth && elementStyle.BorderTopWidth == elementStyle.BorderWidth && elementStyle.BorderBottomWidth == elementStyle.BorderWidth)
			{
				if (elementStyle.BorderWidth == 1)
				{
					borderRectangle.Width++;
					borderRectangle.Height++;
				}
				DisplayHelp.DrawGradientRectangle(e.Graphics, borderRectangle, elementStyle.BorderColor, elementStyle.BorderColor2, elementStyle.BorderGradientAngle, elementStyle.BorderWidth);
				return;
			}
			if (elementStyle.Border == eStyleBorderType.None && elementStyle.BorderRight == eStyleBorderType.Etched && elementStyle.BorderLeft == eStyleBorderType.None && elementStyle.BorderTop == eStyleBorderType.None && elementStyle.BorderBottom == eStyleBorderType.None)
			{
				DisplayHelp.DrawGradientRectangle(e.Graphics, new Rectangle(borderRectangle.Right - 2, borderRectangle.Y, 3, borderRectangle.Height + 1), elementStyle.BorderColorLight, elementStyle.BorderColorLight2, 90, 1);
				DisplayHelp.DrawGradientLine(e.Graphics, new Point(borderRectangle.Right, borderRectangle.Y), new Point(borderRectangle.Right, borderRectangle.Bottom + 1), elementStyle.BorderColor, elementStyle.BorderColor2, 90, 1);
				return;
			}
			if (!elementStyle.BorderColor2.IsEmpty || !elementStyle.BorderColorLight.IsEmpty)
			{
				if (elementStyle.Border == eStyleBorderType.Solid)
				{
					GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(borderRectangle, elementStyle.CornerDiameter, eStyleBackgroundPathPart.Complete, cornerType, cornerType2, cornerType3, cornerType4);
					try
					{
						DisplayHelp.DrawGradientPathBorder(e.Graphics, roundedRectanglePath, elementStyle.BorderColor, elementStyle.BorderColor2, elementStyle.BorderGradientAngle, elementStyle.BorderWidth);
						return;
					}
					finally
					{
						((IDisposable)roundedRectanglePath)?.Dispose();
					}
				}
				if (elementStyle.Border == eStyleBorderType.Etched)
				{
					Rectangle clientRectangle = borderRectangle;
					clientRectangle.Width -= elementStyle.BorderWidth;
					clientRectangle.Height -= elementStyle.BorderWidth;
					clientRectangle.Offset(elementStyle.BorderWidth, elementStyle.BorderWidth);
					if (clientRectangle.Width > 2 && clientRectangle.Height > 2)
					{
						GraphicsPath roundedRectanglePath2 = DisplayHelp.GetRoundedRectanglePath(clientRectangle, elementStyle.CornerDiameter, eStyleBackgroundPathPart.Complete, cornerType, cornerType2, cornerType3, cornerType4);
						try
						{
							DisplayHelp.DrawGradientPathBorder(e.Graphics, roundedRectanglePath2, elementStyle.BorderColorLight, elementStyle.BorderColorLight2, elementStyle.BorderLightGradientAngle, elementStyle.BorderWidth);
						}
						finally
						{
							((IDisposable)roundedRectanglePath2)?.Dispose();
						}
						clientRectangle.Offset(-elementStyle.BorderWidth, -elementStyle.BorderWidth);
						GraphicsPath roundedRectanglePath3 = DisplayHelp.GetRoundedRectanglePath(clientRectangle, elementStyle.CornerDiameter, eStyleBackgroundPathPart.Complete, cornerType, cornerType2, cornerType3, cornerType4);
						try
						{
							DisplayHelp.DrawGradientPathBorder(e.Graphics, roundedRectanglePath3, elementStyle.BorderColor, elementStyle.BorderColor2, elementStyle.BorderGradientAngle, elementStyle.BorderWidth);
							return;
						}
						finally
						{
							((IDisposable)roundedRectanglePath3)?.Dispose();
						}
					}
					return;
				}
				if ((elementStyle.BorderTop == eStyleBorderType.Double && elementStyle.BorderLeft == eStyleBorderType.Double && elementStyle.BorderRight == eStyleBorderType.Double && elementStyle.BorderBottom == eStyleBorderType.Double) || (elementStyle.BorderTop == eStyleBorderType.None && elementStyle.BorderLeft == eStyleBorderType.Double && elementStyle.BorderRight == eStyleBorderType.Double && elementStyle.BorderBottom == eStyleBorderType.Double))
				{
					Rectangle rectangle = borderRectangle;
					Region clip = null;
					bool flag = false;
					if (elementStyle.BorderTop == eStyleBorderType.None)
					{
						Rectangle clip2 = rectangle;
						clip2.Width++;
						clip2.Height++;
						e.Graphics.SetClip(clip2);
						rectangle.Y -= 3;
						rectangle.Height += 3;
					}
					GraphicsPath borderPath = DisplayHelp.GetBorderPath(rectangle, elementStyle.CornerDiameter, eStyleBackgroundPathPart.Complete, cornerType, cornerType2, cornerType3, cornerType4, elementStyle.PaintLeftBorder, elementStyle.PaintRightBorder, elementStyle.PaintTopBorder, elementStyle.PaintBottomBorder);
					try
					{
						Pen val = new Pen(elementStyle.BorderColor, (float)elementStyle.BorderWidth);
						try
						{
							borderPath.Widen(val);
						}
						finally
						{
							((IDisposable)val)?.Dispose();
						}
						DisplayHelp.FillPath(e.Graphics, borderPath, elementStyle.BorderColor, elementStyle.BorderColor2);
					}
					finally
					{
						((IDisposable)borderPath)?.Dispose();
					}
					rectangle.Inflate(-elementStyle.BorderWidth, -elementStyle.BorderWidth);
					GraphicsPath borderPath2 = DisplayHelp.GetBorderPath(rectangle, elementStyle.CornerDiameter, eStyleBackgroundPathPart.Complete, cornerType, cornerType2, cornerType3, cornerType4, elementStyle.PaintLeftBorder, elementStyle.PaintRightBorder, elementStyle.PaintTopBorder, elementStyle.PaintBottomBorder);
					try
					{
						Pen val2 = new Pen(elementStyle.BorderColor, (float)elementStyle.BorderWidth);
						try
						{
							borderPath2.Widen(val2);
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
						DisplayHelp.FillPath(e.Graphics, borderPath2, elementStyle.BorderColorLight, elementStyle.BorderColorLight2);
					}
					finally
					{
						((IDisposable)borderPath2)?.Dispose();
					}
					if (flag)
					{
						e.Graphics.Clip = clip;
					}
					return;
				}
			}
			Color borderColor = elementStyle.BorderColor;
			Color borderColor2 = elementStyle.BorderColor2;
			Color borderColorLight = elementStyle.BorderColorLight;
			Color borderColorLight2 = elementStyle.BorderColorLight2;
			BorderDrawInfo drawInfo = new BorderDrawInfo(elementStyle.PaintLeftBorder, elementStyle.PaintRightBorder, elementStyle.PaintTopBorder, elementStyle.PaintBottomBorder, e.IsTwistedDoubleBorder);
			if (elementStyle.PaintLeftBorder)
			{
				Color color = elementStyle.BorderColor;
				if (!elementStyle.BorderLeftColor.IsEmpty)
				{
					color = elementStyle.BorderLeftColor;
				}
				Point[] array = new Point[2]
				{
					borderRectangle.Location,
					default(Point)
				};
				array[1].X = borderRectangle.X;
				array[1].Y = borderRectangle.Bottom;
				if (cornerType != eCornerType.Square)
				{
					array[0].Y += elementStyle.CornerDiameter;
				}
				if (cornerType3 != eCornerType.Square)
				{
					array[1].Y -= elementStyle.CornerDiameter;
				}
				DrawBorderLine(e.Graphics, array, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color, borderColor2, borderColorLight, borderColorLight2, eBorderSide.Left, drawInfo);
				if (elementStyle.PaintTopBorder)
				{
					switch (cornerType)
					{
					case eCornerType.Diagonal:
						array[0].X = borderRectangle.X;
						array[0].Y = borderRectangle.Y + elementStyle.CornerDiameter;
						array[1].X = borderRectangle.X + elementStyle.CornerDiameter;
						array[1].Y = borderRectangle.Y;
						DrawBorderLine(e.Graphics, array, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color, borderColor2, borderColorLight, borderColorLight2, eBorderSide.TopLeft, drawInfo);
						break;
					case eCornerType.Rounded:
					{
						ArcData cornerArc = GetCornerArc(borderRectangle, elementStyle.CornerDiameter, eCornerArc.TopLeft);
						DrawCornerArc(e.Graphics, cornerArc, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color);
						break;
					}
					}
				}
				if (elementStyle.PaintBottomBorder)
				{
					switch (cornerType3)
					{
					case eCornerType.Diagonal:
						array[0].X = borderRectangle.X;
						array[0].Y = borderRectangle.Bottom - elementStyle.CornerDiameter;
						array[1].X = borderRectangle.X + elementStyle.CornerDiameter;
						array[1].Y = borderRectangle.Bottom;
						DrawBorderLine(e.Graphics, array, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color, borderColor2, borderColorLight, borderColorLight2, eBorderSide.BottomLeft, drawInfo);
						break;
					case eCornerType.Rounded:
					{
						ArcData cornerArc2 = GetCornerArc(borderRectangle, elementStyle.CornerDiameter, eCornerArc.BottomLeft);
						DrawCornerArc(e.Graphics, cornerArc2, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color);
						break;
					}
					}
				}
			}
			if (elementStyle.PaintTopBorder)
			{
				Color colorStart = elementStyle.BorderColor;
				if (!elementStyle.BorderTopColor.IsEmpty)
				{
					colorStart = elementStyle.BorderTopColor;
				}
				Point[] array2 = new Point[2]
				{
					borderRectangle.Location,
					default(Point)
				};
				array2[1].X = borderRectangle.Right;
				array2[1].Y = borderRectangle.Y;
				if (cornerType != eCornerType.Square)
				{
					array2[0].X += elementStyle.CornerDiameter;
				}
				if (cornerType2 != eCornerType.Square)
				{
					array2[1].X -= elementStyle.CornerDiameter;
				}
				DrawBorderLine(e.Graphics, array2, elementStyle.BorderTop, elementStyle.BorderTopWidth, colorStart, borderColor2, borderColorLight, borderColorLight2, eBorderSide.Top, drawInfo);
			}
			if (elementStyle.PaintBottomBorder)
			{
				Color colorStart2 = elementStyle.BorderColor;
				if (!elementStyle.BorderBottomColor.IsEmpty)
				{
					colorStart2 = elementStyle.BorderBottomColor;
				}
				Point[] array3 = new Point[2];
				array3[0].X = borderRectangle.X;
				array3[0].Y = borderRectangle.Bottom;
				array3[1].X = borderRectangle.Right;
				array3[1].Y = borderRectangle.Bottom;
				if (cornerType3 != eCornerType.Square)
				{
					array3[0].X += elementStyle.CornerDiameter;
				}
				if (cornerType4 != eCornerType.Square)
				{
					array3[1].X -= elementStyle.CornerDiameter;
				}
				DrawBorderLine(e.Graphics, array3, elementStyle.BorderBottom, elementStyle.BorderBottomWidth, colorStart2, borderColor2, borderColorLight, borderColorLight2, eBorderSide.Bottom, drawInfo);
			}
			if (!elementStyle.PaintRightBorder)
			{
				return;
			}
			Color color2 = elementStyle.BorderColor;
			if (!elementStyle.BorderRightColor.IsEmpty)
			{
				color2 = elementStyle.BorderRightColor;
			}
			Point[] array4 = new Point[2];
			array4[0].X = borderRectangle.Right;
			array4[0].Y = borderRectangle.Y;
			array4[1].X = borderRectangle.Right;
			array4[1].Y = borderRectangle.Bottom;
			if (cornerType2 != eCornerType.Square)
			{
				array4[0].Y += elementStyle.CornerDiameter;
			}
			if (cornerType4 != eCornerType.Square)
			{
				array4[1].Y -= elementStyle.CornerDiameter;
			}
			DrawBorderLine(e.Graphics, array4, elementStyle.BorderRight, elementStyle.BorderRightWidth, color2, borderColor2, borderColorLight, borderColorLight2, eBorderSide.Right, drawInfo);
			if (elementStyle.PaintTopBorder)
			{
				switch (cornerType2)
				{
				case eCornerType.Diagonal:
					array4[0].X = borderRectangle.Right - elementStyle.CornerDiameter;
					array4[0].Y = borderRectangle.Y;
					array4[1].X = borderRectangle.Right;
					array4[1].Y = borderRectangle.Y + elementStyle.CornerDiameter;
					DrawBorderLine(e.Graphics, array4, elementStyle.BorderLeft, elementStyle.BorderRightWidth, color2, borderColor2, borderColorLight, borderColorLight2, eBorderSide.TopRight, drawInfo);
					break;
				case eCornerType.Rounded:
				{
					ArcData cornerArc3 = GetCornerArc(borderRectangle, elementStyle.CornerDiameter, eCornerArc.TopRight);
					DrawCornerArc(e.Graphics, cornerArc3, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color2);
					break;
				}
				}
			}
			if (elementStyle.PaintBottomBorder)
			{
				switch (cornerType4)
				{
				case eCornerType.Diagonal:
					array4[0].X = borderRectangle.Right;
					array4[0].Y = borderRectangle.Bottom - elementStyle.CornerDiameter;
					array4[1].X = borderRectangle.Right - elementStyle.CornerDiameter;
					array4[1].Y = borderRectangle.Bottom;
					DrawBorderLine(e.Graphics, array4, elementStyle.BorderLeft, elementStyle.BorderRightWidth, color2, borderColor2, borderColorLight, borderColorLight2, eBorderSide.BottomRight, drawInfo);
					break;
				case eCornerType.Rounded:
				{
					ArcData cornerArc4 = GetCornerArc(borderRectangle, elementStyle.CornerDiameter, eCornerArc.BottomRight);
					DrawCornerArc(e.Graphics, cornerArc4, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color2);
					break;
				}
				}
			}
		}
		finally
		{
			if (dispose)
			{
				elementStyle.Dispose();
			}
		}
	}

	private static Pen CreatePen(eStyleBorderType border, int lineWidth, Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		Pen val = new Pen(color, (float)lineWidth);
		val.Alignment = (PenAlignment)1;
		val.DashStyle = GetDashStyle(border);
		return val;
	}

	private static void DrawBorderLine(Graphics g, Point[] p, eStyleBorderType border, int lineWidth, Color colorStart, Color colorEnd, Color colorLightStart, Color colorLightEnd, eBorderSide side, BorderDrawInfo drawInfo)
	{
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		if (border == eStyleBorderType.Etched || border == eStyleBorderType.Double)
		{
			if (colorLightStart.IsEmpty)
			{
				colorLightStart = ControlPaint.Light(colorStart);
				colorStart = ControlPaint.Dark(colorStart);
			}
			if (drawInfo.IsTwistedDoubleBorder && (side == eBorderSide.Bottom || side == eBorderSide.Right))
			{
				Color color = colorStart;
				Color color2 = colorEnd;
				colorStart = colorLightStart;
				colorEnd = colorLightEnd;
				colorLightStart = color;
				colorLightEnd = color2;
			}
			DisplayHelp.DrawGradientLine(g, p[0], p[1], colorStart, colorEnd, 90, lineWidth);
			switch (side)
			{
			case eBorderSide.Top:
				p[0].Y += lineWidth;
				p[1].Y += lineWidth;
				if (drawInfo.HasLeftBorder)
				{
					p[0].X++;
				}
				if (drawInfo.HasRightBorder)
				{
					p[1].X--;
				}
				break;
			case eBorderSide.Bottom:
				p[0].Y -= lineWidth;
				p[1].Y -= lineWidth;
				if (!drawInfo.IsTwistedDoubleBorder)
				{
					if (drawInfo.HasLeftBorder)
					{
						p[0].X++;
					}
					if (drawInfo.HasRightBorder)
					{
						p[1].X--;
					}
				}
				break;
			case eBorderSide.Left:
			case eBorderSide.TopLeft:
			case eBorderSide.BottomLeft:
				p[0].X += lineWidth;
				p[1].X += lineWidth;
				if (drawInfo.HasTopBorder)
				{
					p[0].Y++;
				}
				if (drawInfo.HasBottomBorder)
				{
					p[1].Y--;
				}
				break;
			case eBorderSide.Right:
			case eBorderSide.TopRight:
			case eBorderSide.BottomRight:
				p[0].X -= lineWidth;
				p[1].X -= lineWidth;
				if (!drawInfo.IsTwistedDoubleBorder)
				{
					if (drawInfo.HasTopBorder)
					{
						p[0].Y++;
					}
					if (drawInfo.HasBottomBorder)
					{
						p[1].Y--;
					}
				}
				break;
			}
			DisplayHelp.DrawGradientLine(g, p[0], p[1], colorLightStart, colorLightEnd, 90, lineWidth);
		}
		else if (colorEnd.IsEmpty)
		{
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)3;
			Pen val = CreatePen(border, lineWidth, colorStart);
			try
			{
				g.DrawLine(val, p[0], p[1]);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			g.SmoothingMode = smoothingMode;
		}
		else
		{
			DisplayHelp.DrawGradientLine(g, p[0], p[1], colorStart, colorEnd, 90, lineWidth);
		}
	}

	private static void DrawCornerArc(Graphics g, ArcData arc, eStyleBorderType border, int lineWidth, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		if ((int)smoothingMode == 3)
		{
			g.SmoothingMode = (SmoothingMode)2;
		}
		Pen val = CreatePen(border, lineWidth, color);
		try
		{
			g.DrawArc(val, (float)arc.X, (float)arc.Y, (float)arc.Width, (float)arc.Height, arc.StartAngle, arc.SweepAngle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if ((int)smoothingMode == 3)
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	private static DashStyle GetDashStyle(eStyleBorderType border)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		DashStyle result = (DashStyle)0;
		switch (border)
		{
		case eStyleBorderType.Dash:
			result = (DashStyle)1;
			break;
		case eStyleBorderType.DashDot:
			result = (DashStyle)3;
			break;
		case eStyleBorderType.DashDotDot:
			result = (DashStyle)4;
			break;
		case eStyleBorderType.Dot:
			result = (DashStyle)2;
			break;
		}
		return result;
	}

	internal static ElementStyle GetElementStyle(ElementStyle style, out bool dispose)
	{
		dispose = false;
		if (style.Class == null || style.Class.Length == 0)
		{
			return style;
		}
		IElementStyleClassProvider iElementStyleClassProvider = GetIElementStyleClassProvider();
		if (iElementStyleClassProvider != null)
		{
			ElementStyle @class = iElementStyleClassProvider.GetClass(style.Class);
			if (@class != null)
			{
				if (style.Custom)
				{
					ElementStyle elementStyle = @class.Copy();
					elementStyle.ApplyStyle(style);
					dispose = true;
					return elementStyle;
				}
				return @class;
			}
		}
		return style;
	}

	private static IElementStyleClassProvider GetIElementStyleClassProvider()
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
		}
		return null;
	}

	public static void PaintBackground(ElementStyleDisplayInfo e)
	{
		PaintBackground(e, shapeBackground: true);
	}

	public static void PaintBackground(ElementStyleDisplayInfo e, bool shapeBackground)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		Region clip = e.Graphics.Clip;
		if (clip != null)
		{
			e.Graphics.SetClip(e.Bounds, (CombineMode)1);
		}
		else
		{
			e.Graphics.SetClip(e.Bounds, (CombineMode)0);
		}
		bool dispose = false;
		ElementStyle elementStyle = GetElementStyle(e.Style, out dispose);
		SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
		e.Graphics.SmoothingMode = (SmoothingMode)0;
		Rectangle rectangle = DisplayHelp.GetDrawRectangle(GetBackgroundRectangle(elementStyle, e.Bounds));
		GraphicsPath val = null;
		if (shapeBackground && elementStyle.CornerType != eCornerType.Square)
		{
			val = GetBackgroundPath(elementStyle, GetBorderRectangle(e.Style, e.Bounds));
		}
		else
		{
			val = new GraphicsPath();
			val.AddRectangle(GetBackgroundRectangle(elementStyle, e.Bounds));
		}
		eBackgroundColorBlendType eBackgroundColorBlendType2 = elementStyle.BackColorBlend.GetBlendType();
		switch (eBackgroundColorBlendType2)
		{
		case eBackgroundColorBlendType.Relative:
			try
			{
				if (elementStyle.BackColorGradientType == eGradientType.Linear)
				{
					Rectangle r = rectangle;
					r.Inflate(1, 1);
					LinearGradientBrush val3 = DisplayHelp.CreateLinearGradientBrush(r, elementStyle.BackColor, elementStyle.BackColor2, elementStyle.BackColorGradientAngle);
					try
					{
						val3.InterpolationColors = elementStyle.BackColorBlend.GetColorBlend();
						e.Graphics.FillPath((Brush)(object)val3, val);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				else if (elementStyle.BackColorGradientType == eGradientType.Radial)
				{
					int num = (int)Math.Sqrt(rectangle.Width * rectangle.Width + rectangle.Height * rectangle.Height) + 4;
					GraphicsPath val4 = new GraphicsPath();
					val4.AddEllipse(rectangle.X - (num - rectangle.Width) / 2, rectangle.Y - (num - rectangle.Height) / 2, num, num);
					PathGradientBrush val5 = new PathGradientBrush(val4);
					try
					{
						val5.CenterColor = elementStyle.BackColor;
						val5.SurroundColors = new Color[1] { elementStyle.BackColor2 };
						val5.InterpolationColors = elementStyle.BackColorBlend.GetColorBlend();
						e.Graphics.FillPath((Brush)(object)val5, val);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
					val4.Dispose();
				}
			}
			catch
			{
				eBackgroundColorBlendType2 = eBackgroundColorBlendType.Invalid;
			}
			break;
		default:
		{
			Graphics graphics = e.Graphics;
			rectangle = e.Bounds;
			if (clip != null)
			{
				e.Graphics.SetClip(clip, (CombineMode)0);
				e.Graphics.SetClip(val, (CombineMode)1);
			}
			else
			{
				e.Graphics.SetClip(val, (CombineMode)0);
			}
			BackgroundColorBlendCollection backColorBlend = elementStyle.BackColorBlend;
			for (int i = 0; i < backColorBlend.Count; i += 2)
			{
				BackgroundColorBlend backgroundColorBlend = backColorBlend[i];
				BackgroundColorBlend backgroundColorBlend2 = null;
				if (i < backColorBlend.Count)
				{
					backgroundColorBlend2 = backColorBlend[i + 1];
				}
				if (backgroundColorBlend != null && backgroundColorBlend2 != null)
				{
					Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Y + (int)backgroundColorBlend.Position, rectangle.Width, ((backgroundColorBlend2.Position == 1f) ? rectangle.Height : ((int)backgroundColorBlend2.Position)) - (int)backgroundColorBlend.Position);
					LinearGradientBrush val2 = DisplayHelp.CreateLinearGradientBrush(rectangle2, backgroundColorBlend.Color, backgroundColorBlend2.Color, elementStyle.BackColorGradientAngle);
					try
					{
						graphics.FillRectangle((Brush)(object)val2, rectangle2);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
			}
			break;
		}
		case eBackgroundColorBlendType.Invalid:
			break;
		}
		if (eBackgroundColorBlendType2 == eBackgroundColorBlendType.Invalid)
		{
			if (elementStyle.BackColor2.IsEmpty)
			{
				if (!elementStyle.BackColor.IsEmpty)
				{
					SolidBrush val6 = new SolidBrush(elementStyle.BackColor);
					try
					{
						e.Graphics.FillPath((Brush)(object)val6, val);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
			}
			else if (!elementStyle.BackColor.IsEmpty)
			{
				if (elementStyle.BackColorGradientType == eGradientType.Linear)
				{
					Rectangle r2 = rectangle;
					r2.X--;
					r2.Height++;
					r2.Width += 2;
					LinearGradientBrush val7 = DisplayHelp.CreateLinearGradientBrush(r2, elementStyle.BackColor, elementStyle.BackColor2, elementStyle.BackColorGradientAngle);
					try
					{
						e.Graphics.FillPath((Brush)(object)val7, val);
					}
					finally
					{
						((IDisposable)val7)?.Dispose();
					}
				}
				else if (elementStyle.BackColorGradientType == eGradientType.Radial)
				{
					int num2 = (int)Math.Sqrt(rectangle.Width * rectangle.Width + rectangle.Height * rectangle.Height) + 4;
					GraphicsPath val8 = new GraphicsPath();
					val8.AddEllipse(rectangle.X - (num2 - rectangle.Width) / 2, rectangle.Y - (num2 - rectangle.Height) / 2, num2, num2);
					PathGradientBrush val9 = new PathGradientBrush(val8);
					try
					{
						val9.CenterColor = elementStyle.BackColor;
						val9.SurroundColors = new Color[1] { elementStyle.BackColor2 };
						e.Graphics.FillPath((Brush)(object)val9, val);
					}
					finally
					{
						((IDisposable)val9)?.Dispose();
					}
					val8.Dispose();
				}
			}
		}
		if (val != null)
		{
			val.Dispose();
		}
		e.Graphics.SmoothingMode = smoothingMode;
		if (clip != null)
		{
			e.Graphics.Clip = clip;
			clip.Dispose();
			clip = null;
		}
		else
		{
			e.Graphics.ResetClip();
		}
		if (dispose)
		{
			elementStyle.Dispose();
		}
	}

	public static void PaintBackgroundImage(ElementStyleDisplayInfo e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Expected O, but got Unknown
		bool dispose = false;
		ElementStyle elementStyle = GetElementStyle(e.Style, out dispose);
		GraphicsPath val = null;
		try
		{
			if (elementStyle.BackgroundImage == null)
			{
				return;
			}
			Rectangle backgroundRectangle = GetBackgroundRectangle(elementStyle, e.Bounds);
			if ((int)e.Graphics.SmoothingMode == 4)
			{
				Rectangle bounds = e.Bounds;
				val = GetBackgroundPath(elementStyle, bounds);
			}
			else
			{
				val = GetBackgroundPath(elementStyle, e.Bounds);
			}
			ImageAttributes val2 = null;
			if (elementStyle.BackgroundImageAlpha != byte.MaxValue)
			{
				float[][] array = new float[5][]
				{
					new float[5] { 1f, 0f, 0f, 0f, 0f },
					new float[5] { 0f, 1f, 0f, 0f, 0f },
					new float[5] { 0f, 0f, 1f, 0f, 0f },
					new float[5]
					{
						0f,
						0f,
						0f,
						(float)(int)elementStyle.BackgroundImageAlpha / 255f,
						0f
					},
					new float[5] { 0f, 0f, 0f, 0f, 1f }
				};
				ColorMatrix val3 = new ColorMatrix(array);
				val2 = new ImageAttributes();
				val2.SetColorMatrix(val3, (ColorMatrixFlag)0, (ColorAdjustType)1);
			}
			Region clip = e.Graphics.Clip;
			e.Graphics.SetClip(val, (CombineMode)1);
			eStyleBackgroundImage eStyleBackgroundImage2 = elementStyle.BackgroundImagePosition;
			bool flag = false;
			Image val4 = elementStyle.BackgroundImage;
			if (e.RightToLeft)
			{
				switch (eStyleBackgroundImage2)
				{
				case eStyleBackgroundImage.TopLeft:
					eStyleBackgroundImage2 = eStyleBackgroundImage.TopRight;
					flag = true;
					break;
				case eStyleBackgroundImage.TopRight:
					eStyleBackgroundImage2 = eStyleBackgroundImage.TopLeft;
					break;
				case eStyleBackgroundImage.BottomLeft:
					eStyleBackgroundImage2 = eStyleBackgroundImage.BottomRight;
					break;
				case eStyleBackgroundImage.BottomRight:
					eStyleBackgroundImage2 = eStyleBackgroundImage.BottomLeft;
					break;
				}
			}
			if (flag)
			{
				object obj = val4.Clone();
				val4 = (Image)((obj is Image) ? obj : null);
				val4.RotateFlip((RotateFlipType)4);
			}
			switch (eStyleBackgroundImage2)
			{
			case eStyleBackgroundImage.Stretch:
				if (val2 != null)
				{
					e.Graphics.DrawImage(val4, backgroundRectangle, 0, 0, val4.Width, val4.Height, (GraphicsUnit)2, val2);
				}
				else
				{
					e.Graphics.DrawImage(val4, backgroundRectangle, 0, 0, val4.Width, val4.Height, (GraphicsUnit)2);
				}
				break;
			case eStyleBackgroundImage.Zoom:
			{
				Size size = val4.Size;
				Rectangle empty = Rectangle.Empty;
				float num = (float)backgroundRectangle.Width / (float)size.Width;
				float num2 = (float)backgroundRectangle.Height / (float)size.Height;
				if (num >= num2)
				{
					empty.Height = backgroundRectangle.Height;
					empty.Width = (int)((double)((float)size.Width * num2) + 0.5);
					if (backgroundRectangle.X >= 0)
					{
						empty.X = (backgroundRectangle.Width - empty.Width) / 2;
					}
				}
				else
				{
					empty.Width = backgroundRectangle.Width;
					empty.Height = (int)((double)((float)size.Height * num) + 0.5);
					if (backgroundRectangle.Y >= 0)
					{
						empty.Y = (backgroundRectangle.Height - empty.Height) / 2;
					}
				}
				backgroundRectangle = empty;
				if (val2 != null)
				{
					e.Graphics.DrawImage(val4, backgroundRectangle, 0, 0, val4.Width, val4.Height, (GraphicsUnit)2, val2);
				}
				else
				{
					e.Graphics.DrawImage(val4, backgroundRectangle, 0, 0, val4.Width, val4.Height, (GraphicsUnit)2);
				}
				break;
			}
			case eStyleBackgroundImage.Center:
			{
				Rectangle rectangle2 = new Rectangle(backgroundRectangle.X, backgroundRectangle.Y, val4.Width, val4.Height);
				if (backgroundRectangle.Width > val4.Width)
				{
					rectangle2.X += (backgroundRectangle.Width - val4.Width) / 2;
				}
				if (backgroundRectangle.Height > val4.Height)
				{
					rectangle2.Y += (backgroundRectangle.Height - val4.Height) / 2;
				}
				if (val2 != null)
				{
					e.Graphics.DrawImage(val4, rectangle2, 0, 0, val4.Width, val4.Height, (GraphicsUnit)2, val2);
				}
				else
				{
					e.Graphics.DrawImage(val4, rectangle2, 0, 0, val4.Width, val4.Height, (GraphicsUnit)2);
				}
				break;
			}
			case eStyleBackgroundImage.TopLeft:
			case eStyleBackgroundImage.TopRight:
			case eStyleBackgroundImage.BottomLeft:
			case eStyleBackgroundImage.BottomRight:
			{
				Rectangle rectangle3 = new Rectangle(backgroundRectangle.X, backgroundRectangle.Y, val4.Width, val4.Height);
				switch (eStyleBackgroundImage2)
				{
				case eStyleBackgroundImage.TopRight:
					rectangle3.X = backgroundRectangle.Right - val4.Width;
					break;
				case eStyleBackgroundImage.BottomLeft:
					rectangle3.Y = backgroundRectangle.Bottom - val4.Height;
					break;
				case eStyleBackgroundImage.BottomRight:
					rectangle3.Y = backgroundRectangle.Bottom - val4.Height;
					rectangle3.X = backgroundRectangle.Right - val4.Width;
					break;
				}
				if (val2 != null)
				{
					e.Graphics.DrawImage(val4, rectangle3, 0, 0, val4.Width, val4.Height, (GraphicsUnit)2, val2);
				}
				else
				{
					e.Graphics.DrawImage(val4, rectangle3, 0, 0, val4.Width, val4.Height, (GraphicsUnit)2);
				}
				break;
			}
			case eStyleBackgroundImage.Tile:
				if (val2 != null)
				{
					if (backgroundRectangle.Width > val4.Width || backgroundRectangle.Height > val4.Height)
					{
						int i = backgroundRectangle.X;
						for (int j = backgroundRectangle.Y; j < backgroundRectangle.Bottom; j += val4.Height)
						{
							for (; i < backgroundRectangle.Right; i += val4.Width)
							{
								Rectangle rectangle = new Rectangle(i, j, val4.Width, val4.Height);
								if (rectangle.Right > backgroundRectangle.Right)
								{
									rectangle.Width -= rectangle.Right - backgroundRectangle.Right;
								}
								if (rectangle.Bottom > backgroundRectangle.Bottom)
								{
									rectangle.Height -= rectangle.Bottom - backgroundRectangle.Bottom;
								}
								e.Graphics.DrawImage(val4, rectangle, 0, 0, rectangle.Width, rectangle.Height, (GraphicsUnit)2, val2);
							}
							i = backgroundRectangle.X;
						}
					}
					else
					{
						e.Graphics.DrawImage(val4, new Rectangle(0, 0, val4.Width, val4.Height), 0, 0, val4.Width, val4.Height, (GraphicsUnit)2, val2);
					}
				}
				else
				{
					TextureBrush val5 = new TextureBrush(val4);
					val5.WrapMode = (WrapMode)0;
					e.Graphics.FillPath((Brush)(object)val5, val);
					((Brush)val5).Dispose();
				}
				break;
			}
			if (flag)
			{
				val4.Dispose();
			}
			if (clip != null)
			{
				e.Graphics.Clip = clip;
				clip.Dispose();
			}
			else
			{
				e.Graphics.ResetClip();
			}
		}
		finally
		{
			if (val != null)
			{
				val.Dispose();
			}
			if (dispose)
			{
				elementStyle.Dispose();
			}
		}
	}

	public static Rectangle GetBackgroundRectangle(ElementStyle style, Rectangle bounds)
	{
		bounds.X += style.MarginLeft;
		bounds.Width -= style.MarginLeft + style.MarginRight;
		bounds.Y += style.MarginTop;
		bounds.Height -= style.MarginTop + style.MarginBottom;
		return bounds;
	}

	private static Rectangle GetBorderRectangle(ElementStyle style, Rectangle bounds)
	{
		if (style.PaintRightBorder)
		{
			if (style.BorderRightWidth <= 1)
			{
				bounds.Width -= style.BorderRightWidth;
			}
			else
			{
				bounds.Width -= style.BorderRightWidth / 2;
			}
		}
		if (style.PaintBottomBorder)
		{
			if (style.BorderBottomWidth <= 1)
			{
				bounds.Height -= style.BorderBottomWidth;
			}
			else
			{
				bounds.Height -= style.BorderBottomWidth / 2;
			}
		}
		if (style.PaintLeftBorder && style.BorderLeftWidth > 1)
		{
			bounds.X += style.BorderLeftWidth / 2;
			bounds.Width -= style.BorderLeftWidth / 2;
		}
		if (style.PaintTopBorder && style.BorderTopWidth > 1)
		{
			bounds.Y += style.BorderTopWidth / 2;
			bounds.Height -= style.BorderTopWidth / 2;
		}
		return bounds;
	}

	private static Rectangle GetTextRectangle(ElementStyle style, Rectangle bounds)
	{
		bounds = GetBackgroundRectangle(style, bounds);
		bounds.X += style.PaddingLeft;
		bounds.Width -= style.PaddingLeft + style.PaddingRight;
		bounds.Y += style.PaddingTop;
		bounds.Height -= style.PaddingTop + style.PaddingBottom;
		return bounds;
	}

	public static GraphicsPath GetBackgroundPath(ElementStyle style, Rectangle bounds)
	{
		return GetBackgroundPath(style, bounds, eStyleBackgroundPathPart.Complete);
	}

	public static GraphicsPath GetBackgroundPath(ElementStyle style, Rectangle bounds, eStyleBackgroundPathPart pathPart)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle backgroundRectangle = GetBackgroundRectangle(style, bounds);
		if (pathPart == eStyleBackgroundPathPart.Complete && style.CornerTypeBottomLeft == eCornerType.Inherit && style.CornerTypeBottomRight == eCornerType.Inherit && style.CornerTypeTopLeft == eCornerType.Inherit && style.CornerTypeTopRight == eCornerType.Inherit)
		{
			switch (style.CornerType)
			{
			case eCornerType.Square:
				val.AddRectangle(backgroundRectangle);
				break;
			case eCornerType.Rounded:
			{
				ArcData cornerArc = GetCornerArc(backgroundRectangle, style.CornerDiameter, eCornerArc.TopLeft);
				val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
				cornerArc = GetCornerArc(backgroundRectangle, style.CornerDiameter, eCornerArc.TopRight);
				val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
				cornerArc = GetCornerArc(backgroundRectangle, style.CornerDiameter, eCornerArc.BottomRight);
				val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
				cornerArc = GetCornerArc(backgroundRectangle, style.CornerDiameter, eCornerArc.BottomLeft);
				val.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
				val.CloseAllFigures();
				break;
			}
			case eCornerType.Diagonal:
				val.AddLine(backgroundRectangle.X, backgroundRectangle.Bottom - style.CornerDiameter - 1, backgroundRectangle.X, backgroundRectangle.Y + style.CornerDiameter);
				val.AddLine(backgroundRectangle.X + style.CornerDiameter, backgroundRectangle.Y, backgroundRectangle.Right - style.CornerDiameter, backgroundRectangle.Y);
				val.AddLine(backgroundRectangle.Right, backgroundRectangle.Y + style.CornerDiameter, backgroundRectangle.Right, backgroundRectangle.Bottom - style.CornerDiameter - 1);
				val.AddLine(backgroundRectangle.Right - style.CornerDiameter - 1, backgroundRectangle.Bottom, backgroundRectangle.X + style.CornerDiameter, backgroundRectangle.Bottom);
				val.CloseAllFigures();
				break;
			}
		}
		else
		{
			switch (pathPart)
			{
			case eStyleBackgroundPathPart.TopHalf:
				backgroundRectangle.Height /= 2;
				break;
			case eStyleBackgroundPathPart.BottomHalf:
			{
				int height = backgroundRectangle.Height;
				backgroundRectangle.Height /= 2;
				backgroundRectangle.Y += height - backgroundRectangle.Height - 1;
				break;
			}
			}
			eCornerType eCornerType2 = style.CornerTypeTopLeft;
			if (eCornerType2 == eCornerType.Inherit)
			{
				eCornerType2 = style.CornerType;
			}
			if (pathPart == eStyleBackgroundPathPart.BottomHalf)
			{
				eCornerType2 = eCornerType.Square;
			}
			switch (eCornerType2)
			{
			case eCornerType.Rounded:
			{
				ArcData cornerArc2 = GetCornerArc(backgroundRectangle, style.CornerDiameter, eCornerArc.TopLeft);
				val.AddArc(cornerArc2.X, cornerArc2.Y, cornerArc2.Width, cornerArc2.Height, cornerArc2.StartAngle, cornerArc2.SweepAngle);
				break;
			}
			case eCornerType.Diagonal:
				val.AddLine(backgroundRectangle.X, backgroundRectangle.Y + style.CornerDiameter, backgroundRectangle.X + style.CornerDiameter, backgroundRectangle.Y);
				break;
			default:
				val.AddLine(backgroundRectangle.X, backgroundRectangle.Y + 2, backgroundRectangle.X, backgroundRectangle.Y);
				val.AddLine(backgroundRectangle.X, backgroundRectangle.Y, backgroundRectangle.X + 2, backgroundRectangle.Y);
				break;
			}
			eCornerType2 = style.CornerTypeTopRight;
			if (eCornerType2 == eCornerType.Inherit)
			{
				eCornerType2 = style.CornerType;
			}
			if (pathPart == eStyleBackgroundPathPart.BottomHalf)
			{
				eCornerType2 = eCornerType.Square;
			}
			switch (eCornerType2)
			{
			case eCornerType.Rounded:
			{
				ArcData cornerArc3 = GetCornerArc(backgroundRectangle, style.CornerDiameter, eCornerArc.TopRight);
				val.AddArc(cornerArc3.X, cornerArc3.Y, cornerArc3.Width, cornerArc3.Height, cornerArc3.StartAngle, cornerArc3.SweepAngle);
				break;
			}
			case eCornerType.Diagonal:
				val.AddLine(backgroundRectangle.Right - style.CornerDiameter - 1, backgroundRectangle.Y, backgroundRectangle.Right, backgroundRectangle.Y + style.CornerDiameter);
				break;
			default:
				val.AddLine(backgroundRectangle.Right - 2, backgroundRectangle.Y, backgroundRectangle.Right, backgroundRectangle.Y);
				val.AddLine(backgroundRectangle.Right, backgroundRectangle.Y, backgroundRectangle.Right, backgroundRectangle.Y + 2);
				break;
			}
			eCornerType2 = style.CornerTypeBottomRight;
			if (eCornerType2 == eCornerType.Inherit)
			{
				eCornerType2 = style.CornerType;
			}
			if (pathPart == eStyleBackgroundPathPart.TopHalf)
			{
				eCornerType2 = eCornerType.Square;
			}
			switch (eCornerType2)
			{
			case eCornerType.Rounded:
			{
				ArcData cornerArc4 = GetCornerArc(backgroundRectangle, style.CornerDiameter, eCornerArc.BottomRight);
				val.AddArc(cornerArc4.X, cornerArc4.Y, cornerArc4.Width, cornerArc4.Height, cornerArc4.StartAngle, cornerArc4.SweepAngle);
				break;
			}
			case eCornerType.Diagonal:
				val.AddLine(backgroundRectangle.Right, backgroundRectangle.Bottom - style.CornerDiameter - 1, backgroundRectangle.Right - style.CornerDiameter - 1, backgroundRectangle.Bottom);
				break;
			default:
				val.AddLine(backgroundRectangle.Right, backgroundRectangle.Bottom - 2, backgroundRectangle.Right, backgroundRectangle.Bottom);
				val.AddLine(backgroundRectangle.Right, backgroundRectangle.Bottom, backgroundRectangle.Right - 2, backgroundRectangle.Bottom);
				break;
			}
			eCornerType2 = style.CornerTypeBottomLeft;
			if (eCornerType2 == eCornerType.Inherit)
			{
				eCornerType2 = style.CornerType;
			}
			if (pathPart == eStyleBackgroundPathPart.TopHalf)
			{
				eCornerType2 = eCornerType.Square;
			}
			switch (eCornerType2)
			{
			case eCornerType.Rounded:
			{
				ArcData cornerArc5 = GetCornerArc(backgroundRectangle, style.CornerDiameter, eCornerArc.BottomLeft);
				val.AddArc(cornerArc5.X, cornerArc5.Y, cornerArc5.Width, cornerArc5.Height, cornerArc5.StartAngle, cornerArc5.SweepAngle);
				break;
			}
			case eCornerType.Diagonal:
				val.AddLine(backgroundRectangle.X + 2, backgroundRectangle.Bottom, backgroundRectangle.X, backgroundRectangle.Bottom - style.CornerDiameter - 1);
				break;
			default:
				val.AddLine(backgroundRectangle.X + 2, backgroundRectangle.Bottom, backgroundRectangle.X, backgroundRectangle.Bottom);
				val.AddLine(backgroundRectangle.X, backgroundRectangle.Bottom, backgroundRectangle.X, backgroundRectangle.Bottom - 2);
				break;
			}
		}
		return val;
	}

	public static void AddCornerArc(GraphicsPath path, Rectangle bounds, int cornerDiameter, eCornerArc corner)
	{
		if (cornerDiameter > 0)
		{
			ArcData cornerArc = GetCornerArc(bounds, cornerDiameter, corner);
			path.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
			return;
		}
		switch (corner)
		{
		case eCornerArc.TopLeft:
			path.AddLine(bounds.X, bounds.Y + 2, bounds.X, bounds.Y);
			break;
		case eCornerArc.BottomLeft:
			path.AddLine(bounds.X + 2, bounds.Bottom, bounds.X, bounds.Bottom);
			break;
		case eCornerArc.TopRight:
			path.AddLine(bounds.Right - 2, bounds.Y, bounds.Right, bounds.Y);
			break;
		case eCornerArc.BottomRight:
			path.AddLine(bounds.Right, bounds.Bottom - 2, bounds.Right, bounds.Bottom);
			break;
		}
	}

	internal static ArcData GetCornerArc(Rectangle bounds, int cornerDiameter, eCornerArc corner)
	{
		if (cornerDiameter == 0)
		{
			cornerDiameter = 1;
		}
		int num = cornerDiameter * 2;
		return corner switch
		{
			eCornerArc.TopLeft => new ArcData(bounds.X, bounds.Y, num, num, 180f, 90f), 
			eCornerArc.TopRight => new ArcData(bounds.Right - num, bounds.Y, num, num, 270f, 90f), 
			eCornerArc.BottomLeft => new ArcData(bounds.X, bounds.Bottom - num, num, num, 90f, 90f), 
			_ => new ArcData(bounds.Right - num, bounds.Bottom - num, num, num, 0f, 90f), 
		};
	}
}
