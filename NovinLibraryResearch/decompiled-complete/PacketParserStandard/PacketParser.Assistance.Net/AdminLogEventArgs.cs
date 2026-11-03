using System;

namespace PacketParser.Assistance.Net;

public class AdminLogEventArgs
{
	public int termId { get; set; }

	public string serialNumber { get; set; }

	public string termType { get; set; }

	public int transId { get; set; }

	public int year { get; set; }

	public int month { get; set; }

	public int day { get; set; }

	public int hour { get; set; }

	public int minute { get; set; }

	public int second { get; set; }

	public long adminID { get; set; }

	public long userID { get; set; }

	public string action { get; set; }

	public int result { get; set; }

	public DateTime Logtime => new DateTime(year, month, day, hour, minute, second);
}
