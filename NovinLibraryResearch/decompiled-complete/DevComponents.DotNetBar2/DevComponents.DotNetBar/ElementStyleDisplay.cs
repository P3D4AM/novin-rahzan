using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class ElementStyleDisplay
{
	internal struct ឦ
	{
		public bool ឧ;

		public bool ឨ;

		public bool ឩ;

		public bool ឪ;

		public bool ឫ;

		public ឦ(bool hasLeftBorder, bool hasRightBorder, bool hasTopBorder, bool hasBottomBorder, bool isTwistedDoubleBorder)
		{
			ឧ = hasLeftBorder;
			ឨ = hasRightBorder;
			ឩ = hasTopBorder;
			ឪ = hasBottomBorder;
			ឫ = isTwistedDoubleBorder;
		}
	}

	private enum ឬ
	{
		_074A,
		ራ,
		_0749,
		ሪ,
		ឭ,
		ឮ,
		ឯ,
		ឰ
	}

	public static void PaintText(ElementStyleDisplayInfo e, string text, Font defaultFont)
	{
		PaintText(e, text, defaultFont, useDefaultFont: false);
	}

	public static void PaintText(ElementStyleDisplayInfo e, string text, Font defaultFont, bool useDefaultFont)
	{
		PaintText(e, text, defaultFont, useDefaultFont, e.Style.ݽ);
	}

	public static void PaintText(ElementStyleDisplayInfo e, string text, Font defaultFont, bool useDefaultFont, eTextFormat textFormat)
	{
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		bool ല = false;
		ElementStyle elementStyle = റ(e.Style, out ല);
		if (text == "" || bounds.IsEmpty)
		{
			if (ല)
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
		bounds.X += elementStyle.MarginLeft + elementStyle.PaddingLeft;
		bounds.Width -= elementStyle.MarginLeft + elementStyle.MarginRight + elementStyle.PaddingLeft + elementStyle.PaddingRight;
		bounds.Y += elementStyle.MarginTop + elementStyle.PaddingTop;
		bounds.Height -= elementStyle.MarginTop + elementStyle.MarginBottom + elementStyle.PaddingTop + elementStyle.PaddingBottom;
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
		if (ല)
		{
			elementStyle.Dispose();
		}
	}

	public static Region GetStyleRegion(ElementStyleDisplayInfo e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		if (e.Style.ނ && e.Style.CornerType != eCornerType.Square)
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
			if (e.Style.ނ && (e.Style.CornerType == eCornerType.Rounded || e.Style.CornerType == eCornerType.Diagonal || e.Style.CornerTypeTopLeft == eCornerType.Rounded || e.Style.CornerTypeTopLeft == eCornerType.Diagonal || e.Style.CornerTypeTopRight == eCornerType.Rounded || e.Style.CornerTypeTopRight == eCornerType.Diagonal || e.Style.CornerTypeBottomLeft == eCornerType.Rounded || e.Style.CornerTypeBottomLeft == eCornerType.Diagonal || e.Style.CornerTypeBottomRight == eCornerType.Rounded || e.Style.CornerTypeBottomRight == eCornerType.Diagonal))
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
		if (e.Style.ނ)
		{
			if (e.Style.ހ)
			{
				bounds.Y += e.Style.BorderTopWidth;
				bounds.Height -= e.Style.BorderTopWidth;
				if (e.Style.BorderTop == eStyleBorderType.Etched || e.Style.BorderTop == eStyleBorderType.Double)
				{
					bounds.Y += e.Style.BorderTopWidth;
					bounds.Height -= e.Style.BorderTopWidth;
				}
			}
			if (e.Style.ށ)
			{
				bounds.Height -= e.Style.BorderBottomWidth;
				if (e.Style.BorderBottom == eStyleBorderType.Etched || e.Style.BorderBottom == eStyleBorderType.Double)
				{
					bounds.Height -= e.Style.BorderBottomWidth;
				}
			}
			if (e.Style.ݾ)
			{
				bounds.X += e.Style.BorderLeftWidth;
				bounds.Width -= e.Style.BorderLeftWidth;
				if (e.Style.BorderLeft == eStyleBorderType.Etched || e.Style.BorderLeft == eStyleBorderType.Double)
				{
					bounds.X += e.Style.BorderLeftWidth;
					bounds.Width -= e.Style.BorderLeftWidth;
				}
			}
			if (e.Style.ݿ)
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

	private static eCornerType ണ(eCornerType ത, eCornerType ഥ)
	{
		if (ഥ != 0)
		{
			return ഥ;
		}
		return ത;
	}

	public static void PaintBorder(ElementStyleDisplayInfo e)
	{
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Expected O, but got Unknown
		bool ല = false;
		ElementStyle elementStyle = റ(e.Style, out ല);
		Rectangle backgroundRectangle = GetBackgroundRectangle(elementStyle, e.Bounds);
		Rectangle rectangle = വ(elementStyle, backgroundRectangle);
		try
		{
			if (rectangle.Width < 2 || rectangle.Height < 2)
			{
				return;
			}
			eCornerType eCornerType2 = ണ(elementStyle.CornerType, elementStyle.CornerTypeTopLeft);
			eCornerType eCornerType3 = ണ(elementStyle.CornerType, elementStyle.CornerTypeTopRight);
			eCornerType eCornerType4 = ണ(elementStyle.CornerType, elementStyle.CornerTypeBottomLeft);
			eCornerType eCornerType5 = ണ(elementStyle.CornerType, elementStyle.CornerTypeBottomRight);
			if (elementStyle.ނ && elementStyle.Border == eStyleBorderType.Solid && eCornerType2 == eCornerType.Rounded && eCornerType3 == eCornerType.Rounded && eCornerType4 == eCornerType.Rounded && eCornerType5 == eCornerType.Rounded && elementStyle.BorderLeftWidth == elementStyle.BorderWidth && elementStyle.BorderRightWidth == elementStyle.BorderWidth && elementStyle.BorderTopWidth == elementStyle.BorderWidth && elementStyle.BorderBottomWidth == elementStyle.BorderWidth)
			{
				if (elementStyle.BorderWidth == 1)
				{
					rectangle.Width++;
					rectangle.Height++;
				}
				DisplayHelp.DrawRoundGradientRectangle(e.Graphics, rectangle, elementStyle.BorderColor, elementStyle.BorderColor2, elementStyle.BorderGradientAngle, elementStyle.BorderWidth, elementStyle.CornerDiameter);
				return;
			}
			if (elementStyle.ރ && elementStyle.Border == eStyleBorderType.Solid && eCornerType2 == eCornerType.Square && eCornerType3 == eCornerType.Square && eCornerType4 == eCornerType.Square && eCornerType5 == eCornerType.Square && elementStyle.BorderLeftWidth == elementStyle.BorderWidth && elementStyle.BorderRightWidth == elementStyle.BorderWidth && elementStyle.BorderTopWidth == elementStyle.BorderWidth && elementStyle.BorderBottomWidth == elementStyle.BorderWidth)
			{
				if (elementStyle.BorderWidth == 1)
				{
					rectangle.Width++;
					rectangle.Height++;
				}
				DisplayHelp.DrawGradientRectangle(e.Graphics, rectangle, elementStyle.BorderColor, elementStyle.BorderColor2, elementStyle.BorderGradientAngle, elementStyle.BorderWidth);
				return;
			}
			if (elementStyle.Border == eStyleBorderType.None && elementStyle.BorderRight == eStyleBorderType.Etched && elementStyle.BorderLeft == eStyleBorderType.None && elementStyle.BorderTop == eStyleBorderType.None && elementStyle.BorderBottom == eStyleBorderType.None)
			{
				DisplayHelp.DrawGradientRectangle(e.Graphics, new Rectangle(rectangle.Right - 2, rectangle.Y, 3, rectangle.Height + 1), elementStyle.BorderColorLight, elementStyle.BorderColorLight2, 90, 1);
				DisplayHelp.DrawGradientLine(e.Graphics, new Point(rectangle.Right, rectangle.Y), new Point(rectangle.Right, rectangle.Bottom + 1), elementStyle.BorderColor, elementStyle.BorderColor2, 90, 1);
				return;
			}
			if (!elementStyle.BorderColor2.IsEmpty || !elementStyle.BorderColorLight.IsEmpty)
			{
				if (elementStyle.Border == eStyleBorderType.Solid)
				{
					GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(rectangle, elementStyle.CornerDiameter, eStyleBackgroundPathPart.Complete, eCornerType2, eCornerType3, eCornerType4, eCornerType5);
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
					Rectangle clientRectangle = rectangle;
					clientRectangle.Width -= elementStyle.BorderWidth;
					clientRectangle.Height -= elementStyle.BorderWidth;
					clientRectangle.Offset(elementStyle.BorderWidth, elementStyle.BorderWidth);
					if (clientRectangle.Width > 2 && clientRectangle.Height > 2)
					{
						GraphicsPath roundedRectanglePath2 = DisplayHelp.GetRoundedRectanglePath(clientRectangle, elementStyle.CornerDiameter, eStyleBackgroundPathPart.Complete, eCornerType2, eCornerType3, eCornerType4, eCornerType5);
						try
						{
							DisplayHelp.DrawGradientPathBorder(e.Graphics, roundedRectanglePath2, elementStyle.BorderColorLight, elementStyle.BorderColorLight2, elementStyle.BorderLightGradientAngle, elementStyle.BorderWidth);
						}
						finally
						{
							((IDisposable)roundedRectanglePath2)?.Dispose();
						}
						clientRectangle.Offset(-elementStyle.BorderWidth, -elementStyle.BorderWidth);
						GraphicsPath roundedRectanglePath3 = DisplayHelp.GetRoundedRectanglePath(clientRectangle, elementStyle.CornerDiameter, eStyleBackgroundPathPart.Complete, eCornerType2, eCornerType3, eCornerType4, eCornerType5);
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
					Rectangle rectangle2 = rectangle;
					Region clip = null;
					bool flag = false;
					if (elementStyle.BorderTop == eStyleBorderType.None)
					{
						Rectangle clip2 = rectangle2;
						clip2.Width++;
						clip2.Height++;
						e.Graphics.SetClip(clip2);
						rectangle2.Y -= 3;
						rectangle2.Height += 3;
					}
					GraphicsPath borderPath = DisplayHelp.GetBorderPath(rectangle2, elementStyle.CornerDiameter, eStyleBackgroundPathPart.Complete, eCornerType2, eCornerType3, eCornerType4, eCornerType5, elementStyle.ݾ, elementStyle.ݿ, elementStyle.ހ, elementStyle.ށ);
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
					rectangle2.Inflate(-elementStyle.BorderWidth, -elementStyle.BorderWidth);
					GraphicsPath borderPath2 = DisplayHelp.GetBorderPath(rectangle2, elementStyle.CornerDiameter, eStyleBackgroundPathPart.Complete, eCornerType2, eCornerType3, eCornerType4, eCornerType5, elementStyle.ݾ, elementStyle.ݿ, elementStyle.ހ, elementStyle.ށ);
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
			_ = elementStyle.BorderColor;
			Color borderColor = elementStyle.BorderColor2;
			Color borderColorLight = elementStyle.BorderColorLight;
			Color borderColorLight2 = elementStyle.BorderColorLight2;
			ឦ ഭ = new ឦ(elementStyle.ݾ, elementStyle.ݿ, elementStyle.ހ, elementStyle.ށ, e.IsTwistedDoubleBorder);
			if (elementStyle.ݾ)
			{
				Color color = elementStyle.BorderColor;
				if (!elementStyle.BorderLeftColor.IsEmpty)
				{
					color = elementStyle.BorderLeftColor;
				}
				Point[] array = new Point[2]
				{
					rectangle.Location,
					default(Point)
				};
				array[1].X = rectangle.X;
				array[1].Y = rectangle.Bottom;
				if (eCornerType2 != eCornerType.Square)
				{
					array[0].Y += elementStyle.CornerDiameter;
				}
				if (eCornerType4 != eCornerType.Square)
				{
					array[1].Y -= elementStyle.CornerDiameter;
				}
				ന(e.Graphics, array, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color, borderColor, borderColorLight, borderColorLight2, ឬ._0749, ഭ);
				if (elementStyle.ހ)
				{
					switch (eCornerType2)
					{
					case eCornerType.Diagonal:
						array[0].X = rectangle.X;
						array[0].Y = rectangle.Y + elementStyle.CornerDiameter;
						array[1].X = rectangle.X + elementStyle.CornerDiameter;
						array[1].Y = rectangle.Y;
						ന(e.Graphics, array, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color, borderColor, borderColorLight, borderColorLight2, ឬ.ឭ, ഭ);
						break;
					case eCornerType.Rounded:
					{
						ᒇ യ = സ(rectangle, elementStyle.CornerDiameter, eCornerArc.TopLeft);
						മ(e.Graphics, യ, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color);
						break;
					}
					}
				}
				if (elementStyle.ށ)
				{
					switch (eCornerType4)
					{
					case eCornerType.Diagonal:
						array[0].X = rectangle.X;
						array[0].Y = rectangle.Bottom - elementStyle.CornerDiameter;
						array[1].X = rectangle.X + elementStyle.CornerDiameter;
						array[1].Y = rectangle.Bottom;
						ന(e.Graphics, array, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color, borderColor, borderColorLight, borderColorLight2, ឬ.ឯ, ഭ);
						break;
					case eCornerType.Rounded:
					{
						ᒇ യ2 = സ(rectangle, elementStyle.CornerDiameter, eCornerArc.BottomLeft);
						മ(e.Graphics, യ2, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color);
						break;
					}
					}
				}
			}
			if (elementStyle.ހ)
			{
				Color ഩ = elementStyle.BorderColor;
				if (!elementStyle.BorderTopColor.IsEmpty)
				{
					ഩ = elementStyle.BorderTopColor;
				}
				Point[] array2 = new Point[2]
				{
					rectangle.Location,
					default(Point)
				};
				array2[1].X = rectangle.Right;
				array2[1].Y = rectangle.Y;
				if (eCornerType2 != eCornerType.Square)
				{
					array2[0].X += elementStyle.CornerDiameter;
				}
				if (eCornerType3 != eCornerType.Square)
				{
					array2[1].X -= elementStyle.CornerDiameter;
				}
				ന(e.Graphics, array2, elementStyle.BorderTop, elementStyle.BorderTopWidth, ഩ, borderColor, borderColorLight, borderColorLight2, ឬ._074A, ഭ);
			}
			if (elementStyle.ށ)
			{
				Color ഩ2 = elementStyle.BorderColor;
				if (!elementStyle.BorderBottomColor.IsEmpty)
				{
					ഩ2 = elementStyle.BorderBottomColor;
				}
				Point[] array3 = new Point[2];
				array3[0].X = rectangle.X;
				array3[0].Y = rectangle.Bottom;
				array3[1].X = rectangle.Right;
				array3[1].Y = rectangle.Bottom;
				if (eCornerType4 != eCornerType.Square)
				{
					array3[0].X += elementStyle.CornerDiameter;
				}
				if (eCornerType5 != eCornerType.Square)
				{
					array3[1].X -= elementStyle.CornerDiameter;
				}
				ന(e.Graphics, array3, elementStyle.BorderBottom, elementStyle.BorderBottomWidth, ഩ2, borderColor, borderColorLight, borderColorLight2, ឬ.ራ, ഭ);
			}
			if (!elementStyle.ݿ)
			{
				return;
			}
			Color color2 = elementStyle.BorderColor;
			if (!elementStyle.BorderRightColor.IsEmpty)
			{
				color2 = elementStyle.BorderRightColor;
			}
			Point[] array4 = new Point[2];
			array4[0].X = rectangle.Right;
			array4[0].Y = rectangle.Y;
			array4[1].X = rectangle.Right;
			array4[1].Y = rectangle.Bottom;
			if (eCornerType3 != eCornerType.Square)
			{
				array4[0].Y += elementStyle.CornerDiameter;
			}
			if (eCornerType5 != eCornerType.Square)
			{
				array4[1].Y -= elementStyle.CornerDiameter;
			}
			ന(e.Graphics, array4, elementStyle.BorderRight, elementStyle.BorderRightWidth, color2, borderColor, borderColorLight, borderColorLight2, ឬ.ሪ, ഭ);
			if (elementStyle.ހ)
			{
				switch (eCornerType3)
				{
				case eCornerType.Diagonal:
					array4[0].X = rectangle.Right - elementStyle.CornerDiameter;
					array4[0].Y = rectangle.Y;
					array4[1].X = rectangle.Right;
					array4[1].Y = rectangle.Y + elementStyle.CornerDiameter;
					ന(e.Graphics, array4, elementStyle.BorderLeft, elementStyle.BorderRightWidth, color2, borderColor, borderColorLight, borderColorLight2, ឬ.ឮ, ഭ);
					break;
				case eCornerType.Rounded:
				{
					ᒇ യ3 = സ(rectangle, elementStyle.CornerDiameter, eCornerArc.TopRight);
					മ(e.Graphics, യ3, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color2);
					break;
				}
				}
			}
			if (elementStyle.ށ)
			{
				switch (eCornerType5)
				{
				case eCornerType.Diagonal:
					array4[0].X = rectangle.Right;
					array4[0].Y = rectangle.Bottom - elementStyle.CornerDiameter;
					array4[1].X = rectangle.Right - elementStyle.CornerDiameter;
					array4[1].Y = rectangle.Bottom;
					ന(e.Graphics, array4, elementStyle.BorderLeft, elementStyle.BorderRightWidth, color2, borderColor, borderColorLight, borderColorLight2, ឬ.ឰ, ഭ);
					break;
				case eCornerType.Rounded:
				{
					ᒇ യ4 = സ(rectangle, elementStyle.CornerDiameter, eCornerArc.BottomRight);
					മ(e.Graphics, യ4, elementStyle.BorderLeft, elementStyle.BorderLeftWidth, color2);
					break;
				}
				}
			}
		}
		finally
		{
			if (ല)
			{
				elementStyle.Dispose();
			}
		}
	}

	private static Pen ദ(eStyleBorderType ധ, int _७, Color _065B)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		Pen val = new Pen(_065B, (float)_७);
		val.Alignment = (PenAlignment)1;
		val.DashStyle = ര(ധ);
		return val;
	}

	private static void ന(Graphics _٥, Point[] _065C, eStyleBorderType ധ, int _७, Color ഩ, Color പ, Color ഫ, Color ബ, ឬ _५, ឦ ഭ)
	{
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		if (ധ == eStyleBorderType.Etched || ധ == eStyleBorderType.Double)
		{
			if (ഫ.IsEmpty)
			{
				ഫ = ControlPaint.Light(ഩ);
				ഩ = ControlPaint.Dark(ഩ);
			}
			if (ഭ.ឫ && (_५ == ឬ.ራ || _५ == ឬ.ሪ))
			{
				Color color = ഩ;
				Color color2 = പ;
				ഩ = ഫ;
				പ = ബ;
				ഫ = color;
				ബ = color2;
			}
			DisplayHelp.DrawGradientLine(_٥, _065C[0], _065C[1], ഩ, പ, 90, _७);
			switch (_५)
			{
			case ឬ._074A:
				_065C[0].Y += _७;
				_065C[1].Y += _७;
				if (ഭ.ឧ)
				{
					_065C[0].X++;
				}
				if (ഭ.ឨ)
				{
					_065C[1].X--;
				}
				break;
			case ឬ.ራ:
				_065C[0].Y -= _७;
				_065C[1].Y -= _७;
				if (!ഭ.ឫ)
				{
					if (ഭ.ឧ)
					{
						_065C[0].X++;
					}
					if (ഭ.ឨ)
					{
						_065C[1].X--;
					}
				}
				break;
			case ឬ._0749:
			case ឬ.ឭ:
			case ឬ.ឯ:
				_065C[0].X += _७;
				_065C[1].X += _७;
				if (ഭ.ឩ)
				{
					_065C[0].Y++;
				}
				if (ഭ.ឪ)
				{
					_065C[1].Y--;
				}
				break;
			case ឬ.ሪ:
			case ឬ.ឮ:
			case ឬ.ឰ:
				_065C[0].X -= _७;
				_065C[1].X -= _७;
				if (!ഭ.ឫ)
				{
					if (ഭ.ឩ)
					{
						_065C[0].Y++;
					}
					if (ഭ.ឪ)
					{
						_065C[1].Y--;
					}
				}
				break;
			}
			DisplayHelp.DrawGradientLine(_٥, _065C[0], _065C[1], ഫ, ബ, 90, _७);
		}
		else if (പ.IsEmpty)
		{
			SmoothingMode smoothingMode = _٥.SmoothingMode;
			_٥.SmoothingMode = (SmoothingMode)3;
			Pen val = ദ(ധ, _७, ഩ);
			try
			{
				_٥.DrawLine(val, _065C[0], _065C[1]);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			_٥.SmoothingMode = smoothingMode;
		}
		else
		{
			DisplayHelp.DrawGradientLine(_٥, _065C[0], _065C[1], ഩ, പ, 90, _७);
		}
	}

	private static void മ(Graphics _٥, ᒇ യ, eStyleBorderType ധ, int _७, Color _065B)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = _٥.SmoothingMode;
		if ((int)smoothingMode == 3)
		{
			_٥.SmoothingMode = (SmoothingMode)2;
		}
		Pen val = ദ(ധ, _७, _065B);
		try
		{
			_٥.DrawArc(val, (float)യ.ᒈ, (float)യ.ᒉ, (float)യ.ܐ, (float)യ._0711, യ.ᒊ, യ.ᒋ);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if ((int)smoothingMode == 3)
		{
			_٥.SmoothingMode = smoothingMode;
		}
	}

	private static DashStyle ര(eStyleBorderType ധ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		DashStyle result = (DashStyle)0;
		switch (ധ)
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

	internal static ElementStyle റ(ElementStyle _0657, out bool ല)
	{
		ല = false;
		if (_0657.Class == "")
		{
			return _0657;
		}
		IElementStyleClassProvider elementStyleClassProvider = ള();
		if (elementStyleClassProvider != null)
		{
			ElementStyle @class = elementStyleClassProvider.GetClass(_0657.Class);
			if (@class != null)
			{
				if (_0657.Custom)
				{
					ElementStyle elementStyle = @class.Copy();
					elementStyle.ApplyStyle(_0657);
					ല = true;
					return elementStyle;
				}
				return @class;
			}
		}
		return _0657;
	}

	private static IElementStyleClassProvider ള()
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
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		Region clip = e.Graphics.Clip;
		if (clip != null)
		{
			e.Graphics.SetClip(e.Bounds, (CombineMode)1);
		}
		else
		{
			e.Graphics.SetClip(e.Bounds, (CombineMode)0);
		}
		bool ല = false;
		ElementStyle elementStyle = റ(e.Style, out ല);
		SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
		e.Graphics.SmoothingMode = (SmoothingMode)0;
		Rectangle rectangle = DisplayHelp.GetDrawRectangle(GetBackgroundRectangle(elementStyle, e.Bounds));
		GraphicsPath val = null;
		if (shapeBackground && !ഴ(elementStyle))
		{
			val = GetBackgroundPath(elementStyle, വ(e.Style, e.Bounds));
		}
		else
		{
			val = new GraphicsPath();
			val.AddRectangle(GetBackgroundRectangle(elementStyle, e.Bounds));
		}
		ቸ ቸ2 = elementStyle.BackColorBlend.ት();
		switch (ቸ2)
		{
		case ቸ.ቺ:
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
				ቸ2 = ቸ.ቹ;
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
				if (i + 1 < backColorBlend.Count)
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
		case ቸ.ቹ:
			break;
		}
		if (ቸ2 == ቸ.ቹ)
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
		if (ല)
		{
			elementStyle.Dispose();
		}
	}

	private static bool ഴ(ElementStyle _0657)
	{
		if ((_0657.CornerType == eCornerType.Square || _0657.CornerType == eCornerType.Inherit) && (_0657.CornerTypeTopLeft == eCornerType.Square || _0657.CornerTypeTopLeft == eCornerType.Inherit))
		{
			if (_0657.CornerTypeTopRight != eCornerType.Square)
			{
				return _0657.CornerTypeTopRight == eCornerType.Inherit;
			}
			return true;
		}
		return false;
	}

	public static void PaintBackgroundImage(ElementStyleDisplayInfo e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		bool ല = false;
		ElementStyle elementStyle = റ(e.Style, out ല);
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
				Rectangle rectangle2 = new Rectangle(backgroundRectangle.X, backgroundRectangle.Y, Dpi.Width(val4.Width), Dpi.Height(val4.Height));
				if (backgroundRectangle.Width > rectangle2.Width)
				{
					rectangle2.X += (backgroundRectangle.Width - rectangle2.Width) / 2;
				}
				if (backgroundRectangle.Height > rectangle2.Height)
				{
					rectangle2.Y += (backgroundRectangle.Height - rectangle2.Height) / 2;
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
				Rectangle rectangle3 = new Rectangle(backgroundRectangle.X, backgroundRectangle.Y, Dpi.Width(val4.Width), Dpi.Height(val4.Height));
				switch (eStyleBackgroundImage2)
				{
				case eStyleBackgroundImage.TopRight:
					rectangle3.X = backgroundRectangle.Right - rectangle3.Width;
					break;
				case eStyleBackgroundImage.BottomLeft:
					rectangle3.Y = backgroundRectangle.Bottom - rectangle3.Height;
					break;
				case eStyleBackgroundImage.BottomRight:
					rectangle3.Y = backgroundRectangle.Bottom - rectangle3.Height;
					rectangle3.X = backgroundRectangle.Right - rectangle3.Width;
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
						for (int j = backgroundRectangle.Y; j < backgroundRectangle.Bottom; j += Dpi.Height(val4.Height))
						{
							for (; i < backgroundRectangle.Right; i += Dpi.Width(val4.Width))
							{
								Rectangle rectangle = new Rectangle(i, j, Dpi.Width(val4.Width), Dpi.Height(val4.Height));
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
						e.Graphics.DrawImage(val4, new Rectangle(0, 0, Dpi.Width(val4.Width), Dpi.Height(val4.Height)), 0, 0, val4.Width, val4.Height, (GraphicsUnit)2, val2);
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
			if (ല)
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

	private static Rectangle വ(ElementStyle _0657, Rectangle ശ)
	{
		if (_0657.ݿ)
		{
			if (_0657.BorderRightWidth <= 1)
			{
				ശ.Width -= _0657.BorderRightWidth;
			}
			else
			{
				ശ.Width -= _0657.BorderRightWidth / 2;
			}
		}
		if (_0657.ށ)
		{
			if (_0657.BorderBottomWidth <= 1)
			{
				ശ.Height -= _0657.BorderBottomWidth;
			}
			else
			{
				ശ.Height -= _0657.BorderBottomWidth / 2;
			}
		}
		if (_0657.ݾ && _0657.BorderLeftWidth > 1)
		{
			ശ.X += _0657.BorderLeftWidth / 2;
			ശ.Width -= _0657.BorderLeftWidth / 2;
		}
		if (_0657.ހ && _0657.BorderTopWidth > 1)
		{
			ശ.Y += _0657.BorderTopWidth / 2;
			ശ.Height -= _0657.BorderTopWidth / 2;
		}
		return ശ;
	}

	private static Rectangle ഷ(ElementStyle _0657, Rectangle ശ)
	{
		ശ = GetBackgroundRectangle(_0657, ശ);
		ശ.X += _0657.PaddingLeft;
		ശ.Width -= _0657.PaddingLeft + _0657.PaddingRight;
		ശ.Y += _0657.PaddingTop;
		ശ.Height -= _0657.PaddingTop + _0657.PaddingBottom;
		return ശ;
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
				ᒇ ᒇ2 = സ(backgroundRectangle, style.CornerDiameter, eCornerArc.TopLeft);
				val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
				ᒇ2 = സ(backgroundRectangle, style.CornerDiameter, eCornerArc.TopRight);
				val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
				ᒇ2 = സ(backgroundRectangle, style.CornerDiameter, eCornerArc.BottomRight);
				val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
				ᒇ2 = സ(backgroundRectangle, style.CornerDiameter, eCornerArc.BottomLeft);
				val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
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
				ᒇ ᒇ3 = സ(backgroundRectangle, style.CornerDiameter, eCornerArc.TopLeft);
				val.AddArc(ᒇ3.ᒈ, ᒇ3.ᒉ, ᒇ3.ܐ, ᒇ3._0711, ᒇ3.ᒊ, ᒇ3.ᒋ);
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
				ᒇ ᒇ4 = സ(backgroundRectangle, style.CornerDiameter, eCornerArc.TopRight);
				val.AddArc(ᒇ4.ᒈ, ᒇ4.ᒉ, ᒇ4.ܐ, ᒇ4._0711, ᒇ4.ᒊ, ᒇ4.ᒋ);
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
				ᒇ ᒇ5 = സ(backgroundRectangle, style.CornerDiameter, eCornerArc.BottomRight);
				val.AddArc(ᒇ5.ᒈ, ᒇ5.ᒉ, ᒇ5.ܐ, ᒇ5._0711, ᒇ5.ᒊ, ᒇ5.ᒋ);
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
				ᒇ ᒇ6 = സ(backgroundRectangle, style.CornerDiameter, eCornerArc.BottomLeft);
				val.AddArc(ᒇ6.ᒈ, ᒇ6.ᒉ, ᒇ6.ܐ, ᒇ6._0711, ᒇ6.ᒊ, ᒇ6.ᒋ);
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
			ᒇ ᒇ2 = സ(bounds, cornerDiameter, corner);
			path.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
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

	internal static ᒇ സ(Rectangle ശ, int ഹ, eCornerArc ഺ)
	{
		if (ഹ == 0)
		{
			ഹ = 1;
		}
		int num = ഹ * 2;
		return ഺ switch
		{
			eCornerArc.TopLeft => new ᒇ(ശ.X, ശ.Y, num, num, 180f, 90f), 
			eCornerArc.TopRight => new ᒇ(ശ.Right - num, ശ.Y, num, num, 270f, 90f), 
			eCornerArc.BottomLeft => new ᒇ(ശ.X, ശ.Bottom - num, num, num, 90f, 90f), 
			_ => new ᒇ(ശ.Right - num, ശ.Bottom - num, num, num, 0f, 90f), 
		};
	}
}
