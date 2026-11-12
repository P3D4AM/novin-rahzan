using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Touch;

public class GestureEventArgs : EventArgs
{
	private readonly uint _Flags;

	private uint _GestureId;

	private ulong _GestureArguments;

	private Point _Location;

	private double _RotateAngle;

	private Point _Center;

	private double _ZoomFactor;

	private Size _PanTranslation;

	private Size _PanVelocity;

	private GestureEventArgs _LastBeginEvent;

	private GestureEventArgs _LastEvent;

	private bool _Handled;

	internal uint GestureId
	{
		get
		{
			return _GestureId;
		}
		private set
		{
			_GestureId = value;
		}
	}

	internal ulong GestureArguments
	{
		get
		{
			return _GestureArguments;
		}
		private set
		{
			_GestureArguments = value;
		}
	}

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

	public bool IsBegin => (_Flags & 1) != 0;

	public bool IsEnd => (_Flags & 4) != 0;

	public bool IsInertia => (_Flags & 2) != 0;

	public double RotateAngle
	{
		get
		{
			return _RotateAngle;
		}
		private set
		{
			_RotateAngle = value;
		}
	}

	public Point Center
	{
		get
		{
			return _Center;
		}
		private set
		{
			_Center = value;
		}
	}

	public double ZoomFactor
	{
		get
		{
			return _ZoomFactor;
		}
		private set
		{
			_ZoomFactor = value;
		}
	}

	public Size PanTranslation
	{
		get
		{
			return _PanTranslation;
		}
		set
		{
			_PanTranslation = value;
		}
	}

	public Size PanVelocity
	{
		get
		{
			return _PanVelocity;
		}
		private set
		{
			_PanVelocity = value;
		}
	}

	public GestureEventArgs LastBeginEvent
	{
		get
		{
			return _LastBeginEvent;
		}
		internal set
		{
			_LastBeginEvent = value;
		}
	}

	public GestureEventArgs LastEvent
	{
		get
		{
			return _LastEvent;
		}
		internal set
		{
			_LastEvent = value;
		}
	}

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

	internal GestureEventArgs(TouchHandler handler, ref WinApi.GESTUREINFO gestureInfo)
	{
		_Flags = gestureInfo.dwFlags;
		GestureId = gestureInfo.dwID;
		GestureArguments = gestureInfo.ullArguments;
		LastEvent = handler.LastEventArgs;
		LastBeginEvent = handler.LastBeginEventArgs;
		ParseGesture(handler.ParentControl, ref gestureInfo);
		if (IsBegin)
		{
			LastBeginEvent = null;
			LastEvent = null;
		}
	}

	private void ParseGesture(Control parentControl, ref WinApi.GESTUREINFO gestureInfo)
	{
		Location = parentControl.PointToClient(new Point(gestureInfo.ptsLocation.x, gestureInfo.ptsLocation.y));
		Center = Location;
		switch (GestureId)
		{
		case 5u:
		{
			ushort num = (ushort)(IsBegin ? 0 : LastEvent.GestureArguments);
			RotateAngle = WinApi.GID_ROTATE_ANGLE_FROM_ARGUMENT((ushort)(gestureInfo.ullArguments - num));
			break;
		}
		case 3u:
		{
			Point point = (IsBegin ? Location : LastBeginEvent.Location);
			Center = new Point((Location.X + point.X) / 2, (Location.Y + point.Y) / 2);
			ZoomFactor = (IsBegin ? 1.0 : ((double)gestureInfo.ullArguments / (double)LastEvent.GestureArguments));
			break;
		}
		case 4u:
		{
			PanTranslation = (IsBegin ? new Size(0, 0) : new Size(Location.X - LastEvent.Location.X, Location.Y - LastEvent.Location.Y));
			int number = WinApi.HiDWord((long)gestureInfo.ullArguments);
			PanVelocity = new Size(WinApi.LoWord(number), WinApi.HiWord(number));
			break;
		}
		}
	}
}
