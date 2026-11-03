using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

internal class StiPie3dQuadrilateral
{
	private readonly StiAnimation animation;

	private readonly RectangleF rectangle;

	private readonly PointF point1;

	private readonly PointF point2;

	private readonly PointF point3;

	private readonly PointF point4;

	private readonly bool toClose;

	public static readonly StiPie3dQuadrilateral Empty = new StiPie3dQuadrilateral();

	internal void Draw(StiContext context, StiPenGeom pen, StiBrush brush, bool isMouseOver)
	{
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		list.Add(new StiLineSegmentGeom(point1, point2));
		list.Add(new StiLineSegmentGeom(point2, point3));
		list.Add(new StiLineSegmentGeom(point3, point4));
		if (toClose)
		{
			list.Add(new StiCloseFigureSegmentGeom());
		}
		RectangleF pathBounds = context.GetPathBounds(list);
		if (animation != null)
		{
			context.DrawAnimationPathElement(brush, brush, pen, list, rectangle, null, this, animation);
		}
		else
		{
			context.FillPath(brush, list, pathBounds);
			context.DrawPath(pen, list, pathBounds);
		}
		if (isMouseOver)
		{
			context.FillPath(StiMouseOverHelper.GetMouseOverColor(), list, pathBounds);
		}
	}

	public bool Contains(PointF point)
	{
		PointF[] array = new PointF[4] { point1, point2, point3, point4 };
		if (array.Length == 0)
		{
			return false;
		}
		return Contains(point, array);
	}

	public static bool Contains(PointF point, PointF[] cornerPoints)
	{
		int num = 0;
		for (int i = 1; i < cornerPoints.Length; i++)
		{
			if (DoesIntersect(point, cornerPoints[i], cornerPoints[i - 1]))
			{
				num++;
			}
		}
		if (DoesIntersect(point, cornerPoints[^1], cornerPoints[0]))
		{
			num++;
		}
		return num % 2 != 0;
	}

	private static bool DoesIntersect(PointF point, PointF point1, PointF point2)
	{
		float x = point2.X;
		float y = point2.Y;
		float x2 = point1.X;
		float y2 = point1.Y;
		if ((x < point.X && x2 >= point.X) || (x >= point.X && x2 < point.X))
		{
			float num = (y - y2) / (x - x2) * (point.X - x2) + y2;
			return num > point.Y;
		}
		return false;
	}

	protected StiPie3dQuadrilateral()
	{
	}

	public StiPie3dQuadrilateral(RectangleF rectangle, PointF point1, PointF point2, PointF point3, PointF point4, bool toClose, StiAnimation animation)
	{
		this.rectangle = rectangle;
		this.point1 = point1;
		this.point2 = point2;
		this.point3 = point3;
		this.point4 = point4;
		this.toClose = toClose;
		this.animation = animation;
	}
}
