using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar.Animation;

[ToolboxItem(false)]
internal class AnimationDouble : Animation
{
	public AnimationDouble(AnimationEasing animationEasing, int animationDuration)
		: base(new AnimationRequest[0], animationEasing, animationDuration)
	{
	}

	public AnimationDouble(AnimationRequest animationRequest, AnimationEasing animationEasing, int animationDuration)
		: base(new AnimationRequest[1] { animationRequest }, animationEasing, animationDuration)
	{
	}

	public AnimationDouble(AnimationRequest[] animationRequests, AnimationEasing animationEasing, int animationDuration)
		: base(animationRequests, animationEasing, animationDuration)
	{
	}

	protected override object GetPropertyValueCorrectType(double value)
	{
		return value;
	}
}
