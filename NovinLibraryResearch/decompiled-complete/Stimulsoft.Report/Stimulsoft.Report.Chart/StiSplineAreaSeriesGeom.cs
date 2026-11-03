using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;

namespace Stimulsoft.Report.Chart;

public class StiSplineAreaSeriesGeom : StiSplineSeriesGeom
{
	public override bool Contains(float x, float y)
	{
		IStiAxisArea stiAxisArea = base.Series.Chart.Area as IStiAxisArea;
		float dividerY = stiAxisArea.AxisCore.GetDividerY();
		if (Invisible)
		{
			return false;
		}
		for (int i = 0; i < base.Points.Length - 1; i++)
		{
			PointF? pointF = base.Points[i];
			PointF? pointF2 = base.Points[i + 1];
			if (pointF.HasValue && pointF2.HasValue)
			{
				PointF pointF3 = new PointF(pointF2.Value.X, dividerY);
				PointF pointF4 = new PointF(pointF.Value.X, dividerY);
				if (StiPointHelper.IsPointInPolygon(new PointF(x, y), new PointF[4] { pointF.Value, pointF4, pointF3, pointF2.Value }))
				{
					return true;
				}
			}
		}
		return false;
	}

	public override void Draw(StiContext context)
	{
		IStiSplineAreaSeries stiSplineAreaSeries = base.Series as IStiSplineAreaSeries;
		StiChart stiChart = base.Series.Chart as StiChart;
		IStiAxisArea stiAxisArea = base.Series.Chart.Area as IStiAxisArea;
		float dividerY = stiAxisArea.AxisCore.GetDividerY();
		List<PointF[]> pointsList = StiNullableDrawing.GetPointsList(base.Points);
		List<PointF[]> list = ((base.PointsFrom != null) ? StiNullableDrawing.GetPointsList(base.PointsFrom) : null);
		if (stiChart.IsAnimation)
		{
			int num;
			for (num = 0; num < pointsList.Count; num++)
			{
				PointF[] array = pointsList[num];
				List<StiSegmentGeom> list2 = new List<StiSegmentGeom>();
				StiLineSegmentGeom stiLineSegmentGeom = new StiLineSegmentGeom(new PointF(array[0].X, dividerY), array[0]);
				StiCurveSegmentGeom stiCurveSegmentGeom = new StiCurveSegmentGeom(array, stiSplineAreaSeries.Tension);
				StiLineSegmentGeom stiLineSegmentGeom2 = new StiLineSegmentGeom(array[^1], new PointF(array[^1].X, dividerY));
				list2.Add(stiLineSegmentGeom);
				list2.Add(stiCurveSegmentGeom);
				list2.Add(stiLineSegmentGeom2);
				if (StiPointsAnimation.IsAnimationChangingValues(base.Series, base.PointsIds))
				{
					PointF[] pointsFrom = list[num];
					StiPointsAnimation stiPointsAnimation = new StiPointsAnimation(pointsFrom, array, base.PointsIds, StiChartHelper.GlobalDurationElement, StiChartHelper.GlobalBeginTimeElement);
					stiPointsAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}";
					stiPointsAnimation.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
					stiLineSegmentGeom.Animation = new StiPointsAnimation(new PointF[2]
					{
						new PointF(stiPointsAnimation.PointsFrom.FirstOrDefault().X, dividerY),
						stiPointsAnimation.PointsFrom.FirstOrDefault()
					}, stiPointsAnimation.Duration, stiPointsAnimation.BeginTime)
					{
						BeginTimeCorrect = stiPointsAnimation.BeginTimeCorrect
					};
					stiCurveSegmentGeom.Animation = stiPointsAnimation;
					stiLineSegmentGeom2.Animation = new StiPointsAnimation(new PointF[2]
					{
						stiPointsAnimation.PointsFrom.LastOrDefault(),
						new PointF(stiPointsAnimation.PointsFrom.LastOrDefault().X, dividerY)
					}, stiPointsAnimation.Duration, stiPointsAnimation.BeginTime)
					{
						BeginTimeCorrect = stiPointsAnimation.BeginTimeCorrect
					};
					context.Animations.Add(stiPointsAnimation);
					context.FillDrawAnimationPath(stiSplineAreaSeries.Brush, null, list2, StiPathGeom.GetBoundsState, null, null);
					if (stiSplineAreaSeries.AllowApplyBrushNegative && stiSplineAreaSeries.BrushNegative != null)
					{
						float width = (float)(stiAxisArea.AxisCore.ScrollRangeX * stiAxisArea.AxisCore.ScrollDpiX);
						float height = (float)(stiAxisArea.AxisCore.ScrollRangeY * stiAxisArea.AxisCore.ScrollDpiY - (double)dividerY);
						RectangleF clipRect = new RectangleF(0f, dividerY, width, height);
						context.PushClip(clipRect);
						context.FillDrawAnimationPath(stiSplineAreaSeries.BrushNegative, null, list2, StiPathGeom.GetBoundsState, null, null);
						context.PopClip();
					}
				}
				else
				{
					StiOpacityAnimation animation = new StiOpacityAnimation(StiChartHelper.GlobalDurationElement, StiChartHelper.GlobalBeginTimeElement);
					context.FillDrawAnimationPath(stiSplineAreaSeries.Brush, null, list2, StiPathGeom.GetBoundsState, null, animation);
				}
				num++;
			}
			return;
		}
		foreach (PointF[] item in pointsList)
		{
			List<StiSegmentGeom> list3 = new List<StiSegmentGeom>();
			list3.Add(new StiLineSegmentGeom(new PointF(item[0].X, dividerY), item[0]));
			list3.Add(new StiCurveSegmentGeom(item, stiSplineAreaSeries.Tension));
			list3.Add(new StiLineSegmentGeom(item[^1], new PointF(item[^1].X, dividerY)));
			if (stiSplineAreaSeries.Brush != null)
			{
				context.FillPath(stiSplineAreaSeries.Brush, list3, StiPathGeom.GetBoundsState);
			}
			if (stiSplineAreaSeries.AllowApplyBrushNegative && stiSplineAreaSeries.BrushNegative != null)
			{
				float width2 = (float)(stiAxisArea.AxisCore.ScrollRangeX * stiAxisArea.AxisCore.ScrollDpiX);
				float height2 = (float)(stiAxisArea.AxisCore.ScrollRangeY * stiAxisArea.AxisCore.ScrollDpiY - (double)dividerY);
				RectangleF clipRect2 = new RectangleF(0f, dividerY, width2, height2);
				context.PushClip(clipRect2);
				context.FillPath(stiSplineAreaSeries.BrushNegative, list3, StiPathGeom.GetBoundsState);
				context.PopClip();
			}
			if (IsMouseOver || base.Series.Core.IsMouseOver)
			{
				context.FillPath(StiMouseOverHelper.GetMouseOverColor(), list3, StiPathGeom.GetBoundsState);
			}
		}
	}

	public StiSplineAreaSeriesGeom(StiAreaGeom areaGeom, StiSeriesPointsInfo pointsInfo, IStiSeries series)
		: base(areaGeom, pointsInfo, series)
	{
	}
}
