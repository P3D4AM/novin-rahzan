using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Accounting.Aghsat;

[Table("Acc_AghsatShakhsi")]
public class AghsatSh : IAghsatShakhsi, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; } = true;


	public int ID2 { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	[Required]
	public string Description { get; set; }

	public DateTime DateSarResid { get; set; }

	public DateTime DateM { get; set; }

	public Guid UserGuid { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public DateTime? PardakhtDateM { get; set; }

	public decimal PardakhtSum { get; set; }

	public decimal Price { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public ENSource Source { get; set; }

	public string? MetaData { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public Tafsil Tafsils { get; set; }

	public Moein Moeins { get; set; }

	public Pardakht Pardakhts { get; set; }

	public Cashier CashierItem { get; set; }
}
