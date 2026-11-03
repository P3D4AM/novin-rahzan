using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;

namespace Stimulsoft.Report.Chart;

internal static class StiNullableDrawing
{
	public static void DrawLines(StiContext context, StiPenGeom penGeom, PointF?[] points)
	{
		DrawLines(context, penGeom, null, points, null);
	}

	public static void DrawLines(StiContext context, StiPenGeom penGeom, PointF?[] points, StiAnimation animation)
	{
		DrawLines(context, penGeom, null, points, animation);
	}

	public static void DrawLines(StiContext context, StiPenGeom penGeom, PointF?[] pointsStart, PointF?[] points, StiAnimation animation)
	{
		List<PointF> list = new List<PointF>();
		List<PointF> list2 = new List<PointF>();
		for (int i = 0; i < points.Length; i++)
		{
			if (points[i].HasValue)
			{
				list.Add(points[i].Value);
				if (pointsStart != null && pointsStart.Length > i)
				{
					list2.Add(pointsStart[i].GetValueOrDefault());
				}
			}
			else
			{
				if (list.Count > 1)
				{
					DrawLines(context, penGeom, list2, list, animation);
				}
				list.Clear();
			}
		}
		if (list.Count > 1)
		{
			DrawLines(context, penGeom, list2, list, animation);
		}
	}

	private static void DrawLines(StiContext context, StiPenGeom penGeom, List<PointF> newPointsStart, List<PointF> newPoints, StiAnimation animation)
	{
		if (animation != null)
		{
			context.DrawAnimationLines(penGeom, newPoints.ToArray(), animation);
		}
		else
		{
			context.DrawLines(penGeom, newPoints.ToArray());
		}
	}

	public static void DrawCurve(StiContext context, StiPenGeom penGeom, PointF?[] points, float tension)
	{
		DrawCurve(context, penGeom, points, tension, null);
	}

	public static void DrawCurve(StiContext context, StiPenGeom penGeom, PointF?[] points, float tension, StiAnimation animation)
	{
		DrawCurve(context, penGeom, null, points, tension, animation);
	}

	public static void DrawCurve(StiContext context, StiPenGeom penGeom, PointF?[] pointsStart, PointF?[] points, float tension, StiAnimation animation)
	{
		List<PointF> list = new List<PointF>();
		List<PointF> list2 = new List<PointF>();
		for (int i = 0; i < points.Length; i++)
		{
			if (points[i].HasValue)
			{
				list.Add(points[i].Value);
				if (pointsStart != null && pointsStart.Length > i)
				{
					list2.Add(pointsStart[i].GetValueOrDefault());
				}
			}
			else
			{
				if (list.Count > 1)
				{
					DrawCurve(context, penGeom, list2, list, tension, animation);
				}
				list.Clear();
			}
		}
		if (list.Count > 1)
		{
			DrawCurve(context, penGeom, list2, list, tension, animation);
		}
	}

	private static void DrawCurve(StiContext context, StiPenGeom penGeom, List<PointF> newPointsStart, List<PointF> newPoints, float tension, StiAnimation animation)
	{
		if (animation != null)
		{
			context.DrawAnimationCurve(penGeom, newPoints.ToArray(), tension, animation);
		}
		else
		{
			context.DrawCurve(penGeom, newPoints.ToArray(), tension);
		}
	}

	public static List<PointF[]> GetPointsList(PointF?[] points)
	{
		List<PointF[]> list = new List<PointF[]>();
		List<PointF> list2 = new List<PointF>();
		for (int i = 0; i < points.Length; i++)
		{
			PointF? pointF = points[i];
			if (pointF.HasValue)
			{
				list2.Add(pointF.Value);
				continue;
			}
			if (list2.Count > 1)
			{
				list.Add(list2.ToArray());
			}
			list2.Clear();
		}
		if (list2.Count > 1)
		{
			list.Add(list2.ToArray());
		}
		return list;
	}

	public static List<PointF?[]> GetNullablePointsList(PointF?[] points)
	{
		List<PointF?[]> list = new List<PointF?[]>();
		List<PointF?> list2 = new List<PointF?>();
		for (int i = 0; i < points.Length; i++)
		{
			PointF? item = points[i];
			if (item.HasValue)
			{
				list2.Add(item);
				continue;
			}
			if (list2.Count > 1)
			{
				list.Add(list2.ToArray());
			}
			list2.Clear();
		}
		if (list2.Count >= 1)
		{
			list.Add(list2.ToArray());
		}
		return list;
	}

	public static void GetPointsList(PointF?[] points1, PointF?[] points2, out List<PointF[]> list1, out List<PointF[]> list2)
	{
		list1 = new List<PointF[]>();
		list2 = new List<PointF[]>();
		List<PointF> list3 = new List<PointF>();
		List<PointF> list4 = new List<PointF>();
		int num = 0;
		for (int i = 0; i < points1.Length; i++)
		{
			PointF? pointF = points1[i];
			PointF? pointF2 = points2[num];
			if (pointF.HasValue && pointF2.HasValue)
			{
				list3.Add(pointF.Value);
				list4.Add(pointF2.Value);
			}
			else
			{
				if (list3.Count > 1)
				{
					list1.Add(list3.ToArray());
					list2.Add(list4.ToArray());
				}
				list3.Clear();
				list4.Clear();
			}
			num++;
		}
		if (list3.Count > 1)
		{
			list1.Add(list3.ToArray());
			list2.Add(list4.ToArray());
		}
	}
}
