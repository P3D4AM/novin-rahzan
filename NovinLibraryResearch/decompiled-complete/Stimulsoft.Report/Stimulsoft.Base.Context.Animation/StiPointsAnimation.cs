using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Base.Context.Animation;

public class StiPointsAnimation : StiAnimation
{
	private bool isReverse;

	public PointF[] PointsFrom { get; set; }

	public PointF[] PointsTo { get; set; }

	public string[] PointsIds { get; set; }

	public override StiAnimationType Type => (StiAnimationType)5;

	public static bool IsAnimationChangingValues(IStiSeries series, string[] pointsIds)
	{
		string id = $"series_{series.Chart.Series.IndexOf(series)}";
		return IsAnimationChangingValues(series, pointsIds, id);
	}

	public static bool IsAnimationChangingValues(IStiSeries series, string[] pointsIds, object id)
	{
		if (series.Chart.IsAnimationChangingValues)
		{
			return GetPreviousAnimation(series.Chart.PreviousAnimations, pointsIds, id) != null;
		}
		return false;
	}

	private static StiPointsAnimation GetPreviousAnimation(List<StiAnimation> previousAnimations, string[] pointsIds, object id)
	{
		StiPointsAnimation prevAnimation = ((id != null) ? previousAnimations.Where((StiAnimation a) => a is StiPointsAnimation).Cast<StiPointsAnimation>().FirstOrDefault((StiPointsAnimation a) => id.Equals(a.Id)) : null);
		if (prevAnimation == null)
		{
			prevAnimation = ((id != null) ? previousAnimations.Where((StiAnimation a) => a is StiPointsAnimation).Cast<StiPointsAnimation>().FirstOrDefault((StiPointsAnimation a) => id.Equals(a.Id?.ToString() + "_a")) : null);
		}
		if (prevAnimation == null)
		{
			prevAnimation = ((id != null) ? previousAnimations.Where((StiAnimation a) => a is StiPointsAnimation).Cast<StiPointsAnimation>().FirstOrDefault((StiPointsAnimation a) => (id?.ToString() + "_a").Equals(a.Id)) : null);
		}
		if (prevAnimation == null && id != null)
		{
			prevAnimation = (from a in previousAnimations
				where a != null
				where a.AnotherAnimation != null
				select a).FirstOrDefault((StiAnimation a) => id.Equals(a.Id))?.AnotherAnimation as StiPointsAnimation;
		}
		if (prevAnimation != null && pointsIds != null && !pointsIds.Any((string p) => GetPointIndex(prevAnimation.PointsIds.ToList(), p) >= 0))
		{
			return null;
		}
		return prevAnimation;
	}

	private static int GetPointIndex(List<string> pointsIds, string pointId)
	{
		pointsIds = pointsIds.Where((string id) => id != null).ToList();
		int num = pointsIds.FindIndex((string id) => id.Equals(pointId));
		if (num == -1)
		{
			num = pointsIds.FindIndex((string id) => (id + "_l").Equals(pointId));
		}
		if (num == -1)
		{
			num = pointsIds.FindIndex((string id) => (id + "_h").Equals(pointId));
		}
		if (num == -1)
		{
			num = pointsIds.FindIndex((string id) => id.Equals(pointId + "_h"));
		}
		if (num == -1)
		{
			num = pointsIds.FindIndex((string id) => id.Equals(pointId + "_l"));
		}
		return num;
	}

	public void ApplyPreviousAnimation(List<StiAnimation> previousAnimations)
	{
		StiPointsAnimation previousAnimation = GetPreviousAnimation(previousAnimations, PointsIds, base.Id);
		if (previousAnimation != null)
		{
			if (previousAnimation.isReverse)
			{
				previousAnimation.Reverse();
			}
			for (int i = 0; i < PointsFrom.Length; i++)
			{
				string text = PointsIds[i];
				int pointIndex = GetPointIndex(previousAnimation.PointsIds.ToList(), text);
				if (pointIndex >= 0)
				{
					PointF pointF = previousAnimation.PointsTo[pointIndex];
					PointsFrom[i] = pointF;
				}
				else if (i == 0 && PointsFrom.Length != 0)
				{
					PointsFrom[i] = previousAnimation.PointsTo.FirstOrDefault();
				}
				else if (i > 0 && text == PointsIds[0] + "_e")
				{
					PointsFrom[i] = PointsFrom[0];
				}
				else if (i > 0)
				{
					PointsFrom[i] = PointsFrom[i - 1];
				}
			}
			if (previousAnimation.isReverse)
			{
				previousAnimation.Reverse();
			}
			base.Duration = TimeSpan.FromMilliseconds(300.0);
		}
		else if (previousAnimations.Count > 0)
		{
			base.BeginTimeCorrect = TimeSpan.FromMilliseconds(200.0);
		}
	}

	public void Reverse()
	{
		isReverse = true;
		PointsTo = PointsTo.Reverse().ToArray();
		PointsFrom = PointsFrom.Reverse().ToArray();
		PointsIds = PointsIds.Reverse().ToArray();
	}

	public StiPointsAnimation(PointF[] pointsFrom, PointF[] pointsTo, string[] pointsIds, TimeSpan duration, TimeSpan beginTime)
		: this(pointsFrom, duration, beginTime)
	{
		PointsTo = pointsTo?.Clone() as PointF[];
		PointsIds = pointsIds?.Clone() as string[];
	}

	public StiPointsAnimation(PointF[] pointsFrom, TimeSpan duration, TimeSpan beginTime)
		: base(duration, beginTime)
	{
		PointsFrom = pointsFrom.Clone() as PointF[];
	}
}
