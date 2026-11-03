using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar.Animation;

internal abstract class _1A18 : Component
{
	protected delegate double EasingFunctionDelegate(double t, double b, double c, double d);

	private EventHandler ચ;

	private BackgroundWorker __1A19;

	private AnimationEasing __1A1A = AnimationEasing.EaseOutQuad;

	private double __1A1B = 300.0;

	private EasingFunctionDelegate[] __1A1C;

	private List<ခ> __1A1D = new List<ခ>();

	private bool __1A1E;

	private bool _ᗞ;

	private bool __1A1F;

	private Delegate _ᨠ;

	private Control _ᨡ;

	private int _ᨢ;

	public bool AutoDispose
	{
		get
		{
			return __1A1E;
		}
		set
		{
			__1A1E = value;
		}
	}

	public List<ခ> Animations => __1A1D;

	public bool IsDisposed
	{
		get
		{
			return _ᗞ;
		}
		internal set
		{
			_ᗞ = value;
		}
	}

	public bool IsCompleted => __1A1F;

	public double Duration
	{
		get
		{
			return __1A1B;
		}
		internal set
		{
			__1A1B = value;
		}
	}

	public AnimationEasing EasingFunction => __1A1A;

	protected EasingFunctionDelegate[] AnimationFunctions => __1A1C;

	public Control AnimationUpdateControl
	{
		get
		{
			return _ᨡ;
		}
		set
		{
			_ᨡ = value;
		}
	}

	public int FixedStepCount
	{
		get
		{
			return _ᨢ;
		}
		set
		{
			_ᨢ = value;
		}
	}

	public event EventHandler AnimationCompleted
	{
		add
		{
			EventHandler eventHandler = ચ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ચ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ચ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ચ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnAnimationCompleted(EventArgs e)
	{
		ચ?.Invoke(this, e);
	}

	public _1A18(AnimationEasing animationEasing, int animationDuration)
		: this(new ခ[0], animationEasing, animationDuration)
	{
	}

	public _1A18(ခ animationRequest, AnimationEasing animationEasing, int animationDuration)
		: this(new ခ[1] { animationRequest }, animationEasing, animationDuration)
	{
	}

	public _1A18(ခ[] animationRequests, AnimationEasing animationEasing, int animationDuration)
	{
		if (animationRequests != null && animationRequests.Length > 0)
		{
			__1A1D.AddRange(animationRequests);
		}
		__1A1A = animationEasing;
		__1A1B = animationDuration;
		InitializeAnimationFunctions();
	}

	private void InitializeAnimationFunctions()
	{
		__1A1C = new EasingFunctionDelegate[28];
		__1A1C[24] = EaseInBounce;
		__1A1C[18] = EaseInCirc;
		__1A1C[3] = EaseInCubic;
		__1A1C[21] = EaseInElastic;
		__1A1C[15] = EaseInExpo;
		__1A1C[26] = EaseInOutBounce;
		__1A1C[20] = EaseInOutCirc;
		__1A1C[5] = EaseInOutCubic;
		__1A1C[23] = EaseInOutElastic;
		__1A1C[17] = EaseInOutExpo;
		__1A1C[0] = EaseInOutQuad;
		__1A1C[8] = EaseInOutQuart;
		__1A1C[11] = EaseInOutQuint;
		__1A1C[14] = EaseInOutSine;
		__1A1C[1] = EaseInQuad;
		__1A1C[6] = EaseInQuart;
		__1A1C[9] = EaseInQuint;
		__1A1C[12] = EaseInSine;
		__1A1C[25] = EaseOutBounce;
		__1A1C[19] = EaseOutCirc;
		__1A1C[4] = EaseOutCubic;
		__1A1C[22] = EaseOutElastic;
		__1A1C[16] = EaseOutExpo;
		__1A1C[2] = EaseOutQuad;
		__1A1C[7] = EaseOutQuart;
		__1A1C[10] = EaseOutQuint;
		__1A1C[13] = EaseOutSine;
		__1A1C[27] = Linear;
	}

	protected override void Dispose(bool disposing)
	{
		Stop();
		_ᗞ = true;
		base.Dispose(disposing);
	}

	public void Stop()
	{
		BackgroundWorker backgroundWorker = __1A19;
		if (backgroundWorker != null)
		{
			backgroundWorker.DoWork -= WorkerDoWork;
			backgroundWorker.RunWorkerCompleted -= RunWorkerCompleted;
			backgroundWorker.CancelAsync();
			backgroundWorker.Dispose();
			__1A19 = null;
		}
	}

	public void Start()
	{
		Start(__1A1D.ToArray());
	}

	protected void Start(ခ[] requests)
	{
		if (__1A19 != null)
		{
			throw new InvalidOperationException("Animation is already running animations");
		}
		__1A1F = false;
		__1A19 = new BackgroundWorker();
		__1A19.WorkerSupportsCancellation = true;
		__1A19.DoWork += WorkerDoWork;
		__1A19.RunWorkerCompleted += RunWorkerCompleted;
		__1A19.RunWorkerAsync(requests);
	}

	private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		BackgroundWorker backgroundWorker = __1A19;
		__1A19 = null;
		if (backgroundWorker != null)
		{
			backgroundWorker.DoWork -= WorkerDoWork;
			backgroundWorker.RunWorkerCompleted -= RunWorkerCompleted;
			backgroundWorker.Dispose();
		}
		__1A1F = true;
		OnAnimationCompleted(EventArgs.Empty);
		if (__1A1E)
		{
			Dispose();
		}
	}

	protected virtual void SetTargetPropertyValue(object target, PropertyInfo property, object value)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		MethodInvoker val = null;
		MethodInvoker val2 = null;
		Control c = default(Control);
		ref Control reference = ref c;
		object obj = target;
		reference = (Control)((obj is Control) ? obj : null);
		if (c != null)
		{
			Control obj2 = c;
			if (val == null)
			{
				val = (MethodInvoker)delegate
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
				};
			}
			obj2.Invoke((Delegate)(object)val);
		}
		else if (target is BaseItem)
		{
			if (_ᨡ != null)
			{
				Control ᨡ = _ᨡ;
				if (val2 == null)
				{
					val2 = (MethodInvoker)delegate
					{
						property.SetValue(target, value, null);
						if (value is Rectangle)
						{
							_ᨡ.Invalidate();
						}
					};
				}
				ᨡ.Invoke((Delegate)(object)val2);
			}
			else if (target is MetroTileItem && property.Name == "CurrentFrameOffset")
			{
				((MetroTileItem)target).CurrentFrameOffset = (int)value;
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
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		MethodInvoker val = null;
		ခ[] array = (ခ[])e.Argument;
		double num = 0.0;
		DateTime utcNow = DateTime.UtcNow;
		double num2 = __1A1B;
		bool flag = true;
		if (_ᨢ <= 0)
		{
			while (num <= num2)
			{
				try
				{
					ခ[] array2 = array;
					foreach (ခ ခ2 in array2)
					{
						double num3 = ခ2.ဍ();
						double num4 = ခ2.ဎ();
						double num5 = num3 - num4;
						if (flag)
						{
							SetTargetPropertyValue(ခ2.ဂ, ခ2.စ, ခ2.ဃ);
						}
						double num6 = __1A1C[(int)__1A1A](num, num4, num5, num2);
						if ((num5 < 0.0 && num6 < num3) || (num5 > 0.0 && num6 > num3))
						{
							num6 = num3;
							e.Cancel = true;
							break;
						}
						if ((num5 < 0.0 && num6 > num4) || (num5 > 0.0 && num6 < num4))
						{
							num6 = num4;
						}
						SetTargetPropertyValue(ခ2.ဂ, ခ2.စ, GetPropertyValueCorrectType(num6));
						num = DateTime.UtcNow.Subtract(utcNow).TotalMilliseconds;
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
					if (_ᨡ != null)
					{
						if (_ᨡ.InvokeRequired)
						{
							Control ᨡ = _ᨡ;
							if (val == null)
							{
								val = (MethodInvoker)delegate
								{
									_ᨡ.Update();
								};
							}
							ᨡ.BeginInvoke((Delegate)(object)val);
						}
						else
						{
							_ᨡ.Update();
						}
					}
					if (e.Cancel)
					{
						break;
					}
					flag = false;
					continue;
				}
				catch (TargetInvocationException ex)
				{
					if (ex.InnerException is ObjectDisposedException)
					{
						return;
					}
					throw;
				}
			}
		}
		else
		{
			try
			{
				for (int j = 0; j < _ᨢ; j++)
				{
					ခ[] array3 = array;
					foreach (ခ ခ3 in array3)
					{
						double num7 = ခ3.ဍ();
						double num8 = ခ3.ဎ();
						double num9 = num7 - num8;
						double num10 = num9 / (double)_ᨢ;
						if (flag)
						{
							SetTargetPropertyValue(ခ3.ဂ, ခ3.စ, ခ3.ဃ);
						}
						double num11 = num8 + num10 * (double)(j + 1);
						if ((num9 < 0.0 && num11 < num7) || (num9 > 0.0 && num11 > num7))
						{
							num11 = num7;
							e.Cancel = true;
							break;
						}
						if ((num9 < 0.0 && num11 > num8) || (num9 > 0.0 && num11 < num8))
						{
							num11 = num8;
						}
						SetTargetPropertyValue(ခ3.ဂ, ခ3.စ, GetPropertyValueCorrectType(num11));
						if (e.Cancel)
						{
							break;
						}
						if (!(__1A1B > 0.0))
						{
							continue;
						}
						try
						{
							using ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
							manualResetEvent.WaitOne((int)__1A1B);
						}
						catch
						{
							break;
						}
					}
					if (!e.Cancel)
					{
						ExecuteStepUpdateMethod();
						if (_ᨡ != null)
						{
							_ᨡ.Update();
						}
						if (!e.Cancel)
						{
							flag = false;
							continue;
						}
						break;
					}
					break;
				}
			}
			catch (TargetInvocationException ex2)
			{
				if (ex2.InnerException is ObjectDisposedException)
				{
					return;
				}
				throw;
			}
		}
		ခ[] array4 = array;
		foreach (ခ ခ4 in array4)
		{
			try
			{
				SetTargetPropertyValue(ခ4.ဂ, ခ4.စ, ခ4.င);
			}
			catch (TargetInvocationException ex3)
			{
				if (ex3.InnerException is ObjectDisposedException)
				{
					continue;
				}
				throw;
			}
		}
	}

	protected virtual void ExecuteStepUpdateMethod()
	{
		if ((object)_ᨠ != null)
		{
			_ᨠ.DynamicInvoke(null);
		}
	}

	public void SetStepUpdateMethod(Delegate method)
	{
		_ᨠ = method;
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

	private double Linear(double t, double b, double c, double d)
	{
		return t / d * c + b;
	}
}
internal class _1A6A : _1A18
{
	public _1A6A(AnimationEasing animationEasing, int animationDuration)
		: base(new ခ[0], animationEasing, animationDuration)
	{
	}

	public _1A6A(ခ animationRequest, AnimationEasing animationEasing, int animationDuration)
		: base(new ခ[1] { animationRequest }, animationEasing, animationDuration)
	{
	}

	public _1A6A(ခ[] animationRequests, AnimationEasing animationEasing, int animationDuration)
		: base(animationRequests, animationEasing, animationDuration)
	{
	}

	protected override object GetPropertyValueCorrectType(double value)
	{
		return (int)value;
	}
}
internal class _20EE : _1A18
{
	public _20EE(AnimationEasing animationEasing, int animationDuration)
		: base(new ခ[0], animationEasing, animationDuration)
	{
	}

	public _20EE(ခ animationRequest, AnimationEasing animationEasing, int animationDuration)
		: base(new ခ[1] { animationRequest }, animationEasing, animationDuration)
	{
	}

	public _20EE(ခ[] animationRequests, AnimationEasing animationEasing, int animationDuration)
		: base(animationRequests, animationEasing, animationDuration)
	{
	}

	protected override void WorkerDoWork(object sender, DoWorkEventArgs e)
	{
		ခ[] array = (ခ[])e.Argument;
		double num = 0.0;
		DateTime now = DateTime.Now;
		double duration = base.Duration;
		bool flag = true;
		while (num <= duration)
		{
			ခ[] array2 = array;
			foreach (ခ ခ2 in array2)
			{
				Rectangle rectangle = (Rectangle)ခ2.င;
				Rectangle rectangle2 = (Rectangle)ခ2.ဃ;
				Rectangle rectangle3 = new Rectangle(rectangle.X - rectangle2.X, rectangle.Y - rectangle2.Y, rectangle.Width - rectangle2.Width, rectangle.Height - rectangle2.Height);
				Rectangle rectangle4 = rectangle;
				if (ᗘ(ခ2.ဂ))
				{
					return;
				}
				if (flag)
				{
					SetTargetPropertyValue(ခ2.ဂ, ခ2.စ, ခ2.ဃ);
				}
				if (rectangle3.X != 0)
				{
					rectangle4.X = (int)base.AnimationFunctions[(int)base.EasingFunction](num, rectangle2.X, rectangle3.X, duration);
				}
				if (rectangle3.Y != 0)
				{
					rectangle4.Y = (int)base.AnimationFunctions[(int)base.EasingFunction](num, rectangle2.Y, rectangle3.Y, duration);
				}
				if (rectangle3.Width != 0)
				{
					rectangle4.Width = (int)base.AnimationFunctions[(int)base.EasingFunction](num, rectangle2.Width, rectangle3.Width, duration);
				}
				if (rectangle3.Height != 0)
				{
					rectangle4.Height = (int)base.AnimationFunctions[(int)base.EasingFunction](num, rectangle2.Height, rectangle3.Height, duration);
				}
				SetTargetPropertyValue(ခ2.ဂ, ခ2.စ, rectangle4);
				num = DateTime.Now.Subtract(now).TotalMilliseconds;
				if (e.Cancel)
				{
					return;
				}
			}
			ExecuteStepUpdateMethod();
			flag = false;
		}
		ခ[] array3 = array;
		foreach (ခ ခ3 in array3)
		{
			SetTargetPropertyValue(ခ3.ဂ, ခ3.စ, ခ3.င);
		}
	}

	private bool ᗘ(object _065C)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (_065C == null)
		{
			return true;
		}
		if (_065C is Control && ((Control)_065C).IsDisposed)
		{
			return true;
		}
		if (_065C is BaseItem && ((BaseItem)_065C).IsDisposed)
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
internal class _2488 : Component
{
	private bool _ᗞ;

	private List<_1A18> __2489 = new List<_1A18>();

	private BackgroundWorker __1A19;

	private int __248A;

	private bool __1A1E;

	public bool IsDisposed => _ᗞ;

	public List<_1A18> Animations => __2489;

	public int RepeatStoryTimes
	{
		get
		{
			return __248A;
		}
		set
		{
			__248A = value;
		}
	}

	public bool AutoDispose
	{
		get
		{
			return __1A1E;
		}
		set
		{
			__1A1E = value;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			BackgroundWorker backgroundWorker = __1A19;
			if (backgroundWorker != null)
			{
				backgroundWorker.CancelAsync();
				backgroundWorker.Dispose();
				__1A19 = null;
			}
		}
		_ᗞ = true;
		base.Dispose(disposing);
	}

	public void Run()
	{
		if (__1A19 != null)
		{
			throw new InvalidOperationException("Storyline is already running animations");
		}
		__1A19 = new BackgroundWorker();
		__1A19.WorkerReportsProgress = true;
		__1A19.WorkerSupportsCancellation = true;
		__1A19.DoWork += WorkerDoWork;
		__1A19.RunWorkerCompleted += RunWorkerCompleted;
		__1A19.RunWorkerAsync();
	}

	private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (__248A > 0)
		{
			if (__1A19 == null)
			{
				__248A = 0;
				return;
			}
			__248A--;
			__1A19.RunWorkerAsync();
			return;
		}
		if (__1A19 != null)
		{
			__1A19.Dispose();
		}
		__1A19 = null;
		if (__1A1E && !_ᗞ)
		{
			Dispose();
		}
	}

	private void WorkerDoWork(object sender, DoWorkEventArgs e)
	{
		int count = __2489.Count;
		for (int i = 0; i < count; i++)
		{
			if (e.Cancel)
			{
				break;
			}
			__2489[i].Start();
			while (!__2489[i].IsCompleted)
			{
				try
				{
					using ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
					manualResetEvent.WaitOne(50);
				}
				catch
				{
					__2489[i].Stop();
					return;
				}
				if (e.Cancel)
				{
					__2489[i].Stop();
					return;
				}
			}
		}
	}
}
internal class _2A7B : _1A18
{
	public _2A7B(AnimationEasing animationEasing, int animationDuration)
		: base(new ခ[0], animationEasing, animationDuration)
	{
	}

	public _2A7B(ခ animationRequest, AnimationEasing animationEasing, int animationDuration)
		: base(new ခ[1] { animationRequest }, animationEasing, animationDuration)
	{
	}

	public _2A7B(ခ[] animationRequests, AnimationEasing animationEasing, int animationDuration)
		: base(animationRequests, animationEasing, animationDuration)
	{
	}

	protected override object GetPropertyValueCorrectType(double value)
	{
		return value;
	}
}
internal class _2A7C : _1A18
{
	public _2A7C(AnimationEasing animationEasing, int animationDuration)
		: base(new ခ[0], animationEasing, animationDuration)
	{
	}

	public _2A7C(ခ animationRequest, AnimationEasing animationEasing, int animationDuration)
		: base(new ခ[1] { animationRequest }, animationEasing, animationDuration)
	{
	}

	public _2A7C(ခ[] animationRequests, AnimationEasing animationEasing, int animationDuration)
		: base(animationRequests, animationEasing, animationDuration)
	{
	}

	protected override void WorkerDoWork(object sender, DoWorkEventArgs e)
	{
		ခ[] array = (ခ[])e.Argument;
		double num = 0.0;
		DateTime now = DateTime.Now;
		double duration = base.Duration;
		bool flag = true;
		while (num <= duration)
		{
			ခ[] array2 = array;
			foreach (ခ ခ2 in array2)
			{
				Point point = (Point)ခ2.င;
				Point point2 = (Point)ခ2.ဃ;
				Point point3 = new Point(point.X - point2.X, point.Y - point2.Y);
				Point point4 = point;
				if (flag)
				{
					SetTargetPropertyValue(ခ2.ဂ, ခ2.စ, ခ2.ဃ);
				}
				if (point3.X != 0)
				{
					point4.X = (int)base.AnimationFunctions[(int)base.EasingFunction](num, point2.X, point3.X, duration);
				}
				if (point3.Y != 0)
				{
					point4.Y = (int)base.AnimationFunctions[(int)base.EasingFunction](num, point2.Y, point3.Y, duration);
				}
				SetTargetPropertyValue(ခ2.ဂ, ခ2.စ, point4);
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
		ခ[] array3 = array;
		foreach (ခ ခ3 in array3)
		{
			SetTargetPropertyValue(ခ3.ဂ, ခ3.စ, ခ3.င);
		}
	}

	protected override object GetPropertyValueCorrectType(double value)
	{
		return (int)value;
	}
}
