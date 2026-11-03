using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiSplineRangeSeriesGeom : StiSplineSeriesGeom
{
	public PointF?[] PointsEnd { get; }

	public override void Draw(StiContext context)
	{
		List<PointF[]> pointsList = StiNullableDrawing.GetPointsList(base.Points);
		List<PointF[]> pointsList2 = StiNullableDrawing.GetPointsList(PointsEnd);
		if (pointsList.Count >= 1 && pointsList2.Count >= 1)
		{
			PointF[] points = pointsList[0];
			PointF[] pointsEnd = pointsList2[0];
			string[] pointsEndIds = base.PointsIds.Select((string id) => id + "_e").ToArray();
			FillPath(context, points, pointsEnd, base.PointsIds, pointsEndIds);
		}
	}

	private void FillPath(StiContext context, PointF[] points, PointF[] pointsEnd, string[] pointsIds, string[] pointsEndIds)
	{
		IStiSplineRangeSeries stiSplineRangeSeries = base.Series as IStiSplineRangeSeries;
		StiBrush brush = stiSplineRangeSeries.Brush;
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		PointF[] array = pointsEnd.Reverse().ToArray();
		StiLineSegmentGeom stiLineSegmentGeom = new StiLineSegmentGeom(array[^1], points[0]);
		StiCurveSegmentGeom stiCurveSegmentGeom = new StiCurveSegmentGeom(points, stiSplineRangeSeries.Tension);
		StiLineSegmentGeom stiLineSegmentGeom2 = new StiLineSegmentGeom(points[^1], array[0]);
		StiCurveSegmentGeom stiCurveSegmentGeom2 = new StiCurveSegmentGeom(array, stiSplineRangeSeries.Tension);
		list.Add(stiLineSegmentGeom);
		list.Add(stiCurveSegmentGeom);
		list.Add(stiLineSegmentGeom2);
		list.Add(stiCurveSegmentGeom2);
		StiChart stiChart = base.Series.Chart as StiChart;
		if (stiChart.IsAnimation)
		{
			StiAnimation animation = null;
			if (stiChart.IsAnimationChangingValues)
			{
				StiPointsAnimation stiPointsAnimation = new StiPointsAnimation(points, points, pointsIds, StiChartHelper.GlobalDurationElement, StiChartHelper.GlobalBeginTimeElement);
				stiPointsAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_a";
				StiPointsAnimation stiPointsAnimation2 = new StiPointsAnimation(pointsEnd, pointsEnd, pointsEndIds, stiPointsAnimation.Duration, stiPointsAnimation.BeginTime);
				stiPointsAnimation2.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_a_e";
				context.Animations.Add(stiPointsAnimation);
				context.Animations.Add(stiPointsAnimation2);
				if (StiPointsAnimation.IsAnimationChangingValues(base.Series, pointsIds, stiPointsAnimation.Id))
				{
					stiPointsAnimation.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
					stiPointsAnimation2.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
					stiPointsAnimation2.Reverse();
					stiLineSegmentGeom.Animation = new StiPointsAnimation(new PointF[2]
					{
						stiPointsAnimation2.PointsFrom[stiPointsAnimation2.PointsFrom.Length - 1],
						stiPointsAnimation.PointsFrom[0]
					}, null, null, stiPointsAnimation.Duration, stiPointsAnimation.BeginTime);
					stiLineSegmentGeom2.Animation = new StiPointsAnimation(new PointF[2]
					{
						stiPointsAnimation.PointsFrom[stiPointsAnimation.PointsFrom.Length - 1],
						stiPointsAnimation2.PointsFrom[0]
					}, null, null, stiPointsAnimation.Duration, stiPointsAnimation.BeginTime);
					stiLineSegmentGeom.Animation.BeginTimeCorrect = stiPointsAnimation.BeginTimeCorrect;
					stiLineSegmentGeom2.Animation.BeginTimeCorrect = stiPointsAnimation.BeginTimeCorrect;
					stiCurveSegmentGeom.Animation = stiPointsAnimation;
					stiCurveSegmentGeom2.Animation = stiPointsAnimation2;
				}
				else
				{
					animation = new StiOpacityAnimation(StiChartHelper.GlobalDurationElement, StiChartHelper.GlobalBeginTimeElement);
				}
			}
			context.FillDrawAnimationPath(brush, null, list, StiPathGeom.GetBoundsState, null, animation);
		}
		else
		{
			context.FillPath(brush, list, StiPathGeom.GetBoundsState);
		}
		if (IsMouseOver || base.Series.Core.IsMouseOver)
		{
			context.FillPath(StiMouseOverHelper.GetMouseOverColor(), list, StiPathGeom.GetBoundsState);
		}
	}

	public StiSplineRangeSeriesGeom(StiAreaGeom areaGeom, StiSeriesPointsInfo pointsInfo, IStiSeries series)
		: base(areaGeom, pointsInfo, series)
	{
		PointsEnd = pointsInfo.PointsEnd;
	}
}
