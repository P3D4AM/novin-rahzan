using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;

namespace Stimulsoft.Report.Chart;

public class StiBaseLineSeriesGeom : StiSeriesGeom
{
	private string additionalSeriesId;

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

	public PointF?[] PointsFrom { get; }

	public PointF?[] Points { get; }

	public string[] PointsIds { get; set; }

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

	internal static RectangleF GetClientRectangle(PointF?[] points, float lineWidth)
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
		return new RectangleF(pointF.X - lineWidth / 2f, pointF.Y - lineWidth / 2f, pointF2.X - pointF.X + lineWidth, pointF2.Y - pointF.Y + lineWidth);
	}

	protected StiAnimation GetAnimation()
	{
		return GetAnimation(Points);
	}

	protected StiAnimation GetAnimation(PointF?[] points)
	{
		if (!((StiChart)base.Series.Chart).IsAnimation)
		{
			return null;
		}
		if (PointsFrom == null || PointsIds == null)
		{
			return null;
		}
		List<PointF?> list = new List<PointF?>();
		List<PointF?> list2 = new List<PointF?>();
		List<string> list3 = new List<string>();
		for (int i = 0; i < PointsFrom.Length; i++)
		{
			if (PointsFrom[i].HasValue && Points[i].HasValue && PointsIds[i] != null)
			{
				list.Add(PointsFrom[i].Value);
				list2.Add(Points[i].Value);
				list3.Add(PointsIds[i]);
			}
		}
		return GetAnimation(list.ToArray(), list2.ToArray(), list3.ToArray());
	}

	protected StiAnimation GetAnimationConnect(PointF?[] points)
	{
		if (!((StiChart)base.Series.Chart).IsAnimation)
		{
			return null;
		}
		if (PointsFrom == null || PointsIds == null)
		{
			return null;
		}
		List<PointF?> list = new List<PointF?>();
		List<PointF?> list2 = new List<PointF?>();
		List<string> list3 = new List<string>();
		int num = 0;
		for (int i = 0; i < points.Length; i++)
		{
			PointF? point = points[i];
			if (point.HasValue && !list2.Exists((PointF? p) => p.Equals(point.Value)))
			{
				PointF? item = PointsFrom[num];
				if (!item.HasValue)
				{
					item = point;
				}
				string text = PointsIds[num];
				if (string.IsNullOrEmpty(text))
				{
					text = num + "_";
				}
				list.Add(item);
				list2.Add(point.Value);
				list3.Add(text);
				num++;
			}
		}
		return GetAnimation(list.ToArray(), list2.ToArray(), list3.ToArray());
	}

	protected StiAnimation GetAnimation(PointF?[] pointsFrom, PointF?[] points, string[] pointsIds)
	{
		if (!((StiChart)base.Series.Chart).IsAnimation)
		{
			return null;
		}
		if (pointsFrom.Length == 0)
		{
			return null;
		}
		StiAnimation stiAnimation = null;
		StiPointsAnimation stiPointsAnimation = null;
		if (((StiChart)base.Series.Chart).IsAnimationChangingValues && pointsFrom != null && pointsFrom.First().HasValue && pointsIds != null)
		{
			stiPointsAnimation = new StiPointsAnimation(pointsFrom.Cast<PointF>().ToArray(), points.Cast<PointF>().ToArray(), pointsIds, StiChartHelper.GlobalDurationElement, TimeSpan.Zero);
			stiPointsAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}" + additionalSeriesId;
			if (StiPointsAnimation.IsAnimationChangingValues(base.Series, pointsIds, stiPointsAnimation.Id))
			{
				stiPointsAnimation.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
				stiAnimation = stiPointsAnimation;
			}
		}
		if (stiAnimation == null)
		{
			StiTranslationAnimation stiTranslationAnimation = new StiTranslationAnimation(StiChartHelper.GlobalDurationElement, TimeSpan.Zero);
			stiTranslationAnimation.AnotherAnimation = stiPointsAnimation;
			stiTranslationAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}" + additionalSeriesId;
			stiAnimation = stiTranslationAnimation;
		}
		return stiAnimation;
	}

	public override void Draw(StiContext context)
	{
	}

	public StiBaseLineSeriesGeom(StiAreaGeom areaGeom, StiSeriesPointsInfo pointsInfo, IStiSeries series)
		: this(areaGeom, pointsInfo.PointsFrom, pointsInfo.Points, series)
	{
		PointsIds = pointsInfo.PointsIds;
		additionalSeriesId = pointsInfo.AdditionalSeriesId;
	}

	public StiBaseLineSeriesGeom(StiAreaGeom areaGeom, PointF?[] pointsFrom, PointF?[] points, IStiSeries series)
		: base(areaGeom, series, GetClientRectangle(points, (series is StiBaseLineSeries) ? ((StiBaseLineSeries)series).LineWidth : 0f))
	{
		Points = points;
		if (pointsFrom == null || !pointsFrom.FirstOrDefault().HasValue)
		{
			pointsFrom = (PointF?[])points.Clone();
		}
		PointsFrom = pointsFrom;
	}
}
