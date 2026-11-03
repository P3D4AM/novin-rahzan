using System;

namespace PacketParser.Entities.Attendance;

public class SentSmsLogDet
{
	public string Mobiles { get; set; }

	public int PersonId { get; set; }

	public string Message { get; set; }

	public DateTime Date { get; set; } = DateTime.Now;


	public string RefrencedIds { get; set; }

	public string Desc { get; set; }
}
