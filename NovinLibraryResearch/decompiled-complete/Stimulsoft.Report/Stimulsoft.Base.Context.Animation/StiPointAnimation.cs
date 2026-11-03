using System;
using System.Drawing;

namespace Stimulsoft.Base.Context.Animation;

public class StiPointAnimation : StiAnimation
{
	public PointF PointFrom { get; set; }

	public override StiAnimationType Type => (StiAnimationType)5;

	public StiPointAnimation(PointF pointFrom, TimeSpan duration, TimeSpan beginTime)
		: base(duration, beginTime)
	{
		PointFrom = pointFrom;
	}
}
