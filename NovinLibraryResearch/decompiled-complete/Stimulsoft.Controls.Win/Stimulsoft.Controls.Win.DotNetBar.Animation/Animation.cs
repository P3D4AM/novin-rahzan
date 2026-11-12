using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Animation;

internal abstract class Animation : Component
{
	protected delegate double EasingFunctionDelegate(double t, double b, double c, double d);

	private BackgroundWorker _Worker;

	private AnimationEasing _EasingFunction = AnimationEasing.EaseOutQuad;

	private double _Duration = 300.0;

	private Dictionary<AnimationEasing, EasingFunctionDelegate> _AnimationFunctions = new Dictionary<AnimationEasing, EasingFunctionDelegate>();

	private List<AnimationRequest> _AnimationList = new List<AnimationRequest>();

	private bool _AutoDispose;

	private bool _IsDisposed;

	private bool _IsCompleted;

	private Delegate _StepUpdateMethod;

	private Control _AnimationUpdateControl;

	private int _FixedStepCount;

	public bool AutoDispose
	{
		get
		{
			return _AutoDispose;
		}
		set
		{
			_AutoDispose = value;
		}
	}

	public List<AnimationRequest> Animations => _AnimationList;

	public bool IsDisposed
	{
		get
		{
			return _IsDisposed;
		}
		internal set
		{
			_IsDisposed = value;
		}
	}

	public bool IsCompleted => _IsCompleted;

	public double Duration
	{
		get
		{
			return _Duration;
		}
		internal set
		{
			_Duration = value;
		}
	}

	public AnimationEasing EasingFunction => _EasingFunction;

	protected Dictionary<AnimationEasing, EasingFunctionDelegate> AnimationFunctions => _AnimationFunctions;

	public Control AnimationUpdateControl
	{
		get
		{
			return _AnimationUpdateControl;
		}
		set
		{
			_AnimationUpdateControl = value;
		}
	}

	public int FixedStepCount
	{
		get
		{
			return _FixedStepCount;
		}
		set
		{
			_FixedStepCount = value;
		}
	}

	public event EventHandler AnimationCompleted;

	protected virtual void OnAnimationCompleted(EventArgs e)
	{
		this.AnimationCompleted?.Invoke(this, e);
	}

	public Animation(AnimationEasing animationEasing, int animationDuration)
		: this(new AnimationRequest[0], animationEasing, animationDuration)
	{
	}

	public Animation(AnimationRequest animationRequest, AnimationEasing animationEasing, int animationDuration)
		: this(new AnimationRequest[1] { animationRequest }, animationEasing, animationDuration)
	{
	}

	public Animation(AnimationRequest[] animationRequests, AnimationEasing animationEasing, int animationDuration)
	{
		if (animationRequests != null && animationRequests.Length != 0)
		{
			_AnimationList.AddRange(animationRequests);
		}
		_EasingFunction = animationEasing;
		_Duration = animationDuration;
		InitializeAnimationFunctions();
	}

	private void InitializeAnimationFunctions()
	{
		_AnimationFunctions.Add(AnimationEasing.EaseInBounce, EaseInBounce);
		_AnimationFunctions.Add(AnimationEasing.EaseInCirc, EaseInCirc);
		_AnimationFunctions.Add(AnimationEasing.EaseInCubic, EaseInCubic);
		_AnimationFunctions.Add(AnimationEasing.EaseInElastic, EaseInElastic);
		_AnimationFunctions.Add(AnimationEasing.EaseInExpo, EaseInExpo);
		_AnimationFunctions.Add(AnimationEasing.EaseInOutBounce, EaseInOutBounce);
		_AnimationFunctions.Add(AnimationEasing.EaseInOutCirc, EaseInOutCirc);
		_AnimationFunctions.Add(AnimationEasing.EaseInOutCubic, EaseInOutCubic);
		_AnimationFunctions.Add(AnimationEasing.EaseInOutElastic, EaseInOutElastic);
		_AnimationFunctions.Add(AnimationEasing.EaseInOutExpo, EaseInOutExpo);
		_AnimationFunctions.Add(AnimationEasing.EaseInOutQuad, EaseInOutQuad);
		_AnimationFunctions.Add(AnimationEasing.EaseInOutQuart, EaseInOutQuart);
		_AnimationFunctions.Add(AnimationEasing.EaseInOutQuint, EaseInOutQuint);
		_AnimationFunctions.Add(AnimationEasing.EaseInOutSine, EaseInOutSine);
		_AnimationFunctions.Add(AnimationEasing.EaseInQuad, EaseInQuad);
		_AnimationFunctions.Add(AnimationEasing.EaseInQuart, EaseInQuart);
		_AnimationFunctions.Add(AnimationEasing.EaseInQuint, EaseInQuint);
		_AnimationFunctions.Add(AnimationEasing.EaseInSine, EaseInSine);
		_AnimationFunctions.Add(AnimationEasing.EaseOutBounce, EaseOutBounce);
		_AnimationFunctions.Add(AnimationEasing.EaseOutCirc, EaseOutCirc);
		_AnimationFunctions.Add(AnimationEasing.EaseOutCubic, EaseOutCubic);
		_AnimationFunctions.Add(AnimationEasing.EaseOutElastic, EaseOutElastic);
		_AnimationFunctions.Add(AnimationEasing.EaseOutExpo, EaseOutExpo);
		_AnimationFunctions.Add(AnimationEasing.EaseOutQuad, EaseOutQuad);
		_AnimationFunctions.Add(AnimationEasing.EaseOutQuart, EaseOutQuart);
		_AnimationFunctions.Add(AnimationEasing.EaseOutQuint, EaseOutQuint);
		_AnimationFunctions.Add(AnimationEasing.EaseOutSine, EaseOutSine);
	}

	protected override void Dispose(bool disposing)
	{
		Stop();
		_IsDisposed = true;
		base.Dispose(disposing);
	}

	public void Stop()
	{
		BackgroundWorker worker = _Worker;
		if (worker != null)
		{
			worker.CancelAsync();
			worker.Dispose();
			_Worker = null;
		}
	}

	public void Start()
	{
		Start(_AnimationList.ToArray());
	}

	protected void Start(AnimationRequest[] requests)
	{
		if (_Worker != null)
		{
			throw new InvalidOperationException("Animation is already running animations");
		}
		_IsCompleted = false;
		_Worker = new BackgroundWorker();
		_Worker.WorkerSupportsCancellation = true;
		_Worker.DoWork += WorkerDoWork;
		_Worker.RunWorkerCompleted += RunWorkerCompleted;
		_Worker.RunWorkerAsync(requests);
	}

	private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		BackgroundWorker worker = _Worker;
		_Worker = null;
		worker?.Dispose();
		_IsCompleted = true;
		OnAnimationCompleted(EventArgs.Empty);
		if (_AutoDispose)
		{
			Dispose();
		}
	}

	protected virtual void SetTargetPropertyValue(object target, PropertyInfo property, object value)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		Control c = default(Control);
		ref Control reference = ref c;
		object obj = target;
		reference = (Control)((obj is Control) ? obj : null);
		if (c != null)
		{
			c.Invoke((Delegate)(MethodInvoker)delegate
			{
				property.SetValue(target, value, null);
				if (c.Parent != null)
				{
					c.Parent.Update();
				}
				else
				{
					c.Update();
				}
			});
		}
		else if (target is BaseItem)
		{
			if (_AnimationUpdateControl != null)
			{
				_AnimationUpdateControl.Invoke((Delegate)(MethodInvoker)delegate
				{
					property.SetValue(target, value, null);
					if (value is Rectangle)
					{
						_AnimationUpdateControl.Invalidate();
					}
				});
			}
			else
			{
				property.SetValue(target, value, null);
			}
		}
		else
		{
			property.SetValue(target, value, null);
		}
	}

	protected abstract object GetPropertyValueCorrectType(double value);

	protected virtual void WorkerDoWork(object sender, DoWorkEventArgs e)
	{
		AnimationRequest[] array = (AnimationRequest[])e.Argument;
		double num = 0.0;
		DateTime now = DateTime.Now;
		double duration = _Duration;
		bool flag = true;
		if (_FixedStepCount <= 0)
		{
			while (num <= duration)
			{
				AnimationRequest[] array2 = array;
				foreach (AnimationRequest animationRequest in array2)
				{
					double toValue = animationRequest.GetToValue();
					double fromValue = animationRequest.GetFromValue();
					double num2 = toValue - fromValue;
					if (flag)
					{
						SetTargetPropertyValue(animationRequest.Target, animationRequest.Property, animationRequest.From);
					}
					double num3 = _AnimationFunctions[_EasingFunction](num, fromValue, num2, duration);
					if ((num2 < 0.0 && num3 < toValue) || (num2 > 0.0 && num3 > toValue))
					{
						num3 = toValue;
						e.Cancel = true;
						break;
					}
					if ((num2 < 0.0 && num3 > fromValue) || (num2 > 0.0 && num3 < fromValue))
					{
						num3 = fromValue;
					}
					SetTargetPropertyValue(animationRequest.Target, animationRequest.Property, GetPropertyValueCorrectType(num3));
					num = DateTime.Now.Subtract(now).TotalMilliseconds;
					if (e.Cancel)
					{
						break;
					}
				}
				if (e.Cancel)
				{
					break;
				}
				ExecuteStepUpdateMethod();
				if (_AnimationUpdateControl != null)
				{
					_AnimationUpdateControl.Update();
				}
				flag = false;
			}
		}
		else
		{
			for (int j = 0; j < _FixedStepCount; j++)
			{
				AnimationRequest[] array3 = array;
				foreach (AnimationRequest animationRequest2 in array3)
				{
					double toValue2 = animationRequest2.GetToValue();
					double fromValue2 = animationRequest2.GetFromValue();
					double num4 = toValue2 - fromValue2;
					double num5 = num4 / (double)_FixedStepCount;
					if (flag)
					{
						SetTargetPropertyValue(animationRequest2.Target, animationRequest2.Property, animationRequest2.From);
					}
					double num6 = fromValue2 + num5 * (double)(j + 1);
					if ((num4 < 0.0 && num6 < toValue2) || (num4 > 0.0 && num6 > toValue2))
					{
						num6 = toValue2;
						e.Cancel = true;
						break;
					}
					if ((num4 < 0.0 && num6 > fromValue2) || (num4 > 0.0 && num6 < fromValue2))
					{
						num6 = fromValue2;
					}
					SetTargetPropertyValue(animationRequest2.Target, animationRequest2.Property, GetPropertyValueCorrectType(num6));
					if (e.Cancel)
					{
						break;
					}
					if (_Duration > 0.0)
					{
						Thread.Sleep((int)_Duration);
					}
				}
				if (e.Cancel)
				{
					break;
				}
				ExecuteStepUpdateMethod();
				if (_AnimationUpdateControl != null)
				{
					_AnimationUpdateControl.Update();
				}
				flag = false;
			}
		}
		AnimationRequest[] array4 = array;
		foreach (AnimationRequest animationRequest3 in array4)
		{
			SetTargetPropertyValue(animationRequest3.Target, animationRequest3.Property, animationRequest3.To);
		}
	}

	protected virtual void ExecuteStepUpdateMethod()
	{
		if ((object)_StepUpdateMethod != null)
		{
			_StepUpdateMethod.DynamicInvoke(null);
		}
	}

	public void SetStepUpdateMethod(Delegate method)
	{
		_StepUpdateMethod = method;
	}

	private double EaseInOutQuad(double t, double b, double c, double d)
	{
		if ((t /= d / 2.0) < 1.0)
		{
			return c / 2.0 * t * t + b;
		}
		return (0.0 - c) / 2.0 * ((t -= 1.0) * (t - 2.0) - 1.0) + b;
	}

	private double EaseInQuad(double t, double b, double c, double d)
	{
		return c * (t /= d) * t + b;
	}

	private double EaseOutQuad(double t, double b, double c, double d)
	{
		return (0.0 - c) * (t /= d) * (t - 2.0) + b;
	}

	private double EaseInCubic(double t, double b, double c, double d)
	{
		return c * (t /= d) * t * t + b;
	}

	private double EaseOutCubic(double t, double b, double c, double d)
	{
		return c * ((t = t / d - 1.0) * t * t + 1.0) + b;
	}

	private double EaseInOutCubic(double t, double b, double c, double d)
	{
		if ((t /= d / 2.0) < 1.0)
		{
			return c / 2.0 * t * t * t + b;
		}
		return c / 2.0 * ((t -= 2.0) * t * t + 2.0) + b;
	}

	private double EaseInQuart(double t, double b, double c, double d)
	{
		return c * (t /= d) * t * t * t + b;
	}

	private double EaseOutQuart(double t, double b, double c, double d)
	{
		return (0.0 - c) * ((t = t / d - 1.0) * t * t * t - 1.0) + b;
	}

	private double EaseInOutQuart(double t, double b, double c, double d)
	{
		if ((t /= d / 2.0) < 1.0)
		{
			return c / 2.0 * t * t * t * t + b;
		}
		return (0.0 - c) / 2.0 * ((t -= 2.0) * t * t * t - 2.0) + b;
	}

	private double EaseInQuint(double t, double b, double c, double d)
	{
		return c * (t /= d) * t * t * t * t + b;
	}

	private double EaseOutQuint(double t, double b, double c, double d)
	{
		return c * ((t = t / d - 1.0) * t * t * t * t + 1.0) + b;
	}

	private double EaseInOutQuint(double t, double b, double c, double d)
	{
		if ((t /= d / 2.0) < 1.0)
		{
			return c / 2.0 * t * t * t * t * t + b;
		}
		return c / 2.0 * ((t -= 2.0) * t * t * t * t + 2.0) + b;
	}

	private double EaseInSine(double t, double b, double c, double d)
	{
		return (0.0 - c) * Math.Cos(t / d * (Math.PI / 2.0)) + c + b;
	}

	private double EaseOutSine(double t, double b, double c, double d)
	{
		return c * Math.Sin(t / d * (Math.PI / 2.0)) + b;
	}

	private double EaseInOutSine(double t, double b, double c, double d)
	{
		return (0.0 - c) / 2.0 * (Math.Cos(Math.PI * t / d) - 1.0) + b;
	}

	private double EaseInExpo(double t, double b, double c, double d)
	{
		if (t != 0.0)
		{
			return c * Math.Pow(2.0, 10.0 * (t / d - 1.0)) + b;
		}
		return b;
	}

	private double EaseOutExpo(double t, double b, double c, double d)
	{
		if (t != d)
		{
			return c * (0.0 - Math.Pow(2.0, -10.0 * t / d) + 1.0) + b;
		}
		return b + c;
	}

	private double EaseInOutExpo(double t, double b, double c, double d)
	{
		if (t == 0.0)
		{
			return b;
		}
		if (t == d)
		{
			return b + c;
		}
		if ((t /= d / 2.0) < 1.0)
		{
			return c / 2.0 * Math.Pow(2.0, 10.0 * (t - 1.0)) + b;
		}
		return c / 2.0 * (0.0 - Math.Pow(2.0, -10.0 * (t -= 1.0)) + 2.0) + b;
	}

	private double EaseInCirc(double t, double b, double c, double d)
	{
		return (0.0 - c) * (Math.Sqrt(1.0 - (t /= d) * t) - 1.0) + b;
	}

	private double EaseOutCirc(double t, double b, double c, double d)
	{
		return c * Math.Sqrt(1.0 - (t = t / d - 1.0) * t) + b;
	}

	private double EaseInOutCirc(double t, double b, double c, double d)
	{
		if ((t /= d / 2.0) < 1.0)
		{
			return (0.0 - c) / 2.0 * (Math.Sqrt(1.0 - t * t) - 1.0) + b;
		}
		return c / 2.0 * (Math.Sqrt(1.0 - (t -= 2.0) * t) + 1.0) + b;
	}

	private double EaseInElastic(double t, double b, double c, double d)
	{
		double num = 1.70158;
		double num2 = 0.0;
		double num3 = c;
		if (t == 0.0)
		{
			return b;
		}
		if ((t /= d) == 1.0)
		{
			return b + c;
		}
		if (num2 == 0.0)
		{
			num2 = d * 0.3;
		}
		if (num3 < Math.Abs(c))
		{
			num3 = c;
			num = num2 / 4.0;
		}
		else
		{
			num = num2 / (Math.PI * 2.0) * Math.Asin(c / num3);
		}
		return 0.0 - num3 * Math.Pow(2.0, 10.0 * (t -= 1.0)) * Math.Sin((t * d - num) * (Math.PI * 2.0) / num2) + b;
	}

	private double EaseOutElastic(double t, double b, double c, double d)
	{
		double num = 1.70158;
		double num2 = 0.0;
		double num3 = c;
		if (t == 0.0)
		{
			return b;
		}
		if ((t /= d) == 1.0)
		{
			return b + c;
		}
		if (num2 == 0.0)
		{
			num2 = d * 0.3;
		}
		if (num3 < Math.Abs(c))
		{
			num3 = c;
			num = num2 / 4.0;
		}
		else
		{
			num = num2 / (Math.PI * 2.0) * Math.Asin(c / num3);
		}
		return num3 * Math.Pow(2.0, -10.0 * t) * Math.Sin((t * d - num) * (Math.PI * 2.0) / num2) + c + b;
	}

	private double EaseInOutElastic(double t, double b, double c, double d)
	{
		double num = 1.70158;
		double num2 = 0.0;
		double num3 = c;
		if (t == 0.0)
		{
			return b;
		}
		if ((t /= d / 2.0) == 2.0)
		{
			return b + c;
		}
		if (num2 == 0.0)
		{
			num2 = d * 0.44999999999999996;
		}
		if (num3 < Math.Abs(c))
		{
			num3 = c;
			num = num2 / 4.0;
		}
		else
		{
			num = num2 / (Math.PI * 2.0) * Math.Asin(c / num3);
		}
		if (t < 1.0)
		{
			return -0.5 * (num3 * Math.Pow(2.0, 10.0 * (t -= 1.0)) * Math.Sin((t * d - num) * (Math.PI * 2.0) / num2)) + b;
		}
		return num3 * Math.Pow(2.0, -10.0 * (t -= 1.0)) * Math.Sin((t * d - num) * (Math.PI * 2.0) / num2) * 0.5 + c + b;
	}

	private double EaseInBounce(double t, double b, double c, double d)
	{
		return c - EaseOutBounce(d - t, 0.0, c, d) + b;
	}

	private double EaseOutBounce(double t, double b, double c, double d)
	{
		if ((t /= d) < 0.36363636363636365)
		{
			return c * (7.5625 * t * t) + b;
		}
		if (t < 0.7272727272727273)
		{
			return c * (7.5625 * (t -= 0.5454545454545454) * t + 0.75) + b;
		}
		if (t < 0.9090909090909091)
		{
			return c * (7.5625 * (t -= 0.8181818181818182) * t + 0.9375) + b;
		}
		return c * (7.5625 * (t -= 21.0 / 22.0) * t + 63.0 / 64.0) + b;
	}

	private double EaseInOutBounce(double t, double b, double c, double d)
	{
		if (t < d / 2.0)
		{
			return EaseInBounce(t * 2.0, 0.0, c, d) * 0.5 + b;
		}
		return EaseOutBounce(t * 2.0 - d, 0.0, c, d) * 0.5 + c * 0.5 + b;
	}
}
