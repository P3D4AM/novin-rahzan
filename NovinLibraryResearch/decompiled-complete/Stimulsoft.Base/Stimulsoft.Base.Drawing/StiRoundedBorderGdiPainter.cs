using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Base.Drawing;

public static class StiRoundedBorderGdiPainter
{
	public static bool AllowAntiAlias { get; set; } = true;


	public static void PaintBorder(Graphics g, RectangleF rect, StiSimpleBorder border, StiCornerRadius cornerRadius, StiButtonShapeType shapeType = StiButtonShapeType.Rectangle, float scale = 1f, bool isDesignTime = false)
	{
		switch (shapeType)
		{
		case StiButtonShapeType.Rectangle:
			PaintRoundedBorder(g, rect, border, cornerRadius, scale, isDesignTime);
			break;
		case StiButtonShapeType.Circle:
			PaintCircledBorder(g, rect, border, scale, isDesignTime);
			break;
		}
	}

	private static void PaintRoundedBorder(Graphics g, RectangleF rect, StiSimpleBorder border, StiCornerRadius cornerRadius, float scale, bool isDesignTime = false)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		float borderThickness = (float)(border.Size * (double)scale);
		if (border.Style == StiPenStyle.Double)
		{
			borderThickness = 1f;
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		if (AllowAntiAlias && !cornerRadius.IsEmpty)
		{
			g.SmoothingMode = (SmoothingMode)4;
		}
		try
		{
			if (border.Side == StiBorderSides.None && isDesignTime)
			{
				PaintRoundedBorderDesign(g, rect, cornerRadius, scale);
			}
			else if (border.Side == StiBorderSides.All)
			{
				PaintRoundedBorderAllSides(g, rect, border, cornerRadius, scale, borderThickness);
			}
			else
			{
				PaintRoundedBorderDefinedSides(g, rect, border, cornerRadius, scale, borderThickness, isDesignTime);
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	private static void PaintCircledBorder(Graphics g, RectangleF rect, StiSimpleBorder border, float scale, bool isDesignTime)
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if (border.Side != StiBorderSides.All && !isDesignTime)
		{
			return;
		}
		rect = FitRectangleInCircle(rect);
		float borderThickness = (float)(border.Size * (double)scale * StiScale.Factor);
		if (border.Style == StiPenStyle.Double)
		{
			borderThickness = 1f;
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		if (AllowAntiAlias)
		{
			g.SmoothingMode = (SmoothingMode)4;
		}
		try
		{
			Pen borderPen = GetBorderPen(border, borderThickness, isDesignTime);
			try
			{
				g.DrawEllipse(borderPen, rect);
				if (border.Style == StiPenStyle.Double && border.Side != StiBorderSides.All)
				{
					rect.Inflate(-StiScale.I2, -StiScale.I2);
					g.DrawEllipse(borderPen, rect);
				}
			}
			finally
			{
				((IDisposable)borderPen)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	private static void PaintRoundedBorderDefinedSides(Graphics g, RectangleF rect, StiSimpleBorder border, StiCornerRadius cornerRadius, float scale, float borderThickness, bool isDesignTime)
	{
		Pen borderPen = GetBorderPen(border, borderThickness, isDesignTime);
		try
		{
			StiRoundedRectangleCreator.DrawRoundedRect(g, borderPen, rect, cornerRadius, border.Side, scale);
			if (border.Style == StiPenStyle.Double)
			{
				rect.Inflate(-StiScale.I2, -StiScale.I2);
				StiRoundedRectangleCreator.DrawRoundedRect(g, borderPen, rect, cornerRadius, border.Side, scale);
			}
		}
		finally
		{
			((IDisposable)borderPen)?.Dispose();
		}
	}

	private static void PaintRoundedBorderAllSides(Graphics g, RectangleF rect, StiSimpleBorder border, StiCornerRadius cornerRadius, float scale, float borderThickness)
	{
		Pen borderPen = GetBorderPen(border, borderThickness);
		try
		{
			if (cornerRadius.IsEmpty)
			{
				g.DrawRectangle(borderPen, rect.X, rect.Y, rect.Width, rect.Height);
				if (border.Style == StiPenStyle.Double)
				{
					rect.Inflate(-StiScale.I2, -StiScale.I2);
					g.DrawRectangle(borderPen, rect.X, rect.Y, rect.Width, rect.Height);
				}
				return;
			}
			GraphicsPath val = StiRoundedRectangleCreator.Create(rect, cornerRadius, scale);
			try
			{
				g.DrawPath(borderPen, val);
				if (border.Style == StiPenStyle.Double)
				{
					rect.Inflate(-StiScale.I2, -StiScale.I2);
					GraphicsPath val2 = StiRoundedRectangleCreator.Create(rect, cornerRadius, scale);
					try
					{
						g.DrawPath(borderPen, val2);
						return;
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)borderPen)?.Dispose();
		}
	}

	private static void PaintRoundedBorderDesign(Graphics g, RectangleF rect, StiCornerRadius cornerRadius, float scale)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		Pen val = new Pen(Color.Gray, 1f);
		try
		{
			if (cornerRadius == null || cornerRadius.IsEmpty)
			{
				g.DrawRectangle(val, rect.X, rect.Y, rect.Width, rect.Height);
				return;
			}
			GraphicsPath val2 = StiRoundedRectangleCreator.Create(rect, cornerRadius, scale);
			try
			{
				g.DrawPath(val, val2);
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

	public static RectangleF FitRectangleInCircle(RectangleF rect)
	{
		if (rect.Width > rect.Height)
		{
			return new RectangleF(rect.X + (rect.Width - rect.Height) / 2f, rect.Y, rect.Height, rect.Height);
		}
		return new RectangleF(rect.X, rect.Y + (rect.Height - rect.Width) / 2f, rect.Width, rect.Width);
	}

	public static RectangleD FitRectangleInCircle(RectangleD rect)
	{
		if (rect.Width > rect.Height)
		{
			return new RectangleD(rect.X + (rect.Width - rect.Height) / 2.0, rect.Y, rect.Height, rect.Height);
		}
		return new RectangleD(rect.X, rect.Y + (rect.Height - rect.Width) / 2.0, rect.Width, rect.Width);
	}

	private static Pen GetBorderPen(StiSimpleBorder border, float borderThickness, bool isDesignTime = false)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		DashStyle dashStyle = StiPenUtils.GetPenStyle(border.Style);
		Color color = border.Color;
		if (border.Side == StiBorderSides.None && isDesignTime)
		{
			dashStyle = (DashStyle)1;
			color = Color.LightGray;
		}
		return new Pen(color, borderThickness)
		{
			DashStyle = dashStyle,
			Alignment = (PenAlignment)0,
			StartCap = (LineCap)1,
			EndCap = (LineCap)1
		};
	}
}
