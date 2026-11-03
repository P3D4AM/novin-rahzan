using System;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiStackedBaseLineSeriesGeom : StiSeriesGeom
{
	private PointF?[] points;

	public PointF?[] Points => points;

	internal static RectangleF GetClientRectangle(PointF?[] points)
	{
		if (points == null || points.Length == 0)
		{
			return RectangleF.Empty;
		}
		PointF pointF = PointF.Empty;
		PointF pointF2 = PointF.Empty;
		for (int i = 0; i < points.Length; i++)
		{
			PointF? pointF3 = points[i];
			if (pointF3.HasValue)
			{
				if (pointF == PointF.Empty)
				{
					pointF = pointF3.Value;
					pointF2 = pointF3.Value;
					continue;
				}
				pointF.X = Math.Min(pointF.X, pointF3.Value.X);
				pointF.Y = Math.Min(pointF.Y, pointF3.Value.Y);
				pointF2.X = Math.Max(pointF2.X, pointF3.Value.X);
				pointF2.Y = Math.Max(pointF2.Y, pointF3.Value.Y);
			}
		}
		return new RectangleF(pointF.X, pointF.Y, pointF2.X - pointF.X, pointF2.Y - pointF.Y);
	}

	public override void Draw(StiContext context)
	{
	}

	public StiStackedBaseLineSeriesGeom(StiAreaGeom areaGeom, PointF?[] points, IStiSeries series)
		: base(areaGeom, series, GetClientRectangle(points))
	{
		this.points = points;
	}
}
