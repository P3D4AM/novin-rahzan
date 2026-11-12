using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Touch;

public class TouchEventArgs : EventArgs
{
	private readonly TouchHandler _ParentHandler;

	private readonly float _dpiXFactor;

	private readonly float _dpiYFactor;

	private Point _Location;

	private int _Id;

	private int _Flags;

	private int _Mask;

	private DateTime _AbsoluteTime;

	private int _Time;

	private Size? _ContactSize;

	private bool _Handled;

	public Point Location
	{
		get
		{
			return _Location;
		}
		private set
		{
			_Location = value;
		}
	}

	public int Id
	{
		get
		{
			return _Id;
		}
		private set
		{
			_Id = value;
		}
	}

	public int Flags
	{
		get
		{
			return _Flags;
		}
		private set
		{
			_Flags = value;
		}
	}

	public int Mask
	{
		get
		{
			return _Mask;
		}
		private set
		{
			_Mask = value;
		}
	}

	public DateTime AbsoluteTime
	{
		get
		{
			return _AbsoluteTime;
		}
		private set
		{
			_AbsoluteTime = value;
		}
	}

	public int Time
	{
		get
		{
			return _Time;
		}
		private set
		{
			_Time = value;
		}
	}

	public Size? ContactSize
	{
		get
		{
			return _ContactSize;
		}
		private set
		{
			_ContactSize = value;
		}
	}

	public bool IsPrimaryContact => (Flags & 0x10) != 0;

	public bool IsTouchMove => CheckFlag(1);

	public bool IsTouchDown => CheckFlag(2);

	public bool IsTouchUp => CheckFlag(4);

	public bool IsTouchInRange => CheckFlag(8);

	public bool IsTouchNoCoalesce => CheckFlag(32);

	public bool IsTouchPen => CheckFlag(64);

	public bool IsTouchPalm => CheckFlag(128);

	public bool Handled
	{
		get
		{
			return _Handled;
		}
		set
		{
			_Handled = value;
		}
	}

	internal TouchEventArgs(TouchHandler parentHandler, float dpiX, float dpiY, ref WinApi.TOUCHINPUT touchInput)
	{
		_ParentHandler = parentHandler;
		_dpiXFactor = 96f / dpiX;
		_dpiYFactor = 96f / dpiY;
		DecodeTouch(ref touchInput);
	}

	private bool CheckFlag(int value)
	{
		return (Flags & value) != 0;
	}

	private void DecodeTouch(ref WinApi.TOUCHINPUT touchInput)
	{
		if (((uint)touchInput.dwMask & 4u) != 0)
		{
			ContactSize = new Size(AdjustDpiX(touchInput.cyContact / 100), AdjustDpiY(touchInput.cyContact / 100));
		}
		Id = touchInput.dwID;
		Point location = _ParentHandler.ParentControl.PointToClient(new Point(touchInput.x / 100, touchInput.y / 100));
		Location = location;
		Time = touchInput.dwTime;
		TimeSpan timeSpan = TimeSpan.FromMilliseconds(Environment.TickCount - touchInput.dwTime);
		AbsoluteTime = DateTime.Now - timeSpan;
		Mask = touchInput.dwMask;
		Flags = touchInput.dwFlags;
	}

	private int AdjustDpiX(int value)
	{
		return (int)((float)value * _dpiXFactor);
	}

	private int AdjustDpiY(int value)
	{
		return (int)((float)value * _dpiYFactor);
	}
}
