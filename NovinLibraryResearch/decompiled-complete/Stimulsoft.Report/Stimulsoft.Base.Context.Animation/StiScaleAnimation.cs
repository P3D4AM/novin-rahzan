using System;

namespace Stimulsoft.Base.Context.Animation;

public class StiScaleAnimation : StiAnimation
{
	public double StartScaleX { get; set; }

	public double StartScaleY { get; set; }

	public double EndScaleX { get; set; }

	public double EndScaleY { get; set; }

	public double CenterX { get; set; }

	public double CenterY { get; set; }

	public override StiAnimationType Type => (StiAnimationType)1;

	public StiScaleAnimation(TimeSpan duration, TimeSpan beginTime)
		: this(0.0, 1.0, 0.0, 1.0, double.NaN, double.NaN, duration, beginTime)
	{
	}

	public StiScaleAnimation(double StartScaleX, double EndScaleX, double StartScaleY, double EndScaleY, double centerX, double centerY, TimeSpan duration, TimeSpan beginTime)
		: base(duration, beginTime)
	{
		CenterX = centerX;
		CenterY = centerY;
		this.StartScaleX = StartScaleX;
		this.StartScaleY = StartScaleY;
		this.EndScaleX = EndScaleX;
		this.EndScaleY = EndScaleY;
	}
}
