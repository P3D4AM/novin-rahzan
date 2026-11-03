using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid;

namespace AccSqlServerPersistence.Entities.Accounting.Tolid;

[Table("Acc_Tolid")]
public class Tolid : ITolid, IHasJustGuid, IHasModified, IHasSanadNo, IDeliverableEntity, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public int Id2 { get; set; }

	public DateTime DateM { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public Guid CashierGuid { get; set; }

	[Required]
	public string Description { get; set; }

	public decimal SumMavad { get; set; }

	public decimal SumMahsol { get; set; }

	public decimal SumHazine { get; set; }

	public Guid MavadMoeinGuid { get; set; }

	public Guid? MavadTafsilGuid { get; set; }

	public Guid MahsolMoeinGuid { get; set; }

	public Guid? MahsolTafsilGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public bool SanadAffective { get; set; }

	public bool KardexAffective { get; set; }

	public string? MetaData { get; set; }

	public Cashier Cashier { get; set; }

	public Moein MavadMoein { get; set; }

	public Moein MahsolMoein { get; set; }

	public Tafsil MavadTafsil { get; set; }

	public Tafsil MahsolTafsil { get; set; }

	public ICollection<TolidExport> TolidExport { get; set; }

	public ICollection<TolidMavad> TolidMavad { get; set; }

	public ICollection<TolidHazine> TolidHazine { get; set; }
}
