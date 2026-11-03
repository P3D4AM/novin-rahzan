using System;

namespace PacketParser.Entities.Sms;

public class SentSmsLog
{
	public DateTime SendDate { get; set; }

	public string SmsMessage { get; set; }

	public int SmsOutBoxId { get; set; }

	public long SmsMobileNumber { get; set; }
}
