using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Base.Context.Animation;

public class StiColumnAnimation : StiAnimation
{
	public RectangleF RectFrom { get; set; }

	public RectangleF RectTo { get; set; }

	public override StiAnimationType Type => (StiAnimationType)4;

	public static bool IsAnimationChangingValues(IStiSeries series)
	{
		string id = $"series_{series.Chart.Series.IndexOf(series)}";
		return IsAnimationChangingValues(series, id);
	}

	public static bool IsAnimationChangingValues(IStiSeries series, object id)
	{
		if (series.Chart.IsAnimationChangingValues)
		{
			return GetPreviousAnimation(series.Chart.PreviousAnimations, id) != null;
		}
		return false;
	}

	private static StiColumnAnimation GetPreviousAnimation(List<StiAnimation> previousAnimations, object id)
	{
		if (id == null)
		{
			return null;
		}
		return previousAnimations.Where((StiAnimation a) => a is StiColumnAnimation).Cast<StiColumnAnimation>().FirstOrDefault((StiColumnAnimation a) => id.Equals(a.Id));
	}

	public void ApplyPreviousAnimation(List<StiAnimation> previousAnimations)
	{
		StiColumnAnimation previousAnimation = GetPreviousAnimation(previousAnimations, base.Id);
		if (previousAnimation != null)
		{
			RectFrom = previousAnimation.RectTo;
			base.Duration = TimeSpan.FromMilliseconds(300.0);
		}
		else if (previousAnimations.Count > 0)
		{
			base.BeginTimeCorrect = TimeSpan.FromMilliseconds(200.0);
			base.Duration = TimeSpan.FromMilliseconds(300.0);
		}
	}

	public StiColumnAnimation(RectangleF rectFrom, RectangleF rectTo, TimeSpan duration, TimeSpan beginTime)
		: base(duration, beginTime)
	{
		RectFrom = rectFrom;
		RectTo = rectTo;
	}
}
