using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

internal static class StiLegendMarkerHelper
{
	internal static List<StiSegmentGeom> GetSteppedMarkerPath(RectangleF rect)
	{
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		list.Add(new StiLineSegmentGeom(rect.X, rect.Y, rect.X + rect.Width, rect.Y));
		list.Add(new StiLineSegmentGeom(rect.X + rect.Width, rect.Y, rect.X + rect.Width, rect.Y + rect.Height));
		list.Add(new StiLineSegmentGeom(rect.X + rect.Width, rect.Y + rect.Height, rect.X, rect.Y + rect.Height));
		list.Add(new StiLineSegmentGeom(rect.X, rect.Y + rect.Height, rect.X, rect.Y));
		return list;
	}

	internal static List<StiSegmentGeom> GetAreaMarkerPath(RectangleF rect)
	{
		float num = rect.Width / 5f;
		float num2 = rect.Height / 5f;
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		list.Add(new StiLineSegmentGeom(rect.X, rect.Y + num2 * 3.5f, rect.X, rect.Y + num2 * 3.5f));
		list.Add(new StiLineSegmentGeom(rect.X, rect.Y + num2 * 3.5f, rect.X + num * 2f, rect.Y + num2 * 0.5f));
		list.Add(new StiLineSegmentGeom(rect.X + num * 2f, rect.Y + num2 * 0.5f, rect.X + num * 4f, rect.Y + num2 * 2.5f));
		list.Add(new StiLineSegmentGeom(rect.X + num * 4f, rect.Y + num2 * 2.5f, rect.X + num * 5f, rect.Y + num2 * 1.5f));
		list.Add(new StiLineSegmentGeom(rect.X + num * 5f, rect.Y + num2 * 1.5f, rect.X + num * 5f, rect.Y + num2 * 5f));
		list.Add(new StiLineSegmentGeom(rect.X + num * 5f, rect.Y + num2 * 5f, rect.X, rect.Y + num2 * 5f));
		list.Add(new StiLineSegmentGeom(rect.X, rect.Y + num2 * 5f, rect.X, rect.Y + num2 * 3f));
		return list;
	}

	internal static PointF[] GetAreaMarkerLinePoints(RectangleF rect)
	{
		float num = rect.Width / 5f;
		float num2 = rect.Height / 5f;
		return new PointF[4]
		{
			new PointF(rect.X, rect.Y + num2 * 3.5f),
			new PointF(rect.X + num * 2f, rect.Y + num2 * 0.5f),
			new PointF(rect.X + num * 4f, rect.Y + num2 * 2.5f),
			new PointF(rect.X + num * 5f, rect.Y + num2 * 1.5f)
		};
	}

	internal static List<StiSegmentGeom> GetSplineAreaMarkerPath(RectangleF rect)
	{
		float num = rect.Width / 5f;
		float num2 = rect.Height / 5f;
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		list.Add(new StiLineSegmentGeom(rect.X, rect.Y + num2 * 5f, rect.X, rect.Y + num2 * 3.5f));
		PointF[] splineAreaMarkerLinePoints = GetSplineAreaMarkerLinePoints(rect);
		list.Add(new StiCurveSegmentGeom(splineAreaMarkerLinePoints, 0.55f));
		list.Add(new StiLineSegmentGeom(rect.X + num * 5f, rect.Y + num2 * 1.5f, rect.X + num * 5f, rect.Y + num2 * 5f));
		list.Add(new StiLineSegmentGeom(rect.X + num * 5f, rect.Y + num2 * 5f, rect.X, rect.Y + num2 * 5f));
		return list;
	}

	internal static PointF[] GetSplineAreaMarkerLinePoints(RectangleF rect)
	{
		float num = rect.Width / 5f;
		float num2 = rect.Height / 5f;
		return new PointF[4]
		{
			new PointF(rect.X, rect.Y + num2 * 3.5f),
			new PointF(rect.X + num * 2f, rect.Y + num2 * 0.5f),
			new PointF(rect.X + num * 4f, rect.Y + num2 * 2.5f),
			new PointF(rect.X + num * 5f, rect.Y + num2 * 1.5f)
		};
	}
}
