using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;

namespace Stimulsoft.Report.Chart;

public class StiSteppedAreaSeriesGeom : StiSteppedLineSeriesGeom
{
	public override void Draw(StiContext context)
	{
		IStiSteppedAreaSeries stiSteppedAreaSeries = base.Series as IStiSteppedAreaSeries;
		StiChart stiChart = base.Series.Chart as StiChart;
		IStiAxisArea stiAxisArea = base.Series.Chart.Area as IStiAxisArea;
		float dividerY = stiAxisArea.AxisCore.GetDividerY();
		List<PointF?[]> nullablePointsList = StiNullableDrawing.GetNullablePointsList(base.Points);
		List<PointF?[]> list = ((base.PointsFrom != null) ? StiNullableDrawing.GetNullablePointsList(base.PointsFrom) : null);
		if (stiChart.IsAnimation)
		{
			int num;
			for (num = 0; num < nullablePointsList.Count; num++)
			{
				PointF[] array = GetSteppedPoints(nullablePointsList[num]).Cast<PointF>().ToArray();
				List<StiSegmentGeom> list2 = new List<StiSegmentGeom>();
				StiLineSegmentGeom stiLineSegmentGeom = new StiLineSegmentGeom(new PointF(array[0].X, dividerY), array[0]);
				StiLinesSegmentGeom stiLinesSegmentGeom = new StiLinesSegmentGeom(array);
				StiLineSegmentGeom stiLineSegmentGeom2 = new StiLineSegmentGeom(array[^1], new PointF(array[^1].X, dividerY));
				list2.Add(stiLineSegmentGeom);
				list2.Add(stiLinesSegmentGeom);
				list2.Add(stiLineSegmentGeom2);
				PointF[] pointsFrom = GetSteppedPoints(list[num]).Cast<PointF>().ToArray();
				string[] steppedPointsIds = GetSteppedPointsIds(base.PointsIds);
				if (StiPointsAnimation.IsAnimationChangingValues(base.Series, steppedPointsIds))
				{
					StiPointsAnimation stiPointsAnimation = new StiPointsAnimation(pointsFrom, array, steppedPointsIds, StiChartHelper.GlobalDurationElement, StiChartHelper.GlobalBeginTimeElement);
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
					stiLinesSegmentGeom.Animation = stiPointsAnimation;
					stiLineSegmentGeom2.Animation = new StiPointsAnimation(new PointF[2]
					{
						stiPointsAnimation.PointsFrom.LastOrDefault(),
						new PointF(stiPointsAnimation.PointsFrom.LastOrDefault().X, dividerY)
					}, stiPointsAnimation.Duration, stiPointsAnimation.BeginTime)
					{
						BeginTimeCorrect = stiPointsAnimation.BeginTimeCorrect
					};
					context.Animations.Add(stiPointsAnimation);
					context.FillDrawAnimationPath(stiSteppedAreaSeries.Brush, null, list2, StiPathGeom.GetBoundsState, null, null);
					if (stiSteppedAreaSeries.AllowApplyBrushNegative && stiSteppedAreaSeries.BrushNegative != null)
					{
						float width = (float)(stiAxisArea.AxisCore.ScrollRangeX * stiAxisArea.AxisCore.ScrollDpiX);
						float height = (float)(stiAxisArea.AxisCore.ScrollRangeY * stiAxisArea.AxisCore.ScrollDpiY - (double)dividerY);
						RectangleF clipRect = new RectangleF(0f, dividerY, width, height);
						context.PushClip(clipRect);
						context.FillDrawAnimationPath(stiSteppedAreaSeries.BrushNegative, null, list2, StiPathGeom.GetBoundsState, null, null);
						context.PopClip();
					}
				}
				else if (stiSteppedAreaSeries.Brush != null)
				{
					StiOpacityAnimation animation = new StiOpacityAnimation(StiChartHelper.GlobalDurationElement, StiChartHelper.GlobalBeginTimeElement);
					context.FillDrawAnimationPath(stiSteppedAreaSeries.Brush, null, list2, StiPathGeom.GetBoundsState, null, animation);
				}
				num++;
			}
			return;
		}
		foreach (PointF?[] item in nullablePointsList)
		{
			PointF[] array2 = GetSteppedPoints(item).Cast<PointF>().ToArray();
			List<StiSegmentGeom> list3 = new List<StiSegmentGeom>();
			list3.Add(new StiLineSegmentGeom(new PointF(array2[0].X, dividerY), array2[0]));
			list3.Add(new StiLinesSegmentGeom(array2));
			list3.Add(new StiLineSegmentGeom(array2[^1], new PointF(array2[^1].X, dividerY)));
			if (stiSteppedAreaSeries.Brush != null)
			{
				context.FillPath(stiSteppedAreaSeries.Brush, list3, StiPathGeom.GetBoundsState);
			}
			if (stiSteppedAreaSeries.AllowApplyBrushNegative && stiSteppedAreaSeries.BrushNegative != null)
			{
				float width2 = (float)(stiAxisArea.AxisCore.ScrollRangeX * stiAxisArea.AxisCore.ScrollDpiX);
				float height2 = (float)(stiAxisArea.AxisCore.ScrollRangeY * stiAxisArea.AxisCore.ScrollDpiY - (double)dividerY);
				RectangleF clipRect2 = new RectangleF(0f, dividerY, width2, height2);
				context.PushClip(clipRect2);
				context.FillPath(stiSteppedAreaSeries.BrushNegative, list3, StiPathGeom.GetBoundsState);
				context.PopClip();
			}
			if (IsMouseOver || base.Series.Core.IsMouseOver)
			{
				context.FillPath(StiMouseOverHelper.GetMouseOverColor(), list3, StiPathGeom.GetBoundsState);
			}
		}
	}

	public StiSteppedAreaSeriesGeom(StiAreaGeom areaGeom, StiSeriesPointsInfo pointsInfo, IStiSeries series)
		: base(areaGeom, pointsInfo, series)
	{
	}
}
