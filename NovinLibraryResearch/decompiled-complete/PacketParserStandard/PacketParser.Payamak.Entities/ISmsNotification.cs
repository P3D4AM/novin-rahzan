using PacketParser.Entities;

namespace PacketParser.Payamak.Entities;

public interface ISmsNotification
{
	EnNotification NotificationType { get; set; }

	string Message { get; set; }

	string Title { get; set; }
}
