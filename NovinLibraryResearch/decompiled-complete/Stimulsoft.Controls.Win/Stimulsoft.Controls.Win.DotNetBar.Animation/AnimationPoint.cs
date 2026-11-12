using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Animation;

[ToolboxItem(false)]
internal class AnimationPoint : Animation
{
	public AnimationPoint(AnimationEasing animationEasing, int animationDuration)
		: base(new AnimationRequest[0], animationEasing, animationDuration)
	{
	}

	public AnimationPoint(AnimationRequest animationRequest, AnimationEasing animationEasing, int animationDuration)
		: base(new AnimationRequest[1] { animationRequest }, animationEasing, animationDuration)
	{
	}

	public AnimationPoint(AnimationRequest[] animationRequests, AnimationEasing animationEasing, int animationDuration)
		: base(animationRequests, animationEasing, animationDuration)
	{
	}

	protected override void WorkerDoWork(object sender, DoWorkEventArgs e)
	{
		AnimationRequest[] array = (AnimationRequest[])e.Argument;
		double num = 0.0;
		DateTime now = DateTime.Now;
		double duration = base.Duration;
		bool flag = true;
		while (num <= duration)
		{
			AnimationRequest[] array2 = array;
			foreach (AnimationRequest animationRequest in array2)
			{
				Point point = (Point)animationRequest.To;
				Point point2 = (Point)animationRequest.From;
				Point point3 = new Point(point.X - point2.X, point.Y - point2.Y);
				Point point4 = point;
				if (flag)
				{
					SetTargetPropertyValue(animationRequest.Target, animationRequest.Property, animationRequest.From);
				}
				if (point3.X != 0)
				{
					point4.X = (int)base.AnimationFunctions[base.EasingFunction](num, point2.X, point3.X, duration);
				}
				if (point3.Y != 0)
				{
					point4.Y = (int)base.AnimationFunctions[base.EasingFunction](num, point2.Y, point3.Y, duration);
				}
				SetTargetPropertyValue(animationRequest.Target, animationRequest.Property, point4);
				num = DateTime.Now.Subtract(now).TotalMilliseconds;
				if (e.Cancel)
				{
					return;
				}
			}
			ExecuteStepUpdateMethod();
			if (base.AnimationUpdateControl != null)
			{
				base.AnimationUpdateControl.Update();
			}
			flag = false;
		}
		AnimationRequest[] array3 = array;
		foreach (AnimationRequest animationRequest2 in array3)
		{
			SetTargetPropertyValue(animationRequest2.Target, animationRequest2.Property, animationRequest2.To);
		}
	}

	protected override object GetPropertyValueCorrectType(double value)
	{
		return (int)value;
	}
}
