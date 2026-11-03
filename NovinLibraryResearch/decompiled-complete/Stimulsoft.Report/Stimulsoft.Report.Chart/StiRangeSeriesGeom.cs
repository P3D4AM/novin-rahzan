using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiRangeSeriesGeom : StiLineSeriesGeom
{
	public PointF?[] PointsEnd { get; }

	public override void Draw(StiContext context)
	{
		IStiLineRangeSeries areaSeries = base.Series as IStiLineRangeSeries;
		List<PointF[]> pointsList = StiNullableDrawing.GetPointsList(base.Points);
		List<PointF[]> pointsList2 = StiNullableDrawing.GetPointsList(PointsEnd);
		if (pointsList.Count < 1 || pointsList2.Count < 1)
		{
			return;
		}
		PointF[] array = pointsList[0];
		PointF[] array2 = pointsList2[0];
		int num = Math.Min(array.Length, array2.Length);
		List<PointF> list = new List<PointF>();
		List<PointF> list2 = new List<PointF>();
		List<string> list3 = new List<string>();
		List<string> list4 = new List<string>();
		for (int i = 0; i < num; i++)
		{
			PointF? point = array[i];
			PointF? pointEnd = array2[i];
			string text = base.PointsIds[i];
			string item = base.PointsIds[i] + "_e";
			PointF? pointNext = null;
			PointF? pointNextEnd = null;
			if (i != num - 1)
			{
				pointNext = array[i + 1];
				pointNextEnd = array2[i + 1];
			}
			PointF? pointF = null;
			if (pointNext.HasValue || Intersection(point, pointEnd, pointNext, pointNextEnd))
			{
				pointF = GetPointCross(point.Value, pointEnd.Value, pointNext.Value, pointNextEnd.Value);
			}
			list.Add(point.Value);
			list2.Add(pointEnd.Value);
			list3.Add(text);
			list4.Add(item);
			if (pointF.HasValue)
			{
				list.Add(pointF.Value);
				list2.Add(pointF.Value);
				list3.Add(text + "_c");
				list4.Add(text + "_c_e");
				FillPath(context, GetBrush(areaSeries, point, pointEnd), list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray());
				list.Clear();
				list2.Clear();
				list3.Clear();
				list4.Clear();
				list.Add(pointF.Value);
				list2.Add(pointF.Value);
				list3.Add(text + "_c");
				list4.Add(text + "_c_e");
			}
			else if (!pointNext.HasValue)
			{
				FillPath(context, GetBrush(areaSeries, point, pointEnd), list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray());
			}
		}
	}

	private bool IsPointsEqual(PointF[] pointsLine, PointF[] pointsLineEnd)
	{
		if (pointsLine.Length == pointsLineEnd.Length)
		{
			for (int i = 0; i < pointsLine.Length; i++)
			{
				if (!pointsLine[i].Equals((object?)pointsLineEnd[i]))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	private StiBrush GetBrush(IStiLineRangeSeries areaSeries, PointF? point, PointF? pointEnd)
	{
		StiBrush result = areaSeries.Brush;
		if (areaSeries.AllowApplyBrushNegative)
		{
			result = ((point.Value.Y < pointEnd.Value.Y) ? areaSeries.BrushNegative : areaSeries.Brush);
		}
		return result;
	}

	private void FillPath(StiContext context, StiBrush brush, PointF[] pointsLine, PointF[] pointsLineEnd, string[] pointsIds, string[] pointsEndIds)
	{
		if (IsPointsEqual(pointsLine, pointsLineEnd))
		{
			return;
		}
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		PointF[] array = pointsLineEnd.Reverse().ToArray();
		StiLineSegmentGeom stiLineSegmentGeom = new StiLineSegmentGeom(array[^1], pointsLine[0]);
		StiLinesSegmentGeom stiLinesSegmentGeom = new StiLinesSegmentGeom(pointsLine);
		StiLineSegmentGeom stiLineSegmentGeom2 = new StiLineSegmentGeom(pointsLine[^1], array[0]);
		StiLinesSegmentGeom stiLinesSegmentGeom2 = new StiLinesSegmentGeom(array);
		list.Add(stiLineSegmentGeom);
		list.Add(stiLinesSegmentGeom);
		list.Add(stiLineSegmentGeom2);
		list.Add(stiLinesSegmentGeom2);
		StiChart stiChart = base.Series.Chart as StiChart;
		if (stiChart.IsAnimation)
		{
			StiAnimation animation = null;
			if (stiChart.IsAnimationChangingValues)
			{
				StiPointsAnimation stiPointsAnimation = new StiPointsAnimation(pointsLine, pointsLine, pointsIds, StiChartHelper.GlobalDurationElement, StiChartHelper.GlobalBeginTimeElement);
				stiPointsAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_a";
				StiPointsAnimation stiPointsAnimation2 = new StiPointsAnimation(pointsLineEnd, pointsLineEnd, pointsEndIds, stiPointsAnimation.Duration, stiPointsAnimation.BeginTime);
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
					stiLinesSegmentGeom.Animation = stiPointsAnimation;
					stiLinesSegmentGeom2.Animation = stiPointsAnimation2;
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

	private bool Intersection(PointF? point, PointF? pointEnd, PointF? pointNext, PointF? pointNextEnd)
	{
		if (!pointNext.HasValue)
		{
			return false;
		}
		if ((point.Value.Y > pointEnd.Value.Y && pointNext.Value.Y < pointNextEnd.Value.Y) || (point.Value.Y < pointEnd.Value.Y && pointNext.Value.Y > pointNextEnd.Value.Y) || pointNext.Value.Y == pointNextEnd.Value.Y)
		{
			return true;
		}
		return false;
	}

	private PointF? GetPointCross(PointF point, PointF pointEnd, PointF pointNext, PointF pointNextEnd)
	{
		if (pointNext == pointNextEnd)
		{
			return pointNext;
		}
		float x = point.X;
		float y = point.Y;
		float x2 = pointNext.X;
		float y2 = pointNext.Y;
		float x3 = pointEnd.X;
		float y3 = pointEnd.Y;
		float x4 = pointNextEnd.X;
		float y4 = pointNextEnd.Y;
		float num = (0f - ((x * y2 - x2 * y) * (x4 - x3) - (x3 * y4 - x4 * y3) * (x2 - x))) / ((y - y2) * (x4 - x3) - (y3 - y4) * (x2 - x));
		float y5 = ((y3 - y4) * (0f - num) - (x3 * y4 - x4 * y3)) / (x4 - x3);
		if (num > x && num < x2)
		{
			return new PointF(num, y5);
		}
		return null;
	}

	public StiRangeSeriesGeom(StiAreaGeom areaGeom, StiSeriesPointsInfo pointsInfo, IStiSeries series)
		: base(areaGeom, pointsInfo, series)
	{
		PointsEnd = pointsInfo.PointsEnd;
	}
}
