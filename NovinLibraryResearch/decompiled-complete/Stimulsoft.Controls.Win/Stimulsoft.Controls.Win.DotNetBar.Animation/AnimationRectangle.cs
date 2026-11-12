using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Animation;

[ToolboxItem(false)]
internal class AnimationRectangle : Animation
{
	public AnimationRectangle(AnimationEasing animationEasing, int animationDuration)
		: base(new AnimationRequest[0], animationEasing, animationDuration)
	{
	}

	public AnimationRectangle(AnimationRequest animationRequest, AnimationEasing animationEasing, int animationDuration)
		: base(new AnimationRequest[1] { animationRequest }, animationEasing, animationDuration)
	{
	}

	public AnimationRectangle(AnimationRequest[] animationRequests, AnimationEasing animationEasing, int animationDuration)
		: base(animationRequests, animationEasing, animationDuration)
	{
	}

	protected override void WorkerDoWork(object sender, DoWorkEventArgs e)
	{
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
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
				Rectangle rectangle = (Rectangle)animationRequest.To;
				Rectangle rectangle2 = (Rectangle)animationRequest.From;
				Rectangle rectangle3 = new Rectangle(rectangle.X - rectangle2.X, rectangle.Y - rectangle2.Y, rectangle.Width - rectangle2.Width, rectangle.Height - rectangle2.Height);
				Rectangle rectangle4 = rectangle;
				if (IsDisposed(animationRequest.Target))
				{
					return;
				}
				if (flag)
				{
					SetTargetPropertyValue(animationRequest.Target, animationRequest.Property, animationRequest.From);
				}
				if (rectangle3.X != 0)
				{
					rectangle4.X = (int)base.AnimationFunctions[base.EasingFunction](num, rectangle2.X, rectangle3.X, duration);
				}
				if (rectangle3.Y != 0)
				{
					rectangle4.Y = (int)base.AnimationFunctions[base.EasingFunction](num, rectangle2.Y, rectangle3.Y, duration);
				}
				if (rectangle3.Width != 0)
				{
					rectangle4.Width = (int)base.AnimationFunctions[base.EasingFunction](num, rectangle2.Width, rectangle3.Width, duration);
				}
				if (rectangle3.Height != 0)
				{
					rectangle4.Height = (int)base.AnimationFunctions[base.EasingFunction](num, rectangle2.Height, rectangle3.Height, duration);
				}
				SetTargetPropertyValue(animationRequest.Target, animationRequest.Property, rectangle4);
				num = DateTime.Now.Subtract(now).TotalMilliseconds;
				if (e.Cancel)
				{
					return;
				}
			}
			ExecuteStepUpdateMethod();
			if (base.AnimationUpdateControl != null)
			{
				base.AnimationUpdateControl.Invoke((Delegate)(MethodInvoker)delegate
				{
					base.AnimationUpdateControl.Update();
				});
			}
			flag = false;
		}
		AnimationRequest[] array3 = array;
		foreach (AnimationRequest animationRequest2 in array3)
		{
			SetTargetPropertyValue(animationRequest2.Target, animationRequest2.Property, animationRequest2.To);
		}
	}

	private new bool IsDisposed(object p)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (p == null)
		{
			return true;
		}
		if (p is Control && ((Control)p).IsDisposed)
		{
			return true;
		}
		if (p is BaseItem && ((BaseItem)p).IsDisposed)
		{
			return true;
		}
		return false;
	}

	protected override object GetPropertyValueCorrectType(double value)
	{
		return (int)value;
	}
}
