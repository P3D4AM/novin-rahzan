using System;

namespace Stimulsoft.Base.Context.Animation;

public class StiOpacityAnimation : StiAnimation
{
	public override StiAnimationType Type => (StiAnimationType)0;

	public StiOpacityAnimation(TimeSpan duration, TimeSpan beginTime)
		: base(duration, beginTime)
	{
	}
}
