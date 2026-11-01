using System;

namespace ZDC2911;

public class Records
{
	private int _Action = 0;

	private int _Antipassback = 0;

	private DateTime _Clock = DateTime.MinValue;

	private ulong _DIN = 0uL;

	private int _DN = 0;

	private int _DoorStatus = 0;

	private int _JobCode = 0;

	private ulong _MDIN = 0uL;

	private string _Remark = "";

	private int _Verify = 0;

	public int Action
	{
		get
		{
			return _Action;
		}
		set
		{
			_Action = value;
		}
	}

	public int Antipassback
	{
		get
		{
			return _Antipassback;
		}
		set
		{
			_Antipassback = value;
		}
	}

	public DateTime Clock
	{
		get
		{
			return _Clock;
		}
		set
		{
			_Clock = value;
		}
	}

	public ulong DIN
	{
		get
		{
			return _DIN;
		}
		set
		{
			_DIN = value;
		}
	}

	public int DN
	{
		get
		{
			return _DN;
		}
		set
		{
			_DN = value;
		}
	}

	public int DoorStatus
	{
		get
		{
			return _DoorStatus;
		}
		set
		{
			_DoorStatus = value;
		}
	}

	public int JobCode
	{
		get
		{
			return _JobCode;
		}
		set
		{
			_JobCode = value;
		}
	}

	public ulong MDIN
	{
		get
		{
			return _MDIN;
		}
		set
		{
			_MDIN = value;
		}
	}

	public string Remark
	{
		get
		{
			return _Remark;
		}
		set
		{
			_Remark = value;
		}
	}

	public int Verify
	{
		get
		{
			return _Verify;
		}
		set
		{
			_Verify = value;
		}
	}
}
