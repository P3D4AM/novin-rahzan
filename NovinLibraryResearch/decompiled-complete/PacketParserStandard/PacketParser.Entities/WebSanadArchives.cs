using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Sanad;

namespace PacketParser.Entities;

public class WebSanadArchives : ISanadArchives, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	public Guid Guid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public byte[] Data { get; set; }

	public decimal Price { get; set; }

	public string Description { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }
}
