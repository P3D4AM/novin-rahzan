using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Aghsat;

namespace AccSqlServerPersistence.Entities.Accounting.Aghsat;

[Table("Acc_AghsatM_Group")]
public class AghsatM_Group : IAghsatM_Group, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData, IDeliverableEntity
{
	[Key]
	public Guid Guid { get; set; }

	public decimal Sum { get; set; }

	public int Count { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string? MetaData { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public int? SanadNo { get; set; }

	public decimal ProfitSum { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public Guid? CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public ICollection<AghsatM> AghsatAshkhas { get; set; }

	public Tafsil Tafsils { get; set; }

	public Moein Moeins { get; set; }

	public Cashier CashierItem { get; set; }

	public Tafsil CreditTafsil { get; set; }

	public Moein CreditMoein { get; set; }
}
