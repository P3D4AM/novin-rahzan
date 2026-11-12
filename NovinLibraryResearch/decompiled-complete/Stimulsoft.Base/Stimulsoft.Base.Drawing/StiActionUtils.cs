using System;
using System.Drawing;

namespace Stimulsoft.Base.Drawing;

public sealed class StiActionUtils
{
	public static bool PointInEdge(double x, double y, PointD point, double size)
	{
		decimal num = Math.Round((decimal)point.X, 2);
		decimal num2 = Math.Round((decimal)point.Y, 2);
		decimal num3 = Math.Round((decimal)x, 2);
		decimal num4 = Math.Round((decimal)y, 2);
		decimal num5 = Math.Round((decimal)size, 2);
		if (num - num5 <= num3 && num2 - num5 <= num4 && num + num5 >= num3)
		{
			return num2 + num5 >= num4;
		}
		return false;
	}

	public static StiAction PointInRect(int size, int x, int y, Rectangle sizeRect, Rectangle selectRect, bool isSelected, bool locked)
	{
		return PointInRect(size, x, y, RectangleD.CreateFromRectangle(sizeRect), RectangleD.CreateFromRectangle(selectRect), isSelected, locked);
	}

	public static StiAction PointInRect(float size, float x, float y, RectangleF sizeRect, RectangleF selectRect, bool isSelected, bool locked)
	{
		return PointInRect(size, x, y, RectangleD.CreateFromRectangle(sizeRect), RectangleD.CreateFromRectangle(selectRect), isSelected, locked);
	}

	public static StiAction PointInRect(double size, double x, double y, RectangleD sizeRect, RectangleD selectRect, bool isSelected, bool locked)
	{
		if (isSelected && !locked)
		{
			if (PointInEdge(x, y, new PointD(sizeRect.X, sizeRect.Y), size))
			{
				return StiAction.SizeLeftTop;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X + sizeRect.Width, sizeRect.Y), size))
			{
				return StiAction.SizeRightTop;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X, sizeRect.Y + sizeRect.Height), size))
			{
				return StiAction.SizeLeftBottom;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X + sizeRect.Width, sizeRect.Y + sizeRect.Height), size))
			{
				return StiAction.SizeRightBottom;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X + sizeRect.Width / 2.0, sizeRect.Y), size))
			{
				return StiAction.SizeTop;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X + sizeRect.Width / 2.0, sizeRect.Y + sizeRect.Height), size))
			{
				return StiAction.SizeBottom;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X, sizeRect.Y + sizeRect.Height / 2.0), size))
			{
				return StiAction.SizeLeft;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X + sizeRect.Width, sizeRect.Y + sizeRect.Height / 2.0), size))
			{
				return StiAction.SizeRight;
			}
		}
		decimal num = Math.Round((decimal)selectRect.Left, 2);
		decimal num2 = Math.Round((decimal)selectRect.Top, 2);
		decimal num3 = Math.Round((decimal)selectRect.Right, 2);
		decimal num4 = Math.Round((decimal)selectRect.Bottom, 2);
		decimal num5 = Math.Round((decimal)x, 2);
		decimal num6 = Math.Round((decimal)y, 2);
		if ((num <= num5) & (num2 <= num6) & (num4 >= num6) & (num3 >= num5))
		{
			return StiAction.Move;
		}
		return StiAction.None;
	}

	public static StiAction PointInRect(double size, double x, double y, RectangleD sizeRect, bool isSelected)
	{
		if (isSelected)
		{
			if (PointInEdge(x, y, new PointD(sizeRect.X, sizeRect.Y), size))
			{
				return StiAction.SizeLeftTop;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X + sizeRect.Width, sizeRect.Y), size))
			{
				return StiAction.SizeRightTop;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X, sizeRect.Y + sizeRect.Height), size))
			{
				return StiAction.SizeLeftBottom;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X + sizeRect.Width, sizeRect.Y + sizeRect.Height), size))
			{
				return StiAction.SizeRightBottom;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X + sizeRect.Width / 2.0, sizeRect.Y), size))
			{
				return StiAction.SizeTop;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X + sizeRect.Width / 2.0, sizeRect.Y + sizeRect.Height), size))
			{
				return StiAction.SizeBottom;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X, sizeRect.Y + sizeRect.Height / 2.0), size))
			{
				return StiAction.SizeLeft;
			}
			if (PointInEdge(x, y, new PointD(sizeRect.X + sizeRect.Width, sizeRect.Y + sizeRect.Height / 2.0), size))
			{
				return StiAction.SizeRight;
			}
		}
		decimal num = Math.Round((decimal)sizeRect.Left, 2);
		decimal num2 = Math.Round((decimal)sizeRect.Top, 2);
		decimal num3 = Math.Round((decimal)sizeRect.Right, 2);
		decimal num4 = Math.Round((decimal)sizeRect.Bottom, 2);
		decimal num5 = Math.Round((decimal)x, 2);
		decimal num6 = Math.Round((decimal)y, 2);
		if ((num <= num5) & (num2 <= num6) & (num4 >= num6) & (num3 >= num5))
		{
			return StiAction.Move;
		}
		return StiAction.None;
	}

	public static bool PointInRect(float x, float y, RectangleF rect)
	{
		return PointInRect(x, y, RectangleD.CreateFromRectangle(rect));
	}

	public static bool PointInRect(double x, double y, RectangleD rect)
	{
		decimal num = Math.Round((decimal)rect.Left, 2);
		decimal num2 = Math.Round((decimal)rect.Top, 2);
		decimal num3 = Math.Round((decimal)rect.Right, 2);
		decimal num4 = Math.Round((decimal)rect.Bottom, 2);
		decimal num5 = Math.Round((decimal)x, 2);
		decimal num6 = Math.Round((decimal)y, 2);
		return (num <= num5) & (num2 <= num6) & (num4 > num6) & (num3 > num5);
	}
}
