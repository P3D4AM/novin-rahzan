using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebEntitiesDeliveryStatus : IEntitiesDeliveryStatus, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public EntityPart EntityPart { get; set; }

	public bool SendToAcc { get; set; }

	public short DepId { get; set; }

	public long SenderHashId { get; set; }

	public DateTime InsertTime { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;

}
