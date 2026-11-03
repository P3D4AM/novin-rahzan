using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Base.Context.Animation;

public class StiPieSegmentAnimation : StiAnimation
{
	public RectangleF RectFrom { get; set; }

	public RectangleF RectTo { get; set; }

	public RectangleF RectDtFrom { get; set; }

	public RectangleF RectDtTo { get; set; }

	public float StartAngleFrom { get; set; }

	public float EndAngleFrom { get; set; }

	public float StartAngleTo { get; set; }

	public float EndAngleTo { get; set; }

	public override StiAnimationType Type => (StiAnimationType)6;

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

	private static StiPieSegmentAnimation GetPreviousAnimation(List<StiAnimation> previousAnimations, object id)
	{
		StiPieSegmentAnimation stiPieSegmentAnimation = ((id != null) ? previousAnimations.Where((StiAnimation a) => a is StiPieSegmentAnimation).Cast<StiPieSegmentAnimation>().FirstOrDefault((StiPieSegmentAnimation a) => id.Equals(a.Id)) : null);
		if (stiPieSegmentAnimation == null && id != null)
		{
			stiPieSegmentAnimation = (from a in previousAnimations
				where a != null
				where a.AnotherAnimation != null
				select a).FirstOrDefault((StiAnimation a) => id.Equals(a.Id))?.AnotherAnimation as StiPieSegmentAnimation;
		}
		return stiPieSegmentAnimation;
	}

	public void ApplyPreviousAnimation(List<StiAnimation> previousAnimations)
	{
		StiPieSegmentAnimation previousAnimation = GetPreviousAnimation(previousAnimations, base.Id);
		if (previousAnimation != null)
		{
			StartAngleFrom = previousAnimation.StartAngleTo;
			EndAngleFrom = previousAnimation.EndAngleTo;
			RectFrom = previousAnimation.RectTo;
			RectDtFrom = previousAnimation.RectDtTo;
			base.Duration = TimeSpan.FromMilliseconds(300.0);
		}
		else if (previousAnimations.Count > 0)
		{
			base.BeginTimeCorrect = TimeSpan.FromMilliseconds(200.0);
		}
	}

	public StiPieSegmentAnimation(RectangleF rectFrom, RectangleF rectTo, float startAngleFrom, float endAngleFrom, float startAngleTo, float endAngleTo, TimeSpan duration, TimeSpan beginTime)
		: this(rectFrom, rectTo, RectangleF.Empty, RectangleF.Empty, startAngleFrom, endAngleFrom, startAngleTo, endAngleTo, duration, beginTime)
	{
	}

	public StiPieSegmentAnimation(RectangleF rectFrom, RectangleF rectTo, RectangleF rectDtFrom, RectangleF rectDtTo, float startAngleFrom, float endAngleFrom, float startAngleTo, float endAngleTo, TimeSpan duration, TimeSpan beginTime)
		: base(duration, beginTime)
	{
		RectFrom = rectFrom;
		RectTo = rectTo;
		RectDtFrom = rectDtFrom;
		RectDtTo = rectDtTo;
		StartAngleFrom = startAngleFrom;
		EndAngleFrom = endAngleFrom;
		StartAngleTo = startAngleTo;
		EndAngleTo = endAngleTo;
	}
}
