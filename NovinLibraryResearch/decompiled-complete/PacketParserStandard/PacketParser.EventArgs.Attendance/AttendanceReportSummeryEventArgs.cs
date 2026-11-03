using System;
using System.Threading;

namespace PacketParser.EventArgs.Attendance;

public class AttendanceReportSummeryEventArgs
{
	public TimeSpan SumEk { get; set; }

	public TimeSpan SumKK { get; set; }

	public int CountAbsent { get; set; }

	public int CountIncomplement { get; set; }

	public CancellationToken Token { get; set; }

	public AttendanceReportSummeryEventArgs(CancellationToken token)
	{
		Token = token;
	}
}
