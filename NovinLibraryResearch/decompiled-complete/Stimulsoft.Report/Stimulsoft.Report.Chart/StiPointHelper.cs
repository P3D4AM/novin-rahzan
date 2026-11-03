using System;
using System.Collections.Generic;
using System.Drawing;

namespace Stimulsoft.Report.Chart;

internal class StiPointHelper
{
	private enum PointClassify
	{
		Left,
		Right,
		Beyond,
		Behind,
		Between,
		Origin,
		Destination
	}

	private static PointClassify GetPointClassify(PointF basePoint, PointF point1, PointF point2)
	{
		PointF pointF = new PointF(point2.X - point1.X, point2.Y - point1.Y);
		PointF pointF2 = new PointF(basePoint.X - point1.X, basePoint.Y - point1.Y);
		float num = pointF.X * pointF2.Y - pointF2.X * pointF.Y;
		if ((double)num > 0.0)
		{
			return PointClassify.Left;
		}
		if ((double)num < 0.0)
		{
			return PointClassify.Right;
		}
		if ((double)(pointF.X * pointF2.X) < 0.0 || (double)(pointF.Y * pointF2.Y) < 0.0)
		{
			return PointClassify.Behind;
		}
		if (Math.Sqrt(pointF.X * pointF.X + pointF.Y * pointF.Y) < Math.Sqrt(pointF2.X * pointF2.X + pointF2.Y * pointF2.Y))
		{
			return PointClassify.Beyond;
		}
		if (point1.Equals((object?)basePoint))
		{
			return PointClassify.Origin;
		}
		if (point2.Equals((object?)basePoint))
		{
			return PointClassify.Destination;
		}
		return PointClassify.Between;
	}

	public static bool IsPointInTriangle(PointF p, PointF a, PointF b, PointF c)
	{
		if (GetPointClassify(p, a, b) != 0 && GetPointClassify(p, b, c) != 0)
		{
			return GetPointClassify(p, c, a) != PointClassify.Left;
		}
		return false;
	}

	public static bool IsPointInPolygon(PointF p, PointF[] points)
	{
		for (int i = 0; i < points.Length; i++)
		{
			if (GetPointClassify(p, points[i], (i + 1 < points.Length) ? points[i + 1] : points[0]) == PointClassify.Left)
			{
				return false;
			}
		}
		return true;
	}

	public static PointF[] GetLineOffsetRectangle(PointF point1, PointF point2, double offset)
	{
		double num = Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
		double num2 = offset / 2.0;
		return new PointF[4]
		{
			new PointF((float)((double)point1.X + num2 * Math.Cos(num + Math.PI / 2.0)), (float)((double)point1.Y + num2 * Math.Sin(num + Math.PI / 2.0))),
			new PointF((float)((double)point2.X + num2 * Math.Cos(num + Math.PI / 2.0)), (float)((double)point2.Y + num2 * Math.Sin(num + Math.PI / 2.0))),
			new PointF((float)((double)point2.X + num2 * Math.Cos(num + 4.71238898038469)), (float)((double)point2.Y + num2 * Math.Sin(num + 4.71238898038469))),
			new PointF((float)((double)point1.X + num2 * Math.Cos(num + 4.71238898038469)), (float)((double)point1.Y + num2 * Math.Sin(num + 4.71238898038469)))
		};
	}

	public static bool IsLineContainsPoint(PointF startPoint, PointF endPoint, float offset, PointF point)
	{
		PointF[] lineOffsetRectangle = GetLineOffsetRectangle(startPoint, endPoint, offset);
		return IsPointInPolygon(point, lineOffsetRectangle);
	}

	public static PointF?[] OptimizePoints(PointF?[] points)
	{
		if (points.Length < 800)
		{
			return points;
		}
		int i = 0;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		List<PointF?> list = new List<PointF?>();
		while (i < points.Length)
		{
			if (!points[i].HasValue)
			{
				for (; i < points.Length && !points[i].HasValue; i++)
				{
				}
				list.Add(null);
				continue;
			}
			PointF value = points[i].Value;
			i++;
			if (i < points.Length && points[i].HasValue && points[i].HasValue && points[i].Value.X - value.X < 1f)
			{
				num = value.X;
				num2 = value.Y;
				num3 = value.Y;
				float y = value.Y;
				float y2 = value.Y;
				for (; i < points.Length && points[i].HasValue && points[i].HasValue && points[i].Value.X - num < 1f; i++)
				{
					num2 = Math.Min(num2, points[i].Value.Y);
					num3 = Math.Max(num3, points[i].Value.Y);
					y2 = points[i].Value.Y;
				}
				if (y2 > y)
				{
					list.Add(new PointF(num, num2));
					list.Add(new PointF(num + 0.5f, num3));
				}
				else
				{
					list.Add(new PointF(num, num3));
					list.Add(new PointF(num + 0.5f, num2));
				}
			}
			else
			{
				list.Add(value);
			}
		}
		return list.ToArray();
	}
}
