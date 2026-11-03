using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_LastPrice")]
public class LastPrice : ILastPrice, IHasModified, IHasJustGuid, IHasStatus, IDeliverableEntity, IHasMetaData
{
	[Key]
	[Column(Order = 0)]
	public Guid TafsilGuid { get; set; }

	[Key]
	[Column(Order = 1)]
	public Guid PrdGuid { get; set; }

	public Guid Guid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public DateTime Date { get; set; }

	public DateTime Modified { get; set; }

	public decimal Price { get; set; }

	public bool Invalidated { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public string? MetaData { get; set; }

	public bool Status { get; set; }

	public Product Product { get; set; }

	public Tafsil Tafsil { get; set; }
}
