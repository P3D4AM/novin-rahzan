using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stimulsoft.Base.Context.Animation;

public class StiRotationAnimation : StiAnimation
{
	public double StartAngle { get; set; }

	public double EndAngle { get; set; }

	public PointF CenterPoint { get; set; }

	public override StiAnimationType Type => (StiAnimationType)3;

	private static StiRotationAnimation GetPreviousAnimation(List<StiAnimation> previousAnimations, object id)
	{
		if (id == null)
		{
			return null;
		}
		return previousAnimations.Where((StiAnimation a) => a is StiRotationAnimation).Cast<StiRotationAnimation>().FirstOrDefault((StiRotationAnimation a) => id.Equals(a.Id));
	}

	public void ApplyPreviousAnimation(List<StiAnimation> previousAnimations)
	{
		StiRotationAnimation previousAnimation = GetPreviousAnimation(previousAnimations, base.Id);
		if (previousAnimation != null)
		{
			EndAngle = previousAnimation.StartAngle;
			base.Duration = TimeSpan.FromMilliseconds(300.0);
		}
	}

	public StiRotationAnimation(double startAngle, double endAngle, PointF centerPoint, TimeSpan duration, TimeSpan beginTime)
		: base(duration, beginTime)
	{
		StartAngle = startAngle;
		EndAngle = endAngle;
		CenterPoint = centerPoint;
	}
}
