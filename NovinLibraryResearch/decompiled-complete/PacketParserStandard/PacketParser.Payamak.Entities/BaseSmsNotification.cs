using PacketParser.Entities;

namespace PacketParser.Payamak.Entities;

public class BaseSmsNotification : ISmsNotification
{
	public EnNotification NotificationType { get; set; }

	public string Message { get; set; }

	public string Title { get; set; }
}
