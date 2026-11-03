using System;
using System.Drawing;

namespace Stimulsoft.Base.Context.Animation;

public class StiLabelAnimation : StiAnimation
{
	public PointF PointFrom { get; set; }

	public PointF Point { get; set; }

	public double? ValueFrom { get; set; }

	public double? Value { get; set; }

	public RectangleF LabelRect { get; set; }

	public override StiAnimationType Type => (StiAnimationType)2;

	public StiLabelAnimation(TimeSpan duration, TimeSpan beginTime)
		: this(null, null, PointF.Empty, PointF.Empty, duration, beginTime)
	{
	}

	public StiLabelAnimation(double? valueFrom, double? value, PointF pointFrom, PointF point, TimeSpan duration, TimeSpan beginTime)
		: base(duration, beginTime)
	{
		PointFrom = pointFrom;
		Point = point;
		ValueFrom = valueFrom;
		Value = value;
	}
}
