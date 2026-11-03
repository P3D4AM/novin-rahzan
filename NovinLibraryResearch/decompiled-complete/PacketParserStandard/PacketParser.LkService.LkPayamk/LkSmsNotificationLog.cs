using System;

namespace PacketParser.LkService.LkPayamk;

public class LkSmsNotificationLog
{
	public long Id { get; set; }

	public int LkSmsNotificationTemplateId { get; set; }

	public long LkSerial { get; set; }

	public DateTime RegisteDate { get; set; }
}
