using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class DisplayHelp
{
	private static TextRenderingHint ങ = (TextRenderingHint)5;

	public static ControlStyles DoubleBufferFlag => (ControlStyles)131072;

	public static TextRenderingHint AntiAliasTextRenderingHint
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return ങ;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			ങ = value;
		}
	}

	private DisplayHelp()
	{
	}

	public static Brush CreateBrush(RectangleF bounds, Color[] backColors, int backColorsGradientAngle, float[] backColorsPositions)
	{
		return CreateBrush(Rectangle.Round(bounds), backColors, backColorsGradientAngle, backColorsPositions);
	}

	public static Brush CreateBrush(Rectangle bounds, Color[] backColors, int backColorsGradientAngle, float[] backColorsPositions)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		if (backColors.Length == 0)
		{
			return (Brush)new SolidBrush(Color.White);
		}
		if (backColors.Length == 1)
		{
			return (Brush)new SolidBrush(backColors[0]);
		}
		LinearGradientBrush val = new LinearGradientBrush(bounds, backColors[0], backColors[^1], (float)backColorsGradientAngle);
		if (backColors.Length > 2)
		{
			ColorBlend val2 = new ColorBlend();
			val2.Colors = backColors;
			if (backColorsPositions.Length > 0)
			{
				val2.Positions = backColorsPositions;
			}
			else
			{
				float[] array = new float[backColors.Length];
				float num = 1 / backColors.Length;
				array[backColors.Length - 1] = 1f;
				for (int i = 1; i < backColors.Length - 1; i++)
				{
					array[i] = num * 1f;
				}
				val2.Positions = array;
			}
			val.InterpolationColors = val2;
		}
		return (Brush)(object)val;
	}

	public static LinearGradientBrush CreateLinearGradientBrush(Rectangle r, Color color1, Color color2, float gradientAngle)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		if (r.Width <= 0)
		{
			r.Width = 1;
		}
		if (r.Height <= 0)
		{
			r.Height = 1;
		}
		return new LinearGradientBrush(new Rectangle(r.X, r.Y - 1, r.Width, r.Height + 1), color1, color2, gradientAngle);
	}

	public static LinearGradientBrush CreateLinearGradientBrush(RectangleF r, Color color1, Color color2, float gradientAngle)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		if (r.Width <= 0f)
		{
			r.Width = 1f;
		}
		if (r.Height <= 0f)
		{
			r.Height = 1f;
		}
		return new LinearGradientBrush(new RectangleF(r.X, r.Y - 1f, r.Width, r.Height + 1f), color1, color2, gradientAngle);
	}

	public static LinearGradientBrush CreateLinearGradientBrush(Rectangle r, Color color1, Color color2, float gradientAngle, bool isAngleScalable)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		if (r.Width <= 0)
		{
			r.Width = 1;
		}
		if (r.Height <= 0)
		{
			r.Height = 1;
		}
		return new LinearGradientBrush(new Rectangle(r.X, r.Y - 1, r.Width, r.Height + 1), color1, color2, gradientAngle, isAngleScalable);
	}

	public static Rectangle GetDrawRectangle(Rectangle r)
	{
		r.Width--;
		r.Height--;
		return r;
	}

	public static Rectangle GetPathRectangle(Rectangle r)
	{
		return r;
	}

	public static void DrawRectangle(Graphics g, Color color, int x, int y, int width, int height)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Pen val = new Pen(color, 1f);
		try
		{
			DrawRectangle(g, val, x, y, width, height);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawRectangle(Graphics g, Color color, Rectangle r)
	{
		DrawRectangle(g, color, r.X, r.Y, r.Width, r.Height);
	}

	public static void DrawRectangle(Graphics g, Pen pen, int x, int y, int width, int height)
	{
		width--;
		height--;
		g.DrawRectangle(pen, x, y, width, height);
	}

	public static void DrawRectangle(Graphics g, Pen pen, Rectangle r)
	{
		DrawRectangle(g, pen, r.X, r.Y, r.Width, r.Height);
	}

	private static Pen ച(Color _065B, Color ഛ, Pen ജ)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (ജ == null || _065B != ഛ)
		{
			if (ജ != null)
			{
				ജ.Dispose();
			}
			return new Pen(_065B);
		}
		return ജ;
	}

	public static void DrawRoundedRectangle(Graphics g, Rectangle r, Color[] borderColors, int cornerRadius)
	{
		if (cornerRadius <= 0)
		{
			if (borderColors.Length < 4)
			{
				Pen val = null;
				Color empty = Color.Empty;
				r.Width--;
				r.Height--;
				for (int i = 0; i < borderColors.Length; i++)
				{
					if (!borderColors[i].IsEmpty)
					{
						val = ച(borderColors[i], empty, val);
						g.DrawRectangle(val, r);
						r.Inflate(-1, -1);
					}
				}
				if (val != null)
				{
					val.Dispose();
					val = null;
				}
				return;
			}
			Pen val2 = null;
			Color ഛ = Color.Empty;
			for (int j = 0; j < borderColors.Length; j += 4)
			{
				Color color = borderColors[j];
				Color color2 = borderColors[j + 1];
				Color color3 = borderColors[j + 2];
				Color color4 = borderColors[j + 3];
				if (!color.IsEmpty)
				{
					val2 = ച(color, ഛ, val2);
					g.DrawLine(val2, r.X, r.Y, r.X, r.Bottom);
					ഛ = color;
				}
				if (!color2.IsEmpty)
				{
					val2 = ച(color2, ഛ, val2);
					g.DrawLine(val2, r.X, r.Y, r.Right, r.Y);
					ഛ = color2;
				}
				if (!color3.IsEmpty)
				{
					val2 = ച(color3, ഛ, val2);
					g.DrawLine(val2, r.Right - 1, r.Y, r.Right - 1, r.Bottom);
					ഛ = color3;
				}
				if (!color4.IsEmpty)
				{
					val2 = ച(color4, ഛ, val2);
					g.DrawLine(val2, r.X, r.Bottom - 1, r.Right, r.Bottom - 1);
					ഛ = color4;
				}
			}
			if (val2 != null)
			{
				val2.Dispose();
				val2 = null;
			}
		}
		else
		{
			for (int k = 0; k < borderColors.Length; k++)
			{
				DrawRoundedRectangle(g, borderColors[k], r, cornerRadius);
				r.Inflate(-1, -1);
			}
		}
	}

	public static void DrawRoundedRectangle(Graphics g, Color color, LinearGradientColorTable fillColor, Rectangle bounds, int cornerSize)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		Brush val = CreateBrush(bounds, fillColor);
		try
		{
			Pen val2 = new Pen(color);
			try
			{
				DrawRoundedRectangle(g, val2, val, bounds.X, bounds.Y, bounds.Width, bounds.Height, cornerSize);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawRoundedRectangle(Graphics g, Color color, Color fillColor, Rectangle bounds, int cornerSize)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		Brush val = (Brush)new SolidBrush(fillColor);
		try
		{
			Pen val2 = new Pen(color);
			try
			{
				DrawRoundedRectangle(g, val2, val, bounds.X, bounds.Y, bounds.Width, bounds.Height, cornerSize);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawRoundedRectangle(Graphics g, Color color, Rectangle bounds, int cornerSize)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		if (!color.IsEmpty)
		{
			Pen val = new Pen(color);
			try
			{
				DrawRoundedRectangle(g, val, bounds.X, bounds.Y, bounds.Width, bounds.Height, cornerSize);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public static void DrawRoundedRectangle(Graphics g, Color color, int x, int y, int width, int height, int cornerSize)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		if (!color.IsEmpty)
		{
			Pen val = new Pen(color);
			try
			{
				DrawRoundedRectangle(g, val, x, y, width, height, cornerSize);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public static void DrawRoundedRectangle(Graphics g, Pen pen, Rectangle bounds, int cornerSize)
	{
		DrawRoundedRectangle(g, pen, bounds.X, bounds.Y, bounds.Width, bounds.Height, cornerSize);
	}

	public static void DrawRoundedRectangle(Graphics g, Pen pen, int x, int y, int width, int height, int cornerSize)
	{
		DrawRoundedRectangle(g, pen, null, x, y, width, height, cornerSize);
	}

	public static void DrawRoundedRectangle(Graphics g, Pen pen, Brush fill, int x, int y, int width, int height, int cornerSize)
	{
		width--;
		height--;
		Rectangle r = new Rectangle(x, y, width, height);
		GraphicsPath roundedRectanglePath = GetRoundedRectanglePath(r, cornerSize);
		try
		{
			if (fill != null)
			{
				g.FillPath(fill, roundedRectanglePath);
			}
			g.DrawPath(pen, roundedRectanglePath);
		}
		finally
		{
			((IDisposable)roundedRectanglePath)?.Dispose();
		}
	}

	public static GraphicsPath GetRoundedRectanglePath(Rectangle r, int cornerSize)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		if (cornerSize == 0)
		{
			val.AddRectangle(r);
		}
		else
		{
			ElementStyleDisplay.AddCornerArc(val, r, cornerSize, eCornerArc.TopLeft);
			ElementStyleDisplay.AddCornerArc(val, r, cornerSize, eCornerArc.TopRight);
			ElementStyleDisplay.AddCornerArc(val, r, cornerSize, eCornerArc.BottomRight);
			ElementStyleDisplay.AddCornerArc(val, r, cornerSize, eCornerArc.BottomLeft);
			val.CloseAllFigures();
		}
		return val;
	}

	public static GraphicsPath GetRoundedRectanglePath(Rectangle r, int cornerTopLeft, int cornerTopRight, int cornerBottomRight, int cornerBottomLeft)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		ElementStyleDisplay.AddCornerArc(val, r, cornerTopLeft, eCornerArc.TopLeft);
		ElementStyleDisplay.AddCornerArc(val, r, cornerTopRight, eCornerArc.TopRight);
		ElementStyleDisplay.AddCornerArc(val, r, cornerBottomRight, eCornerArc.BottomRight);
		ElementStyleDisplay.AddCornerArc(val, r, cornerBottomLeft, eCornerArc.BottomLeft);
		val.CloseAllFigures();
		return val;
	}

	public static GraphicsPath GetRoundedRectanglePath(Rectangle clientRectangle, int cornerSize, eStyleBackgroundPathPart pathPart, eCornerType topLeftCornerType, eCornerType topRightCornerType, eCornerType bottomLeftCornerType, eCornerType bottomRightCornerType)
	{
		return GetRoundedRectanglePath(clientRectangle, cornerSize, pathPart, topLeftCornerType, topRightCornerType, bottomLeftCornerType, bottomRightCornerType, 0f);
	}

	public static GraphicsPath GetRoundedRectanglePath(Rectangle clientRectangle, int cornerSize, eStyleBackgroundPathPart pathPart, eCornerType topLeftCornerType, eCornerType topRightCornerType, eCornerType bottomLeftCornerType, eCornerType bottomRightCornerType, float partSize)
	{
		return GetRoundedRectanglePath(clientRectangle, cornerSize, cornerSize, cornerSize, cornerSize, pathPart, topLeftCornerType, topRightCornerType, bottomLeftCornerType, bottomRightCornerType, partSize);
	}

	public static GraphicsPath GetRoundedRectanglePath(Rectangle clientRectangle, int topLeftCornerSize, int topRightCornerSize, int bottomLeftCornerSize, int bottomRightCornerSize, eStyleBackgroundPathPart pathPart, eCornerType topLeftCornerType, eCornerType topRightCornerType, eCornerType bottomLeftCornerType, eCornerType bottomRightCornerType, float partSize)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		switch (pathPart)
		{
		case eStyleBackgroundPathPart.TopHalf:
			if (partSize == 0f)
			{
				clientRectangle.Height /= 2;
			}
			else
			{
				clientRectangle.Height = (int)((float)clientRectangle.Height * partSize);
			}
			break;
		case eStyleBackgroundPathPart.BottomHalf:
		{
			int height = clientRectangle.Height;
			if (partSize == 0f)
			{
				clientRectangle.Height /= 2;
			}
			else
			{
				clientRectangle.Height = (int)((float)clientRectangle.Height * partSize);
			}
			clientRectangle.Y += height - clientRectangle.Height - 1;
			break;
		}
		}
		eCornerType eCornerType2 = topLeftCornerType;
		if (eCornerType2 == eCornerType.Inherit)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (pathPart == eStyleBackgroundPathPart.BottomHalf)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (eCornerType2 == eCornerType.Rounded && topLeftCornerSize > 0)
		{
			ᒇ ᒇ2 = ElementStyleDisplay.സ(clientRectangle, topLeftCornerSize, eCornerArc.TopLeft);
			val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
		}
		else if (eCornerType2 == eCornerType.Diagonal)
		{
			val.AddLine(clientRectangle.X, clientRectangle.Y + topLeftCornerSize, clientRectangle.X + topLeftCornerSize, clientRectangle.Y);
		}
		else
		{
			val.AddLine(clientRectangle.X, clientRectangle.Y + 2, clientRectangle.X, clientRectangle.Y);
			val.AddLine(clientRectangle.X, clientRectangle.Y, clientRectangle.X + 2, clientRectangle.Y);
		}
		eCornerType2 = topRightCornerType;
		if (eCornerType2 == eCornerType.Inherit)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (pathPart == eStyleBackgroundPathPart.BottomHalf)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (eCornerType2 == eCornerType.Rounded && topRightCornerSize > 0)
		{
			ᒇ ᒇ3 = ElementStyleDisplay.സ(clientRectangle, topRightCornerSize, eCornerArc.TopRight);
			val.AddArc(ᒇ3.ᒈ, ᒇ3.ᒉ, ᒇ3.ܐ, ᒇ3._0711, ᒇ3.ᒊ, ᒇ3.ᒋ);
		}
		else if (eCornerType2 == eCornerType.Diagonal)
		{
			val.AddLine(clientRectangle.Right - topRightCornerSize - 1, clientRectangle.Y, clientRectangle.Right, clientRectangle.Y + topRightCornerSize);
		}
		else
		{
			val.AddLine(clientRectangle.Right - 2, clientRectangle.Y, clientRectangle.Right, clientRectangle.Y);
			val.AddLine(clientRectangle.Right, clientRectangle.Y, clientRectangle.Right, clientRectangle.Y + 2);
		}
		eCornerType2 = bottomRightCornerType;
		if (eCornerType2 == eCornerType.Inherit)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (pathPart == eStyleBackgroundPathPart.TopHalf)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (eCornerType2 == eCornerType.Rounded && bottomRightCornerSize > 0)
		{
			ᒇ ᒇ4 = ElementStyleDisplay.സ(clientRectangle, bottomRightCornerSize, eCornerArc.BottomRight);
			val.AddArc(ᒇ4.ᒈ, ᒇ4.ᒉ, ᒇ4.ܐ, ᒇ4._0711, ᒇ4.ᒊ, ᒇ4.ᒋ);
		}
		else if (eCornerType2 == eCornerType.Diagonal)
		{
			val.AddLine(clientRectangle.Right, clientRectangle.Bottom - bottomRightCornerSize - 1, clientRectangle.Right - bottomRightCornerSize - 1, clientRectangle.Bottom);
		}
		else
		{
			val.AddLine(clientRectangle.Right, clientRectangle.Bottom - 2, clientRectangle.Right, clientRectangle.Bottom);
			val.AddLine(clientRectangle.Right, clientRectangle.Bottom, clientRectangle.Right - 2, clientRectangle.Bottom);
		}
		eCornerType2 = bottomLeftCornerType;
		if (eCornerType2 == eCornerType.Inherit)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (pathPart == eStyleBackgroundPathPart.TopHalf)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (eCornerType2 == eCornerType.Rounded && bottomLeftCornerSize > 0)
		{
			ᒇ ᒇ5 = ElementStyleDisplay.സ(clientRectangle, bottomLeftCornerSize, eCornerArc.BottomLeft);
			val.AddArc(ᒇ5.ᒈ, ᒇ5.ᒉ, ᒇ5.ܐ, ᒇ5._0711, ᒇ5.ᒊ, ᒇ5.ᒋ);
		}
		else if (eCornerType2 == eCornerType.Diagonal)
		{
			val.AddLine(clientRectangle.X + 2, clientRectangle.Bottom, clientRectangle.X, clientRectangle.Bottom - bottomLeftCornerSize - 1);
		}
		else
		{
			val.AddLine(clientRectangle.X + 2, clientRectangle.Bottom, clientRectangle.X, clientRectangle.Bottom);
			val.AddLine(clientRectangle.X, clientRectangle.Bottom, clientRectangle.X, clientRectangle.Bottom - 2);
		}
		val.CloseAllFigures();
		return val;
	}

	public static GraphicsPath GetBorderPath(Rectangle clientRectangle, int cornerSize, eStyleBackgroundPathPart pathPart, eCornerType topLeftCornerType, eCornerType topRightCornerType, eCornerType bottomLeftCornerType, eCornerType bottomRightCornerType, bool leftBorder, bool rightBorder, bool topBorder, bool bottomBorder)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		switch (pathPart)
		{
		case eStyleBackgroundPathPart.TopHalf:
			clientRectangle.Height /= 2;
			break;
		case eStyleBackgroundPathPart.BottomHalf:
		{
			int height = clientRectangle.Height;
			clientRectangle.Height /= 2;
			clientRectangle.Y += height - clientRectangle.Height - 1;
			break;
		}
		}
		eCornerType eCornerType2 = topLeftCornerType;
		if (eCornerType2 == eCornerType.Inherit)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (pathPart == eStyleBackgroundPathPart.BottomHalf)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (leftBorder)
		{
			val.AddLine(clientRectangle.X, clientRectangle.Bottom - ((bottomBorder && (bottomLeftCornerType == eCornerType.Diagonal || bottomLeftCornerType == eCornerType.Rounded)) ? cornerSize : 0), clientRectangle.X, clientRectangle.Y + ((topBorder && (topLeftCornerType == eCornerType.Diagonal || topLeftCornerType == eCornerType.Rounded)) ? cornerSize : 0));
		}
		if (leftBorder && topBorder)
		{
			switch (eCornerType2)
			{
			case eCornerType.Rounded:
			{
				ᒇ ᒇ2 = ElementStyleDisplay.സ(clientRectangle, cornerSize, eCornerArc.TopLeft);
				val.AddArc(ᒇ2.ᒈ, ᒇ2.ᒉ, ᒇ2.ܐ, ᒇ2._0711, ᒇ2.ᒊ, ᒇ2.ᒋ);
				break;
			}
			case eCornerType.Diagonal:
				val.AddLine(clientRectangle.X, clientRectangle.Y + cornerSize, clientRectangle.X + cornerSize, clientRectangle.Y);
				break;
			}
		}
		if (topBorder)
		{
			val.AddLine(clientRectangle.X + ((topLeftCornerType == eCornerType.Diagonal || topLeftCornerType == eCornerType.Rounded) ? cornerSize : 0), clientRectangle.Y, clientRectangle.Right - ((rightBorder && (topRightCornerType == eCornerType.Diagonal || topRightCornerType == eCornerType.Rounded)) ? cornerSize : 0), clientRectangle.Y);
		}
		eCornerType2 = topRightCornerType;
		if (eCornerType2 == eCornerType.Inherit)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (pathPart == eStyleBackgroundPathPart.BottomHalf)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (topBorder && rightBorder)
		{
			switch (eCornerType2)
			{
			case eCornerType.Rounded:
			{
				ᒇ ᒇ3 = ElementStyleDisplay.സ(clientRectangle, cornerSize, eCornerArc.TopRight);
				val.AddArc(ᒇ3.ᒈ, ᒇ3.ᒉ, ᒇ3.ܐ, ᒇ3._0711, ᒇ3.ᒊ, ᒇ3.ᒋ);
				break;
			}
			case eCornerType.Diagonal:
				val.AddLine(clientRectangle.Right - cornerSize - 1, clientRectangle.Y, clientRectangle.Right, clientRectangle.Y + cornerSize);
				break;
			}
		}
		if (rightBorder)
		{
			val.AddLine(clientRectangle.Right, clientRectangle.Y + ((topRightCornerType == eCornerType.Diagonal || topRightCornerType == eCornerType.Rounded) ? cornerSize : 0), clientRectangle.Right, clientRectangle.Bottom - ((bottomBorder && (bottomRightCornerType == eCornerType.Diagonal || bottomRightCornerType == eCornerType.Rounded)) ? cornerSize : 0));
		}
		eCornerType2 = bottomRightCornerType;
		if (eCornerType2 == eCornerType.Inherit)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (pathPart == eStyleBackgroundPathPart.TopHalf)
		{
			eCornerType2 = eCornerType.Square;
		}
		switch (eCornerType2)
		{
		case eCornerType.Rounded:
		{
			ᒇ ᒇ4 = ElementStyleDisplay.സ(clientRectangle, cornerSize, eCornerArc.BottomRight);
			val.AddArc(ᒇ4.ᒈ, ᒇ4.ᒉ, ᒇ4.ܐ, ᒇ4._0711, ᒇ4.ᒊ, ᒇ4.ᒋ);
			break;
		}
		case eCornerType.Diagonal:
			val.AddLine(clientRectangle.Right, clientRectangle.Bottom - cornerSize - 1, clientRectangle.Right - cornerSize - 1, clientRectangle.Bottom);
			break;
		}
		if (bottomBorder)
		{
			val.AddLine(clientRectangle.Right - ((bottomRightCornerType == eCornerType.Diagonal || bottomRightCornerType == eCornerType.Rounded) ? cornerSize : 0), clientRectangle.Bottom, clientRectangle.X + ((bottomLeftCornerType == eCornerType.Diagonal || bottomLeftCornerType == eCornerType.Rounded) ? cornerSize : 0), clientRectangle.Bottom);
		}
		eCornerType2 = bottomLeftCornerType;
		if (eCornerType2 == eCornerType.Inherit)
		{
			eCornerType2 = eCornerType.Square;
		}
		if (pathPart == eStyleBackgroundPathPart.TopHalf)
		{
			eCornerType2 = eCornerType.Square;
		}
		switch (eCornerType2)
		{
		case eCornerType.Rounded:
		{
			ᒇ ᒇ5 = ElementStyleDisplay.സ(clientRectangle, cornerSize, eCornerArc.BottomLeft);
			val.AddArc(ᒇ5.ᒈ, ᒇ5.ᒉ, ᒇ5.ܐ, ᒇ5._0711, ᒇ5.ᒊ, ᒇ5.ᒋ);
			break;
		}
		case eCornerType.Diagonal:
			val.AddLine(clientRectangle.X + 2, clientRectangle.Bottom, clientRectangle.X, clientRectangle.Bottom - cornerSize - 1);
			break;
		}
		return val;
	}

	public static void FillRoundedRectangle(Graphics g, Rectangle bounds, int cornerSize, Color color1, Color color2, int gradientAngle)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (color2.IsEmpty)
		{
			if (!color1.IsEmpty)
			{
				SolidBrush val = new SolidBrush(color1);
				try
				{
					FillRoundedRectangle(g, (Brush)(object)val, bounds, cornerSize);
					return;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			return;
		}
		LinearGradientBrush val2 = CreateLinearGradientBrush(bounds, color1, color2, gradientAngle);
		try
		{
			FillRoundedRectangle(g, (Brush)(object)val2, bounds, cornerSize);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static void FillRoundedRectangle(Graphics g, Rectangle bounds, int cornerSize, Color color1, Color color2)
	{
		FillRoundedRectangle(g, bounds, cornerSize, color1, color2, 90);
	}

	public static void FillRoundedRectangle(Graphics g, Rectangle bounds, int cornerSize, Color color1)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		SolidBrush val = new SolidBrush(color1);
		try
		{
			FillRoundedRectangle(g, (Brush)(object)val, bounds, cornerSize);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void FillRoundedRectangle(Graphics g, Brush brush, Rectangle bounds, int cornerSize)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (cornerSize <= 0)
		{
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)3;
			g.FillRectangle(brush, bounds);
			g.SmoothingMode = smoothingMode;
			return;
		}
		bounds.Width--;
		bounds.Height--;
		GraphicsPath roundedRectanglePath = GetRoundedRectanglePath(bounds, cornerSize);
		try
		{
			g.FillPath(brush, roundedRectanglePath);
		}
		finally
		{
			((IDisposable)roundedRectanglePath)?.Dispose();
		}
	}

	public static void FillRectangle(Graphics g, Rectangle bounds, Color color1)
	{
		FillRectangle(g, bounds, color1, Color.Empty, 90);
	}

	public static void FillRectangle(Graphics g, Rectangle bounds, Color color1, Color color2)
	{
		FillRectangle(g, bounds, color1, color2, 90);
	}

	public static void FillRectangle(Graphics g, Rectangle r, LinearGradientColorTable table)
	{
		FillRectangle(g, r, table.Start, table.End, table.GradientAngle);
	}

	public static void FillRectangle(Graphics g, Rectangle r, Color color1, Color color2, int gradientAngle)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (r.Width == 0 || r.Height == 0)
		{
			return;
		}
		if (color2.IsEmpty)
		{
			if (!color1.IsEmpty)
			{
				SmoothingMode smoothingMode = g.SmoothingMode;
				g.SmoothingMode = (SmoothingMode)3;
				SolidBrush val = new SolidBrush(color1);
				try
				{
					g.FillRectangle((Brush)(object)val, r);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				g.SmoothingMode = smoothingMode;
			}
			return;
		}
		LinearGradientBrush val2 = CreateLinearGradientBrush(r, color1, color2, gradientAngle);
		try
		{
			g.FillRectangle((Brush)(object)val2, r);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static void FillRectangle(Graphics g, Rectangle r, Color color1, Color color2, int gradientAngle, float[] factors, float[] positions)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (r.Width == 0 || r.Height == 0)
		{
			return;
		}
		if (color2.IsEmpty)
		{
			if (!color1.IsEmpty)
			{
				SmoothingMode smoothingMode = g.SmoothingMode;
				g.SmoothingMode = (SmoothingMode)3;
				SolidBrush val = new SolidBrush(color1);
				try
				{
					g.FillRectangle((Brush)(object)val, r);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				g.SmoothingMode = smoothingMode;
			}
			return;
		}
		LinearGradientBrush val2 = CreateLinearGradientBrush(r, color1, color2, gradientAngle);
		try
		{
			Blend val3 = new Blend(factors.Length);
			val3.Factors = factors;
			val3.Positions = positions;
			val2.Blend = val3;
			g.FillRectangle((Brush)(object)val2, r);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static void FillPath(Graphics g, GraphicsPath path, Color color1, Color color2)
	{
		FillPath(g, path, color1, color2, 90);
	}

	public static void FillPath(Graphics g, GraphicsPath path, LinearGradientColorTable table)
	{
		FillPath(g, path, table.Start, table.End, table.GradientAngle);
	}

	public static void FillPath(Graphics g, GraphicsPath path, Color color1, Color color2, int gradientAngle)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (color2.IsEmpty)
		{
			if (!color1.IsEmpty)
			{
				SolidBrush val = new SolidBrush(color1);
				try
				{
					g.FillPath((Brush)(object)val, path);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		else if (!color1.IsEmpty)
		{
			LinearGradientBrush val2 = CreateLinearGradientBrush(path.GetBounds(), color1, color2, gradientAngle);
			try
			{
				g.FillPath((Brush)(object)val2, path);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}

	public static void DrawLine(Graphics g, Point start, Point end, Color color, int penWidth)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (!color.IsEmpty)
		{
			Pen val = new Pen(color, (float)penWidth);
			try
			{
				g.DrawLine(val, start, end);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public static void DrawLine(Graphics g, int x1, int y1, int x2, int y2, Color color, int penWidth)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (!color.IsEmpty)
		{
			Pen val = new Pen(color, (float)penWidth);
			try
			{
				g.DrawLine(val, x1, y1, x2, y2);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public static void DrawGradientRectangle(Graphics g, Rectangle bounds, LinearGradientColorTable table, int penWidth)
	{
		DrawGradientRectangle(g, bounds, table.Start, table.End, table.GradientAngle, penWidth);
	}

	public static void DrawGradientRectangle(Graphics g, Rectangle bounds, Color color1, Color color2, int gradientAngle, int penWidth)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		if (color1.IsEmpty || bounds.Width <= 0 || bounds.Height <= 0 || penWidth <= 0)
		{
			return;
		}
		Rectangle rectangle = bounds;
		rectangle.Width--;
		rectangle.Height--;
		if (color2.IsEmpty)
		{
			Pen val = new Pen(color1, (float)penWidth);
			try
			{
				g.DrawRectangle(val, rectangle);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		GraphicsPath val2 = new GraphicsPath();
		try
		{
			val2.AddRectangle(rectangle);
			DrawGradientPath(g, val2, rectangle, color1, color2, gradientAngle, penWidth);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static void DrawGradientPath(Graphics g, GraphicsPath path, Rectangle bounds, LinearGradientColorTable table, int penWidth)
	{
		DrawGradientPath(g, path, bounds, table.Start, table.End, table.GradientAngle, penWidth);
	}

	public static void DrawGradientPath(Graphics g, GraphicsPath path, Rectangle bounds, Color color1, Color color2, int gradientAngle, int penWidth)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		Pen val = new Pen(color1, (float)penWidth);
		try
		{
			path.Widen(val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (color2.IsEmpty)
		{
			if (!color1.IsEmpty)
			{
				SolidBrush val2 = new SolidBrush(color1);
				try
				{
					g.FillPath((Brush)(object)val2, path);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		else if (!color1.IsEmpty)
		{
			LinearGradientBrush val3 = new LinearGradientBrush(bounds, color1, color2, (float)gradientAngle);
			try
			{
				g.FillPath((Brush)(object)val3, path);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
	}

	public static void DrawRoundGradientRectangle(Graphics g, Rectangle bounds, LinearGradientColorTable table, int penWidth, int roundCornerSize)
	{
		DrawRoundGradientRectangle(g, bounds, table.Start, table.End, table.GradientAngle, penWidth, roundCornerSize);
	}

	public static void DrawRoundGradientRectangle(Graphics g, Rectangle bounds, Color color1, Color color2, int gradientAngle, int penWidth, int roundCornerSize)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		if ((color1.IsEmpty && color2.IsEmpty) || bounds.Width <= 0 || bounds.Height <= 0 || roundCornerSize <= 0 || penWidth <= 0)
		{
			return;
		}
		if (color2.IsEmpty)
		{
			Pen val = new Pen(color1, (float)penWidth);
			try
			{
				DrawRoundedRectangle(g, val, bounds, roundCornerSize);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		Rectangle r = bounds;
		r.Width--;
		r.Height--;
		GraphicsPath roundedRectanglePath = GetRoundedRectanglePath(r, roundCornerSize);
		try
		{
			Pen val2 = new Pen(color1, (float)penWidth);
			try
			{
				roundedRectanglePath.Widen(val2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			LinearGradientBrush val3 = new LinearGradientBrush(bounds, color1, color2, (float)gradientAngle);
			try
			{
				g.FillPath((Brush)(object)val3, roundedRectanglePath);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)roundedRectanglePath)?.Dispose();
		}
	}

	public static void DrawGradientPathBorder(Graphics g, GraphicsPath path, LinearGradientColorTable table, int penWidth)
	{
		DrawGradientPathBorder(g, path, table.Start, table.End, table.GradientAngle, penWidth);
	}

	public static void DrawGradientPathBorder(Graphics g, GraphicsPath path, Color color1, Color color2, int gradientAngle, int penWidth)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		if (color1.IsEmpty && color2.IsEmpty)
		{
			return;
		}
		Pen val = new Pen(color1, (float)penWidth);
		try
		{
			path.Widen(val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Rectangle rectangle = Rectangle.Ceiling(path.GetBounds());
		rectangle.Inflate(1, 1);
		if (color2.IsEmpty)
		{
			if (!color1.IsEmpty)
			{
				SolidBrush val2 = new SolidBrush(color1);
				try
				{
					g.FillPath((Brush)(object)val2, path);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		else if (!color1.IsEmpty)
		{
			LinearGradientBrush val3 = new LinearGradientBrush(rectangle, color1, color2, (float)gradientAngle);
			try
			{
				g.FillPath((Brush)(object)val3, path);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
	}

	public static void DrawGradientLine(Graphics g, Point start, Point end, LinearGradientColorTable table, int penWidth)
	{
		DrawGradientLine(g, start, end, table.Start, table.End, table.GradientAngle, penWidth);
	}

	public static void DrawGradientLine(Graphics g, int x1, int y1, int x2, int y2, LinearGradientColorTable table, int penWidth)
	{
		DrawGradientLine(g, new Point(x1, y1), new Point(x2, y2), table.Start, table.End, table.GradientAngle, penWidth);
	}

	public static void DrawGradientLine(Graphics g, Point start, Point end, Color color1, Color color2, int gradientAngle, int penWidth)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		if (color2.IsEmpty || (penWidth == 1 && start.Y == end.Y && gradientAngle == 90))
		{
			if (!color1.IsEmpty)
			{
				Pen val = new Pen(color1, (float)penWidth);
				try
				{
					g.DrawLine(val, start, end);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		else
		{
			if (color1.IsEmpty)
			{
				return;
			}
			GraphicsPath val2 = new GraphicsPath();
			try
			{
				val2.AddLine(start, end);
				Pen val3 = new Pen(color1, (float)penWidth);
				try
				{
					val2.Widen(val3);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				Rectangle r = Rectangle.Ceiling(val2.GetBounds());
				r.Inflate(1, 1);
				SmoothingMode smoothingMode = g.SmoothingMode;
				g.SmoothingMode = (SmoothingMode)0;
				LinearGradientBrush val4 = CreateLinearGradientBrush(r, color1, color2, gradientAngle);
				try
				{
					g.FillPath((Brush)(object)val4, val2);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				g.SmoothingMode = smoothingMode;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}

	public static void DrawGradientLine(Graphics g, Point start, Point end, Color color1, Color color2, int gradientAngle, int penWidth, float[] factors, float[] positions)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		if (color2.IsEmpty)
		{
			if (!color1.IsEmpty)
			{
				Pen val = new Pen(color1, (float)penWidth);
				try
				{
					g.DrawLine(val, start, end);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		else
		{
			if (color1.IsEmpty)
			{
				return;
			}
			GraphicsPath val2 = new GraphicsPath();
			try
			{
				start.Offset(-1, -1);
				end.Offset(-1, -1);
				val2.AddLine(start, end);
				Pen val3 = new Pen(color1, (float)penWidth);
				try
				{
					val2.Widen(val3);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				Rectangle r = Rectangle.Ceiling(val2.GetBounds());
				r.Inflate(1, 1);
				LinearGradientBrush val4 = CreateLinearGradientBrush(r, color1, color2, gradientAngle);
				try
				{
					Blend val5 = new Blend(factors.Length);
					val5.Factors = factors;
					val5.Positions = positions;
					val4.Blend = val5;
					g.FillPath((Brush)(object)val4, val2);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}

	public static Brush CreateBrush(Rectangle bounds, GradientColorTable colorBlend)
	{
		return CreateBrush(bounds, colorBlend.Colors, colorBlend.LinearGradientAngle, colorBlend.GradientType);
	}

	public static Brush CreateBrush(Rectangle bounds, LinearGradientColorTable colorTable)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (!colorTable.End.IsEmpty)
		{
			return (Brush)new LinearGradientBrush(bounds, colorTable.Start, colorTable.End, (float)colorTable.GradientAngle);
		}
		return (Brush)new SolidBrush(colorTable.Start);
	}

	public static Brush CreateBrush(Rectangle bounds, BackgroundColorBlendCollection colorBlend, int gradientAngle, eGradientType gradientType)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		switch (colorBlend.ት())
		{
		case ቸ.ቹ:
			return null;
		case ቸ.ቼ:
			return (Brush)new SolidBrush(colorBlend[0].Color);
		case ቸ.ቺ:
			try
			{
				switch (gradientType)
				{
				case eGradientType.Linear:
				{
					bounds.Inflate(1, 1);
					LinearGradientBrush val3 = CreateLinearGradientBrush(bounds, colorBlend[0].Color, colorBlend[colorBlend.Count - 1].Color, gradientAngle);
					val3.InterpolationColors = colorBlend.GetColorBlend();
					return (Brush)(object)val3;
				}
				case eGradientType.Radial:
				{
					int num = (int)Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height) + 4;
					GraphicsPath val = new GraphicsPath();
					val.AddEllipse(bounds.X - (num - bounds.Width) / 2, bounds.Y - (num - bounds.Height) / 2, num, num);
					PathGradientBrush val2 = new PathGradientBrush(val);
					val2.CenterColor = colorBlend[0].Color;
					val2.SurroundColors = new Color[1] { colorBlend[colorBlend.Count - 1].Color };
					val2.InterpolationColors = colorBlend.GetColorBlend();
					return (Brush)(object)val2;
				}
				}
			}
			catch
			{
				return null;
			}
			break;
		default:
		{
			for (int i = 0; i < colorBlend.Count; i += 2)
			{
				BackgroundColorBlend backgroundColorBlend = colorBlend[i];
				BackgroundColorBlend backgroundColorBlend2 = null;
				if (i < colorBlend.Count)
				{
					backgroundColorBlend2 = colorBlend[i + 1];
				}
				if (backgroundColorBlend != null && backgroundColorBlend2 != null)
				{
					Rectangle r = new Rectangle(bounds.X, bounds.Y + (int)backgroundColorBlend.Position, bounds.Width, ((backgroundColorBlend2.Position == 1f) ? bounds.Height : ((int)backgroundColorBlend2.Position)) - (int)backgroundColorBlend.Position);
					return (Brush)(object)CreateLinearGradientBrush(r, backgroundColorBlend.Color, backgroundColorBlend2.Color, gradientAngle);
				}
			}
			break;
		}
		}
		return null;
	}

	public static Rectangle[] ExcludeRectangle(Rectangle r1, Rectangle exclude)
	{
		if (r1.X >= exclude.X && exclude.Right < r1.Right)
		{
			return new Rectangle[1]
			{
				new Rectangle(exclude.Right, r1.Y, r1.Width - (exclude.Right - r1.X), r1.Height)
			};
		}
		if (r1.Right <= exclude.Right && exclude.X > r1.X)
		{
			return new Rectangle[1]
			{
				new Rectangle(r1.X, r1.Y, r1.Width - (r1.Right - exclude.X), r1.Height)
			};
		}
		if (exclude.X > r1.X && exclude.Right < r1.Right)
		{
			return new Rectangle[2]
			{
				new Rectangle(r1.X, r1.Y, exclude.X - r1.X, r1.Height),
				new Rectangle(exclude.Right, r1.Y, r1.Right - exclude.Right, r1.Height)
			};
		}
		if (exclude.Bottom >= r1.Bottom && exclude.Y > r1.Y)
		{
			return new Rectangle[1]
			{
				new Rectangle(r1.X, r1.Y, r1.Width, Math.Max(0, exclude.Y - r1.Y))
			};
		}
		if (exclude.Y <= r1.Y && exclude.Bottom < r1.Bottom)
		{
			return new Rectangle[1]
			{
				new Rectangle(r1.X, exclude.Bottom, r1.Width, r1.Bottom - exclude.Bottom)
			};
		}
		return new Rectangle[0];
	}

	internal static Size ഝ(Size ഞ, Size ട)
	{
		if (ട.Width > ഞ.Width)
		{
			ഞ.Width = ട.Width;
		}
		if (ട.Height > ഞ.Height)
		{
			ഞ.Height = ട.Height;
		}
		return ഞ;
	}

	internal static void ഠ(ref Rectangle ഡ, Rectangle ഢ)
	{
		if (ഢ.X + ഢ.Width / 2 < ഡ.X + ഡ.Width / 2)
		{
			int num = ഢ.Right - ഡ.X;
			ഡ.X = ഢ.Right;
			ഡ.Width -= num;
		}
		else
		{
			ഡ.Width -= ഡ.Right - ഢ.X;
		}
	}
}
