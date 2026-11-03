using System;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Touch;

public class GestureEventArgs : EventArgs
{
	private readonly uint __1AC0;

	private uint __1AC1;

	private ulong __1AC2;

	private Point _ᡏ;

	private double __1AC3;

	private Point __1AC4;

	private double __1AC5;

	private Size __1AC6;

	private Size __1AC7;

	private GestureEventArgs __1AC8;

	private GestureEventArgs __1AC9;

	private bool __1ACA;

	public uint GestureId
	{
		get
		{
			return __1AC1;
		}
		private set
		{
			__1AC1 = value;
		}
	}

	public ulong GestureArguments
	{
		get
		{
			return __1AC2;
		}
		private set
		{
			__1AC2 = value;
		}
	}

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

	public bool IsBegin => (__1AC0 & 1) != 0;

	public bool IsEnd => (__1AC0 & 4) != 0;

	public bool IsInertia => (__1AC0 & 2) != 0;

	public double RotateAngle
	{
		get
		{
			return __1AC3;
		}
		private set
		{
			__1AC3 = value;
		}
	}

	public Point Center
	{
		get
		{
			return __1AC4;
		}
		private set
		{
			__1AC4 = value;
		}
	}

	public double ZoomFactor
	{
		get
		{
			return __1AC5;
		}
		private set
		{
			__1AC5 = value;
		}
	}

	public Size PanTranslation
	{
		get
		{
			return __1AC6;
		}
		set
		{
			__1AC6 = value;
		}
	}

	public Size PanVelocity
	{
		get
		{
			return __1AC7;
		}
		private set
		{
			__1AC7 = value;
		}
	}

	public GestureEventArgs LastBeginEvent
	{
		get
		{
			return __1AC8;
		}
		internal set
		{
			__1AC8 = value;
		}
	}

	public GestureEventArgs LastEvent
	{
		get
		{
			return __1AC9;
		}
		internal set
		{
			__1AC9 = value;
		}
	}

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

	internal GestureEventArgs(TouchHandler handler, ref _0EFE.GESTUREINFO gestureInfo)
	{
		__1AC0 = gestureInfo.dwFlags;
		GestureId = gestureInfo.dwID;
		GestureArguments = gestureInfo.ullArguments;
		LastEvent = handler._1A9C;
		LastBeginEvent = handler._1A9B;
		_1ACB(handler.ParentControl, ref gestureInfo);
		if (IsBegin)
		{
			LastBeginEvent = null;
			LastEvent = null;
		}
	}

	private void _1ACB(Control _1ACC, ref _0EFE.GESTUREINFO _1ACD)
	{
		Location = _1ACC.PointToClient(new Point(_1ACD.ptsLocation.x, _1ACD.ptsLocation.y));
		Center = Location;
		switch (GestureId)
		{
		case 5u:
		{
			ushort num2 = (ushort)(IsBegin ? 0 : LastEvent.GestureArguments);
			RotateAngle = _0EFE._1AEA((ushort)(_1ACD.ullArguments - num2));
			break;
		}
		case 3u:
		{
			Point point = (IsBegin ? Location : LastBeginEvent.Location);
			Center = new Point((Location.X + point.X) / 2, (Location.Y + point.Y) / 2);
			ZoomFactor = (IsBegin ? 1.0 : ((double)_1ACD.ullArguments / (double)LastEvent.GestureArguments));
			break;
		}
		case 4u:
		{
			PanTranslation = (IsBegin ? new Size(0, 0) : new Size(Location.X - LastEvent.Location.X, Location.Y - LastEvent.Location.Y));
			int num = _0EFE._1AEF((long)_1ACD.ullArguments);
			PanVelocity = new Size(_0EFE._1AEB(num), _0EFE._1AED(num));
			break;
		}
		}
	}
}
