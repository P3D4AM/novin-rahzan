using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebVisitorArea : IVisitorArea, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasContext
{
	public string ContextGuid { get; set; }

	public short Level { get; set; }

	public string Name { get; set; }

	public Guid? ParentGuid { get; set; }

	public string Description { get; set; }

	public long Code { get; set; }

	public long FullCode { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }
}
