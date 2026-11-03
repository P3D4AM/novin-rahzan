using System;

namespace PacketParser.LkService.LkPayamk;

public class LkSmsNotificationTemplate
{
	public int Id { get; set; }

	public string MessageTemplate { get; set; }

	public TimeSpan RunOffsetBasedOnLkExpiration { get; set; }

	public TimeSpan RunPeriod { get; set; }

	public DateTime? LastRunDate { get; set; }

	public bool Status { get; set; }
}
