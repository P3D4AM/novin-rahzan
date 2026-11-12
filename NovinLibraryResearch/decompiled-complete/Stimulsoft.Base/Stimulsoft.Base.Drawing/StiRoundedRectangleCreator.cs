using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Base.Drawing;

public static class StiRoundedRectangleCreator
{
	public static GraphicsPath Create(RectangleF rect, StiCornerRadius radius, float scale)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		scale = (float)((double)scale * StiScale.Factor);
		GraphicsPath val = new GraphicsPath();
		float rad = Math.Min(rect.Width / 2f, rect.Height / 2f);
		float radiusTopLeft = GetRadiusTopLeft(radius, scale, rad);
		float radiusTopRight = GetRadiusTopRight(radius, scale, rad);
		float radiusBottomLeft = GetRadiusBottomLeft(radius, scale, rad);
		float radiusBottomRight = GetRadiusBottomRight(radius, scale, rad);
		PointF pointF;
		if (radiusTopLeft != 0f)
		{
			RectangleF rectangleF = new RectangleF(rect.X, rect.Y, 2f * radiusTopLeft, 2f * radiusTopLeft);
			val.AddArc(rectangleF, 180f, 90f);
			pointF = new PointF(rect.X + radiusTopLeft, rect.Y);
		}
		else
		{
			pointF = new PointF(rect.X, rect.Y);
		}
		PointF pointF2 = ((radiusTopRight == 0f) ? new PointF(rect.Right, rect.Y) : new PointF(rect.Right - radiusTopRight, rect.Y));
		val.AddLine(pointF, pointF2);
		if (radiusTopRight != 0f)
		{
			RectangleF rectangleF2 = new RectangleF(rect.Right - 2f * radiusTopRight, rect.Y, 2f * radiusTopRight, 2f * radiusTopRight);
			val.AddArc(rectangleF2, 270f, 90f);
			pointF = new PointF(rect.Right, rect.Y + radiusTopRight);
		}
		else
		{
			pointF = new PointF(rect.Right, rect.Y);
		}
		pointF2 = ((radiusBottomRight == 0f) ? new PointF(rect.Right, rect.Bottom) : new PointF(rect.Right, rect.Bottom - radiusBottomRight));
		val.AddLine(pointF, pointF2);
		if (radiusBottomRight != 0f)
		{
			RectangleF rectangleF3 = new RectangleF(rect.Right - 2f * radiusBottomRight, rect.Bottom - 2f * radiusBottomRight, 2f * radiusBottomRight, 2f * radiusBottomRight);
			val.AddArc(rectangleF3, 0f, 90f);
			pointF = new PointF(rect.Right - radiusBottomRight, rect.Bottom);
		}
		else
		{
			pointF = new PointF(rect.Right, rect.Bottom);
		}
		pointF2 = ((radiusBottomLeft == 0f) ? new PointF(rect.X, rect.Bottom) : new PointF(rect.X + radiusBottomLeft, rect.Bottom));
		val.AddLine(pointF, pointF2);
		if (radiusBottomLeft != 0f)
		{
			RectangleF rectangleF4 = new RectangleF(rect.X, rect.Bottom - 2f * radiusBottomLeft, 2f * radiusBottomLeft, 2f * radiusBottomLeft);
			val.AddArc(rectangleF4, 90f, 90f);
			pointF = new PointF(rect.X, rect.Bottom - radiusBottomLeft);
		}
		else
		{
			pointF = new PointF(rect.X, rect.Bottom);
		}
		pointF2 = ((radiusTopLeft == 0f) ? new PointF(rect.X, rect.Y) : new PointF(rect.X, rect.Y + radiusTopLeft));
		val.AddLine(pointF, pointF2);
		val.CloseFigure();
		return val;
	}

	public static void DrawRoundedRect(Graphics g, Pen pen, RectangleF rect, StiCornerRadius radius, StiBorderSides sides, float scale)
	{
		scale = (float)((double)scale * StiScale.Factor);
		rect.Inflate((0f - pen.Width) / 2f, (0f - pen.Width) / 2f);
		bool flag = (sides & StiBorderSides.Top) > StiBorderSides.None;
		bool flag2 = (sides & StiBorderSides.Right) > StiBorderSides.None;
		bool flag3 = (sides & StiBorderSides.Bottom) > StiBorderSides.None;
		bool flag4 = (sides & StiBorderSides.Left) > StiBorderSides.None;
		float rad = Math.Min(rect.Width / 2f, rect.Height / 2f);
		float radiusTopLeft = GetRadiusTopLeft(radius, scale, rad);
		float radiusTopRight = GetRadiusTopRight(radius, scale, rad);
		float radiusBottomLeft = GetRadiusBottomLeft(radius, scale, rad);
		float radiusBottomRight = GetRadiusBottomRight(radius, scale, rad);
		if (radiusTopLeft > 0f && flag4 && flag)
		{
			RectangleF rectangleF = new RectangleF(rect.X, rect.Y, 2f * radiusTopLeft, 2f * radiusTopLeft);
			g.DrawArc(pen, rectangleF, 180f, 90f);
		}
		if (flag)
		{
			PointF location = rect.Location;
			PointF pointF = new PointF(rect.Right, rect.Y);
			if (radiusTopLeft > 0f)
			{
				location.X += radiusTopLeft;
			}
			if (radiusTopRight > 0f)
			{
				pointF.X -= radiusTopRight;
			}
			g.DrawLine(pen, location, pointF);
		}
		if (radiusTopRight > 0f && flag && flag2)
		{
			RectangleF rectangleF2 = new RectangleF(rect.Right - 2f * radiusTopRight, rect.Y, 2f * radiusTopRight, 2f * radiusTopRight);
			g.DrawArc(pen, rectangleF2, 270f, 90f);
		}
		if (flag2)
		{
			PointF pointF2 = new PointF(rect.Right, rect.Y);
			PointF pointF3 = new PointF(rect.Right, rect.Bottom);
			if (radiusTopRight > 0f)
			{
				pointF2.Y += radiusTopRight;
			}
			if (radiusBottomRight > 0f)
			{
				pointF3.Y -= radiusBottomRight;
			}
			g.DrawLine(pen, pointF2, pointF3);
		}
		if (radiusBottomRight > 0f && flag2 && flag3)
		{
			RectangleF rectangleF3 = new RectangleF(rect.Right - 2f * radiusBottomRight, rect.Bottom - 2f * radiusBottomRight, 2f * radiusBottomRight, 2f * radiusBottomRight);
			g.DrawArc(pen, rectangleF3, 0f, 90f);
		}
		if (flag3)
		{
			PointF pointF4 = new PointF(rect.Right, rect.Bottom);
			PointF pointF5 = new PointF(rect.X, rect.Bottom);
			if (radiusBottomRight > 0f)
			{
				pointF4.X -= radiusBottomRight;
			}
			if (radiusBottomLeft > 0f)
			{
				pointF5.X += radiusBottomLeft;
			}
			g.DrawLine(pen, pointF4, pointF5);
		}
		if (radiusBottomLeft > 0f && flag3 && flag4)
		{
			RectangleF rectangleF4 = new RectangleF(rect.X, rect.Bottom - 2f * radiusBottomLeft, 2f * radiusBottomLeft, 2f * radiusBottomLeft);
			g.DrawArc(pen, rectangleF4, 90f, 90f);
		}
		if (flag4)
		{
			PointF pointF6 = new PointF(rect.X, rect.Y);
			PointF pointF7 = new PointF(rect.X, rect.Bottom);
			if (radiusTopLeft > 0f)
			{
				pointF6.Y += radiusTopLeft;
			}
			if (radiusBottomLeft > 0f)
			{
				pointF7.Y -= radiusBottomLeft;
			}
			g.DrawLine(pen, pointF6, pointF7);
		}
	}

	private static float GetRadiusTopLeft(StiCornerRadius radius, float scale, float rad)
	{
		float val = ((radius != null) ? (radius.TopLeft * scale) : 0f);
		val = Math.Max(val, 0f);
		return Math.Min(val, rad);
	}

	private static float GetRadiusTopRight(StiCornerRadius radius, float scale, float rad)
	{
		float val = ((radius != null) ? (radius.TopRight * scale) : 0f);
		val = Math.Max(val, 0f);
		return Math.Min(val, rad);
	}

	private static float GetRadiusBottomLeft(StiCornerRadius radius, float scale, float rad)
	{
		float val = ((radius != null) ? (radius.BottomLeft * scale) : 0f);
		val = Math.Max(val, 0f);
		return Math.Min(val, rad);
	}

	private static float GetRadiusBottomRight(StiCornerRadius radius, float scale, float rad)
	{
		float val = ((radius != null) ? (radius.BottomRight * scale) : 0f);
		val = Math.Max(val, 0f);
		return Math.Min(val, rad);
	}
}
