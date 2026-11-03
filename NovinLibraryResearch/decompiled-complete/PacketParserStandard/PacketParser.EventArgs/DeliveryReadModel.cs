using System;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class DeliveryReadModel
{
	public Guid Guid { get; set; }

	[Obsolete]
	public int Id { get; set; }

	public EntityPart Entity { get; set; }
}
