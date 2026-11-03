using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IDelivery : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string DateSh { get; }

	DateTime? DeliveryDate { get; set; }

	int DeliveryId { get; set; }

	bool DeliveryStatus { get; set; }

	string Message { get; set; }

	string ShortMessage { get; set; }

	EntityPart Part { get; set; }
}
