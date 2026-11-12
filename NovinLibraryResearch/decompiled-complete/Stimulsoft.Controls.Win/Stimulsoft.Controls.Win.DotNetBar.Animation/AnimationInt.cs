using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar.Animation;

[ToolboxItem(false)]
internal class AnimationInt : Animation
{
	public AnimationInt(AnimationEasing animationEasing, int animationDuration)
		: base(new AnimationRequest[0], animationEasing, animationDuration)
	{
	}

	public AnimationInt(AnimationRequest animationRequest, AnimationEasing animationEasing, int animationDuration)
		: base(new AnimationRequest[1] { animationRequest }, animationEasing, animationDuration)
	{
	}

	public AnimationInt(AnimationRequest[] animationRequests, AnimationEasing animationEasing, int animationDuration)
		: base(animationRequests, animationEasing, animationDuration)
	{
	}

	protected override object GetPropertyValueCorrectType(double value)
	{
		return (int)value;
	}
}
