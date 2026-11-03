using System;

namespace PacketParser.Records.Calendar;

public record CalenderSmsRecord
{
	public Guid RefrenceGuid { get; set; }

	public string SmsText { get; set; }

	public long Reciever { get; set; }
}
