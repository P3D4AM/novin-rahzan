using System;
using System.Drawing;

namespace DevComponents.DotNetBar.Touch;

public class TouchEventArgs : EventArgs
{
	private readonly TouchHandler __1AF0;

	private readonly float __1AF1;

	private readonly float __1AF2;

	private Point _ᡏ;

	private int _Ⴧ;

	private int __1AC0;

	private int __1AF3;

	private DateTime __1AF4;

	private int __1AF5;

	private Size? __1AF6;

	private bool __1ACA;

	public Point Location
	{
		get
		{
			return _ᡏ;
		}
		private set
		{
			_ᡏ = value;
		}
	}

	public int Id
	{
		get
		{
			return _Ⴧ;
		}
		private set
		{
			_Ⴧ = value;
		}
	}

	public int Flags
	{
		get
		{
			return __1AC0;
		}
		private set
		{
			__1AC0 = value;
		}
	}

	public int Mask
	{
		get
		{
			return __1AF3;
		}
		private set
		{
			__1AF3 = value;
		}
	}

	public DateTime AbsoluteTime
	{
		get
		{
			return __1AF4;
		}
		private set
		{
			__1AF4 = value;
		}
	}

	public int Time
	{
		get
		{
			return __1AF5;
		}
		private set
		{
			__1AF5 = value;
		}
	}

	public Size? ContactSize
	{
		get
		{
			return __1AF6;
		}
		private set
		{
			__1AF6 = value;
		}
	}

	public bool IsPrimaryContact => (Flags & 0x10) != 0;

	public bool IsTouchMove => _1AF7(1);

	public bool IsTouchDown => _1AF7(2);

	public bool IsTouchUp => _1AF7(4);

	public bool IsTouchInRange => _1AF7(8);

	public bool IsTouchNoCoalesce => _1AF7(32);

	public bool IsTouchPen => _1AF7(64);

	public bool IsTouchPalm => _1AF7(128);

	public bool Handled
	{
		get
		{
			return __1ACA;
		}
		set
		{
			__1ACA = value;
		}
	}

	internal TouchEventArgs(TouchHandler parentHandler, float dpiX, float dpiY, ref _0EFE.TOUCHINPUT touchInput)
	{
		__1AF0 = parentHandler;
		__1AF1 = 96f / dpiX;
		__1AF2 = 96f / dpiY;
		_1AF8(ref touchInput);
	}

	private bool _1AF7(int ԝ)
	{
		return (Flags & ԝ) != 0;
	}

	private void _1AF8(ref _0EFE.TOUCHINPUT _1AF9)
	{
		if (((uint)_1AF9.dwMask & 4u) != 0)
		{
			ContactSize = new Size(_1AFA(_1AF9.cyContact / 100), _1AFB(_1AF9.cyContact / 100));
		}
		Id = _1AF9.dwID;
		Point location = __1AF0.ParentControl.PointToClient(new Point(_1AF9.x / 100, _1AF9.y / 100));
		Location = location;
		Time = _1AF9.dwTime;
		TimeSpan timeSpan = TimeSpan.FromMilliseconds(Environment.TickCount - _1AF9.dwTime);
		AbsoluteTime = DateTime.Now - timeSpan;
		Mask = _1AF9.dwMask;
		Flags = _1AF9.dwFlags;
	}

	private int _1AFA(int ԝ)
	{
		return (int)((float)ԝ * __1AF1);
	}

	private int _1AFB(int ԝ)
	{
		return (int)((float)ԝ * __1AF2);
	}
}
