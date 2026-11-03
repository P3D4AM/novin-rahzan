using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Accounting.Aghsat;

[Table("Acc_AghsatAshkhas")]
public class AghsatM : IAghsatAshkhas, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; } = true;


	public int Id2 { get; set; }

	[Required]
	public string? Description { get; set; }

	public DateTime DateSarResid { get; set; }

	public DateTime Date { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public DateTime? ReceptionDateM { get; set; }

	public decimal ReceptionSum { get; set; }

	public decimal Price { get; set; }

	public int TypePardakht { get; set; }

	[MaxLength(200)]
	public string? Mounth { get; set; }

	public Guid GuidGroup { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? MetaData { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public EnRasGiriType RasGiriType { get; set; }

	public decimal VajhEltezam { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public int? SanadNo { get; set; }

	public DateTime? VajhEltezamModified { get; set; }

	public Guid? CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public Reception Receptions { get; set; }

	public AghsatM_Group Groups { get; set; }

	public Tafsil CreditTafsil { get; set; }

	public Moein CreditMoein { get; set; }
}
