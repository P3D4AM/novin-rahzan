using System;

namespace PacketParser.Entities.Interfaces;

public interface IDeliverableEntity
{
	ServerStatusForClient ServerStatus { get; set; }

	DateTime? DeliveryDate { get; set; }

	int DeliveryId { get; set; }

	string DeliveryMessage { get; set; }

	string DeliveryShortMessage { get; set; }

	string HostRefrenced { get; set; }
}
