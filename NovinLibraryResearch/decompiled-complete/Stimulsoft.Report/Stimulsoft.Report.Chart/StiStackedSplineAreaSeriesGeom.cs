using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;

namespace Stimulsoft.Report.Chart;

public class StiStackedSplineAreaSeriesGeom : StiSeriesGeom
{
	public virtual bool AllowMouseOver
	{
		get
		{
			if (base.Series.Interaction.DrillDownEnabled)
			{
				return base.Series.Interaction.AllowSeries;
			}
			return false;
		}
	}

	public virtual bool IsMouseOver
	{
		get
		{
			if (base.Series == null)
			{
				return false;
			}
			return base.Series.Core.IsMouseOver;
		}
		set
		{
			if (base.Series != null)
			{
				base.Series.Core.IsMouseOver = value;
			}
		}
	}

	public PointF?[] StartPoints { get; }

	public PointF?[] EndPoints { get; }

	public string[] PointsIds { get; }

	public override void InvokeMouseEnter(StiInteractionOptions options)
	{
		if (AllowMouseOver && !IsMouseOver)
		{
			IsMouseOver = true;
			options.UpdateContext = true;
		}
	}

	public override void InvokeMouseLeave(StiInteractionOptions options)
	{
		if (AllowMouseOver && IsMouseOver)
		{
			IsMouseOver = false;
			options.UpdateContext = true;
		}
	}

	public override bool Contains(float x, float y)
	{
		IStiAxisArea stiAxisArea = base.Series.Chart.Area as IStiAxisArea;
		float dividerY = stiAxisArea.AxisCore.GetDividerY();
		if (Invisible)
		{
			return false;
		}
		for (int i = 0; i < StartPoints.Length - 1; i++)
		{
			PointF? pointF = StartPoints[i];
			PointF? pointF2 = StartPoints[i + 1];
			PointF? pointF3 = EndPoints[i];
			PointF? pointF4 = EndPoints[i + 1];
			if (pointF.HasValue && pointF3.HasValue && pointF4.HasValue && pointF2.HasValue && StiPointHelper.IsPointInPolygon(new PointF(x, y), new PointF[4] { pointF.Value, pointF2.Value, pointF4.Value, pointF3.Value }))
			{
				return true;
			}
		}
		return false;
	}

	internal static RectangleF GetClientRectangle(PointF?[] startPoints, PointF?[] endPoints)
	{
		if (startPoints == null || startPoints.Length == 0 || endPoints == null || endPoints.Length == 0)
		{
			return RectangleF.Empty;
		}
		PointF pointF = PointF.Empty;
		PointF pointF2 = PointF.Empty;
		for (int i = 0; i < startPoints.Length; i++)
		{
			PointF? pointF3 = startPoints[i];
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
		for (int j = 0; j < endPoints.Length; j++)
		{
			PointF? pointF4 = endPoints[j];
			if (pointF4.HasValue)
			{
				if (pointF == PointF.Empty)
				{
					pointF = pointF4.Value;
					pointF2 = pointF4.Value;
					continue;
				}
				pointF.X = Math.Min(pointF.X, pointF4.Value.X);
				pointF.Y = Math.Min(pointF.Y, pointF4.Value.Y);
				pointF2.X = Math.Max(pointF2.X, pointF4.Value.X);
				pointF2.Y = Math.Max(pointF2.Y, pointF4.Value.Y);
			}
		}
		return new RectangleF(pointF.X, pointF.Y, pointF2.X - pointF.X, pointF2.Y - pointF.Y);
	}

	public override void Draw(StiContext context)
	{
		IStiStackedSplineAreaSeries stiStackedSplineAreaSeries = base.Series as IStiStackedSplineAreaSeries;
		bool isAnimation = ((StiChart)stiStackedSplineAreaSeries.Chart).IsAnimation;
		IStiAxisArea stiAxisArea = base.Series.Chart.Area as IStiAxisArea;
		StiNullableDrawing.GetPointsList(StartPoints, EndPoints, out var list, out var list2);
		StiChart stiChart = base.Series.Chart as StiChart;
		int num = 0;
		foreach (PointF[] item in list)
		{
			PointF[] array = list2[num];
			PointF[] points = item.Reverse().ToArray();
			List<StiSegmentGeom> list3 = new List<StiSegmentGeom>();
			StiLineSegmentGeom stiLineSegmentGeom = new StiLineSegmentGeom(item[0], array[0]);
			StiCurveSegmentGeom stiCurveSegmentGeom = new StiCurveSegmentGeom(array, stiStackedSplineAreaSeries.Tension);
			StiLineSegmentGeom stiLineSegmentGeom2 = new StiLineSegmentGeom(array[^1], item[^1]);
			StiCurveSegmentGeom stiCurveSegmentGeom2 = new StiCurveSegmentGeom(points, stiStackedSplineAreaSeries.Tension);
			list3.Add(stiLineSegmentGeom);
			list3.Add(stiCurveSegmentGeom);
			list3.Add(stiLineSegmentGeom2);
			list3.Add(stiCurveSegmentGeom2);
			StiAnimation animation = null;
			if (isAnimation && ((StiChart)stiStackedSplineAreaSeries.Chart).IsAnimationChangingValues)
			{
				StiPointsAnimation stiPointsAnimation = new StiPointsAnimation(array, array, PointsIds, StiChartHelper.GlobalDurationElement, StiChartHelper.GlobalBeginTimeElement);
				stiPointsAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_a";
				StiPointsAnimation stiPointsAnimation2 = new StiPointsAnimation(item, item, PointsIds, stiPointsAnimation.Duration, stiPointsAnimation.BeginTime);
				stiPointsAnimation2.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_a_r";
				context.Animations.Add(stiPointsAnimation);
				context.Animations.Add(stiPointsAnimation2);
				if (StiPointsAnimation.IsAnimationChangingValues(base.Series, PointsIds, stiPointsAnimation.Id))
				{
					stiPointsAnimation.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
					stiPointsAnimation2.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
					stiPointsAnimation2.Reverse();
					stiLineSegmentGeom.Animation = new StiPointsAnimation(new PointF[2]
					{
						stiPointsAnimation2.PointsFrom.LastOrDefault(),
						stiPointsAnimation.PointsFrom.FirstOrDefault()
					}, stiPointsAnimation.Duration, stiPointsAnimation.BeginTime)
					{
						BeginTimeCorrect = stiPointsAnimation.BeginTimeCorrect
					};
					stiCurveSegmentGeom.Animation = stiPointsAnimation;
					stiLineSegmentGeom2.Animation = new StiPointsAnimation(new PointF[2]
					{
						stiPointsAnimation.PointsFrom.LastOrDefault(),
						stiPointsAnimation2.PointsFrom.FirstOrDefault()
					}, stiPointsAnimation.Duration, stiPointsAnimation.BeginTime)
					{
						BeginTimeCorrect = stiPointsAnimation.BeginTimeCorrect
					};
					stiCurveSegmentGeom2.Animation = stiPointsAnimation2;
				}
				else
				{
					animation = new StiOpacityAnimation(StiChartHelper.GlobalDurationElement, StiChartHelper.GlobalBeginTimeElement);
				}
			}
			if (stiStackedSplineAreaSeries.Brush != null)
			{
				if (isAnimation)
				{
					context.FillDrawAnimationPath(stiStackedSplineAreaSeries.Brush, null, list3, StiPathGeom.GetBoundsState, null, animation);
				}
				else
				{
					context.FillPath(stiStackedSplineAreaSeries.Brush, list3, StiPathGeom.GetBoundsState);
				}
			}
			if (stiStackedSplineAreaSeries.AllowApplyBrushNegative && stiStackedSplineAreaSeries.BrushNegative != null)
			{
				float dividerY = stiAxisArea.AxisCore.GetDividerY();
				float width = (float)(stiAxisArea.AxisCore.ScrollRangeX * stiAxisArea.AxisCore.ScrollDpiX);
				float height = (float)(stiAxisArea.AxisCore.ScrollRangeY * stiAxisArea.AxisCore.ScrollDpiY - (double)dividerY);
				RectangleF clipRect = new RectangleF(0f, dividerY, width, height);
				context.PushClip(clipRect);
				if (isAnimation)
				{
					context.FillDrawAnimationPath(stiStackedSplineAreaSeries.BrushNegative, null, list3, StiPathGeom.GetBoundsState, null, animation);
				}
				else
				{
					context.FillPath(stiStackedSplineAreaSeries.BrushNegative, list3, StiPathGeom.GetBoundsState);
				}
				context.PopClip();
			}
			if (IsMouseOver || base.Series.Core.IsMouseOver)
			{
				context.FillPath(StiMouseOverHelper.GetMouseOverColor(), list3, StiPathGeom.GetBoundsState);
			}
			num++;
		}
	}

	public StiStackedSplineAreaSeriesGeom(StiAreaGeom areaGeom, StiSeriesPointsInfo pointsInfo, IStiSeries series)
		: base(areaGeom, series, GetClientRectangle(pointsInfo.Points, pointsInfo.PointsEnd))
	{
		StartPoints = pointsInfo.PointsStart;
		EndPoints = pointsInfo.PointsEnd;
		PointsIds = pointsInfo.PointsIds;
	}
}
