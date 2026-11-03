using System.Collections.Generic;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public static class StiSimplifyHelper
{
	private static double GetSquareDistance(PointD p1, PointD p2)
	{
		double num = ((PointD)(ref p1)).X - ((PointD)(ref p2)).X;
		double num2 = ((PointD)(ref p1)).Y - ((PointD)(ref p2)).Y;
		return num * num + num2 * num2;
	}

	private static double GetSquareSegmentDistance(PointD p, PointD p1, PointD p2)
	{
		double num = ((PointD)(ref p1)).X;
		double num2 = ((PointD)(ref p1)).Y;
		double num3 = ((PointD)(ref p2)).X - num;
		double num4 = ((PointD)(ref p2)).Y - num2;
		if (!num3.Equals(0.0) || !num4.Equals(0.0))
		{
			double num5 = ((((PointD)(ref p)).X - num) * num3 + (((PointD)(ref p)).Y - num2) * num4) / (num3 * num3 + num4 * num4);
			if (num5 > 1.0)
			{
				num = ((PointD)(ref p2)).X;
				num2 = ((PointD)(ref p2)).Y;
			}
			else if (num5 > 0.0)
			{
				num += num3 * num5;
				num2 += num4 * num5;
			}
		}
		num3 = ((PointD)(ref p)).X - num;
		num4 = ((PointD)(ref p)).Y - num2;
		return num3 * num3 + num4 * num4;
	}

	private static List<PointD> SimplifyRadialDistance(PointD[] points, double sqTolerance)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		PointD val = points[0];
		List<PointD> list = new List<PointD> { val };
		PointD val2 = default(PointD);
		for (int i = 1; i < points.Length; i++)
		{
			val2 = points[i];
			if (GetSquareDistance(val2, val) > sqTolerance)
			{
				list.Add(val2);
				val = val2;
			}
		}
		if (!((object)(PointD)(ref val)).Equals((object?)val2))
		{
			list.Add(val2);
		}
		return list;
	}

	private static List<PointD> SimplifyDouglasPeucker(PointD[] points, double sqTolerance)
	{
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		int num = points.Length;
		int?[] array = new int?[num];
		int? num2 = 0;
		int? num3 = num - 1;
		int? num4 = 0;
		List<int?> list = new List<int?>();
		List<PointD> list2 = new List<PointD>();
		array[num2.Value] = (array[num3.Value] = 1);
		while (num3.HasValue)
		{
			double num5 = 0.0;
			for (int? num6 = num2 + 1; num6 < num3; num6++)
			{
				double squareSegmentDistance = GetSquareSegmentDistance(points[num6.Value], points[num2.Value], points[num3.Value]);
				if (squareSegmentDistance > num5)
				{
					num4 = num6;
					num5 = squareSegmentDistance;
				}
			}
			if (num5 > sqTolerance)
			{
				array[num4.Value] = 1;
				list.AddRange(new int?[4] { num2, num4, num4, num3 });
			}
			if (list.Count > 0)
			{
				num3 = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
			}
			else
			{
				num3 = null;
			}
			if (list.Count > 0)
			{
				num2 = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
			}
			else
			{
				num2 = null;
			}
		}
		for (int i = 0; i < num; i++)
		{
			if (array[i].HasValue)
			{
				list2.Add(points[i]);
			}
		}
		return list2;
	}

	public static List<PointD> Simplify(PointD[] points, double tolerance = 10.0, bool highestQuality = false)
	{
		if (points == null || points.Length == 0)
		{
			return new List<PointD>();
		}
		double sqTolerance = tolerance * tolerance;
		if (highestQuality)
		{
			return SimplifyDouglasPeucker(points, sqTolerance);
		}
		List<PointD> list = SimplifyRadialDistance(points, sqTolerance);
		return SimplifyDouglasPeucker(list.ToArray(), sqTolerance);
	}
}
