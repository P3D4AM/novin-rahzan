using System;

namespace TimeAttendanceDevices.FLREbio;

public class RecordEbio
{
	public int vMaxLogCnt { get; set; } = 30000;


	public int vTMachineNumber { get; set; }

	public int vSEnrollNumber { get; set; }

	public int vSMachineNumber { get; set; }

	public int vVerifyMode { get; set; }

	public int vYear { get; set; } = 1900;


	public int vMonth { get; set; } = 1;


	public int vDay { get; set; } = 1;


	public int vHour { get; set; }

	public int vMinute { get; set; }

	public int vSecond { get; set; }

	public DateTime DateM => new DateTime(vYear, vMonth, vDay, vHour, vMinute, vSecond);

	public RecordEbio(int vTMachineNumber, int vSEnrollNumber, int vSMachineNumber, int vVerifyMode, int vYear, int vMonth, int vDay, int vHour, int vMinute, int vSecond)
	{
		this.vTMachineNumber = vTMachineNumber;
		this.vSEnrollNumber = vSEnrollNumber;
		this.vSMachineNumber = vSMachineNumber;
		this.vVerifyMode = vVerifyMode;
		this.vYear = vYear;
		this.vMonth = vMonth;
		this.vDay = vDay;
		this.vHour = vHour;
		this.vMinute = vMinute;
		this.vSecond = vSecond;
	}
}
