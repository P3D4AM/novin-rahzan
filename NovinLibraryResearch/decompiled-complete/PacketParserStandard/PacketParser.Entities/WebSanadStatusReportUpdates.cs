using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebSanadStatusReportUpdates : ISanadStatusReportUpdates, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	public Guid Guid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public int StatusId { get; set; }

	public Guid CashierGuid { get; set; }

	public DateTime DateM { get; set; }

	public string Description { get; set; }

	public int Ordinal { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }
}
