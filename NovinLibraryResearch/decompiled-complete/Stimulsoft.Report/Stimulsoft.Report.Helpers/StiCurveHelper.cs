using System.Drawing;

namespace Stimulsoft.Report.Helpers;

public static class StiCurveHelper
{
	public static Point[] CardinalSpline(PointF[] pts, bool closed)
	{
		double num = 1.0 / 6.0;
		int num2 = ((!closed) ? (pts.Length * 3 - 2) : ((pts.Length + 1) * 3 - 2));
		Point[] array = new Point[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = default(Point);
		}
		Point p;
		if (!closed)
		{
			Point point = new Point((int)pts[0].X, (int)pts[0].Y);
			CalcCurveEnd(point, new Point((int)pts[1].X, (int)pts[1].Y), num, out p);
			array[0] = point;
			array[1] = p;
		}
		Point p2;
		for (int i = 0; i < pts.Length - (closed ? 1 : 2); i++)
		{
			Point point2 = new Point((int)pts[i + 1].X, (int)pts[i + 1].Y);
			CalcCurve(new Point[3]
			{
				new Point((int)pts[i].X, (int)pts[i].Y),
				point2,
				new Point((int)pts[(i + 2) % pts.Length].X, (int)pts[(i + 2) % pts.Length].Y)
			}, num, out p, out p2);
			array[3 * i + 2] = p;
			array[3 * i + 3] = point2;
			array[3 * i + 4] = p2;
		}
		if (closed)
		{
			Point point3 = new Point((int)pts[0].X, (int)pts[0].Y);
			CalcCurve(new Point[3]
			{
				new Point((int)pts[^1].X, (int)pts[^1].Y),
				point3,
				new Point((int)pts[1].X, (int)pts[1].Y)
			}, num, out p, out p2);
			array[num2 - 2] = p;
			array[0] = point3;
			array[1] = p2;
			array[num2 - 1] = array[0];
		}
		else
		{
			Point point4 = new Point((int)pts[^1].X, (int)pts[^1].Y);
			CalcCurveEnd(point4, new Point((int)pts[^2].X, (int)pts[^2].Y), num, out p);
			array[num2 - 2] = p;
			array[num2 - 1] = point4;
		}
		return array;
	}

	private static void CalcCurveEnd(Point end, Point adj, double tension, out Point p1)
	{
		p1 = new Point((int)(tension * (double)(adj.X - end.X) + (double)end.X), (int)(tension * (double)(adj.Y - end.Y) + (double)end.Y));
	}

	private static void CalcCurve(Point[] pts, double tenstion, out Point p1, out Point p2)
	{
		double num = pts[2].X - pts[0].X;
		double num2 = pts[2].Y - pts[0].Y;
		p1 = new Point((int)((double)pts[1].X - tenstion * num), (int)((double)pts[1].Y - tenstion * num2));
		p2 = new Point((int)((double)pts[1].X + tenstion * num), (int)((double)pts[1].Y + tenstion * num2));
	}
}
