using System;

namespace PacketParser.Records;

public record SendSmsRecord
{
	public Guid RefrenceGuid { get; set; }

	public string SmsText { get; set; }

	public long Reciever { get; set; }
}
