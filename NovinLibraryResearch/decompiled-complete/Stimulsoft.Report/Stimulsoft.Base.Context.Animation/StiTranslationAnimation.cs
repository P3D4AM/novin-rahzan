using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stimulsoft.Base.Context.Animation;

public class StiTranslationAnimation : StiAnimation
{
	public PointF StartPoint { get; set; }

	public PointF EndPoint { get; set; }

	public override StiAnimationType Type => (StiAnimationType)2;

	private static StiTranslationAnimation GetPreviousAnimation(List<StiAnimation> previousAnimations, object id)
	{
		if (id == null)
		{
			return null;
		}
		return previousAnimations.Where((StiAnimation a) => a is StiTranslationAnimation).Cast<StiTranslationAnimation>().FirstOrDefault((StiTranslationAnimation a) => id.Equals(a.Id));
	}

	public void ApplyPreviousAnimation(List<StiAnimation> previousAnimations)
	{
		StiTranslationAnimation previousAnimation = GetPreviousAnimation(previousAnimations, base.Id);
		if (previousAnimation != null)
		{
			StartPoint = previousAnimation.EndPoint;
			base.Duration = TimeSpan.FromMilliseconds(300.0);
		}
	}

	public StiTranslationAnimation(TimeSpan duration, TimeSpan beginTime)
		: this(PointF.Empty, PointF.Empty, duration, beginTime)
	{
	}

	public StiTranslationAnimation(PointF startPoint, PointF endPoint, TimeSpan duration, TimeSpan beginTime)
		: base(duration, beginTime)
	{
		StartPoint = startPoint;
		EndPoint = endPoint;
	}
}
