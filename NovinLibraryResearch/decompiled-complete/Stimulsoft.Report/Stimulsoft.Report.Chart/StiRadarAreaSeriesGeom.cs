using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;

namespace Stimulsoft.Report.Chart;

public class StiRadarAreaSeriesGeom : StiCellGeom
{
	public virtual bool AllowMouseOver
	{
		get
		{
			if (Series.Interaction.DrillDownEnabled)
			{
				return Series.Interaction.AllowSeries;
			}
			return false;
		}
	}

	public virtual bool IsMouseOver
	{
		get
		{
			if (Series == null)
			{
				return false;
			}
			return Series.Core.IsMouseOver;
		}
		set
		{
			if (Series != null)
			{
				Series.Core.IsMouseOver = value;
			}
		}
	}

	public IStiSeries Series { get; }

	public PointF?[] PointsFrom { get; }

	public PointF?[] Points { get; }

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
		if (Invisible)
		{
			return false;
		}
		IStiRadarArea stiRadarArea = Series.Chart.Area as IStiRadarArea;
		StiRadarAreaCoreXF stiRadarAreaCoreXF = stiRadarArea.Core as StiRadarAreaCoreXF;
		for (int i = 0; i < Points.Length; i++)
		{
			PointF? pointF = Points[i];
			PointF? pointF2 = ((i == Points.Length - 1) ? Points[0] : Points[i + 1]);
			PointF centerPoint = stiRadarAreaCoreXF.CenterPoint;
			if (pointF.HasValue && pointF2.HasValue && StiPointHelper.IsPointInTriangle(new PointF(x, y), pointF.Value, centerPoint, pointF2.Value))
			{
				return true;
			}
		}
		return false;
	}

	public override void Draw(StiContext context)
	{
		IStiRadarAreaSeries stiRadarAreaSeries = Series as IStiRadarAreaSeries;
		if (stiRadarAreaSeries.Brush == null)
		{
			return;
		}
		StiChart stiChart = Series.Chart as StiChart;
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		if (stiChart.IsAnimation)
		{
			StiAnimation animation = null;
			StiPointsAnimation stiPointsAnimation = null;
			if (StiPointsAnimation.IsAnimationChangingValues(Series, PointsIds))
			{
				stiPointsAnimation = new StiPointsAnimation(PointsFrom.Cast<PointF>().ToArray(), Points.Cast<PointF>().ToArray(), PointsIds, StiChartHelper.GlobalDurationElement, StiChartHelper.GlobalBeginTimeElement);
				stiPointsAnimation.Id = $"series_{Series.Chart.Series.IndexOf(Series)}";
				stiPointsAnimation.ApplyPreviousAnimation(stiChart.PreviousAnimations);
				context.Animations.Add(stiPointsAnimation);
			}
			else
			{
				animation = new StiOpacityAnimation(StiChartHelper.GlobalDurationElement, StiChartHelper.GlobalBeginTimeElement);
			}
			list.Add(new StiLinesSegmentGeom(Points.Cast<PointF>().ToArray(), stiPointsAnimation));
			context.FillDrawAnimationPath(stiRadarAreaSeries.Brush, null, list, StiPathGeom.GetBoundsState, null, animation);
			return;
		}
		int num = 0;
		PointF?[] points = Points;
		foreach (PointF? pointF in points)
		{
			PointF? pointF2 = pointF;
			PointF? pointF3 = ((num < Points.Length - 1) ? Points[num + 1] : Points[0]);
			list.Add(new StiLineSegmentGeom(pointF2.Value, pointF3.Value));
			num++;
		}
		context.PushSmoothingModeToAntiAlias();
		context.FillPath(stiRadarAreaSeries.Brush, list, RectangleF.Empty);
		if (IsMouseOver || Series.Core.IsMouseOver)
		{
			context.FillPath(StiMouseOverHelper.GetMouseOverColor(), list, RectangleF.Empty);
		}
		context.PopSmoothingMode();
	}

	public StiRadarAreaSeriesGeom(IStiSeries series, StiSeriesPointsInfo pointsInfo)
		: base(RectangleF.Empty)
	{
		Series = series;
		PointsFrom = pointsInfo.PointsFrom;
		Points = pointsInfo.Points;
		PointsIds = pointsInfo.PointsIds;
	}
}
