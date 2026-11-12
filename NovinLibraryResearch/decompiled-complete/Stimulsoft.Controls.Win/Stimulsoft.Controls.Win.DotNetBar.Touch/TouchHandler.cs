using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Touch;

public class TouchHandler
{
	private static class GestureEventId
	{
		public static readonly uint Begin = GetGestureEventId(1u, 0u);

		public static readonly uint End = GetGestureEventId(2u, 0u);

		public static readonly uint PanBegin = GetGestureEventId(4u, 1u);

		public static readonly uint Pan = GetGestureEventId(4u, 0u);

		public static readonly uint PanEnd = GetGestureEventId(4u, 4u);

		public static readonly uint PressAndTap = GetGestureEventId(7u, 0u);

		public static readonly uint RotateBegin = GetGestureEventId(5u, 1u);

		public static readonly uint Rotate = GetGestureEventId(5u, 0u);

		public static readonly uint RotateEnd = GetGestureEventId(5u, 4u);

		public static readonly uint TwoFingerTap = GetGestureEventId(6u, 0u);

		public static readonly uint ZoomBegin = GetGestureEventId(3u, 1u);

		public static readonly uint Zoom = GetGestureEventId(3u, 0u);

		public static readonly uint ZoomEnd = GetGestureEventId(3u, 4u);
	}

	private Control _ParentControl;

	private eTouchHandlerType _HandlerType = eTouchHandlerType.Gesture;

	private IntPtr _originalWindowProcId;

	private WinApi.WindowProcDelegate _windowProcDelegate;

	private GestureEventArgs _LastBeginEventArgs;

	private GestureEventArgs _LastEventArgs;

	private bool _DisablePalmRejection;

	private float _DpiX;

	private float _DpiY;

	public Control ParentControl => _ParentControl;

	internal GestureEventArgs LastBeginEventArgs
	{
		get
		{
			return _LastBeginEventArgs;
		}
		set
		{
			_LastBeginEventArgs = value;
		}
	}

	internal GestureEventArgs LastEventArgs
	{
		get
		{
			return _LastEventArgs;
		}
		set
		{
			_LastEventArgs = value;
		}
	}

	public bool DisablePalmRejection
	{
		get
		{
			return _DisablePalmRejection;
		}
		set
		{
			if (_DisablePalmRejection != value)
			{
				_DisablePalmRejection = value;
				if (_ParentControl.IsHandleCreated)
				{
					WinApi.UnregisterTouchWindow(_ParentControl.Handle);
					RegisterTouchWindow();
				}
			}
		}
	}

	public float DpiX
	{
		get
		{
			return _DpiX;
		}
		set
		{
			_DpiX = value;
		}
	}

	public float DpiY
	{
		get
		{
			return _DpiY;
		}
		set
		{
			_DpiY = value;
		}
	}

	public static bool IsTouchEnabled => (WinApi.GetDigitizerStatus() & (WinApi.DigitizerStatus.MultiInput | WinApi.DigitizerStatus.StackReady)) != 0;

	public event EventHandler<GestureEventArgs> PanBegin;

	public event EventHandler<GestureEventArgs> PanEnd;

	public event EventHandler<GestureEventArgs> Pan;

	public event EventHandler<GestureEventArgs> Begin;

	public event EventHandler<GestureEventArgs> End;

	public event EventHandler<GestureEventArgs> PressAndTap;

	public event EventHandler<GestureEventArgs> RotateBegin;

	public event EventHandler<GestureEventArgs> Rotate;

	public event EventHandler<GestureEventArgs> RotateEnd;

	public event EventHandler<GestureEventArgs> TwoFingerTap;

	public event EventHandler<GestureEventArgs> ZoomBegin;

	public event EventHandler<GestureEventArgs> Zoom;

	public event EventHandler<GestureEventArgs> ZoomEnd;

	public event EventHandler<TouchEventArgs> TouchDown;

	public event EventHandler<TouchEventArgs> TouchUp;

	public event EventHandler<TouchEventArgs> TouchMove;

	protected virtual void OnPanBegin(GestureEventArgs e)
	{
		this.PanBegin?.Invoke(this, e);
	}

	protected virtual void OnPanEnd(GestureEventArgs e)
	{
		this.PanEnd?.Invoke(this, e);
	}

	protected virtual void OnPan(GestureEventArgs e)
	{
		this.Pan?.Invoke(this, e);
	}

	protected virtual void OnBegin(GestureEventArgs e)
	{
		this.Begin?.Invoke(this, e);
	}

	protected virtual void OnEnd(GestureEventArgs e)
	{
		this.End?.Invoke(this, e);
	}

	protected virtual void OnPressAndTap(GestureEventArgs e)
	{
		this.PressAndTap?.Invoke(this, e);
	}

	protected virtual void OnRotateBegin(GestureEventArgs e)
	{
		this.RotateBegin?.Invoke(this, e);
	}

	protected virtual void OnRotate(GestureEventArgs e)
	{
		this.Rotate?.Invoke(this, e);
	}

	protected virtual void OnRotateEnd(GestureEventArgs e)
	{
		this.RotateEnd?.Invoke(this, e);
	}

	protected virtual void OnTwoFingerTap(GestureEventArgs e)
	{
		this.TwoFingerTap?.Invoke(this, e);
	}

	protected virtual void OnZoomBegin(GestureEventArgs e)
	{
		this.ZoomBegin?.Invoke(this, e);
	}

	protected virtual void OnZoom(GestureEventArgs e)
	{
		this.Zoom?.Invoke(this, e);
	}

	protected virtual void OnZoomEnd(GestureEventArgs e)
	{
		this.ZoomEnd?.Invoke(this, e);
	}

	protected virtual void OnTouchDown(TouchEventArgs e)
	{
		this.TouchDown?.Invoke(this, e);
	}

	protected virtual void OnTouchUp(TouchEventArgs e)
	{
		this.TouchUp?.Invoke(this, e);
	}

	protected virtual void OnTouchMove(TouchEventArgs e)
	{
		this.TouchMove?.Invoke(this, e);
	}

	public TouchHandler(Control parentControl)
		: this(parentControl, eTouchHandlerType.Gesture)
	{
	}

	public TouchHandler(Control parentControl, eTouchHandlerType handlerType)
	{
		_ParentControl = parentControl;
		_HandlerType = handlerType;
		if (IsTouchEnabled)
		{
			if (_ParentControl.IsHandleCreated)
			{
				Initialize();
			}
			else
			{
				_ParentControl.HandleCreated += ParentControlHandleCreated;
			}
		}
	}

	private void ParentControlHandleCreated(object sender, EventArgs e)
	{
		Initialize();
	}

	private void Initialize()
	{
		if (!RegisterTouchWindow())
		{
			throw new NotSupportedException("Cannot register window");
		}
		_windowProcDelegate = WindowProc;
		_originalWindowProcId = ((IntPtr.Size == 4) ? WinApi.SubclassWindow(_ParentControl.Handle, -4, _windowProcDelegate) : WinApi.SubclassWindow64(_ParentControl.Handle, -4, _windowProcDelegate));
		Graphics val = Graphics.FromHwnd(_ParentControl.Handle);
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

	private uint WindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
	{
		if (msg == 576 && (_HandlerType & eTouchHandlerType.Touch) == eTouchHandlerType.Touch)
		{
			bool flag = false;
			foreach (TouchEventArgs item in DecodeMessage(hWnd, msg, wParam, lParam, DpiX, DpiY))
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
				WinApi.CloseTouchInputHandle(lParam);
				return 1u;
			}
		}
		if ((long)msg != 281)
		{
			return WinApi.CallWindowProc(_originalWindowProcId, hWnd, msg, wParam, lParam);
		}
		WinApi.GESTUREINFO pGestureInfo = default(WinApi.GESTUREINFO);
		pGestureInfo.cbSize = (uint)Marshal.SizeOf(typeof(WinApi.GESTUREINFO));
		if (!WinApi.GetGestureInfo(lParam, ref pGestureInfo))
		{
			throw new Exception("Cannot retrieve gesture information");
		}
		GestureEventArgs gestureEventArgs = new GestureEventArgs(this, ref pGestureInfo);
		try
		{
			uint gestureEventId = GetGestureEventId(pGestureInfo.dwID, pGestureInfo.dwFlags);
			if (gestureEventId == GestureEventId.Begin)
			{
				OnBegin(gestureEventArgs);
			}
			else if (gestureEventId == GestureEventId.End)
			{
				OnEnd(gestureEventArgs);
			}
			else if (gestureEventId == GestureEventId.Pan)
			{
				OnPan(gestureEventArgs);
			}
			else if (gestureEventId == GestureEventId.PanBegin)
			{
				OnPanBegin(gestureEventArgs);
			}
			else if (gestureEventId == GestureEventId.PanEnd)
			{
				OnPanEnd(gestureEventArgs);
			}
			else if (gestureEventId == GestureEventId.PressAndTap)
			{
				OnPressAndTap(gestureEventArgs);
			}
			else if (gestureEventId == GestureEventId.Rotate)
			{
				OnRotate(gestureEventArgs);
			}
			else if (gestureEventId == GestureEventId.RotateBegin)
			{
				OnRotateBegin(gestureEventArgs);
			}
			else if (gestureEventId == GestureEventId.RotateEnd)
			{
				OnRotateEnd(gestureEventArgs);
			}
			else if (gestureEventId == GestureEventId.TwoFingerTap)
			{
				OnTwoFingerTap(gestureEventArgs);
			}
			else if (gestureEventId == GestureEventId.Zoom)
			{
				OnZoom(gestureEventArgs);
			}
			else if (gestureEventId == GestureEventId.ZoomBegin)
			{
				OnZoomBegin(gestureEventArgs);
			}
			else if (gestureEventId == GestureEventId.ZoomEnd)
			{
				OnZoomEnd(gestureEventArgs);
			}
		}
		catch (ArgumentOutOfRangeException)
		{
		}
		LastEventArgs = gestureEventArgs;
		if (gestureEventArgs.IsBegin)
		{
			LastBeginEventArgs = gestureEventArgs;
		}
		if (gestureEventArgs.Handled)
		{
			WinApi.CloseGestureInfoHandle(lParam);
			return 1u;
		}
		return WinApi.CallWindowProc(_originalWindowProcId, hWnd, msg, wParam, lParam);
	}

	private IEnumerable<TouchEventArgs> DecodeMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, float dpiX, float dpiY)
	{
		int inputCount = WinApi.LoWord(Stimulsoft.Controls.Win.DotNetBar.WinApi.ToInt(wParam));
		WinApi.TOUCHINPUT[] inputs = new WinApi.TOUCHINPUT[inputCount];
		if (!WinApi.GetTouchInputInfo(lParam, inputCount, inputs, Marshal.SizeOf(inputs[0])))
		{
			throw new Exception("Error calling GetTouchInputInfo API");
		}
		for (int i = 0; i < inputCount; i++)
		{
			yield return new TouchEventArgs(this, dpiX, dpiY, ref inputs[i]);
		}
	}

	private static uint GetGestureEventId(uint dwID, uint dwFlags)
	{
		return (dwID << 3) + ((dwID != 6 && dwID != 7 && dwID != 1 && dwID != 2) ? (dwFlags & 5) : 0);
	}

	private bool RegisterTouchWindow()
	{
		bool flag = false;
		if ((_HandlerType & eTouchHandlerType.Gesture) == eTouchHandlerType.Gesture)
		{
			WinApi.GESTURECONFIG[] pGestureConfig = new WinApi.GESTURECONFIG[1]
			{
				new WinApi.GESTURECONFIG(0u, 1u, 0u)
			};
			flag = WinApi.SetGestureConfig(_ParentControl.Handle, 0u, 1u, pGestureConfig, (uint)Marshal.SizeOf(typeof(WinApi.GESTURECONFIG)));
		}
		if ((_HandlerType & eTouchHandlerType.Touch) == eTouchHandlerType.Touch)
		{
			flag |= WinApi.RegisterTouchWindow(_ParentControl.Handle, _DisablePalmRejection ? WinApi.TouchWindowFlag.WantPalm : ((WinApi.TouchWindowFlag)0u));
		}
		return flag;
	}
}
