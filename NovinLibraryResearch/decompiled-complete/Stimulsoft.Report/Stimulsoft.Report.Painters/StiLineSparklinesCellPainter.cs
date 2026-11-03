using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Report.Painters;

public class StiLineSparklinesCellPainter
{
	internal static void Draw(StiContext context, RectangleD rect, object[] array, float zoom, Color sparklineColor, bool showArea, bool showFirstLastMarker = true, bool showHighLowMarker = false)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		if (array == null || array.Length < 2)
		{
			return;
		}
		((RectangleD)(ref rect)).Inflate((double)(-4f * zoom), (0.0 - ((RectangleD)(ref rect)).Height) / 4.0);
		double[] array2 = ((IEnumerable<object>)array).Select((Func<object, double>)StiValueHelper.TryToDouble).ToArray();
		if (array2.All((double v) => v == 0.0))
		{
			return;
		}
		double min = array2.Min((double v) => v);
		double max = array2.Max((double v) => v);
		if (min == 0.0 && max == 0.0)
		{
			return;
		}
		if (min > 0.0)
		{
			array2 = array2.Select((double v) => v - min).ToArray();
			max -= min;
			min = 0.0;
		}
		if (max < 0.0)
		{
			array2 = array2.Select((double v) => v + (0.0 - max)).ToArray();
			min += 0.0 - max;
			max = 0.0;
		}
		float stepWidth = (float)(((RectangleD)(ref rect)).Width / (double)(array2.Length - 1));
		double totalHeight = max + Math.Abs(min);
		if (totalHeight == 0.0)
		{
			return;
		}
		double maxPart = ((RectangleD)(ref rect)).Height * max / totalHeight;
		float x = (float)((RectangleD)(ref rect)).X;
		PointF[] array3 = array2.Select(delegate(double value)
		{
			double num = ((RectangleD)(ref rect)).Height * Math.Abs(value) / totalHeight;
			float num2 = (float)(((RectangleD)(ref rect)).Y + ((value > 0.0) ? (maxPart - num) : maxPart));
			PointF result = new PointF(x, (value > 0.0) ? num2 : (num2 + (float)num));
			x += stepWidth;
			return result;
		}).ToArray();
		if (array3.Length > 1)
		{
			array3 = SimplifyPoints(array3);
			if (showArea)
			{
				DrawArea(context, rect, array3, zoom, sparklineColor);
			}
			DrawLines(context, array3, zoom, sparklineColor);
			if (showFirstLastMarker)
			{
				DrawFirstLastMarkers(context, array3, zoom);
			}
			if (showHighLowMarker)
			{
				DrawHighLowMarkers(context, array3, zoom);
			}
		}
	}

	private static void DrawArea(StiContext context, RectangleD rect, PointF[] points, float zoom, Color sparklineColor)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		List<PointF> list2 = points.ToList();
		list2.Insert(0, new PointF(points.FirstOrDefault().X, (float)(((RectangleD)(ref rect)).Bottom + (double)(2f * zoom))));
		list2.Add(new PointF(points.LastOrDefault().X, (float)(((RectangleD)(ref rect)).Bottom + (double)(2f * zoom))));
		list.Add(new StiLinesSegmentGeom(list2.ToArray()));
		context.FillPath(Color.FromArgb(60, sparklineColor), list, rect);
	}

	private static void DrawLines(StiContext context, PointF[] points, float zoom, Color sparklineColor)
	{
		float num = points.Min((PointF p) => p.Y);
		float num2 = points.Max((PointF p) => p.Y);
		PointF pointF = points.FirstOrDefault();
		PointF pointF2 = points.LastOrDefault();
		new RectangleF(pointF.X, num, pointF2.X - pointF.X, num2 - num).Inflate(1f, 1f);
		context.PushSmoothingModeToAntiAlias();
		float thickness = 2f * zoom;
		context.DrawLines(new StiPenGeom(sparklineColor, thickness), points);
		context.PopSmoothingMode();
	}

	private static void DrawFirstLastMarkers(StiContext context, PointF[] points, float zoom)
	{
		DrawMarker(context, points.FirstOrDefault(), Color.DimGray, zoom);
		DrawMarker(context, points.LastOrDefault(), Color.DimGray, zoom);
	}

	private static void DrawHighLowMarkers(StiContext context, PointF[] points, float zoom)
	{
		if (points.Length < 2)
		{
			return;
		}
		PointF p = points.FirstOrDefault();
		PointF p2 = points.FirstOrDefault();
		for (int i = 0; i < points.Length; i++)
		{
			PointF pointF = points[i];
			if (pointF.Y > p2.Y)
			{
				p2 = pointF;
			}
			if (pointF.Y < p.Y)
			{
				p = pointF;
			}
		}
		DrawMarker(context, p, Color.Green, zoom);
		DrawMarker(context, p2, Color.Red, zoom);
	}

	private static void DrawMarker(StiContext context, PointF p, Color color, float zoom)
	{
		context.PushSmoothingModeToAntiAlias();
		float num = 3.5f * zoom;
		context.FillEllipse(Color.White, new RectangleF(p.X - num, p.Y - num, num * 2f, num * 2f));
		num -= 1f;
		context.FillEllipse(color, new RectangleF(p.X - num, p.Y - num, num * 2f, num * 2f));
		context.PopSmoothingMode();
	}

	private static PointF[] SimplifyPoints(PointF[] points)
	{
		if (points.Length <= 30)
		{
			return points;
		}
		List<PointD> source = StiSimplifyHelper.Simplify(((IEnumerable<PointF>)points).Select((Func<PointF, PointD>)((PointF p) => new PointD((double)p.X, (double)p.Y))).ToArray());
		return source.Select((PointD p) => ((PointD)(ref p)).ToPointF()).ToArray();
	}
}
