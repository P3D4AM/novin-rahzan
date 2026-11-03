using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Touch;

public class TouchHandler
{
	private static class _1AFC
	{
		public static readonly uint _1AA0 = _1ABD(1u, 0u);

		public static readonly uint _1AA1 = _1ABD(2u, 0u);

		public static readonly uint _1A9D = _1ABD(4u, 1u);

		public static readonly uint _1A9F = _1ABD(4u, 0u);

		public static readonly uint _1A9E = _1ABD(4u, 4u);

		public static readonly uint _1AA2 = _1ABD(7u, 0u);

		public static readonly uint _1AA3 = _1ABD(5u, 1u);

		public static readonly uint _1AA4 = _1ABD(5u, 0u);

		public static readonly uint _1AA5 = _1ABD(5u, 4u);

		public static readonly uint _1AA6 = _1ABD(6u, 0u);

		public static readonly uint ᪧ = _1ABD(3u, 1u);

		public static readonly uint _1AA8 = _1ABD(3u, 0u);

		public static readonly uint _1AA9 = _1ABD(3u, 4u);
	}

	private EventHandler<GestureEventArgs> _1A9D;

	private EventHandler<GestureEventArgs> _1A9E;

	private EventHandler<GestureEventArgs> _1A9F;

	private EventHandler<GestureEventArgs> _1AA0;

	private EventHandler<GestureEventArgs> _1AA1;

	private EventHandler<GestureEventArgs> _1AA2;

	private EventHandler<GestureEventArgs> _1AA3;

	private EventHandler<GestureEventArgs> _1AA4;

	private EventHandler<GestureEventArgs> _1AA5;

	private EventHandler<GestureEventArgs> _1AA6;

	private EventHandler<GestureEventArgs> ᪧ;

	private EventHandler<GestureEventArgs> _1AA8;

	private EventHandler<GestureEventArgs> _1AA9;

	private EventHandler<TouchEventArgs> _1AAA;

	private EventHandler<TouchEventArgs> _1AAB;

	private EventHandler<TouchEventArgs> _1AAC;

	private Control __1AAD;

	private eTouchHandlerType __1AAE = eTouchHandlerType.Gesture;

	private IntPtr __1AAF;

	private _0EFE.WindowProcDelegate __1AB0;

	private GestureEventArgs __1AB1;

	private GestureEventArgs __1AB2;

	private bool __1AB3;

	private float __1AB4;

	private float __1AB5;

	private static bool __1AB6 = true;

	public Control ParentControl => __1AAD;

	internal GestureEventArgs _1A9B
	{
		get
		{
			return __1AB1;
		}
		set
		{
			__1AB1 = value;
		}
	}

	internal GestureEventArgs _1A9C
	{
		get
		{
			return __1AB2;
		}
		set
		{
			__1AB2 = value;
		}
	}

	public bool DisablePalmRejection
	{
		get
		{
			return __1AB3;
		}
		set
		{
			if (__1AB3 != value)
			{
				__1AB3 = value;
				if (__1AAD.IsHandleCreated)
				{
					_0EFE.UnregisterTouchWindow(__1AAD.Handle);
					_1ABF();
				}
			}
		}
	}

	public float DpiX
	{
		get
		{
			return __1AB4;
		}
		set
		{
			__1AB4 = value;
		}
	}

	public float DpiY
	{
		get
		{
			return __1AB5;
		}
		set
		{
			__1AB5 = value;
		}
	}

	public static bool IsTouchEnabled
	{
		get
		{
			if (__1AB6)
			{
				return (_0EFE._1AE7() & (_0EFE.DigitizerStatus.MultiInput | _0EFE.DigitizerStatus.StackReady)) != 0;
			}
			return false;
		}
	}

	public static bool EnableTouchSupport
	{
		get
		{
			return __1AB6;
		}
		set
		{
			__1AB6 = value;
		}
	}

	public event EventHandler<GestureEventArgs> PanBegin
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = _1A9D;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1A9D, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = _1A9D;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1A9D, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<GestureEventArgs> PanEnd
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = _1A9E;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1A9E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = _1A9E;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1A9E, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<GestureEventArgs> Pan
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = _1A9F;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1A9F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = _1A9F;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1A9F, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<GestureEventArgs> Begin
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA0;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA0;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<GestureEventArgs> End
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA1;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA1;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<GestureEventArgs> PressAndTap
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA2;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA2;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<GestureEventArgs> RotateBegin
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA3;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA3;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<GestureEventArgs> Rotate
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA4;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA4;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA4, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<GestureEventArgs> RotateEnd
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA5;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA5, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA5;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA5, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<GestureEventArgs> TwoFingerTap
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA6;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA6;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA6, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<GestureEventArgs> ZoomBegin
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = ᪧ;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᪧ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = ᪧ;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᪧ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<GestureEventArgs> Zoom
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA8;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA8, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA8;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA8, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<GestureEventArgs> ZoomEnd
	{
		add
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA9;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<GestureEventArgs> eventHandler = _1AA9;
			EventHandler<GestureEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<GestureEventArgs> value2 = (EventHandler<GestureEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AA9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<TouchEventArgs> TouchDown
	{
		add
		{
			EventHandler<TouchEventArgs> eventHandler = _1AAA;
			EventHandler<TouchEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<TouchEventArgs> value2 = (EventHandler<TouchEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AAA, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<TouchEventArgs> eventHandler = _1AAA;
			EventHandler<TouchEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<TouchEventArgs> value2 = (EventHandler<TouchEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AAA, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<TouchEventArgs> TouchUp
	{
		add
		{
			EventHandler<TouchEventArgs> eventHandler = _1AAB;
			EventHandler<TouchEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<TouchEventArgs> value2 = (EventHandler<TouchEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AAB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<TouchEventArgs> eventHandler = _1AAB;
			EventHandler<TouchEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<TouchEventArgs> value2 = (EventHandler<TouchEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AAB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<TouchEventArgs> TouchMove
	{
		add
		{
			EventHandler<TouchEventArgs> eventHandler = _1AAC;
			EventHandler<TouchEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<TouchEventArgs> value2 = (EventHandler<TouchEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AAC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<TouchEventArgs> eventHandler = _1AAC;
			EventHandler<TouchEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<TouchEventArgs> value2 = (EventHandler<TouchEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1AAC, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnPanBegin(GestureEventArgs e)
	{
		_1A9D?.Invoke(this, e);
	}

	protected virtual void OnPanEnd(GestureEventArgs e)
	{
		_1A9E?.Invoke(this, e);
	}

	protected virtual void OnPan(GestureEventArgs e)
	{
		_1A9F?.Invoke(this, e);
	}

	protected virtual void OnBegin(GestureEventArgs e)
	{
		_1AA0?.Invoke(this, e);
	}

	protected virtual void OnEnd(GestureEventArgs e)
	{
		_1AA1?.Invoke(this, e);
	}

	protected virtual void OnPressAndTap(GestureEventArgs e)
	{
		_1AA2?.Invoke(this, e);
	}

	protected virtual void OnRotateBegin(GestureEventArgs e)
	{
		_1AA3?.Invoke(this, e);
	}

	protected virtual void OnRotate(GestureEventArgs e)
	{
		_1AA4?.Invoke(this, e);
	}

	protected virtual void OnRotateEnd(GestureEventArgs e)
	{
		_1AA5?.Invoke(this, e);
	}

	protected virtual void OnTwoFingerTap(GestureEventArgs e)
	{
		_1AA6?.Invoke(this, e);
	}

	protected virtual void OnZoomBegin(GestureEventArgs e)
	{
		ᪧ?.Invoke(this, e);
	}

	protected virtual void OnZoom(GestureEventArgs e)
	{
		_1AA8?.Invoke(this, e);
	}

	protected virtual void OnZoomEnd(GestureEventArgs e)
	{
		_1AA9?.Invoke(this, e);
	}

	protected virtual void OnTouchDown(TouchEventArgs e)
	{
		_1AAA?.Invoke(this, e);
	}

	protected virtual void OnTouchUp(TouchEventArgs e)
	{
		_1AAB?.Invoke(this, e);
	}

	protected virtual void OnTouchMove(TouchEventArgs e)
	{
		_1AAC?.Invoke(this, e);
	}

	public TouchHandler(Control parentControl)
		: this(parentControl, eTouchHandlerType.Gesture)
	{
	}

	public TouchHandler(Control parentControl, eTouchHandlerType handlerType)
	{
		__1AAD = parentControl;
		__1AAE = handlerType;
		if (IsTouchEnabled)
		{
			__1AAD.HandleCreated += _1AB7;
			if (__1AAD.IsHandleCreated)
			{
				_1AB8();
			}
		}
	}

	private void _1AB7(object _٠, EventArgs ؿ)
	{
		_1AB8();
	}

	private void _1AB8()
	{
		if (!_1ABF())
		{
			throw new NotSupportedException("Cannot register window");
		}
		__1AB0 = _1AB9;
		__1AAF = ((IntPtr.Size == 4) ? _0EFE.SubclassWindow(__1AAD.Handle, -4, __1AB0) : _0EFE.SubclassWindow64(__1AAD.Handle, -4, __1AB0));
		Graphics val = Graphics.FromHwnd(__1AAD.Handle);
		try
		{
			DpiX = val.DpiX;
			DpiY = val.DpiY;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void Release()
	{
		__1AAD.HandleCreated -= _1AB7;
		if (__1AAD.IsHandleCreated && !__1AAD.IsDisposed && __1AAF != IntPtr.Zero)
		{
			if (IntPtr.Size != 4)
			{
				_0EFE.SubclassWindow64(__1AAD.Handle, -4, __1AAF);
			}
			else
			{
				_0EFE.SubclassWindow(__1AAD.Handle, -4, __1AAF);
			}
			__1AAF = IntPtr.Zero;
		}
	}

	private uint _1AB9(IntPtr _0A65, int ᘮ, IntPtr _੦, IntPtr _੧)
	{
		if (ᘮ == 576 && (__1AAE & eTouchHandlerType.Touch) == eTouchHandlerType.Touch)
		{
			bool flag = false;
			foreach (TouchEventArgs item in _1ABA(_0A65, ᘮ, _੦, _੧, DpiX, DpiY))
			{
				if (item.IsTouchDown)
				{
					OnTouchDown(item);
				}
				if (item.IsTouchMove)
				{
					OnTouchMove(item);
				}
				if (item.IsTouchUp)
				{
					OnTouchUp(item);
				}
				flag |= item.Handled;
			}
			if (flag)
			{
				_0EFE.CloseTouchInputHandle(_੧);
				return 1u;
			}
		}
		if ((long)ᘮ != 281)
		{
			return _0EFE.CallWindowProc(__1AAF, _0A65, ᘮ, _੦, _੧);
		}
		_0EFE.GESTUREINFO pGestureInfo = default(_0EFE.GESTUREINFO);
		pGestureInfo.cbSize = (uint)Marshal.SizeOf(typeof(_0EFE.GESTUREINFO));
		if (!_0EFE.GetGestureInfo(_੧, ref pGestureInfo))
		{
			throw new Exception("Cannot retrieve gesture information");
		}
		GestureEventArgs gestureEventArgs = new GestureEventArgs(this, ref pGestureInfo);
		try
		{
			uint num = _1ABD(pGestureInfo.dwID, pGestureInfo.dwFlags);
			if (num == _1AFC._1AA0)
			{
				OnBegin(gestureEventArgs);
			}
			else if (num == _1AFC._1AA1)
			{
				OnEnd(gestureEventArgs);
			}
			else if (num == _1AFC._1A9F)
			{
				OnPan(gestureEventArgs);
			}
			else if (num == _1AFC._1A9D)
			{
				OnPanBegin(gestureEventArgs);
			}
			else if (num == _1AFC._1A9E)
			{
				OnPanEnd(gestureEventArgs);
			}
			else if (num == _1AFC._1AA2)
			{
				OnPressAndTap(gestureEventArgs);
			}
			else if (num == _1AFC._1AA4)
			{
				OnRotate(gestureEventArgs);
			}
			else if (num == _1AFC._1AA3)
			{
				OnRotateBegin(gestureEventArgs);
			}
			else if (num == _1AFC._1AA5)
			{
				OnRotateEnd(gestureEventArgs);
			}
			else if (num == _1AFC._1AA6)
			{
				OnTwoFingerTap(gestureEventArgs);
			}
			else if (num == _1AFC._1AA8)
			{
				OnZoom(gestureEventArgs);
			}
			else if (num == _1AFC.ᪧ)
			{
				OnZoomBegin(gestureEventArgs);
			}
			else if (num == _1AFC._1AA9)
			{
				OnZoomEnd(gestureEventArgs);
			}
		}
		catch (ArgumentOutOfRangeException)
		{
		}
		_1A9C = gestureEventArgs;
		if (gestureEventArgs.IsBegin)
		{
			_1A9B = gestureEventArgs;
		}
		if (gestureEventArgs.Handled)
		{
			_0EFE.CloseGestureInfoHandle(_੧);
			return 1u;
		}
		return _0EFE.CallWindowProc(__1AAF, _0A65, ᘮ, _੦, _੧);
	}

	private IEnumerable<TouchEventArgs> _1ABA(IntPtr _0A65, int ᘮ, IntPtr _੦, IntPtr _੧, float _1ABB, float _1ABC)
	{
		int inputCount = _0EFE._1AEB(DevComponents.DotNetBar._0EFE._0F11(_੦));
		_0EFE.TOUCHINPUT[] inputs = new _0EFE.TOUCHINPUT[inputCount];
		try
		{
			if (!_0EFE.GetTouchInputInfo(_੧, inputCount, inputs, Marshal.SizeOf((object)inputs[0])))
			{
				throw new Exception("Error calling GetTouchInputInfo API");
			}
			for (int i = 0; i < inputCount; i++)
			{
				yield return new TouchEventArgs(this, _1ABB, _1ABC, ref inputs[i]);
			}
		}
		finally
		{
		}
	}

	private static uint _1ABD(uint _1ABE, uint _17BD)
	{
		return (_1ABE << 3) + ((_1ABE != 6 && _1ABE != 7 && _1ABE != 1 && _1ABE != 2) ? (_17BD & 5) : 0);
	}

	private bool _1ABF()
	{
		bool flag = false;
		if ((__1AAE & eTouchHandlerType.Gesture) == eTouchHandlerType.Gesture)
		{
			_0EFE.GESTURECONFIG[] pGestureConfig = new _0EFE.GESTURECONFIG[1]
			{
				new _0EFE.GESTURECONFIG(0u, 1u, 0u)
			};
			flag = _0EFE.SetGestureConfig(__1AAD.Handle, 0u, 1u, pGestureConfig, (uint)Marshal.SizeOf(typeof(_0EFE.GESTURECONFIG)));
		}
		if ((__1AAE & eTouchHandlerType.Touch) == eTouchHandlerType.Touch)
		{
			flag |= _0EFE.RegisterTouchWindow(__1AAD.Handle, __1AB3 ? _0EFE.TouchWindowFlag.WantPalm : ((_0EFE.TouchWindowFlag)0u));
		}
		return flag;
	}
}
