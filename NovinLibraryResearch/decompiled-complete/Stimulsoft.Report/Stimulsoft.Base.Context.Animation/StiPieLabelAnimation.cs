using System;
using System.Drawing;

namespace Stimulsoft.Base.Context.Animation;

public class StiPieLabelAnimation : StiAnimation
{
	public double? ValueFrom { get; set; }

	public double? Value { get; set; }

	public RectangleF RectLabelFrom { get; set; }

	public RectangleF RectLabel { get; set; }

	public RectangleF ClientRect { get; set; }

	public float AngleFrom { get; set; }

	public float Angle { get; set; }

	public override StiAnimationType Type => (StiAnimationType)2;

	public StiPieLabelAnimation(double? valueFrom, double? value, float angleFrom, float angle, RectangleF clientRect, RectangleF rectLabelFrom, RectangleF rectLabel, TimeSpan duration, TimeSpan beginTime)
		: base(duration, beginTime)
	{
		ValueFrom = valueFrom;
		Value = value;
		ClientRect = clientRect;
		RectLabelFrom = rectLabelFrom;
		RectLabel = rectLabel;
		AngleFrom = angleFrom;
		Angle = angle;
	}
}
