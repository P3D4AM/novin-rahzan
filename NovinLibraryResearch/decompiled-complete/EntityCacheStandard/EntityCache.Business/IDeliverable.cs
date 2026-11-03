using System;
using PacketParser.Entities;

namespace EntityCache.Business;

public interface IDeliverable
{
	DateTime? DeliveryDate { get; set; }

	string DeliveryMessage { get; set; }

	string DeliveryShortMessage { get; set; }

	ServerStatusForClient ServerStatus { get; set; }

	string DeliveryShortMessageForAndroid { get; }
}
