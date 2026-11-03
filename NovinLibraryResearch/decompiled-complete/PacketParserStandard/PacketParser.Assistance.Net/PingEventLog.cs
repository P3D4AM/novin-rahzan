using System;

namespace PacketParser.Assistance.Net;

public class PingEventLog
{
	public DateTime pingTime => DateTime.Now;

	public int termId { get; set; }

	public string serialNumber { get; set; }

	public string termType { get; set; }

	public int transId { get; set; }
}
