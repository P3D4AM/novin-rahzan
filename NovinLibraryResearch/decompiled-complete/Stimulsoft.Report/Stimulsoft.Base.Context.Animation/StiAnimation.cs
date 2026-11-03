using System;

namespace Stimulsoft.Base.Context.Animation;

public abstract class StiAnimation
{
	public TimeSpan Duration { get; set; }

	public TimeSpan BeginTime { get; set; }

	public TimeSpan BeginTimeCorrect { get; set; }

	public object Id { get; set; }

	public abstract StiAnimationType Type { get; }

	public StiAnimation AnotherAnimation { get; set; }

	public StiAnimation(TimeSpan duration)
		: this(duration, TimeSpan.Zero)
	{
	}

	public StiAnimation(TimeSpan duration, TimeSpan beginTime)
	{
		Duration = duration;
		BeginTime = beginTime;
		BeginTimeCorrect = TimeSpan.Zero;
	}
}
