using System;

namespace TimeAttendanceDevices.FLRZk;

public class ZkRecord
{
	private int _iMachineNumber = 1;

	private int _idwErrorCode;

	private string _sdwEnrollNumber = "";

	private int _idwVerifyMode;

	private int _idwInOutMode;

	private int _idwYear;

	private int _idwMonth;

	private int _idwDay;

	private int _idwHour;

	private int _idwMinute;

	private int _idwSecond;

	private int _idwWorkcode;

	private int _iGLCount;

	private int _iIndex;

	public int iMachineNumber
	{
		get
		{
			return _iMachineNumber;
		}
		set
		{
			_iMachineNumber = value;
		}
	}

	public int idwErrorCode
	{
		get
		{
			return _idwErrorCode;
		}
		set
		{
			_idwErrorCode = value;
		}
	}

	public string sdwEnrollNumber
	{
		get
		{
			return _sdwEnrollNumber;
		}
		set
		{
			_sdwEnrollNumber = value;
		}
	}

	public int idwVerifyMode
	{
		get
		{
			return _idwVerifyMode;
		}
		set
		{
			_idwVerifyMode = value;
		}
	}

	public int idwInOutMode
	{
		get
		{
			return _idwInOutMode;
		}
		set
		{
			_idwInOutMode = value;
		}
	}

	public int idwYear
	{
		get
		{
			return _idwYear;
		}
		set
		{
			_idwYear = value;
		}
	}

	public int idwMonth
	{
		get
		{
			return _idwMonth;
		}
		set
		{
			_idwMonth = value;
		}
	}

	public int idwDay
	{
		get
		{
			return _idwDay;
		}
		set
		{
			_idwDay = value;
		}
	}

	public int idwHour
	{
		get
		{
			return _idwHour;
		}
		set
		{
			_idwHour = value;
		}
	}

	public int idwMinute
	{
		get
		{
			return _idwMinute;
		}
		set
		{
			_idwMinute = value;
		}
	}

	public int idwSecond
	{
		get
		{
			return _idwSecond;
		}
		set
		{
			_idwSecond = value;
		}
	}

	public int idwWorkcode
	{
		get
		{
			return _idwWorkcode;
		}
		set
		{
			_idwWorkcode = value;
		}
	}

	public int iGLCount
	{
		get
		{
			return _iGLCount;
		}
		set
		{
			_iGLCount = value;
		}
	}

	public int iIndex
	{
		get
		{
			return _iIndex;
		}
		set
		{
			_iIndex = value;
		}
	}

	public DateTime DateM => new DateTime(_idwYear, _idwMonth, _idwDay, int.Parse(_idwHour.ToString()), int.Parse(_idwMinute.ToString()), int.Parse(_idwSecond.ToString()));

	public ZkRecord(int iMachineNumber, string sdwEnrollNumber, int idwVerifyMode, int idwInOutMode, int idwYear, int idwMonth, int idwDay, int idwHour, int idwMinute, int idwSecond, int idwWorkcode)
	{
		this.iMachineNumber = iMachineNumber;
		this.sdwEnrollNumber = sdwEnrollNumber;
		this.idwVerifyMode = idwVerifyMode;
		this.idwInOutMode = idwInOutMode;
		this.idwYear = idwYear;
		this.idwMonth = idwMonth;
		this.idwDay = idwDay;
		this.idwHour = idwHour;
		this.idwMinute = idwMinute;
		this.idwSecond = idwSecond;
		this.idwWorkcode = idwWorkcode;
	}
}
