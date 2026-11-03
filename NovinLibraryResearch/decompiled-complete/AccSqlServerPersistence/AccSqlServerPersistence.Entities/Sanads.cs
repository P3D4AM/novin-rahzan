using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_Sanad")]
public class Sanads : ISanad, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IDeliverableEntity, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; } = Guid.NewGuid();


	public int Id { get; set; }

	public DateTime DateM { get; set; }

	[Required]
	public string Description { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public SanadType SanadType { get; set; }

	public Guid UserGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public ENSource Source { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public short StatusId { get; set; }

	public bool IsTemp { get; set; }

	public string? MetaData { get; set; }

	[NotMapped]
	public decimal Debit => ListSanadDet?.Sum((Sanad_Det q) => q.Debit) ?? 0m;

	[NotMapped]
	public decimal Credit => ListSanadDet?.Sum((Sanad_Det q) => q.Credit) ?? 0m;

	[NotMapped]
	public List<Sanad_Det> ListSanadDet { get; set; }

	public Cashier CashierItem { get; set; }
}
