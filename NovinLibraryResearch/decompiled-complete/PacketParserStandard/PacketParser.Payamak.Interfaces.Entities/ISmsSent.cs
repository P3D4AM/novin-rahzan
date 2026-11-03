using System;
using PacketParser.Entities;

namespace PacketParser.Payamak.Interfaces.Entities;

public interface ISmsSent : IBaseSms
{
	DateTime SendDateM { get; set; }

	DeliveryStatus DeliverySt { get; set; }

	bool DeliveryRequested { get; set; }

	string RefrencedIds { get; set; }

	string SenderNumber { get; set; }

	string RefrenseAccountId { get; set; }

	string RefrencedID { get; }
}
